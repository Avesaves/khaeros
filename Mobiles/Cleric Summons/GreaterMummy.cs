using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a mummy corpse")]
    public class GreaterMummy : BaseCreature, IUndead, IClericSummon
    {
        [Constructable]
        public GreaterMummy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a Greater Mummy";
            BodyValue = 154;
            BaseSoundID = 471;

            SetStr(86, 90);
            SetDex(81, 100);
            SetInt(70);

            SetHits(308, 312);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Blunt, 100);

            SetResistance(ResistanceType.Blunt, 30, 40);
            SetResistance(ResistanceType.Piercing, 40, 50);
            SetResistance(ResistanceType.Slashing, 40, 50);

            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.UnarmedFighting, 99.0, 100.0);

            Fame = 10;
            Karma = -4000;

            VirtualArmor = 50;
        }

        public override void GenerateLoot()
        {
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public GreaterMummy(Serial serial)
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