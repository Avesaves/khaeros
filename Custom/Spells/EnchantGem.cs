using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class EnchantGem : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return false; } }
		public override bool AffectsMobiles{ get{ return false; } }
		public override bool UsesTarget{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.Magery; } }
		public override string Name{ get{ return "Enchant Gem"; } }
		public override int TotalCost{ get{ return FeatLevel > 2 ? 20 : 5; } }
		
		public override SkillName GetSkillName{ get{ return SkillName.Magery; } }
		
		public EnchantGem( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IGem )
			{
				Caster.Emote ("*Pours their life-force into a gemstone*");
				Caster.Mana -= TotalCost;
                Container pack = Caster.Backpack;
				
				if( TargetItem is Amethyst && Caster.RawInt > 19 )
                {
                    GlowingAmethyst glow1 = new GlowingAmethyst();
					TargetItem.Delete();
                    pack.DropItem( glow1 );
                    Caster.RawInt -= 10;
                }

                else if( TargetItem is Citrine )
                {
                    GlowingCitrine glow2 = new GlowingCitrine();
					TargetItem.Delete();
                    pack.DropItem( glow2 );
                    Caster.Age += 2;
                }

                else if (TargetItem is Diamond && Caster.RawMana > 19 )
                {
                    GlowingDiamond glow3 = new GlowingDiamond();
					TargetItem.Delete();
                    pack.DropItem( glow3 );
                    Caster.RawMana -= 10;
                }

                else if (TargetItem is Sapphire && Caster.ExtraCPRewards > 9999 )
                {
                    GlowingSapphire glow4 = new GlowingSapphire();
					TargetItem.Delete();
                    pack.DropItem( glow4 );
                    Caster.ExtraCPRewards -= 10000;
                }

                else if (TargetItem is Cinnabar)
                {
                    if (pack.ConsumeTotal(typeof(Head), 1))
                    {
                        GlowingCinnabar glow5 = new GlowingCinnabar();
                        TargetItem.Delete();
                        pack.DropItem(glow5);
                        Success = true;
                        return;
                    }
                    Success = false;
                    return;
                }

                else if (TargetItem is StarSapphire)
                {
                    if (pack.ConsumeTotal(typeof(StarmetalIngot), 1))
                    {
                        GlowingStarSapphire glow6 = new GlowingStarSapphire();
                        TargetItem.Delete();
                        pack.DropItem(glow6);
                        Success = true;
                        return;
                    }


                    Success = false;
                    return;

                }

                else if (TargetItem is Emerald && Caster.RawDex > 19)
                {
                    GlowingEmerald glow7 = new GlowingEmerald();
                    TargetItem.Delete();
                    pack.DropItem(glow7);
                    Caster.RawDex -= 10;
                }

                else if (TargetItem is Ruby && Caster.RawHits > 19)
                {
                    GlowingRuby glow8 = new GlowingRuby();
                    TargetItem.Delete();
                    pack.DropItem(glow8);
                    Caster.RawHits -= 10;
                }

                else if (TargetItem is Amber && Caster.RawStr > 19)
                {
                    GlowingAmber glow9 = new GlowingAmber();
                    TargetItem.Delete();
                    pack.DropItem(glow9);
                    Caster.RawStr -= 10;
                }

                else if (TargetItem is Tourmaline && Caster.RawStam > 19)
                {
                    GlowingTourmaline glow10 = new GlowingTourmaline();
                    TargetItem.Delete();
                    pack.DropItem(glow10);
                    Caster.RawStam -= 10;
                }

                else
                {
                    Success = false;
                    Caster.SendMessage("Your enchantment fails.");
                    return;
                }
				
				Success = true;
			}
			

			}

		
		public static void Initialize()
		{
			CommandSystem.Register( "EnchantGem", AccessLevel.Player, new CommandEventHandler( EnchantGem_OnCommand ) );
		}
		
		[Usage( "EnchantGem" )]
        [Description( "Casts Enchant Gem." )]
        private static void CureFamine_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new EnchantGem( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Magery) ) ) );
        }
	}
}
