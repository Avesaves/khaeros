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
	public class SkeletonTrap : BaseTrap
	{
		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 3; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromMinutes( 5 ); } }

		public override void OnTrigger( Mobile from )
		{
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				
				if( m.Friendship.Undead < 1 )
				{
					int randomchance = Utility.RandomMinMax( 1, 10 );
					
					if( randomchance > 8 )
					{
						Mobile skeleton = skeleton = new Skeleton();
						
						if( randomchance > 9 )
							skeleton = new SkeletalSoldier();
						
						skeleton.MoveToWorld( this.Location, this.Map );
						skeleton.PlaySound( 1169 );
						skeleton.Combatant = m;
						this.Delete();
					}
				}
			}
		}
		
		[Constructable]
		public SkeletonTrap() : base( 0xED1 )
		{
            Movable = false;
            Visible = true;
            ItemID = 3786 + Utility.RandomMinMax( 0, 8 );
		}

		public SkeletonTrap( Serial serial ) : base( serial )
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
