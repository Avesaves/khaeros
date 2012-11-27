using System;
using Server;
using Server.Multis;
using Server.ContextMenus;
using Server.Prompts;

namespace Server.Items
{
	public class KeyRingRenamePrompt : Prompt
	{
		private Mobile m_Mobile;
		private KeyRing i_KeyRing;

		public KeyRingRenamePrompt( Mobile m, KeyRing cont )
		{
			m_Mobile = m;
			i_KeyRing = cont;
		}

		public override void OnResponse( Mobile from, string text )
		{
			text = text.Trim();

			if ( text.Length > 40 )
				text = text.Substring( 0, 40 );

			if( !i_KeyRing.IsChildOf( from.Backpack ) && !i_KeyRing.IsChildOf( from.BankBox ))
				from.SendMessage("That must be in your pack to rename it.");
			else if ( text.Length > 0 )
			{
				i_KeyRing.Name = text;
				from.SendMessage("You rename the keyring to '{0}'.", text);
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

			return false;
		}
	}

	public class KeyRingRenameEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private KeyRing i_KeyRing;

		public KeyRingRenameEntry( Mobile from, KeyRing cont ) : base( 5104 )
		{
			m_From = from;
			i_KeyRing = cont;
		}

		public override void OnClick()
		{
			if ( i_KeyRing.IsChildOf( m_From.Backpack ) || i_KeyRing.IsChildOf( m_From.BankBox) ) 
			{
				m_From.SendMessage("What do you want to rename this to?");
				m_From.SendMessage("(Esc to cancel)");
				m_From.Prompt = new KeyRingRenamePrompt( m_From, i_KeyRing );
			}
			else
				m_From.SendMessage("That must be in your pack for you to rename it.");
		}
	}
}