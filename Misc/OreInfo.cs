using System;
using System.Collections;

namespace Server.Items
{
	public enum CraftResource
	{
		None = 0,
		Copper = 1,
		Bronze = 2,
		Iron = 3,
		Gold = 4,
		Silver = 5,
        Obsidian = 6,
        Steel = 7,
        Tin = 8,
        Starmetal = 9,
        Electrum = 10,

		RegularLeather = 101,
		ThickLeather,
		BeastLeather,
		ScaledLeather,

		RedScales = 201,
		YellowScales,
		BlackScales,
		GreenScales,
		WhiteScales,
		BlueScales,
		
		Oak = 301,
		Yew = 302,
		Redwood = 303,
		Ash = 304,
		Greenheart = 305,
		
		Cotton = 401,
		Linen = 402,
        Silk = 403,
        Satin = 404,
        Velvet = 405,
        Wool = 406
	}

	public enum CraftResourceType
	{
		None,
		Metal,
		Leather,
		Scales,
		Wood,
		Cloth
	}

	public class CraftAttributeInfo
	{
		private int m_WeaponFireDamage;
		private int m_WeaponColdDamage;
		private int m_WeaponPoisonDamage;
		private int m_WeaponEnergyDamage;
		private int m_WeaponBluntDamage;
		private int m_WeaponSlashingDamage;
		private int m_WeaponPiercingDamage;
		private int m_WeaponDurability;
		private int m_WeaponLuck;
		private int m_WeaponGoldIncrease;
		private int m_WeaponLowerRequirements;

		private int m_ArmorPhysicalResist;
		private int m_ArmorFireResist;
		private int m_ArmorColdResist;
		private int m_ArmorPoisonResist;
		private int m_ArmorEnergyResist;
		private int m_ArmorBluntResist;
		private int m_ArmorSlashingResist;
		private int m_ArmorPiercingResist;
		private int m_ArmorDurability;
		private int m_ArmorLuck;
		private int m_ArmorGoldIncrease;
		private int m_ArmorLowerRequirements;

		private int m_RunicMinAttributes;
		private int m_RunicMaxAttributes;
		private int m_RunicMinIntensity;
		private int m_RunicMaxIntensity;

		public int WeaponFireDamage{ get{ return m_WeaponFireDamage; } set{ m_WeaponFireDamage = value; } }
		public int WeaponColdDamage{ get{ return m_WeaponColdDamage; } set{ m_WeaponColdDamage = value; } }
		public int WeaponPoisonDamage{ get{ return m_WeaponPoisonDamage; } set{ m_WeaponPoisonDamage = value; } }
		public int WeaponEnergyDamage{ get{ return m_WeaponEnergyDamage; } set{ m_WeaponEnergyDamage = value; } }
		public int WeaponDurability{ get{ return m_WeaponDurability; } set{ m_WeaponDurability = value; } }
		public int WeaponLuck{ get{ return m_WeaponLuck; } set{ m_WeaponLuck = value; } }
		public int WeaponGoldIncrease{ get{ return m_WeaponGoldIncrease; } set{ m_WeaponGoldIncrease = value; } }
		public int WeaponLowerRequirements{ get{ return m_WeaponLowerRequirements; } set{ m_WeaponLowerRequirements = value; } }

		public int WeaponBluntDamage{ get{ return m_WeaponBluntDamage; } set{ m_WeaponBluntDamage = value; } }
		public int WeaponSlashingDamage{ get{ return m_WeaponSlashingDamage; } set{ m_WeaponSlashingDamage = value; } }
		public int WeaponPiercingDamage{ get{ return m_WeaponPiercingDamage; } set{ m_WeaponPiercingDamage = value; } }
		
		public int ArmorPhysicalResist{ get{ return m_ArmorPhysicalResist; } set{ m_ArmorPhysicalResist = value; } }
		public int ArmorFireResist{ get{ return m_ArmorFireResist; } set{ m_ArmorFireResist = value; } }
		public int ArmorColdResist{ get{ return m_ArmorColdResist; } set{ m_ArmorColdResist = value; } }
		public int ArmorPoisonResist{ get{ return m_ArmorPoisonResist; } set{ m_ArmorPoisonResist = value; } }
		public int ArmorEnergyResist{ get{ return m_ArmorEnergyResist; } set{ m_ArmorEnergyResist = value; } }
		public int ArmorDurability{ get{ return m_ArmorDurability; } set{ m_ArmorDurability = value; } }
		public int ArmorLuck{ get{ return m_ArmorLuck; } set{ m_ArmorLuck = value; } }
		public int ArmorGoldIncrease{ get{ return m_ArmorGoldIncrease; } set{ m_ArmorGoldIncrease = value; } }
		public int ArmorLowerRequirements{ get{ return m_ArmorLowerRequirements; } set{ m_ArmorLowerRequirements = value; } }

		public int ArmorBluntResist{ get{ return m_ArmorBluntResist; } set{ m_ArmorBluntResist = value; } }
		public int ArmorSlashingResist{ get{ return m_ArmorSlashingResist; } set{ m_ArmorSlashingResist = value; } }
		public int ArmorPiercingResist{ get{ return m_ArmorPiercingResist; } set{ m_ArmorPiercingResist = value; } }
		
