using System;
using Server.Items;

namespace Server.Items
{
	public class BaseLeatherShoes : BaseArmor
	{
		public override int BaseBluntResistance{ get{ return 3; } }
		public override int BaseSlashingResistance{ get{ return 1; } }
		public override int BasePiercingResistance{ get{ return 0; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 3; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 40; } }

		public override int AosStrReq{ get{ return 20; } }
		public override int OldStrReq{ get{ return 10; } }

		public override int ArmorBase{ get{ return 13; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		public BaseLeatherShoes( int id ) : base( id )
		{
			Weight = 1.0;
			Layer = Layer.Shoes;
		}

		public BaseLeatherShoes( Serial serial ) : base( serial )
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
	
	public class HardenedShoes : BaseLeatherShoes
	{
		[Constructable]
		public HardenedShoes() : base( 0x170F )
		{
			Weight = 1.0;
			Layer = Layer.Shoes;
			Name = "Hardened Shoes";
		}

		public HardenedShoes( Serial serial ) : base( serial )
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
	
	public class HardenedElegantShoes : BaseLeatherShoes
	{
		[Constructable]
		public HardenedElegantShoes() : base( 0x3D25 )
		{
			Weight = 1.0;
			Layer = Layer.Shoes;
			Name = "Hardened Elegant Shoes";
		}

		public HardenedElegantShoes( Serial serial ) : base( serial )
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
	
	public class HardenedSandals : BaseLeatherShoes
	{
		[Constructable]
		public HardenedSandals() : base( 0x170D )
		{
			Weight = 1.0;
			Layer = Layer.Shoes;
			Name = "Hardened Sandals";
		}

		public HardenedSandals( Serial serial ) : base( serial )
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
	
	public class HardenedHighHeels : BaseLeatherShoes
	{
		[Constructable]
		public HardenedHighHeels() : base( 0x3D2A )
		{
			Weight = 1.0;
			Layer = Layer.Shoes;
			Name = "Hardened High Heels";
		}

		public HardenedHighHeels( Serial serial ) : base( serial )
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
}
