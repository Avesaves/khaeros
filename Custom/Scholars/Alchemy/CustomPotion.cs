using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public enum PotionType
	{
		Drink = 0,
		Bomb = 1,
		Oil = 2
	}

	public abstract class CustomPotion : BasePotion
	{
		private Dictionary<CustomEffect, int> m_Effects = new Dictionary<CustomEffect, int>(); // effect ID, intensity

		public Dictionary<CustomEffect, int> Effects { get { return m_Effects; } }

		/*public bool IsEmpty()
		{
			return m_Effects.Count > 0;
		}*/

		public override int LabelNumber{ get{ return 0; } }

		public CustomPotion( int itemID ) : base( itemID, PotionEffect.Nightsight ) // effect doesn't matter, just for compatibility
		{
		}

		public CustomPotion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Effects.Count );

			foreach ( KeyValuePair<CustomEffect, int> kvp in m_Effects ) 
			{
				writer.Write( (int)kvp.Key );
				writer.Write( (int)kvp.Value );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			int c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
				m_Effects.Add( (CustomEffect) reader.ReadInt(), reader.ReadInt() );
		}

		public override void Drink( Mobile from )
		{
		}

		public void AddEffect( CustomEffect ID, int intensity )
		{
			m_Effects.Add( ID, intensity );
		}
	}
}
