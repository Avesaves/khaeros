using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dunedigger corpse" )]
	public class DuneDigger : BaseCreature, ILargePredator, IGiantBug
	{
		[Constructable]
		public DuneDigger() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dunedigger";
			Body = 315;
			Hue = 2108;

			SetStr( 101, 160 );
			SetDex( 51, 60 );
			SetInt( 11, 20 );

			SetHits( 280 );

			SetDamage( 14, 16 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 23;
		}

		public override int Meat{ get{ return 10; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 1 );
		}

		public override int GetAttackSound()
		{
			return 0x34C;
		}

		public override int GetHurtSound()
		{
			return 0x354;
		}

		public override int GetAngerSound()
		{
			return 0x34C;
		}

		public override int GetIdleSound()
		{
			return 0x34C;
		}

		public override int GetDeathSound()
		{
			return 0x354;
		}

		public DuneDigger( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 660 )
				BaseSoundID = -1;
		}
	}
}
