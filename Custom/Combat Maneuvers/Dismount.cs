using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class Dismount : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 10; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Dismount; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( defender.Mounted )
				if( ((IKhaerosMobile)attacker).CanUseMartialPower && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                	attacker.Emote( "*grabs {0} and tries to pull {1} off of {2} mount*", defender.Name, ((IKhaerosMobile)defender).GetReflexivePronoun(), ((IKhaerosMobile)defender).GetPossessivePronoun() );
			
			else
            {
                attacker.SendMessage( 60, "This attack only works on mounted opponents." );
                ((IKhaerosMobile)attacker).DisableManeuver();
            }
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			DismountCheck( attacker, defender, 20, ((BaseWeapon)attacker.Weapon).Skill, FeatLevel );
		}
		
		public static void DismountCheck( Mobile attacker, Mobile defender, int disbonus, SkillName skill, int featlevel )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			int dischance = disbonus + (int)(Math.Max( attacker.Skills[skill].Base - defender.Skills[SkillName.Riding].Base - defplayer.RideBonus, 10 * featlevel - defplayer.RideBonus ));

            if( dischance > Utility.RandomMinMax( 1, 100 ) )
            	Effect( defender, featlevel );
		}
		
		public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			IMount mount = defender.Mount;

            if( mount != null )
                mount.Rider = null;
            
            if( defplayer.DismountedTimer != null )
				defplayer.DismountedTimer.Stop();
		
			defplayer.DismountedTimer = new DismountTimer( defender, featlevel );
			defplayer.DismountedTimer.Start();
		}
		
		public Dismount()
		{
		}
		
		public Dismount( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Dismount) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Dismount);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Dismount", AccessLevel.Player, new CommandEventHandler( Dismount_OnCommand ) );
		}
		
		[Usage( "Dismount" )]
        [Description( "Allows the user to attempt to Dismount their opponent." )]
        private static void Dismount_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Dismount) ) && m.CanUseMartialPower )
            	m.ChangeManeuver( new Dismount( m.Feats.GetFeatLevel(FeatList.Dismount) ), FeatList.Dismount, "You prepare an attack to Dismount your enemy." );
        }
        
        public class DismountTimer : Timer
        {
            private Mobile m_from;

            public DismountTimer( Mobile from, int featlevel )
                : base( TimeSpan.FromSeconds( featlevel * 5 ) )
            {
                m_from = from;
                from.SendMessage( 60, "You have been dismounted." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
            	
                m_from.SendMessage( "You may get on your mount again." );
                ((IKhaerosMobile)m_from).DismountedTimer = null;
            }
        }
	}
}
