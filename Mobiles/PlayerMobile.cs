using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Multis;
using Server.Engines.Help;
using Server.ContextMenus;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Necromancy;
using Server.Spells.ExoticWeaponry;
using Server.Spells.Bushido;
using Server.Targeting;
using Server.Engines.Quests;
using Server.Factions;
using Server.Regions;
using Server.Accounting;
using Server.Engines.CannedEvil;
using Server.Engines.Craft;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
	#region Enums
	[Flags]
	public enum PlayerFlag // First 16 bits are reserved for default-distro use, start custom flags at 0x00010000
	{
		None				= 0x00000000,
		Glassblowing		= 0x00000001,
		Masonry				= 0x00000002,
		SandMining			= 0x00000004,
		StoneMining			= 0x00000008,
		ToggleMiningStone	= 0x00000010,
		KarmaLocked			= 0x00000020,
		AutoRenewInsurance	= 0x00000040,
		UseOwnFilter		= 0x00000080,
		PublicMyRunUO		= 0x00000100,
		PagingSquelched		= 0x00000200,
		Young				= 0x00000400,
		AcceptGuildInvites	= 0x00000800,
		DisplayChampionTitle= 0x00001000,
	}
	
	public enum NpcGuild
	{
		None,
		MagesGuild,
		WarriorsGuild,
		ThievesGuild,
		RangersGuild,
		HealersGuild,
		MinersGuild,
		MerchantsGuild,
		TinkersGuild,
		TailorsGuild,
		FishermensGuild,
		BardsGuild,
		BlacksmithsGuild
	}

	public enum SolenFriendship
	{
		None,
		Red,
		Black
	}
	
	public enum ChosenDeity
	{
		None,
		Arianthynt,
		Xipotec,
		Mahtet,
		Xorgoth,
		Ohlm,
		Elysia
	}
	
	public enum KnownLanguage
	{
		Common,
		Alyrian,
		Azhuran,
		Khemetar,
		Mhordul,
		Tyrean,
		Vhalurian,
		Shorthand
	}
	
	public enum BackgroundList
	{
		Strong,
		Quick,
		Smart,
		Tough,
		Fit,
		IronWilled,
		Weak,
		Clumsy,
		Feebleminded,
		Frail,
		Unenergetic,
		WeakWilled,
		QuickHealer,
		Resilient,
		FocusedMind,
		SlowHealer,
		OutOfShape,
		UnfocusedMind,
		Lucky,
		Unlucky,
		Attractive,
		GoodLooking,
		Gorgeous,
		Homely,
		Ugly,
		Hideous,
		Mute,
		Deaf,
		Lame,
		AnimalEmpathy,
		Faithful,
		Hardcore,
		None
	}

    public enum FeatList
    {
        None,
        ShieldBash,
        Cleave,
        ArmourFocus,
        CripplingBlow,
        BruteStrength,
        QuickReflexes,
        BackToBack,
        RopeTrick,
        NonLethalTraps,
        EscortPrisoner,
        AlyrianLanguage,
        AzhuranLanguage,
        KhemetarLanguage,
        MhordulLanguage,
        Intimidate,
        ThrowingMastery,
        CriticalStrike,
        FocusedAttack,
        FlurryOfBlows,
        Trample,
        MountedCharge,
        PolearmsMastery,
        MountedMomentum,
        MountedCombat,
        MountedEndurance,
        MountedDefence,
        TripFoe,
        DefensiveStance,
        SavageStrike,
        GreatweaponFighting,
        DamageIgnore,
        Rage,
        TirelessRage,
        DefensiveFury,
        FastHealing,
        Evade,
        EnhancedDodge,
        StunningBlow,
        EyeRaking,
        BullRush,
        CatchProjectiles,
        Technique,
        Buildup,
        EarBoxing,
        ThroatStrike,
        RacialFightingStyle,
        Disarm,
        Dismount,
        Disable,
        TyreanLanguage,
        FlashyAttack,
        FightingStyle,
        WeaponSpecialization,
        WeaponParrying,
        SunderWeapon,
        SecondSpecialization,
        CircularAttack,
        BowMastery,
        CriticalShot,
        CripplingShot,
        FocusedShot,
        SwiftShot,
        HailOfArrows,
        FarShot,
        TravelingShot,
        MountedArchery,
        ReusableAmmunition,
        CrossbowMastery,
        AimedShot,
        QuickTravelingShot,
        DeflectProjectiles,
        ShieldMastery,
        Feint,
        HeavyLifting,
        Potter,
        GlassBlower,
        Painter,
        Sculptor,
        VerifyCurrency,
        WolfBreeding,
        DogBreeding,
        ExtraPetFeats,
        RacialMounts,
        VhalurianLanguage,
        AnimalTraining,
        PetFeats,
        AnimalControl,
        RetrainPet,
        HorseBreeding,
        LowerSideEffects,
        Fireworks,
        BlackPowder,
        HerbalGathering,
        RacialStaining,
        RacialResource,
        GemHarvesting,
        EnhancePotion,
        Professor,
        Cryptography,
        Blacksmithing,
        Carpentry,
        Fletching,
        Tailoring,
        Tinkering,
        Craftsmanship,
        Alchemy,
        Cartography,
        Inscription,
        EnhancedHarvesting,
        ImprovedSkinning,
        AdvancedDying,
        DurableCrafts,
        Masterwork,
        RenownedMasterwork,
        LeatherDying,
        Shipwright,
        AdvancedLumberjacking,
        WoodStaining,
        EnhancedArms,
        GemEmbedding,
        AdvancedMining,
        ArmourEnameling,
        EnhancedStealth,
        OilMaking,
        DrumsOfWar,
        RacialDyes,
        InspireResilience,
        SongOfEnthrallment,
        SongOfMockery,
        InspireFortitude,
        LingeringCommand,
        WidespreadCommand,
        InspireHeroics,
        RacialEnameling,
        ExpeditiousRetreat,
        Ventriloquism,
        DisguiseOthers,
        ArmouredDodge,
        DisguiseKit,
        PetStealing,
        Hideout,
        Counterfeiting,
        Stash,
        Shorthand,
        ArmouredStealth,
        JudgeWealth,
        PlantEvidence,
        Cutpurse,
        Locksmith,
        Backstab,
        BleedingStrike,
        Mining,
        Lumberjacking,
        HerbalLore,
        AnimalTaming,
        AnimalLore,
        Veterinary,
        MartialOffence,
        HealWounds,
        InflictWounds,
        CureFamine,
        Bless,
        ConsecrateItem,
        Curse,
        HaloOfLight,
        Mending,
        HoldPerson,
        HolyStrike,
        ShieldOfSacrifice,
        SacredBlast,
        Sanctuary,
        AuraOfProtection,
        SummonProtector,
        DivineConsecration,
        CustomMageSpell,
        RangedSpells,
        RangedEffect,
        ExplosiveEffect,
        RecurrentEffect,
        ChainEffect,
        StatusEffect,
        EnchantArmour,
        EnchantWeapon,
        EnchantRing,
        EnchantClothing,
        EnchantTrinket,
        EnchantBracelet,
        LifeI,
        LifeII,
        DeathI,
        DeathII,
        PetEvolution,
        MercTraining,
        GuildManeuver,
        GuildStance,
        MatterI,
        MatterII,
        MindI,
        MindII,
        PrimeI,
        PrimeII,
        ForcesI,
        ForcesII,
        SpiritI,
        SpiritII,
        TimeI,
        TimeII,
        FateI,
        FateII,
        SpaceI,
        SpaceII,
        LightArmour,
        MediumArmour,
        HeavyArmour,
        Skinning,
        Dodge,
        Cooking,
        SearingBreath,
        SwipingClaws,
        TempestuousSea,
        SilentHowl,
        ThunderingHooves,
        VenomousWay,
        HairStyling,
        Alertness,
        BusinessMentor,
        CombinedCommandsI,
        CombinedCommandsII,
        DamagingEffect,
        EnhancedTracking,
        Finesse,
        JewelryCrafting,
        PoisonResistance,
        PureDodge,
        Teaching,
        Archery,
        UnarmedFighting,
        Tactics,
        Anatomy,
        Poisoning,
        Healing,
        MagicResistance,
        DetectHidden,
        Hiding,
        Stealth,
        DisarmTraps,
        Snooping,
        Stealing,
        Lockpicking,
        Parrying,
        Swordsmanship,
        Macing,
        Fencing,
        Polearms,
        Axemanship,
        ExoticWeaponry,
        Throwing,
        Linguistics,
        Camping,
        Riding,
        Leadership,
        Faith,
        Invocation,
        Meditation,
        Concentration,
        Magery,
        Tracking,
        Fishing,
        Greenheart,
        Obsidian,
        Linen,
        Bone,
        Steel,
        Pusantia,
        Farming,
        CraftingSpecialization,
        Obfuscate,
        Feeding,
        Protean,
        Terror,
        ControlUndead,
        Awe,
        Celerity,
        Shapeshift,
        NocturnalProwess,
        Daywalker,
        VampireAbilities,
        PlateMastery,
        WineCrafting,
        Brewer,
		TattooArtist,
        Snakecharmer,
        Beartalker,
        Safecracking,
        RangedDefense,
        LeadershipMastery,
        AvianBreeding,
        Barbery,
        Medicine,
        Surgery,
        Longstrider,
        Ridgeking,
        ScarabWarrior,
        Bearjarl,
        Fanglord,
        Horselord,
        Pathology,
        Compassion,
        Humility,
        Justice,
        HallowGround,
        ArmourSmithing,
        WeaponSmithing,
        Damage,
        Speed,
        HCI,
        DCI,
        Blunt,
        Slashing,
        Piercing,
        Consecrate,
        BalanceDestiny
    }

    public enum Subclass // We're no longer using this.
    {
        None,
        BountyHunter,
        Dragoon,
        Berserker,
        MartialArtist,
        WeaponSpecialist,
        Archer,
        Fighter,
        Stableworker,
        Scholar,
        Tavernworker,
        Tailor,
        Woodworker,
        Metalworker,
        Bard,
        Thief,
        Assassin
    }

    public enum Advanced // We're no longer using this.
    {
        None,
        Cleric,
        BountyHunter,
        Dragoon,
        Berserker,
        MartialArtist,
        WeaponSpecialist,
        Archer,
        Fighter,
        Stableworker,
        Scholar,
        Tavernworker,
        Tailor,
        Woodworker,
        Metalworker,
        Bard,
        Thief,
        Assassin,
        MageExpanded
    }

	public enum Class // We're no longer using thing.
	{
		None,
		Cleric,
		Crafter,
		Mage,
		Rogue,
		Warrior
	}
	
	public enum Nation
	{
		None,
		Azhuran,
		Mhordul,
		Vhalurian,
		Tyrean,
		Alyrian,
		Khemetar,
		Imperial,
        Society,
        Sovereign,
		Insularii
	}

	public enum SongList
	{
		None,
		ExpeditiousRetreat,
		InspireFortitude,
		InspireHeroics,
		InspireResilience,
		SongOfEnthrallment,
		SongOfMockery
	}
	
	#endregion

	public class PlayerMobile : Mobile, IHonorTarget, IKhaerosMobile
	{
		private DateTime m_LastDonationLife;
		private string m_Description;
		private string m_Description2;
		private string m_Description3;
		private Backgrounds m_Backgrounds;
		private Masterwork m_Masterwork;
		private RacialResources m_RacialResources;
		private Friendship m_Friendship;
		private KnownLanguage m_SpokenLanguage;
		private string m_RPTitle;
		private string m_TitlePrefix;
		private int m_Level;
		private int m_XP;
		private int m_NextLevel;
		private int m_CP;
		private int m_StatPoints;
		private int m_SkillPoints;
		private int m_FeatSlots;
		private int m_MaxHits;
		private int m_MaxStam;
		private int m_MaxMana;
		private KnownLanguages m_KnownLanguages;
        private Feats m_Feats;
        private CombatStyles m_CombatStyles;
        private Informants m_Informants;
        private Dictionary<CustomGuildStone, CustomGuildInfo> m_CustomGuilds;
        private double m_Bonus;
        private SkillMod m_SkillMod0;
        private SkillMod m_SkillMod1;
        private SkillMod m_SkillMod2;
        private Mobile m_EscortPrisoner;
        private Mobile m_HuntingHound;
        private int m_FocusedAttack;
        private int m_FlurryOfBlows;
        private Direction m_FormerDirection;
        private int m_ChargeSteps;
        public Timer m_Tripped;
        private int m_RageHits;
        private int m_RageFeatLevel;
        public Timer m_StunnedTimer;
        public Timer m_BlindnessTimer;
        public Timer m_DeafnessTimer;
        public Timer m_MutenessTimer;
        private int m_SearingBreath;
        private int m_SwipingClaws;
        private int m_TempestuousSea;
        private int m_SilentHowl;
        private int m_ThunderingHooves;
        private int m_VenomousWay;
        public Timer m_DismountedTimer;
        public Timer m_DisabledLegsTimer;
        public Timer m_DisabledLeftArmTimer;
        public Timer m_DisabledRightArmTimer;
        private string m_WeaponSpecialization;
        private string m_SecondSpecialization;
        private int m_FocusedShot;
        private int m_SwiftShot;
        public Timer m_FeintTimer;
        private DateTime m_NextAllowance;
        public Timer m_HealingTimer;
        private List<Mobile>	m_AllyList;
        private List<Mobile>	m_LoggedOutPets;
        private int m_Lives;
        private object m_LastSayEmote;
        private DateTime m_LastOffenseToNature;
        public Timer m_DeathTimer;
        public Timer m_InvulTimer;
        private int m_RecreateXP;
        private int m_RecreateCP;
        public Timer m_PetrifiedTimer;
        private Timer m_FreezeTimer;
        public Timer FreezeTimer{ get{ return m_FreezeTimer; } set{ m_FreezeTimer = value; } }
        public DateTime LastStep;
        private ResourceController m_UniqueSpot;
        private int m_Age;
        private Timer m_RageTimer;
        private DateTime m_NextBirthday;
        private int m_MaxAge;
        private int m_CPSpent;
        private int m_CPCapOffset;
        public Timer m_CharInfoTimer;
        private int m_HearAll;
        private DateTime m_NextFeatUse;
        private bool m_AlyrianGuard;
        private bool m_AzhuranGuard;
        private bool m_KhemetarGuard;
        private bool m_MhordulGuard;
        private bool m_TyreanGuard;
        private bool m_VhalurianGuard;
        private bool m_ImperialGuard;
        private string m_DayOfBirth;
        private string m_MonthOfBirth;
        private string m_YearOfBirth;
        private bool m_HideStatus;
        public int HuntingHoundBonusSkill;
        private bool m_Spar;
        private bool m_XPFromCrafting;
        private bool m_XPFromKilling;
        private FeatList m_CurrentSpell;
        public int SpellPower;
        public bool m_FixedStatPoints;
        public DateTime m_NextMending;
        public Timer m_Lasso;
        public Timer m_KOPenalty;
        private bool m_FixedStyles;
        private bool m_FixedReflexes;
        private Mobile m_ShieldingMobile;
        private Mobile m_ShieldedMobile;
        private double m_ShieldValue;
        private Timer m_AuraOfProtection;
        private Timer m_JusticeAura;
        private Timer m_Sanctuary;
        public DateTime NextSummoningAllowed;
        public int LastXP;
        public bool XPFromLearning;
        private Timer m_CrippledTimer;
        private Timer m_DazedTimer;
        private Timer m_TrippedTimer;
        private bool m_FixedRage;
        public DateTime m_LastAttack;
        private int m_LightPenalty;
        private int m_MediumPenalty;
        private int m_HeavyPenalty;
        private int m_ArmourPieces;
        private int m_LightPieces;
		private int m_MediumPieces;
		private int m_HeavyPieces;
        private bool m_FixedRun;
        private int m_FakeHair;
        private int m_FakeHairHue;
        private int m_FakeFacialHair;
        private int m_FakeFacialHairHue;
        private int m_FakeHue;
        private string m_FakeRPTitle;
        private string m_FakeTitlePrefix;
        private bool m_HasStash;
        private bool m_SmoothPicking;
        private bool m_AutoPicking;
        private string m_FakeAge;
        private string m_FakeLooks;
        private DateTime m_LogoutTime;
        private DateTime m_NextSongAllowed;
        private bool m_Crafting = false;
        private Timer m_BloodOfXorgoth;
        private Timer m_ManeuverBonusTimer;
        private int m_ManeuverDamageBonus;
        private int m_ManeuverAccuracyBonus;
        private DateTime m_NextRage;
        private bool m_GemHarvesting;
        private CustomSpellBook m_SpellBook;
        private Container m_CraftContainer;
        private bool m_PureDodge;
        private bool m_OldMapChar;
        private bool m_CanBeThief;
        private DateTime m_EmptyBankBoxOn;
        private string m_CraftingSpecialization;
        private Item m_Claws;
        private int m_Crimes;        
        private Dictionary<Nation, int> m_CrimesList;
        private DateTime m_NextCriminalAct;
        private bool m_CriminalActivity;
        private bool m_Disguised;
        private DateTime m_LastDisguiseTime;
        private DateTime m_LastDeath;
		private int m_AvatarID;
		private bool m_IsApprentice;
        private GroupInfo m_Group;
		private int m_CustomAvatarID1;
		private int m_CustomAvatarID2;
		private int m_CustomAvatarID3;
		private bool m_IsHardcore;
		private int m_Maimings;
		private DateTime m_HCWound;
        private bool m_CanBeFaithful;
        private int m_ConsecratedItems;
        public bool SmithTesting;


        public Item Claws 
        { 
            get 
            {
                if( m_Claws != null && m_Claws.Deleted )
                    m_Claws = null;

                return m_Claws; 
            } 
            set { m_Claws = value; } 
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string CraftingSpecialization{ get{ return m_CraftingSpecialization; } set{ m_CraftingSpecialization = value; } }
        
        public DateTime EmptyBankBoxOn{ get{ return m_EmptyBankBoxOn; } set{ m_EmptyBankBoxOn = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool CanBeThief{ get{ return m_CanBeThief; } set{ m_CanBeThief = value; } }
        
        public bool OldMapChar{ get{ return m_OldMapChar; } set{ m_OldMapChar = value; } }
        
        private Timer m_ChangeStanceTimer;
        private Timer m_InspireHeroicsTimer;
        private Timer m_InspireResilienceTimer;
        private Timer m_InspireFortitudeTimer;
        private Timer m_ExpeditiousRetreatTimer;
        
        public Timer ChangeStanceTimer{ get{ return m_ChangeStanceTimer; } set{ m_ChangeStanceTimer = value; } }
        public Timer InspireHeroicsTimer{ get{ return m_InspireHeroicsTimer; } set{ m_InspireHeroicsTimer = value; } }
        public Timer InspireResilienceTimer{ get{ return m_InspireResilienceTimer; } set{ m_InspireResilienceTimer = value; } }
        public Timer InspireFortitudeTimer{ get{ return m_InspireFortitudeTimer; } set{ m_InspireFortitudeTimer = value; } }
        public Timer ExpeditiousRetreatTimer{ get{ return m_ExpeditiousRetreatTimer; } set{ m_ExpeditiousRetreatTimer = value; } }
        
        private bool m_IsVampire;      
        private int m_BPs;
        private int m_MaxBPs;
        private bool m_AutoVampHeal;
        private DateTime m_NextFeedingAllowed;
        private DateTime m_NextVampHealAllowed;
        private int m_YearOfDeath;
        private int m_MonthOfDeath;
        private int m_DayOfDeath;
        private DateTime m_LastTimeGhouled;
        private bool m_VampSafety;
        private bool m_FreeForRP = true;
        private bool m_VampSight;
        private int m_ReforgesLeft = 1;
        private bool m_Reforging;
        private Point3D m_ReforgeLocation;
        private Map m_ReforgeMap;
        private ChosenDeity m_ChosenDeity;
        private bool m_Forging;
        public DateTime NextAllowedFixMe;
        
        private int chargeCooldown;
        private DateTime lastCharge;
        private DateTime cookingXpLastAwardedOn;
        private DateTime lastSecondWind;
        private int numberOfItemsCookedRecently;
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int ReforgesLeft{ get{ return m_ReforgesLeft; } set{ m_ReforgesLeft = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public ChosenDeity ChosenDeity{ get{ return m_ChosenDeity; } set{ m_ChosenDeity = value; } }
        
        public bool Reforging{ get{ return m_Reforging; } set{ m_Reforging = value; } }
        public bool Forging{ get{ return m_Forging; } set{ m_Forging = value; } }
        public Point3D ReforgeLocation{ get{ return m_ReforgeLocation; } set{ m_ReforgeLocation = value; } }
        public Map ReforgeMap{ get{ return m_ReforgeMap; } set{ m_ReforgeMap = value; } }

        private bool m_EnableOffHand;
        public bool EnableOffHand { get { return m_EnableOffHand; } set { m_EnableOffHand = value; } }

        private bool m_Deserialized;
		public bool Deserialized{ get{ return m_Deserialized; } set{ m_Deserialized = value; } }

        private DisguiseCollection m_MyDisguises = new DisguiseCollection();
        public DisguiseCollection MyDisguises { get { return m_MyDisguises; } set { m_MyDisguises = value; } }

        private DisguiseContext m_Disguise = new DisguiseContext();
        public DisguiseContext Disguise { get { return m_Disguise; } set { m_Disguise = value; } }
        
        public class CommandTimer : Timer
        {
            private PlayerMobile m;
            private int song;

            public CommandTimer( PlayerMobile from, double featlevel, int option )
                : base( TimeSpan.FromSeconds(featlevel) )
            {
                m = from;
                song = option;
            }

            protected override void OnTick()
            {
            	if( m == null || m.Deleted )
            		return;
            	
            	if( song == 1 )
            		m.InspireHeroics();
            	
            	else if( song == 2 )
            		m.InspireResilience();
            	
            	else if( song == 3 )
            		m.InspireFortitude();
            	
            	else
            		m.ExpeditiousRetreat();
            }
        }
        
        [CommandProperty( AccessLevel.Owner )]
        public bool VampSight{ get{ return m_VampSight; } set{ m_VampSight = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool FreeForRP{ get{ return m_FreeForRP; } set{ m_FreeForRP = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextFeedingAllowed{ get{ return m_NextFeedingAllowed; } set{ m_NextFeedingAllowed = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastTimeGhouled{ get{ return m_LastTimeGhouled; } set{ m_LastTimeGhouled = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextVampHealAllowed{ get{ return m_NextVampHealAllowed; } set{ m_NextVampHealAllowed = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool PureDodge{ get{ return m_PureDodge; } set{ m_PureDodge = value; } }
        
        [CommandProperty( AccessLevel.Administrator )]
        public bool IsVampire{ get{ return m_IsVampire; } set{ m_IsVampire = value; } }
        
        public bool VampSafety{ get{ return m_VampSafety; } set{ m_VampSafety = value; } }
        
        [CommandProperty( AccessLevel.Administrator )]
        public int MaxBPs{ get{ return m_MaxBPs; } set{ m_MaxBPs = Math.Max( 0, value ); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NumberOfItemsCookedRecently
        {
            get
            {
                if (this.numberOfItemsCookedRecently == null)
                    this.numberOfItemsCookedRecently = 0;
                return this.numberOfItemsCookedRecently;
            }
            set
            {
                this.numberOfItemsCookedRecently = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime CookingXpLastAwardedOn
        {
            get
            {
                if (this.cookingXpLastAwardedOn == null)
                    this.cookingXpLastAwardedOn = DateTime.Now.AddMinutes(10);
                return this.cookingXpLastAwardedOn;
            }
            set
            {
                this.cookingXpLastAwardedOn = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public DateTime LastCharge
        {
            get
            {
                if (this.lastCharge == null)
                    this.lastCharge = DateTime.Now.AddSeconds(-this.ChargeCooldown);
                return this.lastCharge;
            }
            set
            {
                this.lastCharge = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public DateTime LastSecondWind
        {
            get
            {
                if (this.lastSecondWind == null)
                    this.lastSecondWind = DateTime.Now;
                return this.lastSecondWind;
            }
            set
            {
                this.lastSecondWind = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public int ChargeCooldown
        {
            get
            {
                return this.chargeCooldown;
            }
            set
            {
                this.chargeCooldown = value;
            }
        }

        public bool IsChargeCooldownOver()
        {
            return (this.LastCharge.AddSeconds(this.ChargeCooldown) < DateTime.Now);
        }

        [CommandProperty( AccessLevel.Owner )]
        public int BPs
        {
        	get{ return m_BPs; }
        	set
        	{
        		if( m_BPs > value && value < 5 )
        			SendMessage( "You are running dangerously low on blood." );
        		
        		m_BPs = Math.Max( 0, Math.Min(value, m_MaxBPs) );
        		
        		if( m_BPs == 0 )
        			HueMod = 0;
        		
        		else if( HueMod == 0 )
        			HueMod = -1;
        	}
        }
        
        [CommandProperty( AccessLevel.Owner )]
        public bool AutoVampHeal{ get{ return m_AutoVampHeal; } set{ m_AutoVampHeal = value; } }
        
        public bool CanVampHeal
        {
        	get
        	{
        		if( DateTime.Compare(DateTime.Now, m_NextVampHealAllowed) > 0 )
        		{
        			if( VampSafety )
        				SendMessage( "You cannot heal while your Vampiric Powers Safety Lock is active." );
        			
        			else if( BPs > 0 )
        				return true;
        			
        			else
        				SendMessage( "Not enough blood points to heal." );
        			
        			return false;
        		}
        		
        		SendMessage( "It's too soon to use your blood to heal again." );
        		return false;
        	}
        }
        
        public bool CanFeed
        {
        	get
        	{
        		if( VampSafety )
    				SendMessage( "You cannot feed while your Vampiric Powers Safety Lock is active." );
        		
        		else if( DateTime.Compare(DateTime.Now, m_NextFeedingAllowed) > 0 )
        			return true;
        		
        		else
        			SendMessage( "It's too soon to feed again." );
        		
        		return false;
        	}
        }
        
        [CommandProperty( AccessLevel.Owner )]
        public int YearOfDeath{ get{ return m_YearOfDeath; } set{ m_YearOfDeath = value; } }
        
        [CommandProperty( AccessLevel.Owner )]
        public int MonthOfDeath{ get{ return m_MonthOfDeath; } set{ m_MonthOfDeath = value; } }
        
        [CommandProperty( AccessLevel.Owner )]
        public int DayOfDeath{ get{ return m_DayOfDeath; } set{ m_DayOfDeath = value; } }
        
        private List<Item> m_Tents = new List<Item>();
        
        [CommandProperty( AccessLevel.GameMaster )]
        public List<Item> Tents
        { 
        	get
        	{
        		if( m_Tents == null )
        			m_Tents = new List<Item>();
        		
        		else
        		{
        			for( int i = 0; i < m_Tents.Count; i++ )
        			{
        				Item item = m_Tents[i];

        				if( item == null || item.Deleted )
        					m_Tents.RemoveAt( i );
        			}
        		}
        		
        		return m_Tents;
        	}
        	set{ m_Tents = value; } 
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool RemoveTents
        {
        	get{ return false; }
        	set
        	{
        		if( value == true )
        		{
        			for( int i = 0; i < m_Tents.Count; i++ )
        			{
        				Item item = m_Tents[i];
        				m_Tents.Remove( item );
        					
        				if( item != null && !item.Deleted )
        					item.Delete();
        			}
        		}
        	}
        }
		
		public void ClearHands( bool forced )
		{
			ForcedClearHand( FindItemOnLayer( Layer.OneHanded ) );
			ForcedClearHand( FindItemOnLayer( Layer.TwoHanded ) );
		}

		public void ForcedClearHand( Item item )
		{
			if ( item != null && item.Movable )
			{
				Container pack = this.Backpack;

				if ( pack == null )
					AddToBackpack( item );
				else
					pack.DropItem( item );
			}
		}
        
        public bool CanPitchTent( Item tent )
        {
        	if( this.AccessLevel > AccessLevel.Player )
        		return true;
        	
        	if( Tents.Count > 0 )
        		return false;
        	
        	return true;
        }
        
        private bool m_LogMsgs;
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool LogMsgs{ get{ return m_LogMsgs; } set{ m_LogMsgs = value; } }

        private bool m_NeedsFixing;

        [CommandProperty( AccessLevel.Owner )]
        public bool NeedsFixing { get { return m_NeedsFixing; } set { m_NeedsFixing = value; } }

        public bool AwardedForCurrentCraft;

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Warmode
		{
			get
			{
				return base.Warmode;
			}
			set
			{
				if( value == true ) // removing song of retreat if we are going into war mode
				{
					ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlSpeedHack ) );
					for( int i = 0; i < list.Count; ++i )
					{
						XmlSpeedHack speed = list[i] as XmlSpeedHack;
						speed.Delete();
					}						
				}
				bool temp = Warmode;
				if ( temp != value && !value ) // we're trying to leave warmode
				{
					if ( CombatSystemAttachment.GetCSA( this ).PerformingSequence ) // can't leave if doing something uninterruptible
						return;
				}
				base.Warmode = value;
				if ( temp != value ) // no change
				{
					if ( Warmode )
						CombatSystemAttachment.GetCSA( this ).OnEnteredWarMode();
					else
					{
						Combatant = null;
						CombatSystemAttachment.GetCSA( this ).OnLeftWarMode();
					}
				}
			}
		}
		
		private string m_WikiConfig;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string WikiConfig{ get{ return m_WikiConfig; } set{ m_WikiConfig = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool LoadWikiConfig
		{
			get{ return false; }
			set
			{
				if( value == true )
					BaseCreature.DoLoadWikiConfig( this, WikiConfig );
			}
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

        [CommandProperty( AccessLevel.GameMaster )]
        public override string Name
        {
            get
            {
                if( !String.IsNullOrEmpty( Disguise.Name ) )
                    return Disguise.Name;

                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        [Hue, CommandProperty( AccessLevel.GameMaster )]
        public override int HueMod
        {
            get
            {
                return base.HueMod;
            }
            set
            {
                if( base.HueMod != value )
                {
                    if( IsVampire && BPs < 1 && value < 0 )
                        base.HueMod = 0;

                    else
                        base.HueMod = value;

                    Delta( MobileDelta.Hue );
                }
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public override int Hue
        {
            get
            {
                if( HueMod > -1 )
                    return HueMod;

                if( Disguise.Hue > -1 )
                    return Disguise.Hue;

                return base.Hue;
            }
            set
            {
                base.Hue = value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public override int HairItemID
        {
            get
            {
                if( Disguise.HairItemID > -1 )
                    return Disguise.HairItemID;

                return base.HairItemID;
            }
            set
            {
                base.HairItemID = value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public override int FacialHairItemID
        {
            get
            {
                if( Disguise.FacialHairItemID > -1 )
                    return Disguise.FacialHairItemID;

                return base.FacialHairItemID;
            }
            set
            {
                base.FacialHairItemID = value;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public override int HairHue
        {
            get
            {
                if( HairHueMod > -1 )
                    return HairHueMod;

                if( Disguise.HairHue > -1 )
                    return Disguise.HairHue;

                return base.HairHue;
            }
            set
            {
                base.HairHue = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int FacialHairHue
        {
            get
            {
                if( FacialHairHueMod > -1 )
                    return FacialHairHueMod;

                if( Disguise.FacialHairHue > -1 )
                    return Disguise.FacialHairHue;

                return base.FacialHairHue;
            }
            set
            {
                base.FacialHairHue = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastDeath { get { return m_LastDeath; } set { m_LastDeath = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int AvatarID
        {
            get { return m_AvatarID; }
            set { m_AvatarID = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public bool IsApprentice
		{ 
			get{ return m_IsApprentice; } 
			set{ m_IsApprentice = value; } 
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public GroupInfo Group
        {
            get
            {
                if (m_Group == null)
                    m_Group = new GroupInfo(this);
                return m_Group;
            }
            set
            {
                m_Group = value;
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int CustomAvatarID1
        {
            get { return m_CustomAvatarID1; }
            set { m_CustomAvatarID1 = value; }
        }		
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int CustomAvatarID2
        {
            get { return m_CustomAvatarID2; }
            set { m_CustomAvatarID2 = value; }
        }		

		[CommandProperty( AccessLevel.GameMaster )]
        public int CustomAvatarID3
        {
            get { return m_CustomAvatarID3; }
            set { m_CustomAvatarID3 = value; }
        }				
	
		[CommandProperty( AccessLevel.GameMaster )]
        public bool IsHardcore
		{ 
			get{ return m_IsHardcore; } 
			set{ m_IsHardcore = value; } 
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int Maimings
		{ 
			get{ return m_Maimings; } 
			set{ m_Maimings = value; } 
		}
		
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime HCWound { get { return m_HCWound; } set { m_HCWound = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanBeFaithful { get { return m_CanBeFaithful; } set { m_CanBeFaithful = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ConsecratedItems { get { return m_ConsecratedItems; } set { m_ConsecratedItems = value; } }

		public override void OnMovement( Mobile m, Point3D oldLocation ) // WE are not moving, someone else is
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
		}
		
		public override void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			Animate( action, frameCount, repeatCount, forward, repeat, delay, true );
		}
		public void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay, bool external )
		{
			if ( external && CombatSystemAttachment.GetCSA( this ).PerformingSequence ) // no animations if we're perfoming a sequence
				return;
			base.Animate( action, frameCount, repeatCount, forward, repeat, delay );
			if ( external )
				CombatSystemAttachment.GetCSA( this ).OnExternalAnimation();
		}
		
		public void CSAAnimate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			Animate( action, frameCount, repeatCount, forward, repeat, delay, false );
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			CombatSystemAttachment.GetCSA( this ).OnMoved( oldLocation );

            if (HealthAttachment.HasHealthAttachment(this))
            {
                HealthAttachment.GetHA(this).DoInjury(Injury.MinorConcussion);
                HealthAttachment.GetHA(this).DoInjury(Injury.Exhausted);
                HealthAttachment.GetHA(this).DoInjury(Injury.MajorConcussion);
                HealthAttachment.GetHA(this).DoInjury(Injury.FracturedLeftLeg);
                HealthAttachment.GetHA(this).DoInjury(Injury.FracturedRightLeg);
            }
		}
		
		/// <summary>
		/// Overridable. Virtual event invoked after the <see cref="Combatant" /> property has changed.
		/// <seealso cref="Combatant" />
		/// </summary>
		public override void OnCombatantChange()
		{
			CombatSystemAttachment.GetCSA( this ).OnChangedCombatant();
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
        public Container CraftContainer{ get{ return m_CraftContainer; } set{ m_CraftContainer = value; } }
        
        public CustomSpellBook SpellBook
        {
        	get
        	{
        		if( m_SpellBook != null && !m_SpellBook.IsChildOf(this.Backpack) )
        			m_SpellBook = null;
        		
        		return m_SpellBook; 
        	} 
        	
        	set{ m_SpellBook = value; } }
        
        public DateTime NextRage { get { return m_NextRage; } set { m_NextRage = value; } }
        public bool GemHarvesting { get { return m_GemHarvesting; } set { m_GemHarvesting = value; } }
        
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
			return (DateTime.Compare( DateTime.Now, this.NextSummoningAllowed ) > 0 && this.Followers < this.FollowersMax );
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
		public Timer BloodOfXorgoth { get { return m_BloodOfXorgoth; } set { m_BloodOfXorgoth = value; } }
		public Timer ManeuverBonusTimer { get { return m_ManeuverBonusTimer; } set { m_ManeuverBonusTimer = value; } }
		
		public int ManeuverDamageBonus{ get{ return m_ManeuverDamageBonus; } set{ m_ManeuverDamageBonus = value; } }
		public int ManeuverAccuracyBonus{ get{ return m_ManeuverAccuracyBonus; } set{ m_ManeuverAccuracyBonus = value; } }
        
        public bool CannotSpeak{ get{ return (m_MutenessTimer != null); } }
        
        public FeatList CurrentSpell{ get { return m_CurrentSpell; } set { m_CurrentSpell = value; } }
        public Mobile ShieldedMobile{ get { return m_ShieldedMobile; } set { m_ShieldedMobile = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
       	public bool BuggedCP
       	{
       		get
       		{
       			int max = 175000 + ExtraCPRewards + CPCapOffset;
						
				if( CPSpent > max )
					return true;
				
				return false;
       		}
       	}
		
        [CommandProperty( AccessLevel.GameMaster )]
        public int TotalPenalty{ get{ return ( m_LightPenalty + m_MediumPenalty + m_HeavyPenalty ); } }
        
        public double GetVampiricRegenBonus()
        {
    		double extra = (double)GetVampireTimeOffset( GetHour() );
			
			if( extra > 0 )
				return (extra * 2.5);
			
			return 0.0;
        }
        
        private bool m_SpeedHack;
        
        private SongList m_CurrentCommand;

        public int FakeHair{ get{ return m_FakeHair; } set{ m_FakeHair = value; } }
        public int FakeHairHue{ get{ return m_FakeHairHue; } set{ m_FakeHairHue = value; } }
        public int FakeFacialHair{ get{ return m_FakeFacialHair; } set{ m_FakeFacialHair = value; } }
        public int FakeFacialHairHue{ get{ return m_FakeFacialHairHue; } set{ m_FakeFacialHairHue = value; } }
        public int FakeHue{ get{ return m_FakeHue; } set{ m_FakeHue = value; } }
        public string FakeRPTitle{ get{ return m_FakeRPTitle; } set{ m_FakeRPTitle = value; } }
        public string FakeTitlePrefix{ get{ return m_FakeTitlePrefix; } set{ m_FakeTitlePrefix = value; } }
        
        public bool BreakLock{ get{ return m_SmoothPicking; } set{ m_SmoothPicking = value; } }
        public bool AutoPicking{ get{ return m_AutoPicking; } set{ m_AutoPicking = value; } }
        
        public string FakeAge{ get{ return m_FakeAge; } set{ m_FakeAge = value; } }
        public string FakeLooks{ get{ return m_FakeLooks; } set{ m_FakeLooks = value; } }
        
        public DateTime LogoutTime{ get{ return m_LogoutTime; } set{ m_LogoutTime = value; } }
        
        private int m_VisitDuration;
        public bool m_Visiting;
        private ArrayList m_Visited;
        
        public int VisitDuration{ get{ return m_VisitDuration; } set{ m_VisitDuration = value; } }
        public bool Visiting{ get{ return m_Visiting; } set{ m_Visiting = value; } }
        public bool VisitPending;
        private BaseCombatManeuver m_CombatManeuver;
        private BaseStance m_Stance;
        
        public BaseStance Stance
		{
			get
			{
				if( m_Stance == null )
				{
					m_Stance = new BaseStance();
				}
				
				return m_Stance;
			}
			
			set
			{
				m_Stance = value;
				RemoveBuff( BuffIcon.Polymorph );
				if ( value != null && value.GetType() != typeof( BaseStance ) )
				{ // i don't think there's enough space to list all the bonuses? name will have to do for now.
					string msg = "<CENTER>Active Stance:\t<BR>" + value.Name;
					this.AddBuff( new BuffInfo(
					BuffIcon.Polymorph, 1041600, 1060847, msg, false
					) );
				}
			}
		}
        
        public void ChangeManeuver( BaseCombatManeuver maneuver, FeatList feat, string message )
        {
        	if( this.ManeuverBonusTimer != null )
        	{
        		this.SendMessage( 60, "You are still under the effect of a previously used maneuver." );
        		return;
        	}
        	
        	if( maneuver.GetType() == this.CombatManeuver.GetType() )
        	{
        		this.SendMessage( 60, "You hold back on the maneuver you had prepared." );
        		DisableManeuver();
        		return;
        	}
        	
        	this.SendMessage( 60, message );
        	this.CombatManeuver = maneuver;
        	this.OffensiveFeat = feat;
        	this.ManeuverAccuracyBonus = this.CombatManeuver.AccuracyBonus * this.CombatManeuver.FeatLevel;
        	this.ManeuverDamageBonus = this.CombatManeuver.DamageBonus * this.CombatManeuver.FeatLevel;
        	this.Send( new MobileStatus( this, this ) );
        }
        
        public virtual void DisableManeuver()
        {
			BaseCombatManeuver oldManeuver = this.CombatManeuver;
        	this.CombatManeuver = null;
        	this.OffensiveFeat = FeatList.None;
        	
        	if( this.ManeuverBonusTimer == null )
        	{
	        	this.ManeuverAccuracyBonus = 0;
	        	this.ManeuverDamageBonus = 0;
        	}
        	
        	if( oldManeuver.GetType() != this.CombatManeuver.GetType() )
        		this.Send( new MobileStatus( this, this ) );
        }
        
        public void ChangeStance( BaseStance stance )
		{
        	if( !CanUseMartialStance && !stance.Armour )
        	{
				this.SendMessage( 60, "You can only use this stance while on foot, with your fists and without any armour penalties." );
				return;
        	}
        	
        	if( this.ChangeStanceTimer != null )
        		this.ChangeStanceTimer.Stop();
        	
        	this.ChangeStanceTimer = new StanceTimer( this, stance );
        	this.ChangeStanceTimer.Start();
        	this.SendMessage( "You start changing your stance." );
		}
        
        public void FinishChangeStance( BaseStance stance )
		{
			if( this.Stance.GetType() == stance.GetType() )
			{
				this.Emote( this.Stance.TurnedOffEmote );
				this.Stance = null;
			}
			
			else if( !CanUseMartialStance && !stance.Armour )
				this.SendMessage( 60, "You can only use this stance while on foot and without any armour penalties." );
			
			else
			{
				this.Stance = stance;
				this.Emote( this.Stance.TurnedOnEmote );
			}
			
			this.Send( new MobileStatus( this, this ) );
		}
        
        public class StanceTimer : Timer
        {
            private PlayerMobile m;
            private BaseStance newStance;

            public StanceTimer( PlayerMobile from, BaseStance stance ) : base( TimeSpan.FromSeconds(5) )
            {
            	m = from;
            	newStance = stance;
            }

            protected override void OnTick()
            {
            	if( m != null )
            		m.FinishChangeStance( newStance );
            }
        }
        
        public bool CanDodge
        {
        	get
        	{
                if( Paralyzed || TrippedTimer != null || HeavyPenalty > 0 )
        			return false;

                if( LightPieces < 1 )
                    return true;

        		return (Feats.GetFeatLevel(FeatList.ArmouredDodge) * 3) > TotalPenalty;
        	}
        }
        
        public int RideBonus{ get{ return 0; } }

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
        
        public ArrayList Visited
        {
        	get
        	{
        		if( m_Visited == null )
        			m_Visited = new ArrayList();
        		
        		return m_Visited;
        	}
        	set{ m_Visited = value; } 
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool Crafting{ get{ return m_Crafting; } set{ m_Crafting = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public SongList CurrentCommand
        { 
        	get{ return m_CurrentCommand; } 
        	set
        	{
        		if( value == SongList.None )
        		{
        			if( this.InspireHeroicsTimer != null )
        				this.InspireHeroicsTimer.Stop();
        			
        			if( this.InspireResilienceTimer != null )
        				this.InspireResilienceTimer.Stop();
        			
        			if( this.InspireFortitudeTimer != null )
        				this.InspireFortitudeTimer.Stop();
        			
        			if( this.ExpeditiousRetreatTimer != null )
        				this.ExpeditiousRetreatTimer.Stop();
        		}
        	}
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool RessSick
        {
        	get{ return IsTired(); }
        	set
        	{
        		if( value == false && IsTired() )
        		{
	            	XmlAttachment hitsatt = XmlAttach.FindAttachment( this, typeof( XmlHits ) );
	            	XmlAttachment stamatt = XmlAttach.FindAttachment( this, typeof( XmlStam ) );
	            	XmlAttachment manaatt = XmlAttach.FindAttachment( this, typeof( XmlMana ) );

	            	if( hitsatt != null )
	            		hitsatt.Delete();
	            	if( stamatt != null )
	            		stamatt.Delete();
	            	if( manaatt != null )
	            		manaatt.Delete();

	            	this.RemoveStatMod( "XmlHits" );
	            	this.RemoveStatMod( "XmlStam" );
	            	this.RemoveStatMod( "XmlMana" );
	            	
	            	if( this.m_KOPenalty != null )
	            	{
	            		this.m_KOPenalty.Stop();
	            		this.m_KOPenalty = null;
	            	}
        		}
        	}
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
		public int Fortitude
		{	
			get 
			{
				XmlFortitude fort = XmlAttach.FindAttachment( this, typeof( XmlFortitude ) ) as XmlFortitude;
	            
	            if( fort != null )
	            	return fort.Value;
	            
				return 0; 
			}
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int Heroes
        {
            get
            {
                XmlHeroes heroes = XmlAttach.FindAttachment(this, typeof(XmlHeroes)) as XmlHeroes;

                if (heroes != null)
                    return heroes.Value;

                return 0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Martyrs
        {
            get
            {
                XmlMartyrs martyrs = XmlAttach.FindAttachment(this, typeof(XmlMartyrs)) as XmlMartyrs;

                if (martyrs != null)
                    return martyrs.Value;

                return 0;
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
		public bool SpeedHack
		{	
			get 
			{
				XmlSpeedHack speed = XmlAttach.FindAttachment( this, typeof( XmlSpeedHack ) ) as XmlSpeedHack;
	            
	            if( speed != null )
	            	return true;

                if( this.AccessLevel > AccessLevel.Player )
                    return m_SpeedHack;

				return false; 
			}
			
			set
			{
				if( this.AccessLevel > AccessLevel.Player )
					m_SpeedHack = value;
				
				else if( value == false )
				{
					ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlSpeedHack ) );

					for( int i = 0; i < list.Count; ++i )
					{
						XmlSpeedHack speed = list[i] as XmlSpeedHack;
						speed.Delete();
					}
				}
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
		
		public bool HasSpecializedWeaponSkill()
		{
			BaseWeapon atkWeapon = this.Weapon as BaseWeapon;
			int multiplier = CombatStyles.Swordsmanship + CombatStyles.MaceFighting + CombatStyles.Fencing + CombatStyles.Polearms + CombatStyles.ExoticWeaponry + CombatStyles.Axemanship + CombatStyles.Throwing;
			bool found = false;

	        if ( multiplier > 0 )
	        {
	            if( atkWeapon.Skill == SkillName.Swords && CombatStyles.Swordsmanship > 0 )
	                found = true;
	
	            else if( atkWeapon.Skill == SkillName.Macing && CombatStyles.MaceFighting > 0 )
	                found = true;
	
	            else if( atkWeapon.Skill == SkillName.Fencing && CombatStyles.Fencing > 0 )
	                found = true;
	
	            else if( atkWeapon.Skill == SkillName.Polearms && CombatStyles.Polearms > 0 )
	                found = true;
	
	            else if( atkWeapon.Skill == SkillName.ExoticWeaponry && CombatStyles.ExoticWeaponry > 0 )
	                found = true;
	
	            else if( atkWeapon.Skill == SkillName.Axemanship && CombatStyles.Axemanship > 0 )
	                found = true;
            }
	            
	        return found;
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasStash{ get{ return m_HasStash; } set{ m_HasStash = value; } }
        
        public override bool KeepsItemsOnDeath{ get { return false; } }
		
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
		
		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Poisoned
		{
			get
			{
				return ( XmlAttach.FindAttachment( this, typeof( PoisonAttachment ) ) != null );
			}
		}
        
        public bool FixedRun
        {
            get { return m_FixedRun; }
            set { m_FixedRun = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int LightPieces
        {
            get { return m_LightPieces; }
            set { m_LightPieces = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int MediumPieces
        {
            get { return m_MediumPieces; }
            set { m_MediumPieces = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int HeavyPieces
        {
            get { return m_HeavyPieces; }
            set { m_HeavyPieces = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int ArmourPieces
        {
            get { return m_ArmourPieces; }
            set { m_ArmourPieces = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int LightPenalty
        {
            get { return m_LightPenalty; }
            set { m_LightPenalty = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int MediumPenalty
        {
            get { return m_MediumPenalty; }
            set { m_MediumPenalty = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int HeavyPenalty
        {
            get { return m_HeavyPenalty; }
            set { m_HeavyPenalty = value; }
        }
        
        public bool FixedRage
        {
            get { return m_FixedRage; }
            set { m_FixedRage = value; }
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
        
        public bool FixedReflexes
        {
            get { return m_FixedReflexes; }
            set { m_FixedReflexes = value; }
        }
        
        public bool FixedStyles
        {
            get { return m_FixedStyles; }
            set { m_FixedStyles = value; }
        }
        
        public DateTime NextMending
        {
            get { return m_NextMending; }
            set { m_NextMending = value; }
        }
        
        public bool FixedStatPoints
        {
            get { return m_FixedStatPoints; }
            set { m_FixedStatPoints = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool XPFromCrafting
        {
            get { return m_XPFromCrafting; }
            set { m_XPFromCrafting = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool XPFromKilling
        {
            get { return m_XPFromKilling; }
            set { m_XPFromKilling = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool Spar
        {
            get { return m_Spar; }
            set { m_Spar = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HideStatus
        {
            get { return m_HideStatus; }
            set { m_HideStatus = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string DayOfBirth
        {
            get { return m_DayOfBirth; }
            set { m_DayOfBirth = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string MonthOfBirth
        {
            get { return m_MonthOfBirth; }
            set { m_MonthOfBirth = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string YearOfBirth
        {
            get { return m_YearOfBirth; }
            set { m_YearOfBirth = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool AlyrianGuard
        {
            get { return m_AlyrianGuard; }
            set { m_AlyrianGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool AzhuranGuard
        {
            get { return m_AzhuranGuard; }
            set { m_AzhuranGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool KhemetarGuard
        {
            get { return m_KhemetarGuard; }
            set { m_KhemetarGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool MhordulGuard
        {
            get { return m_MhordulGuard; }
            set { m_MhordulGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool TyreanGuard
        {
            get { return m_TyreanGuard; }
            set { m_TyreanGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool VhalurianGuard
        {
            get { return m_VhalurianGuard; }
            set { m_VhalurianGuard = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool ImperialGuard
        {
            get { return m_ImperialGuard; }
            set { m_ImperialGuard = value; }
        }
        
        public int HearAll
        {
            get { return m_HearAll; }
            set { m_HearAll = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int CPSpent
        {
            get { return m_CPSpent; }
            set { m_CPSpent = value; }
        }
        
        public DateTime NextBirthday
        {
            get { return m_NextBirthday; }
            set { m_NextBirthday = value; }
        }
        
        public ResourceController UniqueSpot
        {
            get { return m_UniqueSpot; }
            set { m_UniqueSpot = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastOffenseToNature
        {
            get { return m_LastOffenseToNature; }
            set { m_LastOffenseToNature = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public object LastSayEmote
        {
            get { return m_LastSayEmote; }
            set { m_LastSayEmote = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastDonationLife
        {
            get { return m_LastDonationLife; }
            set { m_LastDonationLife = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Lives
        {
            get { return m_Lives; }
            set 
            { 
            	m_Lives = value; 
            	
            	if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump(this) );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Age
        {
            get { return m_Age; }
            set 
            {
            	m_Age = value; 
            	
            	if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
            }
        }
        
        public int MaxAge
        {
            get { return m_MaxAge; }
            set { m_MaxAge = value; }
        }
        
        public List<Mobile> AllyList
		{
			get
			{
				if( m_AllyList == null )
					m_AllyList = new List<Mobile>();
				
				return m_AllyList;
			}
			set { m_AllyList = value; }
		}
        
        public List<Mobile> LoggedOutPets
		{
			get
			{
	            if( m_LoggedOutPets == null )
					m_LoggedOutPets = new List<Mobile>();
	            
				return m_LoggedOutPets;
			}
			set { m_LoggedOutPets = value; }
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextAllowance
        {
            get { return m_NextAllowance; }
            set { m_NextAllowance = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextFeatUse
        {
            get { return m_NextFeatUse; }
            set { m_NextFeatUse = value; }
        }

        public string BaseDescription1 { get { return m_Description; } }
        public string BaseDescription2 { get { return m_Description2; } }
        public string BaseDescription3 { get { return m_Description3; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public string Description
        {
            get 
            {
                if( !String.IsNullOrEmpty( Disguise.Description1 ) )
                    return Disguise.Description1;

                return m_Description; 
            }
            set { m_Description = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string Description2
        {
            get
            {
                if( !String.IsNullOrEmpty( Disguise.Description2 ) )
                    return Disguise.Description2;

                if( !String.IsNullOrEmpty( Disguise.Description1 ) )
                    return "";

                return m_Description2;
            }
            set { m_Description2 = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string Description3
        {
            get
            {
                if( !String.IsNullOrEmpty( Disguise.Description3 ) )
                    return Disguise.Description3;

                if( !String.IsNullOrEmpty( Disguise.Description1 ) )
                    return "";

                return m_Description3;
            }
            set { m_Description3 = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public KnownLanguage SpokenLanguage
        {
            get { return m_SpokenLanguage; }
            set { m_SpokenLanguage = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string RPTitle
        {
            get 
            {
                if( Disguise != null && !String.IsNullOrEmpty( Disguise.RPTitle ) )
                    return Disguise.RPTitle;

                List<CustomGuildStone> removeList = new List<CustomGuildStone>();

                foreach (KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in CustomGuilds)
                {
                    if (kvp.Value != null)
                    {
                        if (kvp.Value.ActiveTitle)
                        {
                            if (kvp.Value.RankInfo != null)
                            {
                                if (!String.IsNullOrEmpty(kvp.Value.RankInfo.Title))
                                {
                                    if (kvp.Value.RankInfo.Title.ToLower() != "none")
                                    {
                                        return kvp.Value.RankInfo.Title;
                                    }
                                }
                            }
                        }
                    }
                }

                return m_RPTitle; 
            }
            set { m_RPTitle = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string TitlePrefix
        {
            get
            {
                if( Disguise != null && !String.IsNullOrEmpty( Disguise.TitlePrefix ) )
                {
                    if (Disguise == null)
                        return null;

                    if (Disguise.TitlePrefix == null)
                        return null;

                    if( Disguise.TitlePrefix.ToLower() == "none" )
                        return null;

                    return Disguise.TitlePrefix;
                }

                foreach (KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in CustomGuilds)
                {
                    if (kvp.Value != null)
                    {
                        if (kvp.Value.ActiveTitle)
                        {
                            if (kvp.Value.RankInfo != null)
                            {
                                if(!String.IsNullOrEmpty(kvp.Value.RankInfo.Prefix))
                                {
                                    if( kvp.Value.RankInfo.Prefix.ToLower() != "none" )
                                    {
                                        return kvp.Value.RankInfo.Prefix;
                                    }
                                }
                            }
                        }
                    }
                }

                return m_TitlePrefix;
            }
            set { m_TitlePrefix = value; Delta( MobileDelta.Name ); InvalidateProperties(); }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int FocusedShot
        {
            get { return m_FocusedShot; }
            set { m_FocusedShot = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SwiftShot
        {
            get { return m_SwiftShot; }
            set { m_SwiftShot = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string WeaponSpecialization
        {
            get { return m_WeaponSpecialization; }
            set { m_WeaponSpecialization = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public string SecondSpecialization
        {
            get { return m_SecondSpecialization; }
            set { m_SecondSpecialization = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SearingBreath
        {
            get { return m_SearingBreath; }
            set { m_SearingBreath = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SwipingClaws
        {
            get { return m_SwipingClaws; }
            set { m_SwipingClaws = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int TempestuousSea
        {
            get { return m_TempestuousSea; }
            set { m_TempestuousSea = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SilentHowl
        {
            get { return m_SilentHowl; }
            set { m_SilentHowl = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ThunderingHooves
        {
            get { return m_ThunderingHooves; }
            set { m_ThunderingHooves = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int VenomousWay
        {
            get { return m_VenomousWay; }
            set { m_VenomousWay = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int RageHits
        {
            get { return m_RageHits; }
            set { m_RageHits = value; }
        }

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
        public Direction FormerDirection
        {
            get { return m_FormerDirection; }
            set { m_FormerDirection = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ChargeSteps
        {
            get { return m_ChargeSteps; }
            set { m_ChargeSteps = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int FlurryOfBlows
        {
            get { return m_FlurryOfBlows; }
            set { m_FlurryOfBlows = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int FocusedAttack
        {
            get { return m_FocusedAttack; }
            set { m_FocusedAttack = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile HuntingHound
        {
            get { return m_HuntingHound; }
            set { m_HuntingHound = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile EscortPrisoner
        {
            get { return m_EscortPrisoner; }
            set { m_EscortPrisoner = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
		public Backgrounds Backgrounds
		{ 
			get 
			{ 
				if( m_Backgrounds == null )
					m_Backgrounds = new Backgrounds(); 
				
				return m_Backgrounds; 
			} 
			set { } 
		}

        public int GetBackgroundLevel(BackgroundList b)
        {
            return Backgrounds.BackgroundDictionary[b].Level + XmlBackground.GetLevel(this, b);
        }

		public Feats Feats
		{ 
			get 
			{ 
				if( m_Feats == null )
					m_Feats = new Feats(); 
				
				return m_Feats; 
			} 
			set { } 
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Dictionary<CustomGuildStone, CustomGuildInfo> CustomGuilds
		{ 
			get 
			{ 
				if( m_CustomGuilds == null )
					m_CustomGuilds = new Dictionary<CustomGuildStone, CustomGuildInfo>();
				
				return m_CustomGuilds; 
			} 
			set { } 
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
		public RacialResources RacialResources
		{ 
			get 
			{ 
				if( m_RacialResources == null )
					m_RacialResources = new RacialResources(); 
				
				return m_RacialResources; 
			} 
			set { } 
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
		public Friendship Friendship
		{ 
			get 
			{ 
				if( m_Friendship == null )
					m_Friendship = new Friendship(); 
				
				return m_Friendship; 
			} 
			set { } 
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
		public Masterwork Masterwork
		{ 
			get 
			{ 
				if( m_Masterwork == null )
					m_Masterwork = new Masterwork(); 
				
				return m_Masterwork; 
			} 
			set { } 
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
		public KnownLanguages KnownLanguages
		{ 
			get 
			{ 
				if( m_KnownLanguages == null )
					m_KnownLanguages = new KnownLanguages(); 
				
				return m_KnownLanguages; 
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
		public Informants Informants
		{ 
			get 
			{ 
				if( m_Informants == null )
					m_Informants = new Informants(); 
				
				return m_Informants; 
			} 
			set { } 
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level
		{
			get{ return m_Level; }
			set{ m_Level = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int XP
		{
			get{ return m_XP; }
            set{ m_XP = value; LevelSystem.CheckLevel( this ); }
		}
		
		public int NextLevel
		{
			get{ return m_NextLevel; }
			set{ m_NextLevel = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int CP
		{
			get{ return m_CP; }
			set
			{ 
				m_CP = value; 
				
				if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int StatPoints
		{
			get{ return m_StatPoints; }
			set{ m_StatPoints = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int SkillPoints
		{
			get{ return m_SkillPoints; }
			set{ m_SkillPoints = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int FeatSlots
		{
			get{ return m_FeatSlots; }
			set{ m_FeatSlots = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int CPCapOffset
		{
			get{ return m_CPCapOffset; }
			set
			{ 
				m_CPCapOffset = value; 
				
				if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHits
		{
			get{ return m_MaxHits; }
			set{ m_MaxHits = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxStam
		{
			get{ return m_MaxStam; }
			set{ m_MaxStam = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxMana
		{
			get{ return m_MaxMana; }
			set{ m_MaxMana = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int RecreateXP
        {
            get { return m_RecreateXP; }
            set { m_RecreateXP = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int RecreateCP
        {
            get { return m_RecreateCP; }
            set { m_RecreateCP = value; }
        }

		private class CountAndTimeStamp
		{
			private int m_Count;
			private DateTime m_Stamp;

			public CountAndTimeStamp()
			{
			}

			public DateTime TimeStamp { get{ return m_Stamp; } }
			public int Count 
			{ 
				get { return m_Count; } 
				set	{ m_Count = value; m_Stamp = DateTime.Now; } 
			}
		}
		
		private BaseWeapon m_OffHandWeapon;
		public BaseWeapon OffHandWeapon{ get{ return m_OffHandWeapon; } set{ m_OffHandWeapon = value; } }

		public bool OffHandThrowing
		{
			get{ return ( OffHandWeapon != null ); }
		}
		
		private DesignContext m_DesignContext;

        private FeatList m_SpecialAttack;
        private FeatList m_OffensiveFeat;
        private FeatList m_FightingStance;
        private Subclass m_Subclass;
        private Advanced m_Advanced;
        private bool m_Fizzled;
		private bool m_CanBeMage;
        private bool m_CleaveAttack;
        private bool m_Crippled;
        private bool m_BackToBack;
        private bool m_CanBeReplaced;
        private bool m_FreeToUse;
        private bool m_HasHuntingHoundBonus;
        private bool m_Trample;
        private int m_Intimidated;
		private Class m_Class;
		private Nation m_Nation;
		private NpcGuild m_NpcGuild;
		private DateTime m_NpcGuildJoinTime;
		private TimeSpan m_NpcGuildGameTime;
		private PlayerFlag m_Flags;
		private int m_StepsTaken;
		private int m_Profession;
        private DateTime m_LastChargeStep;
        private int m_Height;
        private int m_Weight;
        private int m_HarvestedCrops;
        private DateTime m_NextHarvestAllowed;
        public Timer HaloTimer;
        public DateTime m_NextTeachingAllowed;
        public List<PlayerMobile> m_Students;
        private DateTime m_LastTeaching;
        private string m_Technique;
        private int m_TechniqueLevel;
        public bool m_WantsTeaching;
        public bool m_Teaching;
        
        private DateTime m_LastCottonFlaxHarvest;
        public DateTime LastCottonFlaxHarvest{ get{ return m_LastCottonFlaxHarvest; } set{ m_LastCottonFlaxHarvest = value; } }
        
        private CustomMageSpell m_TouchSpell;
        public CustomMageSpell TouchSpell{ get{ return m_TouchSpell; } set{ m_TouchSpell = value; } }
        
        private int m_ExtraCPRewards;
        [CommandProperty( AccessLevel.GameMaster )]
        public int ExtraCPRewards
        { 
        	get{ return m_ExtraCPRewards; } 
        	set
        	{
        		m_ExtraCPRewards = value;
        		
        		if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
        	}
        }
        
        public bool IsAllyOf( Mobile mob )
        {
        	return BaseAI.AreAllies( this, mob );
        }
        
        public bool CanBeAwarded
		{
			get
			{
				if( DateTime.Compare( DateTime.Now, ( m_LastTeaching + TimeSpan.FromMinutes( 30 ) ) ) > 0 )
					return true;
				
				return false;
			}
		}
        
        [CommandProperty( AccessLevel.GameMaster )]
		public int HarvestedCrops
		{
			get{ return m_HarvestedCrops; }
			set{ m_HarvestedCrops = value; }
		}
       
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextHarvestAllowed
		{
			get{ return m_NextHarvestAllowed; }
			set{ m_NextHarvestAllowed = value; }
		}

		private DateTime m_LastOnline;
		private Server.Guilds.RankDefinition m_GuildRank;

		private int m_GuildMessageHue, m_AllianceMessageHue;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Owner )]
		public new Account Account
		{
			get { return base.Account as Account; }
			set { base.Account = value; }
		}

		#region Getters & Setters
		
		public string Technique{ get{ return m_Technique; } set{ m_Technique = value; } }
		public int TechniqueLevel{ get{ return m_TechniqueLevel; } set{ m_TechniqueLevel = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Height
		{
			get{ return m_Height; }
			set
			{ 
				m_Height = value; 
				
				if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump(this) );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Weight
		{
			get{ return m_Weight; }
			set
			{ 
				m_Weight = value; 
				
				if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
			}
		}
		
		
		public Server.Guilds.RankDefinition GuildRank
		{
			get
			{
				if( this.AccessLevel >= AccessLevel.GameMaster )
					return Server.Guilds.RankDefinition.Leader;
				else
					return m_GuildRank; 
			}
			set{ m_GuildRank = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int GuildMessageHue
		{
			get{ return m_GuildMessageHue; }
			set{ m_GuildMessageHue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int AllianceMessageHue
		{
			get { return m_AllianceMessageHue; }
			set { m_AllianceMessageHue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Profession
		{
			get{ return m_Profession; }
			set{ m_Profession = value; }
		}

		public int StepsTaken
		{
			get{ return m_StepsTaken; }
			set{ m_StepsTaken = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public FeatList SpecialAttack
        {
            get { return m_SpecialAttack; }
            set { m_SpecialAttack = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public FeatList OffensiveFeat
        {
            get { return m_OffensiveFeat; }
            set { m_OffensiveFeat = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public FeatList FightingStance
        {
            get { return m_FightingStance; }
            set { m_FightingStance = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Subclass Subclass
        {
            get { return m_Subclass; }
            set 
            { 
            	m_Subclass = value; 
            	
            	if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
            }
        }

        [CommandProperty(AccessLevel.Owner)]
        public Advanced Advanced
        {
            get { return m_Advanced; }
            set 
            { 
            	m_Advanced = value; 
            	
            	if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
            }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanBeMage
		{
			get{ return m_CanBeMage; }
			set{ m_CanBeMage = value; }
		}

        [CommandProperty( AccessLevel.GameMaster )]
        public bool CanRun
        {
            get
            {
            	if( GetBackgroundLevel(BackgroundList.Lame) > 0 )
            		return false;
            	
            	if( this.m_CrippledTimer != null || this.m_DisabledLegsTimer != null || this.Region is StickyGooRegion )
            		return false;

                if (HealthAttachment.HasHealthAttachment(this))
                {
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenLeftLeg))
                        return false;
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenRightLeg))
                        return false;
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.FracturedRightLeg))
                    {
                        if (Utility.RandomBool())
                            return false;
                    }
                    if (HealthAttachment.GetHA(this).HasInjury(Injury.FracturedLeftLeg))
                    {
                        if (Utility.RandomBool())
                            return false;
                    }
                }

            	return (((this.TotalPenalty - this.ArmourPieces ) + this.LightPieces) < 17);
            }
        }

        public bool Fizzled{ get{ return m_Fizzled; } set{ m_Fizzled = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool CleaveAttack
        {
            get { return m_CleaveAttack; }
            set { m_CleaveAttack = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Crippled
        {
            get { return m_Crippled; }
            set { m_Crippled = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool BackToBack
        {
            get { return m_BackToBack; }
            set { m_BackToBack = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool RopeTrick
        {
            get
            {
            	if( this.Backpack != null && !this.Backpack.Deleted )
            		return Rope.CheckContainerForRope( this.Backpack );
            	
            	return false;
            }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasHuntingHound{ get{ return false; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasHuntingHoundBonus
        {
            get { return m_HasHuntingHoundBonus; }
            set { m_HasHuntingHoundBonus = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Trample
        {
            get { return m_Trample; }
            set { m_Trample = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Intimidated
        {
            get { return m_Intimidated; }
            set { m_Intimidated = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public Class Class
		{
			get{ return m_Class; }
			set{ m_Class = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Nation Nation
		{
			get{ return m_Nation; }
			set
			{ 
				m_Nation = value; 
				
				if( this.HasGump( typeof( CharInfoGump ) ) )
					this.SendGump( new CharInfoGump( this ) );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public NpcGuild NpcGuild
		{
			get{ return m_NpcGuild; }
			set{ m_NpcGuild = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NpcGuildJoinTime
		{
			get{ return m_NpcGuildJoinTime; }
			set{ m_NpcGuildJoinTime = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastOnline
		{
			get{ return m_LastOnline; }
			set{ m_LastOnline = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NpcGuildGameTime
		{
			get{ return m_NpcGuildGameTime; }
			set{ m_NpcGuildGameTime = value; }
		}

		private int m_ToTItemsTurnedIn;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ToTItemsTurnedIn
		{
			get { return m_ToTItemsTurnedIn; }
			set { m_ToTItemsTurnedIn = value; }
		}

		private int m_ToTTotalMonsterFame;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ToTTotalMonsterFame
		{
			get { return m_ToTTotalMonsterFame; }
			set { m_ToTTotalMonsterFame = value; }
		}

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastChargeStep
        {
            get { return m_LastChargeStep; }
            set { m_LastChargeStep = value; }
        }
       
        [CommandProperty( AccessLevel.GameMaster )]
		public bool EnemyOfNature
		{
			get
			{ 
				if( DateTime.Compare( ( this.LastOffenseToNature + TimeSpan.FromDays( 10 ) ) , DateTime.Now ) < 0 )
					return false;
				
				else
					return true;
			}
			
			set
			{
				if( value == true )
					this.LastOffenseToNature = DateTime.Now;
				
				else
					this.LastOffenseToNature = DateTime.MinValue;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public virtual bool RemoveEquip
        {
            get{ return false; }
            
            set
            {
            	if( value == true && this.Items != null )
            		BaseCreature.RemoveEquipFrom( this );
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public virtual Mobile CopyStatsAndGear
        {
        	get{ return null; }
        	set
        	{
        		if( this.AccessLevel > AccessLevel.Player )
        			BaseCreature.RemoveEquipFrom( this ); BaseCreature.CopyStatsAndGearFrom( value, this );
        	}
        }

		#endregion

		#region PlayerFlags
		public PlayerFlag Flags
		{
			get{ return m_Flags; }
			set{ m_Flags = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PagingSquelched
		{
			get{ return GetFlag( PlayerFlag.PagingSquelched ); }
			set{ SetFlag( PlayerFlag.PagingSquelched, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Glassblowing
		{
			get{ return GetFlag( PlayerFlag.Glassblowing ); }
			set{ SetFlag( PlayerFlag.Glassblowing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Masonry
		{
			get{ return GetFlag( PlayerFlag.Masonry ); }
			set{ SetFlag( PlayerFlag.Masonry, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool SandMining
		{
			get{ return GetFlag( PlayerFlag.SandMining ); }
			set{ SetFlag( PlayerFlag.SandMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StoneMining
		{
			get{ return GetFlag( PlayerFlag.StoneMining ); }
			set{ SetFlag( PlayerFlag.StoneMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ToggleMiningStone
		{
			get{ return GetFlag( PlayerFlag.ToggleMiningStone ); }
			set{ SetFlag( PlayerFlag.ToggleMiningStone, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool KarmaLocked
		{
			get{ return GetFlag( PlayerFlag.KarmaLocked ); }
			set{ SetFlag( PlayerFlag.KarmaLocked, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AutoRenewInsurance
		{
			get{ return GetFlag( PlayerFlag.AutoRenewInsurance ); }
			set{ SetFlag( PlayerFlag.AutoRenewInsurance, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UseOwnFilter
		{
			get{ return GetFlag( PlayerFlag.UseOwnFilter ); }
			set{ SetFlag( PlayerFlag.UseOwnFilter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PublicMyRunUO
		{
			get{ return GetFlag( PlayerFlag.PublicMyRunUO ); }
			set{ SetFlag( PlayerFlag.PublicMyRunUO, value ); InvalidateMyRunUO(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AcceptGuildInvites
		{
			get{ return GetFlag( PlayerFlag.AcceptGuildInvites ); }
			set{ SetFlag( PlayerFlag.AcceptGuildInvites, value ); }
		}
		#endregion

		public static Direction GetDirection4( Point3D from, Point3D to )
		{
			int dx = from.X - to.X;
			int dy = from.Y - to.Y;

			int rx = dx - dy;
			int ry = dx + dy;

			Direction ret;

			if ( rx >= 0 && ry >= 0 )
				ret = Direction.West;
			else if ( rx >= 0 && ry < 0 )
				ret = Direction.South;
			else if ( rx < 0 && ry < 0 )
				ret = Direction.East;
			else
				ret = Direction.North;

			return ret;
		}

		public override bool OnDroppedItemToWorld( Item item, Point3D location )
		{
			if ( !base.OnDroppedItemToWorld( item, location ) )
				return false;

			BounceInfo bi = item.GetBounce();

			if ( bi != null )
			{
				Type type = item.GetType();

				if ( type.IsDefined( typeof( FurnitureAttribute ), true ) || type.IsDefined( typeof( DynamicFlipingAttribute ), true ) )
				{
					object[] objs = type.GetCustomAttributes( typeof( FlipableAttribute ), true );

					if ( objs != null && objs.Length > 0 )
					{
						FlipableAttribute fp = objs[0] as FlipableAttribute;

						if ( fp != null )
						{
							int[] itemIDs = fp.ItemIDs;

							Point3D oldWorldLoc = bi.m_WorldLoc;
							Point3D newWorldLoc = location;

							if ( oldWorldLoc.X != newWorldLoc.X || oldWorldLoc.Y != newWorldLoc.Y )
							{
								Direction dir = GetDirection4( oldWorldLoc, newWorldLoc );

								if ( itemIDs.Length == 2 )
								{
									switch ( dir )
									{
										case Direction.North:
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East:
										case Direction.West: item.ItemID = itemIDs[1]; break;
									}
								}
								else if ( itemIDs.Length == 4 )
								{
									switch ( dir )
									{
										case Direction.South: item.ItemID = itemIDs[0]; break;
										case Direction.East: item.ItemID = itemIDs[1]; break;
										case Direction.North: item.ItemID = itemIDs[2]; break;
										case Direction.West: item.ItemID = itemIDs[3]; break;
									}
								}
							}
						}
					}
				}
			}

			return true;
		}

		public bool GetFlag( PlayerFlag flag )
		{
			return ( (m_Flags & flag) != 0 );
		}

		public void SetFlag( PlayerFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		public DesignContext DesignContext
		{
			get{ return m_DesignContext; }
			set{ m_DesignContext = value; }
		}

		public static void Initialize()
		{
			if ( FastwalkPrevention )
			{
				PacketHandler ph = PacketHandlers.GetHandler( 0x02 );

				ph.ThrottleCallback = new ThrottlePacketCallback( MovementThrottle_Callback );
			}

			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Logout += new LogoutEventHandler( OnLogout );
			EventSink.Connected += new ConnectedEventHandler( EventSink_Connected );
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
			//Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( UpdateGuildBonuses ) );  
		}

		public override void OnSkillInvalidated( Skill skill )
		{
			if ( Core.AOS && skill.SkillName == SkillName.MagicResist )
				UpdateResistances();
		}

		public override int GetMaxResistance( ResistanceType type )
		{
			int max = base.GetMaxResistance( type );

			if ( type != ResistanceType.Physical && 60 < max && Spells.Fourth.CurseSpell.UnderEffect( this ) )
				max = 60;

			if( Core.ML && this.Race == Race.Elf && type == ResistanceType.Energy )
				max += 5; //Intended to go after the 60 max from curse

			return max;
		}

		protected override void OnRaceChange( Race oldRace )
		{
			ValidateEquipment();
			UpdateResistances();
		}

		public override int MaxWeight 
        { 
            get 
            {
                double offset = 1.5;

                int lifting = this.Feats.GetFeatLevel(FeatList.HeavyLifting) * 50;

                offset = offset * this.Str;
                
                int totaloffset = 100 + Convert.ToInt32( offset ) + lifting;

                return totaloffset;
            } 
        }

		private int m_LastGlobalLight = -1, m_LastPersonalLight = -1;

		public override void OnNetStateChanged()
		{
			m_LastGlobalLight = -1;
			m_LastPersonalLight = -1;
		}

		public override void ComputeBaseLightLevels( out int global, out int personal )
		{
			global = LightCycle.ComputeLevelFor( this );

		// ** EDIT ** Time System
		
			/*if ( this.LightLevel < 21 && AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 )
				personal = 21;
			else
				personal = this.LightLevel;*/
		
			if (this.LightLevel < 21 && AosAttributes.GetValue(this, AosAttribute.NightSight) > 0)
			{
				int level = TimeSystem.EffectsEngine.GetNightSightLevel(this, 21);
		
				if (level > -1)
				{
					personal = level;
				}
				else
				{
					personal = 0;
				}
			}
			else
			{
				personal = this.LightLevel;
			}

		}

		public override void CheckLightLevels( bool forceResend )
		{
			NetState ns = this.NetState;

			if ( ns == null )
				return;

			int global, personal;

			ComputeLightLevels( out global, out personal );

			if ( !forceResend )
				forceResend = ( global != m_LastGlobalLight || personal != m_LastPersonalLight );

			if ( !forceResend )
				return;

			m_LastGlobalLight = global;
			m_LastPersonalLight = personal;

			ns.Send( GlobalLightLevel.Instantiate( global ) );
			ns.Send( new PersonalLightLevel( this, personal ) );
		}
		
		public override int BasePoisonResistance
		{
			get{ return GetBaseResistance( ResistanceType.Poison ); }
		}
		
		public override int BaseColdResistance
		{
			get{ return GetBaseResistance( ResistanceType.Cold ); }
		}
		
		public override int BaseFireResistance
		{
			get{ return GetBaseResistance( ResistanceType.Fire ); }
		}
		
		public override int BaseSlashingResistance
		{
			get{ return GetBaseResistance( ResistanceType.Slashing ); }
		}
		
		public override int BasePiercingResistance
		{
			get{ return GetBaseResistance( ResistanceType.Piercing ); }
		}
		
		public override int BaseBluntResistance
		{
			get{ return GetBaseResistance( ResistanceType.Blunt ); }
		}
		
		public override int BaseEnergyResistance
		{
			get{ return GetBaseResistance( ResistanceType.Energy ); }
		}
		
		public int GetPureResistance( ResistanceType type )
		{
			int bonus = GetBaseResistance( type );
            		
    		for( int i = 0; ResistanceMods != null && i < ResistanceMods.Count; ++i )
			{
				ResistanceMod mod = ResistanceMods[i];
				
				if( mod.Type == type )
					bonus += mod.Offset;
			}
    		
    		int min = GetMinResistance( type );
			int max = GetMaxResistance( type );

			if( max < min )
				max = min;

			if( bonus > max )
				bonus = max;
			
			else if( bonus < min )
				bonus = min;
    		
			return bonus;
		}
		
		[CommandProperty( AccessLevel.Counselor )]
        public override int BluntResistance
        {
            get
            {
            	if( PureDodge )
            		return GetPureResistance( ResistanceType.Blunt );
            	
        		return GetResistance( ResistanceType.Blunt ); 
            }
        }

        [CommandProperty( AccessLevel.Counselor )]
        public override int SlashingResistance
        {
            get
            {
            	if( PureDodge )
            		return GetPureResistance( ResistanceType.Slashing );

        		return GetResistance( ResistanceType.Slashing ); 
            }
        }

        [CommandProperty( AccessLevel.Counselor )]
        public override int PiercingResistance
        {
            get
            {
            	if( PureDodge )
            		return GetPureResistance( ResistanceType.Piercing );

        		return GetResistance( ResistanceType.Piercing ); 
            }
        }

		private int GetBaseResistance( ResistanceType type )
		{
			if( type == ResistanceType.Blunt || type == ResistanceType.Piercing || type == ResistanceType.Slashing )
			{
				int minvalue = 0;

                if( Dex > 100 )
                    minvalue += (int)( ( Dex - 100 ) * 0.2 ); //each 5 points of dex above 100 incs base armour by 1.
				
				if( this.TotalPenalty > 0 || this.LightPieces > 0 )
					return minvalue;
				
				if( this.Feats.GetFeatLevel(FeatList.EnhancedDodge) > 0 )
					minvalue += this.Feats.GetFeatLevel(FeatList.EnhancedDodge) * 1;
				
				if( PureDodge )
				{
					if( Feats.GetFeatLevel(FeatList.PureDodge) >= 3 )
						minvalue += 10;
					
					else if( Feats.GetFeatLevel(FeatList.PureDodge) == 2 )
						minvalue += 5;
					
					else if( Feats.GetFeatLevel(FeatList.PureDodge) == 1 )
						minvalue += 1;
				}

				minvalue += (int)((this.Skills[SkillName.Dodge].Base ) * 0.2 );
				
				if ( minvalue > MaxPlayerResistance )
					minvalue = MaxPlayerResistance;
				
				return minvalue;
			}
			
			if( type == ResistanceType.Energy )
			{
			int min = int.MinValue;
			        if( Int > 100 )
			               min += (int)( ( Int - 100 ) ); //each 1 points of int above 100 incs MR by 1.
				

				if ( min > MaxPlayerResistance )
					min = MaxPlayerResistance;
				
				return min;
			
			
			               
			
				/*int magicResist = (int)(Skills[SkillName.MagicResist].Base );
				int min = int.MinValue;
	
				if ( magicResist > 1 )
					min = (int)((magicResist * 70) * 0.01);
	
				if( min > MaxPlayerResistance )
					min = MaxPlayerResistance;
				
				if( min < 1 )
					min = 0;
				
				return min; */
			}
			
			if( type == ResistanceType.Poison )
			{
				int retval = 0;
				
				IKhaerosMobile km = this as IKhaerosMobile;
				if ( km == null )
					return retval;
				else
					return km.Feats.GetFeatLevel(FeatList.PoisonResistance)*20 + retval;
			}
			
			return 0;
		}
		
		public override bool IsBeneficialCriminal( Mobile target )
		{
			return false;
		}
		
		public override void OnBeneficialAction( Mobile target, bool isCriminal )
		{
		}

		public void ResetObjectProperties()
		{
            m_RacialResources = new RacialResources();
            m_Backgrounds = new Backgrounds();
            m_Feats = new Feats();
            m_Masterwork = new Masterwork();
            m_CombatStyles = new CombatStyles();

            if( !Reforging )
            {
                m_CustomGuilds = new Dictionary<CustomGuildStone, CustomGuildInfo>();
                m_AllyList = new List<Mobile>();
            }
		}
		
		public virtual void Visit()
		{
			if( this.VisitPending )
				this.VisitPending = false;
			
			if( !this.Visiting )
				return;
			
			foreach( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if( m != null && !m.Deleted && m != this && !this.Visited.Contains( m ) && m.AccessLevel < AccessLevel.GameMaster )
				{
					this.SendMessage( "Now visiting " + m.Name + "." );
					this.Map = m.Map;
					this.Location = m.Location;
					this.Visited.Add( m );
					Timer.DelayCall( TimeSpan.FromSeconds( this.VisitDuration ), new TimerCallback( Visit ) );
					return;
				}
			}
			
			this.SendMessage( "No more players to visit." );
			this.Visiting = false;
		}
		
		public virtual void RemoveShieldOfSacrifice()
		{
			this.ShieldingMobile = null;
			this.ShieldValue = 0.0;
            this.ShieldedMobile = null;
		}
		
		public virtual void SongOfMockery()
        {
            if (this.CanIssueCommand() && this.HasFeatLevel(this.Feats.GetFeatLevel(FeatList.SongOfMockery), 1) && this.HasEnoughStamina(4 - this.Feats.GetFeatLevel(FeatList.SongOfMockery)))
            {
            	if (GetBackgroundLevel(BackgroundList.Mute) < 1)
                    this.Emote("*sings a spiteful tune*");

                else
                    this.Emote("*plays a spiteful tune*");

                double time = 60 + (this.Feats.GetFeatLevel(FeatList.LingeringCommand) * 30);
                double combo = this.Feats.GetFeatLevel(FeatList.CombinedCommandsI) + this.Feats.GetFeatLevel(FeatList.CombinedCommandsII);
                double cooldown = time - ((10 * combo) + (this.Feats.GetFeatLevel(FeatList.LingeringCommand) * 5 * combo));
                this.m_NextSongAllowed = DateTime.Now + TimeSpan.FromSeconds( cooldown );

                foreach (Mobile mob in this.GetMobilesInRange((6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand)))))
                {
                	if (mob != null && !mob.Deleted )
                    {
                		if( this.AllyList.Contains(mob) || mob == this )
                			continue;
                		
                		int chance = Math.Max( ((this.Feats.GetFeatLevel(FeatList.SongOfMockery) * 30) + 60 - mob.RawInt), 20 );
                    	
                    	if( Utility.Random( 100 ) > chance )
                    		continue;
                    	
                    	if( mob.Combatant != null )
                    		mob.Combatant = this;
                    	
                        mob.SendMessage("You feel outraged by " + this.Name + "{0}" + " song.", this.Name.EndsWith("s") ? "'" : "'s");
                    }
                }

                Timer.DelayCall(TimeSpan.FromSeconds(time), new TimerCallback(SongOfMockery));
            }
        }
		
		public virtual void SongOfEnthrallment()
        {
            if (this.CanIssueCommand() && this.HasFeatLevel(this.Feats.GetFeatLevel(FeatList.SongOfEnthrallment), 1) && this.HasEnoughStamina(4 - this.Feats.GetFeatLevel(FeatList.SongOfEnthrallment)))
            {
                if (GetBackgroundLevel(BackgroundList.Mute) < 1)
                    this.Emote("*sings a soothing tune*");

                else
                    this.Emote("*plays a soothing tune*");

                double time = GetCommandDuration();

                foreach (Mobile mob in this.GetMobilesInRange((6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand)))))
                {
                    if (mob != null && !mob.Deleted )
                    {
                    	if( (this.AllyList.Contains(mob) || mob == this) || !( mob is IKhaerosMobile ) )
                    		continue;
                    	
                    	int chance = Math.Max( ((this.Feats.GetFeatLevel(FeatList.SongOfEnthrallment) * 30) + 60 - mob.RawInt), 20 );
                    	
                    	if( Utility.Random( 100 ) > chance )
                    		continue;
          
                        mob.SendMessage("You feel entranced by " + this.Name + "{0}" + " song.", this.Name.EndsWith("s") ? "'" : "'s");
                        XmlAttach.AttachTo(mob, new XmlEnthrallment(this.Feats.GetFeatLevel(FeatList.SongOfEnthrallment), time));
                    }
                }

                Timer.DelayCall(TimeSpan.FromSeconds(time), new TimerCallback(SongOfEnthrallment));
            }
        }

        public virtual void InspireResilience()
        {
            if (this.CanIssueCommand() && this.HasFeatLevel(this.Feats.GetFeatLevel(FeatList.InspireResilience), 1) && this.HasEnoughStamina(4 - this.Feats.GetFeatLevel(FeatList.InspireResilience)))
            {
                double time = GetCommandDuration();
                
                if( this.InspireResilienceTimer != null )
                	this.InspireResilienceTimer.Stop();
                
                this.InspireResilienceTimer = new CommandTimer( this, time, 2 );
                this.InspireResilienceTimer.Start();

                foreach (NetState ns in this.GetClientsInRange((6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand)))))
                {
                    if (ns.Mobile != null && !ns.Mobile.Deleted && (this.AllyList.Contains(ns.Mobile) || ns == this.NetState))
                    {
                    	ns.Mobile.SendMessage( "Inspire Resilience (from " + Name + ")." );
                        XmlAttach.AttachTo(ns.Mobile, new XmlMartyrs(this.Feats.GetFeatLevel(FeatList.InspireResilience), time));
                    }
                }
            }
        }

        public virtual void InspireHeroics()
        {
            if (this.CanIssueCommand() && this.HasFeatLevel(this.Feats.GetFeatLevel(FeatList.InspireHeroics), 1) && this.HasEnoughStamina(4 - this.Feats.GetFeatLevel(FeatList.InspireHeroics)))
            {
                double time = GetCommandDuration();
                
                if( this.InspireHeroicsTimer != null )
                	this.InspireHeroicsTimer.Stop();
                
                this.InspireHeroicsTimer = new CommandTimer( this, time, 1 );
                this.InspireHeroicsTimer.Start();

                foreach (NetState ns in this.GetClientsInRange((6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand)))))
                {
                    if (ns.Mobile != null && !ns.Mobile.Deleted && (this.AllyList.Contains(ns.Mobile) || ns == this.NetState))
                    {
                        ns.Mobile.SendMessage( "Inspire Heroics (from " + Name + ")." );
                        XmlAttach.AttachTo(ns.Mobile, new XmlHeroes(this.Feats.GetFeatLevel(FeatList.InspireHeroics), time));
                    }
                }
            }
        }
		
		public virtual void InspireFortitude()
		{
			if( this.CanIssueCommand() && this.HasFeatLevel( this.Feats.GetFeatLevel(FeatList.InspireFortitude), 1 ) && this.HasEnoughStamina( 4 - this.Feats.GetFeatLevel(FeatList.InspireFortitude) ) )
        	{
				double time = GetCommandDuration();
                
                if( this.InspireFortitudeTimer != null )
                	this.InspireFortitudeTimer.Stop();
                
                this.InspireFortitudeTimer = new CommandTimer( this, time, 3 );
                this.InspireFortitudeTimer.Start();

    			foreach( NetState ns in this.GetClientsInRange( (6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand))) ) )
				{
        			if( ns.Mobile != null && !ns.Mobile.Deleted && (this.AllyList.Contains(ns.Mobile) || ns == this.NetState) )
					{
						ns.Mobile.SendMessage( "Inspire Fortitude (from " + Name + ")." );
			            XmlAttach.AttachTo( ns.Mobile, new XmlFortitude( this.Feats.GetFeatLevel(FeatList.InspireFortitude), time ) );
					}
				}
        	}
		}
		
		public virtual void ExpeditiousRetreat()
		{
			if( this.CanIssueCommand() && this.HasFeatLevel( this.Feats.GetFeatLevel(FeatList.ExpeditiousRetreat), 1 ) && this.HasEnoughStamina( 4 - this.Feats.GetFeatLevel(FeatList.ExpeditiousRetreat) ) )
        	{
    			double time = GetCommandDuration();
                
                if( this.ExpeditiousRetreatTimer != null )
                	this.ExpeditiousRetreatTimer.Stop();
                
                this.ExpeditiousRetreatTimer = new CommandTimer( this, time, 4 );
                this.ExpeditiousRetreatTimer.Start();

    			foreach( NetState ns in this.GetClientsInRange( (6 + (2 * this.Feats.GetFeatLevel(FeatList.WidespreadCommand))) ) )
				{
        			if( ns.Mobile != null && !ns.Mobile.Deleted && (this.AllyList.Contains(ns.Mobile) || ns == this.NetState) && !ns.Mobile.Warmode )
					{
						ns.Mobile.SendMessage( "Expeditious Retreat (from " + Name + ")." );
			            XmlAttach.AttachTo( ns.Mobile, new XmlSpeedHack( 20, time ) );
					}
				}
        	}
		}
		
		public double GetCommandDuration()
		{
			double lingering = (this.Feats.GetFeatLevel(FeatList.LingeringCommand) * 10);
            double time = 30 + lingering;
            double combo = this.Feats.GetFeatLevel(FeatList.CombinedCommandsI) + this.Feats.GetFeatLevel(FeatList.CombinedCommandsII);
            double combopenalty = Math.Max( 0.0, (lingering - (combo * 5)) );
            double cooldown = (time - lingering) - (5 * combo) + combopenalty;
            this.m_NextSongAllowed = DateTime.Now + TimeSpan.FromSeconds( cooldown );
            return time;
		}
		
		public virtual bool CanIssueCommand()
		{
			if( !this.Alive || this.Paralyzed || this.IsTired() || DateTime.Compare( this.m_NextSongAllowed, DateTime.Now ) > 0 )
			{
				this.SendMessage( "You cannot play your music at the moment." );
				this.CurrentCommand = SongList.None;
				return false;
			}
			
			return true;
		}
		
		public virtual bool HasEnoughStamina( int featlevel )
		{
			if( CheckBardStam(Math.Max(1, featlevel) + 4) )
    			return true;

			return false;
		}
		
		public bool CheckBardStam( int featlevel )
        {
			Mobile mob = this;
        	IKhaerosMobile featuser = mob as IKhaerosMobile;
        		
            if( mob.Stam >= featlevel )
            {
                mob.Stam -= featlevel;
                return true;
            }
            else
            {
                mob.SendMessage( 60, "You need {0} stamina in order to perform this song.", featlevel );
                return false;
            }
        }
		
		public bool RaciallyCompatible( Nation nation )
		{
			if( (this.Nation == Nation.Azhuran || this.Nation == Nation.Khemetar) && (nation == Nation.Azhuran || nation == Nation.Khemetar) )
				return true;
			
			if( (this.Nation == Nation.Vhalurian || this.Nation == Nation.Tyrean) && (nation == Nation.Vhalurian || nation == Nation.Tyrean) )
				return true;
			
			if( (this.Nation == Nation.Mhordul || this.Nation == Nation.Alyrian) && (nation == Nation.Mhordul || nation == Nation.Alyrian) )
				return true;
			
			return false;
		}
		
		public int GetHour()
		{
			if( Map == Map.Internal )
				return 0;
			
			int minute = TimeSystem.Data.Minute + TimeSystem.TimeEngine.GetAdjustments(this, true);
            int hour = TimeSystem.Data.Hour;
            int day = TimeSystem.Data.Day;
            int month = TimeSystem.Data.Month;
            int year = TimeSystem.Data.Year;

            TimeSystem.TimeEngine.CheckTime(ref minute, ref hour, ref day, ref month, ref year, false);
            
            return hour;
		}
		
		public static int GetVampireTimeOffset( int hour )
		{
			int offset = 0;
			
			if( hour == 18 || hour == 19 )
				offset = 0;
			
			else if( hour >= 20 && hour <= 22 )
				offset = 1;
			
			else if( hour >= 23 || hour <= 2 )
				offset = 2;
			
			else if( hour == 3 || hour == 4 )
				offset = 1;
			
			else if( hour == 5 || hour == 6 )
				offset = 0;
			
			else if( hour >= 7 && hour <= 9 )
				offset = -1;
			
			else if( hour >= 10 && hour <= 14 )
				offset = -2;
			
			else if( hour >= 15 && hour <= 17 )
				offset = -1;
			
			return offset;
		}
		
		public void HandleVampireStatOffsets()
		{
			int offset = GetVampireTimeOffset(GetHour());
			
			if( offset > 0 )
                offset *= 20 + Math.Min( 30, ( Feats.GetFeatLevel( FeatList.NocturnalProwess ) * 10 ) );
			
			if( offset < 0 )
				offset *= 15 - Math.Min( 15, (Feats.GetFeatLevel(FeatList.Daywalker) * 5) );
			
			
			string name = String.Format( "[Vampire] {0} Offset", StatType.StamMax );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.StamMax, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.ManaMax );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.ManaMax, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Str );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Str, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Dex );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Dex, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Int );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Int, name );
		}
		
		public void HandleVampireStatMod( StatMod mod, int offset, StatType type, string name )
		{
			if( mod != null && mod.Offset != offset )
			{
				RemoveStatMod( name );

                if( offset != 0 )
                    AddVampireStatMod( new StatMod( type, name, offset, TimeSpan.FromDays( 20 ) ) );
			}

			else if( mod == null && offset != 0 )
                AddVampireStatMod( new StatMod( type, name, offset, TimeSpan.FromDays( 20 ) ) );
		}

        public void AddVampireStatMod( StatMod mod )
        {
            AddStatMod( mod );

            if( mod.Offset > 0 )
            {

                if( mod.Type == StatType.StamMax )
                    Stam += mod.Offset;
                else if( mod.Type == StatType.ManaMax )
                    Mana += mod.Offset;
            }
        }
		
		public static void UpdateGuildBonuses()
		{
			foreach( NetState state in NetState.Instances )
			{
				PlayerMobile m = state.Mobile as PlayerMobile;

				if( state.Mobile != null && state.Mobile is PlayerMobile )
					m.HandleGuildStatOffsets();
			}
		}
		
		public void HandleGuildStatOffsets()
		{
			int offset = GetGuildHitsBonus();
			int hits = Hits;
			string name = String.Format( "[CustomGuild] {0} Offset", StatType.HitsMax );
			StatMod mod = GetStatMod( name );

			if( mod != null && mod.Offset != offset )
			{
				RemoveStatMod( name );
			
				if( offset != 0 )
					AddStatMod( new StatMod( StatType.HitsMax, name, offset, TimeSpan.FromHours(1) ) );
			}

			else if( mod == null && offset != 0 )
				AddStatMod( new StatMod( StatType.HitsMax, name, offset, TimeSpan.FromHours(1) ) );
			
			Hits = hits;
		}
		
		public void LogPetsIn()
		{
			ArrayList list = new ArrayList();
			
			foreach( Mobile mob in LoggedOutPets )
            	list.Add( mob );
			
			for( int i = 0; i < list.Count; ++i )
            {
                Mobile mob = (Mobile)list[i];
				mob.Map = Map;
				mob.Location = Location;
				LoggedOutPets.Remove( mob );
			}
		}
		
		public void EmptyBankBox()
		{
			if( BankBox == null || this == null || Deleted || BankBox.Deleted )
				return;
			
			ArrayList list = new ArrayList();
			
			foreach( Item item in BankBox.Items )
                list.Add( item );
			
			for( int i = 0; i < list.Count; i++ )
			{
				Item item = list[i] as Item;
				
				if( !(item is BaseJewel || item is Gold || item is Copper || item is Silver || item is IGem || item is BankCheck) )
				{
					try{ item.Delete(); }
					catch( Exception e ){ Console.WriteLine(e.Message); }
				}
			}
			
			EmptyBankBoxOn = DateTime.MinValue;
		}

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;
			
			Map map = from.Map;
			from.Map = Map.Trammel;
			from.Map = map;
			
			PlayerMobile m = from as PlayerMobile;
			m.m_LoginTime = DateTime.Now;
			m.Criminal = true;

            if( m.IsVampire && m.BPs < 1 )
                m.HueMod = 0;

			if( !m.OldMapChar )
				m.LogPetsIn();
			
			if( m.EmptyBankBoxOn != DateTime.MinValue && DateTime.Compare(DateTime.Now, m.EmptyBankBoxOn) > 0 )
				m.EmptyBankBox();
			
			if( m.Age != 0 && m.MaxAge != 0 )
				CheckBirthday( m );
			
			if( m.Age == 0 )
				m.Age = 18;
			
			if( m.Lives < 1 && !m.Alive )
			{
				m.SendMessage( 32, "You are dead." );
            	m.SendMessage( 60, "Please, contact a GM if you suffered any kind of injustice in regards to your permanent death." );
			}
            
            m.KnownLanguages.Common = 1500000;
            
            if( m.m_RacialResources == null )
            {
                m.m_RacialResources = new RacialResources();
            }

			CheckAtrophies( from );

			if ( AccountHandler.LockdownLevel > AccessLevel.Player )
			{
				string notice;

				Accounting.Account acct = from.Account as Accounting.Account;

				if ( acct == null || !acct.HasAccess( from.NetState ) )
				{
					if ( from.AccessLevel == AccessLevel.Player )
						notice = "The server is currently under lockdown. No players are allowed to log in at this time.";
					else
						notice = "The server is currently under lockdown. You do not have sufficient access level to connect.";

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Disconnect ), from );
				}
				else if ( from.AccessLevel >= AccessLevel.Administrator )
				{
					notice = "The server is currently under lockdown. As you are an administrator, you may change this from the .Admin gump.";
				}
				else
				{
					notice = "The server is currently under lockdown. You have sufficient access level to connect.";
				}

				from.SendGump( new NoticeGump( 1060637, 30720, notice, 0xFFC000, 300, 140, null, null ) );
			}
			
			if( m.BankBox != null && !( m.BankBox is PlayerBankBox ) )
			{
				BankBox bankbox = m.FindItemOnLayer( Layer.Bank ) as BankBox;
				bankbox.Delete();
				PlayerBankBox pbb = new PlayerBankBox( m );
				m.AddItem( pbb );
			}
			
			m.SendGump( new CharInfoGump( m ) );

			if( !m.FixedStatPoints )
			{
				int dexcap = 100 + ( m.GetBackgroundLevel(BackgroundList.Quick) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Clumsy) * 5 ) - m.TotalPenalty;
				int stamcap = 100 + ( m.GetBackgroundLevel(BackgroundList.Fit) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Unenergetic) * 5 ) - m.TotalPenalty;
	
				switch( m.Nation )
				{
					case Nation.Alyrian: dexcap += 30; stamcap += 20; break;
					case Nation.Azhuran: dexcap += 50; stamcap += 10; break;
					case Nation.Khemetar: dexcap += 40; stamcap += 0; break;
					case Nation.Mhordul: dexcap += 20; stamcap += 30; break;
					case Nation.Tyrean: dexcap += 0; stamcap += 40; break;
					case Nation.Vhalurian: dexcap += 10; stamcap += 50; break;
				}
			
				int dexoffset = m.RawDex - dexcap;
				int stamoffset = m.RawStam - stamcap;
				
				if( dexoffset > 0 )
				{
					m.RawDex -= m.RawDex - dexcap;
					m.StatPoints += dexoffset;
				}
				
				if( stamoffset > 0 )
				{
					m.RawStam -= m.RawStam - stamcap;
					m.StatPoints += stamoffset;
				}
				
				m.FixedStatPoints = true;
			}
			
			if( !m.FixedRun )
			{
				m.ArmourPieces = 0;
				m.LightPieces = 0;
				m.HeavyPieces = 0;
				m.MediumPieces = 0;
				m.LightPenalty = 0;
				m.MediumPenalty = 0;
				m.HeavyPenalty = 0;
				CheckPenalty( m, Layer.Arms );
				CheckPenalty( m, Layer.Gloves );
				CheckPenalty( m, Layer.Helm );
				CheckPenalty( m, Layer.InnerLegs );
				CheckPenalty( m, Layer.InnerTorso );
				CheckPenalty( m, Layer.MiddleTorso );
				CheckPenalty( m, Layer.Neck );
				CheckPenalty( m, Layer.OuterLegs );
				CheckPenalty( m, Layer.OuterTorso );
				CheckPenalty( m, Layer.Pants );
				CheckPenalty( m, Layer.Shirt );
				CheckPenalty( m, Layer.Shoes );
				CheckPenalty( m, Layer.TwoHanded );
				m.FixedRun = true;
			}
			
			if( m.OldMapChar && !m.Reforging && !m.Forging )
				m.Reforge();
			
			else if( !m.Reforging && !m.Forging && m.AccessLevel < AccessLevel.GameMaster && m.BuggedCP )
			{
				m.SendMessage( "The amount of CP you had spent was above your cap due to a bug that allowed it. Please reforge." );
				m.Reforge();
			}
			
			m.SendStatusIcons();
			m.LogoutTime = DateTime.MinValue;

            foreach( NetState state in NetState.Instances )
            {
                PlayerMobile staffer = state.Mobile as PlayerMobile;

                if( state.Mobile != null && state.Mobile is PlayerMobile && staffer.AccessLevel > AccessLevel.Player && staffer.LogMsgs )
                    staffer.SendMessage( m.Account.Username + " has logged in on " + m.Name + "." );

                if( m.AccessLevel > AccessLevel.Player && state.Mobile is PlayerMobile && staffer.NeedsFixing )
                {
                    staffer.Map = Map.Felucca;
                    staffer.NetState.Dispose();
                }
            }
			
			m.FixControlSlots();
			
			m.HandleGhoulStatOffsets();
		}
		
		public void FixControlSlots()
		{
			Followers = 0;
			ArrayList pets = new ArrayList();

			foreach( Mobile m in World.Mobiles.Values )
			{
				if( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;

					if ( (bc.Controlled && bc.ControlMaster == this) || (bc.Summoned && bc.SummonMaster == this) )
						Followers += bc.ControlSlots;
				}
			}
		}
		
		public void HandleGhoulStatOffsets()
		{
			if( IsVampire || LastTimeGhouled == DateTime.MinValue )
				return;
			
			int offset = 0;
			
			if( DateTime.Compare(DateTime.Now, (LastTimeGhouled + TimeSpan.FromDays(10)) ) < 0 )
				offset = 10;
			
			else if( DateTime.Compare(DateTime.Now, (LastTimeGhouled + TimeSpan.FromDays(30)) ) < 0 )
				offset = -10;
			
			else
			{
				LastTimeGhouled = DateTime.MinValue;
				CheckBirthday( this );
			}
			
			string name = String.Format( "[Vampire] {0} Offset", StatType.StamMax );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.StamMax, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.ManaMax );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.ManaMax, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Str );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Str, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Dex );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Dex, name );
			name = String.Format( "[Vampire] {0} Offset", StatType.Int );
			HandleVampireStatMod( GetStatMod(name), offset, StatType.Int, name );
		}
		
		public static void CheckPenalty( PlayerMobile m, Layer layer )
		{
			BaseArmor armor = m.FindItemOnLayer( layer ) as BaseArmor;
			
			if( armor != null && armor is BaseArmor )
			{
				if( armor.ArmourType == ArmourWeight.Light )
				{
					if( armor.Attributes.BonusStam < 1 )
						m.LightPieces++;
					
					m.LightPenalty += armor.Attributes.BonusStam;
				}
				
				else if( armor.ArmourType == ArmourWeight.Medium )
				{
					m.MediumPenalty += armor.Attributes.BonusStam;
					m.MediumPieces++;
				}
				
				else
				{
					m.HeavyPenalty += armor.Attributes.BonusStam;
					m.HeavyPieces++;
				}
				
				m.ArmourPieces++;
			}
		}
		
		public static void CheckBirthday( PlayerMobile m )
		{
			if( m.LastTimeGhouled != DateTime.MinValue || m.IsVampire )
				return;
			
			int year, month, day;
			
			if( !int.TryParse( m.YearOfBirth, out year ) || !int.TryParse( m.MonthOfBirth, out month ) || !int.TryParse( m.DayOfBirth, out day ) )
				return;
			
			int curyear = TimeSystem.Data.Year;
			int curmonth = TimeSystem.Data.Month;
			int curday = TimeSystem.Data.Day;
			
			m.Age = curyear - year - 1;
			
			if( curmonth > month )
				m.Age++;
			
			else if( curmonth == month && curday >= day )
				m.Age++;
		}
		
		public static int ArmourCheck( Item item )
        {
        	if( item != null )
            {
            	if( item is BaseArmor )
            	{
            		return 1;
            	}
            }
        	
        	return 0;
        }
        
		public bool CanPerformAttack( int featlevel )
		{
			if( featlevel > 0 )
				return true;
			
			this.SendMessage( 60, "You do not know how to perform this attack." );
			return false;
		}
		
		public bool CanUseMartialPower
		{
			get
			{
				if( !(this.Weapon is Fists) || !CanUseMartialStance )
				{
					this.SendMessage( 60, "You can only use this power with your bare hands, unmounted and unemcumbered by armour." );
					this.DisableManeuver();
					return false;
				}
				
				return true;
			}
		}
		
		public bool CanUseMartialStance
		{
			get
			{
				if( this.Mounted || this.HasArmourPenalties || !(this.Weapon is Fists) )
					return false;
				
				return true;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasArmourPenalties{ get{ return (TotalPenalty > 0); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsWearingArmour{ get{ return (LightPieces > 0 || MediumPieces > 0 || HeavyPieces > 0); } }

		private bool m_NoDeltaRecursion;

		public void ValidateEquipment()
		{
			if ( m_NoDeltaRecursion || Map == null || Map == Map.Internal )
				return;

			if ( this.Items == null )
				return;

			m_NoDeltaRecursion = true;
			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ValidateEquipment_Sandbox ) );
		}

		private void ValidateEquipment_Sandbox()
		{
			try
			{
				if ( Map == null || Map == Map.Internal )
					return;

				List<Item> items = this.Items;

				if ( items == null )
					return;

				bool moved = false;

				int str = this.Str;
				int dex = this.Dex;
				int intel = this.Int;

				#region Factions
				int factionItemCount = 0;
				#endregion

				Mobile from = this;

				#region Ethics
				Ethics.Ethic ethic = Ethics.Ethic.Find( from );
				#endregion

				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;

					Item item = items[i];

					#region Ethics
					if ( ( item.SavedFlags & 0x100 ) != 0 )
					{
						if ( item.Hue != Ethics.Ethic.Hero.Definition.PrimaryHue )
						{
							item.SavedFlags &= ~0x100;
						}
						else if ( ethic != Ethics.Ethic.Hero )
						{
							from.AddToBackpack( item );
							moved = true;
							continue;
						}
					}
					else if ( ( item.SavedFlags & 0x200 ) != 0 )
					{
						if ( item.Hue != Ethics.Ethic.Evil.Definition.PrimaryHue )
						{
							item.SavedFlags &= ~0x200;
						}
						else if ( ethic != Ethics.Ethic.Evil )
						{
							from.AddToBackpack( item );
							moved = true;
							continue;
						}
					}
					#endregion

					if ( item is BaseWeapon )
					{
						BaseWeapon weapon = (BaseWeapon)item;

						bool drop = false;

						if( dex < weapon.DexRequirement )
							drop = true;
						else if( str < AOS.Scale( weapon.StrRequirement, 100 - weapon.GetLowerStatReq() ) )
							drop = true;
						else if( intel < weapon.IntRequirement )
							drop = true;
						else if( weapon.RequiredRace != null && weapon.RequiredRace != this.Race )
							drop = true;

						if ( drop )
						{
							string name = weapon.Name;

							if ( name == null )
								name = String.Format( "#{0}", weapon.LabelNumber );

							from.SendLocalizedMessage( 1062001, name ); // You can no longer wield your ~1_WEAPON~
							from.AddToBackpack( weapon );
							moved = true;
						}
					}
					else if ( item is BaseArmor )
					{
						BaseArmor armor = (BaseArmor)item;

						bool drop = false;

						if ( !armor.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !armor.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if( armor.RequiredRace != null && armor.RequiredRace != this.Race )
						{
							drop = true;
						}
						else
						{
							int strBonus = armor.ComputeStatBonus( StatType.Str ), strReq = armor.ComputeStatReq( StatType.Str );
							int dexBonus = armor.ComputeStatBonus( StatType.Dex ), dexReq = armor.ComputeStatReq( StatType.Dex );
							int intBonus = armor.ComputeStatBonus( StatType.Int ), intReq = armor.ComputeStatReq( StatType.Int );

							if( dex < dexReq || (dex + dexBonus) < 1 )
								drop = true;
							else if( str < strReq || (str + strBonus) < 1 )
								drop = true;
							else if( intel < intReq || (intel + intBonus) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = armor.Name;

							if ( name == null )
								name = String.Format( "#{0}", armor.LabelNumber );

							if ( armor is BaseShield )
								from.SendLocalizedMessage( 1062003, name ); // You can no longer equip your ~1_SHIELD~
							else
								from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( armor );
							moved = true;
						}
					}
					else if ( item is BaseClothing )
					{
						BaseClothing clothing = (BaseClothing)item;

						bool drop = false;

						if ( !clothing.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !clothing.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if( clothing.RequiredRace != null && clothing.RequiredRace != this.Race )
						{
							drop = true;
						}
						else
						{
							int strBonus = clothing.ComputeStatBonus( StatType.Str );
							int strReq = clothing.ComputeStatReq( StatType.Str );

							if( str < strReq || (str + strBonus) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = clothing.Name;

							if ( name == null )
								name = String.Format( "#{0}", clothing.LabelNumber );

							from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( clothing );
							moved = true;
						}
					}

					FactionItem factionItem = FactionItem.Find( item );

					if ( factionItem != null )
					{
						bool drop = false;

						Faction ourFaction = Faction.Find( this );

						if ( ourFaction == null || ourFaction != factionItem.Faction )
							drop = true;
						else if ( ++factionItemCount > FactionItem.GetMaxWearables( this ) )
							drop = true;

						if ( drop )
						{
							from.AddToBackpack( item );
							moved = true;
						}
					}
				}

				if ( moved )
					from.SendLocalizedMessage( 500647 ); // Some equipment has been moved to your backpack.
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
			}
			finally
			{
				m_NoDeltaRecursion = false;
			}
		}

		public override void Delta( MobileDelta flag )
		{
			base.Delta( flag );

			if ( (flag & MobileDelta.Stat) != 0 )
				ValidateEquipment();

			if ( (flag & (MobileDelta.Name | MobileDelta.Hue)) != 0 )
				InvalidateMyRunUO();
		}

		private static void Disconnect( object state )
		{
			NetState ns = ((Mobile)state).NetState;

			if ( ns != null )
				ns.Dispose();
		}

		private static void OnLogout( LogoutEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;
			
			if( e.Mobile is PlayerMobile && !pm.Alive && pm.Lives > 0 )
			{
				Container m_corpse = pm.Corpse;
				pm.m_DeathTimer.Stop();
				pm.m_DeathTimer = null;
				
				ArrayList list = new ArrayList();
	            Container pack = e.Mobile.Backpack;
	            m_corpse.Movable = true;
	
	            e.Mobile.Resurrect();
	
	            if( m_corpse.Parent != null )
	            {
	                try
	                {
	                    Mobile parent = World.FindMobile( m_corpse.RootParentEntity.Serial );
	                    e.Mobile.Location = parent.Location;
	                }
	
	                catch
	                {
	                    Container parent = m_corpse.Parent as Container;
	                    e.Mobile.Location = parent.Location;
	                }
	            }
	
	            else
	            {
	                e.Mobile.Location = m_corpse.Location;
	            }
	
	            pack.DropItem( m_corpse );
	            m_corpse.OnDoubleClick( e.Mobile );
	            e.Mobile.Squelched = false;
	            e.Mobile.Emote( "*slowly comes to*" );
	            e.Mobile.Animate( 21, 6, 1, false, false, 0 );
	            e.Mobile.SendMessage( "You will be invulnerable for a few seconds to be able to escape from creatures that might instantly kill you again. Additionally, you will be weakened for some minutes." );
	            
	            foreach ( Item item in m_corpse.Items )
	                list.Add( item );
	
	            if( list.Count > 0 )
	            {
	                for( int i = 0; i < list.Count; ++i )
	                {
	                    Item item = (Item)list[i];
	                    pack.AddItem( item );
	                }
	            }
	            
	            m_corpse.Delete();
	            
	            pm.m_InvulTimer = new InvulTimer( e.Mobile );
	            pm.m_InvulTimer.Start();
	            
	            XmlAttachment stratt = XmlAttach.FindAttachment( pm, typeof( XmlStr ) );
            	XmlAttachment dexatt = XmlAttach.FindAttachment( pm, typeof( XmlDex ) );
            	XmlAttachment intatt = XmlAttach.FindAttachment( pm, typeof( XmlInt ) );
            	XmlAttachment hitsatt = XmlAttach.FindAttachment( pm, typeof( XmlHits ) );
            	XmlAttachment stamatt = XmlAttach.FindAttachment( pm, typeof( XmlStam ) );
            	XmlAttachment manaatt = XmlAttach.FindAttachment( pm, typeof( XmlMana ) );
            	
            	if( stratt != null )
            		stratt.Delete();
            	if( dexatt != null )
            		dexatt.Delete();
            	if( intatt != null )
            		intatt.Delete();
            	if( hitsatt != null )
            		hitsatt.Delete();
            	if( stamatt != null )
            		stamatt.Delete();
            	if( manaatt != null )
            		manaatt.Delete();
	            	            
            	XmlAttach.AttachTo( pm, new XmlStr( ( 0 - ( pm.RawStr / 4 ) ), 600.0 ) );
            	XmlAttach.AttachTo( pm, new XmlDex( ( 0 - ( pm.RawDex / 4 ) ), 600.0 ) );
            	XmlAttach.AttachTo( pm, new XmlInt( ( 0 - ( pm.RawInt / 4 ) ), 600.0 ) );
            	XmlAttach.AttachTo( pm, new XmlHits( ( 0 - ( pm.RawHits / 4 ) ), 600.0 ) );
            	XmlAttach.AttachTo( pm, new XmlStam( ( 0 - ( pm.RawStam / 4 ) ), 600.0 ) );
            	XmlAttach.AttachTo( pm, new XmlMana( ( 0 - ( pm.RawMana / 4 ) ), 600.0 ) );
			}
			
			ArrayList newlist = new ArrayList();
			
			foreach( Mobile m in pm.GetMobilesInRange( 2 ) )
                        newlist.Add( m );
			
			for( int i = 0; i < newlist.Count; ++i )
            {
                Mobile m = (Mobile)newlist[i];

                if( m != pm && m is BaseCreature && ( (BaseCreature)m ).ControlMaster == pm && m.Alive && m.Map == pm.Map )
                {
                	pm.LoggedOutPets.Add( m );
                	m.Internalize();
                }
			}
			
			pm.LogoutTime = DateTime.Now;
		}

		private static void EventSink_Connected( ConnectedEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_SessionStart = DateTime.Now;

				if ( pm.m_Quest != null )
					pm.m_Quest.StartTimer();

				pm.BedrollLogout = false;
				pm.LastOnline = DateTime.Now;
			}

			Timer.DelayCall( TimeSpan.Zero, new TimerStateCallback( ClearSpecialMovesCallback ), e.Mobile );
		}

		private static void ClearSpecialMovesCallback( object state )
		{
			Mobile from = (Mobile)state;

			SpecialMove.ClearAllMoves( from );
		}

		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
			DesignContext context = DesignContext.Find( from );

			if ( context != null )
			{
				/* Client disconnected
				 *  - Remove design context
				 *  - Eject all from house
				 *  - Restore relocated entities
				 */

				// Remove design context
				DesignContext.Remove( from );

				// Eject all from house
				from.RevealingAction();

				foreach ( Item item in context.Foundation.GetItems() )
					item.Location = context.Foundation.BanLocation;

				foreach ( Mobile mobile in context.Foundation.GetMobiles() )
					mobile.Location = context.Foundation.BanLocation;

				// Restore relocated entities
				context.Foundation.RestoreRelocatedEntities();
			}

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_GameTime += (DateTime.Now - pm.m_SessionStart);

				if ( pm.m_Quest != null )
					pm.m_Quest.StopTimer();

				pm.m_SpeechLog = null;
				pm.LastOnline = DateTime.Now;
			}
		}
		
		public override void OnDisconnected()
		{
            if( this.NeedsFixing )
                return;

			foreach ( NetState state in NetState.Instances )
			{
				PlayerMobile staffer = state.Mobile as PlayerMobile;

				if( state.Mobile != null && state.Mobile is PlayerMobile && staffer.AccessLevel > AccessLevel.Player && staffer.LogMsgs )
					staffer.SendMessage( this.Account.Username + " has logged out from " + this.Name + "." );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasActiveGuildFlag
		{
			get
			{
				if( FindItemOnLayer( Layer.TwoHanded ) != null && FindItemOnLayer( Layer.TwoHanded ) is GuildFlag && ((GuildFlag)FindItemOnLayer( Layer.TwoHanded )).Active )
					return true;
				
				return false;
			}
		}
		
		public int GetGuildHitsBonus()
		{
			if( this == null || !Alive || CustomGuilds.Count < 1 )
				return 0;
			
			int bonus = 0;
			int mod = 1;
			
			foreach( Mobile mob in GetMobilesInRange(8) )
			{
				if( mob is PlayerMobile && mob != this && CanSee(mob) && mob.CanSee(this) && InLOS(mob) )
				{
					foreach( KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in ((PlayerMobile)mob).CustomGuilds )
					{
						if( CustomGuilds.ContainsKey(kvp.Key) )
						{
							bonus += 1;
							
							if( ((PlayerMobile)mob).HasActiveGuildFlag )
								mod = 2;
						}
					}
				}
			}
			
			if( HasActiveGuildFlag )
				mod = 2;
			
			return (mod * bonus);
		}
		
		public bool IsWarrior()
		{
			return true;
		}
		
		public bool IsRogue()
		{
			return true;
		}
		
		public bool IsCleric()
		{
			return true;
		}

		public override void RevealingAction()
		{
			if ( m_DesignContext != null )
				return;

			Spells.Sixth.InvisibilitySpell.RemoveTimer( this );

			base.RevealingAction();
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Hidden
		{
			get
			{
				return base.Hidden;
			}
			set
			{
				base.Hidden = value;
				
				if ( Hidden )
					CombatSystemAttachment.GetCSA( this ).StopAllActions( true );

				/*RemoveBuff( BuffIcon.Invisibility );	//Always remove, default to the hiding icon EXCEPT in the invis spell where it's explicitly set

				if( !Hidden )
				{
					RemoveBuff( BuffIcon.HidingAndOrStealth );
				}
				else// if( !InvisibilitySpell.HasTimer( this ) )
				{
					BuffInfo.AddBuff( this, new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );	//Hidden/Stealthing & You Are Hidden
				}*/
			}
		}

		public override void OnSubItemAdded( Item item )
		{
			if ( AccessLevel < AccessLevel.GameMaster && item.IsChildOf( this.Backpack ) )
			{
				int maxWeight = WeightOverloading.GetMaxWeight( this );
				int curWeight = Mobile.BodyWeight + this.TotalWeight;

				if ( curWeight > maxWeight )
					this.SendLocalizedMessage( 1019035, true, String.Format( " : {0} / {1}", curWeight, maxWeight ) );
			}
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
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

		public override bool CanBeBeneficial( Mobile target, bool message, bool allowDead )
		{
			if ( m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null) )
				return false;

			return base.CanBeBeneficial( target, message, allowDead );
		}

		public override bool CheckContextMenuDisplay( IEntity target )
		{
			return ( m_DesignContext == null );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
			}

			if ( this.NetState != null )
				CheckLightLevels( false );
				
			if ( item is BaseShield || item is BaseWeapon )
				CombatSystemAttachment.GetCSA( this ).OnUnequippedOrEquipped();

			InvalidateMyRunUO();
			
			if ( item is BaseArmor || item is BaseWeapon || item is IMountItem ) // this must be called last
				SendStatusIcons();
		}
		
		public void AddBuildupBuff() // does not check for requirements, these are implied
		{
			double bonus = CombatSystemAttachment.BuildupBonuses( this );

			if ( bonus > 0 )
			{
				if ( bonus > 0.6 )
					bonus = 0.6;
				this.AddBuff( new BuffInfo(
					BuffIcon.AnimalForm, 1041600, 1060847, "<CENTER>Buildup\t<BR>-" + bonus + "s attack connect time", false
				) );
			}
		}
		
		public void AddFightingStyleBuff() // does not check for requirements, these are implied
		{
			double bonus = Math.Round( CombatSystemAttachment.FightingStyleBonuses( this )*100, 2 );
			if ( bonus > 45 )
				bonus = 45;
			if ( bonus > 0 )
			{
				this.AddBuff( new BuffInfo(
					BuffIcon.EtherealVoyage, 1041600, 1060847, "<CENTER>Fighting Style\t<BR>+" + bonus + "% damage", false
				) );
			}
		}
		
		public void MountedDefenceIconRefresh()
		{
			RemoveBuff( BuffIcon.ArchProtection );
			if ( Feats.GetFeatLevel(FeatList.MountedDefence) > 0 )
			{
				Mobile mount = this.Mount as Mobile;
				if ( mount != null && (((double)mount.Hits)/((double)mount.HitsMax)) >= ((double)(15+Feats.GetFeatLevel(FeatList.MountedDefence)*20))/100.0)
				{
					int amt = 5*Feats.GetFeatLevel(FeatList.MountedDefence);
					this.AddBuff( new BuffInfo(
						BuffIcon.ArchProtection, 1041600, 1060847, "<CENTER>Mounted Defence\t<BR>" + amt + "% of damage transfers to mount", false
					) );
				}
			}
		}
		
		public void CantRunIconRefresh()
		{
			RemoveBuff( BuffIcon.Weaken );
			string reason = "";

			if ( CrippledTimer != null )
				reason = "You have been crippled";
			else if ( DisabledLegsTimer != null )
				reason = "Your legs have been disabled";
			else if ( !CombatSystemAttachment.GetCSA( this ).CanRun() )
				reason = "Recuperating after charge";
			
			if ( reason != "" )
			{
				this.AddBuff( new BuffInfo(
					BuffIcon.Weaken, 1041600, 1060847, "<CENTER>Cannot Run\t<BR>"+reason, false
				) );
			}
		}
		
		public void SendStatusIcons()
		{
			IKhaerosMobile km = this as IKhaerosMobile;
			BaseWeapon weapon = Weapon as BaseWeapon;
			RemoveBuff( BuffIcon.EnemyOfOne );
			RemoveBuff( BuffIcon.AttuneWeapon );
			RemoveBuff( BuffIcon.Protection );
			RemoveBuff( BuffIcon.UnknownKneelingSword );
			RemoveBuff( BuffIcon.GiftOfRenewal );
			RemoveBuff( BuffIcon.GiftOfLife );
			RemoveBuff( BuffIcon.Clumsy );
			RemoveBuff( BuffIcon.DismountPrevention );
			RemoveBuff( BuffIcon.Cunning );
			RemoveBuff( BuffIcon.PainSpike );
			RemoveBuff( BuffIcon.ActiveMeditation );
			RemoveBuff( BuffIcon.MagicReflection );
			//RemoveBuff( BuffIcon.BloodOathCaster ); // Not necessary, CheckForHuntingHound() already does this
			RemoveBuff( BuffIcon.BloodOathCurse );
			RemoveBuff( BuffIcon.Strength );
			RemoveBuff( BuffIcon.NoRearm );
			//RemoveBuff( BuffIcon.EtherealVoyage ); // Not necessary, already handled otherwise
			
			BaseShield shield = FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			BaseWeapon onehander = FindItemOnLayer( Layer.OneHanded ) as BaseWeapon;
			BaseWeapon twohander = FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
			
			if ( weapon == null )
				return;
				
			MountedDefenceIconRefresh();
			if ( RessSick )
			{
				this.AddBuff( new BuffInfo(
						BuffIcon.BloodOathCurse, 1041600, 1060847, "<CENTER>Cannot Attack\t<BR>Currently tired due to knockout", false
					) );
			}
			else if ( weapon.CannotUseOnFoot && !Mounted )
			{
				this.AddBuff( new BuffInfo(
						BuffIcon.BloodOathCurse, 1041600, 1060847, "<CENTER>Cannot Attack\t<BR>Weapon cannot be used on foot", false
					) );
			}
			else if ( weapon.CannotUseOnMount && Mounted )
			{
				this.AddBuff( new BuffInfo(
						BuffIcon.BloodOathCurse, 1041600, 1060847, "<CENTER>Cannot Attack\t<BR>Weapon cannot be used while mounted", false
					) );
			}
			
			if( CanDodge && !Mounted )
			{
				if ( Feats.GetFeatLevel(FeatList.EnhancedDodge) > 0 )
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.ActiveMeditation, 1041600, 1060847, "<CENTER>Dodge\t<BR>" + 5*Feats.GetFeatLevel(FeatList.EnhancedDodge) + "% chance to dodge", false
					) );
				}
				
				if ( Feats.GetFeatLevel(FeatList.CatchProjectiles) > 0 && shield == null && weapon is Fists )
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.MagicReflection, 1041600, 1060847, "<CENTER>Catch Projectiles\t<BR>" + Feats.GetFeatLevel(FeatList.CatchProjectiles)*10 + "% chance to catch", false
					) );
				}
			}
			
			//AddFightingStyleBuff(); // Automatically added by CombatSystemAttachment when relevant

			if ( shield == null && weapon.Critical )
			{
				this.AddBuff( new BuffInfo(
						BuffIcon.Strength, 1041600, 1060847, "<CENTER>Critical\t<BR>This weapon has a 50% chance to do 1.5x damage", false
					) );
			}
			
			if( weapon is Fists && this is IKhaerosMobile && ((IKhaerosMobile)this).TechniqueLevel > 0 )
			{
				int slax = 0;
				int pirc = 0;
				int blut = 100;
				if( ((IKhaerosMobile)this).Technique == "slashing" )
					slax = ((IKhaerosMobile)this).TechniqueLevel;
				else if( ((IKhaerosMobile)this).Technique == "piercing" )
					pirc = ((IKhaerosMobile)this).TechniqueLevel;
				
				blut -= ((IKhaerosMobile)this).TechniqueLevel;
				
				if ( blut > 0 )
				{
					this.AddBuff( new BuffInfo(
							BuffIcon.NoRearm, 1041600, 1060847, "<CENTER>Less-Lethal Property (Blunt)\t<BR>-" + Math.Round((blut*0.2), 2) + "% damage", false
						) );
						
					this.AddBuff( new BuffInfo(
							BuffIcon.GiftOfLife, 1041600, 1060847, "<CENTER>Splash Damage (Blunt)\t<BR>" + (blut/2) + "% chance", false
						) );
				}
			}
			else if ( weapon.AosElementDamages.Blunt > 0 )
			{
				this.AddBuff( new BuffInfo(
						BuffIcon.NoRearm, 1041600, 1060847, "<CENTER>Less-Lethal Property (Blunt)\t<BR>-" + Math.Round((weapon.AosElementDamages.Blunt*0.2), 2) + "% damage", false
					) );
					
				this.AddBuff( new BuffInfo(
						BuffIcon.GiftOfLife, 1041600, 1060847, "<CENTER>Splash Damage (Blunt)\t<BR>" + (weapon.AosElementDamages.Blunt/2) + "% chance", false
					) );
			}
			
			if ( Feats.GetFeatLevel(FeatList.WeaponSpecialization) > 0 )
			{
				if ( weapon.NameType == WeaponSpecialization )
				{
					if ( shield == null && Feats.GetFeatLevel(FeatList.WeaponParrying) > 0 )
					{
						double bonus = 0.125;
						if ( Feats.GetFeatLevel(FeatList.WeaponParrying) == 2 )
							bonus = 0.25;
						else if ( Feats.GetFeatLevel(FeatList.WeaponParrying) >= 3 )
							bonus = 0.5;
							
						this.AddBuff( new BuffInfo(
							BuffIcon.Protection, 1041600, 1060847, "<CENTER>Weapon Parrying\t<BR>-" + bonus + "s reduced parry cooldown", false
						) );
					}
					this.AddBuff( new BuffInfo(
							BuffIcon.AttuneWeapon, 1041600, 1060847, "<CENTER>Specialized Weapon\t<BR>+" + (Feats.GetFeatLevel(FeatList.WeaponSpecialization)*7.5) + "% damage<BR>+" + (Feats.GetFeatLevel(FeatList.WeaponSpecialization)*7.5) + "% speed", false
						) );
				}
				else if ( weapon.NameType == SecondSpecialization )
				{
					if ( shield == null && Feats.GetFeatLevel(FeatList.WeaponParrying) > 0 )
					{
						double bonus = 0.125;
						if ( Feats.GetFeatLevel(FeatList.WeaponParrying) == 2 )
							bonus = 0.25;
						else if ( Feats.GetFeatLevel(FeatList.WeaponParrying) >= 3 )
							bonus = 0.5;
							
						this.AddBuff( new BuffInfo(
							BuffIcon.Protection, 1041600, 1060847, "<CENTER>Weapon Parrying\t<BR>-" + bonus + "s reduced parry cooldown", false
						) );
					}
					this.AddBuff( new BuffInfo(
						BuffIcon.AttuneWeapon, 1041600, 1060847, "<CENTER>Specialized Weapon\t<BR>+" + (Feats.GetFeatLevel(FeatList.SecondSpecialization)*7.5) + "% damage<BR>+" + (Feats.GetFeatLevel(FeatList.SecondSpecialization)*7.5) + "% speed", false
					) );
				}
				else
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.UnknownKneelingSword, 1041600, 1060847, "<CENTER>Unspecialized Weapon\t<BR>-" + (Feats.GetFeatLevel(FeatList.WeaponSpecialization)*7.5) + "% damage<BR>-" + (Feats.GetFeatLevel(FeatList.WeaponSpecialization)*7.5) + "% speed", false
					) );
				}
			}
			
			if ( !(weapon is Fists) )
			{	
				if ( onehander != null && shield == null )
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.EnemyOfOne, 1041600, 1060847, "<CENTER>One-Handed Weapon w/o Shield\t<BR>+20% speed", false
					) );
                }
            
				else if ( twohander != null && !(twohander is BaseRanged) )
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.EnemyOfOne, 1041600, 1060847, "<CENTER>Two-Handed Weapon\t+30% damage", false // this actually looks fine, do not add <BR>!
					) );
					
					if ( Feats.GetFeatLevel(FeatList.GreatweaponFighting) > 0 && !weapon.BastardWeapon)
					{
						double bonus = 0.0;
						if ( Feats.GetFeatLevel(FeatList.GreatweaponFighting) == 1 )
							bonus = 0.125;
						else if ( Feats.GetFeatLevel(FeatList.GreatweaponFighting) == 2 )
							bonus = 0.25;
						else if ( Feats.GetFeatLevel(FeatList.GreatweaponFighting) >= 3 )
							bonus = 0.5;
							
						this.AddBuff( new BuffInfo(
							BuffIcon.GiftOfRenewal, 1041600, 1060847, "<CENTER>Greatweapon Fighting\t<BR>+" + bonus + "s extra cooldown for attacker", false
						) );
					}
					
					if ( weapon.Unwieldy )
					{
						if ( weapon is BasePoleArm )
						{
							this.AddBuff( new BuffInfo(
								BuffIcon.Clumsy, 1041600, 1060847, "<CENTER>Unwieldy Weapon\t<BR>-" + (50-Feats.GetFeatLevel(FeatList.PolearmsMastery)*10) + "% speed", false
							) );
						}
						else // there are no such weapons atm
						{
							this.AddBuff( new BuffInfo(
								BuffIcon.Clumsy, 1041600, 1060847, "<CENTER>Unwieldy Weapon\t<BR>-50% speed", false
							) );
						}
					}
				}
				else if ( twohander != null && twohander is BaseRanged )
				{
					if ( Mounted && Feats.GetFeatLevel(FeatList.MountedArchery) < 3 )
					{
						this.AddBuff( new BuffInfo(
							BuffIcon.DismountPrevention, 1041600, 1060847, "<CENTER>Mounted Archery\t<BR>-" + (75-Feats.GetFeatLevel(FeatList.MountedArchery)*25) + "% hit chance", false
						) );
					}
					
					if ( twohander is HeavyCrossbow || twohander is Crossbow || twohander is RepeatingCrossbow )
					{
						if ( Feats.GetFeatLevel(FeatList.CrossbowMastery) > 0 )
						{
							this.AddBuff( new BuffInfo(
								BuffIcon.Cunning, 1041600, 1060847, "<CENTER>Crossbow Mastery\t<BR>+" + 5*Feats.GetFeatLevel(FeatList.CrossbowMastery) + "% hit chance", false
							) );
						}
					}
					else if ( !(twohander is AzhuranBoomerang) ) // must be a bow
					{
						if ( Feats.GetFeatLevel(FeatList.BowMastery) > 0 )
						{
							this.AddBuff( new BuffInfo(
								BuffIcon.Cunning, 1041600, 1060847, "<CENTER>Bow Mastery\t<BR>+" + 5*Feats.GetFeatLevel(FeatList.BowMastery) + "% hit chance", false
							) );
						}
					}
				}
			}
			
			if ( shield == null )
			{
				if ( weapon.CannotBlock )
				{
					this.AddBuff( new BuffInfo(
							BuffIcon.PainSpike, 1041600, 1060847, "<CENTER>Cannot Parry\t<BR>This weapon cannot parry attacks", false
						) );
				}
				
				if ( weapon is Fists )
				{
					this.AddBuff( new BuffInfo(
							BuffIcon.EnemyOfOne, 1041600, 1060847, "<CENTER>Unarmed w/o Shield\t<BR>+20% speed", false
						) );
				}
			}
			else // we have a shield equipped
			{
				if ( Feats.GetFeatLevel(FeatList.DeflectProjectiles) > 0 )
				{
					this.AddBuff( new BuffInfo(
						BuffIcon.MagicReflection, 1041600, 1060847, "<CENTER>Deflect Projectiles\t<BR>" + Feats.GetFeatLevel(FeatList.DeflectProjectiles)*10 + "% chance to deflect", false
					) );
				}
				if ( Feats.GetFeatLevel(FeatList.ShieldMastery) > 0 )
				{
					double bonus = 0.0;
					if ( Feats.GetFeatLevel(FeatList.ShieldMastery) == 1 )
						bonus = 0.125;
					else if ( Feats.GetFeatLevel(FeatList.ShieldMastery) == 2 )
						bonus = 0.25;
					else if ( Feats.GetFeatLevel(FeatList.ShieldMastery) >= 3 )
						bonus = 0.5;
						
					this.AddBuff( new BuffInfo(
						BuffIcon.Protection, 1041600, 1060847, "<CENTER>Shield Mastery\t<BR>+" + bonus + "s extra cooldown for attacker", false
					) );
				}
			}
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( item is BaseArmor || item is BaseWeapon )
			{
				Hits=Hits; Stam=Stam; Mana=Mana;
				if ( item is BaseWeapon )
					CombatSystemAttachment.CancelFightingStyleBonus( this );
			}

			if ( this.NetState != null )
				CheckLightLevels( false );
				
			if ( item is BaseShield || item is BaseWeapon )
				CombatSystemAttachment.GetCSA( this ).OnUnequippedOrEquipped();

			InvalidateMyRunUO();
			
			if ( item is BaseArmor || item is BaseWeapon || item is IMountItem ) // this must be called last
				SendStatusIcons();
		}

		public override double ArmorRating
		{
			get
			{
				//BaseArmor ar;
				double rating = 0.0;

				AddArmorRating( ref rating, NeckArmor );
				AddArmorRating( ref rating, HandArmor );
				AddArmorRating( ref rating, HeadArmor );
				AddArmorRating( ref rating, ArmsArmor );
				AddArmorRating( ref rating, LegsArmor );
				AddArmorRating( ref rating, ChestArmor );
				AddArmorRating( ref rating, ShieldArmor );

				return VirtualArmor + VirtualArmorMod + rating;
			}
		}

		private void AddArmorRating( ref double rating, Item armor )
		{
			BaseArmor ar = armor as BaseArmor;

			if( ar != null && ( !Core.AOS || ar.ArmorAttributes.MageArmor == 0 ))
				rating += ar.ArmorRatingScaled;
		}

/*		#region [Stats]Max
		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get{ return this.HitsMax; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get{ return this.StamMax; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get{ return this.ManaMax; }
		}
		#endregion
*/
		#region Stat Getters/Setters

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Str
		{
			get
			{
				return base.Str;
			}
			set
			{
				base.Str = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Int
		{
			get
			{
				return base.Int;
			}
			set
			{
				base.Int = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Dex
		{
			get
			{
				return base.Dex;
			}
			set
			{
				base.Dex = value;

                ComputeResistances();

                if( HasGump( typeof( CharInfoGump ) ) && m_CharInfoTimer == null )
                {
                    m_CharInfoTimer = new CharInfoGump.CharInfoTimer( this );
                    m_CharInfoTimer.Start();
                }
			}
		}

        public override void OnRawDexChange( int oldValue )
        {
            base.OnRawDexChange( oldValue );

            ComputeResistances();

            if( HasGump( typeof( CharInfoGump ) ) && m_CharInfoTimer == null )
            {
                m_CharInfoTimer = new CharInfoGump.CharInfoTimer( this );
                m_CharInfoTimer.Start();
            }
        }

		#endregion
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasStreetWise{ get{ return false; } }
		
		public override bool Move( Direction d )
		{
			return Move( d, true );
		}

		public bool Move( Direction d, bool clientRequested )
		{
			if ( ( Paralyzed || Frozen || ( CombatSystemAttachment.GetCSA( this ).PerformingSequence && clientRequested ) ) )
			{
				SendLocalizedMessage( 500111 ); // You are frozen and can not move.

				return false;
			}
			
			NetState ns = this.NetState;

			if ( ns != null )
			{
				List<Gump> gumps = ns.Gumps;

				for ( int i = 0; i < gumps.Count; ++i )
				{
					if ( gumps[i] is ResurrectGump )
					{
						if ( Alive )
						{
							CloseGump( typeof( ResurrectGump ) );
						}
						else
						{
							SendLocalizedMessage( 500111 ); // You are frozen and cannot move.
							return false;
						}
					}
				}
			}

			TimeSpan speed = ComputeMovementSpeed( d );

			bool res;

			if ( !Alive )
				Server.Movement.MovementImpl.IgnoreMovableImpassables = true;

			res = base.Move( d );

			Server.Movement.MovementImpl.IgnoreMovableImpassables = false;

			if ( !res )
				return false;

			m_NextMovementTime += speed;

			return true;
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
			DesignContext context = m_DesignContext;

			if ( context == null )
				return base.CheckMovement( d, out newZ );

			HouseFoundation foundation = context.Foundation;

			newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level, context.Foundation );

			int newX = this.X, newY = this.Y;
			Movement.Movement.Offset( d, ref newX, ref newY );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			return ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map );
		}

		public override bool AllowItemUse( Item item )
		{
			return DesignContext.Check( this );
		}

		public SkillName[] AnimalFormRestrictedSkills{ get{ return m_AnimalFormRestrictedSkills; } }

		private SkillName[] m_AnimalFormRestrictedSkills = new SkillName[]
		{
			SkillName.Craftsmanship,	SkillName.Riding, SkillName.Throwing, SkillName.Forensics,
			SkillName.Inscribe, SkillName.Appraisal, SkillName.Meditation, SkillName.Peacemaking,
			SkillName.Dodge, SkillName.ArmDisarmTraps, SkillName.Linguistics, SkillName.Stealing,	
			SkillName.HerbalLore
		};

		public override bool AllowSkillUse( SkillName skill )
		{
			if ( AnimalForm.UnderTransformation( this ) )
			{
				for( int i = 0; i < m_AnimalFormRestrictedSkills.Length; i++ )
				{
					if( m_AnimalFormRestrictedSkills[i] == skill )
					{
						SendLocalizedMessage( 1070771 ); // You cannot use that skill in this form.
						return false;
					}
				}
			}

			return DesignContext.Check( this );
		}

		private bool m_LastProtectedMessage;
		private int m_NextProtectionCheck = 10;

		public virtual void RecheckTownProtection()
		{
			m_NextProtectionCheck = 10;

			Regions.GuardedRegion reg = (Regions.GuardedRegion) this.Region.GetRegion( typeof( Regions.GuardedRegion ) );
			bool isProtected = ( reg != null && !reg.IsDisabled() );

			if ( isProtected != m_LastProtectedMessage )
			{
				if ( isProtected )
					SendLocalizedMessage( 500112 ); // You are now under the protection of the town guards.
				else
					SendLocalizedMessage( 500113 ); // You have left the protection of the town guards.

				m_LastProtectedMessage = isProtected;
			}
		}

		public override void MoveToWorld( Point3D loc, Map map )
		{
			base.MoveToWorld( loc, map );

			RecheckTownProtection();
		}

		public override void SetLocation( Point3D loc, bool isTeleport )
		{
			if ( !isTeleport && AccessLevel == AccessLevel.Player )
			{
				// moving, not teleporting
				int zDrop = ( this.Location.Z - loc.Z );

				if ( zDrop > 20 ) // we fell more than one story
					Hits -= ((zDrop / 20) * 10) - 5; // deal some damage; does not kill, disrupt, etc
			}

			base.SetLocation( loc, isTeleport );

			if ( isTeleport || --m_NextProtectionCheck == 0 )
				RecheckTownProtection();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from == this )
			{
				if ( m_Quest != null )
					m_Quest.GetContextMenuEntries( list );

				if ( Alive && InsuranceEnabled )
				{
					list.Add( new CallbackEntry( 6201, new ContextCallback( ToggleItemInsurance ) ) );

					if ( AutoRenewInsurance )
						list.Add( new CallbackEntry( 6202, new ContextCallback( CancelRenewInventoryInsurance ) ) );
					else
						list.Add( new CallbackEntry( 6200, new ContextCallback( AutoRenewInventoryInsurance ) ) );
				}

				BaseHouse house = BaseHouse.FindHouseAt( this );

				if ( house != null )
				{
					if ( Alive && house.InternalizedVendors.Count > 0 && house.IsOwner( this ) )
						list.Add( new CallbackEntry( 6204, new ContextCallback( GetVendor ) ) );

					//if ( house.IsAosRules )
						//list.Add( new CallbackEntry( 6207, new ContextCallback( LeaveHouse ) ) );
				}

				/*if ( m_JusticeProtectors.Count > 0 )
					list.Add( new CallbackEntry( 6157, new ContextCallback( CancelProtection ) ) );

				if( Alive )
					list.Add( new CallbackEntry( 6210, new ContextCallback( ToggleChampionTitleDisplay ) ) );*/
			}
		}

		private void CancelProtection()
		{
			for ( int i = 0; i < m_JusticeProtectors.Count; ++i )
			{
				Mobile prot = m_JusticeProtectors[i];

				string args = String.Format( "{0}\t{1}", this.Name, prot.Name );

				prot.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
				this.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
			}

			m_JusticeProtectors.Clear();
		}

		#region Insurance

		private void ToggleItemInsurance()
		{
			if ( !CheckAlive() )
				return;

			BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
			SendLocalizedMessage( 1060868 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
		}

		private bool CanInsure( Item item )
		{
			if ( item is Container || item is BagOfSending || item is KeyRing )
				return false;

			if ( (item is Spellbook && item.LootType == LootType.Blessed)|| item is Runebook || item is PotionKeg || item is Sigil )
				return false;

			if ( item.Stackable )
				return false;

			if ( item.LootType == LootType.Cursed )
				return false;

			if ( item.ItemID == 0x204E ) // death shroud
				return false;

			return true;
		}

		private void ToggleItemInsurance_Callback( Mobile from, object obj )
		{
			if ( !CheckAlive() )
				return;

			Item item = obj as Item;

			if ( item == null || !item.IsChildOf( this ) )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060871, "", 0x23 ); // You can only insure items that you have equipped or that are in your backpack
			}
			else if ( item.Insured )
			{
				item.Insured = false;

				SendLocalizedMessage( 1060874, "", 0x35 ); // You cancel the insurance on the item

				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
			else if ( !CanInsure( item ) )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else if ( item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.BlessedFor == from )
			{
				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060870, "", 0x23 ); // That item is blessed and does not need to be insured
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else
			{
				if ( !item.PayedInsurance )
				{
					if ( Banker.Withdraw( from, 600 ) )
					{
						SendLocalizedMessage( 1060398, "600" ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						return;
					}
				}

				item.Insured = true;

				SendLocalizedMessage( 1060873, "", 0x23 ); // You have insured the item

				BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
		}

		private void AutoRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			SendLocalizedMessage( 1060881, "", 0x23 ); // You have selected to automatically reinsure all insured items upon death
			AutoRenewInsurance = true;
		}

		private void CancelRenewInventoryInsurance()
		{
			if ( !CheckAlive() )
				return;

			if( Core.SE )
			{
				if( !HasGump( typeof( CancelRenewInventoryInsuranceGump ) ) )
					SendGump( new CancelRenewInventoryInsuranceGump( this ) );
			}
			else
			{
				SendLocalizedMessage( 1061075, "", 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
				AutoRenewInsurance = false;
			}
		}

		private class CancelRenewInventoryInsuranceGump : Gump
		{
			private PlayerMobile m_Player;

			public CancelRenewInventoryInsuranceGump( PlayerMobile player ) : base( 250, 200 )
			{
				m_Player = player;

				AddBackground( 0, 0, 240, 142, 0x13BE );
				AddImageTiled( 6, 6, 228, 100, 0xA40 );
				AddImageTiled( 6, 116, 228, 20, 0xA40 );
				AddAlphaRegion( 6, 6, 228, 142 );

				AddHtmlLocalized( 8, 8, 228, 100, 1071021, 0x7FFF, false, false ); // You are about to disable inventory insurance auto-renewal.

				AddButton( 6, 116, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 118, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

				AddButton( 114, 116, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 148, 118, 450, 20, 1071022, 0x7FFF, false, false ); // DISABLE IT!
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( !m_Player.CheckAlive() )
					return;

				if ( info.ButtonID == 1 )
				{
					m_Player.SendLocalizedMessage( 1061075, "", 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
					m_Player.AutoRenewInsurance = false;
				}
				else
				{
					m_Player.SendLocalizedMessage( 1042021 ); // Cancelled.
				}
			}
		}
		#endregion

		private void GetVendor()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( CheckAlive() && house != null && house.IsOwner( this ) && house.InternalizedVendors.Count > 0 )
			{
				CloseGump( typeof( ReclaimVendorGump ) );
				SendGump( new ReclaimVendorGump( house ) );
			}
		}

		private void LeaveHouse()
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null )
				this.Location = house.BanLocation;
		}

		private delegate void ContextCallback();

		private class CallbackEntry : ContextMenuEntry
		{
			private ContextCallback m_Callback;

			public CallbackEntry( int number, ContextCallback callback ) : this( number, -1, callback )
			{
			}

			public CallbackEntry( int number, int range, ContextCallback callback ) : base( number, range )
			{
				m_Callback = callback;
			}

			public override void OnClick()
			{
				if ( m_Callback != null )
					m_Callback();
			}
		}

		public override void DisruptiveAction()
		{
			/*if( Meditating )
			{
				RemoveBuff( BuffIcon.ActiveMeditation );
			}*/

			base.DisruptiveAction();
		}
		public override void OnDoubleClick( Mobile from )
		{
			if ( this == from && !Warmode )
			{
				IMount mount = Mount;

                if( mount != null && !DesignContext.Check( this ) )
                {
                    return;
                }
			}

            if( this == from && ( !DisableDismountInWarmode || !Warmode ) )
            {
                IMount mount = Mount;

                if( mount != null )
                {
                    mount.Rider = null;

                    if( this.Trample )
                    {
                        this.Trample = false;
                        this.SendMessage( 60, "You are no longer attempting to trample your foes." );
                    }
                    return;
                }
            }

            if( CanPaperdollBeOpenedBy( from ) )
                DisplayPaperdollTo( from );
		}

		public override void DisplayPaperdollTo( Mobile to )
		{
			if ( DesignContext.Check( this ) )
				base.DisplayPaperdollTo( to );
		}

		private static bool m_NoRecursion;

		public override bool CheckEquip( Item item )
		{
			if( item == null || item.Deleted )
				return false;

            if( Forging || Reforging )
            {
                SendMessage( "You cannot equip anything while forging or reforging your character." );
                return false;
            }

            if( Claws != null && item is BaseWeapon )
            {
                SendMessage( "You need to remove your claws before equipping a weapon." );
                return false;
            }
			
			if ( !base.CheckEquip( item ) )
				return false;

            if ( item.Layer == Layer.FirstValid || item.Layer == Layer.TwoHanded )
            {
                Corpse corpse = this.Backpack.FindItemByType( typeof( Corpse ) ) as Corpse;

                if ( corpse != null && corpse.Owner != this )
                {
                    this.SendMessage( 60, "You cannot equip this, because you are carrying someone." );
                    return false;
                }
            }

            if (HealthAttachment.HasHealthAttachment(this))
            {
                if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenRightArm))
                {
                    if (item.Layer == Layer.FirstValid || item.Layer == Layer.OneHanded || (item.Layer == Layer.TwoHanded && item is BaseWeapon))
                        return false;
                }

                if (HealthAttachment.GetHA(this).HasInjury(Injury.BrokenLeftArm))
                {
                    if (item.Layer == Layer.TwoHanded)
                        return false;
                }
            }

			#region Factions
			FactionItem factionItem = FactionItem.Find( item );

			if ( factionItem != null )
			{
				Faction faction = Faction.Find( this );

				if ( faction == null )
				{
					SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
					return false;
				}
				else if ( faction != factionItem.Faction )
				{
					SendLocalizedMessage( 1010372 ); // You cannot equip an opposing faction's item!
					return false;
				}
				else
				{
					int maxWearables = FactionItem.GetMaxWearables( this );

					for ( int i = 0; i < Items.Count; ++i )
					{
						Item equiped = Items[i];

						if ( item != equiped && FactionItem.Find( equiped ) != null )
						{
							if ( --maxWearables == 0 )
							{
								SendLocalizedMessage( 1010373 ); // You do not have enough rank to equip more faction items!
								return false;
							}
						}
					}
				}
			}
			#endregion

			if ( this.AccessLevel < AccessLevel.GameMaster && item.Layer != Layer.Mount && this.HasTrade )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null )
				{
					if ( bounce.m_Parent is Item )
					{
						Item parent = (Item) bounce.m_Parent;

						if ( parent == this.Backpack || parent.IsChildOf( this.Backpack ) )
							return true;
					}
					else if ( bounce.m_Parent == this )
					{
						return true;
					}
				}

				SendLocalizedMessage( 1004042 ); // You can only equip what you are already carrying while you have a trade pending.
				return false;
			}

			return true;
		}

		public override bool CheckTrade( Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			int msgNum = 0;

			if ( cont == null )
			{
				if ( to.Holding != null )
					msgNum = 1062727; // You cannot trade with someone who is dragging something.
				else if ( this.HasTrade )
					msgNum = 1062781; // You are already trading with someone else!
				else if ( to.HasTrade )
					msgNum = 1062779; // That person is already involved in a trade
			}

			if ( msgNum == 0 )
			{
				if ( cont != null )
				{
					plusItems += cont.TotalItems;
					plusWeight += cont.TotalWeight;
				}

				if ( this.Backpack == null || !this.Backpack.CheckHold( this, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004040; // You would not be able to hold this if the trade failed.
				else if ( to.Backpack == null || !to.Backpack.CheckHold( to, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004039; // The recipient of this trade would not be able to carry this.
				else
					msgNum = CheckContentForTrade( item );
			}

			if ( msgNum != 0 )
			{
				if ( message )
					this.SendLocalizedMessage( msgNum );

				return false;
			}

			return true;
		}

		private static int CheckContentForTrade( Item item )
		{
			if ( item is TrapableContainer && ((TrapableContainer)item).TrapType != TrapType.None )
				return 1004044; // You may not trade trapped items.

			if ( SkillHandlers.StolenItem.IsStolen( item ) )
				return 1004043; // You may not trade recently stolen items.

			if ( item is Container )
			{
				foreach ( Item subItem in item.Items )
				{
					int msg = CheckContentForTrade( subItem );

					if ( msg != 0 )
						return msg;
				}
			}

			return 0;
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if( this != null && from != null && item != null && from is PlayerMobile )
			{
				PlayerMobile pm = from as PlayerMobile;

				if( pm != this && pm.AccessLevel < AccessLevel.GameMaster && !pm.Hidden && pm.Feats.GetFeatLevel(FeatList.PlantEvidence) > 0 && pm.Alive && this.Alive && pm.InLOS( this ) && pm.InRange( this.Location, 1 ) && pm.CanSee( this ) )
				{
					if( Utility.RandomMinMax( 1, 100 ) < ( pm.Feats.GetFeatLevel(FeatList.PlantEvidence) * 33 ) )
						return true;
					
					else
					{
						string notice = String.Format( "You notice {0} trying to drop something in {1}'s backpack.", pm.Name, this.Name );

						IPooledEnumerable eable = this.Map.GetClientsInRange( this.Location, 8 );

						foreach ( NetState ns in eable )
						{
							if ( ns != pm.NetState )
								ns.Mobile.SendMessage( notice );
						}

						eable.Free();
					}
				}
			}
			
			if ( !base.CheckNonlocalDrop( from, item, target ) )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			Container pack = this.Backpack;
			if ( from == this && this.HasTrade && ( target == pack || target.IsChildOf( pack ) ) )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null && bounce.m_Parent is Item )
				{
					Item parent = (Item) bounce.m_Parent;

					if ( parent == pack || parent.IsChildOf( pack ) )
						return true;
				}

				SendLocalizedMessage( 1004041 ); // You can't do that while you have a trade pending.
				return false;
			}

			return true;
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			CheckLightLevels( false );

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			int newX = this.X, newY = this.Y;
			int newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level, context.Foundation );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			if ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map )
			{
				if ( Z != newZ )
					Location = new Point3D( X, Y, newZ );

				m_NoRecursion = false;
				return;
			}

			Location = new Point3D( foundation.X, foundation.Y, newZ );
			Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is BaseCreature && !((BaseCreature)m).Controlled )
				return false;

			return base.OnMoveOver( m );
		}
		
		public bool HasFeatLevel( int feat, int level )
		{
			if( feat >= level )
				return true;
			
			this.SendMessage( "You lack the appropriate feat." );
			return false;
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

        public override bool CheckShove( Mobile shoved )
        {
            if( !shoved.Alive || !Alive || shoved.IsDeadBondedPet || IsDeadBondedPet )
                return true;
            else if( ( shoved.Hidden && shoved.AccessLevel > AccessLevel.Player ) || shoved is ISmallPredator || shoved is ISmallPrey )
                return true;

            int number = shoved.Hidden ? 1019043 : 1019042;

            if( !Pushing )
            {
            	if( this.Warmode && this.Mounted && this.Trample && !shoved.Mounted && !this.Blessed && !shoved.Blessed && !this.IsAllyOf(shoved) )
                {
                    Mobile mount = this.Mount as Mobile;

                    if( mount.Stam >= 10 )
                    {
                        mount.Stam -= 10;

                        if( shoved is IKhaerosMobile && ( (IKhaerosMobile)shoved ).Evaded() )
                            this.SendMessage( "You try to trample the target, but they evade the attack." );

                        else
                        {
                            shoved.PlaySound( shoved.GetHurtSound() );
                            AOS.Damage( shoved, this, ( this.Feats.GetFeatLevel( FeatList.Trample ) * 2 ), false, 0, 0, 0, 0, 0, 100, 0, 0 );
                            this.SendMessage( "You trample your enemy with your mount." );

                            if( shoved is BaseCreature )
                                ( (BaseCreature)shoved ).AggressiveAction( this, false );
                        }

                        RevealingAction();
                    }

                    else
                        return false;
                }

                else
                {
					if( this.Mounted )
               		{
                    	Mobile mount = this.Mount as Mobile;
                    	
                    	if( mount.Stam >= 10 )
						{
							mount.Stam -= 10;
							RevealingAction();
						}

                        else
                            return false;
					}
					 
					else if( Stam >= 10 )
					{
						Stam -= 10;
						RevealingAction();
					}
					
					else
						return false;
                }
            }
            
            if( shoved.Hidden )
            	shoved.RevealingAction();
            
            Pushing = true;
            SendLocalizedMessage( number );
            return true;
        }

		protected override void OnMapChange( Map oldMap )
		{
			if ( (Map != Faction.Facet && oldMap == Faction.Facet) || (Map == Faction.Facet && oldMap != Faction.Facet) )
				InvalidateProperties();

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			if ( Map != foundation.Map )
				Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			int disruptThreshold;

			if ( !Core.AOS )
				disruptThreshold = 0;
			else if ( from != null && from.Player )
				disruptThreshold = 18;
			else
				disruptThreshold = 25;

			if ( amount > disruptThreshold )
			{
				BandageContext c = BandageContext.GetContext( this );

				if ( c != null )
					c.Slip();
			}

			if( Confidence.IsRegenerating( this ) )
				Confidence.StopRegenerating( this );

			WeightOverloading.FatigueOnDamage( this, amount );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetDamaged( from, amount );
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceDamaged( from, amount );

            ArrayList list = XmlAttach.FindAttachments( this, typeof( XmlAwe ) );

            for( int i = 0; i < list.Count; ++i )
            {
                XmlAwe awe = list[i] as XmlAwe;
                awe.Delete();
            }

			base.OnDamage( amount, from, willKill );
		}
		
		public override void Kill()
		{
			if( IsVampire && AutoVampHeal && CanVampHeal )
			{
				BPs--;
				Hits += 25;
				NextVampHealAllowed = DateTime.Now + TimeSpan.FromSeconds( 2 );
				
				if( Hits > -1 )
				{
					Emote( "*{0} wounds start mending on their own*", this.GetPossessivePronoun() );
					return;
				}
			}
			
			base.Kill();
		}
		
		//the client sends a query upon login, so let's ignore that first one
		private DateTime m_LoginTime;
		
		public bool HadFirstSkillQuery{ get{ return (DateTime.Now > (m_LoginTime + TimeSpan.FromSeconds(3))); } }
		
		public override void OnSkillsQuery( Mobile from )
		{
			if( !HadFirstSkillQuery )
				return;
			
			else if( from == this )
				SendGump( new UniversalFeatsGump(this) );
			
			else
				base.OnSkillsQuery( from );
		}
		
		public override void CriminalAction( bool message )
		{
		}

		public override double RacialSkillBonus
		{
			get
			{
				if( Core.ML && this.Race == Race.Human )
					return 0.0;

				return 0;
			}
		}

		private Mobile m_InsuranceAward;
		private int m_InsuranceCost;
		private int m_InsuranceBonus;

		public override bool OnBeforeDeath()
		{
			m_InsuranceCost = 0;
			m_InsuranceAward = base.FindMostRecentDamager( false );

			if ( m_InsuranceAward is BaseCreature )
			{
				Mobile master = ((BaseCreature)m_InsuranceAward).GetMaster();

				if ( master != null )
					m_InsuranceAward = master;
			}

			if ( m_InsuranceAward != null && (!m_InsuranceAward.Player || m_InsuranceAward == this) )
				m_InsuranceAward = null;

			if ( m_InsuranceAward is PlayerMobile )
				((PlayerMobile)m_InsuranceAward).m_InsuranceBonus = 0;

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetKilled();
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceKilled();

			return base.OnBeforeDeath();
		}

		private bool CheckInsuranceOnDeath( Item item )
		{
			if ( InsuranceEnabled && item.Insured )
			{
				if ( AutoRenewInsurance )
				{
					int cost = ( m_InsuranceAward == null ? 600 : 300 );

					if ( Banker.Withdraw( this, cost ) )
					{
						m_InsuranceCost += cost;
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						item.PayedInsurance = false;
						item.Insured = false;
					}
				}
				else
				{
					item.PayedInsurance = false;
					item.Insured = false;
				}

				if ( m_InsuranceAward != null )
				{
					if ( Banker.Deposit( m_InsuranceAward, 300 ) )
					{
						if ( m_InsuranceAward is PlayerMobile )
							((PlayerMobile)m_InsuranceAward).m_InsuranceBonus += 300;
					}
				}

				return true;
			}

			return false;
		}

		public override DeathMoveResult GetParentMoveResultFor( Item item )
		{
            //if ( CheckInsuranceOnDeath( item ) )
            //    return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetParentMoveResultFor( item );

            //if ( res == DeathMoveResult.MoveToCorpse && item.Movable && this.Young )
            //    res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		public override DeathMoveResult GetInventoryMoveResultFor( Item item )
		{
            //if ( CheckInsuranceOnDeath( item ) )
            //    return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetInventoryMoveResultFor( item );

            //if ( res == DeathMoveResult.MoveToCorpse && item.Movable )
            //    res = DeathMoveResult.MoveToBackpack;

			return res;
		}
		
		public void HandleAwardsOnKill()
		{
			 if( Nation == Nation.Azhuran )
            	Mana += Feats.GetFeatLevel(FeatList.DivineConsecration);
		}
		
		public override void CheckStatTimers()
		{
			if( !Deserialized )
				return;
			
			base.CheckStatTimers();
		}

		public override void OnDeath( Container c )
		{
			//base.OnDeath( c );
			//mod to remove death shroud
			int sound = this.GetDeathSound();

			if ( sound >= 0 )
				Effects.PlaySound( this, this.Map, sound );

			if ( !Player )
			{
				Delete();
			}
			else
			{
                LastDeath = DateTime.Now;

				Send( DeathStatus.Instantiate( true ) );

				Warmode = false;

				BodyMod = 0;
				//Body = this.Female ? 0x193 : 0x192;
				Body = this.Race.GhostBody( this );

				Poison = null;
				Combatant = null;

				Hits = 0;
				Stam = 0;
				Mana = 0;

				EventSink.InvokePlayerDeath( new PlayerDeathEventArgs( this ) );

				ProcessDeltaQueue();

				Send( DeathStatus.Instantiate( false ) );

				CheckStatTimers();
			}
			//end mod to remove death shroud
			bool lostPoint = false;
            Mobile m = FindMostRecentDamager( false );

            if( LastKiller is BaseCreature )
            {
                BaseCreature bc = LastKiller as BaseCreature;
                
                if( bc is ILargePredator || bc is IMediumPredator || bc is ISmallPredator )
                	bc.Hunger += 20;

                if( bc.ControlMaster == null )
                {
                	LevelSystem.AwardExp( bc, Math.Min( bc.Int * 3, this.Level * 50 ) );
                }
                
                /*if( bc.TakesLifeOnKill )
				{
                	Lives--;
					lostPoint = true;
				}*/
            }

			HueMod = -1;
			NameMod = null;
			SavagePaintExpiration = TimeSpan.Zero;

			SetHairMods( -1, -1 );

			PolymorphSpell.StopTimer( this );
			IncognitoSpell.StopTimer( this );
			DisguiseGump.StopTimer( this );

			EndAction( typeof( PolymorphSpell ) );
			EndAction( typeof( IncognitoSpell ) );

			SkillHandlers.StolenItem.ReturnOnDeath( this, c );

			Mobile killer = this.FindMostRecentDamager( true );

			if ( killer is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)killer;

				Mobile master = bc.GetMaster();
				if( master != null )
					killer = master;
			}


			if( m_BuffTable != null )
			{
				List<BuffInfo> list = new List<BuffInfo>();

				foreach( BuffInfo buff in m_BuffTable.Values )
				{
					if( !buff.RetainThroughDeath )
					{
						list.Add( buff );
					}
				}

				for( int i = 0; i < list.Count; i++ )
				{
					RemoveBuff( list[i] );
				}
			}

            int offset = 0;

            if( killer is BaseCreature )
            {
                offset = killer.Fame / 1000;
            }

            if( killer is PlayerMobile && !lostPoint )
            {
                PlayerMobile pm = killer as PlayerMobile;
                offset = pm.Level;
                
                /*if( !pm.Spar )
				{
	                this.Lives--;
					lostPoint = true;
				}*/
                
                ArrayList list = new ArrayList();
                
                foreach( Mobile ally in pm.GetMobilesInRange( 10 ) )
                	list.Add( ally );

                for( int i = 0; i < list.Count; ++i )
                {
                	if( list[i] is PlayerMobile && pm.AllyList.Contains((Mobile)list[i]) )
                    	((PlayerMobile)list[i]).HandleAwardsOnKill();
                }
                            
                pm.HandleAwardsOnKill();
            }
			
			/*if ( !lostPoint )
			{
				PoisonAttachment attachment = XmlAttach.FindAttachment( this, typeof( PoisonAttachment ) ) as PoisonAttachment;
				if ( attachment != null && attachment.Poisoner is PlayerMobile )
				{
					this.Lives--;
					lostPoint = true;
				}
			}*/
            
			OnKilledBy( killer );

            int knockedouttime = 61 - this.Level + offset;

            if( knockedouttime < 15 )
            {
                knockedouttime = 15;
            }
            
            if( knockedouttime > 60 )
            {
                knockedouttime = 60;
            }
            
            DisguiseCommands.RemoveDisguise( this );
            
            if( Lives >= 0 )
            {
            	//this.SendMessage( 60, "You have been knocked out, but you are not dead. You will wake up shortly. Please, wait patiently." );
            	this.SendGump( new DeathGump( this, knockedouttime, c ) );
            }
            
            else
            {
            	this.Location = new Point3D( 5689, 1768, 0  );
            }
			
			CombatSystemAttachment.GetCSA( this ).OnDeath();
		}
		
		public virtual void OnKilledBy( Mobile mob )
		{
			BaseCombatManeuver.Cleave( mob, this );
			
			if( mob != null && mob.Weapon != null && mob.Weapon is BaseWeapon && !(mob.Weapon is Fists) )
				((BaseWeapon)mob.Weapon).XmlOnKilled( this, mob );
        }

		private List<Mobile> m_PermaFlags;
		private List<Mobile> m_VisList;
		private Hashtable m_AntiMacroTable;
		private TimeSpan m_GameTime;
		private TimeSpan m_ShortTermElapse;
		private TimeSpan m_LongTermElapse;
		private DateTime m_SessionStart;
		private DateTime m_LastEscortTime;
		private DateTime m_NextSmithBulkOrder;
		private DateTime m_NextTailorBulkOrder;
		private DateTime m_SavagePaintExpiration;
		private SkillName m_Learning = (SkillName)(-1);

		public SkillName Learning
		{
			get{ return m_Learning; }
			set{ m_Learning = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan SavagePaintExpiration
		{
			get
			{
				TimeSpan ts = m_SavagePaintExpiration - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				m_SavagePaintExpiration = DateTime.Now + value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSmithBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextSmithBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextSmithBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextTailorBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextTailorBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try{ m_NextTailorBulkOrder = DateTime.Now + value; }
				catch{}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastEscortTime
		{
			get{ return m_LastEscortTime; }
			set{ m_LastEscortTime = value; }
		}

		public PlayerMobile()
		{
			m_AllyList = new List<Mobile>();
			
			m_VisList = new List<Mobile>();
			m_PermaFlags = new List<Mobile>();
			m_AntiMacroTable = new Hashtable();

			m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			m_GameTime = TimeSpan.Zero;
			m_ShortTermElapse = TimeSpan.FromHours( 8.0 );
			m_LongTermElapse = TimeSpan.FromHours( 40.0 );

			m_JusticeProtectors = new List<Mobile>();
			m_GuildRank = Guilds.RankDefinition.Lowest;

			m_ChampionTitles = new ChampionTitleInfo();

            m_CrimesList = new Dictionary<Mobiles.Nation, int>();
            m_CrimesList.Add(Nation.Alyrian, 0);
            m_CrimesList.Add(Nation.Azhuran, 0);
            m_CrimesList.Add(Nation.Khemetar, 0);
            m_CrimesList.Add(Nation.Mhordul, 0);
            m_CrimesList.Add(Nation.Tyrean, 0);
            m_CrimesList.Add(Nation.Vhalurian, 0);
            m_CrimesList.Add(Nation.Imperial, 0);
            m_CrimesList.Add(Nation.Sovereign, 0);
            m_CrimesList.Add(Nation.Society, 0);
			m_CrimesList.Add(Nation.Insularii, 0);

			InvalidateMyRunUO();
		}

		public override bool MutateSpeech( List<Mobile> hears, ref string text, ref object context )
		{
			if ( Alive )
				return false;

			if ( Core.AOS )
			{
				for ( int i = 0; i < hears.Count; ++i )
				{
					Mobile m = hears[i];

					if ( m != this && m.Skills[SkillName.Linguistics].Value >= 100.0 )
						return false;
				}
			}

			return base.MutateSpeech( hears, ref text, ref context );
		}
		
		public bool IsTired( bool message )
		{
			bool isTired = IsTired();
			
			if( isTired && message )
				this.SendMessage( "You are too tired for that." );
			
			return isTired;
		}
		
		public bool IsTired()
		{
			if( this.m_KOPenalty != null )
				return true;
			
			return false;
		}

        private static List<Mobile> m_Hears;
        private static ArrayList m_OnSpeech;

        public static int GetLanguageHue( PlayerMobile m )
        {
        	switch( m.SpokenLanguage )
        	{
        		case KnownLanguage.Alyrian: return 482;
        		case KnownLanguage.Azhuran: return 1258;
        		case KnownLanguage.Khemetar: return 1;
        		case KnownLanguage.Mhordul: return 2964;
        		case KnownLanguage.Tyrean: return 2618;
        		case KnownLanguage.Vhalurian: return 138;
        	}
        	return 0;
        }
        
        public bool CanTeach( PlayerMobile mob )
        {
        	int level = Feats.GetFeatLevel(FeatList.Teaching) + Feats.GetFeatLevel(FeatList.Professor);
        	
        	if( mob.Level < 10 || (mob.Level < 15 && level == 2) || (mob.Level < 20 && level == 3) || (mob.Level < 30 && level == 4) ||
        	   (mob.Level < 40 && level == 5) || level == 6 )
        		return true;
        	
        	return false;
        }
        
		public override void DoSpeech( string text, int[] keywords, MessageType type, int hue )
		{
			if( type != MessageType.Emote )
				hue = GetLanguageHue( this );
			
			if( Guilds.Guild.NewGuildSystem && (type == MessageType.Guild || type == MessageType.Alliance) )
			{
				Guilds.Guild g = this.Guild as Guilds.Guild;
				if( g == null )
				{
					SendLocalizedMessage( 1063142 ); // You are not in a guild!
				}
				else if( type == MessageType.Alliance )
				{
					if( g.Alliance != null && g.Alliance.IsMember( g ) )
					{
						//g.Alliance.AllianceTextMessage( hue, "[Alliance][{0}]: {1}", this.Name, text );
						g.Alliance.AllianceChat( this, text );
						SendToStaffMessage( this, "[Alliance]: {0}", this.Name, text );

						m_AllianceMessageHue = hue;
					}
					else
					{
						SendLocalizedMessage( 1071020 ); // You are not in an alliance!
					}
				}
				else	//Type == MessageType.Guild
				{
					m_GuildMessageHue = hue;

					g.GuildChat( this, text );
					SendToStaffMessage( this, "[Guild]: {0}", text );
				}
			}
			else
			{
                if( Deleted || CommandSystem.Handle( this, text, type ) )
                    return;

                int range = 15;

                switch( type )
                {
                    case MessageType.Regular:
                        SpeechHue = hue;
                        break;
                    case MessageType.Emote:
                        EmoteHue = hue;
                        break;
                    case MessageType.Whisper:
                        WhisperHue = hue;
                        range = 1;
                        break;
                    case MessageType.Yell:
                        YellHue = hue;
                        range = 18;
                        break;
                    default:
                        type = MessageType.Regular;
                        break;
                }

                SpeechEventArgs regArgs = new SpeechEventArgs( this, text, type, hue, keywords );

                EventSink.InvokeSpeech( regArgs );
                this.Region.OnSpeech( regArgs );

                if( Squelched && type == MessageType.Emote )
                    regArgs.Blocked = true;

                    OnSaid( regArgs );

                if( regArgs.Blocked )
                    if ( type != MessageType.Emote )
                    return;

                text = regArgs.Speech;

                if( text == null || text.Length == 0 )
                    return;

                if( m_Hears == null )
                    m_Hears = new List<Mobile>();
                else if( m_Hears.Count > 0 )
                    m_Hears.Clear();

                if( m_OnSpeech == null )
                    m_OnSpeech = new ArrayList();
                else if( m_OnSpeech.Count > 0 )
                    m_OnSpeech.Clear();

                List<Mobile> hears = m_Hears;
                ArrayList onSpeech = m_OnSpeech;
                
                string newtext = text.ToLower();
                bool ignore = false;
                                	
            	if( newtext == "all come" || newtext == "all kill" || 
            	   newtext == "all report" || newtext == "all attack" || 
            	   newtext == "all stay" || newtext == "all follow me" || 
            	   newtext == "all guard me" || newtext == "all stop" || 
            	   newtext == "all guard" || newtext == "all follow" || 
            	   newtext == "all release" || newtext == "all obey" ||
            	   newtext == "i wish to release this" || newtext == "i wish to release this." || 
            	   newtext == "i wish to lock this down" || newtext == "i wish to lock this down." || newtext == "bank" )
            	{
            		ignore = true;
            	}

                if( Map != null )
                {
                    IPooledEnumerable eable = Map.GetObjectsInRange( Location, range );
                    
                    string teaching = "";
                    
                    if( this.m_Teaching )
                    	teaching = "[Teaching]";
                    
                    if( !ignore || teaching != "" )
                    {
                        if( newtext != "turn left" && newtext != "turn right" && newtext != "turn about" && newtext != "forward" && newtext != "back" &&
                            newtext != "come about" && newtext != "forward left" && newtext != "forward right" && newtext != "back left" && newtext != "drop anchor" &&
                            newtext != "raise anchor" && newtext != "stop" && newtext != "back right" && newtext != "right" && newtext != "left" )
                        {
                            foreach( NetState state in NetState.Instances )
                            {
                                Mobile staffer = state.Mobile;

                                if( staffer != null && staffer.AccessLevel > AccessLevel.Player && staffer is PlayerMobile && ( ( (PlayerMobile)staffer ).HearAll == 1 || ( (PlayerMobile)staffer ).HearAll == 3 ) )
                                    staffer.SendMessage( hue, "" + teaching + this.Name + ": " + text );
                            }
                        }
                    }
                    
                    this.m_Teaching = false;

                    foreach( object o in eable )
                    {
                        if( o is Mobile )
                        {
                            Mobile heard = (Mobile)o;

                            if( heard.CanSee( this ) && ( NoSpeechLOS || !heard.Player || heard.InLOS( this ) ) )
                            {
                                if( heard is PlayerMobile )
                                {
                                	if( ignore )
                                		continue;
                                	
                                	if( this.SpokenLanguage == KnownLanguage.Shorthand && type != MessageType.Emote )
                                	{
                                		if( !this.InRange( heard.Location, ( this.Feats.GetFeatLevel(FeatList.Shorthand) * 4 ) ) || ( (PlayerMobile)heard ).Feats.GetFeatLevel(FeatList.Shorthand) < 1 )
                                			continue;
                                		
                                		else
                                		{
                                			heard.SendMessage( hue, "[Shorthand]" + this.Name + ": " + text );
                                			continue;
                                		}
                                	}
                                	    
                                    if( ( (PlayerMobile)heard ).DeafnessTimer != null && type != MessageType.Emote )
                                    {
                                        continue;
                                    }
                                    
                                    if( ( (PlayerMobile)heard ).GetBackgroundLevel(BackgroundList.Deaf) > 0 && type != MessageType.Emote && heard.Serial != this.Serial )
                                    {
                                        continue;
                                    }
                                    
                                    if( type != MessageType.Emote && (this.GetBackgroundLevel(BackgroundList.Mute) > 0 || this.CannotSpeak) )
                                    {
                                        continue;
                                    }
   
                                    if( this.SpokenLanguage != KnownLanguage.Shorthand && !KnowsLanguage( ( (PlayerMobile)heard ), this.SpokenLanguage ) && heard.Serial != this.Serial && type != MessageType.Emote )
                                	{
                                    	Packet p = null;
                                    	
                  						if (p == null)
		                        		{
                  							p = new AsciiMessage( this.Serial, this.Body, type, hue, 8, this.Name, RandomString( text.Length, true ) );
		                         	    	p.Acquire();
                  						
	                  						if( heard.NetState != null )
	                  						{
	                                    		heard.NetState.Send(p);
	                                    		Packet.Release(p);
	                  						}
                  						}
                                    }

	                                else
	                                {
		                                if( heard.NetState != null )
		                                {
		                                	PlayerMobile student = heard as PlayerMobile;
		
		                                	if( this.m_Students != null && this.m_Students.Contains( student ) && student.m_WantsTeaching && DateTime.Compare( DateTime.Now, ( student.m_LastTeaching + TimeSpan.FromSeconds( 30 ) ) ) > 0 )
		                                	{
		                                		if( !CanTeach(student) )
		                                		{
		                                			student.SendMessage( 60, "You cannot learn anything else from your teacher." );
		                                			this.SendMessage( 60, "You cannot teach anything else to your student." );
		                                			this.m_Students.Remove( student );
		                                			student.m_WantsTeaching = false;
		                                		}
		                                		
		                                		else
		                                		{
			                                		student.m_LastTeaching = DateTime.Now;
			                                		this.m_LastTeaching = DateTime.Now;
			                                		
			                                		int exp = 200;
			                                		
			                                		if( student.Level > 4 )
			                                			exp = 225;
			                                		
			                                		if( student.Level > 9 )
			                                			exp = 250;
			                                		
			                                		if( student.Level > 14 )
			                                			exp = 275;
			                                		
			                                		if( student.Level > 19 )
			                                			exp = 300;
			                                		
			                                		if( student.Level > 24 )
			                                			exp = 325;
			                                		
			                                		if( student.Level > 29 )
			                                			exp = 350;
			                           
			                                		if( student.Level > 49 )
			                                			exp = 8;			                           
			                                		
			                                		student.XPFromLearning = true;
			                                		this.XPFromLearning = true;
			                                		LevelSystem.AwardExp( student, exp );
			                                		LevelSystem.AwardCP( student, exp / 4 );
			                                		LevelSystem.AwardExp( this, exp / 2 );
			                                		LevelSystem.AwardCP( this, exp / 8 );
			                                		student.XPFromLearning = false;
			                                		this.XPFromLearning = false;
			                                		this.m_Teaching = true;
		                                		}
		                                	}
		                            
		                                    hears.Add( heard );
		                                }
		
		                                if( heard.HandlesOnSpeech( this ) )
		                                    onSpeech.Add( heard );
		
		                                for( int i = 0; i < heard.Items.Count; ++i )
		                                {
		                                    Item item = heard.Items[i];
		
		                                    if( item.HandlesOnSpeech )
		                                        onSpeech.Add( item );
		
		                                    if( item is Container )
		                                        base.AddSpeechItemsFrom( onSpeech, (Container)item );
		                                }
	                                }
                                }
                                
                                else
                                {
	                                if( heard.NetState != null )
	                                    hears.Add( heard );
	
	                                if( heard.HandlesOnSpeech( this ) )
	                                    onSpeech.Add( heard );
	
	                                for( int i = 0; i < heard.Items.Count; ++i )
	                                {
	                                    Item item = heard.Items[i];
	
	                                    if( item.HandlesOnSpeech )
	                                        onSpeech.Add( item );
	
	                                    if( item is Container )
	                                        base.AddSpeechItemsFrom( onSpeech, (Container)item );
	                                }
                                }
                            }
                        }
                        else if( o is Item )
                        {
                            if( ( (Item)o ).HandlesOnSpeech )
                                onSpeech.Add( o );

                            if( o is Container )
                                base.AddSpeechItemsFrom( onSpeech, (Container)o );
                        }
                    }

                    eable.Free();

                    object mutateContext = null;
                    string mutatedText = text;
                    SpeechEventArgs mutatedArgs = null;

                    if( MutateSpeech( hears, ref mutatedText, ref mutateContext ) )
                        mutatedArgs = new SpeechEventArgs( this, mutatedText, type, hue, new int[0] );

                    CheckSpeechManifest();

                    ProcessDelta();

                    Packet regp = null;
                    Packet mutp = null;

                    for( int i = 0; i < hears.Count; ++i )
                    {
                        Mobile heard = hears[i];

                        if( mutatedArgs == null || !CheckHearsMutatedSpeech( heard, mutateContext ) )
                        {
                            heard.OnSpeech( regArgs );

                            NetState ns = heard.NetState;

                            if( ns != null )
                            {
                                if( regp == null )
                                    regp = Packet.Acquire( new UnicodeMessage( Serial, Body, type, hue, 3, Language, Name, text ) );

                                ns.Send( regp );
                            }
                        }
                        else
                        {
                            heard.OnSpeech( mutatedArgs );

                            NetState ns = heard.NetState;

                            if( ns != null )
                            {
                                if( mutp == null )
                                    mutp = Packet.Acquire( new UnicodeMessage( Serial, Body, type, hue, 3, Language, Name, mutatedText ) );

                                ns.Send( mutp );
                            }
                        }
                    }

                    Packet.Release( regp );
                    Packet.Release( mutp );

                    if( onSpeech.Count > 1 )
                        onSpeech.Sort( LocationComparer.GetInstance( this ) );

                    for( int i = 0; i < onSpeech.Count; ++i )
                    {
                        object obj = onSpeech[i];

                        if( obj is Mobile )
                        {
                            Mobile heard = (Mobile)obj;

                            if( mutatedArgs == null || !CheckHearsMutatedSpeech( heard, mutateContext ) )
                                heard.OnSpeech( regArgs );
                            else
                                heard.OnSpeech( mutatedArgs );
                        }
                        else
                        {
                            Item item = (Item)obj;

                            item.OnSpeech( regArgs );
                        }
                    }
                }
			}
		}
		
		public override void OnSaid( SpeechEventArgs e )
		{
			if ( Squelched )
			{
				if( Core.ML )
					this.SendLocalizedMessage( 500168 ); // You can not say anything, you have been muted.
				else
					this.SendMessage( "You can not say anything, you have been squelched." ); //Cliloc ITSELF changed during ML.

				e.Blocked = true;
			}

			if ( !e.Blocked && this.SpokenLanguage != KnownLanguage.Shorthand )
				RevealingAction();
		}
		
		public int GetTotalCopper()
		{
			int totalcopper = 0;
			
			if ( Backpack != null )
			    foreach( Item item in Backpack.Items )
			    {
				    if( item is Copper )
				    {
					    totalcopper += item.Amount;
				    }
			    }
			
			return totalcopper;
		}
		
		public static bool KnowsLanguage( PlayerMobile m, KnownLanguage language )
		{
			if( language == KnownLanguage.Common )
				return true;
			
			int feat = 0;
			int knowledge = m.Feats.GetFeatLevel(FeatList.Linguistics) * 5;
			
			switch( language )
			{
				case KnownLanguage.Alyrian: feat = m.Feats.GetFeatLevel(FeatList.AlyrianLanguage); break;
				case KnownLanguage.Azhuran: feat = m.Feats.GetFeatLevel(FeatList.AzhuranLanguage); break;
				case KnownLanguage.Khemetar: feat = m.Feats.GetFeatLevel(FeatList.KhemetarLanguage); break;
				case KnownLanguage.Mhordul: feat = m.Feats.GetFeatLevel(FeatList.MhordulLanguage); break;
				case KnownLanguage.Tyrean: feat = m.Feats.GetFeatLevel(FeatList.TyreanLanguage); break;
				case KnownLanguage.Vhalurian: feat = m.Feats.GetFeatLevel(FeatList.VhalurianLanguage); break;
			}
			
			if( feat == 1 )
				knowledge += 10;
			
			else if( feat == 2 )
				knowledge += 35;
			
			else if( feat == 3 )
				return true;
			
			if( Utility.RandomMinMax( 1, 100 ) <= knowledge )
				return true;
			
			return false;
		}
		
		public Nation GetDisguisedNation()
		{
			Nation nation = this.Nation;

            if (RPTitle != null)
            {
                if (RPTitle.Contains("of the South") && PlayerMobile.KnowsLanguage(this, KnownLanguage.Mhordul))
                    nation = Nation.Alyrian;

                else if (RPTitle.Contains("of the West") && PlayerMobile.KnowsLanguage(this, KnownLanguage.Azhuran))
                    nation = Nation.Azhuran;

                else if (RPTitle.Contains("the Khemetar") && PlayerMobile.KnowsLanguage(this, KnownLanguage.Khemetar))
                    nation = Nation.Khemetar;

                else if (RPTitle.Contains("the Tyrean") && PlayerMobile.KnowsLanguage(this, KnownLanguage.Tyrean))
                    nation = Nation.Tyrean;

                else if (RPTitle.Contains("of the North") && PlayerMobile.KnowsLanguage(this, KnownLanguage.Vhalurian))
                    nation = Nation.Vhalurian;
                    
                  
            }
			
			return nation;
		}
		
		private string RandomString( int size, bool upperCase )
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			char ch ;
			
			for(int i=0; i<size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) ;
				builder.Append(ch);
			}
		
			if( upperCase )
				return builder.ToString().ToUpper();
		
			return builder.ToString();
		}

		private static void SendToStaffMessage( Mobile from, string text )
		{
			Packet p = null;

			foreach( NetState ns in from.GetClientsInRange( 8 ) )
			{
				Mobile mob = ns.Mobile;

				if( mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel )
				{
					if( p == null )
						p = Packet.Acquire( new UnicodeMessage( from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, text ) );

					ns.Send( p );
				}
			}

			Packet.Release( p );
		}
		private static void SendToStaffMessage( Mobile from, string format, params object[] args )
		{
			SendToStaffMessage( from, String.Format( format, args ) );
		}

		public override void Damage( int amount, Mobile from )
		{
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
            //        ShieldingMobile.Damage( dmgshield, from );
					
            //        if( !ShieldingMobile.Mounted )
            //            ShieldingMobile.Animate( 20, 5, 1, true, false, 0 );
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
			    base.Damage( amount, from );
		}

		#region Poison
		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive )
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
			if ( this.Young )
				return true;

			return base.CheckPoisonImmunity( from, poison );
		}

		public override void OnPoisonImmunity( Mobile from, Poison poison )
		{
			if ( this.Young )
				SendLocalizedMessage( 502808 ); // You would have been poisoned, were you not new to the land of Britannia. Be careful in the future.
			else
				base.OnPoisonImmunity( from, poison );
		}
		#endregion

		public PlayerMobile( Serial s ) : base( s )
		{
			m_VisList = new List<Mobile>();
			m_AntiMacroTable = new Hashtable();
			InvalidateMyRunUO();
		}

		public List<Mobile> VisibilityList
		{
			get{ return m_VisList; }
		}
		
		private List<Mobile> m_ForcedVisibilityList = new List<Mobile>();

		public List<Mobile> PermaFlags
		{
			get{ return m_PermaFlags; }
		}

		public override int Luck{ get{ return AosAttributes.GetValue( this, AosAttribute.Luck ); } }

		public override bool IsHarmfulCriminal( Mobile target )
		{
//			if ( SkillHandlers.Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).m_PermaFlags.Count > 0 )
//			{
//				int noto = Notoriety.Compute( this, target );
//
//				if ( noto == Notoriety.Innocent )
//					target.Delta( MobileDelta.Noto );
//
//				return false;
//			}
//
//			if ( target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled )
//				return false;
//
//			return base.IsHarmfulCriminal( target );
			
			return true;
		}

		public bool AntiMacroCheck( Skill skill, object obj )
		{
			if ( obj == null || m_AntiMacroTable == null || this.AccessLevel != AccessLevel.Player )
				return true;

			Hashtable tbl = (Hashtable)m_AntiMacroTable[skill];
			if ( tbl == null )
				m_AntiMacroTable[skill] = tbl = new Hashtable();

			CountAndTimeStamp count = (CountAndTimeStamp)tbl[obj];
			if ( count != null )
			{
				if ( count.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now )
				{
					count.Count = 1;
					return true;
				}
				else
				{
					++count.Count;
					if ( count.Count <= SkillCheck.Allowance )
						return true;
					else
						return false;
				}
			}
			else
			{
				tbl[obj] = count = new CountAndTimeStamp();
				count.Count = 1;

				return true;
			}
		}

		private void RevertHair()
		{
			SetHairMods( -1, -1 );
		}

		private Engines.BulkOrders.BOBFilter m_BOBFilter;

		public Engines.BulkOrders.BOBFilter BOBFilter
		{
			get{ return m_BOBFilter; }
		}
		
		[CommandProperty( AccessLevel.Owner )]
		public bool FixSkillValues
		{
			get{ return false; }
			set
			{
				if( value == false )
					return;
				
				foreach( KeyValuePair<FeatList, FeatInfo.BaseFeat> kvp in Feats.FeatDictionary )
				{
					FeatInfo.BaseFeat feat = (FeatInfo.BaseFeat)kvp.Value;
					
					for( int i = 0; i < feat.AssociatedSkills.Length; i++ )
						Skills[feat.AssociatedSkills[i]].Base = feat.SkillLevel;
				}
			}
		}
		
		[CommandProperty( AccessLevel.Owner )]
		public bool FixFeatAddons
		{
			get{ return false; }
			set
			{
				if( value == false )
					return;
				
				foreach( KeyValuePair<FeatList, FeatInfo.BaseFeat> kvp in Feats.FeatDictionary )
				{
					FeatInfo.BaseFeat feat = (FeatInfo.BaseFeat)kvp.Value;
					feat.FixAddOns( this );
				}
			}
		}
		
		public void SendItemsToBankBox()
		{
			Container pack = Backpack;
			Container bank = BankBox;
			Bag bag = new Bag();
			
			if( BankBox == null )
				AddItem( new PlayerBankBox(this) );
			
			ArrayList list = new ArrayList();
			
			foreach ( Item item in pack.Items )
                list.Add( item );
			
			for( int i = 0; i < list.Count; i++ )
				bag.DropItem( (Item)list[i] );
			
			bank.DropItem( bag );
			
			EmptyBankBoxOn = DateTime.Now + TimeSpan.FromDays( 7 );
			SendMessage( "Your old items are in your bank box. You have one week to retrieve them from there. After that " +
							                 "time, they will be deleted." );
		}
		
		public void Reforge()
		{
            XmlBackground.CleanUp(this);
            XmlAosAttribute.CleanUp(this);
            XmlAddiction.CleanUp(this);

			Reforging = true;
			ReforgeMap = Map;
			ReforgeLocation = new Point3D( X, Y, Z );
			Map = Map.Felucca;
			Location = new Point3D( 5449, 1238, -95 );
            DisguiseCommands.RemoveDisguise( this );

			if( !OldMapChar && Account != null && !Account.AcceptedNames.Contains(Name) )
				Account.AcceptedNames.Add( Name );

			Container pack = Backpack;
			
			if( pack == null || pack.Deleted )
			{
				pack = new ArmourBackpack();
                pack.Movable = false;
				AddItem( pack );
			}
			
			ArrayList list = new ArrayList();
			
			foreach ( Item item in Items )
                list.Add( item );
			
			for( int i = 0; i < list.Count; i++ )
			{
				Item item = list[i] as Item;
				
				if( (item is BaseArmor && !(item is ArmourBackpack)) || item is BaseClothing || item is BaseWeapon || item is SheathedItem ||
				  item is BaseEquipableLight || item is BaseTool || item is BaseJewel )
					pack.AddItem( item );
			}
			
			if( OldMapChar )
			{
				if( Advanced != Advanced.None )
					ExtraCPRewards = 40000;
				
				if( Subclass == Subclass.Thief || Advanced == Advanced.Thief )
					CanBeThief = true;
				
				if( Class == Class.Mage )
					CanBeMage = true;
			}

			LevelSystem.WipeAllTraits( this );
			
			if( OldMapChar )
				SendItemsToBankBox();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 161:
                    this.NumberOfItemsCookedRecently = reader.ReadInt();
                    this.CookingXpLastAwardedOn = reader.ReadDateTime();
                    goto case 160;
                case 160:
                    this.lastSecondWind = reader.ReadDateTime();
                    goto case 159;
                case 159:
                    {
                        this.lastCharge = reader.ReadDateTime();
                        this.chargeCooldown = reader.ReadInt();
                        goto case 158;
                    }
                case 158: SmithTesting = reader.ReadBool(); goto case 157;
                case 157: m_ConsecratedItems = reader.ReadInt(); goto case 156;
                case 156: m_CanBeFaithful = reader.ReadBool();goto case 155;
				case 155: m_HCWound = reader.ReadDateTime(); goto case 154;
				case 154: m_Maimings = reader.ReadInt(); goto case 153;
                
				case 153: 
				{
						m_CustomAvatarID1 = reader.ReadInt();
						m_CustomAvatarID2 = reader.ReadInt();
						m_CustomAvatarID3 = reader.ReadInt();
						m_IsHardcore = reader.ReadBool(); goto case 152;
						
				}
				
				case 152: GroupInfo.Deserialize(reader, Group); goto case 151;
				case 151: m_IsApprentice = reader.ReadBool(); goto case 150;
				case 150: m_AvatarID = reader.ReadInt(); goto case 149;
                case 149:
                case 148:
                case 147:
                case 146:
                case 145:
                case 144:
                {
                    Disguise.Deserialize( reader );
                    MyDisguises.Deserialize( reader ); 
                    goto case 143;
                }
				case 143: m_GemHarvesting = reader.ReadBool(); goto case 142;
				case 142: 
				{
					m_CanBeThief = reader.ReadBool();
					m_LastCottonFlaxHarvest = reader.ReadDateTime(); 
					goto case 141;
				}
				case 141:
				case 140: m_CraftingSpecialization = reader.ReadString(); goto case 139;
				case 139: m_Forging = reader.ReadBool(); goto case 138;
				case 138: m_ImperialGuard = reader.ReadBool(); goto case 137;
				case 137: m_ExtraCPRewards = reader.ReadInt(); goto case 136;
				case 136: m_EmptyBankBoxOn = reader.ReadDateTime(); goto case 135;
				case 135: m_OldMapChar = reader.ReadBool(); goto case 134;
				case 134:
				{
					int guilds = reader.ReadInt();
					
					for( int i = 0; i < guilds; i++ )
					{
						CustomGuildInfo guild = new CustomGuildInfo( reader );
						
						if( guild != null && guild.GuildStone != null )
							CustomGuilds.Add( guild.GuildStone, guild );
					}
					
					goto case 133;
				}
				case 133: m_ChosenDeity = (ChosenDeity)reader.ReadInt(); goto case 132;
				case 132: m_WikiConfig = reader.ReadString(); goto case 131;
				case 131:
				case 130:
				case 129: 
				{
					m_ReforgeLocation = reader.ReadPoint3D();
					m_ReforgeMap = reader.ReadMap();
					m_Reforging = reader.ReadBool();
					m_ReforgesLeft = reader.ReadInt(); 
					goto case 128;
				}
				case 128:
				case 127:
				{
					m_Technique = reader.ReadString();
					m_TechniqueLevel = reader.ReadInt();
					m_MediumPieces = reader.ReadInt();
					m_HeavyPieces = reader.ReadInt();
					goto case 126;
				}
				case 126: m_VampSight = reader.ReadBool(); goto case 125;
				case 125: m_FreeForRP = reader.ReadBool(); goto case 124;
				case 124: m_VampSafety = reader.ReadBool(); goto case 123;
				case 123:
				{
					m_LastTimeGhouled = reader.ReadDateTime();
					m_YearOfDeath = reader.ReadInt();
					m_MonthOfDeath = reader.ReadInt();
					m_DayOfDeath = reader.ReadInt();
					m_IsVampire = reader.ReadBool(); 
					m_MaxBPs = reader.ReadInt();
					m_BPs = reader.ReadInt();
					m_AutoVampHeal = reader.ReadBool();
					
					goto case 122;
				}
				case 122:
				case 121: m_LogMsgs = reader.ReadBool(); goto case 120;
				case 120: m_PureDodge = reader.ReadBool(); goto case 119;
				case 119: m_Tents = reader.ReadStrongItemList(); goto case 118;
				case 118: m_CraftContainer = (Container)reader.ReadItem(); goto case 117;
				case 117:
				case 116: m_SpellBook = (CustomSpellBook)reader.ReadItem(); goto case 115;
				case 115:
				case 114:
				{
					CustomGuildInfo test = null;
					
					if( version < 134 )
						test = new CustomGuildInfo( reader ); 
					
					goto case 113;
				}
				case 113:
				case 112:
				case 111:
				case 110:
				case 109:
				{
					m_CurrentCommand = (SongList)reader.ReadInt();
					goto case 108;
				}
				case 108:
				{
					m_SpeedHack = reader.ReadBool();
					goto case 107;
				}
				case 107:
				{
					m_LogoutTime = reader.ReadDateTime();
					goto case 106;
				}
				case 106:
				{
					m_FakeAge = reader.ReadString();
					m_FakeLooks = reader.ReadString();
					goto case 105;
				}
				case 105:
				{
					this.NameMod = reader.ReadString();
					m_AutoPicking = reader.ReadBool();
					goto case 104;
				}
				case 104: goto case 103;
				case 103:
				{
					m_SmoothPicking = reader.ReadBool();
					goto case 102;
				}
				case 102:
				{
					m_HasStash = reader.ReadBool();
					goto case 101;
				}
				case 101:
				{
					m_FakeHair = reader.ReadInt();
					m_FakeHairHue = reader.ReadInt();
					m_FakeFacialHair = reader.ReadInt();
					m_FakeFacialHairHue = reader.ReadInt();
					m_FakeHue = reader.ReadInt();
					m_FakeRPTitle = reader.ReadString();
					m_FakeTitlePrefix = reader.ReadString();
					goto case 100;
				}
				case 100: goto case 99;
				case 99: goto case 98;
				case 98: goto case 97;
				case 97: goto case 96;
				case 96:
				{
					m_FixedRun = reader.ReadBool();
					goto case 95;
				}
				case 95:
				{
					m_LightPieces = reader.ReadInt();
					goto case 94;
				}
				case 94:
				{
					m_ArmourPieces = reader.ReadInt();
					goto case 93;
				}
				case 93:
				{
					m_LightPenalty = reader.ReadInt();
					m_MediumPenalty = reader.ReadInt();
					m_HeavyPenalty = reader.ReadInt();
					goto case 92;
				}
				case 92: goto case 91;
				case 91:
				{
					m_FixedRage = reader.ReadBool();
					goto case 90;
				}
					
				case 90: goto case 89;
				case 89: goto case 88;
				case 88: goto case 87;
				case 87: goto case 86;
				
				case 86:
				{
					m_FixedReflexes = reader.ReadBool();
					goto case 85;
				}
				case 85:
				{
					m_FixedStyles = reader.ReadBool();
					goto case 84;
				}
				case 84:
				{
					m_NextMending = reader.ReadDateTime();
					goto case 83;
				}
				case 83:
				{
					m_FixedStatPoints = reader.ReadBool();
					goto case 82;
				}
				case 82:
				{
					m_XPFromCrafting = reader.ReadBool();
					m_XPFromKilling = reader.ReadBool();
					goto case 81;
				}
				case 81:
				{
					m_HarvestedCrops = reader.ReadInt();
					m_NextHarvestAllowed = reader.ReadDateTime();
					goto case 80;
				}
				case 80:
				{
					m_Spar = reader.ReadBool();
					goto case 79;
				}
				case 79:
				{
					m_HideStatus = reader.ReadBool();
					goto case 78;
				}
				case 78:
				{
					m_DayOfBirth = reader.ReadString();
					m_MonthOfBirth = reader.ReadString();
					m_YearOfBirth = reader.ReadString();
					goto case 77;
				}
				case 77:
				{
					m_AlyrianGuard = reader.ReadBool();
					m_AzhuranGuard = reader.ReadBool();
					m_KhemetarGuard = reader.ReadBool();
					m_MhordulGuard = reader.ReadBool();
					m_TyreanGuard = reader.ReadBool();
					m_VhalurianGuard = reader.ReadBool();
					goto case 76;
				}
				case 76:
				{
					m_HearAll = reader.ReadInt();
					goto case 75;
				}
				case 75:
				{
					m_Friendship = new Friendship( reader );
					goto case 74;
				}
				case 74:
				{
					m_CPCapOffset = reader.ReadInt();
					m_CPSpent = reader.ReadInt();
					goto case 73;
				}
				case 73:
				{
					m_Description2 = reader.ReadString();
					m_Description3 = reader.ReadString();
					goto case 72;
				}
				case 72:
				{
					m_NextBirthday = reader.ReadDateTime();
					m_MaxAge = reader.ReadInt();
					goto case 71;
				}
				case 71:
				{
					m_Age = reader.ReadInt();
					goto case 70;
				}
				case 70:
				{
					m_LoggedOutPets = reader.ReadStrongMobileList();
					goto case 69;
				}
				case 69:
				{
					m_RecreateXP = reader.ReadInt();
					m_RecreateCP = reader.ReadInt();
					goto case 68;
				}
				case 68:
				{
					m_LastOffenseToNature = reader.ReadDateTime();
					goto case 66;
				}
					
				case 66:
				{
					Mobile mob = null;
					
					if( version < 92 )
						mob = reader.ReadMobile();
					
					goto case 65;
				}
					
				case 65:
				{
					m_LastDonationLife = reader.ReadDateTime();
					goto case 64;
				}
					
				case 64:
				{
					m_Lives = reader.ReadInt();
					goto case 63;
				}
					
				case 63:
				{
					m_AllyList = reader.ReadStrongMobileList();
					goto case 62;
				}
					
				case 62:
				{
					m_Height = reader.ReadInt();
					m_Weight = reader.ReadInt();
					goto case 61;
				}
					
				case 61:
				{
					m_NextAllowance = reader.ReadDateTime();
					goto case 60;
				}
					
				case 60:
				{
					m_Backgrounds = new Backgrounds( reader );
                    goto case 59;
				}
					
				case 59:
				{
					m_Description = reader.ReadString();
                    goto case 58;
				}
					
				case 58:
				{
					m_Masterwork = new Masterwork( reader );
                    goto case 57;
				}
					
				case 57:
				{
					m_RacialResources = new RacialResources( reader );
                    goto case 56;
				}
					
				case 56:
				{
					m_KnownLanguages = new KnownLanguages( reader );
                    goto case 55;
				}
				
				case 55:
				{
					m_SpokenLanguage = (KnownLanguage)reader.ReadInt();
                    goto case 54;
				}
					
				case 54:
				{
					m_RPTitle = reader.ReadString();
                    m_TitlePrefix = reader.ReadString();
                    goto case 53;
				}
					
                case 53:
                {
                    m_FocusedShot = reader.ReadInt();
                    m_SwiftShot = reader.ReadInt();
                    goto case 52;
                }

                case 52:
                {
                    m_WeaponSpecialization = reader.ReadString();
                    m_SecondSpecialization = reader.ReadString();
                    goto case 51;
                }

                case 51:
                {
                    m_CombatStyles = new CombatStyles( reader );
                    m_SearingBreath = reader.ReadInt();
                    m_SwipingClaws = reader.ReadInt();
                    m_TempestuousSea = reader.ReadInt();
                    m_SilentHowl = reader.ReadInt();
                    m_ThunderingHooves = reader.ReadInt();
                    m_VenomousWay = reader.ReadInt();
                    goto case 50;
                }

                case 50:
                {
                    m_RageHits = reader.ReadInt();
                    m_RageFeatLevel = reader.ReadInt();
                    goto case 49;
                }

                case 49:
                {
                    int test = reader.ReadInt();
                    goto case 48;
                }

                case 48:
                {
                    m_LastChargeStep = reader.ReadDateTime();
                    goto case 47;
                }

                case 47:
                {
                    m_FormerDirection = (Direction)reader.ReadInt();
                    m_ChargeSteps = reader.ReadInt();
                    goto case 46;
                }

                case 46:
                {
                   m_Trample = reader.ReadBool();
                   goto case 45;
                }

                case 45:
                {
                    m_FlurryOfBlows = reader.ReadInt();
                    goto case 44;
                }

                case 44:
                {
                    m_FocusedAttack = reader.ReadInt();
                    m_FightingStance = (FeatList)reader.ReadInt();
                    goto case 43;
                }

                case 43:
                {
                    m_Intimidated = reader.ReadInt();
                    goto case 42;
                }

                case 42:
                {
                    m_HasHuntingHoundBonus = reader.ReadBool();
                    goto case 41;
                }

                case 41:
                {
                    m_HuntingHound = reader.ReadMobile();
                    m_FreeToUse = reader.ReadBool();
                    goto case 40;
                }

                case 40:
                {
                    m_Informants = new Informants( reader );
                    goto case 39;
                }

                case 39:
                {
                    m_EscortPrisoner = reader.ReadMobile();
                    goto case 38;
                }

                case 38:
                {
                    m_CanBeReplaced = reader.ReadBool();
                    goto case 37;
                }

                case 37:
                {
                    m_BackToBack = reader.ReadBool();
                    goto case 36;
                }

                case 36:
                {
                    m_Crippled = reader.ReadBool();
                    goto case 35;
                }

                case 35:
                {
                    m_CleaveAttack = reader.ReadBool();
                    goto case 34;
                }

                case 34: goto case 33;
                
                case 33:
                {
                    goto case 32;
                }

                case 32:
                {
                    m_SpecialAttack = (FeatList)reader.ReadInt();
                    m_OffensiveFeat = (FeatList)reader.ReadInt();
                    goto case 31;
                }

                case 31:
                {
                    m_Feats = new Feats( reader );
                    goto case 30;
                }

                case 30:
                {
                    m_Subclass = (Subclass)reader.ReadInt();
                    m_Advanced = (Advanced)reader.ReadInt();
                    goto case 29;
                }
				case 29:
				{
					m_CanBeMage = reader.ReadBool();
					goto case 28;
				}
					
				case 28:
				{
					m_Level = reader.ReadInt();
					m_XP = reader.ReadInt();
					m_NextLevel = reader.ReadInt();
					m_CP = reader.ReadInt();
					goto case 27;
				}
					
				case 27:
				{
					m_StatPoints = reader.ReadInt();
					m_SkillPoints = reader.ReadInt();
					m_FeatSlots = reader.ReadInt();
					goto case 26;
				}
				case 26:
				{
					m_Class = (Class)reader.ReadInt();
					m_Nation = (Nation)reader.ReadInt();
					goto case 25;
				}
				case 25:
				{
					int recipeCount = reader.ReadInt();

					if( recipeCount > 0 )
					{
						m_AcquiredRecipes = new Dictionary<int, bool>();

						for( int i = 0; i < recipeCount; i++ )
						{
							int r = reader.ReadInt();
							if( reader.ReadBool() )	//Don't add in recipies which we haven't gotten or have been removed
								m_AcquiredRecipes.Add( r, true );
						}
					}
					goto case 24;
				}
				case 24:
				{
					m_LastHonorLoss = reader.ReadDeltaTime();
					goto case 23;
				}
				case 23:
				{
					m_ChampionTitles = new ChampionTitleInfo( reader );
					goto case 22;
				}
				case 22:
				{
					m_LastValorLoss = reader.ReadDateTime();
					goto case 21;
				}
				case 21:
				{
					m_ToTItemsTurnedIn = reader.ReadEncodedInt();
					m_ToTTotalMonsterFame = reader.ReadInt();
					goto case 20;
				}
				case 20:
				{
					m_AllianceMessageHue = reader.ReadEncodedInt();
					m_GuildMessageHue = reader.ReadEncodedInt();

					goto case 19;
				}
				case 19:
				{
					int rank = reader.ReadEncodedInt();
					int maxRank = Guilds.RankDefinition.Ranks.Length -1;
					if( rank > maxRank )
						rank = maxRank;

					m_GuildRank = Guilds.RankDefinition.Ranks[rank];
					m_LastOnline = reader.ReadDateTime();
					goto case 18;
				}
				case 18:
				{
					m_SolenFriendship = (SolenFriendship) reader.ReadEncodedInt();

					goto case 17;
				}
				case 17: // changed how DoneQuests is serialized
				case 16:
				{
					m_Quest = QuestSerializer.DeserializeQuest( reader );

					if ( m_Quest != null )
						m_Quest.From = this;

					int count = reader.ReadEncodedInt();

					if ( count > 0 )
					{
						m_DoneQuests = new List<QuestRestartInfo>();

						for ( int i = 0; i < count; ++i )
						{
							Type questType = QuestSerializer.ReadType( QuestSystem.QuestTypes, reader );
							DateTime restartTime;

							if ( version < 17 )
								restartTime = DateTime.MaxValue;
							else
								restartTime = reader.ReadDateTime();

							m_DoneQuests.Add( new QuestRestartInfo( questType, restartTime ) );
						}
					}

					m_Profession = reader.ReadEncodedInt();
					goto case 15;
				}
				case 15:
				{
					m_LastCompassionLoss = reader.ReadDeltaTime();
					goto case 14;
				}
				case 14:
				{
					m_CompassionGains = reader.ReadEncodedInt();

					if ( m_CompassionGains > 0 )
						m_NextCompassionDay = reader.ReadDeltaTime();

					goto case 13;
				}
				case 13: // just removed m_PayedInsurance list
				case 12:
				{
					m_BOBFilter = new Engines.BulkOrders.BOBFilter( reader );
					goto case 11;
				}
				case 11:
				{
					if ( version < 13 )
					{
						List<Item> payed = reader.ReadStrongItemList();

						for ( int i = 0; i < payed.Count; ++i )
							payed[i].PayedInsurance = true;
					}

					goto case 10;
				}
				case 10:
				{
					if ( reader.ReadBool() )
					{
						m_HairModID = reader.ReadInt();
						m_HairModHue = reader.ReadInt();
						m_BeardModID = reader.ReadInt();
						m_BeardModHue = reader.ReadInt();

						// We cannot call SetHairMods( -1, -1 ) here because the items have not yet loaded
						Timer.DelayCall( TimeSpan.Zero, new TimerCallback( RevertHair ) );
					}

					goto case 9;
				}
				case 9:
				{
					SavagePaintExpiration = reader.ReadTimeSpan();

					if ( SavagePaintExpiration > TimeSpan.Zero )
					{
						BodyMod = ( Female ? 184 : 183 );
						HueMod = 0;
					}

					goto case 8;
				}
				case 8:
				{
					m_NpcGuild = (NpcGuild)reader.ReadInt();
					m_NpcGuildJoinTime = reader.ReadDateTime();
					m_NpcGuildGameTime = reader.ReadTimeSpan();
					goto case 7;
				}
				case 7:
				{
					m_PermaFlags = reader.ReadStrongMobileList();
					goto case 6;
				}
				case 6:
				{
					NextTailorBulkOrder = reader.ReadTimeSpan();
					goto case 5;
				}
				case 5:
				{
					NextSmithBulkOrder = reader.ReadTimeSpan();
					goto case 4;
				}
				case 4:
				{
					m_LastJusticeLoss = reader.ReadDeltaTime();
					m_JusticeProtectors = reader.ReadStrongMobileList();
					goto case 3;
				}
				case 3:
				{
					m_LastSacrificeGain = reader.ReadDeltaTime();
					m_LastSacrificeLoss = reader.ReadDeltaTime();
					m_AvailableResurrects = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Flags = (PlayerFlag)reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_LongTermElapse = reader.ReadTimeSpan();
					m_ShortTermElapse = reader.ReadTimeSpan();
					m_GameTime = reader.ReadTimeSpan();

                    if (version < 147) m_Crimes = reader.ReadInt();
                    //CrimesList
                    if (version < 148)
                    {
                        m_CrimesList = new Dictionary<Mobiles.Nation, int>();
                        m_CrimesList.Add(Nation.Alyrian, 0);
                        m_CrimesList.Add(Nation.Azhuran, 0);
                        m_CrimesList.Add(Nation.Khemetar, 0);
                        m_CrimesList.Add(Nation.Mhordul, 0);
                        m_CrimesList.Add(Nation.Tyrean, 0);
                        m_CrimesList.Add(Nation.Vhalurian, 0);
                        m_CrimesList.Add(Nation.Imperial, 0);
                        m_CrimesList.Add(Nation.Sovereign, 0);
                        m_CrimesList.Add(Nation.Society, 0);

                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Nation n = (Nation)reader.ReadInt();
                            int c = reader.ReadInt();
                            //m_CrimesList.Add(n, c);
                        }
                    }
                    else
                    {
                        m_CrimesList = new Dictionary<Mobiles.Nation, int>();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Nation n = (Nation)reader.ReadInt();
                            int c = reader.ReadInt();
                            m_CrimesList.Add(n, c);
                        }
                    }

                    m_NextCriminalAct = reader.ReadDateTime();
                    m_CriminalActivity = reader.ReadBool();
                    m_Disguised = reader.ReadBool();
                    m_LastDisguiseTime = reader.ReadDateTime();

                    if (version < 149)
                    {
                        m_LastDeath = DateTime.Now - TimeSpan.FromDays(1);
                    }
                    else
                        m_LastDeath = reader.ReadDateTime();

					goto case 0;
				}
				case 0:
				{
					break;
				}
			}

			if (m_AvatarID == 0)
			m_AvatarID = 1076;
			
			//m_CrimesList.Add(Nation.Insularii, 0);
			
			if (m_CustomAvatarID1 == 0)
			m_CustomAvatarID1 = 1076;
			
			if (m_CustomAvatarID2 == 0)
			m_CustomAvatarID2 = 1076;
			
			if (m_CustomAvatarID3 == 0)
			m_CustomAvatarID3 = 1076;
			
			// Professions weren't verified on 1.0 RC0
			if ( !CharacterCreation.VerifyProfession( m_Profession ) )
				m_Profession = 0;

			if ( m_PermaFlags == null )
				m_PermaFlags = new List<Mobile>();

			if ( m_JusticeProtectors == null )
				m_JusticeProtectors = new List<Mobile>();

			if ( m_BOBFilter == null )
				m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			if( m_GuildRank == null )
				m_GuildRank = Guilds.RankDefinition.Member;	//Default to member if going from older verstion to new version (only time it should be null)

			if( m_LastOnline == DateTime.MinValue && Account != null )
				m_LastOnline = ((Account)Account).LastLogin;

			if( m_ChampionTitles == null )
				m_ChampionTitles = new ChampionTitleInfo();

			List<Mobile> list = this.Stabled;

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature bc = list[i] as BaseCreature;

				if ( bc != null )
					bc.IsStabled = true;
			}

			CheckAtrophies( this );

			/*if( Hidden )	//Hiding is the only buff where it has an effect that's serialized.
				AddBuff( new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );*/
			
			if( this.Lives >= 0 && !this.Alive && this.Corpse != null )
            	this.SendGump( new DeathGump( this, 15, this.Corpse ) );
			
			if( this.Backpack is ArmourBackpack )
			{
				ArmourBackpack pack = this.Backpack as ArmourBackpack;
				pack.Attributes.NightSight = 0;
			}

			if( version < 135 && AccessLevel < AccessLevel.GameMaster )
				OldMapChar = true;
			
			if( version < 141 )
			{
				FixSkillCost( FeatInfo.FeatCost.High, FeatInfo.FeatCost.Medium, Feats.GetFeatLevel(FeatList.Magery) );
				FixSkillCost( FeatInfo.FeatCost.High, FeatInfo.FeatCost.Low, Feats.GetFeatLevel(FeatList.Concentration) );
			}

            if( version < 145 )
            {
                m_TitlePrefix = null;
                m_RPTitle = "of the South";

                if( m_Nation == Nation.Alyrian )
                    m_RPTitle = "of the South";
                else if( m_Nation == Nation.Azhuran )
                    m_RPTitle = "of the West";
                else if( m_Nation == Nation.Khemetar )
                    m_RPTitle = "the Khemetar";
                else if( m_Nation == Nation.Mhordul )
                    m_RPTitle = "the Stranger";
                else if( m_Nation == Nation.Tyrean )
                    m_RPTitle = "the Tyrean";
                else if( m_Nation == Nation.Vhalurian )
                    m_RPTitle = "of the North";
            }
			
			m_Intimidated = 0;

            #region Removing Healing and HairStyling as feats and returning to players their CP
            if (Feats.GetFeatLevel(FeatList.Healing) == 3)
            {
                Feats.FeatDictionary[FeatList.Healing].AttemptRemoval(this, 3, true);
            }
            if (Feats.GetFeatLevel(FeatList.Healing) == 2)
            {
                Feats.FeatDictionary[FeatList.Healing].AttemptRemoval(this, 2, true);
            }
            if (Feats.GetFeatLevel(FeatList.Healing) == 1)
            {
                Feats.FeatDictionary[FeatList.Healing].AttemptRemoval(this, 1, true);
            }
            if (Feats.GetFeatLevel(FeatList.HairStyling) == 3)
            {
                Feats.FeatDictionary[FeatList.HairStyling].AttemptRemoval(this, 3, true);
            }
            if (Feats.GetFeatLevel(FeatList.HairStyling) == 2)
            {
                Feats.FeatDictionary[FeatList.HairStyling].AttemptRemoval(this, 2, true);
            }
            if (Feats.GetFeatLevel(FeatList.HairStyling) == 1)
            {
                Feats.FeatDictionary[FeatList.HairStyling].AttemptRemoval(this, 1, true);
            }
            #endregion

            m_Deserialized = true;
		}
		
		[CommandProperty( AccessLevel.Owner )]
		public bool FixMasterworkBonuses
		{
			get{ return false; }
			set
			{
				if( value == false )
					return;
				
				if( Feats.GetFeatLevel(FeatList.RenownedMasterwork) > 0 )
					Feats.FeatDictionary[FeatList.RenownedMasterwork].FixAddOns( this );
			}
		}
		
		public void FixSkillCost( FeatInfo.FeatCost oldCost, FeatInfo.FeatCost newCost, int skill )
		{
			if( oldCost == newCost || skill == 0 || skill < 0 || skill > 3 )
				return;
			
			int realOldCost = GetSkillCost( oldCost, skill );
			int realNewCost = GetSkillCost( newCost, skill );
			int refund = realOldCost - realNewCost;
			CPSpent -= refund;
			FeatSlots -= refund;
			CP += refund;
		}
		
		public int GetSkillCost( FeatInfo.FeatCost cost, int skill )
		{
			int first = 0;
			int second = 0;
			int third = 0;
			
			if( cost == FeatInfo.FeatCost.Low )
			{
				first = 500;
				second = 1500;
				third = 3000;
			}
			
			else if( cost == FeatInfo.FeatCost.Medium )
			{
				first = 1000;
				second = 3000;
				third = 6000;
			}
			
			else if( cost == FeatInfo.FeatCost.High )
			{
				first = 2000;
				second = 6000;
				third = 12000;
			}
			
			if( skill == 1 )
				return first;
			
			if( skill == 2 )
				return second;
			
			if( skill == 3 )
				return third;
			
			return 0;
		}
		
		public int RemoveFeat( int level )
		{
			if( level == 3 )
			{
				CPSpent -= 6000;
				FeatSlots -= 6000;
				CP += 6000;
			}
			
			else if( level == 2 )
			{
				CPSpent -= 1000;
				FeatSlots -= 1000;
				CP += 1000;
			}
			
			else if( level == 1 )
			{
				CPSpent -= 1000;
				FeatSlots -= 1000;
				CP += 1000;
			}
			
			return 0;
		}
		
		private int FixStat( int stat )
		{
			if( stat > 50 )
			{
				this.StatPoints += stat - 50;
				return 50;
			}
			
			return stat;
		}
		
		public override void Serialize( GenericWriter writer )
		{
			//cleanup our anti-macro table 
			foreach ( Hashtable t in m_AntiMacroTable.Values )
			{
				ArrayList remove = new ArrayList();
				foreach ( CountAndTimeStamp time in t.Values )
				{
					if ( time.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.Now )
						remove.Add( time );
				}

				for (int i=0;i<remove.Count;++i)
					t.Remove( remove[i] );
			}

			//decay our kills
			if ( m_ShortTermElapse < this.GameTime )
			{
				m_ShortTermElapse += TimeSpan.FromHours( 8 );
				if ( ShortTermMurders > 0 )
					--ShortTermMurders;
			}

			if ( m_LongTermElapse < this.GameTime )
			{
				m_LongTermElapse += TimeSpan.FromHours( 40 );
				if ( Kills > 0 )
					--Kills;
			}

			CheckAtrophies( this );

			base.Serialize( writer );
			
			writer.Write( (int) 161 ); // version

            writer.Write((int)this.NumberOfItemsCookedRecently);
            writer.Write((DateTime)this.CookingXpLastAwardedOn);

            writer.Write((DateTime)this.lastSecondWind);

            writer.Write((DateTime)this.lastCharge);
            writer.Write((int)this.chargeCooldown);

            writer.Write((bool)SmithTesting);
            writer.Write( (int) m_ConsecratedItems );
            writer.Write( (bool) m_CanBeFaithful );
			writer.Write( (DateTime) m_HCWound );
			writer.Write( (int) m_Maimings );
			writer.Write( (int) m_CustomAvatarID1 );
			writer.Write( (int) m_CustomAvatarID2 );
			writer.Write( (int) m_CustomAvatarID3 );
			writer.Write( (bool) m_IsHardcore );
            GroupInfo.Serialize(writer, Group);
			writer.Write( (bool) m_IsApprentice );
			writer.Write( (int) m_AvatarID );
            Disguise.Serialize( writer );
            MyDisguises.Serialize( writer );
			writer.Write( (bool) m_GemHarvesting );
			writer.Write( (bool) m_CanBeThief );
			writer.Write( (DateTime) m_LastCottonFlaxHarvest );
			writer.Write( (string) m_CraftingSpecialization );
			writer.Write( (bool) m_Forging );
			writer.Write( (bool) m_ImperialGuard );
			writer.Write( (int) m_ExtraCPRewards );
			writer.Write( (DateTime) m_EmptyBankBoxOn );
			writer.Write( (bool) m_OldMapChar );
			
			writer.Write( (int) CustomGuilds.Count );
			
			foreach( KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in CustomGuilds )
				CustomGuildInfo.Serialize( writer, kvp.Value );
			
			writer.Write( (int) m_ChosenDeity );
			writer.Write( (string) m_WikiConfig );
			writer.Write( (Point3D) m_ReforgeLocation );
			writer.Write( (Map) m_ReforgeMap );
			writer.Write( (bool) m_Reforging );
			writer.Write( (int) m_ReforgesLeft );
			writer.Write( (string) m_Technique );
			writer.Write( (int) m_TechniqueLevel );
			writer.Write( (int) m_MediumPieces );
			writer.Write( (int) m_HeavyPieces );
			writer.Write( (bool) m_VampSight );
			writer.Write( (bool) m_FreeForRP );
			writer.Write( (bool) m_VampSafety );
			writer.Write( (DateTime) m_LastTimeGhouled );
			writer.Write( (int) m_YearOfDeath );
			writer.Write( (int) m_MonthOfDeath );
			writer.Write( (int) m_DayOfDeath );
			writer.Write( (bool) m_IsVampire );
			writer.Write( (int) m_MaxBPs );
			writer.Write( (int) m_BPs );
			writer.Write( (bool) m_AutoVampHeal );
			
			writer.Write( (bool) m_LogMsgs );
			
			writer.Write( (bool) m_PureDodge );
			
			writer.Write( (List<Item>) m_Tents );
			
			writer.Write( (Container) m_CraftContainer );
			
			writer.Write( (CustomSpellBook) m_SpellBook );
			
			//CustomGuildInfo.Serialize( writer, CustomGuild );
			
			writer.Write( (int) m_CurrentCommand );
			writer.Write( (bool) m_SpeedHack );
			
			writer.Write( (DateTime) m_LogoutTime );
			
			writer.Write( (string) m_FakeAge );
			writer.Write( (string) m_FakeLooks );
			
			writer.Write( (string) this.NameMod );
			
			writer.Write( (bool) m_AutoPicking );
			
			writer.Write( (bool) m_SmoothPicking );
			
			writer.Write( (bool) m_HasStash );
			
			writer.Write( (int) m_FakeHair );
			writer.Write( (int) m_FakeHairHue );
			writer.Write( (int) m_FakeFacialHair );
			writer.Write( (int) m_FakeFacialHairHue );
			writer.Write( (int) m_FakeHue );
			writer.Write( (string) m_FakeRPTitle );
			writer.Write( (string) m_FakeTitlePrefix );
			
			writer.Write( (bool) m_FixedRun );
			
			writer.Write( (int) m_LightPieces );
			
			writer.Write( (int) m_ArmourPieces );
			
			writer.Write( (int) m_LightPenalty );
			writer.Write( (int) m_MediumPenalty );
			writer.Write( (int) m_HeavyPenalty );
			
			writer.Write( (bool) m_FixedRage );
			
			writer.Write( (bool) m_FixedReflexes );
			
			writer.Write( (bool) m_FixedStyles );
			
			writer.Write( (DateTime) m_NextMending );
			
			writer.Write( (bool) m_FixedStatPoints );
			
			writer.Write( (bool) m_XPFromCrafting );
			writer.Write( (bool) m_XPFromKilling );
			
			writer.Write( (int) m_HarvestedCrops );
			writer.Write( (DateTime) m_NextHarvestAllowed );
			
			writer.Write( (bool) m_Spar );
			
			writer.Write( (bool) m_HideStatus );
			
			writer.Write( (string) m_DayOfBirth );
			writer.Write( (string) m_MonthOfBirth );
			writer.Write( (string) m_YearOfBirth );
			
			writer.Write( (bool) m_AlyrianGuard );
			writer.Write( (bool) m_AzhuranGuard );
			writer.Write( (bool) m_KhemetarGuard );
			writer.Write( (bool) m_MhordulGuard );
			writer.Write( (bool) m_TyreanGuard );
			writer.Write( (bool) m_VhalurianGuard );
			
			writer.Write( (int) m_HearAll );
			
			Friendship.Serialize( writer, Friendship );
			
			writer.Write( (int) m_CPCapOffset );
			writer.Write( (int) m_CPSpent );
			
			writer.Write( (string) m_Description2 );
			writer.Write( (string) m_Description3 );
			
			writer.Write( (DateTime) m_NextBirthday );
			writer.Write( (int) m_MaxAge );
			
			writer.Write( (int) m_Age );
			
			writer.Write( m_LoggedOutPets );
			
			writer.Write( (int) m_RecreateXP );
			writer.Write( (int) m_RecreateCP );
			
			writer.Write( (DateTime) m_LastOffenseToNature );
			
			//writer.Write( (Mobile) m_LastSayEmote );
		
			writer.Write( (DateTime) m_LastDonationLife );
			
			writer.Write( (int) m_Lives );
			
			writer.Write( m_AllyList );
			
			writer.Write( (int) m_Height );
			writer.Write( (int) m_Weight );
			
			writer.Write( (DateTime) m_NextAllowance );
			
			Backgrounds.Serialize( writer, Backgrounds );
			
			writer.Write( (string) m_Description );
			
			Masterwork.Serialize( writer, Masterwork );
			
			RacialResources.Serialize( writer, RacialResources );
			
			KnownLanguages.Serialize( writer, KnownLanguages );
			
			writer.Write( (int) m_SpokenLanguage );
			
			writer.Write( (string) m_RPTitle );

            writer.Write( (string) m_TitlePrefix );

            writer.Write( (int) m_FocusedShot );

            writer.Write( (int) m_SwiftShot );

            writer.Write( (string) m_WeaponSpecialization );

            writer.Write( (string) m_SecondSpecialization );

            CombatStyles.Serialize( writer, CombatStyles );

            writer.Write( (int) m_SearingBreath );

            writer.Write( (int) m_SwipingClaws );

            writer.Write( (int) m_TempestuousSea );

            writer.Write( (int) m_SilentHowl );

            writer.Write( (int) m_ThunderingHooves );

            writer.Write( (int) m_VenomousWay );

            writer.Write( (int) m_RageHits );

            writer.Write( (int) m_RageFeatLevel );

            writer.Write( (int) 0 );

            writer.Write( (DateTime) m_LastChargeStep );

            writer.Write( (int) m_FormerDirection );

            writer.Write( (int) m_ChargeSteps );

            writer.Write( (bool) m_Trample );

            writer.Write( (int) m_FlurryOfBlows );

            writer.Write( (int) m_FocusedAttack );

            writer.Write( (int) m_FightingStance );

            writer.Write( (int) m_Intimidated );

            writer.Write( (bool) m_HasHuntingHoundBonus );

            writer.Write( (Mobile) m_HuntingHound );

            writer.Write( (bool) m_FreeToUse );

            Informants.Serialize( writer, Informants );

            writer.Write( (Mobile) m_EscortPrisoner );

            writer.Write( (bool) m_CanBeReplaced );

            writer.Write( (bool) m_BackToBack );

            writer.Write( (bool) m_Crippled );

            writer.Write( (bool) m_CleaveAttack );

            //writer.Write( (bool) m_CanRun );

            writer.Write( (int) m_SpecialAttack );
            writer.Write( (int) m_OffensiveFeat );

            Feats.Serialize( writer, Feats );

            writer.Write( (int) m_Subclass );
            writer.Write( (int) m_Advanced );
			writer.Write( (bool) m_CanBeMage );
			writer.Write( (int) m_Level );
			writer.Write( (int) m_XP );
			writer.Write( (int) m_NextLevel );
			writer.Write( (int) m_CP );
			writer.Write( (int) m_StatPoints );
			writer.Write( (int) m_SkillPoints );
			writer.Write( (int) m_FeatSlots );
			
			writer.Write( (int) m_Class );
			writer.Write( (int) m_Nation );

			if( m_AcquiredRecipes == null )
			{
				writer.Write( (int)0 );
			}
			else
			{
				writer.Write( m_AcquiredRecipes.Count );

				foreach( KeyValuePair<int, bool> kvp in m_AcquiredRecipes )
				{
					writer.Write( kvp.Key );
					writer.Write( kvp.Value );
				}
			}

			writer.WriteDeltaTime( m_LastHonorLoss );

			ChampionTitleInfo.Serialize( writer, m_ChampionTitles );

			writer.Write( m_LastValorLoss );
			writer.WriteEncodedInt( m_ToTItemsTurnedIn );
			writer.Write( m_ToTTotalMonsterFame );	//This ain't going to be a small #.

			writer.WriteEncodedInt( m_AllianceMessageHue );
			writer.WriteEncodedInt( m_GuildMessageHue );

			writer.WriteEncodedInt( m_GuildRank.Rank );
			writer.Write( m_LastOnline );

			writer.WriteEncodedInt( (int) m_SolenFriendship );

			QuestSerializer.Serialize( m_Quest, writer );

			if ( m_DoneQuests == null )
			{
				writer.WriteEncodedInt( (int) 0 );
			}
			else
			{
				writer.WriteEncodedInt( (int) m_DoneQuests.Count );

				for ( int i = 0; i < m_DoneQuests.Count; ++i )
				{
					QuestRestartInfo restartInfo = m_DoneQuests[i];

					QuestSerializer.Write( (Type) restartInfo.QuestType, QuestSystem.QuestTypes, writer );
					writer.Write( (DateTime) restartInfo.RestartTime );
				}
			}

			writer.WriteEncodedInt( (int) m_Profession );

			writer.WriteDeltaTime( m_LastCompassionLoss );

			writer.WriteEncodedInt( m_CompassionGains );

			if ( m_CompassionGains > 0 )
				writer.WriteDeltaTime( m_NextCompassionDay );

			m_BOBFilter.Serialize( writer );

			bool useMods = ( m_HairModID != -1 || m_BeardModID != -1 );

			writer.Write( useMods );

			if ( useMods )
			{
				writer.Write( (int) m_HairModID );
				writer.Write( (int) m_HairModHue );
				writer.Write( (int) m_BeardModID );
				writer.Write( (int) m_BeardModHue );
			}

			writer.Write( SavagePaintExpiration );

			writer.Write( (int) m_NpcGuild );
			writer.Write( (DateTime) m_NpcGuildJoinTime );
			writer.Write( (TimeSpan) m_NpcGuildGameTime );

			writer.Write( m_PermaFlags, true );

			writer.Write( NextTailorBulkOrder );

			writer.Write( NextSmithBulkOrder );

			writer.WriteDeltaTime( m_LastJusticeLoss );
			writer.Write( m_JusticeProtectors, true );

			writer.WriteDeltaTime( m_LastSacrificeGain );
			writer.WriteDeltaTime( m_LastSacrificeLoss );
			writer.Write( m_AvailableResurrects );

			writer.Write( (int) m_Flags );

			writer.Write( m_LongTermElapse );
			writer.Write( m_ShortTermElapse );
			writer.Write( this.GameTime );

            if (m_CrimesList == null)
            {                
                m_CrimesList = new Dictionary<Mobiles.Nation, int>();
                m_CrimesList.Add(Nation.Alyrian,    0);
                m_CrimesList.Add(Nation.Azhuran,    0);
                m_CrimesList.Add(Nation.Khemetar,   0);
                m_CrimesList.Add(Nation.Mhordul,    0);
                m_CrimesList.Add(Nation.Tyrean,     0);
                m_CrimesList.Add(Nation.Vhalurian,  0);
                m_CrimesList.Add(Nation.Imperial,   0);
                m_CrimesList.Add(Nation.Sovereign,  0);
                m_CrimesList.Add(Nation.Society,    0);

                writer.Write(m_CrimesList.Count);
                foreach (KeyValuePair<Nation, int> kvp in m_CrimesList)
                {
                    writer.Write((int)kvp.Key);
                    writer.Write((int)kvp.Value);
                }
            }
            else
            {
                writer.Write(m_CrimesList.Count);
                foreach (KeyValuePair<Nation, int> kvp in m_CrimesList)
                {
                    writer.Write((int)kvp.Key);
                    writer.Write((int)kvp.Value);
                }
            }

            writer.Write( (DateTime) m_NextCriminalAct );
            writer.Write( (bool) m_CriminalActivity );
            writer.Write( (bool) m_Disguised );
            writer.Write( (DateTime) m_LastDisguiseTime );
            writer.Write( (DateTime) m_LastDeath );
		}

		public static void CheckAtrophies( Mobile m )
		{
			SacrificeVirtue.CheckAtrophy( m );
			JusticeVirtue.CheckAtrophy( m );
			CompassionVirtue.CheckAtrophy( m );
			ValorVirtue.CheckAtrophy( m );
			HonorVirtue.CheckAtrophy( m );

			if( m is PlayerMobile )
				ChampionTitleInfo.CheckAtrophy( (PlayerMobile)m );
		}

		public void ResetKillTime()
		{
			m_ShortTermElapse = this.GameTime + TimeSpan.FromHours( 8 );
			m_LongTermElapse = this.GameTime + TimeSpan.FromHours( 40 );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SessionStart
		{
			get{ return m_SessionStart; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan GameTime
		{
			get
			{
				if ( NetState != null )
					return m_GameTime + (DateTime.Now - m_SessionStart);
				else
					return m_GameTime;
			}
		}

		public override bool CanSee( Mobile m )
		{
            int offset = 100;
            int sightRange = 0;

            if( (m is Wolf || m is Dog) && ((BaseCreature)m).ControlMaster == this )
				return true;

			if ( m is PlayerMobile && ((PlayerMobile)m).m_VisList.Contains( this ) )
				return true;
            
            if( m != this && m.Hidden && m.AccessLevel == AccessLevel && Utility.InUpdateRange(this,m))
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
			
            // Old version of Alertness, pre 12/2/2010 -- TMLOYD
            //if( m != this && m.Hidden && m is PlayerMobile && m.AccessLevel == AccessLevel && Feats.GetFeatLevel(FeatList.Alertness) > 0 && Utility.InUpdateRange(this, m) )
            //{
            //    int offset = 0;

            //    if( m is PlayerMobile && ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.Obfuscate ) > 1 )
            //        offset = 1;
                    
            //    if( (Feats.GetFeatLevel(FeatList.Alertness) - offset) >= (int)GetDistanceToSqrt(m.Location) )
            //    {
            //        if( !m_ForcedVisibilityList.Contains(m) )
            //        {
            //            m_ForcedVisibilityList.Add(m);
            //            Send( new Network.MobileIncoming( this, m ) );

            //            if ( ObjectPropertyList.Enabled )
            //            {
            //                Send( m.OPLPacket );

            //                foreach ( Item item in m.Items )
            //                    Send( item.OPLPacket );
            //            }
            //        }
					
            //        return true;
            //    }

            //    else if( m_ForcedVisibilityList.Contains( m ) && (( Feats.GetFeatLevel( FeatList.Alertness ) - offset ) * 2) >= (int)GetDistanceToSqrt( m.Location ) )
            //    {
            //        m_ForcedVisibilityList.Remove(m);
            //        Send( m.RemovePacket );
            //    }
            //}

			return base.CanSee( m );
		}

        //Cone of Vision is the three-directional ability of a mobile to "spot" targets within a certain space of their vision.
        public bool InFieldOfVision(object o)
        {
            Direction thisDirection = this.Direction;
            Direction targetDirection = this.Direction;

            if (o is Item)
                targetDirection = this.GetDirectionTo(((Item)o).Location);
            else if (o is Mobile)
                targetDirection = this.GetDirectionTo(((Mobile)o).Location);

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

		public override bool CanSee( Item item )
		{
			if ( m_DesignContext != null && m_DesignContext.Foundation.IsHiddenToCustomizer( item ) )
				return false;

            if( item is MilitaryWayPoint )
                return ( (MilitaryWayPoint)item ).CanSeeMe( this );

            if( item is MilitarySpawner )
                return ( (MilitarySpawner)item ).CanSeeMe( this );

            if (item is AtmosphereTile)
                return ((AtmosphereTile)item).CanSeeMe(this);

			return base.CanSee( item );
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Faction faction = Faction.Find( this );

			if ( faction != null )
				faction.RemoveMember( this );

			BaseHouse.HandleDeletion( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Map == Faction.Facet )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						list.Add( 1042733, faction.Definition.PropName ); // Commanding Lord of the ~1_FACTION_NAME~
					else if ( pl.Sheriff != null )
						list.Add( 1042734, "{0}\t{1}", pl.Sheriff.Definition.FriendlyName, faction.Definition.PropName ); // The Sheriff of  ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.Finance != null )
						list.Add( 1042735, "{0}\t{1}", pl.Finance.Definition.FriendlyName, faction.Definition.PropName ); // The Finance Minister of ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.MerchantTitle != MerchantTitle.None )
						list.Add( 1060776, "{0}\t{1}", MerchantTitles.GetInfo( pl.MerchantTitle ).Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
					else
						list.Add( 1060776, "{0}\t{1}", pl.Rank.Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
				}
			}
		}
		
		public override void OnAosSingleClick( Mobile from )
		{
			this.OnSingleClick( from );
		}

		public override void OnSingleClick( Mobile from )
		{
			/*if ( Map == Faction.Facet )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					string text;
					bool ascii = false;

					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						text = String.Concat( this.Female ? "(Commanding Lady of the " : "(Commanding Lord of the ", faction.Definition.FriendlyName, ")" );
					else if ( pl.Sheriff != null )
						text = String.Concat( "(The Sheriff of ", pl.Sheriff.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else if ( pl.Finance != null )
						text = String.Concat( "(The Finance Minister of ", pl.Finance.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")" );
					else
					{
						ascii = true;

						if ( pl.MerchantTitle != MerchantTitle.None )
							text = String.Concat( "(", MerchantTitles.GetInfo( pl.MerchantTitle ).Title.String, ", ", faction.Definition.FriendlyName, ")" );
						else
							text = String.Concat( "(", pl.Rank.Title.String, ", ", faction.Definition.FriendlyName, ")" );
					}

					int hue = ( Faction.Find( from ) == faction ? 98 : 38 );

					PrivateOverheadMessage( MessageType.Label, hue, ascii, text, from.NetState );
				}
			}

			base.OnSingleClick( from );*/
			
			if ( this.Deleted )
				return;
			else if ( AccessLevel == AccessLevel.Player && DisableHiddenSelfClick && Hidden && from == this )
				return;

			int newhue;

			if ( this.NameHue != -1 )
				newhue = NameHue;
			else
				newhue = Notoriety.GetHue( Notoriety.Compute( from, this ) );

			string name = Name;

			if ( name == null )
				name = String.Empty;

			string prefix = this.TitlePrefix;

			string val;

			if ( prefix.Length > 0 )
				val = String.Concat( prefix, " ", name );
			else
				val = name;

			PrivateOverheadMessage( MessageType.Label, newhue, false, val, from.NetState );
		}
		
		public override void AddNameProperties( ObjectPropertyList list )
		{
			string name = Name;

			if ( name == null )
				name = String.Empty;

			string prefix = "";

			if ( this.TitlePrefix != null )
				prefix = this.TitlePrefix;

			if( prefix != "" )
				list.Add( "{0} {1}", prefix, name );
			
			else
				list.Add( name );
		}

		protected override bool OnMove( Direction d )
		{
			if ( CombatSystemAttachment.GetCSA( this ).PerformingSequence )
			{
				CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( this );
				if ( csa.AnimationTimer != null )
					csa.AnimationTimer.RefreshAnimation();
				return false;
			}
			if( !Core.SE )
				return base.OnMove( d );

			if( AccessLevel != AccessLevel.Player )
				return true;

			if( Hidden && DesignContext.Find( this ) == null )	//Hidden & NOT customizing a house
			{
				if( !Mounted && Skills.Stealth.Value >= 25.0 )
				{
					bool running = (d & Direction.Running) != 0;

					if( running )
					{
						if( this.Feats.GetFeatLevel(FeatList.EnhancedStealth) > 0 )
						{
							int offset = 5 - this.Feats.GetFeatLevel(FeatList.EnhancedStealth);
							
							if( (AllowedStealthSteps -= offset) <= 0 )
								Server.SkillHandlers.Stealth.OnUse( this );
						}
						else
						{
							RevealingAction();
						}
					}
					else if( AllowedStealthSteps-- <= 0 )
					{
						Server.SkillHandlers.Stealth.OnUse( this );
					}			
				}
				else
				{
					RevealingAction();
				}
			}

			return true;
		}

		private bool m_BedrollLogout;

		public bool BedrollLogout
		{
			get{ return m_BedrollLogout; }
			set{ m_BedrollLogout = value; }
		}

        public static bool HasParalyzeAtt( Mobile m )
        {
            XmlParalyze par = XmlAttach.FindAttachment( m, typeof( XmlParalyze ) ) as XmlParalyze;

            if( par != null )
                return true;

            return false; 
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Paralyzed
		{
			get
			{
                if( this.StunnedTimer != null || this.m_PetrifiedTimer != null || HasParalyzeAtt(this) )
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
            	if( m_FreezeTimer != null )
            		return true;
            	
            	else
                	return base.Frozen;
            }
            set
            {
                base.Frozen = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Dictionary<Nation, int> CrimesList
        {
            get
            {
                return m_CrimesList;
            }
            set
            {
                m_CrimesList = value;
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool CriminalActivity
        {
            get 
			{
				if( DateTime.Compare( DateTime.Now, this.m_NextCriminalAct ) > 0 )
					return false;
				
				return true;
			}
			
			set
			{
				if( value == false )
					this.m_NextCriminalAct = DateTime.Now;
				
				else
					this.m_NextCriminalAct = DateTime.Now + TimeSpan.FromSeconds( 30 );
			}
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Disguised
        {
            get 
            { 
                return m_Disguised; 
            }
            set
            {
                m_Disguised = value;

                /* When you change your disguise status, you throw off the guards.*/
                XmlAttachment criminalAttachment = null;
                criminalAttachment = XmlAttach.FindAttachment(this, typeof(XmlCriminal));
                if((criminalAttachment != null) && (criminalAttachment is XmlCriminal))
                    criminalAttachment.Delete();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastDisguiseTime
        {
            get
            {
                return m_LastDisguiseTime;
            }
            set
            {
                m_LastDisguiseTime = DateTime.Now;
            }
        }

		#region Ethics
		private Ethics.Player m_EthicPlayer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Ethics.Player EthicPlayer
		{
			get { return m_EthicPlayer; }
			set { m_EthicPlayer = value; }
		}
		#endregion

		#region Factions
		private PlayerState m_FactionPlayerState;

		public PlayerState FactionPlayerState
		{
			get{ return m_FactionPlayerState; }
			set{ m_FactionPlayerState = value; }
		}
		#endregion

		#region Quests
		private QuestSystem m_Quest;
		private List<QuestRestartInfo> m_DoneQuests;
		private SolenFriendship m_SolenFriendship;

		public QuestSystem Quest
		{
			get{ return m_Quest; }
			set{ m_Quest = value; }
		}

		public List<QuestRestartInfo> DoneQuests
		{
			get{ return m_DoneQuests; }
			set{ m_DoneQuests = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SolenFriendship SolenFriendship
		{
			get{ return m_SolenFriendship; }
			set{ m_SolenFriendship = value; }
		}
		#endregion

		#region MyRunUO Invalidation
		private bool m_ChangedMyRunUO;

		public bool ChangedMyRunUO
		{
			get{ return m_ChangedMyRunUO; }
			set{ m_ChangedMyRunUO = value; }
		}

		public void InvalidateMyRunUO()
		{
			if ( !Deleted && !m_ChangedMyRunUO )
			{
				m_ChangedMyRunUO = true;
				Engines.MyRunUO.MyRunUO.QueueMobileUpdate( this );
			}
		}

		public override void OnKillsChange( int oldValue )
		{
			if ( this.Young && this.Kills > oldValue )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 0 );
			}

			InvalidateMyRunUO();
		}

		public override void OnGenderChanged( bool oldFemale )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildChange( Server.Guilds.BaseGuild oldGuild )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildTitleChange( string oldTitle )
		{
			InvalidateMyRunUO();
		}

		public override void OnKarmaChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnFameChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnSkillChange( SkillName skill, double oldBase )
		{
			if ( this.Young && this.SkillsTotal >= 4500 )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 1019036 ); // You have successfully obtained a respectable skill level, and have outgrown your status as a young player!
			}

			InvalidateMyRunUO();
		}

		public override void OnAccessLevelChanged( AccessLevel oldLevel )
		{
			InvalidateMyRunUO();
		}

		public override void OnRawStatChange( StatType stat, int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnDelete()
		{
			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.Cancel();
			if ( m_SentHonorContext != null )
				m_SentHonorContext.Cancel();

			InvalidateMyRunUO();
		}
		#endregion

		#region Fastwalk Prevention
		private static bool FastwalkPrevention = true; // Is fastwalk prevention enabled?
		private static TimeSpan FastwalkThreshold = TimeSpan.FromSeconds( 0.4 ); // Fastwalk prevention will become active after 0.4 seconds

		private DateTime m_NextMovementTime;

		public virtual bool UsesFastwalkPrevention{ get{ return ( AccessLevel < AccessLevel.Counselor ); } }
		
		private void HandleEscortPrisoner()
		{
			Mobile mob = World.FindMobile( this.EscortPrisoner.Serial );

            if( mob != null && mob.Alive && !mob.Deleted && mob.Map == this.Map && mob.Paralyzed && this.Alive && !this.Deleted )
            {
                mob.Location = this.Location;
                mob.Direction = this.Direction;
            }

            else
                this.EscortPrisoner = null;
		}
		
		int m_StepsOnHorseback = 0;
		
		private void HandleFallFromHorse()
		{
			m_StepsOnHorseback++;

        	if( m_StepsOnHorseback > 9 )
        	{
        		if( !CheckSkill(SkillName.Riding, 0.0, 50.0) )
            	{
            		IMount mount = Mount;
                    mount.Rider = null;
                	
                    if( this.DismountedTimer != null )
						this.DismountedTimer.Stop();
			
					DismountedTimer = new Misc.Dismount.DismountTimer( this, 1 );
					DismountedTimer.Start();
                    
                    Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), this, this, Utility.RandomMinMax( 1, 6 ) );
                    Emote( "*falls from {0} mount*", Female == true ? "her" : "his" );
            	}
        	}
		}
		
		private TimeSpan ComputedSpeed( bool onHorse, bool running, AnimalFormContext context )
		{
			if ( CombatSystemAttachment.GetCSA( this ).BullRushing )
			{
				this.Send( SpeedMode.Run );
				return ( Mobile.RunMount );
			}
			
			if( !onHorse && !this.CanRun || !CombatSystemAttachment.GetCSA( this ).CanRun() )
            {
                this.Send( SpeedMode.Walk );                
                return ( Mobile.WalkFoot );
            }

            else
            {
                if( this.EscortPrisoner != null && this.Feats.GetFeatLevel(FeatList.EscortPrisoner) < 2 )
                {
                    this.Send( SpeedMode.Walk );
                    return ( Mobile.WalkFoot );
                }

                else
                {
                    if( onHorse || ( context != null && context.SpeedBoost ) )
                    {
                    	if( onHorse && running )
                    		HandleFallFromHorse();
                    	
                    	this.Send( SpeedMode.Disabled );
                    	return ( running ? Mobile.RunMount : Mobile.WalkMount );
                    }

                    else
                    {
                    	if( this.SpeedHack )
                    	{
                    		this.Send( SpeedMode.Run );
                    		return ( running ? Mobile.RunMount : Mobile.WalkMount );
                    	}
                    	
                    	this.Send( SpeedMode.Disabled );                    	
                    	return ( running ? Mobile.RunFoot : Mobile.WalkFoot );
                    }
                }
            }
		}

		public override TimeSpan ComputeMovementSpeed( Direction dir, bool checkTurning )
		{
			if ( checkTurning && (dir & Direction.Mask) != (this.Direction & Direction.Mask) )
				return TimeSpan.FromSeconds( 0.1 );	// We are NOT actually moving (just a direction change)

			bool running = ( (dir & Direction.Running) != 0 );

			bool onHorse = ( this.Mount != null );

			AnimalFormContext context = AnimalForm.GetContext( this );
            this.LastStep = DateTime.Now;

            if( this.EscortPrisoner != null )
            	HandleEscortPrisoner();

            FightingBackToBack( this, false );
            
            return ComputedSpeed( onHorse, running, context );
		}
        
        public static bool EvadedCheck( Mobile mob )
        {
        	if( ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Evade) > 0 )
            {
                int chancetoevade = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Evade) * 10;
                int evaderoll = Utility.RandomMinMax( 1, 100 );

                if( chancetoevade >= evaderoll )
                    return true;
            }
            return false;
        }

        public bool Evaded()
        {
        	if( Paralyzed )
    			return false;
    		
    		if( TrippedTimer != null )
    			return false;
        		
        	return EvadedCheck( this );
        }
        
        public static bool DodgedCheck( Mobile mob )
        {
        	int offset = 0;

        	if( (((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EnhancedDodge) + offset) > 0 )
            {
        		int chancetoevade = (((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EnhancedDodge) * 7) + offset;
                int evaderoll = Utility.RandomMinMax( 1, 100 );

                if( chancetoevade >= evaderoll )
                    return true;
            }
            return false;
        }

        public bool Dodged()
        {
        	return DodgedCheck( this );
        }
        
        public static bool SnatchedCheck( Mobile mob )
        {
        	if( ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CatchProjectiles) > 0 && mob.Weapon is Fists && !(mob.FindItemOnLayer( Layer.TwoHanded) is BaseShield) )
            {
                int chancetoevade = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CatchProjectiles) * 10;
                int evaderoll = Utility.Random( 100 );

                if( chancetoevade >= evaderoll )
                    return true;
            }
            return false;
        }

        public bool Snatched()
        {
        	return SnatchedCheck( this );
        }
        
        public static bool DeflectedProjectileCheck( Mobile mob )
        {
        	BaseShield shield = mob.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;

            if( shield != null )
            {
                if( shield is BaseShield && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.DeflectProjectiles) > 0 )
                {
                    int chancetoblock = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.DeflectProjectiles) * 10;
                    int blockroll = Utility.RandomMinMax( 1, 100 );

                    if( chancetoblock >= blockroll )
                        return true;
                }
            }
            return false;
        }

        public bool DeflectedProjectile()
        {
        	return DeflectedProjectileCheck( this );
        }
     
        public static void FightingBackToBack( PlayerMobile pm, bool movingaway )
        {
            ArrayList list = new ArrayList();
            ArrayList allies = new ArrayList();
            ArrayList leavelist = new ArrayList();

            if( pm.Feats.GetFeatLevel(FeatList.BackToBack) > 0 )
            {
                if( pm.Warmode && !pm.Mounted )
                {
                    foreach( Mobile m in pm.GetMobilesInRange( 2 ) )
                        list.Add( m );

                    for( int i = 0; i < list.Count; ++i )
                    {
                        PlayerMobile m = (Mobile)list[i] as PlayerMobile;
                        Mobile x = m as Mobile;
                        Mobile y = pm as Mobile;

                        if( m is PlayerMobile && m != pm && m.Warmode && !m.Mounted )
                        {
                            if( m == null || m.Deleted || m.Map != pm.Map || !m.Alive || !pm.CanSee( m ) || m.Feats.GetFeatLevel(FeatList.BackToBack) < 1 || !pm.AllyList.Contains( x ) || !m.AllyList.Contains( y ) )
                                continue;

                            if( pm.InLOS( m ) )
                                allies.Add( m );
                        }
                    }

                    if( allies.Count > 0 )
                    {
                        for( int i = 0; i < allies.Count; ++i )
                        {
                            PlayerMobile m = (Mobile)allies[i] as PlayerMobile;

                            if( !m.BackToBack )
                            {
                                BackToBackBonus( m, true );
                            }

                            if( !pm.BackToBack )
                            {
                                BackToBackBonus( pm, true );
                            }
                        }
                    }

                    else
                    {
                        if( pm.BackToBack )
                        {
                            BackToBackBonus( pm, false );
                        }

                        if( !movingaway )
                        {
                            foreach( Mobile mob in pm.GetMobilesInRange( 3 ) )
                                leavelist.Add( mob );

                            for( int i = 0; i < leavelist.Count; ++i )
                            {
                                PlayerMobile m = (Mobile)leavelist[i] as PlayerMobile;

                                if( m != pm && m is PlayerMobile )
                                {
                                    FightingBackToBack( m, true );
                                }
                            }
                        }
                    }
                }

                else
                {   
                    if( pm.BackToBack )
                    {
                        BackToBackBonus( pm, false );
                    }
                }
            }
        }

        public static void BackToBackBonus( PlayerMobile pm, bool AddBonus )
        {
            if( AddBonus )
            {
                pm.BackToBack = true;
				pm.RemoveBuff( BuffIcon.ArcaneEmpowerment );
				string msg = "+" + pm.Feats.GetFeatLevel(FeatList.BackToBack)*10 + " defense chance increase<BR>";
				if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) == 1 )
					msg += "No attacks of opportunity (back)";
				else if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) == 2 )
					msg += "No attacks of opportunity (back, back flank)";
				else if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) >= 3 )
					msg += "No attacks of opportunity";
				pm.AddBuff( new BuffInfo(
						BuffIcon.ArcaneEmpowerment, 1041600, 1060847, "<CENTER>Back To Back\t<BR>" + msg, false
					) );
            }

            else
            {
                if( pm.BackToBack )
                {
                    pm.BackToBack = false;
					pm.RemoveBuff( BuffIcon.ArcaneEmpowerment );
                }
            }
        }

		public static bool MovementThrottle_Callback( NetState ns )
		{
			PlayerMobile pm = ns.Mobile as PlayerMobile;

			if ( pm == null || !pm.UsesFastwalkPrevention )
				return true;

			if ( pm.m_NextMovementTime == DateTime.MinValue )
			{
				// has not yet moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			TimeSpan ts = pm.m_NextMovementTime - DateTime.Now;

			if ( ts < TimeSpan.Zero )
			{
				// been a while since we've last moved
				pm.m_NextMovementTime = DateTime.Now;
				return true;
			}

			return ( ts < FastwalkThreshold );
		}
		#endregion

		#region Enemy of One
		private Type m_EnemyOfOneType;
		private bool m_WaitingForEnemy;

		public Type EnemyOfOneType
		{
			get{ return m_EnemyOfOneType; }
			set
			{
				Type oldType = m_EnemyOfOneType;
				Type newType = value;

				if ( oldType == newType )
					return;

				m_EnemyOfOneType = value;

				DeltaEnemies( oldType, newType );
			}
		}

		public bool WaitingForEnemy
		{
			get{ return m_WaitingForEnemy; }
			set{ m_WaitingForEnemy = value; }
		}

		private void DeltaEnemies( Type oldType, Type newType )
		{
			foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
			{
				Type t = m.GetType();

				if ( t == oldType || t == newType )
					Send( new MobileMoving( m, Notoriety.Compute( this, m ) ) );
			}
		}
		#endregion

		#region Hair and beard mods
		private int m_HairModID = -1, m_HairModHue;
		private int m_BeardModID = -1, m_BeardModHue;

		public void SetHairMods( int hairID, int beardID )
		{
			if ( hairID == -1 )
				InternalRestoreHair( true, ref m_HairModID, ref m_HairModHue );
			else if ( hairID != -2 )
				InternalChangeHair( true, hairID, ref m_HairModID, ref m_HairModHue );

			if ( beardID == -1 )
				InternalRestoreHair( false, ref m_BeardModID, ref m_BeardModHue );
			else if ( beardID != -2 )
				InternalChangeHair( false, beardID, ref m_BeardModID, ref m_BeardModHue );
		}

		private void CreateHair( bool hair, int id, int hue )
		{
			if( hair )
			{
				//TODO Verification?
				HairItemID = id;
				HairHue = hue;
			}
			else
			{
				FacialHairItemID = id;
				FacialHairHue = hue;
			}
		}

		private void InternalRestoreHair( bool hair, ref int id, ref int hue )
		{
			if ( id == -1 )
				return;

			if ( hair )
				HairItemID = 0;
			else
				FacialHairItemID = 0;

			//if( id != 0 )
			CreateHair( hair, id, hue );

			id = -1;
			hue = 0;
		}

		private void InternalChangeHair( bool hair, int id, ref int storeID, ref int storeHue )
		{
			if ( storeID == -1 )
			{
				storeID = hair ? HairItemID : FacialHairItemID;
				storeHue = hair ? HairHue : FacialHairHue;
			}
			CreateHair( hair, id, 0 );
		}
		#endregion

		#region Virtues
		private DateTime m_LastSacrificeGain;
		private DateTime m_LastSacrificeLoss;
		private int m_AvailableResurrects;

		public DateTime LastSacrificeGain{ get{ return m_LastSacrificeGain; } set{ m_LastSacrificeGain = value; } }
		public DateTime LastSacrificeLoss{ get{ return m_LastSacrificeLoss; } set{ m_LastSacrificeLoss = value; } }
		public int AvailableResurrects{ get{ return m_AvailableResurrects; } set{ m_AvailableResurrects = value; } }

		private DateTime m_NextJustAward;
		private DateTime m_LastJusticeLoss;
		private List<Mobile> m_JusticeProtectors;

		public DateTime LastJusticeLoss{ get{ return m_LastJusticeLoss; } set{ m_LastJusticeLoss = value; } }
		public List<Mobile> JusticeProtectors { get { return m_JusticeProtectors; } set { m_JusticeProtectors = value; } }

		private DateTime m_LastCompassionLoss;
		private DateTime m_NextCompassionDay;
		private int m_CompassionGains;

		public DateTime LastCompassionLoss{ get{ return m_LastCompassionLoss; } set{ m_LastCompassionLoss = value; } }
		public DateTime NextCompassionDay{ get{ return m_NextCompassionDay; } set{ m_NextCompassionDay = value; } }
		public int CompassionGains{ get{ return m_CompassionGains; } set{ m_CompassionGains = value; } }

		private DateTime m_LastValorLoss;

		public DateTime LastValorLoss { get { return m_LastValorLoss; } set { m_LastValorLoss = value; } }

		private DateTime m_LastHonorLoss;
		private DateTime m_LastHonorUse;
		private bool m_HonorActive;
		private HonorContext m_ReceivedHonorContext;
		private HonorContext m_SentHonorContext;

		public DateTime LastHonorLoss{ get{ return m_LastHonorLoss; } set{ m_LastHonorLoss = value; } }
		public DateTime LastHonorUse{ get{ return m_LastHonorUse; } set{ m_LastHonorUse = value; } }
		public bool HonorActive{ get{ return m_HonorActive; } set{ m_HonorActive = value; } }
		public HonorContext ReceivedHonorContext{ get{ return m_ReceivedHonorContext; } set{ m_ReceivedHonorContext = value; } }
		public HonorContext SentHonorContext{ get{ return m_SentHonorContext; } set{ m_SentHonorContext = value; } }
		#endregion

		#region Young system
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Young
		{
			get{ return GetFlag( PlayerFlag.Young ); }
			set{ SetFlag( PlayerFlag.Young, value ); InvalidateProperties(); }
		}

		public override string ApplyNameSuffix( string suffix )
		{
			if ( Young )
			{
				if ( suffix.Length == 0 )
					suffix = "(Young)";
				else
					suffix = String.Concat( suffix, " (Young)" );
			}

			#region Ethics
			if ( m_EthicPlayer != null )
			{
				if ( suffix.Length == 0 )
					suffix = m_EthicPlayer.Ethic.Definition.Adjunct.String;
				else
					suffix = String.Concat( suffix, " ", m_EthicPlayer.Ethic.Definition.Adjunct.String );
			}
			#endregion

			return base.ApplyNameSuffix( suffix );
		}


		public override TimeSpan GetLogoutDelay()
		{
			if ( Young || BedrollLogout || TestCenter.Enabled )
				return TimeSpan.Zero;

			return base.GetLogoutDelay();
		}

		private DateTime m_LastYoungMessage = DateTime.MinValue;

		public bool CheckYoungProtection( Mobile from )
		{
			if ( !this.Young )
				return false;

			if ( Region.IsPartOf( typeof( DungeonRegion ) ) )
				return false;

			if( from is BaseCreature && ((BaseCreature)from).IgnoreYoungProtection )
				return false;

			if ( this.Quest != null && this.Quest.IgnoreYoungProtection( from ) )
				return false;

			if ( DateTime.Now - m_LastYoungMessage > TimeSpan.FromMinutes( 1.0 ) )
			{
				m_LastYoungMessage = DateTime.Now;
				SendLocalizedMessage( 1019067 ); // A monster looks at you menacingly but does not attack.  You would be under attack now if not for your status as a new citizen of Britannia.
			}

			return true;
		}

		private DateTime m_LastYoungHeal = DateTime.MinValue;

		public bool CheckYoungHealTime()
		{
			if ( DateTime.Now - m_LastYoungHeal > TimeSpan.FromMinutes( 5.0 ) )
			{
				m_LastYoungHeal = DateTime.Now;
				return true;
			}

			return false;
		}

		private static Point3D[] m_TrammelDeathDestinations = new Point3D[]
			{
				new Point3D( 1481, 1612, 20 ),
				new Point3D( 2708, 2153,  0 ),
				new Point3D( 2249, 1230,  0 ),
				new Point3D( 5197, 3994, 37 ),
				new Point3D( 1412, 3793,  0 ),
				new Point3D( 3688, 2232, 20 ),
				new Point3D( 2578,  604,  0 ),
				new Point3D( 4397, 1089,  0 ),
				new Point3D( 5741, 3218, -2 ),
				new Point3D( 2996, 3441, 15 ),
				new Point3D(  624, 2225,  0 ),
				new Point3D( 1916, 2814,  0 ),
				new Point3D( 2929,  854,  0 ),
				new Point3D(  545,  967,  0 ),
				new Point3D( 3665, 2587,  0 )
			};

		private static Point3D[] m_IlshenarDeathDestinations = new Point3D[]
			{
				new Point3D( 1216,  468, -13 ),
				new Point3D(  723, 1367, -60 ),
				new Point3D(  745,  725, -28 ),
				new Point3D(  281, 1017,   0 ),
				new Point3D(  986, 1011, -32 ),
				new Point3D( 1175, 1287, -30 ),
				new Point3D( 1533, 1341,  -3 ),
				new Point3D(  529,  217, -44 ),
				new Point3D( 1722,  219,  96 )
			};

		private static Point3D[] m_MalasDeathDestinations = new Point3D[]
			{
				new Point3D( 2079, 1376, -70 ),
				new Point3D(  944,  519, -71 )
			};

		private static Point3D[] m_TokunoDeathDestinations = new Point3D[]
			{
				new Point3D( 1166,  801, 27 ),
				new Point3D(  782, 1228, 25 ),
				new Point3D(  268,  624, 15 )
			};

		public bool YoungDeathTeleport()
		{
			if ( this.Region.IsPartOf( typeof( Jail ) )
				|| this.Region.IsPartOf( "Samurai start location" )
				|| this.Region.IsPartOf( "Ninja start location" )
				|| this.Region.IsPartOf( "Ninja cave" ) )
				return false;

			Point3D loc;
			Map map;

			DungeonRegion dungeon = (DungeonRegion) this.Region.GetRegion( typeof( DungeonRegion ) );
			if ( dungeon != null && dungeon.EntranceLocation != Point3D.Zero )
			{
				loc = dungeon.EntranceLocation;
				map = dungeon.EntranceMap;
			}
			else
			{
				loc = this.Location;
				map = this.Map;
			}

			Point3D[] list;

			if ( map == Map.Trammel )
				list = m_TrammelDeathDestinations;
			else if ( map == Map.Ilshenar )
				list = m_IlshenarDeathDestinations;
			else if ( map == Map.Malas )
				list = m_MalasDeathDestinations;
			else if ( map == Map.Tokuno )
				list = m_TokunoDeathDestinations;
			else
				return false;

			Point3D dest = Point3D.Zero;
			int sqDistance = int.MaxValue;

			for ( int i = 0; i < list.Length; i++ )
			{
				Point3D curDest = list[i];

				int width = loc.X - curDest.X;
				int height = loc.Y - curDest.Y;
				int curSqDistance = width * width + height * height;

				if ( curSqDistance < sqDistance )
				{
					dest = curDest;
					sqDistance = curSqDistance;
				}
			}

			this.MoveToWorld( dest, map );
			return true;
		}

		private void SendYoungDeathNotice()
		{
			this.SendGump( new YoungDeathNotice() );
		}
		#endregion

		#region Speech log
		private SpeechLog m_SpeechLog;

		public SpeechLog SpeechLog{ get{ return m_SpeechLog; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( SpeechLog.Enabled && this.NetState != null )
			{
				if ( m_SpeechLog == null )
					m_SpeechLog = new SpeechLog();

				m_SpeechLog.Add( e.Mobile, e.Speech );
			}
		}
		#endregion

		#region Champion Titles
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisplayChampionTitle
		{
			get { return GetFlag( PlayerFlag.DisplayChampionTitle ); }
			set { SetFlag( PlayerFlag.DisplayChampionTitle, value ); }
		}

		private ChampionTitleInfo m_ChampionTitles;

		[CommandProperty( AccessLevel.GameMaster )]
		public ChampionTitleInfo ChampionTitles { get { return m_ChampionTitles; } set { } }

		private void ToggleChampionTitleDisplay()
		{
			if( !CheckAlive() )
				return;

			if( DisplayChampionTitle )
				SendLocalizedMessage( 1062419, "", 0x23 ); // You have chosen to hide your monster kill title.
			else
				SendLocalizedMessage( 1062418, "", 0x23 ); // You have chosen to display your monster kill title.

			DisplayChampionTitle = !DisplayChampionTitle;
		}

		[PropertyObject]
		public class ChampionTitleInfo
		{
			public static TimeSpan LossDelay = TimeSpan.FromDays( 1.0 );
			public const int LossAmount = 90;

			private class TitleInfo
			{
				private int m_Value;
				private DateTime m_LastDecay;

				public int Value { get { return m_Value; } set { m_Value = value; } }
				public DateTime LastDecay { get { return m_LastDecay; } set { m_LastDecay = value; } }

				public TitleInfo()
				{
				}

				public TitleInfo( GenericReader reader )
				{
					int version = reader.ReadEncodedInt();

					switch( version )
					{
						case 0:
						{
							m_Value = reader.ReadEncodedInt();
							m_LastDecay = reader.ReadDateTime();
							break;
						}
					}
				}

				public static void Serialize( GenericWriter writer, TitleInfo info )
				{
					writer.WriteEncodedInt( (int)0 ); // version

					writer.WriteEncodedInt( info.m_Value );
					writer.Write( info.m_LastDecay );
				}

			}
			private TitleInfo[] m_Values;

			private int m_Ghost;	//Ghost titles do NOT decay


			public int GetValue( ChampionSpawnType type )
			{
				return GetValue( (int)type );
			}

			public void SetValue( ChampionSpawnType type, int value )
			{
				SetValue( (int)type, value );
			}

			public void Award( ChampionSpawnType type, int value )
			{
				Award( (int)type, value );
			}

			public int GetValue( int index )
			{
				if( m_Values == null || index < 0 || index >= m_Values.Length )
					return 0;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				return m_Values[index].Value;
			}

			public DateTime GetLastDecay( int index )
			{
				if( m_Values == null || index < 0 || index >= m_Values.Length )
					return DateTime.MinValue;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				return m_Values[index].LastDecay;
			}

			public void SetValue( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( value < 0 )
					value = 0;

				if( index < 0 || index >= m_Values.Length )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				m_Values[index].Value = value;
			}

			public void Award( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( index < 0 || index >= m_Values.Length || value <= 0 )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				m_Values[index].Value += value;
			}

			public void Atrophy( int index, int value )
			{
				if( m_Values == null )
					m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				if( index < 0 || index >= m_Values.Length || value <= 0 )
					return;

				if( m_Values[index] == null )
					m_Values[index] = new TitleInfo();

				int before = m_Values[index].Value;

				if( (m_Values[index].Value - value) < 0 )
					m_Values[index].Value = 0;
				else
					m_Values[index].Value -= value;

				if( before != m_Values[index].Value )
					m_Values[index].LastDecay = DateTime.Now;
			}

			public override string ToString()
			{
				return "...";
			}

			[CommandProperty( AccessLevel.GameMaster )]
			public int Abyss { get { return GetValue( ChampionSpawnType.Abyss ); } set { SetValue( ChampionSpawnType.Abyss, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int Arachnid { get { return GetValue( ChampionSpawnType.Arachnid ); } set { SetValue( ChampionSpawnType.Arachnid, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int ColdBlood { get { return GetValue( ChampionSpawnType.ColdBlood ); } set { SetValue( ChampionSpawnType.ColdBlood, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int ForestLord { get { return GetValue( ChampionSpawnType.ForestLord ); } set { SetValue( ChampionSpawnType.ForestLord, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int SleepingDragon { get { return GetValue( ChampionSpawnType.SleepingDragon ); } set { SetValue( ChampionSpawnType.SleepingDragon, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int UnholyTerror { get { return GetValue( ChampionSpawnType.UnholyTerror ); } set { SetValue( ChampionSpawnType.UnholyTerror, value ); } }

			[CommandProperty( AccessLevel.GameMaster )]
			public int VerminHorde { get { return GetValue( ChampionSpawnType.VerminHorde ); } set { SetValue( ChampionSpawnType.VerminHorde, value ); } }
			
			[CommandProperty( AccessLevel.GameMaster )]
			public int Ghost { get { return m_Ghost; } set { m_Ghost = value; } }

			public ChampionTitleInfo()
			{
			}

			public ChampionTitleInfo( GenericReader reader )
			{
				int version = reader.ReadEncodedInt();

				switch( version )
				{
					case 0:
					{
						m_Ghost = reader.ReadEncodedInt();

						int length = reader.ReadEncodedInt();
						m_Values = new TitleInfo[length];

						for( int i = 0; i < length; i++ )
						{
							m_Values[i] = new TitleInfo( reader );
						}

						if( m_Values.Length != ChampionSpawnInfo.Table.Length )
						{
							TitleInfo[] oldValues = m_Values;
							m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

							for( int i = 0; i < m_Values.Length && i < oldValues.Length; i++ )
							{
								m_Values[i] = oldValues[i];
							}
						}
						break;
					}
				}
			}

			public static void Serialize( GenericWriter writer, ChampionTitleInfo titles )
			{
				writer.WriteEncodedInt( (int)0 ); // version

				writer.WriteEncodedInt( titles.m_Ghost );

				int length = titles.m_Values.Length;
				writer.WriteEncodedInt( length );

				for( int i = 0; i < length; i++ )
				{
					if( titles.m_Values[i] == null )
						titles.m_Values[i] = new TitleInfo();

					TitleInfo.Serialize( writer, titles.m_Values[i] );
				}
			}

			public static void CheckAtrophy( PlayerMobile pm )
			{
				ChampionTitleInfo t = pm.m_ChampionTitles;
				if( t == null )
					return;

				if( t.m_Values == null )
					t.m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				for( int i = 0; i < t.m_Values.Length; i++ )
				{
					if( (t.GetLastDecay( i ) + LossDelay) < DateTime.Now )
					{
						t.Atrophy( i, LossAmount );
					}
				}
			}

			public static void AwardHarrowerTitle( PlayerMobile pm )	//Called when killing a harrower.  Will give a minimum of 1 point.
			{
				ChampionTitleInfo t = pm.m_ChampionTitles;
				if( t == null )
					return;

				if( t.m_Values == null )
					t.m_Values = new TitleInfo[ChampionSpawnInfo.Table.Length];

				int count = 1;

				for( int i = 0; i < t.m_Values.Length; i++ )
				{
					if( t.m_Values[i].Value > 900 )
						count++;
				}

				t.m_Ghost = Math.Max( count, t.m_Ghost );	//Ghost titles never decay.
			}
		}
		#endregion

		#region Recipes

		private Dictionary<int, bool> m_AcquiredRecipes;
		
		public virtual bool HasRecipe( Recipe r )
		{
			if( r == null ) 
				return false;

			return HasRecipe( r.ID );
		}

		public virtual bool HasRecipe( int recipeID )
		{
			if( m_AcquiredRecipes != null && m_AcquiredRecipes.ContainsKey( recipeID ) )
				return m_AcquiredRecipes[recipeID];

			return false;
		}

		public virtual void AcquireRecipe( int recipeID )
		{
			if( m_AcquiredRecipes == null )
				m_AcquiredRecipes = new Dictionary<int, bool>();

			m_AcquiredRecipes.Add( recipeID, true );
		}

		public virtual void AcquireRecipe( Recipe r )
		{
			if( r != null )
				AcquireRecipe( r.ID );
		}

		public virtual void ResetRecipes()
		{
			m_AcquiredRecipes = null;
		}
	
		[CommandProperty( AccessLevel.GameMaster )]
		public int KnownRecipes
		{
			get 
			{
				if( m_AcquiredRecipes == null )
					return 0;

				return m_AcquiredRecipes.Count;
			}
		}
	

		#endregion

		#region Buff Icons

		public void ResendBuffs()
		{
			if( !BuffInfo.Enabled || m_BuffTable == null )
				return;

			NetState state = this.NetState;

			if( state != null && state.Version >= BuffInfo.RequiredClient )
			{
				foreach( BuffInfo info in m_BuffTable.Values )
				{
					state.Send( new AddBuffPacket( this, info ) );
				}
			}
		}

		private Dictionary<BuffIcon, BuffInfo> m_BuffTable;

		public void AddBuff( BuffInfo b )
		{
			if( !BuffInfo.Enabled || b == null )
				return;

			RemoveBuff( b );	//Check & subsequently remove the old one.

			if( m_BuffTable == null )
				m_BuffTable = new Dictionary<BuffIcon, BuffInfo>();

			m_BuffTable.Add( b.ID, b );

			NetState state = this.NetState;

			if( state != null && state.Version >= BuffInfo.RequiredClient )
			{
				state.Send( new AddBuffPacket( this, b ) );
			}
		}

		public void RemoveBuff( BuffInfo b )
		{
			if( b == null )
				return;

			RemoveBuff( b.ID );
		}

		public void RemoveBuff( BuffIcon b )
		{
			if( m_BuffTable == null || !m_BuffTable.ContainsKey( b ) )
				return;

			BuffInfo info = m_BuffTable[b];

			if( info.Timer != null && info.Timer.Running )
				info.Timer.Stop();

			m_BuffTable.Remove( b );

			NetState state = this.NetState;

			if( state != null && state.Version >= BuffInfo.RequiredClient )
			{
				state.Send( new RemoveBuffPacket( this, b ) );
			}

			if( m_BuffTable.Count <= 0 )
				m_BuffTable = null;
		}
		#endregion
	}
}
