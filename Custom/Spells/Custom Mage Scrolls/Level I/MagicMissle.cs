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
    public class MagicMissleScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new MagicMissleSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public MagicMissleScroll() : base()
        {
            Hue = 2624;
            Name = "A Magic Missile scroll";
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

            BaseCustomSpell.SpellInitiator( new MagicMissleSpell( m, 1 ) );
        }

        public MagicMissleScroll( Serial serial )
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
    public class MagicMissleSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new MagicMissleSpell();
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
        public override string Name { get { return "Magic Missle"; } }
        public override int ManaCost { get { return 30; } }
        public override int BaseRange { get { return 12; } }

        public MagicMissleSpell()
            : this( null, 1 )
        {
        }

        public MagicMissleSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6030;
            Range = 12;
            CustomName = "Magic Missile";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.Magery } );
            }
        }

        public override void Effect()
        {		
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{

                    Mobile targ = TargetMobile as Mobile;


                    Caster.Mana -= TotalCost;
                    Success = true;

                    Caster.PlaySound(513);

                    Caster.MovingParticles(targ, 0x36D4, 7, 0, false, true, 2624, 0, 3043, 4043, 0x211, 0x100);
                    Caster.FixedParticles(0x375A, 244, 25, 9950, 2624, 0, EffectLayer.Waist);
                    if ( targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 45, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                        //phys fire cold pois energy, blunt slash pierce 
                    else
                        AOS.Damage(targ, Caster, 90, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                    Caster.Emote("*Fires a bolt of energy!*");

                    return;
                }

				}
			}
	}

