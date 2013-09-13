using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;
using Server.Targeting;
using Server.Targets;

namespace Server.Mobiles
{
    [CorpseName("a bird carcass")]
    public class BirdOfPrey : BaseBreedableCreature, ISmallPredator, ITinyPet
    {
        private DateTime m_LastSpecial;
        private bool m_Retrieving = false;
        private Item m_RetrieveItem;

        public bool Retrieving 
        { 
            get 
            { 
                return m_Retrieving; 
            } 
            set 
            {
                m_Retrieving = value;

                if (m_Retrieving)
                    CurrentSpeed = ActiveSpeed;
                else
                    CurrentSpeed = PassiveSpeed;
            } 
        }
        public Item RetrieveItem { get { return m_RetrieveItem; } set { m_RetrieveItem = value; } }

        public override int MaxCubs { get { return 2; } }
        public override bool ParryDisabled { get { return true; } }

        public BirdOfPrey()
            : base(AIType.AI_Melee, FightMode.Aggressor, 20, 1, 0.1, 0.2)
        {
            Name = "a bird of prey";
            Body = 5;
            BaseSoundID = 0x08F;

            SetStr(25,35);
            SetDex(75,85);
            SetInt(35,45);

            SetHits(30,50);
            SetMana(0);
            SetStam(30,60);

            SetDamage(5,7);


            SetDamageType(ResistanceType.Slashing, 80);
            SetDamageType(ResistanceType.Piercing, 20);

            SetResistance(ResistanceType.Blunt, 5, 10);
            SetResistance(ResistanceType.Slashing, 15, 30);
            SetResistance(ResistanceType.Piercing, 30, 45);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 0.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.UnarmedFighting, 50.1, 60.1);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 5;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 33.1;

            GiveFeat = "Dodge 3";
            GiveFeat = "EnhancedDodge 3";
            GiveFeat = "PureDodge 3";
            GiveFeat = "Evade 3";
            ManeuverResistance = 50;

            AddItem(new Backpack());
        }

