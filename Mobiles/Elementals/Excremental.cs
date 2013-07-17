using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a pile of excrement")]
    public class Excremental : BaseCreature
    {

        [Constructable]
        public Excremental() : base(AIType.AI_Berserk, FightMode.Berserk, 20, 1, 0.2, 0.4)
        {
            Name = "an excremental";
            BodyValue = 14;
            BaseSoundID = 0x0E0;
            Hue = BreathEffectHue;

            SetStr(300, 500);
            SetDex(100, 200);
            SetInt(40, 100);

            SetHits(500, 1000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Blunt, 100);

            SetResistance(ResistanceType.Blunt, 45, 55);
            SetResistance(ResistanceType.Piercing, 40, 50);
            SetResistance(ResistanceType.Slashing, 40, 50);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 15, 30);

            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.UnarmedFighting, 90.1, 92.5);

            MeleeAttackType = Mobiles.MeleeAttackType.FrontalAOE;

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 40; 
			}
				 public Excremental( Serial serial ) : base( serial )
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
					writer.Write((int)0); //version
				}
			}
			}
/*
            SetExcrementalLoot();
            while (Utility.RandomBool())
                SetExcrementalLoot();
        }

        #region Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 5);
        }

        private void SetExcrementalLoot()
        {
            switch (Utility.RandomMinMax(1, 10))
            {
                case 1: AddToBackpack(new Candelabra()); break;
                case 2: AddToBackpack(new Copper(Utility.RandomMinMax(Utility.RandomMinMax(1,100), Utility.RandomMinMax(100,1000)))); break;
                case 3: AddToBackpack(new GoldIngot(Utility.RandomMinMax(1, Utility.RandomMinMax(10, 25)))); break;
                case 4: AddToBackpack(new Velvet(Utility.RandomMinMax(1, 50))); break;
                case 5: AddToBackpack(new Spam(Utility.RandomMinMax(1, 50))); break;
                case 6: AddToBackpack(new Silver(Utility.RandomMinMax(Utility.RandomMinMax(1, 10), Utility.RandomMinMax(10, 100)))); break;
                case 7: AddToBackpack(new Linen(Utility.RandomMinMax(10, 100))); break;
                case 8: AddToBackpack(new Gold(1, 10)); break;
                case 9: AddToBackpack(new Torso()); break;
                case 10:
                    {
                        int artifactChance = Utility.RandomMinMax(1, 100);
                        if (artifactChance == 100)
                        {
                            switch (Utility.RandomMinMax(1, 4))
                            {
                                case 1:
                                    {
                                        Glaive artifact = new Glaive();
                                        artifact.QualityAccuracy = 3;
                                        artifact.Quality = WeaponQuality.Extraordinary;
                                        artifact.Resource = CraftResource.Obsidian;
                                        artifact.WeaponAttributes.HitPhysicalArea = 20;

                                        artifact.Name = "a xoloitzcuintle";

                                        artifact.DexRequirement = 100;
                                        artifact.Engraved1 = "This long, sinuous blade has seen better days; the obsidian it is crafted from has chipped apart " +
                                            "and fractured across the blade. It is, however, surprisingly light, even for its material; combined with its " +
                                            "length, the wielder would be able to swing this quite far. A skull shorn in half adorns where the blade meets " +
                                            "the haft, and a fierce dog clenches its teeth around the butt of the haft.";
                                        artifact.Engraved2 = "Historians of Azhuran history and weaponology would recognize this as a xoloitzcuintle, one of the " +
                                            "blades mass-produced during the last days of the battle with Memnon by the renowned Azhuran Stonesmith, " +
                                            "Xilomantzin. Most of them were lost in the destruction of Azhur, along with the smith's life; how this one " +
                                            "found its way into your hands is surely an interesting story.";

                                        AddToBackpack(artifact);
                                        break;
                                    }
                                case 2:
                                    {
                                        ThinLongsword artifact = new ThinLongsword();
                                        artifact.QualityDamage = 3;
                                        artifact.Quality = WeaponQuality.Extraordinary;
                                        artifact.Speed = 4.0;
                                        artifact.Resource = CraftResource.Gold;
                                        artifact.WeaponAttributes.HitLowerAttack = 5;

                                        artifact.Name = "a glorious golden blade";

                                        artifact.Engraved1 = "This beautifully-crafted blade is studded with sapphires, opals, and emeralds along the " +
                                            "hilt, the crossguard, and the center of the blade. Closer inspection reveals several scratches and " +
                                            "dents earned in what has probably been a long life for such a beautiful and expensive weapon.";
                                        artifact.Engraved2 = "Even looking upon this blade fills you with awe, and you suppose that any foe who faced " +
                                            "such an awesome weapon would be loathe to stand against anything so beautiful.";

                                        AddToBackpack(artifact);
                                        break;
                                    }
                                case 3:
                                    {
                                        LeatherCap artifact = new LeatherCap();
                                        artifact.PiercingBonus = 3;
                                        artifact.SlashingBonus = 3;
                                        artifact.BluntBonus = -3;
                                        artifact.Quality = ArmorQuality.Extraordinary;
                                        artifact.EnergyBonus = 20;
                                        artifact.Resource = CraftResource.BeastLeather;

                                        artifact.Name = "a hardshelled helm";

                                        artifact.Engraved1 = "This leather helm is undergirded with a series of chitinous shells; they probably compose a " +
                                            "larger shell, though the crafter appears to have carved them into smaller pieces and sewn them into " +
                                            "the lining of a very sturdy hide. The result is a rather lumpy-looking, but quite protective, helmet.";
                                        artifact.Engraved2 = "Closer examination of the shells sewn beneath the first layer of leather reveals that " +
                                            "they are unlike any shell found in nature -- turtle, insect, or otherwise. A sickly greenish hue, " +
                                            "they gleam unnaturally, and you think you can recall a madman on a Citadel streetcorner wearing " +
                                            "something similar. He was muttering something about 'keeping the voices out'.";

                                        AddToBackpack(artifact);
                                        break;
                                    }
                                case 4:
                                    {
                                        FancyMonocle artifact = new FancyMonocle();
                                        artifact.Attributes.NightSight = 100;
                                        artifact.Name = "a starsteel monocle";
                                        artifact.Resource = CraftResource.Starmetal;
                                        artifact.Quality = ClothingQuality.Masterwork;
                                        AddToBackpack(artifact);
                                        break;
                                    }
                            }
                        }
                        else
                            SetExcrementalLoot();
                        break;
                    }
            }
        }
        
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override int Meat { get { return Utility.Random(10); } }
        public override int Bones { get { return Utility.Random(10); } }
        #endregion

        #region Breath Weapon

        public override bool HasBreath { get { return true; } }
        public override double BreathDamageScalar { get { return 0.05; } }
        public override double BreathMinDelay { get { return 5.0; } }
        public override int BreathRange { get { return 10; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathPoisonDamage { get { return 50; } }
        public override int BreathBluntDamage { get { return 50; } }
        public override int BreathEffectItemID { get { return Utility.RandomList(2322, 2323, 3899, 3900, 4338, 4339, 7681, 7682); } }
        public override int BreathEffectHue { get { return Utility.RandomList(1051, 1058, 1116, 1124, 1148, 1190, 1845, 1854, 1881, 1890); } }
        public override int BreathEffectSound { get { return 0x1CA; } }
        public override bool BreathEffectExplodes { get { return false; } }

        public override void BreathDealDamage(Mobile target)
        {
            if (target is PlayerMobile && ((PlayerMobile)target).Evaded())
                return;

            AOS.Damage(target, this, BreathComputeDamage(), 50, 0, 0, 50, 0, 0, 0, 0);

            XmlHue newHueAtt = new XmlHue(BreathEffectHue, Utility.RandomMinMax(1, 10));

            if (Utility.RandomBool())
            {
                Item hueItem = target.Items[Utility.RandomMinMax(0, Math.Max(0, target.Items.Count - 1))];
                XmlAttach.AttachTo(hueItem, newHueAtt as XmlAttachment);

                if (hueItem.Name != null && hueItem.Name != "")
                    target.SendMessage("Your " + hueItem.Name + " has been stained by the excremental's waste!");
            }
            else
            {
                XmlAttach.AttachTo(target, newHueAtt as XmlAttachment);
                target.SendMessage("You have been stained by the execremental's waste!");
            }
            FecesTrailTimer newTimer = new FecesTrailTimer(target);
            newTimer.Start();
            m_FecesTrailTimers.Add(newTimer);
        }
        #endregion*/
