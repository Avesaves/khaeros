using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a vulture corpse" )]
	public class Vulture : BaseCreature, ISmallPredator, IDesertCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Vulture() : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a vulture";
			Body = 5;
			BaseSoundID = 0x2EE;
			Hue = 2405;

			SetStr( 31, 47 );
			SetDex( 36, 40 );
			SetInt( 8, 20 );

			SetHits( 10, 17 );
			SetMana( 0 );

			SetDamage( 2, 3 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Slashing, 10, 15 );
			SetResistance( ResistanceType.Piercing, 20, 25 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 18.1, 37.0 );
			SetSkill( SkillName.UnarmedFighting, 20.1, 30.0 );

			Fame = 100;
			Karma = 0;

			VirtualArmor = 22;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 17.1;
		}

		public override int Meat{ get{ return 1; } }
		public override int Bones{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 8; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public Vulture(Serial serial) : base(serial)
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
