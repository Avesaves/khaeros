using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a unicorn corpse" )]
	
	public class MaleUnicorn : BaseCreature, IMagicalForestCreature, IEnraged
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public MaleUnicorn() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a unicorn";
			Body = 257;
			BaseSoundID = 0xA8;

			SetStr( 181, 205 );
			SetDex( 91, 115 );
			SetInt( 126, 150 );

			SetHits( 269, 283 );
			SetStam( 136, 145 );

			SetDamage( 10, 14 );

			SetDamageType( ResistanceType.Blunt, 70 );
			SetDamageType( ResistanceType.Piercing, 30 );

			SetResistance( ResistanceType.Blunt, 40, 52 );
			SetResistance( ResistanceType.Piercing, 40, 52 );
			SetResistance( ResistanceType.Slashing, 40, 52 );
			SetResistance( ResistanceType.Poison, 70, 82 );
			SetResistance( ResistanceType.Energy, 70, 82 );

			SetSkill( SkillName.Invocation, 155.1, 200.0 );
			SetSkill( SkillName.Magery, 155.1, 200.0 );
			SetSkill( SkillName.MagicResist, 75.1, 80.0 );
			SetSkill( SkillName.Tactics, 155.1, 200.0 );
			SetSkill( SkillName.UnarmedFighting, 155.1, 200.0 );

			Fame = 11500;
			Karma = 11500;

			VirtualArmor = 35;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new UnicornHead() );
			bpc.DropItem( new UnicornHorn() );
		}
		
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 16; } }
		public override int Bones{ get{ return 16; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		
		public MaleUnicorn( Serial serial ) : base( serial )
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
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