/*
        public override void OnGaveMeleeAttack(Mobile target)
        {
            XmlHue newHueAtt = new XmlHue(BreathEffectHue, Utility.RandomMinMax(1, 10));
            if (Utility.RandomBool())
            {
                Item hueItem = target.Items[Utility.RandomMinMax(0, Math.Max(0, target.Items.Count - 1))];
                XmlAttach.AttachTo(hueItem, newHueAtt as XmlAttachment);

                if (hueItem.Name != null && hueItem.Name != "")
                    target.SendMessage("Your " + hueItem.Name + " has been stained by the excremental's waste!");
            }
            else
            {
                XmlAttach.AttachTo(target, newHueAtt as XmlAttachment);
                target.SendMessage("You have been stained by the execremental's waste!");
            }
            FecesTrailTimer newTimer = new FecesTrailTimer(target);
            newTimer.Start();
            FecesTrailTimers.Add(newTimer);
            base.OnGaveMeleeAttack(target);
        }

        private List<FecesTrailTimer> FecesTrailTimers
        {
            get
            {
                if (m_FecesTrailTimers == null)
                    m_FecesTrailTimers = new List<FecesTrailTimer>();
                return m_FecesTrailTimers;
            }
        }
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (attacker.Weapon is BaseMeleeWeapon && Utility.RandomBool())
            {
                XmlHue newHueAtt = new XmlHue(BreathEffectHue, Utility.RandomMinMax(1, 10));
                XmlAttach.AttachTo(attacker.Weapon, newHueAtt as XmlAttachment);

                switch (Utility.Random(10))
                {
                    case 0: attacker.PlaySound(0x021); break;
                    case 1: attacker.PlaySound(0x050); break;
                    case 2: attacker.PlaySound(0x1C8); break;
                    case 3: attacker.PlaySound(0x1C9); break;
                    case 4: attacker.PlaySound(0x428); break;
                    default: break;
                }
            }

            base.OnGotMeleeAttack(attacker);
        }

        protected override bool OnMove(Direction d)
        {
            if (Utility.RandomBool())
            {
                Item i = new Item(Utility.RandomMinMax(4650, 4655));
                i.Hue = Utility.RandomList(BreathEffectHue);
                i.Movable = false;
                i.Name = "fecal matter";
                i.MoveToWorld(Location, Map);
                new InternalTimer(i).Start();
            }

            return base.OnMove(d);
        }*/
   
