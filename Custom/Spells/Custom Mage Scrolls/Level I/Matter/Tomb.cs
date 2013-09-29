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
    public class TombScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new TombSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public TombScroll()
            : base()
        {
            Hue = 934;
            Name = "A Tomb Scroll";
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

            BaseCustomSpell.SpellInitiator( new TombSpell( m, 1 ) );
        }

        public TombScroll( Serial serial )
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
    public class TombSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new TombSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Tomb"; } }
        public override int ManaCost { get { return 80; } }
        public override int BaseRange { get { return 0; } }

        public TombSpell()
            : this( null, 1 )
        {
        }

        public TombSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6077;
            Range = 0;
            CustomName = "Tomb";
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
        		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;

			public override bool BlocksFit{ get{ return true; } }

			public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0x80 )
			{
				Visible = false;
				Movable = false;

				MoveToWorld( loc, map );

				if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 10.0 ) );
				m_Timer.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( 10.0 );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, m_End - DateTime.Now );
						m_Timer.Start();

						break;
					}
					case 0:
					{
						TimeSpan duration = TimeSpan.FromSeconds( 10.0 );

						m_Timer = new InternalTimer( this, duration );
						m_Timer.Start();

						m_End = DateTime.Now + duration;

						break;
					}
				}
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
				{
					Priority = TimerPriority.OneSecond;
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}
        public override void Effect()
        {
		 if( CasterHasEnoughMana )
		 {
             Caster.Mana -= TotalCost;
             Success = true;
             int tx = Caster.Location.X - 1;
             int ty = Caster.Location.Y - 1;
             int tz = Caster.Location.Z + 10;
             int mx = Caster.Location.X + 1;
             int my = Caster.Location.Y + 1;
             int oz = Caster.Location.Z - 2;
             int pz = Caster.Location.Z + 3;
             int rz = Caster.Location.Z;
             int rx = Caster.Location.X;
             int ry = Caster.Location.Y;

             Caster.PlaySound(586);
             Caster.SendMessage("You Summon forth stone walls!");
             Point3D loc = new Point3D(rx, ry, tz);
             Point3D loc2 = new Point3D(tx, ry, pz);
             Point3D loc3 = new Point3D(mx, ry, pz);
             Point3D loc4 = new Point3D(rx, my, pz);
             Point3D loc5 = new Point3D(rx, ty, pz);
             
             Point3D loc6 = new Point3D(tx, ty, oz);
             Point3D loc7 = new Point3D(tx, my, oz);
             Point3D loc8 = new Point3D(mx, ty, oz);
             Point3D loc9 = new Point3D(mx, my, oz);
             MagicStoneWall wall = new MagicStoneWall(Caster);
             MagicStoneWall wall2 = new MagicStoneWall(Caster);
             MagicStoneWall wall3 = new MagicStoneWall(Caster);
             MagicStoneWall wall4 = new MagicStoneWall(Caster);
             MagicStoneWall wall5 = new MagicStoneWall(Caster);
             MagicStoneWall wall6 = new MagicStoneWall(Caster);
             MagicStoneWall wall7 = new MagicStoneWall(Caster);
             MagicStoneWall wall8 = new MagicStoneWall(Caster);
             MagicStoneWall wall9 = new MagicStoneWall(Caster);
             wall.MoveToWorld(loc, Caster.Map);
             wall2.MoveToWorld(loc2, Caster.Map);
             wall3.MoveToWorld(loc3, Caster.Map);
             wall4.MoveToWorld(loc4, Caster.Map);
             wall5.MoveToWorld(loc5, Caster.Map);
             wall6.MoveToWorld(loc6, Caster.Map);
             wall7.MoveToWorld(loc7, Caster.Map);
             wall8.MoveToWorld(loc8, Caster.Map);
             wall9.MoveToWorld(loc9, Caster.Map);
             Effects.SendLocationParticles(wall, 0x376A, 9, 10, 5025);
             Caster.Emote("*{0} body disappears from sight as stone walls appear from nowhere!*", Caster.Female == true ? "her" : "his", Caster.Name);
			}
        }
    }
}
