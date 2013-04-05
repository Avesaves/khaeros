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
			Name = NameList.RandomName( "beastman" )
			Body = 30;

			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 176, 193 );

			SetDamage( 14, 16 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 50.1, 95.0 );
			SetSkill( SkillName.Tactics, 60.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 100.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 34;
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
