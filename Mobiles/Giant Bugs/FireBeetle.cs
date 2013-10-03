using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class FireBeetle : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public FireBeetle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fire beetle";
			Body = 244;
			Hue = 2650;

			SetStr( 350, 400 );
			SetDex( 50, 75 );
			SetInt( 35 );

			SetHits( 650, 775 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Blunt, 100 );			

			SetResistance( ResistanceType.Blunt, 10, 20 );
			SetResistance( ResistanceType.Piercing, 50, 65 );
			SetResistance( ResistanceType.Slashing, 50, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 90.0 );

			Fame = 30000;
			Karma = -30000;

			PackItem( new SulfurousAsh( 10 ) );
            PackItem( new RewardToken( 2 ) );	

            MeleeAttackType = MeleeAttackType.FrontalAOE;			
		}
		
		public override bool HasBreath{ get{ return true; } }
		public override double BreathDamageScalar{ get{ return 0.05; } }
		
		public override int GetAngerSound()
		{
			return 0x4E8;
		}

		public override int GetIdleSound()
		{
			return 0x4E7;
		}

		public override int GetAttackSound()
		{
			return 0x4E6;
		}

		public override int GetHurtSound()
		{
			return 0x4E9;
		}

		public override int GetDeathSound()
		{
			return 0x4E5;
		}

		public override int Meat{ get{ return 20; } }

				public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public FireBeetle( Serial serial ) : base( serial )
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
