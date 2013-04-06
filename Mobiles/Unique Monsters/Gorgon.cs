using System;
using System.Collections;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	public class Gorgon : BaseCreature, ILargePredator, IHasReach, IEnraged
	{
		[Constructable]
		public Gorgon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Gorgon";
			Body = 138;
			BaseSoundID = 0x4B0;
			Female = true;

			SetStr( 202, 300 );
			SetDex( 102, 110 );
			SetInt( 35 );
			SetMana( 401, 450 );

			SetHits( 400 );
			SetStam( 103, 250 );

			SetDamage( 19, 25 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Invocation, 95.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 105.0 );
			SetSkill( SkillName.Meditation, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 90.1, 105.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 105.0 );
			SetSkill( SkillName.Macing, 90.1, 105.0 );
			
			this.RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 30;
            PackItem( new RewardToken( 2 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GorgonsHead() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public override bool Unprovokable{ get{ return true; } }
		public override int Bones{ get{ return 20; } }
		public override int Meat{ get{ return 26; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			if( Utility.Random( 100 ) > 70 && !defender.Paralyzed && this.CanUseSpecial )
			{
				this.Emote( "*tries to paralyze " + defender.Name + " with its gaze*" );
				this.CanUseSpecial = false;
				
				if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
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
                : base( TimeSpan.FromSeconds( 6 ) )
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

		public Gorgon( Serial serial ) : base( serial )
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
