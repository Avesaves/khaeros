using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SpidersSilk : BaseReagent, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( "{0} spiders' silk", Amount );
			}
		}

		[Constructable]
		public SpidersSilk() : this( 1 )
		{
		}

		[Constructable]
		public SpidersSilk( int amount ) : base( 0xF8D, amount )
		{
		}

		public SpidersSilk( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick( Mobile from )
        {
            if( from != null && this.RootParentEntity == from && from.Backpack != null && !from.Backpack.Deleted && from.Backpack is BaseContainer )
            {
                if( Amount > 9 )
                {
                    this.Consume( 10 );
                    from.SendMessage( "You process the silk and turn it into satin." );
                    ( (BaseContainer)from.Backpack ).DropAndStack( new Satin() );
                }

                else
                    from.SendMessage( "You need at least 10 silk to process it into satin." );
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
