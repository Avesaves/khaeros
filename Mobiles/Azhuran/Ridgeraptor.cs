using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a Ridgeraptor corpse" )]
	public class Ridgeraptor : BaseMount, IMediumPredator, IJungleCreature, IEnraged, IReptile, IRacialMount
	{
		public override bool ParryDisabled{ get{ return true; } }
		
		public override int[] Hues{ get{ return new int[]{2842,2582,2843}; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }
		
		[Constructable]
		public Ridgeraptor() : this( "a Ridgeraptor" )
		{
		}
		
		[Constructable]
        public Ridgeraptor(string name)
            : base("a ridgeraptor", 187, 0x3EBA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{			
			BaseSoundID = 0x3F3;
			Hue = 2827;

			SetStr( 120 );
			SetDex( 60 );
			SetInt( 25 );

			SetHits( 150 );

			SetDamage( 5, 6 );

			SetDamageType( ResistanceType.Piercing, 100 );		

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Slashing, 20, 25 );
			SetResistance( ResistanceType.Piercing, 20, 25 );
			SetResistance( ResistanceType.Poison, 25 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.Tactics, 67.6, 69.3 );
			SetSkill( SkillName.UnarmedFighting, 75.5, 82.5 );
			
			HueGroup = Utility.Random(3);

			Fame = 1400;
			Karma = -1400;

			VirtualArmor = 25;

			Tamable = false;
			ControlSlots = 2;
			MinTameSkill = 89.1;
			
			if( !Female )
			{
				BodyValue = 188;
				ItemID = 16056;
			}
		}
        
        public override void PrepareToGiveBirth()
		{
			GiveBirth( new Ridgeraptor() );
		}

		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }	

		public Ridgeraptor( Serial serial ) : base( serial )
		{
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
			
			if( version < 2 )
			{
				RawStr = 120;
				RawDex = 60;
				RawInt = 25;
	
				RawHits = 150;
				RawStam = 150;
				RawMana = 20;
	
				SetDamage( 5, 6 );

				SetResistance( ResistanceType.Blunt, 20, 25 );
				SetResistance( ResistanceType.Slashing, 20, 25 );
				SetResistance( ResistanceType.Piercing, 20, 25 );
				SetResistance( ResistanceType.Poison, 25 );
				SetResistance( ResistanceType.Energy, 65, 75 );

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
			
			if( version < 3 )
				ControlSlots = 2;
		}
	}
}
