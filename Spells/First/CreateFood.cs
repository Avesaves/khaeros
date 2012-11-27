// Modified by Alari - includes all foods, and returns 3 food items.

using System;
using Server.Items;

namespace Server.Spells.First
{
	public class CreateFoodSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Create Food", "In Mani Ylem",
			SpellCircle.First,
			224,
			9011,
			Reagent.Garlic,
			Reagent.Ginseng,
			Reagent.MandrakeRoot
		);
		
		public CreateFoodSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}
		
		private static FoodInfo[] m_Food = new FoodInfo[]
		{
			// baked
			new FoodInfo( typeof( BreadLoaf ), "a loaf of bread" ),
			new FoodInfo( typeof( Cake ), "a cake" ),
			new FoodInfo( typeof( Cookies ), "some cookies" ),
			new FoodInfo( typeof( Donuts ), "some donuts" ),
			new FoodInfo( typeof( FrenchBread ), "a loaf of french bread" ),
			new FoodInfo( typeof( Muffins ), "a muffin" ),
			new FoodInfo( typeof( Pizza ), "a pizza" ),
			new FoodInfo( typeof( CheesePizza ), "a cheese pizza" ),
			new FoodInfo( typeof( SausagePizza ), "a sausage pizza" ),
			
			// specialty baked
			new FoodInfo( typeof( ApplePie ), "an apple pie" ),
			new FoodInfo( typeof( BananaCake ), "a banana cake" ),
			new FoodInfo( typeof( CantaloupeCake ), "a cantaloupe cake" ),
			new FoodInfo( typeof( CarrotCake ), "a carrot cake" ),
			new FoodInfo( typeof( CoconutCake ), "a coconut cake" ),
			new FoodInfo( typeof( FruitCake ), "a fruit cake" ),
			new FoodInfo( typeof( FruitPie ), "a fruit pie" ),
			new FoodInfo( typeof( GrapeCake ), "a grape cake" ),
			new FoodInfo( typeof( HoneydewMelonCake ), "a honeydew melon cake" ),
			new FoodInfo( typeof( KeyLimeCake ), "a key lime cake" ),
			new FoodInfo( typeof( KeyLimePie ), "a key lime pie" ),
			new FoodInfo( typeof( LemonCake ), "a lemon cake" ),
			new FoodInfo( typeof( MeatCake ), "a meat cake" ),
			new FoodInfo( typeof( MeatPie ), "a meat pie" ),
			new FoodInfo( typeof( PeachCake ), "a peach cake" ),
			new FoodInfo( typeof( PeachCobbler ), "a peach cobbler" ),
			new FoodInfo( typeof( PumpkinCake ), "a pumpkin cake" ),
			new FoodInfo( typeof( PumpkinPie ), "a pumpkin pie" ),
			new FoodInfo( typeof( Quiche ), "some quiche" ),
			new FoodInfo( typeof( VegePie ), "a vegetable pie" ),
			new FoodInfo( typeof( VegetableCake ), "a vegetable cake" ),
			new FoodInfo( typeof( WatermelonCake ), "a watermelon cake" ),
			
			// fruits
			new FoodInfo( typeof( Apple ), "an apple" ),
			new FoodInfo( typeof( Banana ), "a banana" ),
			new FoodInfo( typeof( Bananas ), "bananas" ),
			new FoodInfo( typeof( Cantaloupe ), "a cantaloupe" ),
			new FoodInfo( typeof( Coconut ), "a coconut" ),
			new FoodInfo( typeof( Dates ), "some dates" ),
			new FoodInfo( typeof( Grapes ), "some grapes" ),
			new FoodInfo( typeof( GreenGourd ), "a gourd" ),
			new FoodInfo( typeof( HoneydewMelon ), "a honeydew melon" ),
			new FoodInfo( typeof( Lemon ), "a lemon" ),
			new FoodInfo( typeof( Lime ), "a lime" ),
			new FoodInfo( typeof( Peach ), "a peach" ),
			new FoodInfo( typeof( Pear ), "a pear" ),
			new FoodInfo( typeof( Pumpkin ), "a pumpkin" ),
			new FoodInfo( typeof( SplitCoconut ), "a coconut" ),
			new FoodInfo( typeof( Squash ), "a squash" ),
			new FoodInfo( typeof( Watermelon ), "a watermelon" ),
			new FoodInfo( typeof( YellowGourd ), "a gourd" ),
			
			// meat
			new FoodInfo( typeof( Bacon ), "a slice of bacon" ),
			new FoodInfo( typeof( BaconSlab ), "a slab of bacon" ),
			new FoodInfo( typeof( ChickenLeg ), "a chicken leg" ),
			new FoodInfo( typeof( CookedBird ), "a cooked bird" ),
			new FoodInfo( typeof( CookedHeadlessFish ), "a cooked fish" ),
			new FoodInfo( typeof( FishSteak ), "a fish steak" ),
			new FoodInfo( typeof( Ham ), "a ham" ),
			new FoodInfo( typeof( HamSlices ), "a slice of ham" ),
			new FoodInfo( typeof( LambLeg ), "a leg of lamb" ),
			new FoodInfo( typeof( Ribs ), "ribs" ),
		//	new FoodInfo( typeof( RoastPig ), "a whole roast pig" ), // a bit much...
			new FoodInfo( typeof( Sausage ), "a sausage" ),
			
			// others
			new FoodInfo( typeof( BowlOfStew ), "a bowl of stew" ),
			new FoodInfo( typeof( CheeseWedgeSmall ), "a small wedge of cheese" ),
			new FoodInfo( typeof( CheeseWedge ), "a wedge of cheese" ),
			new FoodInfo( typeof( CheeseWheel ), "a wheel of cheese" ),
			new FoodInfo( typeof( FriedEggs ), "fried eggs" ),
			new FoodInfo( typeof( Mushrooms ), "mushrooms" ),
			new FoodInfo( typeof( Spam ), "something unholy" ), // "spam" ),
			new FoodInfo( typeof( TomatoSoup ), "tomato soup" ),
			new FoodInfo( typeof( RedRaspberry ), "some raspberries" ),
			new FoodInfo( typeof( Strawberries ), "some strawberries" ),
			new FoodInfo( typeof( BlackRaspberry ), "some raspberries" ),
	
			
			// special
		//	new FoodInfo( typeof( Hay ), "some hay" ), // not people food! ^.^;
			
			// vegetables
			new FoodInfo( typeof( Cabbage ), "a head of cabbage" ),
			new FoodInfo( typeof( Carrot ), "a carrot" ),
			new FoodInfo( typeof( Corn ), "an ear of corn" ),
			new FoodInfo( typeof( Lettuce ), "a head of lettuce" ),
			new FoodInfo( typeof( Onion ), "an onion" ),
			new FoodInfo( typeof( Turnip ), "a turnip" )
			
// containerfood - personally I think these are too 'heavy' for the spell to make
			/*
			new FoodInfo( typeof( BaconAndEgg ), "a plate of bacon and eggs" ),
			new FoodInfo( typeof( PewterBowlCabbage ), "a pewter bowl of cabbage" ),
			new FoodInfo( typeof( PewterBowlCarrot ), "a pewter bowl of carrots" ),
			new FoodInfo( typeof( PewterBowlCorn ), "a pewter bowl of corn" ),
			new FoodInfo( typeof( PewterBowlLettuce ), "a pewter bowl of lettuce" ),
			new FoodInfo( typeof( PewterBowlPea ), "a pewter bowl of peas" ),
			new FoodInfo( typeof( PlateOfCookies ), "a plate of cookies" ),
			new FoodInfo( typeof( WoodenBowlCabbage ), "a wooden bowl of cabbage" ),
			new FoodInfo( typeof( WoodenBowlCarrot ), "a wooden bowl of carrots" ),
			new FoodInfo( typeof( WoodenBowlCorn ), "a wooden bowl of corn" ),
			new FoodInfo( typeof( WoodenBowlLettuce ), "a wooden bowl of lettuce" ),
			new FoodInfo( typeof( WoodenBowlPea ), "a wooden bowl of peas" ),
			*/
		};
		
		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				
				FoodInfo foodInfo1 = m_Food[Utility.Random( m_Food.Length )];
				Item food1 = foodInfo1.Create();
				
				FoodInfo foodInfo2 = m_Food[Utility.Random( m_Food.Length )];
				Item food2 = foodInfo2.Create();
				
				FoodInfo foodInfo3 = m_Food[Utility.Random( m_Food.Length )];
				Item food3 = foodInfo3.Create();
				
				if ( food1 != null && food2 != null && food3 != null )
				{
					Caster.AddToBackpack( food1 );
					Caster.AddToBackpack( food2 );
					Caster.AddToBackpack( food3 );
					
					// You magically create food in your backpack:
						Caster.SendLocalizedMessage( 1042695, true, " " + foodInfo1.Name + ", " + foodInfo2.Name + ", and " + foodInfo3.Name );
					
					Caster.FixedParticles( 0, 10, 5, 2003, EffectLayer.RightHand );
					Caster.PlaySound( 0x1E2 );
				}
			}
			
			
			/*				FoodInfo foodInfo = m_Food[Utility.Random( m_Food.Length )];
			Item food = foodInfo.Create();
			
			if ( food != null )
			{
				Caster.AddToBackpack( food );
				
				// You magically create food in your backpack:
					Caster.SendLocalizedMessage( 1042695, true, " " + foodInfo.Name );
				
				Caster.FixedParticles( 0, 10, 5, 2003, EffectLayer.RightHand );
				Caster.PlaySound( 0x1E2 );
			}
		}
		*/
		
		FinishSequence();
		}
	}
	
	public class FoodInfo
	{
		private Type m_Type;
		private string m_Name;
		
		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }
		
		public FoodInfo( Type type, string name )
		{
			m_Type = type;
			m_Name = name;
		}
		
		public Item Create()
		{
			Item item;
			
			try
			{
				item = (Item)Activator.CreateInstance( m_Type );
			}
			catch
			{
				item = null;
			}
			
			return item;
		}
	}
}