        public override int Meat { get { return 2; } }
        public override int Bones { get { return 4; } }
        public override int Feathers { get { return Utility.RandomMinMax(10,30); } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void SetBreedsTraits(string breed, int group)
        {
            switch (breed)
            {
                case "Falcon":
                    {
                        this.Name = "a falcon";
                        this.Hue = Utility.RandomMinMax(1819, 1827);
                        this.ChangeBreed = "Falcon";
                        this.Body = 5;
                        break;
                    }

                case "Osprey":
                    {
                        this.Name = "an osprey";
                        this.Hue = Utility.RandomMinMax(2982, 2986);
                        this.ChangeBreed = "Osprey";
                        this.Body = 5;
                        break;
                    }

                case "Hawk":
                    {
                        this.Name = "a hawk";
                        this.Hue = Utility.RandomList(2301, 2306);
                        this.ChangeBreed = "Hawk";
                        this.Body = 5;
                        break;
                    }

                case "Owl":
                    {
                        this.Name = "an owl";
                        this.Hue = Utility.RandomList(1102, 1149);
                        this.ChangeBreed = "Owl";
                        this.Body = 5;
                        break;
                    }

                case "Harrier":
                    {
                        this.Name = "a harrier";
                        this.Hue = Utility.RandomList(1501, 1536);
                        this.ChangeBreed = "Harrier";
                        this.Body = 5;
                        break;
                    }

                case "Kite":
                    {
                        this.Name = "a kite";
                        this.Hue = Utility.RandomList(2307, 2318);
                        this.ChangeBreed = "Kite";
                        this.Body = 5;
                        break;
                    }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if (m_LastSpecial + TimeSpan.FromMinutes(Utility.Random(5, 15)) < DateTime.Now)
            {
                if (Utility.RandomMinMax(1, 100) <= Level)
                {
                    switch (Utility.RandomMinMax(1, 3))
                    {
                        case 1:
                            {
                                XmlAttach.AttachTo(defender, new XmlAosAttribute(AosAttribute.AttackChance, -1 * Utility.RandomMinMax(Level, 50), Utility.RandomMinMax(XPScale, 10)));
                                this.Emote("*rends " + defender.Name + "'s face with its beak, weakning " + (defender.Female == true ? "her" : "his") + " resolve!*");
                                break;
                            }
                        case 2:
                            {
                                XmlAttach.AttachTo(defender, new XmlAosAttribute(AosAttribute.DefendChance, -1 * Utility.RandomMinMax(Level, 50), Utility.RandomMinMax(XPScale, 10)));
                                this.Emote("*swoops at " + defender.Name + ", throwing " + (defender.Female == true ? "her" : "him") + " off balance!*");
                                break;
                            }
                        case 3:
                            {
                                XmlBleedingWound.BeginBleed(defender, this, 5 + (Level / (6 - XPScale)));
                                this.Emote("*rakes " + defender.Name + "'s flesh with its talons!*");
                                break;
                            }
                    }
                }
            }

            if ((defender is ITinyPet || defender is ISmallPrey || defender is IMediumPrey) && this.Hunger < 20 && Utility.RandomBool())
                this.Hunger++;

            base.OnGaveMeleeAttack(defender);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 20))
            {
                if (from.Map == this.Map)
                    return true;
            }

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (Controlled && ControlMaster != null && !ControlMaster.Deleted && e.Mobile == ControlMaster)
            {
                if (Insensitive.StartsWith(e.Speech, this.Name) || e.Speech.Contains("all"))
                {
                    if (e.Speech.ToLower().Contains("retrieve"))
                    {
                        e.Mobile.Target = new RetrieveTarget(this);
                        e.Mobile.SendMessage("Target an item to have " + this.Name + " retrieve it for you.");
                    }
                    else
                    {
                        if (Retrieving)
                        {
                            Retrieving = false;
                            RetrieveItem = null;
                            ControlOrder = OrderType.Come;
                        }
                    }
                }
            }

            base.OnSpeech(e);
        }

