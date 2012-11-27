using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a walrus corpse" )]
	public class Walrus : BaseCreature, IMediumPrey, ITundraCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Walrus() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a walrus";
			Body = 0xDD;
			BaseSoundID = 0xE0;

			SetStr( 21, 29 );
			SetDex( 16, 25 );
			SetInt( 16, 20 );

			SetHits( 14, 17 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Slashing, 20, 25 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 19.2, 29.0 );
			SetSkill( SkillName.UnarmedFighting, 19.2, 29.0 );

			Fame = 150;
			Karma = 0;

			VirtualArmor = 18;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 35.1;
		}

		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 2; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish; } }

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new WalrusBlubber());
        }

		public Walrus(Serial serial) : base(serial)
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
