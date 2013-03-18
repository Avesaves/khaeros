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
    public class DecoyIII : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new DecoyIIISpell();
            }
            set
            {
            }
        }

        [Constructable]
        public DecoyIII()
            : base()
        {
            Hue = 2886;
            Name = "A Decoy III scroll";
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

            BaseCustomSpell.SpellInitiator( new DecoyIIISpell( m, 1 ) );
        }

        public DecoyIII( Serial serial )
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
    public class DecoyIIISpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new DecoyIIISpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Decoy III"; } }
        public override int ManaCost { get { return 25; } }
        public override int BaseRange { get { return 0; } }

        public DecoyIIISpell()
            : this( null, 1 )
        {
        }

        public DecoyIIISpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6128;
            Range = 0;
            CustomName = "Decoy III";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { FeatList.MindII } );
            }
        }

        public override void Effect()
        {
            if( CasterHasEnoughMana )
            {
                Caster.Mana -= TotalCost;
				new Clone( Caster ).MoveToWorld( Caster.Location, Caster.Map );
				Caster.Hidden = true;
                Success = true;
            }

            else
                Caster.SendMessage( "You do not have enough mana to cast this spell!" );
        }
    }
}
