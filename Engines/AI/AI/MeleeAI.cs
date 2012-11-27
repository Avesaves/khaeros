using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

//
// This is a first simple AI
//
//

namespace Server.Mobiles
{
	public class MeleeAI : BaseAI
	{
		public MeleeAI(BaseCreature m) : base (m)
		{
		}

		public override bool DoActionWander()
		{
			m_Mobile.DebugSay( "I have no combatant" );

            if (m_Mobile is Soldier)
                (m_Mobile as Soldier).OnThink();

			if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug && m_Mobile.FocusMob != null )
					m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				
				m_Mobile.RevealingAction();
				
				if( m_Mobile.IsSneaky && m_Mobile.Combatant != null )
				{
					string name = "a creature";
					
					if( m_Mobile.Combatant.Name != null )
						name = m_Mobile.Combatant.Name;
					
					if( m_Mobile is Octopod || m_Mobile is WhippingVine )
						m_Mobile.Emote( "*emerges from the water to attack " + name + "*" );
					
					else if( m_Mobile is Petal )
						m_Mobile.Emote( "*flies out of a tree's foliage to attack " + name + "*" );
					
					else if( m_Mobile is Wraith )
						m_Mobile.Emote( "*manifests itself out of thin air to attack " + name + "*" );
					
					else if( m_Mobile is InsulariiInfiltrator )
						m_Mobile.Emote( "*comes out of the shadows to attack " + name + "*" );
						               
					else
						m_Mobile.Emote( "*leaps from the shadows to attack " + name + "*" );
				}
				
				Action = ActionType.Combat;
			}
			else
			{
                if (m_Mobile.IsSneaky)
                    m_Mobile.Hidden = true;

                else
                {
                    if (m_Mobile is BirdOfPrey && (m_Mobile as BirdOfPrey).Retrieving)
                        (m_Mobile as BirdOfPrey).OnThink();
                    else
                        base.DoActionWander();
                }
			}

			return true;
		}

		public override bool DoActionCombat()
		{
			if ( CombatSystemAttachment.GetCSA( m_Mobile ).DefenseTimer != null )
				return true;
			Mobile combatant = m_Mobile.Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet )
			{
				m_Mobile.DebugSay( "My combatant is gone, so my guard is up" );

				Action = ActionType.Guard;

				return true;
			}

			if ( !m_Mobile.InRange( combatant, m_Mobile.RangePerception ) )
			{
				// They are somewhat far away, can we find something else?

				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.Combatant = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
				else if ( !m_Mobile.InRange( combatant, m_Mobile.RangePerception * 3 ) )
				{
					m_Mobile.Combatant = null;
				}

				combatant = m_Mobile.Combatant;

				if ( combatant == null )
				{
					m_Mobile.DebugSay( "My combatant has fled, so I am on guard" );
					Action = ActionType.Guard;

					return true;
				}
			}

			/*if ( !m_Mobile.InLOS( combatant ) )
			{
				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.Combatant = combatant = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
			}*/

			if ( MoveTo( combatant, true, Math.Max( m_Mobile.Weapon.MaxRange, m_Mobile.RangeFight ) ) )
			{
				m_Mobile.Direction = m_Mobile.GetDirectionTo( combatant );
			}
			else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;

				return true;
			}
			else if ( m_Mobile.GetDistanceToSqrt( combatant ) > m_Mobile.RangePerception + 1 )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I cannot find {0}, so my guard is up", combatant.Name );

				Action = ActionType.Guard;

				return true;
			}
			else
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I should be closer to {0}", combatant.Name );
			}

			if ( !m_Mobile.Controlled && !m_Mobile.Summoned && !m_Mobile.IsParagon && !( m_Mobile is IEnraged ) )
			{
				if ( m_Mobile.Hits < m_Mobile.HitsMax * 20/100 )
				{
					// We are low on health, should we flee?

					bool flee = false;

					if ( m_Mobile.Hits < combatant.Hits )
					{
						// We are more hurt than them

						int diff = combatant.Hits - m_Mobile.Hits;

						flee = ( Utility.Random( 0, 100 ) < (10 + diff) ); // (10 + diff)% chance to flee
					}
					else
					{
						flee = Utility.Random( 0, 100 ) < 10; // 10% chance to flee
					}

					if ( flee )
					{
						if ( m_Mobile.Debug )
							m_Mobile.DebugSay( "I am going to flee from {0}", combatant.Name );

						Action = ActionType.Flee;
					}
				}
			}
			
			if( !(m_Mobile.Weapon is Fists) && m_Mobile.GetDistanceToSqrt( m_Mobile.Combatant ) < m_Mobile.Weapon.MaxRange )
			{
			   	Action = ActionType.Flee;
				return true;
			}

			return true;
		}

		public override bool DoActionGuard()
		{
			if ( AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true) )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
			}
			else
			{
				base.DoActionGuard();
			}

			return true;
		}

		public override bool DoActionFlee()
		{
			if ( m_Mobile.Hits > m_Mobile.HitsMax/2 )
			{
				if( m_Mobile.Combatant != null && !(m_Mobile.Weapon is Fists) && m_Mobile.GetDistanceToSqrt( m_Mobile.Combatant ) < m_Mobile.Weapon.MaxRange )
				{
					m_Mobile.FocusMob = m_Mobile.Combatant;
					base.DoActionFlee();
					return true;
				}
				
				m_Mobile.DebugSay( "I have enough hits and my enemy is within safe distance, so I will continue fighting" );
				Action = ActionType.Combat;
			}
			
			else if( !( m_Mobile is IEnraged ) )
			{
				m_Mobile.FocusMob = m_Mobile.Combatant;
				base.DoActionFlee();
			}
			
			else if( m_Mobile.Combatant != null && !(m_Mobile.Weapon is Fists) && m_Mobile.GetDistanceToSqrt( m_Mobile.Combatant ) < m_Mobile.Weapon.MaxRange )
			{
				m_Mobile.FocusMob = m_Mobile.Combatant;
				base.DoActionFlee();
			}
			
			else
			{
				Action = ActionType.Combat;
			}

			return true;
		}
	}
}
