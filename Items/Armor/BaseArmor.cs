using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Engines.Craft;
using Server.Factions;
using AMA = Server.Items.ArmorMeditationAllowance;
using AMT = Server.Items.ArmorMaterialType;
using ABT = Server.Items.ArmorBodyType;
using Server.Mobiles;
using Server.Misc;
using Server.Commands;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
    public enum ArmourWeight
    {
        Light,
        Medium,
        Heavy
    }

    public abstract class BaseArmor : Item, IScissorable, IFactionItem, ICraftable, IWearableDurability
    {
        #region Factions
        private FactionItem m_FactionState;

        public FactionItem FactionItemState
        {
            get { return m_FactionState; }
            set
            {
                m_FactionState = value;

                if (m_FactionState == null)
                    Hue = CraftResources.GetHue(Resource);

                LootType = (m_FactionState == null ? LootType.Regular : LootType.Blessed);
            }
        }
        #endregion


        /* Armor internals work differently now (Jun 19 2003)
		 * 
		 * The attributes defined below default to -1.
		 * If the value is -1, the corresponding virtual 'Aos/Old' property is used.
		 * If not, the attribute value itself is used. Here's the list:
		 *  - ArmorBase
		 *  - StrBonus
		 *  - DexBonus
		 *  - IntBonus
		 *  - StrReq
		 *  - DexReq
		 *  - IntReq
		 *  - MeditationAllowance
		 */

        // Instance values. These values must are unique to each armor piece.
        private int m_MaxHitPoints;
        private int m_HitPoints;
        private Mobile m_Crafter;
        private ArmorQuality m_Quality;
        private ArmorDurabilityLevel m_Durability;
        private ArmorProtectionLevel m_Protection;
        private CraftResource m_Resource;
        private bool m_Identified, m_PlayerConstructed;
        private int m_PhysicalBonus, m_FireBonus, m_ColdBonus, m_PoisonBonus, m_EnergyBonus, m_BluntBonus, m_SlashingBonus, m_PiercingBonus;

        private AosAttributes m_AosAttributes;
        private AosArmorAttributes m_AosArmorAttributes;
        private AosSkillBonuses m_AosSkillBonuses;

        // Overridable values. These values are provided to override the defaults which get defined in the individual armor scripts.
        private int m_ArmorBase = -1;
        private int m_StrBonus = -1, m_DexBonus = -1, m_IntBonus = -1;
        private int m_HitsMaxBonus = -1;
        private int m_StamMaxBonus = -1;
        private int m_ManaMaxBonus = -1;
        private int m_StrReq = -1, m_DexReq = -1, m_IntReq = -1;
        private AMA m_Meditate = (AMA)(-1);

        public virtual ArmourWeight ArmourType { get { return ArmourWeight.Light; } }

        public virtual bool AllowMaleWearer { get { return true; } }
        public virtual bool AllowFemaleWearer { get { return true; } }

        public abstract AMT MaterialType { get; }

        public virtual int RevertArmorBase { get { return ArmorBase; } }
        public virtual int ArmorBase { get { return 0; } }

        public virtual AMA DefMedAllowance { get { return AMA.None; } }
        public virtual AMA AosMedAllowance { get { return DefMedAllowance; } }
        public virtual AMA OldMedAllowance { get { return DefMedAllowance; } }

        public virtual int AosHitsMaxBonus { get { return 0; } }
        public virtual int AosStamMaxBonus { get { return 0; } }
        public virtual int AosManaMaxBonus { get { return 0; } }

        public virtual int AosStrBonus { get { return 0; } }
        public virtual int AosDexBonus { get { return 0; } }
        public virtual int AosIntBonus { get { return 0; } }
        public virtual int AosStrReq { get { return 0; } }
        public virtual int AosDexReq { get { return 0; } }
        public virtual int AosIntReq { get { return 0; } }

        public virtual int OldHitsMaxBonus { get { return 0; } }
        public virtual int OldStamMaxBonus { get { return 0; } }
        public virtual int OldManaMaxBonus { get { return 0; } }

        public virtual int OldStrBonus { get { return 0; } }
        public virtual int OldDexBonus { get { return 0; } }
        public virtual int OldIntBonus { get { return 0; } }
        public virtual int OldStrReq { get { return 0; } }
        public virtual int OldDexReq { get { return 0; } }
        public virtual int OldIntReq { get { return 0; } }

        private string m_Engraved1;
        private string m_Engraved2;
        private string m_Engraved3;
        private string m_CraftersOriginalName;

        private bool m_OldArmor = true;

        [CommandProperty(AccessLevel.Administrator)]
        public bool OldArmor
        {
            get { return m_OldArmor; }
            set { m_OldArmor = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Engraved1
        {
            get { return m_Engraved1; }
            set { m_Engraved1 = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Engraved2
        {
            get { return m_Engraved2; }
            set { m_Engraved2 = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Engraved3
        {
            get { return m_Engraved3; }
            set { m_Engraved3 = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string CraftersOriginalName
        {
            get { return m_CraftersOriginalName; }
            set { m_CraftersOriginalName = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AMA MeditationAllowance
        {
            get { return (m_Meditate == (AMA)(-1) ? Core.AOS ? AosMedAllowance : OldMedAllowance : m_Meditate); }
            set { m_Meditate = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseArmorRating
        {
            get
            {
                if (m_ArmorBase == -1)
                    return ArmorBase;
                else
                    return m_ArmorBase;
            }
            set
            {
                m_ArmorBase = value; Invalidate();
            }
        }

        public double BaseArmorRatingScaled
        {
            get
            {
                return (BaseArmorRating * ArmorScalar);
            }
        }

        public virtual double ArmorRating
        {
            get
            {
                int ar = BaseArmorRating;

                if (m_Protection != ArmorProtectionLevel.Regular)
                    ar += 10 + (5 * (int)m_Protection);

                switch (m_Resource)
                {
                    case CraftResource.Tin: ar -= 1; break;
                    case CraftResource.Oak: ar += 4; break;
                    case CraftResource.Copper: ar += 6; break;
                    case CraftResource.Yew: ar += 6; break;
                    case CraftResource.Bronze: ar += 8; break;
                    case CraftResource.Electrum: ar += 8; break;
                    case CraftResource.Redwood: ar += 8; break;
                    case CraftResource.Gold: ar += 0; break;
                    case CraftResource.Silver: ar += 0; break;
                    case CraftResource.Iron: ar += 12; break;
                    case CraftResource.Ash: ar += 12; break;
                    case CraftResource.Obsidian: ar -= 1; break;
                    case CraftResource.Steel: ar += 16; break;
                    case CraftResource.Starmetal: ar += 20; break;
                    case CraftResource.Greenheart: ar += 16; break;
                    case CraftResource.ThickLeather: ar += 10; break;
                    case CraftResource.BeastLeather: ar += 13; break;
                    case CraftResource.ScaledLeather: ar += 16; break;
                }

                ar += -8 + (8 * (int)m_Quality);
                return ScaleArmorByDurability(ar);
            }
        }

        public double ArmorRatingScaled
        {
            get
            {
                return (ArmorRating * ArmorScalar);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMaxBonus
        {
            get { return (m_HitsMaxBonus == -1 ? Core.AOS ? AosHitsMaxBonus : OldHitsMaxBonus : m_HitsMaxBonus); }
            set { m_HitsMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StamMaxBonus
        {
            get { return (m_StamMaxBonus == -1 ? Core.AOS ? AosStamMaxBonus : OldStamMaxBonus : m_StamMaxBonus); }
            set { m_StamMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ManaMaxBonus
        {
            get { return (m_ManaMaxBonus == -1 ? Core.AOS ? AosManaMaxBonus : OldManaMaxBonus : m_ManaMaxBonus); }
            set { m_ManaMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrBonus
        {
            get { return (m_StrBonus == -1 ? Core.AOS ? AosStrBonus : OldStrBonus : m_StrBonus); }
            set { m_StrBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexBonus
        {
            get { return (m_DexBonus == -1 ? Core.AOS ? AosDexBonus : OldDexBonus : m_DexBonus); }
            set { m_DexBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntBonus
        {
            get { return (m_IntBonus == -1 ? Core.AOS ? AosIntBonus : OldIntBonus : m_IntBonus); }
            set { m_IntBonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrRequirement
        {
            get { return (m_StrReq == -1 ? Core.AOS ? AosStrReq : OldStrReq : m_StrReq); }
            set { m_StrReq = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexRequirement
        {
            get { return (m_DexReq == -1 ? Core.AOS ? AosDexReq : OldDexReq : m_DexReq); }
            set { m_DexReq = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntRequirement
        {
            get { return (m_IntReq == -1 ? Core.AOS ? AosIntReq : OldIntReq : m_IntReq); }
            set { m_IntReq = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified
        {
            get { return m_Identified; }
            set { m_Identified = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get { return m_PlayerConstructed; }
            set { m_PlayerConstructed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return m_Resource;
            }
            set
            {
                if (m_Resource != value)
                {
                    UnscaleDurability();

                    m_Resource = value;
                    Hue = CraftResources.GetHue(m_Resource);

                    Invalidate();
                    InvalidateProperties();

                    if (Parent is Mobile)
                        ((Mobile)Parent).UpdateResistances();

                    ScaleDurability();
                }
            }
        }

        public virtual double ArmorScalar
        {
            get
            {
                int pos = (int)BodyPosition;

                if (pos >= 0 && pos < m_ArmorScalars.Length)
                    return m_ArmorScalars[pos];

                return 1.0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get { return m_MaxHitPoints; }
            set { m_MaxHitPoints = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get
            {
                return m_HitPoints;
            }
            set
            {
                if (value != m_HitPoints && MaxHitPoints > 0)
                {
                    m_HitPoints = value;

                    if (m_HitPoints < 0)
                        Delete();
                    else if (m_HitPoints > MaxHitPoints)
                        m_HitPoints = MaxHitPoints;

                    InvalidateProperties();
                }
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return m_Crafter; }
            set { m_Crafter = value; InvalidateProperties(); }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorQuality Quality
        {
            get { return m_Quality; }
            set { UnscaleDurability(); m_Quality = value; Invalidate(); InvalidateProperties(); ScaleDurability(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorDurabilityLevel Durability
        {
            get { return m_Durability; }
            set { UnscaleDurability(); m_Durability = value; ScaleDurability(); InvalidateProperties(); }
        }

        public virtual int ArtifactRarity
        {
            get { return 0; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorProtectionLevel ProtectionLevel
        {
            get
            {
                return m_Protection;
            }
            set
            {
                if (m_Protection != value)
                {
                    m_Protection = value;

                    Invalidate();
                    InvalidateProperties();

                    if (Parent is Mobile)
                        ((Mobile)Parent).UpdateResistances();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes
        {
            get { return m_AosAttributes; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosArmorAttributes ArmorAttributes
        {
            get { return m_AosArmorAttributes; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses
        {
            get { return m_AosSkillBonuses; }
            set { }
        }

        public int ComputeStatReq(StatType type)
        {
            int v;

            if (type == StatType.Str)
                v = StrRequirement;
            else if (type == StatType.Dex)
                v = DexRequirement;
            else
                v = IntRequirement;

            return AOS.Scale(v, 100 - GetLowerStatReq());
        }

        public int ComputeStatBonus(StatType type)
        {
            if (type == StatType.Str)
                return StrBonus + Attributes.BonusStr;
            else if (type == StatType.Dex)
                return DexBonus + Attributes.BonusDex;
            else if (type == StatType.Int)
                return IntBonus + Attributes.BonusInt;
            else if (type == StatType.HitsMax)
                return HitsMaxBonus + Attributes.BonusHitsMax;
            else if (type == StatType.StamMax)
                return StamMaxBonus + Attributes.BonusStamMax;
            else
                return ManaMaxBonus + Attributes.BonusManaMax;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalBonus { get { return m_PhysicalBonus; } set { m_PhysicalBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireBonus { get { return m_FireBonus; } set { m_FireBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdBonus { get { return m_ColdBonus; } set { m_ColdBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonBonus { get { return m_PoisonBonus; } set { m_PoisonBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyBonus { get { return m_EnergyBonus; } set { m_EnergyBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BluntBonus { get { return m_BluntBonus; } set { m_BluntBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SlashingBonus { get { return m_SlashingBonus; } set { m_SlashingBonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PiercingBonus { get { return m_PiercingBonus; } set { m_PiercingBonus = value; InvalidateProperties(); } }

        public virtual int BasePhysicalResistance { get { return 0; } }
        public virtual int BaseFireResistance { get { return 0; } }
        public virtual int BaseColdResistance { get { return 0; } }
        public virtual int BasePoisonResistance { get { return 0; } }
        public virtual int BaseEnergyResistance { get { return 0; } }
        public virtual int BaseBluntResistance { get { return 0; } }
        public virtual int BaseSlashingResistance { get { return 0; } }
        public virtual int BasePiercingResistance { get { return 0; } }

        public override int PhysicalResistance { get { return BasePhysicalResistance + GetProtOffset() + GetResourceAttrs().ArmorPhysicalResist + m_PhysicalBonus; } }
        public override int FireResistance { get { return BaseFireResistance + GetProtOffset() + GetResourceAttrs().ArmorFireResist + m_FireBonus; } }
        public override int ColdResistance { get { return BaseColdResistance + GetProtOffset() + GetResourceAttrs().ArmorColdResist + m_ColdBonus; } }
        public override int PoisonResistance { get { return BasePoisonResistance + GetProtOffset() + GetResourceAttrs().ArmorPoisonResist + m_PoisonBonus; } }
        public override int EnergyResistance { get { return BaseEnergyResistance + GetProtOffset() + GetResourceAttrs().ArmorEnergyResist + m_EnergyBonus; } }
        public override int BluntResistance { get { return BaseBluntResistance + GetProtOffset() + GetResourceAttrs().ArmorBluntResist + m_BluntBonus; } }
        public override int SlashingResistance { get { return BaseSlashingResistance + GetProtOffset() + GetResourceAttrs().ArmorSlashingResist + m_SlashingBonus; } }
        public override int PiercingResistance { get { return BasePiercingResistance + GetProtOffset() + GetResourceAttrs().ArmorPiercingResist + m_PiercingBonus; } }

        public virtual int InitMinHits { get { return 0; } }
        public virtual int InitMaxHits { get { return 0; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorBodyType BodyPosition
        {
            get
            {
                switch (this.Layer)
                {
                    default:
                    case Layer.Neck: return ArmorBodyType.Gorget;
                    case Layer.TwoHanded: return ArmorBodyType.Shield;
                    case Layer.Gloves: return ArmorBodyType.Gloves;
                    case Layer.Helm: return ArmorBodyType.Helmet;
                    case Layer.Arms: return ArmorBodyType.Arms;

                    case Layer.InnerLegs:
                    case Layer.OuterLegs:
                    case Layer.Pants: return ArmorBodyType.Legs;

                    case Layer.InnerTorso:
                    case Layer.OuterTorso:
                    case Layer.Shirt: return ArmorBodyType.Chest;
                }
            }
        }

        public void DistributeBonuses(int amount, int bonus)
        {
            for (int i = 0; i < amount; ++i)
            {
                switch (Utility.RandomMinMax(1, 3))
                {
                    case 1: m_BluntBonus += bonus; break;
                    case 2: m_SlashingBonus += bonus; break;
                    case 3: m_PiercingBonus += bonus; break;
                }
            }

            InvalidateProperties();
        }

        public CraftAttributeInfo GetResourceAttrs()
        {
            CraftResourceInfo info = CraftResources.GetInfo(m_Resource);

            if (info == null)
                return CraftAttributeInfo.Blank;

            return info.AttributeInfo;
        }

        public int GetProtOffset()
        {
            switch (m_Protection)
            {
                case ArmorProtectionLevel.Guarding: return 1;
                case ArmorProtectionLevel.Hardening: return 2;
                case ArmorProtectionLevel.Fortification: return 3;
                case ArmorProtectionLevel.Invulnerability: return 4;
            }

            return 0;
        }

        public void UnscaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_HitPoints = ((m_HitPoints * 100) + (scale - 1)) / scale;
            m_MaxHitPoints = ((m_MaxHitPoints * 100) + (scale - 1)) / scale;
            InvalidateProperties();
        }

        public void ScaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_HitPoints = ((m_HitPoints * scale) + 99) / 100;
            m_MaxHitPoints = ((m_MaxHitPoints * scale) + 99) / 100;
            InvalidateProperties();
        }

        public int GetDurabilityBonus()
        {
            int bonus = 0;

            if (m_Quality == ArmorQuality.Exceptional)
                bonus += 25;

            if (m_Quality == ArmorQuality.Extraordinary)
                bonus += 50;

            if (m_Quality == ArmorQuality.Masterwork)
                bonus += 75;

            switch (m_Durability)
            {
                case ArmorDurabilityLevel.Durable: bonus += 20; break;
                case ArmorDurabilityLevel.Substantial: bonus += 50; break;
                case ArmorDurabilityLevel.Massive: bonus += 70; break;
                case ArmorDurabilityLevel.Fortified: bonus += 100; break;
                case ArmorDurabilityLevel.Indestructible: bonus += 120; break;
            }

            if (Core.AOS)
            {
                bonus += m_AosArmorAttributes.DurabilityBonus;

                CraftResourceInfo resInfo = CraftResources.GetInfo(m_Resource);
                CraftAttributeInfo attrInfo = null;

                if (resInfo != null)
                    attrInfo = resInfo.AttributeInfo;

                if (attrInfo != null)
                    bonus += attrInfo.ArmorDurability;
            }

            return bonus;
        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack.
                return false;
            }

            CraftSystem system = DefTailoring.CraftSystem;

            CraftItem item = system.CraftItems.SearchFor(GetType());

            if (item != null && item.Ressources.Count == 1 && item.Ressources.GetAt(0).Amount >= 2)
            {
                try
                {
                    Item res = (Item)Activator.CreateInstance(CraftResources.GetInfo(m_Resource).ResourceTypes[0]);

                    ScissorHelper(from, res, m_PlayerConstructed ? (item.Ressources.GetAt(0).Amount / 2) : 1);
                    return true;
                }
                catch
                {
                }
            }

            from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
            return false;
        }

        private static double[] m_ArmorScalars = { 0.07, 0.07, 0.14, 0.15, 0.22, 0.35 };

        public static double[] ArmorScalars
        {
            get
            {
                return m_ArmorScalars;
            }
            set
            {
                m_ArmorScalars = value;
            }
        }

        public static void ValidateMobile(Mobile m)
        {
            for (int i = m.Items.Count - 1; i >= 0; --i)
            {
                if (i >= m.Items.Count)
                    continue;

                Item item = m.Items[i];

                if (item is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)item;

                    
                    if (m is PlayerMobile)
                    {
                    	
                    PlayerMobile h = m as PlayerMobile;
                    if (h.Feats.GetFeatLevel(FeatList.Invocation) > 1)
                    {
                    	if (armor.Resource == CraftResource.Electrum )
                	 {

                            m.SendMessage("The metal burns against your skin, preventing you from wearing it.");

                	 m.AddToBackpack(armor);
                	 }
                    }
                    }
                    if (armor.RequiredRace != null && m.Race != armor.RequiredRace)
                    {
                        if (armor.RequiredRace == Race.Elf)
                            m.SendLocalizedMessage(1072203); // Only Elves may use this.
                        else
                            m.SendMessage("Only {0} may use this.", armor.RequiredRace.PluralName);

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowFemaleWearer)
                            m.SendLocalizedMessage(1010388); // Only females can wear this.
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowMaleWearer)
                            m.SendLocalizedMessage(1063343); // Only males can wear this.
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                }
            }
        }

        public int GetLowerStatReq()
        {
            if (!Core.AOS)
                return 0;

            int v = m_AosArmorAttributes.LowerStatReq;

            CraftResourceInfo info = CraftResources.GetInfo(m_Resource);

            if (info != null)
            {
                CraftAttributeInfo attrInfo = info.AttributeInfo;

                if (attrInfo != null)
                    v += attrInfo.ArmorLowerRequirements;
            }

            if (v > 100)
                v = 100;

            return v;
        }

        public override void OnAdded(object parent)
        {
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                if (Core.AOS)
                    m_AosSkillBonuses.AddTo(from);

                from.Delta(MobileDelta.Armor); // Tell them armor rating has changed

                if (from is PlayerMobile)
                {
                    PlayerMobile pm = from as PlayerMobile;

                    if (pm.HasGump(typeof(CharInfoGump)) && pm.m_CharInfoTimer == null)
                    {
                        pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer(pm);
                        pm.m_CharInfoTimer.Start();
                    }
                }

                if (parent is Mercenary)
                {
                    BaseCreature bc = parent as BaseCreature;
                    int offset = 1;

                    if (this.ArmourType == ArmourWeight.Medium)
                        offset = 2;

                    else if (this.ArmourType == ArmourWeight.Heavy)
                        offset = 3;

                    //bc.VirtualArmor += offset;
                    bc.BluntResistSeed += offset;
                    bc.SlashingResistSeed += offset;
                    bc.PiercingResistSeed += offset;
                }

                string modName = this.Serial.ToString();

                if (Attributes.BonusStr != 0)
                    from.AddStatMod(new StatMod(StatType.Str, modName + "Str", Attributes.BonusStr, TimeSpan.Zero));

                if (Attributes.BonusDex != 0)
                    from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", Attributes.BonusDex, TimeSpan.Zero));

                if (Attributes.BonusInt != 0)
                    from.AddStatMod(new StatMod(StatType.Int, modName + "Int", Attributes.BonusInt, TimeSpan.Zero));

                if (Attributes.BonusHitsMax != 0)
                    from.AddStatMod(new StatMod(StatType.HitsMax, modName + "Hits", Attributes.BonusHitsMax, TimeSpan.Zero));

                if (Attributes.BonusStamMax != 0)
                    from.AddStatMod(new StatMod(StatType.StamMax, modName + "Stam", Attributes.BonusStamMax, TimeSpan.Zero));

                if (Attributes.BonusManaMax != 0)
                    from.AddStatMod(new StatMod(StatType.ManaMax, modName + "Mana", Attributes.BonusManaMax, TimeSpan.Zero));
            }
        }

        public virtual double ScaleArmorByDurability(double armor)
        {
            int scale = 100;

            if (m_MaxHitPoints > 0 && m_HitPoints < m_MaxHitPoints)
                scale = 50 + ((50 * m_HitPoints) / m_MaxHitPoints);

            return (armor * scale) / 100;
        }

        protected void Invalidate()
        {
            if (Parent is Mobile)
                ((Mobile)Parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
        }

        public BaseArmor(Serial serial)
            : base(serial)
        {
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            ArmorAttributes = 0x00000002,
            PhysicalBonus = 0x00000004,
            FireBonus = 0x00000008,
            ColdBonus = 0x00000010,
            PoisonBonus = 0x00000020,
            EnergyBonus = 0x00000040,
            Identified = 0x00000080,
            MaxHitPoints = 0x00000100,
            HitPoints = 0x00000200,
            Crafter = 0x00000400,
            Quality = 0x00000800,
            Durability = 0x00001000,
            Protection = 0x00002000,
            Resource = 0x00004000,
            BaseArmor = 0x00008000,
            StrBonus = 0x00010000,
            DexBonus = 0x00020000,
            IntBonus = 0x00040000,
            StrReq = 0x00080000,
            DexReq = 0x00100000,
            IntReq = 0x00200000,
            MedAllowance = 0x00400000,
            SkillBonuses = 0x00800000,
            PlayerConstructed = 0x01000000,
            BluntBonus = 0x02000000,
            SlashingBonus = 0x04000000,
            PiercingBonus = 0x08000000,
            HitsMaxBonus = 0x10000000,
            StamMaxBonus = 0x20000000,
            ManaMaxBonus = 0x40000000

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)12); // version

            writer.Write((bool)m_OldArmor);

            writer.Write((string)m_CraftersOriginalName);

            writer.Write((string)m_Engraved1);
            writer.Write((string)m_Engraved2);
            writer.Write((string)m_Engraved3);

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.Attributes, !m_AosAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.ArmorAttributes, !m_AosArmorAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.BluntBonus, m_BluntBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.SlashingBonus, m_SlashingBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.PiercingBonus, m_PiercingBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.PhysicalBonus, m_PhysicalBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.FireBonus, m_FireBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.ColdBonus, m_ColdBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.PoisonBonus, m_PoisonBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.EnergyBonus, m_EnergyBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.Identified, m_Identified != false);
            SetSaveFlag(ref flags, SaveFlag.MaxHitPoints, m_MaxHitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.HitPoints, m_HitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != ArmorQuality.Regular);
            SetSaveFlag(ref flags, SaveFlag.Durability, m_Durability != ArmorDurabilityLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Protection, m_Protection != ArmorProtectionLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Resource, m_Resource != DefaultResource);
            SetSaveFlag(ref flags, SaveFlag.BaseArmor, m_ArmorBase != -1);

            SetSaveFlag(ref flags, SaveFlag.HitsMaxBonus, m_HitsMaxBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.StamMaxBonus, m_StamMaxBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.ManaMaxBonus, m_ManaMaxBonus != -1);

            SetSaveFlag(ref flags, SaveFlag.StrBonus, m_StrBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.DexBonus, m_DexBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.IntBonus, m_IntBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.StrReq, m_StrReq != -1);
            SetSaveFlag(ref flags, SaveFlag.DexReq, m_DexReq != -1);
            SetSaveFlag(ref flags, SaveFlag.IntReq, m_IntReq != -1);
            SetSaveFlag(ref flags, SaveFlag.MedAllowance, m_Meditate != (AMA)(-1));
            SetSaveFlag(ref flags, SaveFlag.SkillBonuses, !m_AosSkillBonuses.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed != false);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Attributes))
                m_AosAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                m_AosArmorAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.BluntBonus))
                writer.WriteEncodedInt((int)m_BluntBonus);

            if (GetSaveFlag(flags, SaveFlag.SlashingBonus))
                writer.WriteEncodedInt((int)m_SlashingBonus);

            if (GetSaveFlag(flags, SaveFlag.PiercingBonus))
                writer.WriteEncodedInt((int)m_PiercingBonus);

            if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                writer.WriteEncodedInt((int)m_PhysicalBonus);

            if (GetSaveFlag(flags, SaveFlag.FireBonus))
                writer.WriteEncodedInt((int)m_FireBonus);

            if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                writer.WriteEncodedInt((int)m_ColdBonus);

            if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                writer.WriteEncodedInt((int)m_PoisonBonus);

            if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                writer.WriteEncodedInt((int)m_EnergyBonus);

            if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                writer.WriteEncodedInt((int)m_MaxHitPoints);

            if (GetSaveFlag(flags, SaveFlag.HitPoints))
                writer.WriteEncodedInt((int)m_HitPoints);

            if (GetSaveFlag(flags, SaveFlag.Crafter))
                writer.Write((Mobile)m_Crafter);

            if (GetSaveFlag(flags, SaveFlag.Quality))
                writer.WriteEncodedInt((int)m_Quality);

            if (GetSaveFlag(flags, SaveFlag.Durability))
                writer.WriteEncodedInt((int)m_Durability);

            if (GetSaveFlag(flags, SaveFlag.Protection))
                writer.WriteEncodedInt((int)m_Protection);

            if (GetSaveFlag(flags, SaveFlag.Resource))
                writer.WriteEncodedInt((int)m_Resource);

            if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                writer.WriteEncodedInt((int)m_ArmorBase);

            if (GetSaveFlag(flags, SaveFlag.HitsMaxBonus))
                writer.WriteEncodedInt((int)m_HitsMaxBonus);

            if (GetSaveFlag(flags, SaveFlag.StamMaxBonus))
                writer.WriteEncodedInt((int)m_StamMaxBonus);

            if (GetSaveFlag(flags, SaveFlag.ManaMaxBonus))
                writer.WriteEncodedInt((int)m_ManaMaxBonus);

            if (GetSaveFlag(flags, SaveFlag.StrBonus))
                writer.WriteEncodedInt((int)m_StrBonus);

            if (GetSaveFlag(flags, SaveFlag.DexBonus))
                writer.WriteEncodedInt((int)m_DexBonus);

            if (GetSaveFlag(flags, SaveFlag.IntBonus))
                writer.WriteEncodedInt((int)m_IntBonus);

            if (GetSaveFlag(flags, SaveFlag.StrReq))
                writer.WriteEncodedInt((int)m_StrReq);

            if (GetSaveFlag(flags, SaveFlag.DexReq))
                writer.WriteEncodedInt((int)m_DexReq);

            if (GetSaveFlag(flags, SaveFlag.IntReq))
                writer.WriteEncodedInt((int)m_IntReq);

            if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                writer.WriteEncodedInt((int)m_Meditate);

            if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                m_AosSkillBonuses.Serialize(writer);


        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            //if (version == 11)
            //version = 12;

            switch (version)
            {
                case 12: m_OldArmor = reader.ReadBool(); goto case 11;
                case 11:
                case 10:
                    {
                        m_CraftersOriginalName = reader.ReadString();
                        goto case 9;
                    }
                case 9:
                    {
                        m_Engraved1 = reader.ReadString();
                        m_Engraved2 = reader.ReadString();
                        m_Engraved3 = reader.ReadString();
                        goto case 5;
                    }
                case 8:
                case 7:
                case 6:
                case 5:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Attributes))
                            m_AosAttributes = new AosAttributes(this, reader);
                        else
                            m_AosAttributes = new AosAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                            m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        else
                            m_AosArmorAttributes = new AosArmorAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.BluntBonus))
                            m_BluntBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.SlashingBonus))
                            m_SlashingBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.PiercingBonus))
                            m_PiercingBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                            m_PhysicalBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.FireBonus))
                            m_FireBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                            m_ColdBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                            m_PoisonBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                            m_EnergyBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Identified))
                            m_Identified = (version >= 7 || reader.ReadBool());

                        if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                            m_MaxHitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.HitPoints))
                            m_HitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Crafter))
                            m_Crafter = reader.ReadMobile();

                        if (GetSaveFlag(flags, SaveFlag.Quality))
                        {
                            m_Quality = (ArmorQuality)reader.ReadEncodedInt();

                        }
                        else
                            m_Quality = ArmorQuality.Regular;

                        if (version == 5 && m_Quality == ArmorQuality.Low)
                            m_Quality = ArmorQuality.Regular;

                        if (GetSaveFlag(flags, SaveFlag.Durability))
                        {
                            m_Durability = (ArmorDurabilityLevel)reader.ReadEncodedInt();

                            if (m_Durability > ArmorDurabilityLevel.Indestructible)
                                m_Durability = ArmorDurabilityLevel.Durable;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Protection))
                        {
                            m_Protection = (ArmorProtectionLevel)reader.ReadEncodedInt();

                            if (m_Protection > ArmorProtectionLevel.Invulnerability)
                                m_Protection = ArmorProtectionLevel.Defense;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Resource))
                            m_Resource = (CraftResource)reader.ReadEncodedInt();
                        else
                            m_Resource = DefaultResource;

                        if (m_Resource == CraftResource.None)
                            m_Resource = DefaultResource;

                        if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                            m_ArmorBase = reader.ReadEncodedInt();
                        else
                            m_ArmorBase = -1;

                        if (GetSaveFlag(flags, SaveFlag.HitsMaxBonus))
                            m_HitsMaxBonus = reader.ReadEncodedInt();
                        else
                            m_HitsMaxBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.StamMaxBonus))
                            m_StamMaxBonus = reader.ReadEncodedInt();
                        else
                            m_StamMaxBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.ManaMaxBonus))
                            m_ManaMaxBonus = reader.ReadEncodedInt();
                        else
                            m_ManaMaxBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrBonus))
                            m_StrBonus = reader.ReadEncodedInt();
                        else
                            m_StrBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexBonus))
                            m_DexBonus = reader.ReadEncodedInt();
                        else
                            m_DexBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntBonus))
                            m_IntBonus = reader.ReadEncodedInt();
                        else
                            m_IntBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrReq))
                            m_StrReq = reader.ReadEncodedInt();
                        else
                            m_StrReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexReq))
                            m_DexReq = reader.ReadEncodedInt();
                        else
                            m_DexReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntReq))
                            m_IntReq = reader.ReadEncodedInt();
                        else
                            m_IntReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                            m_Meditate = (AMA)reader.ReadEncodedInt();
                        else
                            m_Meditate = (AMA)(-1);

                        if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                            m_AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (GetSaveFlag(flags, SaveFlag.PlayerConstructed))
                            m_PlayerConstructed = true;

                        break;
                    }
                case 4:
                    {
                        m_AosAttributes = new AosAttributes(this, reader);
                        m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        goto case 3;
                    }
                case 3:
                    {
                        //m_BluntBonus = reader.ReadInt();
                        // m_SlashingBonus = reader.ReadInt();
                        // m_PiercingBonus = reader.ReadInt();
                        m_PhysicalBonus = reader.ReadInt();
                        m_FireBonus = reader.ReadInt();
                        m_ColdBonus = reader.ReadInt();
                        m_PoisonBonus = reader.ReadInt();
                        m_EnergyBonus = reader.ReadInt();
                        goto case 2;
                    }
                case 2:
                case 1:
                    {
                        m_Identified = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_ArmorBase = reader.ReadInt();
                        m_MaxHitPoints = reader.ReadInt();
                        m_HitPoints = reader.ReadInt();
                        m_Crafter = reader.ReadMobile();
                        m_Quality = (ArmorQuality)reader.ReadInt();
                        m_Durability = (ArmorDurabilityLevel)reader.ReadInt();
                        m_Protection = (ArmorProtectionLevel)reader.ReadInt();

                        AMT mat = (AMT)reader.ReadInt();

                        if (m_ArmorBase == RevertArmorBase)
                            m_ArmorBase = -1;

                        /*m_BodyPos = (ArmorBodyType)*/
                        reader.ReadInt();

                        if (version < 4)
                        {
                            m_AosAttributes = new AosAttributes(this);
                            m_AosArmorAttributes = new AosArmorAttributes(this);
                        }

                        if (version < 3 && m_Quality == ArmorQuality.Exceptional)
                            DistributeBonuses(6, 1);

                        if (version >= 2)
                        {
                            m_Resource = (CraftResource)reader.ReadInt();
                        }
                        else
                        {
                            OreInfo info;

                            switch (reader.ReadInt())
                            {
                                default:
                                case 0: info = OreInfo.Iron; break;
                                case 1: info = OreInfo.Copper; break;
                                case 2: info = OreInfo.Bronze; break;
                                case 3: info = OreInfo.Gold; break;
                                case 4: info = OreInfo.Steel; break;
                                case 5: info = OreInfo.Obsidian; break;
                                case 6: info = OreInfo.Tin; break;
                                case 7: info = OreInfo.Starmetal; break;
                                case 8: info = OreInfo.Electrum; break;
                            }

                            m_Resource = CraftResources.GetFromOreInfo(info, mat);
                        }

                        m_HitsMaxBonus = reader.ReadInt();
                        m_StamMaxBonus = reader.ReadInt();
                        m_ManaMaxBonus = reader.ReadInt();

                        m_StrBonus = reader.ReadInt();
                        m_DexBonus = reader.ReadInt();
                        m_IntBonus = reader.ReadInt();
                        m_StrReq = reader.ReadInt();
                        m_DexReq = reader.ReadInt();
                        m_IntReq = reader.ReadInt();

                        if (m_HitsMaxBonus == OldHitsMaxBonus)
                            m_HitsMaxBonus = -1;

                        if (m_StamMaxBonus == OldStamMaxBonus)
                            m_StamMaxBonus = -1;

                        if (m_ManaMaxBonus == OldManaMaxBonus)
                            m_ManaMaxBonus = -1;

                        if (m_StrBonus == OldStrBonus)
                            m_StrBonus = -1;

                        if (m_DexBonus == OldDexBonus)
                            m_DexBonus = -1;

                        if (m_IntBonus == OldIntBonus)
                            m_IntBonus = -1;

                        if (m_StrReq == OldStrReq)
                            m_StrReq = -1;

                        if (m_DexReq == OldDexReq)
                            m_DexReq = -1;

                        if (m_IntReq == OldIntReq)
                            m_IntReq = -1;

                        m_Meditate = (AMA)reader.ReadInt();

                        if (m_Meditate == OldMedAllowance)
                            m_Meditate = (AMA)(-1);

                        if (m_Resource == CraftResource.None)
                        {
                            if (mat == ArmorMaterialType.Studded || mat == ArmorMaterialType.Leather)
                                m_Resource = CraftResource.RegularLeather;
                            else if (mat == ArmorMaterialType.Thick)
                                m_Resource = CraftResource.ThickLeather;
                            else if (mat == ArmorMaterialType.Beast)
                                m_Resource = CraftResource.BeastLeather;
                            else if (mat == ArmorMaterialType.Scaled)
                                m_Resource = CraftResource.ScaledLeather;
                            else
                                m_Resource = CraftResource.Iron;
                        }

                        if (m_MaxHitPoints == 0 && m_HitPoints == 0)
                            m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

                        break;
                    }
            }

            if (m_AosSkillBonuses == null)
                m_AosSkillBonuses = new AosSkillBonuses(this);

            if (Core.AOS && Parent is Mobile)
                m_AosSkillBonuses.AddTo((Mobile)Parent);

            int hitsBonus = ComputeStatBonus(StatType.HitsMax);
            int stamBonus = ComputeStatBonus(StatType.StamMax);
            int manaBonus = ComputeStatBonus(StatType.ManaMax);
            int strBonus = ComputeStatBonus(StatType.Str);
            int dexBonus = ComputeStatBonus(StatType.Dex);
            int intBonus = ComputeStatBonus(StatType.Int);

            if (Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsBonus != 0 || stamBonus != 0 || manaBonus != 0))
            {
                Mobile m = (Mobile)Parent;

                string modName = Serial.ToString();

                if (hitsBonus != 0)
                    m.AddStatMod(new StatMod(StatType.HitsMax, modName + "Hits", hitsBonus, TimeSpan.Zero));

                if (stamBonus != 0)
                    m.AddStatMod(new StatMod(StatType.StamMax, modName + "Stam", stamBonus, TimeSpan.Zero));

                if (manaBonus != 0)
                    m.AddStatMod(new StatMod(StatType.ManaMax, modName + "Mana", manaBonus, TimeSpan.Zero));

                if (strBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            if (Parent is Mobile)
                ((Mobile)Parent).CheckStatTimers();

            if (version < 7)
                m_PlayerConstructed = true; // we don't know, so, assume it's crafted

            if (version < 11 && this.RootParentEntity != null && this.RootParentEntity is Mercenary)
            {
                Mercenary merc = this.RootParentEntity as Mercenary;

                if (this.ArmourType == ArmourWeight.Light)
                    merc.RawDex -= 1;

                else if (this.ArmourType == ArmourWeight.Medium)
                    merc.RawDex -= 2;

                else
                    merc.RawDex -= 3;
            }
        }

        public virtual CraftResource DefaultResource { get { return CraftResource.Iron; } }

        public BaseArmor(int itemID)
            : base(itemID)
        {
            m_Quality = ArmorQuality.Regular;
            m_Durability = ArmorDurabilityLevel.Regular;
            m_Crafter = null;

            m_Resource = DefaultResource;
            Hue = CraftResources.GetHue(m_Resource);

            m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

            this.Layer = (Layer)ItemData.Quality;

            m_AosAttributes = new AosAttributes(this);
            m_AosArmorAttributes = new AosArmorAttributes(this);
            m_AosSkillBonuses = new AosSkillBonuses(this);

            if ((this is WoodenShield || this is LeatherShield || this is BoiledLeatherShield) && this.Resource == CraftResource.Iron)
                this.Resource = CraftResource.Oak;

        //    else if (this.Resource == CraftResource.Iron)
         //   {
         //       this.Resource = CraftResource.Copper;
         //       this.Hue = 0x96D;
         //   }

            if (this is IBoneArmour)
            {
                this.Hue = 0;
            }
        }

        public override bool AllowSecureTrade(Mobile from, Mobile to, Mobile newOwner, bool accepted)
        {
            if (!Ethics.Ethic.CheckTrade(from, to, newOwner, this))
                return false;

            return base.AllowSecureTrade(from, to, newOwner, accepted);
        }

        public virtual Race RequiredRace { get { return null; } }

        public override bool CanEquip(Mobile from)
        {
            PlayerMobile m = from as PlayerMobile;
           // BaseArmor armor = (BaseArmor)item;

            if (from is PlayerMobile)
            {
            	


                    if (m.Feats.GetFeatLevel(FeatList.Invocation) > 1)
                    {
                    	if (this.Resource == CraftResource.Electrum )
                	 {

                            m.SendMessage("The metal burns against your skin, preventing you from wearing it.");

                            return false;
                	 }
                    }
         	
                switch (this.ArmourType)
                {
                    case ArmourWeight.Light:
                        {
                            if (m.Feats.GetFeatLevel(FeatList.LightArmour) == 0)
                            {
                                from.SendMessage(60, "You do not know how to fight in this armour.");
                                return false;
                            }
                            break;
                        }

                    case ArmourWeight.Medium:
                        {
                            if (m.Feats.GetFeatLevel(FeatList.MediumArmour) == 0)
                            {
                                from.SendMessage(60, "You do not know how to fight in this armour.");
                                return false;
                            }
                            break;
                        }

                    case ArmourWeight.Heavy:
                        {
                            if (m.Feats.GetFeatLevel(FeatList.HeavyArmour) == 0)
                            {
                                from.SendMessage(60, "You do not know how to fight in this armour.");
                                return false;
                            }
                            break;
                        }
                }

                if (from.RawDex - GetArmourPenalties(from, this) < 1)
                {
                    from.SendMessage(60, "That would lower your dexterity below 1.");
                    return false;
                }

                if (from.StamMax - GetArmourPenalties(from, this) < 1)
                {
                    from.SendMessage(60, "That would lower your maximum stamina below 1.");
                    return false;
                }
            }

            if (!Ethics.Ethic.CheckEquip(from, this))
                return false;

            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                if (RequiredRace != null && from.Race != RequiredRace)
                {
                    if (RequiredRace == Race.Elf)
                        from.SendLocalizedMessage(1072203); // Only Elves may use this.
                    else
                        from.SendMessage("Only {0} may use this.", RequiredRace.PluralName);

                    return false;
                }
                else if (!AllowMaleWearer && !from.Female)
                {
                    if (AllowFemaleWearer)
                        from.SendLocalizedMessage(1010388); // Only females can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                else if (!AllowFemaleWearer && from.Female)
                {
                    if (AllowMaleWearer)
                        from.SendLocalizedMessage(1063343); // Only males can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                else
                {
                    /*                   int hitsmaxBonus = ComputeStatBonus( StatType.HitsMax ), strReq = ComputeStatReq( StatType.HitsMax );
                                       int stammaxBonus = ComputeStatBonus( StatType.StamMax ), dexReq = ComputeStatReq( StatType.StamMax );
                                       int manamaxBonus = ComputeStatBonus( StatType.ManaMax ), intReq = ComputeStatReq( StatType.ManaMax );
                                       */
                    int strBonus = ComputeStatBonus(StatType.Str), strReq = ComputeStatReq(StatType.Str);
                    int dexBonus = ComputeStatBonus(StatType.Dex), dexReq = ComputeStatReq(StatType.Dex);
                    int intBonus = ComputeStatBonus(StatType.Int), intReq = ComputeStatReq(StatType.Int);

                    if (from.Dex < dexReq || (from.Dex + dexBonus) < 1)
                    {
                        from.SendLocalizedMessage(502077); // You do not have enough dexterity to equip this item.
                        return false;
                    }
                    else if (from.Str < strReq || (from.Str + strBonus) < 1)
                    {
                        from.SendLocalizedMessage(500213); // You are not strong enough to equip that.
                        return false;
                    }
                    else if (from.Int < intReq || (from.Int + intBonus) < 1)
                    {
                        from.SendMessage("You are not smart enough to equip that.");
                        return false;
                    }
                }
            }

            return base.CanEquip(from);
        }

        public override bool CheckPropertyConfliction(Mobile m)
        {
            if (base.CheckPropertyConfliction(m))
                return true;

            if (Layer == Layer.Pants)
                return (m.FindItemOnLayer(Layer.InnerLegs) != null);

            if (Layer == Layer.Shirt)
                return (m.FindItemOnLayer(Layer.InnerTorso) != null);

            return false;
        }

        public static int GetArmourPenalties(Mobile from, BaseArmor armor)
        {
            PlayerMobile m = from as PlayerMobile;
            int weight = 0;
            int armourfocus = 0;

            if (from is PlayerMobile)
            {
                switch (armor.ArmourType)
                {
                    case ArmourWeight.Light:
                        {
                            weight = 4 - m.Feats.GetFeatLevel(FeatList.LightArmour);
                            armourfocus = 1;
                            break;
                        }

                    case ArmourWeight.Medium:
                        {
                            weight = 5 - m.Feats.GetFeatLevel(FeatList.MediumArmour);
                            armourfocus = 2;
                            break;
                        }

                    case ArmourWeight.Heavy:
                        {
                            weight = 6 - m.Feats.GetFeatLevel(FeatList.HeavyArmour);
                            armourfocus = 3;
                            break;
                        }
                }

                if ((m.Feats.GetFeatLevel(FeatList.ArmourFocus) - armourfocus) >= 0)
                {
                    weight--;
                }
            }

            return weight;
        }

        public override bool OnEquip(Mobile from)
        {
            from.CheckStatTimers();

            if (from is Mercenary)
            {
                if (this.ArmourType == ArmourWeight.Light)
                    from.RawDex -= 1;

                else if (this.ArmourType == ArmourWeight.Medium)
                    from.RawDex -= 2;

                else
                    from.RawDex -= 3;
            }

            if (from is PlayerMobile)
            {
                this.Attributes.BonusStam = GetArmourPenalties(from, this);

                from.RawDex -= this.Attributes.BonusStam;
                from.RawStam -= this.Attributes.BonusStam;

                PlayerMobile pm = from as PlayerMobile;

                if (this.ArmourType == ArmourWeight.Light)
                    pm.LightPenalty += this.Attributes.BonusStam;
                else if (this.ArmourType == ArmourWeight.Medium)
                    pm.MediumPenalty += this.Attributes.BonusStam;
                else
                    pm.HeavyPenalty += this.Attributes.BonusStam;

                pm.ArmourPieces++;

                if (this.Attributes.BonusStam < 1)
                    pm.LightPieces++;

                if (this is BaseShield)
                {
                    if (pm.HealingTimer != null)
                    {
                        pm.SendMessage("You have stopped your attempt to heal someone.");
                        pm.HealingTimer.Stop();
                        pm.HealingTimer = null;
                    }
                }
            }

            int hitsBonus = ComputeStatBonus(StatType.HitsMax);
            int stamBonus = ComputeStatBonus(StatType.StamMax);
            int manaBonus = ComputeStatBonus(StatType.ManaMax);
            int strBonus = ComputeStatBonus(StatType.Str);
            int dexBonus = ComputeStatBonus(StatType.Dex);
            int intBonus = ComputeStatBonus(StatType.Int);

            if (from is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsBonus != 0 || stamBonus != 0 || manaBonus != 0))
            {
                string modName = this.Serial.ToString();

                if (hitsBonus != 0)
                    from.AddStatMod(new StatMod(StatType.HitsMax, modName + "Hits", hitsBonus, TimeSpan.Zero));

                if (stamBonus != 0)
                    from.AddStatMod(new StatMod(StatType.StamMax, modName + "Stam", stamBonus, TimeSpan.Zero));

                if (manaBonus != 0)
                    from.AddStatMod(new StatMod(StatType.ManaMax, modName + "Mana", manaBonus, TimeSpan.Zero));

                if (strBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;

                string modName = this.Serial.ToString();

                m.RemoveStatMod(modName + "Str");
                m.RemoveStatMod(modName + "Dex");
                m.RemoveStatMod(modName + "Int");
                m.RemoveStatMod(modName + "Hits");
                m.RemoveStatMod(modName + "Stam");
                m.RemoveStatMod(modName + "Mana");

                if (Core.AOS)
                    m_AosSkillBonuses.Remove();

                ((Mobile)parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
                m.CheckStatTimers();
            }

            base.OnRemoved(parent);

            if (parent is Mercenary)
            {
                Mercenary from = parent as Mercenary;

                if (this.ArmourType == ArmourWeight.Light)
                    from.RawDex += 1;

                else if (this.ArmourType == ArmourWeight.Medium)
                    from.RawDex += 2;

                else
                    from.RawDex += 3;
            }

            if (parent is PlayerMobile)
            {
                PlayerMobile m = parent as PlayerMobile;

                m.RawDex += this.Attributes.BonusStam;
                m.RawStam += this.Attributes.BonusStam;

                if (this.ArmourType == ArmourWeight.Light)
                    m.LightPenalty -= this.Attributes.BonusStam;
                else if (this.ArmourType == ArmourWeight.Medium)
                    m.MediumPenalty -= this.Attributes.BonusStam;
                else
                    m.HeavyPenalty -= this.Attributes.BonusStam;

                m.ArmourPieces--;

                if (this.Attributes.BonusStam < 1)
                    m.LightPieces--;
            }

            if (parent is PlayerMobile)
            {
                PlayerMobile pm = parent as PlayerMobile;

                if (pm.HasGump(typeof(CharInfoGump)) && pm.m_CharInfoTimer == null)
                {
                    pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer(pm);
                    pm.m_CharInfoTimer.Start();
                }
            }

            if (parent is Mercenary)
            {
                BaseCreature bc = parent as BaseCreature;
                int offset = 1;

                if (this.ArmourType == ArmourWeight.Medium)
                    offset = 2;

                else if (this.ArmourType == ArmourWeight.Heavy)
                    offset = 3;

                //bc.VirtualArmor -= offset;
                bc.BluntResistSeed -= offset;
                bc.SlashingResistSeed -= offset;
                bc.PiercingResistSeed -= offset;
            }
        }

        public virtual int OnHit(BaseWeapon weapon, int damageTaken)
        {
            double HalfAr = ArmorRating / 2.0;
            int Absorbed = (int)(HalfAr + HalfAr * Utility.RandomDouble());

            damageTaken -= Absorbed;

            if (damageTaken < 0)
                damageTaken = 0;

            if (Absorbed < 2)
                Absorbed = 2;

            if (Core.AOS && m_AosArmorAttributes.SelfRepair > Utility.Random(10))
            {
                HitPoints += 2;
            }
            else
            {
                if (Utility.RandomBool() && damageTaken > 0)
                    DegradeArmor(damageTaken);
            }

            return damageTaken;
        }

        public void DegradeArmor(int damage)
        {
            if (this.Parent is BaseCreature && !(this.Parent is Mercenary) && Utility.RandomMinMax(1, 100) < 90)
                return;

            int damageChanceIncrease = this.MaxHitPoints - this.HitPoints;

            if (Utility.RandomMinMax(1, 100) > (Utility.RandomMinMax(90, 95) - damageChanceIncrease))
            {
                switch (this.Quality)
                {
                    case ArmorQuality.Poor: //this.HitPoints -= Utility.Random(11); break;
                    case ArmorQuality.Low: /*this.HitPoints -= Utility.Random(10);*/ this.HitPoints -= Utility.Random(6); break;
                    case ArmorQuality.Inferior: //this.HitPoints -= Utility.Random(9); break;
                    case ArmorQuality.Regular: this.HitPoints -= Utility.Random(5); break; //this.HitPoints -= Utility.Random(8); break;
                    case ArmorQuality.Superior: //this.HitPoints -= Utility.Random(7); break;
                    case ArmorQuality.Exceptional: this.HitPoints -= Utility.Random(4); break; //this.HitPoints -= Utility.Random(6); break;
                    case ArmorQuality.Remarkable: //this.HitPoints -= Utility.Random(5); break;
                    case ArmorQuality.Extraordinary: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(3); break; //this.HitPoints -= Utility.Random(4); break;
                    case ArmorQuality.Antique: //if (Utility.RandomBool()) this.HitPoints -= Utility.Random(3); break;
                    case ArmorQuality.Masterwork: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;
                    case ArmorQuality.Legendary: if (Utility.RandomBool()) this.HitPoints--; break;
                }

                if (Utility.RandomMinMax(1, 100) > (Utility.RandomMinMax(90, 95) - damageChanceIncrease))
                {
                    switch (this.Resource)
                    {
                        case CraftResource.Copper: this.HitPoints -= Utility.Random(5); break;
                        case CraftResource.Bronze: this.HitPoints -= Utility.Random(4); break;
                        case CraftResource.Iron: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Steel: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Obsidian: this.HitPoints -= Utility.Random(6); break;
                        case CraftResource.Starmetal: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Silver: this.HitPoints -= Utility.Random(5); break;
                        case CraftResource.Gold: this.HitPoints -= Utility.Random(6); break;
                        case CraftResource.Electrum: this.HitPoints -= Utility.Random(3); break;

                        case CraftResource.Oak: this.HitPoints -= Utility.Random(4); break;
                        case CraftResource.Redwood: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Yew: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Ash: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Greenheart: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;

                        case CraftResource.RegularLeather: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.ThickLeather: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.BeastLeather: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.ScaledLeather: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Cotton: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Wool: this.HitPoints -= Utility.Random(4); break;
                        case CraftResource.Linen: this.HitPoints -= Utility.Random(5); break;
                    }
                }

                int SevereDamageChance = 1;

                if (Parent is PlayerMobile)
                {
                    PlayerMobile pm = Parent as PlayerMobile;

                    SevereDamageChance += (this.MaxHitPoints - this.HitPoints);

                    switch (ArmourType)
                    {
                        case ArmourWeight.Light: SevereDamageChance -= Utility.Random(10); break;
                        case ArmourWeight.Medium: SevereDamageChance -= Utility.Random(20); break;
                        case ArmourWeight.Heavy: SevereDamageChance -= Utility.Random(30); break;
                    }

                    if (pm.GetBackgroundLevel(BackgroundList.Unlucky) > 0)
                        SevereDamageChance *= Utility.RandomMinMax(1, 3);

                    if (pm.GetBackgroundLevel(BackgroundList.Lucky) > 0)
                        SevereDamageChance = SevereDamageChance / Utility.RandomMinMax(1, 3);
                }

                if ((damage + SevereDamageChance >= 100) && (Utility.RandomMinMax(1, 100) >= 99))
                {
                    int severeDamage = Utility.RandomMinMax(1, Utility.RandomMinMax(1, damage));

                    if (Parent is Mobile)
                        ((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                }

                if (this.HitPoints < 0)
                {
                    int itemDmg = 0;

                    switch (ArmourType)
                    {
                        case ArmourWeight.Light: itemDmg += Utility.Random(8); break;
                        case ArmourWeight.Medium: itemDmg += Utility.Random(5); break;
                        case ArmourWeight.Heavy: itemDmg += Utility.Random(2); break;
                    }

                    switch (this.Quality)
                    {
                        case ArmorQuality.Low: itemDmg += Utility.Random(10); break;
                        case ArmorQuality.Regular: itemDmg += Utility.Random(8); break;
                        case ArmorQuality.Exceptional: itemDmg += Utility.Random(6); break;
                        case ArmorQuality.Extraordinary: itemDmg += Utility.Random(4); break;
                        case ArmorQuality.Masterwork: itemDmg += Utility.Random(2); break;
                    }

                    switch (this.Resource)
                    {
                        case CraftResource.Copper: itemDmg += Utility.Random(10); break;
                        case CraftResource.Bronze: itemDmg += Utility.Random(8); break;
                        case CraftResource.Iron: itemDmg += Utility.Random(6); break;
                        case CraftResource.Steel: if (Utility.RandomBool()) itemDmg += Utility.Random(4); break;
                        case CraftResource.Obsidian: itemDmg += Utility.Random(20); break;
                        case CraftResource.Starmetal: if (Utility.RandomBool()) itemDmg += Utility.Random(2); break;
                        case CraftResource.Silver: itemDmg += Utility.Random(10); break;
                        case CraftResource.Gold: itemDmg += Utility.Random(12); break;
                        case CraftResource.Electrum: itemDmg += Utility.Random(6); break;

                        case CraftResource.Oak: itemDmg += Utility.Random(8); break;
                        case CraftResource.Redwood: itemDmg += Utility.Random(6); break;
                        case CraftResource.Yew: itemDmg += Utility.Random(2); break;
                        case CraftResource.Ash: itemDmg += Utility.Random(4); break;
                        case CraftResource.Greenheart: if (Utility.RandomBool()) itemDmg += Utility.Random(2); break;

                        case CraftResource.RegularLeather: itemDmg += Utility.Random(5); break;
                        case CraftResource.ThickLeather: itemDmg += Utility.Random(4); break;
                        case CraftResource.BeastLeather: itemDmg += Utility.Random(3); break;
                        case CraftResource.ScaledLeather: itemDmg += Utility.Random(2); break;
                        case CraftResource.Cotton: itemDmg += Utility.Random(2); break;
                        case CraftResource.Wool: itemDmg += Utility.Random(3); break;
                        case CraftResource.Linen: itemDmg += Utility.Random(4); break;
                    }

                    this.MaxHitPoints -= Utility.RandomMinMax(1, itemDmg);
                    this.HitPoints = 0;

                    if (this.MaxHitPoints < 1)
                        this.Delete();
                }
            }
        }

        private string GetNameString()
        {
            string name = this.Name;

            if (name == null)
                name = String.Format("#{0}", LabelNumber);

            return name;
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get { return base.Hue; }
            set { base.Hue = value; InvalidateProperties(); }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            /*if ( m_Quality == ArmorQuality.Exceptional )
            {
                if ( oreType != 0 )
                    list.Add( 1053100, "#{0}\t{1}", oreType, GetNameString() ); // exceptional ~1_oretype~ ~2_armortype~
                else
                    list.Add( 1050040, GetNameString() ); // exceptional ~1_ITEMNAME~
            }*/
            //else
            //{
            //if ( oreType != 0 )
            /*		list.Add( 1053099, "#{0}\t{1}", oreType, GetNameString() ); // ~1_oretype~ ~2_armortype~
                else*/
            if (Name == null)
                list.Add(LabelNumber);
            else
                list.Add(Name);
            //}
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            if (base.AllowEquipedCast(from))
                return true;

            return (m_AosAttributes.SpellChanneling != 0);
        }

        public virtual int GetLuckBonus()
        {
            CraftResourceInfo resInfo = CraftResources.GetInfo(m_Resource);

            if (resInfo == null)
                return 0;

            CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

            if (attrInfo == null)
                return 0;

            return attrInfo.ArmorLuck;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int oreType;

            switch (m_Resource)
            {
                case CraftResource.Copper: oreType = 1053106; break; // copper
                case CraftResource.Bronze: oreType = 1053105; break; // bronze
                case CraftResource.Iron: oreType = 1062226; break; // iron
                case CraftResource.Gold: oreType = 1053104; break; // golden
                case CraftResource.Silver: oreType = 1053107; break; // agapite
                case CraftResource.Obsidian: oreType = 1053103; break; // verite
                case CraftResource.Steel: oreType = 1053102; break; // valorite
                case CraftResource.Tin: oreType = 1053101; break; // valorite
                case CraftResource.Starmetal: oreType = 1053108; break; // valorite
                case CraftResource.Electrum: oreType = 1053110; break; // uhhhh idk
                case CraftResource.RegularLeather: oreType = 1062235; break; // leather  
                case CraftResource.ThickLeather: oreType = 1061116; break; // Thick  
                case CraftResource.BeastLeather: oreType = 1061117; break; // Beast
                case CraftResource.ScaledLeather: oreType = 1061118; break; // Scaled
                case CraftResource.RedScales: oreType = 1060814; break; // red
                case CraftResource.YellowScales: oreType = 1060818; break; // yellow
                case CraftResource.BlackScales: oreType = 1060820; break; // black
                case CraftResource.GreenScales: oreType = 1060819; break; // green
                case CraftResource.WhiteScales: oreType = 1060821; break; // white
                case CraftResource.BlueScales: oreType = 1060815; break; // blue

                case CraftResource.Oak: oreType = 1063511; break; // yellow
                case CraftResource.Yew: oreType = 1063512; break; // black
                case CraftResource.Redwood: oreType = 1063513; break; // green
                case CraftResource.Ash: oreType = 1063514; break; // white
                case CraftResource.Greenheart: oreType = 1063515; break; // blue
                default: oreType = 0; break;
            }

            if (this is IBoneArmour)
                oreType = 1023966;

            list.Add(oreType);

            if (m_CraftersOriginalName != null)
                list.Add(1050043, m_CraftersOriginalName); // crafted by ~1_NAME~

            #region Factions
            if (m_FactionState != null)
                list.Add(1041350); // faction item
            #endregion

            if (RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only

            if (m_Quality == ArmorQuality.Poor)
                list.Add(1060659, "Quality\tPoor");

            if (m_Quality == ArmorQuality.Low)
                list.Add(1060659, "Quality\tLow");

            if (m_Quality == ArmorQuality.Inferior)
                list.Add(1060659, "Quality\tInferior");

            if (m_Quality == ArmorQuality.Superior)
                list.Add(1060659, "Quality\tSuperior");

            if (m_Quality == ArmorQuality.Exceptional)
                list.Add(1060659, "Quality\tExceptional");

            if (m_Quality == ArmorQuality.Remarkable)
                list.Add(1060659, "Quality\tRemarkable");

            if (m_Quality == ArmorQuality.Extraordinary)
                list.Add(1060659, "Quality\tExtraordinary");

            if (m_Quality == ArmorQuality.Antique)
                list.Add(1060659, "Quality\tExceptional");

            if (m_Quality == ArmorQuality.Masterwork)
                list.Add(1060659, "Quality\tMasterwork");

            if (m_Quality == ArmorQuality.Legendary)
                list.Add(1060659, "Quality\tLegendary");

            m_AosSkillBonuses.GetProperties(list);

            int prop;

            if ((prop = ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            if ((prop = m_AosAttributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

            if ((prop = m_AosAttributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = m_AosAttributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = m_AosAttributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = m_AosAttributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = m_AosAttributes.CastSpeed) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = m_AosAttributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = m_AosAttributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = m_AosAttributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = m_AosAttributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = m_AosAttributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

            if ((prop = GetLowerStatReq()) != 0)
                list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%

            if ((prop = (GetLuckBonus() + m_AosAttributes.Luck)) != 0)
                list.Add(1060436, prop.ToString()); // luck ~1_val~

            if ((prop = m_AosArmorAttributes.MageArmor) != 0)
                list.Add(1060437); // mage armor

            if ((prop = m_AosAttributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = m_AosAttributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

            if ((prop = m_AosAttributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = m_AosAttributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if ((prop = m_AosAttributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = m_AosAttributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = m_AosArmorAttributes.SelfRepair) != 0)
                list.Add(1060450, prop.ToString()); // self repair ~1_val~

            if ((prop = m_AosAttributes.SpellChanneling) != 0)
                list.Add(1060482); // spell channeling

            if ((prop = m_AosAttributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            //			if ( (prop = m_AosAttributes.BonusStam) != 0 )
            //				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

            if ((prop = m_AosAttributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = m_AosAttributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

            //base.AddResistanceProperties( list );		



            int v = PhysicalResistance;

            if (v != 0)
                list.Add(1060448, v.ToString()); // physical resist ~1_val~%

            v = FireResistance;

            if (v != 0)
                list.Add(1060447, v.ToString()); // fire resist ~1_val~%

            v = ColdResistance;

            if (v != 0)
                list.Add(1060445, v.ToString()); // cold resist ~1_val~%

            v = PoisonResistance;

            if (v != 0)
                list.Add(1060449, v.ToString()); // poison resist ~1_val~%

            v = EnergyResistance;

            if (v != 0)
                list.Add(1060446, v.ToString()); // energy resist ~1_val~%

            v = BluntResistance;

            if (v != 0)
                list.Add(1060526, v.ToString()); // cold resist ~1_val~%

            v = SlashingResistance;

            if (v != 0)
                list.Add(1060527, v.ToString()); // cold resist ~1_val~%

            v = PiercingResistance;

            if (v != 0)
                list.Add(1060528, v.ToString()); // cold resist ~1_val~%



            if ((prop = GetDurabilityBonus()) > 0)
                list.Add(1060410, prop.ToString()); // durability ~1_val~%

            if ((prop = ComputeStatReq(StatType.Str)) > 0)
                list.Add(1061170, prop.ToString()); // strength requirement ~1_val~

            if (m_HitPoints >= 0 && m_MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints); // durability ~1_val~ / ~2_val~

            switch (ArmourType)
            {
                case ArmourWeight.Light: list.Add("Armour Type: Light", ArmourType); break;
                case ArmourWeight.Medium: list.Add("Armour Type: Medium", ArmourType); break;
                case ArmourWeight.Heavy: list.Add("Armour Type: Heavy", ArmourType); break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {

        }

        public override void OnSingleClick(Mobile from)
        {
            List<EquipInfoAttribute> attrs = new List<EquipInfoAttribute>();

            if (DisplayLootType)
            {
                if (LootType == LootType.Blessed)
                    attrs.Add(new EquipInfoAttribute(1038021)); // blessed
                else if (LootType == LootType.Cursed)
                    attrs.Add(new EquipInfoAttribute(1049643)); // cursed
            }

            #region Factions
            if (m_FactionState != null)
                attrs.Add(new EquipInfoAttribute(1041350)); // faction item
            #endregion

            //if ( m_Quality == ArmorQuality.Exceptional )
            //attrs.Add( new EquipInfoAttribute( 1018305 - (int)m_Quality ) );

            if (m_Identified || from.AccessLevel >= AccessLevel.GameMaster)
            {
                if (m_Durability != ArmorDurabilityLevel.Regular)
                    attrs.Add(new EquipInfoAttribute(1038000 + (int)m_Durability));

                if (m_Protection > ArmorProtectionLevel.Regular && m_Protection <= ArmorProtectionLevel.Invulnerability)
                    attrs.Add(new EquipInfoAttribute(1038005 + (int)m_Protection));
            }
            else if (m_Durability != ArmorDurabilityLevel.Regular || (m_Protection > ArmorProtectionLevel.Regular && m_Protection <= ArmorProtectionLevel.Invulnerability))
                attrs.Add(new EquipInfoAttribute(1038000)); // Unidentified

            int number;

            if (Name == null)
            {
                number = LabelNumber;
            }
            else
            {
                this.LabelTo(from, Name);
                number = 1041000;
            }

            if (attrs.Count == 0 && Crafter == null && Name != null)
                return;

            EquipmentInfo eqInfo = new EquipmentInfo(number, m_Crafter, false, attrs.ToArray());

            from.Send(new DisplayEquipmentInfo(this, eqInfo));
        }
        #region ICraftable Members

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Quality = (ArmorQuality)quality;

            PlayerMobile m = from as PlayerMobile;

            Crafter = from;

            if (makersMark)
            {
                CraftersOriginalName = from.Name;
            }

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Ressources.GetAt(0).ItemType;

            Resource = CraftResources.GetFromType(resourceType);
            PlayerConstructed = true;
            m_OldArmor = false;

            if (Resource == CraftResource.Obsidian)
            {
                from.SendMessage(60, "You wasted some of your obsidian trying to craft armour out of it.");
                this.Delete();
            }

            if (Resource == CraftResource.Steel && from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Steel) < 3)
            {
                from.SendMessage(60, "You do not know how to use steel and have ruined your craft.");
                this.Delete();
            }

            if (Resource == CraftResource.Greenheart && from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Greenheart) < 3)
            {
                from.SendMessage(60, "You do not know how to use greenheart and have ruined your craft.");
                this.Delete();
            }

            if ((this is BoneChest || this is BoneArms || this is BoneLegs || this is BoneGloves
               || this is BoneHelm || this is MhordulHornedBoneHelm || this is MhordulHornedSkullHelm)
               && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Bone) < 3)
            {
                from.SendMessage(60, "You do not know how to use bone and have ruined your craft.");
                this.Delete();
            }

            if (this is IBoneArmour)
                this.Resource = CraftResource.RegularLeather;

            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                Hue = 0;

            if (this is IBoneArmour)
                this.Hue = 0;

            bool masterwork = false;

            if (quality == 5)
            {
                if (from is PlayerMobile)
                {
                    bool MWpieces = false;
                    bool EOpieces = false;

                    if (m.Masterwork.HasArmourPieces && Resource == m.Masterwork.ArmourResource)
                    {
                        if (m.Masterwork.MasterworkArmour)
                            MWpieces = true;

                        else
                            EOpieces = true;

                        m.Masterwork.HasArmourPieces = false;
                    }

                    if (m.Feats.GetFeatLevel(FeatList.Masterwork) > 2 && !EOpieces)
                    {
                        double chance = 0.002 + GetCraftingSpecBonus(m);
                        double roll = Utility.RandomDouble();

                        if (m.AccessLevel > AccessLevel.Player)
                            m.SendMessage("Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + ".");

                        if (MWpieces || chance >= roll)
                        {
                            this.Quality = ArmorQuality.Masterwork;
                            MasterworkBonuses(m, 2);
                            m.SendMessage(60, "You have created a masterwork item.");
                            masterwork = true;
                            m.Crafting = true;
                            LevelSystem.AwardExp(m, Math.Min((m.Int * 100), 500));
                            LevelSystem.AwardCP(m, Math.Min((m.Int * 20), 100));
                            m.Crafting = false;

                            if (m.Backpack != null)
                                ((BaseContainer)m.Backpack).DropAndStack(new RewardToken(2));
                        }
                    }

                    if (m.Feats.GetFeatLevel(FeatList.Masterwork) > 0 && !masterwork)
                    {
                        double chance = GetCraftingSpecBonus(m) * 5;
                        double roll = Utility.RandomDouble();

                        if (m.Feats.GetFeatLevel(FeatList.Masterwork) == 1)
                            chance += 0.002;

                        else if (m.Feats.GetFeatLevel(FeatList.Masterwork) == 2)
                            chance += 0.005;

                        else chance += 0.01;

                        if (m.AccessLevel > AccessLevel.Player)
                            m.SendMessage("Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + ".");

                        if (EOpieces || chance >= roll)
                        {
                            this.Quality = ArmorQuality.Extraordinary;
                            MasterworkBonuses(m, 1);
                            m.SendMessage(60, "You have created an extraordinary item.");
                            m.Crafting = true;
                            LevelSystem.AwardExp(m, Math.Min((m.Int * 100), 250));
                            LevelSystem.AwardCP(m, Math.Min((m.Int * 20), 50));
                            m.Crafting = false;

                            if (m.Backpack != null && Utility.RandomBool())
                                ((BaseContainer)m.Backpack).DropAndStack(new RewardToken());
                        }
                    }
                }
            }

            if (Core.AOS && tool is BaseRunicTool)
                ((BaseRunicTool)tool).ApplyAttributesTo(this);

            if (from is PlayerMobile)
            {
                MaxHitPoints += (MaxHitPoints / 10) * (m.Feats.GetFeatLevel(FeatList.DurableCrafts) * 2);
                HitPoints = MaxHitPoints;
            }

            return quality;
        }

        private void MasterworkBonuses(PlayerMobile m, int bonus)
        {
            int offset = 3 - (m.Masterwork.BluntResist + m.Masterwork.SlashingResist + m.Masterwork.PiercingResist);

            if (offset > 0)
                DistributeBonuses(offset, bonus);

            this.BluntBonus += m.Masterwork.BluntResist * bonus;
            this.SlashingBonus += m.Masterwork.SlashingResist * bonus;
            this.PiercingBonus += m.Masterwork.PiercingResist * bonus;
        }

        public double GetCraftingSpecBonus(PlayerMobile m)
        {
            double bonus = 0.0;

            if (m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) > 0 && !String.IsNullOrEmpty(m.CraftingSpecialization))
            {
                string skill = "none";

                if (Resource == CraftResource.Copper || Resource == CraftResource.Iron || Resource == CraftResource.Bronze ||
                   Resource == CraftResource.Silver || Resource == CraftResource.Gold || Resource == CraftResource.Steel ||
                   Resource == CraftResource.Obsidian || Resource == CraftResource.Starmetal || Resource == CraftResource.Electrum )
                    skill = "Blacksmithing";

                else if (Resource == CraftResource.RegularLeather || Resource == CraftResource.ThickLeather || Resource == CraftResource.ScaledLeather ||
                       Resource == CraftResource.BeastLeather)
                    skill = "Tailoring";

                else
                    skill = "Carpentry";

                if (m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 1)
                    bonus = 0.0004;

                if (m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 2)
                    bonus = 0.001;

                else
                    bonus = 0.002;

                if (m.CraftingSpecialization != skill)
                    bonus = -0.001;
            }

            return bonus;
        }

        #endregion
    }
}
