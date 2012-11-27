using System;
using Server;
using Server.Items;

namespace Server.Mobiles 
{ 
	[CorpseName( "a Celestial Warrior corpse" )] 
	public class CelestialWarrior : BaseCreature, ICelestial, IHasReach
	{ 
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}
		
		[Constructable]
		public CelestialWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = "A Celestial Warrior";
			Body = 123;

			SetStr( 186, 195 );
			SetDex( 77, 95 );
			SetInt( 51, 70 );

			SetHits( 252, 271 );

			SetDamage( 23, 25 );

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

			Fame = 10000;
			Karma = 10000;

			VirtualArmor = 60;
			
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
		
		public void Spawnlanternarchons( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newlanternarchons = Utility.RandomMinMax( 1, 2 );

			for ( int i = 0; i < newlanternarchons; ++i )
			{
				LanternArchon lanternarchon = new LanternArchon();

				lanternarchon.Team = this.Team;
				lanternarchon.FightMode = FightMode.Closest;

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

				lanternarchon.MoveToWorld( loc, map );
				lanternarchon.Combatant = target;
				this.PlaySound( 552 );
			}
			
			this.Emote( "*summons lantern archons to defend {0} in combat*", this.Female == true ? "her" : "him" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.2 >= Utility.RandomDouble() )
				Spawnlanternarchons( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 >= Utility.RandomDouble() )
				Spawnlanternarchons( attacker );
		}

		public CelestialWarrior( Serial serial ) : base( serial ) 
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
