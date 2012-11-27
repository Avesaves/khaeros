using System;
using Server.Mobiles;

namespace Server.Items
{
	public enum ContentType2
	{
		Banestone = 0
	}
	/* 	
		BaseSnortable that does not implement content reduction. Basis for pipes and (stackable) stalks of weed
		Override OnSnort to handle content reduction and possibly item deletion
	*/
	public abstract class BaseSnortable : Item
	{
		private int m_ContentRemaining;
		private ContentType2 m_ContentType2;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ContentRemaining
		{
			get{ return m_ContentRemaining; }
			set{ m_ContentRemaining = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual ContentType2 ContentType2
		{
			get{ return m_ContentType2; }
			set{ m_ContentType2 = value; }
		}
		
		[Constructable]
		public BaseSnortable( int itemID, int contentTotal ) : base( itemID )
		{
			m_ContentRemaining = contentTotal;
		}

		public BaseSnortable( Serial serial ) : base( serial )
		{
		}
		
		public virtual void OnSnort( Mobile from )
		{
			if ( m_ContentType2 == ContentType2.Banestone )
				HallucinationEffect.BeginHallucinating( from as PlayerMobile, 120 );

			from.Emote( "*snorts*" );
			from.SendMessage ("A feeling of great euphoria overcomes you!");
			from.PlaySound( 1208 );
			if ( from.Body.IsHuman )
				from.Animate( 34, 5, 1, true, false, 0 );
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from && from is PlayerMobile )
			{
				if ( m_ContentRemaining > 0 )
					OnSnort( from );
				else
					from.SendMessage( "There's nothing left to smoke." );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}
		
		public override bool StackWith( Mobile from, Item dropped )
		{
			if ( dropped is BaseSnortable && ((BaseSnortable)dropped).ContentRemaining == ContentRemaining )
				return base.StackWith( from, dropped );
			
			else
				return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write( (int) m_ContentType2 );
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
					m_ContentType2 = (ContentType2)reader.ReadInt();
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
