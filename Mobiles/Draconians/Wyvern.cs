using System;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wyvern corpse" )]
	public class Wyvern : BaseCreature, ILargePredator, IEnraged, IDraconic
	{
		[Constructable]
		public Wyvern () : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wyvern";
			Body = 62;
			BaseSoundID = 362;

			SetStr( 102, 140 );
			SetDex( 53, 72 );
			SetInt( 35 );

			SetHits( 565, 581 );

			SetDamage( 25, 28 );

			SetDamageType( ResistanceType.Piercing, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 80 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 65.1, 90.0 );
			SetSkill( SkillName.UnarmedFighting, 65.1, 80.0 );

			Fame = 18000;
			Karma = -18000;

            RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			VirtualArmor = 50;
            PackItem( new RewardToken( 1 ) );
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 50)
		}; } }
		public override int PoisonDuration { get { return 180; } }
		public override int PoisonActingSpeed { get { return 3; } }

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new WyvernVenom() );
            bpc.DropItem(new DragonEye()); 
		}

		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 7; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public override int GetAttackSound()
		{
			return 713;
		}

		public override int GetAngerSound()
		{
			return 718;
		}

		public override int GetDeathSound()
		{
			return 716;
		}

		public override int GetHurtSound()
		{
			return 721;
		}

		public override int GetIdleSound()
		{
			return 725;
		}

		public Wyvern( Serial serial ) : base( serial )
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
