using System;
using Server.Mobiles;

namespace Server.Items
{
	public enum ContentType
	{
		Swampweed = 0,
		Tobacco = 1,
        Opium = 2
	}
	/* 	
		BaseSmokable that does not implement content reduction. Basis for pipes and (stackable) stalks of weed
		Override OnSmoke to handle content reduction and possibly item deletion
	*/
	public abstract class BaseSmokable : Item
	{
		private int m_ContentRemaining;
		private ContentType m_ContentType;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ContentRemaining
		{
			get{ return m_ContentRemaining; }
			set{ m_ContentRemaining = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual ContentType ContentType
		{
			get{ return m_ContentType; }
			set{ m_ContentType = value; }
		}
		
		[Constructable]
		public BaseSmokable( int itemID, int contentTotal ) : base( itemID )
		{
			m_ContentRemaining = contentTotal;
		}

		public BaseSmokable( Serial serial ) : base( serial )
		{
		}
		
		public virtual void OnSmoke( Mobile from )
		{
			if ( m_ContentType == ContentType.Swampweed )
				HallucinationEffect.BeginHallucinating( from as PlayerMobile, 60 );

			from.Emote( "*puffs*" );
			from.PlaySound( 1208 );
			if ( from.Body.IsHuman )
				from.Animate( 34, 5, 1, true, false, 0 );
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from && from is PlayerMobile )
			{
				if ( m_ContentRemaining > 0 )
					OnSmoke( from );
				else
					from.SendMessage( "There's nothing left to smoke." );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}
		
		public override bool StackWith( Mobile from, Item dropped )
		{
			if ( dropped is BaseSmokable && ((BaseSmokable)dropped).ContentRemaining == ContentRemaining )
				return base.StackWith( from, dropped );
			
			else
				return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write( (int) m_ContentType );
			writer.Write( (int) m_ContentRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 1:
				{
					m_ContentType = (ContentType)reader.ReadInt();
					goto case 0;
				}

				case 0:
				{
					m_ContentRemaining = reader.ReadInt();
					break;
				}
			}
		}
	}
}
