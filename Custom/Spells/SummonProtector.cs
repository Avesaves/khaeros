using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class SummonProtector : BaseCustomSpell
	{
		public override bool UsesFullEffect{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.SummonProtector; } }
		public override int BaseCost{ get{ return 30; } }
		public override double FullEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.05); } }
		public override string Name
		{
			get
			{
				if( !BadCasting && Caster is PlayerMobile )
				{
					switch( ((PlayerMobile)Caster).ChosenDeity )
					{
						case ChosenDeity.Arianthynt: return "Summon Huorn";
						case ChosenDeity.Xipotec: return "Summon Volcanic Guardian";
						case ChosenDeity.Mahtet: return "Summon Scorpion";
						case ChosenDeity.Xorgoth: return "Summon Spirit Totem";
						case ChosenDeity.Ohlm: return "Summon Servant of Ohlm";
						case ChosenDeity.Elysia: return "Summon Divine Servant";
					}
				}
				
				return "a spell";
			}
		}
		
		public SummonProtector( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( ((IKhaerosMobile)Caster).CanSummon() && Caster is PlayerMobile && CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				PlayerMobile caster = Caster as PlayerMobile;
				
				BaseCreature summoned = new Chicken() as BaseCreature;
						
				switch( caster.ChosenDeity )
				{
					case ChosenDeity.Arianthynt:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterHuorn();
						
						else if( FeatLevel > 1 )
							summoned = new Huorn();
						
						else
							summoned = new LesserHuorn();
						
						break;
					}
					case ChosenDeity.Xipotec:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterVolcanicGuardian();
						
						else if( FeatLevel > 1 )
							summoned = new VolcanicGuardian();
						
						else
							summoned = new VolcanicGuardian();
						
						break;
					}
					case ChosenDeity.Elysia:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterDivineProtector();
						
						else if( FeatLevel > 1 )
							summoned = new DivineProtector();
						
						else
							summoned = new LesserDivineProtector();
						
						break;
					}
					case ChosenDeity.Ohlm:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterServantOfOhlm();
						
						else if( FeatLevel > 1 )
							summoned = new ServantOfOhlm();
						
						else
							summoned = new LesserServantOfOhlm();
						
						break;
					}
					case ChosenDeity.Mahtet:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterClericScorpion();
						
						else if( FeatLevel > 1 )
							summoned = new ClericScorpion();
						
						else
							summoned = new LesserClericScorpion();
						
						break;
					}
					case ChosenDeity.Xorgoth:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterSpiritTotem();
						
						else if( FeatLevel > 1 )
							summoned = new SpiritTotem();
						
						else
							summoned = new LesserSpiritTotem();
						
						break;
					}
				}
				
				Summon( Caster, summoned, TotalEffect, 533 );
				Success = true;
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SummonProtector", AccessLevel.Player, new CommandEventHandler( SummonProtector_OnCommand ) );
		}
		
		[Usage( "SummonProtector" )]
        [Description( "Casts First Racial Power." )]
        private static void SummonProtector_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        	{
        		if( e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).ChosenDeity != ChosenDeity.None )
        			SpellInitiator( new SummonProtector( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.SummonProtector) ) ) );
        		
        		else
        			e.Mobile.SendMessage( "You still need to choose which deity you worship. Please use .ChosenDeity to do so." );
        	}
        }
	}
}
