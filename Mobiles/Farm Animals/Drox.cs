using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a drox corpse" )]
	public class Drox : BaseCreature, ILargePrey, IEnraged
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Drox() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a drox";
			Body = 248;

			SetStr( 146, 175 );
			SetDex( 111, 150 );
			SetInt( 35 );

			SetHits( 111, 130 );
			SetMana( 0 );

			SetDamage( 5, 8 );

			SetDamageType( ResistanceType.Blunt, 50 );
			SetDamageType( ResistanceType.Piercing, 50 );

			SetResistance( ResistanceType.Blunt, 50, 70 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 30, 50 );
			SetResistance( ResistanceType.Slashing, 35, 40 );
			SetResistance( ResistanceType.Piercing, 35, 40 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 70.6, 83.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 57.5 );

			Fame = 1200;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 68.7;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BeefHock( 4 ) );
			bpc.DropItem( new DroxTesticle() );
		}

		public override int GetAngerSound()
		{
			return 0x4F8;
		}

		public override int GetIdleSound()
		{
			return 0x4F7;
		}

		public override int GetAttackSound()
		{
			return 0x4F6;
		}

		public override int GetHurtSound()
		{
			return 0x4F9;
		}

		public override int GetDeathSound()
		{
			return 0x4F5;
		}

		public override int Meat{ get{ return 30; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay; } }

		public Drox( Serial serial ) : base( serial )
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
