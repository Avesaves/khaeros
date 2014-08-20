using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Dire Bear corpse" )]
    public class DireBear : BaseMount, ILargePredator, ITundraCreature, IRacialMount
	{
		public override bool ParryDisabled{ get{ return true; } }
		public override int[] Hues{ get{ return new int[]{2581,2802,2992}; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }
		
		[Constructable]
		public DireBear() : this( "a Dire Bear" )
		{
		}
		
		[Constructable]
        public DireBear(string name)
            : base("a Dire Bear", 213, 0x3EC5, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Body = 213;
			BaseSoundID = 0xA3;

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

		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public DireBear( Serial serial ) : base( serial )
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new DireBear() );
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
