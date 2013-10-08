using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a beetle corpse" )]
	public class DesertCrawler : BaseCreature, ILargePredator, IGiantBug
	{
		[Constructable]
		public DesertCrawler() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a desert crawler";
			Body = 787;
			Hue = 2713;

			SetStr( 190, 220 );
			SetDex( 45, 65 );
			SetInt( 21, 30 );

			SetHits( 350, 450 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 30, 40 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 15, 20 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 20 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Poisoning, 160.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 75.0, 85.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 40;
		}

		public override int Meat{ get{ return 10; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }

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
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 1 );
		}

		public DesertCrawler( Serial serial ) : base( serial )
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
