using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Kobold Warrior corpse" )]
	public class KoboldWarrior : BaseCreature, IMediumPredator, IEnraged, IKobold
	{
		[Constructable]
		public KoboldWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Kobold Warrior";
			Body = 245;
			BaseSoundID = 0x452;

			SetStr( 86, 90 );
			SetDex( 51, 65 );
			SetInt( 35 );

			SetHits( 66, 70 );
			SetMana( 17, 31 );

			SetDamage( 4, 8 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 25, 50 );

			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 15.0 );
			SetSkill( SkillName.Tactics, 95.1, 105.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 107.5 );

			Fame = 1600;	
			Karma = -1600;
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Fish; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		public KoboldWarrior( Serial serial ) : base( serial )
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