        public override void OnThink()
        {
            if (ControlOrder != OrderType.Attack && Controlled && ControlMaster != null && !ControlMaster.Deleted && Retrieving)
            {
                if (RetrieveItem != null && !RetrieveItem.Deleted)
                {
                    DebugSay("Seeking RetrieveItem...");
                    if (RetrieveItem.IsChildOf(this) || Backpack.Items.Contains(RetrieveItem))
                    {
                        DebugSay("I have RetrieveItem!");
                        bool itemRetrieved = false;
                        if (InRange(ControlMaster.Location, 1))
                        {
                            ControlMaster.PlaceInBackpack(RetrieveItem);
                            Emote("*drops the " + (RetrieveItem.Name == null ? "item" : RetrieveItem.Name.ToString()) + " into " + ControlMaster.Name.ToString() + "'s hands*");
                            Retrieving = false;
                            RetrieveItem = null;
                            ControlTarget = ControlMaster;
                            ControlOrder = OrderType.Follow;

                            itemRetrieved = true;
                        }

                        if (!itemRetrieved)
                        {
                            DebugSay("Seeking master...");
                            AIObject.DoMove(GetDirectionTo(ControlMaster));
                        }
                    }
                    else
                    {
                        DebugSay("I do not have RetrieveItem, seeking item...");
                        if ((RetrieveItem.RootParentEntity == null && InRange(RetrieveItem.Location, 1)) || (RetrieveItem.RootParentEntity != null && InRange(RetrieveItem.RootParentEntity.Location, 1)))
                        {
                            if (RetrieveItem.RootParentEntity != null && RetrieveItem.RootParentEntity is Mobile)
                            {
                                Mobile target = RetrieveItem.RootParentEntity as Mobile;

                                if (ControlMaster != null && ControlMaster.Deleted && ControlMaster.InRange(target.Location, 16))
                                    if (ControlMaster is PlayerMobile)
                                        (ControlMaster as PlayerMobile).CriminalActivity = true;

                                if (Utility.RandomMinMax(1, 100) + target.Str < Utility.RandomMinMax( (XPScale * XPScale), 100) + Str)
                                {
                                    Emote("*grapples with " + target.Name.ToString() + ", pulling " + (RetrieveItem.Name == null ? "the item" : RetrieveItem.Name.ToString()) + " from " + (target.Female ? "her" : "him") + "!*");
                                    PlaceInBackpack(RetrieveItem);
                                }
                                else
                                {
                                    Emote("*fails to pull " + (RetrieveItem.Name == null ? "the item" : RetrieveItem.Name.ToString()) + " from " + target.Name.ToString() + "!*");
                                    Retrieving = false;
                                    RetrieveItem = null;
                                    ControlOrder = OrderType.Come;
                                    AddAggressor = target;
                                    if (target is BaseCreature)
                                        (target as BaseCreature).AddAggressor = this;
                                }
                            }
                            else
                            {
                                PlaceInBackpack(RetrieveItem);
                                Emote("*grasps " + (RetrieveItem.Name == null ? "the item" : RetrieveItem.Name.ToString()) + " in its talons*");
                            }
                        }

                        if (RetrieveItem != null && !RetrieveItem.Deleted && !RetrieveItem.IsChildOf(this))
                        {
                            DebugSay("Moving towards item...");
                            if (RetrieveItem.RootParentEntity == null)
                                AIObject.DoMove(GetDirectionTo(RetrieveItem.Location));
                            else
                                AIObject.DoMove(GetDirectionTo(RetrieveItem.RootParentEntity.Location));
                        }
                    }
                }
                else
                {
                    Retrieving = false;
                    RetrieveItem = null;
                    ControlOrder = OrderType.None;
                }
            }

            base.OnThink();
        }

        public static bool CanTameBird(Mobile tamer, BirdOfPrey b)
        {
            if (tamer is PlayerMobile)
            {
                PlayerMobile pm = tamer as PlayerMobile;
                if (pm.Nation == Nation.Southern)
                    return true;
                switch (b.Breed)
                {
                    case "Falcon":  if (pm.Nation == Nation.Haluaroc) return true; break;
                    case "Osprey":  if (pm.Nation == Nation.Tirebladd) return true; break;
                    case "Hawk":    if (pm.Nation == Nation.Northern) return true; break;
                    case "Owl":     if (pm.Nation == Nation.Southern) return true; break;
                    case "Harrier": if (pm.Nation == Nation.Mhordul) return true; break;
                    case "Kite":    if (pm.Nation == Nation.Western) return true; break;
                }
            }

            return true;
        }

