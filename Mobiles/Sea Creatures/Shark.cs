using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a Shark corpse" )]
	public class Shark : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Shark()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a Shark";
			Body = 0x97;
			BaseSoundID = 447;

			SetStr( 121, 149 );
			SetDex( 16, 25 );
			SetInt( 35 );

			SetHits( 135, 147 );

			SetDamage( 13, 16 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Slashing, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );
			SetResistance( ResistanceType.Piercing, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 79.2, 89.0 );
			SetSkill( SkillName.UnarmedFighting, 79.2, 99.0 );

			Fame = 3500;
			Karma = -2000;

			VirtualArmor = 26;
			CanSwim = true;
			CantWalk = true;
		}

		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public Shark( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
