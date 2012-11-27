using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a pig corpse" )]
	public class Pig : BaseCreature, IMediumPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Pig() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a pig";
			Body = 0x122;
			BaseSoundID = 0xC4;

			SetStr( 20 );
			SetDex( 20 );
			SetInt( 5 );

			SetHits( 12 );
			SetMana( 0 );

			SetDamage( 2, 4 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 120;
			Karma = 0;

			VirtualArmor = 12;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new PorkHock( 1 ) );
			bpc.DropItem( new RawHam( 1 ) );
			bpc.DropItem( new RawBaconSlab( 1 ) );
		}
		
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 3; } }

		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Pig(Serial serial) : base(serial)
		{
		}

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
}
