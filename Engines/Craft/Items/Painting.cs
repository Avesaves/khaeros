using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class Painting : Item, IDyable
	{
        //private string m_Description;

        /*[CommandProperty( AccessLevel.GameMaster )]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }*/

        private int m_Index;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

		[Constructable]
        public Painting()
            : base( 0x240D )
		{
			Weight = 5.0;
            Name = "Painting";
            Description = "None";
		}

		public Painting( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick( Mobile from )
        {
            if ( from is PlayerMobile )
            {
                PlayerMobile pm = from as PlayerMobile;

                if ( pm.Feats.GetFeatLevel(FeatList.Painter) < 1 || !this.Movable )
                {
                    pm.SendMessage( 60, "Description: " + this.Description );
                }

                else
                {
                    pm.SendGump( new PaintingGump( pm, this ) );
                }
            }
        }

        public static void ChangeID( Painting painting, int index )
        {
            switch( index )
            {
                case 0: painting.ItemID = 9229; painting.Index = 0; break;
                case 1: painting.ItemID = 9230; painting.Index = index; break;
                case 2: painting.ItemID = 9231; painting.Index = index; break;
                case 3: painting.ItemID = 9232; painting.Index = index; break;
                case 4: painting.ItemID = 9235; painting.Index = index; break;
                case 5: painting.ItemID = 9236; painting.Index = index; break;
                case 6: painting.ItemID = 9233; painting.Index = index; break;
                case 7: painting.ItemID = 9234; painting.Index = index; break;
                case 8: painting.ItemID = 3744; painting.Index = index; break;
                case 9: painting.ItemID = 3745; painting.Index = index; break;
                case 10: painting.ItemID = 3746; painting.Index = index; break;
                case 11: painting.ItemID = 3785; painting.Index = index; break;
                case 12: painting.ItemID = 3815; painting.Index = index; break;
                case 13: painting.ItemID = 3784; painting.Index = index; break;
                case 14: painting.ItemID = 3743; painting.Index = index; break;
                case 15: painting.ItemID = 3749; painting.Index = index; break;
                case 16: painting.ItemID = 3750; painting.Index = index; break;
                case 17: painting.ItemID = 3751; painting.Index = index; break;
                case 18: painting.ItemID = 3752; painting.Index = index; break;
                case 19: painting.ItemID = 9229; painting.Index = 0; break;
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
            //writer.Write( (string) m_Description );
            writer.Write( (int) m_Index );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            //m_Description = reader.ReadString();
            m_Index = reader.ReadInt();
		}
 
        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }
    }
}
