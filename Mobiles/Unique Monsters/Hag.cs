using System;
using System.Collections;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a hag corpse" )]
	public class Hag : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Hag()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a hag";
			Body = 252;
			BaseSoundID = 0x482;
			Female = true;

			SetStr( 36, 45 );
			SetDex( 106, 125 );
			SetInt( 271, 295 );

			SetHits( 96, 115 );

			SetDamage( 3, 6 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 55 );
			SetResistance( ResistanceType.Cold, 70, 90 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 65, 85 );

			SetSkill( SkillName.Magery, 95.1, 110.0 );
			SetSkill( SkillName.MagicResist, 45.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 8000;
			Karma = -8000;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new HagHair() );
		}
		
		public override int Bones{ get{ return 2; } }

		public override int GetDeathSound()
		{
			return 0x370;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool BleedImmune { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public Hag( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
