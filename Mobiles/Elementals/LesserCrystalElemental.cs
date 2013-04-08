using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lesser crystal elemental corpse" )]
	public class LesserCrystalElemental : BaseCreature, IElemental, IEnraged
	{
		[Constructable]
		public LesserCrystalElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lesser crystal elemental";
			Body = 300;
			BaseSoundID = 278;

			SetStr( 136, 160 );
			SetDex( 51, 65 );
			SetInt( 35 );

			SetHits( 150 );

			SetDamage( 10, 12 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Energy, 20 );

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

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 34;
			
			PackItem( new Crystal() );
		}

		public override bool BleedImmune{ get{ return true; } }

				public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 1 );
		}

		public LesserCrystalElemental( Serial serial ) : base( serial )
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
