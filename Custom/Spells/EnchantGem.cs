using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Engines.XmlSpawner2;


namespace Server.Misc
{
	public class EnchantGem : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }

        public override int TotalCost { get { return 10; } }

		
		public override SkillName GetSkillName{ get{ return SkillName.Magery; } }
		
		public EnchantGem( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                return base.CanBeCast && l.CanBeMage;
            }
        }
		
		public override void Effect( )
		{
			if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IGem )
			{
				Caster.Emote ("*Pours their life-force into a gemstone*");
				Caster.Mana -= TotalCost;
                Container pack = Caster.Backpack;
                	PlayerMobile m = Caster as PlayerMobile;
                
                	
				
				if( TargetItem is Amethyst && Caster.RawInt > 19 )
                {
                    GlowingAmethyst glow1 = new GlowingAmethyst();
					TargetItem.Delete();
                    pack.DropItem( glow1 );
                    Caster.RawInt -= 10;
                    Caster.SendMessage ("You feel a little more dull...");
                }
                
              /*  else if ( TargetItem is Jet )
                {
                	if (m.Backgrounds.BackgroundDictionary[Gorgeous].Level = 1)
                	{
         
			        m.Target = new BackgroundTarget( Gorgeous, 0, true );
			        m.Target = new BackgroundTarget( GoodLooking, 1, true );
                		Caster.SendMessage ("You feel ugly.");
                			GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );                		
                	}
                	else if (m.Backgrounds.BackgroundDictionary[GoodLooking].Level = 1 )
                	{
                		m.Backgrounds.BackgroundDictionary[GoodLooking].Level = 0;
                		m.Backgrounds.BackgroundDictionary[Attractive].Level = 1;
                		Caster.SendMessage ("You feel ugly.");
                			GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );                		
                	}
                	else if (m.Backgrounds.BackgroundDictionary[Attractive].Level = 1 )
                	{
                		m.Backgrounds.BackgroundDictionary[Attractive].Level = 0;
                		Caster.SendMessage ("You feel ugly.");
                			GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );                		
                	}
                	else if (m.Backgrounds.BackgroundDictionary[Homely].Level = 1 )
                	{
                		m.Backgrounds.BackgroundDictionary[Homely].Level = 0;
                		m.Backgrounds.BackgroundDictionary[Ugly].Level = 1;
                		Caster.SendMessage ("You feel ugly.");
                			GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );                		
                	}
                	else if (m.Backgrounds.BackgroundDictionary[Ugly].Level = 1 )
                	{
                		m.Backgrounds.BackgroundDictionary[Ugly].Level = 0;
                		m.Backgrounds.BackgroundDictionary[Disfigured].Level = 1;
                		Caster.SendMessage ("You feel ugly.");
                		        GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );
                	}
                	else if (m.Backgrounds.BackgroundDictionary[Disfigured].Level = 1 )
                	{
                		Caster.SendMessage ("You just can't look any worse.");
                	}                	
                	else
                	{
                		m.Backgrounds.BackgroundDictionary[Homely].Level = 1;
                			Caster.SendMessage ("You feel ugly.");
                			GlowingJet glowJ = new GlowingJet();
                			TargetItem.Delete();
                			pack.DropItem( glowJ );
                	}
                }
*/
                else if( TargetItem is Citrine && m.Age < 70 )
                {
                    GlowingCitrine glow2 = new GlowingCitrine();
					TargetItem.Delete();
                    pack.DropItem( glow2 );
                    m.Age += 5;
                    Caster.SendMessage ("You feel a little older...");

                }

                else if (TargetItem is Diamond && Caster.RawMana > 19 )
                {
                    GlowingDiamond glow3 = new GlowingDiamond();
					TargetItem.Delete();
                    pack.DropItem( glow3 );
                    Caster.RawMana -= 10;
                    Caster.SendMessage ("You feel some of your power drain away.");
                }

                else if (TargetItem is Sapphire && Caster.TithingPoints > 0 )
                {
                    GlowingSapphire glow4 = new GlowingSapphire();
					TargetItem.Delete();
                    pack.DropItem( glow4 );
                    Caster.TithingPoints -= 1;
                    Caster.SendMessage ("You feel strange.");
                }

                else if (TargetItem is Cinnabar)
                {
                    if (pack.ConsumeTotal(typeof(Head), 1))
                    {
                        GlowingCinnabar glow5 = new GlowingCinnabar();
                        TargetItem.Delete();
                        pack.DropItem(glow5);
                        Caster.SendMessage ("The stone seems to consume the dead flesh...");
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
                        Caster.SendMessage ("The stone seems to consume the metal...");
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
                    Caster.SendMessage ("You feel a little less dextrous.");
                    Caster.RawDex -= 10;
                }

                else if (TargetItem is Ruby && Caster.RawHits > 19)
                {
                    GlowingRuby glow8 = new GlowingRuby();
                    TargetItem.Delete();
                    pack.DropItem(glow8);
                    Caster.RawHits -= 10;
                    Caster.SendMessage ("You feel unhealthy.");
                }

                else if (TargetItem is Amber && Caster.RawStr > 19)
                {
                    GlowingAmber glow9 = new GlowingAmber();
                    TargetItem.Delete();
                    pack.DropItem(glow9);
                    Caster.RawStr -= 10;
                    Caster.SendMessage ("You feel weaker.");
                }

                else if (TargetItem is Tourmaline && Caster.RawStam > 19)
                {
                    GlowingTourmaline glow10 = new GlowingTourmaline();
                    TargetItem.Delete();
                    pack.DropItem(glow10);
                    Caster.RawStam -= 10;
                    Caster.SendMessage ("You feel more out of shape.");
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
			        private class BackgroundTarget : Target
{
         BackgroundList m_Background;
         int m_Level;
         bool m_Setting;
        
public BackgroundTarget( BackgroundList background, int level, bool setting ) : base( 30, false, TargetFlags.None )
{
m_Background = background;
m_Level = level;
m_Setting = setting;
}

protected override void OnTarget( Mobile from, object targeted )
{
if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is PlayerMobile) )
return;

PlayerMobile target = targeted as PlayerMobile;

if( m_Setting )
{
target.Backgrounds.BackgroundDictionary[m_Background].Level = m_Level;
from.SendMessage( "Property has been set." );
}

else
from.SendMessage( m_Background.ToString() + " = " + target.Backgrounds.BackgroundDictionary[m_Background].Level.ToString() );
}
        }

		
		public static void Initialize()
		{
			CommandSystem.Register( "EnchantGem", AccessLevel.Player, new CommandEventHandler( EnchantGem_OnCommand ) );
		}
		
		[Usage( "EnchantGem" )]
        [Description( "Casts Enchant Gem." )]
        private static void EnchantGem_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new EnchantGem( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Magery) ) ) );
        }
	}
}
