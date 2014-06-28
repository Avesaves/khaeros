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
	public class EnchantWeapon : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
public override FeatList Feat{ get{ return FeatList.EnchantWeapon; } }

        public override int TotalCost { get { return 50; } }


		
		public EnchantWeapon( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                return base.CanBeCast && l.Feats.GetFeatLevel(FeatList.EnchantWeapon) > 0;
            }
        }



        public override void Effect()
        {
            PlayerMobile m = Caster as PlayerMobile;

            BaseWeapon w = TargetItem as BaseWeapon;
            if (w.BetaNerf == true)
            {
                Success = false;
                m.SendMessage("This weapon already has an enchantment on it.");
                return; 

            }

            if (TargetCanBeAffected && CasterHasEnoughMana && TargetItem is IWeapon && w.BetaNerf == false)
            {



                Caster.Mana -= TotalCost;

                Container pack = Caster.Backpack;
                
                if ( m.DayOfDeath != 0)
                {
                    Caster.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*A deep red glow passes into the weapon*");
                    Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                    w.HueMod = 2943;
                    int redcounter = m.DayOfDeath;
                    m.DayOfDeath = 0;
                    m.WikiConfig = null;
                    #region magic effects
                    switch (redcounter)
                    {
                        case 0:
                            {
                                m.SendMessage("This should not have happened"); 
                                Success = false;
                                break; 
                            }
                        case 1:
                            {
                                //medium level normal critical 
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt/4;
                                att.PureDamage = m.RawMana/5;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 2:
                            {
                                goto case 1;
                            }
                        case 3:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It buzzes with energy...");
                                m.SendMessage("The item will have energy damage for a half hour.");
                                  w.AosElementDamages.Energy = 5;
                                //w.AosElementDamages.Cold = 5;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 4:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows a little colder...");
                                m.SendMessage("The item will have cold damage for a half hour.");
                              //  w.AosElementDamages.Energy = 0;
                                w.AosElementDamages.Cold = 5;
                               // w.AosElementDamages.Fire = 0;
                               // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true; 
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 5:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon begins to ... stink like rotten food?"); 
                                m.SendMessage("This is an awful enchantment. It immediately wears off.");
                                w.HueMod = -1; 
                                Success = true;
                                break; 
                            }
                        case 6:
                            {
                                goto case 3;
                            }
                        case 7:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows a little venomous...");
                                m.SendMessage("The item will have poison damage for a half hour.");
                                //  w.AosElementDamages.Energy = 0;
                                //w.AosElementDamages.Cold = 5;
                                // w.AosElementDamages.Fire = 0;
                                 w.AosElementDamages.Poison = 5;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 8:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows a little hotter...");
                                m.SendMessage("The item will have fire damage for a half hour.");
                                //  w.AosElementDamages.Energy = 0;
                               // w.AosElementDamages.Cold = 5;
                                 w.AosElementDamages.Fire = 5;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 9:
                            {
                                goto case 3;
                            }
                        case 10:
                            {
                                goto case 5; 
                            }
                        case 11:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It buzzes with energy...");
                                m.SendMessage("The item will have energy damage for a half hour.");
                                w.AosElementDamages.Energy = 25;
                                //w.AosElementDamages.Cold = 5;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 12:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon grows too hot to hold!");
                                m.SendMessage("Ouch! What a bad enchantment!");
                                w.Movable = false;
                                w.BetaNerf = true;
                                m.Backpack.DropItem(w);
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 13:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows colder...");
                                m.SendMessage("The item will have energy damage for a half hour.");
                                //w.AosElementDamages.Energy = 25;
                                w.AosElementDamages.Cold = 25;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 14:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon grows heavier!");
                                m.SendMessage("Oh no! What a bad enchantment!");
                                w.Weight += 50;
                                w.BetaNerf = true;
                                Success = true; 
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare2));
                                break; 
                            }
                        case 15:
                            {
                                goto case 5; 
                            }
                        case 16:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows brighter...");
                                m.SendMessage("The item simply glows.");
                                //w.AosElementDamages.Energy = 25;
                                //w.AosElementDamages.Cold = 25;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 17:
                            {
                                
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on kill for a half hour.");
                                Engines.XmlSpawner2.XmlSoulEater att = new Server.Engines.XmlSpawner2.XmlSoulEater();
                                att.Chance = m.RawInt / 2;
                                att.Hits = m.RawMana / 4;
                                att.Stam = 5;
                                att.Mana = 20; 
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true; 
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case 18:
                            {
                                m.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon sprays something red all over them!"); 
                                m.SolidHueOverride = 37;
                                Success = true; 
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 19:
                            {
                                switch(Utility.Random(2))
                                {
                                    case 0:
                                        {


                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                            m.SendMessage("The item will steal life on hit for an hour.");
                                            Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                            att.Chance = m.RawInt / 4;
                                            att.Hits = m.RawMana / 4;
                                            att.Expiration = TimeSpan.FromMinutes(60);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.HasHalo = true;
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 1:
                                        {


                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                            m.SendMessage("The item will steal stamina on hit for an hour.");
                                            Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                            att.Chance = m.RawInt / 4;
                                            att.Stam = m.RawMana/5;
                                            att.Expiration = TimeSpan.FromMinutes(60);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.HasHalo = true;
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 2:
                                        {
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                            m.SendMessage("The item will steal mana on hit for an hour.");
                                            Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                            att.Chance = m.RawInt / 4;
                                            att.Mana = m.RawMana / 5;
                                            att.Expiration = TimeSpan.FromMinutes(60);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.HasHalo = true;
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                            break;
                                        }
                                        
                                }
                                break; 
                            }
                        case 20:
                            {
                                goto case 5; 
                            }
                        case 21:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows cold...");
                                m.SendMessage("The item will have energy damage for a half hour.");
                                //w.AosElementDamages.Energy = 12;
                                w.AosElementDamages.Cold = 12;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 22:
                            {
                                switch (Utility.Random(3))
                                {
                                    case 0:
                                        {
                                //medium level normal critical 
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt / 4;
                                att.PureDamage = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                                        }
                                                                            case 1:
                                        {
                                //medium level normal critical 
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt / 4;
                                att.EnergyDamage = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                                        }                                    case 2:
                                        {
                                //medium level normal critical 
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt / 4;
                                att.FrostDamage = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                                        }                                    case 3:
                                        {
                                //medium level normal critical 
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt / 4;
                                att.FireDamage = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                                        }
                                         
                                }
                                break;
                            }
                        case 23:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal mana on hit for an hour.");
                                Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                att.Chance = m.RawMana / 2;
                                att.Mana = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case 24:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                m.SendMessage("The item will have critical hits for a half hour.");
                                Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                att.Chance = m.RawInt / 4;
                                att.PureDamage = m.RawMana / 12;
                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 25:
                            {
                                goto case 5; 
                            }
                        case 26:
                            {
                                goto case 23; 

                            }
                        case 27:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows a little hotter...");
                                m.SendMessage("The item will have fire damage for a half hour.");
                                //  w.AosElementDamages.Energy = 0;
                                // w.AosElementDamages.Cold = 5;
                                w.AosElementDamages.Fire = 10;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 28:
                            {
                                switch (Utility.Random(3))
                                {
                                    case 0:
                                        {
                                            //medium level normal critical 
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                            m.SendMessage("The item will have critical hits for a half hour.");
                                            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                            att.Chance = m.RawInt / 5;
                                            att.PureDamage = 10;
                                            att.Expiration = TimeSpan.FromMinutes(30);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 1:
                                        {
                                            //medium level normal critical 
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                            m.SendMessage("The item will have critical hits for a half hour.");
                                            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                            att.Chance = m.RawInt / 5;
                                            att.EnergyDamage = 10;
                                            att.Expiration = TimeSpan.FromMinutes(30);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 2:
                                        {
                                            //medium level normal critical 
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                            m.SendMessage("The item will have critical hits for a half hour.");
                                            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                            att.Chance = m.RawInt / 5;
                                            att.FrostDamage = 10;
                                            att.Expiration = TimeSpan.FromMinutes(30);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 3:
                                        {
                                            //medium level normal critical 
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It begins to vibrate...");
                                            m.SendMessage("The item will have critical hits for a half hour.");
                                            Engines.XmlSpawner2.XmlCriticalHit att = new Server.Engines.XmlSpawner2.XmlCriticalHit();
                                            att.Chance = m.RawInt / 5;
                                            att.FireDamage = 10;
                                            att.Expiration = TimeSpan.FromMinutes(30);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                       
                                }
                                break;
                            }
                        case 29:
                            {
                                m.Hits = 1;
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "Energy explodes out of the item!");
                                ExplodeItself(Caster);
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(Flare));
                                break; 
                            }
                        case 30:
                            {
                                goto case 5; 
                            }
                        case 31:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on kill for an hour.");
                                Engines.XmlSpawner2.XmlSoulEater att = new Server.Engines.XmlSpawner2.XmlSoulEater();
                                att.Chance = 100;
                                att.Hits = m.RawMana / 4;
                                att.Stam = 20;
                                att.Mana = 30;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case 32:
                            {
                                goto case 24; 
                            }
                        case 33:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It buzzes with energy...");
                                m.SendMessage("The item will have energy damage for a half hour.");
                                w.AosElementDamages.Energy = 12;
                                //w.AosElementDamages.Cold = 5;
                                // w.AosElementDamages.Fire = 0;
                                // w.AosElementDamages.Poison = 0;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case 34:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on hit for an hour.");
                                Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                att.Chance = m.RawMana / 2;
                                att.Hits = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case 35:
                            {
                                goto case 5; 
                            }
                        case 36:
                            {
                                goto case 16;
                            }
                        case 37:
                            {
                                goto case -19; 
                            }
                        case 38:
                            {
                                goto case 22;
                            }
                        case 39:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "You feel nimble.");
                                m.SendMessage("The item will increase your dexterity for a half hour.");
                                Engines.XmlSpawner2.XmlDex att = new Server.Engines.XmlSpawner2.XmlDex(m.RawMana/5, 1800);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case 40:
                            {
                                goto case 5; 
                            }
                        case 41:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon looks more powerful...");
                                w.QualityDamage += 1;
                                w.QualitySpeed += 1;
                                w.QualityAccuracy += 1;
                                w.QualityDefense += 1;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare5));
                                break;
                            }
                        case 42:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "You feel STRONG.");
                                m.SendMessage("The item will increase your strength for a half hour.");
                                Engines.XmlSpawner2.XmlStr att = new Server.Engines.XmlSpawner2.XmlStr(m.RawMana/5, 1800);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case 43:
                            {
                                m.SendMessage("Your weapon gains a strange Slayer.... (can be combined with .bloodmagic)");
                                Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                                // w.Slayer = SlayerName.ArachnidDoom;
                                Success = true;
                                m.WikiConfig = null;

                                XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("Chimera", 80, 50, 30);
                                XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("Devourer", 80, 50, 30);
                                XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("DisplacerBeast", 80, 50, 30);
                                XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("Ettercap", 80, 50, 30);
                                XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("Gorgon", 80, 50, 30);
                                XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("Hag", 80, 50, 30);
                                XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("HookHorror", 80, 50, 30);
                                XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("LavaAbomination", 80, 50, 30);
                                XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("Quaraphon", 80, 50, 30);
                                XmlEnemyMastery ten = new Engines.XmlSpawner2.XmlEnemyMastery("RustMonster", 80, 50, 30);
                                XmlEnemyMastery eleven = new Engines.XmlSpawner2.XmlEnemyMastery("Stalker", 80, 50, 30);
                                XmlEnemyMastery twelve = new Engines.XmlSpawner2.XmlEnemyMastery("Umberhulk", 80, 50, 30);
                                XmlEnemyMastery thirteen = new Engines.XmlSpawner2.XmlEnemyMastery("Wortling", 80, 50, 30);
                                XmlEnemyMastery fourteen = new Engines.XmlSpawner2.XmlEnemyMastery("Gobwort", 80, 50, 30);
                                XmlEnemyMastery fifteen = new Engines.XmlSpawner2.XmlEnemyMastery("Xorn", 80, 50, 30);
                                one.Name = "a";
                                two.Name = "b";
                                three.Name = "c";
                                four.Name = "d";
                                five.Name = "e";
                                six.Name = "f";
                                seven.Name = "g";
                                eight.Name = "h";
                                nine.Name = "i";
                                ten.Name = "j";
                                eleven.Name = "k";
                                twelve.Name = "l";
                                thirteen.Name = "m";
                                fourteen.Name = "n";
                                fifteen.Name = "o";

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

                                w.BetaNerf = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;

                            }
                        case 44:
                            {
                                goto case 24; 
                            }
                        case 45:
                            {
                                goto case 5; 
                            }
                        case 46:
                            {
                                goto case 34;
                            }
                        case 47:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on hit for an hour.");
                                Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                att.Chance = m.RawMana/2;
                                att.Hits = m.RawMana / 5;
                                att.Stam = 10;
                                att.Mana = 10;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case 48:
                            {
                                goto case 16;
                            }
                        case 49:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It feels lucky...");
                                m.SendMessage("The item will make you luckier for a half hour.");
                                Engines.XmlSpawner2.XmlBackground att = new Server.Engines.XmlSpawner2.XmlBackground(BackgroundList.Lucky, 1, 30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }

                        case -1:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows wickedly sharp...");
                                m.SendMessage("The item will bleed your opponent for a half hour.");
                                Engines.XmlSpawner2.XmlBleedingWound att = new Server.Engines.XmlSpawner2.XmlBleedingWound();
                                att.Chance = m.RawInt/5;
                                att.Damage = m.RawMana / 5;

                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case -2:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows wickedly sharp...");
                                m.SendMessage("The item will bleed your opponent for a half hour.");
                                Engines.XmlSpawner2.XmlBleedingWound att = new Server.Engines.XmlSpawner2.XmlBleedingWound();
                                att.Chance = m.RawInt/6;
                                att.Damage = m.RawMana / 4;

                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case -3:
                            {
                                goto case 33;

                            }
                        case -4:
                            {
                                goto case 21; 
                            }
                        case -5:
                            {
                                
                                switch (Utility.Random(5))
                                {
                                    case 0:
                                        {
                                            m.SendMessage("Your weapon gains Undead Slayer (can be combined with .bloodmagic)");
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
                                            one.Name = "a";
                                            two.Name = "b";
                                            three.Name = "c";
                                            four.Name = "d";
                                            five.Name = "e";
                                            six.Name = "f";
                                            seven.Name = "g";
                                            eight.Name = "h";
                                            nine.Name = "i";

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

                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 1:
                                        {
                                            Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                                            // w.Slayer = SlayerName.DragonSlaying;
                                            Success = true;
                                            m.WikiConfig = null;

                                            m.SendMessage("Your weapon gains Dragon Slayer (can be combined with .bloodmagic)");
                                            XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("Dragon", 80, 50, 30);
                                            XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("CopperDragon", 80, 50, 30);
                                            XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("SteelDragon", 80, 50, 30);
                                            XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("IronDragon", 80, 50, 30);
                                            XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("SilverDragon", 80, 50, 30);
                                            XmlEnemyMastery six = new Engines.XmlSpawner2.XmlEnemyMastery("GoldDragon", 80, 50, 30);
                                            XmlEnemyMastery seven = new Engines.XmlSpawner2.XmlEnemyMastery("BronzeDragon", 80, 50, 30);
                                            XmlEnemyMastery eight = new Engines.XmlSpawner2.XmlEnemyMastery("Wyvern", 80, 50, 30);
                                            XmlEnemyMastery nine = new Engines.XmlSpawner2.XmlEnemyMastery("SkeletalDragon", 80, 50, 30);
                                            one.Name = "a";
                                            two.Name = "b";
                                            three.Name = "c";
                                            four.Name = "d";
                                            five.Name = "e";
                                            six.Name = "f";
                                            seven.Name = "g";
                                            eight.Name = "h";
                                            nine.Name = "i";

                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);

                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 2:
                                        {
                                            m.SendMessage("Your weapon gains Fairy Slayer (can be combined with .bloodmagic)");
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
                                            one.Name = "a";
                                            two.Name = "b";
                                            three.Name = "c";
                                            four.Name = "d";
                                            five.Name = "e";
                                            six.Name = "f";
                                            seven.Name = "g";
                                            eight.Name = "h";
                                            nine.Name = "i";

                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, six);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, seven);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, eight);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, nine);

                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 3:
                                        {
                                            m.SendMessage("Your weapon gains Elemental Slayer (can be combined with .bloodmagic)");
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
                                            one.Name = "a";
                                            two.Name = "b";
                                            three.Name = "c";
                                            four.Name = "d";
                                            five.Name = "e";
                                            six.Name = "f";
                                            seven.Name = "g";
                                            eight.Name = "h";
                                            nine.Name = "i";
                                            ten.Name = "j";
                                            eleven.Name = "k";
                                            twelve.Name = "l";
                                            thirteen.Name = "m";


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

                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 4:
                                        {
                                            m.SendMessage("Your weapon gains Bug Slayer (can be combined with .bloodmagic)");
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
                                            one.Name = "a";
                                            two.Name = "b";
                                            three.Name = "c";
                                            four.Name = "d";
                                            five.Name = "e";
                                            six.Name = "f";
                                            seven.Name = "g";
                                            eight.Name = "h";
                                            nine.Name = "i";
                                            ten.Name = "j";
                                            eleven.Name = "k";
                                            twelve.Name = "l";
                                            thirteen.Name = "m";
                                            fourteen.Name = "n";
                                            fifteen.Name = "o";

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

                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 5:
                                        {
                                            m.SendMessage("Your weapon gains Giant Slayer (can be combined with .bloodmagic)");
                                            Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                                            // w.Slayer = SlayerName.Repond;
                                            Success = true;
                                            m.WikiConfig = null;

                                            XmlEnemyMastery one = new Engines.XmlSpawner2.XmlEnemyMastery("FireGiant", 80, 50, 30);
                                            XmlEnemyMastery two = new Engines.XmlSpawner2.XmlEnemyMastery("HillGiant", 80, 50, 30);
                                            XmlEnemyMastery three = new Engines.XmlSpawner2.XmlEnemyMastery("IceGiant", 80, 50, 30);
                                            XmlEnemyMastery four = new Engines.XmlSpawner2.XmlEnemyMastery("StoneGiant", 80, 50, 30);
                                            XmlEnemyMastery five = new Engines.XmlSpawner2.XmlEnemyMastery("StormGiant", 80, 50, 30);

                                            one.Name = "e";
                                            two.Name = "d";
                                            three.Name = "c";
                                            four.Name = "b";
                                            five.Name = "a";


                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, one);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, two);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, three);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, four);
                                            Engines.XmlSpawner2.XmlAttach.AttachTo(w, five);


                                            w.BetaNerf = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                         
                                }
                                break;

                            }
                        case -6:
                            {
                                goto case 33;
                            }
                        case -7:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It grows a little venomous...");
                                m.SendMessage("The item will have poison damage for a half hour.");
                                //  w.AosElementDamages.Energy = 0;
                                //w.AosElementDamages.Cold = 5;
                                // w.AosElementDamages.Fire = 0;
                                w.AosElementDamages.Poison = 15;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case -8:
                            {
                                goto case 27;
                  
                            }
                        case -9:
                            {
                                goto case 3;
                            }
                        case -10:
                            {
                                goto case -5;
                            }
                        case -11:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It seems more forceful...");
                                m.SendMessage("The item will stun on hit for a half hour.");
                                Engines.XmlSpawner2.XmlMiniStun att = new Server.Engines.XmlSpawner2.XmlMiniStun();
                                att.Chance = m.RawInt / 5;
                                att.Duration = 2;

                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case -12:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon turns on its user!");
                                m.Hits -= 100;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(Flare));
                                break; 
                            }
                        case -13:
                            {
                                int hm = w.QualityDamage;
                                int hm2 = w.QualitySpeed;
                                w.QualityDamage = hm2;
                                w.QualitySpeed = hm;
                                m.SendMessage("*Something weird happens to the weapon.*");
                                Success = true;
                                w.HueMod = -1;
                                break; 
                            }
                        case -14:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "You feel healthy.");
                                m.SendMessage("The item will increase your hits for a half hour.");
                                Engines.XmlSpawner2.XmlHits att = new Server.Engines.XmlSpawner2.XmlHits(m.RawMana/5, 1800);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case -15:
                            {
                                goto case -5;
                            }
                        case -16:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Takes on a green sheen*"); 
                                w.Poison = Poison.Deadly;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(Flare));
                                break;

                            }
                        case -17:
                            {

                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on hit for an hour.");
                                Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                att.Chance = m.RawInt / 5;
                                att.Hits = m.RawMana / 5;
                                att.Stam = 5;
                                att.Mana = 5;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case -18:
                            {
                                goto case 24; 
                            }
                        case -19:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It seems more forceful...");
                                m.SendMessage("The item will stun on hit for a half hour.");
                                Engines.XmlSpawner2.XmlMiniStun att = new Server.Engines.XmlSpawner2.XmlMiniStun();
                                att.Chance = m.RawInt / 3;
                                att.Duration = Utility.Random(3);

                                att.Expiration = TimeSpan.FromMinutes(30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case -20:
                            {
                                goto case -5;
                            }
                        case -21:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "You feel intelligent.");
                                m.SendMessage("The item will increase your strength for a half hour.");
                                Engines.XmlSpawner2.XmlInt att = new Server.Engines.XmlSpawner2.XmlInt(m.RawMana/5, 1800);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case -22:
                            {
                                goto case 23; 
                            }
                        case -23:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*This weapon changes you while you hold it!*");
                                m.SendMessage("Your weapon will give you every positive background for half hour, but deafen you while you hold it.");
                                Caster.FixedParticles(0x22AE, 244, 25, 9950, 37, 0, EffectLayer.Waist);
                                // w.Slayer = SlayerName.ArachnidDoom;
                                Success = true;
                                m.WikiConfig = null;

                                XmlBackground one = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Fit, 1, 30);
                                XmlBackground two = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Faithful, 1, 30);
                                XmlBackground three = new Engines.XmlSpawner2.XmlBackground(BackgroundList.FocusedMind, 1, 30);
                                XmlBackground four = new Engines.XmlSpawner2.XmlBackground(BackgroundList.IronWilled, 1, 30);
                                XmlBackground five = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Lucky, 1, 30);
                                XmlBackground six = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Quick, 1, 30);
                                XmlBackground seven = new Engines.XmlSpawner2.XmlBackground(BackgroundList.QuickHealer, 1, 30);
                                XmlBackground eight = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Resilient, 1, 30);
                                XmlBackground nine = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Smart, 1, 30);
                                XmlBackground ten = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Strong, 1, 30);
                                XmlBackground eleven = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Tough, 1, 30);
                                XmlBackground twelve = new Engines.XmlSpawner2.XmlBackground(BackgroundList.AnimalEmpathy, 1, 30);
                                XmlBackground thirteen = new Engines.XmlSpawner2.XmlBackground(BackgroundList.Deaf, 1, 30);
                                /*
                                one.Name = "a";
                                two.Name = "b";
                                three.Name = "c";
                                four.Name = "d";
                                five.Name = "e";
                                six.Name = "f";
                                seven.Name = "g";
                                eight.Name = "h";
                                nine.Name = "i";
                                ten.Name = "j";
                                eleven.Name = "k";
                                twelve.Name = "l";
                                thirteen.Name = "m";
                                */
                                
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


                                w.BetaNerf = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }
                        case -24:
                            {
                                goto case 24; 
                            }
                        case -25:
                            {
                                goto case -5;
                            }
                        case -26:
                            {
                                goto case 22;
                            }
                        case -27:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "You feel you could run forever holding this item...");
                                m.SendMessage("The item will increase your stamina for a half hour.");
                                Engines.XmlSpawner2.XmlStam att = new Server.Engines.XmlSpawner2.XmlStam(m.RawMana/5, 1800);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }
                        case -28:
                            {
                                goto case 14;
                            }
                        case -29:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It sparks in your hand...");
                                m.SendMessage("The item will deal lightning damage for an hour.");
                                Engines.XmlSpawner2.XmlLightningStrike att = new Server.Engines.XmlSpawner2.XmlLightningStrike();
                                att.Chance = m.RawMana / 4;
                                att.Damage = m.RawMana / 5;
                                att.Expiration = TimeSpan.FromMinutes(60);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(60), new TimerCallback(Flare));
                                break;
                            }
                        case -30:
                            {
                                goto case -5;
                            }
                        case -31:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on kill for six hours.");
                                Engines.XmlSpawner2.XmlSoulEater att = new Server.Engines.XmlSpawner2.XmlSoulEater();
                                att.Chance = 100;
                                att.Hits = m.RawMana / 3;
                                att.Stam = 30;
                                att.Mana = 30;
                                att.Expiration = TimeSpan.FromHours(6);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromHours(6), new TimerCallback(Flare));
                                break;
                            }
                        case -32:
                            {
                                goto case 24;
                            }
                        case -33:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It buzzes crazily...!");
                                m.SendMessage("The item will have various damages for a half hour.");
                                w.AosElementDamages.Energy = 5;
                                w.AosElementDamages.Cold = 5;
                                w.AosElementDamages.Fire = 5;
                                w.AosElementDamages.Poison = 5;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break;
                            }

                        case -34:
                            {
                                goto case 34;
                            }
                        case -35:
                            {
                                goto case -5;
                            }
                        case -36:
                            {
                                goto case 24; 
                            }
                        case -37:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon grows longer...");
                                m.SendMessage("The item will have increased range for a half hour.");
                                w.MaxRange += 1;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare3));
                                break; 
                            }
                        case -38:
                            {
                                goto case 22;
                            }
                        case -39:
                            {
                                m.Map = Map.Ilshenar;
                                m.SendMessage("You... don't think you're dead....");
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(Flare));
                                break;
                            }
                        case -40:
                            {
                                goto case -5;
                            }
                        case -41:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon looks more powerful...");
                                w.QualityDamage += 2;
                                w.QualitySpeed += 2;
                                w.QualityAccuracy += 2;
                                w.QualityDefense += 2; 
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare4));
                                break;
                            }
                        case -42:
                            {
                                switch (Utility.Random(3))
                                {
                                    case 0:
                                        {
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "The weapon repairs itself...");
                                            w.HitPoints += 20;
                                            w.BetaNerf = true;
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 1:
                                        {
                                            m.Map = Map.Ilshenar;
                                            m.SendMessage("You... don't think you're dead....");
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(Flare));
                                            break;
                                        }
                                    case 2:
                                        {
                                            m.Hits = 1;
                                            w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "Energy explodes out of the item!");
                                            ExplodeItself(Caster);
                                            Success = true;
                                            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(Flare));
                                            break;
                                        }
                                }
                                break; 
                            }
                        case -43:
                            {
                                Success = true;
                                m.SendMessage("Although nothing happens, you feel immediately ready for a new enchantment...!");
                                int smoop = Utility.Random(49);
                                m.DayOfDeath = smoop;
                                w.HueMod = -1;
                                break;
                            
                            }
                        case -44:
                            {
                                goto case 5; 
                            }
                        case -45:
                            {
                                goto case -5;
                            }
                        case -46:
                            {
                                goto case 34;
                            }
                        case -47:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It glows strangely...");
                                m.SendMessage("The item will steal life on hit for two hours.");
                                Engines.XmlSpawner2.XmlLifeStealer att = new Server.Engines.XmlSpawner2.XmlLifeStealer();
                                att.Chance = 90;
                                att.Hits = m.RawMana / 4;
                                att.Stam = 10;
                                att.Mana = 25;
                                att.Expiration = TimeSpan.FromMinutes(120);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.HasHalo = true;
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(120), new TimerCallback(Flare));
                                break;
                            }
                        case -48:
                            {
                                goto case 24; 
                            }
                        case -49:
                            {
                                w.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "It feels lucky...");
                                m.SendMessage("The item will make you luckier for a half hour.");
                                Engines.XmlSpawner2.XmlBackground att = new Server.Engines.XmlSpawner2.XmlBackground(BackgroundList.Lucky, 1, 30);
                                Engines.XmlSpawner2.XmlAttach.AttachTo(w, att);
                                w.BetaNerf = true;
                                Success = true;
                                Timer.DelayCall(TimeSpan.FromMinutes(30), new TimerCallback(Flare));
                                break; 
                            }

                    }
                    return; 
                }
            }
            else
            {
                m.SendMessage("You haven't eaten anything to use for an enchantment"); 
                Success = false;
                return;
            }
        }
                    #endregion


        public void ExplodeItself(Mobile target)
        {
            Point3D loc = Caster.Location;
            Map map = Caster.Map;

            if (map == null)
                return;

            BombPotion pot = new BombPotion(1);

            pot.InstantExplosion = true;
            pot.ExplosionRange = 6;
            pot.AddEffect(CustomEffect.Explosion, 100);
            pot.AddEffect(CustomEffect.Fire, 100);
            pot.AddEffect(CustomEffect.Shrapnel, 1000);
            pot.HeldBy = Caster;
            pot.PotionEffect = PotionEffect.ExplosionLesser;

            pot.Explode(Caster, false, loc, map);
        }
        private void Flare()
        {
          
            if (TargetItem == null || TargetItem.Deleted)
                return;
            
            BaseWeapon w = TargetItem as BaseWeapon;
            w.BetaNerf = false;
            w.AosElementDamages.Energy = 0;
            w.AosElementDamages.Cold = 0;
            w.AosElementDamages.Fire = 0;
            w.AosElementDamages.Poison = 0;
            Caster.SolidHueOverride = -1;
            Caster.Map = Map.Felucca; 
            w.HasHalo = false; 
            w.Movable = true; 
            w.HueMod = -1; 
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
        private void Flare2()
        {

            if (TargetItem == null || TargetItem.Deleted)
                return;

            BaseWeapon w = TargetItem as BaseWeapon;
            w.BetaNerf = false;
            w.AosElementDamages.Energy = 0;
            w.AosElementDamages.Cold = 0;
            w.AosElementDamages.Fire = 0;
            w.AosElementDamages.Poison = 0;
            w.Movable = true;
            w.Weight -= 50; 
            w.HueMod = -1;
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
        private void Flare3()
        {

            if (TargetItem == null || TargetItem.Deleted)
                return;

            BaseWeapon w = TargetItem as BaseWeapon;
            w.BetaNerf = false;
            w.AosElementDamages.Energy = 0;
            w.AosElementDamages.Cold = 0;
            w.AosElementDamages.Fire = 0;
            w.AosElementDamages.Poison = 0;
            Caster.SolidHueOverride = -1;
            Caster.Map = Map.Felucca;
            w.HasHalo = false;
            w.Movable = true;
            w.HueMod = -1;
            w.MaxRange -= 1; 
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
        private void Flare4()
        {

            if (TargetItem == null || TargetItem.Deleted)
                return;

            BaseWeapon w = TargetItem as BaseWeapon;
            w.BetaNerf = false;
            w.QualityDamage -= 2;
            w.QualitySpeed -= 2;
            w.QualityDefense -= 2;
            w.QualityAccuracy -= 2; 
            Caster.SolidHueOverride = -1;
            Caster.Map = Map.Felucca;
            w.HasHalo = false;
            w.Movable = true;
            w.HueMod = -1;
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
        private void Flare5()
        {

            if (TargetItem == null || TargetItem.Deleted)
                return;

            BaseWeapon w = TargetItem as BaseWeapon;
            w.BetaNerf = false;
            w.QualityDamage -= 1;
            w.QualitySpeed -= 1;
            w.QualityDefense -= 1;
            w.QualityAccuracy -= 1;
            Caster.SolidHueOverride = -1;
            Caster.Map = Map.Felucca;
            w.HasHalo = false;
            w.Movable = true;
            w.HueMod = -1;
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
         
		public static void Initialize()
		{
			CommandSystem.Register( "EnchantWeapon", AccessLevel.Player, new CommandEventHandler( EnchantWeapon_OnCommand ) );
		}
		
		[Usage( "EnchantWeapon" )]
        [Description( "Enchants using Enchant Weapon." )]
        private static void EnchantWeapon_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new EnchantWeapon( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.EnchantWeapon) ) ) );

        }
	}
}
