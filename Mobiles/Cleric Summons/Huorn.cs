using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a  Huorn corpse" )]
	public class Huorn : BaseCreature, IClericSummon
	{
		[Constructable]
		public Huorn() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a Huorn";
			BodyValue = 250;
			BaseSoundID = 442;

			SetStr( 146, 170 );
			SetDex( 60, 70 );
			SetInt( 65 );

			SetHits( 228, 232 );

			SetDamage( 13, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 40, 60 );
			SetResistance( ResistanceType.Slashing, 40, 60 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 75.1, 80.0 );

			Fame = 10;
			Karma = -4000;

			VirtualArmor = 55;
		}

		public Huorn( Serial serial ) : base( serial )
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
