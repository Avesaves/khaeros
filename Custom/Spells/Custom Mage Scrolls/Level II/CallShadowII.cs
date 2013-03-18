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
    public class CallShadowIIScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new CallShadowIISpell();
            }
            set
            {
            }
        }

        [Constructable]
        public CallShadowIIScroll() : base()
        {
            Hue = 2990;
            Name = "A Call Shadow II scroll";
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

            BaseCustomSpell.SpellInitiator( new CallShadowIISpell( m, 1 ) );
        }

        public CallShadowIIScroll( Serial serial )
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
    public class CallShadowIISpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new CallShadowIISpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsItems { get { return false; } }
		public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Call Shadow II"; } }
        public override int ManaCost { get { return 30; } }
        public override int BaseRange { get { return 0; } }

        public CallShadowIISpell()
            : this( null, 1 )
        {
        }

        public CallShadowIISpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6078;
            Range = 12;
            CustomName = "Call Shadow II";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.MatterII } );
            }
        }
		
        public override void Effect()
        {		
			
            if( CasterHasEnoughMana )
            {
				Caster.Mana -= TotalCost;
				Success = true;
				Caster.FixedParticles( 0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot );
				Caster.PlaySound( 0x22F );
				Caster.Emote( "*is engulfed in shadows*" );
				Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( Flare1 ) );
            }
        }
		
		private void Flare1()
		{
			if ( Caster == null )
				return;
				
			Caster.Hidden = true;
		}	
	}
}
