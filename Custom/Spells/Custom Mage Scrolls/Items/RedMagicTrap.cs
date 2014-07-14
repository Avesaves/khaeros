using System;
using Server;
using Server.Network;
using Server.Regions;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class RedMagicTrap : BaseTrap
	{
		private Mobile m_trapper;

		[Constructable]
		public RedMagicTrap( Mobile trapper) : base( 0x36D3 )
		{
			m_trapper = trapper;
			Timer.DelayCall( TimeSpan.FromHours( 24 ), new TimerCallback( Expire ) );
            switch (Utility.Random(5))
            {
                case 0: ItemID = 3676; break;
                case 1: ItemID = 3679; break;
                case 2: ItemID = 3682; break;
                case 3: ItemID = 3685; break;
                case 4: ItemID = 3688; break;

                }
                Hue = 2943; 
		}

        public override bool PassivelyTriggered { get { return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 0; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

		public override void OnTrigger( Mobile from )
		{
			if ( !from.Alive || from.AccessLevel > AccessLevel.Player || m_trapper == null )
				return;

			//m_trapper.SendMessage("A dull moaning reaches your ears; something has disturbed your ghastly vigil!");
			ItemID = 0x3735;
			from.PlaySound(245);
            if (from is PlayerMobile)
                AOS.Damage(from, m_trapper, m_trapper.Mana/3, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
            else
                AOS.Damage(from, m_trapper, m_trapper.Mana/3*2, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
            m_trapper.Followers -= 2;
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerCallback( Trigger ) );
		}
		
		private void Expire()
		{
			if ( this == null || this.Deleted)
				return;
				

			else
			{
				if ( m_trapper != null )
				m_trapper.SendMessage("Your rune loses its energy...");
                
                m_trapper.Followers -= 2;
				this.Delete();

			}
		}	

		public virtual void Trigger()
		{
			if ( this != null)
				Delete();
		}

		public RedMagicTrap( Serial serial ) : base( serial )
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

			if ( ItemID == 0x3735 )
				Expire();
		}
	}
}