using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class ToxinRecipe : BaseRecipe
	{
		private int m_BottleID;
		private string m_ToxinName;
		
		public int BottleID { get { return m_BottleID; } set { m_BottleID = value; } }
		public string ToxinName { get { return m_ToxinName; } set { m_ToxinName = value; } }
		
		[Constructable]
		public ToxinRecipe() : base( 0x14F0 )
		{
			Hue = 655;
		}

		public ToxinRecipe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_BottleID );
			writer.Write( m_ToxinName );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_BottleID = reader.ReadInt();
			m_ToxinName = reader.ReadString();
		}
	}
}
