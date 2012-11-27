using System;

namespace Server.Items
{
	public abstract class BaseNecklace : BaseJewel
	{
		public override int BaseGemTypeNumber{ get{ return 1044241; } } // star sapphire necklace

		public BaseNecklace( int itemID ) : base( itemID, Layer.Neck )
		{
		}

		public BaseNecklace( Serial serial ) : base( serial )
		{
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

	public class Necklace : BaseNecklace
	{
		[Constructable]
		public Necklace() : base( 0x1085 )
		{
			Weight = 0.1;
		}

		public Necklace( Serial serial ) : base( serial )
		{
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

	public class GoldNecklace : BaseNecklace
	{
		[Constructable]
		public GoldNecklace() : base( 0x1088 )
		{
			Weight = 0.1;
			Name = "Gold Necklace";
		}

		public GoldNecklace( Serial serial ) : base( serial )
		{
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

	public class GoldBeadNecklace : BaseNecklace
	{
		[Constructable]
		public GoldBeadNecklace() : base( 0x1089 )
		{
			Weight = 0.1;
			Name = "Gold Bead Necklace";
		}

		public GoldBeadNecklace( Serial serial ) : base( serial )
		{
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


	public class SilverNecklace : BaseNecklace
	{
		[Constructable]
		public SilverNecklace() : base( 0x1F08 )
		{
			Weight = 0.1;
			Name = "Silver Necklace";
		}

		public SilverNecklace( Serial serial ) : base( serial )
		{
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

	public class SilverBeadNecklace : BaseNecklace
	{
		[Constructable]
		public SilverBeadNecklace() : base( 0x1F05 )
		{
			Weight = 0.1;
			Name = "Silver Bead Necklace";
		}

		public SilverBeadNecklace( Serial serial ) : base( serial )
		{
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

    public class SilverSerpentNecklace : BaseNecklace
    {
        [Constructable]
        public SilverSerpentNecklace()
            : base( 0x2C98 )
        {
            Weight = 0.1;
            Name = "Silver Serpent Necklace";
        }

        public SilverSerpentNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class LargeSilverNecklace : BaseNecklace
    {
        [Constructable]
        public LargeSilverNecklace()
            : base( 0x2C93 )
        {
            Weight = 0.1;
            Name = "Large Silver Necklace";
        }

        public LargeSilverNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class GoldAnkhNecklace : BaseNecklace
    {
        [Constructable]
        public GoldAnkhNecklace()
            : base( 0x2C99 )
        {
            Weight = 0.1;
            Name = "Gold Ankh Necklace";
        }

        public GoldAnkhNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class DelicateGoldBeadNecklace : BaseNecklace
    {
        [Constructable]
        public DelicateGoldBeadNecklace()
            : base( 0x2C8E )
        {
            Weight = 0.1;
            Name = "Delicate Gold Bead Necklace";
        }

        public DelicateGoldBeadNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class ExtravagantGoldNecklace : BaseNecklace
    {
        [Constructable]
        public ExtravagantGoldNecklace()
            : base( 0x2C87 )
        {
            Weight = 0.1;
            Name = "Extravagant Gold Necklace";
        }

        public ExtravagantGoldNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class LargeGoldNecklace : BaseNecklace
    {
        [Constructable]
        public LargeGoldNecklace()
            : base( 0x2C88 )
        {
            Weight = 0.1;
            Name = "Large Gold Necklace";
        }

        public LargeGoldNecklace( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class BoneFetish : BaseNecklace
    {
        [Constructable]
        public BoneFetish()
            : base( 15241 )
        {
            Weight = 0.1;
            Name = "Bone Fetish";
        }

        public BoneFetish( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }

        public override void OnRemoved( object parent )
        {
            if( parent is Mobiles.PlayerMobile && !( (Mobiles.PlayerMobile)parent ).IsVampire )
                ( (Mobiles.PlayerMobile)parent ).Friendship.Undead = 0;

            base.OnRemoved( parent );
        }

        public override void OnAdded( object parent )
        {
            if( parent is Mobiles.PlayerMobile && !( (Mobiles.PlayerMobile)parent ).IsVampire )
                ( (Mobiles.PlayerMobile)parent ).Friendship.Undead = 1;

            base.OnAdded( parent );
        }
    }
}
