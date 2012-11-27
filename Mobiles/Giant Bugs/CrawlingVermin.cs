using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Crawling Vermin" )]
	public class CrawlingVermin : BaseCreature, IMediumPrey, IGiantBug
	{
		[Constructable]
		public CrawlingVermin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Crawling Vermin";
			Body = 31;
			Hue = 0;
			BaseSoundID = 898;

			SetStr( 26, 50 );
			SetDex( 36, 55 );
			SetInt( 16, 30 );

			SetHits( 26, 30 );

			SetDamage( 3, 5 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );

			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 25.1, 40.0 );
			SetSkill( SkillName.UnarmedFighting, 25.1, 40.0 );

			Fame = 750;
			Karma = -750;

			VirtualArmor = 18;
		}

		public override int Meat{ get{ return 2; } }
		
		public override bool CanRummageCorpses{ get{ return true; } }

		public CrawlingVermin( Serial serial ) : base( serial )
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
