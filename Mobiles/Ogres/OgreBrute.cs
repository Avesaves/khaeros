using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ogre brute corpse" )]
	public class OgreBrute : BaseCreature, ILargePredator, IMhordulFavoredEnemy, IOgre
	{
		public override int Height{ get{ return 20; } }
		
		[Constructable]
		public OgreBrute() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ogre brute";
			Body = 1;
			BaseSoundID = 427;

			SetStr( 190 );
			SetDex( 45 );
			SetInt( 31, 40 );

			SetHits( 450, 500 );

			SetDamage( 23, 27 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 25, 35 );
			SetResistance( ResistanceType.Fire, 20, 25 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 45;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Poor );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 12; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public OgreBrute( Serial serial ) : base( serial )
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
