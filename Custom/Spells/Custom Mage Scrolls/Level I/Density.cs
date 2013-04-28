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
    public class DensityScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new DensitySpell();
            }
            set
            {
            }
        }

        [Constructable]
        public DensityScroll() : base()
        {
            Hue = 2687;
            Name = "A Density scroll";
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

            BaseCustomSpell.SpellInitiator( new DensitySpell( m, 1 ) );
        }

        public DensityScroll( Serial serial )
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
    public class DensitySpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new DensitySpell();
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
        public override string Name { get { return "Density"; } }
        public override int ManaCost { get { return 20; } }
        public override int BaseRange { get { return 12; } }

        public DensitySpell()
            : this( null, 1 )
        {
        }

        public DensitySpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6141;
            Range = 12;
            CustomName = "Density";
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
			if (TargetItem.Parent is PlayerMobile && CasterHasEnoughMana )
			{
				
				if (TargetItem.Weight >= 100)
				{
				Caster.SendMessage("That seems too heavy to affect.");
				return;
				}
				
				if (TargetItem.Weight <= 99)
				{
				PlayerMobile target = TargetItem.Parent as PlayerMobile;
				Caster.Mana -= TotalCost;
				TargetItem.Weight = (TargetItem.Weight + 50);
				TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*glows briefly*" );
				Timer.DelayCall( TimeSpan.FromSeconds( 10 ), new TimerCallback( Flare ) );
				Success = true;
				if (TargetItem.Name != null)
				target.SendMessage("Your " + TargetItem.Name + " seems to suddenly become very heavy!");
				if (TargetItem.Name == null)
				target.SendMessage("You feel as if some of your equipment has become very heavy!");
				return;
				}
			}
				
            if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem.Movable != false )
            {
				if (TargetItem.Weight >= 100)
				{
				Caster.SendMessage("That seems too heavy to affect.");
				return;
				}
				
				
				if (TargetItem.Weight <= 99)
				{
				Caster.Mana -= TotalCost;
				TargetItem.Weight = (TargetItem.Weight + 50);
				TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*glows briefly*" );
				Timer.DelayCall( TimeSpan.FromSeconds( 60 ), new TimerCallback( Flare ) );
				Success = true;
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
				
			 TargetItem.Weight = (TargetItem.Weight - 50);	
			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*glows briefly*" );
		}				
	}
}