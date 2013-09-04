using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Dog corpse" )]
	public class Dog : BaseBreedableCreature, ISmallPredator, ICanine
	{
		public override int MaxCubs{ get{ return 6; } }
		public override bool ParryDisabled{ get{ return true; } }
		public Dog() : base ( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a dog";
			Body = 225;
			BaseSoundID = 0xE5;

			SetStr( 50 );
			SetDex( 45 );
			SetInt( 25 );

			SetHits( 40 );
			SetMana( 0 );

			SetDamage( 6 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Blunt, 20 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 45.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 55.1, 60.0 );

			Fame = 250;
			Karma = 0;

			VirtualArmor = 20;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 23.1;
		}

		public override bool HasFur { get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 2; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public override void SetBreedsTraits( string breed, int group )
		{
			if( group < 0 )
				group = Utility.Random(3);
			
			this.HueGroup = group;
			this.ChangeBreed = breed;
			
			switch( breed )
			{
				case "Southern Shepherd":
				{
					this.Name = "an Southern Shepherd";
					break;
				}
					
				case "Azhuran Retriever":
				{
					this.Name = "an Azhuran Retriever";
					break;
				}
					
				case "Mhordul Wolfdog":
				{
					this.Name = "a Mhordul Wolfdog";
					break;
				}
					
				case "Khemetar Saluki":
				{
					this.Name = "a Khemetar Saluki";
					break;
				}
					
				case "Tyrean Husky":
				{
					this.Name = "a Tyrean Husky";
					break;
				}
					
				case "bloodhound":
				{
					this.Name = "a bloodhound";
					break;
				}
			}
		}
		
		public Dog(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 2);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version < 2 )
			{
				SetStr( 50 );
				SetDex( 45 );
				SetInt( 25 );
	
				SetHits( 40 );
				SetMana( 0 );
	
				SetDamage( 2, 3 );
	
				SetResistance( ResistanceType.Blunt, 15, 20 );
				SetResistance( ResistanceType.Slashing, 5, 10 );
				SetResistance( ResistanceType.Piercing, 10, 15 );
				SetResistance( ResistanceType.Energy, 10, 15 );
				SetResistance( ResistanceType.Poison, 5, 10 );
				VirtualArmor = 20;
				
				int level = 1;
				
				while( level < this.Level )
				{
					this.RawStr += this.StatScale;
		            this.RawDex += this.StatScale;
		            this.RawInt += this.StatScale;
		            this.RawHits += this.StatScale;
		            this.RawStam += this.StatScale;
		            this.RawMana += this.StatScale;
		            
		            if( level % 2 != 0 )
		            	this.DamageMin++;
		            
		            else
		            	this.DamageMax++;
		            
					level++;
				}
			}
		}
	}
}
