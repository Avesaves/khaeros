using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a giant wolf spider corpse" )]
	public class GiantWolfSpider : BaseCreature, IMediumPredator, IForestCreature, ISpider
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public GiantWolfSpider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant wolf spider";
			Body = 28;
			BaseSoundID = 0x388;
			Hue = 1886;

			SetStr( 76, 100 );
			SetDex( 36, 45 );
			SetInt( 35 );

			SetHits( 46, 60 );
			SetMana( 0 );

			SetDamage( 5, 13 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 65.0 );

			Fame = 1600;
			Karma = -600;

			VirtualArmor = 16;

			PackItem( new SpidersSilk( 5 ) );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 60)
		}; } }
		public override int PoisonDuration { get { return 90; } }
		public override int PoisonActingSpeed { get { return 3; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new WolfSpiderVenom() );
		}
		
		public override int Meat{ get{ return 4; } }

		public GiantWolfSpider( Serial serial ) : base( serial )
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
