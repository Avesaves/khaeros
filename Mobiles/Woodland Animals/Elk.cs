using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an elk corpse" )]
	[TypeAlias( "Server.Mobiles.Greathart" )]
	public class Elk : BaseCreature, IMediumPrey, IForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Elk() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an elk";
			Body = 0xEA;

			SetStr( 41, 71 );
			SetDex( 27, 37 );
			SetInt( 27, 35 );

			SetHits( 27, 31 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Piercing, 40 );

			SetResistance( ResistanceType.Blunt, 10, 15 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 29.8, 37.5 );
			SetSkill( SkillName.UnarmedFighting, 19.8, 27.5 );

			Fame = 500;
			Karma = 0;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 19.1;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Elk(Serial serial) : base(serial)
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
