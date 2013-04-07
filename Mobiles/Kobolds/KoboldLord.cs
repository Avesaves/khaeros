using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Kobold Lord corpse" )]
	public class KoboldLord : BaseCreature, IMediumPredator, IKobold
	{
		[Constructable]
		public KoboldLord() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Kobold Lord";
			Body = 240;
			BaseSoundID = 0x452;

			SetStr( 86, 130 );
			SetDex( 41, 55 );
			SetInt( 35 );

			SetHits( 126, 140 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 25, 50 );

			SetSkill( SkillName.Invocation, 92.6, 107.5 );
			SetSkill( SkillName.Magery, 105.1, 115.0 );
			SetSkill( SkillName.Meditation, 100.1, 110.0 );
			SetSkill( SkillName.MagicResist, 15.0 );
			SetSkill( SkillName.Tactics, 85.1, 95.0 );
			SetSkill( SkillName.UnarmedFighting, 87.6, 97.5 );

			Fame = 3000;
			Karma = -3000;
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Fish; } }
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Poor );
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		public KoboldLord( Serial serial ) : base( serial )
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
		
		public override int GetIdleSound()
		{
			return 0x42A;
		}

		public override int GetAttackSound()
		{
			return 0x435;
		}

		public override int GetHurtSound()
		{
			return 0x436;
		}

		public override int GetDeathSound()
		{
			return 0x43A;
		}
	}
}
