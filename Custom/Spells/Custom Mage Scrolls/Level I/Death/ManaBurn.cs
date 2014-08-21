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
    public class ManaBurnScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ManaBurnSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ManaBurnScroll() : base()
        {
            Hue = 2937;
            Name = "A Mana Burn scroll";
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

            BaseCustomSpell.SpellInitiator( new ManaBurnSpell( m, 1 ) );
        }

        public ManaBurnScroll( Serial serial )
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
    public class ManaBurnSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ManaBurnSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
		public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
		public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Mana Burn"; } }
        public override int ManaCost { get { return 0; } }
        public override int BaseRange { get { return 0; } }

        public ManaBurnSpell()
            : this( null, 1 )
        {
        }

        public ManaBurnSpell( Mobile caster, int featLevel ) 
            : base( caster, featLevel )
        {
            IconID = 6165;
            Range = 0;
            CustomName = "Mana Burn";
        }

        public override bool CanBeCast
        {
            get
            {                     
                return base.CanBeCast && HasRequiredArcanas( new FeatList[]{ FeatList.Magery } );
            }
        }
		
        public override void Effect()
        {
        
			if (CasterHasEnoughMana && Caster.RawMana == Caster.ManaMax )
			{
				//Corpse cp = TargetItem as Corpse;

			/*	if ( cp.Owner is PlayerMobile  )
				{
					Caster.SendMessage("You cannot use this spell on player corpses.");
					return;
				} */
				
				
				Success = true;
				//Blood bd = new Blood();
				Map map = Caster.Map;
				Point3D m_loc = new Point3D( Caster.X, Caster.Y, Caster.Z );
				//bd.MoveToWorld( m_loc, map );

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
							mb.Emote ("*Burns with ethereal flame!*");
							//mb.FixedEffect( 0x3915, 1, 20, 2936, 30 ); // At player  - this made things see through and cool!
                            mb.FixedEffect(0x3915, 1, 20, 2936, 0);
                            if (mb is PlayerMobile)
							    AOS.Damage( mb, Caster, Caster.Mana, 0, 0, 0, 0, 100, 0, 0, 0 );
                            else
                                AOS.Damage(mb, Caster, Caster.Mana*2, 0, 0, 0, 0, 100, 0, 0, 0);
						}

						//Effects.SendLocationEffect( m_loc, map, 0x3915, 17 );
                        Engines.XmlSpawner2.XmlMana att = new Server.Engines.XmlSpawner2.XmlMana(-Caster.Mana, 600);
                        
                Caster.Mana -= Caster.Mana;
                Engines.XmlSpawner2.XmlAttach.AttachTo(Caster, att);
						Caster.PlaySound( 634 );
						Caster.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*Their eyes grow blue with power!*" );

						return;
				}
			}
       
	}
}
