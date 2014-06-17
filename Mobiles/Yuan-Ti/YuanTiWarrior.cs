using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yuan-Ti warrior corpse" )]
	public class YuanTiWarrior : BaseCreature, ILargePredator, IEnraged, IYuanTi
	{
		[Constructable]
		public YuanTiWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Yuan-Ti warrior";
			BodyValue = 86;
			BaseSoundID = 634;
			Hue = 2017;

			SetStr( 196, 205 );
			SetDex( 36, 55 );
			SetInt( 35 );

			SetHits( 248, 255 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 80 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 5.4, 25.0 );
			SetSkill( SkillName.MagicResist, 50.1, 60.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 50;
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new SerpentBile());

        }
		public override int Meat{ get{ return 12; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public YuanTiWarrior( Serial serial ) : base( serial )
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
