using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lynx corpse" )]
	[TypeAlias( "Server.Mobiles.Snowleopard" )]
	public class Lynx : BaseCreature, IMediumPredator, ITundraCreature, IFeline
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Lynx() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lynx";
			Body = Utility.RandomList( 64, 65 );
			BaseSoundID = 0x73;

			SetStr( 56, 80 );
			SetDex( 36, 45 );
			SetInt( 25 );

			SetHits( 34, 48 );
			SetMana( 0 );

			SetDamage( 3, 9 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 50.0 );

			Fame = 750;
			Karma = 0;

			VirtualArmor = 24;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 53.1;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public Lynx(Serial serial) : base(serial)
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
