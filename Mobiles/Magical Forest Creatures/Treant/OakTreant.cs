using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Oak Treant corpse" )]
	public class OakTreant : Treant
	{
		[Constructable]
		public OakTreant()
		{
			Name = "an Oak Treant";

			SetHits( 350, 403 );

			SetDamage( 21, 33 );
			
			PackItem( new Log( Utility.RandomMinMax( 25, 30 ) ) );
		}

		public OakTreant( Serial serial ) : base( serial )
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
				OakTreefellow oakTreefellow = new OakTreefellow();

				oakTreefellow.Team = this.Team;
				oakTreefellow.FightMode = FightMode.Closest;

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

				oakTreefellow.MoveToWorld( loc, map );
				oakTreefellow.Combatant = target;
				oakTreefellow.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				
				PlaySound( 545 );
			}
			
			this.Emote( "*with a deep roar, he convokes his treefellow brethren to aid him in battle*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.2 >= Utility.RandomDouble() && this.CanUseSpecial )
			{
				this.CanUseSpecial = false;
				SpawnTreefellows( caster );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 >= Utility.RandomDouble() )
				SpawnTreefellows( attacker );
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
