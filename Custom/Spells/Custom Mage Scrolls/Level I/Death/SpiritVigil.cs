using System;
using Server.Targeting;
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
    public class SpiritVigilScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new SpiritVigilSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public SpiritVigilScroll() : base()
        {
            Hue = 2687;
            Name = "A Spirit Vigil scroll";
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

            BaseCustomSpell.SpellInitiator( new SpiritVigilSpell( m, 1 ) );
        }

        public SpiritVigilScroll( Serial serial )
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
    public class SpiritVigilSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new SpiritVigilSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return false; } }
		public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Spirit Vigil"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 0; } }

        public SpiritVigilSpell()
            : this( null, 1 )
        {
        }

        public SpiritVigilSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6067;
            Range = 12;
            CustomName = "Spirit Vigil";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{  FeatList.DeathI } );
            }
        }
		
        public override void Effect()
        {		
			
            if( CasterHasEnoughMana && Caster.Followers < 4 )
            {
				Caster.Mana -= TotalCost;
				Success = true;
				int tx = Caster.Location.X;
				int ty = Caster.Location.Y;
				int tz = Caster.Location.Z;
				Caster.PlaySound(586);
				Caster.SendMessage("You summon forth an invisible spirit to guard the area for the next few hours...");
				Point3D loc = new Point3D( tx, ty, tz);
				SpiritVigilTrap trap = new SpiritVigilTrap(Caster);
				trap.MoveToWorld( loc, Caster.Map );
				Effects.SendLocationParticles( trap, 0x376A, 9, 10, 5025 );
                Caster.Followers += 2;

	
			}
        }
	}
}