/*
        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item i)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(Utility.RandomMinMax(10, 600)))
            {
                m_Item = i;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Item.Delete();

                base.OnTick();
            }
        }
    }

    public class FecesTrailTimer : Timer
    {
        private Mobile m_Player;
        private DateTime m_Instantiation;

        public FecesTrailTimer(Mobile m)
            : base(TimeSpan.FromSeconds(Utility.RandomMinMax(1,60)), TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)))
        {
            m_Player = m;
            Priority = TimerPriority.FiveSeconds;
            m_Instantiation = DateTime.Now;
        }

        protected override void OnTick()
        {
            if (m_Player == null || m_Player.Deleted || !m_Player.Alive)
                Stop();

            if (m_Instantiation + TimeSpan.FromMinutes(Utility.RandomMinMax(1, 5)) < DateTime.Now)
                Stop();
            else
            {
                if (Utility.RandomBool())
                {
                    Item i = new Item(Utility.RandomMinMax(4650, 4655));
                    i.Hue = Utility.RandomList(Utility.RandomList(1051, 1058, 1116, 1124, 1148, 1190, 1845, 1854, 1881, 1890));
                    i.Movable = false;
                    i.Name = "fecal matter";
                    i.MoveToWorld(m_Player.Location, m_Player.Map);
                    new InternalTimer(i).Start();
                    m_Player.Emote("*reeks of waste*");
                }
            }

            base.OnTick();
        }

        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item i)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(Utility.RandomMinMax(10, 600)))
            {
                m_Item = i;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Item.Delete();

                base.OnTick();
            }
        }
    }*/

