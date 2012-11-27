using System;
using Server;
using Server.Items;

namespace Server.Mobiles 
{ 
	[CorpseName( "a Celestial Lord corpse" )] 
	public class CelestialLord : BaseCreature, ICelestial, IHasReach
	{ 
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}
		
		[Constructable]
		public CelestialLord() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = "A Celestial Lord";
			Body = 175;

			SetStr( 286, 295 );
			SetDex( 77, 95 );
			SetInt( 51, 70 );

			SetHits( 552, 571 );

			SetDamage( 33, 35 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 50 );
			SetResistance( ResistanceType.Piercing, 45, 50 );
			SetResistance( ResistanceType.Slashing, 45, 50 );
			SetResistance( ResistanceType.Fire, 45, 50 );
			SetResistance( ResistanceType.Cold, 45, 50 );
			SetResistance( ResistanceType.Poison, 45, 50 );
			SetResistance( ResistanceType.Energy, 45, 50 );

			SetSkill( SkillName.Anatomy, 50.1, 75.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 25000;
			Karma = 25000;

			VirtualArmor = 80;
			
			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
		}

		public override int Feathers{ get{ return 100; } }

		public override int GetAngerSound()
		{
			return 0x2F8;
		}

		public override int GetIdleSound()
		{
			return 0x2F8;
		}

		public override int GetAttackSound()
		{
			return Utility.Random( 0x2F5, 2 );
		}

		public override int GetHurtSound()
		{
			return 0x2F9;
		}

		public override int GetDeathSound()
		{
			return 0x2F7;
		}
		
		public void Spawncelestials( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newcelestials = Utility.RandomMinMax( 1, 2 );

			for ( int i = 0; i < newcelestials; ++i )
			{
				Celestial celestial = new Celestial();

				celestial.Team = this.Team;
				celestial.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				celestial.MoveToWorld( loc, map );
				celestial.Combatant = target;
				celestial.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				this.PlaySound( 552 );
			}
			
			this.Emote( "*summons celestials to defend {0} in combat*", this.Female == true ? "her" : "him" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.2 >= Utility.RandomDouble() )
				Spawncelestials( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 >= Utility.RandomDouble() )
				Spawncelestials( attacker );
		}

		public CelestialLord( Serial serial ) : base( serial ) 
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
