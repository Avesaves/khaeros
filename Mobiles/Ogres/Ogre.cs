using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an Ogre corpse" )]
	public class Ogre : BaseCreature, ILargePredator, IMhordulFavoredEnemy, IOgre
	{
		public override int Height{ get{ return 20; } }
		[Constructable]
		public Ogre () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an Ogre";
			Body = Utility.RandomList( 53, 54 );
			BaseSoundID = 461;

			SetStr( 190, 220 );
			SetDex( 45, 65 );
			SetInt( 21, 30 );

			SetHits( 350, 450 );

			SetDamage( 23, 27 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 30, 40 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 15, 20 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 20 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Poisoning, 160.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 75.0, 85.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 40;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
		}

		public Ogre( Serial serial ) : base( serial )
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
