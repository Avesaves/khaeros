using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class DualSwords : BaseSword
    {
        //public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
        //public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Bladeweave; } }
        public override int SheathedMaleBackID { get { return 15193; } }
        public override int SheathedFemaleBackID { get { return 15194; } }

        public override string NameType { get { return "Dual Swords"; } }

        public override int AosStrengthReq { get { return 45; } }
        public override double OverheadPercentage { get { return 0.4; } }
        public override double SwingPercentage { get { return 0.5; } }
        public override double ThrustPercentage { get { return 0.1; } }
        public override double RangedPercentage { get { return 0; } }
        public override int AosMinDamage { get { return 16; } }
        public override int AosMaxDamage { get { return 16; } }
        public override double AosSpeed { get { return 4.25; } }

        public override int OldStrengthReq { get { return 20; } }
        public override int OldMinDamage { get { return 12; } }
        public override int OldMaxDamage { get { return 14; } }
        public override int OldSpeed { get { return 43; } }

        public override int DefHitSound { get { return 0x23B; } }
        public override int DefMissSound { get { return 0x239; } }

        public override int InitMinHits { get { return 30; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public DualSwords()
            : base( 10153 )
        {
            Weight = 7.0;
            Name = "Dual Swords";
            AosElementDamages.Slashing = 100;
            Layer = Layer.TwoHanded;
        }

        public DualSwords( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.WriteEncodedInt( 0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadEncodedInt();
        }
    }
}
