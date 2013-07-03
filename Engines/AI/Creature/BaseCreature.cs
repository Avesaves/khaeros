using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Regions;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Engines.Quests;
using Server.Factions;
using Server.Spells.Bushido;
using Server.Gumps;
using Server.Engines.XmlSpawner2;
using Server.Engines.Poisoning;
using Server.Misc.ImprovedAI;
using System.Reflection;
using Server.Multis;

namespace Server.Mobiles
{
	#region Enums
	/// <summary>
	/// Summary description for MobileAI.
	/// </summary>
	///

    public enum XMLEventType
    {
        None,
        OnActionWander,
        OnActionCombat,
        OnActionGuard,
        OnActionFlee,
        OnActionBackoff,
        OnAfterMove,
        OnDeath,
        OnDeathInvokeOnItem,
        OnDeathInvokeOnMobile,
        OnCarve,
        OnCarveInvokeOnItem,
        OnCarveInvokeOnMobile,
        OnDamage,
        OnDamageInvokeOnMobile,
        OnDoubleClick,
        OnDoubleClickInvokeOnMobile,
        OnSingleClick,
        OnSingleClickInvokeOnMobile,
        OnMoveOver,
        OnMoveOverInvokeOnMobile,
        OnDragDrop,
        OnDragDropInvokeOnMobile,
        OnDragDropInvokeOnItem,

        OnCustomBreathAttack,
        OnCustomBreathAttackInvokeOnItem,

        OnTargetedBySpell,
        OnTargetedBySpellInvokeOnMobile,
        OnTargetedBySpellHit,
        OnTargetedBySpellHitInvokeOnMobile,
        OnTargetedBySpellMissed,
        OnTargetedBySpellMissedInvokeOnMobile,

        OnAttack,
        OnAttackInvokeOnMobile,
        OnAttackHit,
        OnAttackHitInvokeOnMobile,
        OnAttackMissed,
        OnAttackMissedInvokeOnMobile,

        OnRangedAttack,
        OnRangedAttackInvokeOnMobile,
        OnRangedAttackHit,
        OnRangedAttackHitInvokeOnMobile,
        OnRangedAttackMissed,
        OnRangedAttackMissedInvokeOnMobile,

        OnMeleeAttack,
        OnMeleeAttackInvokeOnMobile,
        OnMeleeAttackHit,
        OnMeleeAttackHitInvokeOnMobile,
        OnMeleeAttackMissed,
        OnMeleeAttackMissedInvokeOnMobile,

        OnAOEAttack,
        OnAOEAttackInvokeOnMobile,
        OnAOEAttackHit,
        OnAOEAttackHitInvokeOnMobile,
        OnAOEAttackMissed,
        OnAOEAttackMissedInvokeOnMobile,

        OnGotAttacked,
        OnGotAttackedInvokeOnMobile,
        OnGotAttackedHit,
        OnGotAttackedHitInvokeOnMobile,
        OnGotAttackedMissed,
        OnGotAttackedMissedInvokeOnMobile,

        OnGotRangedAttacked,
        OnGotRangedAttackedInvokeOnMobile,
        OnGotRangedAttackedHit,
        OnGotRangedAttackedHitInvokeOnMobile,
        OnGotRangedAttackedMissed,
        OnGotRangedAttackedMissedInvokeOnMobile,

        OnGotMeleeAttacked,
        OnGotMeleeAttackedInvokeOnMobile,
        OnGotMeleeAttackedHit,
        OnGotMeleeAttackedHitInvokeOnMobile,
        OnGotMeleeAttackedMissed,
        OnGotMeleeAttackedMissedInvokeOnMobile,
    }

	public enum FightMode
	{
		None,			// Never focus on others
		Aggressor,		// Only attack aggressors
		Strongest,		// Attack the strongest
		Weakest,		// Attack the weakest
		Closest, 		// Attack the closest
		Evil,			// Only attack aggressor -or- negative karma
		FindThreat,		// Targets biggest potential threat (used by animals to know when to run away)
        Berserk         // Really attack the closest, ignoring agressors and agressed
	}

	public enum OrderType
	{
		None,			//When no order, let's roam
		Come,			//"(All/Name) come"  Summons all or one pet to your location.  
		Drop,			//"(Name) drop"  Drops its loot to the ground (if it carries any).  
		Follow,			//"(Name) follow"  Follows targeted being.  
						//"(All/Name) follow me"  Makes all or one pet follow you.  
		Friend,			//"(Name) friend"  Allows targeted player to confirm resurrection. 
		Unfriend,		// Remove a friend
		Guard,			//"(Name) guard"  Makes the specified pet guard you. Pets can only guard their owner. 
						//"(All/Name) guard me"  Makes all or one pet guard you.  
		Attack,			//"(All/Name) kill", 
						//"(All/Name) attack"  All or the specified pet(s) currently under your control attack the target. 
		Patrol,			//"(Name) patrol"  Roves between two or more guarded targets.  
		Release,		//"(Name) release"  Releases pet back into the wild (removes "tame" status). 
		Stay,			//"(All/Name) stay" All or the specified pet(s) will stop and stay in current spot. 
		Stop,			//"(All/Name) stop Cancels any current orders to attack, guard or follow.  
		Transfer		//"(Name) transfer" Transfers complete ownership to targeted player. 
	}

	[Flags]
	public enum FoodType
	{
		Meat			= 0x0001,
		FruitsAndVegies	= 0x0002,
		GrainsAndHay	= 0x0004,
		Fish			= 0x0008,
		Eggs			= 0x0010,
		Gold			= 0x0020
	}

	[Flags]
	public enum PackInstinct
	{
		None			= 0x0000,
		Canine			= 0x0001,
		Ostard			= 0x0002,
		Feline			= 0x0004,
		Arachnid		= 0x0008,
		Daemon			= 0x0010,
		Bear			= 0x0020,
		Equine			= 0x0040,
		Bull			= 0x0080
	}

    public enum MeleeAttackType
    {
        Normal,
        FrontalAOE,
        FullAOE,
        TemporaryGrapple,
        PermanentGrapple
    }

    public enum RangedAttackType
    {
        None,
        BreathePoison,
        BreatheFire,
        BreatheCold,
        BreatheEnergy
    }

    public enum CustomBreathType
    {
        Fire,
        Goo
    }

    public enum AuraType
    {
        None,
        Decay
    }
	
	public enum CreatureGroup
	{
		None,
		Brigand,
		Minotaur,
		Beastman,
		Goblin,
		Ogre,
		Undead,
		YuanTi,
		Giant,
		Draconic,
		Canine,
		Serpent,
		Rodent,
		Goatman,
		Troll,
		Troglin,
		Spider,
		Drider,
		Formian,
		Kobold,
		Beholder,
		Feline,
		Bear,
		Abyssal,
		Elemental,
		Reptile,
		GiantBug,
		Celestial,
		Brotherhood,
		Society,
		Insularii
	}

	public enum ScaleType
	{
		Red,
		Yellow,
		Black,
		Green,
		White,
		Blue,
		All
	}

	public enum MeatType
	{
		Ribs,
		Bird,
		LambLeg
	}

	public enum HideType
	{
		Regular,
		Thick,
		Beast,
		Scaled
	}

	#endregion
	
	#region Interfaces

    public interface ITinyPet
    {
    }

	public interface IRacialMount
	{
	}
	
	public interface IMagicalForestCreature
	{
	}
	
	public interface IJungleCreature
	{
	}
	
	public interface IForestCreature
	{
	}
	
	public interface IDesertCreature
	{
	}
	
	public interface ITundraCreature
	{
	}
	
	public interface IPlainsCreature
	{
	}
	
	public interface ICaveCreature
	{
	}
	
	public interface ISmallPrey
	{
	}
	
	public interface IMediumPrey
	{
	}
	
	public interface ILargePrey
	{
	}
	
	public interface ISmallPredator
	{
	}
	
	public interface IMediumPredator
	{
	}
	
	public interface ILargePredator
	{
	}
	
	public interface IAlwaysHungry
	{
	}
	
	public interface IRacialGuard
	{
	}
	
	public interface IMhordulFavoredEnemy
	{
	}
	
	public interface IAzhuranFavoredEnemy
	{
	}
	
	public interface IPeacefulPredator
	{
	}
	
	public interface IHasReach
	{
	}
	
	public interface IBrigand
	{
	}
	
	public interface IOgre
	{
	}
	
	public interface IGoblin
	{
	}
	
	public interface IIncorporeal
	{
	}

	public interface IHuge
	{
	}

	public interface ITooSmart
	{
	}

	public interface IUndead
	{
	}
	
	public interface IMinotaur
	{
	}
	
	public interface IBeastman
	{
	}
	
	public interface IEnraged
	{
	}
	
	public interface IElemental
	{
	}
	
	public interface IGiant
	{
	}
	
	public interface IYuanTi
	{
	}
	
	public interface IDraconic
	{
	}
	
	public interface ICanine
	{
	}
	
	public interface ISerpent
	{
	}
	
	public interface IGoatman
	{
	}
	
	public interface ITroll
	{
	}
	
	public interface ITroglin
	{
	}
	
	public interface ISpider
	{
	}
	
	public interface IDrider
	{
	}
	
	public interface IFormian
	{
	}
	
	public interface IKobold
	{
	}
	
	public interface IBeholder
	{
	}
	
	public interface IFeline
	{
	}
	
	public interface IBear
	{
	}
	
	public interface IAbyssal
	{
	}
	
	public interface ICelestial
	{
	}
	
	public interface IRodent
	{
	}
	
	public interface IReptile
	{
	}
	
	public interface IGiantBug
	{
	}
	
	public interface IFaction
	{
	}
	
	public interface IAlyrian
	{
	}
	
	public interface IAzhuran
	{
	}
	
	public interface IKhemetar
	{
	}
	
	public interface IMhordul
	{
	}
	
	public interface ITyrean
	{
	}
	
	public interface IVhalurian
	{
	}
	
	public interface IImperial
	{
	}
	
	public interface IClericSummon
	{
	}
	
	public interface IInsularii
	{
	}
	
	public interface IKhaerosMobile
	{
		Mobile ShieldingMobile{ get; set; }
		int RideBonus{ get; }
		int Intimidated{ get; set; }
		int Level{ get; set; }
		int RageFeatLevel{ get; set; }
		int ManeuverDamageBonus{ get; set; }
		int ManeuverAccuracyBonus{ get; set; }
		int TechniqueLevel{ get; set; }
		double ShieldValue{ get; set; }
		void RemoveShieldOfSacrifice();
		void DisableManeuver();
		string GetPersonalPronoun();
		string GetReflexivePronoun();
		string GetPossessivePronoun();
		string GetPossessive();
		string Technique{ get; set; }
		bool Fizzled{ get; set; }
		bool Enthralled{ get; set; }
		bool CanUseMartialPower{ get; }
		bool CanUseMartialStance{ get; }
		bool CleaveAttack{ get; set; }
		bool CanDodge{ get; }
		bool IsAllyOf( Mobile mob );
		bool Evaded();
		bool Dodged();
		bool Snatched();
		bool DeflectedProjectile();
		bool IsTired();
		bool IsTired( bool message );
		bool CanSummon();
		Feats Feats{ get; set; }
		CombatStyles CombatStyles{ get; set; }
		DateTime NextFeatUse{ get; set; }
		FeatList OffensiveFeat{ get; set; }
		BaseCombatManeuver CombatManeuver{ get; set; }
		FeatList CurrentSpell{ get; set; }
		Timer CrippledTimer{ get; set; }
		Timer DazedTimer{ get; set; }
		Timer TrippedTimer{ get; set; }
		Timer StunnedTimer{ get; set; }
		Timer DismountedTimer{ get; set; }
		Timer BlindnessTimer{ get; set; }
		Timer DeafnessTimer{ get; set; }
		Timer MutenessTimer{ get; set; }
		Timer DisabledLegsTimer{ get; set; }
		Timer DisabledLeftArmTimer{ get; set; }
		Timer DisabledRightArmTimer{ get; set; }
		Timer FeintTimer{ get; set; }
		Timer HealingTimer{ get; set; }
		Timer AuraOfProtection{ get; set; }
        Timer JusticeAura { get; set; }
		Timer Sanctuary{ get; set; }
		Timer RageTimer{ get; set; }
		Timer ManeuverBonusTimer{ get; set; }
		Timer FreezeTimer{ get; set; }
		BaseStance Stance{ get; set; }
		Mobile ShieldedMobile{ get; set; }
		DateTime NextRage{ get; set; }
		bool Deserialized{ get; set; }
	}
	
	#endregion

	public class DamageStore : IComparable
	{
		public Mobile m_Mobile;
		public int m_Damage;
		public bool m_HasRight;

		public DamageStore( Mobile m, int damage )
		{
			m_Mobile = m;
			m_Damage = damage;
		}

