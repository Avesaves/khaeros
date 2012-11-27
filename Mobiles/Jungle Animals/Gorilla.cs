using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gorilla corpse" )]
	public class Gorilla : BaseCreature, IMediumPrey, IJungleCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Gorilla() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a gorilla";
			Body = 0x1D;
			BaseSoundID = 0x9E;

			SetStr( 53, 95 );
			SetDex( 36, 55 );
			SetInt( 35 );

			SetHits( 38, 51 );
			SetMana( 0 );

			SetDamage( 4, 6 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Slashing, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 43.3, 58.0 );
			SetSkill( SkillName.UnarmedFighting, 43.3, 58.0 );

			Fame = 650;
			Karma = 0;

			VirtualArmor = 20;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Gorilla(Serial serial) : base(serial)
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
