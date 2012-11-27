using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Redwood Treant corpse" )]
	public class RedwoodTreant : Treant
	{
		[Constructable]
		public RedwoodTreant()
		{
			Name = "a Redwood Treant";
			
			Hue = 1194;

			SetHits( 390, 443 );

			SetDamage( 25, 37 );
			
			PackItem( new RedwoodLog( Utility.RandomMinMax( 25, 30 ) ) );
		}

		public RedwoodTreant( Serial serial ) : base( serial )
		{
		}
		
		public void SpawnTreefellows( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newTreefellows = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newTreefellows; ++i )
			{
				RedwoodTreefellow redwoodTreefellow = new RedwoodTreefellow();

				redwoodTreefellow.Team = this.Team;
				redwoodTreefellow.FightMode = FightMode.Closest;

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

				redwoodTreefellow.MoveToWorld( loc, map );
				redwoodTreefellow.Combatant = target;
				redwoodTreefellow.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				
				PlaySound( 545 );
			}
			
			this.Emote( "*with a deep roar, he convokes his treefellow brethren to aid him in battle*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.2 >= Utility.RandomDouble() )
				SpawnTreefellows( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 >= Utility.RandomDouble() && this.CanUseSpecial )
			{
				this.CanUseSpecial = false;
				SpawnTreefellows( attacker );
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
		}
	}
}
