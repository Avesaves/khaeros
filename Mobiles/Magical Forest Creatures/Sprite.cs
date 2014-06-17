using System;
using Server;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Spells;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class Sprite : BaseCreature, IMagicalForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Sprite() : base( AIType.AI_Mage, FightMode.Evil, 18, 1, 0.1, 0.2 )
		{
			Name = "a Sprite";
			Body = 176;
			BaseSoundID = 0x467;

			SetStr( 53, 100 );
			SetDex( 57, 60 );
			SetInt( 103, 120 );

			SetHits( 120 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Blunt, 75 );
			SetDamageType( ResistanceType.Piercing, 25 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 100.0 );
			SetSkill( SkillName.Magery, 57.6, 67.5 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.MagicResist, 70.5, 80.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 5000;
			Karma = 5000;

			VirtualArmor = 30;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new FairyWing( 2 ) );
            bpc.DropItem(new FairyShroom(2));
            bpc.DropItem(new MutilatedFairy());
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 2, 4 ) );
		}

		public Sprite( Serial serial ) : base( serial )
		{
		}
		
		public override void OnKilledBy( Mobile mob )
		{
			if( mob is PlayerMobile )
				( (PlayerMobile)mob ).LastOffenseToNature = DateTime.Now;
			
			base.OnKilledBy( mob );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
