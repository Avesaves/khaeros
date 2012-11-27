using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Misc
{
	public enum DFAlgorithm
	{
		Standard,
		PainSpike
	}

	public class WeightOverloading
	{
		public static void Initialize()
		{
			EventSink.Movement += new MovementEventHandler( EventSink_Movement );
		}

		private static DFAlgorithm m_DFA;

		public static DFAlgorithm DFA
		{
			get{ return m_DFA; }
			set{ m_DFA = value; }
		}

		public static void FatigueOnDamage( Mobile m, int damage )
		{
			/*double fatigue = 0.0;

			switch ( m_DFA )
			{
				case DFAlgorithm.Standard:
				{
					fatigue = (damage * (100.0 / m.Hits) * ((double)m.Stam / 100)) - 5.0;
					break;
				}
				case DFAlgorithm.PainSpike:
				{
					fatigue = (damage * ((100.0 / m.Hits) + ((50.0 + m.Stam) / 100) - 1.0)) - 5.0;
					break;
				}
			}

			if ( fatigue > 0 )
				m.Stam -= (int)fatigue;*/
		}

		public const int OverloadAllowance = 4; // We can be four stones overweight without getting fatigued

		public static int GetMaxWeight( Mobile m )
		{
			//return ((( Core.ML && m.Race == Race.Human) ? 100 : 40 ) + (int)(3.5 * m.Str));
			//Moved to core virtual method for use there

			return m.MaxWeight;
		}

		public static void EventSink_Movement( MovementEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !from.Alive )
				return;
			
			bool reforging = from is PlayerMobile && ((PlayerMobile)from).Reforging;
			bool staff = from.AccessLevel > AccessLevel.Player;

			if( !reforging && !staff )
			{
				if ( !from.Player )
				{
					// Else it won't work on monsters.
					Spells.ExoticWeaponry.DeathStrike.AddStep( from );
					return;
				}
	
				int maxWeight = GetMaxWeight( from ) + OverloadAllowance;
				int overWeight = (Mobile.BodyWeight + from.TotalWeight) - maxWeight;
	
				if ( overWeight > 0 )
				{
	                if( from.Mounted )
	                {
	                    Mobile mob = from.Mount as Mobile;
	                    mob.Stam -= GetStamLoss( from, overWeight, ( e.Direction & Direction.Running ) != 0 );
	                }
	
	                else
					    from.Stam -= GetStamLoss( from, overWeight, (e.Direction & Direction.Running) != 0 );
	
	                if( from.Mounted )
	                {
	                    Mobile mob = from.Mount as Mobile;
	
	                    if( mob.Stam == 0 )
	                    {
	                        from.SendMessage( "Your mount is too fatigued to move." );
	                        e.Blocked = true;
	                        return;
	                    }
	                }
	
					else if ( from.Stam == 0 )
					{
						from.SendLocalizedMessage( 500109 ); // You are too fatigued to move, because you are carrying too much weight!
						e.Blocked = true;
						return;
					}
				}
	
	            if( !from.Mounted )
	            {
	                if( ( ( from.Stam * 100 ) / Math.Max( from.StamMax, 1 ) ) < 10 )
	                    --from.Stam;
	
	                int drain = 0;
	
	                if( from is PlayerMobile )
	                {
	                    PlayerMobile player = from as PlayerMobile;
	                    bool running = ( ( e.Direction & Direction.Running ) != 0 );
	
	                    int penalty = 10 - ( from.Stam * 10 ) / Math.Max( from.StamMax, 1 );
	
	                    int draincheck = Utility.Random( 100 );
	
	                    if( penalty >= draincheck )
	                    {
	                        if( penalty > 6 )
	                            drain++;
	                        if( penalty > 12 )
	                            drain++;
	                        if( penalty > 18 )
	                            drain++;
	                        if( penalty > 24 )
	                            drain++;
	                        if ( penalty > 30 )
	                            drain++;
	                        if( !running )
	                            drain -= 2;
	
	                        drain = Math.Max( drain, 0 );
	
	                        from.Stam -= drain;
	                    }
	                }
	            }
	
	            if( from.Mounted )
	            {
	                Mobile mob = from.Mount as Mobile;
	
	                if( ( ( mob.Stam * 100 ) / Math.Max( mob.StamMax, 1 ) ) < 10 )
	                    --mob.Stam;
	
	                if( mob.Stam == 0 )
	                {
	                    from.SendMessage( "Your mount is too fatigued to move." );
	                    e.Blocked = true;
	                    return;
	                }
	            }
	
				else if ( from.Stam == 0 )
				{
					from.SendLocalizedMessage( 500110 ); // You are too fatigued to move.
					e.Blocked = true;
					return;
				}
			}

			if ( from is PlayerMobile && !from.Mounted )
			{
				int amt = ( from.Mounted ? 48 : 16 );
				PlayerMobile pm = (PlayerMobile)from;

				if ( (++pm.StepsTaken % amt) == 0 )
					--from.Stam;
			}

			Spells.ExoticWeaponry.DeathStrike.AddStep( from );
		}

		public static int GetStamLoss( Mobile from, int overWeight, bool running )
		{
			int loss = 5 + (overWeight / 25);

			if ( running )
				loss *= 2;

			return loss;
		}

		public static bool IsOverloaded( Mobile m )
		{
			if ( !m.Player || !m.Alive || m.AccessLevel > AccessLevel.Player || (m is PlayerMobile && ((PlayerMobile)m).Reforging) )
				return false;

			return ( (Mobile.BodyWeight + m.TotalWeight) > (GetMaxWeight( m ) + OverloadAllowance) );
		}
	}
}
