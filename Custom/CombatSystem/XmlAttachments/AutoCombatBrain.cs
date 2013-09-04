using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Misc;
using Server.Network;
using Server.Engines.BodyAnimationData;

namespace Server.Engines.XmlSpawner2 
{
	public class AutoCombatBrain
	{
		private class ACEntry 
		{
			private int m_Swing = 0;
			private int m_Thrust = 0;
			private int m_Overhead = 0;
			private Item m_Weapon = null;
			
			public int Swing{ get{ return m_Swing; } set { m_Swing = value; } }
			public int Thrust{ get{ return m_Thrust; } set { m_Thrust = value; } }
			public int Overhead{ get{ return m_Overhead; } set { m_Overhead = value; } }
			public Item Weapon{ get{ return m_Weapon; } set { m_Weapon = value; } }
		}
		
		public void RegisterFrequency( AttackType type, Mobile mob )
		{
			if ( !m_FrequencyTable.ContainsKey( mob ) )
			{
				if ( m_FrequencyTable.Count >= 20 ) // wipe if too big
					m_FrequencyTable = new Dictionary< Mobile, ACEntry >();
					
				m_FrequencyTable[mob] = new ACEntry();
				m_FrequencyTable[mob].Weapon = mob.Weapon as Item;
			}
			if ( mob.Weapon != m_FrequencyTable[mob].Weapon )
			{
				m_FrequencyTable[mob].Weapon = mob.Weapon as Item;
				m_FrequencyTable[mob].Swing = m_FrequencyTable[mob].Thrust = m_FrequencyTable[mob].Overhead = 0;
			}
			
			if ( type == AttackType.Swing )
				m_FrequencyTable[mob].Swing++;
			else if ( type == AttackType.Thrust )
				m_FrequencyTable[mob].Thrust++;
			else if ( type == AttackType.Overhead )
				m_FrequencyTable[mob].Overhead++;
		}
		
		private Dictionary< Mobile, ACEntry > m_FrequencyTable = new Dictionary< Mobile, ACEntry >();
		private bool m_Enabled;
		private Mobile m_Body;
		private bool m_Attack = true; // if this is set to false, the mob will only parry
		private bool m_Parry = true; // if this is set to false, the mob will only attack
		private bool m_PerformThrust = true;
		private bool m_PerformSwing = true;
		private bool m_PerformOverhead = true;
		private bool m_ParryThrust = true;
		private bool m_ParrySwing = true;
		private bool m_ParryOverhead = true;
		private bool m_WaitForAimedShot = false;
		
		private int m_QueuedAction = -1;
		
		private bool m_IncomingCharge = false;
		private AttackType m_IncomingChargeDirection = AttackType.Invalid;
		
		private AutoCombatBrainTimer m_Timer;
		
		public bool Enabled { get { return m_Enabled; } }
		public bool Parry{ get{ return m_Parry; } set{ m_Attack = value; } }
		public bool Attack{ get{ return m_Attack; } set{ m_Attack = value; } }
		public Mobile Body{ get{ return m_Body; } }
		public bool PerformThrust{ get{ return m_PerformThrust; } set{ m_PerformThrust = value; } }
		public bool PerformSwing{ get{ return m_PerformSwing; } set{ m_PerformSwing = value; } }
		public bool PerformOverhead{ get{ return m_PerformOverhead; } set{ m_PerformOverhead = value; } }
		
		public bool ParryThrust{ get{ return m_ParryThrust; } set{ m_ParryThrust = value; } }
		public bool ParrySwing{ get{ return m_ParrySwing; } set{ m_ParrySwing = value; } }
		public bool ParryOverhead{ get{ return m_ParryOverhead; } set{ m_ParryOverhead = value; } }
		
		public AutoCombatBrain( Mobile mobile, bool ACParry, bool ACAttack, bool ACParrySwing, bool ACParryThrust, bool ACParryOverhead, bool ACAttackSwing, bool ACAttackThrust, bool ACAttackOverhead, bool waitForAimedShot )
		{
			m_Body = mobile;
			m_Parry = ACParry;
			m_Attack = ACAttack;
			if ( mobile.Body.Type != BodyType.Human )
			{
				/*if ( BAData.GetAnimation( mobile, DefenseType.ParrySwing ) == null )
					ACParrySwing = false;
				if ( BAData.GetAnimation( mobile, DefenseType.ParryThrust ) == null )
					ACParryThrust = false;
				if ( BAData.GetAnimation( mobile, DefenseType.ParryOverhead ) == null )
					ACParryOverhead = false;*/
				if ( BAData.GetAnimation( mobile, AttackType.Swing ) == null && BAData.GetAnimation( mobile, AttackType.Thrust ) == null &&
					BAData.GetAnimation( mobile, AttackType.Overhead ) == null )
				{
					ACAttackSwing = false;
					ACAttackThrust = false;
					ACAttackOverhead = false;
				}
					
				if ( !ACParrySwing && !ACParryThrust && !ACParryOverhead )
					m_Parry = false;
				if ( !ACAttackSwing && !ACAttackOverhead && !ACAttackThrust )
					m_Attack = false;
			}
			
			m_ParrySwing = ACParrySwing;
			m_ParryThrust = ACParryThrust;
			m_ParryOverhead = ACParryOverhead;
			m_PerformSwing = ACAttackSwing;
			m_PerformThrust = ACAttackThrust;
			m_PerformOverhead = ACAttackOverhead;
			
			m_WaitForAimedShot = waitForAimedShot;
			Enable();
		}
		
