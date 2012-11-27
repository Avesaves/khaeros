using System;
using System.Collections;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server
{
	public class PlantSearch
	{
		public static void OnUse( Mobile m )
		{
			m.SendMessage( "What do you wish to search for?" );

			m.CloseGump( typeof( TrackWhatGump ) );
			TrackWhatGump.DisplayTo( m );
		}

		public class TrackingInfo
		{
			public Mobile m_Tracker;
			public Item m_Target;
			public Point2D m_Location;
			public Map m_Map;

			public TrackingInfo( Mobile tracker, Item target )
			{
				m_Tracker = tracker;
				m_Target = target;
				m_Location = new Point2D( target.X, target.Y );
				m_Map = target.Map;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public static void AddInfo( Mobile tracker, Item target )
		{
			TrackingInfo info = new TrackingInfo( tracker, target );
			m_Table[tracker] = info;
		}

		public static void ClearTrackingInfo( Mobile tracker )
		{
			m_Table.Remove( tracker );
		}
	}

	public class TrackWhatGump : Gump
	{
		private Mobile m_From;
		private int m_Range;

		private class InternalSorter : IComparer
		{
			private Mobile m_From;

			public InternalSorter( Mobile from )
			{
				m_From = from;
			}

			public int Compare( object x, object y )
			{
				if ( x == null && y == null )
					return 0;
				else if ( x == null )
					return -1;
				else if ( y == null )
					return 1;

				Item a = x as Item;
				Item b = y as Item;

				if ( a == null || b == null )
					throw new ArgumentException();

				return m_From.GetDistanceToSqrt( a ).CompareTo( m_From.GetDistanceToSqrt( b ) );
			}
		}

		public static void DisplayTo( Mobile from )
		{
			Map map = from.Map;

			if ( map == null )
				return;

			int range = (int)(from.Skills[SkillName.HerbalLore].Base); // add feat levels here

			ArrayList list = new ArrayList();

			foreach ( Item m in from.GetItemsInRange( range ) )
			{
				if ( m is BasePlant )
					list.Add( m );
			}

			if ( list.Count > 0 )
			{
				list.Sort( new InternalSorter( from ) );

				from.SendGump( new TrackWhatGump( from, list, range ) );
				from.SendLocalizedMessage( 1018093 ); // Select the one you would like to track.
			}
			else
				from.SendMessage( "There are no plants in this area." );
		}

		private ArrayList m_List;

		private TrackWhatGump( Mobile from, ArrayList list, int range ) : base( 20, 30 )
		{
			m_From = from;
			m_List = list;
			m_Range = range;

			AddPage( 0 );

			AddBackground( 0, 0, 440, 155, 5054 );

			AddBackground( 10, 10, 420, 75, 2620 );
			AddBackground( 10, 85, 420, 45, 3000 );

			if ( list.Count > 4 )
			{
				AddBackground( 0, 155, 440, 155, 5054 );

				AddBackground( 10, 165, 420, 75, 2620 );
				AddBackground( 10, 240, 420, 45, 3000 );

				if ( list.Count > 8 )
				{
					AddBackground( 0, 310, 440, 155, 5054 );

					AddBackground( 10, 320, 420, 75, 2620 );
					AddBackground( 10, 395, 420, 45, 3000 );
				}
			}

			for ( int i = 0; i < list.Count && i < 12; ++i )
			{
				Item m = (Item)list[i];

				AddItem( 20 + ((i % 4) * 100), 20 + ((i / 4) * 155), m.ItemID );
				AddButton( 20 + ((i % 4) * 100), 130 + ((i / 4) * 155), 4005, 4007, i + 1, GumpButtonType.Reply, 0 );

				if ( m.Name != null )
					AddHtml( 20 + ((i % 4) * 100), 90 + ((i / 4) * 155), 90, 40, m.Name, false, false );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			int index = info.ButtonID - 1;

			if ( index >= 0 && index < m_List.Count && index < 12 )
			{
				Item m = (Item)m_List[index];

				m_From.QuestArrow = new TrackArrow( m_From, m, m_Range * 2 );
			}
		}
	}

	public class TrackArrow : QuestArrow
	{
		private Mobile m_From;
		private Timer m_Timer;

		public TrackArrow( Mobile from, Item target, int range ) : base( from )
		{
			m_From = from;
			m_Timer = new TrackTimer( from, target, range, this );
			m_Timer.Start();
		}

		public override void OnClick( bool rightClick )
		{
			if ( rightClick )
			{
				PlantSearch.ClearTrackingInfo( m_From );

				m_From = null;

				Stop();
			}
		}

		public override void OnStop()
		{
			m_Timer.Stop();

			if ( m_From != null )
			{
				PlantSearch.ClearTrackingInfo( m_From );
			}
		}
	}

	public class TrackTimer : Timer
	{
		private Mobile m_From;
		private Item m_Target;
		private int m_Range;
		private int m_LastX, m_LastY;
		private QuestArrow m_Arrow;

		public TrackTimer( Mobile from, Item target, int range, QuestArrow arrow ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 2.5 ) )
		{
			m_From = from;
			m_Target = target;
			m_Range = range;

			m_Arrow = arrow;
		}

		protected override void OnTick()
		{
			if ( !m_Arrow.Running )
			{
				Stop();
				return;
			}
			else if ( m_From.NetState == null || m_From.Deleted || m_Target.Deleted || m_From.Map != m_Target.Map || !m_From.InRange( m_Target, m_Range ) )
			{
				m_From.Send( new CancelArrow() );

				PlantSearch.ClearTrackingInfo( m_From );

				Stop();
				return;
			}

			if ( m_LastX != m_Target.X || m_LastY != m_Target.Y )
			{
				m_LastX = m_Target.X;
				m_LastY = m_Target.Y;

				m_Arrow.Update( m_LastX, m_LastY );
			}
		}
	}
}
