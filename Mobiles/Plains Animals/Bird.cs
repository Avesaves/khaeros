using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a bird corpse" )]
    public class Bird : BaseCreature, ISmallPrey, ITinyPet
	{
        public static int[] RandomHues = { 2646, 2645, 2828, 2827, 2826, 2825, 2729, 2725, 2724, 2718, 2945, 
                                           2944, 2943, 2937, 2935, 2933, 2932, 2833, 2581, 2582, 2583, 2584, 
                                           2966, 2650, 2643, 2644, 2717, 2715, 2712, 2706, 2982, 2647 };

        public override int MiniatureID { get { return 0x211A; } }
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Bird() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			if ( Utility.RandomBool() )
			{
				Hue = 0x901;

				switch ( Utility.Random( 3 ) )
				{
					case 0: Name = "a crow"; break;
					case 2: Name = "a raven"; break;
					case 1: Name = "a magpie"; break;
				}
			}
			else
			{
				Hue = Utility.RandomBirdHue();
				Name = NameList.RandomName( "bird" );
			}

            if( Utility.RandomMinMax( 1, 10 ) == 10 )
                Hue = RandomHues[Utility.Random( RandomHues.Length )];

			Body = 6;
			BaseSoundID = 0x1B;

			VirtualArmor = Utility.RandomMinMax( 0, 6 );

			SetStr( 10 );
			SetDex( 25, 35 );
			SetInt( 10 );
			SetHits( 1 );

			SetDamage( 2 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.UnarmedFighting, 4.2, 6.4 );
			SetSkill( SkillName.Tactics, 4.0, 6.0 );
			SetSkill( SkillName.MagicResist, 4.0, 5.0 );

			SetFameLevel( 1 );
			SetKarmaLevel( 0 );

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -6.9;
			Fame = 0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Bones{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Bird( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 0 )
				Hue = Utility.RandomBirdHue();
		} 
	}

	[CorpseName( "a bird corpse" )]
	public class TropicalBird : BaseCreature
	{
		public TropicalBird() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Hue = Utility.RandomBirdHue();
			Name = "a tropical bird";

			Body = 6;
			BaseSoundID = 0xBF;

			VirtualArmor = Utility.RandomMinMax( 0, 6 );

			SetStr( 10 );
			SetDex( 25, 35 );
			SetInt( 10 );
			SetHits( 1 );

			SetDamage( 2 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.UnarmedFighting, 4.2, 6.4 );
			SetSkill( SkillName.Tactics, 4.0, 6.0 );
			SetSkill( SkillName.MagicResist, 0.0 );

			SetFameLevel( 1 );
			SetKarmaLevel( 0 );

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}

		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Meat{ get{ return 1; } }
		public override int Feathers{ get{ return 25; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public TropicalBird( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		} 
	}
}
