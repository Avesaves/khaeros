using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a flamingo corpse" )]
	public class Flamingo : BaseCreature, IMediumPrey, IPlainsCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Flamingo() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a flamingo";
			Body = 254;
			BaseSoundID = 0x4D7;
			Hue = 1166;

			SetStr( 26, 35 );
			SetDex( 16, 25 );
			SetInt( 11, 15 );

			SetHits( 9, 12 );
			SetMana( 0 );

			SetDamage( 1, 1 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 5, 5 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 10.1, 11.0 );
			SetSkill( SkillName.UnarmedFighting, 10.1, 11.0 );

			Fame = 0;
			Karma = 50;

			VirtualArmor = 5;
		}

		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 6; } }


		public override int GetAngerSound()
		{
			return 0x4D9;
		}

		public override int GetIdleSound()
		{
			return 0x4D8;
		}

		public override int GetAttackSound()
		{
			return 0x4D7;
		}

		public override int GetHurtSound()
		{
			return 0x4DA;
		}

		public override int GetDeathSound()
		{
			return 0x4D6;
		}

		public Flamingo(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
