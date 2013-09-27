using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;

namespace Server.Misc
{
	public class LevelSystem
	{
		public static void PlayerAwards( PlayerMobile player, BaseCreature killed )
		{
			int xpcap = player.Int * 100;
			int xpmax = (int)( killed.Fame * 0.1 + ((killed.Fame * 0.005) * killed.Level) );
			int xp = (int)( xpmax > xpcap == true ? (xpcap + ((xpmax - xpcap) * 0.25)) : xpmax );
			
			AwardExp( player, xp );
			AwardCP( player, (int)( player.LastXP * 0.12 ) );
		}
		
		public static void PetAwards( BaseCreature player, BaseCreature killed )
		{
			AwardExp( player, Math.Min( player.Int * 100, (int)( killed.Fame * 0.1 + ((killed.Fame * 0.005) * killed.Level) ) ) );
		}
		
        public static void AwardExp( PlayerMobile player, int a )
        {
        	if( !player.CanBeAwarded && !player.XPFromLearning )
        		return;
        	
        	if( player.XPFromKilling && !player.XPFromLearning && !player.Crafting )
        	{
        		if( player.Level < 10 )
        			a = (int)( a * ( 3.2 - ( player.Level * 0.2 ) ) );
        		
        		else if( player.Level > 20 && player.Level < 39 )
        			a = (int)( a * ( 1 - ( ( player.Level - 19 ) * 0.04 ) ) );
        		
        		else if( player.Level > 38 )
        			a = (int)( a * 0.2 );
        			
        		else if( player.Level > 49 )
        			a = (int)( a * 0.01);
        		
        		if( a < ( player.Level * 5 ) )
        			a = (int)( a * 0.5 );
        		
        		if( a > player.Level * 15 )
        			a -= (int)(( a - (player.Level * 15) ) * 0.5 );
        	}

        	if( a > 0 && player.Nation == Nation.Northern )
				a++;

            if (player.IsHardcore)
                a = (a + a);

        	if( player.Level < 60 )
            	player.XP += a;
        	
            player.LastXP = a;
            player.SendMessage( 0x35, "You have gained " + a + " experience point{0}!", a == 1 ? "" : "s" );
            CheckLevel( player );
        }

		public static void AwardExp( BaseCreature player, int a )
		{
			double expbonus = 1.0;
			PlayerMobile bcowner = null;
			
			if( player.ControlMaster != null )
			{
				Mobile mob = player.ControlMaster;
				
				if( mob is PlayerMobile )		
					bcowner = mob as PlayerMobile;
			}
			
			if ( player.Level < 60 )
			{
                /*if( player is BaseMount && bcowner != null )
                {
                	if( bcowner.InRange( player, 5 ) || ((BaseMount)player).Rider == bcowner )
	            		expbonus += 0.5 * bcowner.Feats.MountTraining;
                }*/
                
                if( player is BaseBreedableCreature && bcowner != null )
                {
                	if( bcowner.InRange( player, 5 ) || (player is BaseMount && ((BaseMount)player).Rider == bcowner) )
	            		expbonus += 0.5 * bcowner.Feats.GetFeatLevel(FeatList.AnimalTraining);
                }

                if (player is Soldier && player.Government != null && !player.Government.Deleted)
                {
                    double offset = player.Government.Members.Count * 0.01;
                    expbonus += offset;
                }
                
                if( bcowner != null )
                {
                	double offset = bcowner.Skills[SkillName.AnimalLore].Base * 0.01;
                            
                    if( player is Mercenary )
                    	offset = bcowner.Skills[SkillName.Leadership].Base * 0.01;
                    
                    expbonus += offset;
                }
                
                a = Convert.ToInt32( a * expbonus );
                
                player.XP += a;
				CheckLevel( player );
			}
		}

