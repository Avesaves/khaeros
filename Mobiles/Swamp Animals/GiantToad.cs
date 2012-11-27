using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a giant toad corpse" )]
	[TypeAlias( "Server.Mobiles.Gianttoad" )]
	public class GiantToad : BaseCreature, IMediumPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public GiantToad() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant toad";
			Body = 80;
			BaseSoundID = 0x26B;

			SetStr( 76, 100 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

			SetHits( 46, 60 );
			SetMana( 0 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 40.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 60.0 );

			Fame = 450;
			Karma = -450;

			VirtualArmor = 14;
		}

		public override int Hides{ get{ return 3; } }
		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 4; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public GiantToad(Serial serial) : base(serial)
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
