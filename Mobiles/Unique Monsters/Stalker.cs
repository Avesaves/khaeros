using System;
using System.Collections;
using Server.Items;
using Server.Network;
using System.Xml;
using Server.Mobiles;
using Server.Regions;

namespace Server.Mobiles
{
    [CorpseName("a Stalker corpse")]
    public class Stalker : BaseCreature, IAbyssal, IHasReach
    {
        [Constructable]
        public Stalker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Troll God";
            Team = 1;
            Body = 130;
            Hue = 2994;
            BaseSoundID = 0x175;

            SetStr(200, 300);
            SetDex(200, 250);
            SetInt(150);

            SetHits(1000, 2000);

            SetDamage(20, 30);

            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Magery, 112.6, 117.5);
            SetSkill(SkillName.Meditation, 200.0);

            BluntResistSeed = 55;
            PiercingResistSeed = 55;
            SlashingResistSeed = 55;
            RangeFight = 2;
            CombatSkills = 100;
            SlashingDamage = 50;
            EnergyDamage = 25;
            ColdDamage = 25;

            MeleeAttackType = MeleeAttackType.FrontalAOE;
            CreatureGroup = CreatureGroup.Abyssal;

            Fame = 90000;
            Karma = -60000;
            PackItem(new RewardToken(11));
            PackItem(new Quest7Item());
            int rand = Utility.Random(40);
            if (rand > 38)
                PackItem(new StarmetalOre(2)); 
        }

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
        }

        public override int Meat { get { return 15; } }
        public override int Bones { get { return 30; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override int GetAngerSound()
        {
            return 0x174;
        }

        public override int GetIdleSound()
        {
            return 0x175;
        }

        public override int GetAttackSound()
        {
            return 0x176;
        }

        public override int GetHurtSound()
        {
            return 0x177;
        }

        public override int GetDeathSound()
        {
            return 0x178;
        }

        public void DisplaceItself(Mobile target)
        {
            Map map = this.Map;

            if (map == null)
                return;

            bool validLocation = false;
            Point3D loc = this.Location;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(6) - 1;
                int y = Y + Utility.Random(6) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                    loc = new Point3D(x, y, Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

            this.PlaySound(0x1F1);

            this.Location = loc;
            this.ProcessDelta();
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (this.Region is SanctuaryRegion)
            {
                int randomEmote = RandomMinMaxScaled(1, 5);

                if (randomEmote == 5)
                {
                    this.Emote("*Roars in pain!*");
                    this.PlaySound(0x167);
                }
                else if (randomEmote == 4)
                {
                    this.Say("Damn the gods!");
                    this.PlaySound(0x168);
                }
            }
            else
            {
                DisplaceItself(attacker);
            }
        }

    	public override void GenerateLoot()
		{
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.Gems, 15);
		}

        public Stalker(Serial serial)
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

    //public class SanctuaryCheck : BaseRegion
    //{
    //    public bool SanctuaryCheck(Mobile m, Rectangle3D rect)
    //    {

    //    }
         
    //}
}
