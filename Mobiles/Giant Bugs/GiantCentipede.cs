using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a centipede corpse" )]
	public class GiantCentipede : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public GiantCentipede() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant centipede";
			Body = 31;

			SetStr( 175, 200 );
			SetDex( 50, 75 );
			SetInt( 35 );

			SetHits( 450, 500 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Piercing, 50 );	
			SetDamageType( ResistanceType.Poison, 50 );			

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
		
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 50)
		}; } }
		public override int PoisonDuration { get { return 180; } }
		public override int PoisonActingSpeed { get { return 3; } }

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new LargeInsectVenom() );
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
			AddLoot( LootPack.Rich, 1 );
		}

		public GiantCentipede( Serial serial ) : base( serial )
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
