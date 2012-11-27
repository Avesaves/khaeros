using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a spectre corpse" )]
	public class Spectre : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public Spectre() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a spectre";
			Body = 153;
			BaseSoundID = 0x482;
			Hue = 12345678;

			SetStr( 76, 100 );
			SetDex( 76, 95 );
			SetInt( 35 );

			SetHits( 286, 290 );
			SetMana( 0 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 55.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 48;
			
			PackItem( new Necroplasm( 9 ) );
		}

		public override bool BleedImmune{ get{ return true; } }

		public Spectre( Serial serial ) : base( serial )
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
