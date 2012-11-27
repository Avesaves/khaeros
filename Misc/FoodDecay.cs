// FoodDecay.cs - Modified by Alari ( alarihyena@gmail.com )
// Includes hunger/thirst messages, opens the hunger/thirst gump when low, and
// lowers player HP if they become extremely dehydrated or begin to starve.
//
// There are two easy customizations you can make to this file:
//
//
// this code:  (don't actually modify this one, this is an example. Look further down.)
//
//    public static bool KeepAlive { get { return true; } }
//
// determines whether or not extreme hunger or thirst can kill the player.
//
// If true, the player's hit points will not be decreased if that would cause 
// the player to go below 0 HP.
//
// If false, the player CAN die from starvation/dehydration.
//
//
// this code: (second verse, same as the first. Change the one further down.)
//
//    public static bool StaffImmune { get { return true; } }
//
// determines whether staff hunger and thirst will be allowed to decay below 10.
// (out of 20)
//
// If true, staff hunger/thirst will stop decaying below 10, and if lower when
// the timer fires it will be boosted back up to 10.
//
// If false, staff hunger/thirst can and will decay below 10 unless they eat and
// drink.
//
// I allow it to decay to 10 so that counselors, etc can still eat and drink,
// but won't be inconvenienced by it should they not wish to do so.


using System;
using Server.Network;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{

		// keep player from dying from hunger?
		public static bool KeepAlive { get { return true; } }

		// if true, staff hunger/thirst will not decay below 10.
		public static bool StaffImmune { get { return true; } }



// deprecated
		// keep player from dying from hunger?
//		public static bool KeepAlive = true;

		// if true, staff hunger/thirst will not decay below 10.
//		public static bool StaffImmune = true;


		
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}
		
		public FoodDecayTimer() : base( TimeSpan.FromMinutes( 10 ), TimeSpan.FromMinutes( 10 ) )
		{
			Priority = TimerPriority.OneMinute;
		}
		
		protected override void OnTick()
		{
			FoodDecay();
		}
		
		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				HungerDecay( state.Mobile );
				ThirstDecay( state.Mobile );
			}
		}
		
		public static void HungerDecay( Mobile m )
		{
			int chance = Utility.Random( 1, 100 );
			
			if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.IsVampire )
				{
					pm.Hunger = 0;
					pm.Thirst = 0;
					return;
				}
			}
			
			if ( m != null && m.Hunger >= 1 && chance > 60 )
				m.Hunger -= 1;
			
			// new additions
			
			bool keepAlive = KeepAlive;	// keep player from dying from this
			
			if ( !( m is PlayerMobile ) )
				return;

			// bool staffImmune = StaffImmune;
			if ( StaffImmune && ( m.AccessLevel > AccessLevel.Player ) && ( m.Hunger < 10 ) )
				m.Hunger = 10;

			CalculatePenalty( m );

			if ( m.Hunger < 6 )
			{
				try
				{
					m.CloseGump( typeof( gumpfaim ) );
				    m.SendGump( new Server.Gumps.gumpfaim ( m ) ); // popup hunger gump.
				}
				catch
				{}
			}

			switch ( m.Hunger )
			{
				case 5: m.SendMessage( "You are a little hungry." ); break;
				case 4: m.SendMessage( "You are hungry." ); break;
				case 3: m.SendMessage( 33, "You are really hungry." ); break;
				case 2: m.SendMessage( 33, "You are very hungry!" ); break;

				case 1:
				{
					m.SendMessage( "You need food now!" );

					if ( m.Hits < ( m.HitsMax / 20 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 20 );
					m.SendMessage(33, "" + -( m.HitsMax / 20 ) + " hp, due to hunger!" );
					break;
				}

				case 0:
				{
					m.SendMessage( "You are starving!" );
					if ( m.Hits < ( m.HitsMax / 10 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 10 );
					m.SendMessage(33, "" + -( m.HitsMax / 10 ) + " hp, due to hunger!" );
					break;
				}
			}
		}
		
		public static void CalculatePenalty( Mobile m )
		{
            if( m is PlayerMobile && ( (PlayerMobile)m ).IsVampire )
                return;

			double percentagePenalty;
			int strOffset, dexOffset;
			if ( m.Thirst < 5 )
			{
				percentagePenalty = ((5-m.Thirst)*2)*0.01;
				strOffset = (int)(m.RawStr*percentagePenalty);
				dexOffset = (int)(m.RawDex*percentagePenalty);
				m.AddStatMod( new StatMod( StatType.Str, "[Dehydration] STR", -strOffset, TimeSpan.Zero ) );
				m.AddStatMod( new StatMod( StatType.Dex, "[Dehydration] DEX", -dexOffset, TimeSpan.Zero ) );
			}
			else
			{
				m.RemoveStatMod( "[Dehydration] STR" );
				m.RemoveStatMod( "[Dehydration] DEX" );
			}
			
			if ( m.Hunger < 5 )
			{
				percentagePenalty = ((5-m.Hunger)*2)*0.01;
				strOffset = (int)(m.RawStr*percentagePenalty);
				dexOffset = (int)(m.RawDex*percentagePenalty);
				m.AddStatMod( new StatMod( StatType.Str, "[Starvation] STR", -strOffset, TimeSpan.Zero ) );
				m.AddStatMod( new StatMod( StatType.Dex, "[Starvation] DEX", -dexOffset, TimeSpan.Zero ) );
			}
			else
			{
				m.RemoveStatMod( "[Starvation] STR" );
				m.RemoveStatMod( "[Starvation] DEX" );
			}
		}
		
		public static void ThirstDecay( Mobile m )
		{
			int chance = Utility.Random( 1, 100 );
			
			if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.IsVampire )
				{
					pm.Hunger = 0;
					pm.Thirst = 0;
					return;
				}
			}
			
			if ( m != null && m.Thirst >= 1 && chance > 60 )
				m.Thirst -= 1;
			
			// new additions:
				bool keepAlive = KeepAlive;	// keep player from dying from this
				
				if ( !( m is PlayerMobile ) )
					return;

			// bool staffImmune = StaffImmune;
			if ( StaffImmune && ( m.AccessLevel > AccessLevel.Player ) && ( m.Thirst < 10 ) )
				m.Thirst = 10;
			
			CalculatePenalty( m );

			if ( m.Thirst < 6 )
			{
				try
				{
					m.CloseGump( typeof( gumpfaim ) );
				        m.SendGump( new Server.Gumps.gumpfaim ( m ) );
				}
				catch
				{}
			}


			switch ( m.Thirst )
			{
				case 5: m.SendMessage( "You are a little thirsty." ); break;
				case 4: m.SendMessage( "You are thirsty." ); break;
				case 3: m.SendMessage( 33, "You are really thirsty." ); break;
				case 2: m.SendMessage( 33, "You are very thirsty!" ); break;

// you can go for a while without food but cannot survive long without water! (RL ;)
				case 1:
				{
					m.SendMessage( "You need liquid now!" );

					if ( m.Hits < ( m.HitsMax / 10 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 10 );
					m.SendMessage(33, "" + -( m.HitsMax / 10 ) + " hp, due to thirst!" );
					break;
				}

				case 0:
				{
					m.SendMessage( "You are dehydrated!" );

					if ( m.Hits < ( m.HitsMax / 5 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 5 );
					m.SendMessage(33, "" + -( m.HitsMax / 5 ) + " hp, due to thirst!" );
					break;
				}
			}
		}
	}
}
