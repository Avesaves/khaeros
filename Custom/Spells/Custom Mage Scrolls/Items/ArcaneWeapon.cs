using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class ArcaneWeapon : BaseSword
    {
        public override string NameType { get { return "Arcane Weapon"; } }

        public override int SheathedMaleBackID{ get{ return 15168; } }
		public override int SheathedFemaleBackID{ get{ return 15169; } }

        public override int AosStrengthReq { get { return 15; } }
        public override double OverheadPercentage { get { return 0.3; } }
        public override double SwingPercentage { get { return 0.5; } }
        public override double ThrustPercentage { get { return 0.2; } }
        public override double RangedPercentage { get { return 0; } }
        public override int AosMinDamage { get { return 15; } }
        public override int AosMaxDamage { get { return 15; } }
        public override double AosSpeed { get { return 3.0; } }

        public override int OldStrengthReq { get { return 25; } }
        public override int OldMinDamage { get { return 5; } }
        public override int OldMaxDamage { get { return 33; } }
        public override int OldSpeed { get { return 35; } }

        public override int DefHitSound { get { return 0x23B; } }
        public override int DefMissSound { get { return 0x239; } }

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 50; } }
		public override SkillName DefSkill{ get{ return SkillName.Swords; } }

        [Constructable]
        public ArcaneWeapon() : base( 0x2D35 )
        {
            Weight = 0.0;
            Name = "Arcane Weapon";
            AosElementDamages.Energy = 100;
            Quality = WeaponQuality.Masterwork;
            QualityAccuracy = 1;
            QualityDamage = 1;
            QualitySpeed = 1;
            Description = "This swirling mass of shadows appears to have taken the shape of a blade, albeit barely. It glimmers with twisting voltaic arcs.";
            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
            att.Chance = 100;
            att.EnergyDamage = 15;
            Engines.XmlSpawner2.XmlAttach.AttachTo( this, att );
            Resource = CraftResource.Satin;
            Hue = 2992;
        }

        public ArcaneWeapon( Serial serial )
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