		public int RunicMinAttributes{ get{ return m_RunicMinAttributes; } set{ m_RunicMinAttributes = value; } }
		public int RunicMaxAttributes{ get{ return m_RunicMaxAttributes; } set{ m_RunicMaxAttributes = value; } }
		public int RunicMinIntensity{ get{ return m_RunicMinIntensity; } set{ m_RunicMinIntensity = value; } }
		public int RunicMaxIntensity{ get{ return m_RunicMaxIntensity; } set{ m_RunicMaxIntensity = value; } }

		public CraftAttributeInfo()
		{
		}

		public static readonly CraftAttributeInfo Blank;
		public static readonly CraftAttributeInfo Copper, Bronze, Iron, Golden, Silver, Obsidian, Steel, Tin, Starmetal, Electrum;
		public static readonly CraftAttributeInfo Regular, Thick, Beast, Scaled;
		public static readonly CraftAttributeInfo RedScales, YellowScales, BlackScales, GreenScales, WhiteScales, BlueScales;
		public static readonly CraftAttributeInfo Oak, Yew, Redwood, Ash, Greenheart;
		public static readonly CraftAttributeInfo Cotton, Linen, Silk, Satin, Velvet, Wool;
			
		static CraftAttributeInfo()
		{
			Blank = new CraftAttributeInfo();

			CraftAttributeInfo copper = Copper = new CraftAttributeInfo();
			
			copper.ArmorBluntResist = 0;
			copper.ArmorSlashingResist = 2;
			copper.ArmorPiercingResist = 0;
			copper.ArmorPhysicalResist = 0;
			copper.ArmorFireResist = 0;
			copper.ArmorPoisonResist = 0;
			copper.ArmorEnergyResist = 0;
			copper.WeaponPoisonDamage = 0;
			copper.WeaponEnergyDamage = 0;
			copper.RunicMinAttributes = 0;
			copper.RunicMaxAttributes = 0;
			copper.RunicMinIntensity = 0;
			copper.RunicMaxIntensity = 0;

			CraftAttributeInfo bronze = Bronze = new CraftAttributeInfo();

			bronze.ArmorBluntResist = 0;
			bronze.ArmorSlashingResist = 0;
			bronze.ArmorPiercingResist = 2;
			bronze.ArmorPhysicalResist = 0;
			bronze.ArmorColdResist = 0;
			bronze.ArmorPoisonResist = 0;
			bronze.ArmorEnergyResist = 0;
			bronze.ArmorDurability = 50;
			bronze.WeaponFireDamage = 0;
			bronze.WeaponDurability = 50;
			bronze.RunicMinAttributes = 0;
			bronze.RunicMaxAttributes = 0;
			bronze.RunicMinIntensity = 0;
			bronze.RunicMaxIntensity = 0;
			
			CraftAttributeInfo iron = Iron = new CraftAttributeInfo();
			
			iron.ArmorBluntResist = 2;
			iron.ArmorSlashingResist = 0;
			iron.ArmorPiercingResist = 0;
			iron.ArmorPhysicalResist = 0;
			iron.ArmorFireResist = 0;
			iron.ArmorPoisonResist = 0;
			iron.ArmorEnergyResist = 0;
			iron.ArmorDurability = 25;
			iron.WeaponPoisonDamage = 0;
			iron.WeaponEnergyDamage = 0;
			iron.WeaponDurability = 25;
			iron.RunicMinAttributes = 0;
			iron.RunicMaxAttributes = 0;
			iron.RunicMinIntensity = 0;
			iron.RunicMaxIntensity = 0;

			CraftAttributeInfo golden = Golden = new CraftAttributeInfo();
			
			golden.ArmorBluntResist = 0;
			golden.ArmorSlashingResist = 0;
			golden.ArmorPiercingResist = 0;
			golden.ArmorPhysicalResist = 0;
			golden.ArmorFireResist = -1;
			golden.ArmorColdResist = 0;
			golden.ArmorEnergyResist = 0;
			golden.ArmorLuck = 0;
			golden.ArmorLowerRequirements = 0;
			golden.ArmorDurability = -10;
			golden.WeaponLuck = 0;
			golden.WeaponLowerRequirements = 0;
			golden.WeaponDurability = -10;
			golden.RunicMinAttributes = 0;
			golden.RunicMaxAttributes = 0;
			golden.RunicMinIntensity = 0;
			golden.RunicMaxIntensity = 0;

			CraftAttributeInfo silver = Silver = new CraftAttributeInfo();

			silver.ArmorBluntResist = 0;
			silver.ArmorSlashingResist = 0;
			silver.ArmorPiercingResist = 0;
			silver.ArmorPhysicalResist = 0;
			silver.ArmorFireResist = 0;
			silver.ArmorColdResist = 0;
			silver.ArmorPoisonResist = 0;
			silver.ArmorEnergyResist = 0;
			silver.WeaponColdDamage = 0;
			silver.WeaponEnergyDamage = 0;
			silver.RunicMinAttributes = 0;
			silver.RunicMaxAttributes = 0;
			silver.RunicMinIntensity = 0;
			silver.RunicMaxIntensity = 0;

			CraftAttributeInfo obsidian = Obsidian = new CraftAttributeInfo();

			obsidian.ArmorBluntResist = 0;
			obsidian.ArmorSlashingResist = 0;
			obsidian.ArmorPiercingResist = 0;
			obsidian.ArmorPhysicalResist = 0;
			obsidian.ArmorFireResist = 0;
			obsidian.ArmorColdResist = 0;
			obsidian.ArmorPoisonResist = 0;
			obsidian.ArmorEnergyResist = 0;
			obsidian.ArmorDurability = 0;
			obsidian.WeaponPoisonDamage = 0;
			obsidian.WeaponEnergyDamage = 0;
			obsidian.WeaponDurability = 0;
			obsidian.RunicMinAttributes = 0;
			obsidian.RunicMaxAttributes = 0;
			obsidian.RunicMinIntensity = 0;
			obsidian.RunicMaxIntensity = 0;

			CraftAttributeInfo steel = Steel = new CraftAttributeInfo();
			
			steel.ArmorBluntResist = 1;
			steel.ArmorSlashingResist = 1;
			steel.ArmorPiercingResist = 1;
			steel.ArmorPhysicalResist = 0;
			steel.ArmorColdResist = 1;
			steel.ArmorFireResist = 1;
			steel.ArmorPoisonResist = 0;
			steel.ArmorEnergyResist = 0;
			steel.ArmorDurability = 75;
			steel.WeaponFireDamage = 0;
			steel.WeaponColdDamage = 0;
			steel.WeaponPoisonDamage = 0;
			steel.WeaponDurability = 75;
			steel.WeaponEnergyDamage = 0;
			steel.RunicMinAttributes = 0;
			steel.RunicMaxAttributes = 0;
			steel.RunicMinIntensity = 0;
			steel.RunicMaxIntensity = 0;
			
			CraftAttributeInfo tin = Tin = new CraftAttributeInfo();

			tin.ArmorBluntResist = 0;
			tin.ArmorSlashingResist = 0;
			tin.ArmorPiercingResist = 0;
			tin.ArmorPhysicalResist = 0;
			tin.ArmorColdResist = 0;
			tin.ArmorPoisonResist = 0;
			tin.ArmorEnergyResist = 0;
			tin.ArmorDurability = 0;
			tin.WeaponFireDamage = 0;
			tin.WeaponColdDamage = 0;
			tin.WeaponPoisonDamage = 0;
			tin.WeaponEnergyDamage = 0;
			tin.WeaponDurability = 0;
			tin.RunicMinAttributes = 0;
			tin.RunicMaxAttributes = 0;
			tin.RunicMinIntensity = 0;
			tin.RunicMaxIntensity = 0;
			
			CraftAttributeInfo starmetal = Starmetal = new CraftAttributeInfo();
			
			starmetal.ArmorBluntResist = 2;
			starmetal.ArmorSlashingResist = 2;
			starmetal.ArmorPiercingResist = 2;
			starmetal.ArmorPhysicalResist = 0;
			starmetal.ArmorColdResist = 1;
			starmetal.ArmorFireResist = 0;
			starmetal.ArmorPoisonResist = 0;
			starmetal.ArmorEnergyResist = 1;
			starmetal.ArmorDurability = 100;
			starmetal.WeaponFireDamage = 0;
			starmetal.WeaponColdDamage = 0;
			starmetal.WeaponPoisonDamage = 0;
			starmetal.WeaponDurability = 100;
			starmetal.WeaponEnergyDamage = 0;
			starmetal.RunicMinAttributes = 0;
			starmetal.RunicMaxAttributes = 0;
			starmetal.RunicMinIntensity = 0;
			starmetal.RunicMaxIntensity = 0;
			
			CraftAttributeInfo electrum = Electrum = new CraftAttributeInfo();
			
			electrum.ArmorBluntResist = 0;
			electrum.ArmorSlashingResist = 0;
			electrum.ArmorPiercingResist = 0;
			electrum.ArmorPhysicalResist = 0;
			electrum.ArmorDurability = 70;
			electrum.WeaponDurability = 70;
			electrum.ArmorFireResist = 8;
			electrum.ArmorColdResist = 8;
			electrum.ArmorPoisonResist = 0;
			electrum.ArmorEnergyResist = 8;
			electrum.WeaponPoisonDamage = 0;
			electrum.WeaponEnergyDamage = 0;
			electrum.RunicMinAttributes = 0;
			electrum.RunicMaxAttributes = 0;
			electrum.RunicMinIntensity = 0;
			electrum.RunicMaxIntensity = 0;

			CraftAttributeInfo regular = Regular = new CraftAttributeInfo();

			regular.ArmorBluntResist = 0;
			regular.ArmorSlashingResist = 1;
			regular.ArmorPiercingResist = 1;
			regular.WeaponFireDamage = 0;
			regular.ArmorColdResist = 0;
			regular.ArmorPhysicalResist = 0;
			regular.RunicMinAttributes = 0;
			regular.RunicMaxAttributes = 0;
			regular.RunicMinIntensity = 0;
			regular.RunicMaxIntensity = 0;
			
			CraftAttributeInfo thick = Thick = new CraftAttributeInfo();

			thick.ArmorBluntResist = 0;
			thick.ArmorSlashingResist = 0;
			thick.ArmorPiercingResist = 2;
			thick.WeaponFireDamage = 0;
			thick.ArmorColdResist = 0;
			thick.ArmorPhysicalResist = 0;
			thick.RunicMinAttributes = 0;
			thick.RunicMaxAttributes = 0;
			thick.RunicMinIntensity = 0;
			thick.RunicMaxIntensity = 0;

			CraftAttributeInfo beast = Beast = new CraftAttributeInfo();

			beast.ArmorBluntResist = 0;
			beast.ArmorSlashingResist = 2;
			beast.ArmorPiercingResist = 0;
			beast.WeaponFireDamage = 0;
			beast.ArmorColdResist = 0;
			beast.ArmorPhysicalResist = 0;
			beast.ArmorPoisonResist = 0;
			beast.ArmorEnergyResist = 0;
			beast.RunicMinAttributes = 0;
			beast.RunicMaxAttributes = 0;
			beast.RunicMinIntensity = 0;
			beast.RunicMaxIntensity = 0;

			CraftAttributeInfo scaled = Scaled = new CraftAttributeInfo();

			scaled.ArmorBluntResist = 2;
			scaled.ArmorSlashingResist = 0;
			scaled.ArmorPiercingResist = 0;
			scaled.WeaponFireDamage = 0;
			scaled.ArmorColdResist = 0;
			scaled.ArmorPhysicalResist = 0;
			scaled.ArmorPoisonResist = 0;
			scaled.ArmorEnergyResist = 0;
			scaled.RunicMinAttributes = 0;
			scaled.RunicMaxAttributes = 0;
			scaled.RunicMinIntensity = 0;
			scaled.RunicMaxIntensity = 0;

			CraftAttributeInfo red = RedScales = new CraftAttributeInfo();

			red.ArmorFireResist = 10;
			red.ArmorColdResist = -3;

			CraftAttributeInfo yellow = YellowScales = new CraftAttributeInfo();

			yellow.ArmorPhysicalResist = -3;
			yellow.ArmorLuck = 20;

			CraftAttributeInfo black = BlackScales = new CraftAttributeInfo();

			black.ArmorPhysicalResist = 10;
			black.ArmorEnergyResist = -3;

			CraftAttributeInfo green = GreenScales = new CraftAttributeInfo();

			green.ArmorFireResist = -3;
			green.ArmorPoisonResist = 10;

			CraftAttributeInfo white = WhiteScales = new CraftAttributeInfo();

			white.ArmorPhysicalResist = -3;
			white.ArmorColdResist = 10;

			CraftAttributeInfo blue = BlueScales = new CraftAttributeInfo();

			blue.ArmorPoisonResist = -3;
			blue.ArmorEnergyResist = 10;
			
			CraftAttributeInfo oak = Oak = new CraftAttributeInfo();
			
			oak.ArmorBluntResist = 0;
			oak.ArmorSlashingResist = 0;
			oak.ArmorPiercingResist = 2;
			oak.ArmorPhysicalResist = 0;
			oak.ArmorFireResist = 0;
			oak.ArmorPoisonResist = 0;
			oak.ArmorEnergyResist = 0;
			oak.ArmorDurability = 10;
			oak.WeaponPoisonDamage = 0;
			oak.WeaponEnergyDamage = 0;
			oak.WeaponDurability = 10;
			oak.RunicMinAttributes = 0;
			oak.RunicMaxAttributes = 0;
			oak.RunicMinIntensity = 0;
			oak.RunicMaxIntensity = 0;
			
			CraftAttributeInfo yew = Yew = new CraftAttributeInfo();
			
			yew.ArmorBluntResist = 0;
			yew.ArmorSlashingResist = 2;
			yew.ArmorPiercingResist = 0;
			yew.ArmorPhysicalResist = 0;
			yew.ArmorFireResist = 0;
			yew.ArmorPoisonResist = 0;
			yew.ArmorEnergyResist = 0;
			yew.ArmorDurability = 25;
			yew.WeaponPoisonDamage = 0;
			yew.WeaponEnergyDamage = 0;
			yew.WeaponDurability = 25;
			yew.RunicMinAttributes = 0;
			yew.RunicMaxAttributes = 0;
			yew.RunicMinIntensity = 0;
			yew.RunicMaxIntensity = 0;
			
			CraftAttributeInfo redwood = Redwood = new CraftAttributeInfo();
			
			redwood.ArmorBluntResist = 2;
			redwood.ArmorSlashingResist = 0;
			redwood.ArmorPiercingResist = 0;
			redwood.ArmorPhysicalResist = 0;
			redwood.ArmorFireResist = 0;
			redwood.ArmorPoisonResist = 0;
			redwood.ArmorEnergyResist = 0;
			redwood.ArmorDurability = 50;
			redwood.WeaponPoisonDamage = 0;
			redwood.WeaponEnergyDamage = 0;
			redwood.WeaponDurability = 50;
			redwood.RunicMinAttributes = 0;
			redwood.RunicMaxAttributes = 0;
			redwood.RunicMinIntensity = 0;
			redwood.RunicMaxIntensity = 0;
			
			CraftAttributeInfo ash = Ash = new CraftAttributeInfo();
			
			ash.ArmorBluntResist = 1;
			ash.ArmorSlashingResist = 1;
			ash.ArmorPiercingResist = 1;
			ash.ArmorPhysicalResist = 0;
			ash.ArmorFireResist = 0;
			ash.ArmorPoisonResist = 0;
			ash.ArmorEnergyResist = 0;
			ash.ArmorDurability = 75;
			ash.WeaponPoisonDamage = 0;
			ash.WeaponEnergyDamage = 0;
			ash.WeaponDurability = 75;
			ash.RunicMinAttributes = 0;
			ash.RunicMaxAttributes = 0;
			ash.RunicMinIntensity = 0;
			ash.RunicMaxIntensity = 0;
			
			CraftAttributeInfo greenheart = Greenheart = new CraftAttributeInfo();
			
			greenheart.ArmorBluntResist = 2;
			greenheart.ArmorSlashingResist = 0;
			greenheart.ArmorPiercingResist = 2;
			greenheart.ArmorPhysicalResist = 0;
			greenheart.ArmorFireResist = 0;
			greenheart.ArmorPoisonResist = 0;
			greenheart.ArmorEnergyResist = 0;
			greenheart.ArmorDurability = 100;
			greenheart.WeaponPoisonDamage = 0;
			greenheart.WeaponEnergyDamage = 0;
			greenheart.WeaponDurability = 100;
			greenheart.RunicMinAttributes = 0;
			greenheart.RunicMaxAttributes = 0;
			greenheart.RunicMinIntensity = 0;
			greenheart.RunicMaxIntensity = 0;
			
			CraftAttributeInfo cotton = Cotton = new CraftAttributeInfo();
			
			cotton.ArmorBluntResist = 0;

			CraftAttributeInfo linen = Linen = new CraftAttributeInfo();
			
			linen.ArmorBluntResist = 0;

            CraftAttributeInfo silk = Silk = new CraftAttributeInfo();

            silk.ArmorBluntResist = 0;

            CraftAttributeInfo satin = Satin = new CraftAttributeInfo();

            satin.ArmorBluntResist = 0;

            CraftAttributeInfo velvet = Velvet = new CraftAttributeInfo();

            velvet.ArmorBluntResist = 0;

            CraftAttributeInfo wool = Wool = new CraftAttributeInfo();

            wool.ArmorBluntResist = 0;

		}
	}

