using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using System.Collections;

namespace Server.Misc
{
	public class HolyStrike : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.HolyStrike; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*channels divine energy with which to strike {0}*", defender.Name );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( attacker, defender, FeatLevel );
		}
		
		public static void Effect( Mobile attacker, Mobile defender, int featlevel )
		{
			if( !BaseCustomSpell.HasEnoughMana( attacker, ( featlevel * 5 ) ) )
    			return;
    		
    		ArrayList list = new ArrayList();
    		IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
			foreach( Mobile m in attacker.GetMobilesInRange( 5 ) )
            	list.Add( m );
		
			for( int i = 0; i < list.Count; ++i )
        	{
                Mobile m = (Mobile)list[i];
                
                if( ((IKhaerosMobile)attacker).IsAllyOf( m ) )
                {
					if( m == null || m.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.CanSee( m ) )
						continue;
					
					if( attacker.InLOS( m ) && m.Hits < m.HitsMax )
					{
						double heal = ( 0.10 * featlevel ) * attacker.Skills[SkillName.Faith].Base;
			
						if( m is PlayerMobile && attacker is PlayerMobile && ((PlayerMobile)m).ChosenDeity != ChosenDeity.None &&
						   ((PlayerMobile)m).ChosenDeity == ((PlayerMobile)attacker).ChosenDeity &&
						   ((PlayerMobile)m).Backgrounds.BackgroundDictionary[BackgroundList.Faithful].Level > 0 )
							heal = heal + ( heal * 0.1 );
						
						m.PlaySound( 0x1F2 );
						m.FixedEffect( 0x376A, 9, 32 );
						m.Hits += Convert.ToInt32( heal );
						attacker.Mana -= ( 5 * featlevel );
						m.LocalOverheadMessage( Network.MessageType.Regular, 170, false, "+" + Convert.ToInt32( heal ) );
						
						if( attacker.Target != null && attacker.Target is BaseCustomSpell.CustomSpellTarget )
							attacker.Target = null;
						
						break;
					}
				}
			}
		}
		
		public HolyStrike()
		{
		}
		
		public HolyStrike( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.HolyStrike) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.HolyStrike);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "HolyStrike", AccessLevel.Player, new CommandEventHandler( HolyStrike_OnCommand ) );
		}
		
		[Usage( "HolyStrike" )]
        [Description( "Allows the user to attempt a Holy Strike." )]
        private static void HolyStrike_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.HolyStrike) ) )
            	m.ChangeManeuver( new HolyStrike( m.Feats.GetFeatLevel(FeatList.HolyStrike) ), FeatList.HolyStrike, "You prepare a Holy Strike." );
        }
	}
}
