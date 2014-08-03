using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wolf corpse" )]
	public class Wolf : BaseBreedableCreature, IMediumPredator, ICanine
	{
		public override int MaxCubs{ get{ return 3; } }
		public override bool ParryDisabled{ get{ return true; } }
		public Wolf() : base ( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wolf";
			Body = 225;
			BaseSoundID = 0xE5;
            FavouriteManeuver = "criticalstrike";
            FavouriteStance = "defensivestance"; 
			SetStr( 70 );
			SetDex( 45 );
			SetInt( 20 );

			SetHits( 60 );
			SetMana( 0 );

			SetDamage( 7 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Blunt, 20 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Slashing, 5, 10 );
			SetResistance( ResistanceType.Piercing, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 45.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 42.0 );

			Fame = 1000;
			Karma = 0;

			VirtualArmor = 25;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 33.1;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 2; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public override void SetBreedsTraits( string breed, int group )
		{
			switch( breed )
			{
				case "Red Wolf":
				{
					this.Name = "a Red Wolf";
					this.Hue = 2682;
					this.ChangeBreed = "Red Wolf";
					break;
				}
					
				case "Maned Wolf":
				{
					this.Name = "a Maned Wolf";
					this.Hue = 2787;
					this.ChangeBreed = "Maned Wolf";
					break;
				}
					
				case "Timberwolf":
				{
					this.Name = "a Timberwolf";
					this.Hue = 2986;
					this.ChangeBreed = "Timberwolf";
					break;
				}
					
				case "Jackal":
				{
					this.Name = "a Jackal";
					this.Hue = 2811;
					this.ChangeBreed = "Jackal";
					break;
				}
					
				case "Snow Wolf":
				{
					this.Name = "a Snow Wolf";
					this.Hue = 2984;
					this.ChangeBreed = "Snow Wolf";
					break;
				}
					
				case "Gray Wolf":
				{
					this.Name = "a Gray Wolf";
					this.Hue = 2617;
					this.ChangeBreed = "Gray Wolf";
					break;
				}
			}
		}
		
		public Wolf(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 3);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version < 1 )
				Fame += 250;
			
			if( version < 3 )
			{
				SetStr( 70 );
				SetDex( 45 );
				SetInt( 20 );
	
				SetHits( 60 );
				SetMana( 0 );
	
				SetDamage( 5, 6 );

				SetResistance( ResistanceType.Blunt, 15, 20 );
				SetResistance( ResistanceType.Slashing, 5, 10 );
				SetResistance( ResistanceType.Piercing, 10, 15 );
				SetResistance( ResistanceType.Energy, 10, 15 );
				SetResistance( ResistanceType.Poison, 5, 10 );

				VirtualArmor = 25;
				
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
