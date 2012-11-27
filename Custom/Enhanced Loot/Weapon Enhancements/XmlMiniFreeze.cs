using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
	public class XmlMiniFreeze : XmlAttachment
	{
		private double m_Duration = 0;
		private int m_Chance = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public double Duration { get{ return m_Duration; } set { m_Duration = Math.Max( 0.0, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max(0, value) ); } }

		public XmlMiniFreeze(ASerial serial) : base(serial)
		{
		}

		[Attachable]
		public XmlMiniFreeze(double duration, int chance)
		{
			m_Duration = duration;
			m_Chance = chance;
		}

		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
		{
			if( Duration <= 0.0 || Chance < 1 || defender.Paralyzed )
				return;
			
			if( Chance >= Utility.RandomMinMax(0, 100) )
			{
				XmlAttach.AttachTo( defender, new XmlFreeze(Duration) );
				defender.PlaySound( 516 );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
			
			writer.Write( (double) m_Duration );
			writer.Write( (int) m_Chance );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			m_Duration = reader.ReadDouble();
			m_Chance = reader.ReadInt();
		}
	}
}