		public int CompareTo( object obj )
		{
			DamageStore ds = (DamageStore)obj;

			return ds.m_Damage - m_Damage;
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class FriendlyNameAttribute : Attribute
	{
		//future use: Talisman 'Protection/Bonus vs. Specific Creature
		private TextDefinition m_FriendlyName;

		public TextDefinition FriendlyName
		{
			get
			{
				return m_FriendlyName;
			}
		}

		public FriendlyNameAttribute( TextDefinition friendlyName )
		{
			m_FriendlyName = friendlyName;
		}

		public static TextDefinition GetFriendlyNameFor( Type t )
		{
			if( t.IsDefined( typeof( FriendlyNameAttribute ), false ) )
			{
				object[] objs = t.GetCustomAttributes( typeof( FriendlyNameAttribute ), false );

				if( objs != null && objs.Length > 0 )
				{
					FriendlyNameAttribute friendly = objs[0] as FriendlyNameAttribute;

					return friendly.FriendlyName;
				}
			}

			return t.Name;
		}
	}

	public class BaseCreature : Mobile, IHonorTarget, IKhaerosMobile
	{
        public virtual int MiniatureID { get { return 0x2D9C; } }
		public virtual bool ProtectedFromTermination{ get{ return ProtectedHourFromTermination; } }
		public const int MaxLoyalty = 100;
		public virtual bool ProtectedHourFromTermination
		{
			get
			{
				if( DateTime.Compare( (ReleaseTime + TimeSpan.FromHours(1)), DateTime.Now ) > 0 )
					return true;
				
				return false;
			}
		}

        

        public virtual bool HoldSmartSpawning
        {
            get
            {
                if (CanRummageCorpses)
                    return true;

                if (this is BaseKhaerosMobile || this.GetType().IsSubclassOf(typeof(BaseKhaerosMobile)) )
                    return true;

                return false;
            }
        }

		#region Var declarations
		private BaseAI	m_AI;					// THE AI
		
		private AIType	m_CurrentAI;			// The current AI
		private AIType	m_DefaultAI;			// The default AI

		private Mobile	m_FocusMob;				// Use focus mob instead of combatant, maybe we don't whan to fight
		private FightMode m_FightMode;			// The style the mob uses

		private int		m_iRangePerception;		// The view area
		private int		m_iRangeFight;			// The fight distance
       
		private bool	m_bDebugAI;				// Show debug AI messages

		private int		m_iTeam;				// Monster Team

		private double	m_dActiveSpeed;			// Timer speed when active
		private double	m_dPassiveSpeed;		// Timer speed when not active
		private double	m_dCurrentSpeed;		// The current speed, lets say it could be changed by something;

		private Point3D m_pHome;				// The home position of the creature, used by some AI
		private int		m_iRangeHome = 10;		// The home range of the creature

		List<Type>		m_arSpellAttack;		// List of attack spell/power
		List<Type>		m_arSpellDefense;		// List of defensive spell/power

		private bool		m_bControlled;		// Is controlled
		private Mobile		m_ControlMaster;	// My master
		private Mobile		m_ControlTarget;	// My target mobile
		private Point3D		m_ControlDest;		// My target destination (patrol)
		private OrderType	m_ControlOrder;		// My order

		private int			m_Loyalty;

		private double	m_dMinTameSkill;
		private bool	m_bTamable;

		private bool		m_bSummoned = false;
		private DateTime	m_SummonEnd;
		private int			m_iControlSlots = 1;

		private bool		m_bBardProvoked = false;
		private bool		m_bBardPacified = false;
		private Mobile		m_bBardMaster = null;
		private Mobile		m_bBardTarget = null;
		private DateTime	m_timeBardEnd;
		private WayPoint	m_CurrentWayPoint = null;
		private Point2D		m_TargetLocation = Point2D.Zero;

		private Mobile		m_SummonMaster;

		//private int			m_HitsMax = -1;
		//private	int			m_StamMax = -1;
		//private int			m_ManaMax = -1;
		private int			m_DamageMin = -1;
		private int			m_DamageMax = -1;

		private int			m_PhysicalResistance, m_PhysicalDamage;
		private int			m_FireResistance, m_FireDamage;
		private int			m_ColdResistance, m_ColdDamage;
		private int			m_PoisonResistance, m_PoisonDamage;
		private int			m_EnergyResistance, m_EnergyDamage;
		private int			m_BluntResistance, m_BluntDamage;
		private int			m_SlashingResistance, m_SlashingDamage;
		private int			m_PiercingResistance, m_PiercingDamage;

		private List<Mobile> m_Owners;
		private List<Mobile> m_Friends;

		private bool		m_IsStabled;

		private bool		m_HasGeneratedLoot; // have we generated our loot yet?

		private bool		m_Paragon;

        private bool m_CanBeInformant;

        private DateTime m_LastSeen;

        private string m_TargetsName;

        private Mobile m_Employer;

        private DateTime m_HiringTime;

        private bool m_Bribed;

        private int m_EmployerFeatLevel;

        private DateTime m_BribingTime;

        private DateTime m_WarningTime;

        private Nation m_Nation;
        private GovernmentEntity m_Government;
        private CustomGuildStone m_Organization;

        private bool m_Warned;

        private int m_XPScale;
        private int m_StatScale;
        private int m_SkillScale;
        private int m_Level;
        private int m_XP;
        private int m_NextLevel;
        private bool m_IsHuntingHound;
        private int m_Intimidated;

        public Timer m_TrippedTimer;
        public Timer m_BlindnessTimer;
        public Timer m_DeafnessTimer;
        public Timer m_MutenessTimer;
        public Timer m_DisabledLegsTimer;
        public Timer m_DisabledLeftArmTimer;
        public Timer m_DisabledRightArmTimer;
        public Timer m_FeintTimer;
        public Timer m_PetrifiedTimer;
        private Timer m_FreezeTimer;
        public Timer FreezeTimer{ get{ return m_FreezeTimer; } set{ m_FreezeTimer = value; } }
        public Timer m_HealingTimer;
        private Mobile m_ShieldingMobile;
        private double m_ShieldValue;
        private Timer m_AuraOfProtection;
        private Timer m_JusticeAura;
        public Timer m_SummoningTimer;
        private Timer m_ManeuverBonusTimer;
        private int m_ManeuverDamageBonus;
        private int m_ManeuverAccuracyBonus;
        
        private string m_Description;
        
        private bool m_IsSneaky;
        
        private bool m_TakesLifeOnKill;
        
        private CreatureGroup m_CreatureGroup;
        
        private DateTime m_VanishTime;
        private string m_VanishEmote = "*vanishes back to whence it came*";
        private string m_NoDeathMsg;
        private bool m_NoDeath;
        private int m_NoDeathSound;
        private bool m_NoDeathCondition;
        private CombatStyles m_CombatStyles;
        private Feats m_Feats;
        private DateTime m_NextFeatUse;
        private FeatList m_OffensiveFeat;
        private BaseCombatManeuver m_CombatManeuver;
        private Timer m_CrippledTimer;
        private Timer m_DazedTimer;
        private Timer m_StunnedTimer;
        private Timer m_DismountedTimer;
        private bool m_CleaveAttack;
        private bool m_Fizzled;
        private BaseStance m_Stance;
        private FeatList m_CurrentSpell;
        private Mobile m_ShieldedMobile;
        private Timer m_Sanctuary;
        private int m_RageFeatLevel;
        private Timer m_RageTimer;
        private bool m_FixedScales;
        private BaseStance m_SetStance;
        private BaseCombatManeuver m_SetManeuver;
        private string m_FavouriteStance;
        private string m_FavouriteManeuver;
        private bool m_MarkedForTermination;
        private DateTime m_ReleaseTime;
        private DateTime m_NextRage;

		public virtual int Height{ get{ return 0; } }
		
		private bool m_HadFirstThought;
		private bool m_ReceivedNewLoot;
		
		private string m_Technique;
		private int m_TechniqueLevel;
		
		private Mobile m_StabledOwner;
		public Mobile StabledOwner{ get{ return m_StabledOwner; } set{ m_StabledOwner = value; } }
		
		private Item m_StableTicket;
		public Item StableTicket{ get{ return m_StableTicket; } set{ m_StableTicket = value; } }
		
		private bool m_Deserialized;
		public bool Deserialized{ get{ return m_Deserialized; } set{ m_Deserialized = value; } }

        private Dictionary<XMLEventType, List<string>> m_XMLEventsDatabase = new Dictionary<XMLEventType, List<string>>();
        public Dictionary<XMLEventType, List<string>> XMLEventsDatabase { get { return m_XMLEventsDatabase; } set { m_XMLEventsDatabase = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public string AddXMLEvent
        {
            get { return "XMLEventType Code"; }

            set
            {
                string error = null;

                if( String.IsNullOrEmpty( value ) )
                    error = "Error: string was empty.";

                else
                {
                    string[] args = new string[]{};

                    try
                    {
                        args = value.Split( new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries );
                    }
                    catch( Exception e )
                    {
                        error = "Error: " + e.Message;
                    }

                    if( args.Length != 2 )
                        error = "Error: string must have two arguments.";
                    else
                    {
                        XMLEventType type = XMLEventType.None;

                        try
                        {
                            type = (XMLEventType)Enum.Parse( typeof( XMLEventType ), args[0].Trim(), true );
                        }
                        catch( Exception e )
                        {
                            error = "Error: " + e.Message;
                        }

                        if( type != XMLEventType.None )
                        {
                            try
                            {
                                if( !XMLEventsDatabase.ContainsKey( type ) )
                                    XMLEventsDatabase.Add( type, new List<string>() );

                                XMLEventsDatabase[type].Add( args[1] );
                            }
                            catch( Exception e )
                            {
                                error = "Error: " + e.Message;
                            }
                        }
                    }
                }

                if( !String.IsNullOrEmpty( error ) )
                    Say( error );
            }
        }

        private MeleeAttackType m_MeleeAttackType;
        [CommandProperty( AccessLevel.GameMaster )]
        public MeleeAttackType MeleeAttackType { get { return m_MeleeAttackType; } set { m_MeleeAttackType = value; } }

        private RangedAttackType m_RangedAttackType;
        [CommandProperty( AccessLevel.GameMaster )]
        public RangedAttackType RangedAttackType { get { return m_RangedAttackType; } set { m_RangedAttackType = value; } }

        private CustomBreathType m_CustomBreathType;
        [CommandProperty( AccessLevel.GameMaster )]
        public CustomBreathType CustomBreathType { get { return m_CustomBreathType; } set { m_CustomBreathType = value; } }

        private AuraType m_AuraType;
        [CommandProperty( AccessLevel.GameMaster )]
        public AuraType AuraType { get { return m_AuraType; } set { m_AuraType = value; } }

        private Timer m_AuraTimer;
        public Timer AuraTimer { get { return m_AuraTimer; } set { m_AuraTimer = value; } }

        private bool m_NoWoundedMovePenalty;
        [CommandProperty( AccessLevel.GameMaster )]
        public bool NoWoundedMovePenalty 
        { 
            get 
            {
                if( AI == AIType.AI_Berserk )
                    return true;
                return m_NoWoundedMovePenalty; 
            } 
            set { m_NoWoundedMovePenalty = value; } 
        }

        private bool m_CantBeInterrupted;
        [CommandProperty( AccessLevel.GameMaster )]
        public bool CantBeInterrupted
        {
            get
            {
                if( AI == AIType.AI_Berserk )
                    return true;
                return m_CantBeInterrupted;
            }
            set { m_CantBeInterrupted = value; }
        }

        private bool m_CantParry;
        [CommandProperty( AccessLevel.GameMaster )]
        public bool CantParry
        {
            get { return m_CantParry; }
            set { m_CantParry = value; }
        }

        private bool m_CantInterrupt;
        [CommandProperty( AccessLevel.GameMaster )]
        public bool CantInterrupt
        {
            get { return m_CantInterrupt; }
            set { m_CantInterrupt = value; }
        }

        private bool m_HasNoCorpse;
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasNoCorpse
        {
            get { return m_HasNoCorpse; }
            set { m_HasNoCorpse = value; }
        }

        private int m_ManeuverResistance;
        [CommandProperty( AccessLevel.GameMaster )]
        public int ManeuverResistance
        {
            get { return m_ManeuverResistance; }
            set { m_ManeuverResistance = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string RemoveAllMy
        {
            get { return "ItemType"; }
            set 
            {
                if( !String.IsNullOrEmpty( value ) )
                {
                    Type t = ScriptCompiler.FindTypeByName( value, true );

                    if( t != null && Backpack != null )
                    {
                        List<Item> list = new List<Item>();

                        for( int i = 0; i < Backpack.Items.Count; i++ )
                        {
                            Item item = Backpack.Items[i];

                            if( t.IsAssignableFrom( item.GetType() ) )
                                list.Add( item );
                        }

                        for( int i = 0; i < list.Count; i++ )
                        {
                            Item item = list[i];
                            item.Delete();
                        }

                        return;
                    }
                }
                
                Say( "Type not found." );
            }
        }

        private int m_Deadly;
        [CommandProperty(AccessLevel.GameMaster)]
        public int Deadly 
        { 
            get 
            { 
                return m_Deadly; 
            } 
            set 
            { 
                m_Deadly = value; 
                if (m_Deadly < 0) 
                    m_Deadly = 0; 
                if (m_Deadly > 100) 
                    m_Deadly = 100;
            } 
        }

        private List<OrderInfo> m_Orders = new List<OrderInfo>();
        public List<OrderInfo> Orders { get { return m_Orders; } set { Console.WriteLine("Beginning to edit m_Orders; OrderCount = " + m_Orders.Count.ToString()); m_Orders = value; Console.WriteLine("Orders altered; OrderCount = " + m_Orders.Count.ToString()); } }

		#endregion
		
		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Warmode
		{
			get
			{
				return base.Warmode;
			}
			set
			{
				bool temp = Warmode;
				if ( temp != value && !value ) // we're trying to leave warmode
				{
					if ( CombatSystemAttachment.GetCSA( this ).PerformingSequence ) // can't leave if tripped
						return;
				}
				base.Warmode = value;
				if ( temp != value ) // no change
				{
                    if( Warmode )
                    {
                        CombatSystemAttachment.GetCSA( this ).OnEnteredWarMode();
                        ActivateAura();
                    }
                    else
                    {
                        Combatant = null;
                        CombatSystemAttachment.GetCSA( this ).OnLeftWarMode();
                    }
				}
			}
		}

        public virtual void ActivateAura()
        {
            if( AuraType == AuraType.Decay && ( AuraTimer == null || !AuraTimer.Running ) )
            {
                AuraTimer = new AuraOfDecayTimer( this );
                AuraTimer.Start();
            }                
        }
		
		public override void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			Animate( action, frameCount, repeatCount, forward, repeat, delay, true );
		}
		public virtual void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay, bool external )
		{
			if ( external && CombatSystemAttachment.GetCSA( this ).PerformingSequence )
				return; // no animations if we're performing a sequence
			base.Animate( action, frameCount, repeatCount, forward, repeat, delay );
			if ( external )
				CombatSystemAttachment.GetCSA( this ).OnExternalAnimation();
		}
		
		public void CSAAnimate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			Animate( action, frameCount, repeatCount, forward, repeat, delay, false );
		}
		
		/// <summary>
		/// Overridable. Virtual event invoked after the <see cref="Combatant" /> property has changed.
		/// <seealso cref="Combatant" />
		/// </summary>
		public override void OnCombatantChange()
		{
			CombatSystemAttachment.GetCSA( this ).OnChangedCombatant();
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			CombatSystemAttachment.GetCSA( this ).OnMoved( oldLocation );
            OnXMLEvent( XMLEventType.OnAfterMove );
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ReceivedNewLoot
		{
			get{ return m_ReceivedNewLoot; }
			set{ m_ReceivedNewLoot = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool MarkedForTermination
		{
			get{ return (m_MarkedForTermination && !Controlled && ControlMaster == null && !ProtectedFromTermination && !(this is Soldier)); }
			set{ m_MarkedForTermination = value; }
		}
		
		public bool IsOldPet()
		{
			return m_MarkedForTermination;
		}

		public DateTime ReleaseTime
		{
			get{ return m_ReleaseTime; }
			set{ m_ReleaseTime = value; }
		}
		
		public bool FixedScales
		{
			get{ return m_FixedScales; }
			set{ m_FixedScales = value; }
		}
		
		public virtual string GetPersonalPronoun()
		{
			if( Female )
				return "she";
			
			return "he";
		}
		
		public virtual string GetReflexivePronoun()
		{
			if( Female )
				return "her";
			
			return "him";
		}
		
		public virtual string GetPossessivePronoun()
		{
			if( Female )
				return "her";
			
			return "his";
		}
		
		public virtual string GetPossessive()
		{
			if( Name.EndsWith( "s" ) )
				return "'";
			
			return "'s";
		}
		
		public virtual bool CanSummon()
		{
			return this.CanUseSpecial;
		}
		
		public Timer CrippledTimer { get { return m_CrippledTimer; } set { m_CrippledTimer = value; } }
		public Timer DazedTimer { get { return m_DazedTimer; } set { m_DazedTimer = value; } }
		public Timer TrippedTimer { get { return m_TrippedTimer; } set { m_TrippedTimer = value; } }
		public Timer StunnedTimer { get { return m_StunnedTimer; } set { m_StunnedTimer = value; } }
		public Timer DismountedTimer { get { return m_DismountedTimer; } set { m_DismountedTimer = value; } }
		public Timer BlindnessTimer { get { return m_BlindnessTimer; } set { m_BlindnessTimer = value; } }
		public Timer DeafnessTimer { get { return m_DeafnessTimer; } set { m_DeafnessTimer = value; } }
		public Timer MutenessTimer { get { return m_MutenessTimer; } set { m_MutenessTimer = value; } }
		public Timer DisabledLegsTimer { get { return m_DisabledLegsTimer; } set { m_DisabledLegsTimer = value; } }
		public Timer DisabledLeftArmTimer { get { return m_DisabledLeftArmTimer; } set { m_DisabledLeftArmTimer = value; } }
		public Timer DisabledRightArmTimer { get { return m_DisabledRightArmTimer; } set { m_DisabledRightArmTimer = value; } }
		public Timer FeintTimer { get { return m_FeintTimer; } set { m_FeintTimer = value; } }
		public Timer HealingTimer { get { return m_HealingTimer; } set { m_HealingTimer = value; } }
		public Timer AuraOfProtection { get { return m_AuraOfProtection; } set { m_AuraOfProtection = value; } }
        public Timer JusticeAura { get { return m_JusticeAura; } set { m_JusticeAura = value; } }
		public Timer Sanctuary { get { return m_Sanctuary; } set { m_Sanctuary = value; } }
		public Timer RageTimer { get { return m_RageTimer; } set { m_RageTimer = value; } }
		public Timer ManeuverBonusTimer { get { return m_ManeuverBonusTimer; } set { m_ManeuverBonusTimer = value; } }
		
		public DateTime NextRage { get { return m_NextRage; } set { m_NextRage = value; } }
		
		public int ManeuverDamageBonus{ get{ return m_ManeuverDamageBonus; } set{ m_ManeuverDamageBonus = value; } }
		public int ManeuverAccuracyBonus{ get{ return m_ManeuverAccuracyBonus; } set{ m_ManeuverAccuracyBonus = value; } }
		
		public bool CleaveAttack{ get{ return m_CleaveAttack; } set{ m_CleaveAttack = value; } }
		public bool Fizzled{ get{ return m_Fizzled; } set{ m_Fizzled = value; } }
		
		public virtual int RideBonus{ get{ return 0; } }
		
		public FeatList CurrentSpell{ get{ return m_CurrentSpell; } set{ m_CurrentSpell = value; } }
		public Mobile ShieldedMobile{ get { return m_ShieldedMobile; } set { m_ShieldedMobile = value; } }
		
		public virtual bool ParryDisabled{ get{ return m_CantParry; } }
		
		private string m_WikiConfig;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string WikiConfig{ get{ return m_WikiConfig; } set{ m_WikiConfig = value; } }

        private List<string> m_SecondaryWikiConfigs = new List<string>();
        public List<string> SecondaryWikiConfigs { get { return m_SecondaryWikiConfigs; } set { m_SecondaryWikiConfigs = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public string AddSecondaryWikiConfig 
        {
            get { return null; }
            set
            {
                if( !String.IsNullOrEmpty( value ) )
                    SecondaryWikiConfigs.Add( value );
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool ClearSecondaryWikiConfigs
        {
            get { return false; }
            set
            {
                if( value == true )
                    SecondaryWikiConfigs.Clear();
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool LoadWikiConfig
		{
			get{ return false; }
			set
			{
				if( value == true )
					DoLoadWikiConfig( this, WikiConfig );

                foreach( string st in SecondaryWikiConfigs )
                    DoLoadWikiConfig( this, st );
			}
		}
		
		public static void DoLoadWikiConfig( object o, string config )
		{
			Dictionary<string, string> entries;
			string msg = null;
			string action = null;
			
			if( Commands.WikiDataBase.CreatureEntries.TryGetValue(config, out entries) )
			{
				foreach( KeyValuePair<string, string> kvp in entries )
				{
					msg = null;

                    if( kvp.Key.Contains( "Special" ) )
                        action = "/" + kvp.Value;

                    else
                    {
                        if( kvp.Key.Contains( "AddXMLEvent" ) )
                        {
                            if( o is BaseCreature )
                            {
                                ( (BaseCreature)o ).AddXMLEvent = kvp.Value;
                                continue;
                            }
                        }

                        if( kvp.Key.Contains( "AddSecondaryWikiConfig" ) )
                        {
                            if( o is BaseCreature )
                            {
                                ( (BaseCreature)o ).Say( "Parsing: '" + kvp.Value + "'." );
                                ( (BaseCreature)o ).AddSecondaryWikiConfig = kvp.Value;
                                continue;
                            }
                        }
                        action = "/" + kvp.Key + "/" + kvp.Value;
                    }
					
					BaseXmlSpawner.ApplyObjectStringProperties( null, action, o, null, null, out msg );
					
					if( msg != null )
					{
						string error = "Could not set property '" + kvp.Key + "' to value '" + kvp.Value + "'.";
						
						if( o is Mobile )
							((Mobile)o).Say( error );
						
						if( o is Item )
							((Item)o).PublicOverheadMessage( MessageType.Regular, 0x3B2, false, error );
					}
				}
			}
		}

        [CommandProperty( AccessLevel.GameMaster )]
        public string GiveFeat
        {
            get { return "FeatName Level"; }
            set
            {
                if( String.IsNullOrEmpty( value ) )
                    return;

                string[] args = value.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

                if( args.Length < 2 )
                    return;

                int level = 0;

                if( !int.TryParse(args[1], out level) )
                    return;

                try
                {
                    FeatList feat = (FeatList)Enum.Parse( typeof(FeatList), args[0], true );
                    Feats.SetFeatLevel( feat, level );
                }

                catch
                {
                    return;
                }
            }
        }
		
		public BaseStance Stance
		{
			get
			{
				if( m_Stance == null )
					m_Stance = new BaseStance();
				
				return m_Stance;
			}
			
			set{ m_Stance = value; }
		}
		
		public bool IsTired( bool message )
		{
			return false;
		}
		
		public bool IsTired()
		{
			return false;
		}
		
		public bool IsAllyOf( Mobile mob )
		{
			return BaseAI.AreAllies( this, mob );
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Nation Nation
        {
            get { return m_Nation; }
            set { m_Nation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public GovernmentEntity Government
        {
            get { return m_Government; }
            set { m_Government = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CustomGuildStone Organization
        {
            get { return m_Organization; }
            set { m_Organization = value; }
        }

		public bool CanDodge
		{
			get
			{
				if( Paralyzed )
        			return false;
        		
        		if( TrippedTimer != null )
        			return false;
        		
				return (Feats.GetFeatLevel(FeatList.EnhancedDodge) > 0);
			}
		}
		
		public bool CanUseMartialPower
		{
			get
			{
				if( CanUseMartialStance && this.Weapon is Fists )
					return true;
				
				this.DisableManeuver();
				return false;
			}
		}
		
		public bool CanUseMartialStance
		{
			get
			{
				foreach( Item item in this.Items )
				{
					if( item is BaseArmor && !(item is Backpack) )
						return false;
				}
				
				if( this.Mounted )
					return false;
				
				return true;
			}
		}
		
		public bool Evaded()
        {
			if( Paralyzed )
    			return false;
    		
    		if( TrippedTimer != null )
    			return false;
        		
        	return PlayerMobile.EvadedCheck( this );
        }
		
		public bool Dodged()
        {
        	return PlayerMobile.DodgedCheck( this );
        }
		
		public bool Snatched()
        {
            return PlayerMobile.SnatchedCheck( this );
        }

        public bool DeflectedProjectile()
        {
        	return PlayerMobile.DeflectedProjectileCheck( this );
        }
		
		public Feats Feats
		{ 
			get 
			{ 
				if( m_Feats == null )
					m_Feats = new Feats( true );
				
				return m_Feats; 
			} 
			set { } 
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public CombatStyles CombatStyles
		{ 
			get 
			{ 
				if( m_CombatStyles == null )
					m_CombatStyles = new CombatStyles(); 
				
				return m_CombatStyles; 
			} 
			set { } 
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextFeatUse { get { return m_NextFeatUse; } set { m_NextFeatUse = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public FeatList OffensiveFeat { get { return m_OffensiveFeat; } set { m_OffensiveFeat = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public BaseCombatManeuver CombatManeuver
        { 
        	get 
        	{
        		if( m_CombatManeuver == null )
        			m_CombatManeuver = new BaseCombatManeuver();
        		
        		return m_CombatManeuver; 
        	} 
        	set 
        	{ 
        		m_CombatManeuver = value; 
        	} 
        }
        
        public BaseStance SetStance { get { return m_SetStance; } set { m_SetStance = value; } }
        public BaseCombatManeuver SetManeuver { get { return m_SetManeuver; } set { m_SetManeuver = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int RageFeatLevel
        {
            get
            {
            	if( RageTimer == null )
            		return 0;
            	
        		return m_RageFeatLevel; 
        	}
            set { m_RageFeatLevel = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public string NoDeathMsg{ get{ return m_NoDeathMsg; } set{ m_NoDeathMsg = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool NoDeath{ get{ return m_NoDeath; } set{ m_NoDeath = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int NoDeathSound{ get{ return m_NoDeathSound; } set{ m_NoDeathSound = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool NoDeathCondition{ get{ return m_NoDeathCondition; } set{ m_NoDeathCondition = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseSpecial
		{	
			get 
			{
				if( DateTime.Compare( DateTime.Now, this.NextFeatUse ) > 0 )
					return true;
				
				return false;
			}
			
			set
			{
				if( value == true )
					this.NextFeatUse = DateTime.Now;
				
				else
					this.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( 15 );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Enthralled
		{	
			get 
			{
				XmlEnthrallment enthrall = XmlAttach.FindAttachment( this, typeof( XmlEnthrallment ) ) as XmlEnthrallment;
	            
	            if( enthrall != null )
	            	return true;
	            
				return false; 
			}
			
			set
			{
				if( value == false )
				{
					XmlAttachment att = XmlAttach.FindAttachment( this, typeof( XmlEnthrallment ) );

					if( att != null )
					{
						this.Emote( "*snaps out of a trance*" );
						att.Delete();
					}
				}
			}
		}
		
		public Mobile ShieldingMobile
        {
            get { return m_ShieldingMobile; }
            set { m_ShieldingMobile = value; }
        }
        
        public double ShieldValue
        {
            get { return m_ShieldValue; }
            set { m_ShieldValue = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool SummonedByMob
		{
			get
			{
				if( DateTime.Compare( VanishTime, DateTime.MinValue ) == 0 )
					return false;
				
				return true;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
        public DateTime VanishTime
        {
            get { return m_VanishTime; }
            set { m_VanishTime = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string VanishEmote
        {
            get { return m_VanishEmote; }
            set { m_VanishEmote = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
		public override bool Paralyzed
		{
			get
			{
                if( this.StunnedTimer != null || this.m_PetrifiedTimer != null || PlayerMobile.HasParalyzeAtt(this) )
					return true;
				
				return base.Paralyzed;
			}
			set
			{
				base.Paralyzed = value;
			}
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
        public override bool Frozen
        {
            get
            {
            	if( m_FreezeTimer != null || m_CrippledTimer != null || m_DisabledLegsTimer != null || (this.Region is StickyGooRegion && CustomBreathType != CustomBreathType.Goo) )
            		return true;
            	
            	else
               		return base.Frozen;
            }
            set
            {
                base.Frozen = value;
            }
        }

        private List<Item> m_CustomSkinnableParts = new List<Item>();
        public List<Item> CustomSkinnableParts
        {
            get { return m_CustomSkinnableParts; }
            set { m_CustomSkinnableParts = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string AddSkinnablePart
        {
            get { return null; }
            set { NewSkinnablePart( value ); }
        }

        public void NewSkinnablePart( string part )
        {
            if( String.IsNullOrEmpty( part ) )
                return;

            string[] args = part.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

            if( string.IsNullOrEmpty( args[0] ) )
                return;

            double chance = 1.0;

            if( args.Length > 1 && !string.IsNullOrEmpty( args[1] ) )
                double.TryParse( args[1], out chance );

            Type partType = ScriptCompiler.FindTypeByName( args[0] );

            if( partType != null && partType.IsSubclassOf( typeof( Item ) ) && chance >= Utility.RandomDouble() )
            {
                Item finishedPart = (Item)Activator.CreateInstance( partType );
                m_CustomSkinnableParts.Add( finishedPart );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual int DyeArmour
        {
            get{ return 0; }
            
            set
            {
            	if( value > -1 )
            	{
            		foreach( Item item in this.Items )
            		{
            			if( item is BaseArmor )
            				item.Hue = value;
            		}
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual int DyeClothes
        {
            get{ return 0; }
            
            set
            {
            	if( value > -1 )
            	{
            		foreach( Item item in this.Items )
            		{
            			if( item is BaseClothing )
            				item.Hue = value;
            		}
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual double CombatSkills
        {
            get{ return 0; }
            
            set
            {
            	Skills[SkillName.Tactics].Cap = value;
            	Skills[SkillName.Parry].Cap = value;
            	Skills[SkillName.Anatomy].Cap = value;
            	Skills[SkillName.Swords].Cap = value;
            	Skills[SkillName.Axemanship].Cap = value;
            	Skills[SkillName.UnarmedFighting].Cap = value;
            	Skills[SkillName.Macing].Cap = value;
            	Skills[SkillName.Polearms].Cap = value;
            	Skills[SkillName.ExoticWeaponry].Cap = value;
            	Skills[SkillName.Fencing].Cap = value;
                Skills[SkillName.Archery].Cap = value;
            	
            	Skills[SkillName.Tactics].Base = value;
            	Skills[SkillName.Parry].Base = value;
            	Skills[SkillName.Anatomy].Base = value;
            	Skills[SkillName.Swords].Base = value;
            	Skills[SkillName.Axemanship].Base = value;
            	Skills[SkillName.UnarmedFighting].Base = value;
            	Skills[SkillName.Macing].Base = value;
            	Skills[SkillName.Polearms].Base = value;
            	Skills[SkillName.ExoticWeaponry].Base = value;
            	Skills[SkillName.Fencing].Base = value;
                Skills[SkillName.Archery].Base = value;
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual double MagicSkills
        {
            get{ return 0; }
            
            set
            {
            	Skills[SkillName.Invocation].Base = value;
            	Skills[SkillName.Magery].Base = value;
            	Skills[SkillName.MagicResist].Base = value;
            	Skills[SkillName.Meditation].Base = value;
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual CraftResource ArmourResource
        {
            get{ return CraftResource.None; }
            
            set
            {
        		foreach( Item item in this.Items )
        		{
        			if( item is BaseArmor )
        				( (BaseArmor)item ).Resource = value;
        		}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual CraftResource ClothingResource
        {
            get{ return CraftResource.None; }
            
            set
            {
        		foreach( Item item in this.Items )
        		{
        			if( item is BaseClothing )
        				( (BaseClothing)item ).Resource = value;
        		}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool HalvePower
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            	{
            		this.RawHits = (int)( this.RawHits * 0.5 );
            		this.RawStam = (int)( this.RawStam * 0.5 );
            		this.RawMana = (int)( this.RawMana * 0.5 );
            		this.RawStr = (int)( this.RawStr * 0.5 );
            		this.RawDex = (int)( this.RawDex * 0.5 );
            		this.RawInt = (int)( this.RawInt * 0.5 );
            		
            		this.DamageMin = (int)( this.DamageMin * 0.5 );
            		this.DamageMax = (int)( this.DamageMax * 0.5 );
            		
            		this.VirtualArmor = (int)( this.VirtualArmor * 0.5 );
            		this.Fame = (int)( this.Fame * 0.5 );
            		
            		this.ColdResistSeed = (int)( this.ColdResistSeed * 0.5 );
            		this.FireResistSeed = (int)( this.FireResistSeed * 0.5 );
            		this.PoisonResistSeed = (int)( this.PoisonResistSeed * 0.5 );
            		this.EnergyResistSeed = (int)( this.EnergyResistSeed * 0.5 );
            		this.BluntResistSeed = (int)( this.BluntResistSeed * 0.5 );
            		this.SlashingResistSeed = (int)( this.SlashingResistSeed * 0.5 );
            		this.PiercingResistSeed = (int)( this.PiercingResistSeed * 0.5 );
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool RemoveAtts
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            		
            	ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlAttachment ) );

				for( int i = 0; i < list.Count; ++i )
				{
					XmlAttachment att = list[i] as XmlAttachment;
					att.Delete();
				}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool AlyrianName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            		
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Alyrian, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool AzhuranName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Azhuran, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool KhemetarName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Khemetar, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool MhordulName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Mhordul, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TyreanName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Tyrean, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool VhalurianName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.RandomName( Nation.Vhalurian, this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool InsulariiName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnAlyrian
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Alyrian );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnAzhuran
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Azhuran );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnKhemetar
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Khemetar );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnMhordul
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Mhordul );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnTyrean
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Tyrean );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TurnVhalurian
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            		ProcessLooks( Nation.Vhalurian );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TrollName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveTrollName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool DragonName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveDragonName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool GoblinName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveGoblinName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool YuanTiName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveYuanTiName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool GiantName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveGiantName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool OgreName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveOgreName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool BeastmanName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveBeastmanName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool DriderName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveDriderName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool KoboldName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveKoboldName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool LesserGiantName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveLesserGiantName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool TroglinName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveTroglinName( this.Female );
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool GoatmanName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveGoatmanName( this.Female );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool BeholderName
        {
            get{ return false; }
            
            set
            {
            	FixGender();
            	
            	if( value == true )
            		this.Name = GiveBeholderName( this.Female );
            }
        }
        
        private void FixGender()
        {
        	if( this.BodyValue == 401 )
        		this.Female = true;
        	
        	else if( this.BodyValue == 400 )
        		this.Female = false;
        }
        
        public string GiveBeholderName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Oxath,Ulmaryl,Iqonta,Thorgla,Xanarth,Qyrgla,Ukera,Zertina,Zyr,Xerglarith,Utha,Xira,Akir," +
					"Kexaxyrx,Xarglyn,Xaxir,Zuthyn,Xaxtin,Xith,Xonathdyl,Karixyl,Oxeskirq,Uthlib,Xathir,Zultyk,Zir," +
					"Khalirx,Gzonir,Naxill,Zunglyrlr,Othy,Zerik,Xoxloril,Zunysm,Zulmis,Qentixyk,Xothoril,Ullyx," +
					"Qoril,Qorin,Zentonirlr,Xoxtirq,Qyr,Xoblin,Xuthill,Xikimt,Ullyr,Ukirmyn,Xuthlandryx," +
					"Mirylq,Mirx,Xanirx,Xora,Sarglothdyll,Karkhyll,Zonoxis,Zulin,Xoxlirx,Zencyr,Xulorcyr";
			}
			
			else
			{
				namelist = "Zelloxoph,Xakwod,Xixon,Xixek,Uske,Gzonxun,Zothek,Qethlo,Zoxach,Xixor,Iros,Zoxtus,Athus,Xoxirtor," +
					"Xaxoth,Yuthon,Marus,Doolulq,Oole,Unolq,Oolthor,Xuntoll,Xalorth,Xokob,Xablerkhorx,Unglokirest,Zunord," +
					"Zenir,Xuliq,Mongris,Iluae,Illak,Ikarq,Illest,Xinuul,Zendroxixall,Zulall,Xelus,Othdall,Ool," +
					"Xilaxoth,Xonoth,Uson,Axtenus,Xervest,Ukandros,Unglok,Onxom,Qyroch,Oru,Xoth,Xoblob,Qothdor,Zythlorq," +
					"Altox,Zoblor,Xolu,Saxox,Iloth";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveLesserGiantName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Maila,Manele,Susurin,Chiluna,Dagles,Jesunna,Dula,Eraega,Seaira,Jiaata," +
					"Shanuga,Sula,Jusa,Chaelua,Miasua,Coruna,Selune,Manugle,Marunna,Jesugle,Caisua,Marune," +
					"Jaetua,Jugla,Chilunda,Saerua,Jula,Maru,Jaerua,Duna,Susa,Jenugla,Dusua,Erauga,Darule," +
					"Daenua,Dusega,Shanusa,Elaidua,Ceaisa,Shanula,Chula,Jelendua,Duruna,Jula,Jennuta,Esura," +
					"Jenua,Chaune,Dasua";
			}
			
			else
			{
				namelist = "Paeserg,Edrag,Cailg,Tailveg,Dulg,Telg,Roedrank,Fark,Rang,Haesek,Culaink,Helvg,Rarg," +
					"Helaenk,Paersk,Raiserk,Haeryg,Fadarg,Cedryg,Elveg,Paeseg,Fanerg,Pank,Toerk,Perang,Hulaing,Taidwag," +
					"Tonak,Darg,Fadrik,Poedweng,Pelk,Fanelg,Paitrg,Culg,Eadwing,Donerk,Haerk,Telaing,Fidwyg,Haeserg,Donk," +
					"Failverg,Roedark,Roedryg,Cairg,Caedwak,Darylg,Fank,Hoedreg";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveKoboldName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Traspeska,Arvita,Pigila,Sorna,Tekkexsa,Harna,Fombeka,Aglypa,Hossaja," +
					"Dwirgopa,Turga,Numkura,Ingula,Dloammila,Gwyla,Olpa,Linnpa,Lypa," +
					"Cymstarla,Elgipa,Tronpa,Kirshandypa,Erma,Nispa,Varta,Ixala,Bamba,Ulbesceka," +
					"Vloarpa,Ijultha,Chjanthuska,Sota,Elbipa,Yulminka,Rerbicka,Cruna,Urmixoba,Cuspa,Undinspritha," +
					"Jarborna,Coelmerta,Bhepa,Jhesbankipa,Bauldolta,Lynta,Kryrela,Irtra,Wamborrilpa,Roblepa,Naxsompa";
			}
			
			else
			{
				namelist = "Burlgahupool,Blapguul,Slilpoop,Thoolm,Gilpoolgab,Goolgh,Woolm,Hurguulmon," +
					"Boolgh,Bopoorgopoorp,Thoorp,Thooglup,Vaburlgoolp,Thoopdil,Slurgoop,Burpliboolp," +
					"Hurguulool,Slagmool,Dilp-Upoorg,Oolugap,Slupgoon,Kurlgaluorg,Gaboolm,Doolp," +
					"Thopalbpuurp,Dapoolm,Thoolp,Viloopdaguulp,Hurgoopgap,Vahurplilool,Hupagoolgh," +
					"Gahugoolgh,Kugiloolm,Slapguulmoorg,Vibupdabool,Daboolm,Huguulgiloolgh,Oolugib," +
					"Kupoopgiloorp,Guulgop,Quolm,Goolplool,Sloolp-Up,Dilupoorp,Boorlguul,Quolgh," +
					"Hurlgagoolp,Quolkup,Vabdoolgh,Quolp";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveGoatmanName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Nayllian,Norduki,Nollia,Nondriali,Niothansta,Neenfuree,Nornnald,Nislianna," +
					"Nalla,Nonstilfre,Nayrtha,Nankethi,Neerrinda,Nayre,Nitcharli,Narne,Nirvalgatte,Niva," +
					"Nosilfrienda,Nolfriashi,Neeshatte,Neerthor,Neti,Neenla,Neerkent,Nette,Nondrayna," +
					"Niothfaria,Naysra,Nore,Nosgilde,Naylgotte,Norhiki,Needmin,Nengelia,Nolla,Niothayna," +
					"Nelleenga,Nilgara,Naylusa,Nornaya,Niothfurnaya,Narya,Nolwioth,Nonkaelleenna,Nalseda," +
					"Niothetcheta,Nayldiska,Nayllornna,Naylioth";
			}
			
			else
			{
				namelist = "Morrenda,Mostrida,Milfreethi,Murnnarra,Mingaria,Munta,Marthund,Marya," +
					"Marnnirya,Mothel,Melia,Mitchalde,Manni,Mitinfyntha,Metta,Mire,Mediandia" +
					"Molfryji,Merniornaya,Mendrianna,Mornnis,Mensta,Mossiva,Mondrorne,Murvulia," +
					"Masoncha,Mirngent,Mala,Mittirne,Malgioth,Merleti,Muvoki,Mesa,Mathferee," +
					"Mosgenth,Manta,Menna,Murdix,Mincha,Morvornay,Milfris,Mureerna,Masgiska," +
					"Melda,Mati,Mostia,Mebfurla,Milfryalla,Mohellia,Medosha";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveTroglinName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Winka,Urchera,Gruwsva,Missfa,Thorthunva,Janbroutha,Mourkwinga,Fluembra," +
					"Brienktonta,Busfigha,Wista,Virthlechta,Drenvirra,Creiddla,Varlcheldiga," +
					"Borienghonta,Thulia,Oadfiella,Luturga,Bunsfiela,Mustborndonta,Fleentvarnbra,Sourra," +
					"Nondrorfa,Treellfa,Tryntvillwa,Woortlaklinga,Sorcteaka,Bleengtalda,Selgoutha,Cranbra," +
					"Triennmooka,Bridwalla,Jonktallsvanda,Flotena,Ponnmoonfolda,Luembrona,Olldunbeaka,Broodla," +
					"Pankteima,Perimbra,Walga,Onchbalda,Cerolda,Nytlla,Threlgunfa,Onsfanda,Anchespilla," +
					"Kanntla,Spentla";
			}
			
			else
			{
				namelist = "Troodlurg,Moonsferg,Etwig,Klenbridog,Blystwong,Oadwayhoog,Jorsbrong,Emoorgong,Ungtank," +
					"Orctyngsg,Breidfack,Croorkshorg,Breerfiek,Fritwog,Albunvog,Whirksheikeg,Ensullfig," +
					"Velgook,,Ankfartolg,Eorsbrig,Gumbriennmeak,Brortsmolfballs,Flywivasg,Cruitog,Grurfelg," +
					"Nirtlarthlog,Edellg,Klisg,Drasswig,Druitak,Panktoork,Droorb,Jofforib,Dasheikfeb," +
					"Grertsfeng,Bonchab,Cotlidsvorg,Rylmood,Vottenb,Hitarnding,Spirtlib,Gristb,Roonsorg,Truitamg," +
					"Klestlong,Kelldeng,Flanshurg,Throrndurg,Gaark,Nonthusg";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveGiantName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Eustobebranda,Wennilla,Rhiba,Riedacuda,Fleoddoina,Glennia,Obbusva,Trulerna,Debruna,Baulmiuswurgela," +
					"Friandehieda,Trerna,Wafriuddulta,Hennia,Motziga,Utsoania,Hulta,Chliedita,Criedriusva,Joththeodiuddaulpha,Vugna," +
					"Rolfassa,Segta,Cnufruca,Aorolmunda,Dibburdena,Wima,Treaglebrera,Tottiussa,Hemprurtalda,Teiasvunda,Ornfriva," +
					"Othmilfela,Bincyrulta,Soarottitha,Brogainthura,Eudrergia,Criblotumla,Euthmolda,Muntisbosa,Thriatha,Cnurcuba," +
					"Nendeganda,Flievittiva,Aornfroda,Purna,Puca,Chleovetthaitha,Jorga,Hunderta";
			}
			
			else
			{
				namelist = "Aithuin,Osturdoltias,Ublach,Modolf,Hispo,Bufraugne,Wutrindo,Chlunir,Krorduindointhied,Hebralluinth," +
					"Dusbeurcius,Vurneccoar,Ufriediulmert,Cyrdisvans,Ostoas,Shasso,Brefrembas,Baiswoththiv,Omnoda,Euraut,Naulchento," +
					"Rhikerung,Naththen,Rodrud,Chiccotrespec,Chiancussi,Ultiovoas,Duinduba,Flobbauldes,Miulleonda,Vecad,Usoarwild," +
					"Aiththert,Theov,Creagrung,Tobriuswied,Bedinth,Cnilliedus,Kriulfiltut,Thindald,Glostrachedir,Kreza,Pusossa,Jaro," +
					"Pusbauloin,Evikenthas,Friarworguinth,Tauspurnult,Fanded,Ainus";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveGoblinName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Kuurga,Krauloga,Dulla,Uruumsha,Fahgtra,Hona,Blargula,Yurdrusa,Ghaurgha,Darra,Urrha,Vauurga,Blahka," +
					"Trorculla,Uelaugga,Maerogha,Gahka,Vraurra,Illisha,Hothronga,Ghauusta,Ngorga,Rorminga,Grauusta,Olglogha,Wullda," +
					"Vunga,Nerra,Oraurgha,Ruura,Klonga,Nnerla,Thaulda,Shanda,Maurogha,Umggoka,Brauusta,Thauga,Oldua,Ulrunda,Rauusta," +
					"Gordrogha,Mahgtrua,Groncha,Daurra,Ulraila,Urdrogha,Uhragha,Furtha,Ullukha";
			}
			
			else
			{
				namelist = "Thuldahk,Kuurogh,Huragh,Vrarg,Rauus,Yuraurgh,Barl,Grellagh,Urash,Urogh,Blagrong,Truaughkh," +
					"Wurgok,Troruaugh,Ulrogh,Uthragh,Uell,Huld,Huorg,Greng,Uelkh,Balglog,Dzarl,Oldoul,Braeunk,Derm," +
					"Ahrund,Sharr,Khulg,Glorund,Grarl,Lauth,Glaur,Ogruth,Raeun,Uhrag,Arog,Uldok,Ruurg,Kung,Rong,Uldae," +
					"Grarth,Ghahgtru,Tonk,Gaulkh,Urog,Raurukh,Brerg,Ormang";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveOgreName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Mallit,Zwejda,Gwiza,Mina,Siba,Tisent,Lilla,Wina,Tatrust,Titbirt,Tella,Ghnima,Tanest," +
					"Kutreght,Aklit,Triya,Wriya,Wres,Tatiri,Zazemt,Rujigt,Wellet,Lwat,Menibbirt,Tulla,Tallit," +
					"Myasut,Mulilla,Jdira,Tilla,Zibirt,Lyasut,Tayri,Tannest,Tinimunt,Menet,Tinnan,Marrust,Bghnat," +
					"Tissut,Zibba,Tillula,Tarust,Zimina,Zureght,Damemt,Zumidjigt,Tanwes,Ghirit";
			}
			
			else
			{
				namelist = "Ifnadir,Kedis,Chadli,Ultan,Yub,Anis,Irgan,Abn,Immas,Meksa,Ilusser,Yunza,Finnas,Sekla,Zwinzer," +
					"Moudan,Ikzer,Amigen,Messer,Iggen,Nsefal,Amenzu,Medun,Lwat,Mazruc,Zwissan,Yeliyen,Nser,Yeldir,Felaji," +
					"Kaousten,Yil,Akber,Nselal,Ilaji,Ixtan,Urayghur,Nsen,Mismir,Urasser,Masnser,Muzizaig,Saled,Iklan,Ighleder," +
					"Yebrir,Buran,Zigza,Itras,Yunes";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveBeastmanName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Alanisa,Belisu,Mesure,Timesu,Amanera,Nerela,Eramene,Kumera,Esalera,Belera,Nicia," +
					"Nelisa,Tisune,Kumora,Tisanelia,Kuresa,Elensu,Amerene,Elansa,Melera,Alanere,Besurelia,Kumala," +
					"Elimene,Neresu,Besura,Belia,Bimuliria,Kumore,Kurenicia,Kurela,Elisura,Kumene,Amenele,Timesa," +
					"Elamesa,Kurale,Amensa,Amelene,Amerala,Nelime,Malia,Besune,Kesia,Melia,Nimora,Erensa,Amansa," +
					"Alansa,Besalicia";
			}
			
			else
			{
				namelist = "Suno,Blaenar,Manema,Kangu,Kanaru,Manan,Menin,Nantan,Hunema,Non,Blaerin,Kanta," +
					"Ingar,Chagana,Nantu,Anangen,Ninaro,Baran,Sangu,Rakan,Rayen,Sangen,Hunga,Easar,Chanana," +
					"Blaenin,Barta,Mangan,Nanin,Ranena,Sandao,Semao,Chan,Bangen,Nangu,Nangan,Mana,Nagao,Naran," +
					"Manin,Nongan,Sagao,Kangan,Kartu,Masena,San,Sanga,Runan,Charo,Maeto";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveDriderName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Ginstra,Jezothglauthe,Burther,Brilaurolu,Byrnil,Irulis,Ghaeess,Camress,Quere," +
					"Rhaussriel,Ssalulis,Kel,Kyoreddyth,G'elchyra,Aruit,Kyornyl,Dwillerin,Miclis,Irstra," +
					"Naerthe,Ssaririn,Irdnat,Zirrius,Reriliira,Melthel,Elintra,Lylynrae,Shulrirva,X'lulae," +
					"Zelin,Sulyn,Gidyth,Sulymma,Zirniiryl,Camshyrnyl,Rilree,Nulisstree,Jharress,Felonia," +
					"Noanker,Lirnithra,Yasree,Mossryress,Phaelyrr,Teena,Shirae,Phaeld,Wilrae,Mispriress,Imreryra";
			}
			
			else
			{
				namelist = "Zalonel,Rilorn,Nandromph,Culeryn,Qul,Pindyrvar,Ilimar,Bhaerlyr,Gen'nar,Sylandryn," +
					"Marolel,Illein,Ritearn,Kelaxle,Dramph,Berzenin,Mirlel,Minnet,Darroos,Uuedurnn,Myrlist," +
					"Sheerdar,Reldaer,Tolraryn,Orgonir,Dinderril,Otirane,Sorlorn,Medonzar,Rendaeril,Solanden," +
					"Deraenozz,Krolaxle,Khartus,Ruirar,Firasymmyr,Imberahc,Ghaulein,Khue-Guy,Horn,Gosyrak," +
					"Rylmracc,Tlar,Vulil,Lelonshar,Weirius,Orlinode,Melrahc,Indaer,Mileinozz";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveYuanTiName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Shaglene,Suranda,Shasu,Shaglena,Surin,Sesin,Saerina,Shaglenda,Saeraget," +
        		"Susena,Seset,Shannin,Shara,Saerane,Seglian,Shagian,Shagles,Seglan,Sesu,Shannes,Seaile," +
        		"Sendet,Surine,Shana,Seaira,Sharinda,Shannele,Sera,Sharin,Sunda,Suseainnes,Saera,Saerasu," +
        		"Shan,Saena,Seaindes,Shaglet,Saereget,Shan,Segline,Surannes,Saerian,Saesu,Sus,Sharene," +
        		"Shannaret,Shages,Shaglian,Sennasu,Sharile";
			}
			
			else
			{
				namelist = "Shaglen,Suran,Shas,Shasos,Sur,Ses,Saerin,Shaglen,Saerag,Sust," +
					"Sest,Shann,Shar,Saeran,Segian,Shaian,Shaes,Segan,Sesst,Shan,Seais,Sens," +
					"Surn,Shagt,Seair,Sharind,Shannel,Ser,Shars,Sund,Suseain,Saer,Saeras,Shan," +
					"Saen,Seain,Shaass,Saereg,Sesn,Surann,Saers,Saes,Sus,Shares,Shases," +
					"Shages,Shaglias,Senens,Sharis";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveTrollName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Iratik,Abuktik,Girtirg,Kipdik,Atirg,Rirtputik,Adik,Gtutirg,Dabatidrig,Drutirk," +
					"Apadrig,Gdiuputik,Rardrig,Idudrig,Tdatirg,Adik,Udrig,Atirk,Dtabkidrig," +
					"Bratirk,Bartik,Draidadik,Griikutirk,Aitirk,Aatik,Rartirk,Gditirg,Drugkutirg,Bartirk," +
					"Ukitik,Dabdik,Dtidik,Dartirk,Iabudik,Uridik,Gukbikdik,Uradrig,Atik,Budkibtik";
			}
			
			else
			{
				namelist = "Bugbatak,Ukodrog,Upudrog,Utgak,Bdobetak,Aredak,Gogtuk,Baetuk,Kdetarg,Eedrog," +
					"Kgabettuk,Kedrotarg,Eadrog,Kedrog,Reetarg,Etettarg,Egetuk,Aretak,Utuk,Eedarg," +
					"Gaarg,Teptarg,Aebadrog,Odtetuk,Ebetarg,Oeputak,Boktarg,Kpatuk,Oretuk,Gkardrog," +
					"Tubtak,Pogtak,Dugtarg,Atuk,Ububattarg,Otegarg,Uadak,Poadak,Adrog,Dratuk";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        public string GiveDragonName( bool female )
        {
        	string namelist = "";
			
			if( female )
			{
				namelist = "Emeyth,Iryth,Uskyth,Dalryth,Icayth,Ieryth,Yanujth,Ieoldynth,Euackyth,Tajnth,Ordajth,Aeghajth" +
					"Auhonith,Udihth,Whajth,Tasradylth,Reyrth,Tanoith,Taiuth,Lereldith,Elmulth,Esysth,Avyth,Rayuth,Iaygth," +
					"Emuth,Ykalyth,Eyrakuth,Ezuth,Seretynth,Kaenurth,Hinundith,Enyth,Yuth,Athelmith,Ooverunth,Aeckuhth," +
					"Atuntycth,Ysyth,Aruth,Eekynth,Zuth,Tinuth,Noutanyth,Ilduth,Poleuth,Ieqyth,Saygynth,Skelrodyth," +
					"Uisidth,Dryth";
			}
			
			else
			{
				namelist = "Otarth,Oithanth,Aishath,Shoisayonth,Uanorth,Lyeashiath,Dath,Aldosalth,Thrieth," +
					"Sayanth,Creanth,Luhatarth,Iadarath,Aukarth,Yeranth,Roiathanth,Rhochath,Aightanth,Issieth," +
					"Torath,Ukarth,Teinarth,Harneth,Fethnarth,Hexth,Rakarth,Drackarth,Druiteth,Rotheth,Atarth," +
					"Swath,Toreth,Oidareth,Omisoth,Eideloeth,Itarth,Atath,Taneth,Zhasth,Ainneth,Ooinaoth," +
					"Doxarth,Istath,Lereth,Yersayath,Lath,Emath,Swarth,Ildrilath,Sayath,Ackarth";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
        }
        
        private void ProcessLooks( Nation nation )
        {
        	if( this.BodyValue != 400 && this.BodyValue != 401 )
        	{
        		this.Say( "This only works on human mobiles." );
        		return;
        	}
        	
        	FixGender();
        	
    		this.Name = BaseKhaerosMobile.RandomName( nation, this.Female );
    		this.Hue = BaseKhaerosMobile.AssignRacialHue( nation );
    		this.HairItemID = BaseKhaerosMobile.AssignRacialHair( nation, this.Female );
    		int hairhue = BaseKhaerosMobile.AssignRacialHairHue( nation );
    		this.HairHue = hairhue;
    		
    		if( !this.Female )
    		{
    			this.FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair( nation );
    			this.FacialHairHue = hairhue;
    		}
    		
    		else
    			this.FacialHairItemID = 0;
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool EmptyHanded
        {
            get{ return false; }
            
            set
            {
            	if( value == true )
            	{
            		Item item = this.FindItemOnLayer( Layer.FirstValid );
            		
            		if( item != null && !item.Deleted )
            			item.Delete();
            		
            		item = this.FindItemOnLayer( Layer.OneHanded );
            		
            		if( item != null && !item.Deleted )
            			item.Delete();
            		
            		item = this.FindItemOnLayer( Layer.TwoHanded );
            		
            		if( item != null && !item.Deleted )
            			item.Delete();
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool RemoveLoot
        {
            get{ return false; }
            
            set
            {
            	if( value == true && this.Backpack != null && !this.Backpack.Deleted )
            	{
	            	ArrayList list = new ArrayList();
	            	
	            	foreach( Item item in this.Backpack.Items )
	            	{
	            		list.Add( item );
	            	}
	        	
	        		for( int i = 0; i < list.Count; ++i )
	        		{
	        			Item item = list[i] as Item;
	        			
	        			if( item != null && !item.Deleted )
	        			{
	        				try
	        				{
	        					item.Delete();
	        				}
	        				
	        				catch( Exception e )
	        				{
	        					Console.WriteLine( e.Message );
	        				}
	        			}
	        		}
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual Mobile CopyStatsAndGear
        {
        	get{ return null; }
        	set
        	{
        		RemoveEquipFrom( this ); CopyStatsAndGearFrom( value, this );
        	}
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual bool RemoveEquip
        {
            get{ return false; }
            
            set
            {
            	if( value == true && this.Items != null )
            		RemoveEquipFrom( this );
            }
        }
        
        public static void RemoveEquipFrom( Mobile mob )
        {
        	ArrayList list = new ArrayList();
            	
        	foreach( Item item in mob.Items )
        	{
        		if( item is BankBox || item is Backpack || item is BackpackOfHolding || item is ArmourBackpack || item is MountItem )
        			continue;
        		
        		list.Add( item );
        	}
    	
    		for( int i = 0; i < list.Count; ++i )
    		{
    			Item item = list[i] as Item;
    			
    			if( item != null && !item.Deleted )
    			{
    				try
    				{
    					item.Delete();
    				}
    				
    				catch( Exception e )
    				{
    					Console.WriteLine( e.Message );
    				}
    			}
    		}
        }
        
        public static void CopyStatsAndGearFrom( Mobile mob, Mobile copist )
        {
        	if( mob != null && mob is Mobile && !mob.Deleted )
        	{
        		copist.RawStr = mob.RawStr;
        		copist.RawDex = mob.RawDex;
        		copist.RawInt = mob.RawInt;
        		copist.RawHits = mob.RawHits;
        		copist.RawStam = mob.RawStam;
        		copist.RawMana = mob.RawMana;
        		copist.Hits = mob.Hits;
        		copist.Stam = mob.Stam;
        		copist.Mana = mob.Mana;
        		copist.Hue = mob.Hue;
        		copist.HairItemID = mob.HairItemID;
        		copist.FacialHairItemID = mob.FacialHairItemID;
        		copist.HairHue = mob.HairHue;
        		copist.FacialHairHue = mob.FacialHairHue;
        		
        		foreach( Item item in mob.Items )
        		{
        			Item copy = item;
                    Type t = copy.GetType();
                    ConstructorInfo c = t.GetConstructor( Type.EmptyTypes );

                    if( c != null )
                    {
                        object o = c.Invoke( null );

                        if( o != null && o is Item )
                        {
                            Item newItem = (Item)o;
                            Commands.Dupe.CopyProperties( newItem, copy );
                            copy.OnAfterDuped( newItem );
                            newItem.Parent = null;
                            copist.EquipItem( newItem );
                        }
                    }
        		}
        	}
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual BaseCreature CopyCreatureLooks
        {
            get{ return null; }
            
            set
            {
            	if( value is BaseCreature )
            	{
            		BaseCreature bc = value as BaseCreature;
            		this.Name = bc.Name;
            		this.Title = bc.Title;
            		this.BaseSoundID = bc.BaseSoundID;
            		this.BodyValue = bc.BodyValue;
            		this.Hue = bc.Hue;
            	}
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public CreatureGroup CreatureGroup
        {
            get { return m_CreatureGroup; }
            set { m_CreatureGroup = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Intimidated
        {
            get { return m_Intimidated; }
            set { m_Intimidated = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool IsHuntingHound
        {
            get { return m_IsHuntingHound; }
            set { m_IsHuntingHound = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int XPScale
        {
            get { m_XPScale = FixScale(m_XPScale); return m_XPScale; }
            set { m_XPScale = FixScale(value); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SkillScale
        {
            get { m_XPScale = FixScale(m_XPScale); return m_XPScale; }
            set { m_SkillScale = FixScale(value); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int StatScale
        {
            get { m_XPScale = FixScale(m_XPScale); return m_XPScale; }
            set { m_StatScale = FixScale(value); }
        }
        
        public static int FixScale( int scale )
        {
        	if( scale > 5 )
        		scale = 5;
        	
        	else if( scale < 1 )
        		scale = 1;
        	
        	return scale;
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int XP
        {
            get { return m_XP; }
            set { m_XP = value; LevelSystem.CheckLevel( this ); }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int NextLevel
        {
            get { return m_NextLevel; }
            set { m_NextLevel = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime WarningTime
        {
            get { return m_WarningTime; }
            set { m_WarningTime = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Warned
        {
            get { return m_Warned; }
            set { m_Warned = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime BribingTime
        {
            get { return m_BribingTime; }
            set { m_BribingTime = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int EmployerFeatLevel
        {
            get { return m_EmployerFeatLevel; }
            set { m_EmployerFeatLevel = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastSeen
        {
            get { return m_LastSeen; }
            set { m_LastSeen = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime HiringTime
        {
            get { return m_HiringTime; }
            set { m_HiringTime = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile Employer
        {
            get { return m_Employer; }
            set { m_Employer = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Bribed
        {
            get { return m_Bribed; }
            set { m_Bribed = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string TargetsName
        {
            get { return m_TargetsName; }
            set { m_TargetsName = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool CanBeInformant
        {
            get { return m_CanBeInformant; }
            set { m_CanBeInformant = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool TakesLifeOnKill
        {
            get { return m_TakesLifeOnKill; }
            set { m_TakesLifeOnKill = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool IsSneaky
        {
            get { return m_IsSneaky; }
            set { m_IsSneaky = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool IsPredator
        {
            get
            { 
            	if( this is ISmallPredator || this is IMediumPredator || this is ILargePredator )
            		return true;
            	
            	return false;
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool IsPrey
        {
            get 
            { 
            	if( this is ISmallPrey || this is IMediumPrey || this is ILargePrey )
            		return true;
            	
            	return false;
            }
        }

		public virtual InhumanSpeech SpeechType{ get{ return null; } }

		public bool IsStabled
		{
			get{ return m_IsStabled; }
			set{ m_IsStabled = value; }
		}

		protected DateTime SummonEnd
		{
			get { return m_SummonEnd; }
			set { m_SummonEnd = value; }
		}

		public virtual Faction FactionAllegiance{ get{ return null; } }
		public virtual int FactionSilverWorth{ get{ return 30; } }

		#region Bonding
		public const bool BondingEnabled = true;

		public virtual bool IsBondable{ get{ return ( BondingEnabled && !Summoned ); } }
		public virtual TimeSpan BondingDelay{ get{ return TimeSpan.FromDays( 7.0 ); } }
		public virtual TimeSpan BondingAbandonDelay{ get{ return TimeSpan.FromDays( 1.0 ); } }

		public override bool CanRegenHits{ get{ return !m_IsDeadPet && base.CanRegenHits; } }
		public override bool CanRegenStam{ get{ return !m_IsDeadPet && base.CanRegenStam; } }
		public override bool CanRegenMana{ get{ return !m_IsDeadPet && base.CanRegenMana; } }

		public override bool IsDeadBondedPet{ get{ return m_IsDeadPet; } }

		private bool m_IsBonded;
		private bool m_IsDeadPet;
		private DateTime m_BondingBegin;
		private DateTime m_OwnerAbandonTime;
		
		public virtual bool WaitForRess{ get{ return (Lives >= 0); } }
		
		private int m_Lives;
		private DateTime m_TimeOfDeath;
        private Timer m_RessTimer;
        private DateTime m_RessTime;
        
        private static TimeSpan m_DefaultRessTime = TimeSpan.FromMinutes(1);

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime TimeOfDeath
        {
            get { return m_TimeOfDeath; }
            set { m_TimeOfDeath = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
		public int Lives
		{
			get{ return m_Lives; }
			set{ m_Lives = value; }
		}
		
		public void BeginRess( TimeSpan delay, Container c )
        {
            if( m_RessTimer != null )
                m_RessTimer.Stop();

            m_RessTime = DateTime.Now + delay;

            m_RessTimer = new InternalTimer( this, c, delay );
            m_RessTimer.Start();
        }
		
		private class InternalTimer : Timer
        {
            private BaseCreature m_Creature;
            private Container m_Corpse;

            public InternalTimer( BaseCreature bc, Container corpse, TimeSpan delay )
                : base( delay )
            {
                m_Creature = bc;
                m_Corpse = corpse;
                Priority = TimerPriority.FiveSeconds;
                bc.Corpse = corpse;
            }

            protected override void OnTick()
            {
            	if( m_Creature != null && m_Creature.ControlMaster != null && m_Corpse != null )
            	{
	            	m_Creature.Location = m_Corpse.Location;
		            m_Creature.ResurrectPet();
		            m_Creature.m_RessTimer = null;
		            m_Creature.Emote( "*slowly comes to*" );
	           		m_Creature.Animate( 21, 6, 1, false, false, 0 );
	           		
	           		if( m_Corpse != null )
	            	{
		            	m_Corpse.OnDoubleClick( m_Creature );
		            	m_Corpse.Delete();
	            	}
            	}
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile LastOwner
		{
			get
			{
				if ( m_Owners == null || m_Owners.Count == 0 )
					return null;

				return m_Owners[m_Owners.Count - 1];
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsBonded
		{
			get{ return (Controlled && ControlMaster != null); }
			set{ m_IsBonded = value; InvalidateProperties(); }
		}

		public bool IsDeadPet
		{
			get{ return m_IsDeadPet; }
			set{ m_IsDeadPet = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime BondingBegin
		{
			get{ return m_BondingBegin; }
			set{ m_BondingBegin = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime OwnerAbandonTime
		{
			get{ return m_OwnerAbandonTime; }
			set{ m_OwnerAbandonTime = value; }
		}
		#endregion

		public virtual double WeaponAbilityChance{ get{ return 0.4; } }

		public virtual WeaponAbility GetWeaponAbility()
		{
			return null;
		}

		#region Elemental Resistance/Damage

		public override int BasePhysicalResistance{ get{ return m_PhysicalResistance; } }
		public override int BaseFireResistance{ get{ return m_FireResistance; } }
		public override int BaseColdResistance{ get{ return m_ColdResistance; } }
		public override int BasePoisonResistance{ get{ return m_PoisonResistance; } }
		public override int BaseEnergyResistance{ get{ return m_EnergyResistance; } }
		public override int BaseBluntResistance{ get{ return m_BluntResistance; } }
		public override int BaseSlashingResistance{ get{ return m_SlashingResistance; } }
		public override int BasePiercingResistance{ get{ return m_PiercingResistance; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalResistanceSeed{ get{ return m_PhysicalResistance; } set{ m_PhysicalResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireResistSeed{ get{ return m_FireResistance; } set{ m_FireResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdResistSeed{ get{ return m_ColdResistance; } set{ m_ColdResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonResistSeed{ get{ return m_PoisonResistance; } set{ m_PoisonResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyResistSeed{ get{ return m_EnergyResistance; } set{ m_EnergyResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int BluntResistSeed{ get{ return m_BluntResistance; } set{ m_BluntResistance = value; UpdateResistances(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int SlashingResistSeed{ get{ return m_SlashingResistance; } set{ m_SlashingResistance = value; UpdateResistances(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int PiercingResistSeed{ get{ return m_PiercingResistance; } set{ m_PiercingResistance = value; UpdateResistances(); } }
		
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalDamage{ get{ return m_PhysicalDamage; } set{ m_PhysicalDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireDamage{ get{ return m_FireDamage; } set{ m_FireDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdDamage{ get{ return m_ColdDamage; } set{ m_ColdDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonDamage{ get{ return m_PoisonDamage; } set{ m_PoisonDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyDamage{ get{ return m_EnergyDamage; } set{ m_EnergyDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int BluntDamage{ get{ return m_BluntDamage; } set{ m_BluntDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SlashingDamage{ get{ return m_SlashingDamage; } set{ m_SlashingDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PiercingDamage{ get{ return m_PiercingDamage; } set{ m_PiercingDamage = value; } }

		public void VerifiedFavouriteStance( string stance )
		{
			m_FavouriteStance = stance;
		}
		
		public void VerifiedFavouriteManeuver( string maneuver )
		{
			m_FavouriteManeuver = maneuver;
		}
		
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string FavouriteStance
		{ 
			get{ return m_FavouriteStance; }
			set
			{
				if( value != null )
				{
					BaseStance stance = null;
					
					foreach( KeyValuePair<string, BaseStance> kvp in Misc.ImprovedAI.Commands.ValidStances )
		        	{
						if( kvp.Key.ToLower().Replace( " ", "" ) == value.ToLower().Replace( " ", "" ) )
		        			stance = kvp.Value;
		        	}
					
					if( stance != null && stance.CanUseThisStance(this) )
					{
						this.SetStance = stance;
						m_FavouriteStance = value;
					}
				}
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string FavouriteManeuver
		{ 
			get{ return m_FavouriteManeuver; } 
			set
			{
				if( value != null )
				{
					BaseCombatManeuver maneuver = null;
					
					foreach( KeyValuePair<string, BaseCombatManeuver> kvp in Misc.ImprovedAI.Commands.ValidManeuvers )
		        	{
						if( kvp.Key.ToLower().Replace( " ", "" ) == value.ToLower().Replace( " ", "" ) )
		        			maneuver = kvp.Value;
		        	}
					
					if( maneuver != null && maneuver.CanUseThisManeuver(this) )
					{
						this.SetManeuver = maneuver;
						m_FavouriteManeuver = value;
					}
				}
			}
		}
		
		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsParagon
		{
			get{ return m_Paragon; }
			set
			{
				if ( m_Paragon == value )
					return;
				else if ( value )
					Paragon.Convert( this );
				else
					Paragon.UnConvert( this );

				m_Paragon = value;

				InvalidateProperties();
			}
		}
		
		public string Technique{ get{ return m_Technique; } set{ m_Technique = value; } }
		public int TechniqueLevel{ get{ return m_TechniqueLevel; } set{ m_TechniqueLevel = value; } }

		public virtual FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public virtual PackInstinct PackInstinct{ get{ return PackInstinct.None; } }

		public List<Mobile> Owners { get { return m_Owners; } }

		public virtual bool AllowMaleTamer{ get{ return true; } }
		public virtual bool AllowFemaleTamer{ get{ return true; } }
		public virtual bool SubdueBeforeTame{ get{ return false; } }
		public virtual bool StatLossAfterTame{ get{ return SubdueBeforeTame; } }

		public virtual bool Commandable{ get{ return true; } }

		public virtual KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return null; } }
		public virtual int PoisonDuration { get { return 1; } }
		public virtual int PoisonActingSpeed { get { return 1; } }
		public virtual double HitPoisonChance{ get{ return 0.5; } }
		public virtual Poison PoisonImmune{ get{ return null; } }

		public virtual bool BardImmune{ get{ return false; } }
		public virtual bool Unprovokable{ get{ return BardImmune || m_IsDeadPet; } }
		public virtual bool Uncalmable{ get{ return BardImmune || m_IsDeadPet; } }

		public virtual bool BleedImmune{ get{ return false; } }
		public virtual double BonusPetDamageScalar{ get{ return 1.0; } }

		public virtual bool DeathAdderCharmable{ get{ return false; } }

		//TODO: Find the pub 31 tweaks to the DispelDifficulty and apply them of course.
		public virtual double DispelDifficulty{ get{ return 0.0; } } // at this skill level we dispel 50% chance
		public virtual double DispelFocus{ get{ return 20.0; } } // at difficulty - focus we have 0%, at difficulty + focus we have 100%

		#region Breath ability, like dragon fire breath
		private DateTime m_NextBreathTime;

		// Must be overriden in subclass to enable
		public virtual bool HasBreath{ get{ return false; } }

		// Base damage given is: CurrentHitPoints * BreathDamageScalar
		public virtual double BreathDamageScalar{ get{ return 0.1; } }

		// Min/max seconds until next breath
		public virtual double BreathMinDelay{ get{ return 10.0; } }
		public virtual double BreathMaxDelay{ get{ return 15.0; } }

		// Creature stops moving for 1.0 seconds while breathing
		public virtual double BreathStallTime{ get{ return 1.0; } }

		// Effect is sent 1.3 seconds after BreathAngerSound and BreathAngerAnimation is played
		public virtual double BreathEffectDelay{ get{ return 1.3; } }

		// Damage is given 1.0 seconds after effect is sent
		public virtual double BreathDamageDelay{ get{ return 1.0; } }

		public virtual int BreathRange{ get{ return RangePerception; } }

		// Damage types
		public virtual int BreathPhysicalDamage{ get{ return 0; } }
		public virtual int BreathFireDamage{ get{ return 100; } }
		public virtual int BreathColdDamage{ get{ return 0; } }
		public virtual int BreathPoisonDamage{ get{ return 0; } }
		public virtual int BreathEnergyDamage{ get{ return 0; } }
        public virtual int BreathBluntDamage { get { return 0; } }
        public virtual int BreathSlashingDamage { get { return 0; } }
        public virtual int BreathPiercingDamage { get { return 0; } }

		// Effect details and sound
		public virtual int BreathEffectItemID{ get{ return 0x36D4; } }
		public virtual int BreathEffectSpeed{ get{ return 5; } }
		public virtual int BreathEffectDuration{ get{ return 0; } }
		public virtual bool BreathEffectExplodes{ get{ return false; } }
		public virtual bool BreathEffectFixedDir{ get{ return false; } }
		public virtual int BreathEffectHue{ get{ return 0; } }
		public virtual int BreathEffectRenderMode{ get{ return 0; } }

		public virtual int BreathEffectSound{ get{ return 0x227; } }

		// Anger sound/animations
		public virtual int BreathAngerSound{ get{ return GetAngerSound(); } }
		public virtual int BreathAngerAnimation{ get{ return 12; } }

		public virtual void BreathStart( Mobile target )
		{
			BreathStallMovement();
			BreathPlayAngerSound();
			BreathPlayAngerAnimation();

			this.Direction = this.GetDirectionTo( target );

			Timer.DelayCall( TimeSpan.FromSeconds( BreathEffectDelay ), new TimerStateCallback( BreathEffect_Callback ), target );
		}

		public virtual void BreathStallMovement()
		{
			if ( m_AI != null )
				m_AI.NextMove = DateTime.Now + TimeSpan.FromSeconds( BreathStallTime );
		}

		public virtual void BreathPlayAngerSound()
		{
			PlaySound( BreathAngerSound );
		}

		public virtual void BreathPlayAngerAnimation()
		{
			Animate( BreathAngerAnimation, 5, 1, true, false, 0 );
		}

		public virtual void BreathEffect_Callback( object state )
		{
			Mobile target = (Mobile)state;

			if ( !target.Alive || !CanBeHarmful( target ) )
				return;

			BreathPlayEffectSound();
			BreathPlayEffect( target );

			Timer.DelayCall( TimeSpan.FromSeconds( BreathDamageDelay ), new TimerStateCallback( BreathDamage_Callback ), target );
		}

		public virtual void BreathPlayEffectSound()
		{
			PlaySound( BreathEffectSound );
		}

		public virtual void BreathPlayEffect( Mobile target )
		{
			Effects.SendMovingEffect( this, target, BreathEffectItemID,
				BreathEffectSpeed, BreathEffectDuration, BreathEffectFixedDir,
				BreathEffectExplodes, BreathEffectHue, BreathEffectRenderMode );
		}

		public virtual void BreathDamage_Callback( object state )
		{
			Mobile target = (Mobile)state;

            if (target is BaseCreature)
                return;

			if ( CanBeHarmful( target ) )
			{
				DoHarmful( target );
				BreathDealDamage( target );
			}
		}

        public virtual void BreathDealDamage(Mobile target)
        {
            FireEffect fire = new FireEffect();
            fire.OnExplode(this, null, BreathComputeDamage(), target.Location, target.Map);

            if (target is PlayerMobile && ((PlayerMobile)target).Evaded())
                return;

            AOS.Damage(target, this, BreathComputeDamage(), 0, 100, 0, 0, 0, 0, 0, 0);
        }

		public virtual int BreathComputeDamage()
		{
			int damage = (int)(Hits * BreathDamageScalar);
			
			return damage;
		}

        public virtual void OnXMLEvent( XMLEventType eventType )
        {
            OnXMLEvent( eventType, null );
        }

        public virtual void OnXMLEvent( XMLEventType eventType, object obj )
        {
            try
            {
                if( XMLEventsDatabase.ContainsKey( eventType ) )
                {
                    foreach( string code in XMLEventsDatabase[eventType] )
                    {
                        if( obj != null ) //If there is an object, let's invoke the code on it
                            XmlSpawner.ExecuteAction( obj, this, code );
                        else //Or else let's invoke it on the creature
                            XmlSpawner.ExecuteAction( this, this, code );
                    }
                }
            }
            catch( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        public virtual void PerformCustomBreathAttack( Mobile target )
        {
            BreathStallMovement();
            BreathPlayAngerSound();
            BreathPlayAngerAnimation();

            Direction = GetDirectionTo( target );

            Point3D location = new Point3D( target.X, target.Y, target.Z );
            Map map = target.Map;
            VisualTarget visualTarget = new VisualTarget( map, location );
            Timer.DelayCall( TimeSpan.FromSeconds( BreathEffectDelay ), new TimerStateCallback( CustomBreathEffect_Callback ), visualTarget );
        }

        public virtual void CustomBreathEffect_Callback( object state )
		{
			VisualTarget target = (VisualTarget)state;
			BreathPlayEffectSound();

            int hue = 0;

            if( RangedAttackType == RangedAttackType.BreathePoison )
                hue = 2596;

            if( RangedAttackType == RangedAttackType.BreatheCold )
                hue = 2971;

            if( RangedAttackType == RangedAttackType.BreatheEnergy )
                hue = 2832;

			MovingEffect( target, 14036, 5, 0, false, true, hue, 0 );

			Timer.DelayCall( TimeSpan.FromSeconds( (0.1 * GetDistanceToSqrt(target)) ), new TimerStateCallback( CustomBreathDamage_Callback ), target );
		}

        public virtual void CustomBreathDamage_Callback( object state )
        {
            VisualTarget target = (VisualTarget)state;

            int radius = 1 + (int)(Fame * 0.0001 );
            List<Mobile> list = new List<Mobile>();

            foreach( Mobile mob in target.GetMobilesInRange(radius) )
            {
                if( mob != this && CanBeHarmful( mob ) )
                    list.Add( mob );
            }

            for( int i = 0; i < list.Count; i++ )
            {
                DoHarmful( list[i] );
                CustomBreathDealDamage( list[i] );
            }

            OnXMLEvent( XMLEventType.OnCustomBreathAttack );
            OnXMLEvent( XMLEventType.OnCustomBreathAttackInvokeOnItem, target );

            Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerStateCallback( DeleteVisualTarget ), target );
        }

        public void DeleteVisualTarget( object state )
        {
            if( state == null )
                return;

            VisualTarget target = (VisualTarget)state;
            target.Delete();
        }

        public virtual void CustomBreathDealDamage( Mobile target )
        {
            Server.Items.FireEffectType effectType = Server.Items.FireEffectType.Fire;
            int fDamage = 0;
            int cDamage = 0;
            int pDamage = 0;
            int eDamage = 0;

            if( RangedAttackType == RangedAttackType.BreathePoison )
            {
                effectType = Server.Items.FireEffectType.Poison;
                pDamage = 100;
            }

            if( RangedAttackType == RangedAttackType.BreatheCold )
            {
                effectType = Server.Items.FireEffectType.Cold;
                cDamage = 100;
            }

            if( RangedAttackType == RangedAttackType.BreatheEnergy )
            {
                effectType = Server.Items.FireEffectType.Energy;
                eDamage = 100;
            }

            if( ( cDamage + eDamage + pDamage ) < 1 )
                fDamage = 100;

            if( CustomBreathType == CustomBreathType.Fire )
            {
                FireEffect fire = new FireEffect();
                fire.FireEffectType = effectType;
                fire.OnExplode( this, null, BreathComputeDamage(), target.Location, target.Map );
            }

            else if( CustomBreathType == CustomBreathType.Goo )
            {
                StickyGooEffect goo = new StickyGooEffect();
                goo.OnExplode( this, null, 100, target.Location, target.Map );
            }

            if( target is PlayerMobile && ( (PlayerMobile)target ).Evaded() )
                return;

            AOS.Damage(target, this, BreathComputeDamage(), 0, fDamage, cDamage, pDamage, eDamage, 0, 0, 0);
        }

        public virtual int CustomBreathComputeDamage()
        {
            int damage = (int)( Hits * BreathDamageScalar );

            return damage;
        }

		#endregion

		#region Spill Acid
		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage )
		{
			SpillAcid( duration, minDamage, maxDamage, null, 1, 1 );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, Mobile target )
		{
			SpillAcid( duration, minDamage, maxDamage, target, 1, 1 );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, int count )
		{
			SpillAcid( duration, minDamage, maxDamage, null, count, count );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, int minAmount, int maxAmount )
		{
			SpillAcid( duration, minDamage, maxDamage, null, minAmount, maxAmount );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, Mobile target, int count )
		{
			SpillAcid( duration, minDamage, maxDamage, target, count, count );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, Mobile target, int minAmount, int maxAmount )
		{
			if ( (target != null && target.Map == null) || this.Map == null )
				return;

			int pools = Utility.RandomMinMax( minAmount, maxAmount );

			for ( int i = 0; i < pools; ++i )
			{
				PoolOfAcid acid = new PoolOfAcid( duration, minDamage, maxDamage );

				if ( target != null && target.Map != null )
				{
					acid.MoveToWorld( target.Location, target.Map );
					continue;
				}

				bool validLocation = false;
				Point3D loc = this.Location;
				Map map = this.Map;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				acid.MoveToWorld( loc, map );
			}
		}
		#endregion

		#region Flee!!!
		private DateTime m_EndFlee;

		public DateTime EndFleeTime
		{
			get{ return m_EndFlee; }
			set{ m_EndFlee = value; }
		}

		public virtual void StopFlee()
		{
			m_EndFlee = DateTime.MinValue;
		}

		public virtual bool CheckFlee()
		{
			if ( m_EndFlee == DateTime.MinValue )
				return false;

			if ( DateTime.Now >= m_EndFlee )
			{
				StopFlee();
				return false;
			}

			return true;
		}

		public virtual void BeginFlee( TimeSpan maxDuration )
		{
			m_EndFlee = DateTime.Now + maxDuration;
		}
		#endregion

		public BaseAI AIObject{ get{ return m_AI; } }

		public const int MaxOwners = 5;

		public virtual OppositionGroup OppositionGroup
		{
			get{ return null; }
		}

		#region Friends
		public List<Mobile> Friends { get { return m_Friends; } }

		public virtual bool AllowNewPetFriend
		{
			get{ return ( m_Friends == null || m_Friends.Count < 5 ); }
		}

		public virtual bool IsPetFriend( Mobile m )
		{
			return ( m_Friends != null && m_Friends.Contains( m ) );
		}

		public virtual void AddPetFriend( Mobile m )
		{
			if ( m_Friends == null )
				m_Friends = new List<Mobile>( 8 );

			m_Friends.Add( m );
		}

		public virtual void RemovePetFriend( Mobile m )
		{
			if ( m_Friends != null )
				m_Friends.Remove( m );
		}

		public virtual bool IsFriend( Mobile m )
		{
			OppositionGroup g = this.OppositionGroup;

			if ( g != null && g.IsEnemy( this, m ) )
				return false;

			if ( !(m is BaseCreature) )
				return false;

			BaseCreature c = (BaseCreature)m;

			return ( m_iTeam == c.m_iTeam && ( (m_bSummoned || m_bControlled) == (c.m_bSummoned || c.m_bControlled) )/* && c.Combatant != this */);
		}
		#endregion

		#region Allegiance
		public virtual Ethics.Ethic EthicAllegiance { get { return null; } }

		public enum Allegiance
		{
			None,
			Ally,
			Enemy
		}

		public virtual Allegiance GetFactionAllegiance( Mobile mob )
		{
			if ( mob == null || mob.Map != Faction.Facet || FactionAllegiance == null )
				return Allegiance.None;

			Faction fac = Faction.Find( mob, true );

			if ( fac == null )
				return Allegiance.None;

			return ( fac == FactionAllegiance ? Allegiance.Ally : Allegiance.Enemy );
		}

		public virtual Allegiance GetEthicAllegiance( Mobile mob )
		{
			if ( mob == null || mob.Map != Faction.Facet || EthicAllegiance == null )
				return Allegiance.None;

			Ethics.Ethic ethic = Ethics.Ethic.Find( mob, true );

			if ( ethic == null )
				return Allegiance.None;

			return ( ethic == EthicAllegiance ? Allegiance.Ally : Allegiance.Enemy );
		}
		#endregion

		public void ClearInformantInfo()
		{
			this.Employer = null;
        	this.EmployerFeatLevel = 0;
        	this.Bribed = false;
        	this.BribingTime = DateTime.MinValue;
        	this.LastSeen = DateTime.MinValue;
        	this.Warned = false;
        	this.HiringTime = DateTime.MinValue;
		}

        private List<Mobile> m_ForcedVisibilityList = new List<Mobile>();
        public override bool CanSee( Mobile m )
        {
            int offset = 100;
            int sightRange = 0;

            if (m != this && m.Hidden && m.AccessLevel == AccessLevel && Utility.InUpdateRange(this, m))
            {
                if (this.InFieldOfVision(m))
                {
                    sightRange = 4;
                    offset = 0;

                    sightRange += ((IKhaerosMobile)this).Feats.GetFeatLevel(FeatList.Alertness);

                    offset += ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.Stealth);
                    offset += ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.Obfuscate) * 2;

                    if (m is BaseCreature)
                    {
                        if (((BaseCreature)m).IsSneaky)
                            offset += 6;
                    }

                    if ((sightRange - offset) >= (int)GetDistanceToSqrt(m.Location))
                    {
                        if (!m_ForcedVisibilityList.Contains(m))
                        {
                            m_ForcedVisibilityList.Add(m);
                            Send(new Network.MobileIncoming(this, m));

                            if (ObjectPropertyList.Enabled)
                            {
                                Send(m.OPLPacket);

                                foreach (Item item in m.Items)
                                    Send(item.OPLPacket);
                            }
                        }

                        return true;
                    }
                }
                else if (!this.InFieldOfVision(m) && (((IKhaerosMobile)this).Feats.GetFeatLevel(FeatList.Alertness) > 0))
                {
                    sightRange = ((IKhaerosMobile)this).Feats.GetFeatLevel(FeatList.Alertness) * 2;
                    offset = 0;

                    offset += ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.Obfuscate);
                    offset += ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.EnhancedStealth);
                    sightRange += this.RawInt / 50;

                    if ((sightRange - offset) >= (int)GetDistanceToSqrt(m.Location))
                    {
                        if (!m_ForcedVisibilityList.Contains(m))
                        {
                            m_ForcedVisibilityList.Add(m);
                            Send(new Network.MobileIncoming(this, m));

                            if (ObjectPropertyList.Enabled)
                            {
                                Send(m.OPLPacket);
                                foreach (Item item in m.Items)
                                    Send(item.OPLPacket);
                            }
                        }
                        return true;
                    }
                }
                else if (m_ForcedVisibilityList.Contains(m) && ((sightRange - offset) <= (int)GetDistanceToSqrt(m.Location)))
                {
                    m_ForcedVisibilityList.Remove(m);
                    Send(m.RemovePacket);
                }
            }

            return base.CanSee( m );
        }

        public bool InFieldOfVision(object o)
        {
            Direction thisDirection = this.Direction;
            Direction targetDirection = this.Direction;

            if (o is Item)
            {
                if ((o as Item).Visible)
                    return true;

                if (this.InRange((o as Item).Location, 6))
                    return true;
                else
                    targetDirection = this.GetDirectionTo(((Item)o).Location);
            }
            else if (o is Mobile)
            {
                if (!(o as Mobile).Hidden)
                    return true;

                if (this.InRange((o as Mobile).Location, 6))
                    return true;
                else
                    targetDirection = this.GetDirectionTo(((Mobile)o).Location);
            }

            switch (targetDirection)
            {
                case Direction.North:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return true;
                            case Direction.South: return false;
                            case Direction.East: return false;
                            case Direction.West: return false;
                            case Direction.Up: return true;
                            case Direction.Down: return false;
                            case Direction.Left: return false;
                            case Direction.Right: return true;
                        }
                        break;
                    }
                case Direction.South:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return false;
                            case Direction.South: return true;
                            case Direction.East: return false;
                            case Direction.West: return false;
                            case Direction.Up: return false;
                            case Direction.Down: return true;
                            case Direction.Left: return true;
                            case Direction.Right: return false;
                        }
                        break;
                    }
                case Direction.East:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return false;
                            case Direction.South: return false;
                            case Direction.East: return true;
                            case Direction.West: return false;
                            case Direction.Up: return false;
                            case Direction.Down: return true;
                            case Direction.Left: return false;
                            case Direction.Right: return true;
                        }
                        break;
                    }
                case Direction.West:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return false;
                            case Direction.South: return false;
                            case Direction.East: return false;
                            case Direction.West: return true;
                            case Direction.Up: return true;
                            case Direction.Down: return false;
                            case Direction.Left: return true;
                            case Direction.Right: return false;
                        }
                        break;
                    }
                case Direction.Up:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return true;
                            case Direction.South: return false;
                            case Direction.East: return false;
                            case Direction.West: return true;
                            case Direction.Up: return true;
                            case Direction.Down: return false;
                            case Direction.Left: return false;
                            case Direction.Right: return false;
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return false;
                            case Direction.South: return true;
                            case Direction.East: return true;
                            case Direction.West: return false;
                            case Direction.Up: return false;
                            case Direction.Down: return true;
                            case Direction.Left: return false;
                            case Direction.Right: return false;
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return false;
                            case Direction.South: return true;
                            case Direction.East: return false;
                            case Direction.West: return true;
                            case Direction.Up: return false;
                            case Direction.Down: return false;
                            case Direction.Left: return true;
                            case Direction.Right: return false;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        switch (thisDirection)
                        {
                            case Direction.North: return true;
                            case Direction.South: return false;
                            case Direction.East: return true;
                            case Direction.West: return false;
                            case Direction.Up: return false;
                            case Direction.Down: return false;
                            case Direction.Left: return false;
                            case Direction.Right: return true;
                        }
                        break;
                    }
            }

            return false;
        }
        
		public virtual bool IsEnemy( Mobile m )
		{
			OppositionGroup g = this.OppositionGroup;

			if ( g != null && g.IsEnemy( this, m ) )
				return true;

			if ( m is BaseGuard )
				return false;

			if ( GetFactionAllegiance( m ) == Allegiance.Ally )
				return false;

			Ethics.Ethic ourEthic = EthicAllegiance;
			Ethics.Player pl = Ethics.Player.Find( m, true );

			if ( pl != null && pl.IsShielded && ( ourEthic == null || ourEthic == pl.Ethic ) )
				return false;

			if ( !(m is BaseCreature) )
				return true;

			BaseCreature c = (BaseCreature)m;

			return ( m_iTeam != c.m_iTeam || ( (m_bSummoned || m_bControlled) != (c.m_bSummoned || c.m_bControlled) )/* || c.Combatant == this*/ );
		}

		public override string ApplyNameSuffix( string suffix )
		{
			if ( IsParagon )
			{
				if ( suffix.Length == 0 )
					suffix = "(Paragon)";
				else
					suffix = String.Concat( suffix, " (Paragon)" );
			}

			return base.ApplyNameSuffix( suffix );
		}

		public virtual bool CheckControlChance( Mobile m )
		{
			Loyalty += 1;
			return true;
		}

		public virtual bool CanBeControlledBy( Mobile m )
		{
			return ( GetControlChance( m ) > 0.0 );
		}

		public virtual double GetControlChance( Mobile m )
		{
			return 100.0;
		}

		private static Type[] m_AnimateDeadTypes = new Type[]
			{
				typeof( FleshJelly ), 
				typeof( Banshee ), typeof( Wraith ), typeof( SkeletalDragon ),
				typeof( FleshGolem ),
				typeof( SkeletalSoldier ), typeof( Mummy ),
				typeof( SkeletalLord ), typeof( LesserBoneGolem )
			};

		public virtual bool IsAnimatedDead
		{
			get
			{
				if ( !Summoned )
					return false;

				Type type = this.GetType();

				bool contains = false;

				for ( int i = 0; !contains && i < m_AnimateDeadTypes.Length; ++i )
					contains = ( type == m_AnimateDeadTypes[i] );

				return contains;
			}
		}

		public override void Damage( int amount, Mobile from )
		{
			int oldHits = this.Hits;

			if ( !this.Summoned && this.Controlled && 0.2 > Utility.RandomDouble() )
				amount = (int)(amount * BonusPetDamageScalar);

            //if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
            //    amount = (int)(amount * 1.25);

            //Mobile oath = Spells.Necromancy.BloodOathSpell.GetBloodOath( from );

            //if ( oath == this )
            //{
            //    amount = (int)(amount * 1.1);
            //    from.Damage( amount, from );
            //}
			
            //if( this.ShieldingMobile != null )
            //{
            //    IKhaerosMobile pm = this.ShieldingMobile as IKhaerosMobile;
				
            //    if( from != null && !from.Deleted && from.Alive && pm.ShieldedMobile == this && from.InRange( this.Location, 12 ) && !from.Blessed )
            //    {
            //        int dmgshield = Convert.ToInt32( amount * this.ShieldValue );
            //        amount -= dmgshield;
            //        from.Damage( dmgshield, from );
					
            //        if( !from.Mounted )
            //            from.Animate( 20, 5, 1, true, false, 0 );
            //    }
				
            //    else
            //        this.RemoveShieldOfSacrifice();
            //}

            if (this is IKhaerosMobile)
            {
                if (((IKhaerosMobile)this).JusticeAura != null)
                {
                    if (from != null && !from.Deleted && from.Alive && !from.Blessed && from.InRange(this.Location, 12))
                    {
                        from.Damage(amount, from);
                        base.Damage(0, from);
                    }
                }
                else
                    base.Damage(amount, from);
            }
            else
                base.Damage(amount, from);

			if ( SubdueBeforeTame && !Controlled )
			{
				if ( (oldHits > (this.HitsMax / 10)) && (this.Hits <= (this.HitsMax / 10)) )
					PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "* The creature has been beaten into subjugation! *" );
			}
		}

		public virtual bool DeleteCorpseOnDeath
		{
			get
			{
                if( m_HasNoCorpse )
                    return true;

				return !Core.AOS && m_bSummoned;
			}
		}

		public override void SetLocation( Point3D newLocation, bool isTeleport )
		{
			base.SetLocation( newLocation, isTeleport );

			if ( isTeleport && m_AI != null )
				m_AI.OnTeleported();
		}

		public override void OnBeforeSpawn( Point3D location, Map m )
		{
			//if ( Paragon.CheckConvert( this, location, m ) )
				//IsParagon = true;

			base.OnBeforeSpawn( location, m );
		}

		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive || IsDeadPet )
				return ApplyPoisonResult.Immune;

			if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
				poison = PoisonImpl.IncreaseLevel( poison );

			ApplyPoisonResult result = base.ApplyPoison( from, poison );

			if ( from != null && result == ApplyPoisonResult.Poisoned && PoisonTimer is PoisonImpl.PoisonTimer )
				(PoisonTimer as PoisonImpl.PoisonTimer).From = from;

			return result;
		}

		public override bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			if ( base.CheckPoisonImmunity( from, poison ) )
				return true;

			Poison p = this.PoisonImmune;

			return ( p != null && p.Level >= poison.Level );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Loyalty
		{
			get
			{
				return m_Loyalty;
			}
			set
			{
				m_Loyalty = Math.Min( Math.Max( value, 0 ), MaxLoyalty );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WayPoint CurrentWayPoint 
		{
			get
			{
				return m_CurrentWayPoint;
			}
			set
			{
				m_CurrentWayPoint = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point2D TargetLocation
		{
			get
			{
				return m_TargetLocation;
			}
			set
			{
				m_TargetLocation = value;
			}
		}

		public virtual Mobile ConstantFocus{ get{ return null; } }

		public virtual bool DisallowAllMoves
		{
			get
			{
				return false;
			}
		}

		public virtual bool InitialInnocent
		{
			get
			{
				return false;
			}
		}

		public virtual bool AlwaysMurderer
		{
			get
			{
				return false;
			}
		}

		public virtual bool AlwaysAttackable
		{
			get
			{
				return false;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int DamageMin{ get{ return m_DamageMin; } set{ m_DamageMin = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int DamageMax{ get{ return m_DamageMax; } set{ m_DamageMax = value; } }
/*
		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get
			{
				if ( m_HitsMax >= 0 )
					return m_HitsMax;

				return Str;
			}
		}
*/
		[CommandProperty( AccessLevel.GameMaster )]
		public int HitsMaxSeed
		{
			get{ return HitsMax; }
			set{ HitsMax = value; }
		}
/*
		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get
			{
				if ( m_StamMax >= 0 )
					return m_StamMax;

				return Dex;
			}
		}
*/
		[CommandProperty( AccessLevel.GameMaster )]
		public int StamMaxSeed
		{
			get{ return StamMax; }
			set{ StamMax = value; }
		}
/*
		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get
			{
				if ( m_ManaMax >= 0 )
					return m_ManaMax;

				return Int;
			}
		}
*/
		[CommandProperty( AccessLevel.GameMaster )]
		public int ManaMaxSeed
		{
			get{ return ManaMax; }
			set{ ManaMax = value; }
		}

		public virtual bool CanOpenDoors
		{
			get
			{
				return !this.Body.IsAnimal && !this.Body.IsSea;
			}
		}

		public virtual bool CanMoveOverObstacles
		{
			get
			{
				return Core.AOS || this.Body.IsMonster;
			}
		}

		public virtual bool CanDestroyObstacles
		{
			get
			{
				// to enable breaking of furniture, 'return CanMoveOverObstacles;'
				return false;
			}
		}

		public void Unpacify()
		{
			BardEndTime = DateTime.Now;
			BardPacified = false;
		}

		private HonorContext m_ReceivedHonorContext;

		public HonorContext ReceivedHonorContext{ get{ return m_ReceivedHonorContext; } set{ m_ReceivedHonorContext = value; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( BardPacified && (HitsMax - Hits) * 0.001 > Utility.RandomDouble() )
				Unpacify();

			int disruptThreshold;
			//NPCs can use bandages too!
			if( !Core.AOS )
				disruptThreshold = 0;
			else if( from != null && from.Player )
				disruptThreshold = 18;
			else
				disruptThreshold = 25;

			if( amount > disruptThreshold )
			{
				BandageContext c = BandageContext.GetContext( this );

				if( c != null )
					c.Slip();
			}

			if( Confidence.IsRegenerating( this ) )
				Confidence.StopRegenerating( this );

			WeightOverloading.FatigueOnDamage( this, amount );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && !willKill )
				speechType.OnDamage( this, amount );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetDamaged( from, amount );

            if(this != null && !this.Deleted)
                OnXMLEvent( XMLEventType.OnDamage );
            if (this != null && !this.Deleted && from != null && !from.Deleted)
                OnXMLEvent( XMLEventType.OnDamageInvokeOnMobile, from );

            if (this != null && !this.Deleted)
            {
                ArrayList list = XmlAttach.FindAttachments(this, typeof(XmlAwe));

                for (int i = 0; i < list.Count; ++i)
                {
                    XmlAwe awe = list[i] as XmlAwe;
                    awe.Delete();
                }
            }

			base.OnDamage( amount, from, willKill );
		}
		
		public override void Kill()
		{
			if( this.NoDeath )
			{
				this.Hits = Math.Max( this.Hits, (int)( this.HitsMax * 0.5 ) );
				
				if( this.NoDeathMsg != null && this.NoDeathMsg.Length > 0 )
					this.Emote( this.NoDeathMsg );
				
				if( this.NoDeathSound > 0 )
					this.PlaySound( this.NoDeathSound );
				
				this.NoDeathCondition = true;
				
				return;
			}
			
			base.Kill();
		}

		public virtual void OnDamagedBySpell( Mobile from )
		{
		}

		#region Alter[...]Damage From/To

		public virtual void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
		}

		public virtual void AlterDamageScalarTo( Mobile target, ref double scalar )
		{
		}

		public virtual void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
		}

		public virtual void AlterSpellDamageTo( Mobile to, ref int damage )
		{
		}

		public virtual void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
		}

		public virtual void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
		}
		#endregion
		
		public virtual void CheckReflect( Mobile caster, ref bool reflect )
		{
		}

        public override bool CheckEquip(Item item)
        {
            if (HealthAttachment.HasHealthAttachment(this))
            {
                if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenLeftArm))
                {
                    if (item.Layer == Layer.TwoHanded)
                        return false;
                }
                else if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenRightArm))
                {
                    if (item.Layer == Layer.OneHanded)
                        return false;
                    if (item.Layer == Layer.FirstValid)
                        return false;
                    if (item.Layer == Layer.TwoHanded && item is BaseWeapon)
                        return false;
                }
            }

            return base.CheckEquip(item);
        }

        public bool AOEAttack;

        public virtual void TriggerAOE( Mobile defender )
        {
            if( AOEAttack )
                return;

            if( MeleeAttackType == MeleeAttackType.FrontalAOE || MeleeAttackType == MeleeAttackType.FullAOE )
            {
                int range = Math.Max( RangeFight, Weapon.MaxRange );
                ArrayList toAttack = new ArrayList();

                foreach( Mobile m in GetMobilesInRange( range ) )
                {
                    if( m == defender || !m.Alive || !CanSee( m ) || m.Blessed || !InLOS( m ) || BaseAI.AreAllies( this, m ) || !InRange( m, range ) )
                        continue;

                    if( MeleeAttackType == MeleeAttackType.FrontalAOE )
                    {
                        string mDir = BaseWeapon.GetPosition( m, this, true );

                        if( mDir == "back" || mDir == "flank" || mDir == "back flank" )
                            continue;
                    }

                    toAttack.Add( m );
                }

                AOEAttack = true;

                for( int i = 0; i < toAttack.Count; i++ )
                {
                    Mobile target = toAttack[i] as Mobile;
                    PerformMeleeDamageTo( target );
                }

                AOEAttack = false;
            }
        }

        public virtual void TriggerGrapple( Mobile defender )
        {
            if( !GrappleTimer.LegalGrappling( this ) || !GrappleTimer.LegalGrappling( defender ) )
                return;

            Emote( "*grapples {0}*", defender.Name );
            int reps = 1;

            if( MeleeAttackType == MeleeAttackType.TemporaryGrapple )
                reps += Math.Min( 5, (int)(Fame * 0.0001) );

            new GrappleTimer( this, defender, reps ).Start();
        }

        public virtual void OnGaveAttack( bool melee, bool parried, Mobile defender )
        {
            //Invoking all the events on the creature
            OnXMLEvent( XMLEventType.OnAttack ); 

            if( melee )
            {
                OnXMLEvent( XMLEventType.OnMeleeAttack );

                if( IsFirstAOEAttack )
                {
                    OnXMLEvent( XMLEventType.OnAOEAttack ); //Invoked when the mob aims at the main target on the AOE
                    TriggerAOE( defender ); //Invoked to trigger the extra AOE attacks
                }
            }
            else
                OnXMLEvent( XMLEventType.OnRangedAttack );

            if( parried )
            {
                OnXMLEvent( XMLEventType.OnAttackMissed );

                if( melee )
                {
                    OnXMLEvent( XMLEventType.OnMeleeAttackMissed );
                    
                    if( IsFirstAOEAttack )
                        OnXMLEvent( XMLEventType.OnAOEAttackMissed );
                }
                else
                    OnXMLEvent( XMLEventType.OnRangedAttackMissed );
            }
            else
            {
                OnXMLEvent( XMLEventType.OnAttackHit );

                if( melee )
                {
                    OnXMLEvent( XMLEventType.OnMeleeAttackHit );

                    if( MeleeAttackType == MeleeAttackType.PermanentGrapple || MeleeAttackType == MeleeAttackType.TemporaryGrapple )
                        TriggerGrapple( defender );

                    if( IsFirstAOEAttack )
                        OnXMLEvent( XMLEventType.OnAOEAttackHit );
                }
                else
                    OnXMLEvent( XMLEventType.OnRangedAttackHit );
            }

            //Invoking all the events on the mobile
            if( defender != null && defender.Alive && !defender.Deleted && !defender.Blessed )
            {
                OnXMLEvent( XMLEventType.OnAttackInvokeOnMobile, defender );

                if( melee )
                {
                    OnXMLEvent( XMLEventType.OnMeleeAttackInvokeOnMobile, defender );

                    if( IsFirstAOEAttack )
                        OnXMLEvent( XMLEventType.OnAOEAttackInvokeOnMobile, defender );
                }
                else
                    OnXMLEvent( XMLEventType.OnRangedAttackInvokeOnMobile, defender );

                if( parried )
                {
                    OnXMLEvent( XMLEventType.OnAttackMissedInvokeOnMobile, defender );

                    if( melee )
                    {
                        OnXMLEvent( XMLEventType.OnMeleeAttackMissedInvokeOnMobile, defender );

                        if( IsFirstAOEAttack )
                            OnXMLEvent( XMLEventType.OnAOEAttackMissedInvokeOnMobile, defender );
                    }
                    else
                        OnXMLEvent( XMLEventType.OnRangedAttackMissedInvokeOnMobile, defender );
                }

                else
                {
                    OnXMLEvent( XMLEventType.OnAttackHitInvokeOnMobile, defender );

                    if( melee )
                    {
                        OnXMLEvent( XMLEventType.OnMeleeAttackHitInvokeOnMobile, defender );

                        if( IsFirstAOEAttack )
                            OnXMLEvent( XMLEventType.OnAOEAttackHitInvokeOnMobile, defender );
                    }
                    else
                        OnXMLEvent( XMLEventType.OnRangedAttackHitInvokeOnMobile, defender );
                }
            }
        }

        public virtual void OnReceivedAttack( bool melee, bool parried, Mobile attacker )
        {
            //Invoking all the events on the creature
            OnXMLEvent( XMLEventType.OnGotAttacked );

            if( melee )
                OnXMLEvent( XMLEventType.OnGotMeleeAttacked );
            else
                OnXMLEvent( XMLEventType.OnGotRangedAttacked );

            if( parried )
            {
                OnXMLEvent( XMLEventType.OnGotAttackedMissed );

                if( melee )
                    OnXMLEvent( XMLEventType.OnGotMeleeAttackedMissed );
                else
                    OnXMLEvent( XMLEventType.OnGotRangedAttackedMissed );
            }
            else
            {
                OnXMLEvent( XMLEventType.OnGotAttackedHit );

                if( melee )
                    OnXMLEvent( XMLEventType.OnGotMeleeAttackedHit );
                else
                    OnXMLEvent( XMLEventType.OnGotRangedAttackedHit );
            }

            //Invoking all the events on the mobile
            if( attacker != null && attacker.Alive && !attacker.Deleted && !attacker.Blessed )
            {
                OnXMLEvent( XMLEventType.OnGotAttackedInvokeOnMobile, attacker );

                if( melee )
                    OnXMLEvent( XMLEventType.OnGotMeleeAttackedInvokeOnMobile, attacker );
                else
                    OnXMLEvent( XMLEventType.OnGotRangedAttackedInvokeOnMobile, attacker );

                if( parried )
                {
                    OnXMLEvent( XMLEventType.OnGotAttackedMissedInvokeOnMobile, attacker );

                    if( melee )
                        OnXMLEvent( XMLEventType.OnGotMeleeAttackedMissedInvokeOnMobile, attacker );
                    else
                        OnXMLEvent( XMLEventType.OnGotRangedAttackedMissedInvokeOnMobile, attacker );
                }

                else
                {
                    OnXMLEvent( XMLEventType.OnGotAttackedHitInvokeOnMobile, attacker );

                    if( melee )
                        OnXMLEvent( XMLEventType.OnGotMeleeAttackedHitInvokeOnMobile, attacker );
                    else
                        OnXMLEvent( XMLEventType.OnGotRangedAttackedHitInvokeOnMobile, attacker );
                }
            }
        }

        public bool IsFirstAOEAttack
        {
            get
            {
                if( (MeleeAttackType == MeleeAttackType.FrontalAOE || MeleeAttackType == MeleeAttackType.FullAOE) && !AOEAttack )
                    return true;

                return false;
            }
        }

        public virtual void PerformMeleeDamageTo( Mobile target )
        {
            CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( this );
            csa.Opponent = target;
            csa.FinishAttack();
        }
		
		public virtual bool HasFur { get{ return false; } }

		/*public virtual Type[] ToxinIngredients
		{
			get{ return new Type[] { }; }
		}
		
		public virtual Type[] BodyParts
		{
			get{ return new Type[] { }; }
		}
		
		public virtual void AddMeat( List<Item> list )
		{
		}
		
		public virtual List<Item> GetBodyParts( Mobile from, Corpse corpse )
		{
			List<Item> retList = new List<Item>();
			foreach ( Type type in ToxinIngredients )
			{
				BaseToxinIngredient ingredient = Activator.CreateInstance(type) as BaseToxinIngredient;
				if ( ingredient != null )
				{
					if ( from.Skills[SkillName.Poisoning].Fixed >= ingredient.SkillRequired )
						retList.Add( ingredient );
					else
						ingredient.Delete();
				}
			}
			return retList;
		}*/
		
		public virtual int MeatID { get { return 0; } }
		public virtual int MeatHue { get { return 0; } }

		public virtual void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			int feathers = Feathers;
			int wool = Wool;
			int meat = Meat;
			int hides = Hides;
			int fur = ( this.HasFur ? hides : 0 );
			int scales = Scales;
			int bones = Bones;

			if ( corpse.Map == Map.Felucca )
			{
				feathers *= 2;
				wool *= 2;
				hides *= 2;
			}
			
			if( bones != 0 )
			{
				bpc.DropItem( new Bone( bones ) );
				//from.SendMessage( "You dismember the creature and can now extract the bones from the corpse." );
			}

			if ( feathers != 0 )
			{
				bpc.DropItem( new Feather( feathers ) );
				//from.SendLocalizedMessage( 500479 ); // You pluck the bird. The feathers are now on the corpse.
			}

			if ( wool != 0 )
			{
				bpc.DropItem( new Wool( wool ) );
				//from.SendLocalizedMessage( 500483 ); // You shear it, and the wool is now on the corpse.
			}

			if ( meat != 0 )
			{
				if ( MeatType == MeatType.Ribs )
					bpc.DropItem( new RawRibs( meat ) );
				else if ( MeatType == MeatType.Bird )
					bpc.DropItem( new RawBird( meat ) );
				else if ( MeatType == MeatType.LambLeg )
					bpc.DropItem( new RawLambLeg( meat ) );

				//from.SendLocalizedMessage( 500467 ); // You carve some meat, which remains on the corpse.
			}

			if ( hides != 0 )
			{
				if ( HideType == HideType.Regular )
					bpc.DropItem( new Hides( hides ) );
				else if ( HideType == HideType.Thick )
					bpc.DropItem( new ThickHides( hides ) );
				else if ( HideType == HideType.Beast )
					bpc.DropItem( new BeastHides( hides ) );
				else if ( HideType == HideType.Scaled )
					bpc.DropItem( new ScaledHides( hides ) );

				//from.SendLocalizedMessage( 500471 ); // You skin it, and the hides are now in the corpse.
			}
			
			if ( fur != 0 )
			{
				bpc.DropItem( new Fur( fur ) );
			}

			if ( scales != 0 )
			{
				ScaleType sc = this.ScaleType;

				switch ( sc )
				{
					case ScaleType.Red:		bpc.DropItem( new RedScales( scales ) ); break;
					case ScaleType.Yellow:	bpc.DropItem( new YellowScales( scales ) ); break;
					case ScaleType.Black:	bpc.DropItem( new BlackScales( scales ) ); break;
					case ScaleType.Green:	bpc.DropItem( new GreenScales( scales ) ); break;
					case ScaleType.White:	bpc.DropItem( new WhiteScales( scales ) ); break;
					case ScaleType.Blue:	bpc.DropItem( new BlueScales( scales ) ); break;
					case ScaleType.All:
					{
						bpc.DropItem( new RedScales( scales ) );
						bpc.DropItem( new YellowScales( scales ) );
						bpc.DropItem( new BlackScales( scales ) );
						bpc.DropItem( new GreenScales( scales ) );
						bpc.DropItem( new WhiteScales( scales ) );
						bpc.DropItem( new BlueScales( scales ) );
						break;
					}
				}

				//from.SendMessage( "You cut away some scales, but they remain on the corpse." );
			}
			
			if( m_CustomSkinnableParts != null )
			{
				foreach( Item item in m_CustomSkinnableParts )
					bpc.DropItem( item );
			}
		}
		
		public virtual void OnCarve( Mobile from, Corpse corpse )
		{
			if ( !from.InRange( corpse.Location, 1 ) )
			{
				from.SendMessage( "You are too far away." );
				return;
			}
			
			BodyPartsContainer bpc = corpse.FindItemByType( typeof( BodyPartsContainer ) ) as BodyPartsContainer;
			if (bpc == null)
			{
				bpc = new BodyPartsContainer();
				corpse.DropItem( bpc );
			}
			
			if ( !corpse.Carved && !Summoned && !IsBonded )
			{
				AddBodyParts( bpc, corpse );
				corpse.Carved = true;
			}
			
			if ( bpc.TotalItems > 0 )
			{
				from.CloseGump( typeof( SkinningGump ) );
				from.SendGump( new SkinningGump( corpse, corpse.FindItemByType( typeof( BodyPartsContainer ) ) as BodyPartsContainer, from ) );
			}
			else
				from.SendMessage( "There's nothing left to carve." );
			
			PlayerMobile pm = from as PlayerMobile;
			if ( pm != null )
			{
                OnXMLEvent( XMLEventType.OnCarveInvokeOnMobile, pm );
                OnXMLEvent( XMLEventType.OnCarveInvokeOnItem, corpse );
                OnXMLEvent( XMLEventType.OnCarve );
			}
		}

		public const int DefaultRangePerception = 16;
		public const int OldRangePerception = 10;

		public BaseCreature(AIType ai,
			FightMode mode,
			int iRangePerception,
			int iRangeFight,
			double dActiveSpeed, 
			double dPassiveSpeed)
		{
			if ( iRangePerception == OldRangePerception )
				iRangePerception = DefaultRangePerception;

			m_Loyalty = MaxLoyalty; // Wonderfully Happy

			m_CurrentAI = ai;
			m_DefaultAI = ai;

			m_iRangePerception = iRangePerception;
			m_iRangeFight = iRangeFight;
			
			m_FightMode = mode;

			m_iTeam = 0;

			SpeedInfo.GetSpeeds( this, ref dActiveSpeed, ref dPassiveSpeed );

			m_dActiveSpeed = dActiveSpeed;
			m_dPassiveSpeed = dPassiveSpeed;
			m_dCurrentSpeed = dPassiveSpeed;

			m_bDebugAI = false;

			m_arSpellAttack = new List<Type>();
			m_arSpellDefense = new List<Type>();

			m_bControlled = false;
			m_ControlMaster = null;
			m_ControlTarget = null;
			m_ControlOrder = OrderType.None;

			m_bTamable = false;

			m_Owners = new List<Mobile>();

			m_NextReacquireTime = DateTime.Now + ReacquireDelay;

			ChangeAIType(AI);

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnConstruct( this );

			GenerateLoot( true );
			
			Hunger = 50;
			Level = 1;
			Lives = 1;
			
			if( this is IBrigand )
				this.CreatureGroup = CreatureGroup.Brigand;
			
			else if( this is IMinotaur )
				this.CreatureGroup = CreatureGroup.Minotaur;
			
			else if( this is IBeastman )
				this.CreatureGroup = CreatureGroup.Beastman;
			
			else if( this is IGoblin )
				this.CreatureGroup = CreatureGroup.Goblin;
			
			else if( this is IOgre )
				this.CreatureGroup = CreatureGroup.Ogre;
			
			else if( this is IUndead )
				this.CreatureGroup = CreatureGroup.Undead;
				
			else if( this is IYuanTi )
				this.CreatureGroup = CreatureGroup.YuanTi;
			
			else if( this is IGiant )
				this.CreatureGroup = CreatureGroup.Giant;
			
			else if( this is IDraconic )
				this.CreatureGroup = CreatureGroup.Draconic;
			
			else if( this is IDraconic )
				this.CreatureGroup = CreatureGroup.Draconic;
			
			else if( this is ICanine )
				this.CreatureGroup = CreatureGroup.Canine;
			
			else if( this is ISerpent )
				this.CreatureGroup = CreatureGroup.Serpent;
			
			else if( this is IRodent )
				this.CreatureGroup = CreatureGroup.Rodent;
			
			else if( this is IGoatman )
				this.CreatureGroup = CreatureGroup.Goatman;
			
			else if( this is ITroll )
				this.CreatureGroup = CreatureGroup.Troll;
			
			else if( this is ITroglin )
				this.CreatureGroup = CreatureGroup.Troglin;
			
			else if( this is ISpider )
				this.CreatureGroup = CreatureGroup.Spider;
			
			else if( this is IDrider )
				this.CreatureGroup = CreatureGroup.Drider;
			
			else if( this is IFormian )
				this.CreatureGroup = CreatureGroup.Formian;
			
			else if( this is IKobold )
				this.CreatureGroup = CreatureGroup.Kobold;
			
			else if( this is IBeholder )
				this.CreatureGroup = CreatureGroup.Beholder;
			
			else if( this is IFeline )
				this.CreatureGroup = CreatureGroup.Feline;
			
			else if( this is IBear )
				this.CreatureGroup = CreatureGroup.Bear;
			
			else if( this is IAbyssal )
				this.CreatureGroup = CreatureGroup.Abyssal;
			
			else if( this is IElemental )
				this.CreatureGroup = CreatureGroup.Elemental;
			
			else if( this is IReptile )
				this.CreatureGroup = CreatureGroup.Reptile;
			
			else if( this is IGiantBug )
				this.CreatureGroup = CreatureGroup.GiantBug;
			
			else if( this is ICelestial )
				this.CreatureGroup = CreatureGroup.Celestial;
			
			else if( this is IInsularii )
				this.CreatureGroup = CreatureGroup.Insularii;
			
			else if( this is SocietyGuard )
				this.CreatureGroup = CreatureGroup.Society;
			
			else if( this is BrotherhoodGuard )
				this.CreatureGroup = CreatureGroup.Brotherhood;
		}

		public BaseCreature( Serial serial ) : base( serial )
		{
			m_arSpellAttack = new List<Type>();
			m_arSpellDefense = new List<Type>();

			m_bDebugAI = false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 58 ); // version

            writer.Write((int)m_Orders.Count); // Version 58
            for (int oCount = 0; oCount < m_Orders.Count; oCount++)
                OrderInfo.Serialize(writer, m_Orders[oCount]);

            writer.Write((int)m_Deadly); // Version 56
            writer.Write((int)m_Nation); // Version 55
            writer.Write((GovernmentEntity)m_Government); // Version 55
            writer.Write((CustomGuildStone)m_Organization); // Version 55

            writer.Write( (int) m_AuraType );
            writer.Write( (int) m_CustomBreathType );
            writer.Write( (int) m_ManeuverResistance );
            writer.Write( (bool) m_CantInterrupt );
            writer.Write( (bool) m_CantParry );
            writer.Write( (bool) m_HasNoCorpse );

            writer.Write( (int) m_SecondaryWikiConfigs.Count );

            foreach( string st in m_SecondaryWikiConfigs )
                writer.Write( st );

            writer.Write( (int) m_XMLEventsDatabase.Count );

            foreach( KeyValuePair<XMLEventType, List<string>> kvp in XMLEventsDatabase )
            {
                writer.Write( (int) kvp.Key );
                writer.Write( (int) kvp.Value.Count );

                foreach( string st in kvp.Value )
                    writer.Write( (string) st );
            }

            writer.Write( (int)m_RangedAttackType );
            writer.Write( m_CustomSkinnableParts );
            writer.Write( (bool) m_NoWoundedMovePenalty );
            writer.Write( (int) m_MeleeAttackType );
			writer.Write( (string) m_WikiConfig );
			writer.Write( (string) m_Technique );
			writer.Write( (int) m_TechniqueLevel );
			writer.Write( (Item) m_StableTicket );
			writer.Write( (Mobile) m_StabledOwner );
			writer.Write( (bool) m_ReceivedNewLoot );
			writer.WriteDeltaTime( m_TimeOfDeath );

            writer.Write( m_RessTimer != null );

            if( m_RessTimer != null )
                writer.WriteDeltaTime( m_RessTime );
			
			writer.Write( (int) m_Lives );
			
			writer.Write( (DateTime) m_ReleaseTime );
			writer.Write( (bool) m_MarkedForTermination );
			writer.Write( (string) m_FavouriteStance );
			writer.Write( (string) m_FavouriteManeuver );
			
			CombatStyles.Serialize( writer, CombatStyles );
			
			Feats.Serialize( writer, Feats );
			
			writer.Write( (bool) m_FixedScales );
			
			writer.Write( (bool) m_NoDeathCondition );
			
			writer.Write( (bool) m_NoDeath );
			writer.Write( (string) m_NoDeathMsg );
			writer.Write( (int) m_NoDeathSound );
			
			writer.Write( (bool) this.Frozen );
			
			writer.Write( (DateTime) m_VanishTime );
			writer.Write( (string) m_VanishEmote );
			
			writer.Write( (int) m_CreatureGroup );
			
			writer.Write( (bool) m_IsSneaky );
			
			writer.Write( (bool) m_TakesLifeOnKill );
			
			writer.Write( (string) m_Description );

            writer.Write( (int) m_Intimidated );

            writer.Write( (bool) m_IsHuntingHound );

            writer.Write( (int) m_XPScale );

            writer.Write( (int) m_StatScale );

            writer.Write( (int) m_SkillScale );

            writer.Write( (int) m_Level );

            writer.Write( (int) m_XP );

            writer.Write( (int) m_NextLevel );

            writer.Write( (bool) m_Warned );

            writer.Write( (DateTime) m_WarningTime );

            writer.Write( (DateTime) m_BribingTime );

            writer.Write( (int) m_EmployerFeatLevel );

            writer.Write( (string) m_TargetsName );

            writer.Write( (Mobile) m_Employer );

            writer.Write( (DateTime) m_HiringTime );

            writer.Write( (bool) m_Bribed );
           
            writer.Write( (DateTime) m_LastSeen );

            writer.Write( (bool) m_CanBeInformant );

			writer.Write( (int) m_CurrentAI );
			writer.Write( (int) m_DefaultAI );

			writer.Write( (int) m_iRangePerception );
			writer.Write( (int) m_iRangeFight );

			writer.Write( (int) m_iTeam );

			writer.Write( (double) m_dActiveSpeed );
			writer.Write( (double) m_dPassiveSpeed );
			writer.Write( (double) m_dCurrentSpeed );

			writer.Write( (int) m_pHome.X );
			writer.Write( (int) m_pHome.Y );
			writer.Write( (int) m_pHome.Z );

			// Version 1
			writer.Write( (int) m_iRangeHome );

			int i = 0;

			writer.Write( (int) m_arSpellAttack.Count );
			for ( i=0; i< m_arSpellAttack.Count; i++ )
			{
				writer.Write( m_arSpellAttack[i].ToString() );
			}

			writer.Write( (int) m_arSpellDefense.Count );
			for ( i=0; i< m_arSpellDefense.Count; i++ )
			{
				writer.Write( m_arSpellDefense[i].ToString() );
			}

			// Version 2
			writer.Write( (int) m_FightMode );

			writer.Write( (bool) m_bControlled );
			writer.Write( (Mobile) m_ControlMaster );
			writer.Write( (Mobile) m_ControlTarget );
			writer.Write( (Point3D) m_ControlDest );
			writer.Write( (int) m_ControlOrder );
			writer.Write( (double) m_dMinTameSkill );
			// Removed in version 9
			//writer.Write( (double) m_dMaxTameSkill );
			writer.Write( (bool) m_bTamable );
			writer.Write( (bool) m_bSummoned );

			if ( m_bSummoned )
				writer.WriteDeltaTime( m_SummonEnd );

			writer.Write( (int) m_iControlSlots );

			// Version 3
			writer.Write( (int)m_Loyalty );

			// Version 4 
			writer.Write( m_CurrentWayPoint );

			// Verison 5
			writer.Write( m_SummonMaster );

			// Version 6
			//writer.Write( (int) m_HitsMax );
			//writer.Write( (int) m_StamMax );
			//writer.Write( (int) m_ManaMax );
			writer.Write( (int) m_DamageMin );
			writer.Write( (int) m_DamageMax );

			// Version 7
			writer.Write( (int) m_PhysicalResistance );
			writer.Write( (int) m_PhysicalDamage );

			writer.Write( (int) m_FireResistance );
			writer.Write( (int) m_FireDamage );

			writer.Write( (int) m_ColdResistance );
			writer.Write( (int) m_ColdDamage );

			writer.Write( (int) m_PoisonResistance );
			writer.Write( (int) m_PoisonDamage );

			writer.Write( (int) m_EnergyResistance );
			writer.Write( (int) m_EnergyDamage );

			// Version 8
			writer.Write( m_Owners, true );

			// Version 10
			writer.Write( (bool) m_IsDeadPet );
			writer.Write( (bool) m_IsBonded );
			writer.Write( (DateTime) m_BondingBegin );
			writer.Write( (DateTime) m_OwnerAbandonTime );

			// Version 11
			writer.Write( (bool) m_HasGeneratedLoot );

			// Version 12
			writer.Write( (bool) m_Paragon );

			// Version 13
			writer.Write( (bool) ( m_Friends != null && m_Friends.Count > 0 ) );

			if ( m_Friends != null && m_Friends.Count > 0 )
				writer.Write( m_Friends, true );

			// Version 14
			writer.Write( (bool) m_RemoveIfUntamed );
			writer.Write( (int) m_RemoveStep );

            writer.Write((int) m_BluntResistance);
            writer.Write((int) m_BluntDamage);
            writer.Write((int) m_SlashingResistance);
            writer.Write((int) m_SlashingDamage);
            writer.Write((int) m_PiercingResistance);
            writer.Write((int) m_PiercingDamage);
		}

		private static double[] m_StandardActiveSpeeds = new double[]
			{
				0.175, 0.1, 0.15, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8
			};

		private static double[] m_StandardPassiveSpeeds = new double[]
			{
				0.350, 0.2, 0.4, 0.5, 0.6, 0.8, 1.0, 1.2, 1.6, 2.0
			};

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			int test = 0;

            if (version > 57)
            {
                m_Orders = new List<OrderInfo>();
                int orderCount = reader.ReadInt();
                for (int i = 0; i < orderCount; i++)
                {
                    OrderInfo newOrder = new OrderInfo("", 1, 1, this);
                    OrderInfo.Deserialize(reader, newOrder);
                    m_Orders.Add(newOrder);
                }
            }

            if (version > 55)
            {
                m_Deadly = reader.ReadInt();
            }

            if (version > 54)
            {
                m_Nation = (Nation)reader.ReadInt();
                m_Government = (GovernmentEntity)reader.ReadItem();
                m_Organization = (CustomGuildStone)reader.ReadItem();
            }

            if( version > 53 )
                m_AuraType = (AuraType)reader.ReadInt();

            if( version > 52 )
                m_CustomBreathType = (CustomBreathType)reader.ReadInt();

            if( version > 51 )
                m_ManeuverResistance = reader.ReadInt();

            if( version > 50 )
            {
                m_CantInterrupt = reader.ReadBool();
                m_CantParry = reader.ReadBool();
                m_HasNoCorpse = reader.ReadBool();
            }

            if( version > 49 )
            {
                int count = reader.ReadInt();

                for( int i = 0; i < count; i++ )
                    m_SecondaryWikiConfigs.Add( reader.ReadString() );

                count = reader.ReadInt();

                for( int i = 0; i < count; i++ )
                {
                    XMLEventType eventType = (XMLEventType)reader.ReadInt();
                    List<string> list = new List<string>();
                    int codeCount = reader.ReadInt();

                    for( int a = 0; a < codeCount; a++ )
                        list.Add( reader.ReadString() );

                    m_XMLEventsDatabase.Add( eventType, list );
                }
            }
                    
            if( version > 48 )
                m_RangedAttackType = (RangedAttackType)reader.ReadInt();
            
            if( version > 47 )
                m_CustomSkinnableParts = reader.ReadStrongItemList();

            if( version > 46 )
            {
                m_NoWoundedMovePenalty = reader.ReadBool();
                m_MeleeAttackType = (MeleeAttackType)reader.ReadInt();
            }

			if( version > 45 )
				m_WikiConfig = reader.ReadString();
			
			if( version > 44 )
			{
				m_Technique = reader.ReadString();
				m_TechniqueLevel = reader.ReadInt();
			}
			
			if( version > 43 )
				m_StableTicket = reader.ReadItem();
			
			if( version > 41 )
				m_StabledOwner = reader.ReadMobile();
			
			if( version > 40 )
				m_ReceivedNewLoot = reader.ReadBool();
			
			if( version > 39 )
			{
				m_TimeOfDeath = reader.ReadDeltaTime();
	
	            if( reader.ReadBool() )
	            	BeginRess( reader.ReadDeltaTime() - DateTime.Now, this.Corpse );

				m_Lives = reader.ReadInt();
			}
			
			if( version > 38 )
				m_ReleaseTime = reader.ReadDateTime();
			
			if( version > 37 )
				m_MarkedForTermination = reader.ReadBool();
			
			if( version > 36 )
			{
				m_FavouriteStance = reader.ReadString();
				m_FavouriteManeuver = reader.ReadString();
			}
			
			if( version > 34 )
				m_CombatStyles = new CombatStyles( reader );
			
			if( version > 32 )
				m_Feats = new Feats( reader, true );
			
			if( version > 33 )
				m_FixedScales = reader.ReadBool();
			
			if( version > 31 )
			{
				m_NoDeathCondition = reader.ReadBool();
				m_NoDeath = reader.ReadBool();
				m_NoDeathMsg = reader.ReadString();
				m_NoDeathSound = reader.ReadInt();
			}
			
			if( version > 30 )
				this.Frozen = reader.ReadBool();
			
			if( version > 27 )
			{
				m_VanishTime = reader.ReadDateTime();
				m_VanishEmote = reader.ReadString();
			}

			m_CreatureGroup = (CreatureGroup)reader.ReadInt();
			
			if( version < 29 )
			{
				test = reader.ReadInt();
				m_IsSneaky = test > 0;
			}
				
			else
				m_IsSneaky = reader.ReadBool();
			
			if( version < 29 )
			{
				test = reader.ReadInt();
				m_TakesLifeOnKill = test > 0;
			}
				
			else
				m_TakesLifeOnKill = reader.ReadBool();
					
			m_Description = reader.ReadString();
			
            m_Intimidated = reader.ReadInt();

            if( version < 29 )
			{
				test = reader.ReadInt();
				m_IsHuntingHound = test > 0;
			}
				
			else
				m_IsHuntingHound = reader.ReadBool();

            m_XPScale = reader.ReadInt();

            m_StatScale = reader.ReadInt();

            m_SkillScale = reader.ReadInt();

            m_Level = reader.ReadInt();

            m_XP = reader.ReadInt();

            m_NextLevel = reader.ReadInt();

            if( version < 29 )
			{
				test = reader.ReadInt();
				m_Warned = test > 0;
			}
				
			else
				m_Warned = reader.ReadBool();

            m_WarningTime = reader.ReadDateTime();

            m_BribingTime = reader.ReadDateTime();

            m_EmployerFeatLevel = reader.ReadInt();

            m_TargetsName = reader.ReadString();

            m_Employer = reader.ReadMobile();

            m_HiringTime = reader.ReadDateTime();

            if( version < 29 )
			{
				test = reader.ReadInt();
				m_Bribed = test > 0;
			}
				
			else
				m_Bribed = reader.ReadBool();

            m_LastSeen = reader.ReadDateTime();

            if( version < 29 )
			{
				test = reader.ReadInt();
				m_CanBeInformant = test > 0;
			}
				
			else
				m_CanBeInformant = reader.ReadBool();

			m_CurrentAI = (AIType)reader.ReadInt();
			m_DefaultAI = (AIType)reader.ReadInt();

			m_iRangePerception = reader.ReadInt();
			m_iRangeFight = reader.ReadInt();

			m_iTeam = reader.ReadInt();

			m_dActiveSpeed = reader.ReadDouble();
			m_dPassiveSpeed = reader.ReadDouble();
			m_dCurrentSpeed = reader.ReadDouble();

			if ( m_iRangePerception == OldRangePerception )
				m_iRangePerception = DefaultRangePerception;

			m_pHome.X = reader.ReadInt();
			m_pHome.Y = reader.ReadInt();
			m_pHome.Z = reader.ReadInt();
            			
			if ( version >= 1 )
			{
				m_iRangeHome = reader.ReadInt();

				int i, iCount;
				
				iCount = reader.ReadInt();
				for ( i=0; i< iCount; i++ )
				{
					string str = reader.ReadString();
					Type type = Type.GetType( str );

					if ( type != null )
					{
						m_arSpellAttack.Add( type );
					}
				}

				iCount = reader.ReadInt();
				for ( i=0; i< iCount; i++ )
				{
					string str = reader.ReadString();
					Type type = Type.GetType( str );

					if ( type != null )
					{
						m_arSpellDefense.Add( type );
					}			
				}
			}
			else
			{
				m_iRangeHome = 0;
			}

			if ( version >= 2 )
			{
				m_FightMode = ( FightMode )reader.ReadInt();

				m_bControlled = reader.ReadBool();
				m_ControlMaster = reader.ReadMobile();
				m_ControlTarget = reader.ReadMobile();
				m_ControlDest = reader.ReadPoint3D();
				m_ControlOrder = (OrderType) reader.ReadInt();

				m_dMinTameSkill = reader.ReadDouble();

				if ( version < 9 )
					reader.ReadDouble();

				m_bTamable = reader.ReadBool();
				m_bSummoned = reader.ReadBool();

				if ( m_bSummoned )
				{
					m_SummonEnd = reader.ReadDeltaTime();
					new UnsummonTimer( m_ControlMaster, this, m_SummonEnd - DateTime.Now ).Start();
				}

				m_iControlSlots = reader.ReadInt();
			}
			else
			{
				m_FightMode = FightMode.Closest;

				m_bControlled = false;
				m_ControlMaster = null;
				m_ControlTarget = null;
				m_ControlOrder = OrderType.None;
			}

			if ( version >= 3 )
				m_Loyalty = reader.ReadInt();
			else
				m_Loyalty = MaxLoyalty; // Wonderfully Happy

			if ( version >= 4 )
				m_CurrentWayPoint = reader.ReadItem() as WayPoint;

			if ( version >= 5 )
				m_SummonMaster = reader.ReadMobile();

			if ( version >= 6 )
			{
				//m_HitsMax = reader.ReadInt();
				//m_StamMax = reader.ReadInt();
				//m_ManaMax = reader.ReadInt();
				m_DamageMin = reader.ReadInt();
				m_DamageMax = reader.ReadInt();
			}

			if ( version >= 7 )
			{
				m_PhysicalResistance = reader.ReadInt();
				m_PhysicalDamage = reader.ReadInt();

				m_FireResistance = reader.ReadInt();
				m_FireDamage = reader.ReadInt();

				m_ColdResistance = reader.ReadInt();
				m_ColdDamage = reader.ReadInt();

				m_PoisonResistance = reader.ReadInt();
				m_PoisonDamage = reader.ReadInt();

				m_EnergyResistance = reader.ReadInt();
				m_EnergyDamage = reader.ReadInt();
			}

			if ( version >= 8 )
				m_Owners = reader.ReadStrongMobileList();
			else
				m_Owners = new List<Mobile>();

			if ( version >= 10 )
			{
				m_IsDeadPet = reader.ReadBool();
				m_IsBonded = reader.ReadBool();
				m_BondingBegin = reader.ReadDateTime();
				m_OwnerAbandonTime = reader.ReadDateTime();
			}

			if ( version >= 11 )
				m_HasGeneratedLoot = reader.ReadBool();
			else
				m_HasGeneratedLoot = true;

			if ( version >= 12 )
				m_Paragon = reader.ReadBool();
			else
				m_Paragon = false;

			if ( version >= 13 && reader.ReadBool() )
				m_Friends = reader.ReadStrongMobileList();
			else if ( version < 13 && m_ControlOrder >= OrderType.Unfriend )
				++m_ControlOrder;

			if ( version < 16 )
				Loyalty *= 10;

			double activeSpeed = m_dActiveSpeed;
			double passiveSpeed = m_dPassiveSpeed;

			SpeedInfo.GetSpeeds( this, ref activeSpeed, ref passiveSpeed );

			bool isStandardActive = false;
			for ( int i = 0; !isStandardActive && i < m_StandardActiveSpeeds.Length; ++i )
				isStandardActive = ( m_dActiveSpeed == m_StandardActiveSpeeds[i] );

			bool isStandardPassive = false;
			for ( int i = 0; !isStandardPassive && i < m_StandardPassiveSpeeds.Length; ++i )
				isStandardPassive = ( m_dPassiveSpeed == m_StandardPassiveSpeeds[i] );

			if ( isStandardActive && m_dCurrentSpeed == m_dActiveSpeed )
				m_dCurrentSpeed = activeSpeed;
			else if ( isStandardPassive && m_dCurrentSpeed == m_dPassiveSpeed )
				m_dCurrentSpeed = passiveSpeed;

			if ( isStandardActive && !m_Paragon )
				m_dActiveSpeed = activeSpeed;

			if ( isStandardPassive && !m_Paragon )
				m_dPassiveSpeed = passiveSpeed;

			if ( version >= 14 )
			{
				m_RemoveIfUntamed = reader.ReadBool();
				m_RemoveStep = reader.ReadInt();
			}

			if( version <= 14 && m_Paragon && Hue == 0x31 )
			{
				Hue = Paragon.Hue; //Paragon hue fixed, should now be 0x501.
			}

            m_BluntResistance = reader.ReadInt();
            m_BluntDamage = reader.ReadInt();
            m_SlashingResistance = reader.ReadInt();
            m_SlashingDamage = reader.ReadInt();
            m_PiercingResistance = reader.ReadInt();
            m_PiercingDamage = reader.ReadInt();

			CheckStatTimers();

			Timer.DelayCall( TimeSpan.FromSeconds( 5 ), new TimerCallback( WaitToChangeAI ) );

			AddFollowers();

			if ( IsAnimatedDead )
				Spells.Necromancy.AnimateDeadSpell.Register( m_SummonMaster, this );
			
			if( this.Level < 1 )
				this.Level = 1;
			
			if( this.Blessed && this.Alive )
				this.CanBeInformant = true;
			
			m_Intimidated = 0;
			
			if( !( this is Mercenary ) && version < 34 && this.Level > 1 )
			{
				int bonus = this.Level / 2;
				int rest = this.Level % 2;
				
				this.DamageMax += bonus + rest;
				this.DamageMin += bonus;
			}
			
			LevelSystem.FixStatsAndSkills( this );
			
			m_Deserialized = true;
		}

		public virtual void WaitToChangeAI()
		{
			ChangeAIType(m_CurrentAI);
		}

		public virtual bool IsHumanInTown()
		{
			return ( Body.IsHuman );
		}

		public virtual bool CheckGold( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
				return OnGoldGiven( from, (Gold)dropped );

			return false;
		}

		public virtual bool OnGoldGiven( Mobile from, Gold dropped )
		{
			if ( CheckTeachingMatch( from ) )
			{
				if ( Teach( m_Teaching, from, dropped.Amount, true ) )
				{
					dropped.Delete();
					return true;
				}
			}
			else if ( IsHumanInTown() )
			{
				Direction = GetDirectionTo( from );

				int oldSpeechHue = this.SpeechHue;

				this.SpeechHue = 0x23F;
				SayTo( from, "Thou art giving me gold?" );

				if ( dropped.Amount >= 400 )
					SayTo( from, "'Tis a noble gift." );
				else
					SayTo( from, "Money is always welcome." );

				this.SpeechHue = 0x3B2;
				SayTo( from, 501548 ); // I thank thee.

				this.SpeechHue = oldSpeechHue;

				dropped.Delete();
				return true;
			}

			return false;
		}

		public override bool ShouldCheckStatTimers{ get{ return false; } }

		#region Food
		private static Type[] m_Eggs = new Type[]
			{
				typeof( FriedEggs ), typeof( Eggs )
			};

		private static Type[] m_Fish = new Type[]
			{
				typeof( FishSteak ), typeof( RawFishSteak )
			};

		private static Type[] m_GrainsAndHay = new Type[]
			{
				typeof( Hay ), typeof( Wheat )
			};

		private static Type[] m_Meat = new Type[]
			{
				/* Cooked */
				typeof( Bacon ), typeof( CookedBird ), typeof( Sausage ),
				typeof( Ham ), typeof( Ribs ), typeof( LambLeg ),
				typeof( ChickenLeg ),

				/* Uncooked */
				typeof( RawBird ), typeof( RawRibs ), typeof( RawLambLeg ),
				typeof( RawChickenLeg ),

				/* Body Parts */
				typeof( Head ), typeof( LeftArm ), typeof( LeftLeg ),
				typeof( Torso ), typeof( RightArm ), typeof( RightLeg )
			};

		private static Type[] m_FruitsAndVegies = new Type[]
			{
				typeof( HoneydewMelon ), typeof( YellowGourd ), typeof( GreenGourd ),
				typeof( Banana ), typeof( Bananas ), typeof( Lemon ), typeof( Lime ),
				typeof( Dates ), typeof( Grapes ), typeof( Peach ), typeof( Pear ),
				typeof( Apple ), typeof( Watermelon ), typeof( Squash ),
				typeof( Cantaloupe ), typeof( Carrot ), typeof( Cabbage ),
				typeof( Onion ), typeof( Lettuce ), typeof( Pumpkin )
			};

		private static Type[] m_Gold = new Type[]
			{
				// white wyrms eat gold..
				typeof( Gold )
			};

		public virtual bool CheckFoodPreference( Item f )
		{
			if ( CheckFoodPreference( f, FoodType.Eggs, m_Eggs ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Fish, m_Fish ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.GrainsAndHay, m_GrainsAndHay ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Meat, m_Meat ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.FruitsAndVegies, m_FruitsAndVegies ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Gold, m_Gold ) )
				return true;

			return false;
		}

		public virtual bool CheckFoodPreference( Item fed, FoodType type, Type[] types )
		{
			if ( (FavoriteFood & type) == 0 )
				return false;

			Type fedType = fed.GetType();
			bool contains = false;

			for ( int i = 0; !contains && i < types.Length; ++i )
				contains = ( fedType == types[i] );

			return contains;
		}

		public virtual bool CheckFeed( Mobile from, Item dropped )
		{
			if ( !IsDeadPet && Controlled && (ControlMaster == from || IsPetFriend( from )) && (dropped is Food || dropped is Copper || dropped is CookableFood || dropped is Head || dropped is LeftArm || dropped is LeftLeg || dropped is Torso || dropped is RightArm || dropped is RightLeg || dropped is Hay || dropped is Wheat) )
			{
				Item f = dropped;

				if ( CheckFoodPreference( f ) )
				{
					int amount = f.Amount;

					if ( amount > 0 )
					{
						bool happier = false;

						int stamGain;

						if ( f is Copper )
							stamGain = amount - 50;
						else
							stamGain = (amount * 15) - 50;

						if ( stamGain > 0 )
							Stam += stamGain;

						if ( Core.SE )
						{
							if ( m_Loyalty < MaxLoyalty )
							{
								m_Loyalty = MaxLoyalty;
								happier = true;
							}
						}
						else
						{
							for ( int i = 0; i < amount; ++i )
							{
								if ( m_Loyalty < MaxLoyalty  && 0.5 >= Utility.RandomDouble() )
								{
									m_Loyalty += 10;
									happier = true;
								}
							}
						}

						if ( happier )
							SayTo( from, 502060 ); // Your pet looks happier.

						if ( Body.IsAnimal )
							Animate( 3, 5, 1, true, false, 0 );
						else if ( Body.IsMonster )
							Animate( 17, 5, 1, true, false, 0 );

						if ( IsBondable && !IsBonded )
						{
							Mobile master = m_ControlMaster;

							if ( master != null && master == from )	//So friends can't start the bonding process
							{
								if ( m_dMinTameSkill <= 29.1 || master.Skills[SkillName.AnimalTaming].Value >= m_dMinTameSkill || GetControlChance( master ) >= 1.0 )
								{
									if ( BondingBegin == DateTime.MinValue )
									{
										BondingBegin = DateTime.Now;
									}
									else if ( (BondingBegin + BondingDelay) <= DateTime.Now )
									{
										IsBonded = true;
										BondingBegin = DateTime.MinValue;
										from.SendLocalizedMessage( 1049666 ); // Your pet has bonded with you!
									}
								}
							}
						}

						dropped.Delete();
						return true;
					}
				}
			}

			return false;
		}

		#endregion

		public virtual bool CanAngerOnTame{ get{ return false; } }

		#region OnAction[...]
		public virtual void OnActionWander()
		{
            OnXMLEvent( XMLEventType.OnActionWander );
		}

		public virtual void OnActionCombat()
		{
			if( this.Combatant != null && (this.Combatant.Hidden || this.Combatant.Blessed) )
				this.Combatant = null;

            OnXMLEvent( XMLEventType.OnActionCombat );
		}

		public virtual void OnActionGuard()
		{
            OnXMLEvent( XMLEventType.OnActionGuard );
		}

		public virtual void OnActionFlee()
		{
            OnXMLEvent( XMLEventType.OnActionFlee );
		}

		public virtual void OnActionInteract()
		{
		}

		public virtual void OnActionBackoff()
		{
            OnXMLEvent( XMLEventType.OnActionBackoff );
		}
		#endregion

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
            OnXMLEvent( XMLEventType.OnDragDrop );
            OnXMLEvent( XMLEventType.OnDragDropInvokeOnItem, dropped );
            OnXMLEvent( XMLEventType.OnDragDropInvokeOnMobile, from );

			if ( CheckFeed( from, dropped ) )
				return true;
			else if ( CheckGold( from, dropped ) )
				return true;

			return base.OnDragDrop( from, dropped );
		}

		protected virtual BaseAI ForcedAI { get { return null; } }

		public  void ChangeAIType( AIType NewAI )
		{
			if ( m_AI != null )
				m_AI.m_Timer.Stop();

			if( ForcedAI != null )
			{
				m_AI = ForcedAI;
				return;
			}

			m_AI = null;

			switch ( NewAI )
			{
				case AIType.AI_Melee:
					m_AI = new MeleeAI(this);
					break;
				case AIType.AI_Animal:
					m_AI = new AnimalAI(this);
					break;
				case AIType.AI_Berserk:
					m_AI = new BerserkAI(this);
					break;
				case AIType.AI_Archer:
					m_AI = new ArcherAI(this);
					break;
				case AIType.AI_Healer:
					m_AI = new HealerAI(this);
					break;
				case AIType.AI_Vendor:
					m_AI = new VendorAI(this);
					break;
				case AIType.AI_Mage:
					m_AI = new MageAI(this);
					break;
				case AIType.AI_Predator:
					//m_AI = new PredatorAI(this);
					m_AI = new MeleeAI(this);
					break;
				case AIType.AI_Thief:
					m_AI = new ThiefAI(this);
					break;
				case AIType.AI_Lobotomized:
					m_AI = new LobotomizedAI(this);
					break;
			}
		}

		public void ChangeAIToDefault()
		{
			ChangeAIType(m_DefaultAI);
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AIType AI
		{
			get
			{
				return m_CurrentAI;
			}
			set
			{
				m_CurrentAI = value;

				if (m_CurrentAI == AIType.AI_Use_Default)
				{
					m_CurrentAI = m_DefaultAI;
				}
				
				ChangeAIType(m_CurrentAI);
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool Debug
		{
			get
			{
				return m_bDebugAI;
			}
			set
			{
				m_bDebugAI = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Team
		{
			get
			{
				return m_iTeam;
			}
			set
			{
				m_iTeam = value;
				
				OnTeamChange();
			}
		}

		public virtual void OnTeamChange()
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile FocusMob
		{
			get
			{
				return m_FocusMob;
			}
			set
			{
				m_FocusMob = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public FightMode FightMode
		{
			get
			{
				return m_FightMode;
			}
			set
			{
				m_FightMode = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangePerception
		{
			get
			{
				return m_iRangePerception;
			}
			set
			{
				m_iRangePerception = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangeFight
		{
			get
			{
				return m_iRangeFight;
			}
			set
			{
				m_iRangeFight = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangeHome
		{
			get
			{
				return m_iRangeHome;
			}
			set
			{
				m_iRangeHome = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual double ActiveSpeed
		{
			get
			{
				return m_dActiveSpeed;
			}
			set
			{
				m_dActiveSpeed = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual double PassiveSpeed
		{
			get
			{
				return m_dPassiveSpeed;
			}
			set
			{
				m_dPassiveSpeed = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double CurrentSpeed
		{
			get
			{
				return m_dCurrentSpeed;
			}
			set
			{
				if ( m_dCurrentSpeed != value )
				{
					m_dCurrentSpeed = value;

					if (m_AI != null)
						m_AI.OnCurrentSpeedChanged();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Home
		{
			get
			{
				return m_pHome;
			}
			set
			{
				m_pHome = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Controlled
		{
			get
			{
				return m_bControlled;
			}
			set
			{
				if ( m_bControlled == value )
					return;

				m_bControlled = value;

				Delta( MobileDelta.Noto );

				InvalidateProperties();
			}
		}

		public override void RevealingAction()
		{
			Spells.Sixth.InvisibilitySpell.RemoveTimer( this );

			base.RevealingAction();
		}

		public void RemoveFollowers()
		{
			if ( m_ControlMaster != null )
				m_ControlMaster.Followers -= ControlSlots;
			else if ( m_SummonMaster != null )
				m_SummonMaster.Followers -= ControlSlots;

			if ( m_ControlMaster != null && m_ControlMaster.Followers < 0 )
				m_ControlMaster.Followers = 0;

			if ( m_SummonMaster != null && m_SummonMaster.Followers < 0 )
				m_SummonMaster.Followers = 0;
		}

		public void AddFollowers()
		{
			if ( m_ControlMaster != null )
				m_ControlMaster.Followers += ControlSlots;
			else if ( m_SummonMaster != null )
				m_SummonMaster.Followers += ControlSlots;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile ControlMaster
		{
			get
			{
				return m_ControlMaster;
			}
			set
			{
				if ( m_ControlMaster == value )
					return;

                if( IsHuntingHound )
                {
                    Dog dog = this as Dog;

                    if( m_ControlMaster != null )
                    {
                        PlayerMobile pm = World.FindMobile( m_ControlMaster.Serial ) as PlayerMobile;
                        pm.HasHuntingHoundBonus = false;
                        pm.HuntingHound = null;
                    }
                    this.IsHuntingHound = false;
                }

				RemoveFollowers();
				m_ControlMaster = value;
				AddFollowers();

				Delta( MobileDelta.Noto );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile SummonMaster
		{
			get
			{
				return m_SummonMaster;
			}
			set
			{
				if ( m_SummonMaster == value )
					return;

				RemoveFollowers();
				m_SummonMaster = value;
				AddFollowers();

				Delta( MobileDelta.Noto );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile ControlTarget
		{
			get
			{
				return m_ControlTarget;
			}
			set
			{
				m_ControlTarget = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D ControlDest
		{
			get
			{
				return m_ControlDest;
			}
			set
			{
				m_ControlDest = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public OrderType ControlOrder
		{
			get
			{
				return m_ControlOrder;
			}
			set
			{
				m_ControlOrder = value;

				if ( m_AI != null )
					m_AI.OnCurrentOrderChanged();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardProvoked
		{
			get
			{
				return m_bBardProvoked;
			}
			set
			{
				m_bBardProvoked = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardPacified
		{
			get
			{
				return m_bBardPacified;
			}
			set
			{
				m_bBardPacified = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardMaster
		{
			get
			{
				return m_bBardMaster;
			}
			set
			{
				m_bBardMaster = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardTarget
		{
			get
			{
				return m_bBardTarget;
			}
			set
			{
				m_bBardTarget = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime BardEndTime
		{
			get
			{
				return m_timeBardEnd;
			}
			set
			{
				m_timeBardEnd = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double MinTameSkill
		{
			get
			{
				return m_dMinTameSkill;
			}
			set
			{
				m_dMinTameSkill = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Tamable
		{
			get
			{
				return m_bTamable && !m_Paragon;
			}
			set
			{
				m_bTamable = value;
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool Summoned
		{
			get
			{
				return m_bSummoned;
			}
			set
			{
				if ( m_bSummoned == value )
					return;

				m_NextReacquireTime = DateTime.Now;

				m_bSummoned = value;
				Delta( MobileDelta.Noto );

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int ControlSlots
		{
			get
			{
				return m_iControlSlots;
			}
			set
			{
				m_iControlSlots = value;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Poisoned
		{
			get
			{
				return ( XmlAttach.FindAttachment( this, typeof( PoisonAttachment ) ) != null );
			}
		}
		
		public override int GetPacketFlags()
		{
			int flags = 0x0;

			if ( Female )
				flags |= 0x02;

			if ( Poisoned )
				flags |= 0x04;

			if ( Blessed || YellowHealthbar )
				flags |= 0x08;

			if ( Warmode )
				flags |= 0x40;

			if ( Hidden )
				flags |= 0x80;

			return flags;
		}

		public virtual bool NoHouseRestrictions{ get{ return false; } }
		public virtual bool IsHouseSummonable{ get{ return false; } }

		#region Corpse Resources
		public virtual int Feathers{ get{ return 0; } }
		public virtual int Wool{ get{ return 0; } }

		public virtual MeatType MeatType{ get{ return MeatType.Ribs; } }
		public virtual int Meat{ get{ return 0; } }

		public virtual int Hides{ get{ return 0; } }
		public virtual HideType HideType{ get{ return HideType.Regular; } }

		public virtual int Scales{ get{ return 0; } }
		public virtual ScaleType ScaleType{ get{ return ScaleType.Red; } }
		
		public virtual int Bones{ get{ return 0; } }
		#endregion

		public virtual bool AutoDispel{ get{ return false; } }
		public virtual double AutoDispelChance{ get { return ((Core.SE) ? .10 : 1.0); } }

		public virtual bool IsScaryToPets{ get{ return false; } }
		public virtual bool IsScaredOfScaryThings{ get{ return true; } }

		public virtual bool CanRummageCorpses{ get{ return false; } }

		public virtual void OnGotMeleeAttack( Mobile attacker )
		{
			if ( AutoDispel && attacker is BaseCreature && ((BaseCreature)attacker).IsDispellable && AutoDispelChance > Utility.RandomDouble() )
				Dispel( attacker );
		}

		public virtual void Dispel( Mobile m )
		{
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
			Effects.PlaySound( m, m.Map, 0x201 );

			m.Delete();
		}

		public virtual bool DeleteOnRelease{ get{ return (m_bSummoned || (this.VanishTime != DateTime.MinValue)); } }

		public virtual void OnGaveMeleeAttack( Mobile defender )
		{
			if ( HitPoison != null )
				if ( HitPoisonChance >= Utility.RandomDouble() )
					PoisonEffect.Poison( defender, this, HitPoison, PoisonDuration, PoisonActingSpeed, false );

			if( AutoDispel && defender is BaseCreature && ((BaseCreature)defender).IsDispellable && AutoDispelChance > Utility.RandomDouble() )
				Dispel( defender );
		}
		
		public override bool CanBeHarmful( Mobile target )
		{
			int i = 0;
			
			if( this.Region is SanctuaryRegion )
				i++;
			
			if( target.Region is SanctuaryRegion )
				i++;
			
			if( i == 1 )
				return false;
			
			return base.CanBeHarmful( target );
		}

		public override void OnAfterDelete()
		{
			if ( m_AI != null )
			{
				if ( m_AI.m_Timer != null )
					m_AI.m_Timer.Stop();

				m_AI = null;
			}

			FocusMob = null;

			if ( IsAnimatedDead )
				Spells.Necromancy.AnimateDeadSpell.Unregister( m_SummonMaster, this );

			if ( m_RessTimer != null )
				m_RessTimer.Stop();

			m_RessTimer = null;
			
			base.OnAfterDelete();
		}

		public void DebugSay( string text )
		{
			if ( m_bDebugAI )
				this.PublicOverheadMessage( MessageType.Regular, 41, false, text );
		}

		public void DebugSay( string format, params object[] args )
		{
			if ( m_bDebugAI )
				this.PublicOverheadMessage( MessageType.Regular, 41, false, String.Format( format, args ) );
		}

		/*
		 * Will need to be givent a better name
		 * 
		 * This function can be overriden.. so a "Strongest" mobile, can have a different definition depending
		 * on who check for value
		 * -Could add a FightMode.Prefered
		 * 
		 */
		public virtual double GetValueFrom( Mobile m, FightMode acqType, bool bPlayerOnly )
		{
			if ( ( bPlayerOnly && m.Player ) ||  !bPlayerOnly )
			{
				switch( acqType )
				{
					case FightMode.Strongest : 
						return (m.Skills[SkillName.Tactics].Value + m.Str); //returns strongest mobile

					case FightMode.Weakest : 
						return -m.Hits; // returns weakest mobile

					default : 
						return -GetDistanceToSqrt( m ); // returns closest mobile
				}
			}
			else
			{
				return double.MinValue;
			}
		}

		// Turn, - for let, + for right
		// Basic for now, needs work
		public virtual void Turn(int iTurnSteps)
		{
			int v = (int)Direction;

			Direction = (Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80));
		}

		public virtual void TurnInternal(int iTurnSteps)
		{
			int v = (int)Direction;

			SetDirection( (Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80)) );
		}

		public bool IsHurt()
		{
			return ( Hits != HitsMax );
		}

		public double GetHomeDistance()
		{
			return GetDistanceToSqrt( m_pHome );
		}

		public virtual int GetTeamSize(int iRange)
		{
			int iCount = 0;

			foreach ( Mobile m in this.GetMobilesInRange( iRange ) )
			{
				if (m is BaseCreature)
				{
					if ( ((BaseCreature)m).Team == Team )
					{
						if ( !m.Deleted )
						{
							if ( m != this )
							{
								if ( CanSee( m ) )
								{
									iCount++;
								}
							}
						}
					}
				}
			}
			
			return iCount;
		}

		private class TameEntry : ContextMenuEntry
		{
			private BaseCreature m_Mobile;

			public TameEntry( Mobile from, BaseCreature creature ) : base( 6130, 6 )
			{
				m_Mobile = creature;

				Enabled = Enabled && ( from.Female ? creature.AllowFemaleTamer : creature.AllowMaleTamer );
			}

			public override void OnClick()
			{
				if ( !Owner.From.CheckAlive() )
					return;

				Owner.From.TargetLocked = true;
				SkillHandlers.AnimalTaming.DisableMessage = true;

				if ( Owner.From.UseSkill( SkillName.AnimalTaming ) )
					Owner.From.Target.Invoke( Owner.From, m_Mobile );

				SkillHandlers.AnimalTaming.DisableMessage = false;
				Owner.From.TargetLocked = false;
			}
		}
        
        public virtual void DisableManeuver()
        {
        	this.CombatManeuver = null;
        	this.OffensiveFeat = FeatList.None;
        }
        
        public virtual void RemoveShieldOfSacrifice()
		{
			this.ShieldingMobile = null;
			this.ShieldValue = 0.0;
		}

        public static void CheckBriberyAge( BaseCreature bc )
        {
            if( !bc.Bribed )
            {
                return;
            }

            TimeSpan maxspan = new TimeSpan( 0, 8, 0, 0, 0 );
            DateTime maxage = bc.BribingTime.Add( maxspan );

            if( DateTime.Compare( DateTime.Now, maxage ) > 0 )
            {
                bc.BribingTime = DateTime.MinValue;
                bc.Bribed = false;
            }
        }

		#region Teaching
		public virtual bool CanTeach{ get{ return false; } }

		public virtual bool CheckTeach( SkillName skill, Mobile from )
		{
			if ( !CanTeach )
				return false;

			if( skill == SkillName.Stealth && from.Skills[SkillName.Hiding].Base < ((Core.SE) ? 50.0 : 80.0) )
				return false;

			if ( skill == SkillName.ArmDisarmTraps && (from.Skills[SkillName.Lockpicking].Base < 50.0 || from.Skills[SkillName.DetectHidden].Base < 50.0) )
				return false;

			if ( !Core.AOS && (skill == SkillName.Concentration || skill == SkillName.Faith || skill == SkillName.Polearms) )
				return false;

			return true;
		}

		public enum TeachResult
		{
			Success,
			Failure,
			KnowsMoreThanMe,
			KnowsWhatIKnow,
			SkillNotRaisable,
			NotEnoughFreePoints
		}

		public virtual TeachResult CheckTeachSkills( SkillName skill, Mobile m, int maxPointsToLearn, ref int pointsToLearn, bool doTeach )
		{
			if ( !CheckTeach( skill, m ) || !m.CheckAlive() )
				return TeachResult.Failure;

			Skill ourSkill = Skills[skill];
			Skill theirSkill = m.Skills[skill];

			if ( ourSkill == null || theirSkill == null )
				return TeachResult.Failure;

			int baseToSet = ourSkill.BaseFixedPoint / 3;

			if ( baseToSet > 420 )
				baseToSet = 420;
			else if ( baseToSet < 200 )
				return TeachResult.Failure;

			if ( baseToSet > theirSkill.CapFixedPoint )
				baseToSet = theirSkill.CapFixedPoint;

			pointsToLearn = baseToSet - theirSkill.BaseFixedPoint;

			if ( maxPointsToLearn > 0 && pointsToLearn > maxPointsToLearn )
			{
				pointsToLearn = maxPointsToLearn;
				baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
			}

			if ( pointsToLearn < 0 )
				return TeachResult.KnowsMoreThanMe;

			if ( pointsToLearn == 0 )
				return TeachResult.KnowsWhatIKnow;

			if ( theirSkill.Lock != SkillLock.Up )
				return TeachResult.SkillNotRaisable;

			int freePoints = m.Skills.Cap - m.Skills.Total;
			int freeablePoints = 0;

			if ( freePoints < 0 )
				freePoints = 0;

			for ( int i = 0; (freePoints + freeablePoints) < pointsToLearn && i < m.Skills.Length; ++i )
			{
				Skill sk = m.Skills[i];

				if ( sk == theirSkill || sk.Lock != SkillLock.Down )
					continue;

				freeablePoints += sk.BaseFixedPoint;
			}

			if ( (freePoints + freeablePoints) == 0 )
				return TeachResult.NotEnoughFreePoints;

			if ( (freePoints + freeablePoints) < pointsToLearn )
			{
				pointsToLearn = freePoints + freeablePoints;
				baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
			}

			if ( doTeach )
			{
				int need = pointsToLearn - freePoints;

				for ( int i = 0; need > 0 && i < m.Skills.Length; ++i )
				{
					Skill sk = m.Skills[i];

					if ( sk == theirSkill || sk.Lock != SkillLock.Down )
						continue;

					if ( sk.BaseFixedPoint < need )
					{
						need -= sk.BaseFixedPoint;
						sk.BaseFixedPoint = 0;
					}
					else
					{
						sk.BaseFixedPoint -= need;
						need = 0;
					}
				}

				/* Sanity check */
				if ( baseToSet > theirSkill.CapFixedPoint || (m.Skills.Total - theirSkill.BaseFixedPoint + baseToSet) > m.Skills.Cap )
					return TeachResult.NotEnoughFreePoints;

				theirSkill.BaseFixedPoint = baseToSet;
			}

			return TeachResult.Success;
		}

		public virtual bool CheckTeachingMatch( Mobile m )
		{
			if ( m_Teaching == (SkillName)(-1) )
				return false;

			if ( m is PlayerMobile )
				return ( ((PlayerMobile)m).Learning == m_Teaching );

			return true;
		}

		private SkillName m_Teaching = (SkillName)(-1);

		public virtual bool Teach( SkillName skill, Mobile m, int maxPointsToLearn, bool doTeach )
		{
			int pointsToLearn = 0;
			TeachResult res = CheckTeachSkills( skill, m, maxPointsToLearn, ref pointsToLearn, doTeach );

			switch ( res )
			{
				case TeachResult.KnowsMoreThanMe:
				{
					Say( 501508 ); // I cannot teach thee, for thou knowest more than I!
					break;
				}
				case TeachResult.KnowsWhatIKnow:
				{
					Say( 501509 ); // I cannot teach thee, for thou knowest all I can teach!
					break;
				}
				case TeachResult.NotEnoughFreePoints:
				case TeachResult.SkillNotRaisable:
				{
					// Make sure this skill is marked to raise. If you are near the skill cap (700 points) you may need to lose some points in another skill first.
					m.SendLocalizedMessage( 501510, "", 0x22 );
					break;
				}
				case TeachResult.Success:
				{
					if ( doTeach )
					{
						Say( 501539 ); // Let me show thee something of how this is done.
						m.SendLocalizedMessage( 501540 ); // Your skill level increases.

						m_Teaching = (SkillName)(-1);

						if ( m is PlayerMobile )
							((PlayerMobile)m).Learning = (SkillName)(-1);
					}
					else
					{
						// I will teach thee all I know, if paid the amount in full.  The price is:
						Say( 1019077, AffixType.Append, String.Format( " {0}", pointsToLearn ), "" );
						Say( 1043108 ); // For less I shall teach thee less.

						m_Teaching = skill;

						if ( m is PlayerMobile )
							((PlayerMobile)m).Learning = skill;
					}

					return true;
				}
			}

			return false;
		}
		#endregion

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( m_AI != null )
				m_AI.OnAggressiveAction( aggressor );

			StopFlee();

			ForceReacquire();

			if ( !IsEnemy( aggressor ) )
			{
				Ethics.Player pl = Ethics.Player.Find( aggressor, true );

				if ( pl != null && pl.IsShielded )
					pl.FinishShield();
			}

			OrderType ct = m_ControlOrder;

			if ( aggressor.ChangingCombatant && (m_bControlled || m_bSummoned) && (ct == OrderType.Come || ct == OrderType.Stay || ct == OrderType.Stop || ct == OrderType.None || ct == OrderType.Follow) )
			{
				ControlTarget = aggressor;
				ControlOrder = OrderType.Attack;
			}
			else if ( Combatant == null && !m_bBardPacified )
			{
				Warmode = true;
				Combatant = aggressor;
			}
		}

		public override bool OnMoveOver( Mobile m )
		{
            OnXMLEvent( XMLEventType.OnMoveOver );
            OnXMLEvent( XMLEventType.OnMoveOver, m );

			if ( m is BaseCreature && !((BaseCreature)m).Controlled )
				return false;
			
			if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				return pm.CheckShove( this );
			}

			return base.OnMoveOver( m );
		}

		public virtual void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
            if( Controlled && ControlMaster == from && this is ITinyPet )
                list.Add( new HoldEntry( this, from ) );

            BaseHouse house = BaseHouse.FindHouseAt( this );

            if( house != null && house.IsCoOwner( from ) )
                list.Add( new HouseEntry( this, from ) );
		}

		public virtual bool CanDrop { get { return !Summoned; } }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( m_AI != null && Commandable )
				m_AI.GetContextMenuEntries( from, list );

			if ( m_bTamable && !m_bControlled && from.Alive )
				list.Add( new TameEntry( from, this ) );

			AddCustomContextEntries( from, list );

			if ( CanTeach && from.Alive )
			{
				Skills ourSkills = this.Skills;
				Skills theirSkills = from.Skills;

				for ( int i = 0; i < ourSkills.Length && i < theirSkills.Length; ++i )
				{
					Skill skill = ourSkills[i];
					Skill theirSkill = theirSkills[i];

					if ( skill != null && theirSkill != null && skill.Base >= 60.0 && CheckTeach( skill.SkillName, from ) )
					{
						double toTeach = skill.Base / 3.0;

						if ( toTeach > 42.0 )
							toTeach = 42.0;

						list.Add( new TeachEntry( (SkillName)i, this, from, ( toTeach > theirSkill.Base ) ) );
					}
				}
			}

            OnXMLEvent( XMLEventType.OnSingleClick );
            OnXMLEvent( XMLEventType.OnSingleClickInvokeOnMobile, from );
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && (speechType.Flags & IHSFlags.OnSpeech) != 0 && from.InRange( this, 3 ) )
				return true;

			return ( m_AI != null && m_AI.HandlesOnSpeech( from ) && from.InRange( this, m_iRangePerception ) );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && speechType.OnSpeech( this, e.Mobile, e.Speech ) )
				e.Handled = true;
			else if ( !e.Handled && m_AI != null && e.Mobile.InRange( this, m_iRangePerception ) )
				m_AI.OnSpeech( e );
		}

		public override bool IsHarmfulCriminal( Mobile target )
		{
//			if ( (Controlled && target == m_ControlMaster) || (Summoned && target == m_SummonMaster) )
//				return false;
//
//			if ( target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled )
//				return false;
//
//			if ( target is PlayerMobile && ((PlayerMobile)target).PermaFlags.Count > 0 )
//				return false;
//
//			return base.IsHarmfulCriminal( target );
			
			return true;
		}

		public override void CriminalAction( bool message )
		{
//			base.CriminalAction( message );
//
//			if ( Controlled || Summoned )
//			{
//				if ( m_ControlMaster != null && m_ControlMaster.Player )
//					m_ControlMaster.CriminalAction( false );
//				else if ( m_SummonMaster != null && m_SummonMaster.Player )
//					m_SummonMaster.CriminalAction( false );
//			}
		}

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile AddAggressor
        {
            get { return null; }
            set
            {
                if( value != null )
                {
                    Aggressors.Add( AggressorInfo.Create( value, this, value.Criminal ) );
                    UpdateAggrExpire();
                }
            }
        }

		public override void DoHarmful( Mobile target, bool indirect )
		{
			base.DoHarmful( target, indirect );

			if ( target == this || target == m_ControlMaster || target == m_SummonMaster || (!Controlled && !Summoned) )
				return;

			List<AggressorInfo> list = this.Aggressors;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo ai = list[i];

				if ( ai.Attacker == target )
					return;
			}

			list = this.Aggressed;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo ai = list[i];

				if ( ai.Defender == target )
				{
					if ( m_ControlMaster != null && m_ControlMaster.Player && m_ControlMaster.CanBeHarmful( target, false ) )
						m_ControlMaster.DoHarmful( target, true );
					else if ( m_SummonMaster != null && m_SummonMaster.Player && m_SummonMaster.CanBeHarmful( target, false ) )
						m_SummonMaster.DoHarmful( target, true );

					return;
				}
			}
		}

		private static Mobile m_NoDupeGuards;

		public void ReleaseGuardDupeLock()
		{
			m_NoDupeGuards = null;
		}

		public void ReleaseGuardLock()
		{
			EndAction( typeof( GuardedRegion ) );
		}

		private DateTime m_IdleReleaseTime;

		public virtual bool CheckIdle()
		{
			if ( Combatant != null )
				return false; // in combat.. not idling

			if ( m_IdleReleaseTime > DateTime.MinValue )
			{
				// idling...

				if ( DateTime.Now >= m_IdleReleaseTime )
				{
					m_IdleReleaseTime = DateTime.MinValue;
					return false; // idle is over
				}

				return true; // still idling
			}

			if ( 95 > Utility.Random( 100 ) )
				return false; // not idling, but don't want to enter idle state

			m_IdleReleaseTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );

			if( BodyValue == 763 )
			{
				Animate( 5, 5, 1, true,  true, 1 );
				return true;
			}
				
			if ( Body.IsHuman )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: Animate( 5, 5, 1, true,  true, 1 ); break;
					case 1: Animate( 6, 5, 1, true, false, 1 ); break;
				}	
			}
			else if ( Body.IsAnimal )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: Animate(  3, 3, 1, true, false, 1 ); break;
					case 1: Animate(  9, 5, 1, true, false, 1 ); break;
					case 2: Animate( 10, 5, 1, true, false, 1 ); break;
				}
			}
			else if ( Body.IsMonster )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: Animate( 17, 5, 1, true, false, 1 ); break;
					case 1: Animate( 18, 5, 1, true, false, 1 ); break;
				}
			}

			PlaySound( GetIdleSound() );
			return true; // entered idle state
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			Map map = this.Map;
			
			if ( PlayerRangeSensitive && m_AI != null && map != null && map.GetSector( this.Location ).Active )
				m_AI.Activate();
			
			base.OnLocationChange( oldLocation );
		}
		
		public override void SetDirection( Direction dir )
		{
			if ( !CombatSystemAttachment.GetCSA( this ).PerformingSequence && !Frozen )
			{
				Direction tmp = Direction;
				if ( tmp != dir )
				{
					base.SetDirection( dir );
					CombatSystemAttachment.GetCSA( this ).OnMoved( Location );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override Direction Direction
		{
			get
			{
				return base.Direction;
			}
			set
			{
				if ( !CombatSystemAttachment.GetCSA( this ).PerformingSequence && !Frozen )
				{
					if ( value != Direction )
					{
						base.Direction = value;
						CombatSystemAttachment.GetCSA( this ).OnMoved( Location ); // must be called after
					}
				}
			}
		}
		
		public override bool Move( Direction d )
		{
			return Move( d, true );
		}
		public bool Move( Direction d, bool clientRequested ) // this is really AI requested sort of
		{
            if (CombatSystemAttachment.GetCSA(this).PerformingSequence && clientRequested)
                return false;
            else
            {
                if (HealthAttachment.HasHealthAttachment(this))
                {
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.MinorConcussion))
                        HealthAttachment.GetHA(this).DoInjury(Injury.MinorConcussion);
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.MajorConcussion))
                        HealthAttachment.GetHA(this).DoInjury(Injury.MajorConcussion);
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.Exhausted))
                        HealthAttachment.GetHA(this).DoInjury(Injury.Exhausted);
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.FracturedLeftLeg))
                        HealthAttachment.GetHA(this).DoInjury(Injury.FracturedLeftLeg);
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.FracturedRightLeg))
                        HealthAttachment.GetHA(this).DoInjury(Injury.FracturedRightLeg);
                }

                return base.Move(d);
            }
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );
			
			if ( m.CanSee( this ) && m.NetState != null )
			{
				if ( !Utility.InUpdateRange( Location, oldLocation ) ) // they weren't in our update range a step ago
				{
					if ( Utility.InUpdateRange( Location, m.Location ) ) // but they are now
					{
						CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( this );
						if ( csa.AnimationTimer != null )
							csa.AnimationTimer.LocalAnimationRefresh( m );
					}
				}
				
			}

			if ( ReacquireOnMovement || m_Paragon )
				ForceReacquire();

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnMovement( this, m, oldLocation );

			/* Begin notice sound */
			if ( (!m.Hidden || m.AccessLevel == AccessLevel.Player) && m.Player && m_FightMode != FightMode.Aggressor && m_FightMode != FightMode.None && Combatant == null && !Controlled && !Summoned )
			{
				// If this creature defends itself but doesn't actively attack (animal) or
				// doesn't fight at all (vendor) then no notice sounds are played..
				// So, players are only notified of aggressive monsters

				// Monsters that are currently fighting are ignored

				// Controlled or summoned creatures are ignored

				if ( InRange( m.Location, 18 ) && !InRange( oldLocation, 18 ) )
				{
					/*if ( Body.IsMonster && TrippedTimer == null )
						Animate( 11, 5, 1, true, false, 1 );*/

					PlaySound( GetAngerSound() );
				}
			}
			/* End notice sound */

			if ( m_NoDupeGuards == m )
				return;

			if ( !Body.IsHuman || Kills >= 5 || AlwaysMurderer || AlwaysAttackable || m.Kills < 5 || !m.InRange( Location, 12 ) || !m.Alive )
				return;

			/*GuardedRegion guardedRegion = (GuardedRegion) this.Region.GetRegion( typeof( GuardedRegion ) );

			if ( guardedRegion != null )
			{
				if ( !guardedRegion.IsDisabled() && guardedRegion.IsGuardCandidate( m ) && BeginAction( typeof( GuardedRegion ) ) )
				{
					Say( 1013037 + Utility.Random( 16 ) );
					guardedRegion.CallGuards( this.Location );

					Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( ReleaseGuardLock ) );

					m_NoDupeGuards = m;
					Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ReleaseGuardDupeLock ) );
				}
			}*/
		}


		public void AddSpellAttack( Type type )
		{
			m_arSpellAttack.Add ( type );
		}

		public void AddSpellDefense( Type type )
		{
			m_arSpellDefense.Add ( type );
		}

		public Spell GetAttackSpellRandom()
		{
			if ( m_arSpellAttack.Count > 0 )
			{
				Type type = m_arSpellAttack[Utility.Random(m_arSpellAttack.Count)];

				object[] args = {this, null};
				return Activator.CreateInstance( type, args ) as Spell;
			}
			else
			{
				return null;
			}
		}

		public Spell GetDefenseSpellRandom()
		{
			if ( m_arSpellDefense.Count > 0 )
			{
				Type type = m_arSpellDefense[Utility.Random(m_arSpellDefense.Count)];

				object[] args = {this, null};
				return Activator.CreateInstance( type, args ) as Spell;
			}
			else
			{
				return null;
			}
		}

		public Spell GetSpellSpecific( Type type )
		{
			int i;

			for( i=0; i< m_arSpellAttack.Count; i++ )
			{
				if( m_arSpellAttack[i] == type )
				{
					object[] args = { this, null };
					return Activator.CreateInstance( type, args ) as Spell;
				}
			}

			for ( i=0; i< m_arSpellDefense.Count; i++ )
			{
				if ( m_arSpellDefense[i] == type )
				{
					object[] args = {this, null};
					return Activator.CreateInstance( type, args ) as Spell;
				}			
			}

			return null;
		}

		#region Set[...]

		public void SetDamage( int val )
		{
			m_DamageMin = val;
			m_DamageMax = val;
		}

		public void SetDamage( int min, int max )
		{
			m_DamageMin = min;
			m_DamageMax = max;
		}

		public void SetHits( int val )
		{
			if ( val < 1000 && !Core.AOS )
				val = (val * 100) / 60;

			RawHits = val;
			Hits = RawHits;
		}

		public void SetHits( int min, int max )
		{
			if ( min < 1000 && !Core.AOS )
			{
				min = (min * 100) / 60;
				max = (max * 100) / 60;
			}

			RawHits = Utility.RandomMinMax( min, max );
			Hits = RawHits;
		}

		public void SetStam( int val )
		{
			RawStam = val;
			Stam = StamMax;
		}

		public void SetStam( int min, int max )
		{
			RawStam = Utility.RandomMinMax( min, max );
			Stam = RawStam;
		}

		public void SetMana( int val )
		{
			RawMana = val;
			Mana = RawMana;
		}

		public void SetMana( int min, int max )
		{
			RawMana = Utility.RandomMinMax( min, max );
			Mana = RawMana;
		}

		public void SetStr( int val )
		{
			RawStr = val;
            
			Hits = HitsMax;
		}

		public void SetStr( int min, int max )
		{
			RawStr = Utility.RandomMinMax( min, max );
            HitsMax = RawStr;
			Hits = HitsMax;
		}

		public void SetDex( int val )
		{
			RawDex = val;
            StamMax = RawDex;
			Stam = StamMax;
		}

		public void SetDex( int min, int max )
		{
			RawDex = Utility.RandomMinMax( min, max );
            StamMax = RawDex;
			Stam = StamMax;
		}

		public void SetInt( int val )
		{
			RawInt = val;
            ManaMax = RawInt;
			Mana = ManaMax;
		}

		public void SetInt( int min, int max )
		{
			RawInt = Utility.RandomMinMax( min, max );
            ManaMax = RawInt;
			Mana = ManaMax;
		}

		public void SetDamageType( ResistanceType type, int min, int max )
		{
			SetDamageType( type, Utility.RandomMinMax( min, max ) );
		}

		public void SetDamageType( ResistanceType type, int val )
		{
			switch ( type )
			{
				case ResistanceType.Physical: m_PhysicalDamage = val; break;
				case ResistanceType.Fire: m_FireDamage = val; break;
				case ResistanceType.Cold: m_ColdDamage = val; break;
				case ResistanceType.Poison: m_PoisonDamage = val; break;
				case ResistanceType.Energy: m_EnergyDamage = val; break;
				case ResistanceType.Blunt: m_BluntDamage = val; break;
				case ResistanceType.Slashing: m_SlashingDamage = val; break;
				case ResistanceType.Piercing: m_PiercingDamage = val; break;
			}
		}

		public void SetResistance( ResistanceType type, int min, int max )
		{
			SetResistance( type, Utility.RandomMinMax( min, max ) );
		}

		public void SetResistance( ResistanceType type, int val )
		{
			switch ( type )
			{
				case ResistanceType.Physical: m_PhysicalResistance = val; break;
				case ResistanceType.Fire: m_FireResistance = val; break;
				case ResistanceType.Cold: m_ColdResistance = val; break;
				case ResistanceType.Poison: m_PoisonResistance = val; break;
				case ResistanceType.Energy: m_EnergyResistance = val; break;
				case ResistanceType.Blunt: m_BluntResistance = val; break;
				case ResistanceType.Slashing: m_SlashingResistance = val; break;
				case ResistanceType.Piercing: m_PiercingResistance = val; break;
			}

			UpdateResistances();
		}

		public void SetSkill( SkillName name, double val )
		{
			Skills[name].BaseFixedPoint = (int)(val * 10);

			if ( Skills[name].Base > Skills[name].Cap ) 
				Skills[name].Cap = Skills[name].Base;
		}

		public void SetSkill( SkillName name, double min, double max )
		{
			int minFixed = (int)(min * 10);
			int maxFixed = (int)(max * 10);

			Skills[name].BaseFixedPoint = Utility.RandomMinMax( minFixed, maxFixed );

			if ( Skills[name].Base > Skills[name].Cap ) 
				Skills[name].Cap = Skills[name].Base;
		}

		public void SetFameLevel( int level )
		{
			switch ( level )
			{
				case 1: Fame = Utility.RandomMinMax(     0,  1249 ); break;
				case 2: Fame = Utility.RandomMinMax(  1250,  2499 ); break;
				case 3: Fame = Utility.RandomMinMax(  2500,  4999 ); break;
				case 4: Fame = Utility.RandomMinMax(  5000,  9999 ); break;
				case 5: Fame = Utility.RandomMinMax( 10000, 10000 ); break;
			}
		}

		public void SetKarmaLevel( int level )
		{
			switch ( level )
			{
				case 0: Karma = -Utility.RandomMinMax(     0,   624 ); break;
				case 1: Karma = -Utility.RandomMinMax(   625,  1249 ); break;
				case 2: Karma = -Utility.RandomMinMax(  1250,  2499 ); break;
				case 3: Karma = -Utility.RandomMinMax(  2500,  4999 ); break;
				case 4: Karma = -Utility.RandomMinMax(  5000,  9999 ); break;
				case 5: Karma = -Utility.RandomMinMax( 10000, 10000 ); break;
			}
		}

		#endregion

		public static void Cap( ref int val, int min, int max )
		{
			if ( val < min )
				val = min;
			else if ( val > max )
				val = max;
		}

		#region Pack & Loot
		public void PackPotion()
		{
			PackItem( Loot.RandomPotion() );
		}

		public void PackNecroScroll( int index )
		{
			if ( !Core.AOS || 0.05 <= Utility.RandomDouble() )
				return;

			PackItem( Loot.Construct( Loot.NecromancyScrollTypes, index ) );
		}

		public void PackScroll( int minCircle, int maxCircle )
		{
			PackScroll( Utility.RandomMinMax( minCircle, maxCircle ) );
		}

		public void PackScroll( int circle )
		{
			int min = (circle - 1) * 8;

			PackItem( Loot.RandomScroll( min, min + 7, SpellbookType.Regular ) );
		}

		public void PackMagicItems( int minLevel, int maxLevel )
		{
			PackMagicItems( minLevel, maxLevel, 0.30, 0.15 );
		}

		public void PackMagicItems( int minLevel, int maxLevel, double armorChance, double weaponChance )
		{
			if ( !PackArmor( minLevel, maxLevel, armorChance ) )
				PackWeapon( minLevel, maxLevel, weaponChance );
		}

		protected bool m_Spawning;
		protected int m_KillersLuck;

		public virtual void GenerateLoot( bool spawning )
		{
			m_Spawning = spawning;

			if ( !spawning )
				m_KillersLuck = LootPack.GetLuckChanceForKiller( this );

			GenerateLoot();

			if ( m_Paragon )
			{
				if ( Fame < 1250 )
					AddLoot( LootPack.Meager );
				else if ( Fame < 2500 )
					AddLoot( LootPack.Average );
				else if ( Fame < 5000 )
					AddLoot( LootPack.Rich );
				else if ( Fame < 10000 )
					AddLoot( LootPack.FilthyRich );
				else
					AddLoot( LootPack.UltraRich );
			}

			m_Spawning = false;
			m_KillersLuck = 0;
		}

		public virtual void GenerateLoot()
		{
		}

		public virtual void AddLoot( LootPack pack, int amount )
		{
			for ( int i = 0; i < amount; ++i )
				AddLoot( pack );
		}

		public virtual void AddLoot( LootPack pack )
		{
			if ( Summoned )
				return;

			Container backpack = Backpack;

			if ( backpack == null )
			{
				backpack = new Backpack();

				backpack.Movable = false;

				AddItem( backpack );
			}

			pack.Generate( this, backpack, m_Spawning, m_KillersLuck );
		}

		public bool PackArmor( int minLevel, int maxLevel )
		{
			return PackArmor( minLevel, maxLevel, 1.0 );
		}

		public bool PackArmor( int minLevel, int maxLevel, double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			Cap( ref minLevel, 0, 5 );
			Cap( ref maxLevel, 0, 5 );

			if ( Core.AOS )
			{
				Item item = Loot.RandomArmorOrShieldOrJewelry();

				if ( item == null )
					return false;

				int attributeCount, min, max;
				GetRandomAOSStats( minLevel, maxLevel, out attributeCount, out min, out max );

				if ( item is BaseArmor )
					BaseRunicTool.ApplyAttributesTo( (BaseArmor)item, attributeCount, min, max );
				else if ( item is BaseJewel )
					BaseRunicTool.ApplyAttributesTo( (BaseJewel)item, attributeCount, min, max );

				PackItem( item );
			}
			else
			{
				BaseArmor armor = Loot.RandomArmorOrShield();

				if ( armor == null )
					return false;

				armor.ProtectionLevel = (ArmorProtectionLevel)RandomMinMaxScaled( minLevel, maxLevel );
				armor.Durability = (ArmorDurabilityLevel)RandomMinMaxScaled( minLevel, maxLevel );

				PackItem( armor );
			}

			return true;
		}

		public static void GetRandomAOSStats( int minLevel, int maxLevel, out int attributeCount, out int min, out int max )
		{
			int v = RandomMinMaxScaled( minLevel, maxLevel );

			if ( v >= 5 )
			{
				attributeCount = Utility.RandomMinMax( 2, 6 );
				min = 20; max = 70;
			}
			else if ( v == 4 )
			{
				attributeCount = Utility.RandomMinMax( 2, 4 );
				min = 20; max = 50;
			}
			else if ( v == 3 )
			{
				attributeCount = Utility.RandomMinMax( 2, 3 );
				min = 20; max = 40;
			}
			else if ( v == 2 )
			{
				attributeCount = Utility.RandomMinMax( 1, 2 );
				min = 10; max = 30;
			}
			else
			{
				attributeCount = 1;
				min = 10; max = 20;
			}
		}

		public static int RandomMinMaxScaled( int min, int max )
		{
			if ( min == max )
				return min;

			if ( min > max )
			{
				int hold = min;
				min = max;
				max = hold;
			}

			/* Example:
			 *    min: 1
			 *    max: 5
			 *  count: 5
			 * 
			 * total = (5*5) + (4*4) + (3*3) + (2*2) + (1*1) = 25 + 16 + 9 + 4 + 1 = 55
			 * 
			 * chance for min+0 : 25/55 : 45.45%
			 * chance for min+1 : 16/55 : 29.09%
			 * chance for min+2 :  9/55 : 16.36%
			 * chance for min+3 :  4/55 :  7.27%
			 * chance for min+4 :  1/55 :  1.81%
			 */

			int count = max - min + 1;
			int total = 0, toAdd = count;

			for ( int i = 0; i < count; ++i, --toAdd )
				total += toAdd*toAdd;

			int rand = Utility.Random( total );
			toAdd = count;

			int val = min;

			for ( int i = 0; i < count; ++i, --toAdd, ++val )
			{
				rand -= toAdd*toAdd;

				if ( rand < 0 )
					break;
			}

			return val;
		}

		public bool PackSlayer()
		{
			return PackSlayer( 0.05 );
		}

		public bool PackSlayer( double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			if ( Utility.RandomBool() )
			{
				BaseInstrument instrument = Loot.RandomInstrument();

				if ( instrument != null )
				{
					instrument.Slayer = SlayerGroup.GetLootSlayerType( GetType() );
					PackItem( instrument );
				}
			}
			else if ( !Core.AOS )
			{
				BaseWeapon weapon = Loot.RandomWeapon();

				if ( weapon != null )
				{
					weapon.Slayer = SlayerGroup.GetLootSlayerType( GetType() );
					PackItem( weapon );
				}
			}

			return true;
		}

		public bool PackWeapon( int minLevel, int maxLevel )
		{
			return PackWeapon( minLevel, maxLevel, 1.0 );
		}

		public bool PackWeapon( int minLevel, int maxLevel, double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			Cap( ref minLevel, 0, 5 );
			Cap( ref maxLevel, 0, 5 );

			if ( Core.AOS )
			{
				Item item = Loot.RandomWeaponOrJewelry();

				if ( item == null )
					return false;

				int attributeCount, min, max;
				GetRandomAOSStats( minLevel, maxLevel, out attributeCount, out min, out max );

				if ( item is BaseWeapon )
					BaseRunicTool.ApplyAttributesTo( (BaseWeapon)item, attributeCount, min, max );
				else if ( item is BaseJewel )
					BaseRunicTool.ApplyAttributesTo( (BaseJewel)item, attributeCount, min, max );

				PackItem( item );
			}
			else
			{
				BaseWeapon weapon = Loot.RandomWeapon();

				if ( weapon == null )
					return false;

				if ( 0.05 > Utility.RandomDouble() )
					weapon.Slayer = SlayerName.Silver;

				weapon.DamageLevel = (WeaponDamageLevel)RandomMinMaxScaled( minLevel, maxLevel );
				weapon.AccuracyLevel = (WeaponAccuracyLevel)RandomMinMaxScaled( minLevel, maxLevel );
				weapon.DurabilityLevel = (WeaponDurabilityLevel)RandomMinMaxScaled( minLevel, maxLevel );

				PackItem( weapon );
			}

			return true;
		}

		public void PackGold( int amount )
		{
			if ( amount > 0 )
				PackItem( new Copper( amount ) );
		}

		public void PackGold( int min, int max )
		{
			PackGold( Utility.RandomMinMax( min, max ) );
		}

		public void PackStatue( int min, int max )
		{
			PackStatue( Utility.RandomMinMax( min, max ) );
		}

		public void PackStatue( int amount )
		{
			for ( int i = 0; i < amount; ++i )
				PackStatue();
		}

		public void PackStatue()
		{
			PackItem( Loot.RandomStatue() );
		}

		public void PackGem()
		{
			PackGem( 1 );
		}

		public void PackGem( int min, int max )
		{
			PackGem( Utility.RandomMinMax( min, max ) );
		}

		public void PackGem( int amount )
		{
			if ( amount <= 0 )
				return;

			Item gem = Loot.RandomGem();

			gem.Amount = amount;

			PackItem( gem );
		}

		public void PackNecroReg( int min, int max )
		{
			PackNecroReg( Utility.RandomMinMax( min, max ) );
		}

		public void PackNecroReg( int amount )
		{
			for ( int i = 0; i < amount; ++i )
				PackNecroReg();
		}

		public void PackNecroReg()
		{
			if ( !Core.AOS )
				return;

			PackItem( Loot.RandomNecromancyReagent() );
		}

		public void PackReg( int min, int max )
		{
			PackReg( Utility.RandomMinMax( min, max ) );
		}

		public void PackReg( int amount )
		{
			if ( amount <= 0 )
				return;

			Item reg = Loot.RandomReagent();

			reg.Amount = amount;

			PackItem( reg );
		}

		public void PackItem( Item item )
		{
			if ( Summoned || item == null )
			{
				if ( item != null )
					item.Delete();

				return;
			}

			Container pack = Backpack;

			if ( pack == null )
			{
				pack = new Backpack();

				pack.Movable = false;

				AddItem( pack );
			}

			if ( !item.Stackable || !pack.TryDropItem( this, item, false ) ) // try stack
				pack.DropItem( item ); // failed, drop it anyway
		}
		#endregion

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster && !Body.IsHuman )
			{
				Container pack = this.Backpack;

				if ( pack != null )
					pack.DisplayTo( from );
			}

			if ( this.DeathAdderCharmable && from.CanBeHarmful( this, false ) )
			{
				DeathAdder da = Spells.Necromancy.SummonFamiliarSpell.Table[from] as DeathAdder;

				if ( da != null && !da.Deleted )
				{
					from.SendAsciiMessage( "You charm the snake.  Select a target to attack." );
					from.Target = new DeathAdderCharmTarget( this );
				}
			}

            OnXMLEvent( XMLEventType.OnDoubleClick );
            OnXMLEvent( XMLEventType.OnDoubleClickInvokeOnMobile, from );

			base.OnDoubleClick( from );
		}

		private class DeathAdderCharmTarget : Target
		{
			private BaseCreature m_Charmed;

			public DeathAdderCharmTarget( BaseCreature charmed ) : base( -1, false, TargetFlags.Harmful )
			{
				m_Charmed = charmed;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !m_Charmed.DeathAdderCharmable || m_Charmed.Combatant != null || !from.CanBeHarmful( m_Charmed, false ) )
					return;

				DeathAdder da = Spells.Necromancy.SummonFamiliarSpell.Table[from] as DeathAdder;
				if ( da == null || da.Deleted )
					return;

				Mobile targ = targeted as Mobile;
				if ( targ == null || !from.CanBeHarmful( targ, false ) )
					return;

				from.RevealingAction();
				from.DoHarmful( targ, true );

				m_Charmed.Combatant = targ;

				if ( m_Charmed.AIObject != null )
					m_Charmed.AIObject.Action = ActionType.Combat;
			}
		}
		
		public virtual void OldNameProperties( ObjectPropertyList list )
		{
			string name = Name;

			if ( name == null )
				name = String.Empty;

			string prefix = "";

			string suffix = "";

			//if ( PropertyTitle && Title != null && Title.Length > 0 )
				//suffix = Title;

			suffix = ApplyNameSuffix( suffix );

            list.Add(1050045, "{0} \t{1}\t {2}", prefix, name, suffix); // ~1_PREFIX~~2_NAME~~3_SUFFIX~
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			OldNameProperties( list );

			if ( Controlled && Commandable )
			{
				if ( Summoned )
					list.Add( 1049646 ); // (summoned)
				else if ( IsBonded )	//Intentional difference (showing ONLY bonded when bonded instead of bonded & tame)
					list.Add( 1049608 ); // (bonded)
				else
					list.Add( 502006 ); // (tame)
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( Controlled && Commandable )
			{
				int number;

				if ( Summoned )
					number = 1049646; // (summoned)
				else if ( IsBonded )
					number = 1049608; // (bonded)
				else
					number = 502006; // (tame)

				PrivateOverheadMessage( MessageType.Regular, 0x3B2, number, from.NetState );
			}

			base.OnSingleClick( from );
		}

		public virtual double TreasureMapChance{ get{ return TreasureMap.LootChance; } }
		public virtual int TreasureMapLevel{ get{ return -1; } }

		public virtual bool IgnoreYoungProtection { get { return false; } }
		
		public override bool OnBeforeDeath()
		{
			int treasureLevel = TreasureMapLevel;
			
			if( this is HobgoblinRider )
			{
				this.Body = 0xB6;
				this.Hue = 2422;
				
				GrayWolf wolf = new GrayWolf();
				wolf.MoveToWorld( this.Location, this.Map );
				
				if( this.Combatant != null )
					wolf.Combatant = this.Combatant;
			}
			
			if ( treasureLevel == 1 && this.Map == Map.Trammel && TreasureMap.IsInHavenIsland( this ) )
			{
				Mobile killer = this.LastKiller;

				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();

				if ( killer is PlayerMobile && ((PlayerMobile)killer).Young )
					treasureLevel = 0;
			}

			if ( !Summoned && !NoKillAwards && !IsBonded && treasureLevel >= 0 )
			{
				if ( m_Paragon && Paragon.ChestChance > Utility.RandomDouble() )
					PackItem( new ParagonChest( this.Name, treasureLevel ) );
				else if ( (Map == Map.Felucca || Map == Map.Trammel) && TreasureMap.LootChance >= Utility.RandomDouble() )
					PackItem( new TreasureMap( treasureLevel, Map ) );
			}		

			if ( !Summoned && !NoKillAwards && !m_HasGeneratedLoot )
			{
				m_HasGeneratedLoot = true;
				GenerateLoot( false );
			}

			if ( IsAnimatedDead )
				Effects.SendLocationEffect( Location, Map, 0x3728, 13, 1, 0x461, 4 );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnDeath( this );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetKilled();

			return base.OnBeforeDeath();
		}

		private bool m_NoKillAwards;

		public bool NoKillAwards
		{
			get{ return m_NoKillAwards; }
			set{ m_NoKillAwards = value; }
		}

		public int ComputeBonusDamage( List<DamageEntry> list, Mobile m )
		{
			int bonus = 0;

			for ( int i = list.Count - 1; i >= 0; --i )
			{
				DamageEntry de = list[i];

				if ( de.Damager == m || !(de.Damager is BaseCreature) )
					continue;

				BaseCreature bc = (BaseCreature)de.Damager;
				Mobile master = null;

				master = bc.GetMaster();

				if ( master == m )
					bonus += de.DamageGiven;
			}

			return bonus;
		}

		public Mobile GetMaster()
		{
			if ( Controlled && ControlMaster != null )
				return ControlMaster;
			else if ( Summoned && SummonMaster != null )
				return SummonMaster;

			return null;
		}

		private class FKEntry
		{
			public Mobile m_Mobile;
			public int m_Damage;

			public FKEntry( Mobile m, int damage )
			{
				m_Mobile = m;
				m_Damage = damage;
			}
		}

		public static List<DamageStore> GetLootingRights( List<DamageEntry> damageEntries, int hitsMax )
		{
			List<DamageStore> rights = new List<DamageStore>();

			for ( int i = damageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= damageEntries.Count )
					continue;

				DamageEntry de = damageEntries[i];

				if ( de.HasExpired )
				{
					damageEntries.RemoveAt( i );
					continue;
				}

				int damage = de.DamageGiven;

				List<DamageEntry> respList = de.Responsible;

				if ( respList != null )
				{
					for ( int j = 0; j < respList.Count; ++j )
					{
						DamageEntry subEntry = respList[j];
						Mobile master = subEntry.Damager;

						if ( master == null || master.Deleted || !master.Player )
							continue;

						bool needNewSubEntry = true;

						for ( int k = 0; needNewSubEntry && k < rights.Count; ++k )
						{
							DamageStore ds = rights[k];

							if ( ds.m_Mobile == master )
							{
								ds.m_Damage += subEntry.DamageGiven;
								needNewSubEntry = false;
							}
						}

						if ( needNewSubEntry )
							rights.Add( new DamageStore( master, subEntry.DamageGiven ) );

						damage -= subEntry.DamageGiven;
					}
				}

				Mobile m = de.Damager;

				if ( m == null || m.Deleted || !m.Player )
					continue;

				if ( damage <= 0 )
					continue;

				bool needNewEntry = true;

				for ( int j = 0; needNewEntry && j < rights.Count; ++j )
				{
					DamageStore ds = rights[j];

					if ( ds.m_Mobile == m )
					{
						ds.m_Damage += damage;
						needNewEntry = false;
					}
				}

				if ( needNewEntry )
					rights.Add( new DamageStore( m, damage ) );
			}

			if ( rights.Count > 0 )
			{
				if ( rights.Count > 1 )
					rights.Sort();

				int topDamage = rights[0].m_Damage;
				int minDamage;

				if ( hitsMax >= 3000 )
					minDamage = topDamage / 16;
				else if ( hitsMax >= 1000 )
					minDamage = topDamage / 8;
				else if ( hitsMax >= 200 )
					minDamage = topDamage / 4;
				else
					minDamage = topDamage / 2;

				for ( int i = 0; i < rights.Count; ++i )
				{
					DamageStore ds = rights[i];

					ds.m_HasRight = ( ds.m_Damage >= minDamage );
				}
			}

			return rights;
		}

		public virtual void OnKilledBy( Mobile mob )
		{
			BaseCombatManeuver.Cleave( mob, this );
			
			if( mob != null && mob.Weapon != null && mob.Weapon is BaseWeapon && !(mob.Weapon is Fists) )
				((BaseWeapon)mob.Weapon).XmlOnKilled( this, mob );
        }
		
		public virtual void AwardXPnCP()
		{
            PlayerMobile bcowner = null;
            ArrayList toAward = new ArrayList();
            
			//KILLED BY A CREATURE
            if( LastKiller is BaseCreature )
            {
                BaseCreature bc = LastKiller as BaseCreature;
                
                //BECOMING A LESS HUNGER PREDATOR
                if( ( bc is ILargePredator || bc is IMediumPredator || bc is ISmallPredator ) && ( this is ILargePrey || this is IMediumPrey || this is ISmallPrey ) )
                	bc.Hunger += 15;

                //IF IT HAS A MASTER, LET'S AWARD HIM AND HIS ALLIES
                if( bc.ControlMaster != null )
                {
                    ArrayList list = new ArrayList();
                    
                    if( bc.ControlMaster is PlayerMobile )
                    {
                    	bcowner = bc.ControlMaster as PlayerMobile;
                    	
                    	//SEARCHING FOR THE MASTER'S ALLIES
                        foreach( Mobile m in this.GetMobilesInRange( 10 ) )
                            list.Add( m );

                        for( int i = 0; i < list.Count; ++i )
                        {
                            Mobile m = (Mobile)list[i];

                            //EVERYONE GETS AWARDED, EXCEPT THE KILLER, WHO HAS THEIR OWN METHOD
                            if( m != this && m != LastKiller )
                            {
                                if( m != null && !m.Deleted && m.Map == bc.Map && m.Alive && this.InRange( m, 10 ) && this.InLOS( m ) )
                                {
                                    if( m == bcowner || bcowner.AllyList.Contains( m ) )
                                    {
                                    	//AWARDING PLAYER ALLIES
                                        if( m is PlayerMobile && ( (PlayerMobile)m ).XPFromKilling )
                                        	toAward.Add( m );

                                        //AWARDING PET ALLIES
                                        if( m is BaseCreature )
                                        	toAward.Add( m );
                                        
                                        //AWARDING MOUNTS
                                        if( m.Mount != null && bcowner.AllyList.Contains( (Mobile)m.Mount ) )
		                                	toAward.Add( (Mobile)m.Mount );
                                    }
                                }
                            }
                        }
                    }
                }

                //KILLER'S METHOD
                toAward.Add( bc );
                
                //AWARDING THE KILLER'S MOUNT
				if( bc.Mount != null && bcowner != null && bcowner.AllyList.Contains( (Mobile)bc.Mount ) )
					toAward.Add( (Mobile)bc.Mount );
            }

            //KILLED BY A PLAYER
            if( LastKiller is PlayerMobile )
            {
                PlayerMobile attacker = LastKiller as PlayerMobile;
                
                //modification to support XmlQuest Killtasks
				XmlQuest.RegisterKill( this, LastKiller);

                ArrayList list = new ArrayList();
                
                //SEARCHING FOR THE PLAYER'S ALLIES
                foreach( Mobile m in this.GetMobilesInRange( 10 ) )
                        list.Add( m );

                for( int i = 0; i < list.Count; ++i )
                {
                    Mobile m = (Mobile)list[i];

                    //EVERYONE GETS AWARDED, EXCEPT THE KILLER, WHO HAS THEIR OWN METHOD
                    if( m != this && m != LastKiller )
                    {
                        if( m != null && !m.Deleted && m.Map == LastKiller.Map && m.Alive && this.InRange( m, 10 ) && this.InLOS( m ) )
                        {
                            if( attacker.AllyList.Contains( m ) )
                            {
                            	//AWARDING PLAYER ALLIES
                            	if( m is PlayerMobile && ( (PlayerMobile)m ).XPFromKilling )
                                	toAward.Add( m );

                                //AWARDING PET ALLIES
                                if( m is BaseCreature )
                                	toAward.Add( m );
                                
                                //AWARDING MOUNTS
                                if( m.Mount != null && attacker.AllyList.Contains( (Mobile)m.Mount ) )
                                	toAward.Add( (Mobile)m.Mount );
                            }
                        }
                    }
                }
                
				//KILLER'S METHOD
				if( attacker.XPFromKilling )
                	toAward.Add( attacker );
				
				//AWARDING THE KILLER'S MOUNT
				if( attacker.Mount != null && attacker.AllyList.Contains( (Mobile)attacker.Mount ) )
					toAward.Add( (Mobile)attacker.Mount );
            }
            
            //FAME PENALTY BASED ON THE NUMBER OF MOBILES WHO WILL GET AWARDED
            this.Fame = Math.Max( (int)( this.Fame * 0.25 ), (int)( this.Fame * (1.05 - (0.05 * toAward.Count)) ) );
            
            //AWARDING EVERYBODY
            for( int i = 0; i < toAward.Count; i++ )
            {
            	Mobile m = toAward[i] as Mobile;
            	
            	if( m is PlayerMobile )
            	{
            		LevelSystem.PlayerAwards( (PlayerMobile)m, this );
            		((PlayerMobile)m).HandleAwardsOnKill();
            	}
            	
            	if( m is BaseCreature )
            		LevelSystem.PetAwards( (BaseCreature)m, this );
            }
		}

		public override void OnDeath( Container c )
		{
            if( this.VanishTime == DateTime.MinValue )
            	AwardXPnCP();
            
            Lives--;
			
			if( Lives >= 0 )
			{
				m_TimeOfDeath = DateTime.Now;
            	BeginRess( m_DefaultRessTime, c );
			}
			
			else
				this.IsBonded = false;

			if ( IsBonded && WaitForRess )
			{
				int sound = this.GetDeathSound();

				if ( sound >= 0 )
					Effects.PlaySound( this, this.Map, sound );

				Warmode = false;

				Poison = null;
				Combatant = null;

				Hits = 0;
				Stam = 0;
				Mana = 0;

				IsDeadPet = true;
				ControlTarget = ControlMaster;
				ControlOrder = OrderType.Follow;

				ProcessDeltaQueue();
				SendIncomingPacket();
				SendIncomingPacket();

				List<AggressorInfo> aggressors = this.Aggressors;

				for ( int i = 0; i < aggressors.Count; ++i )
				{
					AggressorInfo info = aggressors[i];

					if ( info.Attacker.Combatant == this )
						info.Attacker.Combatant = null;
				}

				List<AggressorInfo> aggressed = this.Aggressed;

				for ( int i = 0; i < aggressed.Count; ++i )
				{
					AggressorInfo info = aggressed[i];

					if ( info.Defender.Combatant == this )
						info.Defender.Combatant = null;
				}

				Mobile owner = this.ControlMaster;

				if ( owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange( this, 12 ) || !this.CanSee( owner ) || !this.InLOS( owner ) )
				{
					if ( this.OwnerAbandonTime == DateTime.MinValue )
						this.OwnerAbandonTime = DateTime.Now;
				}
				else
				{
					this.OwnerAbandonTime = DateTime.MinValue;
				}

				CheckStatTimers();
			}
			else
			{			
				Mobile killer = this.FindMostRecentDamager( true );

                if( killer != null )
                {
                    OnKilledBy( killer );
                    OnXMLEvent( XMLEventType.OnDeathInvokeOnMobile, killer );
                }

				base.OnDeath( c );

                if( DeleteCorpseOnDeath )
                    c.Delete();

                else
                    OnXMLEvent( XMLEventType.OnDeathInvokeOnItem, c );

                OnXMLEvent( XMLEventType.OnDeath );
			}
		}

		/* To save on cpu usage, RunUO creatures only reacquire creatures under the following circumstances:
		 *  - 10 seconds have elapsed since the last time it tried
		 *  - The creature was attacked
		 *  - Some creatures, like dragons, will reacquire when they see someone move
		 * 
		 * This functionality appears to be implemented on OSI as well
		 */

		private DateTime m_NextReacquireTime;

		public DateTime NextReacquireTime{ get{ return m_NextReacquireTime; } set{ m_NextReacquireTime = value; } }

		public virtual TimeSpan ReacquireDelay
        {
            get
            {
                if( AI == AIType.AI_Berserk )
                    return TimeSpan.FromSeconds( 2.0 ); 

                return TimeSpan.FromSeconds( 10.0 ); 
            } 
        }

		public virtual bool ReacquireOnMovement{ get{ return false; } }

		public void ForceReacquire()
		{
			m_NextReacquireTime = DateTime.MinValue;
		}

		public override void OnDelete()
		{
			SetControlMaster( null );
			SummonMaster = null;

            if( Employer != null )
            {
                try
                {
                    Mobile m = World.FindMobile( Employer.Serial );
                    PlayerMobile pm = m as PlayerMobile;

                    if( pm.Informants.Informant1.Serial == this.Serial )
                        pm.Informants.Informant1 = null;

                    if( pm.Informants.Informant2.Serial == this.Serial )
                        pm.Informants.Informant2 = null;

                    if( pm.Informants.Informant3.Serial == this.Serial )
                        pm.Informants.Informant3 = null;

                    if( pm.Informants.Informant4.Serial == this.Serial )
                        pm.Informants.Informant4 = null;

                    if( pm.Informants.Informant5.Serial == this.Serial )
                        pm.Informants.Informant5 = null;

                    Employer = null;
                }

                catch
                {
                    Employer = null;
                }
            }

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.Cancel();

			base.OnDelete();
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( target is BaseFactionGuard )
				return false;

			if ( (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier )
			{
				if ( message )
				{
					if ( target.Title == null )
						SendMessage( "{0} the vendor cannot be harmed.", target.Name );
					else
						SendMessage( "{0} {1} cannot be harmed.", target.Name, target.Title );
				}

				return false;
			}

			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}

		public override bool CanBeRenamedBy( Mobile from )
		{
			bool ret = base.CanBeRenamedBy( from );

			if ( Controlled && from == ControlMaster )
				ret = true;

			return ret;
		}

		public bool SetControlMaster( Mobile m )
		{
			if ( m == null )
			{
				ControlMaster = null;
				Controlled = false;
				ControlTarget = null;
				ControlOrder = OrderType.None;
				Guild = null;

				Delta( MobileDelta.Noto );
			}
			else
			{
				SpawnEntry se = this.Spawner as SpawnEntry;
				if ( se != null && se.UnlinkOnTaming )
				{
					this.Spawner.Remove( this );
					this.Spawner = null;
				}

				if ( m.Followers + ControlSlots > m.FollowersMax )
				{
					m.SendLocalizedMessage( 1049607 ); // You have too many followers to control that creature.
					return false;
				}

				CurrentWayPoint = null;//so tamed animals don't try to go back
			
				ControlMaster = m;
				Controlled = true;
				ControlTarget = null;
				ControlOrder = OrderType.Come;
				Guild = null;

				Delta( MobileDelta.Noto );
			}

			return true;
		}

		public override void OnRegionChange( Region Old, Region New )
		{
			base.OnRegionChange( Old, New );

			if ( this.Controlled )
			{
				SpawnEntry se = this.Spawner as SpawnEntry;

				if ( se != null && !se.UnlinkOnTaming && ( New == null || !New.AcceptsSpawnsFrom( se.Region ) ) )
				{
					this.Spawner.Remove( this );
					this.Spawner = null;
				}
			}
		}

		private static bool m_Summoning;

		public static bool Summoning
		{
			get{ return m_Summoning; }
			set{ m_Summoning = value; }
		}

		public static bool Summon( BaseCreature creature, Mobile caster, Point3D p, int sound, TimeSpan duration )
		{
			return Summon( creature, true, caster, p, sound, duration );
		}

		public static bool Summon( BaseCreature creature, bool controlled, Mobile caster, Point3D p, int sound, TimeSpan duration )
		{
			if ( caster.Followers + creature.ControlSlots > caster.FollowersMax )
			{
				caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				creature.Delete();
				return false;
			}

			m_Summoning = true;

			if ( controlled )
				creature.SetControlMaster( caster );

			creature.RangeHome = 10;
			creature.Summoned = true;

			creature.SummonMaster = caster;

			Container pack = creature.Backpack;

			if ( pack != null )
			{
				for ( int i = pack.Items.Count - 1; i >= 0; --i )
				{
					if ( i >= pack.Items.Count )
						continue;

					pack.Items[i].Delete();
				}
			}

			new UnsummonTimer( caster, creature, duration ).Start();
			creature.m_SummonEnd = DateTime.Now + duration;

			creature.MoveToWorld( p, caster.Map );

			Effects.PlaySound( p, creature.Map, sound );

			m_Summoning = false;

			return true;
		}

		private static bool EnableRummaging = true;

		private const double ChanceToRummage = 0.5; // 50%

		private const double MinutesToNextRummageMin = 1.0;
		private const double MinutesToNextRummageMax = 4.0;

		private const double MinutesToNextChanceMin = 0.25;
		private const double MinutesToNextChanceMax = 0.75;

		private DateTime m_NextRummageTime;

		public virtual bool CanBreath { get { return HasBreath && !Summoned; } }
		public virtual bool IsDispellable { get { return Summoned && !IsAnimatedDead; } }

		public virtual void FixScales()
		{
			if( this is IRacialMount )
			{
				this.XPScale = 3;
				this.StatScale = 3;
				this.SkillScale = 3;
			}
			
			else if( this is Dog )
			{
				this.XPScale = 1;
				this.StatScale = 1;
				this.SkillScale = 1;
			}
			
			else if( this is Horse || this is Wolf )
			{
				this.XPScale = 2;
				this.StatScale = 2;
				this.SkillScale = 2;
			}
			
			else if( !( this is Mercenary ) )
			{
				if( this.HitsMax < 50 )
				{
					this.XPScale = 1;
					this.StatScale = 1;
					this.SkillScale = 1;
				}
				
				else if( this.HitsMax < 100 )
				{
					this.XPScale = 2;
					this.StatScale = 2;
					this.SkillScale = 2;
				}
				
				else if( this.HitsMax < 200 )
				{
					this.XPScale = 3;
					this.StatScale = 3;
					this.SkillScale = 3;
				}
				
				else if( this.HitsMax < 300 )
				{
					this.XPScale = 4;
					this.StatScale = 4;
					this.SkillScale = 4;
				}
			}
			
			if( this.HitsMax > 400 || this is Mercenary )
			{
				this.XPScale = 5;
				this.StatScale = 5;
				this.SkillScale = 5;
			}
			
			if( this.HitsMax >= 300 )
				this.TakesLifeOnKill = true;
			
			this.FixedScales = true;
		}
		
		public CraftResource GetRandomLeather()
		{
			int chance = Utility.RandomMinMax( 1, 10 );
			
			if( chance == 4 || chance == 5 )
				return CraftResource.ThickLeather;
			
			if( chance == 6 || chance == 7 )
				return CraftResource.BeastLeather;
			
			if( chance == 8 || chance == 9 )
				return CraftResource.ScaledLeather;

			return CraftResource.RegularLeather;
		}
		
		public void AddLeastLoot( int amount, int chance )
		{
			amount--;
			
			if( Utility.RandomMinMax(1, 100) <= chance )
				AddLeastLoot();
			
			if( amount > 0 )
				AddLeastLoot( amount, chance );
		}
		
		public void AddLeastLoot( int amount )
		{
			AddLeastLoot( amount, 100 );
		}
		
		public void AddLeastLoot()
		{
			ItemPiece piece = new ItemPiece();
			
			if( Utility.RandomMinMax(1, 100) > 60 )
				piece.Type = ItemType.Clothing;
			
			else
				piece.Resource = GetLeastResource();
			
			if( Utility.RandomMinMax(1, 100) > 40 )
				piece.Type = ItemType.Armour;
			
			this.PackItem( piece );
		}
		
		public CraftResource GetLeastResource()
		{
			int chance = Utility.RandomMinMax( 1, 10 );
			
			if( chance == 3 || chance == 4 )
				return CraftResource.Bronze;
			
			if( chance == 5 || chance == 6 )
				return CraftResource.Oak;
			
			if( chance == 7 || chance == 8 )
				return CraftResource.Yew;
			
			if( chance == 9 )
				return CraftResource.Iron;
			
			if( chance == 10 )
				return CraftResource.Redwood;
			
			return CraftResource.Copper;
		}
		
		public void AddLesserLoot( int amount, int chance )
		{
			amount--;
			
			if( Utility.RandomMinMax(1, 100) <= chance )
				AddLesserLoot();
			
			if( amount > 0 )
				AddLesserLoot( amount, chance );
		}
		
		public void AddLesserLoot( int amount )
		{
			AddLesserLoot( amount, 100 );
		}
		
		public void AddLesserLoot()
		{
			ItemPiece piece = new ItemPiece();
			
			if( Utility.RandomMinMax(1, 100) > 70 )
				piece.Type = ItemType.Clothing;
			
			else
				piece.Resource = GetLesserResource();
			
			if( Utility.RandomMinMax(1, 100) > 50 )
			{
				piece.Type = ItemType.Armour;
				
				if( Utility.RandomMinMax(1, 100) > 40 )
					piece.Resource = GetRandomLeather();
			}
			
			this.PackItem( piece );
		}
		
		public CraftResource GetLesserResource()
		{
			int chance = Utility.RandomMinMax( 1, 10 );
			
			if( chance == 2 || chance == 3 )
				return CraftResource.Bronze;

			if( chance == 4 || chance == 5 || chance == 6 )
				return CraftResource.Yew;
			
			if( chance == 7 || chance == 8 )
				return CraftResource.Iron;
			
			if( chance == 9 || chance == 10 )
				return CraftResource.Redwood;
			
			return CraftResource.Bronze;
		}
		
		public void AddGreaterLoot( int amount, int chance )
		{
			amount--;
			
			if( Utility.RandomMinMax(1, 100) <= chance )
				AddGreaterLoot();
			
			if( amount > 0 )
				AddGreaterLoot( amount, chance );
		}
		
		public void AddGreaterLoot( int amount )
		{
			AddGreaterLoot( amount, 100 );
		}
		
		public void AddGreaterLoot()
		{
			ItemPiece piece = new ItemPiece();
			piece.Masterwork = true;
			
			if( Utility.RandomMinMax(1, 100) > 80 )
				piece.Type = ItemType.Clothing;
			
			else
				piece.Resource = GetGreaterResource();
			
			if( Utility.RandomMinMax(1, 100) > 50 )
			{
				piece.Type = ItemType.Armour;
				
				if( Utility.RandomMinMax(1, 100) > 60 )
					piece.Resource = GetRandomLeather();
			}
			
			this.PackItem( piece );
		}
		
		public CraftResource GetGreaterResource()
		{
			int chance = Utility.RandomMinMax( 1, 10 );
			
			if( chance == 1 || chance == 2 )
				return CraftResource.Bronze;
			
			if( chance == 3 )
				return CraftResource.Copper;

			if( chance == 4 || chance == 5 )
				return CraftResource.Redwood;
			
			if( chance == 6 )
				return CraftResource.Oak;
			
			if( chance == 7 )
				return CraftResource.Ash;
			
			if( chance == 8 )
				return CraftResource.Yew;
			
			return CraftResource.Iron;
		}
		
		public void AddGreatestLoot( int amount, int chance )
		{
			amount--;
			
			if( Utility.RandomMinMax(1, 100) <= chance )
				AddGreatestLoot();
			
			if( amount > 0 )
				AddGreatestLoot( amount, chance );
		}
		
		public void AddGreatestLoot( int amount )
		{
			AddGreatestLoot( amount, 100 );
		}
		
		public void AddGreatestLoot()
		{
			ItemPiece piece = new ItemPiece();
			piece.Masterwork = true;
			
			if( Utility.RandomMinMax(1, 100) > 90 )
				piece.Type = ItemType.Clothing;
			
			else
				piece.Resource = GetGreatestResource();
			
			if( piece.Resource != CraftResource.Linen && Utility.RandomMinMax(1, 100) > 50 )
				piece.Type = ItemType.Armour;
			
			this.PackItem( piece );
		}
		
		public CraftResource GetGreatestResource()
		{
			int chance = Utility.RandomMinMax( 1, 20 );
			
			if( chance == 1 || chance == 2 )
				return CraftResource.Silver;
			
			if( chance == 3 )
				return CraftResource.Gold;
			
			if( chance == 4 )
				return CraftResource.Steel;
			
			if( chance == 5 )
				return CraftResource.Obsidian;

			if( chance == 6 || chance == 7 || chance == 8 )
				return CraftResource.Redwood;
			
			if( chance == 9 || chance == 10 || chance == 11 )
				return CraftResource.Bronze;
			
			if( chance == 12 || chance == 13 || chance == 14 )
				return CraftResource.Ash;
			
			if( chance == 15 )
				return CraftResource.Copper;
			
			if( chance == 16 )
				return CraftResource.Oak;
			
			if( chance == 17 )
				return CraftResource.Linen;
			
			return CraftResource.Iron;
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveLeastLoot
		{
			get{ return false; }
			set
			{
				if( value == true )
					AddLeastLoot();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveLesserLoot
		{
			get{ return false; }
			set
			{
				if( value == true )
					AddLesserLoot();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveGreaterLoot
		{
			get{ return false; }
			set
			{
				if( value == true )
					AddGreaterLoot();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveGreatestLoot
		{
			get{ return false; }
			set
			{
				if( value == true )
					AddGreatestLoot();
			}
		}
		
		public virtual void HandleNewLoot()
		{
			if( Fame >= 70000 )
			{
				AddLeastLoot( 1, 32 );
				AddLesserLoot( 1, 16 );
				AddGreaterLoot( 1, 8 );
				AddGreatestLoot( 1, 4 );
			}
			
			else if( Fame >= 50000 )
			{
				AddLeastLoot( 1, 16 );
				AddLesserLoot( 1, 8 );
				AddGreaterLoot( 1, 4 );
				AddGreatestLoot( 1, 2 );
			}
			
			else if( Fame >= 40000 )
			{
				AddLeastLoot( 1, 8 );
				AddLesserLoot( 1, 4 );
				AddGreaterLoot( 1, 2 );
			}
			
			else if( Fame >= 30000 )
			{
				AddLeastLoot( 1, 4 );
				AddLesserLoot( 1, 2 );
				AddGreaterLoot( 1, 1 );
			}
			
			else if( Fame >= 20000 )
			{
				AddLeastLoot( 1, 2 );
				AddLesserLoot( 1, 1 );
			}
			
			m_ReceivedNewLoot = true;
		}
		
		public virtual BaseImprovedAI ImprovedAI{ get{ return new BaseImprovedAI(); } }
		
		public virtual void OnThink()
		{
            if (this != null && !this.Deleted && this.Alive)
            {
                IPooledEnumerable eable = Map.GetMobilesInRange(Location, RangePerception);
                foreach (Mobile m in eable)
                {
                    if(m != null && !m.Deleted && m.Alive)
                        OrderInfo.ExecuteOrders(this, m);
                }
                eable.Free();
            }

			if( !m_HadFirstThought )
			{
				FavouriteStance = m_FavouriteStance;
				FavouriteManeuver = m_FavouriteManeuver;
				m_HadFirstThought = true;
			}
			
			if( !m_ReceivedNewLoot )
				HandleNewLoot();
			
			ImprovedAI.HandleStance(this);
			
			if( SetManeuver != null )
				ImprovedAI.ChangeToSetManeuver(this);
			
			ImprovedAI.HandleRage(this);
			
			//Only fix scales if it's not a newly born creature
			if( !FixedScales )
				FixScales();

            if (!(this is Mercenary) && !(this is Soldier) && (Utility.RandomMinMax(1, 100) > (94 + XPScale) || Hits < ((HitsMax / 4) * 3)) && Hunger > 0)
            {
                if (!(this is Employee))
                {
                    Hunger--;
                }
                else
                {
                    Employee e = this as Employee;
                    if (e.IsSlave)
                    {
                        int leadership = 0;
                        if (this.Controlled && this.ControlMaster != null && !this.ControlMaster.Deleted && this.ControlMaster is PlayerMobile)
                            leadership = (ControlMaster as PlayerMobile).Feats.GetFeatLevel(FeatList.Leadership);
                        if (Utility.RandomMinMax(1, 100) <= (4 - leadership))
                            Hunger--;
                    }
                }
            }
			
			if ( EnableRummaging && CanRummageCorpses && !Summoned && !Controlled && DateTime.Now >= m_NextRummageTime )
			{
				double min, max;

				if ( ChanceToRummage > Utility.RandomDouble() && Rummage() )
				{
					min = MinutesToNextRummageMin;
					max = MinutesToNextRummageMax;
				}
				else
				{
					min = MinutesToNextChanceMin;
					max = MinutesToNextChanceMax;
				}

				double delay = min + (Utility.RandomDouble() * (max - min));
				m_NextRummageTime = DateTime.Now + TimeSpan.FromMinutes( delay );
			}

			if ( CanBreath && DateTime.Now >= m_NextBreathTime ) // tested: controled dragons do breath fire, what about summoned skeletal dragons?
			{
				Mobile target = this.Combatant;

				if ( target != null && target.Alive && !target.IsDeadBondedPet && CanBeHarmful( target ) && target.Map == this.Map && !IsDeadBondedPet && target.InRange( this, BreathRange ) && InLOS( target ) && !BardPacified )
					BreathStart( target );

				m_NextBreathTime = DateTime.Now + TimeSpan.FromSeconds( BreathMinDelay + (Utility.RandomDouble() * BreathMaxDelay) );
                CanUseSpecial = false;
			}

            if( CanUseSpecial && RangedAttackType != RangedAttackType.None )
            {
                Mobile target = this.Combatant;

                if( target != null && target.Alive && !target.IsDeadBondedPet && CanBeHarmful( target ) && target.Map == this.Map && !IsDeadBondedPet && !InRange( target, RangeFight ) && target.InRange( this, BreathRange ) && InLOS( target ) && !BardPacified )
                {
                    if( RangedAttackType >= RangedAttackType.BreathePoison && RangedAttackType <= RangedAttackType.BreatheEnergy )
                        PerformCustomBreathAttack( target );
                }

                CanUseSpecial = false;
            }
		}

		public virtual bool Rummage()
		{
			Corpse toRummage = null;

			foreach ( Item item in this.GetItemsInRange( 2 ) )
			{
				if ( item is Corpse && item.Items.Count > 0 )
				{
					toRummage = (Corpse)item;
					break;
				}
			}

			if ( toRummage == null )
				return false;

			Container pack = this.Backpack;

			if ( pack == null )
				return false;

			List<Item> items = toRummage.Items;

			bool rejected;
			LRReason reason;

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[Utility.Random( items.Count )];

				Lift( item, item.Amount, out rejected, out reason );

				if ( !rejected && Drop( this, new Point3D( -1, -1, 0 ) ) )
				{
					// *rummages through a corpse and takes an item*
					PublicOverheadMessage( MessageType.Emote, 0x3B2, 1008086 );
					return true;
				}
			}

			return false;
		}

		public void Pacify( Mobile master, DateTime endtime )
		{
			BardPacified = true;
			BardEndTime = endtime;
		}

		public override Mobile GetDamageMaster( Mobile damagee )
		{
			if ( m_bBardProvoked && damagee == m_bBardTarget )
				return m_bBardMaster;
			else if ( m_bControlled && m_ControlMaster != null )
				return m_ControlMaster;
			else if ( m_bSummoned && m_SummonMaster != null )
				return m_SummonMaster;

			return base.GetDamageMaster( damagee );
		}
 
		public void Provoke( Mobile master, Mobile target, bool bSuccess )
		{
			BardProvoked = true;

			this.PublicOverheadMessage( MessageType.Emote, EmoteHue, false, "*looks furious*" );
 
			if ( bSuccess )
			{
				PlaySound( GetIdleSound() );
 
				BardMaster = master;
				BardTarget = target;
				Combatant = target;
				BardEndTime = DateTime.Now + TimeSpan.FromSeconds( 30.0 );

				if ( target is BaseCreature )
				{
					BaseCreature t = (BaseCreature)target;

					if ( t.Unprovokable || (t.IsParagon && BaseInstrument.GetBaseDifficulty( t ) >= 160.0) )
						return;

					t.BardProvoked = true;

					t.BardMaster = master;
					t.BardTarget = this;
					t.Combatant = this;
					t.BardEndTime = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
				}
			}
			else
			{
				PlaySound( GetAngerSound() );

				BardMaster = master;
				BardTarget = target;
			}
		}

		public bool FindMyName( string str, bool bWithAll )
		{
			int i, j;

			string name = this.Name;
 
			if( name == null || str.Length < name.Length )
				return false;
 
			string[] wordsString = str.Split(' ');
			string[] wordsName = name.Split(' ');
 
			for ( j=0 ; j < wordsName.Length; j++ )
			{
				string wordName = wordsName[j];
 
				bool bFound = false;
				for ( i=0 ; i < wordsString.Length; i++ )
				{
					string word = wordsString[i];

					if ( Insensitive.Equals( word, wordName ) )
						bFound = true;
 
					if ( bWithAll && Insensitive.Equals( word, "all" ) )
						return true;
				}
 
				if ( !bFound )
					return false;
			}
 
			return true;
		}

		public static void TeleportPets( Mobile master, Point3D loc, Map map )
		{
			TeleportPets( master, loc, map, false );
		}

		public static void TeleportPets( Mobile master, Point3D loc, Map map, bool onlyBonded )
		{
			List<Mobile> move = new List<Mobile>();

			foreach ( Mobile m in master.GetMobilesInRange( 3 ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature pet = (BaseCreature)m;

					if ( pet.Controlled && pet.ControlMaster == master )
					{
						if ( !onlyBonded || pet.IsBonded )
						{
							if ( pet.ControlOrder == OrderType.Guard || pet.ControlOrder == OrderType.Follow || pet.ControlOrder == OrderType.Come )
								move.Add( pet );
						}
					}
				}
			}

			foreach ( Mobile m in move )
				m.MoveToWorld( loc, map );
		}

		public virtual void ResurrectPet()
		{
			if ( !IsDeadPet )
				return;

			OnBeforeResurrect();

			Poison = null;

			Warmode = false;

			Hits = 10;
			Stam = StamMax;
			Mana = 0;

			ProcessDeltaQueue();

			IsDeadPet = false;

			Effects.SendPacket( Location, Map, new BondedStatus( 0, this.Serial, 0 ) );

			this.SendIncomingPacket();
			this.SendIncomingPacket();

			OnAfterResurrect();
			PoisonEffect.Cure( this );

			Mobile owner = this.ControlMaster;

			if ( owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange( this, 12 ) || !this.CanSee( owner ) || !this.InLOS( owner ) )
			{
				if ( this.OwnerAbandonTime == DateTime.MinValue )
					this.OwnerAbandonTime = DateTime.Now;
			}
			else
			{
				this.OwnerAbandonTime = DateTime.MinValue;
			}

			CheckStatTimers();
		}

		public override bool CanBeDamaged()
		{
			if ( IsDeadPet )
				return false;

			return base.CanBeDamaged();
		}

		public virtual bool PlayerRangeSensitive{ get{ return true; } }

		public override void OnSectorDeactivate()
		{
			if ( PlayerRangeSensitive && m_AI != null )
				m_AI.Deactivate();

			base.OnSectorDeactivate();
		}

		public override void OnSectorActivate()
		{
			if ( PlayerRangeSensitive && m_AI != null )
				m_AI.Activate();

			base.OnSectorActivate();
		}

		private bool m_RemoveIfUntamed;

		// used for deleting untamed creatures [in houses]
		private int m_RemoveStep; 

		[CommandProperty( AccessLevel.GameMaster )] 
		public bool RemoveIfUntamed{ get{ return m_RemoveIfUntamed; } set{ m_RemoveIfUntamed = value; } }

		[CommandProperty( AccessLevel.GameMaster )] 
		public int RemoveStep { get { return m_RemoveStep; } set { m_RemoveStep = value; } }
	}

	public class LoyaltyTimer : Timer
	{
		private static TimeSpan InternalDelay = TimeSpan.FromMinutes( 5.0 );

		public static void Initialize()
		{
			new LoyaltyTimer().Start();
		}

		public LoyaltyTimer() : base( InternalDelay, InternalDelay )
		{
			m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours( 1.0 );
			Priority = TimerPriority.FiveSeconds;
		}

		private DateTime m_NextHourlyCheck;

		protected override void OnTick() 
		{ 
			bool hasHourElapsed = ( DateTime.Now >= m_NextHourlyCheck );

			if ( hasHourElapsed )
				m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours( 1.0 );

			List<BaseCreature> toRelease = new List<BaseCreature>();

			// added array for wild creatures in house regions to be removed
			List<BaseCreature> toRemove = new List<BaseCreature>();

			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( ( m is BaseMount && ((BaseMount)m).Rider != null ) || m is IClericSummon || m is Mercenary )
				{
					((BaseCreature)m).OwnerAbandonTime = DateTime.MinValue;
					continue;
				}

                if (m is PlayerMobile)
                {
                    if ((m as PlayerMobile).Group.Members.Count > 0)
                        GroupInfo.CheckReset(m as PlayerMobile);
                }

				if ( m is BaseCreature && !(m is Employee && (m as BaseCreature).ControlMaster != null))
				{
					BaseCreature c = (BaseCreature)m;

					if ( c.IsDeadPet )
					{
						Mobile owner = c.ControlMaster;

						if ( owner == null || owner.Deleted || owner.Map != c.Map || !owner.InRange( c, 12 ) || !c.CanSee( owner ) || !c.InLOS( owner ) )
						{
							if ( c.OwnerAbandonTime == DateTime.MinValue )
								c.OwnerAbandonTime = DateTime.Now;
							else
							{
								try
								{
									if ( (c.OwnerAbandonTime + c.BondingAbandonDelay) <= DateTime.Now )
										toRemove.Add( c );
								}
								catch
								{
									toRemove.Add( c );
								}
							}
						}
						else
						{
							c.OwnerAbandonTime = DateTime.MinValue;
						}
					}
					else if ( c.Controlled && c.Commandable && !(c is Employee))
					{
						c.OwnerAbandonTime = DateTime.MinValue;
						
						if ( c.Map != Map.Internal && !c.Blessed )
						{
							// Every hour all pets lose 10% of max loyalty.
							if ( hasHourElapsed )
								c.Loyalty -= (BaseCreature.MaxLoyalty / 10);

							if( c.Loyalty < (BaseCreature.MaxLoyalty / 10) )
							{
								c.Say( 1043270, c.Name ); // * ~1_NAME~ looks around desperately *
								c.PlaySound( c.GetIdleSound() );
							}

							if ( c.Loyalty <= 0 )
								toRelease.Add( c );
						}
					}

					// added lines to check if a wild creature in a house region has to be removed or not
					if ( !(c is Soldier) && !c.Controlled && ( c.Region.IsPartOf( typeof( HouseRegion ) ) && c.CanBeDamaged() || hasHourElapsed && c.RemoveIfUntamed && c.Spawner == null ) )
					{
						c.RemoveStep++;

						if ( c.RemoveStep >= 20 )
							toRemove.Add( c );
					}
					else
					{
						c.RemoveStep = 0;
					}
				}
			}

			foreach ( BaseCreature c in toRelease )
			{
				c.Say( 1043255, c.Name ); // ~1_NAME~ appears to have decided that is better off without a master!
				c.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
				c.IsBonded = false;
				c.BondingBegin = DateTime.MinValue;
				c.OwnerAbandonTime = DateTime.MinValue;
				c.ControlTarget = null;
				//c.ControlOrder = OrderType.Release;
				c.AIObject.DoOrderRelease(); // this will prevent no release of creatures left alone with AI disabled (and consequent bug of Followers)
			}

			// added code to handle removing of wild creatures in house regions
			foreach ( BaseCreature c in toRemove )
			{
				c.Delete();
			}
		}
	}
}
