using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class PincerBeetle : BaseCreature, ILargePredator, IEnraged, IGiantBug
	{
		[Constructable]
		public PincerBeetle() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a pincer beetle";
			Body = 242;

			SetStr( 356, 385 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 622, 651 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Macing, 90.1, 100.0 );

			Fame = 12000;
			Karma = -12000;

			PackItem( new RewardToken( 1 ) );	

			MeleeAttackType = MeleeAttackType.FrontalAOE;		
		}

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if ( Utility.Random(100) > 80 )
			{
                XmlBleedingWound.BeginBleed(defender, this, Utility.RandomMinMax(30, 40));
                this.Emote("*Cuts " + defender.Name + "with its horrible pincers!*");
                
            } 

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

		public PincerBeetle( Serial serial ) : base( serial )
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
