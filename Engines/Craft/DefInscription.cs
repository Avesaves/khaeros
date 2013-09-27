using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.Craft
{
	public class DefInscription : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Inscribe; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044009; } // <CENTER>INSCRIPTION MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefInscription();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefInscription() : base( 1, 1, 4.0 )// base( 1, 1, 3.0 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type typeItem )
		{
			PlayerMobile m = from as PlayerMobile;
			
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.
			 
			if( from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Magery) < 1 && (typeItem == typeof( CustomSpellBook ) || typeItem == typeof( CustomSpellScroll )) )
				return 1063492; // You lack the required feat.
				
			if( from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Magery) > 1 && m.IsApprentice && (typeItem == typeof( CustomSpellBook ) || typeItem == typeof( CustomSpellScroll )) )
				return 1063492; // You lack the required feat.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x249 );
		}

		private static Type typeofSpellScroll = typeof( SpellScroll );

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
				return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			AddCraft( typeof( BoneCoveredBook ), "Books", "bone covered book", 60.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( ChainCoveredBook ), "Books", "chain covered book", 60.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( GreenBook ), "Books", "green book", 50.0, 70.0, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( PlatedBook ), "Books", "plated book", 60.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( RedLeatherBook ), "Books", "red leather book", 40.0, 65.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( SmallBlackBook ), "Books", "small black book", 35.0, 55.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( SmallBlueBook ), "Books", "small blue book", 35.0, 55.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( SmallBrownBook ), "Books", "small brown book", 35.0, 55.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( SmallLeatherBook ), "Books", "small leather book", 30.0, 50.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( CustomSpellBook ), "Books", "spell book", 0.0, 0.0, typeof( Leather ), 1044462, 12, 1044463 );
			
			AddCraft( typeof( OldPieceOfPaper ), "Scrolls", "old piece of paper", 5.0, 25.0, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( typeof( OrnateScroll ), "Scrolls", "ornate scroll", 15.0, 40.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( PieceOfPaper ), "Scrolls", "piece of paper", 5.0, 30.0, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( typeof( RoughScroll ), "Scrolls", "rough scroll", 10.0, 30.0, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( AlchemicalFormula ), "Scrolls", "alchemical formula", 20.0, 40.0, typeof( Leather ), 1044462, 4, 1044463 );
			//AddCraft( typeof( CustomSpellScroll ), "Scrolls", "spell scroll", 0.0, 0.0, typeof( Leather ), 1044462, 4, 1044463 );
			
			AddCraft( typeof( StoneTablet ), "Tablets", "stone tablet", 95.0, 102.5, typeof( Granite ), 1044514, 2, 1044513 );
			AddCraft( typeof( RoughStoneTablet ), "Tablets", "rough stone tablet", 95.0, 102.5, typeof( Granite ), 1044514, 2, 1044513 );
			AddCraft( typeof( OrnateStoneTablet ), "Tablets", "ornate stone tablet", 95.0, 102.5, typeof( Granite ), 1044514, 2, 1044513 );

			MarkOption = true;
		}
	}
}
