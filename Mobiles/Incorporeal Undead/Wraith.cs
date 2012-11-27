using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Wraith corpse" )]
	public class Wraith : BaseCreature, IUndead, IEnraged
	{
		private int m_ManaLeeched;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ManaLeeched
		{
			get{ return m_ManaLeeched; }
			set{ m_ManaLeeched = value; }
		}
		
		[Constructable]
		public Wraith() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a Wraith";
			BodyValue = 250;
			BaseSoundID = 442;
			Hue = 12345678;

			SetStr( 146, 170 );
			SetDex( 41, 50 );
			SetInt( 35 );

			SetHits( 628, 642 );

			SetDamage( 4, 5 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 45, 65 );
			SetResistance( ResistanceType.Piercing, 40, 60 );
			SetResistance( ResistanceType.Slashing, 40, 60 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 85.1, 90.0 );
			SetSkill( SkillName.UnarmedFighting, 75.1, 90.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;
			
			IsSneaky = true;
            RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FrontalAOE;
			
			PackItem( new Necroplasm( 11 ) );

            if( Utility.RandomBool() )
                PackItem( new RewardToken( 1 ) );
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			
			if( this.Hue == 12345678 )
			{
				defender.Mana -= Utility.Random( 20, 10 );
				this.ManaLeeched += 20;
				base.OnGaveMeleeAttack( defender );
				
				if( this.ManaLeeched >= 100 )
				{
					defender.Emote( "*shudders as the wraith becomes corporeal*" );
					this.Hue = 2989;
					this.SetDamage( 25, 30 );
				}
			}
			
			if( this.Hue != 12345678 )
			{
				this.ManaLeeched -= 2;
				
				if( this.ManaLeeched <= 0 )
				{
					defender.Emote( "*notices that the wraith became incorporeal again*" );
					this.Hue = 12345678;
					this.SetDamage( 4, 5 );
				}
			}
		}

		public Wraith( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			
			writer.Write( (int) m_ManaLeeched );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			m_ManaLeeched = reader.ReadInt();
		}
	}
}
