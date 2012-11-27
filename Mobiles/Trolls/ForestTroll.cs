using System;
using Server.Items;

namespace Server.Mobiles
{
	public interface IRegenerativeCreature
	{
	}
	
	[CorpseName( "a Forest Troll corpse" )]
	public class ForestTroll : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged, ITroll
	{
		[Constructable]
		public ForestTroll() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Forest Troll";
			Body = 241;

			SetStr( 101, 110 );
			SetDex( 81, 90 );
			SetInt( 11, 25 );

			SetHits( 161, 170 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 45, 70 );
			SetResistance( ResistanceType.Energy, 45, 65 );

			SetSkill( SkillName.Invocation, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 96.1, 106.0 );
			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 86.1, 101.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );

			Fame = 4000;
			Karma = -4000;
			
			VirtualArmor = 30;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new TrollBlood() );
		}

		public override bool HasFur{ get{ return true; } }
		public override int Meat{ get{ return 12; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
			
		public override int GetAngerSound()
		{
			return 0x4E3;
		}

		public override int GetIdleSound()
		{
			return 0x4E2;
		}

		public override int GetAttackSound()
		{
			return 0x4E1;
		}

		public override int GetHurtSound()
		{
			return 0x4E4;
		}

		public override int GetDeathSound()
		{
			return 0x4E0;
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		public ForestTroll( Serial serial ) : base( serial )
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
