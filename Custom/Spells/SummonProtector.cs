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
					switch( ((PlayerMobile)Caster).Nation )
					{
						case Nation.Western: return "Summon Spirit of Balance";
						case Nation.Northern: return "Summon Spirit of Faith";
						case Nation.Southern: return "Summon Spirit of Power";
						case Nation.Haluaroc: return "Summon Spirit Totem";
						case Nation.Unknown: return "Summon Servant of Ohlm";
						case Nation.Tirebladd: return "Summon Divine Servant";
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
						
				switch( caster.Nation )
				{
					case Nation.Western:
					{
                        WanderingSpirit wand = new WanderingSpirit();
                        wand.Name = "A flittering spirit";

                        wand.ControlSlots = 2;
                        wand.RawHits = 150;
                        wand.Hits = 150;
                        wand.RangedAttackType = RangedAttackType.BreatheEnergy; 
                        if (FeatLevel > 2)
                        {
                            summoned = wand;
                            wand.RawMana = 100;
                            wand.Mana = 100;
                            wand.RangeFight = 5;
                            wand.Emote("*Giggles*");
                        }

                        else if (FeatLevel > 1)
                        {
                            summoned = wand;
                            wand.RawMana = 50;
                            wand.Mana = 50;
                            wand.Emote("*Giggles*");
                        }

                        else
                        {
                            summoned = wand;
                        }
						
						break;
					}
					case Nation.Northern:
					{
                        SpiritSoldier spir = new SpiritSoldier();
                        spir.ControlSlots = 3;
                        if (FeatLevel > 2)
                        {
                            summoned = spir;
                            spir.Say("For the Mother and Father.");
                            spir.RawHits = 300;
                            spir.Hits = 300;
                        }

                        else if (FeatLevel > 1)
                        {
                            summoned = spir;
                            spir.RawHits = 100;
                            spir.Hits = 100;
                            spir.RawDex = 30;
                            spir.Say("For the Mother and Father.");
                        }

                        else
                        {
                            summoned = spir;
                            spir.RawHits = 50;
                            spir.Hits = 50;
                            spir.RawDex = 10;
                            spir.Say("For the Mother and Father.");
                        }
						
						break;
					}
					case Nation.Southern:
					{
                        LesserEnergyElemental energ = new LesserEnergyElemental();
                        energ.Name = "A spirit of power";
                        energ.Hue = 2968;
                        energ.ControlSlots = 3;
						if( FeatLevel > 2 )
                        {
							summoned = energ;
                            energ.RangeFight = 2;
                            energ.Say("By the will of Aetreus.");
                        }

                        else if (FeatLevel > 1)
                        {
                            summoned = energ;
                            energ.Say("By the will of Aetreus.");
                        }

                        else
                        {
                            summoned = energ;
                            energ.RawHits = 50;
                            energ.Say("By the will of Aetreus.");
                        }
						
						break;
					}
					case Nation.Haluaroc:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterServantOfOhlm();
						
						else if( FeatLevel > 1 )
							summoned = new ServantOfOhlm();
						
						else
							summoned = new LesserServantOfOhlm();
						
						break;
					}
					case Nation.Unknown:
					{
						if( FeatLevel > 2 )
							summoned = new GreaterClericScorpion();
						
						else if( FeatLevel > 1 )
							summoned = new ClericScorpion();
						
						else
							summoned = new LesserClericScorpion();
						
						break;
					}
					case Nation.Tirebladd:
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
        		if( e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Nation != Nation.None )
        			SpellInitiator( new SummonProtector( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.SummonProtector) ) ) );
        		
        		else
        			e.Mobile.SendMessage( "You belong to no culture." );
        	}
        }
	}
}
