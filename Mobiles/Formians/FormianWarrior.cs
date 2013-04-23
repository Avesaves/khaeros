using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a formian warrior corpse" )]
	public class FormianWarrior : BaseCreature, IMediumPredator, IEnraged, IFormian
	{
		public override int Height{ get{ return 10; } }
		
		[Constructable]
		public FormianWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a formian warrior";
			Body = 806;
			BaseSoundID = 959;
			Hue = 0;

			SetStr( 206, 230 );
			SetDex( 61, 85 );
			SetInt( 35 );

			SetHits( 96, 107 );

			SetDamage( 5, 12 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Blunt, 20, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 20, 35 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 20, 35 );
			SetResistance( ResistanceType.Energy, 10, 25 );

			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;		
		}

		public override int GetAngerSound()
		{
			return 0xB5;
		}

		public override int GetIdleSound()
		{
			return 0xB5;
		}

		public override int GetAttackSound()
		{
			return 0x289;
		}

		public override int GetHurtSound()
		{
			return 0xBC;
		}

		public override int GetDeathSound()
		{
			return 0xE4;
		}
		
		public override int Meat{ get{ return 8; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 1 );
		}

		public FormianWarrior( Serial serial ) : base( serial )
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
