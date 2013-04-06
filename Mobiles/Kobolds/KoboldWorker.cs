using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Kobold Worker corpse" )]
	public class KoboldWorker : BaseCreature, IMediumPredator, IKobold
	{

		[Constructable]
		public KoboldWorker() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Kobold Worker";
			Body = 240;

			SetStr( 26, 30 );
			SetDex( 21, 35 );
			SetInt( 35 );

			SetHits( 21, 30 );

			SetDamage( 2, 4 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 35, 50 );
			SetResistance( ResistanceType.Cold, 25, 50 );
			SetResistance( ResistanceType.Poison, 35, 50 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 79.1, 89.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 70.0 );

			Fame = 900;
			Karma = -900;
			PackItem( new Copper( 1 ) );
		}
		
		public override FoodType FavoriteFood{ get{ return FoodType.Fish; } }
		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 6; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		 
		public override int GetAngerSound()
		{
			return 0x50B;
		}

		public override int GetIdleSound()
		{
			return 0x50A;
		}

		public override int GetAttackSound()
		{
			return 0x509;
		}

		public override int GetHurtSound()
		{
			return 0x50C;
		}

		public override int GetDeathSound()
		{
			return 0x508;
		}

		public KoboldWorker( Serial serial ) : base( serial )
		{
		}

				public override void GenerateLoot()
		{
			AddLoot( LootPack.poor, 1 );
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
