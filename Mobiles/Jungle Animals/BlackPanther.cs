using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a black panther corpse" )]
	public class BlackPanther : BaseCreature, IMediumPredator, IJungleCreature, IFeline
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public BlackPanther() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a black panther";
			Body = 0xD6;
			Hue = 0x901;
			BaseSoundID = 0x462;

			SetStr( 61, 85 );
			SetDex( 46, 555 );
			SetInt( 25 );

			SetHits( 57, 71 );
			SetMana( 0 );

			SetDamage( 5, 8 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 65.0 );

			Fame = 1300;
			Karma = 0;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 53.1;
		}

		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }
		public override bool HasFur{ get{ return true; } }

		public BlackPanther(Serial serial) : base(serial)
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
