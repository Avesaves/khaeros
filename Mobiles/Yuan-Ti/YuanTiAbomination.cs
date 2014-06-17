using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yuan-Ti Abomination corpse" )]
	public class YuanTiAbomination : BaseCreature, ILargePredator, IHasReach, IEnraged, IYuanTi
	{
		[Constructable]
		public YuanTiAbomination() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Yuan-Ti Abomination";
			BodyValue = 4;
			BaseSoundID = 634;
			Hue = 0;
			Female = true;

			SetStr( 296, 305 );
			SetDex( 36, 55 );
			SetInt( 35 );

			SetHits( 520, 653 );

			SetDamage( 26, 32 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 5.4, 25.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Macing, 90.1, 100.0 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 6000;
			Karma = -6000;

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 50;
            PackItem( new RewardToken( 2 ) );
		}

		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new SerpentBile());

        }
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public YuanTiAbomination( Serial serial ) : base( serial )
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
