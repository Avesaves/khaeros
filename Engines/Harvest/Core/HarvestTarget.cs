using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Spells;

namespace Server.Engines.Harvest
{
	public class HarvestTarget : Target
	{
		private Item m_Tool;
		private HarvestSystem m_System;

		public HarvestTarget( Item tool, HarvestSystem system ) : base( -1, true, TargetFlags.None )
		{
			m_Tool = tool;
			m_System = system;

			DisallowMultis = true;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
//			if ( m_System is Mining && targeted is StaticTarget )
//			{
//				int itemID = ((StaticTarget)targeted).ItemID;
//
//				// grave
//				if ( itemID == 0xED3 || itemID == 0xEDF || itemID == 0xEE0 || itemID == 0xEE1 || itemID == 0xEE2 || itemID == 0xEE8 )
//				{
//					PlayerMobile player = from as PlayerMobile;
//
//					if ( player != null )
//					{
//						QuestSystem qs = player.Quest;
//
//						if ( qs is WitchApprenticeQuest )
//						{
//							FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;
//
//							if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.Bones )
//							{
//								player.SendLocalizedMessage( 1055037 ); // You finish your grim work, finding some of the specific bones listed in the Hag's recipe.
//								obj.Complete();
// WITCHCRAFT
//								return;
//							}
//						}
//					}
//				}
//			}
            ///////
            if (targeted is LandTarget && m_Tool is Shovel && ( (targeted as LandTarget).Name == "grass" || (targeted as LandTarget).Name == "forest" || (targeted as LandTarget).Name == "furrows") )
            {
                    LandTarget t = (LandTarget)targeted;
                    Map map = from.Map;

                    if (t.Name == "grass" || t.Name == "forest" || t.Name == "furrows")
                    {
                        bool tilled = false;
                        IPooledEnumerable eable = map.GetItemsInRange(t.Location, 0);
                        foreach (Item i in eable)
                        {
                            if (i is FarmSoil)
                            {
                                tilled = true;
                                continue;
                            }
                        }
                        eable.Free();
                       

                        if ((!map.CanSpawnMobile(t.X, t.Y, t.Z)) || (SpellHelper.CheckMulti(new Point3D(t), map)))
                        {
                            from.SendLocalizedMessage(501942); // That location is blocked.
                        }
                        else if(tilled)
                            from.SendMessage("This land is already tilled.");
                        else
                        {
                            int FarmingFeatLevel = (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Farming));

                                if (!from.Mounted)
                                {
                                    if (FarmingFeatLevel > 0)
                                    {
                                        if (from.InRange(((LandTarget)targeted).Location, 1) && from.CanSee(targeted))
                                        {
                                                
                                            ((Shovel)m_Tool).UsesRemaining -= 1;
                                            
                                            if (m_Tool != null && ((Shovel)m_Tool).UsesRemaining < 1)
                                            {
                                                from.SendMessage("Your shovel is worn out!");
                                                from.PlaySound(0x049);
                                                m_Tool.Delete();
                                            }

                                            FarmSoil newSoil = new FarmSoil(from);
                                            newSoil.setOwner(from);
                                            newSoil.MoveToWorld(t.Location, from.Map);
                                            from.PlaySound(0x365);
                                            from.SendMessage("You till the soil, preparing it for seeding.");

                                            from.Direction = from.GetDirectionTo(t.Location);

                                            //from.Animate(Utility.RandomList(def.EffectActions), 5, 1, true, false, 0);
                                        }
                                        else
                                            from.SendMessage("You are too far away to till there.");
                                    }
                                    else
                                        from.SendMessage("You do not know enough about agriculture to do that.");
                                }
                                else
                                    from.SendMessage("You cannot till soil while mounted.");
                        }
                    }
                    else
                        from.SendMessage("You cannot till that."); 
                }
                else if ( m_System is Lumberjacking && targeted is IChopable )
				    ((IChopable)targeted).OnChop( from );
			    else if ( m_System is Lumberjacking && FurnitureAttribute.Check( targeted as Item ) )
				    DestroyFurniture( from, (Item)targeted );
			    else if ( m_System is Mining && targeted is TreasureMap )
				    ((TreasureMap)targeted).OnBeginDig( from );
			    else
				    m_System.StartHarvesting( from, m_Tool, targeted );
		}

		private void DestroyFurniture( Mobile from, Item item )
		{
			if ( !from.InRange( item.GetWorldLocation(), 3 ) )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
				return;
			}
			else if ( !item.IsChildOf( from.Backpack ) && !item.Movable )
			{
				from.SendLocalizedMessage( 500462 ); // You can't destroy that while it is here.
				return;
			}

			from.SendLocalizedMessage( 500461 ); // You destroy the item.
			Effects.PlaySound( item.GetWorldLocation(), item.Map, 0x3B3 );

			if ( item is Container )
			{
				if ( item is TrapableContainer )
					(item as TrapableContainer).ExecuteTrap( from );

				((Container)item).Destroy();
			}
			else
			{
				item.Delete();
			}
		}
	}
}
