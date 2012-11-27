// Modified by Alari to include mushroom hunting.
//
// TODO: Seasonal (fall) mushrooms can't be targetted!
//

using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.SkillHandlers
{
	public class HerbalLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.HerbalLore].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage( 502807 ); // What would you like to taste?

			return TimeSpan.FromSeconds( 1.0 );
		}

		private class InternalTarget : Target
		{
			public InternalTarget() :  base ( 2, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					from.SendLocalizedMessage( 502816 ); // You feel that such an action would be inappropriate
				}
				else if ( targeted is Food )
				{
					if ( from.CheckTargetSkill( SkillName.HerbalLore, targeted, 0, 100 ) )
					{
						Food targ = (Food)targeted;

						if ( targ.Poison != null )
						{
							from.SendLocalizedMessage( 1038284 ); // It appears to have poison smeared on it
						}
						else
						{
							// No poison on the food
							from.SendLocalizedMessage( 502823 ); // You cannot discern anything about this substance
						}
					}
					else
					{
						// Skill check failed
						from.SendLocalizedMessage( 502823 ); // You cannot discern anything about this substance
					}
				}


// HerbalLore: Mushroom harvesting by Alari - insert below "else if ( targeted is Food )" block
// TODO: Make it work on !movable Items that are not in a player's house.
// TODO: make it deplete resource bank for that location?

				else if ( targeted is StaticTarget )
				{
					StaticTarget targ = (StaticTarget)targeted;

					if ( targ.ItemID >= 3340 && targ.ItemID <= 3348 )
					{
						if ( from.CheckTargetSkill( SkillName.HerbalLore, targeted, 0, 100 ) )
						{
							from.AddToBackpack( new Mushrooms( targ.ItemID ) );
							from.SendMessage( "You find some edible mushrooms!" );
						}
						else
						{
							from.SendMessage( "You fail to find any edible mushrooms." );
						}
					}
				}

				else if ( targeted is AddonComponent )
				{
					AddonComponent targ = (AddonComponent)targeted;

					if ( targ.ItemID >= 3340 && targ.ItemID <= 3348 )
					{
						if ( from.CheckTargetSkill( SkillName.HerbalLore, targeted, 0, 100 ) )
						{
							from.AddToBackpack( new Mushrooms( targ.ItemID ) );
							from.SendMessage( "You find some edible mushrooms!" );
						}
						else
						{
							from.SendMessage( "You fail to find any edible mushrooms." );
						}
					}
				}

// end HerbalLore: Mushroom harvesting


				else
				{
					// The target is not food. (Potion support in the next version)
					from.SendLocalizedMessage( 502820 ); // That's not something you can taste
				}
			}
		}
	}
}
