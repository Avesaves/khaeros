using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Spells;

namespace Server.Items
{
	public class DesertEventsController : BaseTrap
	{
		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 9; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromSeconds( 30 ); } }

		public override void OnTrigger( Mobile from )
		{
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				
				if( m.Thirst < 1 )
				{
					if( m.Nation == Nation.Haluaroc )
					{
						if( m.AccessLevel > AccessLevel.Player )
							m.SendMessage( "Staff-only Debug Message: avoided being damaged by the desert's heat due to racial merit." );
					}
					
					else if( Utility.RandomMinMax( 1, 100 ) > 50 )
					{
						m.SendMessage( "You are thirsty and the desert's heat takes its toll on you." );
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, m, Utility.RandomMinMax( 1, 4 ) );
					}
					
					else if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Staff-only Debug Message: avoided being damaged by the desert's heat due to luck." );
				}
				
				if( m.Nation == Nation.Haluaroc )
				{
					if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Staff-only Debug Message: avoided trap due to racial merit." );
					
					return;
				}
				
				if( Utility.RandomMinMax( 1, 100 ) > 80 )
				{
					m.SendMessage( "You got dragged in the sand and into a Dunedigger's lair!" );
					from.Location = new Point3D( 5910, 2762, 0 );
					from.PlaySound( 545 );
				}
				
				else
				{
					if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Staff-only Debug Message: avoided trap due to luck." );
				}
			}
		}
		
		[Constructable]
		public DesertEventsController() : base( 0xDDA )
		{
            Name = "Desert Events Controller";
            Movable = false;
            Visible = false;
		}

		public DesertEventsController( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
