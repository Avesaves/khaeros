using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a funnel web spider spider corpse" )] // stupid corpse name
	public class FunnelWebSpider : BaseCreature, IMediumPredator, ICaveCreature, ISpider
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public FunnelWebSpider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a funnel web spider";
			Body =  0x9D;
			BaseSoundID = 0x388; // TODO: validate
			
			SetStr( 76, 100 );
			SetDex( 26, 35 );
			SetInt( 35 );

			SetHits( 146, 160 );

			SetDamage( 11, 14 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 30 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 30.3, 75.0 );
			SetSkill( SkillName.Poisoning, 60.1, 80.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 65.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 85.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 24;

			PackItem( new SpidersSilk( 5 ) );
		}
		
		public override int Meat{ get{ return 4; } }

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 100),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 20)
		}; } }
		public override int PoisonDuration { get { return 360; } }
		public override int PoisonActingSpeed { get { return 2; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new FunnelWebSpiderVenom() );
            bpc.DropItem(new SpiderHeart()); 
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 40 && this.CanUseSpecial )
			{
				this.PlaySound( 1173 );
				SpiderWeb spdw = new SpiderWeb();
		        spdw.MoveToWorld( this.Location );
		        spdw.Map = this.Map;
		        this.CanUseSpecial = false;
			}
		}

		public FunnelWebSpider( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			if( version < 1 )
				Fame += 1000;
		}
	}
}
