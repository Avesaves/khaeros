using System; 

namespace Server.Items 
{
    public class GuardStatueEast : Item
    {
        [Constructable]
        public GuardStatueEast()
            : base( 0x12D8 )
        {
            Weight = 30;
            Name = "Statue";
        }

        public GuardStatueEast(Serial serial)
            : base( serial )
        {
        }

        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class GuardStatueSouth : Item
    {
        [Constructable]
        public GuardStatueSouth()
            : base( 0x12D9 )
        {
            Weight = 30;
            Name = "Statue";
        }

        public GuardStatueSouth(Serial serial)
            : base( serial )
        {
        }

        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class Pedestal : Item
    {
        [Constructable]
        public Pedestal()
            : base( 0x1F2A )
        {
            Weight = 10;
            Name = "Pedestal";
        }

        public Pedestal(Serial serial)
            : base( serial )
        {
        }

        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class LargePedestal : Item
    {
        [Constructable]
        public LargePedestal()
            : base( 0x1223 )
        {
            Weight = 10;
            Name = "Large Pedestal";
        }

        public LargePedestal(Serial serial)
            : base( serial )
        {
        }

        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

	public class ClayStatueSouth : Item 
	{ 
		[Constructable] 
		public ClayStatueSouth() : base(0x139A) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueSouth(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueSouth2 : Item 
	{ 
		[Constructable] 
		public ClayStatueSouth2() : base(0x1227) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueSouth2(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueNorth : Item 
	{ 
		[Constructable] 
		public ClayStatueNorth() : base(0x139B) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueNorth(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueWest : Item 
	{ 
		[Constructable] 
		public ClayStatueWest() : base(0x1226) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueWest(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueEast : Item 
	{ 
		[Constructable] 
		public ClayStatueEast() : base(0x139C) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueEast(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueEast2 : Item 
	{ 
		[Constructable] 
		public ClayStatueEast2() : base(0x1224) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueEast2(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatueSouthEast : Item 
	{ 
		[Constructable] 
		public ClayStatueSouthEast() : base(0x1225) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatueSouthEast(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayBustSouth : Item 
	{ 
		[Constructable] 
		public ClayBustSouth() : base(0x12CB) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay ClayBust";
		} 

		public ClayBustSouth(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayBustEast : Item 
	{ 
		[Constructable] 
		public ClayBustEast() : base(0x12CA) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay ClayBust";
		} 

		public ClayBustEast(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatuePegasus : Item 
	{ 
		[Constructable] 
		public ClayStatuePegasus() : base(0x139D) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatuePegasus(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class ClayStatuePegasus2 : Item 
	{ 
		[Constructable] 
		public ClayStatuePegasus2() : base(0x1228) 
		{ 
			Weight = 10; 
			Hue = 1542;
			Name = "Clay Statue";
		} 

		public ClayStatuePegasus2(Serial serial) : base(serial) 
		{ 
		} 

		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } } 

		public override void Serialize(GenericWriter writer) 
		{ 
			base.Serialize(writer); 

			writer.Write((int) 0); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize(reader); 

			int version = reader.ReadInt(); 
		} 
	} 

	public class SmallClayTowerSculpture : Item
	{
		[Constructable]
		public SmallClayTowerSculpture() : base(0x241A)
		{
			Weight = 20.0;
			Hue = 1542;
			Name = "Clay Tower Sculture";
		}

		public SmallClayTowerSculpture(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
