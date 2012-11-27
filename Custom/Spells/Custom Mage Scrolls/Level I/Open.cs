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
    public class OpenScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new OpenSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public OpenScroll() : base()
        {
            Hue = 2687;
            Name = "A Open scroll";
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

            BaseCustomSpell.SpellInitiator( new OpenSpell( m, 1 ) );
        }

        public OpenScroll( Serial serial )
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
    public class OpenSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new OpenSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
		public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Open"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }

        public OpenSpell()
            : this( null, 1 )
        {
        }

        public OpenSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6069;
            Range = 12;
            CustomName = "Open";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.ForcesI } );
            }
        }
		
        public override void Effect()
        {		
			if (TargetItem.Parent is Mobile)
			{
				Caster.SendMessage("You cannot use that on an equipped item.");
				Success = false;
				return;
			}
			
			if (TargetItem.IsChildOf( Caster.Backpack ))
			{
				Caster.SendMessage("You cannot use that on an item in your pack.");
				Success = false;
				return;
			}
				
            if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem is BaseDoor && TargetItem.Movable != true )
            {
				BaseDoor door = TargetItem as BaseDoor;
				Caster.Mana -= TotalCost;
				
				if (door.Locked == true)
				{
				Caster.SendMessage("The door doesn't budge; it appears to be locked.");
				Success = false;
				return;
				}
				
				if (door.Open == false)
				{
				door.Open = true;
				Success = true;
				Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerCallback( Flare ) );
				return;
				}
				
				if (door.Open == true)
				{
				door.Open = false;
				Success = true;
				Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerCallback( Flare ) );
				return;
				}
            }
        }
		
		private void Flare()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*shakes slightly*" );

		}				
	}
}