	public class CraftResourceInfo
	{
		private int m_Hue;
		private int m_Number;
		private string m_Name;
		private CraftAttributeInfo m_AttributeInfo;
		private CraftResource m_Resource;
		private Type[] m_ResourceTypes;

		public int Hue{ get{ return m_Hue; } }
		public int Number{ get{ return m_Number; } }
		public string Name{ get{ return m_Name; } }
		public CraftAttributeInfo AttributeInfo{ get{ return m_AttributeInfo; } }
		public CraftResource Resource{ get{ return m_Resource; } }
		public Type[] ResourceTypes{ get{ return m_ResourceTypes; } }

		public CraftResourceInfo( int hue, int number, string name, CraftAttributeInfo attributeInfo, CraftResource resource, params Type[] resourceTypes )
		{
			m_Hue = hue;
			m_Number = number;
			m_Name = name;
			m_AttributeInfo = attributeInfo;
			m_Resource = resource;
			m_ResourceTypes = resourceTypes;

			for ( int i = 0; i < resourceTypes.Length; ++i )
				CraftResources.RegisterType( resourceTypes[i], resource );
		}
	}

	public class CraftResources
	{
		public static int GetResourceHue( CraftResource resource )
		{
			foreach( CraftResourceInfo c in m_MetalInfo )
			{
				if( c.Resource == resource )
					return c.Hue;
			}
			
			foreach( CraftResourceInfo c in m_LeatherInfo )
			{
				if( c.Resource == resource )
					return c.Hue;
			}
			
			foreach( CraftResourceInfo c in m_WoodInfo )
			{
				if( c.Resource == resource )
					return c.Hue;
			}
			
			foreach( CraftResourceInfo c in m_ClothInfo )
			{
				if( c.Resource == resource )
					return c.Hue;
			}
			
			return 0;
		}
		
