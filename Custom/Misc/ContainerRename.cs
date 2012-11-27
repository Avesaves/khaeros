using System;
using Server;
using Server.Multis;
using Server.ContextMenus;
using Server.Prompts;

namespace Server.Items
{
	public class ContainerRenamePrompt : Prompt
	{
		private Mobile m_Mobile;
		private BaseContainer i_Cont;

		public ContainerRenamePrompt( Mobile m, BaseContainer cont )
		{
			m_Mobile = m;
			i_Cont = cont;
		}

		public override void OnResponse( Mobile from, string text )
		{
			text = text.Trim();

			if ( text.Length > 40 )
				text = text.Substring( 0, 40 );

			if( !i_Cont.IsChildOf( from.Backpack ) && !i_Cont.IsChildOf( from.BankBox ))
				from.SendMessage("That must be in your pack or in a home you own for you to rename it.");
			else if ( text.Length > 0 )
			{
				i_Cont.Name = text;
				from.SendMessage("You rename the container to '{0}'.", text);
			}
		}
		
		public static void RemoveExcess( ref string str ) 
		{ 
			int start = str.IndexOf('<'); 
			int end = str.IndexOf('>'); 

			if( start > -1 && end > start )
			{
				str = str.Remove(start, (end - start) + 1); 
				RemoveExcess(ref str); 
			}
		}

		public static bool HasAccess( Mobile mob, Item item )
		{
			if( item == mob.Backpack )
				return false;
			else if( mob.AccessLevel >= AccessLevel.GameMaster ) // staff have no limits
				return true;
			else if( item.IsChildOf( mob.Backpack ) )
				return true;
			else if( CheckHouse( mob, item ) )
				return true;

			return false;
		}

		private static bool CheckHouse( Mobile mob, Item item )
		{
			BaseHouse house = BaseHouse.FindHouseAt( item );

			if ( house == null )
				return false;

			if( !mob.InRange( item.Location, 3 ) )
				return false;

			SecureAccessResult res = house.CheckSecureAccess( mob, item );

			switch ( res )
			{
				case SecureAccessResult.Insecure: break;
				case SecureAccessResult.Accessible: return true;
				case SecureAccessResult.Inaccessible: return false;
			}

			if ( house.IsLockedDown( item ) )
				return house.IsCoOwner( mob ) && (item is Container);

			return true;
		}		
	}

	public class ContainerRenameEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private BaseContainer i_Cont;

		public ContainerRenameEntry( Mobile from, BaseContainer cont ) : base( 5104 )
		{
			m_From = from;
			i_Cont = cont;
		}

		public override void OnClick()
		{
			if ( i_Cont.IsChildOf( m_From.Backpack ) || i_Cont.IsChildOf( m_From.BankBox) ) 
			{
				m_From.SendMessage("What do you want to rename this to?");
				m_From.SendMessage("(Esc to cancel)");
				m_From.Prompt = new ContainerRenamePrompt( m_From, i_Cont );
			}
			else
				m_From.SendMessage("That must be in your pack or in a home you own for you to rename it.");
		}
	}
}