using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Gobworts corpse" )]
	public class Gobwort : BaseCreature, ILargePredator
	{
		[Constructable]
		public Gobwort() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Gobwort";
			Body = 47;
			BaseSoundID = 442;

			SetStr( 66, 115 );
			SetDex( 16, 25 );
			SetInt( 11, 25 );

			SetHits( 140, 169 );
			SetStam( 0 );

			SetDamage( 9, 12 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 25.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 60.0 );

			Fame = 7000;
			Karma = -7000;

			this.RangeFight = 3;

			PackItem( new Log( Utility.RandomMinMax( 20, 30 ) ) );
		}
		
		public override bool DisallowAllMoves{ get{ return true; } }

		public Gobwort( Serial serial ) : base( serial )
		{
		}
		
		public void SpawnWortlings( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newWortlings = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newWortlings; ++i )
			{
				Wortling wortling = new Wortling();

				wortling.Team = this.Team;
				wortling.FightMode = FightMode.Closest;

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

				wortling.MoveToWorld( loc, map );
				wortling.Combatant = target;
				wortling.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				wortling.VanishEmote = "*collapses into a pile of wortfruit*";
				PlaySound( 898 );
			}
			
			this.Emote( "*some of its fruits fall from its branches and start fighting in its defense.*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.3 >= Utility.RandomDouble() )
				SpawnWortlings( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.3 >= Utility.RandomDouble() )
				SpawnWortlings( attacker );
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
