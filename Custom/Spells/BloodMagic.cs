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
           PlayerMobile m = Caster as PlayerMobile;
                
            BaseWeapon w = TargetItem as BaseWeapon;


            if (TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IWeapon && w.Identified == false)
            {
                

                
                Caster.Mana -= TotalCost;

                Container pack = Caster.Backpack;
                if (m.WikiConfig != null)
                {
                    Caster.Emote("*A deep red glow passes into the weapon*");
                    w.HueMod = 2943;
                    m.DayOfDeath = 0; 
                   
                    if (m.WikiConfig == "undead")
                    {
                        if (pack == null)
                            return;
                            Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                            //w.Slayer = SlayerName.Silver;
                            Success = true;
                            m.WikiConfig = null;
                            XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("SkeletalLord", 80, 50, 30);
                            XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("SkeletalSoldier", 80, 50, 30);
                            XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("Zombie", 80, 50, 30);
                            XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("Skeleton", 80, 50, 30);
                            XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("FamineSpirit", 80, 50, 30);
                            XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("FleshGolem", 80, 50, 30);
                            XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("BoneGolem", 80, 50, 30);
                            XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("LesserFleshGolem", 80, 50, 30);
                            XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("SkeletalDragon", 80, 50, 30);
                            XmlEnemyMastery ten = new Engines.XmlSpawner2.XmlEnemyMastery("ImperialZombie", 80, 50, 30);
                            XmlEnemyMastery eleven = new Engines.XmlSpawner2.XmlEnemyMastery("SocietyZombie", 80, 50, 30);
                            XmlEnemyMastery twelve = new Engines.XmlSpawner2.XmlEnemyMastery("UndeadMinotaur", 80, 50, 30);
                            one.Name = "One";
                            two.Name = "Two";
                            three.Name = "Three";
                            four.Name = "Four";
                            five.Name = "Five";
                            six.Name = "Six";
                            seven.Name = "Seven";
                            eight.Name = "Eight";
                            nine.Name = "Nine";
                            ten.Name = "ten";
                            eleven.Name = "eleven";
                            twelve.Name = "twelve"; 

                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, new Engines.XmlSpawner2.XmlEnemyMastery("LesserBoneGolem", 80, 50, 30));
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, ten);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, eleven);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, twelve);

                            w.Identified = true; 
                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));

                            return;
                       
                     }
                    else if (m.WikiConfig == "dragon")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                           // w.Slayer = SlayerName.DragonSlaying;
                            Success = true;
                            m.WikiConfig = null;


                            XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("Dragon", 80, 50, 30);
                            XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("CopperDragon", 80, 50, 30);
                            XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("SteelDragon", 80, 50, 30);
                            XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("IronDragon", 80, 50, 30);
                            XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("SilverDragon", 80, 50, 30);
                            XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("GoldDragon", 80, 50, 30);
                            XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("BronzeDragon", 80, 50, 30);
                            XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("Wyvern", 80, 50, 30);
                            XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("SkeletalDragon", 80, 50, 30);
                            one.Name = "One";
                            two.Name = "Two";
                            three.Name = "Three";
                            four.Name = "Four";
                            five.Name = "Five";
                            six.Name = "Six";
                            seven.Name = "Seven";
                            eight.Name = "Eight";
                            nine.Name = "Nine";

                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);

                            w.Identified = true; 
                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                            return;

                    }
                    else if (m.WikiConfig == "fey")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        //w.Slayer = SlayerName.Fey;
                        Success = true;
                        m.WikiConfig = null;

                        XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("Centaur", 80, 50, 30);
                        XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("Petal", 80, 50, 30);
                        XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("Pixie", 80, 50, 30);
                        XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("Satyr", 80, 50, 30);
                        XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("Unicorn", 80, 50, 30);
                        XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("Sprite", 80, 50, 30);
                        XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("MaleUnicorn", 80, 50, 30);
                        XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("Dryad", 80, 50, 30);
                        XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("ElderDryad", 80, 50, 30);
                        one.Name = "One";
                        two.Name = "Two";
                        three.Name = "Three";
                        four.Name = "Four";
                        five.Name = "Five";
                        six.Name = "Six";
                        seven.Name = "Seven";
                        eight.Name = "Eight";
                        nine.Name = "Nine";

                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);

                        w.Identified = true; 
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }

                    else if (m.WikiConfig == "elemental")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                        //w.Slayer = SlayerName.ElementalBan;
                        Success = true;
                        m.WikiConfig = null;


                        XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("AirElemental", 80, 50, 30);
                        XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("WaterElemental", 80, 50, 30);
                        XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("FireElemental", 80, 50, 30);
                        XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("EarthElemental", 80, 50, 30);
                        XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("CrystalElemental", 80, 50, 30);
                        XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("Excremental", 80, 50, 30);
                        XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("StormElemental", 80, 50, 30);
                        XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("LesserAirElemental", 80, 50, 30);
                        XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("LesserWaterElemental", 80, 50, 30);
                        XmlEnemyMastery ten = new Engines.XmlSpawner2.XmlEnemyMastery("LesserFireElemental", 80, 50, 30);
                        XmlEnemyMastery eleven = new Engines.XmlSpawner2.XmlEnemyMastery("LesserEarthElemental", 80, 50, 30);
                        XmlEnemyMastery twelve = new Engines.XmlSpawner2.XmlEnemyMastery("LesserCrystalElemental", 80, 50, 30);
                        XmlEnemyMastery thirteen = new Engines.XmlSpawner2.XmlEnemyMastery("LesserStormElemental", 80, 50, 30);
                        one.Name = "One";
                        two.Name = "Two";
                        three.Name = "Three";
                        four.Name = "Four";
                        five.Name = "Five";
                        six.Name = "Six";
                        seven.Name = "Seven";
                        eight.Name = "Eight";
                        nine.Name = "Nine";
                        ten.Name = "Ten";
                        eleven.Name = "Eleven";
                        twelve.Name = "Twelve";
                        thirteen.Name = "Thirteen";
 

                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, ten);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, eleven);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, twelve);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, thirteen);

                        w.Identified = true; 
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }
                    else if (m.WikiConfig == "arachnid")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                       // w.Slayer = SlayerName.ArachnidDoom;
                        Success = true;
                        m.WikiConfig = null;

                        XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("CrawlingVermin", 80, 50, 30);
                        XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("DesertCrawler", 80, 50, 30);
                        XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("DireSpider", 80, 50, 30);
                        XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("DuneDigger", 80, 50, 30);
                        XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("FireBeetle", 80, 50, 30);
                        XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("GiantCentipede", 80, 50, 30);
                        XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("HornedBeetle", 80, 50, 30);
                        XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("Larva", 80, 50, 30);
                        XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("PincerBeetle", 80, 50, 30);
                        XmlEnemyMastery ten = new Engines.XmlSpawner2.XmlEnemyMastery("RhinoBeetle", 80, 50, 30);
                        XmlEnemyMastery eleven = new Engines.XmlSpawner2.XmlEnemyMastery("RuneBeetle", 80, 50, 30);
                        XmlEnemyMastery twelve = new Engines.XmlSpawner2.XmlEnemyMastery("Snowdigger", 80, 50, 30);
                        XmlEnemyMastery thirteen = new Engines.XmlSpawner2.XmlEnemyMastery("AssassinSpider", 80, 50, 30);
                        XmlEnemyMastery fourteen = new Engines.XmlSpawner2.XmlEnemyMastery("Scorpion", 80, 50, 30);
                        XmlEnemyMastery fifteen = new Engines.XmlSpawner2.XmlEnemyMastery("AmbusherSpider", 80, 50, 30);
                        XmlEnemyMastery sixteen = new Engines.XmlSpawner2.XmlEnemyMastery("FunnelWebSpider", 80, 50, 30);
                        XmlEnemyMastery seventeen = new Engines.XmlSpawner2.XmlEnemyMastery("GiantWolfSpider", 80, 50, 30);
                        one.Name = "One";
                        two.Name = "Two";
                        three.Name = "Three";
                        four.Name = "Four";
                        five.Name = "Five";
                        six.Name = "Six";
                        seven.Name = "Seven";
                        eight.Name = "Eight";
                        nine.Name = "Nine";
                        ten.Name = "Ten";
                        eleven.Name = "Eleven";
                        twelve.Name = "Twelve";
                        thirteen.Name = "Thirteen";
                        fourteen.Name = "fourteen";
                        fifteen.Name = "fifteen";
                        sixteen.Name = "sixteen";
                        seventeen.Name = "seventeen";

                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, ten);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, eleven);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, twelve);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, thirteen);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, fourteen);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, fifteen);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, sixteen);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, seventeen);

                        w.Identified = true; 
                        Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                        return;

                    }
                    else if (m.WikiConfig == "repond")
                    {
                        Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                       // w.Slayer = SlayerName.Repond;
                        Success = true;
                        m.WikiConfig = null;

                        XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("FireGiant", 80, 50, 30);
                        XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("HillGiant", 80, 50, 30);
                        XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("IceGiant", 80, 50, 30);
                        XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("StoneGiant", 80, 50, 30);
                        XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("StormGiant", 80, 50, 30);

                        one.Name = "One";
                        two.Name = "Two";
                        three.Name = "Three";
                        four.Name = "Four";
                        five.Name = "Five";


                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                        Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);


                        w.Identified = true; 
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
            w.Identified = false;
            w.HueMod = -1; 
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


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
        		SpellInitiator( new BloodMagic( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.RedMagic) ) ) );

        }
	}
}