		public void DelayThink( TimeSpan duration ) // call Act() after AT MOST this much time ( could be sooner )
		{
			if ( Enabled )
			{
				if ( m_Timer == null )
					m_Timer = new AutoCombatBrainTimer( this );
				else if ( m_Timer.Running )
				{
					if ( m_Timer.Next > DateTime.Now + duration )
						m_Timer.Stop();
					else
						return; // we need to call it sooner, so ignore
				}
					
				m_Timer.Delay = duration;
				m_Timer.Start();
			}
		}
		
		public void Disable()
		{
			m_Enabled = false;
			if ( m_Timer != null )
			{
				if ( m_Timer.Running )
					m_Timer.Stop();
				m_Timer = null;
			}
		}
		
		public void Enable()
		{
			m_Enabled = true;
			if ( m_Timer == null )
				m_Timer = new AutoCombatBrainTimer( this );
		}
		
		public void ChargeAlert( AttackType attacktype )
		{
			m_IncomingChargeDirection = attacktype;
			m_IncomingCharge = true;
		}
		
		public virtual bool ValidateCombat()
		{
			if ( m_Body == null || m_Body.Combatant == null || m_Body.Combatant.Deleted || !m_Body.Combatant.Alive || 
				 !m_Body.Alive || !m_Body.Warmode || m_Body.Deleted || m_Body.IsDeadBondedPet || m_Body.Combatant.IsDeadBondedPet )
				return false; // Gregor Samsa was here.
			
			if ( m_Body is BaseMount && ((BaseCreature)m_Body).ControlMaster != null )
				return false;
			
			if ( !m_Enabled )
				return false;

			if ( m_Body is BaseCreature && ( ((BaseCreature)m_Body).AIObject != null && ( ((BaseCreature)m_Body).AIObject.Action != ActionType.Combat ) ) )
				return false; // can't they just leave me in peace?!
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			
			if ( !m_Body.CanSee( m_Body.Combatant ) || !m_Body.InLOS( m_Body.Combatant ) )
				return false;
			
			if (m_IncomingCharge && csa.DefenseTimer != null)
				return false;
			
			if ( !(csa.CanBeginCombatAction()) || csa.Charging )
				return false;
			
			if ( csa.QueuedActionTimer != null ) // something is queued, let the queue handle it
				return false;
				
			return true;
		}
		
		public virtual bool AttemptChargeParry(bool clairvoyance)
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			DefenseType deftype = DefenseType.Invalid;
			List<DefenseType> possibleDefenses = new List<DefenseType>();
			if ( ParryThrust )
				possibleDefenses.Add( DefenseType.ParryThrust );
			if ( ParryOverhead )
				possibleDefenses.Add( DefenseType.ParryOverhead );
			if ( ParrySwing )
				possibleDefenses.Add( DefenseType.ParrySwing );
			if ( possibleDefenses.Count == 0 ) // this really shouldn't happen
				return false;

			if ( m_Body is BaseCreature || clairvoyance )
			{
				if ( possibleDefenses.Contains( (DefenseType)m_IncomingChargeDirection ) )
				{ // we're smart enough to not try to parry if we are unable to
					deftype = ((DefenseType)m_IncomingChargeDirection);
				}
			}
			else
			{
				int dieRoll = Utility.Random( 0, possibleDefenses.Count );
				deftype = possibleDefenses[dieRoll];
			}

			if ( deftype != DefenseType.Invalid ) // if we decided to skip defense, let's continue
			{
				csa.BeginDefense( deftype );
				return true;
			}
			return false;
		}
		
