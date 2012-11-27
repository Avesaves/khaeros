using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a mummy corpse")]
    public class Mummy : BaseCreature, IUndead, IClericSummon
    {
        [Constructable]
        public Mummy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a Mummy";
            BodyValue = 154;
            BaseSoundID = 471;

            SetStr(86, 90);
            SetDex(81, 100);
            SetInt(65);

            SetHits(208, 212);

            SetDamage(13, 17);

            SetDamageType(ResistanceType.Blunt, 100);

            SetResistance(ResistanceType.Blunt, 30, 40);
            SetResistance(ResistanceType.Piercing, 40, 50);
            SetResistance(ResistanceType.Slashing, 40, 50);

            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.UnarmedFighting, 85.1, 90.0);

            Fame = 10;
            Karma = -4000;

            VirtualArmor = 45;
        }

        public override void GenerateLoot()
        {
        }

        public override bool BleedImmune { get { return true; } }

        public Mummy(Serial serial)
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
        }
    }
}