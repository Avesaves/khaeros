using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a minotaur corpse" )]
	
	public class Minotaur : BaseCreature, ILargePredator, IMinotaur, IEnraged
	{
		[Constructable]
		public Minotaur() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Minotaur";
			Body = 263;
			BaseSoundID = 427;

			SetStr( 167, 185 );
			SetDex( 46, 55 );
			SetInt( 35 );

			SetHits( 600 );

			SetDamage( 15, 20 );

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

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 30;
		}
		
		public override bool HasFur{ get{ return true; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new MinotaurHorn( 2 ) );
            bpc.DropItem(new MinotaurHooves()); 
		}

		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		
						public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Meager, 1 );
		}

		public Minotaur( Serial serial ) : base( serial )
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