		public virtual bool AttemptDefense( bool clairvoyance )
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			BaseWeapon opponentWeapon = m_Body.Combatant.Weapon as BaseWeapon;
			CombatSystemAttachment opponentCSA = CombatSystemAttachment.GetCSA( m_Body.Combatant );
			Mobile opponent = m_Body.Combatant;
			bool rotating = ( opponentCSA.RotationTimer != null );
			DateTime finishTime = rotating ? opponentCSA.RotationTimer.FinishTime : opponentCSA.AttackTimer.FinishTime;
			
			if ( (finishTime - DateTime.Now) > TimeSpan.FromSeconds( csa.GetParryDuration() ) )
			{ // parrying now would be too soon
				return false;
			}
			else
			{
				DefenseType deftype = DefenseType.Invalid;
				List<DefenseType> possibleDefenses = new List<DefenseType>();
				if ( ParryThrust )
					possibleDefenses.Add( DefenseType.ParryThrust );
				if ( ParryOverhead )
					possibleDefenses.Add( DefenseType.ParryOverhead );
				if ( ParrySwing )
					possibleDefenses.Add( DefenseType.ParrySwing );
				if ( possibleDefenses.Count == 0 ) // this really shouldn't happen
					return false;
					
				if ( !clairvoyance ) // this is effectively the same as guessing
				{
					int dieRoll = Utility.Random( 0, possibleDefenses.Count );
					if ( !m_FrequencyTable.ContainsKey( opponent ) )
					{
						// use weapon direction percentages to calculate probability estimation
						if ( Utility.RandomDouble() < opponentWeapon.SwingPercentage && possibleDefenses.Contains( DefenseType.ParrySwing) )
							deftype = DefenseType.ParrySwing;
						else if ( Utility.RandomDouble() < opponentWeapon.OverheadPercentage && possibleDefenses.Contains( DefenseType.ParryOverhead ) )
							deftype = DefenseType.ParryOverhead;
						else if ( possibleDefenses.Contains( DefenseType.ParryThrust ) )
							deftype = DefenseType.ParryThrust;
						else
							deftype = possibleDefenses[dieRoll];
					}
					else
					{
						int total = m_FrequencyTable[opponent].Swing + m_FrequencyTable[opponent].Thrust + m_FrequencyTable[opponent].Overhead;
						if ( total > 0 )
						{
							if ( Utility.RandomDouble() < ((double)m_FrequencyTable[opponent].Swing) / ((double)total) && possibleDefenses.Contains( DefenseType.ParrySwing ) )
								deftype = DefenseType.ParrySwing;
							else if ( Utility.RandomDouble() < ((double)m_FrequencyTable[opponent].Thrust) / ((double)total) && possibleDefenses.Contains( DefenseType.ParryThrust ) )
								deftype = DefenseType.ParryThrust;
							else if ( possibleDefenses.Contains( DefenseType.ParryOverhead ) )
								deftype = DefenseType.ParryOverhead;
							else 
								deftype = possibleDefenses[dieRoll];
						}
					}
				}
				else
				{
					DefenseType correct = (rotating ? DefenseType.ParrySwing : (DefenseType)(opponentCSA.AttackTimer.Type));
					// we'll do the right action, if we can, otherwise, nothing
					if ( possibleDefenses.Contains( correct ) )
						deftype = correct;
				}
				
				if ( deftype != DefenseType.Invalid )
				{
					csa.BeginDefense( deftype );
					return true;
				} // otherwise, perform something else
			}
			
