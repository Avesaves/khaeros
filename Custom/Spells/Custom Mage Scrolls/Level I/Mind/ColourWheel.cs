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
    public class ColourWheelScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ColourWheelSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ColourWheelScroll() : base()
        {
            Hue = 200;
            Name = "A ColourWheel scroll";
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

            BaseCustomSpell.SpellInitiator( new ColourWheelSpell( m, 1 ) );
        }

        public ColourWheelScroll( Serial serial )
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
    public class ColourWheelSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ColourWheelSpell();
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
        public override string Name { get { return "ColourWheel"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }

        public ColourWheelSpell()
            : this( null, 1 )
        {
        }

        public ColourWheelSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6087;
            Range = 12;
            CustomName = "ColourWheel";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.MindI } );
            }
        }
		
        public override void Effect()
        {				
			if (TargetItem.IsChildOf( Caster.Backpack ))
			{
				Caster.SendMessage("You cannot use that on an item in your pack.");
				Success = false;
				return;
			}
				
            if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem is Item && TargetItem.Movable != false )
            {
				Caster.Mana -= TotalCost;
				Success = true;
                Random random = new Random();
                int randomNumber = random.Next(1, 200);
                TargetItem.HueMod = randomNumber;
                Caster.Emote("*{0} eyes flash a strange colour*", Caster.Female == true ? "her" : "his");
				TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*Shimmers as it changes colour*" );
				Timer.DelayCall( TimeSpan.FromSeconds( 60 ), new TimerCallback( Flare ) );
            }
        }
				
		private void Flare()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 TargetItem.HueMod = -1;
			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*returns to its ordinary colour*" );

		}				
	}
}