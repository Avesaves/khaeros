using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a goblin corpse" )]
	public class Quest1 : BaseCreature, IMediumPredator, IGoblin
	{
		//public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Quest1() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = "a strange goblin thing";
			BodyValue = 793;
			BaseSoundID = 594;
            
			SetStr( 46, 65 );
			SetDex( 126, 135 );
			SetInt( 35 );
            Hue = 2832; 
			SetHits( 100, 200 );
			SetMana( 0 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.Poisoning, 40.1, 60.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 30.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 50.0 );

			Fame = 1800;
			Karma = -1800;

            PackItem(new Quest1Item());
			VirtualArmor = 5;

            int rand = Utility.Random(1000);
            if (rand > 999)
                PackItem(new StarmetalOre(1));  
            
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			//bpc.DropItem( new GoblinBrain() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 2 );

		}
		
		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }
		

		public Quest1( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 589 )
				BaseSoundID = 594;
		}
	}
}