		public static void AwardCP( PlayerMobile player, int a )
		{
			if( player.LastXP < 1 )
        		a = 0;
			
			if( !player.CanBeAwarded && !player.XPFromLearning )
            	return;

        	if( a > 0 )
        	{
        		if( player.XPFromKilling && !player.XPFromLearning && !player.Crafting )
        		{
        			if( player.Level < 10 )
        				a *= 6;
        			
        			else if( player.Level < 15 )
        				a *= 3;
        			
        			else if( player.Level < 20 )
        				a = (int)(a * 2.5);
        			
        			else if( player.Level < 25 )
        				a = (int)(a * 2.0);
        			
        			else if( player.Level < 30 )
        				a = (int)(a * 1.75);

        			else if( player.Level > 49 )
        				a = (int)(a * 0.5);
        				
        			else
        				a = (int)(a * 1.5);
        		}
        		if (player.XPFromLearning && player.Level > 49 )
        			a = (int)(a * 0.2);

                if ((a > player.Level * 40) && (player.IsHardcore))
                    a = player.Level * 40;
                else if ((a > player.Level * 20) && (player.IsHardcore == false))
                    a = player.Level * 20;
        		
	        	if( player.Nation == Nation.Northern )
					a++;

	        	int cpcap = 175000 + player.ExtraCPRewards;
	        	
	        	//if( player.Advanced != Advanced.None )
	        		//cpcap += 75000;

	        	if( ( player.CP + player.CPSpent + a ) > ( player.CPCapOffset + cpcap ) )
	        	{
	        		player.CP = ( player.CPCapOffset + cpcap ) - player.CPSpent;
	        		a = ( player.CPCapOffset + cpcap ) - ( player.CPSpent + player.CP );
	        	}
        	}


			player.CP += a;
			player.SendMessage( 0x35, "You have gained " + a + " character point{0}!", a == 1 ? "" : "s" );
		}
		
		public static void CheckLevel( PlayerMobile player )
		{
			if ( player.Level <= 0 )
			{
			    player.Level = 1;
			    player.NextLevel = 1000;
			}
			
			if ( player.XP >= player.NextLevel && player.Level < 60 )
			    LevelUp( player );
		}

        public static void CheckLevel( BaseCreature player )
		{
			if ( player.Level <= 0 )
			{
				player.NextLevel = (5000 / player.XPScale);
			    player.Level = 1;
			}
			
			if ( player.XP >= player.NextLevel && player.Level < 60 )
			    LevelUp( player );
		}

		public static void LevelUp( PlayerMobile player )
		{
			player.NextLevel += ( player.Level + 1 ) * 1000;
            player.Level++;
            
            if( player.Age < player.MaxAge )
	            player.Lives = Math.Min( player.Lives + 1, 50 );
            
			player.SendMessage( 13, "You have reached level " + player.Level + "! Use .statpoints in order to spend your newly acquired bonus points." );
			player.StatPoints += 10;
				
			if( player.Level < 60 )
				CheckLevel( player );
		}

