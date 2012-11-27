using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class Curse : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return !(TargetMobile is PlayerMobile); } }
		public override FeatList Feat{ get{ return FeatList.Curse; } }
		public override string Name{ get{ return "Curse"; } }
		public override int BaseCost{ get{ return 10; } }
		public override double FullEffect{ get{ return 0.05; } }
		public override double PartialEffect{ get{ return 0.025; } }
		
		public Curse( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				FinalEffect( Caster, TargetMobile, ((UsesFullEffect ? FullEffect : PartialEffect) * FeatLevel), FeatLevel );
				Success = true;
			}
		}
		
		public static void FinalEffect( Mobile caster, Mobile target, double offset, int featLevel )
		{
			target.PlaySound( 490 );
			target.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist );
			int hits = - Convert.ToInt32(target.RawHits * offset);
			int stam = - Convert.ToInt32(target.RawStam * offset);
			int mana = - Convert.ToInt32(target.RawMana * offset);
			Spells.SpellHelper.AddStatBonus( caster, target, StatType.HitsMax, hits, TimeSpan.FromSeconds( 300.0 * featLevel ) );
			Spells.SpellHelper.AddStatBonus( caster, target, StatType.StamMax, stam, TimeSpan.FromSeconds( 300.0 * featLevel ) );
			Spells.SpellHelper.AddStatBonus( caster, target, StatType.ManaMax, mana, TimeSpan.FromSeconds( 300.0 * featLevel ) );
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Curse", AccessLevel.Player, new CommandEventHandler( Curse_OnCommand ) );
		}
		
		[Usage( "Curse" )]
        [Description( "Casts Curse." )]
        private static void Curse_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new Curse( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Curse) ) ) );
        }
	}
}
