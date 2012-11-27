using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a bull frog corpse" )]
	[TypeAlias( "Server.Mobiles.Bullfrog" )]
	public class BullFrog : BaseCreature, ISmallPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public BullFrog() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a bull frog";
			Body = 81;
			Hue = 0;
			BaseSoundID = 0x266;

			SetStr( 46, 70 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

			SetHits( 28, 42 );
			SetMana( 0 );

			SetDamage( 1, 2 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 40.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 60.0 );

			Fame = 150;
			Karma = 0;

			VirtualArmor = 6;
		}

		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }
		public override int Hides{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public BullFrog(Serial serial) : base(serial)
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
