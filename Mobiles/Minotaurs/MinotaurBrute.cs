using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Minotaur Brute corpse" )]
	
	public class MinotaurBrute : BaseCreature, ILargePredator, IMinotaur, IEnraged
	{
		[Constructable]
		public MinotaurBrute() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Minotaur Brute";
			Body = 281;
			BaseSoundID = 427;

			SetStr( 167, 195 );
			SetDex( 26, 35 );
			SetInt( 35 );

			SetHits( 800 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Blunt, 100 );			

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 50 );
			SetResistance( ResistanceType.Slashing, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 90.0 );

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 35;

			PackItem( new WarHammer() );
		}
		
		public override bool HasFur{ get{ return true; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new MinotaurHorn( 2 ) );
            bpc.DropItem(new MinotaurHooves()); 
		}

		public override int Meat{ get{ return 25; } }
		public override int Bones{ get{ return 15; } }
		public override int Hides{ get{ return 9; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Rich, 1 );
		}

		public MinotaurBrute( Serial serial ) : base( serial )
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
