using System;
using Server;
using Server.Items;
using Server.Factions;
using System.Collections;
using Server.Network;
using Server.Engines.XmlSpawner2;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a sköllwulf corpse")]
    public class Sköllwulf : BaseCreature
    {
        [Constructable]
        public Sköllwulf() : base(AIType.AI_Predator, FightMode.Weakest, Utility.RandomMinMax(25,50), 1, 0.175, 0.4)
        {
            Name = "a sköllwulf";
            Body = 225;
            BaseSoundID = 0x52B;
            Hue = Utility.RandomList(1886, 1887, 1888, 1889, 1890, 1895, 1896, 1897, 1898, 1899, 1905, 1906, 1907, 1908);
            CreatureGroup = CreatureGroup.Canine;

            SetStr(200,300);
            SetDex(150,250);
            SetInt(35,85);            
            SetHits(200,500);
            SetStam(100,200);
            SetMana(100,200);

            SetDamage(Utility.RandomMinMax(3,11), Utility.RandomMinMax(10,18));
            SetDamageType(ResistanceType.Piercing, 70);
            SetDamageType(ResistanceType.Slashing, 30);

            SetResistance(ResistanceType.Blunt, 30, 45);
            SetResistance(ResistanceType.Piercing, 30, 45);
            SetResistance(ResistanceType.Slashing, 20, 35);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 25.0, 75.0);
            SetSkill(SkillName.UnarmedFighting, 50.0, 100.0);

            switch (Utility.Random(10))
            {
                case 0:
                    GiveFeat = "BleedingStrike " + Utility.RandomMinMax(1,3).ToString();
                    FavouriteManeuver = "BleedingStrike";
                    break;
                case 1:
                    GiveFeat = "FocusedAttack " + Utility.RandomMinMax(1, 3).ToString();
                    FavouriteStance = "FocusedAttack";
                    break;
                case 2:
                    GiveFeat = "Buildup " + Utility.RandomMinMax(1, 3).ToString();
                    GiveFeat = "QuickReflexes " + Utility.RandomMinMax(1, 3).ToString();
                    break;
                case 3:
                    GiveFeat = "SwipingClaws " + Utility.RandomMinMax(1, 3).ToString();
                    FavouriteStance = "SwipingClaws";
                    break;
                case 4:
                    GiveFeat = "BruteStrength " + Utility.RandomMinMax(1, 3).ToString();
                    GiveFeat = "Cleave " + Utility.RandomMinMax(1, 3).ToString();
                    break;
            }

            Fame = (((RawStr + RawDex + RawInt + RawHits) / 10) + DamageMin + DamageMax) * 100;
            Karma = Fame * -1;
            Tamable = false;
            if (Utility.RandomBool())
                IsSneaky = true;

            AddItem(new BonePile());
            AddItem(new Blood());

            if (Utility.RandomMinMax(1, 100) > 75)
            {
                EmptyJar wolfFang = new EmptyJar();
                wolfFang.ItemID = 0x0f83;
                wolfFang.Hue = 2986;
                wolfFang.Name = "a sköllwulf fang";
                wolfFang.Stackable = false;
            }

            if (Utility.RandomMinMax(1, 100) == 100)
            {
                Name = "a sköllwulf alpha";
                RawStr *= 2;
                RawDex *= 2;
                RawInt *= 2;
                RawHits *= 2;
                RawStam *= 2;

                GiveFeat = "BruteStrength 3";
                GiveFeat = "QuickReflexes 3";
                GiveFeat = "FastHealing 3";

                if (FavouriteStance == null)
                {
                    GiveFeat = "Rage 3";
                    FavouriteStance = "Rage";
                }
                
                if (FavouriteManeuver == null)
                {
                    GiveFeat = "CriticalStrike 3";
                    FavouriteManeuver = "CriticalStrike";
                }

                Fame *= 2;
                AI = AIType.AI_Berserk;
                FightMode = FightMode.Strongest;
                IsSneaky = true;
            }

            Description = "a large, slavering wolf, fur matted with blood long-since dried, and eyes wild with hunger and rage";
        }

        public override bool HasFur { get { return true; } }
        public override int Meat { get { return Utility.RandomMinMax(4,12); } }
        public override int Bones { get { return Utility.RandomMinMax(2, 20); } }
        public override int Hides { get { return Utility.RandomMinMax(8, 16); } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }

        public override void OnThink()
        {
            if (Combatant == null || Combatant.Deleted || !Combatant.Alive)
            {
                IPooledEnumerable eable = Map.GetMobilesInRange(Location, 100);
                foreach (Mobile m in eable)
                {
                    if (m is Sköllwulf)
                    {
                        if (m.InRange(this.Location, 10))
                        {
                            if (m.Combatant != null && !m.Combatant.Deleted && m.Combatant.Alive)
                                AddAggressor = m.Combatant;

                            continue;
                        }

                        this.Move(this.GetDirectionTo(m.Location));
                        continue;
                    }
                }
                eable.Free();
            }

            base.OnThink();
        }

        public override void OnReceivedAttack(bool melee, bool parried, Mobile attacker)
        {
            if (attacker.Weapon is BaseRanged)
            {
                int damage = Utility.RandomMinMax(1,attacker.RawStr);
                PlaySound(0x133);
                Damage(damage, attacker);
                Blood blood = new Blood();
                blood.ItemID = Utility.Random(0x122A, 5);
                blood.MoveToWorld(Location, Map);

                if (Utility.RandomBool())
                    AIObject.Action = ActionType.Flee;
            }

            base.OnReceivedAttack(melee, parried, attacker);
        }

        public Sköllwulf(Serial serial)
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

