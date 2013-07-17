using System;
using Server.Items;

namespace Server.Items
{
	public class BaseStuddedLeatherShoes : BaseArmor
	{
		public override int BaseBluntResistance{ get{ return 2; } }
		public override int BaseSlashingResistance{ get{ return 1; } }
		public override int BasePiercingResistance{ get{ return 2; } }
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
		
		public override ArmourWeight ArmourType { get { return ArmourWeight.Medium; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		[Constructable]
		public BaseStuddedLeatherShoes( int id ) : base( id )
		{
			Weight = 2.0;
			Layer = Layer.Shoes;
		}

		public BaseStuddedLeatherShoes( Serial serial ) : base( serial )
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
	
	public class HardenedBoots : BaseStuddedLeatherShoes
	{
        public override int BaseBluntResistance { get { return 2; } }
        public override int BaseSlashingResistance { get { return 1; } }
        public override int BasePiercingResistance { get { return 0; } }
        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 30; } }
        public override int InitMaxHits { get { return 40; } }

        public override int AosStrReq { get { return 20; } }
        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 13; } }

        public override ArmourWeight ArmourType { get { return ArmourWeight.Light; } }

		[Constructable]
		public HardenedBoots() : base( 0x170B )
		{
			Name = "Hardened Boots";
		}

		public HardenedBoots( Serial serial ) : base( serial )
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
	
	public class HardenedFurBoots : BaseStuddedLeatherShoes
	{
		[Constructable]
		public HardenedFurBoots() : base( 0x3D24 )
		{
			Name = "Hardened Fur Boots";
		}

		public HardenedFurBoots( Serial serial ) : base( serial )
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
	
	public class HardenedThighBoots : BaseStuddedLeatherShoes
	{
		[Constructable]
		public HardenedThighBoots() : base( 0x1711 )
		{
			Name = "Hardened Thigh Boots";
		}

		public HardenedThighBoots( Serial serial ) : base( serial )
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
	
	public class HardenedLeatherBoots : BaseStuddedLeatherShoes
	{
		[Constructable]
		public HardenedLeatherBoots() : base( 0x3D20 )
		{
			Name = "Hardened Leather Boots";
		}

		public HardenedLeatherBoots( Serial serial ) : base( serial )
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
	
	public class HardenedBlackLeatherBoots : BaseStuddedLeatherShoes
	{
		[Constructable]
		public HardenedBlackLeatherBoots() : base( 0x3D23 )
		{
			Name = "Hardened Black Leather Boots";
		}

		public HardenedBlackLeatherBoots( Serial serial ) : base( serial )
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
