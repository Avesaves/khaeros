using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a hydra's corpse" )]
	public class Hydra : BaseCreature, ILargePredator, IEnraged, ISerpent
	{
		[Constructable]
		public Hydra() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Hydra";
			Body = 265;
			BaseSoundID = 0x388;

			SetStr( 276, 305 );
			SetDex( 46, 65 );
			SetInt( 35 );

			SetHits( 1550, 1560 );

			SetDamage( 31, 40 );
			
			SetDamageType( ResistanceType.Piercing, 60 );
			SetDamageType( ResistanceType.Blunt, 40 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );

			SetSkill( SkillName.Invocation, 120.1, 140.0 );
			SetSkill( SkillName.Magery, 120.1, 140.0 );
			SetSkill( SkillName.MagicResist, 50.5, 60.0 );
			SetSkill( SkillName.Tactics, 120.1, 140.0 );
			SetSkill( SkillName.UnarmedFighting, 120.1, 140.0 );

			Fame = 128000;
			Karma = -128000;

			VirtualArmor = 50;
            RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FullAOE;
			
			CanSwim = true;
			CantWalk = true;
			PackItem( new SulfurousAsh( 30 ) );
            PackItem( new RewardToken( 15 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new HydraScale() );
            bpc.DropItem(new Gills());
		}

		public override bool HasBreath{ get{ return true; } }
		public override double BreathDamageScalar{ get{ return 0.05; } }
		
		public override int Meat { get { return 30; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss);
		}

		public Hydra( Serial serial ) : base( serial )
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
