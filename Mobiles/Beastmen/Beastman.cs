using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Beastman corpse" )]
	public class Beastman : BaseCreature, IMediumPredator, IBeastman, IEnraged
	{
		[Constructable]
		public Beastman() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Beastman";
			Body = 30;

			SetStr( 101, 110 );
			SetDex( 81, 90 );
			SetInt( 11, 25 );

			SetHits( 101, 130 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 45, 70 );
			SetResistance( ResistanceType.Energy, 45, 65 );

			SetSkill( SkillName.Invocation, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 96.1, 106.0 );
			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 86.1, 101.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Parry, 100.0 );

			Fame = 3500;
			Karma = -3500;
			
			VirtualArmor = 20;
		}

		public override int GetAngerSound()
		{
			return 0x4E3;
		}

		public override int GetIdleSound()
		{
			return 0x4E2;
		}

		public override int GetAttackSound()
		{
			return 0x4E1;
		}

		public override int GetHurtSound()
		{
			return 0x4E4;
		}

		public override int GetDeathSound()
		{
			return 0x4E0;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}
		
		public override bool HasFur { get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public override bool CanRummageCorpses{ get{ return true; } }

		public Beastman( Serial serial ) : base( serial )
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
