using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using System.Collections;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
	public class BaseCombatManeuver : object
	{
		public virtual int DamageBonus{ get{ return 0; } }
		public virtual int AccuracyBonus{ get{ return 0; } }
		public virtual bool Melee{ get{ return true; } }
		public virtual bool Ranged{ get{ return true; } }
		public virtual bool Throwing{ get{ return true; } }
		public virtual FeatList ListedName{ get{ return FeatList.None; } }
		
		private int m_FeatLevel;
		public int FeatLevel{ get{ return m_FeatLevel; } set{ m_FeatLevel = value; } }
		
		public virtual void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
		}
		
		public virtual void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public virtual bool CanUseThisManeuver( Mobile mob )
		{
			if( mob != null && !mob.Deleted && mob is IKhaerosMobile && mob.Alive && CanUseManeuver( mob, ((BaseWeapon)mob.Weapon).Skill ) )
				return true;
			
			return false;
		}
		
		public virtual void CanUseManeuverCheck( Mobile attacker, SkillName skill )
		{
			if( !CanUseManeuver( attacker, skill ) )
			{
				((IKhaerosMobile)attacker).DisableManeuver();
				attacker.SendMessage( 60, "You cannot perform the chosen attack with your current weapon." );
			}
		}
		
		public virtual bool CanUseManeuver( Mobile attacker, SkillName skill )
		{
			if( attacker.Weapon is BaseRanged && !Ranged )
				return false;
			
			if( !(attacker.Weapon is BaseRanged) && !Melee )
				return false;
			
			if( skill == SkillName.Throwing && !Throwing )
				return false;
			
			return true;
		}
		
		public BaseCombatManeuver( int featlevel )
		{
			m_FeatLevel = featlevel;
		}
		
		public BaseCombatManeuver() : this( 0 )
		{
		}
		
		public static void Cleave( Mobile mob, Mobile killed )
		{
			if ( mob == null || killed == null || ((IKhaerosMobile)mob).CleaveAttack )
                return;
			
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( mob );
			if ( csa.Charging || (csa.AttackTimer != null && csa.AttackTimer.Type == AttackType.Throw) ) // does not trigger from charge/throw
				return;
			BaseWeapon weapon = mob.Weapon as BaseWeapon;
            IKhaerosMobile attacker = mob as IKhaerosMobile;
            ArrayList list = new ArrayList();

            if ( attacker.Feats.GetFeatLevel(FeatList.Cleave) > 0 && !( mob.Weapon is BaseRanged ) )
            {
                foreach( Mobile m in mob.GetMobilesInRange( 2 ) )
                    list.Add( m );

                ArrayList targets = new ArrayList();

                for( int i = 0; i < list.Count; ++i )
                {
                    Mobile m = (Mobile)list[i];

                    if( m != killed && m != mob && Spells.SpellHelper.ValidIndirectTarget( mob, m ) )
                    {
                    	if( m == null || m.Deleted || m.Map != mob.Map || !m.Alive || !mob.CanSee( m ) || !mob.CanBeHarmful( m ) || BaseAI.AreAllies( mob, m ) )
                            continue;

                        if( !mob.InRange( m, weapon.MaxRange ) )
                            continue;

                        if (mob.InLOS(m))
                        {
                            bool govAlly = false;
                            if (m is Soldier && (m as Soldier).Government != null && !(m as Soldier).Government.Deleted)
                            {
                                if (attacker is Soldier && (attacker as Soldier).Government != null && !(attacker as Soldier).Government.Deleted)
                                {
                                    if ((attacker as Soldier).Government == (m as Soldier).Government)
                                        govAlly = true;
                                    else if ((attacker as Soldier).Government.AlliedGuilds.Contains((m as Soldier).Government))
                                        govAlly = true;
                                }
                                else if (attacker is PlayerMobile)
                                {
                                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                                    {
                                        if (CustomGuildStone.IsGuildMember(attacker as PlayerMobile, g))
                                            govAlly = (g == (m as Soldier).Government || g.AlliedGuilds.Contains((m as Soldier).Government));

                                        if (govAlly)
                                            break;
                                    }

                                    if (govAlly)
                                        continue;
                                }
                            }
                            else if (m is PlayerMobile && attacker is Soldier)
                            {
                                if ((attacker as Soldier).Government.Members.Contains(m))
                                    govAlly = true;
                                else
                                {
                                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                                    {
                                        if (CustomGuildStone.IsGuildMember(m as PlayerMobile, g))
                                            govAlly = (attacker as Soldier).Government.AlliedGuilds.Contains(g);

                                        if(govAlly)
                                            break;
                                    }
                                }
                            }

                            if(!govAlly)
                                targets.Add(m);
                        }
                    }
                }

                if( targets.Count > 0 )
                {
                    mob.RevealingAction();
                    Mobile m = (Mobile)targets[0];
                    mob.Emote( "*unleashes a cleave attack on {0}*", m.Name ); 
                    attacker.CleaveAttack = true;
                    weapon.OnSwing( mob, m, ((attacker.Feats.GetFeatLevel(FeatList.Cleave) * 0.25) + 0.25), true );
                }
            }
		}
		
		public class ManeuverBonusTimer : Timer
        {
            private Mobile mob;

            public ManeuverBonusTimer( Mobile from )
            	: base( TimeSpan.FromSeconds( 5 ) )
            {
            	mob = from;
            }

            protected override void OnTick()
            {
            	if( mob != null )
            	{
            		((IKhaerosMobile)mob).ManeuverAccuracyBonus = 0;
            		((IKhaerosMobile)mob).ManeuverDamageBonus = 0;
            		((IKhaerosMobile)mob).ManeuverBonusTimer = null;
            		((IKhaerosMobile)mob).CombatManeuver = null;
            		((IKhaerosMobile)mob).OffensiveFeat = FeatList.None;
            		
            		if( mob is PlayerMobile )
            			mob.Send( new Network.MobileStatus( mob, mob ) );
            	}
            }
        }
	}
}
