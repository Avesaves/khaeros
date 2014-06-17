using System;
using Server.Items;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an ambusher spider corpse" )]
	public class AmbusherSpider : BaseCreature, IMediumPredator, IJungleCreature, ISpider
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public AmbusherSpider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ambusher spider";
			Body = 20;
			BaseSoundID = 0x388;
			Hue = 2801;
			RangePerception = 6;
			IsSneaky = true;
			
			SetStr( 76, 100 );
			SetDex( 126, 145 );
			SetInt( 35 );

			SetHits( 46, 60 );
			SetMana( 0 );

			SetDamage( 6, 16 );

			SetDamageType( ResistanceType.Piercing, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.Hiding, 100.0 );
			SetSkill( SkillName.Stealth, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 65.0 );

			Fame = 775;
			Karma = -775;

			VirtualArmor = 28; 

			PackItem( new SpidersSilk( 5 ) );
		}


		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, 100),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 20)
		}; } }
		public override int PoisonDuration { get { return 360; } }
		public override int PoisonActingSpeed { get { return 2; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new AmbusherSpiderVenom() );
            bpc.DropItem( new SpiderHeart());
		}
		
		public override int Meat{ get{ return 4; } }
		
		public AmbusherSpider( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 387 )
				BaseSoundID = 0x388;
		}
	}
}
