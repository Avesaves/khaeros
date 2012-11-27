using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Factions;
using Server.Mobiles;
using Server.Misc;
using AMA = Server.Items.ArmorMeditationAllowance;
using Server.Gumps;
using Server.Commands;

namespace Server.Items
{
    public class ArmourBackpack : BaseContainer
    {
    	public override int DefaultMaxWeight{ get{ return 1200; } }
    	
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
        private AMA m_Meditate = (AMA)( -1 );

        public virtual bool AllowMaleWearer { get { return true; } }
        public virtual bool AllowFemaleWearer { get { return true; } }

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

        [CommandProperty( AccessLevel.GameMaster )]
        public AMA MeditationAllowance
        {
            get { return ( m_Meditate == (AMA)( -1 ) ? Core.AOS ? AosMedAllowance : OldMedAllowance : m_Meditate ); }
            set { m_Meditate = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int BaseArmorRating
        {
            get
            {
                if( m_ArmorBase == -1 )
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
                return ( BaseArmorRating * ArmorScalar );
            }
        }

        public virtual double ArmorRating
        {
            get
            {
                int ar = BaseArmorRating;

                if( m_Protection != ArmorProtectionLevel.Regular )
                    ar += 10 + ( 5 * (int)m_Protection );

                return ScaleArmorByDurability( ar );
            }
        }

        public double ArmorRatingScaled
        {
            get
            {
                return ( ArmorRating * ArmorScalar );
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int HitsMaxBonus
        {
            get { return ( m_HitsMaxBonus == -1 ? Core.AOS ? AosHitsMaxBonus : OldHitsMaxBonus : m_HitsMaxBonus ); }
            set { m_HitsMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int StamMaxBonus
        {
            get { return ( m_StamMaxBonus == -1 ? Core.AOS ? AosStamMaxBonus : OldStamMaxBonus : m_StamMaxBonus ); }
            set { m_StamMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ManaMaxBonus
        {
            get { return ( m_ManaMaxBonus == -1 ? Core.AOS ? AosManaMaxBonus : OldManaMaxBonus : m_ManaMaxBonus ); }
            set { m_ManaMaxBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int StrBonus
        {
            get { return ( m_StrBonus == -1 ? Core.AOS ? AosStrBonus : OldStrBonus : m_StrBonus ); }
            set { m_StrBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int DexBonus
        {
            get { return ( m_DexBonus == -1 ? Core.AOS ? AosDexBonus : OldDexBonus : m_DexBonus ); }
            set { m_DexBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int IntBonus
        {
            get { return ( m_IntBonus == -1 ? Core.AOS ? AosIntBonus : OldIntBonus : m_IntBonus ); }
            set { m_IntBonus = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int StrRequirement
        {
            get { return ( m_StrReq == -1 ? Core.AOS ? AosStrReq : OldStrReq : m_StrReq ); }
            set { m_StrReq = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int DexRequirement
        {
            get { return ( m_DexReq == -1 ? Core.AOS ? AosDexReq : OldDexReq : m_DexReq ); }
            set { m_DexReq = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int IntRequirement
        {
            get { return ( m_IntReq == -1 ? Core.AOS ? AosIntReq : OldIntReq : m_IntReq ); }
            set { m_IntReq = value; InvalidateProperties(); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Identified
        {
            get { return m_Identified; }
            set { m_Identified = value; InvalidateProperties(); }
        }

        public virtual double ArmorScalar
        {
            get
            {
                return 1.0;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public ArmorProtectionLevel ProtectionLevel
        {
            get
            {
                return m_Protection;
            }
            set
            {
                if( m_Protection != value )
                {
                    m_Protection = value;

                    Invalidate();
                    InvalidateProperties();

                    if( Parent is Mobile )
                        ( (Mobile)Parent ).UpdateResistances();
                }
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public AosAttributes Attributes
        {
            get { return m_AosAttributes; }
            set { }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public AosArmorAttributes ArmorAttributes
        {
            get { return m_AosArmorAttributes; }
            set { }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public AosSkillBonuses SkillBonuses
        {
            get { return m_AosSkillBonuses; }
            set { }
        }

        public int ComputeStatBonus(StatType type)
        {
            if( type == StatType.Str )
                return StrBonus + Attributes.BonusStr;
            else if( type == StatType.Dex )
                return DexBonus + Attributes.BonusDex;
            else if( type == StatType.Int )
                return IntBonus + Attributes.BonusInt;
            else if( type == StatType.HitsMax )
                return HitsMaxBonus + Attributes.BonusHitsMax;
            else if( type == StatType.StamMax )
                return StamMaxBonus + Attributes.BonusStamMax;
            else
                return ManaMaxBonus + Attributes.BonusManaMax;
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int PhysicalBonus { get { return m_PhysicalBonus; } set { m_PhysicalBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int FireBonus { get { return m_FireBonus; } set { m_FireBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ColdBonus { get { return m_ColdBonus; } set { m_ColdBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int PoisonBonus { get { return m_PoisonBonus; } set { m_PoisonBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int EnergyBonus { get { return m_EnergyBonus; } set { m_EnergyBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int BluntBonus { get { return m_BluntBonus; } set { m_BluntBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SlashingBonus { get { return m_SlashingBonus; } set { m_SlashingBonus = value; InvalidateProperties(); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int PiercingBonus { get { return m_PiercingBonus; } set { m_PiercingBonus = value; InvalidateProperties(); } }

        public virtual int BasePhysicalResistance { get { return 0; } }
        public virtual int BaseFireResistance { get { return 0; } }
        public virtual int BaseColdResistance { get { return 0; } }
        public virtual int BasePoisonResistance { get { return 0; } }
        public virtual int BaseEnergyResistance { get { return 0; } }
        public virtual int BaseBluntResistance { get { return 0; } }
        public virtual int BaseSlashingResistance { get { return 0; } }
        public virtual int BasePiercingResistance { get { return 0; } }

        public override int PhysicalResistance { get { return BasePhysicalResistance + GetProtOffset() + m_PhysicalBonus; } }
        public override int FireResistance { get { return BaseFireResistance + GetProtOffset() + m_FireBonus; } }
        public override int ColdResistance { get { return BaseColdResistance + GetProtOffset() + m_ColdBonus; } }
        public override int PoisonResistance { get { return BasePoisonResistance + GetProtOffset() + m_PoisonBonus; } }
        public override int EnergyResistance { get { return BaseEnergyResistance + GetProtOffset() + m_EnergyBonus; } }
        public override int BluntResistance { get { return BaseBluntResistance + GetProtOffset() + m_BluntBonus; } }
        public override int SlashingResistance { get { return BaseSlashingResistance + GetProtOffset() + m_SlashingBonus; } }
        public override int PiercingResistance { get { return BasePiercingResistance + GetProtOffset() + m_PiercingBonus; } }

        public void DistributeBonuses(int amount)
        {
            InvalidateProperties();
        }

        public int GetProtOffset()
        {
            switch( m_Protection )
            {
                case ArmorProtectionLevel.Guarding: return 1;
                case ArmorProtectionLevel.Hardening: return 2;
                case ArmorProtectionLevel.Fortification: return 3;
                case ArmorProtectionLevel.Invulnerability: return 4;
            }

            return 0;
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
            for( int i = m.Items.Count - 1; i >= 0; --i )
            {
                if( i >= m.Items.Count )
                    continue;

                Item item = m.Items[i];

                if( item is BaseArmor )
                {
                    BaseArmor armor = (BaseArmor)item;

                    if( armor.RequiredRace != null && m.Race != armor.RequiredRace )
                    {
                        if( armor.RequiredRace == Race.Elf )
                            m.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
                        else
                            m.SendMessage( "Only {0} may use this.", armor.RequiredRace.PluralName );

                        m.AddToBackpack( armor );
                    }
                    else if( !armor.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster )
                    {
                        if( armor.AllowFemaleWearer )
                            m.SendLocalizedMessage( 1010388 ); // Only females can wear this.
                        else
                            m.SendMessage( "You may not wear this." );

                        m.AddToBackpack( armor );
                    }
                    else if( !armor.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster )
                    {
                        if( armor.AllowMaleWearer )
                            m.SendLocalizedMessage( 1063343 ); // Only males can wear this.
                        else
                            m.SendMessage( "You may not wear this." );

                        m.AddToBackpack( armor );
                    }
                }
            }
        }

        public virtual int GetLuckBonus()
        {
            return Attributes.Luck;
        }

        public override void OnAdded(object parent)
        {
            if( parent is Mobile )
            {
                Mobile from = (Mobile)parent;

                if( Core.AOS )
                    m_AosSkillBonuses.AddTo( from );

                from.Delta( MobileDelta.Armor ); // Tell them armor rating has changed
            }
            
            if( parent is PlayerMobile )
			{
				PlayerMobile pm = parent as PlayerMobile;
				
				if( pm.HasGump( typeof( CharInfoGump ) ) && pm.m_CharInfoTimer == null )
				{
					pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( pm );
					pm.m_CharInfoTimer.Start();
				}
			}
        }

        public virtual double ScaleArmorByDurability(double armor)
        {
            int scale = 100;

            return ( armor * scale ) / 100;
        }

        protected void Invalidate()
        {
            if( Parent is Mobile )
                ( (Mobile)Parent ).Delta( MobileDelta.Armor ); // Tell them armor rating has changed
        }

        public ArmourBackpack( Serial serial ) : base( serial )
		{
		}

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if( setIf )
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ( ( flags & toGet ) != 0 );
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
            Protection = 0x00000100,
            BaseArmor = 0x00000200,
            StrBonus = 0x00000400,
            DexBonus = 0x00000800,
            IntBonus = 0x00001000,
            StrReq = 0x00002000,
            DexReq = 0x00004000,
            IntReq = 0x00008000,
            MedAllowance = 0x00010000,
            SkillBonuses = 0x00020000,
            BluntBonus = 0x00040000,
            SlashingBonus = 0x00080000,
            PiercingBonus = 0x00100000,
            HitsMaxBonus = 0x00200000,
            StamMaxBonus = 0x00400000,
            ManaMaxBonus = 0x00800000
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)12 ); // version

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag( ref flags, SaveFlag.Attributes, !m_AosAttributes.IsEmpty );
            SetSaveFlag( ref flags, SaveFlag.ArmorAttributes, !m_AosArmorAttributes.IsEmpty );
            SetSaveFlag( ref flags, SaveFlag.BluntBonus, m_BluntBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.SlashingBonus, m_SlashingBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.PiercingBonus, m_PiercingBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.PhysicalBonus, m_PhysicalBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.FireBonus, m_FireBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.ColdBonus, m_ColdBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.PoisonBonus, m_PoisonBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.EnergyBonus, m_EnergyBonus != 0 );
            SetSaveFlag( ref flags, SaveFlag.Identified, m_Identified != false );
            SetSaveFlag( ref flags, SaveFlag.Protection, m_Protection != ArmorProtectionLevel.Regular );
            SetSaveFlag( ref flags, SaveFlag.BaseArmor, m_ArmorBase != -1 );

            SetSaveFlag( ref flags, SaveFlag.HitsMaxBonus, m_HitsMaxBonus != -1 );
            SetSaveFlag( ref flags, SaveFlag.StamMaxBonus, m_StamMaxBonus != -1 );
            SetSaveFlag( ref flags, SaveFlag.ManaMaxBonus, m_ManaMaxBonus != -1 );

            SetSaveFlag( ref flags, SaveFlag.StrBonus, m_StrBonus != -1 );
            SetSaveFlag( ref flags, SaveFlag.DexBonus, m_DexBonus != -1 );
            SetSaveFlag( ref flags, SaveFlag.IntBonus, m_IntBonus != -1 );
            SetSaveFlag( ref flags, SaveFlag.StrReq, m_StrReq != -1 );
            SetSaveFlag( ref flags, SaveFlag.DexReq, m_DexReq != -1 );
            SetSaveFlag( ref flags, SaveFlag.IntReq, m_IntReq != -1 );
            SetSaveFlag( ref flags, SaveFlag.MedAllowance, m_Meditate != (AMA)( -1 ) );
            SetSaveFlag( ref flags, SaveFlag.SkillBonuses, !m_AosSkillBonuses.IsEmpty );

            writer.WriteEncodedInt( (int)flags );

            if( GetSaveFlag( flags, SaveFlag.Attributes ) )
                m_AosAttributes.Serialize( writer );

            if( GetSaveFlag( flags, SaveFlag.ArmorAttributes ) )
                m_AosArmorAttributes.Serialize( writer );

            if( GetSaveFlag( flags, SaveFlag.BluntBonus ) )
                writer.WriteEncodedInt( (int)m_BluntBonus );

            if( GetSaveFlag( flags, SaveFlag.SlashingBonus ) )
                writer.WriteEncodedInt( (int)m_SlashingBonus );

            if( GetSaveFlag( flags, SaveFlag.PiercingBonus ) )
                writer.WriteEncodedInt( (int)m_PiercingBonus );

            if( GetSaveFlag( flags, SaveFlag.PhysicalBonus ) )
                writer.WriteEncodedInt( (int)m_PhysicalBonus );

            if( GetSaveFlag( flags, SaveFlag.FireBonus ) )
                writer.WriteEncodedInt( (int)m_FireBonus );

            if( GetSaveFlag( flags, SaveFlag.ColdBonus ) )
                writer.WriteEncodedInt( (int)m_ColdBonus );

            if( GetSaveFlag( flags, SaveFlag.PoisonBonus ) )
                writer.WriteEncodedInt( (int)m_PoisonBonus );

            if( GetSaveFlag( flags, SaveFlag.EnergyBonus ) )
                writer.WriteEncodedInt( (int)m_EnergyBonus );

            if( GetSaveFlag( flags, SaveFlag.Protection ) )
                writer.WriteEncodedInt( (int)m_Protection );

            if( GetSaveFlag( flags, SaveFlag.BaseArmor ) )
                writer.WriteEncodedInt( (int)m_ArmorBase );

            if( GetSaveFlag( flags, SaveFlag.HitsMaxBonus ) )
                writer.WriteEncodedInt( (int)m_HitsMaxBonus );

            if( GetSaveFlag( flags, SaveFlag.StamMaxBonus ) )
                writer.WriteEncodedInt( (int)m_StamMaxBonus );

            if( GetSaveFlag( flags, SaveFlag.ManaMaxBonus ) )
                writer.WriteEncodedInt( (int)m_ManaMaxBonus );

            if( GetSaveFlag( flags, SaveFlag.StrBonus ) )
                writer.WriteEncodedInt( (int)m_StrBonus );

            if( GetSaveFlag( flags, SaveFlag.DexBonus ) )
                writer.WriteEncodedInt( (int)m_DexBonus );

            if( GetSaveFlag( flags, SaveFlag.IntBonus ) )
                writer.WriteEncodedInt( (int)m_IntBonus );

            if( GetSaveFlag( flags, SaveFlag.StrReq ) )
                writer.WriteEncodedInt( (int)m_StrReq );

            if( GetSaveFlag( flags, SaveFlag.DexReq ) )
                writer.WriteEncodedInt( (int)m_DexReq );

            if( GetSaveFlag( flags, SaveFlag.IntReq ) )
                writer.WriteEncodedInt( (int)m_IntReq );

            if( GetSaveFlag( flags, SaveFlag.MedAllowance ) )
                writer.WriteEncodedInt( (int)m_Meditate );

            if( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
                m_AosSkillBonuses.Serialize( writer );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
            	case 12:
            	case 11:
            	case 10:
            	case 9:
                case 8:
                case 7:
                case 6:
                case 5:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.Attributes ) )
                            m_AosAttributes = new AosAttributes( this, reader );
                        else
                            m_AosAttributes = new AosAttributes( this );

                        if( GetSaveFlag( flags, SaveFlag.ArmorAttributes ) )
                            m_AosArmorAttributes = new AosArmorAttributes( this, reader );
                        else
                            m_AosArmorAttributes = new AosArmorAttributes( this );

                        if( GetSaveFlag( flags, SaveFlag.BluntBonus ) )
                            m_BluntBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.SlashingBonus ) )
                            m_SlashingBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.PiercingBonus ) )
                            m_PiercingBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.PhysicalBonus ) )
                            m_PhysicalBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.FireBonus ) )
                            m_FireBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.ColdBonus ) )
                            m_ColdBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.PoisonBonus ) )
                            m_PoisonBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.EnergyBonus ) )
                            m_EnergyBonus = reader.ReadEncodedInt();

                        if( GetSaveFlag( flags, SaveFlag.Identified ) )
                            m_Identified = ( version >= 7 || reader.ReadBool() );

                        if( GetSaveFlag( flags, SaveFlag.Protection ) )
                        {
                            m_Protection = (ArmorProtectionLevel)reader.ReadEncodedInt();

                            if( m_Protection > ArmorProtectionLevel.Invulnerability )
                                m_Protection = ArmorProtectionLevel.Defense;
                        }

                        if( GetSaveFlag( flags, SaveFlag.BaseArmor ) )
                            m_ArmorBase = reader.ReadEncodedInt();
                        else
                            m_ArmorBase = -1;

                        if( GetSaveFlag( flags, SaveFlag.HitsMaxBonus ) )
                            m_HitsMaxBonus = reader.ReadEncodedInt();
                        else
                            m_HitsMaxBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.StamMaxBonus ) )
                            m_StamMaxBonus = reader.ReadEncodedInt();
                        else
                            m_StamMaxBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.ManaMaxBonus ) )
                            m_ManaMaxBonus = reader.ReadEncodedInt();
                        else
                            m_ManaMaxBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.StrBonus ) )
                            m_StrBonus = reader.ReadEncodedInt();
                        else
                            m_StrBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.DexBonus ) )
                            m_DexBonus = reader.ReadEncodedInt();
                        else
                            m_DexBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.IntBonus ) )
                            m_IntBonus = reader.ReadEncodedInt();
                        else
                            m_IntBonus = -1;

                        if( GetSaveFlag( flags, SaveFlag.StrReq ) )
                            m_StrReq = reader.ReadEncodedInt();
                        else
                            m_StrReq = -1;

                        if( GetSaveFlag( flags, SaveFlag.DexReq ) )
                            m_DexReq = reader.ReadEncodedInt();
                        else
                            m_DexReq = -1;

                        if( GetSaveFlag( flags, SaveFlag.IntReq ) )
                            m_IntReq = reader.ReadEncodedInt();
                        else
                            m_IntReq = -1;

                        if( GetSaveFlag( flags, SaveFlag.MedAllowance ) )
                            m_Meditate = (AMA)reader.ReadEncodedInt();
                        else
                            m_Meditate = (AMA)( -1 );

                        if( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
                            m_AosSkillBonuses = new AosSkillBonuses( this, reader );

                        break;
                    }
                case 4:
                    {
                        m_AosAttributes = new AosAttributes( this, reader );
                        m_AosArmorAttributes = new AosArmorAttributes( this, reader );
                        goto case 3;
                    }
                case 3:
                    {
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
                        m_Protection = (ArmorProtectionLevel)reader.ReadInt();

                        if( m_ArmorBase == RevertArmorBase )
                            m_ArmorBase = -1;

                        reader.ReadInt();

                        if( version < 4 )
                        {
                            m_AosAttributes = new AosAttributes( this );
                            m_AosArmorAttributes = new AosArmorAttributes( this );
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

                        if( m_HitsMaxBonus == OldHitsMaxBonus )
                            m_HitsMaxBonus = -1;

                        if( m_StamMaxBonus == OldStamMaxBonus )
                            m_StamMaxBonus = -1;

                        if( m_ManaMaxBonus == OldManaMaxBonus )
                            m_ManaMaxBonus = -1;

                        if( m_StrBonus == OldStrBonus )
                            m_StrBonus = -1;

                        if( m_DexBonus == OldDexBonus )
                            m_DexBonus = -1;

                        if( m_IntBonus == OldIntBonus )
                            m_IntBonus = -1;

                        if( m_StrReq == OldStrReq )
                            m_StrReq = -1;

                        if( m_DexReq == OldDexReq )
                            m_DexReq = -1;

                        if( m_IntReq == OldIntReq )
                            m_IntReq = -1;

                        m_Meditate = (AMA)reader.ReadInt();

                        if( m_Meditate == OldMedAllowance )
                            m_Meditate = (AMA)( -1 );

                        break;
                    }
            }

            if( m_AosSkillBonuses == null )
                m_AosSkillBonuses = new AosSkillBonuses( this );

            if( Core.AOS && Parent is Mobile )
                m_AosSkillBonuses.AddTo( (Mobile)Parent );

            int hitsmaxBonus = ComputeStatBonus( StatType.HitsMax );
            int stammaxBonus = ComputeStatBonus( StatType.StamMax );
            int manamaxBonus = ComputeStatBonus( StatType.ManaMax );

            int strBonus = ComputeStatBonus( StatType.Str );
            int dexBonus = ComputeStatBonus( StatType.Dex );
            int intBonus = ComputeStatBonus( StatType.Int );

            if( Parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsmaxBonus != 0 || stammaxBonus != 0 || manamaxBonus != 0 ) )
            {
                Mobile m = (Mobile)Parent;

                string modName = Serial.ToString();

                if( hitsmaxBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.HitsMax, modName + "HitsMax", hitsmaxBonus, TimeSpan.Zero ) );

                if( stammaxBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.StamMax, modName + "StamMax", stammaxBonus, TimeSpan.Zero ) );

                if( manamaxBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.ManaMax, modName + "ManaMax", manamaxBonus, TimeSpan.Zero ) );

                if( strBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

                if( dexBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

                if( intBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
            }

            if( Parent is Mobile )
                ( (Mobile)Parent ).CheckStatTimers();
            
            if( version < 12 )
			{
            	if( this.ParentEntity != null && this.ParentEntity is PlayerMobile )
            	{
            		PlayerMobile pm = this.ParentEntity as PlayerMobile;
					Attributes.WeaponSpeed = 0;
					Attributes.WeaponDamage = 0;
					Attributes.AttackChance = 0;
					Attributes.DefendChance = 0;
	            }
			}
        }

        public virtual CraftResource DefaultResource { get { return CraftResource.Iron; } }

        [Constructable]
		public ArmourBackpack() : base( 0xE75 )
		{
			Layer = Layer.Backpack;
			Weight = 3.0;

            m_AosAttributes = new AosAttributes( this );
            m_AosArmorAttributes = new AosArmorAttributes( this );
            m_AosSkillBonuses = new AosSkillBonuses( this );
        }

        public override bool AllowSecureTrade(Mobile from, Mobile to, Mobile newOwner, bool accepted)
        {
            if( !Ethics.Ethic.CheckTrade( from, to, newOwner, this ) )
                return false;

            return base.AllowSecureTrade( from, to, newOwner, accepted );
        }

        public virtual Race RequiredRace { get { return null; } }

        public override bool CanEquip(Mobile from)
        {
            return base.CanEquip( from );
        }

        public override bool OnEquip(Mobile from)
        {
            from.CheckStatTimers();

            int hitsmaxBonus = ComputeStatBonus( StatType.HitsMax );
            int stammaxBonus = ComputeStatBonus( StatType.StamMax );
            int manamaxBonus = ComputeStatBonus( StatType.ManaMax );

            int strBonus = ComputeStatBonus( StatType.Str );
            int dexBonus = ComputeStatBonus( StatType.Dex );
            int intBonus = ComputeStatBonus( StatType.Int );

            if( strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsmaxBonus != 0 || stammaxBonus != 0 || manamaxBonus != 0 )
            {
                string modName = this.Serial.ToString();

                if( hitsmaxBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.HitsMax, modName + "HitsMax", hitsmaxBonus, TimeSpan.Zero ) );

                if( stammaxBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.StamMax, modName + "StamMax", stammaxBonus, TimeSpan.Zero ) );

                if( manamaxBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.ManaMax, modName + "ManaMax", manamaxBonus, TimeSpan.Zero ) );

                if( strBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

                if( dexBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

                if( intBonus != 0 )
                    from.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
            }

            return base.OnEquip( from );
        }

        public override void OnRemoved(object parent)
        {
            if( parent is Mobile )
            {
                Mobile m = (Mobile)parent;

                string modName = this.Serial.ToString();

                m.RemoveStatMod( modName + "HitsMax" );
                m.RemoveStatMod( modName + "StamMax" );
                m.RemoveStatMod( modName + "ManaMax" );

                m.RemoveStatMod( modName + "Str" );
                m.RemoveStatMod( modName + "Dex" );
                m.RemoveStatMod( modName + "Int" );

                if( Core.AOS )
                    m_AosSkillBonuses.Remove();

                ( (Mobile)parent ).Delta( MobileDelta.Armor ); // Tell them armor rating has changed
                m.CheckStatTimers();
            }

            base.OnRemoved( parent );
            
            if( parent is PlayerMobile )
			{
				PlayerMobile pm = parent as PlayerMobile;
				
				if( pm.HasGump( typeof( CharInfoGump ) ) && pm.m_CharInfoTimer == null )
				{
					pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( pm );
					pm.m_CharInfoTimer.Start();
				}
			}
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            if( base.AllowEquipedCast( from ) )
                return true;

            return ( m_AosAttributes.SpellChanneling != 0 );
        }
    }
}
