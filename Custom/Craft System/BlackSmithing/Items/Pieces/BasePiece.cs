using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public abstract class BaseAssemblyPiece : Item
    {
        private int m_Durability;
        private int m_Quality;
        private CraftResource m_Resource;
        private int m_ResourceAmount;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Durability
        {
            get{ return m_Durability; }
            set { m_Durability = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quality
        {
            get{ return m_Quality; }
            set { m_Quality = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ResourceAmount
        {
            get { return m_ResourceAmount; }
            set { m_ResourceAmount = value; InvalidateProperties(); }
        }

        public BaseAssemblyPiece(int itemID): base(itemID)
        {
            Durability = 30;
            if (m_Resource == null || m_Resource == CraftResource.None)
                m_Resource = CraftResource.Copper;
        }

        public BaseAssemblyPiece(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int oreType;

            switch (m_Resource)
            {
                case CraftResource.Copper:          oreType = 1053106; break; // copper
                case CraftResource.Bronze:          oreType = 1053105; break; // bronze
                case CraftResource.Iron:            oreType = 1062226; break; // iron
                case CraftResource.Gold:            oreType = 1053104; break; // golden
                case CraftResource.Silver:          oreType = 1053107; break; // agapite
                case CraftResource.Obsidian:        oreType = 1053103; break; // verite
                case CraftResource.Steel:           oreType = 1053102; break; // valorite
                case CraftResource.Tin:             oreType = 1053101; break; // valorite
                case CraftResource.Starmetal:       oreType = 1053108; break; // valorite
                case CraftResource.ThickLeather:    oreType = 1061118; break; // Thick
                case CraftResource.BeastLeather:    oreType = 1061117; break; // Beast
                case CraftResource.ScaledLeather:   oreType = 1061116; break; // Scaled
                case CraftResource.RedScales:       oreType = 1060814; break; // red
                case CraftResource.YellowScales:    oreType = 1060818; break; // yellow
                case CraftResource.BlackScales:     oreType = 1060820; break; // black
                case CraftResource.GreenScales:     oreType = 1060819; break; // green
                case CraftResource.WhiteScales:     oreType = 1060821; break; // white
                case CraftResource.BlueScales:      oreType = 1060815; break; // blue

                case CraftResource.Oak:             oreType = 1063511; break; // yellow
                case CraftResource.Yew:             oreType = 1063512; break; // black
                case CraftResource.Redwood:         oreType = 1063513; break; // green
                case CraftResource.Ash:             oreType = 1063514; break; // white
                case CraftResource.Greenheart:      oreType = 1063515; break; // blue
                default: oreType = 0; break;
            }

            list.Add(oreType);

            list.Add(1060639, "{0}\t{1}", m_Durability, m_Durability);
            list.Add("Quality: {0}", m_Quality); 
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Durability);
            writer.Write((int)m_Quality);
            writer.WriteEncodedInt((int)m_Resource);
            writer.Write((int)m_ResourceAmount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Durability = reader.ReadInt();
            m_Quality = reader.ReadInt();
            m_Resource = (CraftResource)reader.ReadEncodedInt();
            m_ResourceAmount = reader.ReadInt();
        } 
    }

    public abstract class BaseWeaponPiece : BaseAssemblyPiece
    {
        private int m_Damage;
        private int m_Speed;
        private int m_DCI;
        private int m_HCI;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage
        {
            get{ return m_Damage; }
            set { m_Damage = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Speed
        {
            get{ return m_Speed; }
            set { m_Speed = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Defense
        {
            get{ return m_DCI; }
            set { m_DCI = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Attack
        {
            get{ return m_HCI; }
            set { m_HCI = value; InvalidateProperties(); }
        }

        public BaseWeaponPiece(int itemID): base(itemID)
        {            
        }

        public BaseWeaponPiece(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);            

            if (m_Damage != 0)
                list.Add(1060658, "Damage Increase\t+{0}", m_Damage);
            if (m_Speed != 0)
                list.Add(1060661, "Speed Increase\t+{0}", m_Speed);
            if (m_DCI != 0)
                list.Add(1060408, m_DCI.ToString()); // defense chance increase ~1_val~%
            if (m_HCI != 0)
                list.Add(1060415, m_HCI.ToString()); // hit chance increase ~1_val~%
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Damage = reader.ReadInt();
            m_Speed = reader.ReadInt();
            m_DCI = reader.ReadInt();
            m_HCI = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Damage);
            writer.Write((int)m_Speed);
            writer.Write((int)m_DCI);
            writer.Write((int)m_HCI);
        }
    }

    public abstract class BaseAttackPiece : BaseWeaponPiece
    {
        public BaseAttackPiece(int itemID): base(itemID)
        {            
        }

        public BaseAttackPiece(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
    }

    public abstract class BaseHandlePiece : BaseWeaponPiece
    {
        public BaseHandlePiece(int itemID): base(itemID)
        {            
        }

        public BaseHandlePiece(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
    }

    public abstract class BaseArmorPiece : BaseAssemblyPiece
    {
        private int m_Blunt;
        private int m_Slash;
        private int m_Pierce;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Blunt
        {
            get{ return m_Blunt; }
            set { m_Blunt = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Slash
        {
            get{ return m_Slash; }
            set { m_Slash = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Pierce
        {
            get{ return m_Pierce; }
            set { m_Pierce = value; InvalidateProperties(); }
        }

        public BaseArmorPiece(int itemID): base(itemID)
        {            
        }

        public BaseArmorPiece(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060526, m_Blunt.ToString());
            list.Add(1060527, m_Slash.ToString());
            list.Add(1060528, m_Pierce.ToString());
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Blunt = reader.ReadInt();
            m_Slash = reader.ReadInt();
            m_Pierce = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Blunt);
            writer.Write((int)m_Slash);
            writer.Write((int)m_Pierce);
        }
    }
}
