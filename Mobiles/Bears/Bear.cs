using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bear corpse")]
    public class Bear : BaseBreedableCreature, IMediumPredator, IEnraged, IBear
    {
        public override int MaxCubs { get { return 1; } }
        public override bool ParryDisabled { get { return true; } }
        public Bear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bear";
            Body = 213;
            BaseSoundID = 0xA3;
            Tamable = true; 
			SetStr( 150, 185 );
			SetDex( 100, 110 );
			SetInt( 26, 30 );

			SetHits( 180, 196 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 30;
                        ControlSlots = 2;
		}

		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 24; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 16; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } }

        public override void SetBreedsTraits(string breed, int group)
        {
            switch (breed)
            {
                case "Waste Bear": //Polar bear
                    {
                        this.Name = "a waste bear";
                        this.Body = 213;
                        this.ChangeBreed = "Waste Bear";
                        break;
                    }

                case "Corvinus Bear": //Black bear
                    {
                        this.Name = "a Corvinus bear";
                        this.Body = 211;
                        this.Hue = 1899;
                        this.ChangeBreed = "Corvinus Bear";
                        break;
                    }

                case "Bradoc's Bear": //Brown bear
                    {
                        this.Name = "a Bradoc's bear";
                        this.Body = 167;
                        this.Hue = 1127;
                        this.ChangeBreed = "Bradoc's Bear";
                        break;
                    }

                case "Southern Bear": //Grizzly bear
                    {
                        this.Name = "a southern bear";
                        this.Body = 212;
                        this.ChangeBreed = "Southern Bear";
                        break;
                    }
            }
        }

        public Bear(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
            {
                ActiveSpeed = 0.2;
                PassiveSpeed = 0.4;
            }
        }
    }
}
