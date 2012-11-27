using System;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;
using Server.Commands;

namespace Server.Items
{
	public class Diamond : Item, IGem
	{
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public Diamond() : this( 1 )
		{
		}

		[Constructable]
		public Diamond( int amount ) : base( 0xF26 )
		{
			Stackable = true;
			Amount = amount;
		}

		public Diamond( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				
				if( this.RootParentEntity != m )
        		{
        			m.SendMessage( 60, "That must be in your backpack before you can enbed it on an item." );
        			return;
        		}
				
				if( m.Feats.GetFeatLevel(FeatList.GemEmbedding) > 0 )
				{
					m.Target = new LevelSystemCommands.EmbedTarget( m, "diamond", this );
				}
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
