using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;
using Server.Mobiles;

namespace Server.Commands
{
	public class CombatAlerts
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "CombatAlerts", AccessLevel.Player, new CommandEventHandler( CombatAlerts_OnCommand ) );
		}

		[Usage( "CombatAlerts" )]
		[Description( "Enables you to toggle combat alerts between relevant only and all." )]
		private static void CombatAlerts_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

			if ( !csa.AlwaysDisplayIcons )
			{
				csa.AlwaysDisplayIcons = true;
				mob.SendMessage( "All combat alerts will be displayed from now on." );
			}
			else
			{
				csa.AlwaysDisplayIcons = false;
				mob.SendMessage( "Only relevant combat alerts will be displayed from now on." );
			}
		}
	}
	
	public class CombatQueue
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "CombatQueue", AccessLevel.Player, new CommandEventHandler( CombatQueue_OnCommand ) );
		}

		[Usage( "CombatQueue" )]
		[Description( "Enables you to toggle combat queue on and off." )]
		private static void CombatQueue_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

			if ( !csa.Queue )
			{
				csa.Queue = true;
				mob.SendMessage( "Combat queue enabled." );
			}
			else
			{
				csa.Queue = false;
				mob.SendMessage( "Combat queue disabled." );
			}
		}
	}
	public class Attack
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Attack", AccessLevel.Player, new CommandEventHandler( Attack_OnCommand ) );
		}

		[Usage( "Attack" )]
		[Description( "Allows you to attack with your weapon in the specified manner." )]
		private static void Attack_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			AttackType attacktype = AttackType.Invalid;
			string str = e.ArgString.Trim();
			if ( String.Compare( str, "swing", true ) == 0 )
				attacktype = AttackType.Swing;
			else if ( String.Compare( str, "thrust", true ) == 0 )
				attacktype = AttackType.Thrust;
			else if ( String.Compare( str, "overhead", true ) == 0 )
				attacktype = AttackType.Overhead;
			else if ( String.Compare( str, "circular", true ) == 0 )
				attacktype = AttackType.Circular;
			else if ( String.Compare( str, "shieldbash", true ) == 0 )
				attacktype = AttackType.ShieldBash;
			else
			{
				mob.SendMessage( "Usage: Attack swing" );
				mob.SendMessage( "Usage: Attack thrust" );
				mob.SendMessage( "Usage: Attack overhead" );
				return;
			}
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

			if ( !csa.CruiseControl )
			{
				if ( DateTime.Now >= csa.NextAttackAction ) // if we can act, let's act
				{
					if ( !csa.BeginAttack( attacktype, true ) )
					{
						if ( csa.ErrorMessage != "" )
							mob.SendMessage( 30, csa.ErrorMessage );
					}
				}
				else if ( csa.Queue )
					csa.DisplayQueueResultMessage( csa.QueueAction( attacktype ) );
			}
			else
			{
				csa.DisplayQueueResultMessage( csa.QueueAction( attacktype ) );
			}
		}
	}
	
	public class Parry
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Parry", AccessLevel.Player, new CommandEventHandler( Parry_OnCommand ) );
		}

		[Usage( "Parry" )]
		[Description( "Allows you to parry with your weapon or shield in the specified manner." )]
		private static void Parry_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			DefenseType defensetype = DefenseType.Invalid;
			string str = e.ArgString.Trim();
			if ( String.Compare( str, "swing", true ) == 0 )
				defensetype = DefenseType.ParrySwing;
			else if ( String.Compare( str, "thrust", true ) == 0 )
				defensetype = DefenseType.ParryThrust;
			else if ( String.Compare( str, "overhead", true ) == 0 )
				defensetype = DefenseType.ParryOverhead;
			else
			{
				mob.SendMessage( "Usage: Parry swing" );
				mob.SendMessage( "Usage: Parry thrust" );
				mob.SendMessage( "Usage: Parry overhead" );
				return;
			}
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

			if ( !csa.CruiseControl )
			{
				if ( DateTime.Now >= csa.NextDefenseAction ) // if we can act, let's act
				{
					if ( !csa.BeginDefense( defensetype, true ) )
					{
						if ( csa.ErrorMessage != "" )
							mob.SendMessage( 30, csa.ErrorMessage );
					}
				}
				else if ( csa.Queue )
					csa.DisplayQueueResultMessage( csa.QueueAction( defensetype ) );
			}
			else
			{
				csa.DisplayQueueResultMessage( csa.QueueAction( defensetype ) );
			}
		}
	}
	
	public class DefensiveFormation
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "DefensiveFormation", AccessLevel.Player, new CommandEventHandler( DefensiveFormation_OnCommand ) );
		}

		[Usage( "DefensiveFormation" )]
		[Description( "Allows you to point the polearm out towards the enemy." )]
		private static void DefensiveFormation_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			if ( !csa.BeginDefensiveFormation() )
				if ( csa.ErrorMessage != "" )
					mob.SendMessage( 30, csa.ErrorMessage );
		}
	}
	
	public class Charge
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Charge", AccessLevel.Player, new CommandEventHandler( Charge_OnCommand ) );
		}

		[Usage( "Charge" )]
		[Description( "Allows you to attempt a charge attack." )]
		private static void Charge_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

			if ( !csa.BeginCharging() )
			{
				if ( csa.ErrorMessage != "" )
					mob.SendMessage( 30, csa.ErrorMessage );
			}
			else
				mob.SendMessage( "You begin your charge! You have " + csa.GetChargeTimeWindow() + " seconds to reach your opponent!" );
		}
	}
	
	public class Ranged
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Ranged", AccessLevel.Player, new CommandEventHandler( Ranged_OnCommand ) );
		}

		[Usage( "Ranged" )]
		[Description( "Allows you to attempt a ranged attack." )]
		private static void Ranged_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
					
			if ( !csa.CruiseControl )
			{
				if ( DateTime.Now >= csa.NextAttackAction ) // if we can act, let's act
				{
					if ( !csa.BeginRangedAttack() )
					{
						if ( csa.ErrorMessage != "" )
							mob.SendMessage( 30, csa.ErrorMessage ); // there's a hack in place there, which will queue if we're not still
					}
				}
				else
					csa.DisplayQueueResultMessage( csa.QueueRanged() );
			}
			else
			{
				csa.DisplayQueueResultMessage( csa.QueueRanged() );
			}
		}
	}
	
	public class Throw
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Throw", AccessLevel.Player, new CommandEventHandler( Throw_OnCommand ) );
		}

		[Usage( "Throw" )]
		[Description( "Allows you to throw your weapon at your opponent." )]
		private static void Throw_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			
			if( mob is PlayerMobile )
				((PlayerMobile)mob).EnableOffHand = false;
					
			if ( !csa.CruiseControl )
			{
				if ( DateTime.Now >= csa.NextAttackAction ) // if we can act, let's act
				{
					if ( !csa.BeginAttack( AttackType.Throw, true ) )
					{
						if ( csa.ErrorMessage != "" )
							mob.SendMessage( 30, csa.ErrorMessage );
					}
				}
				else
					csa.DisplayQueueResultMessage( csa.QueueAction( AttackType.Throw ) );
			}
			else
			{
				csa.DisplayQueueResultMessage( csa.QueueAction( AttackType.Throw ) );
			}
		}
	}
	
	public class OffHandThrow
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "OffHandThrow", AccessLevel.Player, new CommandEventHandler( OffHandThrow_OnCommand ) );
		}
		
		public static bool CheckForFreeHand( Mobile m )
		{
			Item weapon = m.FindItemOnLayer( Layer.FirstValid ) as Item;
            Item shield = m.FindItemOnLayer( Layer.TwoHanded ) as Item;
			
            if( (weapon == null && (shield == null || !(shield is BaseWeapon))) || shield == null )
            	return true;

            m.SendMessage( "You need a free hand for that." );
            return false;
		}

        public static bool EnableOffHand( PlayerMobile m )
        {
            if( !CheckForFreeHand( m ) )                
                return false;

            if( m.CraftContainer != null && !m.CraftContainer.Deleted && m.Backpack != null && !m.Backpack.Deleted && m.CraftContainer.IsChildOf( m.Backpack ) )
            {
                foreach( Item item in m.CraftContainer.Items )
                {
                    if( item is Dagger || item is ThrowingAxe )
                    {
                        m.OffHandWeapon = (BaseWeapon)item;
                        break;
                    }
                }
            }

            if( m.OffHandWeapon == null )
            {
                m.SendMessage( "Please set up a container in your backpack with .CraftContainer, then drop a dagger or a throwing axe inside it." );
                return false;
            }

            return true;
        }

		[Usage( "OffHandThrow" )]
		[Description( "Allows you to throw your off-hand weapon at your opponent." )]
		private static void OffHandThrow_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			PlayerMobile m = mob as PlayerMobile;
			
			if( !CheckForFreeHand(m) )
				return;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );

            if( mob is PlayerMobile )
                m.EnableOffHand = true;
	
			if ( !csa.CruiseControl )
			{
				if ( DateTime.Now >= csa.NextAttackAction ) // if we can act, let's act
				{
					if ( !csa.BeginAttack( AttackType.Throw, true ) )
					{
						if ( csa.ErrorMessage != "" )
							mob.SendMessage( 30, csa.ErrorMessage );
					}
				}
				else
					csa.DisplayQueueResultMessage( csa.QueueAction( AttackType.Throw ) );
			}
			else
			{
				csa.DisplayQueueResultMessage( csa.QueueAction( AttackType.Throw ) );
			}
		}
	}
	
	public class AutoCombat
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "AutoCombat", AccessLevel.Player, new CommandEventHandler( AutoCombat_OnCommand ) );
		}

		[Usage( "AutoCombat" )]
		[Description( "Enables or disables automatic combat." )]
		private static void AutoCombat_OnCommand( CommandEventArgs e )
		{
            if (HealthAttachment.HasHealthAttachment(e.Mobile))
            {
                if (HealthAttachment.GetHA(e.Mobile).HasInjury(Injury.InternalBleeding))
                    return;
            }

			Mobile mob = e.Mobile;
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			
			if ( e.Length == 0 ) // this is enable/disable
			{
				if ( csa.CruiseControl )
				{
					mob.SendMessage( "AutoCombat disabled." );
					if (Utility.RandomDouble() == 0.01)
						mob.SendMessage( "\"(You just committed robocide.)\"" );
					csa.DisableAutoCombat();
				}
				else
				{
					mob.SendMessage( "AutoCombat enabled." );
					csa.EnableAutoCombat();
				}
			}
			else if ( e.Length == 1 )
			{
				string arg = e.Arguments[0].ToLower();
				if ( String.Compare( arg, "waitforaimedshot", true ) == 0 )
				{
					if ( csa.ACWaitForAimedShot )
					{
						mob.SendMessage( "AutoCombat will no longer wait for the aimed shot bonus." );
						csa.ACWaitForAimedShot = false;
					}
					else
					{
						mob.SendMessage( "AutoCombat will now wait for the aimed shot bonus." );
						csa.ACWaitForAimedShot = true;
					}
					
					if ( csa.CruiseControl )
					{
						csa.DisableAutoCombat();
						csa.EnableAutoCombat();
					}
					return;
				}
			}
			else if ( e.Length == 2 ) // this is "attack|parry argument"
			{
				string type = e.Arguments[0].ToLower();
				string arg = e.Arguments[1].ToLower();
				
				if ( String.Compare( type, "parry", true ) == 0 ) // setting parry options
				{
					if ( String.Compare( arg, "all", true ) == 0 ) // all directions
					{
						csa.ACParryThrust = csa.ACParrySwing = csa.ACParryOverhead = true;
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "thrust", true ) == 0 )
					{
						csa.ACParryThrust = true;
						csa.ACParrySwing = csa.ACParryOverhead = false;
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "overhead", true ) == 0 )
					{
						csa.ACParryOverhead = true;
						csa.ACParrySwing = csa.ACParryThrust = false;
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "swing", true ) == 0 )
					{
						csa.ACParrySwing = true;
						csa.ACParryThrust = csa.ACParryOverhead = false;
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "none", true ) == 0 )
					{
						csa.ACParryThrust = csa.ACParrySwing = csa.ACParryOverhead = false;
						csa.ACParry = false;
					}
					else
					{
						mob.SendMessage( "Valid arguments are: all, none, swing, thrust, overhead" );
						return;
					}
				}
				else if ( String.Compare( type, "attack", true ) == 0 ) // setting attack options
				{
					if ( String.Compare( arg, "all", true ) == 0 ) // all directions
					{
						csa.ACAttackThrust = csa.ACAttackSwing = csa.ACAttackOverhead = true;
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "thrust", true ) == 0 )
					{
						csa.ACAttackThrust = true;
						csa.ACAttackSwing = csa.ACAttackOverhead = false;
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "overhead", true ) == 0 )
					{
						csa.ACAttackOverhead = true;
						csa.ACAttackSwing = csa.ACAttackThrust = false;
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "swing", true ) == 0 )
					{
						csa.ACAttackSwing = true;
						csa.ACAttackThrust = csa.ACAttackOverhead = false;
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "none", true ) == 0 )
					{
						csa.ACAttackThrust = csa.ACAttackSwing = csa.ACAttackOverhead = false;
						csa.ACAttack = false;
					}
					else
					{
						mob.SendMessage( "Valid arguments are: all, none, swing, thrust, overhead" );
						return;
					}
				}
				else
				{
					mob.SendMessage( ".autocombat parry <arguments>" );
					mob.SendMessage( ".autocombat attack <arguments>" );
					return;
				}
			}
			else if ( e.Length == 3 )
			{
				string type = e.Arguments[0].ToLower();
				string arg = e.Arguments[1].ToLower();
				string arg2 = e.Arguments[2].ToLower();
				
				if ( String.Compare( type, "parry", true ) == 0 ) // setting parry options
				{
					if ( String.Compare( arg, "thrust", true ) == 0 )
					{
						csa.ACParryThrust = true;
						if ( String.Compare( arg2, "swing", true ) == 0 )
						{
							csa.ACParryOverhead = false;
							csa.ACParrySwing = true;
						}
						else if ( String.Compare( arg2, "overhead", true ) == 0 )
						{
							csa.ACParrySwing = false;
							csa.ACParryOverhead = true;
						}
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "overhead", true ) == 0 )
					{
						csa.ACParryOverhead = true;
						if ( String.Compare( arg2, "swing", true ) == 0 )
						{
							csa.ACParryThrust = false;
							csa.ACParrySwing = true;
						}
						else if ( String.Compare( arg2, "thrust", true ) == 0 )
						{
							csa.ACParrySwing = false;
							csa.ACParryThrust = true;
						}
						csa.ACParry = true;
					}
					else if ( String.Compare( arg, "swing", true ) == 0 )
					{
						csa.ACParrySwing = true;
						if ( String.Compare( arg2, "overhead", true ) == 0 )
						{
							csa.ACParryThrust = false;
							csa.ACParryOverhead = true;
						}
						else if ( String.Compare( arg2, "thrust", true ) == 0 )
						{
							csa.ACParryOverhead = false;
							csa.ACParryThrust = true;
						}
						csa.ACParry = true;
					}
					else
					{
						mob.SendMessage( "Valid arguments are: swing, thrust, overhead" );
						return;
					}
				}
				else if ( String.Compare( type, "attack", true ) == 0 ) // setting attack options
				{
					if ( String.Compare( arg, "thrust", true ) == 0 )
					{
						csa.ACAttackThrust = true;
						if ( String.Compare( arg2, "swing", true ) == 0 )
						{
							csa.ACAttackOverhead = false;
							csa.ACAttackSwing = true;
						}
						else if ( String.Compare( arg2, "overhead", true ) == 0 )
						{
							csa.ACAttackSwing = false;
							csa.ACAttackOverhead = true;
						}
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "overhead", true ) == 0 )
					{
						csa.ACAttackOverhead = true;
						if ( String.Compare( arg2, "swing", true ) == 0 )
						{
							csa.ACAttackThrust = false;
							csa.ACAttackSwing = true;
						}
						else if ( String.Compare( arg2, "thrust", true ) == 0 )
						{
							csa.ACAttackSwing = false;
							csa.ACAttackThrust = true;
						}
						csa.ACAttack = true;
					}
					else if ( String.Compare( arg, "swing", true ) == 0 )
					{
						csa.ACAttackSwing = true;
						if ( String.Compare( arg2, "overhead", true ) == 0 )
						{
							csa.ACAttackThrust = false;
							csa.ACAttackOverhead = true;
						}
						else if ( String.Compare( arg2, "thrust", true ) == 0 )
						{
							csa.ACAttackOverhead = false;
							csa.ACAttackThrust = true;
						}
						csa.ACAttack = true;
					}
					else
					{
						mob.SendMessage( "Valid arguments are: swing, thrust, overhead" );
						return;
					}
				}
				else
				{
					mob.SendMessage( ".autocombat parry <arguments>" );
					mob.SendMessage( ".autocombat attack <arguments>" );
					return;
				}
			}
			else
			{
				mob.SendMessage( ".autocombat" );
				mob.SendMessage( ".autocombat parry <arguments>" );
				mob.SendMessage( ".autocombat attack <arguments>" );
				return;
			}
			
			if ( csa.CruiseControl )
			{
				csa.DisableAutoCombat();
				csa.EnableAutoCombat();
				string directions = "";
				if ( csa.ACAttack )
				{
					directions = "Attack directions:";
					if ( csa.ACAttackOverhead )
						directions += " overhead";
					if ( csa.ACAttackSwing )
						directions += " swing";
					if ( csa.ACAttackThrust )
						directions += " thrust";
				}
				else
					directions = "Attacking is disabled";
				directions += ". ";
				if ( csa.ACParry )
				{
					directions += "Parry directions:";
					if ( csa.ACParryOverhead )
						directions += " overhead";
					if ( csa.ACParrySwing )
						directions += " swing";
					if ( csa.ACParryThrust )
						directions += " thrust";
				}
				else
					directions += "Parry is disabled.";
					
				mob.SendMessage( directions );
			}
		}
	}
	
	public class CheckSpeed
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "CheckSpeed", AccessLevel.Player, new CommandEventHandler( CheckSpeed_OnCommand ) );
		}

		[Usage( "CheckSpeed" )]
		[Description( "Displays (modified) weapon speed in seconds." )]
		private static void CheckSpeed_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon != null )
				mob.SendMessage( weapon.GetDelay( mob ).TotalSeconds + "s" );
		}
	}
	
	public class CheckDPS
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "CheckDPS", AccessLevel.Administrator, new CommandEventHandler( CheckDPS_OnCommand ) );
		}

		[Usage( "CheckDPS" )]
		[Description( "" )]
		private static void CheckDPS_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon != null )
			{
				int dmg = (int)(((double)(Math.Max( (int)weapon.ScaleDamageAOS( mob, null, weapon.MinDamage, true, true ), 1 ) +
					Math.Max( (int)weapon.ScaleDamageAOS( mob, null, weapon.MaxDamage, true, false ), 1 ))) / 2.0); 
				mob.SendMessage( ((double)dmg)/(double)(csa.ComputeNextSwingTime() ).TotalSeconds + " DPS" );
			}
		}
	}
	
	public class CheckDPS1
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "CheckDPS1", AccessLevel.Administrator, new CommandEventHandler( CheckDPS1_OnCommand ) );
		}

		[Usage( "CheckDPS1" )]
		[Description( "" )]
		private static void CheckDPS1_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
			if ( weapon != null )
			{
				int dmg = (int)(((double)(Math.Max( (int)weapon.ScaleDamageAOS( mob, null, weapon.MinDamage, true, true ), 1 ) +
					Math.Max( (int)weapon.ScaleDamageAOS( mob, null, weapon.MaxDamage, true, false ), 1 ))) / 2.0); 
				mob.SendMessage( ((double)dmg)/(double)(csa.ComputeNextSwingTime() + 
					TimeSpan.FromSeconds( csa.ComputeAnimationDelay() ) ).TotalSeconds + " DPS" );
			}
		}
	}

    public class CheckKnockTime
    {
        public static void Initialize()
        {
            Register();
        }

        public static void Register()
        {
            CommandSystem.Register("CheckKnockTime", AccessLevel.Player, new CommandEventHandler(CheckKnockTime_OnCommand));
        }

        [Usage("CheckKnockTime")]
        [Description("Displays the time that will pass between your last movement and firing your first arrow.")]
        private static void CheckKnockTime_OnCommand(CommandEventArgs e)
        {
            Mobile mob = e.Mobile;
            if (mob.Weapon is BaseRanged)
            {
                mob.SendMessage("Knocking time is " + (mob.Weapon as BaseRanged).RequiredStillTime(mob).TotalSeconds + " seconds.");
            }
            else
                mob.SendMessage("You do not have a ranged weapon equipped.");
        }
    }
	
	public class ParticleTest
	{
		private class SoundTimer : Timer
		{
			int i=1;
			public SoundTimer(  ) :
			base( TimeSpan.FromSeconds( 0.4 ), TimeSpan.FromSeconds( 0.4 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			protected override void OnTick()
			{
				if ( i > 15 )
					return;
				switch( i )
				{
					case 1: mob.PlaySound( 0x4A9 ); break;
					case 2: mob.PlaySound( 0x4A9 ); break;
					case 3: mob.PlaySound( 0x4AB ); break;
					case 4: mob.PlaySound( 0x4AF ); break;
					case 5: mob.PlaySound( 0x4AF ); break;
					case 6: mob.PlaySound( 0x4AB ); break;
					case 7: mob.PlaySound( 0x4A9 ); break;
					case 8: mob.PlaySound( 0x4A5 ); break;
					case 9: mob.PlaySound( 0x4A1 ); break;
					case 10: mob.PlaySound( 0x4A1 ); break;
					case 11: mob.PlaySound( 0x4A5 ); break;
					case 12: mob.PlaySound( 0x4A9 ); break;
					case 13: mob.PlaySound( 0x4A5 ); break;
					case 14: mob.PlaySound( 0x4A1 ); break;
					case 15: mob.PlaySound( 0x4A1 ); break;
				}
			/*Timer.DelayCall( TimeSpan.FromMilliseconds( 500*1 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*2 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*3 ),
				new TimerStateCallback( Callback ), "0x4AB" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*4 ),
				new TimerStateCallback( Callback ), "0x4AF" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*5 ),
				new TimerStateCallback( Callback ), "0x4AF" );
				Timer.DelayCall( TimeSpan.FromMilliseconds( 500*6 ),
				new TimerStateCallback( Callback ), "0x4AB" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*7 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*8 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*9 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*10 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*11 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*12 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*13 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*14 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*15 ),
				new TimerStateCallback( Callback ), "0x4A1" );*/
				i++;
				//if ( i-1 == 13 )
					//this.Delay = this.Interval = TimeSpan.FromSeconds( 0.6 );
				//else
					this.Delay = this.Interval = TimeSpan.FromSeconds( 0.4 );
			}
		}
		public static Mobile mob;
		public static void Callback( object obj )
		{
			mob.PlaySound(Convert.ToInt32(obj as string, 16));
		}
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "ParticleTest", AccessLevel.Administrator, new CommandEventHandler( ParticleTest_OnCommand ) );
		}

		[Usage( "ParticleTest" )]
		[Description( "ParticleTest" )]
		private static void ParticleTest_OnCommand( CommandEventArgs e )
		{
			Mobile mob = e.Mobile;
			ParticleTest.mob = mob;
			SoundTimer timer = new SoundTimer();
			timer.Start();
			//CombatSystemAttachment.GetCSA( mob ).DoTrip( 3 );
			/*Timer.DelayCall( TimeSpan.FromMilliseconds( 500*1 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*2 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*3 ),
				new TimerStateCallback( Callback ), "0x4AB" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*4 ),
				new TimerStateCallback( Callback ), "0x4AF" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*5 ),
				new TimerStateCallback( Callback ), "0x4AF" );
				Timer.DelayCall( TimeSpan.FromMilliseconds( 500*6 ),
				new TimerStateCallback( Callback ), "0x4AB" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*7 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*8 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*9 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*10 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*11 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*12 ),
				new TimerStateCallback( Callback ), "0x4A9" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*13 ),
				new TimerStateCallback( Callback ), "0x4A5" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*14 ),
				new TimerStateCallback( Callback ), "0x4A1" );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500*15 ),
				new TimerStateCallback( Callback ), "0x4A1" );*/
			/*mob.PlaySound( 0x4A8 );
			mob.PlaySound( 0x4A8 );
			mob.PlaySound( 0x4AA );
			mob.PlaySound( 0x4AE );
			mob.PlaySound( 0x4AE );
			mob.PlaySound( 0x4AA );
			mob.PlaySound( 0x4A8 );
			mob.PlaySound( 0x4A4 );
			mob.PlaySound( 0x49F );
			mob.PlaySound( 0x49F );
			mob.PlaySound( 0x4A4 );-
			mob.PlaySound( 0x4A8 );
			mob.PlaySound( 0x4A4 );
			mob.PlaySound( 0x49F );
			mob.PlaySound( 0x49F );*/
			
			if (e.Length < 10)
				return;
			
			/*IEntity from = new Entity( Server.Serial.Zero, new Point3D( mob.X, mob.Y, mob.Z ), mob.Map );
			IEntity to = new Entity( Server.Serial.Zero, new Point3D( mob.X, mob.Y, mob.Z + 50 ), mob.Map );
			Effects.SendMovingParticles( from, to, Convert.ToInt32(e.Arguments[0]), Convert.ToInt32(e.Arguments[1]), Convert.ToInt32(e.Arguments[2]), Convert.ToBoolean(e.Arguments[3]), Convert.ToBoolean(e.Arguments[4]), Convert.ToInt32(e.Arguments[5]), Convert.ToInt32(e.Arguments[6]), Convert.ToInt32(e.Arguments[7]), Convert.ToInt32(e.Arguments[8]), Convert.ToInt32(e.Arguments[9]), EffectLayer.Head, 0x100 );
*/
			/*for ( int i=-5; i<5; i++)
			{
				for (int j=-5; j<5; j++)
				{
					Packet regular = null;
					NetState state = mob.NetState;
					if ( state != null )
					{
						IEntity entity = new Entity( Server.Serial.Zero, new Point3D( mob.X+(i*2), mob.Y+(j*2), mob.Z + 5 ), mob.Map );
						regular = new LocationEffect( entity, 8391, 5, -1, 4410, 500 );

						state.Send( regular );
					}
				}
			}	
			for ( int i=-5; i<5; i++)
			{
				for (int j=-5; j<5; j++)
				{
					Packet regular = null;
					NetState state = mob.NetState;
					if ( state != null )
					{
						IEntity entity = new Entity( Server.Serial.Zero, new Point3D( mob.X+(i*2+1), mob.Y+(j*2+1), mob.Z + 5 ), mob.Map );
						regular = new LocationEffect( entity, 8391, 5, -1, 4410, 500 );

						state.Send( regular );
					}
				}
			}*/
		}
	}
	
	/*public class BullRush
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "BullRush", AccessLevel.Player, new CommandEventHandler( BullRush_OnCommand ) );
		}
		
		
	
		[Usage( "BullRush" )]
		[Description( "Charges in a straight line and attacks people that are in the way." )]
		private static void BullRush_OnCommand( CommandEventArgs e )
		{
            PlayerMobile pm = e.Mobile as PlayerMobile;
            if( pm.Feats.GetFeatLevel(FeatList.BullRush) < 1 )
            {
                pm.SendMessage( 60, "You do not have this feat." );
                return;
            }
			else if ( pm.Mounted )
			{
				pm.SendMessage( 60, "This ability cannot be used while mounted." );
				return;
			}

			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( pm );
			
			if( !BaseWeapon.CheckStam( pm, pm.Feats.GetFeatLevel(FeatList.BullRush), false, false ) )
				return;
			
			pm.PlaySound ( (pm.Female ? 824 : 1098) );
			csa.DoBullRush();
		}
	}*/
}
