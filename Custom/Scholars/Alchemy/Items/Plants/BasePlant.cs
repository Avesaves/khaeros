using System;
using Server;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BasePlant : Item
	{
        private FarmSoil m_Soil;
        private bool m_Planted;

		public virtual Type Ingredient { get { return null; } }
        public bool Planted { get { return m_Planted; } set { m_Planted = value; } }
        public FarmSoil Soil { get { return m_Soil; } set { m_Soil = value; } }

		public BasePlant( int itemID ) : this( itemID, 1 )
		{
		}

		public BasePlant( int itemID, int amount ) : base( itemID )
		{
			Amount = amount;
			Movable = false;
            Planted = false;
            Soil = null;
		}

		public BasePlant( Serial serial ) : base( serial )
		{
		}

        public void IsPlanted(Mobile from)
        {
            if (Planted)
            {                
                if (Soil == null)
                {
                    Planted = false;
                    return;
                }

                if (Soil.Deleted)
                {
                    Planted = false;
                    return;
                }

                Soil.FullGrown = false;
                Soil.Bloomed = DateTime.Now.AddHours(12);
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write((bool)m_Planted);
            writer.Write((FarmSoil)m_Soil);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 1: m_Planted = reader.ReadBool(); m_Soil = (FarmSoil)reader.ReadItem(); break;
                case 0: m_Planted = false; m_Soil = null; break;
            }
		}
	}
}
