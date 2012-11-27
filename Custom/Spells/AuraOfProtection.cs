using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Gumps;

namespace Server.Misc
{
	public class AuraOfProtection : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFaith{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return !Self; } }
		public override FeatList Feat{ get{ return FeatList.AuraOfProtection; } }
		public override string Name{ get{ return "Aura of Protection"; } }
		public override int BaseCost{ get{ return 15; } }
		public override double FullEffect{ get{ return 5.0; } }
		public override double PartialEffect{ get{ return 3.0; } }
		public override double FaithModifier{ get{ return Faith == true ? FeatLevel : 0; }	}
		public override int TotalEffect
		{
			get
			{
				if( UsesFullEffect )
					return (int)((FullEffect * FeatLevel) + FaithModifier);
				
				return (int)((PartialEffect * FeatLevel) + FaithModifier);
			}
		}
		
		public AuraOfProtection( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				FinalEffect( TargetMobile, TotalEffect, (int)(Caster.Skills[SkillName.Faith].Base - 1) );
				Success = true;
			}
		}
		
		public static void FinalEffect( Mobile target, int protection, int duration )
		{
			if( target == null || target.Deleted || !(target is IKhaerosMobile) )
				return;
			
			if( target.ResistanceMods != null )
				target.ResistanceMods.Clear();
			
			if( ((IKhaerosMobile)target).AuraOfProtection != null )
				((IKhaerosMobile)target).AuraOfProtection.Stop();
			
			target.PlaySound( 0x1E9 );
			target.FixedParticles( 0x375A, 9, 20, 5016, EffectLayer.Waist );
			
			ResistanceMod blunt = new ResistanceMod( ResistanceType.Blunt, protection );
			ResistanceMod slash = new ResistanceMod( ResistanceType.Slashing, protection );
			ResistanceMod pierce = new ResistanceMod( ResistanceType.Piercing, protection );
			target.AddResistanceMod( blunt );
			target.AddResistanceMod( slash );
			target.AddResistanceMod( pierce );
			
			((IKhaerosMobile)target).AuraOfProtection = new AuraOfProtectionTimer( target, duration );
			((IKhaerosMobile)target).AuraOfProtection.Start();
			
			if( target is PlayerMobile && ((PlayerMobile)target).HasGump( typeof( CharInfoGump ) ) )
				((PlayerMobile)target).SendGump( new CharInfoGump( (PlayerMobile)target ) );
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "AuraOfProtection", AccessLevel.Player, new CommandEventHandler( AuraOfProtection_OnCommand ) );
		}
		
		[Usage( "AuraOfProtection" )]
        [Description( "Casts Aura of Protection." )]
        private static void AuraOfProtection_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new AuraOfProtection( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.AuraOfProtection) ) ) );
        }
        
        public class AuraOfProtectionTimer : Timer
        {
            private Mobile m_target;
            private int m_animsleft;

            public AuraOfProtectionTimer( Mobile target, int animsleft )
            	: base( TimeSpan.FromSeconds( 10 ) )
            {
                m_target = target;
                m_animsleft = animsleft;
            }

            protected override void OnTick()
            {
            	if( m_target == null )
            		return;
            	
            	if( m_animsleft > 0 )
            	{
            		m_animsleft--;
            		m_target.FixedParticles( 0x375A, 9, 20, 5016, EffectLayer.Waist );
    				( (IKhaerosMobile)m_target ).AuraOfProtection = new AuraOfProtectionTimer( m_target, m_animsleft );
    				( (IKhaerosMobile)m_target ).AuraOfProtection.Start();
            	}
            	
            	else
            	{
            		if( m_target.ResistanceMods != null )
            			m_target.ResistanceMods.Clear();
            		
            		ResistanceMod blunt = new ResistanceMod( ResistanceType.Blunt, 0 );
					ResistanceMod slash = new ResistanceMod( ResistanceType.Slashing, 0 );
					ResistanceMod pierce = new ResistanceMod( ResistanceType.Piercing, 0 );
					m_target.AddResistanceMod( blunt );
					m_target.AddResistanceMod( slash );
					m_target.AddResistanceMod( pierce );
						
					if( m_target is PlayerMobile && ( (PlayerMobile)m_target ).HasGump( typeof( CharInfoGump ) ) && ( (PlayerMobile)m_target ).m_CharInfoTimer == null )
					{
						( (PlayerMobile)m_target ).m_CharInfoTimer = new CharInfoGump.CharInfoTimer( ( (PlayerMobile)m_target ) );
						( (PlayerMobile)m_target ).m_CharInfoTimer.Start();
					}
					
					if( ( (IKhaerosMobile)m_target ).AuraOfProtection != null )
					{
						( (IKhaerosMobile)m_target ).AuraOfProtection.Stop();
	    				( (IKhaerosMobile)m_target ).AuraOfProtection = null;
					}
            	}
            }
        }
	}
}
