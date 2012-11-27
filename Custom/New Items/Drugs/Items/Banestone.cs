using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class BanestonePebble : BaseIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		public override int SkillRequired { get { return 500; } }

		[Constructable]
		public BanestonePebble( int amount ) : base( 2514, amount )
		{
			Name = "a pebble of black rock";
			Hue = 2989;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from is PlayerMobile && from.Backpack != null && IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "Target a mortar to grind this up in." );
				from.Target = new PickTarget( this );
			}
		}

		[Constructable]
		public BanestonePebble() : this( 1 )
		{
		}

		public BanestonePebble( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
		
		private class PickTarget : Target
		{
			private BanestonePebble m_Pebble;
			public PickTarget( BanestonePebble item ) : base( 15, false, TargetFlags.None )
			{
				m_Pebble = item;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( from is PlayerMobile && from.Backpack != null && m_Pebble.IsChildOf( from.Backpack ) )
				{
					if ( targ is MortarPestle )
					{
						Item mp = targ as Item;
						if ( mp.IsChildOf( from.Backpack ) )
						{
							m_Pebble.Consume( 1 );
							BanestoneAsh ash = new BanestoneAsh();
							from.Backpack.DropItem( ash );
							from.SendMessage( "You grind the pebble to ash." );
						}
						else
							from.SendMessage( "That needs to be in your pack." );
					}
					else
						from.SendMessage( "You must target a mortar to grind that up!" );
				}
			}
		}
	}
}
