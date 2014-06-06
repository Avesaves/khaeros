using System;
using System.Text;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Bushido;
using Server.Spells.ExoticWeaponry;
using Server.Factions;
using Server.Engines.Craft;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;
using Server.Gumps;
using Server.Engines.XmlSpawner2;
using Server.Misc;

namespace Server.Items
{
	public interface ISlayer
	{
		SlayerName Slayer { get; set; }
		SlayerName Slayer2 { get; set; }
	}

	public abstract class BaseWeapon : Item, IWeapon, IFactionItem, ICraftable, ISlayer, IDurability
	{
        private Item featitem1;
		
		#region Factions
		private FactionItem m_FactionState;

		public FactionItem FactionItemState
		{
			get{ return m_FactionState; }
			set
			{
				m_FactionState = value;

				if ( m_FactionState == null )
					Hue = CraftResources.GetHue( Resource );

				LootType = ( m_FactionState == null ? LootType.Regular : LootType.Blessed );
			}
		}
		#endregion

		/* Weapon internals work differently now (Mar 13 2003)
		 * 
		 * The attributes defined below default to -1.
		 * If the value is -1, the corresponding virtual 'Aos/Old' property is used.
		 * If not, the attribute value itself is used. Here's the list:
		 *  - MinDamage
		 *  - MaxDamage
		 *  - Speed
		 *  - HitSound
		 *  - MissSound
		 *  - StrRequirement, DexRequirement, IntRequirement
		 *  - WeaponType
		 *  - WeaponAnimation
		 *  - MaxRange
		 */

		#region Var declarations

		// Instance values. These values are unique to each weapon.

		private WeaponDamageLevel m_DamageLevel;
		private WeaponAccuracyLevel m_AccuracyLevel;
		private WeaponDurabilityLevel m_DurabilityLevel;
		private WeaponQuality m_Quality;
		private Mobile m_Crafter;
		private Poison m_Poison;
		private int m_PoisonCharges;
		private bool m_Identified;
		private int m_Hits;
		private int m_MaxHits;
		private SlayerName m_Slayer;
		private SlayerName m_Slayer2;
		private SkillMod m_SkillMod, m_MageMod;
		private CraftResource m_Resource;
		private bool m_PlayerConstructed;

		private bool m_Cursed; // Is this weapon cursed via Curse Weapon necromancer spell? Temporary; not serialized.
		private bool m_Consecrated; // Is this weapon blessed via Consecrate Weapon paladin ability? Temporary; not serialized.

		private AosAttributes m_AosAttributes;
		private AosWeaponAttributes m_AosWeaponAttributes;
		private AosSkillBonuses m_AosSkillBonuses;
		private AosElementAttributes m_AosElementDamages;

		// Overridable values. These values are provided to override the defaults which get defined in the individual weapon scripts.
		private int m_StrReq, m_DexReq, m_IntReq;
		private int m_MinDamage, m_MaxDamage;
		private int m_HitSound, m_MissSound;
		private double m_Speed;
		private int m_MaxRange;
		private SkillName m_Skill;
		private WeaponType m_Type;
		private WeaponAnimation m_Animation;
		public Mobile ThrowMob;
		
		private bool m_HasHalo;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasHalo{ get{ return m_HasHalo; } set{ m_HasHalo = value; } }
		
		#endregion

		#region Virtual Properties
		public virtual WeaponAbility PrimaryAbility{ get{ return null; } }
		public virtual WeaponAbility SecondaryAbility{ get{ return null; } }
		
		public virtual double DamagePenalty{ get{ return 0.0; } }
		
		public virtual double RangedPercentage{ get{ return 0.0; } }
		public virtual double OverheadPercentage{ get{ return 0.0; } }
		public virtual double SwingPercentage{ get{ return 0.0; } }
		public virtual double ThrustPercentage{ get{ return 0.0; } }
		
		public virtual bool Critical{ get{ return false; } }
		public virtual bool CannotBlock{ get{ return false; } }
		public virtual bool CannotUseOnMount{ get{ return false; } }
		public virtual bool CannotUseOnFoot{ get{ return false; } }
		public virtual bool ChargeOnly{ get{ return false; } }
		public virtual bool Unwieldy{ get{ return false; } }
		public virtual bool CanThrustOnMount{ get{ return false; } }
		public virtual bool CanUseDefensiveFormation{ get{ return false; } }
		
		public virtual int SheathedMaleWaistID{ get{ return -1; } }
		public virtual int SheathedFemaleWaistID{ get{ return -1; } }
		public virtual int SheathedMaleBackID{ get{ return -1; } }
		public virtual int SheathedFemaleBackID{ get{ return -1; } }
		
		public virtual bool BastardWeapon{ get{ return false; } }
		public virtual bool ImprovisedWeapon{ get{ return false; } }

		public virtual int DefMaxRange{ get{ return 1; } }
		public virtual int DefHitSound{ get{ return 0; } }
		public virtual int DefMissSound{ get{ return 0; } }
		public virtual SkillName DefSkill{ get{ return SkillName.Swords; } }
		public virtual WeaponType DefType{ get{ return WeaponType.Slashing; } }
		public virtual WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

		public virtual int AosStrengthReq{ get{ return 0; } }
		public virtual int AosDexterityReq{ get{ return 0; } }
		public virtual int AosIntelligenceReq{ get{ return 0; } }
		public virtual int AosMinDamage{ get{ return 0; } }
		public virtual int AosMaxDamage{ get{ return 0; } }
		public virtual double AosSpeed{ get{ return 0.0; } }
		public virtual int AosMaxRange{ get{ return DefMaxRange; } }
		public virtual int AosHitSound{ get{ return DefHitSound; } }
		public virtual int AosMissSound{ get{ return DefMissSound; } }
		public virtual SkillName AosSkill{ get{ return DefSkill; } }
		public virtual WeaponType AosType{ get{ return DefType; } }
		public virtual WeaponAnimation AosAnimation{ get{ return DefAnimation; } }

		public virtual int OldStrengthReq{ get{ return 0; } }
		public virtual int OldDexterityReq{ get{ return 0; } }
		public virtual int OldIntelligenceReq{ get{ return 0; } }
		public virtual int OldMinDamage{ get{ return 0; } }
		public virtual int OldMaxDamage{ get{ return 0; } }
		public virtual int OldSpeed{ get{ return 0; } }
		public virtual int OldMaxRange{ get{ return DefMaxRange; } }
		public virtual int OldHitSound{ get{ return DefHitSound; } }
		public virtual int OldMissSound{ get{ return DefMissSound; } }
		public virtual SkillName OldSkill{ get{ return DefSkill; } }
		public virtual WeaponType OldType{ get{ return DefType; } }
		public virtual WeaponAnimation OldAnimation{ get{ return DefAnimation; } }

		public virtual int InitMinHits{ get{ return 0; } }
		public virtual int InitMaxHits{ get{ return 0; } }

        public virtual bool Throwable { get { return false; } }

		public override int PhysicalResistance{ get{ return m_AosWeaponAttributes.ResistPhysicalBonus; } }
		public override int FireResistance{ get{ return m_AosWeaponAttributes.ResistFireBonus; } }
		public override int ColdResistance{ get{ return m_AosWeaponAttributes.ResistColdBonus; } }
		public override int PoisonResistance{ get{ return m_AosWeaponAttributes.ResistPoisonBonus; } }
		public override int EnergyResistance{ get{ return m_AosWeaponAttributes.ResistEnergyBonus; } }
        public override int BluntResistance { get { return m_AosWeaponAttributes.ResistBluntBonus; } }
        public override int SlashingResistance { get { return m_AosWeaponAttributes.ResistSlashingBonus; } }
        public override int PiercingResistance { get { return m_AosWeaponAttributes.ResistPiercingBonus; } }

		public virtual SkillName AccuracySkill { get { return SkillName.Tactics; } }
		
		private string m_Engraved1;
		private string m_Engraved2;
		private string m_Engraved3;
		private string m_CraftersOriginalName;
		
		private int m_QualityDamage;
		private int m_QualitySpeed;
		private int m_QualityAccuracy;

        private bool m_NewCrafting = false;
        private int m_QualityDefense;
        private List<Item> m_Components;

        private bool m_BetaNerf = false;
		
		#endregion

