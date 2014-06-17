using System;
using Server;
using Server.Items;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an assassin spider corpse" )]
	public class AssassinSpider : BaseCreature, IMediumPredator, IDesertCreature, ISpider
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public AssassinSpider () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an assassin spider";
			Body = 11;
			BaseSoundID = 1170;
			Hue = 2601;

			SetStr( 96, 120 );
			SetDex( 26, 35 );
			SetInt( 16, 20 );

			SetHits( 118, 132 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.Invocation, 65.1, 80.0 );
			SetSkill( SkillName.Magery, 65.1, 80.0 );
			SetSkill( SkillName.Meditation, 65.1, 80.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 55.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 75.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 26;

			PackItem( new SpidersSilk( 8 ) );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 60),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 60)
		}; } }
		public override int PoisonDuration { get { return 180; } }
		public override int PoisonActingSpeed { get { return 3; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new AssassinSpiderVenom() );
            bpc.DropItem(new SpiderHeart());
		}
		
		public override int Meat{ get{ return 4; } }

		public AssassinSpider( Serial serial ) : base( serial )
		{
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 80 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( this.InRange( m, 5 ) && this.CanSee( m ) )
					{
						this.MovingEffect( m, 4306, 12, 1, false, false );
						this.PlaySound( 1173 );
						SpiderWeb spdw = new SpiderWeb();
		                spdw.MoveToWorld( m.Location );
		                spdw.Map = m.Map;
		                this.CanUseSpecial = false;
		                
		                if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
		                
		                spdw.OnTrigger( m );
					}
				}
			}
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 1170;
		}
	}
}
