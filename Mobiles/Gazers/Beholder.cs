using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Beholder corpse" )]
	public class Beholder : BaseCreature, IMediumPredator, IEnraged, IBeholder
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Beholder () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Beholder";
			Body = 22;
			BaseSoundID = 377;

			SetStr( 296, 325 );
			SetDex( 46, 55 );
			SetInt( 291, 385 );

			SetHits( 178, 195 );

			SetDamage( 12, 20 );

			SetDamageType( ResistanceType.Slashing, 50 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 45, 55 );
			SetResistance( ResistanceType.Slashing, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 40;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BeholdersEye() );
            bpc.DropItem(new BeholderArm()); 
		}
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
		}
		
		public Beholder( Serial serial ) : base( serial )
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
