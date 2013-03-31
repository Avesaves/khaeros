using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a panther corpse" )]
	public class Panther : BaseCreature, IMediumPredator, IForestCreature, IFeline
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Panther() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a panther";
			Body = 0xD6;
			Hue = 0x901;
			BaseSoundID = 0x462;

			SetStr( 105, 115 );
			SetDex( 115, 135 );
			SetInt( 11, 25 );

			SetHits( 161, 170 );

			SetDamage( 9, 13 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 45, 70 );
			SetResistance( ResistanceType.Energy, 45, 65 );

			SetSkill( SkillName.Invocation, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 96.1, 106.0 );
			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 86.1, 101.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );

			Fame = 4100;
			Karma = -4100;
			
			VirtualArmor = 30;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 8; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public Panther(Serial serial) : base(serial)
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
