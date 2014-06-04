using System;
using Server;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Engines.XmlSpawner2;
using Server.Prompts;


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
                return base.CanBeCast && l.Feats.GetFeatLevel(FeatList.RedMagic) > 0;
            }
        }


		
		public override void Effect( )
		{
            if (TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IWeapon)
            {
                PlayerMobile m = Caster as PlayerMobile;
                BaseWeapon w = TargetItem as BaseWeapon;

                
                Caster.Mana -= TotalCost;

                Container pack = Caster.Backpack;
                if (m.WikiConfig != null)
                {
                    Caster.Emote("*Glows a deep red*");
                    if (m.WikiConfig == "undead")
                    {
                        if (pack == null)
                            return;
                            Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                            w.Slayer = SlayerName.Silver;
                            Success = true;
                            m.WikiConfig = null;
                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                            return;
                       
                     }
                    else if (m.WikiConfig == "dragon")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                            w.Slayer = SlayerName.DragonSlaying;
                            Success = true;
                            m.WikiConfig = null;
                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                            return;

                    }
                    else if (m.WikiConfig == "fey")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        w.Slayer = SlayerName.Fey;
                        Success = true;
                        m.WikiConfig = null;
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }

                    else if (m.WikiConfig == "elemental")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        w.Slayer = SlayerName.ElementalBan;
                        Success = true;
                        m.WikiConfig = null;
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }
                    else if (m.WikiConfig == "arachnid")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        w.Slayer = SlayerName.ArachnidDoom;
                        Success = true;
                        m.WikiConfig = null;
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }
                    else if (m.WikiConfig == "repond")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        w.Slayer = SlayerName.Repond;
                        Success = true;
                        m.WikiConfig = null;
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }
                    }
                }
                Success = false;
            

        }
			

       
        
   
        private void Flare()
        {
          
            if (TargetItem == null || TargetItem.Deleted)
                return;
            
            BaseWeapon w = TargetItem as BaseWeapon;
            w.Slayer = SlayerName.None;
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
