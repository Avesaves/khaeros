using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class Mending : BaseCustomSpell
	{
		public override bool AffectsItems{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return true; } }
		public override bool BackpackItemsOnly{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.Mending; } }
		public override string Name{ get{ return "Mending"; } }
		public override int BaseCost{ get{ return 5; } }
		public override double FullEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.1) * (double)FeatLevel; } }
		
		public Mending( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( Caster is PlayerMobile && DateTime.Compare( DateTime.Now, ((PlayerMobile)Caster).NextMending ) > 0 && TargetCanBeAffected && TargetItem is IDurability && CasterHasEnoughMana)
			{
				Caster.PlaySound( 506 );
				Caster.Mana -= TotalCost;
				((IDurability)TargetItem).HitPoints += (int)FullEffect;
				((PlayerMobile)Caster).NextMending = DateTime.Now + TimeSpan.FromHours( 8 );
				Success = true;
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Mending", AccessLevel.Player, new CommandEventHandler( Mending_OnCommand ) );
		}
		
		[Usage( "Mending" )]
        [Description( "Casts Mending." )]
        private static void Mending_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new Mending( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Mending) ) ) );
        }
	}
}
