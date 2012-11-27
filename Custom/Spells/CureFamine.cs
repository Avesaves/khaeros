using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class CureFamine : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool UsesTarget{ get{ return FeatLevel > 2 ? false : true; } }
		public override FeatList Feat{ get{ return FeatList.CureFamine; } }
		public override string Name{ get{ return "Cure Famine"; } }
		public override int TotalCost{ get{ return FeatLevel > 2 ? 20 : 5; } }
		
		public override SkillName GetSkillName{ get{ return SkillName.Invocation; } }
		
		public CureFamine( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				TargetMobile.FixedEffect( 0x376A, 9, 32 );
				TargetMobile.PlaySound( 480 );
				Caster.Mana -= TotalCost;
				
				if( FeatLevel == 1 )
					TargetMobile.Thirst = 20;
				
				else if( FeatLevel == 2 )
					TargetMobile.Hunger = 20;
				
				Success = true;
			}
			
			else if( FeatLevel > 2 && CasterHasEnoughMana )
			{
				Caster.PlaySound( 480 );
				Caster.Mana -= TotalCost;

				foreach( Mobile m in Caster.GetMobilesInRange( 5 ) )
				{
                    TargetMobile = m;
                    
                    if( TargetCanBeAffected && ((IKhaerosMobile)Caster).IsAllyOf( m ) )
					{
						TargetMobile.FixedEffect( 0x376A, 9, 32 );
						TargetMobile.Thirst = 20;
						TargetMobile.Hunger = 20;
					}
				}
				
				TargetMobile = null;
				Success = true;
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "CureFamine", AccessLevel.Player, new CommandEventHandler( CureFamine_OnCommand ) );
		}
		
		[Usage( "CureFamine" )]
        [Description( "Casts Cure Famine." )]
        private static void CureFamine_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new CureFamine( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.CureFamine) ) ) );
        }
	}
}
