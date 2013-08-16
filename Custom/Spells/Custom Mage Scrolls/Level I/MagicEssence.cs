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
    public class MagicEssenceScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new MagicEssenceSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public MagicEssenceScroll()
            : base()
        {
            Hue = 2687;
            Name = "A Magic Essence scroll";
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

            BaseCustomSpell.SpellInitiator( new MagicEssenceSpell( m, 1 ) );
        }

        public MagicEssenceScroll( Serial serial )
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
    public class MagicEssenceSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new MagicEssenceSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Magic Essence"; } }
        public override int ManaCost { get { return 0; } }
        public override int BaseRange { get { return 0; } }

        public MagicEssenceSpell()
            : this( null, 1 )
        {
        }

        public MagicEssenceSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6028;
            Range = 0;
            CustomName = "Magic Essence";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { FeatList.Magery } );
            }
        }

        public override void Effect()
        {
            if( Caster.Stam > 20 && Caster.Hits > 20 && CasterHasEnoughMana )
            {
                Caster.Mana -= TotalCost;
				Caster.Stam -= 25;
				Caster.Hits -= 25;
				Caster.Mana += 25;
				Caster.SendMessage( "You sacrifice some of your own vitality to summon forth the essences of magic!" );
                Caster.FixedParticles( 0x376A, 1, 14, 0x13B5, EffectLayer.Waist );
				Caster.PlaySound( 0x491 );
                Success = true;
            }

            else
                Caster.SendMessage( "You do not have enough stamina or hit points to cast this spell!" );
        }
    }
}
