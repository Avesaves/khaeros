using System;
using System.Collections.Generic;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Items
{
	public class BombPotion : CustomPotion
	{
		private static bool LeveledExplosion = false;	// Should explosion potions explode other nearby potions?
		private bool m_InstantExplosion = false;	// Should explosion potions explode on impact?
		private int m_ExplosionRange = 0;			// How large is the blast radius?
		private int m_FuseTimer = 3;			// How long 'till boom

		public bool InstantExplosion { get { return m_InstantExplosion; } set { m_InstantExplosion = value; } }
		public int ExplosionRange { get { return m_ExplosionRange; } set { m_ExplosionRange = value; } }
		public override bool RequireFreeHand{ get{ return false; } }

		public BombPotion( int itemID ) : base( itemID )
		{
		}

		public BombPotion( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060847, "{0}\t{1}", "Potion Type:", "Bomb" ); // ~1_val~ ~2_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_ExplosionRange );
			writer.Write( (bool) m_InstantExplosion );
			writer.Write( (int) m_FuseTimer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_ExplosionRange = reader.ReadInt();
			m_InstantExplosion = reader.ReadBool();
			m_FuseTimer = reader.ReadInt();
		}

		public virtual object FindParent( Mobile from )
		{
			Mobile m = this.HeldBy;

			if ( m != null && m.Holding == this )
				return m;

			object obj = this.RootParent;

			if ( obj != null )
				return obj;

			if ( Map == Map.Internal )
				return from;

			return this;
		}

		private Timer m_Timer;

		private ArrayList m_Users;

		public override void Drink( Mobile from )
		{
			if ( Core.AOS && (from.Paralyzed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)) )
			{
				from.SendMessage( "You cannot use a bomb while paralyzed." );
				return;
			}

			ThrowTarget targ = from.Target as ThrowTarget;

			if ( targ != null && targ.Potion == this )
				return;

			from.RevealingAction();

			if ( m_Users == null )
				m_Users = new ArrayList();

			if ( !m_Users.Contains( from ) )
				m_Users.Add( from );

			from.Target = new ThrowTarget( this );

			if ( m_Timer == null )
			{
				from.SendLocalizedMessage( 500236 ); // You should throw it now!
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), TimeSpan.FromSeconds( 1.0 ), 4, new TimerStateCallback( Detonate_OnTick ), new object[]{ from, m_FuseTimer } );
			}
		}

		private void Detonate_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			int timer = (int)states[1];

			object parent = FindParent( from );

			if ( timer == 0 )
			{
				Point3D loc;
				Map map;

				if ( parent is Item )
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if ( parent is Mobile )
				{
					Mobile m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}

				Explode( from, true, loc, map );
			}
			else
			{
				if ( parent is Item )
					((Item)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );
				else if ( parent is Mobile )
					((Mobile)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			IPoint3D p = (IPoint3D)states[1];
			Map map = (Map)states[2];

			Point3D loc = new Point3D( p );

			if ( m_InstantExplosion )
				Explode( from, true, loc, map );
			else
				MoveToWorld( loc, map );
		}

		private class ThrowTarget : Target
		{
			private BombPotion m_Potion;

			public BombPotion Potion
			{
				get{ return m_Potion; }
			}

			public ThrowTarget( BombPotion potion ) : base( 12, true, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Potion.Deleted || m_Potion.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

				SpellHelper.GetSurfaceTop( ref p );

				from.RevealingAction();

				IEntity to;

				if ( p is Mobile )
					to = (Mobile)p;
				else
					to = new Entity( Serial.Zero, new Point3D( p ), map );

				/* Based on Throwing skill
				at 0.0 skill:
					0-250: success
					250-550: 1-2 deviation
					550-850: 2-3 deviation 
					850-1000: 3-4 deviation
				at 75.0 skill:
					0-775: success
					775-865: 1-2 deviation 
					865-955: 2-3 deviation
					955-1000: 3-4 deviation
				at 100.0 skill:
					0-950: success
					950-970: chance for 1-2 deviation
					970-990: chance for 2-3 deviation
					990-1000: chance for 3-4 deviation
				*/
				Point3D positionMod = new Point3D( 0, 0, 0);
				if ( from.GetDistanceToSqrt( p ) > 1 ) // no point in failing a 1 tile "throw"
				{
					int success = 250 + (int)(from.Skills[(SkillName)15].Value*7);
					int failPart = (1000 - success) / 5;
					int slightDeviation = success + failPart*2;
					int mediumDeviation = slightDeviation + failPart*2;

					int random = Utility.Random( 1000 );
					int changeType = Utility.Random( 3 );
					int changeAmount = 0;

					if ( random >= mediumDeviation ) // severe dev
						changeAmount = Utility.Random( 2 ) + 3;
					else if ( random >= slightDeviation ) // medium dev
						changeAmount = Utility.Random( 2 ) + 2;
					else if ( random >= success ) // slight dev
						changeAmount = Utility.Random( 2 ) + 1;

					if ( changeType == 0 ) // x 
						positionMod.X = ( Utility.RandomBool() ? 1 : -1 ) * changeAmount;
					else if ( changeType == 1 ) // y
						positionMod.Y = ( Utility.RandomBool() ? 1 : -1 ) * changeAmount;
					else // both
					{
						positionMod.X = ( Utility.RandomBool() ? 1 : -1 ) * changeAmount;
						positionMod.Y = ( Utility.RandomBool() ? 1 : -1 ) * changeAmount;
					}
				}

				Point3D p3d = new Point3D( to.Location.X + positionMod.X, to.Location.Y + positionMod.Y, to.Location.Z );
				if ( !from.InLOS( p3d ) ) // if the miss was supposed to go out of line of sight, don't miss.
					p3d = new Point3D( to.Location.X, to.Location.Y, to.Location.Z );
				IEntity newEnt = new Entity( Serial.Zero, p3d, map );
				p = newEnt as IPoint3D;

				Server.Effects.SendMovingEffect( from, newEnt, m_Potion.ItemID & 0x3FFF, 7, 0, false, false, m_Potion.Hue, 0 );

				m_Potion.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Potion.Reposition_OnTick ), new object[]{ from, p, map } );
			}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			if ( Deleted )
				return;

			Delete();

			for ( int i = 0; m_Users != null && i < m_Users.Count; ++i )
			{
				Mobile m = (Mobile)m_Users[i];
				ThrowTarget targ = m.Target as ThrowTarget;

				if ( targ != null && targ.Potion == this )
					Target.Cancel( m );
			}

			if ( map == null )
			{
				return;
			}

			IPooledEnumerable eable = LeveledExplosion ? map.GetObjectsInRange( loc, m_ExplosionRange ) : map.GetMobilesInRange( loc, m_ExplosionRange );
			ArrayList toExplode = new ArrayList();

			int toDamage = 0;

			foreach ( object o in eable )
			{
				if ( o is Mobile )
				{
					toExplode.Add( o );
					++toDamage;
				}
				else if ( o is BombPotion && o != this )
				{
					toExplode.Add( o );
				}
			}

			eable.Free();

			foreach ( KeyValuePair<CustomEffect, int> kvp in Effects )
			{
				CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
				if ( effect != null )
					effect.OnExplode( from, this, kvp.Value, loc, map );
			}
			
			Point3D eye = new Point3D( loc );
			eye.Z += 14;

			for ( int i = 0; i < toExplode.Count; ++i )
			{
				object o = toExplode[i];

				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;
					Point3D target = new Point3D( m.Location );
					target.Z += 14;

					if ( from == null || (SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false )) )
					{
						if ( o != null && map.LineOfSight( eye, target ) )
						{
							foreach ( KeyValuePair<CustomEffect, int> kvp in Effects )
							{
								CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
								if ( effect != null )
									effect.ApplyEffect( m, from, kvp.Value, this );
							}
						}
					}
				}
				else if ( o is BombPotion )
				{
					BombPotion pot = (BombPotion)o;
					Point3D target = new Point3D( pot.Location );
					target.Z += 14;
					if ( o != null && map.LineOfSight( eye, target ) )
						pot.Explode( from, false, pot.GetWorldLocation(), pot.Map );
				}
			}
		}
	}
}
