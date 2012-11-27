using System;

namespace Server.Items
{
    public class SGEnergyFieldSouth : Item
    {
        private Point3D m_Target;
        private Map m_TargetMap;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Target
        { get { return m_Target; } set { m_Target = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap
        { get { return m_TargetMap; } set { m_TargetMap = value; } }

        [Constructable]
        public SGEnergyFieldSouth(Point3D target, Map targetMap ) : base(8148)
        {
            m_Target = target;
            m_TargetMap = targetMap;
            Movable = false;
            Name = "swirling vortex";
            Hue = 2656;
			Light = LightType.Circle300;
        }

        public SGEnergyFieldSouth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Target);
            writer.Write(m_TargetMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Target = reader.ReadPoint3D();
            m_TargetMap = reader.ReadMap();
        }
    }
}