			return false;
		}
		
		public virtual bool AttemptRangedAttack()
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			IKhaerosMobile km = m_Body as IKhaerosMobile;
			BaseWeapon weapon = m_Body.Weapon as BaseWeapon;
			if ( !(weapon is BaseRanged) )
				return false;
			Container pack = m_Body.Backpack;
			if ( ( !(m_Body is PlayerMobile) && !(m_Body is Mercenary) ) || 
			( pack != null && pack.FindItemByType( ((BaseRanged)weapon).AmmoType ) != null ) )
			{
				if ( csa.AimingTimer != null )
				{
					DelayThink( CombatSystemAttachment.Max( TimeSpan.Zero, 
						csa.AimingTimer.Next - DateTime.Now ) );
					return true;
				}
				else if ( csa.Aiming )
				{
					if ( ((BaseRanged)weapon).AmmoType == typeof( Bolt ) && m_WaitForAimedShot && csa.CanUseAimedShot() )
					{
						if ( csa.AimConverge > DateTime.Now )
						{
							DelayThink( CombatSystemAttachment.Max( TimeSpan.Zero, 
								csa.AimConverge - DateTime.Now ) );
							return true;
						}
					}
					
					return csa.Fire();
				}
				else if ( csa.CanBeginAiming() )
				{
					if (((BaseRanged)weapon).IsStill( m_Body ) )
					{
						if ( m_Body.CanSee( m_Body.Combatant ) && m_Body.InLOS( m_Body.Combatant ) )
						{
							if ( !m_Body.InRange( m_Body.Combatant, 1 ) )
							{
								csa.BeginAiming();
								return true;
							}
						}
					}
					else
					{
						DelayThink( CombatSystemAttachment.Max( TimeSpan.Zero, 
						DateTime.Now - m_Body.LastMoveTime + ((BaseRanged)weapon).RequiredStillTime( m_Body ) ) );
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual bool AttemptThrownAttack()
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			BaseWeapon weapon = m_Body.Weapon as BaseWeapon;
			if ( csa.CanThrow() )
			{
				if ( weapon is BaseRanged && ((BaseRanged)weapon).IsStill( m_Body ) )
				{
					csa.BeginAttack( AttackType.Throw );
					return true;
				}
				else
				{
					DelayThink( CombatSystemAttachment.Max( TimeSpan.Zero, 
					DateTime.Now - m_Body.LastMoveTime + ((BaseRanged)weapon).RequiredStillTime( m_Body ) ) );
					return true;
				}
			}
			
			return false;
		}
		
		public virtual bool AttemptMeleeAttack( bool clairvoyance )
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			CombatSystemAttachment opponentCSA = CombatSystemAttachment.GetCSA( m_Body.Combatant );
			AttackType attack = AttackType.Invalid;
			AttackType candidate = AttackType.Invalid;
			List<AttackType> possibleAttacks = csa.GetPossibleAttacks( true );
			double myAttackAnimationDuration = csa.ComputeAnimationDelay();
			if ( opponentCSA.AttackTimer != null && opponentCSA.AttackTimer.FinishTime > DateTime.Now + TimeSpan.FromSeconds( myAttackAnimationDuration ) ) // let's attempt an interrupt
			{ // it cannot be a circular, or else AttackTimer would be null
				if ( opponentCSA.AttackTimer.Type != AttackType.ShieldBash )
				{
					AttackType interruptDirection = opponentCSA.AttackTimer.Type;
					if ( clairvoyance ) // success check
					{
						if ( possibleAttacks.Contains( interruptDirection ) )
						{
							attack = interruptDirection;
						}
					}
					else // failed
					{
						foreach ( AttackType cand in possibleAttacks )
						{
							if ( candidate == AttackType.Invalid )
								candidate = cand;
							else if ( cand == interruptDirection )
								continue; // skip correct
						}
						attack = candidate;
					}
				}
			}
			
			if ( possibleAttacks.Count > 0 )
			{
				if ( attack == AttackType.Invalid ) // random guesing
				{
					if ( opponentCSA.AttackTimer == null || m_Body.Combatant.Combatant != m_Body ) // if they are attacking, we'd have tried to interrupt if we could
					{
						int dieRoll = Utility.Random( 0, possibleAttacks.Count );
						csa.BeginAttack( possibleAttacks[dieRoll] );
						return true;
					}
				}
				else
				{
					csa.BeginAttack( attack );
					return true;
				}
			}
			
			return false;
		}
		
		public virtual bool AttemptMeleeAttackDefending() // opponent is defending
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			CombatSystemAttachment opponentCSA = CombatSystemAttachment.GetCSA( m_Body.Combatant );
			List<AttackType> possibleAttacks = csa.GetPossibleAttacks( true );
					
			if ( possibleAttacks.Count > 0 )
			{
				foreach ( AttackType atkType in possibleAttacks )
				{
					if ( (AttackType)(opponentCSA.DefenseTimer.Type) != atkType ) // they're not defending this one
					{
						csa.BeginAttack( atkType );
						return true;
					}
				}
			}
			
			return false;
		}
		
		public virtual void Act() // called when interesting things occur
		{
			if ( !ValidateCombat() )
				return;
				
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			
			// only the shorter of these three timespans will be used
			if ( csa.NextAttackAction > DateTime.Now )
				DelayThink( csa.NextAttackAction - DateTime.Now );
			if ( csa.NextDefenseAction > DateTime.Now )
				DelayThink( csa.NextDefenseAction - DateTime.Now );
				
			DelayThink( TimeSpan.FromMilliseconds( 100 ) ); // should be called every ms
			
			bool beingCharged = m_IncomingCharge;
			m_IncomingCharge = false;

			double intel = m_Body.Int;
			double AIQ = intel / 140.0; // max AIQ at 150 int, which is 1 AIQ and complete clairvoyance

			BaseWeapon weapon = m_Body.Weapon as BaseWeapon;

			Mobile opponent = m_Body.Combatant;
			CombatSystemAttachment opponentCSA = CombatSystemAttachment.GetCSA( opponent );
			bool rotating = ( opponentCSA.RotationTimer != null );
			BaseWeapon opponentWeapon = opponent.Weapon as BaseWeapon;
			BaseShield shield = m_Body.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			AttackType theirAttack = AttackType.Invalid;
			double myAttackAnimationDuration = csa.ComputeAnimationDelay();
			DateTime ourAttackWillConnectOn = DateTime.Now + TimeSpan.FromSeconds( myAttackAnimationDuration ); // this would be a new one
			DateTime? currentAttackWillConnectOn = null; // if we have one already
			if ( csa.AttackTimer != null )
				currentAttackWillConnectOn = csa.AttackTimer.FinishTime;
			else if ( csa.RotationTimer != null )
				currentAttackWillConnectOn = csa.RotationTimer.FinishTime;
				
			if ( rotating )
				theirAttack = AttackType.Swing;
			else if ( opponentCSA.AttackTimer != null )
				theirAttack = opponentCSA.AttackTimer.Type;
			bool canParryOpponent = true;
			if ( shield == null )
				if ( (weapon.CannotBlock && m_Body.Body.Type == BodyType.Human) && !opponentWeapon.CannotBlock ) // we cant but they can, thus, we cant
					canParryOpponent = false;
			
			if ( m_Body.Player && !(opponent is BaseCreature) ) // pvm
				AIQ = 0;
				
			bool clairvoyance = Utility.RandomDouble() < AIQ;
			
			if ( beingCharged && m_Parry && ( csa.GetParryDuration() > 0.25 && csa.CanBeginParry() ) && canParryOpponent )
			{ // we're being charged, we must act.
				if ( AttemptChargeParry(clairvoyance) )
					return;
			}
			int opponentRange = opponentWeapon.MaxRange;
			if ( opponent is BaseCreature && ((BaseCreature)opponent).RangeFight > opponentRange )
				opponentRange = ((BaseCreature)opponent).RangeFight;
			int myRange = weapon.MaxRange;
			if ( m_Body is BaseCreature && ((BaseCreature)m_Body).RangeFight > myRange )
				myRange = ((BaseCreature)m_Body).RangeFight;
			if ( ( opponent.Combatant == m_Body && m_Body.InRange( opponent, opponentRange ) && m_Parry ) && canParryOpponent &&
			theirAttack != AttackType.ShieldBash && theirAttack != AttackType.Throw && theirAttack != AttackType.Invalid )
			{ // we are being attacked
				if ( csa.GetParryDuration() > 0.25 && csa.CanBeginParry() ) // let's not cheat too much
				{
					DateTime finishTime = ( rotating ? opponentCSA.RotationTimer.FinishTime : opponentCSA.AttackTimer.FinishTime );
					if ( currentAttackWillConnectOn != null && currentAttackWillConnectOn <= finishTime )
						return; // we shall prevail
						
					if ( ourAttackWillConnectOn >= finishTime ) // otherwise we should attack and interrupt them
					{
						if ( AttemptDefense( clairvoyance ) )
							return;
					}
				}
			}
			
			// we haven't taken any defensive action yet
			if ( m_Body.InRange( opponent, myRange ) && csa.CanBeginAttack() && ( m_Attack || weapon is BaseRanged ) )
			{
				if ( weapon is BaseRanged && !(weapon is Boomerang) )
				{
					if ( AttemptRangedAttack() )
						return;
				}
				else if ( weapon is Boomerang )
				{
					if ( AttemptThrownAttack() )
						return;
				}
				else if ( opponentCSA.DefenseTimer == null ) // if they're defending, we'll be informed when they stop
				{ 
					if ( AttemptMeleeAttack( clairvoyance ) )
						return;
				}
				else // they are defending, but we're going to spice things up for them a bit
				{
					if ( AttemptMeleeAttackDefending() )
						return;
				}
				return;
			}
		}
		
		private class AutoCombatBrainTimer : Timer
		{
			private AutoCombatBrain m_ACBrain;
			
			public AutoCombatBrainTimer( AutoCombatBrain ac ) : base( TimeSpan.MaxValue )
			{
				Priority = TimerPriority.TwoFiftyMS;
				m_ACBrain = ac;
			}
			
			protected override void OnTick()
			{
				if ( m_ACBrain != null && m_ACBrain.Enabled && m_ACBrain.m_Body != null && !m_ACBrain.m_Body.Deleted && CombatSystemAttachment.GetCSA( m_ACBrain.m_Body ).ACBrain == m_ACBrain )
					m_ACBrain.Act();
			}
		}
	}
}
