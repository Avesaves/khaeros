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
    public class FadeScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new FadeSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public FadeScroll() : base()
        {
            Hue = 2980;
            Name = "A Fade scroll";
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

            BaseCustomSpell.SpellInitiator( new FadeSpell( m, 1 ) );
        }

        public FadeScroll( Serial serial )
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
    public class FadeSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new FadeSpell();
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
        public override string Name { get { return "Fade"; } }
        public override int ManaCost { get { return 30; } }
        public override int BaseRange { get { return 0; } }

        public FadeSpell()
            : this( null, 1 )
        {
        }

        public FadeSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6027;
            Range = 12;
            CustomName = "Fade";
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
			
            if( CasterHasEnoughMana )
            {
				Caster.Mana -= TotalCost;
				Success = true;
				Caster.FixedParticles( 0x376a, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot );
				Caster.PlaySound( 0x3C4 );
				Caster.Emote( "*Fades away...*" );
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