        public static void LevelUp( BaseCreature player )
		{
            player.NextLevel += ( player.Level + 1 ) * (5000 / player.XPScale);
            player.Level++;
            
            if( player.Level % 2 != 0 )
            	player.DamageMin++;
            
            else
            	player.DamageMax++;
            
            double i = 0.5 * player.SkillScale;

            if (player is Serpent)
            {
                player.RawStr += player.StatScale;
                player.RawDex += player.StatScale;
                player.RawStam += player.StatScale;
                player.RawMana += player.StatScale;

                if (player.XPScale == 1)
                {
                    player.RawInt += 1;
                    player.RawHits += 1;
                }
                else
                {
                    player.RawInt += (player.StatScale / 2);
                    player.RawHits += (player.StatScale / 2);
                }
            }
            else if (player is BirdOfPrey)
            {
                if (player.StatScale == 1)
                {
                    player.RawStr += 1;
                    player.RawHits += 1;
                }
                else
                {
                    player.RawStr += (player.StatScale / 2);
                    player.RawHits += (player.StatScale / 2);
                }

                player.RawDex += (int)(player.StatScale * Utility.RandomMinMax(1,2));
                player.RawInt += player.StatScale;
                player.RawStam += player.StatScale;
                player.RawMana += player.StatScale;
            }
            else if (player is Bear)
            {
                int statBonus = Utility.Random(player.StatScale);
                int armorBonus = Utility.Random(5);

                if (player.XPScale == 1)
                {
                    player.RawStr += 1;
                    player.RawDex += Utility.Random(2);
                }
                else
                {
                    player.RawStr += (player.StatScale / 2);
                    player.RawDex += ((player.StatScale - (statBonus + 1)) / 2);
                }

                player.RawInt += (player.StatScale - statBonus);
                player.RawHits += (player.StatScale + (statBonus + 1));
                player.RawStam += (player.StatScale + statBonus);
                player.RawMana += (player.StatScale / 2);

                if (armorBonus == 0)
                    player.BluntResistSeed += (player.StatScale - 1);
                else if (armorBonus == 1)
                    player.SlashingResistSeed += (player.StatScale - 1);
                else if (armorBonus == 2)
                    player.PiercingResistSeed += (player.StatScale - 1);
                else if (armorBonus == 3)
                {
                    player.PoisonResistSeed += (player.StatScale - 1);
                    player.EnergyResistSeed += (player.StatScale - 1);
                }
                else
                {
                    if (player.Level % 2 != 0)
                        player.DamageMin--;
                    else
                        player.DamageMax--;
                }
            }
            else if (player is Mercenary)
            {
                int statUpgrade = Utility.RandomMinMax(1, 4);

                switch (statUpgrade)
                {
                    case 1:
                        {
                            player.RawStr += player.StatScale;
                            player.RawDex += Utility.RandomMinMax(1, player.StatScale);
                            player.RawInt += 1;
                            player.RawHits += Utility.RandomMinMax(1, player.StatScale);
                            player.RawStam += Utility.RandomMinMax(1, player.StatScale);
                            break;
                        }
                    case 2:
                        {
                            player.RawStr += Utility.RandomMinMax(1, player.StatScale);
                            player.RawDex += player.StatScale;
                            player.RawInt += 1;
                            player.RawHits += Utility.RandomMinMax(1, player.StatScale);
                            player.RawStam += Utility.RandomMinMax(1, player.StatScale);
                            break;
                        }
                    case 3:
                        {
                            player.RawStr += Utility.RandomMinMax(1, player.StatScale);
                            player.RawDex += Utility.RandomMinMax(1, player.StatScale);
                            player.RawInt += 1;
                            player.RawHits += player.StatScale;
                            player.RawStam += Utility.RandomMinMax(1, player.StatScale);
                            break;
                        }
                    case 4:
                        {
                            player.RawStr += Utility.RandomMinMax(1, player.StatScale);
                            player.RawDex += Utility.RandomMinMax(1, player.StatScale);
                            player.RawInt += 1;
                            player.RawHits += Utility.RandomMinMax(1, player.StatScale);
                            player.RawStam += player.StatScale;
                            break;
                        }
                }

                if (player.ControlMaster != null)
                {
                    switch (((PlayerMobile)player.ControlMaster).Nation)
                    {
                        case Nation.Southern: { if (Utility.RandomBool()) player.RawDex += Utility.Random(player.StatScale); else player.RawInt += Utility.Random(player.StatScale); break; }
                        case Nation.Western: { player.RawDex += Utility.Random(player.StatScale); break; }
                        case Nation.Haluaroc: { player.RawInt += Utility.Random(player.StatScale); break; }
                        case Nation.Mhordul: { player.RawStr += Utility.Random(player.StatScale); break; }
                        case Nation.Tirebladd: { if (Utility.RandomBool()) player.RawStr += Utility.Random(player.StatScale); else player.RawStam += Utility.Random(player.StatScale); break; }
                        case Nation.Northern: { player.RawHits += Utility.Random(player.StatScale); break; }
                    }
                }
            }
            else
            {
                player.RawStr += player.StatScale;
                player.RawDex += player.StatScale;
                player.RawInt += player.StatScale;
                player.RawHits += player.StatScale;
                player.RawStam += player.StatScale;
                player.RawMana += player.StatScale;
            }

            if( player is Dragon )
            {
            	if( player.Level > 19 && player.BodyValue == 61 )
            	{
            		Gold newloot = new Gold( Utility.RandomMinMax( 1, 3 ) );
            		Container pack = player.Backpack;
            		
            		if( pack != null )
            			pack.DropItem( newloot );
            		
            		player.BodyValue = 59;
            	}
            	
            	if( player.Level > 38 && player.BodyValue == 59 )
            	{
            		Gold newloot = new Gold( Utility.RandomMinMax( 3, 6 ) );
            		Container pack = player.Backpack;
            		
            		if( pack != null )
            			pack.DropItem( newloot );
            		
            		player.BodyValue = 46;
            	}
            }

            if( player is Mercenary && ((Mercenary)player).Lives < 8 )
            	((Mercenary)player).Lives++;
            
            else if( player is BaseBreedableCreature )
            {
            	BaseBreedableCreature bbc = player as BaseBreedableCreature;
            		
            	if( player.Level % 5 == 0 && bbc.PetEvolution > 0 )
            	{
            		bbc.RaiseRandomFeat();
            		bbc.PetEvolution--;
            	}
            	
                bbc.UpdateSpeeds();

            	if( ((BaseBreedableCreature)player).Lives < (8 + ((BaseBreedableCreature)player).ExtraLives) )
            		((BaseBreedableCreature)player).Lives++;

                if (player is Serpent)
                {
                    if (player.Level == 40)
                    {
                        if (player is GrassSnake)
                        {
                            ((GrassSnake)player).SetConstrict(true);
                            player.SetDex(player.RawDex / 3);
                            player.BodyValue = 89;
                            player.Emote("*" + player.Name + " has grown more langurous but also more powerful!*");
                            player.PlaySound(0x05E);
                        }
                        else
                        {
                            player.BodyValue = 89;
                            player.Emote("*" + player.Name + " is growing!*");
                            player.PlaySound(0x05E);
                        }
                    }

                    if(player is Viper)
                        ((Viper)player).incSerpentPoison();
                    else if(player is Copperhead)
                        ((Copperhead)player).incSerpentPoison();
                    else if(player is Cobra)
                        ((Cobra)player).incSerpentPoison();
                    else if(player is BlackMamba)
                        ((BlackMamba)player).incSerpentPoison();
                }

                    if (player is WorkHorse)
                    {
                        List<Item> packItems = new List<Item>();
                        foreach (Item item in (player as WorkHorse).Backpack.Items)
                            packItems.Add(item);

                        StrongBackpack newPack = new StrongBackpack((player as WorkHorse).Backpack.MaxWeight + player.StatScale);
                        newPack.MaxItems += Utility.Random(player.StatScale);
                        foreach (Item item in packItems)
                            newPack.AddItem(item);

                        (player as WorkHorse).Backpack.Delete();
                        (player as WorkHorse).AddItem(newPack);
                    }
            }
            
            else if( player.Controlled && player.Lives < 8 )
            	player.Lives++;
            
            AwardSkill( player, i );
            
            if( player.Level < 50 )
				CheckLevel( player );
		}
        
