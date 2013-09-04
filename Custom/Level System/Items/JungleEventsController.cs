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
	public class JungleEventsController : BaseTrap
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
				
				if( m.Hits < ( m.HitsMax / 2 ) )
				{
					if( m.Nation == Nation.Western )
					{
						if( m.AccessLevel > AccessLevel.Player )
							m.SendMessage( "Staff-only Debug Message: avoided being damaged by the jungle's diseases due to racial merit." );
					}
					
					else if( Utility.RandomMinMax( 1, 100 ) > 50 )
					{
						m.SendMessage( "Some of your wounds have become infected by the jungle's environment." );
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, m, Utility.RandomMinMax( 1, 4 ) );
					}
					
					else if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Staff-only Debug Message: avoided being damaged by the jungle's diseases due to luck." );
				}
			}
		}
		
		[Constructable]
		public JungleEventsController() : base( 0xDDA )
		{
            Name = "Jungle Events Controller";
            Movable = false;
            Visible = false;
		}

		public JungleEventsController( Serial serial ) : base( serial )
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
