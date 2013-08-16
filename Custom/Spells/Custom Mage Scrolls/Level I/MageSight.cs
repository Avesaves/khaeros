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
    public class MageSightScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new MageSightSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public MageSightScroll() : base()
        {
            Hue = 2687;
            Name = "A Mage Sight scroll";
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

            BaseCustomSpell.SpellInitiator( new MageSightSpell( m, 1 ) );
        }

        public MageSightScroll( Serial serial )
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
    public class MageSightSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new MageSightSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return false; } }
		public override bool UsesTarget{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Mage Sight"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }

        public MageSightSpell()
            : this( null, 1 )
        {
        }

        public MageSightSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6018;
            Range = 12;
            CustomName = "Mage Sight";
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
            if( TargetCanBeAffected && CasterHasEnoughMana )
            {
                TargetMobile.PlaySound( 483 );
                Caster.Mana -= TotalCost;
                XmlAttach.AttachTo( TargetMobile, new XmlMageSight( 0, 60 ) );
                Success = true;
            }
        }
    }
}
