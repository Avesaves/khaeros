using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class HornedBeetle : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public HornedBeetle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a horned beetle";
			Body = 43;

			SetStr( 101, 160 );
			SetDex( 121, 170 );
			SetInt( 26, 30 );

			SetHits( 191, 220 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 35, 50 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 75, 95 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Invocation, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.Poisoning, 120.1, 140.0 );
			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 78.1, 93.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 77.5 );

			Fame = 6000;
			Karma = -6000;
				
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
			AddLoot( LootPack.Poor, 1 );
		}
		
		public HornedBeetle( Serial serial ) : base( serial )
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
