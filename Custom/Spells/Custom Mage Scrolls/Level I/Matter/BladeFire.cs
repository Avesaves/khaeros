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
    public class BladeFireScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new BladeFireSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public BladeFireScroll()
            : base()
        {
            Hue = 2687;
            Name = "An Blade Fire scroll";
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

            BaseCustomSpell.SpellInitiator( new BladeFireSpell( m, 1 ) );
        }

        public BladeFireScroll( Serial serial )
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
    public class BladeFireSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new BladeFireSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Blade Fire"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 0; } }

        public BladeFireSpell()
            : this( null, 1 )
        {
        }

        public BladeFireSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6117;
            Range = 0;
            CustomName = "Blade Fire";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { 
				FeatList.MatterI
				} );
            }
        }

        public override void Effect()
        {
		
			Item ArcWeap = Caster.FindItemOnLayer( Layer.FirstValid ); 
			Item BurnTorch = Caster.FindItemOnLayer( Layer.TwoHanded );
			
		if (CasterHasEnoughMana)
		{
			
			if (BurnTorch is Torch && (ArcWeap is ArcaneWeapon || ArcWeap is LesserArcaneWeapon))
			{
					ArcWeap.Delete();
					Caster.Mana -= TotalCost;
					Caster.PlaySound( 838 );
					Caster.Emote ("*holds their blade into the burning flame*");
				
					if (ArcWeap is ArcaneWeapon)
					{
					FieryArcaneWeapon weapon = new FieryArcaneWeapon();
					weapon.DelayDelete();
					Caster.EquipItem( weapon );
					Success = true;
					}
				
					if (ArcWeap is LesserArcaneWeapon)
					{
					FieryLesserArcaneWeapon weapon = new FieryLesserArcaneWeapon();
					weapon.DelayDelete();
					Caster.EquipItem( weapon );
					Success = true;
					}
			}
			
			else
			{
			Success = false;	
			Caster.SendMessage ("You need a torch and an arcane weapon in your hands to complete this incantation.");
			return;
			}
		}
		}
    }
}

