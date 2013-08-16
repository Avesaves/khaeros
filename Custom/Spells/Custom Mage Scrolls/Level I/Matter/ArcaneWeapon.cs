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
    public class ArcaneWeaponScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ArcaneWeaponSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ArcaneWeaponScroll()
            : base()
        {
            Hue = 2990;
            Name = "An Arcane Weapon II scroll";
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

            BaseCustomSpell.SpellInitiator( new ArcaneWeaponSpell( m, 1 ) );
        }

        public ArcaneWeaponScroll( Serial serial )
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
    public class ArcaneWeaponSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ArcaneWeaponSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Arcane Weapon II"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 0; } }

        public ArcaneWeaponSpell()
            : this( null, 1 )
        {
        }

        public ArcaneWeaponSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6023;
            Range = 0;
            CustomName = "Arc. Weap. II";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] {  
				FeatList.MatterI } );
            }
        }

        public override void Effect()
        {
		if( CasterHasEnoughMana )
		{
            Caster.Mana -= TotalCost;
            Caster.PlaySound( 655 );
			Caster.Emote ("*shadows form in their hand, taking the form of a blade*");
            Caster.ClearHands();
            ArcaneWeapon weapon = new ArcaneWeapon();
            weapon.DelayDelete();
            Caster.EquipItem( weapon );
            Success = true;
		}
        }
    }
}
