using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class HealWounds : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFaith{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return !Self; } }
		public override FeatList Feat{ get{ return FeatList.HealWounds; } }
		public override string Name{ get{ return "Heal Wounds"; } }
		public override int BaseCost{ get{ return 10; } }
		public override double FullEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.15); } }
		public override double PartialEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.10); } }
		
		public HealWounds( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				FinalEffect( TargetMobile, TotalEffect );
				Success = true;
			}
		}
		
		public static void FinalEffect( Mobile target, int heal )
		{
			target.PlaySound( 0x1F2 );
			target.FixedEffect( 0x376A, 9, 32 );
			target.Hits += heal;
			target.LocalOverheadMessage( MessageType.Regular, 170, false, "+" + heal );
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "HealWounds", AccessLevel.Player, new CommandEventHandler( HealWounds_OnCommand ) );
		}
		
		[Usage( "HealWounds" )]
        [Description( "Casts Heal Wounds." )]
        private static void HealWounds_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new HealWounds( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.HealWounds) ) ) );
        }
	}
}
