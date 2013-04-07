using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Gazer corpse" )]
	public class Gazer : BaseCreature, IMediumPredator, IEnraged, IBeholder
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Gazer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Gazer";
			Body = 103;
			BaseSoundID = 377;

			SetStr( 296, 325 );
			SetDex( 46, 55 );
			SetInt( 291, 385 );

			SetHits( 178, 195 );

			SetDamage( 12, 20 );

			SetDamageType( ResistanceType.Slashing, 50 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 45, 55 );
			SetResistance( ResistanceType.Slashing, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 50;
            PackItem( new RewardToken( 2 ) );
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			if( Utility.Random( 100 ) > 80 && !defender.Paralyzed && this.CanUseSpecial )
			{
				this.Emote( "*tries to paralyze " + defender.Name + " with its gaze*" );
				this.CanUseSpecial = false;
				
				if( defender is PlayerMobile && ((PlayerMobile)defender).Evaded() )
			    	return;
				
				if( defender is PlayerMobile )
	            {
	            	( (PlayerMobile)defender ).m_PetrifiedTimer = new StoneTimer( defender );
	            	( (PlayerMobile)defender ).m_PetrifiedTimer.Start();
	            }

				if( defender is BaseCreature )
	            {
	            	( (BaseCreature)defender ).m_PetrifiedTimer = new StoneTimer( defender );
	            	( (BaseCreature)defender ).m_PetrifiedTimer.Start();
	            }
			}
			
			base.OnGaveMeleeAttack( defender );
		}
		
		public class StoneTimer : Timer
        {
            private Mobile m_m;

            public StoneTimer( Mobile m )
                : base( TimeSpan.FromSeconds( 10 ) )
            {
                m_m = m;
                m.Emote( "*got temporarily petrified*" );
            }

            protected override void OnTick()
            {
                m_m.Emote( "*is no longer petrified*" );
                
                if( m_m is PlayerMobile )
            		( (PlayerMobile)m_m ).m_PetrifiedTimer = null;
                
				else if( m_m is BaseCreature )
            		( (BaseCreature)m_m ).m_PetrifiedTimer = null;
            }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public Gazer( Serial serial ) : base( serial )
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
