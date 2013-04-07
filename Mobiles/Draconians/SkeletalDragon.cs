using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragon : BaseCreature, IUndead
	{
		[Constructable]
		public SkeletalDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal dragon";
			Body = 104;
			BaseSoundID = 0x488;

			SetStr( 298, 330 );
			SetDex( 68, 80 );
			SetInt( 18, 20 );

			SetHits( 318, 329 );

			SetDamage( 19, 25 );

			SetDamageType( ResistanceType.Piercing, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Blunt, 35, 55 );
			SetResistance( ResistanceType.Piercing, 60, 70 );
			SetResistance( ResistanceType.Slashing, 60, 70 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Invocation, 80.1, 100.0 );
			SetSkill( SkillName.Magery, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 60;
			
			PackItem( new Bone( 50 ) );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BleedImmune{ get{ return true; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
		}

		public SkeletalDragon( Serial serial ) : base( serial )
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