        public BirdOfPrey(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((DateTime)m_LastSpecial);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_LastSpecial = reader.ReadDateTime();

            m_Retrieving = false;
            m_RetrieveItem = null;
        }
    }

    public class RetrieveTarget : Target
    {
        private Mobile m_Retriever;

        public RetrieveTarget(Mobile m)
            : base(30, true, TargetFlags.None)
        {
            m_Retriever = m;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is Item)
            {
                Item i = targeted as Item;

                if (i is Corpse)
                {
                    Corpse c = i as Corpse;
                    if (!(c.Owner is ITinyPet) && !(c.Owner is ISmallPredator) && !(c.Owner is ISmallPrey))
                    {
                        from.SendMessage("That is too large for " + m_Retriever.Name.ToString() + " to carry.");
                        return;
                    }
                }

                if ( (i.Movable || i.CanBeGrabbed) && (i.RootParentEntity == null || (m_Retriever is BirdOfPrey && (m_Retriever as BirdOfPrey).Level >= 40)) && i.Weight <= (m_Retriever.Str / 4))
                {
                    if (m_Retriever is BirdOfPrey)
                    {
                        BirdOfPrey b = m_Retriever as BirdOfPrey;
                        if (b.Warmode || b.Combatant != null)
                        {
                            from.SendMessage(b.Name + " is still in combat.");
                        }
                        else
                        {
                            b.RetrieveItem = i;
                            b.Retrieving = true;
                            b.ControlTarget = null;
                            b.ControlOrder = OrderType.Stay;
                            from.SendMessage(b.Name.ToString() + " will fly to retrieve " + ( i.Name == null ? "that item" : i.Name.ToString() ) + ".");
                        }
                    }
                }
                else if (!i.Movable && !i.CanBeGrabbed)
                {
                    from.SendMessage(m_Retriever.Name + " cannot retrieve that.");
                    return;

                }
                else if (!i.Movable)
                {
                    from.SendMessage(m_Retriever.Name + " cannot retrieve that.");
                    return;
                }
                else if (i.RootParentEntity != null && (m_Retriever as BaseCreature).Level < 40)
                {
                    from.SendMessage(m_Retriever.Name + " cannot retrieve items from there.");
                    return;
                }
                else if (i.Weight > m_Retriever.Str / 4)
                {
                    from.SendMessage("That is too heavy for " + m_Retriever.Name + " to carry.");
                    return;
                }
            }

            base.OnTarget(from, targeted);
        }
    }

    public class Falcon : BirdOfPrey, IDesertCreature
	{
		public override int[] Hues{ get{ return new int[]{1819, 1820, 1821, 1822, 1823, 1824, 1825, 1826, 1827}; } }
		
		[Constructable]
		public Falcon() : base()
		{
			NewBreed = "Falcon";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new Falcon() );
		}

		public Falcon(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

    public class Osprey : BirdOfPrey, ITundraCreature
    {
        public override int[] Hues { get { return new int[] { 2982, 2983, 2984, 2985, 2986 }; } }

        [Constructable]
        public Osprey()
            : base()
        {
            NewBreed = "Osprey";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Osprey());
        }

        public Osprey(Serial serial)
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

    public class Hawk : BirdOfPrey, IPlainsCreature
    {
        public override int[] Hues { get { return new int[] { 2301, 2302, 2303, 2304, 2305, 2306 }; } }

        [Constructable]
        public Hawk()
            : base()
        {
            NewBreed = "Hawk";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Hawk());
        }

        public Hawk(Serial serial)
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

    public class Owl : BirdOfPrey, IForestCreature
    {
        public override int[] Hues { get { return new int[] { 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109,
                                                                1110, 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119,
                                                                1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129,
                                                                1130, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139,
                                                                1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149}; } }

        [Constructable]
        public Owl()
            : base()
        {
            NewBreed = "Owl";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Owl());
        }

        public Owl(Serial serial)
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

    public class Harrier : BirdOfPrey, ICaveCreature
    {
        public override int[] Hues
        {
            get
            {
                return new int[] { 1501, 1502, 1503, 1504, 1505, 1506, 1507, 1508, 1509, 1510,
                                    1511, 1512, 1513, 1514, 1515, 1516, 1517, 1518, 1519, 1520,
                                    1521, 1522, 1523, 1524, 1525, 1526, 1527, 1528, 1529, 1530,
                                    1531, 1532, 1533, 1534, 1535, 1536 };
            }
        }

        [Constructable]
        public Harrier()
            : base()
        {
            NewBreed = "Harrier";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Harrier());
        }

        public Harrier(Serial serial)
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

    public class Kite : BirdOfPrey, IJungleCreature
    {
        public override int[] Hues
        {
            get
            {
                return new int[] { 2307, 2308, 2309, 2310, 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318 };
            }
        }

        [Constructable]
        public Kite()
            : base()
        {
            NewBreed = "Kite";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Kite());
        }

        public Kite(Serial serial)
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
