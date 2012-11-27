using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Constricting Vine corpse" )]
	public class ConstrictingVine : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public ConstrictingVine() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Constricting Vine";
			Body = 66;
			BaseSoundID = 352;
			Hue = 0;

			SetStr( 96, 120 );
			SetDex( 26, 35 );
			SetInt( 16, 30 );

			SetHits( 58, 72 );
			SetMana( 0 );

			SetDamage( 6, 8 );

			SetDamageType( ResistanceType.Blunt, 40 );
			SetDamageType( ResistanceType.Poison, 60 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 60, 80 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 65.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 65.1, 80.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;
		}

		public ConstrictingVine( Serial serial ) : base( serial )
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
