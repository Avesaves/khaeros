using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using System.Collections;
using Server.Network;

namespace Server.Misc
{
	public class Backstab : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Backstab; } }
		private bool m_WasHidden = false;
		public bool WasHidden{ set{ m_WasHidden = value; } get{ return m_WasHidden; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( ValidateBackstab( attacker, defender ) )
			{
				if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
				{
					attacker.Emote( "*surprises {0}, landing a vicious blow to {1} neck*", defender.Name, ((IKhaerosMobile)defender).GetPossessivePronoun() );
					defender.FixedParticles( 0x37B9, 1, 5, 0x251D, 0x651, 0, EffectLayer.Waist );
				}
			}
			else
			{
				attacker.SendMessage( "You must be hidden, at the back or back flank of your opponent, and using a knife, sword or axe in order to perform this attack!" );
				((IKhaerosMobile)attacker).DisableManeuver();
			}
		}
		
		public bool ValidateBackstab( Mobile attacker )
		{
			return ( (attacker.Hidden || m_WasHidden) && ( attacker.Weapon is BaseSword || attacker.Weapon is BaseKnife || attacker.Weapon is BaseAxe ) );
		}
		
		public bool ValidateBackstab( Mobile attacker, Mobile defender )
		{
			if ( ValidateBackstab( attacker ) )
			{
				string pos = BaseWeapon.GetPosition( attacker, defender, true ); // assume correct direction
				if ( pos == "back" || pos == "back flank" )
					return true;
				else
					return false;
			}
			else
				return false;
		}
		
		public Backstab()
		{
		}
		
		public Backstab( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Backstab) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Backstab);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Backstab", AccessLevel.Player, new CommandEventHandler( Backstab_OnCommand ) );
		}
		
		[Usage( "Backstab" )]
        [Description( "Allows the user to attempt a Backstab." )]
        private static void Backstab_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Backstab) ) )
			{
				if ( m.Weapon is BaseSword || m.Weapon is BaseKnife || m.Weapon is BaseAxe )
					m.ChangeManeuver( new Backstab( m.Feats.GetFeatLevel(FeatList.Backstab) ), FeatList.Backstab, "You prepare a Backstab." );
				else
					m.SendMessage( "You can't perform this attack with your current weapon." );
			}
        }
	}
}
