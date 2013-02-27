using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a boa constrictor corpse" )]
	public class BoaConstrictor : BaseCreature, ILargePredator, ISerpent
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public BoaConstrictor() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a boa constrictor";
			BodyValue = 89;
			Hue = 1442;
			BaseSoundID = 219;

			SetStr( 186, 215 );
			SetDex( 36, 40 );
			SetInt( 26, 35 );

			SetHits( 212, 229 );
			SetMana( 0 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Piercing, 40 );
			SetDamageType( ResistanceType.Blunt, 60 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 70, 90 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Poisoning, 70.1, 100.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 65.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 22;
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
        public override HideType HideType { get { return HideType.Scaled; } }

		public BoaConstrictor(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

            if( version < 1 )
                BodyValue = 89;

			if ( BaseSoundID == -1 )
				BaseSoundID = 219;
		}
	}
}
