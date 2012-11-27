using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class InflictWounds : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return !(TargetMobile is PlayerMobile); } }
		public override FeatList Feat{ get{ return FeatList.InflictWounds; } }
		public override string Name{ get{ return "Inflict Wounds"; } }
		public override int BaseCost{ get{ return 10; } }
		public override double FullEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.15); } }
		public override double PartialEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.15); } }
		
		public InflictWounds( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				FinalEffect( Caster, TargetMobile, TotalEffect );
				Success = true;
			}
		}
		
		public static void FinalEffect( Mobile caster, Mobile target, int wound )
		{
			target.FixedParticles( 0x374A, 10, 15, 5013, EffectLayer.Waist );
			target.PlaySound( 0x1F1 );
			new SpellDamageTimer( caster, target, 0.5, wound ).Start();
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "InflictWounds", AccessLevel.Player, new CommandEventHandler( InflictWounds_OnCommand ) );
		}
		
		[Usage( "InflictWounds" )]
        [Description( "Casts Inflict Wounds." )]
        private static void InflictWounds_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new InflictWounds( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.InflictWounds) ) ) );
        }
	}
}
