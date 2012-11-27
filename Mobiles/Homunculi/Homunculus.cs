using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Homunculus corpse" )]
	public class Homunculus : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Homunculus() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Homunculus";
			Body = 307;
			BaseSoundID = 422;

			SetStr( 41, 65 );
			SetDex( 101, 125 );
			SetInt( 56, 80 );

			SetHits( 45, 59 );

			SetDamage( 3, 6 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 67.6, 92.5 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 17;
		}

		public Homunculus( Serial serial ) : base( serial )
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
