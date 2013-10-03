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
    public class MagelightScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new MagelightSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public MagelightScroll()
            : base()
        {
            Hue = 2877;
            Name = "A Magelight scroll";
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

            BaseCustomSpell.SpellInitiator( new MagelightSpell( m, 1 ) );
        }

        public MagelightScroll( Serial serial )
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
    public class MagelightSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new MagelightSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Magelight"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 0; } }

        public MagelightSpell()
            : this( null, 1 )
        {
        }

        public MagelightSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6154;
            Range = 0; 
            CustomName = "Magelight";
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
            if( Caster.Followers < Caster.FollowersMax && CasterHasEnoughMana )
            {
                Caster.Mana -= TotalCost;
                WanderingSpirit horse = new WanderingSpirit();
                Caster.Emote("*A glowing light seperates itself from " + Caster.Name + "'s body*");
                horse.Hue = 2877;
                horse.Name = "a living lantern";
                horse.Blessed = true;
                horse.EquipItem( new LightSource() );
                Summon( Caster, horse, 30, 535 );
                Success = true;
            }

            else
                Caster.SendMessage( "You do not have enough follower slots to do that." );
        }
    }
}
