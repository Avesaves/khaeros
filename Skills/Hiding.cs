using System;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Multis;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class Hiding
	{
		private static bool m_CombatOverride;

		public static bool CombatOverride
		{
			get{ return m_CombatOverride; }
			set{ m_CombatOverride = value; }
		}

		public static void Initialize()
		{
			SkillInfo.Table[21].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			if ( m.Target != null || m.Spell != null )
			{
				m.SendLocalizedMessage( 501238 ); // You are busy doing something else and cannot hide.
				return TimeSpan.FromSeconds( 1.0 );
			}

			double bonus = 0.0;

			//int range = 18 - (int)(m.Skills[SkillName.Hiding].Value / 10);
			int range = 15;	//Cap of 18 not OSI-exact, intentional difference

			bool badCombat = false;
			bool ok = false;

            int allowed = 0;
            int against = 0;

            if (m is PlayerMobile && ( (PlayerMobile) m).IsVampire && ( (PlayerMobile) m).Feats.GetFeatLevel(FeatList.Obfuscate) > 2 && !( (PlayerMobile) m).VampSafety )
            {
                int hour = ((PlayerMobile)m).GetHour();
                int daycheck = PlayerMobile.GetVampireTimeOffset(hour);
                if (daycheck < 0)
                    daycheck *= -1;
                else
                    daycheck = 0;

                if (daycheck != 0 && daycheck > ((PlayerMobile)m).Feats.GetFeatLevel(FeatList.Daywalker))
                    allowed = 0;                
                else
                    allowed = 40;
            }

			foreach ( Mobile check in m.GetMobilesInRange( range ) )
			{
				if ( check != m && check.InLOS( m ) && check.Alive && !( check.AccessLevel > AccessLevel.Player && check.Hidden ) )
				{
					if( check.Combatant == m || check.Fame >= 1000 || check is PlayerMobile )
					{
						if( (check is Wolf || check is Dog) && ((BaseCreature)check).ControlMaster == m )
							continue;
						
						if( check is PlayerMobile && ((PlayerMobile)check).VisibilityList.Contains(m) )
							continue;

                        if( allowed > 0 )
                        {
                            against += (int)( check.Skills[SkillName.DetectHidden].Base * 0.05 );

                            if( check is PlayerMobile )
                                against += 5 * ( (PlayerMobile)check ).Feats.GetFeatLevel( FeatList.Alertness );

                            else if( check is BaseCreature )
                                against += (int)( ( (BaseCreature)check ).Fame * 0.001 );
								

                            if( check.GetDistanceToSqrt( m.Location ) < 2 )
                                against += 40;

                            else if( check.GetDistanceToSqrt( m.Location ) < 3 )
                                against += 30;

                            else if( check.GetDistanceToSqrt( m.Location ) < 4 )
                                against += 20;

                            else if( check.GetDistanceToSqrt( m.Location ) < 5 )
                                against += 10;
                        }

                        else
						    badCombat = true;

                        if (badCombat)
                            break;
					}
				}
			}

            if( allowed < against )
            {
                if (allowed > 0)
                {
                    m.SendMessage("You failed to hide in plain sight.");

                    badCombat = true;
                }
            }

            else if( allowed > 0 && against > 0 )
            {
                if( ( (PlayerMobile)m ).BPs < 2 )
                {
                    badCombat = true;
                    m.SendMessage( "You lack enough blood points to hide in plain sight." );
                }

                else
                {
                    ( (PlayerMobile)m ).BPs -= 2;
                    m.SendMessage( "You have managed to hide in plain sight." );
                }
            }
			
			ok = ( !badCombat && m.CheckSkill( SkillName.Hiding, 0.0 - bonus, 100.0 - bonus ) );
			
			if ( m.Mounted )
			{
				m.RevealingAction();
				m.LocalOverheadMessage( MessageType.Regular, 0x22, 501237 ); // You can't seem to hide right now.
				return TimeSpan.FromSeconds( 1.0 );
			}

			if ( badCombat )
			{
				m.RevealingAction();

				m.LocalOverheadMessage( MessageType.Regular, 0x22, 501237 ); // You can't seem to hide right now.

				return TimeSpan.FromSeconds( 1.0 );
			}
			
			else 
			{
				if ( ok )
				{
					m.Hidden = true;
					m.LocalOverheadMessage( MessageType.Regular, 0x1F4, 501240 ); // You have hidden yourself well.
					
					if( m is PlayerMobile )
					{
						PlayerMobile tracker = m as PlayerMobile;
								
						foreach( Mobile mob in m.GetMobilesInRange( 2 ) )
						{
							if( (mob is Wolf || mob is Dog) && ((BaseCreature)mob).ControlMaster == m )
							{
								mob.Hidden = true;
								mob.AllowedStealthSteps = 2;
							}
						}
					}
				}
				else
				{
					m.RevealingAction();
					m.LocalOverheadMessage( MessageType.Regular, 0x22, 501241 ); // You can't seem to hide here.
				}

				return TimeSpan.FromSeconds( 10.0 );
			}
		}
	}
}
