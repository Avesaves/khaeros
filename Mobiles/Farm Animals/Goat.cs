using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a goat corpse" )]
	public class Goat : BaseCreature, IMediumPrey, IEnraged
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Goat() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a goat";
			Body = 0xD1;
			BaseSoundID = 0x99;

			SetStr( 19 );
			SetDex( 15 );
			SetInt( 5 );

			SetHits( 12 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Blunt, 50 );
			SetDamageType( ResistanceType.Piercing, 50 );

			SetResistance( ResistanceType.Blunt, 5, 15 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Slashing, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 100;
			Karma = 0;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;
		}
		
		public override bool HasFur{ get{ return true; } }

		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

		public Goat(Serial serial) : base(serial)
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
