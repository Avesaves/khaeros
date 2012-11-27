using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Lesser Huorn corpse" )]
	public class LesserHuorn : BaseCreature, IClericSummon
	{
		[Constructable]
		public LesserHuorn() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a Lesser Huorn";
			BodyValue = 250;
			BaseSoundID = 442;

			SetStr( 146, 170 );
			SetDex( 50, 60 );
			SetInt( 60 );

			SetHits( 118, 122 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 40, 60 );
			SetResistance( ResistanceType.Slashing, 40, 60 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 55.1, 60.0 );

			Fame = 10;
			Karma = -4000;

			VirtualArmor = 50;
		}

		public LesserHuorn( Serial serial ) : base( serial )
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