        public static void AwardMinimumXP( PlayerMobile m, int multiplier )
        {
        	LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), (multiplier * 5) ) );
			LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), (multiplier) ) );
        }
        
        public static void AwardCraftXP( PlayerMobile m )
        {
        	AwardCraftXP( m, false );
        }

        public static void AwardCookingXP(PlayerMobile m)
        {            
            m.Crafting = true;

            int firstTier;
            int secondTier;
            int thirdTier;
            int fourthTier;

            if (m.CookingXpLastAwardedOn.AddMinutes(10) < DateTime.Now)
            {
                firstTier = 100;
                secondTier = 90;
                thirdTier = 80;
                fourthTier = 10;
                m.NumberOfItemsCookedRecently = 0;
            }
            else
            {
                firstTier = Math.Max(0, (100 - (m.NumberOfItemsCookedRecently * 5)));
                secondTier = Math.Max(0, (90 - (m.NumberOfItemsCookedRecently * 5)));
                thirdTier = Math.Max(0, (80 - (m.NumberOfItemsCookedRecently * 5)));
                fourthTier = Math.Max(0, (10 - (m.NumberOfItemsCookedRecently * 5)));
            }

            m.CookingXpLastAwardedOn = DateTime.Now;
            m.NumberOfItemsCookedRecently++;

            if (m.Level < 20)
            {
                LevelSystem.AwardExp(m, Math.Min((m.Int * 100), firstTier));
                LevelSystem.AwardCP(m, Math.Min((m.Int * 20), (firstTier / 5)));

            }

            else if (m.Level < 30)
            {
                LevelSystem.AwardExp(m, Math.Min((m.Int * 100), secondTier));
                LevelSystem.AwardCP(m, Math.Min((m.Int * 20), (secondTier / 5)));
            }
                        else if (m.Level > 49)
            {
                LevelSystem.AwardExp(m, Math.Min((m.Int * 100), fourthTier));
                LevelSystem.AwardCP(m, Math.Min((m.Int * 20), (fourthTier / 5)));
            }

            else
            {
                LevelSystem.AwardExp(m, Math.Min((m.Int * 100), thirdTier));
                LevelSystem.AwardCP(m, Math.Min((m.Int * 20), (thirdTier / 5)));
            }

            m.Crafting = false;
        }

        public static void AwardCraftXP( PlayerMobile m, bool taming )
        {
        	m.Crafting = true;
			
        	int firstTier = 100;
        	int secondTier = 90;
        	int thirdTier = 80;
        	int fourthTier = 10;
        	
        	if( taming )
        	{
        		firstTier *= 2;
        		secondTier *= 2;
        		thirdTier *= 2;
        		fourthTier *= 2;
        	}
        	
			if( m.Level < 20 )
			{
				LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), firstTier ) );
				LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), (firstTier / 5) ) );
				
			}
			
			else if( m.Level < 30 )
			{
				LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), secondTier ) );
				LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), (secondTier / 5) ) );
			}
			
			else if( m.Level > 49 )
			{
				LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), fourthTier ) );
				LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), (fourthTier / 5) ) );
			}
			
			else
			{
				LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), thirdTier ) );
				LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), (thirdTier / 5) ) );
			}
			
			m.Crafting = false;
        }
        
        public static void IncreaseSkill( BaseCreature player, SkillName skill, double i, double cap )
        {
        	player.Skills[skill].Base = SkillValue( player.Skills[skill].Base, i, cap );
        	player.Skills[skill].Cap = player.Skills[skill].Base;
        }
        
        public static double SkillValue( double skill, double i, double cap )
        {
        	skill += i;
        	
        	if( skill > cap )
        		skill = cap;
        	
        	return skill;
        }

        public static void AwardSkill( BaseCreature player, double i )
        {
        	double cap = 100 + (10 * i);
        	
        	IncreaseSkill( player, SkillName.Tactics, i, cap );
			IncreaseSkill( player, SkillName.Parry, i, cap );
            IncreaseSkill( player, SkillName.UnarmedFighting, i, cap );
            IncreaseSkill( player, SkillName.Axemanship, i, cap );
            IncreaseSkill( player, SkillName.Polearms, i, cap );
            IncreaseSkill( player, SkillName.ExoticWeaponry, i, cap );
            IncreaseSkill( player, SkillName.Fencing, i, cap );
            IncreaseSkill( player, SkillName.Macing, i, cap );
            IncreaseSkill( player, SkillName.Swords, i, cap );
            IncreaseSkill( player, SkillName.Archery, i, cap );

            if (player is Serpent)
                IncreaseSkill(player, SkillName.Poisoning, i, cap);
            
            FixStatsAndSkills( player );
        }
        
        public static void FixStatsAndSkills( BaseCreature player )
        {
        	if( player is Mercenary )
        	{
        		int penalty = GetMercArmourPenalties( player );
        		
	        	if( player.RawStr > 150 )
	        		player.RawStr = 150;
	        	
	        	if( player.RawDex > (150 - penalty) )
	        		player.RawDex = 150 - penalty;
	        	
	        	if( player.RawInt > 150 )
	        		player.RawInt = 150;
	        	
	        	if( player.ManaMax > 150 )
	        		player.ManaMax = 150;
	        	
	        	if( player.StamMax > (150 - penalty) )
	        		player.StamMax = 150 - penalty;
	        	
	        	if( player.HitsMax > 150 )
	        		player.HitsMax = 150;
        	}
        	
        	if( player is Mercenary || player is BaseBreedableCreature )
        	{
	        	for( int i = 0; i < 55; ++i )
	        	{
	        		if( player.Skills[i].Base > 100 )
		        	{
		        		player.Skills[i].Base = 100;
		        		player.Skills[i].Cap = 100;
		        	}
	        	}
	        	
	        	if( player.RawDex > 150 )
	        		player.RawDex = 150;
	        }       	
        }
        
        public static int GetMercArmourPenalties( Mobile m )
        {
        	int penalty = GetPenaltyFromLayer( m, Layer.Arms );
			penalty += GetPenaltyFromLayer( m, Layer.Gloves );
			penalty += GetPenaltyFromLayer( m, Layer.Helm );
			penalty += GetPenaltyFromLayer( m, Layer.InnerLegs );
			penalty += GetPenaltyFromLayer( m, Layer.InnerTorso );
			penalty += GetPenaltyFromLayer( m, Layer.MiddleTorso );
			penalty += GetPenaltyFromLayer( m, Layer.Neck );
			penalty += GetPenaltyFromLayer( m, Layer.OuterLegs );
			penalty += GetPenaltyFromLayer( m, Layer.OuterTorso );
			penalty += GetPenaltyFromLayer( m, Layer.Pants );
			penalty += GetPenaltyFromLayer( m, Layer.Shirt );
			penalty += GetPenaltyFromLayer( m, Layer.Shoes );
			penalty += GetPenaltyFromLayer( m, Layer.TwoHanded );
			
			return penalty;
        }
        
        public static int GetPenaltyFromLayer( Mobile m, Layer layer )
		{
			BaseArmor armor = m.FindItemOnLayer( layer ) as BaseArmor;
			
			if( armor != null && armor is BaseArmor )
			{
				if( armor.ArmourType == ArmourWeight.Light )
					return 1;
				
				else if( armor.ArmourType == ArmourWeight.Medium )
					return 2;
				
				else
					return 3;
			}
			
			return 0;
		}
        
        public static void InitialStats( PlayerMobile m )
        {
        	InitialStats( m, true );
        }
		
		public static void InitialStats( PlayerMobile m, bool sendGump )
		{
			m.RawStr = 10;
			m.RawDex = 10;
			m.RawInt = 10;
			m.RawHits = 10;
			m.RawStam = 10;
			m.RawMana = 10;
			m.Hits = 10;
			m.Stam = 10;
			m.Mana = 10;
			m.StatPoints = 100;
			
			if( sendGump )
				m.SendGump( new InitialStatsGump( m ) );
		}
		
		public static void NullifySkills( PlayerMobile m )
		{
			int i = 0;
			
			while ( i < 55 )
			{
				m.Skills[i].Base = 0;
				m.Skills[i].Cap = 100;
				i++;
			}
		}
		
		public static bool CanSpendCP( PlayerMobile m, int amount )
		{
			int limit = 175000 + m.ExtraCPRewards;
			
			if( m.FeatSlots >= limit )
			{
				m.SendMessage( "You have already reached the limit of CPs you were allowed to spend." );
				return false;
			}
			
			if( ( m.FeatSlots + amount ) > limit )
			{
				m.SendMessage( "That will exceed the amount of CP you are allowed to spend." );
				return false;
			}
			
			return true;
		}
		
		public static string SubclassName( PlayerMobile from )
		{
			if( from.Subclass == Subclass.Archer )
				return "Archer";
			else if( from.Subclass == Subclass.Assassin )
				return "Assassin";
			else if( from.Subclass == Subclass.Bard )
				return "Bard";
			else if( from.Subclass == Subclass.Berserker )
				return "Berserker";
			else if( from.Subclass == Subclass.BountyHunter )
				return "Bounty Hunter";
			else if( from.Subclass == Subclass.Dragoon )
				return "Dragoon";
			else if( from.Subclass == Subclass.Fighter )
				return "Fighter";
			else if( from.Subclass == Subclass.MartialArtist )
				return "Martial Artist";
			else if( from.Subclass == Subclass.Metalworker )
				return "Metalworker";
			else if( from.Subclass == Subclass.Scholar )
				return "Scholar";
			else if( from.Subclass == Subclass.Stableworker )
				return "Stableworker";
			else if( from.Subclass == Subclass.Tailor )
				return "Tailor";
			else if( from.Subclass == Subclass.Tavernworker )
				return "Tavernworker";
			else if( from.Subclass == Subclass.Thief )
				return "Thief";
			else if( from.Subclass == Subclass.WeaponSpecialist )
				return "Weapon Specialist";
			else if( from.Subclass == Subclass.Woodworker )
				return "Woodworker";
			else if( from.Class == Class.Mage )
			{
				switch( from.Nation )
				{
					case Nation.Southern: return from.Female == true ? "Sorceress" : "Sorcerer";
					case Nation.Western: return "Diviner";
					case Nation.Haluaroc: return from.Female == true ? "Enchantress" : "Enchanter";
					case Nation.Mhordul: return from.Female == true ? "Warlock" : "Witch";
					case Nation.Tirebladd: return "Wizard";
					case Nation.Northern: return "Mage";
				}
			}
			else if( from.Class == Class.Cleric )
			{
				switch( from.Nation )
				{
					case Nation.Southern: return from.Female == true ? "Druidess" : "Druid";
					case Nation.Western: return "Shaman";
					case Nation.Haluaroc: return from.Female == true ? "Priestess" : "Priest";
					case Nation.Mhordul: return from.Female == true ? "Medicine Woman" : "Medicine Man";
					case Nation.Tirebladd: return "Prophet";
					case Nation.Northern: return "Cleric";
				}
			}
			
			return "None";
		}
		
		public static string AdvancedName( PlayerMobile from )
		{
			if( from.Advanced == Advanced.Archer )
				return "Archer";
			else if( from.Advanced == Advanced.Assassin )
				return "Assassin";
			else if( from.Advanced == Advanced.Bard )
				return "Bard";
			else if( from.Advanced == Advanced.Berserker )
				return "Berserker";
			else if( from.Advanced == Advanced.BountyHunter )
				return "Bounty Hunter";
			else if( from.Advanced == Advanced.Dragoon )
				return "Dragoon";
			else if( from.Advanced == Advanced.Fighter )
				return "Fighter";
			else if( from.Advanced == Advanced.MartialArtist )
				return "Martial Artist";
			else if( from.Advanced == Advanced.Metalworker )
				return "Metalworker";
			else if( from.Advanced == Advanced.Scholar )
				return "Scholar";
			else if( from.Advanced == Advanced.Stableworker )
				return "Stableworker";
			else if( from.Advanced == Advanced.Tailor )
				return "Tailor";
			else if( from.Advanced == Advanced.Tavernworker )
				return "Tavernworker";
			else if( from.Advanced == Advanced.Thief )
				return "Thief";
			else if( from.Advanced == Advanced.WeaponSpecialist )
				return "Weapon Specialist";
			else if( from.Advanced == Advanced.Woodworker )
				return "Woodworker";
			else if( from.Advanced == Advanced.Cleric )
			{
				switch( from.Nation )
				{
					case Nation.Southern: return from.Female == true ? "Druidess" : "Druid";
					case Nation.Western: return "Shaman";
					case Nation.Haluaroc: return from.Female == true ? "Priestess" : "Priest";
					case Nation.Mhordul: return from.Female == true ? "Medicine Woman" : "Medicine Man";
					case Nation.Tirebladd: return "Prophet";
					case Nation.Northern: return "Cleric";
				}
			}
			
			return "None";
		}
		
		public static void WipeAllTraits( PlayerMobile from )
		{
			if( !from.Reforging )
				from.Forging = true;
			
			from.XPFromKilling = true;
			from.XPFromCrafting = true;
			from.Blessed = true;
			
			InitialStats( from, false );
			
			if( !from.Reforging || from.OldMapChar )
			{
				from.Hue = 0;
				from.HairItemID = 0;
				from.FacialHairItemID = 0;
			}
			
			if( !from.Reforging )
			{
				from.Description = null;
            	from.Description2 = null;
            	from.Description3 = null;
            	from.Height = 100;
				from.Weight = 100;
			}
			
            from.Class = Class.None;
            from.Advanced = Advanced.None;
            from.Subclass = Subclass.None;
            
            from.RecreateCP = Math.Max( 0, (from.CPSpent + from.CP - 20000) );
			from.RecreateXP = from.XP;
            from.XP = 0;
            from.CP = 20000;
            from.NextLevel = 1000;
            from.Level = 1;
            from.SkillPoints = 0;
            from.FeatSlots = 0;
            from.CPCapOffset = 0;
            from.CPSpent = 0;
            from.FollowersMax = 5;
            from.XP += from.RecreateXP;
            from.CP += from.RecreateCP;
            
            if( !from.Reforging || from.OldMapChar )
            {
	           	from.Age = 18;
	            from.MaxAge = 0;
	            from.Nation = Nation.None;
            }
            
            if( from.Nation != Nation.None )
            	from.SendGump( new InitialStatsGump(from) );
            
            from.ResetObjectProperties();
            
            from.TitlePrefix = null;
            from.Glassblowing = false; from.Masonry = false; from.SandMining = false; from.StoneMining = false;
            from.LastDonationLife = DateTime.MinValue;
            from.LastOffenseToNature = DateTime.MinValue;
            from.WeaponSpecialization = null;
            from.SecondSpecialization = null;
            from.SpokenLanguage = KnownLanguage.Common;
            from.Stance = null;
            from.CombatManeuver = null;
            from.ChosenDeity = ChosenDeity.None;
            from.CraftingSpecialization = null;
            from.AutoPicking = false; from.BreakLock = false; from.GemHarvesting = false;
            from.InvalidateProperties();

			if( from.Backpack is ArmourBackpack )
			{
				ArmourBackpack pack = from.Backpack as ArmourBackpack;
				pack.BluntBonus = 0;
				pack.PiercingBonus = 0;
				pack.SlashingBonus = 0;
				pack.ColdBonus = 0;
				pack.FireBonus = 0;
				pack.PoisonBonus = 0;
				pack.EnergyBonus = 0;
				pack.Attributes.NightSight = 0;
				pack.Attributes.WeaponDamage = 0;
				pack.Attributes.WeaponSpeed = 0;
				pack.Attributes.AttackChance = 0;
			}

			NullifySkills( from );
			
			if( from.Reforging && !from.OldMapChar )
			{
				if( from.Nation == Nation.Southern )
					from.Feats.SetFeatLevel(FeatList.SouthernLanguage, 3);
				
				else if( from.Nation == Nation.Western )
					from.Feats.SetFeatLevel(FeatList.WesternLanguage, 3);
				
				else if( from.Nation == Nation.Haluaroc )
					from.Feats.SetFeatLevel(FeatList.HaluarocLanguage, 3);
				
				else if( from.Nation == Nation.Mhordul )
					from.Feats.SetFeatLevel(FeatList.MhordulLanguage, 3);
				
				else if( from.Nation == Nation.Tirebladd )
					from.Feats.SetFeatLevel(FeatList.TirebladdLanguage, 3);
				
				else if( from.Nation == Nation.Northern )
					from.Feats.SetFeatLevel(FeatList.NorthernLanguage, 3);
			}
		}
	}
}
