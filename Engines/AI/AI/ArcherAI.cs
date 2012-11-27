using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	public class ArcherAI : BaseAI
	{
		public ArcherAI(BaseCreature m) : base (m)
		{
		}

		public override bool DoActionWander()
		{
			m_Mobile.DebugSay( "I have no combatant" );

            if (m_Mobile is Soldier)
                (m_Mobile as Soldier).OnThink();

			if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I have detected {0} and I will attack", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
			}
			else
			{
				return base.DoActionWander();
			}

			return true;
		}

		public override bool DoActionCombat()
		{
			if ( m_Mobile.Combatant == null || m_Mobile.Combatant.Deleted || !m_Mobile.Combatant.Alive || m_Mobile.Combatant.IsDeadBondedPet )
			{
				m_Mobile.DebugSay("My combatant is deleted");
				Action = ActionType.Guard;
				return true;
			}

			if ( (m_Mobile.LastMoveTime + TimeSpan.FromSeconds( 1.0 )) < DateTime.Now && m_Mobile.Weapon != null )
			{
				int minRange = 3;
				int myRange = ((BaseWeapon)m_Mobile.Weapon).MaxRange;
				if ( m_Mobile is BaseCreature && ((BaseCreature)m_Mobile).RangeFight > myRange )
					myRange = ((BaseCreature)m_Mobile).RangeFight;
				if (WalkMobileRange(m_Mobile.Combatant, 6, true, minRange, myRange))
				{
					// Be sure to face the combatant
					// We moved! Our opponent could've gotten an AOO at this point and we could've died, thus resetting our combatant!
					if ( m_Mobile.Combatant != null )
						m_Mobile.Direction = m_Mobile.GetDirectionTo(m_Mobile.Combatant.Location);
				}
				
				else
				{
					if ( m_Mobile.Combatant != null )
					{
						if ( m_Mobile.Debug )
							m_Mobile.DebugSay( "I am still not in range of {0}", m_Mobile.Combatant.Name);

						if ( (int) m_Mobile.GetDistanceToSqrt( m_Mobile.Combatant ) > m_Mobile.RangePerception + 1 )
						{
							if ( m_Mobile.Debug )
								m_Mobile.DebugSay( "I have lost {0}", m_Mobile.Combatant.Name);

							m_Mobile.Combatant = null;
							Action = ActionType.Guard;
							return true;
						}
					}
				}
			}

			// When we have no ammo, we flee
			Container pack = m_Mobile.Backpack;

			if ( pack == null || ( pack.FindItemByType( typeof( Arrow ) ) == null && pack.FindItemByType( typeof( Bolt ) ) == null ) )
			{
				Action = ActionType.Flee;
				return true;
			}

			
			/*if( m_Mobile.Combatant != null && m_Mobile.InRange( m_Mobile.Combatant, 6 ) )
			{
				m_Mobile.DebugSay( "I am too close from my enemy, I will back off." );
			   	Action = ActionType.Flee;
				return true;
			}*/
			
			// At 20% we should check if we must leave
			if ( m_Mobile.Hits < m_Mobile.HitsMax*20/100 )
			{
				bool bFlee = false;
				// if my current hits are more than my opponent, i don't care
				if ( m_Mobile.Combatant != null && m_Mobile.Hits < m_Mobile.Combatant.Hits)
				{
					int iDiff = m_Mobile.Combatant.Hits - m_Mobile.Hits;

					if ( Utility.Random(0, 100) > 10 + iDiff) // 10% to flee + the diff of hits
					{
						bFlee = true;
					}
				}
				else if ( m_Mobile.Combatant != null && m_Mobile.Hits >= m_Mobile.Combatant.Hits)
				{
					if ( Utility.Random(0, 100) > 10 ) // 10% to flee
					{
						bFlee = true;
					}
				}
						
				if (bFlee)
				{
					Action = ActionType.Flee; 
				}
			}

			return true;
		}
		
		public override bool DoActionFlee()
		{
			if ( m_Mobile.Hits > m_Mobile.HitsMax/3 )
			{
				if( m_Mobile.Combatant != null && m_Mobile.InRange( m_Mobile.Combatant, 6 ) )
				{
					m_Mobile.FocusMob = m_Mobile.Combatant;
					base.DoActionFlee();
				}
				
				m_Mobile.DebugSay( "I have enough hits and my enemy is within safe distance, so I will continue fighting" );
				Action = ActionType.Combat;
			}
			
			else
			{
				m_Mobile.FocusMob = m_Mobile.Combatant;
				base.DoActionFlee();
			}

			return true;
		}

		public override bool DoActionGuard()
		{
			if ( AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
				m_Mobile.ControlTarget = m_Mobile.Combatant;
				m_Mobile.ControlOrder = OrderType.Attack;
			}
			else
			{
				base.DoActionGuard();
			}

			return true;
		}
	}
}
