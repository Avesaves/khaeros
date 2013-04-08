using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lesser air elemental corpse" )]
	public class LesserAirElemental : BaseCreature, IElemental, IEnraged, IIncorporeal
	{
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }

		[Constructable]
		public LesserAirElemental () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lesser air elemental";
			Body = 13;
			Hue = 12345678;
			BaseSoundID = 655;

			SetStr( 126, 155 );
			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 176, 193 );

			SetDamage( 12, 14 );

			SetDamageType( ResistanceType.Blunt, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Invocation, 60.1, 75.0 );
			SetSkill( SkillName.Magery, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 30;
			ControlSlots = 2;
		}

		public override bool BleedImmune{ get{ return true; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 1 );
		}

		public LesserAirElemental( Serial serial ) : base( serial )
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
				BaseSoundID = 655;
		}
	}
}
