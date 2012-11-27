using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Goatman Archer corpse" )]
	public class GoatmanArcher : BaseCreature, IMediumPredator, IEnraged, IGoatman
	{
		[Constructable]
		public GoatmanArcher() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Goatman Archer";
			Body = 42;
			BaseSoundID = 461;

			SetStr( 67, 85 );
			SetDex( 106, 115 );
			SetInt( 26, 30 );

			SetHits( 210, 226 );

			SetDamage( 13, 16 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );
			SetSkill( SkillName.Archery, 80.1, 100.0 );

			Fame = 7000;
			Karma = -7000;

			VirtualArmor = 30;
			Bow bow = new Bow();
			AddItem( bow );
			EquipItem( bow );
			PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GoatmanFur() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}
		
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }

		public GoatmanArcher( Serial serial ) : base( serial )
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
