using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Troglin corpse" )]
	public class Troglin : BaseCreature, IRegenerativeCreature, IMediumPredator, ITroglin
	{
		[Constructable]
		public Troglin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Troglin";
			Body = 282;
			BaseSoundID = 461;

			SetStr( 67, 85 );
			SetDex( 36, 45 );
			SetInt( 26, 30 );

			SetHits( 180, 196 );

			SetDamage( 10, 12 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 30;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}
		
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public Troglin( Serial serial ) : base( serial )
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
