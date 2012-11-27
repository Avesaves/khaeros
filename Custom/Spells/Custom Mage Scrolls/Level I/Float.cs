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
    public class FloatScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new FloatSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public FloatScroll() : base()
        {
            Hue = 2687;
            Name = "A Float scroll";
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

            BaseCustomSpell.SpellInitiator( new FloatSpell( m, 1 ) );
        }

        public FloatScroll( Serial serial )
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
    public class FloatSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new FloatSpell();
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
        public override string Name { get { return "Float"; } }
        public override int ManaCost { get { return 10; } }
        public override int BaseRange { get { return 12; } }

        public FloatSpell()
            : this( null, 1 )
        {
        }

        public FloatSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6059;
            Range = 12;
            CustomName = "Float";
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
				
            if( TargetCanBeAffected && CasterHasEnoughMana && TargetItem is Item && TargetItem.Movable != false )
            {
				Caster.Mana -= TotalCost;
				Success = true;
				Map map = TargetItem.Map;
				Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z + 1 );
				TargetItem.MoveToWorld( move, map );
				TargetItem.Movable = false;
				TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*rises in mid-air*" );
				Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerCallback( Flare1 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( Flare2 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 3 ), new TimerCallback( Flare3 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 4 ), new TimerCallback( Flare4 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 5 ), new TimerCallback( Flare5 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 10 ), new TimerCallback( Flare ) );
            }
        }
		
		private void Flare1()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 Map map = TargetItem.Map;
			 Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z + 1 );
			 TargetItem.MoveToWorld( move, map );
		}	

		private void Flare2()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 Map map = TargetItem.Map;
			 Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z + 1 );
			 TargetItem.MoveToWorld( move, map );
		}		

		private void Flare3()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 Map map = TargetItem.Map;
			 Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z + 1 );
			 TargetItem.MoveToWorld( move, map );
		}		

		private void Flare4()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 Map map = TargetItem.Map;
			 Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z + 1 );
			 TargetItem.MoveToWorld( move, map );
		}			
		
		private void Flare5()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;

			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*seems frozen in mid-air*" );
		}	
		
		private void Flare()
		{
			if ( Caster == null )
				return;
				
			if (TargetItem == null || TargetItem.Deleted)
				return;
				
			 Map map = TargetItem.Map;
			 TargetItem.Movable = true;
			 Point3D move = new Point3D( TargetItem.X + 0, TargetItem.Y + 0, TargetItem.Z - 5 );
			 TargetItem.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*drops to the ground*" );
			 TargetItem.MoveToWorld( move, map );
		}				
	}
}