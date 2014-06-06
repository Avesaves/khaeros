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
using Server.Regions;

namespace Server.Engines.XmlSpawner2 
{
	public enum AttackType
	{
		Invalid = 0,
		Swing = 1,
		Thrust = 2,
		Overhead = 3,
		Throw,
		Circular,
		ShieldBash,
        FrontalAOE,
        FullAOE
	}
	
	public enum DefenseType
	{
		Invalid = 0,
		ParrySwing = 1,
		ParryThrust = 2,
		ParryOverhead = 3,
	}
	
	public enum AttackFlags
	{
		None = 0,
		DisregardDelay = 0x1,
		FlashyFollowup = 0x2
	}
	public class CombatSystemAttachment : XmlAttachment 
	{
		private List<Mobile> m_Aggressors = new List<Mobile>(); // people that are currently targeting me
		private Mobile m_Opponent;	// person that I'm currently targeting
		private AttackTimer m_AttackTimer; // am I swinging right now?
		private DefenseTimer m_DefenseTimer; // am I defending right now?
		private RotationTimer m_RotationTimer; // am I doing something completely awesome?
		private AimingTimer m_AimingTimer;
		private DateTime m_LastAnimAction;
		private DateTime m_NextDefenseAction;
		private DateTime m_NextAttackAction;
		private DateTime m_AimStart;
		private bool m_Aiming;
		private DateTime m_AimConverge;
		private bool m_CruiseControl;
		private AutoCombatBrain m_ACBrain;
		private AnimationTimer m_AnimationTimer;
		private ChargeTimer m_ChargeTimer;
		private DefensiveFormationTimer m_DefensiveFormationTimer;
		private bool m_PerformingSequence;
        private bool m_OffHand;
		private bool m_AlwaysDisplayIcons = false;

        public bool Offhand { get { return m_OffHand; } }
		
		// AC settings
		private bool m_ACParry, m_ACAttack, m_ACParrySwing, m_ACParryThrust, m_ACParryOverhead, 
		m_ACAttackThrust, m_ACAttackOverhead, m_ACAttackSwing, m_ACWaitForAimedShot;
		private bool m_Queue;
		// AC settings end
		
		private AttackType m_LastMeleeAttackType;
		
		private string m_ErrorMessage;
		
		private bool m_Charging;
		private Point3D m_ChargeStart;
		private DateTime? m_ChargeEndTime = null;
		private bool m_DefensiveFormation;
		private bool m_BullRushing;
		private int m_BullRushSteps;
		private Direction m_BullRushDirection;
		private PushTimer m_PushTimer;
		
		private QueuedActionTimer m_QueuedActionTimer;

		public bool AlwaysDisplayIcons { get{ return m_AlwaysDisplayIcons; } set { m_AlwaysDisplayIcons = value; } }
		public DateTime? ChargeEndTime{ get{ return m_ChargeEndTime; } set{ m_ChargeEndTime = value; } }
		public bool CruiseControl { get { return m_CruiseControl; } }
		public List<Mobile> Aggressors { get { return m_Aggressors; } set { m_Aggressors = value; } }
		public DefenseTimer DefenseTimer { get { return m_DefenseTimer; } }
		public PushTimer PushTimer{ get{ return m_PushTimer; } set{ m_PushTimer = value; } }
		public AttackTimer AttackTimer { get { return m_AttackTimer; } set { m_AttackTimer = value; } }
		//public DateTime NextAction { get { return m_NextAction; } set { m_NextAction = value; } }
		public DateTime NextAttackAction { get { return m_NextAttackAction; } set { m_NextAttackAction = value; } }
		public DateTime NextDefenseAction { get { return m_NextDefenseAction; } set { m_NextDefenseAction = value; } }
		public bool Charging{ get{ return m_Charging; } }
		public bool Aiming { get{ return m_Aiming; } }
		public DateTime AimStart{ get{ return m_AimStart; } }
		public AnimationTimer AnimationTimer{ get { return m_AnimationTimer; } set { m_AnimationTimer = value; } }
		public string ErrorMessage{ get{ return m_ErrorMessage; } set{ m_ErrorMessage = value; } }
		public RotationTimer RotationTimer{ get{ return m_RotationTimer; } set { m_RotationTimer = value; } }
		public bool DefensiveFormation{ get{ return m_DefensiveFormation; } }
		public bool PerformingSequence{ get{ return m_PerformingSequence; } set{ m_PerformingSequence = value; } }
		public bool BullRushing{ get{ return m_BullRushing; } set{ m_BullRushing = value; } }
		public QueuedActionTimer QueuedActionTimer{ get{ return m_QueuedActionTimer; } set { m_QueuedActionTimer = value; } }
		public AimingTimer AimingTimer{ get{ return m_AimingTimer; } }
		public DateTime AimConverge{ get{ return m_AimConverge; } }
        public Mobile Opponent{ get { return m_Opponent; } set { m_Opponent = value; } }
		
		// AC settings
		public bool ACParry{ get{ return m_ACParry; } set{ m_ACParry = value; } }
		public bool ACAttack{ get{ return m_ACAttack; } set{ m_ACAttack = value; } }
		public bool ACParrySwing{ get{ return m_ACParrySwing; } set{ m_ACParrySwing = value; } }
		public bool ACParryThrust{ get{ return m_ACParryThrust; } set{ m_ACParryThrust = value; } }
		public bool ACParryOverhead{ get{ return m_ACParryOverhead; } set{ m_ACParryOverhead = value; } }
		public bool ACAttackSwing{ get{ return m_ACAttackSwing; } set{ m_ACAttackSwing = value; } }
		public bool ACAttackThrust{ get{ return m_ACAttackThrust; } set{ m_ACAttackThrust = value; } }
		public bool ACAttackOverhead{ get{ return m_ACAttackOverhead; } set{ m_ACAttackOverhead = value; } }
		public bool ACWaitForAimedShot{ get{ return m_ACWaitForAimedShot; } set{ m_ACWaitForAimedShot = value; } }
		public bool Queue{ get{ return m_Queue; } set{ m_Queue = value; } }
		// AC settings end
		public AutoCombatBrain ACBrain{ get{ return m_ACBrain; } }

		public CombatSystemAttachment ( ASerial serial ) : base( serial ) 
		{ 
		}
		
		public CombatSystemAttachment()
		{
			m_ACParry = m_ACAttack = m_ACParrySwing = m_ACParryThrust = 
			m_ACParryOverhead = m_ACAttackSwing = m_ACAttackThrust = m_ACAttackOverhead = true;
			m_CruiseControl = true; // enabled by default
			m_Queue = false;
			m_ACWaitForAimedShot = false;
			m_AlwaysDisplayIcons = false;
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 5 ); // version
			writer.Write( (bool) m_AlwaysDisplayIcons );
			writer.Write( (bool) m_ACWaitForAimedShot );
			writer.Write( (bool) m_Queue );
			writer.Write( (bool) m_ACParry );
			writer.Write( (bool) m_ACAttack );
			writer.Write( (bool) m_ACParrySwing );
			writer.Write( (bool) m_ACParryThrust );
			writer.Write( (bool) m_ACParryOverhead );
			writer.Write( (bool) m_ACAttackSwing );
			writer.Write( (bool) m_ACAttackThrust );
			writer.Write( (bool) m_ACAttackOverhead );
			
