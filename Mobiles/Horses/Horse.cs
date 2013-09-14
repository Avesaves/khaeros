using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a horse corpse" )]
	[TypeAlias( "Server.Mobiles.BrownHorse", "Server.Mobiles.DirtyHorse", "Server.Mobiles.GrayHorse", "Server.Mobiles.TanHorse" )]
	public class Horse : BaseMount, ILargePrey, IPlainsCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		private static int[] m_IDs = new int[]
			{
				0xC8, 0x3E9F,
				0xE2, 0x3EA0,
				0xE4, 0x3EA1,
				0xCC, 0x3EA2
			};

		public Horse() : this( "a horse" )
		{
		}

		public Horse( string name ) : base( name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			int random = Utility.Random( 4 );

			Body = m_IDs[random * 2];
			ItemID = m_IDs[random * 2 + 1];
			BaseSoundID = 0xA8;

			SetStr( 90 );
			SetDex( 65 );
			SetInt( 10 );

			SetHits( 50 );
			SetMana( 0 );

			SetDamage( 1, 2 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 39.3, 44.0 );
			SetSkill( SkillName.UnarmedFighting, 39.3, 44.0 );

			Fame = 300;
			Karma = 300;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
			VirtualArmor = 10;
		}

		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 4; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Horse( Serial serial ) : base( serial )
		{
		}
		
		public override void SetBreedsTraits( string breed, int group )
		{
			if( group < 0 )
				group = Utility.Random(3);
			
			this.HueGroup = group;
			this.ChangeBreed = breed;
			
			switch( breed )
			{
				case "Galloway Horse":
				{
					this.Name = "a Galloway Horse";
					this.BodyValue = 228;
					this.ItemID = 0x3EA1;
					break;
				}
					
				case "Western Horse":
				{
					this.Name = "an Western Horse";
					this.BodyValue = 204;
					this.ItemID = 0x3EA2;
					break;
				}
					
				case "Steppe Horse":
				{
					this.Name = "a Steppe Horse";
					this.BodyValue = 204;
					this.ItemID = 0x3EA2;
					break;
				}
					
				case "Barb Horse":
				{
					this.Name = "a Barb Horse";
					this.BodyValue = 228;
					this.ItemID = 0x3EA1;
					break;
				}
					
				case "Tyrean Horse":
				{
					this.Name = "a Tyrean Horse";
					this.BodyValue = 226;
					this.ItemID = 0x3EA0;
					break;
				}
					
				case "Northern Horse":
				{
					this.Name = "a Northern Horse";
					this.BodyValue = 226;
					this.ItemID = 0x3EA0;
					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				SetDamage( 1, 2 );
			
			if( version < 3 )
			{
				SetStr( 90 );
				SetDex( 65 );
				SetInt( 10 );
	
				SetHits( 50 );
				SetMana( 0 );
	
				SetDamage( 1, 2 );

				VirtualArmor = 10;
				
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
