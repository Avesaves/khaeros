using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a deer corpse" )]
	public class Deer : BaseCreature, IMediumPrey, IForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Deer() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a deer";
			Body = 0xED;

			SetStr( 21, 51 );
			SetDex( 37, 37 );
			SetInt( 17, 35 );

			SetHits( 15, 19 );
			SetMana( 0 );

			SetDamage( 3 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 5, 15 );
			SetResistance( ResistanceType.Cold, 5 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 19.0 );
			SetSkill( SkillName.UnarmedFighting, 26.0 );

			Fame = 300;
			Karma = 0;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 23.1;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 6; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Deer(Serial serial) : base(serial)
		{
		}

		public override int GetAttackSound() 
		{ 
			return 0x82; 
		} 

		public override int GetHurtSound() 
		{ 
			return 0x83; 
		} 

		public override int GetDeathSound() 
		{ 
			return 0x84; 
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
