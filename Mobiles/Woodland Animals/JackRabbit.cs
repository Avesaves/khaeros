using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a jack rabbit corpse" )]
	[TypeAlias( "Server.Mobiles.Rabbit" )]
    public class Rabbit : BaseCreature, ISmallPrey, IForestCreature, ITinyPet
	{
        public static int[] RandomHues = { 2967, 2585, 2591, 2592, 2594, 2674, 2984, 2689, 2822, 2812, 2818, 
                                           2965, 2725, 2724, 2714, 2706, 2813, 2821, 2796, 2797, 2786, 2815, 
                                           2798, 2982, 2966, 2939, 2932, 2840, 2817, 2816, 2814, 2810 };

        public override int MiniatureID { get { return 0x2125; } }
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Rabbit() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a jack rabbit";
			Body = 0xCD;
            Hue = Utility.RandomAnimalHue();

            if( Utility.RandomMinMax( 1, 10 ) == 10 )
                Hue = RandomHues[Utility.Random( RandomHues.Length )];

			SetStr( 15 );
			SetDex( 25 );
			SetInt( 5 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 1, 2 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 2, 5 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 50;
			Karma = 0;

			VirtualArmor = 4;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies; } }

		public Rabbit(Serial serial) : base(serial)
		{
		}

		public override int GetAttackSound() 
		{ 
			return 0xC9; 
		} 

		public override int GetHurtSound() 
		{ 
			return 0xCA; 
		} 

		public override int GetDeathSound() 
		{ 
			return 0xCB; 
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