		#region Getters & Setters

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BetaNerf 
        {
            get { return m_BetaNerf; }
            set { m_BetaNerf = value; } 
        }        
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string Engraved1
		{
			get{ return m_Engraved1; }
			set{ m_Engraved1 = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string Engraved2
		{
			get{ return m_Engraved2; }
			set{ m_Engraved2 = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string Engraved3
		{
			get{ return m_Engraved3; }
			set{ m_Engraved3 = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string CraftersOriginalName
		{
			get{ return m_CraftersOriginalName; }
			set{ m_CraftersOriginalName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosWeaponAttributes WeaponAttributes
		{
			get{ return m_AosWeaponAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosSkillBonuses SkillBonuses
		{
			get{ return m_AosSkillBonuses; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes AosElementDamages
		{
			get { return m_AosElementDamages; }
			set { }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Cursed
		{
			get{ return m_Cursed; }
			set{ m_Cursed = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Consecrated
		{
			get
			{ 
				XmlConsecrateWeapon consecrate = XmlAttach.FindAttachment( this, typeof(XmlConsecrateWeapon) ) as XmlConsecrateWeapon;
	            
	            if( consecrate != null )
	            	return true;
	            
				return m_Consecrated;
			}
			
			set{ m_Consecrated = value; }
		}
		
		public int HolyWaterPower
		{
			get
			{
				XmlHolyWater hwatt = XmlAttach.FindAttachment( this, typeof(XmlHolyWater) ) as XmlHolyWater;
	            
				if( hwatt != null )
					return hwatt.Value;
				
				return 0;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Identified
		{
			get{ return m_Identified; }
			set{ m_Identified = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitPoints
		{
			get{ return m_Hits; }
			set
			{
				if ( m_Hits == value )
					return;

				if ( value > m_MaxHits )
					value = m_MaxHits;

				m_Hits = value;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get{ return m_MaxHits; }
			set{ m_MaxHits = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonCharges
		{
			get{ return m_PoisonCharges; }
			set{ m_PoisonCharges = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get{ return m_Poison; }
			set{ m_Poison = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponQuality Quality
		{
			get{ return m_Quality; }
			set{ UnscaleDurability(); m_Quality = value; ScaleDurability(); InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer
		{
			get{ return m_Slayer; }
			set{ m_Slayer = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer2
		{
			get { return m_Slayer2; }
			set { m_Slayer2 = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set
			{ 
				UnscaleDurability();  
				m_Resource = value; 
				
				if( this is IBoneArmour )
					Hue = 0;
				
				else
					Hue = CraftResources.GetHue( m_Resource );
				
				InvalidateProperties();
				ScaleDurability(); 
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponDamageLevel DamageLevel
		{
			get{ return m_DamageLevel; }
			set{ m_DamageLevel = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponDurabilityLevel DurabilityLevel
		{
			get{ return m_DurabilityLevel; }
			set{ UnscaleDurability(); m_DurabilityLevel = value; InvalidateProperties(); ScaleDurability(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PlayerConstructed
		{
			get{ return m_PlayerConstructed; }
			set{ m_PlayerConstructed = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int MaxRange
		{
            get{
                int rangebonus = 0;

                if( this.RootParentEntity != null && this.RootParentEntity is PlayerMobile && this is BaseRanged )
                {
                    try
                    {
                        rangebonus = ( (PlayerMobile)RootParentEntity ).Feats.GetFeatLevel(FeatList.FarShot) * 2;
                    }

                    catch
                    {
                    }
                }

                return ( m_MaxRange == -1 ? Core.AOS ? AosMaxRange + rangebonus : OldMaxRange + rangebonus : m_MaxRange + rangebonus );
            }
			set{ m_MaxRange = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponAnimation Animation
		{
			get{ return ( m_Animation == (WeaponAnimation)(-1) ? Core.AOS ? AosAnimation : OldAnimation : m_Animation ); } 
			set{ m_Animation = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponType Type
		{
			get{ return ( m_Type == (WeaponType)(-1) ? Core.AOS ? AosType : OldType : m_Type ); }
			set{ m_Type = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SkillName Skill
		{
			get{ return ( m_Skill == (SkillName)(-1) ? Core.AOS ? AosSkill : OldSkill : m_Skill ); }
			set{ m_Skill = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitSound
		{
			get{ return ( m_HitSound == -1 ? Core.AOS ? AosHitSound : OldHitSound : m_HitSound ); }
			set{ m_HitSound = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MissSound
		{
			get{ return ( m_MissSound == -1 ? Core.AOS ? AosMissSound : OldMissSound : m_MissSound ); }
			set{ m_MissSound = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MinDamage
		{
			get{ return ( m_MinDamage == -1 ? Core.AOS ? AosMinDamage : OldMinDamage : m_MinDamage ); }
			set{ m_MinDamage = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxDamage
		{
			get{ return ( m_MaxDamage == -1 ? Core.AOS ? AosMaxDamage : OldMaxDamage : m_MaxDamage ); }
			set{ m_MaxDamage = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double Speed
		{
			get{ return ( m_Speed == -1 ? Core.AOS ? AosSpeed : OldSpeed : m_Speed ); }
			set{ m_Speed = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int StrRequirement
		{
			get{ return ( m_StrReq == -1 ? Core.AOS ? AosStrengthReq : OldStrengthReq : m_StrReq ); }
			set{ m_StrReq = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int DexRequirement
		{
			get{ return ( m_DexReq == -1 ? Core.AOS ? AosDexterityReq : OldDexterityReq : m_DexReq ); }
			set{ m_DexReq = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int IntRequirement
		{
			get{ return ( m_IntReq == -1 ? Core.AOS ? AosIntelligenceReq : OldIntelligenceReq : m_IntReq ); }
			set{ m_IntReq = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponAccuracyLevel AccuracyLevel
		{
			get
			{
				return m_AccuracyLevel;
			}
			set
			{
				if ( m_AccuracyLevel != value )
				{
					m_AccuracyLevel = value;

					if ( UseSkillMod )
					{
						if ( m_AccuracyLevel == WeaponAccuracyLevel.Regular )
						{
							if ( m_SkillMod != null )
								m_SkillMod.Remove();

							m_SkillMod = null;
						}
						else if ( m_SkillMod == null && Parent is Mobile )
						{
							m_SkillMod = new DefaultSkillMod( AccuracySkill, true, (int)m_AccuracyLevel * 5 );
							((Mobile)Parent).AddSkillMod( m_SkillMod );
						}
						else if ( m_SkillMod != null )
						{
							m_SkillMod.Value = (int)m_AccuracyLevel * 5;
						}
					}

					InvalidateProperties();
				}
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int QualityDamage{ get{ return m_QualityDamage; } set{ m_QualityDamage = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int QualitySpeed{ get{ return m_QualitySpeed; } set{ m_QualitySpeed = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int QualityAccuracy{ get{ return m_QualityAccuracy; } set{ m_QualityAccuracy = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NewCrafting
        {
            get { return m_NewCrafting; }
            set { m_NewCrafting = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int QualityDefense
        {
            get { return m_QualityDefense; }
            set { m_QualityDefense = value; InvalidateProperties(); }
        }

        public List<Item> Components
        {
            get { return m_Components; }
            set { m_Components = value; }
        }

		#endregion

        public override void AddItem( Item item )
        {           
			if (item.Parent is Item)
				((Item)item.Parent).RemoveItem(item);
			else if (item.Parent is Mobile)
				((Mobile)item.Parent).RemoveItem(item);
			else
				item.SendRemovePacket();
				
            m_Components.Add(item);
            item.OnAdded(this);
            OnItemAdded(item);

            //base.AddItem(item);
        }

        public virtual string NameType { get { return "Name Not Found - please report this bug"; } }
      
        #region Backwards Compatibility Fixes
        private int GetOldResourceDamage()
    	{
        	switch( this.Resource )
        	{
        		case CraftResource.Bronze: return 2;
        		case CraftResource.Yew: return 2;
        		case CraftResource.Redwood: return 4;
        		case CraftResource.Iron: return 4;
        		case CraftResource.Steel: return 6;
        		case CraftResource.Ash: return 6;
        		case CraftResource.Obsidian: return 8;
        		case CraftResource.Greenheart: return 8;
        		case CraftResource.Starmetal: return 10;
        		case CraftResource.Electrum: return 2;
        		default: return (this is IBoneArmour) == true ? 2 : 0;
        	}
    	}
        
        private int GetOldResourceSpeed()
    	{
        	switch( this.Resource )
        	{
        		case CraftResource.Steel: return 6;
        		case CraftResource.Obsidian: return 8;
        		case CraftResource.Greenheart: return 4;
        		case CraftResource.Starmetal: return 10;
        		case CraftResource.Electrum: return 5;
        		default: return (this is IBoneArmour) == true ? 6 : 0;
        	}
    	}
        
        public virtual void FixResource()
        {
        	int oldDamage = GetOldResourceDamage();
        	int oldSpeed = GetOldResourceSpeed();
        	
        	this.Attributes.WeaponDamage -= oldDamage;
        	this.Attributes.WeaponSpeed -= oldSpeed;
        	
        	if( this.Quality > WeaponQuality.Exceptional )
        		this.Attributes.WeaponDamage -= 4;
        }
        
        #endregion

        public static int GetRacialMountAbility(Mobile rider, Type mountTypeCheck)
        {
            if (rider == null || rider.Deleted || !rider.Alive)
                return 0;

            if (rider.Mount == null)
                return 0;

            if (rider.Mount is BaseMount)
            {
                if ((rider.Mount as BaseMount).GetType() == mountTypeCheck)
                {
                    return ((rider.Mount as BaseMount).Level / (6 - (rider.Mount as BaseMount).XPScale));
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public static int GetRacialMountFeatBonus(Mobile r, Type mountTypeCheck)
        {
            if (r == null || r.Deleted || !r.Alive)
                return 0;

            if (!(r is PlayerMobile))
                return 0;

            PlayerMobile rider = r as PlayerMobile;

            bool HasCorrectMount = ( rider.Mount != null && rider.Mount.GetType() == mountTypeCheck);

            if (mountTypeCheck == typeof(BarbHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.HorseArcher) * (HasCorrectMount ? 3 : -0);
            }
            else if (mountTypeCheck == typeof(KudaHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.KudaRider) * (HasCorrectMount ? 3 : -0);
            }
            else if (mountTypeCheck == typeof(RoseanHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.Clibanarii) * (HasCorrectMount ? 5 : -0);
            }
            else if (mountTypeCheck == typeof(GallowayHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.Skirmisher) * (HasCorrectMount ? 5 : -0);
            }
            else if (mountTypeCheck == typeof(SteppeHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.SteppeRaider) * (HasCorrectMount ? 3 : -0);
            }
            else if (mountTypeCheck == typeof(RuganHorse))
            {
                return rider.Feats.GetFeatLevel(FeatList.HeavyCavalry) * (HasCorrectMount ? 3 : -0);
            }
            else
                return 0;
        }
        
        private int GetResourceDamage()
    	{
        	if( this is IBoneArmour )
        		return 2;
        	
        	switch( this.Resource )
        	{
        		case CraftResource.Copper: return 0;
        		case CraftResource.Bronze: return 3;
        		case CraftResource.Iron: return 3;
        		case CraftResource.Steel: return 4;
        		case CraftResource.Obsidian: return 3;
        		case CraftResource.Starmetal: return 5;
        		case CraftResource.Electrum: return 0;
        		case CraftResource.Oak: return 0;
        		case CraftResource.Yew: return 4;
        		case CraftResource.Redwood: return 2;
        		case CraftResource.Ash: return 4;
        		case CraftResource.Greenheart: return 3;
        	}
        	
        	return 0;
    	}
        
        private int GetResourceSpeed()
    	{
        	if( this is IBoneArmour )
        		return 4;
        	
        	switch( this.Resource )
        	{
        		case CraftResource.Copper: return 0;
        		case CraftResource.Bronze: return 2;
        		case CraftResource.Iron: return 0;
        		case CraftResource.Steel: return 2;
        		case CraftResource.Obsidian: return 4;
        		case CraftResource.Starmetal: return 5;
        		case CraftResource.Electrum: return 3;
        		case CraftResource.Oak: return 3;
        		case CraftResource.Yew: return 0;
        		case CraftResource.Redwood: return 3;
        		case CraftResource.Ash: return 2;
        		case CraftResource.Greenheart: return 4;
        	}
        	
        	return 0;
    	}
        
        private int GetQualityDamage()
    	{
        	switch( this.Quality )
        	{
        		case WeaponQuality.Low: return 0;
        		case WeaponQuality.Regular: return 0;
        		case WeaponQuality.Exceptional: return 2;
        		case WeaponQuality.Extraordinary: return ( 3 + (this.QualityDamage * 2) );
        		case WeaponQuality.Masterwork: return ( 4 + (this.QualityDamage * 4) );
        		default: return 0;
        	}
    	}
        
        private int GetQualitySpeed()
    	{
        	switch( this.Quality )
        	{
        		case WeaponQuality.Low: return 0;
        		case WeaponQuality.Regular: return 0;
        		case WeaponQuality.Exceptional: return 2;
        		case WeaponQuality.Extraordinary: return ( 3 + (this.QualitySpeed * 2) );
        		case WeaponQuality.Masterwork: return ( 4 + (this.QualitySpeed * 4) );
        		default: return 0;
        	}
    	}

		public virtual void UnscaleDurability()
		{
			int scale = 100 + GetDurabilityBonus();

			m_Hits = ((m_Hits * 100) + (scale - 1)) / scale;
			m_MaxHits = ((m_MaxHits * 100) + (scale - 1)) / scale;
			InvalidateProperties();
		}

		public virtual void ScaleDurability()
		{
			int scale = 100 + GetDurabilityBonus();

			m_Hits = ((m_Hits * scale) + 99) / 100;
			m_MaxHits = ((m_MaxHits * scale) + 99) / 100;
			InvalidateProperties();
		}

		public int GetDurabilityBonus()
		{
			int bonus = 0;

			if ( m_Quality == WeaponQuality.Exceptional )
				bonus += 25;
			
			if ( m_Quality == WeaponQuality.Extraordinary )
				bonus += 50;
			
			if ( m_Quality == WeaponQuality.Masterwork )
				bonus += 75;

			switch ( m_DurabilityLevel )
			{
				case WeaponDurabilityLevel.Durable: bonus += 20; break;
				case WeaponDurabilityLevel.Substantial: bonus += 50; break;
				case WeaponDurabilityLevel.Massive: bonus += 70; break;
				case WeaponDurabilityLevel.Fortified: bonus += 100; break;
				case WeaponDurabilityLevel.Indestructible: bonus += 120; break;
			}

			if ( Core.AOS )
			{
				bonus += m_AosWeaponAttributes.DurabilityBonus;

				CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );
				CraftAttributeInfo attrInfo = null;

				if ( resInfo != null )
					attrInfo = resInfo.AttributeInfo;

				if ( attrInfo != null )
					bonus += attrInfo.WeaponDurability;
			}

			return bonus;
		}

		public int GetLowerStatReq()
		{
			if ( !Core.AOS )
				return 0;

			int v = m_AosWeaponAttributes.LowerStatReq;

			CraftResourceInfo info = CraftResources.GetInfo( m_Resource );

			if ( info != null )
			{
				CraftAttributeInfo attrInfo = info.AttributeInfo;

				if ( attrInfo != null )
					v += attrInfo.WeaponLowerRequirements;
			}

			if ( v > 100 )
				v = 100;

			return v;
		}

		public static void BlockEquip( Mobile m, TimeSpan duration )
		{
			if ( m.BeginAction( typeof( BaseWeapon ) ) )
				new ResetEquipTimer( m, duration ).Start();
		}

		private class ResetEquipTimer : Timer
		{
			private Mobile m_Mobile;

			public ResetEquipTimer( Mobile m, TimeSpan duration ) : base( duration )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.EndAction( typeof( BaseWeapon ) );
			}
		}

		public override bool CheckConflictingLayer( Mobile m, Item item, Layer layer )
		{
			if ( base.CheckConflictingLayer( m, item, layer ) )
				return true;

			if ( this.Layer == Layer.TwoHanded && layer == Layer.OneHanded )
			{
				m.SendLocalizedMessage( 500214 ); // You already have something in both hands.
				return true;
			}
			else if ( this.Layer == Layer.OneHanded && layer == Layer.TwoHanded && item is BaseWeapon )
			{
				m.SendLocalizedMessage( 500215 ); // You can only wield one weapon at a time.
				return true;
			}

			return false;
		}

		public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if ( !Ethics.Ethic.CheckTrade( from, to, newOwner, this ) )
				return false;

			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}

		public virtual Race RequiredRace { get { return null; } }	//On OSI, there are no weapons with race requirements, this is for custom stuff

		public override bool CanEquip( Mobile from )
		{
			if ( !Ethics.Ethic.CheckEquip( from, this ) )
				return false;

			if( RequiredRace != null && from.Race != RequiredRace )
			{
				if( RequiredRace == Race.Elf )
					from.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
				else
					from.SendMessage( "Only {0} may use this.", RequiredRace.PluralName );

				return false;
			}
			else if ( from.Dex < DexRequirement )
			{
				from.SendMessage( "You are not nimble enough to equip that." );
				return false;
			} 
			else if ( from.Str < AOS.Scale( StrRequirement, 100 - GetLowerStatReq() ) )
			{
				from.SendLocalizedMessage( 500213 ); // You are not strong enough to equip that.
				return false;
			}
			else if ( from.Int < IntRequirement )
			{
				from.SendMessage( "You are not smart enough to equip that." );
				return false;
			}
			else if ( !from.CanBeginAction( typeof( BaseWeapon ) ) )
			{
				return false;
			}

			else
			{
				return base.CanEquip( from );
			}
		}

		public virtual bool UseSkillMod{ get{ return !Core.AOS; } }

		public override bool OnEquip( Mobile from )
		{
			int strBonus = m_AosAttributes.BonusStr;
			int dexBonus = m_AosAttributes.BonusDex;
			int intBonus = m_AosAttributes.BonusInt;

			if ( (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
			{
				Mobile m = from;

				string modName = this.Serial.ToString();

				if ( strBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

				if ( dexBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

				if ( intBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}

			from.NextCombatTime = DateTime.Now + GetDelay( from );

			if ( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular )
			{
				if ( m_SkillMod != null )
					m_SkillMod.Remove();

				m_SkillMod = new DefaultSkillMod( AccuracySkill, true, (int)m_AccuracyLevel * 5 );
				from.AddSkillMod( m_SkillMod );
			}

			if ( Core.AOS && m_AosWeaponAttributes.MageWeapon != 0 && m_AosWeaponAttributes.MageWeapon != 30 )
			{
				if ( m_MageMod != null )
					m_MageMod.Remove();

				m_MageMod = new DefaultSkillMod( SkillName.Magery, true, -30 + m_AosWeaponAttributes.MageWeapon );
				from.AddSkillMod( m_MageMod );
			}
			
			if( from is PlayerMobile )
			{
				PlayerMobile attacker = from as PlayerMobile;
				
				if( attacker.HealingTimer != null )
                {
                	attacker.SendMessage( "You have stopped your attempt to heal someone." );
                	attacker.HealingTimer.Stop();
                	attacker.HealingTimer = null;
                }
			}

			return true;
		}
		
		public void AddHalo( Mobile m )
		{
			LightSource newlight = new LightSource();
			newlight.Layer = Layer.ShopBuy;
			newlight.Light = LightType.Circle300;
			newlight.Name = "Weapon Halo";
			
			Item light = m.FindItemOnLayer( Layer.Talisman );
	
			if( light != null && light is LightSource )
				light.Delete();

            light = m.FindItemOnLayer( Layer.ShopBuy );

            if( light != null && light is LightSource )
                light.Delete();

            if( m.FindItemOnLayer( Layer.ShopBuy ) == null )
				m.EquipItem( newlight );
		}
		
		public void RemoveHalo( Mobile m )
		{
			Item light = m.FindItemOnLayer( Layer.Talisman );
			
			if( light != null && light is LightSource && light.Name == "Weapon Halo" )
				light.Delete();

            light = m.FindItemOnLayer( Layer.ShopBuy );

            if( light != null && light is LightSource && light.Name == "Weapon Halo" )
                light.Delete();
		}

        public int ComputeStatBonus( StatType type )
        {
            if( type == StatType.Str )
                return Attributes.BonusStr;
            else if( type == StatType.Dex )
                return Attributes.BonusDex;
            else if( type == StatType.Int )
                return Attributes.BonusInt;
            else if( type == StatType.HitsMax )
                return Attributes.BonusHitsMax;
            else if( type == StatType.StamMax )
                return Attributes.BonusStamMax;
            else
                return Attributes.BonusManaMax;
        }

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );
			
			if ( parent is Mobile )
			{
				Mobile from = (Mobile)parent;

				if ( Core.AOS )
					m_AosSkillBonuses.AddTo( from );

				from.CheckStatTimers();
				from.Delta( MobileDelta.WeaponDamage );
				
				if( HasHalo )
					AddHalo( from );

                int hitsBonus = ComputeStatBonus( StatType.HitsMax );
                int stamBonus = ComputeStatBonus( StatType.StamMax );
                int manaBonus = ComputeStatBonus( StatType.ManaMax );
                int strBonus = ComputeStatBonus( StatType.Str );
                int dexBonus = ComputeStatBonus( StatType.Dex );
                int intBonus = ComputeStatBonus( StatType.Int );

                if( parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsBonus != 0 || stamBonus != 0 || manaBonus != 0 ) )
                {
                    Mobile m = (Mobile)Parent;

                    string modName = Serial.ToString();

                    if( hitsBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.HitsMax, modName + "Hits", hitsBonus, TimeSpan.Zero ) );

                    if( stamBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.StamMax, modName + "Stam", stamBonus, TimeSpan.Zero ) );

                    if( manaBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.ManaMax, modName + "Mana", manaBonus, TimeSpan.Zero ) );

                    if( strBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

                    if( dexBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

                    if( intBonus != 0 )
                        m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
                }
			}
			
			if( parent is Mercenary )
			{
				Mercenary m_Mobile = parent as Mercenary;
				
				m_Mobile.MercWeapon( AosElementDamages.Blunt, AosElementDamages.Slashing, AosElementDamages.Piercing, MinDamage, MaxDamage );
				
				if( this is BaseRanged && m_Mobile.AI == AIType.AI_Melee )
				{
					m_Mobile.AI = AIType.AI_Archer;
				}
				
				//Chaning to AI_Melee if we added a melee weapon.
				else if( this is BaseMeleeWeapon && m_Mobile.AI == AIType.AI_Archer )
				{
					m_Mobile.AI = AIType.AI_Melee;
				}
			}
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile m = (Mobile)parent;
				BaseWeapon weapon = m.Weapon as BaseWeapon;

				string modName = this.Serial.ToString();

				m.RemoveStatMod( modName + "Str" );
				m.RemoveStatMod( modName + "Dex" );
				m.RemoveStatMod( modName + "Int" );
                m.RemoveStatMod( modName + "Hits" );
                m.RemoveStatMod( modName + "Stam" );
                m.RemoveStatMod( modName + "Mana" );

				if ( weapon != null )
					m.NextCombatTime = DateTime.Now + weapon.GetDelay( m );

				if ( UseSkillMod && m_SkillMod != null )
				{
					m_SkillMod.Remove();
					m_SkillMod = null;
				}

				if ( m_MageMod != null )
				{
					m_MageMod.Remove();
					m_MageMod = null;
				}

				if ( Core.AOS )
					m_AosSkillBonuses.Remove();

				m.CheckStatTimers();

				m.Delta( MobileDelta.WeaponDamage );
				
				if( HasHalo )
					RemoveHalo( m );
				
				if( parent is Mercenary )
				{
					( (Mercenary)parent ).MercWeapon( 100, 0, 0, 3, 4 );
				}
			}
		}

		public virtual SkillName GetUsedSkill( Mobile m, bool checkSkillAttrs )
		{
			SkillName sk;

			if ( checkSkillAttrs && m_AosWeaponAttributes.UseBestSkill != 0 )
			{
				double swrd = m.Skills[SkillName.Swords].Value;
				double fenc = m.Skills[SkillName.Fencing].Value;
				double mcng = m.Skills[SkillName.Macing].Value;
				double val;

				sk = SkillName.Swords;
				val = swrd;

				if ( fenc > val ){ sk = SkillName.Fencing; val = fenc; }
				if ( mcng > val ){ sk = SkillName.Macing; val = mcng; }
			}
			else if ( m_AosWeaponAttributes.MageWeapon != 0 )
			{
				if ( m.Skills[SkillName.Magery].Value > m.Skills[Skill].Value )
					sk = SkillName.Magery;
				else
					sk = Skill;
			}
			else
			{
				sk = Skill;

				if ( sk != SkillName.UnarmedFighting && !m.Player && !m.Body.IsHuman && m.Skills[SkillName.UnarmedFighting].Value > m.Skills[sk].Value )
					sk = SkillName.UnarmedFighting;
			}

			return sk;
		}

		public virtual double GetAttackSkillValue( Mobile attacker, Mobile defender )
		{
			return attacker.Skills[GetUsedSkill( attacker, true )].Value;
		}

		public virtual double GetDefendSkillValue( Mobile attacker, Mobile defender )
		{
			return defender.Skills[GetUsedSkill( defender, true )].Value;
		}

		private static bool CheckAnimal( Mobile m, Type type )
		{
			return AnimalForm.UnderTransformation( m, type );
		}
		
		public void ModifySkill( Mobile m, ref double atkValue )
		{
			switch ( ((IKhaerosMobile)m).OffensiveFeat )
            {
                case FeatList.None: break;
                case FeatList.ShieldBash: atkValue = m.Skills[SkillName.Parry].Value; break;
                case FeatList.ThrowingMastery: atkValue = m.Skills[SkillName.Throwing].Value; break;
            }
		}
		
		public static int GetDirectionValue( Direction dir )
		{
			if( dir > Direction.Mask )
				dir -= 128;
			
			if( dir == Direction.North )
				return 0;
			else if( dir == Direction.Right )
				return 1;
			else if( dir == Direction.East )
				return 2;
			else if( dir == Direction.Down )
				return 3;
			else if( dir == Direction.South )
				return 4;
			else if( dir == Direction.Left )
				return 5;
			else if( dir == Direction.West )
				return 6;
			
			return 7;
		}
		
		public static int FixDirection( int dir )
		{
			if( dir > 7 )
				dir -= 8;
			
			if( dir < 0 )
				dir += 8;
			
			return dir;
		}
		public static string GetPosition( Mobile attacker, Mobile defender )
		{
			return GetPosition( attacker, defender, false );
		}
		public static string GetPosition( Mobile attacker, Mobile defender, bool assumeProperDirection )
		{			
			int attdir = GetDirectionValue(attacker.Direction);
			if ( assumeProperDirection )
				attdir = GetDirectionValue( attacker.GetDirectionTo( defender.Location ) );
			int defdir = GetDirectionValue(defender.Direction);
			int correct = GetDirectionValue(attacker.GetDirectionTo( defender.Location ));

			if( attdir != correct )
				return "incorrect";
			
			int right = FixDirection(defdir - 2);
			int left = FixDirection(defdir + 2);
			int backright = FixDirection(defdir - 1);
			int backleft = FixDirection(defdir + 1);

			if( attdir == defdir )
				return "back";

			if( attdir == right || attdir == left )
				return "flank";

			if( attdir == backright || attdir == backleft )
				return "back flank";
			
			return "irrelevant";
		}
		
		/*public static int GetPositionBonus( Mobile attacker, Mobile defender )
		{
			string position = GetPosition( attacker, defender );
			
			if( position == "back" )
				return 40;

			if( position == "back flank" )
				return 20;
			
			if( position == "flank" )
				return 10;
			
			return 0;
		}*/
		
		public int GetAttackChanceBonuses( Mobile attacker )
		{
			int bonus = GetHitChanceBonus();

            if (HealthAttachment.HasHealthAttachment(attacker))
            {
                if (this.Layer == Layer.TwoHanded)
                {
                    if (HealthAttachment.GetHA(attacker).HasInjury(Injury.FracturedLeftArm))
                        bonus -= 15;
                    else if (HealthAttachment.GetHA(attacker).HasInjury(Injury.FracturedRightArm))
                        bonus -= 15;
                }
                else if (this.Layer == Layer.OneHanded || this.Layer == Layer.FirstValid)
                {
                    if (HealthAttachment.GetHA(attacker).HasInjury(Injury.FracturedRightArm))
                        bonus -= 15;
                }
            }

			if ( !(this is BaseRanged) ) // bows don't get split bonuses
				bonus /= 2;
			bonus += AosAttributes.GetValue( attacker, AosAttribute.AttackChance );
            bonus += ((IKhaerosMobile)attacker).ManeuverAccuracyBonus;
            
            if( ((IKhaerosMobile)attacker).RageFeatLevel <= 3 ) // normal rage, add bonus
				bonus += ((IKhaerosMobile)attacker).RageFeatLevel * 5;
            else
            	bonus -= (((IKhaerosMobile)attacker).RageFeatLevel - 3) * 5; // defensive fury, remove bonus
                
			if ( attacker is PlayerMobile )
			{
				if( ((PlayerMobile)attacker).GetBackgroundLevel(BackgroundList.Lucky) > 0 )
                	bonus += 1;
                else if( ((PlayerMobile)attacker).GetBackgroundLevel(BackgroundList.Unlucky) > 0 )
                	bonus -= 1;

                bonus += GetRacialMountFeatBonus(attacker, typeof(DireWolf));
			}

			if ( attacker.Weapon is BaseRanged )
			{
				if ( attacker.Weapon is HeavyCrossbow || attacker.Weapon is RepeatingCrossbow || attacker.Weapon is Crossbow )
					bonus += 10*((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.CrossbowMastery);
				else if ( !(attacker.Weapon is Boomerang) ) // this can only be a bow of some sort
					bonus += 10*((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.BowMastery);
			}
			
			if( attacker.Weapon is BaseRanged && attacker.Mounted ) // this says bows in the description? why does it work for every ranged weapon?
				bonus -= 150 - ( 50 * ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.MountedArchery) ); // note, this includes boomerangs as they are baseranged

            if( attacker.Weapon != null )
            {
	            bonus += 5;
			}

            if( ((IKhaerosMobile)attacker).BlindnessTimer != null )
                bonus -= 50;
            
            bonus += ((IKhaerosMobile)attacker).Stance.AccuracyBonus * ((IKhaerosMobile)attacker).Stance.FeatLevel;
            
            return Math.Min( 60, bonus );
		}
		
		public int GetDefendChanceBonuses( Mobile defender )
		{
            int bonus;
            if (!NewCrafting)
                bonus = GetHitChanceBonus() / 2;
            else if (!BetaNerf && NewCrafting)
            {
                QualityDefense /= 2;
                BetaNerf = true;
                bonus = QualityDefense;
                InvalidateProperties();
            }
            else
                bonus = QualityDefense;
			if ( this is BaseRanged )
				bonus = 0; // bows don't get def. chance

			IKhaerosMobile def = defender as IKhaerosMobile;
			bonus += AosAttributes.GetValue(defender, AosAttribute.DefendChance);
			bonus += def.Stance.DefensiveBonus * def.Stance.FeatLevel;

			if( def.RageFeatLevel <= 3 ) // normal rage, remove bonus
				bonus -= def.RageFeatLevel * 5;
			else // defensive rage, add bonus
				bonus += (def.RageFeatLevel - 3) * 5;
			
			if( defender is PlayerMobile )
			{
				bonus += ( (PlayerMobile)defender ).Martyrs * 10;
				if ( ((PlayerMobile)defender).BackToBack )
					bonus += 10*((PlayerMobile)defender).Feats.GetFeatLevel(FeatList.BackToBack);

                bonus += ((PlayerMobile)defender).GetBackgroundLevel(BackgroundList.Lucky);
                bonus -= ((PlayerMobile)defender).GetBackgroundLevel(BackgroundList.Unlucky);
                bonus += GetRacialMountFeatBonus(defender, typeof(RoseanHorse));
			}
			
			return Math.Min( 60, bonus );
		}

		public virtual bool CheckHit( Mobile attacker, Mobile defender )
		{
			BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
			if ( !( atkWeapon is BaseRanged || ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ThrowingMastery ) )
				return true; // melee weapons always hit if within range and haven't been parried/dodged
			// otherwise, carry on. ranged weapons are mostly the same as before.
			BaseWeapon defWeapon = defender.Weapon as BaseWeapon;

			Skill atkSkill = attacker.Skills[atkWeapon.Skill];
			Skill defSkill = defender.Skills[defWeapon.Skill];

			double atkValue = Math.Max( -19.9, atkWeapon.GetAttackSkillValue(attacker, defender) );
			double defValue = Math.Max( -19.9, defWeapon.GetDefendSkillValue(attacker, defender) - 20 );

			ModifySkill( attacker, ref atkValue );

			double ourValue, theirValue;

			int atkBonus = Math.Min( 150, GetAttackChanceBonuses(attacker) );
			int defBonus = Math.Min( 150, GetDefendChanceBonuses(defender) );
			//int atkBonus = 0;
			//int defBonus = 0;

			ourValue = (atkValue + 20.0) * (100 + atkBonus);
			theirValue = (defValue + 20.0) * (100 + defBonus);

			double chance = ourValue / (theirValue * 2.0);
			chance = Math.Max( 0.02, chance );

			return attacker.CheckSkill( atkSkill.SkillName, chance );
		}
		
		public int GetSpeedBonuses( Mobile m )
		{
			if( !(m is IKhaerosMobile) || m is PlayerVendor || m is PlayerMadeStatue )
				return 0;
			
			int bonus = AosAttributes.GetValue( m, AosAttribute.WeaponSpeed );
			bonus += GetSpeedBonus();
			bonus += ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.QuickReflexes)*3;
			if (((IKhaerosMobile)m).Stance.MartialArtistStance)
				bonus += (int)(((IKhaerosMobile)m).Stance.SpeedBonus * ((IKhaerosMobile)m).Stance.FeatLevel);
            
            if(m.Weapon != null && m.Weapon is BaseMeleeWeapon)
                bonus += GetRacialMountFeatBonus(m, typeof(KudaHorse));

            if( m is PlayerMobile && m.Weapon is Fists && ( (PlayerMobile)m ).Claws != null )
                bonus += 3;

			return bonus;
		}

		public virtual TimeSpan GetDelay( Mobile m )
		{
			/*double factor = 1.0;
			if ( m is PlayerMobile )
				factor = 1.0;*/
			
			if( m is PlayerVendor || m is PlayerMadeStatue )
				return TimeSpan.FromSeconds( 20 );
				
			int directBonus = GetSpeedBonuses( m );
			double speedFactor = GetSpeedFactorBonus( m );
			double speedPenalty = GetSpeedPenalty( m );
			double weaponSpeed = this.Speed;
			int dex = m.Dex;
			if ( m is IKhaerosMobile && this is Fists && ((IKhaerosMobile)m).CanUseMartialStance )
				dex += (int)(((0.2*((IKhaerosMobile)m ).Feats.GetFeatLevel(FeatList.MartialOffence))*m.Dex)/2.0);
			if ( m is BaseCreature && ((BaseCreature)m).ControlMaster != null ) // player-controlled stuff is capped at 4
			{
				double mult = 2.0;
				
				mult -= 0.4*(((BaseCreature)m).ControlMaster.Skills[SkillName.Leadership].Value / 100.0);
				weaponSpeed *= mult;
			}
			// some black number magic follows
			// this formula is balanced for 0.5 damage increase per damage point bonus, as it is now. otherwise, it is not.
			// (5/this.Speed)*10*0.125*dex*0.00165
			//double dexfactor = 4.0;
			//double auxspeed = weaponSpeed - (5/this.Speed)*10*0.09*dex*0.00165*dexfactor - directBonus*0.05125 - (5/this.Speed)*10*0.05125*speedFactor;
			//double auxspeed = weaponSpeed - this.Speed*dex*0.0033 - directBonus*0.05125 - (5/this.Speed)*10*0.05125*speedFactor;
			
			// Current dexFactor:
			//double dexFactor = (5/this.Speed)*10*0.125*dex*0.00165;
			
			// Experimental
			double dexFactor = -((this.Speed*2.0/5.0)*35*0.125*100.0*0.00165) + (this.Speed*2.0/5.0)*41.25*0.125*dex*0.00165;
			double auxspeed = weaponSpeed - dexFactor - directBonus*0.05125 - (/*5/*/this.Speed*2.0/5.0)*10*0.05125*speedFactor;

			double overallFactors = -(this.Speed*2.0/4.425)*speedPenalty;
			if (m is IKhaerosMobile && !((IKhaerosMobile)m).Stance.MartialArtistStance)
				overallFactors += (this.Speed*2.0/4.425)*((IKhaerosMobile)m).Stance.SpeedBonus * (double)((IKhaerosMobile)m).Stance.FeatLevel;
				
			double speed = auxspeed - overallFactors;

			if ( speed < 0.5 )
				speed = 0.5;

            if (this is BaseRanged)
            {
                speed += 1.0;
            }
				
			return TimeSpan.FromSeconds( speed );
		}

		public virtual void OnBeforeSwing( Mobile attacker, Mobile defender )
		{
			bool hidden = attacker.Hidden;
			if ( attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).CombatManeuver is Server.Misc.Backstab )
			{
				((Server.Misc.Backstab)((IKhaerosMobile)attacker).CombatManeuver).WasHidden = hidden;
			}
		}
		
		public bool BadCombat( Mobile attacker, Mobile defender )
		{
			if( attacker == null || defender == null )
				return true;
			
        	IKhaerosMobile featuser = attacker as IKhaerosMobile;
            Corpse corpse = null;
            XmlAttachment freeze = XmlAttach.FindAttachment( attacker, typeof( XmlFreeze ) );
            
            if( attacker.Backpack != null )
            	corpse = attacker.Backpack.FindItemByType( typeof( Corpse ) ) as Corpse;
            
            if( corpse != null )
                return true;

            if( freeze != null && ( freeze.Name == "icast" || freeze.Name == "fcast" ) )
               return true;

            if( featuser.DisabledRightArmTimer != null )
                return true;

            if( attacker.Weapon is Lance && !attacker.Mounted )
                return true;
            
            if( featuser.IsTired() && defender is PlayerMobile )
            	return true;

            if( featuser.Enthralled )
            	return true;
            
            if( attacker.Paralyzed )
            	return true;
            
            if( this is BaseRanged && attacker.InRange( defender, 1 ) )
                return true;
		
            return false;
		}
		
		public void BackpackCheck( Mobile mob )
		{
			if( !( mob is PlayerMobile ) )
			   return;
			   
			if( mob.Backpack == null )
			{
				Container pack = new ArmourBackpack();
                pack.Movable = false;
                mob.AddItem( pack );
			}
		}
		
		public void HealingCheck( Mobile mob )
		{
			IKhaerosMobile featuser = mob as IKhaerosMobile;
			
			if( featuser.HealingTimer != null )
            {
            	mob.SendMessage( "You have stopped your attempt to heal someone." );
            	featuser.HealingTimer.Stop();
            	featuser.HealingTimer = null;
            }
		}
		
		public void InitializeManeuver( Mobile attacker, Mobile defender, bool Cleave )
		{
			((IKhaerosMobile)attacker).CombatManeuver.CanUseManeuverCheck( attacker, ((BaseWeapon)attacker.Weapon).Skill );
			if ( defender != null )
				((IKhaerosMobile)attacker).CombatManeuver.OnSwing( attacker, defender, Cleave );
			else if ( ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.None )// defender is null, so auto fumble
			{
				if ( ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.ShieldBash ) // that one already took the stamina
					BaseWeapon.CheckStam( attacker, ((IKhaerosMobile)attacker).CombatManeuver.FeatLevel, Cleave, false );
				attacker.SendMessage( "You fumble the maneuver you had prepared!" );
			}
			
			if( ((IKhaerosMobile)attacker).CombatManeuver.AccuracyBonus > 0 || ((IKhaerosMobile)attacker).CombatManeuver.DamageBonus > 0 )
			{
				((IKhaerosMobile)attacker).ManeuverBonusTimer = new BaseCombatManeuver.ManeuverBonusTimer( attacker );
				((IKhaerosMobile)attacker).ManeuverBonusTimer.Start();
			}
		}
		
		public void EndManeuver( Mobile attacker )
		{
			((IKhaerosMobile)attacker).DisableManeuver();

            if( attacker is PlayerMobile )
            	((PlayerMobile)attacker).m_LastAttack = DateTime.Now;
            
            if( attacker is BaseKhaerosMobile )
        	{
        		((IKhaerosMobile)attacker).ManeuverAccuracyBonus = 0;
        		((IKhaerosMobile)attacker).ManeuverDamageBonus = 0;
        		((IKhaerosMobile)attacker).ManeuverBonusTimer = null;
        		((IKhaerosMobile)attacker).CombatManeuver = null;
        		((IKhaerosMobile)attacker).OffensiveFeat = FeatList.None;
        		
        		if( attacker is PlayerMobile )
        		{
        			((PlayerMobile)attacker).m_LastAttack = DateTime.Now;
        			attacker.Send( new Network.MobileStatus( attacker, attacker ) );
        		}
        	}
		}
		
		public void CheckForInvalidStance( Mobile attacker )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			bool disable = false;
			
			if( !(this is BaseRanged) && !featuser.Stance.Melee )
				disable = true;
			
			if( this is BaseRanged && !featuser.Stance.Ranged )
				disable = true;
			
			if( !featuser.Stance.Armour && !featuser.CanUseMartialStance )
				disable = true;
			
			if( disable )
			{
				if( attacker is PlayerMobile )
					attacker.Emote( featuser.Stance.TurnedOffEmote );
				
				featuser.Stance = null;
			}
		}

		public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			//return OnSwing( attacker, defender, 1.0, false );
			return TimeSpan.FromDays( 365 );
		}
		
		public virtual bool IsStill( Mobile attacker )
		{
			return true;
		}
		
		public virtual bool OnFired( Mobile attacker, Mobile defender )
		{
			return true;
		}

		public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender, double damageBonus, bool Cleave )
		{
			return OnSwing( attacker, defender, damageBonus, Cleave, false );
		}
		public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender, double damageBonus, bool Cleave, bool assumeHit )
		{
			if( BadCombat( attacker, defender ) )
               return TimeSpan.FromSeconds( 1.0 );
			
            BackpackCheck( attacker );
            BackpackCheck( defender );
            HealingCheck( attacker );
			double bestDamage=0.0;
            
            if( !Cleave )
            	((IKhaerosMobile)attacker).CleaveAttack = false;
			else
			{
				bestDamage = RangedPercentage;
				if ( ThrustPercentage > bestDamage )
					bestDamage = ThrustPercentage;
				if ( OverheadPercentage > bestDamage )
					bestDamage = OverheadPercentage;
				if ( SwingPercentage > bestDamage )
					bestDamage = SwingPercentage;
			}

            if( IsStill( attacker ) && attacker.HarmfulCheck( defender ) && OnFired( attacker, defender ) )
			{
				attacker.DisruptiveAction();
				attacker.RevealingAction();
				((IKhaerosMobile)defender).Enthralled = false;
				CheckForInvalidStance( attacker );

				/*if( attacker.NetState != null )
					attacker.Send( new Swing( 0, attacker, defender ) );*/

				InitializeManeuver( attacker, defender, Cleave );
	
				if ( assumeHit || CheckHit( attacker, defender ) )
				{
					if ( Cleave )
						OnHit( attacker, defender, bestDamage*damageBonus );
					else
						OnHit( attacker, defender, damageBonus );
				}
	
				else
					OnMiss( attacker, defender );
				
				EndManeuver( attacker );
				return GetDelay( attacker );
			}

            if (HealthAttachment.HasHealthAttachment(attacker))
            {
                if (HealthAttachment.GetHA(attacker).HasInjury(Injury.FracturedRibs))
                    HealthAttachment.GetHA(attacker).DoInjury(Injury.FracturedRibs);
            }
			attacker.RevealingAction();
			return TimeSpan.FromSeconds( 0.25 );
		}
		
		public virtual void OnSplash( Mobile attacker, Mobile defender, double damageBonus )
		{
			if( BadCombat( attacker, defender ) )
               return;

            if( attacker.HarmfulCheck( defender ) )
			{
				((IKhaerosMobile)defender).Enthralled = false;
				CheckForInvalidStance( attacker );
	
				OnHit( attacker, defender, damageBonus, true );
			}
			
			return;
		}

        public static bool CheckStam( Mobile mob, int featlevel, bool Cleave, bool Double )
        {
        	IKhaerosMobile featuser = mob as IKhaerosMobile;
        	
        	if( featuser.IsTired( true ) )
        		return false;
        		
            if( mob.Stam >= ( featlevel * 2 ) + 4 )
            {
            	if( DateTime.Compare( featuser.NextFeatUse, DateTime.Now ) < 0 )
            	{
            		
            		if ( !Cleave )
	                {
           				featuser.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( Math.Max( 10, ( 60 - featuser.Level ) ) );
            			mob.Stam -= ( featlevel * 2 ) + 4;
	
	                    if ( Double )
	                        mob.Stam -= ( featlevel * 2 ) + 4;
	                }
            		
	                return true;
            	}
            	
            	else
            	{
            		TimeSpan waitingtime = featuser.NextFeatUse - DateTime.Now;
            		mob.SendMessage( 60, "You need wait another " + waitingtime.Seconds + " seconds before using another ability." );
            		featuser.DisableManeuver();
            		return false;
            	}
            }
            
            else
            {
                mob.SendMessage( 60, "You need {0} stamina in order to perform this attack.", ( featlevel * 2 ) + 4 );
                featuser.DisableManeuver();
                return false;
            }
        }

        public static bool CheckStam( Mobile mob, int featlevel )
        {
        	IKhaerosMobile featuser = mob as IKhaerosMobile;
        	
        	if( featuser.IsTired( true ) )
        		return false;
        		
            if( mob.Stam >= featlevel )
            {
            	if( DateTime.Compare( featuser.NextFeatUse, DateTime.Now ) < 0 )
            	{
	                mob.Stam -= featlevel;
	                featuser.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( Math.Max( 10, ( 60 - featuser.Level ) ) );
	                return true;
            	}
            	
            	else
            	{
            		TimeSpan waitingtime = featuser.NextFeatUse - DateTime.Now;
            		mob.SendMessage( 60, "You need wait another " + waitingtime.Seconds + " seconds before using another ability." );
            		return false;
            	}
            }
            else
            {
                mob.SendMessage( 60, "You need {0} stamina in order to perform this attack.", featlevel );
                return false;
            }
        }
        
        public static bool CheckStam( Mobile mob, int amount, int time )
        {
        	PlayerMobile featuser = mob as PlayerMobile;
        	
        	if( featuser.IsTired( true ) )
        		return false;
        		
            if( featuser.Stam >= amount )
            {
            	if( DateTime.Compare( featuser.NextFeatUse, DateTime.Now ) < 0 )
            	{
	                featuser.Stam -= amount;
	                featuser.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( time );
	                return true;
            	}
            	
            	else
            	{
            		TimeSpan waitingtime = featuser.NextFeatUse - DateTime.Now;
            		featuser.SendMessage( 60, "You need wait another " + waitingtime.Seconds + " seconds before using another ability." );
            		return false;
            	}
            }
            else
            {
                featuser.SendMessage( 60, "You need {0} stamina in order to perform this attack.", amount );
                return false;
            }
        }

		#region Sounds
		public virtual int GetHitAttackSound( Mobile attacker, Mobile defender )
		{
			int sound = attacker.GetAttackSound();

			if ( sound == -1 || (attacker.BodyValue == 400 || attacker.BodyValue == 401) )
				sound = HitSound;

			return sound;
		}

		public virtual int GetHitDefendSound( Mobile attacker, Mobile defender )
		{
			return defender.GetHurtSound();
		}

		public virtual int GetMissAttackSound( Mobile attacker, Mobile defender )
		{
			if ( attacker.GetAttackSound() == -1 || (attacker.BodyValue == 400 || attacker.BodyValue == 401) )
				return MissSound;
			else
				return -1;
		}

		public virtual int GetMissDefendSound( Mobile attacker, Mobile defender )
		{
			return -1;
		}
		#endregion

		public virtual void XmlOnKilled( Mobile killed, Mobile killer )
		{
			ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlAttachment ) );

			if( list == null )
				return;

			for( int i = 0; i < list.Count; ++i )
			{
				XmlAttachment enhancement = list[i] as XmlAttachment;
				enhancement.OnKilled( killed, killer );
			}
		}
		
		public virtual void XmlOnWeaponHit( Mobile attacker, Mobile defender, int damage )
		{
			ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlAttachment ) );

			if( list == null )
				return;

			for( int i = 0; i < list.Count; ++i )
			{
				XmlAttachment enhancement = list[i] as XmlAttachment;
				enhancement.OnWeaponHit( attacker, defender, this, damage );
			}
		}
		
		public static bool CheckParry( Mobile attacker, Mobile defender )
		{
            double pbonus = 0.0;

			if ( defender == null )
				return false;
			
			XmlAttachment freeze = XmlAttach.FindAttachment( defender, typeof( XmlFreeze ) );
                
        	if( freeze != null && ( freeze.Name == "icast" || freeze.Name == "fcast" ) )
               return false;

			BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			
			double consbonus = 0.0;
			
			BaseWeapon defweapon = defender.Weapon as BaseWeapon;
                
            if( defweapon != null && defweapon.Consecrated )
	            consbonus = 0.05;

			double parry = defender.Skills[SkillName.Parry].Value;
			
			if ( shield != null )
			{
				//pbonus += ((IKhaerosMobile)defender).Feats.GetFeatLevel(FeatList.ShieldMastery) * 0.05;
				double chance = parry / 400.0;

				// Low dexterity lowers the chance.
				if ( defender.Dex < 80 )
					chance = chance * (20 + defender.Dex) / 100;
				
				chance += consbonus + pbonus;

				return defender.CheckSkill( SkillName.Parry, chance );
			}
			else if( !(defender.Weapon is BaseRanged) )
			{
				BaseWeapon weapon = defender.Weapon as BaseWeapon;

				double divisor = (weapon.Layer == Layer.OneHanded) ? 1600.0 : 800.0;

				double chance = (parry) / divisor;

				// Low dexterity lowers the chance.
				if( defender.Dex < 80 )
					chance = chance * (20 + defender.Dex) / 100;
                
                chance += consbonus + pbonus;

				return defender.CheckSkill( SkillName.Parry, chance );
			}

			return false;
		}

		public virtual int AbsorbDamageAOS( Mobile attacker, Mobile defender, int damage )
		{
			bool blocked = false;

			if ( defender.Player || defender.Body.IsHuman )
			{
				if ( ( attacker.Weapon is BaseRanged || ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ThrowingMastery ) )
				{
					if( attacker is PlayerMobile )
					{
						//if( ( (PlayerMobile)attacker ).OffensiveFeat != FeatList.FlashyAttack )
						//{
							blocked = CheckParry( attacker, defender );
						//}
					}

					else
						blocked = CheckParry( attacker, defender );
				}

				if ( blocked )
				{
					defender.FixedEffect( 0x37B9, 10, 16 );
					damage = 0;

					// Successful block removes the Honorable Execution penalty.
					HonorableExecution.RemovePenalty( defender );

					if ( CounterAttack.IsCountering( defender ) )
					{
						BaseWeapon weapon = defender.Weapon as BaseWeapon;

						if ( weapon != null )
							weapon.OnSwing( defender, attacker );

						CounterAttack.StopCountering( defender );
					}

					if ( Confidence.IsConfident( defender ) )
					{
						defender.SendLocalizedMessage( 1063117 ); // Your confidence reassures you as you successfully block your opponent's blow.

						double bushido = defender.Skills.Leadership.Value;

						defender.Hits += Utility.RandomMinMax( 1, (int)(bushido / 12) );
						defender.Stam += Utility.RandomMinMax( 1, (int)(bushido / 5) );
					}

					BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;

					if ( shield != null )
						shield.OnHit( this, damage );
				}
			}

			if ( !blocked )
			{
				double positionChance = Utility.RandomDouble();

				Item armorItem;

				if( positionChance < 0.07 )
					armorItem = defender.NeckArmor;
				else if( positionChance < 0.14 )
					armorItem = defender.HandArmor;
				else if( positionChance < 0.28 )
					armorItem = defender.ArmsArmor;
				else if( positionChance < 0.43 )
					armorItem = defender.HeadArmor;
				else if( positionChance < 0.65 )
					armorItem = defender.LegsArmor;
				else
					armorItem = defender.ChestArmor;

				IWearableDurability armor = armorItem as IWearableDurability;

				if ( armor != null )
					armor.OnHit( this, damage ); // call OnHit to lose durability
			}

			return damage;
		}

		public virtual int AbsorbDamage( Mobile attacker, Mobile defender, int damage )
		{
			if ( Core.AOS )
				return AbsorbDamageAOS( attacker, defender, damage );

			double chance = Utility.RandomDouble();

			Item armorItem;

			if( chance < 0.07 )
				armorItem = defender.NeckArmor;
			else if( chance < 0.14 )
				armorItem = defender.HandArmor;
			else if( chance < 0.28 )
				armorItem = defender.ArmsArmor;
			else if( chance < 0.43 )
				armorItem = defender.HeadArmor;
			else if( chance < 0.65 )
				armorItem = defender.LegsArmor;
			else
				armorItem = defender.ChestArmor;

			IWearableDurability armor = armorItem as IWearableDurability;

			if ( armor != null )
				damage = armor.OnHit( this, damage );

			BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			if ( shield != null )
				damage = shield.OnHit( this, damage );

			int virtualArmor = defender.VirtualArmor + defender.VirtualArmorMod;

			if ( virtualArmor > 0 )
			{
				double scalar;

				if ( chance < 0.14 )
					scalar = 0.07;
				else if ( chance < 0.28 )
					scalar = 0.14;
				else if ( chance < 0.43 )
					scalar = 0.15;
				else if ( chance < 0.65 )
					scalar = 0.22;
				else
					scalar = 0.35;

				int from = (int)(virtualArmor * scalar) / 2;
				int to = (int)(virtualArmor * scalar);

				damage -= Utility.Random( from, (to - from) + 1 );
			}

			return damage;
		}

		public virtual int GetPackInstinctBonus( Mobile attacker, Mobile defender )
		{
			if ( attacker.Player || defender.Player )
				return 0;

			BaseCreature bc = attacker as BaseCreature;

			if ( bc == null || bc.PackInstinct == PackInstinct.None || (!bc.Controlled && !bc.Summoned) )
				return 0;

			Mobile master = bc.ControlMaster;

			if ( master == null )
				master = bc.SummonMaster;

			if ( master == null )
				return 0;

			int inPack = 1;

			foreach ( Mobile m in defender.GetMobilesInRange( 1 ) )
			{
				if ( m != attacker && m is BaseCreature )
				{
					BaseCreature tc = (BaseCreature)m;

					if ( (tc.PackInstinct & bc.PackInstinct) == 0 || (!tc.Controlled && !tc.Summoned) )
						continue;

					Mobile theirMaster = tc.ControlMaster;

					if ( theirMaster == null )
						theirMaster = tc.SummonMaster;

					if ( master == theirMaster && tc.Combatant == defender )
						++inPack;
				}
			}

			if ( inPack >= 5 )
				return 100;
			else if ( inPack >= 4 )
				return 75;
			else if ( inPack >= 3 )
				return 50;
			else if ( inPack >= 2 )
				return 25;

			return 0;
		}

		private static bool m_InDoubleStrike;

		public static bool InDoubleStrike
		{
			get{ return m_InDoubleStrike; }
			set{ m_InDoubleStrike = value; }
		}

		public virtual void OnHit( Mobile attacker, Mobile defender )
		{
	        OnHit( attacker, defender, 1.0 );
		}
		
		public virtual bool AvoidedDamage( Mobile attacker, Mobile defender, ref double damageBonus  )
		{
			PlayerMobile attplayer = attacker as PlayerMobile;
            PlayerMobile defplayer = defender as PlayerMobile;
            	
			if( ((IKhaerosMobile)defender).CanDodge &&  ( !defender.Mounted || Utility.RandomMinMax(1,100) < GetRacialMountAbility(defender, typeof(ForestStrider))))
            {
                if( ((IKhaerosMobile)defender).Dodged() )
                {
                	defender.Emote( "*skillfully dodged a blow from {0}*", attacker.Name );
                	return true;
                }

                if( attacker.Weapon is BaseRanged && ((IKhaerosMobile)defender).Snatched() )
                {
                    defender.Emote( "*snatched a projectile shot at {0} by {1}*", defender.Female == true ? "her" : "him", attacker.Name );
                    return true;
                }
        	}

            if (attacker.Weapon is BaseRanged && Utility.RandomMinMax(1, 100) < GetRacialMountAbility(defender, typeof(KudaHorse)))
            {
                defender.Emote("*uses {0} Kuda Horse's agility to evade a projectile shot at {1} by {2}*", defender.Female == true ? "her" : "his", defender.Female == true ? "her" : "him", attacker.Name);
                return true;
            }
            	
        	if( attacker.Weapon is BaseRanged && ((IKhaerosMobile)defender).DeflectedProjectile() )
            {
                defender.Emote( "*uses {0} shield to deflect a projectile shot at {1} by {2}*", defender.Female == true ? "her" : "his", defender.Female == true ? "her" : "him", attacker.Name );
                return true;
            }
            	
        	if( defender is PlayerMobile )
        	{
            	if( Utility.Random( 1, 100 ) == defplayer.GetBackgroundLevel(BackgroundList.Lucky) )
                	return true;
                
                if( Utility.Random( 1, 100 ) == defplayer.GetBackgroundLevel(BackgroundList.Unlucky) )
                	damageBonus += 1.0;
            }
            
            if( attacker is PlayerMobile )
            {
            	if( Utility.Random( 1, 100 ) == attplayer.GetBackgroundLevel(BackgroundList.Lucky) )
                	damageBonus += 1.0;
                
                if( Utility.Random( 1, 100 ) == attplayer.GetBackgroundLevel(BackgroundList.Unlucky) )
                	return true;
            }
            
            return false;
		}
		
		public double GetDamagePenalty( Mobile attacker, Mobile defender )
		{
			if ( attacker.Weapon == null )
				return 0.0;
			double penalty = this.DamagePenalty;
			if( attacker.Weapon is Fists && attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).TechniqueLevel > 0 )
			{
				int slax = 0;
				int pirc = 0;
				int blut = 100;
				if( ((IKhaerosMobile)attacker).Technique == "slashing" )
					slax = ((IKhaerosMobile)attacker).TechniqueLevel;
				else if( ((IKhaerosMobile)attacker).Technique == "piercing" )
					pirc = ((IKhaerosMobile)attacker).TechniqueLevel;
				
				blut -= ((IKhaerosMobile)attacker).TechniqueLevel;
				
				if ( blut > 0 )
					penalty += blut*0.002;
			}
			else if ( AosElementDamages.Blunt > 0 )
				penalty += (AosElementDamages.Blunt)*0.002; // 20% penalty at 100% bashing

            if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
                penalty = 0.0;

			return penalty;
		}
		
		public double GetDamageFactorBonus( Mobile attacker, Mobile defender )
		{
			double factor = 0.0; // this means no bonus
			IKhaerosMobile attk = null;
			if ( attacker is IKhaerosMobile )
				attk = attacker as IKhaerosMobile;
			
			if ( attacker is PlayerMobile )
			{
				PlayerMobile featuser = attacker as PlayerMobile;
              	if( attk.OffensiveFeat != FeatList.ShieldBash && attk.OffensiveFeat != FeatList.ThrowingMastery )
				{
					if( ((BaseWeapon)attacker.Weapon).NameType == featuser.WeaponSpecialization )
						factor += 0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.WeaponSpecialization);

					else if( ((BaseWeapon)attacker.Weapon).NameType == featuser.SecondSpecialization )
						factor += 0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.SecondSpecialization);
					else
						factor -= (0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.WeaponSpecialization));
					double fsbonuses = CombatSystemAttachment.FightingStyleBonuses( attacker );
					if ( fsbonuses > 0.45 )
						fsbonuses = 0.45;
					factor += fsbonuses; // conditions are implied here
				}

                if(this is BaseRanged) // Editing Archery to make it more of a support build -- TMLOYD
                    factor += Utility.RandomMinMax(2,5);
				
				factor += featuser.Heroes * 0.075;
			}
			
			//factor += ((IKhaerosMobile)attacker).Stance.DamageBonus * (double)((IKhaerosMobile)attacker).Stance.FeatLevel;
			
			if( attk != null && attk.RageFeatLevel <= 3 )
				factor += ((IKhaerosMobile)attacker).RageFeatLevel * 0.075;
				
			if( attk != null && attk.OffensiveFeat != FeatList.ShieldBash )
			{
				if ( BastardWeapon && (!(attacker.FindItemOnLayer( Layer.TwoHanded ) is BaseShield)) )
					factor += 0.10;
					
				else if ( Layer == Layer.TwoHanded && !(this is BaseRanged) )
					factor += 0.30;
			}
			
			return factor;
		}
		
		public double GetSpeedPenalty( Mobile attacker ) // this should not be used at all atm
		{
			double penalty = 0.0; // this means no penalty.
			//IKhaerosMobile km = attacker as IKhaerosMobile;
			
			return penalty;
		}
		
		public double GetSpeedFactorBonus( Mobile attacker )
		{
			double factor = 0.0; // this means no bonus
			if( attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).DazedTimer != null )
				factor -= 0.30;
			
			if ( attacker is PlayerMobile && ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.ThrowingMastery )
			{
				PlayerMobile featuser = attacker as PlayerMobile;
				if( ((BaseWeapon)attacker.Weapon).NameType == featuser.WeaponSpecialization )
					factor += 0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.WeaponSpecialization);

				else if( ((BaseWeapon)attacker.Weapon).NameType == featuser.SecondSpecialization )
					factor += 0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.SecondSpecialization);

				else
					factor -= 0.075 * (double)featuser.Feats.GetFeatLevel(FeatList.WeaponSpecialization);
				
				factor += featuser.Heroes * 0.075;
			}
			
 			if ( Unwieldy )
			{
				factor -= 0.2; 
				// 50% speed for polearms (this is not supposed to be counted in the penalty calculations)
				// as that would penalize post-bonus, we don't want that in this case
/* 				if ( this is BasePoleArm && attacker is IKhaerosMobile )
					factor += ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.PolearmsMastery)*0.1; */
			} 
			
			//factor += ((IKhaerosMobile)attacker).Stance.SpeedBonus * (double)((IKhaerosMobile)attacker).Stance.FeatLevel;
				
			Item twohanded = attacker.FindItemOnLayer( Layer.TwoHanded ) as Item;
			
			if (( Layer == Layer.OneHanded || this is Fists ) && (!(twohanded is BaseShield)) )
			{
				if ( BastardWeapon )
					factor += 0.10;
				else
					factor += 0.20;
			}
			
			if( attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).RageFeatLevel <= 3 )
				factor += ((IKhaerosMobile)attacker).RageFeatLevel * 0.075;

            if (this is BaseRanged) // Editing to improve archery as a support skill. -- TMLOYD
                factor += 0.5;
			
			return factor;
		}
		
		public virtual int GetDamageBonuses( Mobile attacker, Mobile defender )
		{
			int damage = 0;
           
            BaseWeapon weapon = attacker.Weapon as BaseWeapon;
            
            if( weapon != null && weapon.Consecrated )
	            damage += 3;

			if ( attacker is IKhaerosMobile )
			{
				damage += ((IKhaerosMobile)attacker).ManeuverDamageBonus; // TODO: check bonuses
				damage += ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.BruteStrength)*3;
                
                if (((IKhaerosMobile)attacker).Stance.MartialArtistStance)
				    damage += (int)(((IKhaerosMobile)attacker).Stance.DamageBonus * ((IKhaerosMobile)attacker).Stance.FeatLevel);
			}
            if (attacker is PlayerMobile && attacker.Weapon != null && attacker.Weapon is BaseRanged)
            {
                damage += GetRacialMountFeatBonus(attacker, typeof(BarbHorse));
            }
            else if (attacker is PlayerMobile && attacker.Weapon != null && !(attacker.Weapon is BaseRanged))
            {
                damage += GetRacialMountFeatBonus(attacker, typeof(SteppeHorse));
            }
            damage += GetDamageBonus();


            return damage;
		}
		
		public virtual void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			OnHit( attacker, defender, damageBonus, false );
		}

		public virtual void OnHit( Mobile attacker, Mobile defender, double damageBonus, bool splashAttack )
		{
            if (defender is IKhaerosMobile)
            {
                if (((IKhaerosMobile)defender).ShieldValue <= 0)
                {
                    ((IKhaerosMobile)defender).RemoveShieldOfSacrifice();
                }
                else if (((IKhaerosMobile)defender).ShieldingMobile != null && !((IKhaerosMobile)defender).ShieldingMobile.Deleted && ((IKhaerosMobile)defender).ShieldingMobile.Alive)
                {
                    ((IKhaerosMobile)defender).ShieldValue--;
                    defender = ((IKhaerosMobile)defender).ShieldingMobile;
                }
            }

            BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
            BaseWeapon defWeapon = defender.Weapon as BaseWeapon;

            Skill atkSkill = attacker.Skills[atkWeapon.Skill];
            Skill defSkill = defender.Skills[defWeapon.Skill];
			bool thrown = (((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ThrowingMastery );

			//PlaySwingAnimation( attacker );
			
			if(  ( atkWeapon is BaseRanged || ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ThrowingMastery ) && AvoidedDamage( attacker, defender, ref damageBonus ) )
				return;

			//PlayHurtAnimation( defender );

        	/*if( ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ShieldBash )
                attacker.PlaySound( 0x3AC );

            else
                attacker.PlaySound( GetHitAttackSound( attacker, defender ) );

			defender.PlaySound( GetHitDefendSound( attacker, defender ) );*/
			
			if ( !splashAttack && ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.ShieldBash )
				CombatSystemAttachment.PlayHitSound( attacker, defender );

			int damage = (int)((splashAttack ? 0.4 : 1.0)*(damageBonus*ComputeDamage( attacker, defender )));

			if ( attacker is BaseCreature )
				((BaseCreature)attacker).AlterMeleeDamageTo( defender, ref damage );

			if ( defender is BaseCreature )
				((BaseCreature)defender).AlterMeleeDamageFrom( attacker, ref damage );

			damage = AbsorbDamage( attacker, defender, damage );

			if ( damage < 0 )
				damage = 0;
			
			if ( damage > 0 && !splashAttack )
			{
				if ( attacker.Weapon != null )
				{
					OilAttachment attachment = XmlAttach.FindAttachment( attacker.Weapon, typeof( OilAttachment ) ) as OilAttachment;
					if ( attachment != null )
						attachment.OnWeaponHit( defender, attacker );
				}
			}
			
			if ( !splashAttack )
				AddBlood( attacker, defender, damage );

			bool shieldbash = (((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ShieldBash);
			int phys, fire, cold, pois, nrgy, blut, slax, pirc;

			GetDamageTypes( attacker, out phys, out fire, out cold, out pois, out nrgy, out blut, out slax, out pirc );

			//Technique Mod: changing the damage type to slashing or piercing and lowering blunt damage based on that.
            if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
            {
                blut = 0;
                slax = 80;
                pirc = 20;
            }

			if( attacker.Weapon is Fists && attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).TechniqueLevel > 0 )
			{
				if( ((IKhaerosMobile)attacker).Technique == "slashing" )
					slax = ((IKhaerosMobile)attacker).TechniqueLevel;
				else if( ((IKhaerosMobile)attacker).Technique == "piercing" )
					pirc = ((IKhaerosMobile)attacker).TechniqueLevel;
				
				blut -= ((IKhaerosMobile)attacker).TechniqueLevel;
			}
			//End Technique Mod
			
			int damageGiven = damage;
			bool ignoreArmor = false;
			
			if ( !splashAttack )
			{
				if( ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.None )
				{
                    if( defender is BaseCreature && ( (BaseCreature)defender ).ManeuverResistance > Utility.RandomMinMax( 1, 100 ) )
                        attacker.SendMessage( defender.Name + " resisted your attack." );

                    else
					    ((IKhaerosMobile)attacker).CombatManeuver.OnHit( attacker, defender );

					((IKhaerosMobile)attacker).OffensiveFeat = FeatList.None;
				}
			}

            int damageignore = 0;
            
            if( defender is PlayerMobile )
            {
            	damageignore += ( (PlayerMobile)defender ).Feats.GetFeatLevel(FeatList.DamageIgnore);
                damageignore += ( (PlayerMobile)defender ).Martyrs;
                damageignore += ( (PlayerMobile)defender ).BloodOfXorgoth == null ? 0 : 3;
                int plateMastery = ( (PlayerMobile)defender ).Feats.GetFeatLevel(FeatList.PlateMastery);

                if( plateMastery > 0 )
                    damageignore += (int)(( (PlayerMobile)defender ).HeavyPieces * ( 0.5 + ( 0.25 * plateMastery ) ));

                int rangedDefense = ( (PlayerMobile)defender ).Feats.GetFeatLevel(FeatList.RangedDefense);

                if (this is BaseRanged && rangedDefense > 0)
                    if (Utility.RandomMinMax(1, 100) < (3 * rangedDefense * (defender as PlayerMobile).MediumPieces))
                        damageignore += ((defender as PlayerMobile).MediumPieces * (rangedDefense * Utility.RandomMinMax(1, 10)));

                if (defender.FindItemOnLayer(Layer.TwoHanded) is BaseShield && 
                    (defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield).ArmourType == ArmourWeight.Heavy && 
                    this is BaseRanged)
                    damageignore += (defender.ShieldArmor as BaseShield).BasePiercingResistance / 10;

				if( ((IKhaerosMobile)attacker).RageFeatLevel > 3 )
					damageignore += ((IKhaerosMobile)attacker).RageFeatLevel - 3;

                damageignore += GetRacialMountFeatBonus(defender, typeof(RuganHorse));
            }

            IKhaerosMobile defkm = defender as IKhaerosMobile;

			if ( defender.Mounted )
			{
				if ( defkm.Feats.GetFeatLevel(FeatList.MountedDefence) > 0 )
				{
					Mobile mount = defender.Mount as Mobile;
					if ( mount != null && (((double)mount.Hits)/((double)mount.HitsMax)) >= ((double)(15+defkm.Feats.GetFeatLevel(FeatList.MountedDefence)*20))/100.0)
					{
						int amt = (int)(damage*0.05*defkm.Feats.GetFeatLevel(FeatList.MountedDefence));
						damageignore += amt;
						mount.Hits -= amt;
						if ( defender is PlayerMobile )
							((PlayerMobile)defender).MountedDefenceIconRefresh();
					}
				}
			}
			
			int finaldamage = Math.Max( damage - damageignore, 0 );
			
			if ( finaldamage < 0 )
				finaldamage = 0;
			
			if ( attacker is PlayerMobile && ((PlayerMobile)attacker).Spar && !(this is BaseRanged) &&
				!thrown )
			{
				finaldamage = 1;
				ignoreArmor = true;
			}

			if ( shieldbash )
				damageGiven = AOS.Damage( defender, attacker, finaldamage, ignoreArmor, 0, 0, 0, 0, 0, 100, 0, 0 ); // blunt all the way
			else if ( !splashAttack )
				damageGiven = AOS.Damage( defender, attacker, finaldamage, ignoreArmor, 0, fire, cold, pois, nrgy, blut, slax, pirc );
			else
			{
				damageGiven = AOS.Damage( defender, attacker, finaldamage, ignoreArmor, 0, 0, 0, 0, 0, 100, 0, 0 ); // blunt all the way
				return; // we're done here for splash attacks
			}

            if (splashAttack)
            {
                if (Utility.RandomMinMax(1,100) < GetRacialMountAbility(attacker, typeof(SteppeHorse)))
                {
                    damageGiven += (int) (damageGiven * (double)(0.01 * GetRacialMountAbility(attacker, typeof(SteppeHorse))));
                }
            }

            if (!splashAttack)
            {

                if (defender is PlayerMobile && Utility.RandomMinMax(1, 200) < GetRacialMountAbility(defender, typeof(RuganHorse)))
                    if ((defender as PlayerMobile).Feats.GetFeatLevel(FeatList.PlateMastery) > 0)
                        if ((defender.Mount as BaseMount).BodyValue == 284)
                        {
                            damageGiven -= (int)(damageGiven * ((0.01 * GetRacialMountAbility(defender, typeof(RuganHorse)))));
                            defender.Emote(attacker.Name + "'s attack glances off " + (defender.Female ? "her" : "his") + " horse's barding*");
                        }
            }

            if (splashAttack)
            {
                if (Utility.RandomMinMax(1, 100) < GetRacialMountAbility(defender, typeof(RoseanHorse)))
                {
                    damageGiven = 0;
                    defender.Emote("*defends " + (defender.Female ? "herself" : "himself") + " using skillfull horsemanship*");
                }
            }

			XmlOnWeaponHit( attacker, defender, damageGiven );

			if (HealthAttachment.HasHealthAttachment(defender) && HealthAttachment.GetHA(defender).HasInjury(Injury.Bruised))
                HealthAttachment.GetHA(defender).DoInjury(Injury.Bruised);

			if( defender.Alive && ((defender is PlayerMobile && ((PlayerMobile)defender).IsVampire) || 
			                       (defender is BaseCreature && defender is IUndead)) && HolyWaterPower > 0 )
				SpellHelper.Damage( TimeSpan.Zero, defender, attacker, HolyWaterPower, 100, 0, 0, 0, 0 );
			
            if( !defender.CheckSkill( SkillName.Concentration, ((defender.Skills[SkillName.Concentration].Base - finaldamage) * 0.01) ) )
            	((IKhaerosMobile)defender).Fizzled = true;
            
            if(Utility.RandomBool())
                DegradeWeapon();

            if (this.Type == WeaponType.Bashing && Utility.RandomBool())
            {
                if (defender.FindItemOnLayer(Layer.TwoHanded) != null && 
                    !defender.FindItemOnLayer(Layer.TwoHanded).Deleted && 
                    defender.FindItemOnLayer(Layer.TwoHanded) is BaseShield && 
                    (defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield).ArmourType == ArmourWeight.Heavy)
                        (defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield).DegradeArmor(1);
                else
                {
                    int armor = Utility.RandomMinMax(1, 9);
                    BaseArmor ar = null;

                    switch (armor)
                    {
                        case 1: if (defender.FindItemOnLayer(Layer.Helm) != null) ar = (defender.FindItemOnLayer(Layer.Helm) as BaseArmor); break;
                        case 2: if (defender.FindItemOnLayer(Layer.Neck) != null) ar = (defender.FindItemOnLayer(Layer.Neck) as BaseArmor); break;
                        case 3: if (defender.FindItemOnLayer(Layer.Arms) != null) ar = (defender.FindItemOnLayer(Layer.Arms) as BaseArmor); break;
                        case 4: if (defender.FindItemOnLayer(Layer.InnerTorso) != null) ar = (defender.FindItemOnLayer(Layer.InnerTorso) as BaseArmor); break;
                        case 5: if (defender.FindItemOnLayer(Layer.MiddleTorso) != null) ar = (defender.FindItemOnLayer(Layer.MiddleTorso) as BaseArmor); break;
                        case 6: if (defender.FindItemOnLayer(Layer.OuterTorso) != null) ar = (defender.FindItemOnLayer(Layer.OuterTorso) as BaseArmor); break;
                        case 7: if (defender.FindItemOnLayer(Layer.InnerLegs) != null) ar = (defender.FindItemOnLayer(Layer.InnerLegs) as BaseArmor); break;
                        case 8: if (defender.FindItemOnLayer(Layer.OuterLegs) != null) ar = (defender.FindItemOnLayer(Layer.OuterLegs) as BaseArmor); break;
                        case 9: if (defender.FindItemOnLayer(Layer.Gloves) != null) ar = (defender.FindItemOnLayer(Layer.Gloves) as BaseArmor); break;
                    }

                    if ( ar != null && !ar.Deleted && ar.ArmourType == ArmourWeight.Heavy)
                        ar.DegradeArmor(1);
                }
            }
            
            if( attacker is BlackPudding || attacker is GelatinousBlob || attacker is JellyOoze || attacker is BoaConstrictor || attacker is ConstrictingVine || attacker is Wortling )
            	if( Utility.Random( 100 ) > 30 )
            		Misc.CripplingBlow.Effect( attacker, defender, 3 );

			if ( attacker is BaseCreature )
				((BaseCreature)attacker).OnGaveMeleeAttack( defender );

			if ( defender is BaseCreature )
				((BaseCreature)defender).OnGotMeleeAttack( attacker );
			
			if( attacker is PlayerMobile && ((PlayerMobile)attacker).TouchSpell != null && defender.Alive )
			{
				((PlayerMobile)attacker).TouchSpell.TargetMobile = defender;
				((PlayerMobile)attacker).TouchSpell.HandleEffect( true );
				((PlayerMobile)attacker).TouchSpell = null;
			}
		}
		
		public void DegradeWeapon()
		{
            if (this.Parent is BaseCreature && !(this.Parent is Mercenary) && Utility.RandomMinMax(1, 100) > 90)
                return;

            int qualityDivisor = 0;
            switch(this.Quality)
            {
                case WeaponQuality.Poor: qualityDivisor = 1; break;
                case WeaponQuality.Low: qualityDivisor = 1; break;
                case WeaponQuality.Inferior: qualityDivisor = 1; break;
                case WeaponQuality.Regular: qualityDivisor = 1; break;
                case WeaponQuality.Superior: qualityDivisor = 2; break;
                case WeaponQuality.Exceptional: qualityDivisor = 2; break;
                case WeaponQuality.Remarkable: qualityDivisor = 2; break;
                case WeaponQuality.Extraordinary: qualityDivisor = 3; break;
                case WeaponQuality.Antique: qualityDivisor = 3; break;
                case WeaponQuality.Masterwork: qualityDivisor = 4; break;
                case WeaponQuality.Legendary: qualityDivisor = 10; break;
                default: qualityDivisor = 2; break;
            }

            int damageChanceIncrease = (this.MaxHitPoints - this.HitPoints) / qualityDivisor;

 			if( Utility.RandomMinMax( 1, 100 ) > ( 95 - damageChanceIncrease ) )
            {
/*                 switch (this.Quality)
                {
                    case WeaponQuality.Poor: this.HitPoints -= Utility.Random(2); break;
                    case WeaponQuality.Low: this.HitPoints -= Utility.Random(2); break;
                    case WeaponQuality.Inferior: this.HitPoints -= Utility.Random(2); break;
                    case WeaponQuality.Regular: this.HitPoints -= Utility.Random(3); break;
                    case WeaponQuality.Superior: this.HitPoints -= Utility.Random(2); break;
                    case WeaponQuality.Exceptional: this.HitPoints -= Utility.Random(1); break;
                    case WeaponQuality.Remarkable: this.HitPoints -= Utility.Random(1); break;
                    case WeaponQuality.Extraordinary: this.HitPoints -= Utility.Random(1); break;
                    case WeaponQuality.Antique: if (Utility.RandomBool()) this.HitPoints -= Utility.Random(1); break;
                    case WeaponQuality.Masterwork: if(Utility.RandomBool()) this.HitPoints -= Utility.Random(1); break;
                    case WeaponQuality.Legendary: if (Utility.RandomBool()) this.HitPoints--; break;
                }  */

                if (Utility.RandomMinMax(1, 100) > ( 95 - damageChanceIncrease ) )
                {
                    switch (this.Resource)
                    {
                        case CraftResource.Copper: this.HitPoints -= Utility.Random(5); break;
                        case CraftResource.Bronze: this.HitPoints -= Utility.Random(1); break;
                        case CraftResource.Iron: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Steel: if(Utility.RandomBool()) this.HitPoints -= Utility.Random(1); break;
                        case CraftResource.Obsidian: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Starmetal: if(Utility.RandomBool()) this.HitPoints -= Utility.Random(1); break;
                        case CraftResource.Silver: this.HitPoints -= Utility.Random(5); break;
                        case CraftResource.Gold: this.HitPoints -= Utility.Random(6); break;

                        case CraftResource.Oak: this.HitPoints -= Utility.Random(4); break;
                        case CraftResource.Redwood: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Yew: this.HitPoints -= Utility.Random(2); break;
                        case CraftResource.Ash: this.HitPoints -= Utility.Random(3); break;
                        case CraftResource.Greenheart: if(Utility.RandomBool()) this.HitPoints -= Utility.Random(2); break;
                    }
                }

	            if( this.HitPoints < 0 )
	            {
	                this.MaxHitPoints--;
	                this.HitPoints = 0;                    
	            }

                if ( this.MaxHitPoints < 1 )
                    this.Delete();
            }
		}

        public static void PickRandomEmote( Mobile attacker, Mobile defender, int index )
        {
        	if( !(attacker.Weapon is Fists) )
        		return;
        	
        	IKhaerosMobile aggressor = attacker as IKhaerosMobile;
        	IKhaerosMobile aggressed = defender as IKhaerosMobile;
        	
            int number = Utility.Dice( 1, 6, 0 );

            switch( index )
            {
                case 1:
                {
                    switch( number )
                    {
                    	case 1: attacker.Emote( "*grabs {0} by the head, leans back, and thrusts a devastating knee into the side of {1} ribcage*", defender.Name, aggressed.GetPossessivePronoun() ); break;
                    	case 2: attacker.Emote( "*launches a series of short punches as {0} wades in, then elbows {1} in the side of {2} head*", aggressor.GetPersonalPronoun(), aggressed.GetPossessivePronoun() ); break;
                        case 3: attacker.Emote( "*switches {0} stance then fires a crippling side kick into {1}{2} thigh*", aggressor.GetPossessivePronoun(), defender.Name, aggressed.GetPossessive() ); break;
                        case 4: attacker.Emote( "*attacks with a powerful roundhouse kick to the side {0}{1} head*", defender.Name, aggressed.GetPossessive() ); break;
                        case 5: attacker.Emote( "*accepts {0}{1} blow to {2} abdomen, instead holding {3} torso down and kneeing {4} chest*", defender.Name, aggressed.GetPossessive(), aggressor.GetPossessivePronoun(), aggressed.GetPossessivePronoun(), aggressed.GetPossessivePronoun() ); break;
                        case 6: attacker.Emote( "*fires a pair of devastating crosses, then follows it up with a solid front kick to {0}{1} chest*", defender.Name, aggressed.GetPossessive() ); break;
                    }
                    break;
                }

                case 2:
                {
                    switch( number )
                    {
                        case 1: attacker.Emote( "*spins around, lashing out with {0} foot*", aggressor.GetPossessivePronoun() ); break;
                        case 2: attacker.Emote( "*steps into {0} and unleashes a high side kick at {1} chin*", defender.Name, aggressed.GetPossessivePronoun() ); break;
                        case 3: attacker.Emote( "*drops low and spins on {0} hands, sweeping {1} {2} legs*", aggressor.GetPossessivePronoun(), defender.Name, aggressor.GetPossessivePronoun() ); break;
                        case 4: attacker.Emote( "*tumbles backward onto {0} hands, unleashing a double upward kick*", aggressor.GetPossessivePronoun() ); break;
                        case 5: attacker.Emote( "*tumbles to the side onto one hand, spins around, swinging {0} heel swiftly at {1}*", aggressor.GetPossessivePronoun(), defender.Name ); break;
                        case 6: attacker.Emote( "*spins around, tripping {0} with a low kick*", defender.Name ); break;
                    }
                    break;
                }

                case 3:
                {
                    switch( number )
                    {
                        case 1: attacker.Emote( "*knees {0} in the groin*", defender.Name ); break;
                        case 2: attacker.Emote( "*launches a lumbering, devastatingly powerful hook*" ); break;
                        case 3: attacker.Emote( "*jerks forward, smashing {0} forehead into {1}{2} face*", aggressor.GetPossessivePronoun(), defender.Name, aggressed.GetPossessive() ); break;
                        case 4: attacker.Emote( "*attacks with a series of  furious blows {0}{1} lower abdomen*", defender.Name, aggressed.GetPossessive() ); break;
                        case 5: attacker.Emote( "*thrusts a forearm across {0}{1} face*", defender.Name, aggressed.GetPossessive() ); break;
                        case 6: attacker.Emote( "*fires a series of powerful, frantic jabs*" ); break;
                    }
                    break;
                }

                case 4:
                {
                    switch( number )
                    {
                        case 1: attacker.Emote( "*launches a series of straight, solid strikes to {0}{1} face and chest*", defender.Name, aggressed.GetPossessive() ); break;
                        case 2: attacker.Emote( "*widens {0} stance and pummels {1} amid a storm of circular feints*", aggressor.GetPossessivePronoun(), defender.Name ); break;
                        case 3: attacker.Emote( "*steps forward with one foot and launches a swift front kick at {0}{1} abdomen*", defender.Name, aggressed.GetPossessive() ); break;
                        case 4: attacker.Emote( "*attacks with a pair of quick jabs, then drops low, spinning into a sweep*" ); break;
                        case 5: attacker.Emote( "*attacks {0} with a hook, then reverses the momentum of the strike for a backhand blow*", defender.Name ); break;
                        case 6: attacker.Emote( "*kicks at {0}{1} head, then spins on that foot, firing a roundhouse kick with the other*", defender.Name, aggressed.GetPossessive() ); break;
                    }
                    break;
                }

                case 5:
                {
                    switch( number )
                    {
                        case 1: attacker.Emote( "*launches a series of light, calculated jabs*" ); break;
                        case 2: attacker.Emote( "*jabs {0} a few times, then follows it up with a solid cross*", defender.Name ); break;
                        case 3: attacker.Emote( "*blocks an attack with {0} curled arm and counters with a powerful hook*", aggressor.GetPossessivePronoun() ); break;
                        case 4: attacker.Emote( "*steadily paces into {0} and launches a series of furious blows to {1} abdomen*", defender.Name, aggressed.GetPossessivePronoun() ); break;
                        case 5: attacker.Emote( "*attacks with a cross, turns in to deliver a hook, then rotates his torso up for a brutal uppercut*" ); break;
                        case 6: attacker.Emote( "*launches a light jab at {0}{1} face, strikes at the abdomen, and then fires a hook*", defender.Name, aggressed.GetPossessive() ); break;
                    }
                    break;
                }

                case 6:
                {
                    switch( number )
                    {
                        case 1: attacker.Emote( "*launches a fast pair of punches at {0} and follows it with a front kick to the groin*", defender.Name ); break;
                        case 2: attacker.Emote( "*kicks {0} in the stomach, doubling {1} over, then elbows {2} in the back of the head*", defender.Name, aggressed.GetReflexivePronoun(), aggressed.GetReflexivePronoun() ); break;
                        case 3: attacker.Emote( "*sidesteps {0}{1} attack and elbows {2} in the side of the head*", defender.Name, aggressed.GetPossessive(), aggressed.GetReflexivePronoun() ); break;
                        case 4: attacker.Emote( "*grabs {0} by the shoulders, jerking {1} down and thrusting a knee into {2} chest*", defender.Name, aggressed.GetReflexivePronoun(), aggressed.GetPossessivePronoun() ); break;
                        case 5: attacker.Emote( "*leans around {0}{1} attack and thrusts an open palm into {1} face*", defender.Name, aggressed.GetPossessive(), aggressed.GetPossessivePronoun() ); break;
                        case 6: attacker.Emote( "*steps into {0} and launches an elbow at {1} face*", defender.Name, aggressed.GetPossessivePronoun() ); break;
                    }
                    break;
                }
            }
        }

		public virtual double GetAosDamage( Mobile attacker, int bonus, int dice, int sides )
		{
			int damage = Utility.Dice( dice, sides, bonus ) * 100;
			int damageBonus = 0;

			// Inscription bonus
			int inscribeSkill = attacker.Skills[SkillName.Inscribe].Fixed;

			damageBonus += inscribeSkill / 200;

			if ( inscribeSkill >= 1000 )
				damageBonus += 5;

			if ( attacker.Player )
			{
				// Int bonus
				damageBonus += (attacker.Int / 10);

				// SDI bonus
				damageBonus += AosAttributes.GetValue( attacker, AosAttribute.SpellDamage );
			}

			damage = AOS.Scale( damage, 100 + damageBonus );

			return damage / 100;
		}

		#region Do<AoSEffect>
		public virtual void DoMagicArrow( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 10, 1, 4 );

			attacker.MovingParticles( defender, 0x36E4, 5, 0, false, true, 3006, 4006, 0 );
			attacker.PlaySound( 0x1E5 );

			SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, damage, 0, 100, 0, 0, 0 );
		}

		public virtual void DoHarm( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 17, 1, 5 );

			if ( !defender.InRange( attacker, 2 ) )
				damage *= 0.25; // 1/4 damage at > 2 tile range
			else if ( !defender.InRange( attacker, 1 ) )
				damage *= 0.50; // 1/2 damage at 2 tile range

			defender.FixedParticles( 0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist );
			defender.PlaySound( 0x0FC );

			SpellHelper.Damage( TimeSpan.Zero, defender, attacker, damage, 0, 0, 100, 0, 0 );
		}

		public virtual void DoFireball( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 19, 1, 5 );

			attacker.MovingParticles( defender, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160 );
			attacker.PlaySound( 0x15E );

			SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, damage, 0, 100, 0, 0, 0 );
		}

		public virtual void DoLightning( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 23, 1, 4 );

			defender.BoltEffect( 0 );

			SpellHelper.Damage( TimeSpan.Zero, defender, attacker, damage, 0, 0, 0, 0, 100 );
		}

		public virtual void DoDispel( Mobile attacker, Mobile defender )
		{
			bool dispellable = false;

			if ( defender is BaseCreature )
				dispellable = ((BaseCreature)defender).Summoned && !((BaseCreature)defender).IsAnimatedDead;

			if ( !dispellable )
				return;

			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			Spells.Spell sp = new Spells.Sixth.DispelSpell( attacker, null );

			if ( sp.CheckResisted( defender ) )
			{
				defender.FixedEffect( 0x3779, 10, 20 );
			}
			else
			{
				Effects.SendLocationParticles( EffectItem.Create( defender.Location, defender.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
				Effects.PlaySound( defender, defender.Map, 0x201 );

				defender.Delete();
			}
		}

		public virtual void DoLowerAttack( Mobile from, Mobile defender )
		{
			if ( HitLower.ApplyAttack( defender ) )
			{
				defender.PlaySound( 0x28E );
				Effects.SendTargetEffect( defender, 0x37BE, 1, 4, 0xA, 3 );
			}
		}

		public virtual void DoLowerDefense( Mobile from, Mobile defender )
		{
			if ( HitLower.ApplyDefense( defender ) )
			{
				defender.PlaySound( 0x28E );
				Effects.SendTargetEffect( defender, 0x37BE, 1, 4, 0x23, 3 );
			}
		}

		public virtual void DoAreaAttack( Mobile from, Mobile defender, int sound, int hue, int phys, int fire, int cold, int pois, int nrgy, int blunt, int slax, int pirc )
		{
			Map map = from.Map;

			if ( map == null )
				return;

			List<Mobile> list = new List<Mobile>();

			foreach ( Mobile m in from.GetMobilesInRange( 10 ) )
			{
				if ( from != m && defender != m && SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false ) && from.InLOS( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
				return;

			Effects.PlaySound( from.Location, map, sound );

			// TODO: What is the damage calculation?

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = list[i];

				double scalar = (11 - from.GetDistanceToSqrt( m )) / 10;

				if ( scalar > 1.0 )
					scalar = 1.0;
				else if ( scalar < 0.0 )
					continue;

				from.DoHarmful( m, true );
				m.FixedEffect( 0x3779, 1, 15, hue, 0 );
				AOS.Damage( m, from, (int)(GetScaledDamage( from, null ) * scalar), phys, fire, cold, pois, nrgy, blunt, slax, pirc );
			}
		}
		#endregion

		public virtual CheckSlayerResult CheckSlayers( Mobile attacker, Mobile defender )
		{
			BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
			SlayerEntry atkSlayer = SlayerGroup.GetEntryByName( atkWeapon.Slayer );
			SlayerEntry atkSlayer2 = SlayerGroup.GetEntryByName( atkWeapon.Slayer2 );

			if ( atkSlayer != null && atkSlayer.Slays( defender )  || atkSlayer2 != null && atkSlayer2.Slays( defender ) )
				return CheckSlayerResult.Slayer;

			if ( !Core.SE )
			{
				ISlayer defISlayer = Spellbook.FindEquippedSpellbook( defender );

				if( defISlayer == null )
					defISlayer = defender.Weapon as ISlayer;

				if( defISlayer != null )
				{
					SlayerEntry defSlayer = SlayerGroup.GetEntryByName( defISlayer.Slayer );
					SlayerEntry defSlayer2 = SlayerGroup.GetEntryByName( defISlayer.Slayer2 );

					if( defSlayer != null && defSlayer.Group.OppositionSuperSlays( attacker ) || defSlayer2 != null && defSlayer2.Group.OppositionSuperSlays( attacker ) )
						return CheckSlayerResult.Opposition;
				}
			}

			return CheckSlayerResult.None;
		}

		public virtual void AddBlood( Mobile attacker, Mobile defender, int damage )
		{
			if ( damage > 0 && defender.BodyValue != 803 && defender.Hue != 12345678 )
			{
				new Blood().MoveToWorld( defender.Location, defender.Map );

				int extraBlood = (Core.SE ? Utility.RandomMinMax( 3, 4 ) : Utility.RandomMinMax( 0, 1 ) );

				for( int i = 0; i < extraBlood; i++ )
				{
					new Blood().MoveToWorld( new Point3D(
						defender.X + Utility.RandomMinMax( -1, 1 ),
						defender.Y + Utility.RandomMinMax( -1, 1 ),
						defender.Z ), defender.Map );
				}
			}

			/* if ( damage <= 2 )
				return;

			Direction d = defender.GetDirectionTo( attacker );

			int maxCount = damage / 15;

			if ( maxCount < 1 )
				maxCount = 1;
			else if ( maxCount > 4 )
				maxCount = 4;

			for( int i = 0; i < Utility.Random( 1, maxCount ); ++i )
			{
				int x = defender.X;
				int y = defender.Y;

				switch( d )
				{
					case Direction.North:
						x += Utility.Random( -1, 3 );
						y += Utility.Random( 2 );
						break;
					case Direction.East:
						y += Utility.Random( -1, 3 );
						x += Utility.Random( -1, 2 );
						break;
					case Direction.West:
						y += Utility.Random( -1, 3 );
						x += Utility.Random( 2 );
						break;
					case Direction.South:
						x += Utility.Random( -1, 3 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Up:
						x += Utility.Random( 2 );
						y += Utility.Random( 2 );
						break;
					case Direction.Down:
						x += Utility.Random( -1, 2 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Left:
						x += Utility.Random( 2 );
						y += Utility.Random( -1, 2 );
						break;
					case Direction.Right:
						x += Utility.Random( -1, 2 );
						y += Utility.Random( 2 );
						break;
				}

				new Blood().MoveToWorld( new Point3D( x, y, defender.Z ), defender.Map );
			}*/
		}

		public virtual void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int blut, out int slax, out int pirc )
		{
			if( wielder is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)wielder;

				phys = bc.PhysicalDamage;
				fire = bc.FireDamage;
				cold = bc.ColdDamage;
				pois = bc.PoisonDamage;
				nrgy = bc.EnergyDamage;
                blut = bc.BluntDamage;
                slax = bc.SlashingDamage;
                pirc = bc.PiercingDamage;
			}
			else
			{
				fire = m_AosElementDamages.Fire;
				cold = m_AosElementDamages.Cold;
				pois = m_AosElementDamages.Poison;
				nrgy = m_AosElementDamages.Energy;
                blut = m_AosElementDamages.Blunt;
                slax = m_AosElementDamages.Slashing;
                pirc = m_AosElementDamages.Piercing;

				phys = 100 - fire - cold - pois - nrgy - blut - slax - pirc;

				CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

				if( resInfo != null )
				{
					CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

					if( attrInfo != null )
					{
						int left = phys;

						left = ApplyCraftAttributeElementDamage( attrInfo.WeaponColdDamage,		ref cold, left );
						left = ApplyCraftAttributeElementDamage( attrInfo.WeaponEnergyDamage,	ref nrgy, left );
						left = ApplyCraftAttributeElementDamage( attrInfo.WeaponFireDamage,		ref fire, left );
						left = ApplyCraftAttributeElementDamage( attrInfo.WeaponPoisonDamage,	ref pois, left );
                        left = ApplyCraftAttributeElementDamage(attrInfo.WeaponBluntDamage, ref blut, left);
                        left = ApplyCraftAttributeElementDamage(attrInfo.WeaponSlashingDamage, ref slax, left);
                        left = ApplyCraftAttributeElementDamage(attrInfo.WeaponPiercingDamage, ref pirc, left);

						phys = left;
					}
				}
			}
		}

		private int ApplyCraftAttributeElementDamage( int attrDamage, ref int element, int totalRemaining )
		{
			if( totalRemaining <= 0 )
				return 0;

			if ( attrDamage <= 0 )
				return totalRemaining;

			int appliedDamage = attrDamage;

			if ( (appliedDamage + element) > 100 )
				appliedDamage = 100 - element;

			if( appliedDamage > totalRemaining )
				appliedDamage = totalRemaining;

			element += appliedDamage;

			return totalRemaining - appliedDamage;
		}

		public virtual void OnMiss( Mobile attacker, Mobile defender )
		{
			//PlaySwingAnimation( attacker );
			/*attacker.PlaySound( GetMissAttackSound( attacker, defender ) );
			defender.PlaySound( GetMissDefendSound( attacker, defender ) );*/
			CombatSystemAttachment.PlayMissSound( attacker, defender );

			WeaponAbility ability = WeaponAbility.GetCurrentAbility( attacker );

			if ( ability != null )
				ability.OnMiss( attacker, defender );

			SpecialMove move = SpecialMove.GetCurrentMove( attacker );

			if ( move != null )
				move.OnMiss( attacker, defender );

			if ( defender is IHonorTarget && ((IHonorTarget)defender).ReceivedHonorContext != null )
				((IHonorTarget)defender).ReceivedHonorContext.OnTargetMissed( attacker );
		}

		public virtual void GetBaseDamageRange( Mobile attacker, out int min, out int max )
		{
			min = max = 0;
			if ( attacker is BaseCreature && !(attacker is Mercenary) )
			{
				BaseCreature c = (BaseCreature)attacker;

				if ( c.DamageMin >= 0 )
				{
					min = c.DamageMin;
					max = c.DamageMax;
					return;
				}

				/*if ( this is Fists && !attacker.Body.IsHuman )
				{
					min = attacker.Str / 28;
					max = attacker.Str / 28;
					return;
				}*/
			}
			
			min = MinDamage;
			max = MaxDamage;
		}

		public virtual double GetScaledDamage( Mobile attacker, Mobile defender )
		{
			return GetScaledDamage( attacker, defender, 1.0 );
		}
		public virtual double GetScaledDamage( Mobile attacker, Mobile defender, double factor )
		{
			int min, max;
			GetBaseDamageRange( attacker, out min, out max );
            /*if (((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ThrowingMastery)
                factor = .75;*/
			min = (int)(factor*ScaleDamageAOS( attacker, defender, min, true, true )); 
			max = (int)(factor*ScaleDamageAOS( attacker, defender, max, true, false ));

			return Utility.RandomMinMax( min, max );
		}

		public virtual double GetBonus( double value, double scalar, double threshold, double offset )
		{
			double bonus = value * scalar;

			if ( bonus > 0 )
				bonus = bonus / threshold;

			return bonus * offset;
		}

		public virtual int GetHitChanceBonus()
		{
			if ( !Core.AOS )
				return 0;

			int bonus = 0;

			switch ( m_AccuracyLevel )
			{
				case WeaponAccuracyLevel.Accurate:		bonus += 02; break;
				case WeaponAccuracyLevel.Surpassingly:	bonus += 04; break;
				case WeaponAccuracyLevel.Eminently:		bonus += 06; break;
				case WeaponAccuracyLevel.Exceedingly:	bonus += 08; break;
				case WeaponAccuracyLevel.Supremely:		bonus += 10; break;
			}

            if (!NewCrafting)
            {
                if (Quality == WeaponQuality.Extraordinary)
                    bonus += this.QualityAccuracy * 5;

                else if (Quality == WeaponQuality.Masterwork)
                    bonus += this.QualityAccuracy * 10;
            }
            else if (!BetaNerf && NewCrafting)
            {
                this.QualityAccuracy /= 2;
                this.BetaNerf = true;
                bonus = this.QualityAccuracy * 2;
                InvalidateProperties();
            }
            else
                bonus = this.QualityAccuracy * 2;

			return bonus;
		}
		
		public virtual int GetSpeedBonus()
		{
            int bonus = 0;
            if (!NewCrafting)
            {
                bonus = GetResourceSpeed();
                bonus += GetQualitySpeed();
            }
            else if (!BetaNerf && NewCrafting)
            {                
                if (QualitySpeed > 20)
                    QualitySpeed = 20;
                bonus += QualitySpeed;
                BetaNerf = true;
                InvalidateProperties();
            }
            else
                bonus = QualitySpeed;

			return bonus;
		}

		public virtual int GetDamageBonus()
		{
			int bonus = VirtualDamageBonus;
            if (!NewCrafting)
            {
                bonus += GetResourceDamage();
                bonus += GetQualityDamage();
            }
            else if (!BetaNerf && NewCrafting)
            {                
                if (QualityDamage > 20)
                    QualityDamage = 20;
                bonus += QualityDamage;
                int i = GetSpeedBonus();
                QualityDefense /= 2;
                QualityAccuracy /= 2;
                BetaNerf = true;
                InvalidateProperties();
            }
            else
                bonus += QualityDamage;

			return bonus;
		}

		public virtual void GetStatusDamage( Mobile from, out int min, out int max )
		{
			GetBaseDamageRange( from, out min, out max );

			if ( Core.AOS )
			{
				min = Math.Max( (int)ScaleDamageAOS( from, null, min, true, true ), 1 ); 
				max = Math.Max( (int)ScaleDamageAOS( from, null, max, true, false ), 1 ); 
				
				//min = Math.Max( (int)ScaleDamageAOS( from, null, baseMin, false ), 1 );
				//max = Math.Max( (int)ScaleDamageAOS( from, null, baseMax, false ), 1 );
			}
			/*else
			{
				min = Math.Max( (int)ScaleDamageOld( from, baseMin, false ), 1 );
				max = Math.Max( (int)ScaleDamageOld( from, baseMax, false ), 1 );
			}*/
		}

        private AttackType GetHighestAttackType(BaseWeapon weapon)
        {
            AttackType result;

            double highestPercentage = weapon.ThrustPercentage;
            result = AttackType.Thrust;

            if (highestPercentage < weapon.SwingPercentage)
            {
                highestPercentage = weapon.SwingPercentage;
                result = AttackType.Swing;
            }
            if (highestPercentage < weapon.OverheadPercentage)
            {
                highestPercentage = weapon.OverheadPercentage;
                result = AttackType.Overhead;
            }

            return result;
        }

        private int GetAverageResistance(Mobile mob)
        {
            int result = mob.SlashingResistance;
                result += mob.PiercingResistance;
                result += mob.BluntResistance;

                return result / 3;
        }

        private double MaxChargeDamage(Mobile attacker, Mobile defender, double currentDamage)
        {
            PlayerMobile target = defender as PlayerMobile;
            if (target != null)
            {
                PlayerMobile charger = attacker as PlayerMobile;
                if (charger == null)
                    return currentDamage;

                CombatSystemAttachment chargerCSA = CombatSystemAttachment.GetCSA(attacker);
                if (!chargerCSA.Charging)
                    return currentDamage;

                int maxResist = this.GetAverageResistance(defender);
                int health = defender.HitsMax;

                if ((health - maxResist+20) < currentDamage)
                    return (health - maxResist) + 20;
                else
                    return currentDamage;
            }
            else
                return currentDamage;
        }

		public virtual double ScaleDamageAOS( Mobile attacker, Mobile defender, double damage, bool checkSkills, bool min )
		{
			IKhaerosMobile attk = attacker as IKhaerosMobile;
			bool shieldbash = ( attk.OffensiveFeat == FeatList.ShieldBash );
			if( shieldbash )
				damage = attk.Feats.GetFeatLevel(FeatList.ShieldBash);
			
			double strFactor = 1.0;
			double DamagePointWorth = 2.0;
			if ( !min ) // this is the maximum damage roll
				strFactor = 3.0; // strength gives 3x increased bonus for determining max damage
				
			//if ( this is BaseRanged || (attacker is BaseCreature && !(attacker is Mercenary)) )
				//strFactor *= 0.25; // str only gives a quarter the bonus to ranged weaponry
			
			double scalar = 1.0;
			if ( this is BaseRanged && !(attacker is BaseCreature) )
				scalar = 0.65;
			if( attacker is BaseCreature && !(attacker.BodyValue != 400 && attacker.BodyValue != 401) )
				scalar = 0.5; // they hit too low, but we don't want creatures with weapons getting this bonus
			
			if ( attacker != null && defender != null )
			{
				if( ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.Backstab )
					damage = 10.0 + (5.0*((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.Backstab));
				/*else if ( Critical && !(attacker.FindItemOnLayer( Layer.TwoHanded ) is BaseShield) ) // doesn't stack with backstab or throwing
				{
					if ( ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.ThrowingMastery && Utility.RandomBool() )
						damage *= 1.5; // double damage
				}*/ 
			}

            if( attacker is PlayerMobile && attacker.Weapon is Fists && ( (PlayerMobile)attacker ).Claws != null )
                scalar += 0.2;
                
        //This is an attempt to put slayers back in for red magic
CheckSlayerResult cs = CheckSlayers(attacker, defender);
	if (cs != CheckSlayerResult.None)
{
if (cs == CheckSlayerResult.Slayer)
defender.FixedEffect(0x37B9, 10, 5);
damage *= 1.5;

//factor *= 2.0;
}
            if (defender is PlayerMobile && ((PlayerMobile)defender).IsVampire && this.Resource == CraftResource.Silver)
            {
                damage *= 1.5; 

                ArrayList list = XmlAttach.FindAttachments(this, typeof(XmlAttachment));
				if (list != null)
				{
					for (int i = 0; i < list.Count; ++i)
					{
						XmlAttachment enhancement = list[i] as XmlAttachment;
						if (enhancement is XmlHolyWater)
							scalar += 1;
					}
				}
            }

			double effectiveStrength = attacker.Str;
			if( attacker.Weapon is Fists && ((IKhaerosMobile)attacker).CanUseMartialStance && !shieldbash)
            	effectiveStrength += ((0.2*((IKhaerosMobile)attacker ).Feats.GetFeatLevel(FeatList.MartialOffence))*attacker.Dex)/2;
				
			//double skillBonus = scalar*damage*((attacker.Skills[SkillName.Tactics].Base)*0.0045 + (attacker.Skills[SkillName.Anatomy].Base)*0.00225); // old value, testing new due to insufficient dmg
			double skillBonus = scalar*damage*((attacker.Skills[SkillName.Tactics].Base)*0.01 + (attacker.Skills[SkillName.Anatomy].Base)*0.005);			
			
			double strBonus = strFactor*scalar*damage*(effectiveStrength*0.0066*DamagePointWorth);
			//double strBonus = damage*(effectiveStrength*0.0033);
			double directScalar = scalar;
            if (this is BaseRanged)
                directScalar = 0.5;
			double directBonus = DamagePointWorth*directScalar*(AosAttributes.GetValue( attacker, AosAttribute.WeaponDamage ) + GetDamageBonuses( attacker, defender ));

			double percentageBonus = DamagePointWorth*scalar*GetDamageFactorBonus( attacker, defender )*damage;
			double damagePenalty = GetDamagePenalty( attacker, defender );
			if( shieldbash )
			{
				directBonus = 0;
				percentageBonus = 0;
				damagePenalty = 0;
			}

			double finalDamage = scalar*((1.0-damagePenalty)*(damage + strBonus + skillBonus + directBonus + percentageBonus));
			
			if( defender is PlayerMobile )
        	{
            	if( Utility.Random( 1, 100 ) == ((PlayerMobile)defender).GetBackgroundLevel(BackgroundList.Lucky) )
                	finalDamage = 0;
                
                if( Utility.Random( 1, 100 ) == ((PlayerMobile)defender).GetBackgroundLevel(BackgroundList.Unlucky) )
                	finalDamage *= 2.0;
            }
            
            if( attacker is PlayerMobile && defender != null ) // otherwise this stuff will show up on status
            {
            	if( Utility.Random( 1, 100 ) == ((PlayerMobile)attacker).GetBackgroundLevel(BackgroundList.Lucky) )
                	finalDamage *= 2.0;
                
                if( Utility.Random( 1, 100 ) == ((PlayerMobile)attacker).GetBackgroundLevel(BackgroundList.Unlucky) )
                	finalDamage = 0;
            }
			
			/*if( defender != null && defender is PlayerMobile && attacker is PlayerMobile )
				finalDamage *= 0.75;*/
			if (!((IKhaerosMobile)attacker).Stance.MartialArtistStance)
				finalDamage += finalDamage*((IKhaerosMobile)attacker).Stance.DamageBonus * (double)((IKhaerosMobile)attacker).Stance.FeatLevel;
			
			if ( attacker is Mercenary && ((BaseCreature)attacker).ControlMaster != null )
			{
                finalDamage = GetMercenaryDamage(attacker, defender, finalDamage);
			}

            finalDamage = ReducePetDamageAgainstPlayers(attacker, defender, finalDamage); 
            
            if (!min)
               finalDamage = this.MaxChargeDamage(attacker, defender, finalDamage);

			if ( finalDamage < 1.0 )
				finalDamage = 1.0;
			return finalDamage;
		}

        private double ReducePetDamageAgainstPlayers(Mobile attacker, Mobile defender, double finalDamage)
        {
            PlayerMobile target = defender as PlayerMobile;

            if (target != null)
            {
                if (attacker is BirdOfPrey)
                    finalDamage = finalDamage / 2;
                else if (attacker is Ogre || attacker is Wolf)
                    finalDamage = finalDamage / 1.5;
            }

            return finalDamage;
        }

        private double GetMercenaryDamage(Mobile attacker, Mobile defender, double finalDamage)
        {
            double divider;
            double factor = ((BaseCreature)attacker).ControlMaster.Skills[SkillName.Leadership].Value;

            if (factor == 0)
                factor = 1.0;

            PlayerMobile target = defender as PlayerMobile;

            if (target != null)
            {
                divider = 14.0; // PvP
            }
            else
            {
                divider = 10.0; // PvE
            }

            factor = factor / divider; //formerly factor = factor/100.0; was scaling down Mercenary damage to be non-existant.
            finalDamage *= factor;
            return finalDamage;

        }


		public virtual int VirtualDamageBonus{ get{ return 0; } }

		public virtual int ComputeDamageAOS( Mobile attacker, Mobile defender )
		{
			return (int)GetScaledDamage( attacker, defender );
		}

		public virtual double ScaleDamageOld( Mobile attacker, double damage, bool checkSkills )
		{
			if ( checkSkills )
			{
				attacker.CheckSkill( SkillName.Tactics, 0.0, 120.0 ); // Passively check tactics for gain
				attacker.CheckSkill( SkillName.Anatomy, 0.0, 120.0 ); // Passively check Anatomy for gain

				if ( Type == WeaponType.Axe )
					attacker.CheckSkill( SkillName.Lumberjacking, 0.0, 100.0 ); // Passively check Lumberjacking for gain
			}

			/* Compute tactics modifier
			 * :   0.0 = 50% loss
			 * :  50.0 = unchanged
			 * : 100.0 = 50% bonus
			 */
			double tacticsBonus = (attacker.Skills[SkillName.Tactics].Value - 50.0) / 100.0;

			/* Compute strength modifier
			 * : 1% bonus for every 5 strength
			 */
			double strBonus = 0; //(attacker.Str / 100.0) / 100.0;

			/* Compute anatomy modifier
			 * : 1% bonus for every 5 points of anatomy
			 * : +10% bonus at Grandmaster or higher
			 */
			double anatomyValue = attacker.Skills[SkillName.Anatomy].Value;
			double anatomyBonus = (anatomyValue / 5.0) / 100.0;

			if ( anatomyValue >= 100.0 )
				anatomyBonus += 0.1;

			/* Compute lumberjacking bonus
			 * : 1% bonus for every 5 points of lumberjacking
			 * : +10% bonus at Grandmaster or higher
			 */
			double lumberBonus;

			if ( Type == WeaponType.Axe )
			{
				double lumberValue = attacker.Skills[SkillName.Lumberjacking].Value;

				lumberBonus = (lumberValue / 5.0) / 100.0;

				if ( lumberValue >= 100.0 )
					lumberBonus += 0.1;
			}
			else
			{
				lumberBonus = 0.0;
			}

			// New quality bonus:
			double qualityBonus = ((int)m_Quality - 1) * 0.2;

			// Apply bonuses
			damage += (damage * tacticsBonus) + (damage * strBonus) + (damage * anatomyBonus) + (damage * lumberBonus) + (damage * qualityBonus) + ((damage * VirtualDamageBonus) / 100);

			// Old quality bonus:
#if false
			/* Apply quality offset
			 * : Low         : -4
			 * : Regular     :  0
			 * : Exceptional : +4
			 */
			damage += ((int)m_Quality - 1) * 4.0;
#endif

			/* Apply damage level offset
			 * : Regular : 0
			 * : Ruin    : 1
			 * : Might   : 3
			 * : Force   : 5
			 * : Power   : 7
			 * : Vanq    : 9
			 */
			if ( m_DamageLevel != WeaponDamageLevel.Regular )
				damage += (2.0 * (int)m_DamageLevel) - 1.0;

			// Halve the computed damage and return
			damage /= 2.0;

			return ScaleDamageByDurability( (int)damage );
		}

		public virtual int ScaleDamageByDurability( int damage )
		{
			int scale = 100;

			if ( m_MaxHits > 0 && m_Hits < m_MaxHits )
				scale = 50 + ((50 * m_Hits) / m_MaxHits);

			return AOS.Scale( damage, scale );
		}

		public virtual int ComputeDamage( Mobile attacker, Mobile defender )
		{
			if ( Core.AOS )
				return ComputeDamageAOS( attacker, defender );

			return 0; //(int)ScaleDamageOld( attacker, GetBaseDamage( attacker ), true );
		}

		public virtual void PlayHurtAnimation( Mobile from )
		{
			int action;
			int frames;

			switch ( from.Body.Type )
			{
				case BodyType.Sea:
				case BodyType.Animal:
				{
					action = 7;
					frames = 5;
					break;
				}
				case BodyType.Monster:
				{
					action = 10;
					frames = 4;
					break;
				}
				case BodyType.Human:
				{
					action = 20;
					frames = 5;
					break;
				}
				default: return;
			}

			if ( from.Mounted )
				return;

			from.Animate( action, frames, 1, true, false, 0 );
		}
		
		public void ThrowCallback()
        {
			try
			{
        		MoveToWorld( ThrowMob.Location, ThrowMob.Map );
			}
			
			catch
			{
				Console.WriteLine( "Error while throwing a weapon." );
			}
        }

		public virtual void PlaySwingAnimation( Mobile from )
		{
			/*int action;

			switch ( from.Body.Type )
			{
				case BodyType.Sea:
				case BodyType.Animal:
				{
					action = Utility.Random( 5, 2 );
					break;
				}
				case BodyType.Monster:
				{
					switch ( Animation )
					{
						default:
						case WeaponAnimation.Wrestle:
						case WeaponAnimation.Bash1H:
						case WeaponAnimation.Pierce1H:
						case WeaponAnimation.Slash1H:
						case WeaponAnimation.Bash2H:
						case WeaponAnimation.Pierce2H:
						case WeaponAnimation.Slash2H: action = Utility.Random( 4, 3 ); break;
						case WeaponAnimation.ShootBow:  return; // 7
						case WeaponAnimation.ShootXBow: return; // 8
					}

					break;
				}
				case BodyType.Human:
				{
					if ( !from.Mounted )
					{
						action = (int)Animation;
					}
					else
					{
						switch ( Animation )
						{
							default:
							case WeaponAnimation.Wrestle:
							case WeaponAnimation.Bash1H:
							case WeaponAnimation.Pierce1H:
							case WeaponAnimation.Slash1H: action = 26; break;
							case WeaponAnimation.Bash2H:
							case WeaponAnimation.Pierce2H:
							case WeaponAnimation.Slash2H: action = 29; break;
							case WeaponAnimation.ShootBow: action = 27; break;
							case WeaponAnimation.ShootXBow: action = 28; break;
						}
					}

					break;
				}
				default: return;
			}

			from.Animate( action, 7, 1, true, false, 0 );*/
		}

		#region Serialization/Deserialization
		private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
		{
			if ( setIf )
				flags |= toSet;
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( (flags & toGet) != 0 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 27 ); // version

            writer.Write( (bool) m_BetaNerf );
            writer.Write( (List<Item>) m_Components);
            writer.Write( (bool) m_NewCrafting);
            writer.Write( (int) m_QualityDefense);
			
			writer.Write( (bool) m_HasHalo );

			writer.Write( (int) m_QualityDamage );
			writer.Write( (int) m_QualitySpeed );
			writer.Write( (int) m_QualityAccuracy );
			
			writer.Write( (string) m_CraftersOriginalName );
			writer.Write( (string) m_Engraved1 );
			writer.Write( (string) m_Engraved2 );
			writer.Write( (string) m_Engraved3 );

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.DamageLevel,		m_DamageLevel != WeaponDamageLevel.Regular );
			SetSaveFlag( ref flags, SaveFlag.AccuracyLevel,		m_AccuracyLevel != WeaponAccuracyLevel.Regular );
			SetSaveFlag( ref flags, SaveFlag.DurabilityLevel,	m_DurabilityLevel != WeaponDurabilityLevel.Regular );
			SetSaveFlag( ref flags, SaveFlag.Quality,			m_Quality != WeaponQuality.Regular );
			SetSaveFlag( ref flags, SaveFlag.Hits,				m_Hits != 0 );
			SetSaveFlag( ref flags, SaveFlag.MaxHits,			m_MaxHits != 0 );
			SetSaveFlag( ref flags, SaveFlag.Slayer,			m_Slayer != SlayerName.None );
			SetSaveFlag( ref flags, SaveFlag.Poison,			m_Poison != null );
			SetSaveFlag( ref flags, SaveFlag.PoisonCharges,		m_PoisonCharges != 0 );
			SetSaveFlag( ref flags, SaveFlag.Crafter,			m_Crafter != null );
			SetSaveFlag( ref flags, SaveFlag.Identified,		m_Identified != false );
			SetSaveFlag( ref flags, SaveFlag.StrReq,			m_StrReq != -1 );
			SetSaveFlag( ref flags, SaveFlag.DexReq,			m_DexReq != -1 );
			SetSaveFlag( ref flags, SaveFlag.IntReq,			m_IntReq != -1 );
			SetSaveFlag( ref flags, SaveFlag.MinDamage,			m_MinDamage != -1 );
			SetSaveFlag( ref flags, SaveFlag.MaxDamage,			m_MaxDamage != -1 );
			SetSaveFlag( ref flags, SaveFlag.HitSound,			m_HitSound != -1 );
			SetSaveFlag( ref flags, SaveFlag.MissSound,			m_MissSound != -1 );
			SetSaveFlag( ref flags, SaveFlag.Speed,				m_Speed != -1 );
			SetSaveFlag( ref flags, SaveFlag.MaxRange,			m_MaxRange != -1 );
			SetSaveFlag( ref flags, SaveFlag.Skill,				m_Skill != (SkillName)(-1) );
			SetSaveFlag( ref flags, SaveFlag.Type,				m_Type != (WeaponType)(-1) );
			SetSaveFlag( ref flags, SaveFlag.Animation,			m_Animation != (WeaponAnimation)(-1) );
			SetSaveFlag( ref flags, SaveFlag.Resource,			m_Resource != CraftResource.Iron );
			SetSaveFlag( ref flags, SaveFlag.xAttributes,		!m_AosAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.xWeaponAttributes,	!m_AosWeaponAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.PlayerConstructed,	m_PlayerConstructed );
			SetSaveFlag( ref flags, SaveFlag.SkillBonuses,		!m_AosSkillBonuses.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Slayer2,			m_Slayer2 != SlayerName.None );
			SetSaveFlag( ref flags, SaveFlag.ElementalDamages,	!m_AosElementDamages.IsEmpty );

			writer.Write( (int) flags );

			if ( GetSaveFlag( flags, SaveFlag.DamageLevel ) )
				writer.Write( (int) m_DamageLevel );

			if ( GetSaveFlag( flags, SaveFlag.AccuracyLevel ) )
				writer.Write( (int) m_AccuracyLevel );

			if ( GetSaveFlag( flags, SaveFlag.DurabilityLevel ) )
				writer.Write( (int) m_DurabilityLevel );

			if ( GetSaveFlag( flags, SaveFlag.Quality ) )
				writer.Write( (int) m_Quality );

			if ( GetSaveFlag( flags, SaveFlag.Hits ) )
				writer.Write( (int) m_Hits );

			if ( GetSaveFlag( flags, SaveFlag.MaxHits ) )
				writer.Write( (int) m_MaxHits );

			if ( GetSaveFlag( flags, SaveFlag.Slayer ) )
				writer.Write( (int) m_Slayer );

			if ( GetSaveFlag( flags, SaveFlag.Poison ) )
				Poison.Serialize( m_Poison, writer );

			if ( GetSaveFlag( flags, SaveFlag.PoisonCharges ) )
				writer.Write( (int) m_PoisonCharges );

			if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
				writer.Write( (Mobile) m_Crafter );

			if ( GetSaveFlag( flags, SaveFlag.StrReq ) )
				writer.Write( (int) m_StrReq );

			if ( GetSaveFlag( flags, SaveFlag.DexReq ) )
				writer.Write( (int) m_DexReq );

			if ( GetSaveFlag( flags, SaveFlag.IntReq ) )
				writer.Write( (int) m_IntReq );

			if ( GetSaveFlag( flags, SaveFlag.MinDamage ) )
				writer.Write( (int) m_MinDamage );

			if ( GetSaveFlag( flags, SaveFlag.MaxDamage ) )
				writer.Write( (int) m_MaxDamage );

			if ( GetSaveFlag( flags, SaveFlag.HitSound ) )
				writer.Write( (int) m_HitSound );

			if ( GetSaveFlag( flags, SaveFlag.MissSound ) )
				writer.Write( (int) m_MissSound );

			if ( GetSaveFlag( flags, SaveFlag.Speed ) )
				writer.Write( (int) m_Speed );

			if ( GetSaveFlag( flags, SaveFlag.MaxRange ) )
				writer.Write( (int) m_MaxRange );

			if ( GetSaveFlag( flags, SaveFlag.Skill ) )
				writer.Write( (int) m_Skill );

			if ( GetSaveFlag( flags, SaveFlag.Type ) )
				writer.Write( (int) m_Type );

			if ( GetSaveFlag( flags, SaveFlag.Animation ) )
				writer.Write( (int) m_Animation );

			if ( GetSaveFlag( flags, SaveFlag.Resource ) )
				writer.Write( (int) m_Resource );

			if ( GetSaveFlag( flags, SaveFlag.xAttributes ) )
				m_AosAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.xWeaponAttributes ) )
				m_AosWeaponAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
				m_AosSkillBonuses.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Slayer2 ) )
				writer.Write( (int)m_Slayer2 );

			if( GetSaveFlag( flags, SaveFlag.ElementalDamages ) )
				m_AosElementDamages.Serialize( writer );
		}

		[Flags]
		private enum SaveFlag
		{
			None					= 0x00000000,
			DamageLevel				= 0x00000001,
			AccuracyLevel			= 0x00000002,
			DurabilityLevel			= 0x00000004,
			Quality					= 0x00000008,
			Hits					= 0x00000010,
			MaxHits					= 0x00000020,
			Slayer					= 0x00000040,
			Poison					= 0x00000080,
			PoisonCharges			= 0x00000100,
			Crafter					= 0x00000200,
			Identified				= 0x00000400,
			StrReq					= 0x00000800,
			DexReq					= 0x00001000,
			IntReq					= 0x00002000,
			MinDamage				= 0x00004000,
			MaxDamage				= 0x00008000,
			HitSound				= 0x00010000,
			MissSound				= 0x00020000,
			Speed					= 0x00040000,
			MaxRange				= 0x00080000,
			Skill					= 0x00100000,
			Type					= 0x00200000,
			Animation				= 0x00400000,
			Resource				= 0x00800000,
			xAttributes				= 0x01000000,
			xWeaponAttributes		= 0x02000000,
			PlayerConstructed		= 0x04000000,
			SkillBonuses			= 0x08000000,
			Slayer2					= 0x10000000,
			ElementalDamages		= 0x20000000
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 27:
                case 26: m_BetaNerf = reader.ReadBool(); goto case 25;
                case 25: m_Components = reader.ReadStrongItemList(); goto case 24;
                case 24: m_NewCrafting = reader.ReadBool(); m_QualityDefense = reader.ReadInt(); goto case 23;
				case 23: m_HasHalo = reader.ReadBool(); goto case 22;
				case 22:
				case 21:
				case 20:
				{
					m_QualityDamage = reader.ReadInt();
					m_QualitySpeed = reader.ReadInt();
					m_QualityAccuracy = reader.ReadInt();
					goto case 19;
				}
				case 19:
				case 18:
				case 17:
				case 16:
				case 15:
				case 14:
				case 13:
				case 12:
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
					SaveFlag flags = (SaveFlag)reader.ReadInt();

					if ( GetSaveFlag( flags, SaveFlag.DamageLevel ) )
					{
						m_DamageLevel = (WeaponDamageLevel)reader.ReadInt();

						if ( m_DamageLevel > WeaponDamageLevel.Vanq )
							m_DamageLevel = WeaponDamageLevel.Ruin;
					}

					if ( GetSaveFlag( flags, SaveFlag.AccuracyLevel ) )
					{
						m_AccuracyLevel = (WeaponAccuracyLevel)reader.ReadInt();

						if ( m_AccuracyLevel > WeaponAccuracyLevel.Supremely )
							m_AccuracyLevel = WeaponAccuracyLevel.Accurate;
					}

					if ( GetSaveFlag( flags, SaveFlag.DurabilityLevel ) )
					{
						m_DurabilityLevel = (WeaponDurabilityLevel)reader.ReadInt();

						if ( m_DurabilityLevel > WeaponDurabilityLevel.Indestructible )
							m_DurabilityLevel = WeaponDurabilityLevel.Durable;
					}

					if ( GetSaveFlag( flags, SaveFlag.Quality ) )
						m_Quality = (WeaponQuality)reader.ReadInt();
					else
						m_Quality = WeaponQuality.Regular;

					if ( GetSaveFlag( flags, SaveFlag.Hits ) )
						m_Hits = reader.ReadInt();

					if ( GetSaveFlag( flags, SaveFlag.MaxHits ) )
						m_MaxHits = reader.ReadInt();

					if ( GetSaveFlag( flags, SaveFlag.Slayer ) )
						m_Slayer = (SlayerName)reader.ReadInt();

					if ( GetSaveFlag( flags, SaveFlag.Poison ) )
						m_Poison = Poison.Deserialize( reader );

					if ( GetSaveFlag( flags, SaveFlag.PoisonCharges ) )
						m_PoisonCharges = reader.ReadInt();

					if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
						m_Crafter = reader.ReadMobile();

					if ( GetSaveFlag( flags, SaveFlag.Identified ) )
						m_Identified = ( version >= 6 || reader.ReadBool() );

					if ( GetSaveFlag( flags, SaveFlag.StrReq ) )
						m_StrReq = reader.ReadInt();
					else
						m_StrReq = -1;

					if ( GetSaveFlag( flags, SaveFlag.DexReq ) )
						m_DexReq = reader.ReadInt();
					else
						m_DexReq = -1;

					if ( GetSaveFlag( flags, SaveFlag.IntReq ) )
						m_IntReq = reader.ReadInt();
					else
						m_IntReq = -1;

					if ( GetSaveFlag( flags, SaveFlag.MinDamage ) )
						m_MinDamage = reader.ReadInt();
					else
						m_MinDamage = -1;

					if ( GetSaveFlag( flags, SaveFlag.MaxDamage ) )
						m_MaxDamage = reader.ReadInt();
					else
						m_MaxDamage = -1;

					if ( GetSaveFlag( flags, SaveFlag.HitSound ) )
						m_HitSound = reader.ReadInt();
					else
						m_HitSound = -1;

					if ( GetSaveFlag( flags, SaveFlag.MissSound ) )
						m_MissSound = reader.ReadInt();
					else
						m_MissSound = -1;

					if ( GetSaveFlag( flags, SaveFlag.Speed ) )
						m_Speed = reader.ReadInt();
					else
						m_Speed = -1;

					if ( GetSaveFlag( flags, SaveFlag.MaxRange ) )
						m_MaxRange = reader.ReadInt();
					else
						m_MaxRange = -1;

					if ( GetSaveFlag( flags, SaveFlag.Skill ) )
						m_Skill = (SkillName)reader.ReadInt();
					else
						m_Skill = (SkillName)(-1);

					if ( GetSaveFlag( flags, SaveFlag.Type ) )
						m_Type = (WeaponType)reader.ReadInt();
					else
						m_Type = (WeaponType)(-1);

					if ( GetSaveFlag( flags, SaveFlag.Animation ) )
						m_Animation = (WeaponAnimation)reader.ReadInt();
					else
						m_Animation = (WeaponAnimation)(-1);

					if ( GetSaveFlag( flags, SaveFlag.Resource ) )
						m_Resource = (CraftResource)reader.ReadInt();
					else
						m_Resource = CraftResource.Iron;

					if ( GetSaveFlag( flags, SaveFlag.xAttributes ) )
						m_AosAttributes = new AosAttributes( this, reader );
					else
						m_AosAttributes = new AosAttributes( this );

					if ( GetSaveFlag( flags, SaveFlag.xWeaponAttributes ) )
						m_AosWeaponAttributes = new AosWeaponAttributes( this, reader );
					else
						m_AosWeaponAttributes = new AosWeaponAttributes( this );

					if ( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular && Parent is Mobile )
					{
						m_SkillMod = new DefaultSkillMod( AccuracySkill, true, (int)m_AccuracyLevel * 5 );
						((Mobile)Parent).AddSkillMod( m_SkillMod );
					}

					if ( version < 7 && m_AosWeaponAttributes.MageWeapon != 0 )
						m_AosWeaponAttributes.MageWeapon = 30 - m_AosWeaponAttributes.MageWeapon;

					if ( Core.AOS && m_AosWeaponAttributes.MageWeapon != 0 && m_AosWeaponAttributes.MageWeapon != 30 && Parent is Mobile )
					{
						m_MageMod = new DefaultSkillMod( SkillName.Magery, true, -30 + m_AosWeaponAttributes.MageWeapon );
						((Mobile)Parent).AddSkillMod( m_MageMod );
					}

					if ( GetSaveFlag( flags, SaveFlag.PlayerConstructed ) )
						m_PlayerConstructed = true;

					if( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
						m_AosSkillBonuses = new AosSkillBonuses( this, reader );
					else
						m_AosSkillBonuses = new AosSkillBonuses( this );

					if( GetSaveFlag( flags, SaveFlag.Slayer2 ) )
						m_Slayer2 = (SlayerName)reader.ReadInt();

					if( GetSaveFlag( flags, SaveFlag.ElementalDamages ) )
						m_AosElementDamages = new AosElementAttributes( this, reader );
					else
						m_AosElementDamages = new AosElementAttributes( this );

					break;
				}
				case 4:
				{
					m_Slayer = (SlayerName)reader.ReadInt();

					goto case 3;
				}
				case 3:
				{
					m_StrReq = reader.ReadInt();
					m_DexReq = reader.ReadInt();
					m_IntReq = reader.ReadInt();

					goto case 2;
				}
				case 2:
				{
					m_Identified = reader.ReadBool();

					goto case 1;
				}
				case 1:
				{
					m_MaxRange = reader.ReadInt();

					goto case 0;
				}
				case 0:
				{
					if ( version == 0 )
						m_MaxRange = 1; // default

					if ( version < 5 )
					{
						m_Resource = CraftResource.Iron;
						m_AosAttributes = new AosAttributes( this );
						m_AosWeaponAttributes = new AosWeaponAttributes( this );
						m_AosElementDamages = new AosElementAttributes( this );
						m_AosSkillBonuses = new AosSkillBonuses( this );
					}

					m_MinDamage = reader.ReadInt();
					m_MaxDamage = reader.ReadInt();

					m_Speed = reader.ReadInt();

					m_HitSound = reader.ReadInt();
					m_MissSound = reader.ReadInt();

					m_Skill = (SkillName)reader.ReadInt();
					m_Type = (WeaponType)reader.ReadInt();
					m_Animation = (WeaponAnimation)reader.ReadInt();
					m_DamageLevel = (WeaponDamageLevel)reader.ReadInt();
					m_AccuracyLevel = (WeaponAccuracyLevel)reader.ReadInt();
					m_DurabilityLevel = (WeaponDurabilityLevel)reader.ReadInt();
					m_Quality = (WeaponQuality)reader.ReadInt();

					m_Crafter = reader.ReadMobile();

					m_Poison = Poison.Deserialize( reader );
					m_PoisonCharges = reader.ReadInt();

					if ( m_StrReq == OldStrengthReq )
						m_StrReq = -1;

					if ( m_DexReq == OldDexterityReq )
						m_DexReq = -1;

					if ( m_IntReq == OldIntelligenceReq )
						m_IntReq = -1;

					if ( m_MinDamage == OldMinDamage )
						m_MinDamage = -1;

					if ( m_MaxDamage == OldMaxDamage )
						m_MaxDamage = -1;

					if ( m_HitSound == OldHitSound )
						m_HitSound = -1;

					if ( m_MissSound == OldMissSound )
						m_MissSound = -1;

					if ( m_Speed == OldSpeed )
						m_Speed = -1;

					if ( m_MaxRange == OldMaxRange )
						m_MaxRange = -1;

					if ( m_Skill == OldSkill )
						m_Skill = (SkillName)(-1);

					if ( m_Type == OldType )
						m_Type = (WeaponType)(-1);

					if ( m_Animation == OldAnimation )
						m_Animation = (WeaponAnimation)(-1);

					if ( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular && Parent is Mobile )
					{
						m_SkillMod = new DefaultSkillMod( AccuracySkill, true, (int)m_AccuracyLevel * 5);
						((Mobile)Parent).AddSkillMod( m_SkillMod );
					}

					break;
				}
			}

			if ( Core.AOS && Parent is Mobile )
				m_AosSkillBonuses.AddTo( (Mobile)Parent );

			int strBonus = m_AosAttributes.BonusStr;
			int dexBonus = m_AosAttributes.BonusDex;
			int intBonus = m_AosAttributes.BonusInt;

			if ( this.Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
			{
				Mobile m = (Mobile)this.Parent;

				string modName = this.Serial.ToString();

				if ( strBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

				if ( dexBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

				if ( intBonus != 0 )
					m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
			}

			if ( Parent is Mobile )
				((Mobile)Parent).CheckStatTimers();

			if ( m_Hits <= 0 && m_MaxHits <= 0 )
			{
				m_Hits = m_MaxHits = Utility.RandomMinMax( InitMinHits, InitMaxHits );
			}

			if( version < 15 && this is IBoneArmour )
				this.Attributes.WeaponSpeed -= 4;
			
			if( version < 16 )
				FixResource();
			
			if( version < 17 )
			{
				this.m_Speed = -1;
				this.m_MinDamage = -1;
				this.m_MaxDamage = -1;
				
				if( this.Hue == 2413 )
					this.Resource = CraftResource.Copper;
				
				if( this.Hue == 2418 )
					this.Resource = CraftResource.Bronze;
					
				if( this.Hue == 2669 )
					this.Resource = CraftResource.Electrum;					
			}
			
			if( version < 18 )
				this.DurabilityLevel = WeaponDurabilityLevel.Regular;
			
			if( version < 19 )
			{
				if( this.HitPoints > 175 )
					this.HitPoints = 175;
				
				if( this.MaxHitPoints > 175 )
					this.MaxHitPoints = 175;
			}
			
			if( version < 20 )
				FixBonuses();
			
			if( version < 21 )
			{
				if( this is IBoneArmour )
					Hue = 0;
				
				else
					Hue = CraftResources.GetHue( this.Resource );
			}
			
			if( version < 22 )
			{
				Speed = -1;
				MinDamage = -1;
				MaxDamage = -1;
			}

            if ( version == 26 )
                BetaNerf = false;
		}
		#endregion

		private void FixBonuses()
		{
			int factor = 0;
			
			if( this.Quality == WeaponQuality.Extraordinary )
				factor = 1;
			
			if( this.Quality == WeaponQuality.Masterwork )
				factor = 2;
			
			if( factor > 0 )
				CheckForBonuses( factor, 0 );
		}
		
		private void CheckForBonuses( int factor, int found )
		{
			if( found > 2 )
			{
				this.Attributes.WeaponDamage = 0;
				this.Attributes.WeaponSpeed = 0;
				this.Attributes.AttackChance = 0;
				return;
			}
			
			if( this.Attributes.WeaponDamage > 0 )
			{
				this.Attributes.WeaponDamage -= 2 * factor;
				this.QualityDamage++;
			}
			
			else if( this.Attributes.WeaponSpeed > 0 )
			{
				this.Attributes.WeaponSpeed -= 2 * factor;
				this.QualitySpeed++;
			}
			
			else if( this.Attributes.AttackChance > 0 )
			{
				this.Attributes.AttackChance -= 5 * factor;
				this.QualityAccuracy++;
			}
			
			else if( found < 3 )
				DistributeBonuses(1);

			CheckForBonuses( factor, found + 1 );
		}
		
		public BaseWeapon( int itemID ) : base( itemID )
		{
			Layer = (Layer)ItemData.Quality;

			m_Quality = WeaponQuality.Regular;
			m_StrReq = -1;
			m_DexReq = -1;
			m_IntReq = -1;
			m_MinDamage = -1;
			m_MaxDamage = -1;
			m_HitSound = -1;
			m_MissSound = -1;
			m_Speed = -1;
			m_MaxRange = -1;
			m_Skill = (SkillName)(-1);
			m_Type = (WeaponType)(-1);
			m_Animation = (WeaponAnimation)(-1);

			m_Hits = m_MaxHits = Utility.RandomMinMax( InitMinHits, InitMaxHits );

            m_Components = new List<Item>();

			m_AosAttributes = new AosAttributes( this );
			m_AosWeaponAttributes = new AosWeaponAttributes( this );
			m_AosSkillBonuses = new AosSkillBonuses( this );
			m_AosElementDamages = new AosElementAttributes( this );
			
			if( this is BaseRanged || this is Boomerang ||
			    this is DruidStaff || this is ProphetDiviningRod || this is ClericCrook || 
			    this is QuarterStaff || this is GnarledStaff || this is SpikedClub || this is Club ||
			    this is BlackStaff )
				this.Resource = CraftResource.Oak;
			
			else
			{
				this.Resource = CraftResource.Iron;
				this.Hue = 0;
			}
			
			if( this is IBoneArmour )
				this.Hue = 0;
		}

		public BaseWeapon( Serial serial ) : base( serial )
		{
		}

		private string GetNameString()
		{
			string name = this.Name;

			if ( name == null )
				name = String.Format( "#{0}", LabelNumber );

			return name;
		}

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public override int Hue
		{
			get{ return base.Hue; }
			set{ base.Hue = value; InvalidateProperties(); }
		}

		public int GetElementalDamageHue()
		{
			int phys, fire, cold, pois, nrgy, blut, slax, pirc;
			GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy, out blut, out slax, out pirc );
			//Order is Cold, Energy, Fire, Poison, Physical left

			int currentMax = 50;
			int hue = 0;

			if( pois >= currentMax )
			{
				hue = 1267 + (pois - 50) / 10;
				currentMax = pois;
			}

			if( fire >= currentMax )
			{
				hue = 1255 + (fire - 50) / 10;
				currentMax = fire;
			}

			if( nrgy >= currentMax )
			{
				hue = 1273 + (nrgy - 50) / 10;
				currentMax = nrgy;
			}

			if( cold >= currentMax )
			{
				hue = 1261 + (cold - 50) / 10;
				currentMax = cold;
			}

            if ( blut >= currentMax )
            {
                hue = 1261 + ( blut - 50 ) / 10;
                currentMax = blut;
            }

            if( slax >= currentMax )
			{
				hue = 1261 + ( slax - 50 ) / 10;
				currentMax = slax;
			}

            if( pirc >= currentMax )
			{
				hue = 1261 + ( pirc - 50 ) / 10;
				currentMax = pirc;
			}

			return hue;
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			/*if ( oreType != 0 )
				list.Add( 1053099, "#{0}\t{1}", oreType, GetNameString() ); // ~1_oretype~ ~2_armortype~
			else */if ( Name == null )
				list.Add( LabelNumber );
			else
				list.Add( Name );
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( base.AllowEquipedCast( from ) )
				return true;

			return ( m_AosAttributes.SpellChanneling != 0 );
		}

		public virtual int ArtifactRarity
		{
			get{ return 0; }
		}

		public virtual int GetLuckBonus()
		{
			CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

			if ( resInfo == null )
				return 0;

			CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

			if ( attrInfo == null )
				return 0;

			return attrInfo.WeaponLuck;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			// MOD BEGIN
			OilAttachment attachment = XmlAttach.FindAttachment( this, typeof( OilAttachment ) ) as OilAttachment;
			if ( attachment != null )
			{
				int duration = attachment.Duration;
				
				int seconds = duration % 60;
				int minutes = ((duration-=seconds) / 60) % 60;
				int hours = ((duration-=minutes*60) / 3600) % 24;
				int days = ((duration-=hours*3600) / 86400);
			
				string timeLeft = (days > 0 ? days + "d " : "") + (hours > 0 ? hours + "h " : "") +
						(minutes > 0 ? minutes + "m " : "") + (seconds > 0 ? seconds + "s" : "" );
				
				list.Add( 1060660, "{0}\t{1}", "Oil Duration", timeLeft ); // ~1_val~: ~2_val~
			}
			// MOD END
			
			if( Consecrated )
				list.Add( 1043341 );
			
			if( HolyWaterPower > 0 )
				list.Add( 1060479 );
			
			int oreType;

			switch ( m_Resource )
			{
				case CraftResource.Copper:			oreType = 1053106; break; // copper
				case CraftResource.Bronze:			oreType = 1053105; break; // bronze
				case CraftResource.Iron:			oreType = 1062226; break; // iron
				case CraftResource.Gold:			oreType = 1053104; break; // golden
				case CraftResource.Silver:			oreType = 1053107; break; // agapite
				case CraftResource.Obsidian:		oreType = 1053103; break; // verite
				case CraftResource.Steel:	    	oreType = 1053102; break; // valorite
				case CraftResource.Tin:	    		oreType = 1053101; break; // valorite
				case CraftResource.Starmetal:	   	oreType = 1053108; break; // valorite
				case CraftResource.Electrum:	   	oreType = 1053110; break;
				case CraftResource.ThickLeather:	oreType = 1061118; break; // Thick
				case CraftResource.BeastLeather:	oreType = 1061117; break; // Beast
				case CraftResource.ScaledLeather:	oreType = 1061116; break; // Scaled
				case CraftResource.RedScales:		oreType = 1060814; break; // red
				case CraftResource.YellowScales:	oreType = 1060818; break; // yellow
				case CraftResource.BlackScales:		oreType = 1060820; break; // black
				case CraftResource.GreenScales:		oreType = 1060819; break; // green
				case CraftResource.WhiteScales:		oreType = 1060821; break; // white
				case CraftResource.BlueScales:		oreType = 1060815; break; // blue
				
				case CraftResource.Oak:				oreType = 1063511; break; // yellow
				case CraftResource.Yew:				oreType = 1063512; break; // black
				case CraftResource.Redwood:			oreType = 1063513; break; // green
				case CraftResource.Ash:				oreType = 1063514; break; // white
				case CraftResource.Greenheart:		oreType = 1063515; break; // blue
				default: oreType = 0; break;
			}
			
			if( this is IBoneArmour )
				oreType = 1023966;
			
			list.Add( oreType );
			
			// free slots:
			// 1041522 ~1~2~3
			// 1042971 ~NOTHING
			// 1049644 [~sumthin~]
			// 1050039 ~stuff~ ~stuff~
			// 1060752 <CENTER><COLORSHITS>~stuff</COLORSHITS>
			// 1062613 "~stuff"
			// 1063483 ~stuff~ ~stuff~
			// 1070722 ~NOTHING
			//if ( Critical ) // this and unwieldy cannot be both set, due to cliloc issues atm
				//list.Add( 1070722, "<BASEFONT COLOR=#009600>Improved critical<BASEFONT COLOR=#FFFFFF>" ); // ~NOTHIN
			if ( Unwieldy )
				list.Add( 1070722, "<BASEFONT COLOR=#960000>Unwieldy<BASEFONT COLOR=#FFFFFF>" ); // ~NOTHIN
			if ( CanThrustOnMount )
				list.Add( 1049644, "<BASEFONT COLOR=#009600>Can thrust on mount<BASEFONT COLOR=#FFFFFF>\t" ); // [~sumthin~]
			if ( CanUseDefensiveFormation )
				list.Add( 1050039, "<BASEFONT COLOR=#009600>Can use defensive formation\t<BASEFONT COLOR=#FFFFFF>" ); // ~stuff~ ~stuff~
			if ( CannotBlock )
				list.Add( 1041522, "<BASEFONT COLOR=#960000>\tCannot parry\t<BASEFONT COLOR=#FFFFFF>" ); // ~1~2~3
			if ( CannotUseOnMount )
				list.Add( 1063483, "<BASEFONT COLOR=#960000>Cannot use while mounted\t<BASEFONT COLOR=#FFFFFF>" ); // ~stuff~ ~stuff~
			else if ( CannotUseOnFoot )
				list.Add( 1063483, "<BASEFONT COLOR=#960000>Cannot use on foot\t<BASEFONT COLOR=#FFFFFF>" ); // ~stuff~ ~stuff~
				
			if ( ChargeOnly )
				list.Add( 1049644, "<BASEFONT COLOR=#960000>Charge only<BASEFONT COLOR=#FFFFFF>\t" ); // [~sumthin~]

			if ( m_CraftersOriginalName != null )
				list.Add( 1050043, m_CraftersOriginalName ); // crafted by ~1_NAME~

			#region Factions
			if ( m_FactionState != null )
				list.Add( 1041350 ); // faction item
			#endregion

			if ( m_AosSkillBonuses != null )
				m_AosSkillBonuses.GetProperties( list );

            if (m_Quality == WeaponQuality.Poor)
                list.Add(1060659, "Quality\tPoor");

            if (m_Quality == WeaponQuality.Low)
                list.Add(1060659, "Quality\tLow");

            if (m_Quality == WeaponQuality.Inferior)
                list.Add(1060659, "Quality\tInferior");

            if (m_Quality == WeaponQuality.Superior)
                list.Add(1060659, "Quality\tExceptional");

            if (m_Quality == WeaponQuality.Exceptional)
                list.Add(1060659, "Quality\tExceptional");

			if ( m_Quality == WeaponQuality.Remarkable )
				list.Add( 1060659, "Quality\tRemarkable" );

            if (m_Quality == WeaponQuality.Antique)
                list.Add(1060659, "Quality\tAntique");
			
			if ( m_Quality == WeaponQuality.Masterwork )
				list.Add( 1060659, "Quality\tMasterwork" );
			
			if ( m_Quality == WeaponQuality.Extraordinary )
				list.Add( 1060659, "Quality\tExtraordinary" );

            if (m_Quality == WeaponQuality.Legendary)
                list.Add(1060659, "Quality\tLegendary");

			if( RequiredRace == Race.Elf )
				list.Add( 1075086 ); // Elves Only

			if ( ArtifactRarity > 0 )
				list.Add( 1061078, ArtifactRarity.ToString() ); // artifact rarity ~1_val~

			if ( this is IUsesRemaining && ((IUsesRemaining)this).ShowUsesRemaining )
				list.Add( 1060584, ((IUsesRemaining)this).UsesRemaining.ToString() ); // uses remaining: ~1_val~

			if ( m_Poison != null && m_PoisonCharges > 0 )
				list.Add( 1062412 + m_Poison.Level, m_PoisonCharges.ToString() );

			if( m_Slayer != SlayerName.None )
			{
				SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer );
				if( entry != null )
					list.Add( entry.Title );
			}

			if( m_Slayer2 != SlayerName.None )
			{
				SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer2 );
				if( entry != null )
					list.Add( entry.Title );
			}

			//base.AddResistanceProperties( list );
			
			
			
			int v = PhysicalResistance;

			if ( v != 0 )
				list.Add( 1060448, v.ToString() ); // physical resist ~1_val~%

			v = FireResistance;

			if ( v != 0 )
				list.Add( 1060447, v.ToString() ); // fire resist ~1_val~%

			v = ColdResistance;

			if ( v != 0 )
				list.Add( 1060445, v.ToString() ); // cold resist ~1_val~%

			v = PoisonResistance;

			if ( v != 0 )
				list.Add( 1060449, v.ToString() ); // poison resist ~1_val~%

			v = EnergyResistance;

			if ( v != 0 )
				list.Add( 1060446, v.ToString() ); // energy resist ~1_val~%

            v = BluntResistance;

			if ( v != 0 )
				list.Add( 1060526, v.ToString() ); // cold resist ~1_val~%
           
            v = SlashingResistance;

			if ( v != 0 )
				list.Add( 1060527, v.ToString() ); // cold resist ~1_val~%

            v = PiercingResistance;

			if ( v != 0 )
				list.Add( 1060528, v.ToString() ); // cold resist ~1_val~%
			
			

			int prop;

			if ( (prop = m_AosWeaponAttributes.UseBestSkill) != 0 )
				list.Add( 1060400 ); // use best weapon skill

			if ( (prop = (GetDamageBonus() + m_AosAttributes.WeaponDamage)) != 0 )
			{
				list.Add( 1060658, "Damage Increase\t+{0}", prop );
				//list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%
			}
			
			if ( (prop = (GetSpeedBonus() + m_AosAttributes.WeaponSpeed)) != 0 )
				list.Add( 1060661, "Speed Increase\t+{0}", prop );
				//list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

            if (!(this is BaseRanged))
            {
                if (!NewCrafting)
                    prop = GetHitChanceBonus() / 2 + m_AosAttributes.DefendChance;
                else
                    prop = QualityDefense + m_AosAttributes.DefendChance;
            }
            else
                prop = 0;
				if ( prop != 0 )
					list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( (prop = m_AosAttributes.EnhancePotions) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( (prop = m_AosAttributes.CastRecovery) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( (prop = m_AosAttributes.CastSpeed) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

            if (!NewCrafting)
                prop = (GetHitChanceBonus() / (this is BaseRanged ? 1 : 2) + m_AosAttributes.AttackChance);
            else
                prop = QualityAccuracy;
			if ( prop != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitColdArea) != 0 )
				list.Add( 1060416, prop.ToString() ); // hit cold area ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitDispel) != 0 )
				list.Add( 1060417, prop.ToString() ); // hit dispel ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitEnergyArea) != 0 )
				list.Add( 1060418, prop.ToString() ); // hit energy area ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitFireArea) != 0 )
				list.Add( 1060419, prop.ToString() ); // hit fire area ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitFireball) != 0 )
				list.Add( 1060420, prop.ToString() ); // hit fireball ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitHarm) != 0 )
				list.Add( 1060421, prop.ToString() ); // hit harm ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLeechHits) != 0 )
				list.Add( 1060422, prop.ToString() ); // hit life leech ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLightning) != 0 )
				list.Add( 1060423, prop.ToString() ); // hit lightning ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLowerAttack) != 0 )
				list.Add( 1060424, prop.ToString() ); // hit lower attack ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLowerDefend) != 0 )
				list.Add( 1060425, prop.ToString() ); // hit lower defense ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitMagicArrow) != 0 )
				list.Add( 1060426, prop.ToString() ); // hit magic arrow ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLeechMana) != 0 )
				list.Add( 1060427, prop.ToString() ); // hit mana leech ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitPhysicalArea) != 0 )
				list.Add( 1060428, prop.ToString() ); // hit physical area ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitPoisonArea) != 0 )
				list.Add( 1060429, prop.ToString() ); // hit poison area ~1_val~%

			if ( (prop = m_AosWeaponAttributes.HitLeechStam) != 0 )
				list.Add( 1060430, prop.ToString() ); // hit stamina leech ~1_val~%

			if ( (prop = m_AosAttributes.BonusDex) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( (prop = m_AosAttributes.BonusHits) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( (prop = m_AosAttributes.BonusInt) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( (prop = m_AosAttributes.LowerManaCost) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( (prop = m_AosAttributes.LowerRegCost) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( (prop = GetLowerStatReq()) != 0 )
				list.Add( 1060435, prop.ToString() ); // lower requirements ~1_val~%

			if ( (prop = (GetLuckBonus() + m_AosAttributes.Luck)) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( (prop = m_AosWeaponAttributes.MageWeapon) != 0 )
				list.Add( 1060438, (30 - prop).ToString() ); // mage weapon -~1_val~ skill

			if ( (prop = m_AosAttributes.BonusMana) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( (prop = m_AosAttributes.RegenMana) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( (prop = m_AosAttributes.NightSight) != 0 )
				list.Add( 1060441 ); // night sight

			if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( (prop = m_AosAttributes.RegenStam) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( (prop = m_AosAttributes.RegenHits) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( (prop = m_AosWeaponAttributes.SelfRepair) != 0 )
				list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

			if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( (prop = m_AosAttributes.SpellDamage) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusStam) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( (prop = m_AosAttributes.BonusStr) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			int phys, fire, cold, pois, nrgy, blut, slax, pirc;

			GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy, out blut, out slax, out pirc );

			if ( phys != 0 )
				list.Add( 1060403, phys.ToString() ); // physical damage ~1_val~%

			if ( fire != 0 )
				list.Add( 1060405, fire.ToString() ); // fire damage ~1_val~%

			if ( cold != 0 )
				list.Add( 1060404, cold.ToString() ); // cold damage ~1_val~%

			if ( pois != 0 )
				list.Add( 1060406, pois.ToString() ); // poison damage ~1_val~%

			if ( nrgy != 0 )
				list.Add( 1060407, nrgy.ToString() ); // energy damage ~1_val~%

            if ( blut != 0 )
				list.Add( 1060394, blut.ToString() );

            if ( slax != 0 )
				list.Add( 1060395, slax.ToString() );

            if ( pirc != 0 )
				list.Add( 1060396, pirc.ToString() );

			list.Add( 1061168, "{0}\t{1}", MinDamage.ToString(), MaxDamage.ToString() ); // weapon damage ~1_val~ - ~2_val~
			list.Add( 1061167, Speed.ToString() + "s" ); // weapon speed ~1_val~
			
			if ( OverheadPercentage > 0 )
				list.Add( 1060847, "{0}\t{1}", "Overhead:", ((int)(OverheadPercentage*100)).ToString() + "%" ); // ~1_val~ ~2_val~
			if ( SwingPercentage > 0 )
				list.Add( 1060663, "{0}\t{1}", "Swing", ((int)(SwingPercentage*100)).ToString() + "%" ); // ~1_val~: ~2_val~
			if ( ThrustPercentage > 0 )
				list.Add( 1060662, "{0}\t{1}", "Thrust", ((int)(ThrustPercentage*100)).ToString() + "%" ); // ~1_val~: ~2_val~

			if ( MaxRange > 1 )
				list.Add( 1061169, MaxRange.ToString() ); // range ~1_val~

			int strReq = AOS.Scale( StrRequirement, 100 - GetLowerStatReq() );

			if ( strReq > 0 )
				list.Add( 1061170, strReq.ToString() ); // strength requirement ~1_val~

			if ( Layer == Layer.TwoHanded )
				list.Add( 1061171 ); // two-handed weapon
			else
				list.Add( 1061824 ); // one-handed weapon

			if ( Core.SE || m_AosWeaponAttributes.UseBestSkill == 0 )
			{
				switch ( Skill )
				{
					case SkillName.Swords:  list.Add( 1061172 ); break; // skill required: swordsmanship
					case SkillName.Macing:  list.Add( 1061173 ); break; // skill required: mace fighting
					case SkillName.Fencing: list.Add( 1061174 ); break; // skill required: fencing
					case SkillName.Archery: list.Add( 1061175 ); break; // skill required: archery
					case SkillName.Polearms: list.Add( 1061182 ); break; // skill required: archery
					case SkillName.ExoticWeaponry: list.Add( 1061183 ); break; // skill required: archery
					case SkillName.Axemanship: list.Add( 1061184 ); break; // skill required: archery

				}
			}

			if ( m_Hits >= 0 && m_MaxHits > 0 )
				list.Add( 1060639, "{0}\t{1}", m_Hits, m_MaxHits ); // durability ~1_val~ / ~2_val~
		}

		public override void OnSingleClick( Mobile from )
		{
			List<EquipInfoAttribute> attrs = new List<EquipInfoAttribute>();

			if ( DisplayLootType )
			{
				if ( LootType == LootType.Blessed )
					attrs.Add( new EquipInfoAttribute( 1038021 ) ); // blessed
				else if ( LootType == LootType.Cursed )
					attrs.Add( new EquipInfoAttribute( 1049643 ) ); // cursed
			}

			#region Factions
			if ( m_FactionState != null )
				attrs.Add( new EquipInfoAttribute( 1041350 ) ); // faction item
			#endregion
			
			//if ( m_Quality == WeaponQuality.Masterwork )
				//attrs.Add( new EquipInfoAttribute( 1063532 ) );
			
			//else if ( m_Quality == WeaponQuality.Exceptional )
				//attrs.Add( new EquipInfoAttribute( 1018303 ) );

			//else if ( m_Quality == WeaponQuality.Exceptional )
				//attrs.Add( new ( 1018305 - (int)m_Quality ) );

			if ( m_Identified || from.AccessLevel >= AccessLevel.GameMaster )
			{
				if( m_Slayer != SlayerName.None )
				{
					SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer );
					if( entry != null )
						attrs.Add( new EquipInfoAttribute( entry.Title ) );
				}

				if( m_Slayer2 != SlayerName.None )
				{
					SlayerEntry entry = SlayerGroup.GetEntryByName( m_Slayer2 );
					if( entry != null )
						attrs.Add( new EquipInfoAttribute( entry.Title ) );
				}

				if ( m_DurabilityLevel != WeaponDurabilityLevel.Regular )
					attrs.Add( new EquipInfoAttribute( 1038000 + (int)m_DurabilityLevel ) );

				if ( m_DamageLevel != WeaponDamageLevel.Regular )
					attrs.Add( new EquipInfoAttribute( 1038015 + (int)m_DamageLevel ) );

				if ( m_AccuracyLevel != WeaponAccuracyLevel.Regular )
					attrs.Add( new EquipInfoAttribute( 1038010 + (int)m_AccuracyLevel ) );
			}
			else if( m_Slayer != SlayerName.None || m_Slayer2 != SlayerName.None || m_DurabilityLevel != WeaponDurabilityLevel.Regular || m_DamageLevel != WeaponDamageLevel.Regular || m_AccuracyLevel != WeaponAccuracyLevel.Regular )
				attrs.Add( new EquipInfoAttribute( 1038000 ) ); // Unidentified

			if ( m_Poison != null && m_PoisonCharges > 0 )
				attrs.Add( new EquipInfoAttribute( 1017383, m_PoisonCharges ) );

			int number;

			if ( Name == null )
			{
				number = LabelNumber;
			}
			else
			{
				this.LabelTo( from, Name );
				number = 1041000;
			}

			if ( attrs.Count == 0 && Crafter == null && Name != null )
				return;

			EquipmentInfo eqInfo = new EquipmentInfo( number, m_Crafter, false, attrs.ToArray() );

			from.Send( new DisplayEquipmentInfo( this, eqInfo ) );
		}

		private static BaseWeapon m_Fists; // This value holds the default--fist--weapon

		public static BaseWeapon Fists
		{
			get{ return m_Fists; }
			set{ m_Fists = value; }
		}

		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Quality = (WeaponQuality)quality;
			
			PlayerMobile m = from as PlayerMobile;

			if ( makersMark )
			{
				Crafter = from;
				CraftersOriginalName = from.Name;
			}

			PlayerConstructed = true;

			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			if ( from is PlayerMobile )
			{
				CraftResource thisResource = CraftResources.GetFromType( resourceType );
				
				if( !( this is IBoneArmour ) && thisResource == CraftResource.Obsidian && from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Obsidian) < 3 )
				{
					from.SendMessage( 60, "You do not know how to use obsidian and have ruined your craft." );
					this.Delete();
				}
				
				if( !( this is IBoneArmour ) && thisResource == CraftResource.Steel && from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Steel) < 3 )
				{
					from.SendMessage( 60, "You do not know how to use steel and have ruined your craft." );
					this.Delete();
				}
				
				if( !( this is IBoneArmour ) && thisResource == CraftResource.Greenheart && from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Greenheart) < 3 )
				{
					from.SendMessage( 60, "You do not know how to use greenheart and have ruined your craft." );
					this.Delete();
				}
				
				if( ( this is IBoneArmour && m.Feats.GetFeatLevel(FeatList.Bone) < 3 ) )
				{
					from.SendMessage( 60, "You do not know how to use bone and have ruined your craft." );
					this.Delete();
				}

				Resource = thisResource;
				
				CraftContext context = craftSystem.GetContext( from );

				if( ( context != null && context.DoNotColor ) || this is IBoneArmour )
					Hue = 0;
			}
			
			bool masterwork = false;
			
			if ( quality == 5 )
			{
				bool MWpieces = false;
				bool EOpieces = false;
				
				if( m.Masterwork.HasWeaponPieces && Resource == m.Masterwork.WeaponResource )
				{
					if( m.Masterwork.MasterworkWeapon )
						MWpieces = true;
					
					else
						EOpieces = true;
					
					m.Masterwork.HasWeaponPieces = false;
				}
				
				if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 2 && !EOpieces )
				{
					double chance = 0.002 + GetCraftingSpecBonus( m );
					double roll = Utility.RandomDouble();
					
					if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
					
					if( MWpieces || chance >= roll )
					{
						Quality = WeaponQuality.Masterwork;
						MasterworkBonuses( m );
						m.SendMessage( 60, "You have created a masterwork item." );
						masterwork = true;
						m.Crafting = true;
						LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), 500 ) );
						LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), 100 ) );
						m.Crafting = false;

                        if( m.Backpack != null )
                            ( (BaseContainer)m.Backpack ).DropAndStack( new RewardToken( 2 ) );
					}
				}
				
				if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 0 && !masterwork )
				{
					double chance = GetCraftingSpecBonus( m ) * 5;
					double roll = Utility.RandomDouble();
						
					if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 1 )
						chance += 0.002;
					
					else if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 2 )
						chance += 0.005;
					
					else chance += 0.01;
					
					if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
					
					if( EOpieces || chance >= roll )
					{
						this.Quality = WeaponQuality.Extraordinary;
						MasterworkBonuses( m );
						m.SendMessage( 60, "You have created an extraordinary item." );
						m.Crafting = true;
						LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), 250 ) );
						LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), 50 ) );
						m.Crafting = false;

                        if( m.Backpack != null && Utility.RandomBool() )
                            ( (BaseContainer)m.Backpack ).DropAndStack( new RewardToken() );
					}
				}
			}
			
			MaxHitPoints += ( MaxHitPoints / 10 ) * ( m.Feats.GetFeatLevel(FeatList.DurableCrafts) * 2 );
			HitPoints = MaxHitPoints;
			InvalidateProperties();

			return quality;
		}
		
		public double GetCraftingSpecBonus( PlayerMobile m )
		{
			double bonus = 0.0;
			
			if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) > 0 && !String.IsNullOrEmpty(m.CraftingSpecialization) )
			{
				string skill = "none";
				
				if( Resource == CraftResource.Copper || Resource == CraftResource.Iron || Resource == CraftResource.Bronze || 
				   Resource == CraftResource.Silver || Resource == CraftResource.Gold || Resource == CraftResource.Steel ||
				   Resource == CraftResource.Obsidian || Resource == CraftResource.Starmetal || Resource == CraftResource.Electrum )
					skill = "Blacksmithing";
				
				else if( (this is Boomerang || !(this is BaseRanged)) )
					skill = "Carpentry";
				
				else
					skill = "Fletching";

				if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 1 )
					bonus = 0.0004;
				
				if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 2 )
					bonus = 0.001;
				
				else
					bonus = 0.002;
				
				if( m.CraftingSpecialization != skill )
					bonus = -0.001;
			}
			
			return bonus;
		}
		
		private void MasterworkBonuses( PlayerMobile m )
		{
			int amount = 3 - (m.Masterwork.WeaponDamage + m.Masterwork.WeaponSpeed + m.Masterwork.WeaponAccuracy);
			
			if( amount > 0 )
				DistributeBonuses( amount );
			
			this.QualityDamage += m.Masterwork.WeaponDamage;
			this.QualitySpeed += m.Masterwork.WeaponSpeed;
			this.QualityAccuracy += m.Masterwork.WeaponAccuracy; 
		}
		
		public void DistributeBonuses( int amount )
		{
			for ( int i = 0; i < amount; ++i )
			{
				switch ( Utility.RandomMinMax( 1, 3 ) )
				{
					case 1: this.QualityDamage++; break;
					case 2: this.QualitySpeed++; break;
					case 3: this.QualityAccuracy++; break;
				}
			}
		}

		#endregion
	}

	public enum CheckSlayerResult
	{
		None,
		Slayer,
		Opposition
	}

    
}
