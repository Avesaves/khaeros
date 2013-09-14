using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a Giant Scarab corpse" )]
	public class GiantScarab : BaseMount, IMediumPredator, IDesertCreature, IEnraged, IGiantBug, IRacialMount
	{
		public override int[] Hues{ get{ return new int[]{2814,2818,2813}; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public GiantScarab() : this( "a Giant Scarab" )
		{
		}
		
		[Constructable]
        public GiantScarab(string name)
            : base("a Giant Scarab", 0x317, 0x3EBC, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{			
			Hue = 2592;

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
		}
        
        public override void PrepareToGiveBirth()
		{
			GiveBirth( new GiantScarab() );
		}

		public override int GetAngerSound()
		{
			return 0x21D;
		}

		public override int GetIdleSound()
		{
			return 0x21D;
		}

		public override int GetAttackSound()
		{
			return 0x162;
		}

		public override int GetHurtSound()
		{
			return 0x163;
		}

		public override int GetDeathSound()
		{
			return 0x21D;
		}

		public override int Meat{ get{ return 10; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }	

		public GiantScarab( Serial serial ) : base( serial )
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
