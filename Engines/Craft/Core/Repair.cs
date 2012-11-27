using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
	public class Repair
	{
		public Repair()
		{
		}

		public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			from.Target = new InternalTarget( craftSystem, tool );
			from.SendLocalizedMessage( 1044276 ); // Target an item to repair.
		}

		public static void Do( Mobile from, CraftSystem craftSystem, RepairDeed deed )
		{
			from.Target = new InternalTarget( craftSystem, deed );
			from.SendLocalizedMessage( 1044276 ); // Target an item to repair.
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;
			private RepairDeed m_Deed;

			public InternalTarget( CraftSystem craftSystem, BaseTool tool ) :  base ( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			public InternalTarget( CraftSystem craftSystem, RepairDeed deed ) : base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Deed = deed;
			}

			private int GetWeakenChance( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				// 40% - (1% per hp lost) - (1% per 10 craft skill)
				return (40 + (maxHits - curHits)) - (int)(((m_Deed != null)? m_Deed.SkillLevel : mob.Skills[skill].Value) / 10);
			}

			private bool CheckWeaken( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				return ( GetWeakenChance( mob, skill, curHits, maxHits ) > Utility.Random( 100 ) );
			}

			private int GetRepairDifficulty( int curHits, int maxHits )
			{
				return (((maxHits - curHits) * 1250) / Math.Max( maxHits, 1 )) - 250;
			}

			private bool CheckRepairDifficulty( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				double difficulty = GetRepairDifficulty( curHits, maxHits ) * 0.1;


				if( m_Deed != null )
				{
					double value = m_Deed.SkillLevel;
					double minSkill = difficulty - 25.0;
					double maxSkill = difficulty + 25;

					if( value < minSkill )
						return false; // Too difficult
					else if( value >= maxSkill )
						return true; // No challenge

					double chance = (value - minSkill) / (maxSkill - minSkill);

					return (chance >= Utility.RandomDouble());
				}
				else
				{
					return mob.CheckSkill( skill, difficulty - 25.0, difficulty + 25.0 );
				}
			}

			private bool CheckDeed( Mobile from )
			{
				if( m_Deed != null )
				{
					return m_Deed.Check( from );
				}

				return true;
			}

			private bool IsSpecialWeapon( BaseWeapon weapon )
			{
				// Weapons repairable but not craftable

				if ( m_CraftSystem is DefTinkering )
				{
					return ( weapon is Cleaver )
						|| ( weapon is Hatchet )
						|| ( weapon is Pickaxe )
						|| ( weapon is ButcherKnife )
						|| ( weapon is SkinningKnife );
				}
				else if ( m_CraftSystem is DefCarpentry )
				{
					return ( weapon is Club )
						|| ( weapon is BlackStaff )
						|| ( weapon is MagicWand );
				}
				else if ( m_CraftSystem is DefBlacksmithy )
				{
					return ( weapon is Pitchfork );
				}

				return false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                int number;

				if( !CheckDeed( from ) )
					return;

				bool usingDeed = (m_Deed != null);
				bool toDelete = false;

				//TODO: Make a IRepairable

				if ( targeted is BaseWeapon )
				{
					BaseWeapon weapon = (BaseWeapon)targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 0;

					if ( Core.AOS )
					{
						toWeaken = 1;
					}
					else if ( skill != SkillName.Tailoring )
					{
						double skillLevel = (usingDeed)? m_Deed.SkillLevel : from.Skills[skill].Base;

						if ( skillLevel >= 90.0 )
							toWeaken = 1;
						else if ( skillLevel >= 70.0 )
							toWeaken = 2;
						else
							toWeaken = 3;
					}

					if ( m_CraftSystem.CraftItems.SearchForSubclass( weapon.GetType() ) == null && !IsSpecialWeapon( weapon ) )
					{
						number = (usingDeed)? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
					}
					else if ( !weapon.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( weapon.MaxHitPoints <= 0 || weapon.HitPoints == weapon.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( weapon.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
							weapon.MaxHitPoints -= toWeaken;
							weapon.HitPoints = Math.Max( 0, weapon.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
                            switch (weapon.Resource)
                            {
                                case CraftResource.Copper:
                                    if (from.Backpack.GetAmount(typeof(CopperIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough copper ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(CopperIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Bronze:
                                    if (from.Backpack.GetAmount(typeof(BronzeIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough bronze ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(BronzeIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Iron:
                                    if (from.Backpack.GetAmount(typeof(IronIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough iron ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(IronIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Steel:
                                    if (from.Backpack.GetAmount(typeof(SteelIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough steel ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(SteelIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Obsidian:
                                    if (from.Backpack.GetAmount(typeof(ObsidianIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough obsidian to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ObsidianIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Starmetal:
                                    if (from.Backpack.GetAmount(typeof(StarmetalIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough starmetal ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(StarmetalIngot), 10);
                                    break;
                                case CraftResource.Oak:
                                    if (from.Backpack.GetAmount(typeof(Log)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough oak logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Log), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Redwood:
                                    if (from.Backpack.GetAmount(typeof(RedwoodLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough redwood logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(RedwoodLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Yew:
                                    if (from.Backpack.GetAmount(typeof(YewLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough yew logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(YewLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Ash:
                                    if (from.Backpack.GetAmount(typeof(AshLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough ash logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(AshLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Greenheart:
                                    if (from.Backpack.GetAmount(typeof(GreenheartLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough greenheart logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(GreenheartLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Cotton:
                                    if (from.Backpack.GetAmount(typeof(Cloth)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough cloth to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Cloth), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Wool:
                                    if (from.Backpack.GetAmount(typeof(Wool)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough wool to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Wool), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Silk:
                                    if (from.Backpack.GetAmount(typeof(SpidersSilk)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough silk to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(SpidersSilk), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Satin:
                                    if (from.Backpack.GetAmount(typeof(Satin)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough satin to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Satin), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Velvet:
                                    if (from.Backpack.GetAmount(typeof(Velvet)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough velvet to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Velvet), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Linen:
                                    if (from.Backpack.GetAmount(typeof(Linen)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough linen to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Linen), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.RegularLeather:
                                    if (from.Backpack.GetAmount(typeof(Leather)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Leather), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.ThickLeather:
                                    if (from.Backpack.GetAmount(typeof(ThickLeather)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough thick leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ThickLeather), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.BeastLeather:
                                    if (from.Backpack.GetAmount(typeof(BeastLeather)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough beast leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(BeastLeather), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.ScaledLeather:
                                    if (from.Backpack.GetAmount(typeof(ScaledLeather)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough scaled leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ScaledLeather), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                            }

							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							weapon.HitPoints = weapon.MaxHitPoints;
						}
						else
						{
							number = (usingDeed)? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
							m_CraftSystem.PlayCraftEffect( from );
						}

						toDelete = true;
					}
				}
				else if ( targeted is BaseArmor )
				{
					BaseArmor armor = (BaseArmor)targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 0;

					if ( Core.AOS )
					{
						toWeaken = 1;
					}
					else if ( skill != SkillName.Tailoring )
					{
						double skillLevel = (usingDeed)? m_Deed.SkillLevel : from.Skills[skill].Base;

						if ( skillLevel >= 90.0 )
							toWeaken = 1;
						else if ( skillLevel >= 70.0 )
							toWeaken = 2;
						else
							toWeaken = 3;
					}

					if ( m_CraftSystem.CraftItems.SearchForSubclass( armor.GetType() ) == null )
					{
						number = (usingDeed)? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
					}
					else if ( !armor.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( armor.MaxHitPoints <= 0 || armor.HitPoints == armor.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( armor.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
							armor.MaxHitPoints -= toWeaken;
							armor.HitPoints = Math.Max( 0, armor.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
                            switch (armor.Resource)
                            {
                                case CraftResource.Copper:
                                    if (from.Backpack.GetAmount(typeof(CopperIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough copper ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(CopperIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Bronze:
                                    if (from.Backpack.GetAmount(typeof(BronzeIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough bronze ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(BronzeIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Iron:
                                    if (from.Backpack.GetAmount(typeof(IronIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough iron ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(IronIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Steel:
                                    if (from.Backpack.GetAmount(typeof(SteelIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough steel ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(SteelIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Obsidian:
                                    if (from.Backpack.GetAmount(typeof(ObsidianIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough obsidian to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ObsidianIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Starmetal:
                                    if (from.Backpack.GetAmount(typeof(StarmetalIngot)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough starmetal ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(StarmetalIngot), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Oak:
                                    if (from.Backpack.GetAmount(typeof(Log)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough oak logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Log), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Redwood:
                                    if (from.Backpack.GetAmount(typeof(RedwoodLog)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough redwood logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(RedwoodLog), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Yew:
                                    if (from.Backpack.GetAmount(typeof(YewLog)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough yew logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(YewLog), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Ash:
                                    if (from.Backpack.GetAmount(typeof(AshLog)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough ash logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(AshLog), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Greenheart:
                                    if (from.Backpack.GetAmount(typeof(GreenheartLog)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough greenheart logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(GreenheartLog), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Cotton:
                                    if (from.Backpack.GetAmount(typeof(Cloth)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough cloth to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Cloth), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Wool:
                                    if (from.Backpack.GetAmount(typeof(Wool)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough wool to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Wool), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Silk:
                                    if (from.Backpack.GetAmount(typeof(SpidersSilk)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough silk to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(SpidersSilk), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Satin:
                                    if (from.Backpack.GetAmount(typeof(Satin)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough satin to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Satin), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Velvet:
                                    if (from.Backpack.GetAmount(typeof(Velvet)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough velvet to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Velvet), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.Linen:
                                    if (from.Backpack.GetAmount(typeof(Linen)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough linen to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Linen), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.RegularLeather:
                                    if (from.Backpack.GetAmount(typeof(Leather)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Leather), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.ThickLeather:
                                    if (from.Backpack.GetAmount(typeof(ThickLeather)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough thick leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ThickLeather), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.BeastLeather:
                                    if (from.Backpack.GetAmount(typeof(BeastLeather)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough beast leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(BeastLeather), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                                case CraftResource.ScaledLeather:
                                    if (from.Backpack.GetAmount(typeof(ScaledLeather)) < (armor.MaxHitPoints - armor.HitPoints))
                                    {
                                        from.SendMessage("You do not have enough scaled leather to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ScaledLeather), (armor.MaxHitPoints - armor.HitPoints));
                                    break;
                            }

							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							armor.HitPoints = armor.MaxHitPoints;
						}
						else
						{
							number = (usingDeed)? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
							m_CraftSystem.PlayCraftEffect( from );
						}

						toDelete = true;
					}
				}
				else if ( targeted is BaseClothing )
				{
					BaseClothing clothing = (BaseClothing)targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 0;

					if ( Core.AOS )
					{
						toWeaken = 1;
					}
					else if ( skill != SkillName.Tailoring )
					{
						double skillLevel = (usingDeed) ? m_Deed.SkillLevel : from.Skills[skill].Base;

						if ( skillLevel >= 90.0 )
							toWeaken = 1;
						else if ( skillLevel >= 70.0 )
							toWeaken = 2;
						else
							toWeaken = 3;
					}

					if ( m_CraftSystem.CraftItems.SearchForSubclass( clothing.GetType() ) == null )
					{
						number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
					}
					else if ( !clothing.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( clothing.MaxHitPoints <= 0 || clothing.HitPoints == clothing.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( clothing.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, clothing.HitPoints, clothing.MaxHitPoints ) )
						{
							clothing.MaxHitPoints -= toWeaken;
							clothing.HitPoints = Math.Max( 0, clothing.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, clothing.HitPoints, clothing.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							clothing.HitPoints = clothing.MaxHitPoints;
						}
						else
						{
							number = (usingDeed) ? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
							m_CraftSystem.PlayCraftEffect( from );
						}

						toDelete = true;
					}
				}
				else if( !usingDeed && targeted is BlankScroll )
				{
					SkillName skill = m_CraftSystem.MainSkill;

					if( from.Skills[skill].Value >= 50.0 )
					{
						((BlankScroll)targeted).Consume( 1 );
						RepairDeed deed = new RepairDeed( RepairDeed.GetTypeFor( m_CraftSystem ), from.Skills[skill].Value, from );
						from.AddToBackpack( deed );

						number = 500442; // You create the item and put it in your backpack.
					}
					else
						number = 1047005; // You must be at least apprentice level to create a repair service contract.
				}
				else if ( targeted is Item )
				{
					number = (usingDeed)? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
				}
				else
				{
					number = 500426; // You can't repair that.
				}

				if( !usingDeed )
				{
					CraftContext context = m_CraftSystem.GetContext( from );
					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, number ) );
				}
				else if( toDelete )
				{
					m_Deed.Delete();
				}
			}
		}
	}
}
