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
    public class SilverBulletScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new SilverBulletSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public SilverBulletScroll() : base()
        {
            Hue = 2985;
            Name = "A Silver Bullet scroll";
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

            BaseCustomSpell.SpellInitiator( new SilverBulletSpell( m, 1 ) );
        }

        public SilverBulletScroll( Serial serial )
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
    public class SilverBulletSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new SilverBulletSpell();
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
        public override string Name { get { return "Silver Bullet"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }

        public SilverBulletSpell()
            : this( null, 1 )
        {
        }

        public SilverBulletSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6108;
            Range = 12;
            CustomName = "Silver Bullet";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.MindI } );
            }
        }
		public virtual bool ConsumeReagents()
        {
            
                Container pack = Caster.Backpack;

                if (pack == null)
                    return false;

                if (pack.ConsumeTotal(typeof(Silver), 1))
                    return true;
                return false;
                
               

         }

        public override void Effect()
        {		
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{

                if (!ConsumeReagents())
                {
                    Caster.Emote("*digs in {0} pocket, but finds nothing usable...*", Caster.Female == true ? "her" : "his", Caster.Name);
                    Success = false;
                }
                else
                {

                    Mobile targ = TargetMobile as Mobile;


                    Caster.Mana -= TotalCost;
                    Success = true;

                    Caster.PlaySound(0x2E6);

                    Caster.MovingParticles(targ, 0xF1B, 7, 0, false, true, 3043, 4043, 0x211);
                    targ.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);
                    if ( targ is IUndead )
                        AOS.Damage(targ, Caster, 150, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                    else if (targ is PlayerMobile )
                        AOS.Damage(targ, Caster, 50, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                    else
                        AOS.Damage(targ, Caster, 100, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                    Caster.Emote("*magically sends a silver coin flying through the air!*");

                    return;
                }

				}
			}
	}
}
