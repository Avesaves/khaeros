using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class RhinoBeetle : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public RhinoBeetle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rhino beetle";
			Body = 43;

			SetStr( 201, 260 );
			SetDex( 121, 170 );
			SetInt( 26, 30 );

			SetHits( 750, 850 );

			SetDamage( 35, 50 );

			SetDamageType( ResistanceType.Piercing, 50 );
			SetDamageType( ResistanceType.Blunt, 50 );

			SetResistance( ResistanceType.Blunt, 15, 25 );
			SetResistance( ResistanceType.Piercing, 65, 75 );
			SetResistance( ResistanceType.Slashing, 65, 75 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 75, 95 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Invocation, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.Poisoning, 120.1, 140.0 );
			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 88.1, 93.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 99.5 );

			Fame = 35000;
			Karma = -35000;
			PackItem( new RewardToken( 4 ) );
			
			RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FullAOE;
				
			ControlSlots = 3;
			MinTameSkill = 93.9;	
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new HornedBeetleHorn() );
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
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		
						public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public RhinoBeetle( Serial serial ) : base( serial )
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
