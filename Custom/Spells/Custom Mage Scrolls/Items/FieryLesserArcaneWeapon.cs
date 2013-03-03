using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class FieryLesserArcaneWeapon : BaseSword
    {
		private RotTimer m_Timer;
        public override string NameType { get { return "Fiery Arcane Weapon"; } }

        public override int SheathedMaleBackID{ get{ return 15168; } }
		public override int SheathedFemaleBackID{ get{ return 15169; } }

        public override int AosStrengthReq { get { return 15; } }
        public override double OverheadPercentage { get { return 0.3; } }
        public override double SwingPercentage { get { return 0.4; } }
        public override double ThrustPercentage { get { return 0.3; } }
        public override double RangedPercentage { get { return 0; } }
        public override int AosMinDamage { get { return 10; } }
        public override int AosMaxDamage { get { return 10; } }
        public override double AosSpeed { get { return 2.75; } }

        public override int OldStrengthReq { get { return 20; } }
        public override int OldMinDamage { get { return 5; } }
        public override int OldMaxDamage { get { return 33; } }
        public override int OldSpeed { get { return 35; } }

        public override int DefHitSound { get { return 0x23B; } }
        public override int DefMissSound { get { return 0x239; } }

        public override int InitMinHits { get { return 15; } }
        public override int InitMaxHits { get { return 25; } }
		public override SkillName DefSkill{ get{ return SkillName.Swords; } }

        [Constructable]
        public FieryLesserArcaneWeapon() : base( 0x2D22 )
        {
            Weight = 0.0;
            Name = "Fiery Arcane Weapon";
            AosElementDamages.Energy = 100;
            Description = "This swirling mass of shadows appears to have taken the shape of a blade, albeit barely. It glimmers with twisting voltaic arcs.";
            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
            att.Chance = 100;
            att.FireDamage = 5;
            Engines.XmlSpawner2.XmlAttach.AttachTo( this, att );
            Resource = CraftResource.Satin;
            Hue = 2992;
			m_Timer = new RotTimer(this);
            m_Timer.Start();
        }
		
		private class RotTimer : Timer
		{
			private Item m_Item;
			
			public RotTimer( Item m ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Item = m;
			}

			protected override void OnTick()
			{
				RotTimer rotTimer = new RotTimer( m_Item );
				
				switch ( Utility.Random( 3 ))
				{
				case 0: m_Item.Hue = 1257;  break;
				case 1: m_Item.Hue = 1258;  break;
				case 2: m_Item.Hue = 1259;  break;
				}
				Stop();
				rotTimer.Start();
			}
		}

        public FieryLesserArcaneWeapon( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            Delete();
        }

        public void DelayDelete()
        {
            Timer.DelayCall( TimeSpan.FromHours( 2 ), new TimerCallback( Delete ) );
        }

        public override void OnRemoved( object parent )
        {
            base.OnRemoved( parent );
            Delete();
        }
    }
}
