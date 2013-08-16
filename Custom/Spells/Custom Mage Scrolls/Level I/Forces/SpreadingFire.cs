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
    public class SpreadingFireScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new SpreadingFireSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public SpreadingFireScroll() : base()
        {
            Hue = 2778;
            Name = "A Spreading fire scroll";
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

            BaseCustomSpell.SpellInitiator( new SpreadingFireSpell( m, 1 ) );
        }

        public SpreadingFireScroll( Serial serial )
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
    public class SpreadingFireSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new SpreadingFireSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
		public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return true; } }
        public override bool UsesTarget { get { return true; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Spreading Fire"; } }
        public override int ManaCost { get { return 150; } }
        public override int BaseRange { get { return 12; } }

        public SpreadingFireSpell()
            : this( null, 1 )
        {
        }

        public SpreadingFireSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6184;
            Range = 12;
            CustomName = "Spreading Fire";
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
			if (TargetMobile is Mobile && CasterHasEnoughMana )
			{
				Mobile cp = TargetMobile as Mobile;
				BaseCreature vp = TargetMobile as BaseCreature;

				if ( cp.BodyValue != 15  )
				{
					Caster.SendMessage("You cannot use this spell on this.");
					return;
				}
				
				Caster.Mana -= TotalCost;
				Success = true;
                
				Map map = cp.Map;
				Point3D m_loc = new Point3D( cp.X, cp.Y, cp.Z );
				

						ArrayList targets = new ArrayList();

						IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( m_loc ), 4 );

						foreach ( Mobile md in eable )
						{
							if( md != Caster && md.AccessLevel < AccessLevel.GameMaster )
								targets.Add( md );
						}
						for ( int i = 0; i < targets.Count; ++i )
						{
							Mobile mb = (Mobile)targets[i];
							mb.DoHarmful( mb );
							mb.Emote ("*Is engulfed by fire!*");
							mb.FixedEffect( 0x398C, 0, 30, 0, 0 ); // At player
                            if (mb is PlayerMobile)
                                AOS.Damage(mb, Caster, Utility.RandomMinMax(50, 120), 0, 100, 0, 0, 0, 0, 0, 0);
                            else
							    AOS.Damage( mb, Caster, Utility.RandomMinMax( 100, 200 ), 0, 100, 0, 0, 0, 0, 0, 0 );
						}

						Effects.SendLocationEffect( m_loc, map, 0x36A0, 17 );
						Caster.PlaySound( 560 );
						cp.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*Expands outwards in a field of flames*" );
                        if (vp.Controlled = true)
                            vp.Kill();
						return;
				}
			}
	}
}
