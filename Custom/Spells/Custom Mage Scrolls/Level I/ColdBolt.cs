﻿using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ColdBoltScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ColdBoltSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ColdBoltScroll() : base()
        {
            Hue = 2968;
            Name = "A Cold Bolt scroll";
        }

        public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
        {
        }

        public override void OnDoubleClick( Mobile m )
        {
            if( !this.IsChildOf( m.Backpack ) )
                return;

            if( !IsMageCheck( m, true ) )
                return;

            BaseCustomSpell.SpellInitiator( new ColdBoltSpell( m, 1 ) );
        }

        public ColdBoltScroll( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }

    [PropertyObject]
    public class ColdBoltSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ColdBoltSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
		public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return true; } }
        public override bool UsesTarget { get { return true; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Cold Bolt"; } }
        public override int ManaCost { get { return 65; } }
        public override int BaseRange { get { return 12; } }

        public ColdBoltSpell()
            : this( null, 1 )
        {
        }

        public ColdBoltSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6144;
            Range = 12;
            CustomName = "Cold Bolt";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.MindI } );
            }
        }

        public override void Effect()
        {		
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{

                    Mobile targ = TargetMobile as Mobile;


                    Caster.Mana -= TotalCost;
                    Success = true;

                    Caster.PlaySound(521);

                    Caster.MovingParticles(targ, 0x36FE, 7, 0, false, true, 2968, 0, 3043, 4043, 0x211, 0x100);
                    
                    Caster.Emote("*Fires a bolt of pure cold!*");
                    Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(Flare1));
                Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(Flare2));
                Timer.DelayCall(TimeSpan.FromSeconds(4), new TimerCallback(Flare3));
                Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Flare4));
                Timer.DelayCall(TimeSpan.FromSeconds(6), new TimerCallback(Flare5));
                Timer.DelayCall(TimeSpan.FromSeconds(7), new TimerCallback(Flare6));
            }
        }

                	private void Flare1()
                    {
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;

                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 10, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 20, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                    targ.Emote("*Grows colder...*");
                    Caster.FixedParticles(0x375A, 244, 25, 9950, 2968, 0, EffectLayer.Waist);

                }	
                        	private void Flare2()
                    {
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;                    


                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 10, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 20, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                    targ.Emote("*Grows colder...*");
                    

                    return;
                }
                        	private void Flare3()
                    {
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;                    


                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 10, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 20, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                    targ.Emote("*Freezes in place!*");
                    targ.Frozen = true;
                    targ.SolidHueOverride = 90; 
                    

                    return;
                }
                                	private void Flare4()
                    {
                    if ( Caster == null )
                    {
                        targ.Frozen = false;
                        targ.SolidHueOverride = -1;
                    return;
                    }

                    if ( targ.Alive == false )
                       {
                        targ.Frozen = false;
                        targ.SolidHueOverride = -1;
                    return;
                    }                   


                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 10, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 20, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
 
                    

                    return;
                }
                                	private void Flare5()
                    {
                    if ( Caster == null )
                    {
                        targ.Frozen = false;
                        targ.SolidHueOverride = -1;
                    return;
                    }

                    if ( targ.Alive == false )
                       {
                        targ.Frozen = false;
                        targ.SolidHueOverride = -1;
                    return;
                    }                    


                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 10, false, 0, 0, 100, 0, 0, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 20, false, 0, 0, 100, 0, 0, 0, 0, 0, false);

                    

                    return;
                }
        private void Flare6()
                    {
                 targ.Frozen = false;
            targ.SolidHueOverride = -1;    




                    

                    return;
                }
				}
			}
	

