using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a devourer" )]
	public class Devourer : BaseCreature, ILargePredator, IEnraged
	{
		[Constructable]
		public Devourer() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a devourer";
			Body = 303;
			BaseSoundID = 357;

			SetStr( 201, 350 );
			SetDex( 66, 75 );
			SetInt( 21, 30 );

			SetHits( 650 );

			SetDamage( 32, 36 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 25.0 );
			SetSkill( SkillName.Tactics, 75.1, 85.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 18500;
			Karma = -18500;

			VirtualArmor = 54;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new DevourersTeeth() );
		}
		
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 6; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public Devourer( Serial serial ) : base( serial )
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
