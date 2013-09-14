using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;

namespace Server.Misc.BreedingSystem
{
	public class Utilities
	{		
		public static void TryToMate( PlayerMobile owner, BaseBreedableCreature first, BaseBreedableCreature second )
		{
			if( owner == null || owner.Deleted || first == null || first.Deleted || second == null || second.Deleted )
				return;
			
			int featlevel = HasFeat( owner, first );
				
			if( !first.CanBeHandledBy(owner) || !second.CanBeHandledBy(owner) )
				owner.SendMessage( "You must either control or be a friend of the creatures." );
			
			else if( first == second )
				owner.SendMessage( "You need to target two different creatures of the same species and breed." );
			
			else if( first.Breed != second.Breed )
				owner.SendMessage( "Both creatures must be of the same breed." );
			
			else if( (first.Female && second.Female) || (!first.Female && !second.Female) )
				owner.SendMessage( "The creatures must be of different genders." );
			
			else if( !ValidPosition( owner, first ) || !ValidPosition( owner, second ) || !ValidPosition( first, second ) )
				owner.SendMessage( "You and the creatures must be close to each other." );
			
			else if( first.Pregnant || second.Pregnant )
				owner.SendMessage( "The female is already pregnant." );
			
			else if( (!first.Female && first.Gelt) || (!second.Female && second.Gelt) )
				owner.SendMessage( "The male animal cannot be gelt." );
			
			else if( featlevel > 0 )
			{
				if( RaciallyCompatible( owner, first ) )
				{
					owner.SendMessage( "The creatures have mated sucessfully." );
					LevelSystem.AwardCraftXP( owner, true );
					
					if( first.Female )
						first.Impregnate( second, owner, featlevel );
					
					else
						second.Impregnate( first, owner, featlevel );
				}
			}
			
			else
				owner.SendMessage( "You lack the appropriate feat." );
		}
		
		public static bool RaciallyCompatible( PlayerMobile owner, BaseCreature creat )
		{
			bool valid = true;
			
			//if( (creat is RedWolf || creat is GallowayHorse || creat is AlyrianShepherd || creat is ForestStrider) && owner.Nation != Nation.Southern )
			//	valid = false;
			
		//else if( (creat is ManedWolf || creat is KudaHorse || creat is AzhuranRetriever || creat is Ridgeraptor) && owner.Nation != Nation.Western )
				//valid = false;
			
			//else if( (creat is Jackal || creat is BarbHorse || creat is Saluki || creat is GiantScarab) && owner.Nation != Nation.Haluaroc )
				//valid = false;
			
			//else if( (creat is Timberwolf || creat is SteppeHorse || creat is MhordulWolfdog || creat is DireWolf || creat is SkullcrusherOgre) && owner.Nation != Nation.Mhordul )
				//valid = false;
			
			//else if( (creat is SnowWolf || creat is RuganHorse || creat is Husky || creat is DireBear) && owner.Nation != Nation.Tirebladd )
				//valid = false;
			
			//else if( (creat is GrayWolf || creat is RoseanHorse || creat is BloodHound || creat is WarHorse) && owner.Nation != Nation.Northern )
				//valid = false;

           //if (creat is Serpent && owner.Nation != Nation.Haluaroc)
              //valid = false;

          // if (creat is Bear && owner.Nation != Nation.Tirebladd)
                //valid = false;

           //if (creat is BirdOfPrey)
               //valid = BirdOfPrey.CanTameBird(owner, creat as BirdOfPrey);
				
			//if( !valid )
			//{
			//	owner.SendMessage( "Your race does not know how to handle that kind of animal." );
			//	return false;
			//}
			
			return true;
		}
		
		public static int HasFeat( PlayerMobile owner, BaseBreedableCreature creat )
		{
			if( creat is Dog )
				return owner.Feats.GetFeatLevel(FeatList.DogBreeding);
			
			if( creat is Wolf )
				return owner.Feats.GetFeatLevel(FeatList.WolfBreeding);
			
			if( creat is Horse || creat is WorkHorse )
				return owner.Feats.GetFeatLevel(FeatList.HorseBreeding);
			
			if( creat is IRacialMount && owner.Feats.GetFeatLevel(FeatList.RacialMounts) > 0 )
				return owner.Feats.GetFeatLevel(FeatList.RacialMounts);

            if( creat is Serpent )
                return owner.Feats.GetFeatLevel(FeatList.Snakecharmer);

            if (creat is Bear)
                return owner.Feats.GetFeatLevel(FeatList.Beartalker);

            if (creat is BirdOfPrey)
                return owner.Feats.GetFeatLevel(FeatList.AvianBreeding);
			
			return 0;
		}
		
		public static bool ValidPosition( Mobile one, Mobile two )
		{
			if( one.CanSee(two) && two.CanSee(one) && one.InLOS(two) && one.InRange(two, 2) && one.Alive && two.Alive )
				return true;
			
			return false;
		}
		
		public static int NewScale( int scale, int level )
		{
			if( level >= Utility.RandomMinMax(1, 50) )
				return (scale + 1);
			
			else if( level >= Utility.RandomMinMax(1, 25) )
				return scale;
			
			return (scale - 1);
		}
		
		public static string GetBBCFeatName( int index )
		{
			switch( index )
			{
				case 1: return "Brute Strength";
				case 2: return "Quick Reflexes";
				case 3: return "Cleave";
				case 4: return "Evade";
				case 5: return "Damage Ignore";
				case 6: return "Fast Healing";
				case 7: return "Critical Strike";
				case 8: return "Savage Strike";
				case 9: return "Crippling Blow";
				case 10: return "Enhanced Dodge";
				case 11: return "Buildup";
				case 12: return "Flurry of Blows";
				case 13: return "Focused Attack";
				case 14: return "Defensive Stance";
				case 15: return "Rage";
			}
			
			return "Invalid";
		}
	}
}
