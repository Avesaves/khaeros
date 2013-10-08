using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a larva corpse" )]
	public class BeetleLarva : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public BeetleLarva() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a beetle larva";
			Body = 319;

			SetStr( 167, 185 );
			SetDex( 46, 55 );
			SetInt( 35 );

			SetHits( 100, 195 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );			

			SetResistance( ResistanceType.Blunt, 30, 45 );
			SetResistance( ResistanceType.Piercing, 30, 50 );
			SetResistance( ResistanceType.Slashing, 35 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 90.0 );

			Fame = 6000;
			Karma = -6000;

		}

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
			AddLoot( LootPack.Meager, 1 );
		}

		public BeetleLarva( Serial serial ) : base( serial )
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
