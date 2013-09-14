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
    public class SpiritSteedScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new SpiritSteedSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public SpiritSteedScroll()
            : base()
        {
            Hue = 2687;
            Name = "A Spirit Steed scroll";
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

            BaseCustomSpell.SpellInitiator( new SpiritSteedSpell( m, 1 ) );
        }

        public SpiritSteedScroll( Serial serial )
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
    public class SpiritSteedSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new SpiritSteedSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Spirit Steed"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 0; } }

        public SpiritSteedSpell()
            : this( null, 1 )
        {
        }

        public SpiritSteedSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6063;
            Range = 0;
            CustomName = "Spirit Steed";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { FeatList.MindI } );
            }
        }

        public override void Effect()
        {
            if( Caster.Followers < Caster.FollowersMax && CasterHasEnoughMana )
            {
                Caster.Mana -= TotalCost;
                RoseanHorse horse = new RoseanHorse();
                horse.Hue = 12345678;
                horse.Name = "a spirit steed";
                Summon( Caster, horse, 60, 535 );
                Success = true;
            }

            else
                Caster.SendMessage( "You do not have enough follower slots to do that." );
        }
    }
}
