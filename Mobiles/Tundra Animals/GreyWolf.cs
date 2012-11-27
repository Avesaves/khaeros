using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a grey wolf corpse" )]
	[TypeAlias( "Server.Mobiles.Greywolf" )]
	public class GreyWolf : BaseCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		public GreyWolf() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a grey wolf";
			Body = Utility.RandomList( 25, 27 );
			BaseSoundID = 0xE5;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 35 );

			SetHits( 34, 48 );
			SetMana( 0 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Blunt, 20 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Slashing, 10, 15 );
			SetResistance( ResistanceType.Piercing, 20, 25 );
			SetResistance( ResistanceType.Energy, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 60.0 );

			Fame = 450;
			Karma = 0;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 53.1;
		}

		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public GreyWolf(Serial serial) : base(serial)
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
