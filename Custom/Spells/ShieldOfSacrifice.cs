using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class ShieldOfSacrifice : BaseCustomSpell
	{
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.ShieldOfSacrifice; } }
		public override string Name{ get{ return "Shield of Sacrifice"; } }
		public override int BaseCost{ get{ return 10; } }
		public override double FullEffect{ get{ return 3; } }
		public override double PartialEffect{ get{ return (FeatLevel * 0.25); } }
		
		public ShieldOfSacrifice( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
                if( ((IKhaerosMobile)TargetMobile).ShieldingMobile != null || ((IKhaerosMobile)TargetMobile).ShieldedMobile != null )
                {
                    Caster.SendMessage( "That target is already involved in another Shield of Sacrifice spell." );
                    return;
                }

                if( ((IKhaerosMobile)Caster).ShieldingMobile != null || ((IKhaerosMobile)Caster).ShieldedMobile != null )
                {
                    Caster.SendMessage( "You are already involved in another Shield of Sacrifice spell." );
                    return;
                }

				Caster.Mana -= TotalCost;
				FinalEffect( Caster, TargetMobile, TotalEffect, PartialEffect );
				Success = true;
			}
		}
		
		public static void FinalEffect( Mobile caster, Mobile target, int duration, double protection )
        {
            target.PlaySound( 0x202 );
            target.FixedParticles( 0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist );
            target.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
            ( (IKhaerosMobile)target ).ShieldingMobile = caster;
            ( (IKhaerosMobile)target ).ShieldValue = protection;
            ( (IKhaerosMobile)caster ).ShieldedMobile = target;
            Timer.DelayCall( TimeSpan.FromMinutes( duration ), new TimerCallback( ( (IKhaerosMobile)target ).RemoveShieldOfSacrifice ) );
            Timer.DelayCall( TimeSpan.FromMinutes( duration ), new TimerCallback( ( (IKhaerosMobile)caster ).RemoveShieldOfSacrifice ) );
        }
		
		public static void Initialize()
		{
			CommandSystem.Register( "ShieldOfSacrifice", AccessLevel.Player, new CommandEventHandler( ShieldOfSacrifice_OnCommand ) );
		}
		
		[Usage( "ShieldOfSacrifice" )]
        [Description( "Casts Shield of Sacrifice." )]
        private static void ShieldOfSacrifice_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new ShieldOfSacrifice( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.ShieldOfSacrifice) ) ) );
        }
	}
}
