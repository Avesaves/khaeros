using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yuan-Ti corpse" )]
	public class YuanTi : BaseCreature, ILargePredator, IYuanTi
	{
		[Constructable]
		public YuanTi() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Yuan-Ti";
			Body = 87;
			BaseSoundID = 644;
			Hue = 2582;
			Female = true;

			SetStr( 96, 105 );
			SetDex( 26, 35 );
			SetInt( 35 );

			SetHits( 190, 203 );

			SetDamage( 8, 13 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 5.4, 25.0 );
			SetSkill( SkillName.MagicResist, 40.1, 50.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 30;
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new SerpentBile());

        }
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public YuanTi( Serial serial ) : base( serial )
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
