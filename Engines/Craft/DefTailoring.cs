using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Multis;

namespace Server.Engines.Craft
{
	public class DefTailoring : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tailoring; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044005; } // <CENTER>TAILORING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefTailoring();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefTailoring() : base( 1, 1, 4.00 )// base( 1, 1, 4.5 )
		{
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1 )
		{
			if( m.Nation == nation1 )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) )
				return true;
			
			return false;			
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3, Nation nation4 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) || TestRace( m, nation4 ) )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3, Nation nation4, Nation nation5 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) || TestRace( m, nation4 ) || TestRace( m, nation5 ) )
				return true;
			
			return false;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.
			else if( from is PlayerMobile && itemType != null )
			{
				PlayerMobile m = from as PlayerMobile;
			/*	
				if( itemType == typeof( Cap ) && !TestRace( m, Nation.Northern, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WideBrimHat ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TricorneHat ) && !TestRace( m, Nation.Northern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( JesterHat ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Doublet ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Tunic ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Surcoat ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlainDress ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( JesterSuit ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantShirt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Kilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Sandals ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LeatherShorts ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LeatherSkirt ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FleshMask ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TribalWarriorMask ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( JesterMask ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RogueCowl ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CeremonialMask ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RogueMask ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Turban ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExecutionerMask ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WesternRogueMask ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HatWithMask ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MonsterMask ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( DeerMask ) && ( !TestRace( m, Nation.Mhordul ) && !TestRace( m, Nation.Southern ) && !TestRace( m, Nation.Tirebladd ) ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WolfMask ) && ( !TestRace( m, Nation.Tirebladd ) && !TestRace( m, Nation.Mhordul ) ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BearMask ) && ( !TestRace( m, Nation.Tirebladd ) && !TestRace( m, Nation.Mhordul ) ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WhiteFeatheredHat ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveHat ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Beret ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( GreenBeret ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RunicCloak ) && !TestRace( m, Nation.Haluaroc, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveCloak ) && !TestRace( m, Nation.Northern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Doublet ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HighHeels ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FurBoots ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyGloves ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveShirt ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RaggedBra ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LongRaggedBra ) && !TestRace( m, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateShirt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyBra ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExtravagantShirt ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FormalShirt ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BaggyPants ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Stockings ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RaggedPants ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LoinCloth ) && !TestRace( m, Nation.Mhordul, Nation.Western, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MaleLoinCloth ) && !TestRace( m, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FemaleLoinCloth ) && !TestRace( m, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WaistCloth ) && !TestRace( m, Nation.Mhordul, Nation.Western, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateWaistCloth ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RaggedSkirt ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SmallRaggedSkirt ) && !TestRace( m, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantSkirt ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlainKilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantKilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateKilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FemaleKilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantFemaleKilt ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantSurcoat ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantTunic ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PaddedVest ) && !TestRace( m, Nation.Northern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyDoublet ) && !TestRace( m, Nation.Haluaroc, Nation.Northern, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantVest ) && !TestRace( m, Nation.Haluaroc, Nation.Northern, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantDoublet ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancySurcoat ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ClericRobe ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( DruidRobe ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ProphetRobe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PriestessGown ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PriestRobe ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ShamanRobe ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantLongGown ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantGown ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveGown ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveLongGown ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LongGown ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LacedGown ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BeltedDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LongVest ) && !TestRace( m, Nation.Haluaroc, Nation.Northern, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( GildedGown ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( GildedFancyDress ) && !TestRace( m, Nation.Northern, Nation.Haluaroc, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveDress ) && !TestRace( m, Nation.Northern, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MaleDress ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ExpensiveShortDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Haluaroc, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateDress ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyShortDress ) && !TestRace( m, Nation.Southern, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ElegantShortDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd, Nation.Haluaroc, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PuffyDress ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RunedDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Western, Nation.Haluaroc, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlainLongVest ) && !TestRace( m, Nation.Mhordul, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LongOrnateDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd, Nation.Western, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SmallDress ) && !TestRace( m, Nation.Northern, Nation.Southern, Nation.Tirebladd, Nation.Haluaroc, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SoftLeatherPauldrons ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SoftLeatherBoots ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SoftLeatherTunic ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SoftLeatherLegs ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneGloves ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneChest ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneArms ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneLegs ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneHelm ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulHornedBoneHelm ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulHornedSkullHelm ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MedicineManBoneChest ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
					*/
			}

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x248 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
				else if ( makersMark && quality == 5 )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 5 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

			#region Hats
			index = AddCraft( typeof( SkullCap ), 1011375, 1025444, 5.0, 25.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Bandana ), 1011375, 1025440, 5.0, 25.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FloppyHat ), 1011375, 1025907, 5.0, 30.0, typeof( Cloth ), 1063525, 11, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Cap ), 1011375, 1025909, 5.0, 20.0, typeof( Cloth ), 1063525, 11, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( WideBrimHat ), 1011375, 1025908, 5.0, 30.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( StrawHat ), 1011375, 1025911, 5.0, 30.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( TallStrawHat ), 1011375, 1025910, 5.0, 30.0, typeof( Cloth ), 1063525, 13, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( TallBrimHat ), 1011375, "tall brim hat", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Bonnet ), 1011375, 1025913, 5.0, 30.0, typeof( Cloth ), 1063525, 11, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FeatheredHat ), 1011375, 1025914, 5.0, 30.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( TricorneHat ), 1011375, 1025915, 5.0, 30.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( JesterHat ), 1011375, 1025916, 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( JesterMask ), 1011375, "jester mask", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( TribalWarriorMask ), 1011375, "tribal warrior's mask", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Cowl ), 1011375, "cowl", 60, 100, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RogueCowl ), 1011375, "rogue's cowl", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Turban ), 1011375, "turban", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RogueMask ), 1011375, "rogue's mask", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( CeremonialMask ), 1011375, "ceremonial mask", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExecutionerMask ), 1011375, "executioner's mask", 5.0, 30.0, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Beret ), 1011375, "beret", 40, 80, typeof( Cloth ), 1063525, 5, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( GreenBeret ), 1011375, "green beret", 40, 80, typeof( Cloth ), 1063525, 5, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( HatWithMask ), 1011375, "hat with mask", 40, 80, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( MonsterMask ), 1011375, "monster mask", 30, 70, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( DeerMask ), 1011375, "deer mask", 30, 70, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( WolfMask ), 1011375, "wolf mask", 30, 70, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( BearMask ), 1011375, "bear mask", 30, 70, typeof( Cloth ), 1063525, 15, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( SkullMask ), 1011375, "skull mask", 30.0, 70.0, typeof( Cloth ), 1063525, 2, 1044287 );
			AddRes( index, typeof( Bone ), 1049064, 4, 1049063 );
			index = AddCraft( typeof( WhiteFeatheredHat ), 1011375, "white feathered hat", 50, 90, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveHat ), 1011375, "expensive hat", 30, 70, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );

            index = AddCraft( typeof( SurgicalMask ), 1011375, "surgical mask", 30, 70, typeof( Cloth ), 1063525, 4, 1044287 );
            SetUseSubRes2( index, true );
            index = AddCraft( typeof( SmallScarf ), 1011375, "small scarf", 30, 70, typeof( Cloth ), 1063525, 4, 1044287 );
            SetUseSubRes2( index, true );
            index = AddCraft( typeof( Scarf ), 1011375, "scarf", 30, 70, typeof( Cloth ), 1063525, 6, 1044287 );
            SetUseSubRes2( index, true );
            index = AddCraft( typeof( LargeScarf ), 1011375, "large scarf", 30, 70, typeof( Cloth ), 1063525, 8, 1044287 );
            SetUseSubRes2( index, true );
			
			#endregion

			#region Shirts
			index = AddCraft( typeof( Doublet ), 1015269, 1028059, 0, 25.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Shirt ), 1015269, 1025399, 20.0, 45.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancyShirt ), 1015269, 1027933, 25.0, 50.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Tunic ), 1015269, 1028097, 00.0, 25.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Surcoat ), 1015269, 1028189, 10.0, 30.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PlainDress ), 1015269, 1027937, 10.0, 40.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancyDress ), 1015269, 1027935, 30.0, 60.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Cloak ), 1015269, 1025397, 40.0, 65.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Robe ), 1015269, 1027939, 55.0, 80.0, typeof( Cloth ), 1063525, 16, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( JesterSuit ), 1015269, 1028095, 10.0, 35.0, typeof( Cloth ), 1063525, 24, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FormalShirt ), 1015269, 1028975, 25.0, 50.0, typeof( Cloth ), 1063525, 16, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantShirt ), 1015269, "elegant shirt", 40.0, 65.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( MetallicBra ), 1015269, "metallic bra", 40.0, 65.0, typeof( Cloth ), 1063525, 4, 1044287 );
			AddRes( index, typeof( TinIngot ), 1044036, 4, 1044037 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExtravagantShirt ), 1015269, "extravagant shirt", 60.0, 100.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveShirt ), 1015269, "expensive shirt", 40.0, 65.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( OrnateShirt ), 1015269, "ornate shirt", 60.0, 100.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongRaggedBra ), 1015269, "long ragged bra", 10.0, 35.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RaggedBra ), 1015269, "ragged bra", 10.0, 35.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancyBra ), 1015269, "fancy bra", 60.0, 100.0, typeof( Cloth ), 1063525, 5, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantCloak ), 1015269, "elegant cloak", 60.0, 95.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveCloak ), 1015269, "expensive cloak", 70.0, 105.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RunicCloak ), 1015269, "runic cloak", 70.0, 105.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantTunic ), 1015269, "elegant tunic", 45.0, 90.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantSurcoat ), 1015269, "elegant surcoat", 45.0, 90.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PaddedVest ), 1015269, "padded vest", 35.0, 80.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			AddRes( index, typeof( Fur ), 1063537, 4, 1063536 );
			index = AddCraft( typeof( FancyDoublet ), 1015269, "fancy doublet", 55.0, 100.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantVest ), 1015269, "elegant vest", 45.0, 90.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantDoublet ), 1015269, "elegant doublet", 50.0, 100.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancySurcoat ), 1015269, "fancy surcoat", 50.0, 100.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongVest ), 1015269, "long vest", 20.0, 65.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PlainLongVest ), 1015269, "plain long dress", 20.0, 60.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ClericRobe ), 1015269, "cleric robe", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ShamanRobe ), 1015269, "shaman robe", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( DruidRobe ), 1015269, "druid robe", 50.0, 95.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ProphetRobe ), 1015269, "prophet robe", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PriestessGown ), 1015269, "priestess gown", 60.0, 105.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PriestRobe ), 1015269, "priest robe", 60.0, 105.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( MaleDress ), 1015269, "male dress", 40.0, 85.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ShortPlainDress ), 1015269, "short plain dress", 10.0, 35.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongPlainDress ), 1015269, "long plain dress", 10.0, 35.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Nightgown ), 1015269, "nightgown", 30.0, 75.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongDress ), 1015269, "long dress", 20.0, 65.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RunedDress ), 1015269, "runed dress", 35.0, 75.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantShortDress ), 1015269, "elegant short dress", 40.0, 85.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancyShortDress ), 1015269, "fancy short dress", 40.0, 85.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( BeltedDress ), 1015269, "belted dress", 30.0, 75.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveDress ), 1015269, "expensive dress", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( GildedFancyDress ), 1015269, "gilded fancy dress", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PuffyDress ), 1015269, "puffy dress", 40.0, 85.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( OrnateDress ), 1015269, "ornate dress", 40.0, 85.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongOrnateDress ), 1015269, "long ornate dress", 50.0, 95.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveShortDress ), 1015269, "expensive short dress", 60.0, 105.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( SmallDress ), 1015269, "small dress", 30.0, 75.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LacedGown ), 1015269, "laced gown", 30.0, 75.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveGown ), 1015269, "expensive gown", 60.0, 105.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ExpensiveLongGown ), 1015269, "expensive long gown", 40.0, 85.0, typeof( Cloth ), 1063525, 14, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantGown ), 1015269, "elegant gown", 50.0, 95.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( GildedGown ), 1015269, "gilded gown", 40.0, 85.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongGown ), 1015269, "long gown", 50.0, 95.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantLongGown ), 1015269, "elegant long gown", 60.0, 105.0, typeof( Cloth ), 1063525, 12, 1044287 );
			SetUseSubRes2( index, true );

			
			#endregion

			#region Pants
			index = AddCraft( typeof( ShortPants ), 1015279, 1025422, 25.0, 50.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongPants ), 1015279, 1025433, 25.0, 50.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Kilt ), 1015279, 1025431, 20.0, 45.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Skirt ), 1015279, 1025398, 30.0, 55.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( BeltedPants ), 1015279, "belted pants", 60.0, 85.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( Stockings ), 1015279, "stockings", 65.0, 105.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( BaggyPants ), 1015279, "baggy pants", 50.0, 75.0, typeof( Cloth ), 1063525, 9, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RaggedPants ), 1015279, "ragged pants", 20.0, 45.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( SmallRaggedSkirt ), 1015269, "small ragged skirt", 25.0, 60.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( OrnateKilt ), 1015269, "ornate kilt", 65.0, 110.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( PlainKilt ), 1015269, "plain kilt", 25.0, 60.0, typeof( Cloth ), 1063525, 5, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FemaleKilt ), 1015269, "female kilt", 25.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantFemaleKilt ), 1015269, "elegant female kilt", 45.0, 90.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantKilt ), 1015269, "elegant kilt", 45.0, 90.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantSkirt ), 1015269, "elegant skirt", 55.0, 100.0, typeof( Cloth ), 1063525, 5, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LongSkirt ), 1015269, "long skirt", 25.0, 60.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( RaggedSkirt ), 1015269, "ragged skirt", 25.0, 60.0, typeof( Cloth ), 1063525, 7, 1044287 );
			SetUseSubRes2( index, true );
			
			#endregion

			#region Misc
			index = AddCraft( typeof( BodySash ), 1015283, 1025441, 5.0, 30.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( HalfApron ), 1015283, 1025435, 20.0, 45.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FullApron ), 1015283, 1025437, 30.0, 55.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( HairStylingApron ), 1015283, "hair styling apron", 30.0, 55.0, typeof( Cloth ), 1063525, 10, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( BeltPouch ), 1015283, "belt pouch", 40.0, 75.0, typeof( Cloth ), 1063525, 6, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( SmallBeltPouch ), 1015283, "small belt pouch", 20.0, 40.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( EyePatch ), 1015283, "eyepatch", 5.0, 40.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FancyGloves ), 1015283, "fancy gloves", 60.0, 100.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( SimpleGloves ), 1015283, "simple gloves", 40.0, 80.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( WaistSash ), 1015283, "waist sash", 40.0, 80.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( FemaleLoinCloth ), 1015283, "female loin cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( MaleLoinCloth ), 1015283, "male loin cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( LoinCloth ), 1015283, "loin cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( OrnateWaistCloth ), 1015283, "ornate waist cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( ElegantWaistCloth ), 1015283, "elegant waist cloth", 50.0, 90.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( WaistCloth ), 1015283, "waist cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			SetUseSubRes2( index, true );
			AddCraft( typeof( Pouch ), 1015283, "pouch", 10.0, 35.0, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( Bag ), 1015283, "bag", 20.0, 45.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( Backpack ), 1015283, "backpack", 30.0, 55.0, typeof( Leather ), 1044462, 8, 1044463 );
			index = AddCraft( typeof( Quiver ), 1015269,  "quiver", 25.0, 50.0, typeof( Cloth ), 1063525, 8, 1044287 );
			SetUseSubRes2( index, true );
			AddCraft( typeof( PolarBearRugEastDeed ), 1015283, "polar bear rug", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			AddCraft( typeof( BrownBearRugEastDeed ), 1015283, "brown bear rug", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			AddCraft( typeof( AG_SmallBlueCarpetAddonDeed ), 1015283, "small blue carpet", 80.0, 95.0, typeof( Fur ), 1063537, 30, 1063536 );
			AddCraft( typeof( AG_MediumBlueCarpetAddonDeed ), 1015283, "medium blue carpet", 80.0, 95.0, typeof( Fur ), 1063537, 40, 1063536 );
			AddCraft( typeof( AG_LargeBlueCarpetAddonDeed ), 1015283, "large blue carpet", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			
			AddCraft( typeof( AG_SmallBrownCarpetAddonDeed ), 1015283, "small brown carpet", 80.0, 95.0, typeof( Fur ), 1063537, 30, 1063536 );
			AddCraft( typeof( AG_MediumBrownCarpetAddonDeed ), 1015283, "medium brown carpet", 80.0, 95.0, typeof( Fur ), 1063537, 40, 1063536 );
			AddCraft( typeof( AG_LargeBrownCarpetAddonDeed ), 1015283, "large brown carpet", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			
			AddCraft( typeof( AG_SmallGoldenCarpetAddonDeed ), 1015283, "small golden carpet", 80.0, 95.0, typeof( Fur ), 1063537, 30, 1063536 );
			AddCraft( typeof( AG_MediumGoldenCarpetAddonDeed ), 1015283, "medium golden carpet", 80.0, 95.0, typeof( Fur ), 1063537, 40, 1063536 );
			AddCraft( typeof( AG_LargeGoldenCarpetAddonDeed ), 1015283, "large golden carpet", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			
			AddCraft( typeof( AG_SmallRedCarpetAddonDeed ), 1015283, "small red carpet", 80.0, 95.0, typeof( Fur ), 1063537, 30, 1063536 );
			AddCraft( typeof( AG_MediumRedCarpetAddonDeed ), 1015283, "medium red carpet", 80.0, 95.0, typeof( Fur ), 1063537, 40, 1063536 );
			AddCraft( typeof( AG_LargeRedCarpetAddonDeed ), 1015283, "large red carpet", 80.0, 95.0, typeof( Fur ), 1063537, 50, 1063536 );
			
			index = AddCraft( typeof( TentPackEast ), 1015283, "tent pack (east)", 80.0, 95.0, typeof( Cloth ), 1063525, 200, 1044287 );
			AddRes( index, typeof( Log ), 1044041, 50, 1044351 );
			AddSkill( index, SkillName.Tinkering, 20.0, 25.0 );
			index = AddCraft( typeof( TentPackSouth ), 1015283, "tent pack (south)", 80.0, 95.0, typeof( Cloth ), 1063525, 200, 1044287 );
			AddRes( index, typeof( Log ), 1044041, 50, 1044351 );
			AddSkill( index, SkillName.Tinkering, 20.0, 25.0 );
			
			AddCraft( typeof( OilCloth ), 1015283, "oil cloth", 20.0, 60.0, typeof( Cloth ), 1063525, 2, 1044287 );
			
			index = AddCraft( typeof( EmptyBackSheath ), 1015283, "back sheath", 55.0, 80.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			index = AddCraft( typeof( EmptyWaistSheath ), 1015283, "waist sheath", 55.0, 80.0, typeof( Cloth ), 1063525, 4, 1044287 );
			SetUseSubRes2( index, true );
			
			index = AddCraft( typeof( GuildFlag ), 1015283, "guild flag", 20.0, 60.0, typeof( Cloth ), 1063525, 20, 1044287 );
			AddRes( index, typeof( Log ), 1044041, 10, 1044351 );
			
			index = AddCraft( typeof( Pillow1 ), 1015283, "pillow (1)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow2 ), 1015283, "pillow (2)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow3 ), 1015283, "pillow (3)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow4 ), 1015283, "pillow (4)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow5 ), 1015283, "pillow (5)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow6 ), 1015283, "pillow (6)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow7 ), 1015283, "pillow (7)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow8 ), 1015283, "pillow (8)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow9 ), 1015283, "pillow (9)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow10 ), 1015283, "pillow (10)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow11 ), 1015283, "pillow (11)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			
			index = AddCraft( typeof( Pillow12 ), 1015283, "pillow (12)", 20.0, 40.0, typeof( Cloth ), 1063525, 5, 1044287 );
			AddRes( index, typeof( Feather ), 1044562, 20, 1044563 );
			AddCraft( typeof( Hammock ), 1015283, "hammock", 20.0, 40.0, typeof( Cloth ), 1063525, 8, 1044287 );
			AddCraft( typeof( SmallCurtain ), 1015283, "small curtain", 20.0, 40.0, typeof( Cloth ), 1063525, 8, 1044287 );
			AddCraft( typeof( Curtain ), 1015283, "curtain", 20.0, 40.0, typeof( Cloth ), 1063525, 12, 1044287 );
			AddCraft( typeof( CurtainSash ), 1015283, "curtain sash", 20.0, 40.0, typeof( Cloth ), 1063525, 10, 1044287 );
			AddCraft( typeof( Towel ), 1015283, "towel", 20.0, 40.0, typeof( Cloth ), 1063525, 4, 1044287 );
			AddCraft( typeof( Blanket ), 1015283, "blanket", 20.0, 40.0, typeof( Cloth ), 1063525, 8, 1044287 );
			AddCraft( typeof( Sheets ), 1015283, "sheet", 20.0, 40.0, typeof( Cloth ), 1063525, 6, 1044287 );
			AddCraft( typeof( HalfFur ), 1015283, "decorative fur", 20.0, 40.0, typeof( Fur ), 1063537, 1, 1063536 );
			AddCraft( typeof( BrownRug ), 1015283, "brown rug", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( GreenRug ), 1015283, "green rug", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( RedCarpet ), 1015283, "red carpet", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( BlueCarpet ), 1015283, "blue carpet", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( GoldCarpet ), 1015283, "gold carpet", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( GazaMat ), 1015283, "mat", 20.0, 40.0, typeof( Fur ), 1063537, 3, 1063536 );
			AddCraft( typeof( SmallCarpet ), 1015283, "small carpet", 20.0, 40.0, typeof( Fur ), 1063537, 2, 1063536 );
			
			#endregion

			#region Footwear
			index = AddCraft( typeof( FurBoots ), 1015288, "fur boots", 50.0, 75.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddRes( index, typeof( Fur ), 1063537, 4, 1063536 );
			AddCraft( typeof( Sandals ), 1015288, "sandals", 10.0, 35.0, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( Shoes ), 1015288, "shoes", 15.0, 40.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( Boots ), 1015288, "boots", 35.0, 60.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( ThighBoots ), 1015288, "thigh boots", 40.0, 65.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( LeatherBoots ), 1015288, "leather boots", 65.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( ElegantShoes ), 1015288, "elegant shoes", 65.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( BlackLeatherBoots ), 1015288, "black leather boots", 65.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( HighHeels ), 1015288, "high heels", 70.0, 110.0, typeof( Leather ), 1044462, 10, 1044463 );
			
			index = AddCraft( typeof( HardenedFurBoots ), 1015288, "hardened fur boots", 50.0, 75.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddRes( index, typeof( Fur ), 1063537, 4, 1063536 );
			AddCraft( typeof( HardenedSandals ), 1015288, "hardened sandals", 10.0, 35.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( HardenedShoes ), 1015288, "hardened shoes", 15.0, 40.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( HardenedBoots ), 1015288, "hardened boots", 35.0, 60.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( HardenedThighBoots ), 1015288, "hardened thigh boots", 40.0, 65.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( HardenedLeatherBoots ), 1015288, "hardened leather boots", 65.0, 100.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( HardenedElegantShoes ), 1015288, "hardened elegant shoes", 65.0, 100.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( HardenedBlackLeatherBoots ), 1015288, "hardened black leather boots", 65.0, 100.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( HardenedHighHeels ), 1015288, "hardened high heels", 70.0, 110.0, typeof( Leather ), 1044462, 12, 1044463 );
			
			#endregion

			#region Leather Armor
			AddCraft( typeof( LeatherGorget ), 1015293, 1025063, 55.0, 80.0, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( LeatherCap ), 1015293, 1027609, 5.0, 30.0, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( typeof( LeatherGloves ), 1015293, 1025062, 50.0, 75.0, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( typeof( LeatherArms ), 1015293, 1025061, 55.0, 80.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( LeatherLegs ), 1015293, 1025067, 65.0, 90.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( LeatherChest ), 1015293, 1025068, 70.0, 95.0, typeof( Leather ), 1044462, 12, 1044463 );

			AddCraft( typeof( SoftLeatherBoots ), 1015293, "soft leather boots", 80.0, 120.0, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( typeof( SoftLeatherPauldrons ), 1015293, "soft leather pauldrons", 80.0, 120.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( SoftLeatherLegs ), 1015293, "soft leather legs", 80.0, 120.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( SoftLeatherTunic ), 1015293, "soft leather tunic", 80.0, 120.0, typeof( Leather ), 1044462, 12, 1044463 );
            AddCraft( typeof( LeatherCowl ), 1015293, "leather cowl", 5.0, 30.0, typeof( Leather ), 1044462, 4, 1044463 );
            AddCraft( typeof( HardenedLeatherCowl ), 1015293, "hardened leather cowl", 5.0, 30.0, typeof( Leather ), 1044462, 6, 1044463 );

			#endregion

			#region Studded Armor
			AddCraft( typeof( StuddedGorget ), 1015300, 1025078, 80.0, 120.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( StuddedGloves ), 1015300, 1025077, 80.0, 120.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( StuddedArms ), 1015300, 1025076, 80.0, 120.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( StuddedLegs ), 1015300, 1025082, 80.0, 120.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( StuddedChest ), 1015300, 1025083, 80.0, 120.0, typeof( Leather ), 1044462, 14, 1044463 );

			#endregion

			#region Female Armor
			AddCraft( typeof( LeatherShorts ), 1015306, 1027168, 60.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( LeatherSkirt ), 1015306, 1027176, 60.0, 100.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( LeatherBustierArms ), 1015306, 1027178, 60.0, 100.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( StuddedBustierArms ), 1015306, 1027180, 60.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( FemaleLeatherChest ), 1015306, 1027174, 60.0, 100.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( FemaleStuddedChest ), 1015306, 1027170, 60.0, 100.0, typeof( Leather ), 1044462, 10, 1044463 );
			#endregion

			// Set the overridable material
			SetSubRes( typeof( Leather ), 1049150 );

			// Add every material you want the player to be able to choose from
			// This will override the overridable material
			AddSubRes( typeof( Leather ),		1049150, 00.0, 1044462, 1049311 );
			AddSubRes( typeof( ThickLeather ),	1049151, 65.0, 1044462, 1049311 );
			AddSubRes( typeof( BeastLeather ),	1049152, 80.0, 1044462, 1049311 );
			AddSubRes( typeof( ScaledLeather ),	1049153, 99.0, 1044462, 1049311 );
			
			SetSubRes2( typeof( Cloth ), 1063523 );
            AddSubRes2( typeof( Cloth ),            1063523, 0.0, 1063527, 1063522 );
            AddSubRes2( typeof( Wool ),             1063544, 0.0, 1063527, 1063522 );
			AddSubRes2( typeof( Linen ),			1063524, 60.0, 1063527, 1063522 );
            AddSubRes2( typeof( SpidersSilk ),      1063538, 90.0, 1063527, 1063522 );
            AddSubRes2( typeof( Satin ),            1063540, 90.0, 1063527, 1063522 );
            AddSubRes2( typeof( Velvet ),           1063542, 90.0, 1063527, 1063522 );
            
			MarkOption = true;
			Repair = Core.AOS;
			CanEnhance = false;
		}
	}
}
