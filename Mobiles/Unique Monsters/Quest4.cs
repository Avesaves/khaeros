using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a strange corpse" )]
	public class Quest4 : BaseCreature, ILargePredator
	{
		[Constructable]
		public Quest4() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a monster";
			Body = 313;
			BaseSoundID = 0xE0;
			Hue = 2832;

			SetStr( 201, 230 );
			SetDex( 101, 110 );
			SetInt( 35 );

			SetHits( 1200 );

			SetDamage( 22, 26 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.Invocation, 118.1, 120.0 );
			SetSkill( SkillName.Magery, 112.6, 120.0 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 90.9 );

			Fame = 15000;
			Karma = -15000;
            PackItem(new Quest4Item());
			VirtualArmor = 45;
            int rand = Utility.Random(125);
            if (rand > 124)
                PackItem(new StarmetalOre(2)); 
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
		
		}
		
		public override int Meat{ get{ return 18; } }
		public override int Bones{ get{ return 12; } }
		public override int Hides{ get{ return 9; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }

		public override bool BardImmune{ get{ return !Core.SE; } }
		public override bool Unprovokable{ get{ return Core.SE; } }
		public override bool Uncalmable{ get{ return Core.SE; } }
		public override bool BleedImmune{ get{ return true; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public Quest4( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 471 )
				BaseSoundID = 0xE0;
		}
	}
}
