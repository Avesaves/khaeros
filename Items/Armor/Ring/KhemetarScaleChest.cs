using System;
using Server.Items;

namespace Server.Items
{
    public class ScaleArmorChest : BaseArmor
    {
        public override int BaseBluntResistance { get { return 12; } }
        public override int BaseSlashingResistance { get { return 17; } }
        public override int BasePiercingResistance { get { return 14; } }
        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 5; } }
        public override int BaseColdResistance { get { return 1; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 40; } }
        public override int InitMaxHits { get { return 50; } }

        public override int AosStrReq { get { return 40; } }
        public override int OldStrReq { get { return 20; } }

        public override int OldDexBonus { get { return -2; } }

        public override int ArmorBase { get { return 22; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Ringmail; } }

        [Constructable]
        public ScaleArmorChest()
            : base(0x3BD2)
        {
            Weight = 15.0;
            Name = "scale armor Chest";
        }

        public ScaleArmorChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (Weight == 1.0)
                Weight = 15.0;
        }
    }
}
