using System;
using Server;
using Server.Items;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a Drider queen corpse" )]
	public class DriderQueen : BaseCreature, IMediumPredator, IEnraged, IDrider
	{
		[Constructable]
		public DriderQueen() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Drider queen";
			Body = 152;
			BaseSoundID = 0x24D;
			Hue = 2886;

			SetStr( 467, 645 );
			SetDex( 77, 95 );
			SetInt( 70 );

			SetHits( 826, 872 );
			SetMana( 446, 470 );

			SetDamage( 18, 22 );

			SetDamageType( ResistanceType.Piercing, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 80 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 70.3, 100.0 );
			SetSkill( SkillName.Magery, 70.3, 100.0 );
			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );

			Fame = 22000;
			Karma = -22000;

			VirtualArmor = 50;
			
			PackItem( new SpidersSilk( 20 ) );
            PackItem( new RewardToken( 2 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 60)
		}; } }
		public override int PoisonDuration { get { return 180; } }
		public override int PoisonActingSpeed { get { return 3; } }
		public override int Meat{ get{ return 10; } }

		public DriderQueen( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 0x24D;
		}
	}
}
