using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class SacredBlast : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool UsesFullEffect{ get{ return !(TargetMobile is PlayerMobile); } }
		public override FeatList Feat{ get{ return FeatList.SacredBlast; } }
		public override string Name{ get{ return "Sacred Blast"; } }
		public override int BaseCost{ get{ return 15; } }
		public override double FullEffect{ get{ return 10.0; } }
		public override double PartialEffect{ get{ return 10.0; } }
		
		public SacredBlast( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				Mobile oldTarget = TargetMobile;
				Caster.MovingEffect( TargetMobile, 0x36E4, 5, 0, false, true, 2967, 0 );
				Caster.PlaySound( 552 );
				Caster.Mana -= TotalCost;
                new SpellDamageTimer( Caster, TargetMobile, 1, FullEffect * FeatLevel ).Start();
				Success = true;
				
				foreach( Mobile m in Caster.GetMobilesInRange( 5 ) )
            	{
                    TargetMobile = m;
                    
                    if( TargetCanBeAffected && m.Hits < m.HitsMax && ((IKhaerosMobile)Caster).IsAllyOf( m ) )
					{
						HealWounds.FinalEffect( m, (int)((PartialEffect * FeatLevel) * (Self == true ? 0.5 : 1) * (Faith == true ? 1.1 : 1)) );
						break;
					}
				}
				
				TargetMobile = oldTarget;
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SacredBlast", AccessLevel.Player, new CommandEventHandler( SacredBlast_OnCommand ) );
		}
		
		[Usage( "SacredBlast" )]
        [Description( "Casts Sacred Blast." )]
        private static void SacredBlast_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new SacredBlast( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.SacredBlast) ) ) );
        }
	}
}