			writer.Write( (bool) m_CruiseControl );
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 5:
				{
					m_AlwaysDisplayIcons = reader.ReadBool();
					goto case 4;
				}
				case 4:
				{
					m_ACWaitForAimedShot = reader.ReadBool();
					goto case 3;
				}
				case 3:
				{
					m_Queue = reader.ReadBool();
					goto case 2;
				}
				case 2:
				{
					m_ACParry = reader.ReadBool();
					m_ACAttack = reader.ReadBool();
					m_ACParrySwing = reader.ReadBool();
					m_ACParryThrust = reader.ReadBool();
					m_ACParryOverhead = reader.ReadBool();
					m_ACAttackSwing = reader.ReadBool();
					m_ACAttackThrust = reader.ReadBool();
					m_ACAttackOverhead = reader.ReadBool();
					goto case 1;
				}
				case 1:
				{
					m_CruiseControl = reader.ReadBool();
					goto case 0;
				}
				case 0:
				{
					break;
				}
			}
		}
		
		public void StartAnimating( int action, int framecount, int repeatcount, bool forward, bool repeat, int delay )
		{
			if ( m_AnimationTimer != null )
			{
				m_AnimationTimer.Cancel( false );
				m_AnimationTimer = null;
			}
			
			m_AnimationTimer = new AnimationTimer( AttachedTo as Mobile, action, framecount, repeatcount, forward, repeat, delay );
			m_AnimationTimer.Start();
			m_AnimationTimer.RefreshAnimation();
		}
		
		public override void OnDelete()
		{
			StopAllActions( false );
			DisableAutoCombat();
		}
		
		public void StopAnimating( bool resetAnim )
		{
			if ( m_AnimationTimer != null )
			{
				m_AnimationTimer.Cancel( resetAnim );
				m_AnimationTimer = null;
			}
			else if ( resetAnim )
				ResetAnimation( AttachedTo as Mobile );
		}

		public override void OnAttach() 
		{
			if ( !(AttachedTo is Mobile) )
				this.Delete();
			
			base.OnAttach();
			
			if ( AttachedTo is BaseCreature )
				EnableAutoCombat();
		}
		
		public void EnableAutoCombat()
		{
			if ( m_ACBrain != null )
				m_ACBrain.Disable();
			m_ACBrain = new AutoCombatBrain( AttachedTo as Mobile, m_ACParry, m_ACAttack, m_ACParrySwing, 
			m_ACParryThrust, m_ACParryOverhead, m_ACAttackSwing, m_ACAttackThrust, m_ACAttackOverhead, m_ACWaitForAimedShot );
			m_CruiseControl = true;
		}
		
		public void DisableAutoCombat()
		{
			if ( m_ACBrain != null )
			{
				m_ACBrain.Disable();
				m_ACBrain = null;
			}
			
			m_CruiseControl = false;
		}
		
		public void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			PlayerMobile player = AttachedTo as PlayerMobile;
			BaseCreature creature = AttachedTo as BaseCreature;
			
			if ( player != null )
				player.CSAAnimate( action, frameCount, repeatCount, forward, repeat, delay );
			else if ( creature != null )
				creature.CSAAnimate( action, frameCount, repeatCount, forward, repeat, delay );
		}
		
		public void UpdateACBrainExternal()
		{
			if ( m_CruiseControl )
			{
				if ( m_ACBrain == null || !m_ACBrain.Enabled )
					EnableAutoCombat();
				m_ACBrain.DelayThink( TimeSpan.FromMilliseconds( 10 )); // this is nearly guaranteed to be called on the next timer slice
			} // direct calling to Act() might cause arbitrary stack flow (e.g. calling AI in the middle of an attack method)
		}
		
		public double GetBestDirectionDamage()
		{
			Mobile mob = AttachedTo as Mobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon == null )
				return 0;
			
			double bestDamage = weapon.ThrustPercentage;
			if ( weapon.OverheadPercentage > bestDamage )
				bestDamage = weapon.OverheadPercentage;
			if ( weapon.SwingPercentage > bestDamage )
				bestDamage = weapon.SwingPercentage;
			if ( weapon.RangedPercentage > bestDamage )
				bestDamage = weapon.RangedPercentage;
			
			return bestDamage;
		}
		
		public double GetDirectionDamage( AttackType atk )
		{
			Mobile mob = AttachedTo as Mobile;
			BaseWeapon wep = mob.Weapon as BaseWeapon;
			if ( wep == null || atk == AttackType.Invalid )
				return 0;
			if ( atk == AttackType.Swing )
				return wep.SwingPercentage;
			else if ( atk == AttackType.Thrust )
				return wep.ThrustPercentage;
			else if ( atk == AttackType.Overhead )
				return wep.OverheadPercentage;
			else if ( atk == AttackType.Throw )
				return ( wep is Boomerang ? wep.RangedPercentage : GetBestDirectionDamage() );
			else if ( atk == AttackType.ShieldBash )
				return 1.0;
			else
				return 0;
		}
		
		public bool DoFlashyAttack()
		{
			Mobile mob = AttachedTo as Mobile;
			AttackType newAttack = AttackType.Invalid;//m_LastMeleeAttackType;
			List<AttackType> possibleAttacks = GetPossibleAttacks();
			foreach ( AttackType atktype in possibleAttacks )
			{
				if ( atktype == m_LastMeleeAttackType )
					continue; // skip same attack as before
				else if ( newAttack == AttackType.Invalid )
					newAttack = atktype;
				else if ( GetDirectionDamage( atktype ) > GetDirectionDamage( newAttack ) )
					newAttack = atktype;
			}
			
			if ( newAttack == AttackType.Invalid )
			{
				mob.SendMessage( "This weapon cannot perform attacks in any other direction!" );
				return false;
			}

			BeginAttack( newAttack, AttackFlags.DisregardDelay|AttackFlags.FlashyFollowup );
			
			return true;
		}
		
		public void CancelSequences()
		{
			m_PerformingSequence = false;
		}
		
		public void GotBullRushed( int xOffset, int yOffset, int timesToMove )
		{
			m_BullRushing = false;
			m_PerformingSequence = true;
			if ( m_PushTimer != null )
				m_PushTimer.Stop();
			m_PushTimer = new PushTimer( AttachedTo as Mobile, TimeSpan.FromSeconds( 0.25 ), timesToMove );
			m_PushTimer.XOffset = xOffset;
			m_PushTimer.YOffset = yOffset;
			m_PushTimer.Start();
		}
		
		public void DoBullRush()
		{
			Mobile mob = AttachedTo as Mobile;
			PlayerMobile pm = mob as PlayerMobile;
			BaseCreature bc = mob as BaseCreature;
			if ( pm == null && bc == null )
				return;
			StopAllActions( false );
			m_BullRushing = true;
			m_BullRushSteps = 0;
			m_BullRushDirection = mob.Direction&Direction.Mask;
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( BullRushTimeout ), this );
		}
		
		public static void BullRushTimeout( object state )
		{
			CombatSystemAttachment csa = state as CombatSystemAttachment;
			csa.BullRushing = false;
		}
		public bool CheckForFreeAttack( Mobile opponent )
		{
			return CheckForFreeAttack( opponent, null, false );
		}
		public bool CheckForFreeAttack( Mobile opponent, Point3D? oldLocation )
		{
			return CheckForFreeAttack( opponent, oldLocation, false );
		}
		public bool CheckForFreeAttack( Mobile opponent, bool assumeTimerRunning )
		{
			return CheckForFreeAttack( opponent, null, assumeTimerRunning );
		}
		public bool CheckForFreeAttack( Mobile opponent, Point3D? oldLocation, bool assumeTimerRunning )
		{
			if ( opponent == null )
				return false;
				
			Mobile mob = AttachedTo as Mobile;
			IWeapon weapon = mob.Weapon;
			PlayerMobile pm = opponent as PlayerMobile;
			if ( weapon is BaseRanged )
				return false;
			if ( m_PerformingSequence ) // tripped, etc people do not get attacks of opportunity on them
				return false;
			if ( !opponent.Alive )
				return false;
			if ( opponent.Frozen ) // no opportunity attacks for frozen mobs
				return false;
			if ( opponent is IKhaerosMobile && ( ((IKhaerosMobile)opponent).StunnedTimer != null || ((IKhaerosMobile)opponent).TrippedTimer != null || opponent.Paralyzed ) )
				return false;
			if ( m_RotationTimer != null ) // we're too busy being awesome
				return false;
			if ( GetCSA( opponent ).RotationTimer != null ) // he's too cool to hit!
				return false;
			if ( m_Charging )
				return false;

            XmlAwe awe = XmlAttach.FindAttachment( mob, typeof( XmlAwe ) ) as XmlAwe;
            if( awe != null )
                return false;

			if ( m_Opponent == opponent && m_AttackTimer != null && ( m_AttackTimer.Running || assumeTimerRunning ) )
			{
				if ( oldLocation != null )
				{
					if ( mob.InRange( oldLocation, 0 ) ) // someone was on top of us and then moved/turned; no AOO.
						return false;
					else if ( mob.InRange( oldLocation, 1 ) && mob.InRange( opponent.Location, 0 ) ) // walked on top of us -> free attack for us
					{
						m_AttackTimer.Force( true ); // AOO
						return true;
					}
				}
				int distance = (int) mob.GetDistanceToSqrt( opponent );
				
				int myRange = weapon.MaxRange;
				if ( AttachedTo is BaseCreature && ((BaseCreature)AttachedTo).RangeFight > myRange )
					myRange = ((BaseCreature)AttachedTo).RangeFight;
					
				if ( distance > myRange || distance == 0 ) // don't consider people too far away or people standing on us
					return false;
					
				int attdir = BaseWeapon.GetDirectionValue(mob.GetDirectionTo( opponent.Location )); // always "assume" WE are turned properly
				int defdir = BaseWeapon.GetDirectionValue(opponent.Direction);
				
				int right = BaseWeapon.FixDirection(defdir - 2);
				int left = BaseWeapon.FixDirection(defdir + 2);
				int backright = BaseWeapon.FixDirection(defdir - 1);
				int backleft = BaseWeapon.FixDirection(defdir + 1);

				if( attdir == defdir || attdir == right || attdir == left || attdir == backright || attdir == backleft )
				{
					// we're attacking from flank, or back/backflank, so we might get a free attack
					if ( pm != null && pm.BackToBack )
					{
						if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) == 1 && attdir == defdir ) // no back attacks at level 1
							return false;
						else if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) == 2 && attdir == defdir || attdir == backright || attdir == backleft ) // no backflank
							return false;
						else if ( pm.Feats.GetFeatLevel(FeatList.BackToBack) >= 3 )
							return false; // no attacks of opportunity at level 3
					}

					m_AttackTimer.Force( true ); // opportunity attack
					return true;
				}
			}
			
			return false;
		}
		
		public void CombatInterrupt( bool resetAnim ) // used by queue specifically, only called upon interruptions due to getting hit
		{
			/*if ( m_AttackTimer != null ) // we were attacking
			{
				if ( m_AttackTimer.Type != AttackType.ShieldBash ) // shieldbash isn't penalized anyway
					m_NextAction = m_NextAction - TimeSpan.FromSeconds( 1.5 ); // the 1.5 second penalty does not apply to interrupts
			}*/
			Interrupted( resetAnim );
			// check if an attack is queued or defense and use appropriate NextAction
			UpdateQueueDelay();
		}
		public void Interrupted( bool resetAnim ) // this primarily concerns instances which the ACBrain deems "interesting"
		{
			StopAllActions( resetAnim );			
			UpdateACBrainExternal();
		}
		
		public static void ResetAnimation( Mobile mob )
		{
			if ( mob == null )
				return;
				
			if ( mob.Body.Type == BodyType.Human )
			{
				if ( mob.Mounted )
					GetCSA( mob ).Animate( 25, 1, 1, false, false, 0 );
				else if ( mob.Warmode )
				{
					if ( ReqsAltWarmodeAnimReset( mob.Weapon as BaseWeapon ) )
						GetCSA( mob ).Animate( 8, 1, 1, true, false, 0 );
					else
						GetCSA( mob ).Animate( 7, 1, 1, true, false, 0 );
				}
				else
					GetCSA( mob ).Animate( 4, 1, 1, true, false, 0 );
			}
			else if ( mob.Body.Type == BodyType.Animal )
			{
				GetCSA( mob ).Animate( 2, 1, 1, true, false, 0 );
			}
			else if ( mob.Body.Type == BodyType.Sea )
			{
				GetCSA( mob ).Animate( 2, 1, 1, true, false, 0 );
			}
			else if ( mob.Body.Type == BodyType.Monster )
			{
				GetCSA( mob ).Animate( 1, 1, 1, true, false, 0 );
			}
		}
		public TimeSpan GetChargeNoRunDelay()
		{
			Mobile mob = AttachedTo as Mobile;
			IKhaerosMobile km = mob as IKhaerosMobile;
			double seconds = 4.0;
			if ( mob.Mounted && km.Feats.GetFeatLevel(FeatList.MountedCombat) > 0 )
			{
				if ( km.Feats.GetFeatLevel(FeatList.MountedCombat) >= 3 )
					seconds -= seconds/2.0;
				else if ( km.Feats.GetFeatLevel(FeatList.MountedCombat) == 2 )
					seconds -= seconds/4.0;
				else if ( km.Feats.GetFeatLevel(FeatList.MountedCombat) == 1 )
					seconds -= seconds/8.0;
			}
			
			return TimeSpan.FromSeconds( seconds );
		}
		public static void ChargeIconRefreshCallback( object state )
		{
			PlayerMobile pm = state as PlayerMobile;
			if ( pm != null )
				pm.CantRunIconRefresh();
		}
		public bool CanRun()
		{
			TimeSpan delay = GetChargeNoRunDelay();
			
			if ( m_ChargeEndTime == null )
				return true;
			else if ( m_ChargeEndTime + delay > DateTime.Now )
				return false;
			else
				return true;
		}
		
		public void StopAllActions( bool resetAnim )
		{
			if ( m_RotationTimer != null )
			{
				m_RotationTimer.Stop();
				m_RotationTimer = null;
			}
			CancelAim( resetAnim );
			CancelCharge();
			CancelDefensiveFormation( resetAnim );
			if ( m_DefenseTimer != null )
			{
				if ( resetAnim )
					m_DefenseTimer.Interrupt();
				else
					m_DefenseTimer.Stop();
				m_DefenseTimer = null;
			}
			
			if ( m_AttackTimer != null )
			{
				if ( resetAnim )
					m_AttackTimer.Interrupt();
				else
					m_AttackTimer.Stop();
				m_AttackTimer = null;
			}
			
			if ( resetAnim && m_AnimationTimer != null )
				m_AnimationTimer.RefreshAnimation();
		}
		
		public void OnUnequippedOrEquipped() // equipping and unequipping stuff in the hands
		{
			CancelQueuedAction();
			if ( m_PerformingSequence )
				Interrupted( true );
			else
				Interrupted( true ); // was false, but why?
				
			CancelFightingStyleBonus( AttachedTo as Mobile );
			CancelBuildupBonus( AttachedTo as Mobile );
			
			//m_NextAction = DateTime.Now + ComputeNextSwingTime() + TimeSpan.FromSeconds( ComputeAnimationDelay() )
			//+ TimeSpan.FromSeconds( 0.5 ); // extra penalty
			m_NextAttackAction = DateTime.Now + ComputeNextSwingTime() + TimeSpan.FromSeconds( ComputeAnimationDelay() ) + TimeSpan.FromSeconds( 0.5 ); // extra penalty
			m_NextDefenseAction = DateTime.Now + TimeSpan.FromSeconds( 1.5 );
		}
		
		public static void AnimationRefreshCallback( object state )
		{
			CombatSystemAttachment csa = state as CombatSystemAttachment;
			if ( csa.AnimationTimer != null )
				csa.AnimationTimer.RefreshAnimation();
		}
		
		public void StopTrip()
		{
			/*Mobile defender = AttachedTo as Mobile;
			StopAnimating( false );
			if ( defender.Body.Type == BodyType.Human )
			{
				Animate( 21, 6, 1, false, false, 0 );
			}
			else if ( defender.Body.Type == BodyType.Animal )
			{
				Animate( 8, 4, 1, false, false, 0 );
			}
			else if ( defender.Body.Type == BodyType.Sea ) // this really, eh.. can't fall down
			{
			}
			else if ( defender.Body.Type == BodyType.Monster )
			{
				Animate( 2, 4, 1, false, false, 0 );
			}
			Timer.DelayCall( TimeSpan.FromSeconds( 0.4 ), new TimerStateCallback( TripGetUpCallback ), this );*/
		}
		
		public void DoTrip( int featLevel )
		{
			Mobile mob = AttachedTo as Mobile;
			IKhaerosMobile km = AttachedTo as IKhaerosMobile;
			if( km.TrippedTimer != null )
				km.TrippedTimer.Stop();
			m_PerformingSequence = true;
			
			km.TrippedTimer = new Unhorse.UnhorseTimer( mob, featLevel );
			km.TrippedTimer.Start();
			
			/*if ( mob.Body.Type == BodyType.Human )
			{
				Animate ( 21, 7, 1, true, false, 0 );
			}
			else if ( mob.Body.Type == BodyType.Animal )
			{
				Animate( 8, 4, 1, true, false, 0 );
			}
			else if ( mob.Body.Type == BodyType.Sea ) // this really, eh.. can't fall down
			{
			}
			else if ( mob.Body.Type == BodyType.Monster )
			{
				Animate( 2, 4, 1, true, false, 0 );
			}
			Timer.DelayCall( TimeSpan.FromSeconds( 0.4 ), new TimerStateCallback( TripLoopCallback ), this );*/
		}
		
		public static void TripGetUpCallback( object state )
		{
			CombatSystemAttachment csa = state as CombatSystemAttachment;
			((IKhaerosMobile)(csa.AttachedTo)).TrippedTimer = null;
			((Mobile)(csa.AttachedTo)).SendMessage( 60, "You have regained your balance." );
			csa.PerformingSequence = false;
		}
		
		public void LoopLyingDownAnimation()
		{
			Mobile mob = AttachedTo as Mobile;
			if ( mob.Body.Type == BodyType.Human )
			{
				StartAnimating( 21, 6, 1, false, false, 255 );
			}
			else if ( mob.Body.Type == BodyType.Animal )
			{
				StartAnimating( 8, 4, 1, false, false, 255 );
			}
			else if ( mob.Body.Type == BodyType.Sea ) // this really, eh.. can't fall down
			{
			}
			else if ( mob.Body.Type == BodyType.Monster )
			{
				StartAnimating( 2, 4, 1, false, false, 255 );
			}
		}
		
		public static void TripLoopCallback( object state )
		{
			CombatSystemAttachment csa = state as CombatSystemAttachment;
			csa.LoopLyingDownAnimation();
		}
		
		public double GetDCI()
		{
			BaseWeapon weapon = ((Mobile)AttachedTo).Weapon as BaseWeapon;
			if ( weapon == null )
				return 0.0;
			return ((double)weapon.GetDefendChanceBonuses( AttachedTo as Mobile ))/100.0;
		}
		
		public double GetHCI()
		{
			BaseWeapon weapon = ((Mobile)AttachedTo).Weapon as BaseWeapon;
			if ( weapon == null )
				return 0.0;
			return ((double)weapon.GetAttackChanceBonuses( AttachedTo as Mobile ))/100.0;
		}
		
		public static bool HasTwoHandedGraphic( BaseWeapon weapon )
		{
			if ( weapon.Layer == Layer.TwoHanded ) // could be a two-hander script-wise only
			{
				if ( weapon is BaseSword ) // there aren't any actual 2h swords.. are there?
					return false;
				return true;
			}
			else
				return false;
		}
		
		public static bool ValidateCharge( Mobile charger, Mobile defender )
		{
			if ( charger == null || charger.Deleted )
				return false;
				
			CombatSystemAttachment chargerCSA = GetCSA( charger );
				
			if ( defender == null || defender.Deleted )
			{
				chargerCSA.ErrorMessage = "Aborting charge (no target)";
				return false;
			}
			
			if ( !chargerCSA.Charging )
			{
				chargerCSA.ErrorMessage = "";
				return false;
			}
			BaseWeapon weapon = charger.Weapon as BaseWeapon;
			if ( weapon == null || weapon.Deleted )
			{
				chargerCSA.ErrorMessage = "";
				return false;
			}
			
			double chargeDistance = chargerCSA.CalculateChargeDistance( defender.Location );
			if ( chargeDistance <= 3 )
			{
				chargerCSA.ErrorMessage = "Charge failed because the distance was too short!";
				return false;
			}

			bool valid = false;
			if ( charger.Mounted ) // mounted charging takes into account weapon position
			{
				Direction direction = (Direction)(((int)charger.Direction)&((int)(Direction.Mask)));
				bool twohander = HasTwoHandedGraphic( weapon );
				// Animations are mirrored, so, e.g. south - north have same sides
				int xDiff = defender.Location.X-charger.Location.X;
				int yDiff = defender.Location.Y-charger.Location.Y;
				int sum = xDiff + yDiff;
				int diff = xDiff - yDiff;

				if ( direction == Direction.North || direction == Direction.South )
				{
					if ( twohander )
					{
						if ( xDiff == 1 )
							valid = true;
					}
					else if ( xDiff == -1 )
						valid = true;
				}
				else if ( direction == Direction.East || direction == Direction.West )
				{
					if ( twohander )
					{
						if ( yDiff == 1 )
							valid = true;
					}
					else if ( yDiff == -1 )
						valid = true;
				}
				else if ( direction == Direction.Up ) // up and down do not mirror
				{
					if ( twohander )
					{
						if ( diff == -1 )
							valid = true;
					}
					else if ( diff == 1 )
						valid = true;
				}
				else if ( direction == Direction.Down ) // up and down do not mirror
				{
					if ( twohander )
					{
						if ( diff == 1 )
							valid = true;
					}
					else if ( diff == -1 )
						valid = true;
				}
				else if ( direction == Direction.Right || direction == Direction.Left )
				{
					if ( twohander )
					{
						if ( sum == 1 )
							valid = true;
					}
					else if ( sum == -1 )
						valid = true;
				}
			}
			else
				valid = chargerCSA.IsOnCollisionCourse( defender ); // foot charging requires the opponent to be in front of us
			if (!valid)
				chargerCSA.ErrorMessage = "Incorrect positioning.";
			return valid;
		}
		
		public bool IsOnCollisionCourse( Mobile with )
		{
			Mobile mob = AttachedTo as Mobile;
			
			int xDiff = with.Location.X-mob.Location.X;
			int yDiff = with.Location.Y-mob.Location.Y;
			int diff = xDiff - yDiff;
			int sum = xDiff + yDiff;
			switch ( mob.Direction&Direction.Mask ) // only those directly in front of us
			{
				case Direction.South:
				{
					if ( mob.Location.Y <= with.Location.Y && mob.Location.X == with.Location.X )
						return true;
					break;
				}
				case Direction.North:
				{
					if ( mob.Location.Y >= with.Location.Y && mob.Location.X == with.Location.X )
						return true;
					break;
				}
				case Direction.West:
				{
					if ( mob.Location.X >= with.Location.X && mob.Location.Y == with.Location.Y )
						return true;
					break;
				}
				case Direction.East:
				{
					if ( mob.Location.X <= with.Location.X && mob.Location.Y == with.Location.Y )
						return true;
					break;
				}
				case Direction.Up:
				{
					if ( sum <= 0 && diff == 0 )
						return true;
					break;
				}
				case Direction.Down:
				{
					if ( sum >= 0 && diff == 0 )
						return true;
					break;
				}
				case Direction.Left:
				{
					if ( sum == 0 )
						return true;
					break;
				}
				case Direction.Right:
				{
					if ( sum == 0 )
						return true;
					break;
				}
			}
			
			return false;
		}
		
		public void OnMoved( Point3D oldLocation ) // I've moved or turned
		{
			m_LastAnimAction = DateTime.Now;
			Mobile mob = AttachedTo as Mobile;
			if ( m_RotationTimer != null )
			{
				if ( oldLocation == mob.Location )
					return; // we can't possibly stop this awesomeness
				else // foul play!
				{
					m_RotationTimer.Stop(); // stop that shit
					m_RotationTimer = null;
				}
			}
			
			CancelQueuedAction();
			
			if ( m_DefensiveFormationTimer != null )
			{
				m_DefensiveFormationTimer.Stop();
				m_DefensiveFormationTimer = null;
			}
			
			if ( m_Charging )
			{
				if ( mob.Hidden )
					mob.RevealingAction();
				if ( ((int)mob.Direction) <= 7 ) // walking in charge cancels it
				{
					mob.SendMessage( "You stopped running and thus fumbled your charge." );
					CancelCharge();
				}
			}
				
			// fumble my attack and defense
			if ( m_AttackTimer != null )
			{
				m_AttackTimer.Stop();
				m_AttackTimer = null;
			}
			if ( m_DefenseTimer != null )
			{
				m_DefenseTimer.Stop();
				m_DefenseTimer = null;
			}
			
			if ( m_DefensiveFormation )
			{
				if ( mob.Location != oldLocation ) // not just turning
					CancelDefensiveFormation( false );	// moved while in defensive formation
				else // this needs a tiny delay, otherwise the animation is sent together with the 'movement' one
					Timer.DelayCall( TimeSpan.FromMilliseconds( 50 ), new TimerStateCallback( AnimationRefreshCallback ), this );
			}
			
			if ( m_Aiming )
			{
				if ( mob.Location != oldLocation ) // not just turning
					CancelAim( false );	// moved while aiming

				else // this needs a tiny delay, otherwise the animation is sent together with the 'movement' one
					Timer.DelayCall( TimeSpan.FromMilliseconds( 50 ), new TimerStateCallback( AnimationRefreshCallback ), this );
			}
			
			// check if i'm showing my back to any of my combatants, if so, check if they're swinging anything, and make it connect
			List<Mobile> opponents = new List<Mobile>( m_Aggressors ); // necessary due to concurrency

			foreach ( Mobile opponent in opponents )
			{
				GetCSA( opponent ).CheckForFreeAttack( mob, oldLocation );
				GetCSA( opponent ).OnAfterOpponentMoved( mob );
			}
			
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
		
			int myRange = weapon.MaxRange;
			if ( AttachedTo is BaseCreature && ((BaseCreature)AttachedTo).RangeFight > myRange )
				myRange = ((BaseCreature)AttachedTo).RangeFight;
		
			if ( m_Charging && m_Opponent != null )
			{
				if ( m_Opponent.InRange( mob, myRange ) ) // we are in range, but charge might not be valid
				{
					if ( ValidateCharge( mob, m_Opponent ) )
					{
						//mob.SendMessage( "Charge Okay." );
						bool weWin = true;
						// we're going to strike them now, but let's see if they are, too
						if ( ValidateCharge( m_Opponent, mob ) )
						{ // seems they have a valid charge
							BaseWeapon opponentWeapon = m_Opponent.Weapon as BaseWeapon;
							int theirRange = opponentWeapon.MaxRange;
							if ( m_Opponent is BaseCreature && ((BaseCreature)m_Opponent).RangeFight > theirRange )
								theirRange = ((BaseCreature)m_Opponent).RangeFight;
							if ( m_Opponent.InRange( mob, theirRange ) )
							{
								// they can charge-strike us as well! lets make it somewhat randomized
								double myRidingSkill = mob.Skills[SkillName.Riding].Value + ((IKhaerosMobile)mob).RideBonus;
								double theirRidingSkill = m_Opponent.Skills[SkillName.Riding].Value + ((IKhaerosMobile)m_Opponent).RideBonus;
								if ( !mob.Mounted )
									myRidingSkill = 0;
								if ( !m_Opponent.Mounted )
									theirRidingSkill = 0;
								double myWeaponSkill = mob.Skills[weapon.Skill].Base;
								double theirWeaponSkill = m_Opponent.Skills[opponentWeapon.Skill].Base;
								double myTacticsSkill = mob.Skills[SkillName.Tactics].Base;
								double theirTacticsSkill = m_Opponent.Skills[SkillName.Tactics].Base;
								double ourSum = myRidingSkill + myWeaponSkill + myTacticsSkill;
								double theirSum = theirRidingSkill + theirWeaponSkill + theirTacticsSkill;
								double chance = ( ourSum / theirSum ) / 2; // fifty-fifty at same skills and backgrounds
								if ( Utility.RandomDouble() < chance )
									weWin = true;
								else
									weWin = false;
							}
						}
						if ( weWin )
						{
							double chargeBonus = CalculateChargeBonus( m_Opponent.Location );
							m_ChargeEndTime = DateTime.Now;
                            SetChargeCooldown(mob);
							ChargeIconRefreshCallback( AttachedTo );
							Timer.DelayCall( GetChargeNoRunDelay(),
								new TimerStateCallback( ChargeIconRefreshCallback ), AttachedTo );
							AttackType chargetype = ChargeAttackType( weapon );
							m_AttackTimer = new AttackTimer( mob, chargetype, TimeSpan.Zero );
							FinishAttack( chargeBonus );
							
							CancelCharge();
						}
						else // they win :(
						{
							CombatSystemAttachment opCSA = GetCSA( m_Opponent );
							double chargeBonus = opCSA.CalculateChargeBonus( mob.Location );
							opCSA.ChargeEndTime = DateTime.Now;
                            SetChargeCooldown(m_Opponent);
							ChargeIconRefreshCallback( m_Opponent );
							Timer.DelayCall( GetChargeNoRunDelay(),
								new TimerStateCallback( ChargeIconRefreshCallback ), m_Opponent );
							AttackType chargetype = ChargeAttackType( m_Opponent.Weapon );
							opCSA.AttackTimer = new AttackTimer( m_Opponent, chargetype, TimeSpan.Zero );
							opCSA.FinishAttack( chargeBonus );
							opCSA.CancelCharge();
						}
					}
					else // this is a showstopper
					{
						if ( m_ErrorMessage != "" )
							mob.SendMessage( m_ErrorMessage );

						CancelCharge();
					}
				}
				else if ( m_Opponent.InRange( mob, myRange+2 ) )
				{
					GetCSA( m_Opponent ).ChargeAlert( ChargeAttackType( weapon ) ); // alerts auto combat
				}
			}
			else if ( m_BullRushing )
			{
				IKhaerosMobile km = mob as IKhaerosMobile;
				if ( (mob.Direction&Direction.Mask) != m_BullRushDirection )
				{
					m_BullRushing = false;
					mob.SendMessage( "Bull Rush interrupted due to not rushing in a straight line." );
				}
				else
				{
					List<Mobile> list = new List<Mobile>();
					int moveXOffset = 0;
					int moveYOffset = 0;
					switch ( mob.Direction&Direction.Mask )
					{
						case Direction.South:
						case Direction.North:
						{
							moveXOffset = 1;
							break;
						}
						case Direction.West:
						case Direction.East:
						{
							moveYOffset = 1;
							break;
						}
						case Direction.Up:
						case Direction.Down:
						{
							moveXOffset = 1;
							moveYOffset = -1;
							break;
						}
						case Direction.Left:
						case Direction.Right:
						{
							moveXOffset = 1;
							moveYOffset = 1;
							break;
						}
					}

					foreach ( Mobile candidate in mob.GetMobilesInRange( myRange ) )
					{
						if ( candidate != null && mob != candidate )
							if ( IsOnCollisionCourse( candidate ) )
								list.Add( candidate );
					}
					
					foreach ( Mobile opponent in list )
					{
						bool randomDir = Utility.RandomBool();
						if ( !BaseAI.AreAllies( mob, opponent ) )
						{ // only attacks non-allies
							mob.Combatant = opponent;
							m_AttackTimer = new AttackTimer( mob, AttackType.Swing, TimeSpan.Zero );
							FinishAttack();
						}
						Point3D newLoc = new Point3D();
						newLoc.X = opponent.Location.X + ( randomDir ? -1 : 1 ) * moveXOffset;
						newLoc.Y = opponent.Location.Y + ( randomDir ? -1 : 1 ) * moveYOffset;
						newLoc.Z = opponent.Location.Z;
						if ( opponent.Map.CanSpawnMobile( newLoc ) )
							opponent.SetLocation( newLoc, true );
						int additionalPush = 0;
						if ( km.Feats.GetFeatLevel(FeatList.BullRush) >= 3 )
							additionalPush = 2;
						else if ( km.Feats.GetFeatLevel(FeatList.BullRush) == 2 )
							additionalPush = 1;
							
						// move x more times
						if ( additionalPush > 0 )
							GetCSA( opponent ).GotBullRushed( ( randomDir ? -1 : 1 ) * moveXOffset, ( randomDir ? -1 : 1 ) * moveYOffset, 2 );
					}
				}
				m_BullRushSteps++;
				if ( m_BullRushSteps > km.Feats.GetFeatLevel(FeatList.BullRush) * 2 )
					CancelSequences();
			}
			
			UpdateACBrainExternal();
		}

        private void SetChargeCooldown(Mobile mob)
        {
            PlayerMobile charger = mob as PlayerMobile;
            if (charger != null)
            {
                charger.LastCharge = DateTime.Now;
                if (charger.Mounted)
                {
                    if (charger.Weapon is Lance)
                        charger.ChargeCooldown = 5;
                    else
                        charger.ChargeCooldown = 10;
                }
                else
                    charger.ChargeCooldown = 5;
            }
        }

		public void ChargeAlert( AttackType type )
		{
			if ( m_CruiseControl )
			{
				if ( m_ACBrain != null && m_ACBrain.Enabled )
					m_ACBrain.ChargeAlert( type );
			}
		}
		
		public static AttackType ChargeAttackType( IWeapon iweapon )
		{
			BaseWeapon weapon = iweapon as BaseWeapon;
			if ( weapon == null )
				return AttackType.Invalid;
            AttackType chargetype = GetHighestAttackType(weapon);
			return chargetype;
		}
		
		private static AttackType GetHighestAttackType(BaseWeapon weapon)
        {
            AttackType result;

            double highestPercentage = weapon.ThrustPercentage;
            result = AttackType.Thrust;

            if (highestPercentage < weapon.SwingPercentage)
            {
                highestPercentage = weapon.SwingPercentage;
                result = AttackType.Swing;
            }
            if (highestPercentage < weapon.OverheadPercentage)
            {
                highestPercentage = weapon.OverheadPercentage;
                result = AttackType.Overhead;
            }

            return result;
        }
		
		public double CalculateChargeBonus( Point3D FinalLoc )
		{
			double damageBonus = 1.0 + CalculateChargeDistance( FinalLoc )/10.0;
			return damageBonus;
		}
		
		public double CalculateChargeDistance( Point3D FinalLoc )
		{
			int xDelta = m_ChargeStart.X - FinalLoc.X;
			int yDelta = m_ChargeStart.Y - FinalLoc.Y;

			return Math.Sqrt( (xDelta * xDelta) + (yDelta * yDelta) );
		}
		
		public void OnAfterOpponentMoved( Mobile opponent )
		{
			Mobile mob = AttachedTo as Mobile;

			if ( opponent == null || mob.Combatant != opponent || !opponent.Alive )
				return;
			CombatSystemAttachment opponentCSA = GetCSA( opponent );
			if ( m_Aiming || m_DefensiveFormation )
			{
				int myRange = ((BaseWeapon)mob.Weapon).MaxRange;
				if ( mob is BaseCreature && ((BaseCreature)mob).RangeFight > myRange )
					myRange = ((BaseCreature)mob).RangeFight;
				if ( opponent.InRange( mob, myRange ) && m_DefensiveFormation )
				{
					if ( opponentCSA.Charging ) // he charged straight into us
					{
						m_AttackTimer = new AttackTimer( mob, AttackType.Thrust, TimeSpan.Zero );
						FinishAttack( opponentCSA.CalculateChargeBonus( mob.Location ), true ); // steal his damage bonus
						CancelDefensiveFormation( true );
					}
					else if ( ((int)opponent.Direction) > 7 ) // running
					{
						m_AttackTimer = new AttackTimer( mob, AttackType.Thrust, TimeSpan.Zero );
						FinishAttack( 1.0, true );
						CancelDefensiveFormation( true );
					}
				}
				
				if ( mob.Direction != mob.GetDirectionTo( mob.Combatant ) && mob.CanSee(mob.Combatant) )
				{
					mob.Direction = mob.GetDirectionTo( mob.Combatant );
					if ( m_AnimationTimer != null )
						m_AnimationTimer.RefreshAnimation();
				}
			}
			else if ( !m_PerformingSequence && DateTime.Now >= (mob.LastMoveTime + TimeSpan.FromSeconds(0.75)) &&
						mob.InRange( mob.Combatant, 3 ) )
			{
				if ( mob.Direction != mob.GetDirectionTo( mob.Combatant ) && mob.CanSee(mob.Combatant) )
				{
					mob.Direction = mob.GetDirectionTo( mob.Combatant );
				}
			}
					
			UpdateACBrainExternal();
		}
		
		public void OnExternalAnimation()
		{
			Interrupted( false );
		}
		
		public void OnEnteredWarMode()
		{
			if ( m_CruiseControl )
			{
				if ( m_ACBrain == null || !m_ACBrain.Enabled )
					EnableAutoCombat(); // reenable
			}
		}
		
		public void OnLeftWarMode()
		{
			Mobile mob = AttachedTo as Mobile;

			if ( m_ACBrain != null )
			{
				m_ACBrain.Disable();
				m_ACBrain = null;
			}
			
			CancelQueuedAction();
			StopAllActions( true );
		}
		
		public void OnDeath()
		{
			if ( m_ACBrain != null )
			{
				m_ACBrain.Disable();
				m_ACBrain = null;
			}
			CancelQueuedAction();
			StopAllActions( false );
			CancelSequences();
		}
		
		public void OnChangedCombatant()
		{
			Mobile mob = AttachedTo as Mobile;
			CombatSystemAttachment csa;

			if ( mob.Combatant == null && mob.Warmode ) // if we left warmode, let OnLeftWarMode handle it
			{
				CancelQueuedAction();
				StopAllActions( true );
			}

			if ( m_Opponent != mob.Combatant )
			{
				if ( m_Aiming )	// switched targets, so nullify all aiming bonuses we had
				{
					m_AimStart = DateTime.Now;
					BaseRanged rangedweapon = mob.Weapon as BaseRanged;
					IKhaerosMobile km = mob as IKhaerosMobile;
					if ( rangedweapon != null && rangedweapon.AmmoType == typeof( Bolt ) ) // crossbows
					{
						if ( CanUseAimedShot() )
						{
							m_AimConverge = DateTime.Now + TimeSpan.FromSeconds( AimedShotConvergenceSpeed() );
							Timer.DelayCall( TimeSpan.FromSeconds( AimedShotConvergenceSpeed() ),
							new TimerStateCallback( AimedShotCueCallback ), this );
						}
					}
				}

				if ( m_Opponent != null ) // we were fighting some guy before, let's remove ourselves from his aggressor list
				{
					csa = GetCSA( m_Opponent );
					csa.Aggressors.Remove( mob );
				}
				
				if ( mob.Combatant != null )
				{
					csa = GetCSA( mob.Combatant );
					csa.Aggressors.Add( mob );
				}
				
				m_Opponent = mob.Combatant;
				
				if ( ( m_Aiming || m_DefensiveFormation ) && mob.Combatant != null )
				{
					if ( mob.Direction != mob.GetDirectionTo( mob.Combatant ) && mob.CanSee(mob.Combatant) )
					{
						mob.Direction = mob.GetDirectionTo( mob.Combatant );
						if ( m_AnimationTimer != null )
							m_AnimationTimer.RefreshAnimation();
					}
				}
			}
			
			UpdateACBrainExternal();
		}
		
		public double GetChargeTimeWindow()
		{
			Mobile mob = AttachedTo as Mobile;
			IKhaerosMobile km = mob as IKhaerosMobile;
			double duration = 0;
			if ( mob.Mounted )
			{
				double skill = mob.Skills[SkillName.Riding].Base;
						
				duration = ((skill)/100.0)*3.0;
				if ( km != null )
				{
					duration += km.Feats.GetFeatLevel(FeatList.MountedCharge)*0.5;
				}
			}
			else
				duration = 3.0;
				
			return duration;
		}
		
		public bool CanBeginDefensiveFormation() 
		{
			Mobile mob = AttachedTo as Mobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			
			if ( weapon == null )
				return false;
				
			if ( m_PerformingSequence )
			{
				m_ErrorMessage = "You cannot do that right now.";
				return false;
			}
			else if ( mob.Body.Type != BodyType.Human )
			{
				m_ErrorMessage = "You cannot do that with a non-human body.";
			}
			else if ( mob.Mounted )
			{
				m_ErrorMessage = "You must be on foot.";
				return false;
			}
			else if ( !mob.Warmode )
			{
				m_ErrorMessage = "You must enter war mode first.";
				return false;
			}
			else if ( !mob.Alive )
			{
				m_ErrorMessage = "You cannot fight in this state";
				return false;
			}
			else if ( !weapon.CanUseDefensiveFormation )
			{
				m_ErrorMessage = "This weapon cannot be used in defensive formation.";
				return false;
			}
			else if ( m_DefensiveFormation )
			{
				m_ErrorMessage = "You are already in defensive formation.";
				return false;
			}
			else if ( m_DefensiveFormationTimer != null )
			{
				m_ErrorMessage = "You are already entering defensive formation.";
				return false;
			}
				
			return true;
		}
		
		public void CancelDefensiveFormation( bool resetAnim )
		{
			if ( m_DefensiveFormationTimer != null )
			{
				m_DefensiveFormationTimer.Stop();
				m_DefensiveFormationTimer = null;
			}
			if ( m_DefensiveFormation )
			{
				m_DefensiveFormation = false;
				StopAnimating( resetAnim );
			}
		}
		
		public bool BeginDefensiveFormation()
		{
			Mobile defender = AttachedTo as Mobile;
			if ( !CanBeginDefensiveFormation() )
				return false;
				
			defender.RevealingAction();
			StopAllActions( false );
			m_DefensiveFormation = false;
			m_DefensiveFormationTimer = new DefensiveFormationTimer( defender, TimeSpan.FromSeconds( 1.0 ) );
			m_DefensiveFormationTimer.Start();
			Animate( 19, 1, 1, true, false, 2 );
			return true;
		}
		
		public bool EnterDefensiveFormation()
		{
			Mobile defender = AttachedTo as Mobile;
			
            if(m_DefensiveFormationTimer != null)
			    m_DefensiveFormationTimer.Stop();
			m_DefensiveFormationTimer = null;
			
			m_DefensiveFormation = true;
			StartAnimating( 19, 3, 1, false, false, 255 );
			return true;
		}
		
		public void CancelCharge()
		{
			m_Charging = false;
			if ( m_ChargeTimer != null )
			{
				m_ChargeTimer.Stop();
			}
		}
		
		public bool CanBeginCharging()
		{
            Mobile mob = AttachedTo as Mobile;
            PlayerMobile charger = mob as PlayerMobile;
            BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon == null )
				return false;

            if (charger != null)
            {
                if (!charger.IsChargeCooldownOver())
                {
                    m_ErrorMessage = "You cannot charge again so quick.";
                    return false;
                }
            }

			if ( m_PerformingSequence || m_BullRushing )
			{
				m_ErrorMessage = "You cannot do that right now.";
				return false;
			}
			else if ( mob.Weapon is BaseRanged )
			{
				m_ErrorMessage = "Only melee weapons can be used for charging.";
				return false;
			}
			else if ( mob.Mounted && weapon.CannotUseOnMount )
			{
				m_ErrorMessage = "That weapon cannot be used while mounted.";
				return false;
			}
			else if ( !mob.Mounted && weapon.CannotUseOnFoot )
			{
				m_ErrorMessage = "That weapon can only be used while mounted.";
				return false;
			}
			else if ( !mob.Warmode )
			{
				m_ErrorMessage = "You must enter war mode first.";
				return false;
			}
			else if ( !mob.Alive )
			{
				m_ErrorMessage = "You cannot fight in this state";
				return false;
			}
			
			return true;
		}
		
		public bool BeginCharging()
		{
			Mobile charger = AttachedTo as Mobile;
			Mobile attacker = charger;
			if ( !CanBeginCharging() )
				return false;
			StopAllActions( true );
			
			m_ChargeStart = new Point3D( charger.Location.X, charger.Location.Y, charger.Location.Z );
			m_Charging = true;
			m_ChargeTimer = new ChargeTimer( charger, TimeSpan.FromSeconds( GetChargeTimeWindow() ) );
			m_ChargeTimer.Start();
			AttackType type = ChargeAttackType( charger.Weapon );
			int zMod = 0;
			if ( attacker is BaseCreature )
				zMod = ((BaseCreature)attacker).Height;
			switch ( (int)type )
			{
				case (int)AttackType.Swing:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 15290, 3, 0, true, false, 3, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					break;
				}
				
				case (int)AttackType.Thrust:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 15290, 3, 0, true, false, 62, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					break;
				}
				
				case (int)AttackType.Overhead:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 15290, 3, 0, true, false, 37, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					break;
				}
			}
			
			return true;
		}

		public bool CanBeginCombatAction( )
		{
			Mobile mob = AttachedTo as Mobile;

            if (mob.Region is SanctuaryRegion)
            {
                return false;
            }

			if ( ( mob.Combatant != null ) && ( !(mob.Combatant.Alive) || mob.Combatant.IsDeadBondedPet || mob.Combatant.Deleted ) )
				mob.Combatant = null;

            XmlAwe awe = XmlAttach.FindAttachment( mob, typeof( XmlAwe ) ) as XmlAwe;

            if( awe != null )
                return false;

			if ( mob is PlayerMobile && ((PlayerMobile)mob).RessSick && mob.Combatant is PlayerMobile)
			{
				m_ErrorMessage = "You must wait an additional " + ((PlayerMobile)mob).m_KOPenalty.Next + " seconds due to the knockout penalty before you can attack.";
				return false;
			}
			else if ( mob.Frozen )
			{
				m_ErrorMessage = "You are frozen and cannot perform any actions.";
				return false;
			}
			else if ( ((IKhaerosMobile)mob).StunnedTimer != null || mob.Paralyzed )
			{
				m_ErrorMessage = "You are stunned and cannot perform any actions.";
				return false;
			}
			else if ( !mob.Alive || mob.IsDeadBondedPet || mob.Deleted )
			{
				m_ErrorMessage = "You cannot fight in this state.";
				return false;
			}
			else if ( !mob.Warmode )
			{
				m_ErrorMessage = "You must enter war mode first.";
				return false;
			}
			
			return true;
		}
		
		public double GetParryDuration()
		{
			Mobile defender = AttachedTo as Mobile;
			if ( defender is BaseCreature )
			{
				if (((BaseCreature)defender).ParryDisabled )
					return 0.0;
				else
					return Math.Max( 1.0, 1.0*((defender.Skills[SkillName.Parry].Base)/100.0) ); // all npcs get 1 second parry
			}
			
			return 1.0*((defender.Skills[SkillName.Parry].Base)/100.0);
		}
		public bool BeginDefense( DefenseType defensetype )
		{
			return BeginDefense( defensetype, false );
		}
		public bool BeginDefense( DefenseType defensetype, bool playerInitiated )
		{
			Mobile defender = AttachedTo as Mobile;
			if ( !CanBeginCombatAction() || !CanBeginParry() )
				return false;
				
			if ( m_Opponent != null && !m_PerformingSequence )
				SpellHelper.Turn( defender, m_Opponent );
				
			StopAllActions( false );
				
			BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			BaseWeapon twohander = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
			BaseWeapon onehander = defender.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon;
			
			if ( ((IKhaerosMobile)defender).TrippedTimer != null ) // always same animation if tripped
			{
				StopAnimating( false );
				if ( defender.Body.Type == BodyType.Human )
				{
					if ( twohander != null )
						Animate( 21, 4, 1, false, false, 255 );
					else
						Animate( 21, 5, 1, false, false, 255 );
				}
			}
			int zMod = 0;
			if ( defender is BaseCreature )
				zMod = ((BaseCreature)defender).Height;
			switch ( (int)defensetype )
			{
				case (int)DefenseType.ParrySwing:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + zMod ), defender.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + 25 + zMod*2 ), defender.Map );
					SendCombatAlerts( from, to, 7035, 3, 0, true, false, 3, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					if ( ((IKhaerosMobile)defender).TrippedTimer == null )
					{
						if ( defender.Body.Type == BodyType.Human )
						{
							if ( defender.Mounted )
							{
								if ( shield != null || twohander != null )
									Animate( 27, 4, 1, false, false, 255 );
								else
									Animate( 28, 2, 1, false, false, 255 );
							}
							else
								Animate( 14, 2, 1, false, false, 255 );
						}
						else
						{
							int[] anim = BAData.GetAnimation( defender, DefenseType.ParrySwing );
							if ( anim != null )
								Animate( anim[0], anim[1], 1, false, false, 255 );
						}
					}
					break;
				}
				
				case (int)DefenseType.ParryThrust:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + zMod ), defender.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + 25 + zMod*2 ), defender.Map );
					SendCombatAlerts( from, to, 7035, 3, 0, true, false, 62, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					if ( ((IKhaerosMobile)defender).TrippedTimer == null )
					{
						if ( defender.Body.Type == BodyType.Human )
						{
							if ( defender.Mounted )
								if ( twohander != null )
									Animate( 28, 8, 1, false, false, 255 );
								else
									Animate( 27, 2, 1, false, false, 255 );
							else
								Animate( 16, 3, 1, false, false, 255 );
						}
						else
						{
							int[] anim = BAData.GetAnimation( defender, DefenseType.ParryThrust );
							if ( anim != null )
								Animate( anim[0], anim[1], 1, false, false, 255 );
						}
					}
					break;
				}
				
				case (int)DefenseType.ParryOverhead:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + zMod ), defender.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + 25 + zMod*2 ), defender.Map );
					SendCombatAlerts( from, to, 7035, 3, 0, true, false, 37, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					if ( ((IKhaerosMobile)defender).TrippedTimer == null )
					{
						if ( defender.Body.Type == BodyType.Human )
						{
							if ( defender.Mounted )
							{
								if ( shield != null || onehander != null )
									Animate( 27, 3, 1, false, false, 255 );
								else
									Animate( 27, 2, 1, false, false, 255 );
									
							}
							else
							{
								if ( shield != null )
									Animate( 12, 2, 1, false, false, 255 );
								else
									Animate( 17, 1, 1, false, false, 255 );
							}
						}
						else
						{
							int[] anim = BAData.GetAnimation( defender, DefenseType.ParryOverhead );
							if ( anim != null )
								Animate( anim[0], anim[1], 1, false, false, 255 );
						}
					}
					break;
				}
			}
			
			TimeSpan delay = TimeSpan.FromSeconds( 1.5 );
			IKhaerosMobile km = defender as IKhaerosMobile;
			BaseWeapon weapon = defender.Weapon as BaseWeapon;
			if ( defender.FindItemOnLayer( Layer.TwoHanded ) is BaseShield )
			{
				// we parried with a shield
				delay -= TimeSpan.FromSeconds( 0.5 ); // parrying with shield takes 0.5s less
			}
			else if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) > 0 && defender is PlayerMobile ) // weapon parrying feat
			{
				PlayerMobile pm = defender as PlayerMobile;
				if ( weapon.NameType == pm.WeaponSpecialization || weapon.NameType == pm.SecondSpecialization )
				{
					if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) == 1 )
						delay -= TimeSpan.FromSeconds( 0.125 );
					else if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) == 2 )
						delay -= TimeSpan.FromSeconds( 0.25 );
					else if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) == 3 )
						delay -= TimeSpan.FromSeconds( 0.5 );
				}
			}
			
			m_NextDefenseAction = DateTime.Now + delay;
			
			m_DefenseTimer = new DefenseTimer( defender, defensetype, TimeSpan.FromSeconds( GetParryDuration() ) );
			m_DefenseTimer.Start();
			if ( BandageContext.GetContext( defender ) != null )
			{
				BandageContext.GetContext( defender ).StopHeal();
				if ( defender is IKhaerosMobile )
				{
					if ( ((IKhaerosMobile)defender).HealingTimer != null )
					{
						((IKhaerosMobile)defender).HealingTimer.Stop();
						((IKhaerosMobile)defender).HealingTimer = null;
					}
				}
			}
			return true;
		}
		
		public bool CanThrustOnMount()
		{
			Mobile attacker = AttachedTo as Mobile;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			
			if ( weapon == null )
				return false;
			
			if ( weapon.CanThrustOnMount )
				return true;
			else
			{
				m_ErrorMessage = "That weapon cannot perform a thrust attack while mounted.";
				return false;
			}
		}
		
		public bool CanThrow()
		{
			Mobile attacker = AttachedTo as Mobile;
			IKhaerosMobile km = attacker as IKhaerosMobile;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if( m_Opponent == null || attacker == null || weapon == null )
				return false;
			
			int myRange = weapon.MaxRange;
			if ( attacker is BaseCreature && ((BaseCreature)attacker).RangeFight > myRange )
				myRange = ((BaseCreature)attacker).RangeFight;

            if( m_OffHand && !Commands.OffHandThrow.CheckForFreeHand(attacker) )
			{
				m_ErrorMessage = "You need a free hand in order to do that.";
				return false;
			}
					
			if ( m_Opponent.InRange( attacker, 1 ) && !(weapon is Boomerang) )
			{
				m_ErrorMessage = "You are too close.";
				return false;
			}
            else if( ( weapon is Fists || ( weapon is BaseRanged && !( weapon is Boomerang ) ) || weapon is Lance ) && !m_OffHand )
			{
				m_ErrorMessage = "That weapon cannot be thrown.";
				return false;
			}
			else if ( !weapon.Throwable && km.Feats.GetFeatLevel(FeatList.ThrowingMastery) <= 0 && !(weapon is Boomerang) )
			{
				m_ErrorMessage = "That weapon cannot be thrown.";
				return false;
			}
			else if ( m_Opponent == null )
			{
				m_ErrorMessage = "You must be in combat with someone.";
				return false;
			}
			else if ( !m_Opponent.InRange( attacker, 3 + km.Feats.GetFeatLevel(FeatList.ThrowingMastery) ) && !(weapon is Boomerang) )
			{
				m_ErrorMessage = "You are out of range.";
				return false;
			}
			else if ( m_Opponent.InRange( attacker, 1 ) && !(weapon is Boomerang) )
			{
				m_ErrorMessage = "You are too close.";
				return false;
			}
			else if ( weapon is Boomerang && !m_Opponent.InRange( attacker, myRange ) )
			{
				m_ErrorMessage = "You are out of range.";
				return false;
			}
				
			return true;
		}
		
		
		public bool BeginAttack( AttackType attacktype, AttackFlags flags )
		{
			return BeginAttack( attacktype, flags, false );
		}
		
		public bool BeginAttack( AttackType attacktype, bool playerInitiated )
		{
			return BeginAttack( attacktype, AttackFlags.None, true );
		}
		
		public bool BeginAttack( AttackType attacktype )
		{
			return BeginAttack( attacktype, AttackFlags.None, false );
		}
		
		public double ComputeAnimationDelay()
		{
			Mobile attacker = AttachedTo as Mobile;
			if ( attacker == null )
				return 0.0;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			if ( weapon == null )
				return 0.0;
			
			double delay = 4.0 - (3.0*((attacker.Skills[weapon.Skill].Base)/100.0));
			if ( attacker is BaseCreature && ((BaseCreature)attacker).ControlMaster == null )
				delay = 1.0;

			if ( weapon is Fists && ((IKhaerosMobile)attacker).CanUseMartialStance )
				delay -= Math.Min( 0.6, BuildupBonuses( attacker ) );
			
			return delay;
		}
		
		public bool BeginAttack( AttackType attacktype, AttackFlags flags, bool playerInitiated ) // melee attacks and throwing only
		{
			if ( attacktype == AttackType.Invalid )
				return false;

			bool flashyFollowup = ( (flags&AttackFlags.FlashyFollowup) != AttackFlags.None );
			bool disregardDelay = ( (flags&AttackFlags.DisregardDelay) != AttackFlags.None );
			bool feint = false;
			bool flashy = false;
			Mobile attacker = AttachedTo as Mobile;
			IKhaerosMobile km = attacker as IKhaerosMobile;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			BaseShield shield = attacker.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;

            if( attacker is PlayerMobile )
            {
                PlayerMobile m = attacker as PlayerMobile;

                if( !m.EnableOffHand )
                    m.OffHandWeapon = null;
                else
                {
                    m.EnableOffHand = false;

                    if( !Commands.OffHandThrow.EnableOffHand( m ) )
                        return false;
                }
            }

            m_OffHand = ( attacker is PlayerMobile && ( (PlayerMobile)attacker ).OffHandThrowing ) && attacktype == AttackType.Throw;

			if ( !CanBeginCombatAction( ) || !CanBeginAttack( flags ) )
				return false;
				
			if ( attacktype == AttackType.Throw )
			{
                if( m_OffHand )
					weapon = ((PlayerMobile)attacker).OffHandWeapon;
				
				if ( !CanThrow() )
					return false;
				if ( weapon is Boomerang ) // requires still time
				{
					if ( !((BaseRanged)weapon).IsStill( attacker ) )
					{
						if ( playerInitiated )
						{
							m_ErrorMessage = "";
							DisplayQueueResultMessage( QueueRanged() );
						}
						
						return false;
					}
				}
			}
			else if ( !CanBeginMeleeAttack() )
				return false;
			else if ( attacktype == AttackType.ShieldBash && shield == null )
			{
				m_ErrorMessage = "A shield is needed in order to perform a shield bash!";
				return false;
			}
				
			if ( attacker.Mounted )
			{
				if ( attacktype == AttackType.Swing || attacktype == AttackType.Circular )
				{
					m_ErrorMessage = "Cannot perform swing attacks while mounted.";
					return false;
				}
				else if ( attacktype == AttackType.Thrust && !(CanThrustOnMount()) )
					return false;
				else if ( attacktype == AttackType.ShieldBash )
				{
					m_ErrorMessage = "This attack cannot be performed while mounted.";
					return false;
				}
			}
			
			StopAllActions( false );
			
			if ( attacktype == AttackType.Circular ) // it is important that all of these are part of the same if/elseif chain
			{ // as some feats are actually additional attacks, but that does not mean they can be used together with maneuvers.
				PlayerMobile pm = attacker as PlayerMobile;
				if ( km.Feats.GetFeatLevel(FeatList.CircularAttack) <= 0 )
				{
					m_ErrorMessage = "You do not have the required feat.";
					return false;
				}
				else if ( pm == null || weapon.Layer != Layer.TwoHanded || weapon is BaseRanged )
				{
					m_ErrorMessage = "You can only perform this attack with two-handed melee weapons.";
					return false;
				}
				else if ( weapon.NameType != pm.WeaponSpecialization && weapon.NameType != pm.SecondSpecialization )
				{
					m_ErrorMessage = "You can only perform this attack with a weapon you have specialized in.";
					return false;
				}
				else if ( !BaseWeapon.CheckStam( attacker, km.Feats.GetFeatLevel(FeatList.CircularAttack), false, false ) )
				{
					m_ErrorMessage = ""; // will be displayed by the checkstam thingy
					return false;
				}
			}
			else if ( attacktype == AttackType.ShieldBash )
			{
				if ( km.Feats.GetFeatLevel(FeatList.ShieldBash) <= 0 )
				{
					m_ErrorMessage = "You do not have the required feat.";
					return false;
				}
				else if ( !BaseWeapon.CheckStam( attacker, km.Feats.GetFeatLevel(FeatList.ShieldBash), false, false ) )
				{
					m_ErrorMessage = ""; // will be displayed by the checkstam thingy
					return false;
				}

				km.CombatManeuver = new ShieldBash( km.Feats.GetFeatLevel(FeatList.ShieldBash) );
				km.OffensiveFeat = FeatList.ShieldBash;
				
			}
			else if ( km.OffensiveFeat == FeatList.Feint )
			{
				if ( BaseWeapon.CheckStam( attacker, km.Feats.GetFeatLevel(FeatList.Feint), false, false ) )
					feint = true;
					
				weapon.EndManeuver( attacker );
			}
			else if ( km.OffensiveFeat == FeatList.FlashyAttack )
			{
				if ( BaseWeapon.CheckStam( attacker, km.Feats.GetFeatLevel(FeatList.FlashyAttack), false, false ) )
					flashy = true;
					
				weapon.EndManeuver( attacker );
			}
			
			// standard delay is 1 second
			double delay = ComputeAnimationDelay();
			if ( attacktype == AttackType.ShieldBash )
				delay = 4.0 - (3.0*((attacker.Skills[SkillName.Parry].Base)/100.0));
			if ( feint )
				delay *= 2.5-(((double)km.Feats.GetFeatLevel(FeatList.Feint))*0.5); // lvl 1 -> 2x slower, lvl 2 -> 1.5x slower, lvl 3 -> same as normal
				
			if ( attacktype == AttackType.Throw )
			{
				if ( !(weapon is Boomerang) ) // otherwise the duration is already calculated
					delay = 0.5;
			}
			int animspeed = (int)(delay*2);
			IKhaerosMobile kmob = attacker as IKhaerosMobile;
			AttackTimer newTimer = null;
			if ( attacktype != AttackType.Circular )
			{
				newTimer = new AttackTimer( attacker, attacktype, TimeSpan.FromSeconds( delay ) );
				newTimer.Flashy = flashy;
				newTimer.Feint = feint;
				newTimer.FlashyFollowup = flashyFollowup;
			}
			
			if ( attacktype == AttackType.Throw ) // throws cannot have a rotationtimer anyway
			{
				SpellHelper.Turn( attacker, m_Opponent );
				m_AttackTimer = newTimer;
			}
			
			if ( attacktype != AttackType.Circular ) // not if we're spinning
			{
				m_AttackTimer = newTimer;
				if ( m_Opponent != null )
				{ // checking of AoO is necessary so we don't force Turn, which causes jerky movement
					m_NextAttackAction = DateTime.Now + ComputeNextSwingTime() + TimeSpan.FromSeconds( delay );
					if ( !CheckForFreeAttack( m_Opponent, true ) ) // attack of opportunity
					{
						m_AttackTimer = null;
						SpellHelper.Turn( attacker, m_Opponent );
						m_AttackTimer = newTimer;
					}
					else // we DID get an AoO, so we're done here
					{
						if ( BandageContext.GetContext( attacker ) != null )
						{
							BandageContext.GetContext( attacker ).StopHeal();
							if ( attacker is IKhaerosMobile )
							{
								if ( ((IKhaerosMobile)attacker).HealingTimer != null )
								{
									((IKhaerosMobile)attacker).HealingTimer.Stop();
									((IKhaerosMobile)attacker).HealingTimer = null;
								}
							}
						}
						return true;
					}
				}
			}
			int zMod = 0;
			if ( attacker is BaseCreature )
				zMod = ((BaseCreature)attacker).Height;
			if ( !feint )
			{
				if ( attacktype == AttackType.ShieldBash )
					m_NextAttackAction = DateTime.Now + TimeSpan.FromSeconds( 1.0 );
				else
					m_NextAttackAction = DateTime.Now + ComputeNextSwingTime() + TimeSpan.FromSeconds( delay );
			}
			
			switch ( (int)attacktype )
			{
				case (int)AttackType.ShieldBash:
				{
					if ( attacker.Body.Type == BodyType.Human )
						Animate( 30, 7, 1, true, false, animspeed );
					else
					{
						int[] anim = BAData.GetAnimation( attacker, AttackType.ShieldBash );
						if ( anim != null )
							Animate( anim[0], anim[1], 1, true, false, animspeed );
					}
					break;
				}
				case (int)AttackType.Throw:
				{
					if ( attacker.Body.Type == BodyType.Human )
					{
						if ( !attacker.Mounted )
						{
                            if( weapon.Layer == Layer.OneHanded || m_OffHand )
								Animate( 11, 7, 1, true, false, animspeed );
							else
								Animate( 12, 7, 1, true, false, animspeed );
						}
						else
						{
                            if( weapon.Layer == Layer.OneHanded || m_OffHand )
								Animate( 26, 8, 1, true, false, animspeed );
							else
								Animate( 29, 7, 1, true, false, animspeed );
						}
					}
					else
					{
						int[] anim = BAData.GetAnimation( attacker, AttackType.Throw );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Thrust );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Overhead );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Swing );
						if ( anim != null )
							Animate( anim[0], anim[1], 1, true, false, animspeed );
					}
					break;
				}
				case (int)AttackType.Circular: goto case (int)AttackType.Swing;
				case (int)AttackType.Swing:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 9556, 3, 0, true, false, 3, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					if ( attacker.Body.Type == BodyType.Human )
					{
						if ( weapon is Fists )
						{
							if ( kmob.Stance is VenomousWay )
								Animate( 9, 7, 1, true, false, animspeed ); // karate chop
							else
								Animate( 31, 7, 1, true, false, animspeed ); // regular wrestling
						}
						else
						{
							if ( weapon.Layer == Layer.TwoHanded )
								Animate( 13, 7, 1, true, false, animspeed );
							else
								Animate( 9, 7, 1, true, false, animspeed );
						}
					}
					else
					{
						int[] anim = BAData.GetAnimation( attacker, AttackType.Swing );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Thrust );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Overhead );
						if ( anim != null )
							Animate( anim[0], anim[1], 1, true, false, animspeed );
					}
					break;
				}
				
				case (int)AttackType.Thrust:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 9556, 3, 0, true, false, 62, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					if ( attacker.Body.Type == BodyType.Human )
					{
						if ( attacker.Mounted )
							Animate( 28, 7, 1, false, false, animspeed );
						else
						{
							if ( weapon.Layer == Layer.TwoHanded )
								Animate( 14, 7, 1, true, false, animspeed );
							else
							{
								if ( weapon is Fists && kmob.Stance is SearingBreath )
									Animate( 32, 5, 1, false, false, animspeed+2 ); // headbutt! Nah. People could abuse bow to do this.
								else if ( weapon is Fists && kmob.Stance is TempestuousSea ) // this is fine, though, heartstopper punch!
									Animate( 16, 7, 1, true, false, animspeed );
								else
									Animate( 10, 7, 1, true, false, animspeed );
							}
						}
					}
					else
					{
						int[] anim = BAData.GetAnimation( attacker, AttackType.Thrust );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Overhead );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Swing );
						if ( anim != null )
							Animate( anim[0], anim[1], 1, true, false, animspeed );
					}
					break;
				}
				
				case (int)AttackType.Overhead:
				{
					IEntity from = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + zMod ), attacker.Map );
					IEntity to = new Entity( Server.Serial.Zero, new Point3D( attacker.X, attacker.Y, attacker.Z + 25 + zMod*2 ), attacker.Map );
					SendCombatAlerts( from, to, 9556, 3, 0, true, false, 37, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );
					
					if ( attacker.Body.Type == BodyType.Human )
					{
						if ( attacker.Mounted )
						{
							if ( weapon.Layer == Layer.TwoHanded )
								Animate( 29, 7, 1, true, false, animspeed );
							else
								Animate( 26, 7, 1, true, false, animspeed );
						}
						else
						{
							if ( weapon.Layer == Layer.TwoHanded )
								Animate( 12, 7, 1, true, false, animspeed );
							else
								Animate( 11, 7, 1, true, false, animspeed );
						}
					}
					else
					{
						int[] anim = BAData.GetAnimation( attacker, AttackType.Overhead );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Thrust );
						if ( anim == null )
							anim = BAData.GetAnimation( attacker, AttackType.Swing );
						if ( anim != null )
							Animate( anim[0], anim[1], 1, true, false, animspeed );
					}
					break;
				}
			}
            if (attacktype == AttackType.Circular)
            {
                m_RotationTimer = new RotationTimer(attacker, TimeSpan.FromSeconds(delay));
                m_RotationTimer.Start();
            }
            else
            {
                if (m_AttackTimer == null)
                {
                    attacker.SendMessage("There was an error in the CombatSystemAttachment @ line 2226.");
                    return false;
                }
                else
                    m_AttackTimer.Start();
            }
			
			// this delays as if we were parried, but if we actually won't be, then the delay will shorten
			// what if we get interrupted, though? it should count as a hit on our part, no extra delay.
			if ( BandageContext.GetContext( attacker ) != null )
			{
				BandageContext.GetContext( attacker ).StopHeal();
				if ( attacker is IKhaerosMobile )
				{
					if ( ((IKhaerosMobile)attacker).HealingTimer != null )
					{
						((IKhaerosMobile)attacker).HealingTimer.Stop();
						((IKhaerosMobile)attacker).HealingTimer = null;
					}
				}
			}
			
			if ( m_Opponent != null )
			{
				GetCSA( m_Opponent ).UpdateACBrainExternal();
			}
			
			return true;
		}
		
		public static bool HasMetallicSound( Item item )
		{
			if ( item is BaseWeapon )
			{
				BaseWeapon weapon = item as BaseWeapon;
				return ( weapon.Resource == CraftResource.Copper || weapon.Resource == CraftResource.Bronze ||
				weapon.Resource == CraftResource.Starmetal || weapon.Resource == CraftResource.Steel || weapon.Resource == CraftResource.Iron );
			}
			else if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;
				return ( armor.Resource == CraftResource.Copper || armor.Resource == CraftResource.Bronze ||
				armor.Resource == CraftResource.Starmetal || armor.Resource == CraftResource.Steel || armor.Resource == CraftResource.Iron );
			}
			else
				return false;
		}
		
		public void FinishAttack()
		{
			FinishAttack( 1.0, false );
		}
		
		public void FinishAttack( double damageBonus )
		{
			FinishAttack( damageBonus, false );
		}
		
		public void FinishAttack( double damageBonus, bool cannotParry )
		{
			FinishAttack( damageBonus, cannotParry, false );
		}
		
		public void FinishAttack( double damageBonus, bool cannotParry, bool spinning )
		{
			FinishAttack( damageBonus, cannotParry, spinning, false );
		}
		
		public void FinishSpinAttack()
		{
			if ( m_RotationTimer == null )
				return;
			
			m_RotationTimer.Stop();
			m_RotationTimer = null;
			FinishAttack( 1.0, false, true );
		}
		
		public void FinishAttack( double damageBonus, bool cannotParry, bool spinning, bool opportunity )
		{
			bool flashy = false;
			AttackType type;
            bool AOE = AttachedTo != null && AttachedTo is BaseCreature && ( (BaseCreature)AttachedTo ).AOEAttack;

			if ( !spinning && !AOE )
			{
				if ( m_AttackTimer == null )
					return;	// we weren't swinging anything
				else
				{
					type = m_AttackTimer.Type;
					flashy = m_AttackTimer.Flashy;
					m_AttackTimer.Stop();
				}
			}
			else
				type = AttackType.Swing;
			
			m_LastMeleeAttackType = type;

			Mobile attacker = AttachedTo as Mobile;
			Mobile opponent = m_Opponent;
            bool throwing = (m_OffHand || type == AttackType.Throw);
			
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

            if( attacker != null && attacker is IKhaerosMobile )
            {
                SkillName skill = SkillName.Tactics;

                if( throwing )
                    skill = SkillName.Throwing;

                ( (IKhaerosMobile)attacker ).CombatManeuver.CanUseManeuverCheck( attacker, skill );
            }
			
			double percentageDamage = 0.0;
			double bestDamage = 0.0;
			
			percentageDamage = GetDirectionDamage( type );
			if ( type == AttackType.ShieldBash )
				percentageDamage = 1.0;
			bestDamage = GetBestDirectionDamage();
				
			if ( !m_Charging && !spinning && !m_DefensiveFormation && !opportunity )
				SpellHelper.Turn( attacker, m_Opponent );
			if ( ( spinning || m_Charging || m_DefensiveFormation ) && ((IKhaerosMobile)attacker).OffensiveFeat != FeatList.None )
				((IKhaerosMobile)attacker).DisableManeuver(); // no maneuvers while we're doing these things
			
			List<Mobile> defenders = new List<Mobile>();
			if ( spinning )
			{
				damageBonus *= ((double)((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.CircularAttack))*0.5;
				int myRange = ((BaseWeapon)attacker.Weapon).MaxRange;
				if ( attacker is BaseCreature && ((BaseCreature)attacker).RangeFight > myRange )
					myRange = ((BaseCreature)attacker).RangeFight;
				foreach( Mobile m in attacker.GetMobilesInRange( myRange ) )
				{
					if ( !((IKhaerosMobile)attacker).IsAllyOf( m ) )
						defenders.Add( m );
				}
			}
			else
			{
				defenders.Add( opponent );
				/*if ( opportunity )
				{
					double damageIgnoreFactor = 0.0;
					if ( opponent is PlayerMobile ) // reduce damage a bit
					{
						PlayerMobile opponentPM = opponent as PlayerMobile;
						damageIgnoreFactor += opponentPM.HeavyPieces * 0.0625;
						damageIgnoreFactor += opponentPM.MediumPieces * 0.03125;
					}
					damageBonus *= (1.0 - damageIgnoreFactor);
				}*/
			}

			foreach ( Mobile defender in defenders )
			{
				double attackerDCI, defenderDCI, attackerHCI, defenderHCI;
				attackerDCI = defenderDCI = attackerHCI = defenderHCI = 0.0;
				bool defenderDCIRoll, attackerDCIRoll, attackerHCIRoll, defenderHCIRoll;
				defenderDCIRoll = attackerDCIRoll = attackerHCIRoll = defenderHCIRoll = false;
				CombatSystemAttachment csa = null;
				AttackType newAttack = AttackType.Invalid;
				if ( defender != null )
				{
					csa = GetCSA( defender );
					attackerDCI = GetDCI(); // this is used "offensively", but only for unarmed fighting
					defenderHCI = csa.GetHCI(); // this is used "defensively" but only against unarmed fighting
					if ( defenderHCI < 0 && attackerDCI < 0 ) // both are negative, we won't favor either of them
					{
						double temp = defenderHCI - attackerDCI;
						attackerDCI -= defenderHCI;
						defenderHCI = temp;
					}
					else if ( defenderHCI < 0 ) // attackerDCI is >=0
						attackerDCI -= defenderHCI; // it's negative, so our opponent is adding their negative HCI to our DCI
					else // attackerDCI < 0 and defenderHCI >= 0
						defenderHCI -= attackerDCI;
						
					attackerDCIRoll = ( Utility.RandomDouble() < attackerDCI );
					defenderHCIRoll = ( Utility.RandomDouble() < defenderHCI );
					
					if ( attackerDCIRoll && defenderHCIRoll )
						attackerDCIRoll = defenderHCIRoll = false; // cancel them out
					
					defenderDCI = csa.GetDCI();
					attackerHCI = GetHCI();
					if ( attackerHCI < 0 && defenderDCI < 0 ) // both are negative, we won't favor either of them
					{
						double temp = attackerHCI - defenderDCI;
						defenderDCI -= attackerHCI;
						attackerHCI = temp;
					}
					else if ( attackerHCI < 0 ) // defenderDCI is >=0
						defenderDCI -= attackerHCI; // it's negative, so we're adding our negative HCI to our opponent's DCI
					else // defenderDCI < 0 and attackerHCI >= 0
						attackerHCI -= defenderDCI;
					defenderDCIRoll = ( Utility.RandomDouble() < defenderDCI );
					attackerHCIRoll = ( Utility.RandomDouble() < attackerHCI );
					if ( defenderDCIRoll && attackerHCIRoll ) // cancel them out
						defenderDCIRoll = attackerHCIRoll = false;
					
					if ( m_Charging || spinning || type == AttackType.ShieldBash || m_DefensiveFormation || type == AttackType.Throw )
					{
						attackerHCIRoll = false; // there's only one possible direction when doing these attacks
						attackerDCIRoll = false;
					}
					if ( type == AttackType.ShieldBash || type == AttackType.Throw )
					{
						defenderDCIRoll = false; // this cannot be parried anyway
						defenderHCIRoll = false; // they can't change direction to avoid interruption
					}
					if ( csa.Charging || csa.RotationTimer != null || csa.DefensiveFormation || 
					(csa.AttackTimer != null && (csa.AttackTimer.Type == AttackType.Throw || csa.AttackTimer.Type == AttackType.ShieldBash) ) )
					{
						defenderHCIRoll = false; // they cannot change direction
						attackerDCIRoll = false; // we can't interrupt that, no matter the direction
					}
					
					newAttack = AttackType.Invalid;
					
					if ( attackerHCIRoll )
					{
						foreach ( AttackType atktype in GetPossibleAttacks() )
						{
							if ( atktype == type )
								continue; // skip same attack
							else if ( newAttack == AttackType.Invalid )
								newAttack = atktype;
							else if ( GetDirectionDamage( atktype ) > GetDirectionDamage( newAttack ) )
								newAttack = atktype;
						}
						
						if ( newAttack == AttackType.Invalid )
							attackerHCIRoll = false;
					}
				}
				int myRange = ((BaseWeapon)attacker.Weapon).MaxRange;
				if ( attacker is BaseCreature && ((BaseCreature)attacker).RangeFight > myRange )
					myRange = ((BaseCreature)attacker).RangeFight;
				if ( defender != null && attacker.InRange( defender.Location, myRange ) && defender.Alive && type != AttackType.Throw &&
					attacker.CanSee( defender ) && attacker.InLOS( defender ))
				{ // melee
					bool parried = false;
					if ( csa.DefenseTimer != null && !cannotParry ) // defending
					{
						DefenseType deftype = csa.DefenseTimer.Type;
						BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
						BaseWeapon twohander = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
						BaseWeapon onehander = defender.FindItemOnLayer( Layer.OneHanded ) as BaseWeapon;
                        bool weaponCannotBlock = weapon.CannotBlock;

                        if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
                            weaponCannotBlock = false;
						
						BaseWeapon opponentWeapon = defender.Weapon as BaseWeapon;
                        bool opponentWeaponCannotBlock = opponentWeapon.CannotBlock;

                        if( defender is PlayerMobile && ( (PlayerMobile)defender ).Claws != null )
                            opponentWeaponCannotBlock = false;

						if ( ( !opponentWeaponCannotBlock || defender.Body.Type != BodyType.Human ) || weaponCannotBlock || shield != null )
						{
							if ( ( type == AttackType.Swing && deftype == DefenseType.ParrySwing ) ||	// this handles parries
							( type == AttackType.Thrust && deftype == DefenseType.ParryThrust ) ||
							( type == AttackType.Overhead && deftype == DefenseType.ParryOverhead ) || defenderDCIRoll )
							{
								if ( attackerHCIRoll )
								{
									type = newAttack;
									percentageDamage = GetDirectionDamage( type );
								}
								else
								{
									if ( shield != null )
									{
										int dmg = (int)weapon.GetScaledDamage( attacker, defender, damageBonus );
										shield.OnHit( weapon, dmg );
										PlayBlockSound( attacker, defender, attacker.Weapon as BaseWeapon, shield );
									}
									else if ( twohander != null )
									{
										twohander.DegradeWeapon(); // this should degrade more.
										PlayBlockSound( attacker, defender, attacker.Weapon as BaseWeapon, twohander );
									}
									else
									{
										if ( onehander != null )  // could be fists
											onehander.DegradeWeapon(); // this should degrade more.
											
										PlayBlockSound( attacker, defender, attacker.Weapon as BaseWeapon, defender.Weapon as BaseWeapon );
									}
									
									parried = true;
									if ( !spinning )
									{
										weapon.InitializeManeuver( attacker, null, false );
										weapon.EndManeuver( attacker );
									}
									csa.DefenseTimer.Parries++;
									PlayerMobile pmatk = attacker as PlayerMobile;
									int blut = weapon.AosElementDamages.Blunt;
									if( attacker.Weapon is Fists && attacker is IKhaerosMobile && ((IKhaerosMobile)attacker).TechniqueLevel > 0 )
									{
										int slax = 0;
										int pirc = 0;
										blut = 100;
										if( ((IKhaerosMobile)attacker).Technique == "slashing" )
											slax = ((IKhaerosMobile)attacker).TechniqueLevel;
										else if( ((IKhaerosMobile)attacker).Technique == "piercing" )
											pirc = ((IKhaerosMobile)attacker).TechniqueLevel;
										
										blut -= ((IKhaerosMobile)attacker).TechniqueLevel;

                                        if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
                                            blut = 0;
									}
									
									if ( Utility.RandomDouble() < (((double)(blut))/200.0) )
										weapon.OnSplash( attacker, defender, damageBonus*percentageDamage ); // 20% dmg done in OnHit
									if ( pmatk != null && pmatk.HasSpecializedWeaponSkill() )
									{
										CombatSystemAttachment.FightingStyleOnParry( attacker );
										/*PlayerMobile pmdef = defender as PlayerMobile;
										if ( pmdef != null && pmdef.HasSpecializedWeaponSkill() )
										{
											CombatSystemAttachment.FightingStyleOnHit( defender );
										}*/
									}
									PlayerMobile pmdef = defender as PlayerMobile;
									if ( pmdef != null && pmdef.HasSpecializedWeaponSkill() )
									{
										// This is a positive bonus for the defender, but it's not a hit, it's a parry (confusing :\)
										CombatSystemAttachment.FightingStyleOnHit( defender );
									}
									
									if ( ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.Buildup) > 0 )
										CombatSystemAttachment.BuildupOnParry( attacker );
									if ( !spinning )
									{
										TimeSpan extraPenalty = TimeSpan.Zero;
										IKhaerosMobile km = defender as IKhaerosMobile;
	
										if ( km != null )
										{
											BaseWeapon greatweapon = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseWeapon;
											if( greatweapon != null && !(greatweapon is BaseRanged) )
											{
												if ( km.Feats.GetFeatLevel(FeatList.GreatweaponFighting) == 1 )
													extraPenalty += TimeSpan.FromSeconds( 0.125 );
												else if ( km.Feats.GetFeatLevel(FeatList.GreatweaponFighting) == 2 )
													extraPenalty += TimeSpan.FromSeconds( 0.25 );
												else if ( km.Feats.GetFeatLevel(FeatList.GreatweaponFighting) == 3 )
													extraPenalty += TimeSpan.FromSeconds( 0.5 );
											}
											
											if ( shield != null )
											{
												if ( km.Feats.GetFeatLevel(FeatList.ShieldMastery) == 1 )
													extraPenalty += TimeSpan.FromSeconds( 0.125 );
												else if ( km.Feats.GetFeatLevel(FeatList.ShieldMastery) == 2 )
													extraPenalty += TimeSpan.FromSeconds( 0.25 );
												else if ( km.Feats.GetFeatLevel(FeatList.ShieldMastery) == 3 )
													extraPenalty += TimeSpan.FromSeconds( 0.5 );
											}
										}
										if (opportunity)
											m_NextAttackAction += extraPenalty;
										else
											m_NextAttackAction = DateTime.Now + ComputeNextSwingTime() + extraPenalty; // extra penalty since we were parried
									}
									if ( m_Charging ) // we're charging and the guy parried, this is the mounted momentum feat
									{
										if ( ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.MountedMomentum) > 0 && !defender.Mounted )
										{
											double distance = CalculateChargeDistance( defender.Location ); // this was csa.CalcCh...? Why?
											if ( distance - (19-(((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.MountedMomentum)*3)) >= 0 )
											{ // sufficient distance for feat to activate
												if( !csa.PerformingSequence )
													csa.DoTrip( 1 ); // 2 seconds
											}
										}
									}
								}
							}
						}
					}
					
					if ( !parried )
					{
						if( ((IKhaerosMobile)defender).CanDodge &&  ( !defender.Mounted || Utility.RandomMinMax(1,100) < BaseWeapon.GetRacialMountAbility(defender, typeof(ForestStrider)) ) && !csa.PerformingSequence )
						{
							if( ((IKhaerosMobile)defender).Dodged() )
							{
								parried = true;
								if ( !spinning )
								{
									weapon.InitializeManeuver( attacker, null, false );
									weapon.EndManeuver( attacker );
									((IKhaerosMobile)attacker).DisableManeuver();
									if ( !opportunity )
									{
										if ( type == AttackType.ShieldBash )
											m_NextAttackAction = DateTime.Now + TimeSpan.FromSeconds( 1.0 );
										else
											m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
									}
								}
								/*if ( csa.RotationTimer == null ) // no more delay for dodging
								{
									//csa.NextAction = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
									csa.CombatInterrupt( false );
								}*/ // Dodge no longer interrupts
								
								attacker.PlaySound( 1307 );
								defender.Say( "*dodges*" );
								
								/*if ( csa.RotationTimer == null )
								{
									switch ( (int)type )
									{
										case (int)AttackType.Swing:
										{
											if ( defender.Body.Type == BodyType.Human )
												csa.Animate( 21, 1, 1, false, false, 4 );
											break;
										}
										
										case (int)AttackType.Thrust:
										{
											if ( defender.Body.Type == BodyType.Human )
												csa.Animate( 30, 7, 1, true, false, 0 );
											break;
										}
										
										case (int)AttackType.Overhead:
										{
											goto case (int)AttackType.Thrust; // same anim
										}
									}
									csa.UpdateACBrainExternal();
								}*/
							}
						}
						if ( !parried )
						{
                            bool noInterruption = defender is BaseCreature && ( (BaseCreature)defender ).CantBeInterrupted;
                            bool cantInterrupt = attacker is BaseCreature && ( (BaseCreature)attacker ).CantInterrupt;
							bool success = true;
							if ( m_Charging || m_DefensiveFormation )
							{
								double skill = (attacker.Skills[weapon.Skill].Base)/100.0;
								if ( Utility.RandomDouble() > skill )
									success = false;
							}
							if ( success ) // this can only miss when charging with below 100.0 skill
							{
								if ( type == AttackType.ShieldBash )
									attacker.PlaySound( 0x3AC );
								else
									PlayHitSound( attacker, defender );
									
								if ( weapon is Fists && !defender.Mounted && csa.AttackTimer != null )
								{
									if ( csa.AttackTimer.Type == type || attackerDCIRoll || csa.AttackTimer.Type == AttackType.Throw )
									{ // throw always gets interrupted
										// this is also an interrupt, as it is an external animation
										if ( attackerDCIRoll ) // this and the defenderHCIRoll cannot happen both, we made sure of that
										{
											type = csa.AttackTimer.Type;
											percentageDamage = GetDirectionDamage( type );
										}
										bool changedDirection = false;
										if ( defenderHCIRoll ) // they might change direction
										{
											AttackType newType = AttackType.Invalid;
											foreach ( AttackType atktype in csa.GetPossibleAttacks() )
											{
												if ( atktype == csa.AttackTimer.Type )
													continue; // skip same attack
												else if ( newType == AttackType.Invalid )
													newType = atktype;
												else if ( csa.GetDirectionDamage( atktype ) > csa.GetDirectionDamage( newType ) )
													newType = atktype;
											}
											
											if ( newType != AttackType.Invalid )
											{
												csa.AttackTimer.Type = newType;
												changedDirection = true;
											}
										}
										if ( !csa.PerformingSequence && csa.RotationTimer == null && !changedDirection )
										{
											if ( !opportunity && !noInterruption && !cantInterrupt ) // opportunity attacks do not interrupt
											{
												weapon.PlayHurtAnimation( defender );
												if ( defender.Mounted ) // there's no hurt animation for it
													ResetAnimation( defender );
												csa.CombatInterrupt( true );
											}
											if ( ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.Disarm) > 0 && attacker is PlayerMobile )
											{
												if ( ((PlayerMobile)attacker).CanUseMartialPower )
												{ // Disarm class will check for fists, not movable, etc.. we don't have to
													if ( Utility.RandomDouble() <= ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.Disarm)*0.01 )
														Server.Misc.Disarm.Effect( attacker, defender );
												}
											}
										}
									}
								}
								else
								{
									if ( !csa.PerformingSequence && csa.RotationTimer == null )
									{
                                        if( !opportunity && !noInterruption && !cantInterrupt )
										{ // AoO does not interrupt
											weapon.PlayHurtAnimation( defender );
											if ( defender.Mounted ) // there's no hurt animation for it
												ResetAnimation( defender );
											csa.CombatInterrupt( true );
										}
									}
								}
								
								if ( csa.CruiseControl && csa.ACBrain != null && csa.ACBrain.Enabled )
									csa.ACBrain.RegisterFrequency( type, defender );
								weapon.OnBeforeSwing( attacker, defender );
								weapon.OnSwing( attacker, defender, damageBonus*percentageDamage, false );
								
								PlayerMobile pmatk = attacker as PlayerMobile;
								if ( type != AttackType.ShieldBash )
								{
									if ( pmatk != null )
									{
										if ( pmatk.HasSpecializedWeaponSkill() )
											CombatSystemAttachment.FightingStyleOnHit( attacker );
									}
									
									if ( attacker.Weapon is Fists && ((IKhaerosMobile)attacker).CanUseMartialStance )
									{
										if ( ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.Buildup) > 0 )
											CombatSystemAttachment.BuildupOnHit( attacker );
									}
								}
							}
							else
								attacker.SendMessage( "Your charge attack misses!" );
							
							if ( !spinning )
							{
								if ( !opportunity )
								{
									if ( type == AttackType.ShieldBash )
										m_NextAttackAction = DateTime.Now + TimeSpan.FromSeconds( 1.0 );
									else
										m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
								}
							}
						}
					}

                    if( attacker is BaseCreature )
                    {
                        bool melee = !( attacker.Weapon is BaseRanged ) && type != AttackType.Throw;
                        ( (BaseCreature)attacker ).OnGaveAttack( melee, parried, defender );                  
                    }

                    if( defender is BaseCreature )
                    {
                        bool melee = !( attacker.Weapon is BaseRanged ) && type != AttackType.Throw;
                        ( (BaseCreature)defender ).OnReceivedAttack( melee, parried, attacker );
                    }
				}
				else if ( type == AttackType.Throw && defender != null && defender.Alive ) // throw attacks, this can't be spinning so who cares
				{
                    if( weapon is Boomerang && !m_OffHand ) // this isn't really "thrown" mechanically
					{
						if ( defender.InRange( attacker, myRange ) && attacker.CanSee( defender ) && attacker.InLOS( defender ) )
						{
							attacker.PlaySound( 1329 );
							weapon.OnSwing( attacker, defender, weapon.RangedPercentage, false );
							if ( !opportunity )
								m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
						}
					}
					else // actually mechanically thrown weapon
					{
						IKhaerosMobile km = attacker as IKhaerosMobile;
						if ( defender.InRange( attacker, 3 + km.Feats.GetFeatLevel(FeatList.ThrowingMastery) ) && !defender.Deleted && defender.Map == attacker.Map &&
							attacker.CanSee( defender ) && attacker.InLOS( defender ))
						{
							bool snatched = false;
							bool deflected = false;

							if( defender is PlayerMobile )
							{
								if( ( (PlayerMobile)defender ).Snatched() )
									snatched = true;

								if( ( (PlayerMobile)defender ).DeflectedProjectile() )
									deflected = true;
							}

                            if( m_OffHand )
								weapon = ((PlayerMobile)attacker).OffHandWeapon;
							
							double damagebonus = 1.0;

							if( weapon.Throwable )
							{
								switch( km.Feats.GetFeatLevel(FeatList.Finesse) )
								{
									case 0: break;
									case 1: damagebonus = 1.1; break;
									case 2: damagebonus = 1.2; break;
									case 3: damagebonus = 1.3; break;
								}
							}
							
							/*if ( km.Feats.GetFeatLevel(FeatList.Finesse) > 0 && Utility.RandomDouble() <= (0.6*(km.Feats.GetFeatLevel(FeatList.Finesse))/(weapon.Weight*weapon.Weight) ) )
								damagebonus += 1.0;*/
                            /*if (km.Feats.GetFeatLevel(FeatList.Finesse) > 0)
                                damagebonus += (0.2 * km.Feats.GetFeatLevel(FeatList.Finesse));*/

							km.OffensiveFeat = FeatList.ThrowingMastery;
							Point3D loc = defender.Location;
							Map map = defender.Map;
							//attacker.MovingEffect( defender, weapon.ItemID, 9, 1, false, false );
							SendCombatAlerts( attacker, defender, weapon.ItemID, 5, 0, false, false, weapon.Hue, 1, 9501, 1, 0, EffectLayer.Waist, 0x100 );
							if( snatched )
							{
								attacker.PlaySound( 1329 );
								defender.Emote( "*snatches the weapon thrown at {0} by {1}*", defender.Female == true ? "her" : "him", attacker.Name );
							}

							else if( deflected )
							{
								attacker.PlaySound( 1329 );
								defender.Emote( "*uses {0} shield to deflect the weapon thrown at {1} by {2}*", defender.Female == true ? "her" : "him", defender.Female == true ? "her" : "him", attacker.Name );
							}

							else
							{
								attacker.PlaySound( 1329 );
								
								// 50% damage penalty from off-hand throwing
                                if( m_OffHand )
									damagebonus *= 0.5;
								
								weapon.OnSwing( attacker, defender, damagebonus, true ); // bestDamage will be multiplied with in BaseWeapon due to "cleave"
							}
								
							if ( !snatched )
								weapon.MoveToWorld( loc, map );
							else
								defender.AddToBackpack( weapon ); // Nigga stole my ______!
							
							// auto-equipping
                            if( ( weapon is Dagger || weapon is ThrowingAxe ) && !m_OffHand && attacker is PlayerMobile && ( (PlayerMobile)attacker ).Feats.GetFeatLevel( FeatList.ThrowingMastery ) > 2 )
							{
								PlayerMobile m = attacker as PlayerMobile;
								
								if( m.CraftContainer != null && !m.CraftContainer.Deleted && m.Backpack != null && !m.Backpack.Deleted && m.CraftContainer.IsChildOf(m.Backpack) )
								{
									foreach( Item item in m.CraftContainer.Items )
						        	{
										if( item is BaseWeapon && weapon.NameType == ((BaseWeapon)item).NameType )
						        		{
											m.EquipItem( item );
						        			break;
						        		}
						        	}
								}
							}
							if ( !opportunity )
								m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
						}
					}
				}
				else // miss due to range
				{
					if ( !spinning )
					{
						weapon.InitializeManeuver( attacker, null, false );
						weapon.EndManeuver( attacker );
					}
					PlayMissSound( attacker, defender );
				}
			}

			if ( spinning )
			{
				m_RotationTimer = null;
				if ( !opportunity )
				m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
			}
			
			m_AttackTimer = null;
			if ( !flashy ) // otherwise another attack is coming right after
			{
				UpdateACBrainExternal();
				UpdateQueueDelay();
			}

            if (attacker != null && !attacker.Deleted && attacker.Weapon != null && attacker.Weapon is BaseWeapon && !(attacker.Weapon as BaseWeapon).Deleted)
            {
                    if ((attacker.Weapon as BaseWeapon).MaxHitPoints < 1)
                    {
                        attacker.SendMessage("Your weapon has fallen apart!");
                        attacker.SendSound(0x3E8, attacker.Location);
                        (attacker.Weapon as BaseWeapon).Delete();
                    }
            }
		}
		
		public void CancelQueuedAction()
		{
			if ( m_QueuedActionTimer != null )
			{
				m_QueuedActionTimer.Stop();
				m_QueuedActionTimer = null;
			}
		}
		
		public bool QueueRanged()
		{
			return QueueAction( 999 );
		}
		public bool QueueAction( AttackType attack )
		{
			return QueueAction( (int)attack );
		}
		public bool QueueAction( DefenseType defense )
		{
			return QueueAction( ((int)defense)+100 );
		}
		
		public bool QueueAction( int actionID )
		{
			Mobile mob = AttachedTo as Mobile;
			if ( m_QueuedActionTimer != null )
			{
				if ( m_QueuedActionTimer.Action == actionID ) // clear queue
				{
					m_QueuedActionTimer.Stop();
					m_QueuedActionTimer = null;
					return false; // tell them we cleared our queue
				}
				else
				{
					m_QueuedActionTimer.Stop();
					m_QueuedActionTimer = new QueuedActionTimer( AttachedTo as Mobile, actionID );
				}
			}
			else
				m_QueuedActionTimer = new QueuedActionTimer( AttachedTo as Mobile, actionID );
			
			bool atk = ( actionID < 100 || actionID == 999 );
			
			TimeSpan waitTime;
			if ( atk )
				waitTime = Max( TimeSpan.Zero, m_NextAttackAction - DateTime.Now );
			else
				waitTime = Max( TimeSpan.Zero, m_NextDefenseAction - DateTime.Now );
				
			if ( ( actionID == 999 || actionID == (int)AttackType.Throw ) && mob.Weapon is BaseRanged ) // ranged
			{
				waitTime = Max( waitTime, ((BaseRanged)mob.Weapon).RequiredStillTime( mob ) - (DateTime.Now - mob.LastMoveTime) );
				if ( m_AimingTimer != null )
					waitTime = Max( waitTime, m_AimingTimer.Next - DateTime.Now );
			}
			m_QueuedActionTimer.Delay = waitTime;
			m_QueuedActionTimer.Start();
			
			return true;
		}
		
		public static void PlayBlockSound( Mobile attacker, Mobile defender, Item attackerWeapon, Item defenderItem )
		{
			bool opponentHasMetalSound = HasMetallicSound( defenderItem );
			bool attackerHasMetalSound = HasMetallicSound( attackerWeapon );

            if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
                attackerHasMetalSound = true;

            if( defender is PlayerMobile && ( (PlayerMobile)defender ).Claws != null )
                opponentHasMetalSound = true;

			if ( attacker.Weapon is Fists && defender.Weapon is Fists )
			{
				switch ( Utility.Random(1, 5) )
				{
					case 1:
					{
						attacker.PlaySound( 933 );
						break;
					}
					case 2:
					{
						attacker.PlaySound( 934 );
						break;
					}
					
					case 3:
					{
						attacker.PlaySound( 935 );
						break;
					}
					
					case 4:
					{
						attacker.PlaySound( 937 );
						break;
					}
				}
			}
			else if ( opponentHasMetalSound && attackerHasMetalSound ) // both have metallic sounds
			{
				if ( defenderItem is BaseShield )
					attacker.PlaySound( 567 );
				else // two weapons clashing
				{
					switch ( Utility.Random( 1, 4 ) )
					{
						case 1:
						{
							attacker.PlaySound( 954 );
							break;
						}
						
						case 2:
						{
							attacker.PlaySound( 955 );
							break;
						}
						
						case 3:
						{
							attacker.PlaySound( 956 );
							break;
						}
					}
				}
			}
			else if ( !opponentHasMetalSound && !attackerHasMetalSound ) // both are wooden
				attacker.PlaySound( Utility.RandomBool() ? 1334 : 1335 );
			else // mix metal and wood
			{
				switch ( Utility.Random(1, 4) )
				{
					case 1:
					{
						attacker.PlaySound( 320 );
						break;
					}
					case 2:
					{
						attacker.PlaySound( 313 );
						break;
					}
					
					case 3:
					{
						attacker.PlaySound( 322 );
						break;
					}
				}
			}
		}
		
		public static TimeSpan Max( TimeSpan a, TimeSpan b )
		{
			if ( a >= b )
				return a;
			else
				return b;
		}
		
		public static TimeSpan Min( TimeSpan a, TimeSpan b )
		{
			if ( a <= b )
				return a;
			else
				return b;
		}
		
		public static void PlayMissSound( Mobile attacker, Mobile defender )
		{
			if ( attacker.Weapon is Fists )
				attacker.PlaySound( Utility.RandomBool() ? 1337 : 1338 );
			else
				if ( attacker.Weapon is BaseWeapon )
					attacker.PlaySound(((BaseWeapon)attacker.Weapon).GetMissAttackSound( attacker, defender ));
		}
		
		public static void PlayHitSound( Mobile attacker, Mobile defender )
		{
            CombatSystemAttachment csa = XmlAttach.FindAttachment( attacker, typeof( CombatSystemAttachment ) ) as CombatSystemAttachment;

            if( csa.Offhand )
				return; // no sound
			if ( attacker.Body.Type != BodyType.Human )
			{
				attacker.PlaySound( attacker.GetAttackSound() );
			}
			
			else if ( attacker.Weapon is Fists )
			{
                if( attacker is PlayerMobile && ( (PlayerMobile)attacker ).Claws != null )
                {
                    attacker.PlaySound( 0x23B );
                    return;
                }

				switch ( Utility.Random(1, 8) )
				{
					case 1:
					{
						attacker.PlaySound( 309 );
						break;
					}
					case 2:
					{
						attacker.PlaySound( 311 );
						break;
					}
					
					case 3:
					{
						attacker.PlaySound( 315 );
						break;
					}
					
					case 4:
					{
						attacker.PlaySound( 316 );
						break;
					}
					
					case 5:
					{
						attacker.PlaySound( 317 );
						break;
					}
					
					case 6:
					{
						attacker.PlaySound( 321 );
						break;
					}
					
					case 7:
					{
						attacker.PlaySound( 330 );
						break;
					}
				}
			}
			else
				if ( attacker.Weapon is BaseWeapon )
					attacker.PlaySound(((BaseWeapon)(attacker.Weapon)).GetHitAttackSound( attacker, defender ));
		}
		
		public void CancelAim( bool resetAnim )
		{
			if ( m_AimingTimer != null )
			{
				m_AimingTimer.Stop();
				m_AimingTimer = null;
			}
			if ( m_Aiming )
			{
				m_Aiming = false;
				StopAnimating( resetAnim );
			}
		}
		
		public bool BeginRangedAttack()
		{
            m_OffHand = false;

			if ( m_Aiming )
				return Fire( true );
			else
				return BeginAiming( true );
		}
		
		public bool CanBeginAiming()
		{
			Mobile mob = AttachedTo as Mobile;
			
			if ( m_PerformingSequence )
			{
				m_ErrorMessage = "You cannot do that right now.";
				return false;
			}
			else if ( !(mob.Weapon is BaseRanged) )
			{
				m_ErrorMessage = "Only ranged weapons can perform this attack.";
				return false;
			}
			else if ( mob.Weapon is Boomerang )
			{
				m_ErrorMessage = "Boomerangs use the throw attack, not the ranged one.";
				return false;
			}
			else if ( !((BaseRanged)mob.Weapon).IsStill( mob ) )
			{
				m_ErrorMessage = "You must be standing still in order to aim.";
				return false;
			}
			else if ( m_AimingTimer != null )
			{
				m_ErrorMessage = "You are already entering aiming mode.";
				return false;
			}
			
			return true;
		}
		
		public double AimedShotConvergenceSpeed()
		{
			IKhaerosMobile km = AttachedTo as IKhaerosMobile;
			return 1.0 + Math.Max( 0.0, (m_NextAttackAction - DateTime.Now).TotalSeconds );
		}
		
		public static void AimedShotCueCallback( object objstate )
		{
			CombatSystemAttachment csa = objstate as CombatSystemAttachment;
			Mobile mob = csa.AttachedTo as Mobile;
			if ( csa.Aiming && csa.AimConverge <= DateTime.Now )
			{
				// this will send a local effect only (won't be seen by others)
				NetState state = mob.NetState;
				if ( state == null )
					return;
				
				Packet particles = null, regular = null;
				state.Mobile.ProcessDelta();
				
				IEntity from = new Entity( Server.Serial.Zero, new Point3D( mob.X, mob.Y, mob.Z ), mob.Map );
				IEntity to = new Entity( Server.Serial.Zero, new Point3D( mob.X, mob.Y, mob.Z + 50 ), mob.Map );
				
				if ( Effects.SendParticlesTo( state ) )
				{
					particles = Packet.Acquire( new MovingParticleEffect( from, to, 7726, 1, 0, true, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 ) );
					state.Send( particles );
				}
				else
				{
					if ( regular == null )
						regular = Packet.Acquire( new MovingEffect( from, to, 7726, 1, 0, true, false, 33, 3 ) );

					state.Send( regular );
				}
				
				Packet.Release( particles );
				Packet.Release( regular );

				mob.SendSound( 0x145 ); // local sound as well
			}
		}
		
		public bool EnterAimingMode()
		{
			Mobile attacker = AttachedTo as Mobile;
			IKhaerosMobile km = attacker as IKhaerosMobile;
			BaseRanged weapon = attacker.Weapon as BaseRanged;
			if ( weapon == null || km == null ) return false;
			if ( m_AimingTimer != null )
			{
				m_AimingTimer.Stop();
				m_AimingTimer = null;
			}
			
			m_Aiming = true;
			m_AimStart = DateTime.Now;
			if ( weapon.AmmoType == typeof( Bolt ) ) // crossbows
			{
				if ( CanUseAimedShot() )
				{
					m_AimConverge = DateTime.Now + TimeSpan.FromSeconds( AimedShotConvergenceSpeed() );
					Timer.DelayCall( TimeSpan.FromSeconds( AimedShotConvergenceSpeed() ),
					new TimerStateCallback( AimedShotCueCallback ), this );
				}
			
				if ( attacker.Body.Type == BodyType.Human )
				{
					if ( !attacker.Mounted )
						StartAnimating( 19, 3, 1, false, false, 255 );
					else
						StartAnimating( 28, 3, 1, false, false, 255 );
				}
				else
				{
					int[] anim = BAData.GetRangedAnimation( attacker );
					if ( anim != null )
						Animate( anim[0], anim[1], 1, false, false, 255 );
				}
			}
			else // bows
			{
				if ( attacker.Body.Type == BodyType.Human )
				{
					if ( !attacker.Mounted )
						StartAnimating( 18, 5, 1, false, false, 255 );
					else
						StartAnimating( 27, 3, 1, false, false, 255 );
				}
				else
				{
					int[] anim = BAData.GetRangedAnimation( attacker );
					if ( anim != null )
						Animate( anim[0], anim[1], 1, false, false, 255 );
				}
			}
			
			UpdateACBrainExternal();
			return true;
		}
		
		public bool BeginAiming()
		{
			return BeginAiming( false );
		}
		public bool BeginAiming( bool playerInitiated )
		{
			Mobile mob = AttachedTo as Mobile;
			BaseRanged weapon = mob.Weapon as BaseRanged;
			if ( playerInitiated && weapon != null && ( !weapon.IsStill( mob ) || m_AimingTimer != null ) ) // queuing purposes
			{
				m_ErrorMessage = "";
				DisplayQueueResultMessage( QueueRanged() );
				
				return false;
			}
			
			if ( !CanBeginAttack() || !CanBeginCombatAction() || !CanBeginAiming() )
				return false;
				
			StopAllActions( false );
				
			if ( mob.Body.Type == BodyType.Human ) // non humans don't have a pull up animation, they just get an aiming one
			{
				if ( weapon.AmmoType == typeof( Bolt ) ) // crossbows
				{
					if ( !mob.Mounted )
						Animate( 19, 3, 1, true, false, 0 );
					else
						Animate( 28, 3, 1, true, false, 0 );
				}
				else // bows
				{
					if ( !mob.Mounted )
						Animate( 18, 5, 1, true, false, 0 );
					else
						Animate( 27, 3, 1, true, false, 0 );
				}
			}
			
			m_Aiming = false;
			m_AimingTimer = new AimingTimer( mob, TimeSpan.FromSeconds( 0.3 ) );
			m_AimingTimer.Start();
			if ( BandageContext.GetContext( mob ) != null )
			{
				BandageContext.GetContext( mob ).StopHeal();
				if ( mob is IKhaerosMobile )
				{
					if ( ((IKhaerosMobile)mob).HealingTimer != null )
					{
						((IKhaerosMobile)mob).HealingTimer.Stop();
						((IKhaerosMobile)mob).HealingTimer = null;
					}
				}
			}
			
			if ( mob.Combatant != null && mob.Direction != mob.GetDirectionTo( mob.Combatant ) && mob.CanSee(mob.Combatant) )
				mob.Direction = mob.GetDirectionTo( mob.Combatant );
			return true;
		}
		
		public void DisplayQueueResultMessage( bool result )
		{
			Mobile mob = AttachedTo as Mobile;
			if ( !m_CruiseControl || ( m_CruiseControl && m_ACBrain != null && m_ACBrain.Enabled ) )
			{
				if ( result )
					mob.SendMessage( "Action queued." );
				else
					mob.SendMessage( "Queue cleared." );
			}
			// otherwise was not queued, because we are in autocombat without an ACBrain
		}
		
		public bool Fire()
		{
			return Fire( false );
		}
		
		public bool CanUseAimedShot() 
		{
			IKhaerosMobile km = AttachedTo as IKhaerosMobile;
			if ( km == null || km.Feats.GetFeatLevel(FeatList.AimedShot) <= 0 || !((AttachedTo as Mobile).Weapon is BaseRanged) )
				return false;
			if ( (AttachedTo as Mobile).Mounted && km.Feats.GetFeatLevel(FeatList.MountedArchery) < 3 )
				return false;
			return true;
		}
		
		public bool Fire( bool playerInitiated )
		{
			Mobile mob = AttachedTo as Mobile;
			if ( mob == null )
				return false;
			BaseRanged weapon = mob.Weapon as BaseRanged;
			double aimedShotChance = 0.0;
			if ( !CanBeginCombatAction() )
				return false;
			
			if ( m_Aiming && m_Opponent != null && mob.Combatant != null && weapon != null )
			{
				IKhaerosMobile km = mob as IKhaerosMobile;
				if ( km == null )
					return false;
				if ( CanUseAimedShot() && weapon.AmmoType == typeof( Bolt ) )
				{
					double scalar = 0;
					if ( DateTime.Now >= m_AimConverge )
						scalar = 1.0;
					else
						scalar = 1.0 - ((m_AimConverge - DateTime.Now).TotalSeconds);
					scalar = Math.Min( 1.0, Math.Max( 0.0, scalar ) );
					aimedShotChance = (0.25+km.Feats.GetFeatLevel(FeatList.AimedShot)*0.25)*scalar;
				}
				
				int myRange = weapon.MaxRange;
				if ( mob is BaseCreature && ((BaseCreature)mob).RangeFight > myRange )
					myRange = ((BaseCreature)mob).RangeFight;
				if ( m_Opponent.InRange( mob, myRange ) )
				{
					if ( mob.CanSee( m_Opponent ) && mob.InLOS( m_Opponent ) )
					{
						if ( !m_Opponent.InRange( mob, 1 ) )
						{
							if ( weapon.IsStill( mob ) )
							{
								Container pack = mob.Backpack;
								if ( ( !(mob is PlayerMobile) && !(mob is Mercenary )) || (pack != null && pack.FindItemByType( weapon.AmmoType ) != null ))
								{
									m_NextAttackAction = DateTime.Now + ComputeNextSwingTime();
									m_AimStart = DateTime.Now;
									m_AimConverge = DateTime.Now + TimeSpan.FromSeconds( AimedShotConvergenceSpeed() );
									
									if ( mob.Direction != mob.GetDirectionTo( m_Opponent ) && mob.CanSee(mob.Combatant) )
										mob.Direction = mob.GetDirectionTo( m_Opponent );
										
									if ( aimedShotChance > Utility.RandomDouble() )
										weapon.OnSwing( mob, m_Opponent, weapon.RangedPercentage, false, true ); // assume hit
									else
										weapon.OnSwing( mob, m_Opponent, weapon.RangedPercentage, false );
									
									if ( weapon.AmmoType == typeof( Bolt ) )
									{
										if ( CanUseAimedShot() )
										{
											Timer.DelayCall( TimeSpan.FromSeconds( AimedShotConvergenceSpeed() ),
											new TimerStateCallback( AimedShotCueCallback ), this );
										}
										
										if ( mob.Body.Type == BodyType.Human )
										{
											if ( !mob.Mounted )
												Animate( 19, 5, 1, false, false, 0 );
											else
												Animate( 28, 5, 1, false, false, 0 );
										}
									}
									else
									{
										if ( mob.Body.Type == BodyType.Human )
										{
											if ( !mob.Mounted )
												Animate( 18, 5, 1, false, false, 0 );
											else
												Animate( 27, 4, 1, false, false, 0 );
										}
									}

									UpdateACBrainExternal();
									Timer.DelayCall( TimeSpan.FromMilliseconds( 500 ), new TimerStateCallback( AnimationRefreshCallback ), this );
									return true;
								}
								else
								{
									m_ErrorMessage = "You are out of ammunition.";
									return false;
								}
							}
							else
							{
								if ( playerInitiated )
								{
									m_ErrorMessage = "";
									DisplayQueueResultMessage( QueueRanged() );
								}
								
								return false;
							}
						}
						else
						{
							m_ErrorMessage = "Your opponent is too close!";
							return false;
						}
					}
					else
					{
						m_ErrorMessage = "You cannot see your opponent from here.";
						return false;
					}
				}
				else
				{
					m_ErrorMessage = "That is out of range.";
					return false;
				}
			}
			else
			{
				m_ErrorMessage = "You must be aiming at someone in order to fire.";
				return false;
			}
		}
		public bool CanBeginAttack( )
		{
			return CanBeginAttack( AttackFlags.None );
		}
		
		public bool CanBeginAttack( AttackFlags flags )
		{
			Mobile attacker = AttachedTo as Mobile;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			bool sbash = ( ((IKhaerosMobile)attacker).OffensiveFeat == FeatList.ShieldBash );
			bool disregardDelay = ((flags&AttackFlags.DisregardDelay) != AttackFlags.None);
			if ( m_PerformingSequence )
			{
				m_ErrorMessage = "You cannot do that right now.";
				return false;
			}
			else if ( ( m_AttackTimer != null && !m_AttackTimer.Feint ) || m_RotationTimer != null )
			{
				m_ErrorMessage = "You are too busy attacking.";
				return false;
			}
			else if ( weapon.CannotUseOnFoot && !attacker.Mounted && !sbash )
			{
				m_ErrorMessage = "That weapon cannot be used on foot.";
				return false;
			}
			else if ( weapon.ChargeOnly && !sbash )
			{
				m_ErrorMessage = "That weapon can only be charged with.";
				return false;
			}
			else if ( attacker.Mounted && sbash )
			{
				m_ErrorMessage = "That attack cannot be used while mounted.";
				return false;
			}
			else if( weapon.CannotUseOnMount && attacker.Mounted )
			{
				m_ErrorMessage = "That weapon cannot be used while mounted.";
				return false;
			}
			else if ( !disregardDelay && m_NextAttackAction > DateTime.Now )
			{
				m_ErrorMessage = "You must wait before performing another offensive action.";
				return false;
			}
			
			return true;
		}
		
		public bool CanBeginMeleeAttack()
		{
			Mobile attacker = AttachedTo as Mobile;
			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			if ( weapon == null )
				return false;
				
			if ( attacker.Weapon is BaseRanged )
			{
				m_ErrorMessage = "That weapon cannot be used to perform melee attacks.";
				return false;
			}
			
			return true;
		}
		
		public bool CanBeginParry()
		{
			Mobile defender = AttachedTo as Mobile;
			BaseWeapon weapon = defender.Weapon as BaseWeapon;
			BaseShield shield = defender.FindItemOnLayer( Layer.TwoHanded ) as BaseShield;
			
			if ( weapon == null && !(defender is PlayerMobile && ((PlayerMobile)defender).Claws != null) )
				return false;
			if ( weapon is BaseRanged )
			{
				m_ErrorMessage = "Ranged weapons cannot parry.";
				return false;
			}
			else if ( m_NextDefenseAction > DateTime.Now )
			{
				m_ErrorMessage = "You must wait before performing another defensive action.";
				return false;
			}
			else if ( m_PerformingSequence && ((IKhaerosMobile)defender).TrippedTimer == null ) // if tripped, they can parry
			{
				m_ErrorMessage = "You cannot do that right now."; // i think this is for queue so it is not queued
				return false;
			}
			
			return true;
		}
		
		public TimeSpan ComputeNextSwingTime()
		{
			Mobile mob = AttachedTo as Mobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon == null )
				return TimeSpan.FromSeconds( 1.0 );
			else
			{
				TimeSpan speed = weapon.GetDelay( mob );
				return speed;
			}
		}
		
		public void FinishDefense()
		{
			Mobile mob = AttachedTo as Mobile;
			IKhaerosMobile km = mob as IKhaerosMobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( m_DefenseTimer == null )
				return;	// we weren't defending

			int parries = m_DefenseTimer.Parries;
			DefenseType type = m_DefenseTimer.Type;
			/*bool weaponParrySuccess = false;
			
			if ( km != null && weapon != null && !( mob.FindItemOnLayer( Layer.TwoHanded) is BaseShield ) )
			{
				if ( parries > 0 && km.Feats.GetFeatLevel(FeatList.WeaponParrying) > 0 && mob is PlayerMobile ) // weapon parrying feat
				{
					PlayerMobile pm = mob as PlayerMobile;
					if ( weapon.NameType == pm.WeaponSpecialization || weapon.NameType == pm.SecondSpecialization )
					{
						if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) == 1 && type == DefenseType.ParrySwing )
							weaponParrySuccess = true;
						else if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) == 2 && (type == DefenseType.ParrySwing || type == DefenseType.ParryThrust) )
							weaponParrySuccess = true;
						else if ( km.Feats.GetFeatLevel(FeatList.WeaponParrying) >= 3 )
							weaponParrySuccess = true;
					}
				}
			}
			if ( weaponParrySuccess )
				m_NextAction = DateTime.Now;*/
			m_DefenseTimer.Stop();
			m_DefenseTimer = null;
			// reset animation
			if ( km.TrippedTimer == null )
				ResetAnimation( mob );
			else // assume 'dead' position again
				LoopLyingDownAnimation();
			
			UpdateACBrainExternal();
			//UpdateQueueDelay();
				
			List<Mobile> opponents = new List<Mobile>( m_Aggressors ); // necessary due to concurrency

			foreach ( Mobile opponent in opponents )
				GetCSA( opponent ).UpdateACBrainExternal();
		}
		
		public void UpdateQueueDelay()
		{
			if ( m_QueuedActionTimer != null )
			{
				bool attk = ( m_QueuedActionTimer.Action < 100 || m_QueuedActionTimer.Action == 999 );
				TimeSpan delay = TimeSpan.Zero;
				if ( attk )
					delay = m_NextAttackAction - DateTime.Now;
				else
					delay = m_NextDefenseAction - DateTime.Now;
					
				if ( m_QueuedActionTimer.Running )
				{
					m_QueuedActionTimer.Stop();
					m_QueuedActionTimer.Delay = delay;
					m_QueuedActionTimer.Start();
				}
				else
				{
					m_QueuedActionTimer.Delay = delay;
					m_QueuedActionTimer.Start();
				}
			}
		}
		
		public void SendCombatAlerts( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
		{
			if ( from is Mobile )
				((Mobile)from).ProcessDelta();

			if ( to is Mobile )
				((Mobile)to).ProcessDelta();

			Map map = from.Map;
			
			if ( map != null )
			{
				Packet particles = null, regular = null;

				IPooledEnumerable eable = map.GetClientsInRange( from.Location );

				foreach ( NetState state in eable )
				{
					state.Mobile.ProcessDelta();

					Mobile mob = state.Mobile;
					CombatSystemAttachment mobCSA = CombatSystemAttachment.GetCSA( mob );
					if ( !mobCSA.AlwaysDisplayIcons && mobCSA != this )
					{
						if ( m_Opponent != mob && mobCSA.Opponent != AttachedTo )
							continue;
					}
					
					if ( Effects.SendParticlesTo( state ) )
					{
						if ( particles == null )
							particles = Packet.Acquire( new MovingParticleEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, layer, unknown ) );

						state.Send( particles );
					}
					else if ( itemID > 1 )
					{
						if ( regular == null )
							regular = Packet.Acquire( new MovingEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode ) );

						state.Send( regular );
					}
				}

				Packet.Release( particles );
				Packet.Release( regular );

				eable.Free();
			}

			//SendPacket( from.Location, from.Map, new MovingParticleEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, unknown ) );
		}
		
		public static CombatSystemAttachment GetCSA( Mobile mob )
		{
			if ( mob == null )
				return null;
			CombatSystemAttachment csa = XmlAttach.FindAttachment( mob, typeof( CombatSystemAttachment ) ) as CombatSystemAttachment;
			if ( csa == null )
			{
				csa = new CombatSystemAttachment();
				XmlAttach.AttachTo( mob, csa );
			}
			
			return csa;
		}
		public List<AttackType> GetPossibleAttacks()
		{
			return GetPossibleAttacks( false );
		}
		public List<AttackType> GetPossibleAttacks( bool considerAC )
		{
			List<AttackType> possibleAttacks = new List<AttackType>();
			Mobile mob = AttachedTo as Mobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			
			if ( (m_ACBrain == null || !m_CruiseControl) && considerAC )
				considerAC = false;
			
			if ( weapon == null || (considerAC && !m_ACBrain.Attack) )
				return possibleAttacks;
			
			if ( weapon.OverheadPercentage > 0 )
				if ( !considerAC || m_ACBrain.PerformOverhead )
					possibleAttacks.Add( AttackType.Overhead );
			if ( !mob.Mounted )
			{
				if ( weapon.SwingPercentage > 0 )
					if ( !considerAC || m_ACBrain.PerformSwing )
						possibleAttacks.Add( AttackType.Swing);
				if ( weapon.ThrustPercentage > 0 )
					if ( !considerAC || m_ACBrain.PerformThrust )
						possibleAttacks.Add( AttackType.Thrust );
			}
			else if ( mob.Mounted && CanThrustOnMount() )
				if ( !considerAC || m_ACBrain.PerformThrust )
					if ( weapon.ThrustPercentage > 0 )
						possibleAttacks.Add( AttackType.Thrust );
					
			return possibleAttacks;
		}
		
		public static bool ReqsAltWarmodeAnimReset( Item item )
		{	// I think this is all of them. There's no property that could be extracted that made them behave so strangely,
			// so ultimately I had to compile a list.
			return ( item is Bardiche || item is Halberd || item is Pitchfork || item is QuarterStaff || item is ClericCrook || 
				item is BlackStaff || item is GnarledStaff || item is WarHammer || item is BattleAxe || item is Hatchet ||
				item is LargeBattleAxe || item is TwoHandedAxe || item is HaftedAxe || item is BeardedDoubleAxe );
		}
		
		private static Dictionary<Mobile, BonusTableEntry> m_FightingStyleTable = new Dictionary<Mobile, BonusTableEntry>();
		private static Dictionary<Mobile, BonusTableEntry> m_BuildupTable = new Dictionary<Mobile, BonusTableEntry>();
		
		private class BonusTableEntry
		{
			double m_Bonus;
			BonusTimer m_DecayTimer;
			Mobile m_Mobile;
			bool m_FightingStyle; // is this the fighting style? if not, it's buildup
			
			public BonusTableEntry( double bonus, TimeSpan decay, Mobile mob, bool fightingstyle )
			{
				m_Bonus = bonus;
				m_Mobile = mob;
				m_FightingStyle = fightingstyle;
				m_DecayTimer = new BonusTimer( decay, this );
				m_DecayTimer.Start();
			}
			
			public void RefreshTimer( TimeSpan newDuration )
			{
				m_DecayTimer.Stop();
				m_DecayTimer = new BonusTimer( newDuration, this );
				m_DecayTimer.Start();
			}
			
			public double Bonus { get{ return m_Bonus; } set{ m_Bonus = value; } }
			
			public void ForceEnd()
			{
				m_DecayTimer.Stop();
				if ( !m_FightingStyle )
					CombatSystemAttachment.CancelBuildupBonus( m_Mobile );
				else
					CombatSystemAttachment.CancelFightingStyleBonus( m_Mobile );
			}
			
			private class BonusTimer : Timer
			{
				BonusTableEntry m_Parent;
				public BonusTimer( TimeSpan delay, BonusTableEntry parent ) : base( delay )
				{
					m_Parent = parent;
					Priority = TimerPriority.OneSecond;
				}

				protected override void OnTick()
				{
					m_Parent.ForceEnd();
				}
			}
		}
		
		public static double BuildupBonuses( Mobile mob )
		{
			if ( m_BuildupTable.ContainsKey( mob ) )
				return m_BuildupTable[mob].Bonus;
			else
				return 0;
		}
		private static void BuildupOnParry( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			
			if ( m_BuildupTable.ContainsKey( mob ) )
				m_BuildupTable[mob].ForceEnd(); // this will take care of everything
		}
		
		public static void CancelBuildupBonus( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			if ( m_BuildupTable.ContainsKey( mob ) )
			{
				m_BuildupTable.Remove( mob );
				if ( pm != null )
					pm.RemoveBuff( BuffIcon.AnimalForm );
			}
		}

		private static void BuildupOnHit( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			IKhaerosMobile km = mob as IKhaerosMobile;
			if ( pm != null )
				pm.RemoveBuff( BuffIcon.AnimalForm );
			double amount = km.Feats.GetFeatLevel(FeatList.Buildup)*0.05;
			
			if ( m_BuildupTable.ContainsKey( mob ) )
			{
				m_BuildupTable[mob].RefreshTimer( TimeSpan.FromSeconds( 20 ) );
				m_BuildupTable[mob].Bonus += amount;
			}
			else
				m_BuildupTable[mob] = new BonusTableEntry( amount, TimeSpan.FromSeconds( 20 ), mob, false );
			if ( pm != null )
				pm.AddBuildupBuff();
		}
		
		private static void FightingStyleOnHit( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			IKhaerosMobile km = mob as IKhaerosMobile;
			if ( pm != null )
				pm.RemoveBuff( BuffIcon.EtherealVoyage );
			double amount = km.Feats.GetFeatLevel(FeatList.FightingStyle)*0.03;
			
			if ( m_FightingStyleTable.ContainsKey( mob ) )
			{
				m_FightingStyleTable[mob].Bonus += amount;
				m_FightingStyleTable[mob].RefreshTimer( TimeSpan.FromSeconds( 20 ) );
			}
			else
			{
				m_FightingStyleTable[mob] = new BonusTableEntry( amount, TimeSpan.FromSeconds( 20 ), mob, true );
			}
			if ( pm != null )
				pm.AddFightingStyleBuff();
		}
		
		private static void FightingStyleOnParry( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			IKhaerosMobile km = mob as IKhaerosMobile;

			double amount = 0.03 * km.Feats.GetFeatLevel(FeatList.FightingStyle);
			
			if ( m_FightingStyleTable.ContainsKey( mob ) )
			{
				m_FightingStyleTable[mob].Bonus -= amount;
				m_FightingStyleTable[mob].RefreshTimer( TimeSpan.FromSeconds( 20 ) );
				if ( pm != null )
					pm.RemoveBuff( BuffIcon.EtherealVoyage );
				if ( m_FightingStyleTable[mob].Bonus <= 0 )
					m_FightingStyleTable[mob].ForceEnd();
				else if ( pm != null )
					pm.AddFightingStyleBuff();
			}
		}
		
		public static double FightingStyleBonuses( Mobile mob )
		{
			if ( m_FightingStyleTable.ContainsKey( mob ) )
				return m_FightingStyleTable[mob].Bonus;
			else
				return 0;
		}
		
		public static void CancelFightingStyleBonus( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;
			if ( m_FightingStyleTable.ContainsKey( mob ) )
			{
				m_FightingStyleTable.Remove( mob );
				if ( pm != null )
					pm.RemoveBuff( BuffIcon.EtherealVoyage );
			}
		}
	}
	
	
	
	public class DefensiveFormationTimer : Timer // this is used for pulling up the polearm
	{
		private Mobile m_Attacker;
		
		public DefensiveFormationTimer( Mobile attacker, TimeSpan delay ) : base( delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Attacker = attacker;
		}

		protected override void OnTick()
		{
			CombatSystemAttachment.GetCSA( m_Attacker ).EnterDefensiveFormation();
		}
	}
	
	public class AimingTimer : Timer // this is used for pulling up the ranged weapon
	{
		private Mobile m_Attacker;
		
		public AimingTimer( Mobile attacker, TimeSpan delay ) : base( delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Attacker = attacker;
		}

		protected override void OnTick()
		{
			CombatSystemAttachment.GetCSA( m_Attacker ).EnterAimingMode();
		}
	}
	
	public class AttackTimer : Timer
	{
		private Mobile m_Attacker;
		private AttackType m_Type;
		private DateTime m_FinishTime;
		private bool m_Feint;
		private bool m_Flashy;
		private bool m_FlashyFollowup;
		
		public bool Feint{ get{ return m_Feint; } set{ m_Feint = value; } }
		public DateTime FinishTime{ get{ return m_FinishTime; } }
		public AttackType Type { get { return m_Type; } set{ m_Type = value; } }
		public bool Flashy{ get{ return m_Flashy; } set{ m_Flashy = value; } }
		public bool FlashyFollowup{ get{ return m_FlashyFollowup; } set{ m_FlashyFollowup = value; } }
		
		public AttackTimer( Mobile attacker, AttackType type, TimeSpan delay ) : base( delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Attacker = attacker;
			m_Type = type;
			m_FinishTime = DateTime.Now + delay;
		}
		
		public void Interrupt()
		{
			CombatSystemAttachment.ResetAnimation( m_Attacker );
			Stop();
		}
		
		public void Force( bool opportunity )
		{
			if( !((IKhaerosMobile)m_Attacker).Deserialized )
				Interrupt();
			
			double dmgBonus = 1.0;
			if ( FlashyFollowup )
				dmgBonus -= (1.0 - ((double)((IKhaerosMobile)m_Attacker).Feats.GetFeatLevel(FeatList.FlashyAttack)) * 0.2);
			if ( !m_Feint || opportunity ) // feint becomes a real attack if AOO is triggered
			{
				CombatSystemAttachment.GetCSA( m_Attacker ).FinishAttack( dmgBonus, false, false, opportunity );
				if ( m_Flashy )
				{
					m_Attacker.Emote( "*immediately follows up with another attack*" );
					CombatSystemAttachment.GetCSA( m_Attacker ).DoFlashyAttack();
				}
			}
			else
				Stop();
		}

		protected override void OnTick()
		{
			Force( false );
		}
	}
	
	public class RotationTimer : Timer
	{
		private Mobile m_Attacker;
		private DateTime m_FinishTime;
		private int m_Stage;
		private TimeSpan m_SpinDuration;
		private Direction m_StartDirection;
		
		public DateTime FinishTime{ get{ return m_FinishTime; } }
		
		public RotationTimer( Mobile attacker, TimeSpan duration ) : base( TimeSpan.FromSeconds((duration.TotalSeconds)/8.0),
		TimeSpan.FromSeconds((duration.TotalSeconds)/8.0))
		{
			Priority = TimerPriority.FiftyMS;
			m_Attacker = attacker;
			m_StartDirection = attacker.Direction;
			m_SpinDuration = duration;
			m_Stage = 1;
			m_FinishTime = DateTime.Now + duration;
			m_Attacker.PlaySound( 1318 );
		}

		protected override void OnTick()
		{
			m_Attacker.Direction = (Direction)(((((int)m_StartDirection)&((int)Direction.Mask))+m_Stage)%8);
			if ( ++m_Stage > 8 )
			{
				CombatSystemAttachment.GetCSA( m_Attacker ).FinishSpinAttack();
				Stop();
			}
			else
				this.Delay = this.Interval = TimeSpan.FromSeconds((m_SpinDuration.TotalSeconds)/8.0);
		}
	}
	
	public class PushTimer : Timer
	{
		private Mobile m_Body;
		private int m_Stage;
		private TimeSpan m_Delay;
		private int m_XOffset;
		private int m_YOffset;
		private int m_ZOffset;
		private int m_Count;
		
		public int XOffset{ get{ return m_XOffset; } set{ m_XOffset = value; } }
		public int YOffset{ get{ return m_YOffset; } set{ m_YOffset = value; } }
		public int ZOffset{ get{ return m_ZOffset; } set{ m_ZOffset = value; } }
		
		public PushTimer( Mobile mob, TimeSpan delay, int count ) : base( delay, delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Body = mob;
			m_Stage = 0;
			m_Delay = delay;
			m_Count = count;
		}

		protected override void OnTick()
		{
			Point3D newLoc = new Point3D();
			newLoc.X = m_Body.Location.X + m_XOffset;
			newLoc.Y = m_Body.Location.Y + m_YOffset;
			newLoc.Z = m_Body.Location.Z + m_ZOffset;

			if ( m_Body.Map.CanSpawnMobile( newLoc ) )
				m_Body.SetLocation( newLoc, true );
			
			if ( ++m_Stage >= m_Count )
			{
				Stop();
				CombatSystemAttachment.GetCSA( m_Body ).PerformingSequence = false;
				CombatSystemAttachment.GetCSA( m_Body ).PushTimer = null;
			}
			else
				this.Delay = this.Interval = m_Delay;
		}
	}
	
	public class DefenseTimer : Timer
	{
		private Mobile m_Defender;
		private DefenseType m_Type;
		private int m_Parries;
		
		public int Parries 
		{ 
			get{ return m_Parries; } 
			set
			{ 
				m_Parries = value;
			}
	 	}
		public DefenseType Type { get { return m_Type; } }
		
		public DefenseTimer( Mobile defender, DefenseType type, TimeSpan delay ) : base( delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Defender = defender;
			m_Type = type;
		}
		
		public void Interrupt()
		{
			CombatSystemAttachment.ResetAnimation( m_Defender );
			Stop();
		}

		protected override void OnTick()
		{
			CombatSystemAttachment.GetCSA( m_Defender ).FinishDefense();
		}
	}
	
	public class QueuedActionTimer : Timer
	{
		private Mobile m_Body;
		private int m_Action;
		
		public int Action{ get{ return m_Action; } set{ m_Action = value; } }
		public QueuedActionTimer( Mobile body, int action ) : base( TimeSpan.MaxValue )
		{
			Priority = TimerPriority.FiftyMS;
			m_Body = body;
			m_Action = action;
		}
		
		public void Force()
		{
			Stop();
			Perform();
		}
		
		public void Perform()
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			csa.QueuedActionTimer = null;
			if ( m_Action == 999 ) // ranged
			{
				csa.BeginRangedAttack();
			}
			else if ( m_Action < 100 ) // attack
			{
				csa.BeginAttack( (AttackType)m_Action );
			}
			else if ( m_Action >= 100 ) // defense
			{
				m_Action -= 100;
				csa.BeginDefense( (DefenseType)m_Action );
			}
		}

		protected override void OnTick()
		{
			Perform();
		}
	}
	
	public class ChargeTimer : Timer
	{
		private Mobile m_Charger;
		public ChargeTimer( Mobile charger, TimeSpan delay ) : base( delay )
		{
			Priority = TimerPriority.FiftyMS;
			m_Charger = charger;
		}

		protected override void OnTick()
		{
			m_Charger.SendMessage( "You've waited too long, your charge attack has expired." );
			CombatSystemAttachment.GetCSA( m_Charger ).CancelCharge();
		}
	}
	
	public class AnimationTimer : Timer
	{
		private Mobile m_Body;
		private int m_Action;
		private int m_FrameCount;
		private int m_RepeatCount;
		private bool m_Forward;
		private bool m_Repeat;
		private int m_Delay;
		
		public AnimationTimer( Mobile body, int action, int framecount, int repeatcount, bool forward, bool repeat, int delay ) :
		base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
			Priority = TimerPriority.OneSecond;
			m_Body = body; m_Action = action; m_FrameCount = framecount; m_RepeatCount = repeatcount; 
			m_Forward = forward; m_Repeat = repeat; m_Delay = delay;
		}
		
		protected override void OnTick()
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Body );
			if ( csa.PerformingSequence || csa.Aiming || csa.DefensiveFormation )
			{
				RefreshAnimation();
				this.Delay = this.Interval = TimeSpan.FromSeconds( 1.0 );
			}
			else if ( csa.AnimationTimer != null )
				csa.AnimationTimer = null;
		}
		
		public void RefreshAnimation()
		{
			CombatSystemAttachment.GetCSA( m_Body ).Animate( m_Action, m_FrameCount, m_RepeatCount, m_Forward, m_Repeat, m_Delay );
		}
		
		public void LocalAnimationRefresh( Mobile m ) // refreshes the animation for a specific mobile only to reduce lag
		{
			if ( m.NetState == null )
				return;
			Packet p = Packet.Acquire( new MobileAnimation( m_Body, m_Action, m_FrameCount, m_RepeatCount, m_Forward, m_Repeat, m_Delay ) );
			m.NetState.Send( p );
			Packet.Release( p );
		}
		
		public void Cancel( bool resetAnim )
		{
			Stop();
			if ( resetAnim )
				CombatSystemAttachment.ResetAnimation( m_Body );
		}
	}
}
