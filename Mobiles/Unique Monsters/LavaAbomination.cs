using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class LavaAbomination : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public LavaAbomination() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava abomination";
			Body = 302;
			BaseSoundID = 451;
			Hue = 2618;

			SetStr( 181, 200 );
			SetDex( 135, 185 );
			SetInt( 100 );

			SetHits( 435, 485 );

			SetDamage( 16, 19 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 60, 70 );
			SetResistance( ResistanceType.Slashing, 60, 70 );
			SetResistance( ResistanceType.Fire, 90, 100 );
			SetResistance( ResistanceType.Cold, 15, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 60.1, 70.0 );
			SetSkill( SkillName.MagicResist, 55.1, 70.0 );
			SetSkill( SkillName.Tactics, 90.0, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0, 110.0 );
			SetSkill( SkillName.Swords, 100.0, 110.0 );

			Fame = 20000;
			Karma = -20000;
			
			this.RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FullAOE;

			VirtualArmor = 68;

			PackItem( new Bone( 7 ) );
			EquipItem( new LightSource() );
			EquipItem( new Greatsword() );
			PackItem( new RewardToken( 1 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}
		
		public override bool BleedImmune{ get{ return true; } }

		public LavaAbomination( Serial serial ) : base( serial )
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
