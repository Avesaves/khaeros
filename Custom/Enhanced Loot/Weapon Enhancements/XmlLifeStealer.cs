using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
	public class XmlLifeStealer : XmlAttachment
	{
		private int m_Hits = 0;
		private int m_Stam = 0;
		private int m_Mana = 0;
		private int m_Chance = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Hits { get{ return m_Hits; } set { m_Hits = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Stam { get{ return m_Stam; } set { m_Stam = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Mana { get{ return m_Mana; } set { m_Mana = Math.Max( 0, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max(0, value) ); } }

		public XmlLifeStealer(ASerial serial) : base(serial)
		{
		}
		
		[Attachable]
		public XmlLifeStealer() : this( 0, 0, 0, 0 )
		{
		}

		[Attachable]
		public XmlLifeStealer(int hits, int stam, int mana, int chance)
		{
			m_Hits = hits;
			m_Stam = stam;
			m_Mana = mana;
			m_Chance = chance;
		}
		
		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int HitsGiven)
		{
			if( attacker == null || defender == null || Chance < 1 || (Hits < 1 && Stam < 1 && Mana < 1) )
				return;
			
			if( Chance >= Utility.RandomMinMax(0, 100) )
			{
				if( Hits > 0 )
				{
					attacker.Hits += Hits;
					attacker.LocalOverheadMessage( MessageType.Regular, 170, false, "+" + Hits.ToString() );
				}
				
				if( Stam > 0 )
				{
					attacker.Stam += Stam;
					defender.Stam -= Stam;
					attacker.LocalOverheadMessage( MessageType.Regular, 154, false, "+" + Stam.ToString() );
				}
				
				if( Mana > 0 )
				{
					attacker.Mana += Mana;
					defender.Mana -= Mana;
					attacker.LocalOverheadMessage( MessageType.Regular, 290, false, "+" + Mana.ToString() );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 1 );
			
			writer.Write( (int) m_Hits );
			writer.Write( (int) m_Chance );
			writer.Write( (int) m_Stam );
			writer.Write( (int) m_Mana );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			m_Hits = reader.ReadInt();
			m_Chance = reader.ReadInt();
			
			if( version > 0 )
			{
				m_Stam = reader.ReadInt();
				m_Mana = reader.ReadInt();
			}
		}
	}
}
