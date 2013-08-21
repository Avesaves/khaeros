using System;
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
    public class EnergyVampireScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new EnergyVampireSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public EnergyVampireScroll() : base()
        {
            Hue = 2968;
            Name = "A Energy Vampire scroll";
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

            BaseCustomSpell.SpellInitiator( new EnergyVampireSpell( m, 1 ) );
        }

        public EnergyVampireScroll( Serial serial )
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
    public class EnergyVampireSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new EnergyVampireSpell();
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
        public override string Name { get { return "Energy Vampire"; } }
        public override int ManaCost { get { return 0; } }
        public override int BaseRange { get { return 12; } }

        public EnergyVampireSpell()
            : this( null, 1 )
        {
        }

        public EnergyVampireSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6160;
            Range = 12;
            CustomName = "Energy Vampire";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.ForcesI } );
            }
        }


        public override void Effect()
        {		
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{

                    Mobile targ = TargetMobile as Mobile;


                    
                    Caster.Mana -= TotalCost;
                    Success = true;

                    Caster.PlaySound(254);
                //:(
                    
                    
                    Caster.Emote("*Glows with stolen power...*");
                    Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(Flare1));
                Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Flare2));
                Timer.DelayCall(TimeSpan.FromSeconds(8), new TimerCallback(Flare3));
                Timer.DelayCall(TimeSpan.FromSeconds(11), new TimerCallback(Flare4));
                Timer.DelayCall(TimeSpan.FromSeconds(14), new TimerCallback(Flare5));

            }
        }

                	private void Flare1()
                    {
                        Mobile targ = TargetMobile as Mobile;
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;

                    targ.MovingParticles(Caster, 0x37B9, 7, 0, false, false, 0, 0, 3043, 4043, 0x211, 0x100);
                    targ.SendMessage("You feel your lifeforce slipping away!");
                    Caster.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                    targ.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                    Caster.Emote("*Glows...*");
                    if (Caster.Mana < Caster.RawMana && targ.Mana >= 10)
                    {
                        targ.Mana -= 10;
                        Caster.Mana += 10;
                        Caster.Stam -= 10;
                    }
                    return;

                }	
                        	private void Flare2()
                    {
                        Mobile targ = TargetMobile as Mobile;
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;


                targ.MovingParticles(Caster, 0x37B9, 7, 0, false, false, 0, 0, 3043, 4043, 0x211, 0x100);

                Caster.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                targ.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                Caster.Emote("*Glows...*");
                if (Caster.Mana < Caster.RawMana && targ.Mana >= 10)
                {
                    targ.Mana -= 10;
                    Caster.Mana += 10;
                    Caster.Stam -= 10;
                }
                return;
                }
                        	private void Flare3()
                    {
                        Mobile targ = TargetMobile as Mobile;
                    if ( Caster == null )
                    return;

                    if ( targ.Alive == false )
                    return;


                targ.MovingParticles(Caster, 0x37B9, 7, 0, false, false, 0, 0, 3043, 4043, 0x211, 0x100);

                Caster.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                targ.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                Caster.Emote("*Glows...*");
                if (Caster.Mana < Caster.RawMana && targ.Mana >= 10)
                {
                    targ.Mana -= 10;
                    Caster.Mana += 10;
                    Caster.Stam -= 10;
                }
                return;
                }
                                	private void Flare4()
                    {
                        Mobile targ = TargetMobile as Mobile;
                        if (Caster == null)
                            return;
                   

                    if ( targ.Alive == false )
                        return;



                    targ.MovingParticles(Caster, 0x37B9, 7, 0, false, false, 0, 0, 3043, 4043, 0x211, 0x100);

                    Caster.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                    targ.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                    Caster.Emote("*Glows...*");
                    if (Caster.Mana < Caster.RawMana && targ.Mana >= 10)
                    {
                        targ.Mana -= 10;
                        Caster.Mana += 10;
                        Caster.Stam -= 10;
                    }
                    return;
                }

                                	private void Flare5()
                    {
                        Mobile targ = TargetMobile as Mobile;
                        if (Caster == null)
                            return;


                        if (targ.Alive == false)
                            return;



                        targ.MovingParticles(Caster, 0x37B9, 7, 0, false, false, 0, 0, 3043, 4043, 0x211, 0x100);



                        Caster.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                        targ.FixedParticles(0x3967, 244, 25, 9950, 0, 0, EffectLayer.Waist);
                        Caster.Emote("*Glows...*");
                        if (Caster.Mana < Caster.RawMana && targ.Mana >= 10)
                        {
                            targ.Mana -= 10;
                            Caster.Mana += 10;
                            Caster.Stam -= 10;
                        }
                        return;
                }

				}
			}
	

