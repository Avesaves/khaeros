using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a mountain goat corpse" )]
	public class MountainGoat : BaseCreature, IMediumPrey, ITundraCreature, IEnraged
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public MountainGoat() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a mountain goat";
			Body = 88;
			BaseSoundID = 0x99;
			Hue = 2101;

			SetStr( 22, 64 );
			SetDex( 26, 35 );
			SetInt( 16, 30 );

			SetHits( 20, 33 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10, 20 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 29.3, 44.0 );
			SetSkill( SkillName.UnarmedFighting, 29.3, 44.0 );

			Fame = 200;
			Karma = 0;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

		public MountainGoat(Serial serial) : base(serial)
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
