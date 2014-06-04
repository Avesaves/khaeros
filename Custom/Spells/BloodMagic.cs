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
	public class BloodMagic : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
public override FeatList Feat{ get{ return FeatList.RedMagic; } }

        public override int TotalCost { get { return 50; } }


		
		public BloodMagic( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                return base.CanBeCast;
            }
        }


		
		public override void Effect( )
		{
            if (TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IWeapon)
            {
                PlayerMobile m = Caster as PlayerMobile;
                BaseWeapon w = TargetItem as BaseWeapon;
                m.Prompt = new LookForRPPrompt();
                Caster.Emote("*Glows a deep red*");
                Caster.Mana -= TotalCost;
                Container pack = Caster.Backpack;

                Success = true;
            }
            else
                Success = false;

			}

        private class BloodMagicPrompt : Prompt
        {
            public BloodMagicPrompt()
            {
            }

            public override void OnResponse(Mobile from, string text)
            {
                BaseWeapon w = TargetItem as BaseWeapon;
                Container pack = Caster.Backpack;
                if (pack == null)
                    return;
                else if (text == "undead")
                {
                    if (pack.ConsumeTotal(typeof(Diamond), 1))
                    {
                        w.Slayer = SlayerName.Silver;
                        Timer.DelayCall(TimeSpan.FromSeconds(360), new TimerCallback(Flare));
                        return;
                    }
                    else
                    {
                        from.SendMessage("You lack the materials needed for this spell.");
                        return;
                    }
                }

                else
                {
                    from.SendMessage("You lack the knowledge to make this work.");
                    return;
                }
            }
        }
        private void Flare()
        {
          
            if (TargetItem == null || TargetItem.Deleted)
                return;
            
            BaseWeapon w = TargetItem as BaseWeapon;
            w.Slayer == SlayerName.None;
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Ceases to glow*");


        }

		public static void Initialize()
		{
			CommandSystem.Register( "BloodMagic", AccessLevel.Player, new CommandEventHandler( BloodMagic_OnCommand ) );
		}
		
		[Usage( "BloodMagic" )]
        [Description( "Enchants using blood magic." )]
        private static void BloodMagic_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new BloodMagic( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Magery) ) ) );

        }
	}
}