		public static Type GetResourceType( CraftResource resource )
		{
			foreach( CraftResourceInfo c in m_MetalInfo )
			{
				if( c.Resource == resource )
					return c.ResourceTypes[0];
			}
			
			foreach( CraftResourceInfo c in m_LeatherInfo )
			{
				if( c.Resource == resource )
					return c.ResourceTypes[0];
			}
			
			foreach( CraftResourceInfo c in m_WoodInfo )
			{
				if( c.Resource == resource )
					return c.ResourceTypes[0];
			}
			
			foreach( CraftResourceInfo c in m_ClothInfo )
			{
				if( c.Resource == resource )
					return c.ResourceTypes[0];
			}
			
			return null;
		}
		
		private static CraftResourceInfo[] m_MetalInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x96D, 1053106, "Copper",		CraftAttributeInfo.Copper,		CraftResource.Copper,			typeof( CopperIngot ),		typeof( CopperOre ),		typeof( CopperGranite ) ),
				new CraftResourceInfo( 0x972, 1053105, "Bronze",		CraftAttributeInfo.Bronze,		CraftResource.Bronze,			typeof( BronzeIngot ),		typeof( BronzeOre ),		typeof( BronzeGranite ) ),
				new CraftResourceInfo( 0x000, 1053109, "Iron",			CraftAttributeInfo.Iron,		CraftResource.Iron,				typeof( IronIngot ),		typeof( IronOre ),			typeof( Granite ) ),
				new CraftResourceInfo( 0xB77, 1053104, "Gold",			CraftAttributeInfo.Golden,		CraftResource.Gold,				typeof( GoldIngot ),		typeof( GoldOre ),			typeof( GoldGranite ) ),
				new CraftResourceInfo( 0xBA9, 1053107, "Silver",		CraftAttributeInfo.Silver,		CraftResource.Silver,			typeof( SilverIngot ),		typeof( SilverOre ),		typeof( SilverGranite ) ),
				new CraftResourceInfo( 0xBAD, 1053103, "Obsidian",		CraftAttributeInfo.Obsidian,	CraftResource.Obsidian,			typeof( ObsidianIngot ),	typeof( ObsidianOre ),		typeof( ObsidianGranite ) ),
                new CraftResourceInfo( 0x579, 1053102, "Steel",	    	CraftAttributeInfo.Steel,   	CraftResource.Steel,			typeof( SteelIngot ),	    typeof( SteelOre ),		    typeof( SteelGranite ) ),
				new CraftResourceInfo( 0x836, 1053101, "Tin",	    	CraftAttributeInfo.Tin,   	    CraftResource.Tin,			    typeof( TinIngot ),	        typeof( TinOre ),		    typeof( SteelGranite ) ),
				new CraftResourceInfo( 0xB10, 1053108, "Starmetal",	    CraftAttributeInfo.Starmetal,   CraftResource.Starmetal,		typeof( StarmetalIngot ),   typeof( StarmetalOre ),		typeof( Granite ) ),
				new CraftResourceInfo( 0xA6D, 1053110, "Electrum",			CraftAttributeInfo.Electrum,		CraftResource.Electrum,				typeof( ElectrumIngot ),		typeof( ElectrumOre ),			typeof( ElectrumGranite ) )
			};

		private static CraftResourceInfo[] m_ScaleInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x66D, 1053129, "Red Scales",	CraftAttributeInfo.RedScales,		CraftResource.RedScales,		typeof( RedScales ) ),
				new CraftResourceInfo( 0x8A8, 1053130, "Yellow Scales",	CraftAttributeInfo.YellowScales,	CraftResource.YellowScales,		typeof( YellowScales ) ),
				new CraftResourceInfo( 0x455, 1053131, "Black Scales",	CraftAttributeInfo.BlackScales,		CraftResource.BlackScales,		typeof( BlackScales ) ),
				new CraftResourceInfo( 0x851, 1053132, "Green Scales",	CraftAttributeInfo.GreenScales,		CraftResource.GreenScales,		typeof( GreenScales ) ),
				new CraftResourceInfo( 0x8FD, 1053133, "White Scales",	CraftAttributeInfo.WhiteScales,		CraftResource.WhiteScales,		typeof( WhiteScales ) ),
				new CraftResourceInfo( 0x8B0, 1053134, "Blue Scales",	CraftAttributeInfo.BlueScales,		CraftResource.BlueScales,		typeof( BlueScales ) )
			};

		private static CraftResourceInfo[] m_LeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Regular",		CraftAttributeInfo.Regular,		CraftResource.RegularLeather,	typeof( Leather ),			typeof( Hides ) ),
				new CraftResourceInfo( 0x283, 1049354, "Thick",		CraftAttributeInfo.Thick,		CraftResource.ThickLeather,	typeof( ThickLeather ),	typeof( ThickHides ) ),
				new CraftResourceInfo( 0x227, 1049355, "Beast",		CraftAttributeInfo.Beast,		CraftResource.BeastLeather,	typeof( BeastLeather ),	typeof( BeastHides ) ),
				new CraftResourceInfo( 0x1C1, 1049356, "Scaled",		CraftAttributeInfo.Scaled,		CraftResource.ScaledLeather,	typeof( ScaledLeather ),	typeof( ScaledHides ) )
		};

		private static CraftResourceInfo[] m_AOSLeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Regular",		CraftAttributeInfo.Regular,		CraftResource.RegularLeather,	typeof( Leather ),			typeof( Hides ) ),
				new CraftResourceInfo( 0x907, 1049354, "Thick",		CraftAttributeInfo.Thick,		CraftResource.ThickLeather,	typeof( ThickLeather ),	typeof( ThickHides ) ),
				new CraftResourceInfo( 0x965, 1049355, "Beast",		CraftAttributeInfo.Beast,		CraftResource.BeastLeather,	typeof( BeastLeather ),	typeof( BeastHides ) ),
				new CraftResourceInfo( 0x5A5, 1049356, "Scaled",		CraftAttributeInfo.Scaled,		CraftResource.ScaledLeather,	typeof( ScaledLeather ),	typeof( ScaledHides ) ),
		};
		
		private static CraftResourceInfo[] m_WoodInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1063511, "Oak",			CraftAttributeInfo.Oak,				CraftResource.Oak,				typeof( Log ) ),
				new CraftResourceInfo( 0x96D, 1063512, "Yew",			CraftAttributeInfo.Yew,				CraftResource.Yew,				typeof( YewLog ) ),
				new CraftResourceInfo( 0x4AA, 1063513, "Redwood",		CraftAttributeInfo.Redwood,			CraftResource.Redwood,			typeof( RedwoodLog ) ),
				new CraftResourceInfo( 0x966, 1063514, "Ash",			CraftAttributeInfo.Ash,				CraftResource.Ash,				typeof( AshLog ) ),
				new CraftResourceInfo( 0xBAA, 1063515, "Greenheart",	CraftAttributeInfo.Greenheart,		CraftResource.Greenheart,		typeof( GreenheartLog ) ),
			};
		
		private static CraftResourceInfo[] m_ClothInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1063525, "Cotton",		CraftAttributeInfo.Cotton,				CraftResource.Cotton,		typeof( Cloth ) ),
				new CraftResourceInfo( 0x227, 1063526, "Linen",			CraftAttributeInfo.Linen,				CraftResource.Linen,		typeof( Linen ) ),
			    new CraftResourceInfo( 2995, 1063539, "Silk",			CraftAttributeInfo.Silk,				CraftResource.Silk,		    typeof( SpidersSilk ) ),
                new CraftResourceInfo( 2725, 1063541, "Satin",			CraftAttributeInfo.Satin,				CraftResource.Satin,		typeof( Satin ) ),
                new CraftResourceInfo( 2799, 1063543, "Velvet",			CraftAttributeInfo.Velvet,				CraftResource.Velvet,		typeof( Velvet ) ),
                new CraftResourceInfo( 0x000, 1063545, "Wool",			CraftAttributeInfo.Wool,				CraftResource.Wool,			typeof( Wool ) ),
            };

		/// <summary>
		/// Returns true if '<paramref name="resource"/>' is None, Iron, or RegularLeather. False if otherwise.
		/// </summary>
		public static bool IsStandard( CraftResource resource )
		{
			return ( resource == CraftResource.None );
		}

		private static Hashtable m_TypeTable;

		/// <summary>
		/// Registers that '<paramref name="resourceType"/>' uses '<paramref name="resource"/>' so that it can later be queried by <see cref="CraftResources.GetFromType"/>
		/// </summary>
		public static void RegisterType( Type resourceType, CraftResource resource )
		{
			if ( m_TypeTable == null )
				m_TypeTable = new Hashtable();

			m_TypeTable[resourceType] = resource;
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value for which '<paramref name="resourceType"/>' uses -or- CraftResource.None if an unregistered type was specified.
		/// </summary>
		public static CraftResource GetFromType( Type resourceType )
		{
			if ( m_TypeTable == null )
				return CraftResource.None;

			object obj = m_TypeTable[resourceType];

			if ( !(obj is CraftResource) )
				return CraftResource.None;

			return (CraftResource)obj;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceInfo"/> instance describing '<paramref name="resource"/>' -or- null if an invalid resource was specified.
		/// </summary>
		public static CraftResourceInfo GetInfo( CraftResource resource )
		{
			CraftResourceInfo[] list = null;

			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal: list = m_MetalInfo; break;
				case CraftResourceType.Leather: list = Core.AOS ? m_AOSLeatherInfo : m_LeatherInfo; break;
				case CraftResourceType.Scales: list = m_ScaleInfo; break;
				case CraftResourceType.Wood: list = m_WoodInfo; break;
				case CraftResourceType.Cloth: list = m_ClothInfo; break;
			}

			if ( list != null )
			{
				int index = GetIndex( resource );

				if ( index >= 0 && index < list.Length )
					return list[index];
			}

			return null;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceType"/> value indiciating the type of '<paramref name="resource"/>'.
		/// </summary>
		public static CraftResourceType GetType( CraftResource resource )
		{
			if ( resource >= CraftResource.Copper && resource <= CraftResource.Electrum )
				return CraftResourceType.Metal;

			if ( resource >= CraftResource.RegularLeather && resource <= CraftResource.ScaledLeather )
				return CraftResourceType.Leather;

			if ( resource >= CraftResource.RedScales && resource <= CraftResource.BlueScales )
				return CraftResourceType.Scales;
			
			if ( resource >= CraftResource.Oak && resource <= CraftResource.Greenheart )
				return CraftResourceType.Wood;
			
			if ( resource == CraftResource.Cotton || resource == CraftResource.Linen || resource == CraftResource.Silk ||
                resource == CraftResource.Satin || resource == CraftResource.Velvet || resource == CraftResource.Wool )
				return CraftResourceType.Cloth;

			return CraftResourceType.None;
		}

		/// <summary>
		/// Returns the first <see cref="CraftResource"/> in the series of resources for which '<paramref name="resource"/>' belongs.
		/// </summary>
		public static CraftResource GetStart( CraftResource resource )
		{
			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal: return CraftResource.Copper;
				case CraftResourceType.Leather: return CraftResource.RegularLeather;
				case CraftResourceType.Scales: return CraftResource.RedScales;
				case CraftResourceType.Wood: return CraftResource.Oak;
				case CraftResourceType.Cloth: return CraftResource.Cotton;
			}

			return CraftResource.None;
		}

		/// <summary>
		/// Returns the index of '<paramref name="resource"/>' in the seriest of resources for which it belongs.
		/// </summary>
		public static int GetIndex( CraftResource resource )
		{
			CraftResource start = GetStart( resource );

			if ( start == CraftResource.None )
				return 0;

			return (int)(resource - start);
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Number"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetLocalizationNumber( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Number );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Hue"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetHue( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Hue );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Name"/> property of '<paramref name="resource"/>' -or- an empty string if the resource specified was invalid.
		/// </summary>
		public static string GetName( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? String.Empty : info.Name );
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>' -or- CraftResource.None if unable to convert.
		/// </summary>
		public static CraftResource GetFromOreInfo( OreInfo info )
		{
			if ( info.Name.IndexOf( "Thick" ) >= 0 )
				return CraftResource.ThickLeather;
			else if ( info.Name.IndexOf( "Beast" ) >= 0 )
				return CraftResource.BeastLeather;
			else if ( info.Name.IndexOf( "Scaled" ) >= 0 )
				return CraftResource.ScaledLeather;
			else if ( info.Name.IndexOf( "Leather" ) >= 0 )
				return CraftResource.RegularLeather;

			if ( info.Level == 0 )
				return CraftResource.Copper;
			else if ( info.Level == 1 )
				return CraftResource.Bronze;
			else if ( info.Level == 2 )
				return CraftResource.Iron;
			else if ( info.Level == 3 )
				return CraftResource.Gold;
			else if ( info.Level == 4 )
				return CraftResource.Silver;
			else if ( info.Level == 5 )
				return CraftResource.Obsidian;
			else if ( info.Level == 6 )
				return CraftResource.Steel;
			else if ( info.Level == 7 )
				return CraftResource.Tin;
			else if ( info.Level == 8 )
				return CraftResource.Starmetal;
			else if ( info.Level == 9 )
				return CraftResource.Electrum;
			
			if ( info.Level == 301 )
				return CraftResource.Oak;
			else if ( info.Level == 302 )
				return CraftResource.Yew;
			else if ( info.Level == 303 )
				return CraftResource.Redwood;
			else if ( info.Level == 304 )
				return CraftResource.Ash;
			else if ( info.Level == 305 )
				return CraftResource.Greenheart;
			
			if ( info.Level == 401 )
				return CraftResource.Cotton;
			else if ( info.Level == 402 )
				return CraftResource.Linen;
            else if( info.Level == 403 )
                return CraftResource.Silk;
            else if( info.Level == 404 )
                return CraftResource.Satin;
            else if( info.Level == 405 )
                return CraftResource.Velvet;
            else if( info.Level == 406 )
                return CraftResource.Wool;

			return CraftResource.None;
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>', using '<paramref name="material"/>' to help resolve leather OreInfo instances.
		/// </summary>
		public static CraftResource GetFromOreInfo( OreInfo info, ArmorMaterialType material )
		{
			if ( material == ArmorMaterialType.Studded || material == ArmorMaterialType.Leather || material == ArmorMaterialType.Thick ||
				material == ArmorMaterialType.Beast || material == ArmorMaterialType.Scaled )
			{
				if ( info.Level == 0 )
					return CraftResource.RegularLeather;
				else if ( info.Level == 1 )
					return CraftResource.ThickLeather;
				else if ( info.Level == 2 )
					return CraftResource.BeastLeather;
				else if ( info.Level == 3 )
					return CraftResource.ScaledLeather;

				return CraftResource.None;
			}

			return GetFromOreInfo( info );
		}
	}

	// NOTE: This class is only for compatability with very old RunUO versions.
	// No changes to it should be required for custom resources.
	public class OreInfo
	{
		public static readonly OreInfo Copper		= new OreInfo( 0, 0x96D, "Copper" );
		public static readonly OreInfo Bronze		= new OreInfo( 1, 0x972, "Bronze" );
		public static readonly OreInfo Iron			= new OreInfo( 2, 0x000, "Iron" );
		public static readonly OreInfo Gold			= new OreInfo( 3, 0xB77, "Gold" );
		public static readonly OreInfo Silver		= new OreInfo( 4, 0xBA9, "Silver" );
		public static readonly OreInfo Obsidian		= new OreInfo( 5, 0xBAD, "Obsidian" );
		public static readonly OreInfo Steel		= new OreInfo( 6, 0x579, "Steel" );
        public static readonly OreInfo Tin          = new OreInfo( 7, 0x836, "Tin" );
        public static readonly OreInfo Starmetal    = new OreInfo( 8, 0xB10, "Starmetal" );
        public static readonly OreInfo Electrum    = new OreInfo( 9, 0xA6D, "Electrum" );
        
        public static readonly OreInfo Oak			= new OreInfo( 301, 0x000, "Oak" );
		public static readonly OreInfo Yew			= new OreInfo( 302, 0x96D, "Yew" );
		public static readonly OreInfo Redwood		= new OreInfo( 303, 0x4AA, "Redwood" );
		public static readonly OreInfo Ash			= new OreInfo( 304, 0x966, "Ash" );
		public static readonly OreInfo Greenheart	= new OreInfo( 305, 0xBAA, "Greenheart" );
		
		public static readonly OreInfo Cotton		= new OreInfo( 401, 0x000, "Cotton" );
		public static readonly OreInfo Linen		= new OreInfo( 402, 0x227, "Linen" );
        public static readonly OreInfo Silk		    = new OreInfo( 403, 2995, "Silk" );
        public static readonly OreInfo Satin	    = new OreInfo( 404, 2995, "Satin" );
        public static readonly OreInfo Velvet	    = new OreInfo( 405, 2995, "Velvet" );
        public static readonly OreInfo Wool		    = new OreInfo( 406, 0x000, "Wool" );
		
		private int m_Level;
		private int m_Hue;
		private string m_Name;

		public OreInfo( int level, int hue, string name )
		{
			m_Level = level;
			m_Hue = hue;
			m_Name = name;
		}

		public int Level
		{
			get
			{
				return m_Level;
			}
		}

		public int Hue
		{
			get
			{
				return m_Hue;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}
	}
}
