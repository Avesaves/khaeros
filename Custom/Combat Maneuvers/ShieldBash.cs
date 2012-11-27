using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class ShieldBash : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.ShieldBash; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			/*IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
            if( attacker.FindItemOnLayer( Layer.TwoHanded ) is BaseShield )
            	if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
					attacker.Emote( "*attempts to bash {0} with {1} shield*", defender.Name, ((IKhaerosMobile)attacker).GetPossessivePronoun() );

            else
            {
                attacker.SendMessage( 60, "You need to be equipping a shield in order to perform this attack." );
                featuser.DisableManeuver();
            }*/
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( attacker, defender, FeatLevel );
		}
		
		public static void Effect( Mobile attacker, Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.DazedTimer != null )
				defplayer.DazedTimer.Stop();
			
			defplayer.DazedTimer = new ShieldBashTimer( defender, featlevel );
			defplayer.DazedTimer.Start();
			
			if( defender.Combatant != null && attacker != null )
			{
				if( defender is BaseCreature )
					((BaseCreature)defender).FocusMob = attacker;
				
				defender.Combatant = attacker;
			}
		}
		
		public ShieldBash()
		{
		}
		
		public ShieldBash( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ShieldBash) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ShieldBash);
				return true;
			}
			
			return false;
		}
		
		/*public static void Initialize() 
		{
			CommandSystem.Register( "ShieldBash", AccessLevel.Player, new CommandEventHandler( ShieldBash_OnCommand ) );
		}
		
		[Usage( "ShieldBash" )]
        [Description( "Allows the user to attempt a Shield Bash." )]
        private static void ShieldBash_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.ShieldBash) ) )
            {
                if( m.FindItemOnLayer( Layer.TwoHanded ) is BaseShield )
                    m.ChangeManeuver( new ShieldBash( m.Feats.GetFeatLevel(FeatList.ShieldBash) ), FeatList.ShieldBash, "You prepare a Shield Bash." );

                else
                    m.SendMessage( 60, "You need to be equipping a shield in order to perform this attack." );
            }
        }*/
        
        public class ShieldBashTimer : Timer
        {
            private Mobile m_from;

            public ShieldBashTimer( Mobile from, int featlevel )
            	: base( TimeSpan.FromSeconds(featlevel * 3) )
            {
                m_from = from;
                from.SendMessage( 60, "You have been dazed." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
                
                m_from.SendMessage( 60, "You are no longer dazed" );
            	((IKhaerosMobile)m_from).DazedTimer = null;
            }
        }
	}
}
