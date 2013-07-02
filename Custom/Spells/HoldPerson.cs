using System;
using Server.Mobiles;
using Server.Commands;

namespace Server.Misc
{
	public class HoldPerson : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.HoldPerson; } }
		public override string Name{ get{ return "Hold Person"; } }
		public override int BaseCost{ get{ return 20; } }
		public override double FullEffect{ get{ return 2.0; } }
		
		public HoldPerson( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				FinalEffect( Caster, TargetMobile, ReduceDelay(TotalEffect) );
				Success = true;
			}
		}

        private int ReduceDelay(int delay)
        {
            PlayerMobile target = TargetMobile as PlayerMobile;

            if (target != null)
            {
                int resistLevel = target.Feats.GetFeatLevel(FeatList.MagicResistance);
                return delay - resistLevel;
            }

            return delay;
        }
		
		public static void FinalEffect( Mobile caster, Mobile target, int hold )
		{
			target.PlaySound( 0x204 );
			target.FixedParticles( 0x37C4, 1, 8, 9916, 39, 3, EffectLayer.Head );
			target.FixedParticles( 0x37C4, 1, 8, 9502, 39, 4, EffectLayer.Head );
			
			if( caster != null )
				target.Emote( "*was paralyzed by " + caster.Name + "*" );

            if (((IKhaerosMobile)target).StunnedTimer != null)
                ((IKhaerosMobile)target).StunnedTimer.Stop();

            ((IKhaerosMobile)target).StunnedTimer = new HoldPersonTimer(target, hold);
            ((IKhaerosMobile)target).StunnedTimer.Start();
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "HoldPerson", AccessLevel.Player, new CommandEventHandler( HoldPerson_OnCommand ) );
		}
		
		[Usage( "HoldPerson" )]
        [Description( "Casts Hold Person." )]
        private static void HoldPerson_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new HoldPerson( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.HoldPerson) ) ) );
        }
        
        public class HoldPersonTimer : Timer
        {
            private Mobile m;

            public HoldPersonTimer( Mobile from, int delay )
            	: base( TimeSpan.FromSeconds( delay ) )
            {
                m = from;
            }

            protected override void OnTick()
            {
            	if( m == null || m.Deleted )
            		return;

                ((IKhaerosMobile)m).StunnedTimer = null;
        		m.SendMessage( "You are no longer under the effect of Hold Person." );
            }
        }
	}
}
