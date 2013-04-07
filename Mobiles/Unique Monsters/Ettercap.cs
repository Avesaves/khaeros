using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Ettercap corpse" )]
	public class Ettercap : BaseCreature, ILargePredator, IEnraged, ISpider
	{
		[Constructable]
		public Ettercap() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an Ettercap";
			Body = 314;
			BaseSoundID = 357;

			SetStr( 251, 275 );
			SetDex( 41, 55 );
			SetInt( 35 );

			SetHits( 161, 175 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 50, 60 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 15.0 );
			SetSkill( SkillName.Tactics, 75.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 90.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 34;
			
			PackItem( new EttercapSilk( 30 ) );
		}
		
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.HealthDecrease, 100),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
		}; } }
		public override int PoisonDuration { get { return 360; } }
		public override int PoisonActingSpeed { get { return 2; } }

		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 80 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );

                    if (this != null || m != null)
                    {
                        if (this.InRange(m, 5) && this.CanSee(m))
                        {
                            this.MovingEffect(m, 4306, 12, 1, false, false);
                            this.PlaySound(1173);
                            SpiderWeb spdw = new SpiderWeb();
                            spdw.MoveToWorld(m.Location);
                            spdw.Map = m.Map;
                            this.CanUseSpecial = false;

                            if (this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded())
                                return;

                            spdw.OnTrigger(m);

                        }
                    }
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public Ettercap( Serial serial ) : base( serial )
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
