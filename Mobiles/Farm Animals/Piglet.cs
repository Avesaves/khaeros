using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a piglet corpse" )]
	public class Piglet : BaseCreature, IMediumPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Piglet() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a piglet";
			Body = 0xCB;
			BaseSoundID = 0xC4;

			SetStr( 5 );
			SetDex( 10 );
			SetInt( 5 );

			SetHits( 4 );
			SetMana( 0 );

			SetDamage( 1, 2 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 30;
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

		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Piglet(Serial serial) : base(serial)
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
