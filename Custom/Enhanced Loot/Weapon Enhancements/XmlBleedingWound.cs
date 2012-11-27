using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using System.Collections;

namespace Server.Engines.XmlSpawner2
{
	public class XmlBleedingWound : XmlAttachment
	{
		private int m_Damage = 0;
		private int m_Chance = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Damage { get{ return m_Damage; } set { m_Damage = Math.Max( 0, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max(0, value) ); } }

		public XmlBleedingWound(ASerial serial) : base(serial)
		{
		}
		
		[Attachable]
		public XmlBleedingWound() : this( 0, 0 )
		{
		}

		[Attachable]
		public XmlBleedingWound(int damage, int chance)
		{
			m_Damage = damage;
			m_Chance = chance;
		}

		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
		{
			if( Damage <= 0 || Chance < 1 || IsBleeding(defender) )
				return;
			
			if ( (defender is BaseCreature && ((BaseCreature)defender).BleedImmune) )
				return;
			
			if( Chance >= Utility.RandomMinMax(0, 100) )
			{
				defender.PlaySound( 0x133 );
				defender.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );
				BeginBleed( defender, attacker, m_Damage );
			}
		}
		
		private static Hashtable m_Table = new Hashtable();

		public static bool IsBleeding( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginBleed( Mobile m, Mobile from, int damage )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
				t.Stop();

            t = new InternalTimer( from, m, damage );
			m_Table[m] = t;

			t.Start();
		}

		public static void DoBleed( Mobile m, Mobile from, int level )
		{
			if ( m.Alive )
			{
				m.PlaySound( 0x133 );
				m.Damage( level, from );
				Blood blood = new Blood();
				blood.ItemID = Utility.Random( 0x122A, 5 );
				blood.MoveToWorld( m.Location, m.Map );
			}
			else
				EndBleed( m, false );
		}

		public static void EndBleed( Mobile m, bool message )
		{
			Timer t = (Timer)m_Table[m];

			if ( t == null )
				return;

			t.Stop();
			m_Table.Remove( m );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_Mobile;
			private int m_Count;
            private int m_Damage;

			public InternalTimer( Mobile from, Mobile m, int damage ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_From = from;
				m_Mobile = m;
				Priority = TimerPriority.TwoFiftyMS;
                m_Damage = damage;
			}

			protected override void OnTick()
			{
				DoBleed( m_Mobile, m_From, (int)(m_Damage * (0.75 - (0.25 * m_Count))) );

				if ( ++m_Count == 3 )
					EndBleed( m_Mobile, true );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
			
			writer.Write( (int) m_Damage );
			writer.Write( (int) m_Chance );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			m_Damage = reader.ReadInt();
			m_Chance = reader.ReadInt();
		}
	}
}
