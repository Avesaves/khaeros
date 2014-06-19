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
    public class TelepathyScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new TelepathySpell();
            }
            set
            {
            }
        }

        [Constructable]
        public TelepathyScroll()
            : base()
        {
            Hue = 2659;
            Name = "A Telepathy scroll";
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

            BaseCustomSpell.SpellInitiator( new TelepathySpell( m, 1 ) );
        }

        public TelepathyScroll( Serial serial )
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
    public class TelepathySpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new TelepathySpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Telepathy"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 0; } }

        public TelepathySpell()
            : this( null, 1 )
        {
        }

        public TelepathySpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6018;
            Range = 0;
            CustomName = "Telepathy";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] {  FeatList.MindI  } );
            }
        }

        public override void Effect()
        {
            if( CasterHasEnoughMana )
            {
                Caster.Mana -= TotalCost;
                Caster.SendMessage("You feel your mind expand outwards...");
                Caster.SendMessage("You may now use .Telepathy for thirty minutes."); 
                XmlData telep = new Engines.XmlSpawner2.XmlData("Telepathy", "this is necessary for telepathy to work", 30);
                Engines.XmlSpawner2.XmlAttach.AttachTo(Caster, telep);
                Success = true;
            }

            else
                Caster.SendMessage( "You do not have enough mana to cast this spell!" );
        }
    }
}
