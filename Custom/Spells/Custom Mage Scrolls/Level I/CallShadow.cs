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
    public class CallShadowScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new CallShadowSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public CallShadowScroll() : base()
        {
            Hue = 2687;
            Name = "A Call Shadow scroll";
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

            BaseCustomSpell.SpellInitiator( new CallShadowSpell( m, 1 ) );
        }

        public CallShadowScroll( Serial serial )
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
    public class CallShadowSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new CallShadowSpell();
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
        public override string Name { get { return "Call Shadow"; } }
        public override int ManaCost { get { return 35; } }
        public override int BaseRange { get { return 0; } }

        public CallShadowSpell()
            : this( null, 1 )
        {
        }

        public CallShadowSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6078;
            Range = 12;
            CustomName = "Call Shadow";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.ForcesI, FeatList.MatterI } );
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
				Timer.DelayCall( TimeSpan.FromSeconds( 7 ), new TimerCallback( Flare2 ) );
            }
        }
		
		private void Flare1()
		{
			if ( Caster == null )
				return;
				
			Caster.Hidden = true;
		}	

		private void Flare2()
		{
			if ( Caster == null )
				return;
				
			if ( Caster.Hidden)
			{
			Caster.Hidden = false;
			Caster.Emote( "*shadows dissipate*" );
			Caster.FixedParticles( 0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot );
			}
			
		}		
	}
}