using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a krakens corpse" )]
	public class Kraken : BaseCreature, ILargePredator, IEnraged, ISerpent
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Kraken() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kraken";
			Body = 77;
			BaseSoundID = 353;

			SetStr( 256, 280 );
			SetDex( 26, 45 );
			SetInt( 35 );

			SetHits( 854, 968 );
			SetMana( 0 );

			SetDamage( 19, 23 );

			SetDamageType( ResistanceType.Blunt, 70 );
			SetDamageType( ResistanceType.Cold, 30 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 60.0 );

			Fame = 26000;
			Karma = -26000;

			VirtualArmor = 50;
            RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FullAOE;

			CanSwim = true;
			CantWalk = true;
            PackItem( new RewardToken( 2 ) );
		}

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new Gills());

        }
		public override int Meat { get { return 30; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich);
		}

		public Kraken( Serial serial ) : base( serial )
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
