using System;
using Server;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
	public class SpiritVigilTrap : BaseTrap
	{
		private Mobile m_trapper;

		[Constructable]
		public SpiritVigilTrap( Mobile trapper) : base( 0x36D3 )
		{
			m_trapper = trapper;
			Timer.DelayCall( TimeSpan.FromHours( 3 ), new TimerCallback( Expire ) );
		}

		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 2; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

		public override void OnTrigger( Mobile from )
		{
			if ( !from.Alive || ItemID != 0x36D3 || from.AccessLevel > AccessLevel.Player || m_trapper == null )
				return;

			m_trapper.SendMessage("A dull moaning reaches your ears; something has disturbed your ghastly vigil!");
			ItemID = 0x3735;
			m_trapper.PlaySound(383);
            AOS.Damage(from, m_trapper, 45, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
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
				m_trapper.SendMessage("Your spectral summon returns to the underword...");
                
                m_trapper.Followers -= 2;
				this.Delete();

			}
		}	

		public virtual void Trigger()
		{
			if ( this != null)
				Delete();
		}

		public SpiritVigilTrap( Serial serial ) : base( serial )
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