using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gambol corpse" )]
	public class Gambol : BaseCreature, ILargePredator, IJungleCreature, IWesternFavoredEnemy, IEnraged
	{
		[Constructable]
		public Gambol() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gambol";
			Body = 0x7c;
			BaseSoundID = 0x9E;
			Hue = 2879;

			SetStr( 153, 195 );
			SetDex( 36, 45 );
			SetInt( 35 );

			SetHits( 238, 251 );
			SetMana( 0 );

			SetDamage( 14, 17 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40, 45 );
			SetResistance( ResistanceType.Piercing, 25, 30 );
			SetResistance( ResistanceType.Slashing, 30, 35 );
			SetResistance( ResistanceType.Poison, 25, 30 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 63.3, 78.0 );
			SetSkill( SkillName.UnarmedFighting, 63.3, 78.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 20;
		}
		
		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public Gambol(Serial serial) : base(serial)
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
