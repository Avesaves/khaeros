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
    public class ExplodeCorpseScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ExplodeCorpseSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ExplodeCorpseScroll() : base()
        {
            Hue = 2687;
            Name = "An Explode Corpse scroll";
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

            BaseCustomSpell.SpellInitiator( new ExplodeCorpseSpell( m, 1 ) );
        }

        public ExplodeCorpseScroll( Serial serial )
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
    public class ExplodeCorpseSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ExplodeCorpseSpell();
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
        public override string Name { get { return "Explode Corpse"; } }
        public override int ManaCost { get { return 25; } }
        public override int BaseRange { get { return 12; } }

        public ExplodeCorpseSpell()
            : this( null, 1 )
        {
        }

        public ExplodeCorpseSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6153;
            Range = 12;
            CustomName = "Explode Corpse";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.DeathI } );
            }
        }
		
        public override void Effect()
        {		
			if (TargetItem is Corpse && CasterHasEnoughMana )
			{
				Corpse cp = TargetItem as Corpse;

				if ( cp.Owner is PlayerMobile  )
				{
					Caster.SendMessage("You cannot use this spell on player corpses.");
					return;
				}
				
				Caster.Mana -= TotalCost;
				Success = true;
				Blood bd = new Blood();
				Map map = cp.Map;
				Point3D m_loc = new Point3D( cp.X, cp.Y, cp.Z );
				bd.MoveToWorld( m_loc, map );

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
							mb.Emote ("*is enveloped with corpse gasses*");
							mb.FixedEffect( 0x376A, 1, 12, 1685, 30 ); // At player
                            if (mb is PlayerMobile)
							    AOS.Damage( mb, Caster, Utility.RandomMinMax( 25, 50 ), 0, 0, 0, 100, 0, 0, 0, 0 );
                            else
                                AOS.Damage(mb, Caster, Utility.RandomMinMax(50, 100), 0, 0, 0, 100, 0, 0, 0, 0);
						}

						Effects.SendLocationEffect( m_loc, map, 0x3915, 17 );
						Caster.PlaySound( 560 );
						cp.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*releases a cloud of noxious fumes*" );
						return;
				}
			}
	}
}