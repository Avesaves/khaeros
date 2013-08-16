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
    public class ExtendBoneScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ExtendBoneSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ExtendBoneScroll()
            : base()
        {
            Hue = 834;
            Name = "An Extend Bones Scroll";
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

            BaseCustomSpell.SpellInitiator( new ExtendBoneSpell( m, 1 ) );
        }

        public ExtendBoneScroll( Serial serial )
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
    public class ExtendBoneSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ExtendBoneSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Extend Bones"; } }
        public override int ManaCost { get { return 80; } }
        public override int BaseRange { get { return 0; } }

        public ExtendBoneSpell()
            : this( null, 1 )
        {
        }

        public ExtendBoneSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6075;
            Range = 0;
            CustomName = "Extend Bones";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { 
				FeatList.MatterI
				} );
            }
        }

        public override void Effect()
        {
		 if( CasterHasEnoughMana )
		 {
             

            Item gloves = Caster.FindItemOnLayer(Layer.Gloves);

            if (gloves != null)
                 Caster.Backpack.DropItem(gloves);

            Caster.Mana -= TotalCost;
            Caster.PlaySound(86);
            Caster.Emote("*monstrous claws grow from the tips of their fingers!*");
            Caster.ClearHands();
            VampireClaws claws = new VampireClaws();
            claws.DelayDelete();
            Caster.EquipItem( claws );
            Success = true;
			}
        }
    }
}
