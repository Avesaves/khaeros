using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a ghost corpse" )]
	public class Ghost : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public Ghost() : base( AIType.AI_Melee, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			Name = "a ghost";
			BodyValue = 146;
			Hue = 12345678;

			SetStr( 90, 100 );
			SetDex( 25, 35 );
			SetInt( 35 );
			
			SetHits( 230, 244 );

			SetDamage( 10, 12 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 55, 65 );
			SetResistance( ResistanceType.Piercing, 55, 65 );
			SetResistance( ResistanceType.Slashing, 55, 65 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.2, 110.0 );
			SetSkill( SkillName.MagicResist, 80.2, 90.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Invocation, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			
			PackItem( new Necroplasm( 3 ) );

		}

		public Ghost( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}
