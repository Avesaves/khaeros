using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yew Treant corpse" )]
	public class YewTreant : Treant
	{
		[Constructable]
		public YewTreant()
		{
			Name = "a Yew Treant";
			
			Hue = 2413;

			SetHits( 370, 423 );

			SetDamage( 23, 35 );
			
			PackItem( new YewLog( Utility.RandomMinMax( 25, 30 ) ) );
		}

		public YewTreant( Serial serial ) : base( serial )
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
				YewTreefellow yewTreefellow = new YewTreefellow();

				yewTreefellow.Team = this.Team;
				yewTreefellow.FightMode = FightMode.Closest;

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

				yewTreefellow.MoveToWorld( loc, map );
				yewTreefellow.Combatant = target;
				yewTreefellow.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				
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
