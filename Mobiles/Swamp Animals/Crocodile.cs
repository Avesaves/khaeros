using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a crocodile corpse" )]
	public class Crocodile : BaseCreature, IMediumPredator, IReptile
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Crocodile() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a crocodile";
			Body = 0xCE;
			Hue = 1443;
			BaseSoundID = 0x5A;

			SetStr( 126, 150 );
			SetDex( 36, 45 );
			SetInt( 11, 20 );

			SetHits( 76, 90 );
			SetMana( 0 );

			SetDamage( 6, 14 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;
		}

		public override int Hides{ get{ return 5; } }
		public override int Bones{ get{ return 8; } }
		public override int Meat{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public Crocodile(Serial serial) : base(serial)
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
