﻿using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Engines.Craft;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChangeMetalIScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ChangeMetalISpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ChangeMetalIScroll() : base()
        {
            Hue = 2832;
            Name = "A Change Metal I scroll";
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

            BaseCustomSpell.SpellInitiator( new ChangeMetalISpell( m, 1 ) );
        }

        public ChangeMetalIScroll( Serial serial )
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
    public class ChangeMetalISpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ChangeMetalISpell();
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
        public override string Name { get { return "Change Metal I"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }
        

        public ChangeMetalISpell()
            : this( null, 1 )
        {
        }

        public ChangeMetalISpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6107;
            Range = 12;
            CustomName = "Change Metal I";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.MatterI } );
            }
        }
        public static string reso = TargetItem.CraftResource; 
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
                Item sword = TargetItem as Item;

                
                Caster.Emote("*{0} hand glows a strange metallic blue colour*", Caster.Female == true ? "her" : "his");
				TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*Shimmers as its composition is altered*" );
                TargetItem.Movable = false;
                sword.CraftResource = CraftResource.Starmetal; 
				Timer.DelayCall( TimeSpan.FromSeconds( 360 ), new TimerCallback( Flare ) );
            }
        }
				
		private void Flare()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 
			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*returns to its ordinary material*" );
             TargetItem.Movable = true;
             TargetItem.CraftResource = reso;   

		}				
	}
}