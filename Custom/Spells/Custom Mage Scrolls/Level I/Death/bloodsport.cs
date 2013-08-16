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
    public class BloodSportScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new BloodSportSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public BloodSportScroll() : base()
        {
            Hue = 37;
            Name = "A Blood Sport scroll";
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

            BaseCustomSpell.SpellInitiator( new BloodSportSpell( m, 1 ) );
        }

        public BloodSportScroll( Serial serial )
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
    public class BloodSportSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new BloodSportSpell();
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
        public override string Name { get { return "Blood Sport"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 12; } }

        public BloodSportSpell()
            : this( null, 1 )
        {
        }

        public BloodSportSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6166;
            Range = 12;
            CustomName = "Blood Sport";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.DeathI } );
            }
        }
		
        public override void Effect()
        {		
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{
				Mobile targ = TargetMobile as Mobile;

				
				Caster.Mana -= TotalCost;
				Success = true;

                Caster.PlaySound(0x175);

                Caster.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
                Caster.FixedParticles(0x3728, 1, 13, 9502, 33, 7, (EffectLayer)255);

                targ.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
                targ.FixedParticles(0x3728, 1, 13, 9502, 33, 7, (EffectLayer)255);
                if (targ is PlayerMobile)
                    AOS.Damage(targ, Caster, 50, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                else
                    AOS.Damage(targ, Caster, 100, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
                if (targ.Hits > 25)
                    Caster.Hits += 25;
                Caster.Emote("*looks healthier*");
                targ.Emote("*gasps in pain*");
						return;
				}
			}
	}
}
