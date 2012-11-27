using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
	public class XmlSoulEater : XmlAttachment
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

		public XmlSoulEater(ASerial serial) : base(serial)
		{
		}

		[Attachable]
		public XmlSoulEater() : this( 0, 0, 0, 0 )
		{
		}
		
		[Attachable]
		public XmlSoulEater(int hits, int stam, int mana, int chance)
		{
			m_Hits = hits;
			m_Stam = stam;
			m_Mana = mana;
			m_Chance = chance;
		}
		
		public override void OnKilled( Mobile killed, Mobile killer )
		{
			if( killer == null || Chance < 1 || (Hits < 1 && Stam < 1 && Mana < 1) )
				return;
			
			if( Chance >= Utility.RandomMinMax(0, 100) )
			{
				if( Hits > 0 )
				{
					killer.Hits += Hits;
					killer.LocalOverheadMessage( MessageType.Regular, 170, false, "+" + Hits.ToString() );
				}
				
				if( Stam > 0 )
				{
					killer.Stam += Stam;
					killer.LocalOverheadMessage( MessageType.Regular, 154, false, "+" + Stam.ToString() );
				}
				
				if( Mana > 0 )
				{
					killer.Mana += Mana;
					killer.LocalOverheadMessage( MessageType.Regular, 290, false, "+" + Mana.ToString() );
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
