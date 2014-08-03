using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blue crystal elemental corpse" )]
	public class Quest3 : BaseCreature, IElemental, IEnraged
	{
		[Constructable]
		public Quest3() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a blue crystal elemental";
			BodyValue = 33;
			BaseSoundID = 278;

			SetStr( 236, 260 );
			SetDex( 51, 65 );
			SetInt( 35 );

			SetHits( 500 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Blunt, 40 );
			SetDamageType( ResistanceType.Piercing, 60 );

			SetResistance( ResistanceType.Blunt, 20, 30 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 55, 70 );

			SetSkill( SkillName.Invocation, 70.1, 75.0 );
			SetSkill( SkillName.Magery, 70.1, 75.0 );
			SetSkill( SkillName.Meditation, 65.1, 75.0 );
			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 75.1, 85.0 );
			SetSkill( SkillName.UnarmedFighting, 65.1, 75.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 34;
			
			PackItem( new Quest3Item() );
            int rand = Utility.Random(250);
            if (rand > 249)
                PackItem(new StarmetalOre(2)); 
		}

		public override bool BleedImmune{ get{ return true; } }

				public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 1 );
		}

		public Quest3( Serial serial ) : base( serial )
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
