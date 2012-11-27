using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using System.Collections;
using Server.Network;

namespace Server.Misc
{
	public class BleedingStrike : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.BleedingStrike; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if ( ( attacker.Weapon is BaseSword || attacker.Weapon is BaseKnife || attacker.Weapon is BaseAxe || attacker.Weapon is BasePoleArm || attacker.Weapon is BaseSpear ) )
				if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
					attacker.Emote( "*steps into {0} and slices {1} vein*", defender.Name, ((IKhaerosMobile)defender).GetPossessivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			if ( !( attacker.Weapon is BaseSword || attacker.Weapon is BaseKnife || attacker.Weapon is BaseAxe || attacker.Weapon is BasePoleArm || attacker.Weapon is BaseSpear ) )
				attacker.SendMessage( "This attack cannot be used with that weapon." );
			if ( (defender is BaseCreature && ((BaseCreature)defender).BleedImmune) )
			{
				attacker.SendLocalizedMessage( 1062052 ); // Your target is not affected by the bleed attack!
				return;
			}

			attacker.SendLocalizedMessage( 1060159 ); // Your target is bleeding!
			defender.SendLocalizedMessage( 1060160 ); // You are bleeding!

			if ( defender is PlayerMobile )
			{
				defender.LocalOverheadMessage( MessageType.Regular, 0x21, 1060757 ); // You are bleeding profusely
				defender.NonlocalOverheadMessage( MessageType.Regular, 0x21, 1060758, defender.Name ); // ~1_NAME~ is bleeding profusely
			}

			defender.PlaySound( 0x133 );
			defender.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );

			BeginBleed( defender, attacker );
		}
		
		public BleedingStrike()
		{
		}
		
		public BleedingStrike( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if ( ( mob.Weapon is BaseSword || mob.Weapon is BaseKnife || mob.Weapon is BaseAxe || mob.Weapon is BasePoleArm || mob.Weapon is BaseSpear ) )
			{
				if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.BleedingStrike) > 0 )
				{
					this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.BleedingStrike);
					return true;
				}
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "BleedingStrike", AccessLevel.Player, new CommandEventHandler( BleedingStrike_OnCommand ) );
		}
		
		[Usage( "BleedingStrike" )]
        [Description( "Allows the user to attempt a Bleeding Strike." )]
        private static void BleedingStrike_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.BleedingStrike) ) )
			{
				if ( m.Weapon is BaseSword || m.Weapon is BaseKnife || m.Weapon is BaseAxe || m.Weapon is BasePoleArm || m.Weapon is BaseSpear )
					m.ChangeManeuver( new BleedingStrike( m.Feats.GetFeatLevel(FeatList.BleedingStrike) ), FeatList.BleedingStrike, "You prepare a Bleeding Strike." );
				else
					m.SendMessage( "You can't perform this attack with your current weapon." );
			}
        }
		
		private static Hashtable m_Table = new Hashtable();

		public static bool IsBleeding( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginBleed( Mobile m, Mobile from )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
				t.Stop();

			t = new InternalTimer( from, m );
			m_Table[m] = t;

			t.Start();
		}

		public static void DoBleed( Mobile m, Mobile from, int level )
		{
			if ( m.Alive )
			{
				int damage = Utility.RandomMinMax( level, level * ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.BleedingStrike) );

				m.PlaySound( 0x133 );
				m.Damage( damage, from );

				Blood blood = new Blood();

				blood.ItemID = Utility.Random( 0x122A, 5 );

				blood.MoveToWorld( m.Location, m.Map );
			}
			else
			{
				EndBleed( m, false );
			}
		}

		public static void EndBleed( Mobile m, bool message )
		{
			Timer t = (Timer)m_Table[m];

			if ( t == null )
				return;

			t.Stop();
			m_Table.Remove( m );

			m.SendLocalizedMessage( 1060167 ); // The bleeding wounds have healed, you are no longer bleeding!
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_Mobile;
			private int m_Count;

			public InternalTimer( Mobile from, Mobile m ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_From = from;
				m_Mobile = m;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				DoBleed( m_Mobile, m_From, 5 - m_Count );

				if ( ++m_Count == 5 )
					EndBleed( m_Mobile, true );
			}
		}
	}
}
