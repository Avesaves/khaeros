using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
	public class XmlCriticalHit : XmlAttachment
	{
		private int m_PureDamage = 0;
		private int m_FireDamage = 0;
		private int m_FrostDamage = 0;
		private int m_EnergyDamage = 0;
		private int m_PoisonDamage = 0;
		private int m_Chance = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public int PureDamage { get{ return m_PureDamage; } set { m_PureDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int FireDamage { get{ return m_FireDamage; } set { m_FireDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int FrostDamage { get{ return m_FrostDamage; } set { m_FrostDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyDamage { get{ return m_EnergyDamage; } set { m_EnergyDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonDamage { get{ return m_PoisonDamage; } set { m_PoisonDamage = Math.Max( 0, value ); } }
		

		[CommandProperty( AccessLevel.GameMaster )]
		public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max(0, value) ); } }

		public XmlCriticalHit(ASerial serial) : base(serial)
		{
		}
		
		[Attachable]
		public XmlCriticalHit() : this( 0, 0, 0, 0, 0, 0 )
		{
		}

		[Attachable]
		public XmlCriticalHit(int damage, int fire, int frost, int energy, int poison, int chance)
		{
			m_PureDamage = damage;
			m_FireDamage = fire;
			m_FrostDamage = frost;
			m_EnergyDamage = energy;
			m_PoisonDamage = poison;
			m_Chance = chance;
		}

		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
		{
			if( (PureDamage < 1 && FireDamage < 1 && FrostDamage < 1 && EnergyDamage < 1 && PoisonDamage < 1) || Chance < 1 )
				return;
			
			if( Chance >= Utility.RandomMinMax(0, 100) )
			{
				if( PureDamage > 0 )
					SpellHelper.Damage( TimeSpan.Zero, defender, attacker, PureDamage, 100, 0, 0, 0, 0 );
				
				if( FireDamage > 0 )
				{
					SpellHelper.Damage( TimeSpan.Zero, defender, attacker, FireDamage, 0, 100, 0, 0, 0 );
					defender.FixedParticles( 14284, 10, 10, 9950, 2617, 0, EffectLayer.Waist );
				}
				
				if( FrostDamage > 0 )
				{
					SpellHelper.Damage( TimeSpan.Zero, defender, attacker, FrostDamage, 0, 0, 100, 0, 0 );
					defender.FixedParticles( 14284, 10, 10, 9950, 2967, 0, EffectLayer.Waist );
				}
				
				if( EnergyDamage > 0 )
				{
					SpellHelper.Damage( TimeSpan.Zero, defender, attacker, EnergyDamage, 0, 0, 0, 0, 100 );
					defender.FixedParticles( 14284, 10, 10, 9950, 2953, 0, EffectLayer.Waist );
				}
				
				if( PoisonDamage > 0 )
				{
					SpellHelper.Damage( TimeSpan.Zero, defender, attacker, PoisonDamage, 0, 0, 0, 100, 0 );
					defender.FixedParticles( 14284, 10, 10, 9950, 2949, 0, EffectLayer.Waist );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 1 );
			
			writer.Write( (int) m_PureDamage );
			writer.Write( (int) m_Chance );
			writer.Write( (int) m_FireDamage );
			writer.Write( (int) m_FrostDamage );
			writer.Write( (int) m_EnergyDamage );
			writer.Write( (int) m_PoisonDamage );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			m_PureDamage = reader.ReadInt();
			m_Chance = reader.ReadInt();
			
			if( version > 0 )
			{
				m_FireDamage = reader.ReadInt();
				m_FrostDamage = reader.ReadInt();
				m_EnergyDamage = reader.ReadInt();
				m_PoisonDamage = reader.ReadInt();
			}
		}
	}
}
