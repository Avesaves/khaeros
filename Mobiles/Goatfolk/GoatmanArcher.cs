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

			SetStr( 66, 75 );
			SetDex( 135, 150 );
			SetInt( 35 );

			SetHits( 45, 51 );
			SetMana( 0 );

			SetDamage( 4, 6 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );
			SetSkill( SkillName.Archery, 80.1, 100.0 );

			Fame = 1600;
			Karma = -1600;

			VirtualArmor = 10;
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
			AddLoot( LootPack.Meager, 1 );
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
