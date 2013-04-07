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
			SetInt( 35 );

			SetHits( 460 );

			SetDamage( 23, 27 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Poisoning, 160.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 45;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
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
