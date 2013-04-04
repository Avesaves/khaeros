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

			SetStr( 550, 750 );
			SetDex( 200, 275 );
			SetInt( 351, 450 );

			SetHits( 352, 471 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 85, 90 );
			SetResistance( ResistanceType.Piercing, 85, 90 );
			SetResistance( ResistanceType.Slashing, 85, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 50.1, 75.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 50;
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
