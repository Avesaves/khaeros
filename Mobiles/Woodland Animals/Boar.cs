using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a boar corpse" )]
	public class Boar : BaseCreature, IMediumPrey, IForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Boar() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a boar";
			Body = 0x122;
			BaseSoundID = 0xC4;
			Hue = 1808;

			SetStr( 80 );
			SetDex( 30 );
			SetInt( 5 );

			SetHits( 40 );
			SetMana( 0 );

			SetDamage( 4, 5 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Piercing, 20 );

			SetResistance( ResistanceType.Blunt, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 500;
			Karma = 0;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new RawHam( 3 ) );
			bpc.DropItem( new RawBaconSlab( 3 ) );
			bpc.DropItem( new PorkHock( 3 ) );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
		
		public Boar(Serial serial) : base(serial)
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
