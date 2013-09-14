using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a jaguar corpse" )]
	[TypeAlias( "Server.Mobiles.Snowleopard" )]
	public class Jaguar : BaseCreature, IMediumPredator, IJungleCreature, IEnraged, IFeline
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Jaguar() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a jaguar";
			Body = 0x7E;
			BaseSoundID = 0x73;

			SetStr( 140, 160 );
			SetDex( 165, 185 );
			SetInt( 26, 30 );

			SetHits( 200, 250 );
			SetMana( 0 );

			SetDamage( 5, 20 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 40 );
			SetResistance( ResistanceType.Piercing, 20, 40 );
			SetResistance( ResistanceType.Slashing, 20, 40 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 24;

			Tamable = true;
			ControlSlots = 5;
			MinTameSkill = 53.1;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 12; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public Jaguar(Serial serial) : base(serial)
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
