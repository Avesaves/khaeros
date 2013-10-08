using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class RuneBeetle : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public RuneBeetle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rune beetle";
			Body = 244;

			SetStr( 167, 185 );
			SetDex( 46, 55 );
			SetInt( 35 );

			SetHits( 300 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Blunt, 100 );			

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 50 );
			SetResistance( ResistanceType.Slashing, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 90.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 30;
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
			AddLoot( LootPack.Average, 1 );
		}

		public RuneBeetle( Serial serial ) : base( serial )
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
