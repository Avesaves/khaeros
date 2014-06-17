using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a Dire Spider corpse" )]
	public class DireSpider : BaseCreature, ILargePredator, IEnraged, ISpider
	{
		[Constructable]
		public DireSpider() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 173;
			Name = "a Dire Spider";

			BaseSoundID = 0x183;

			SetStr( 105, 150 );
			SetDex( 122, 130 );
			SetInt( 22, 30 );

			SetHits( 1200 );
			SetStam( 105, 200 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 50 );
			SetResistance( ResistanceType.Piercing, 50, 60 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 32500;
			Karma = -32500;

			VirtualArmor = 50;
            RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;
			
			PackItem( new SpidersSilk( 50 ) );
            PackItem( new RewardToken( 3 ) );
		}

		public override int Meat{ get{ return 16; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 60)
		}; } }
		public override int PoisonDuration { get { return 90; } }
		public override int PoisonActingSpeed { get { return 3; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new DireSpiderVenom() );
            bpc.DropItem(new SpiderHeart()); 
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 80 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( this.InRange( m, 8 ) && this.CanSee( m ) )
					{
						this.MovingEffect( m, 4306, 12, 1, false, false );
						this.PlaySound( 1173 );
						SpiderWeb spdw = new SpiderWeb();
		                spdw.MoveToWorld( m.Location );
		                spdw.Map = m.Map;
		                spdw.OnTrigger( m );
		                this.CanUseSpecial = false;
					}
				}
			}
		}

		public DireSpider( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
