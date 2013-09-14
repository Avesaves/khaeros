using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Spells.Necromancy;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.XmlSpawner2;
using Server.Commands;
using Server.Targeting;
using Server.Items;

namespace Server.SkillHandlers
{
	public class Tracking
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Tracking].Callback = new SkillUseCallback( OnUse );
            CommandSystem.Register( "Mark", AccessLevel.Player, new CommandEventHandler( Mark_OnCommand ) );
            CommandSystem.Register( "TrackMark", AccessLevel.Player, new CommandEventHandler( TrackMark_OnCommand ) );
		}

        private class MarkTarget : Target
        {
            public MarkTarget( Mobile m )
                : base( 15, false, TargetFlags.None )
            {
                m.SendMessage( "Whom do you wish to mark for tracking?" );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || !(obj is Mobile) )
            		return;

                Mobile target = obj as Mobile;

                if( m.CanSee( target ) && m.InLOS( target ) )
                {
                    TrackWhoGump.NewMark( m, target );
                    m.SendMessage( "You have successfully set " + target.Name + " as your mark for Tracking." );
                }

                else
                    m.SendMessage( "Target out of sight." );
            }
        }

        [Usage( "Mark" )]
        [Description( "Marks a target for Tracking." )]
        private static void Mark_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            e.Mobile.Target = new MarkTarget( e.Mobile );
        }

        [Usage( "TrackMark" )]
        [Description( "Tracks the mobile you have marked for tracking." )]
        private static void TrackMark_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            Mobile from = e.Mobile;

            XmlTracking trackAtt = XmlAttach.FindAttachment( e.Mobile, typeof( XmlTracking ) ) as XmlTracking;

            SkillInfo info = SkillInfo.Table[(int)SkillName.Tracking];

            int range = TrackWhoGump.GetTrackingRange( from );

            from.CloseGump( typeof( TrackWhoGump ) );
            from.CloseGump( typeof( TrackWhatGump ) );
            Tracking.ClearTrackingInfo( from );

            if( from.QuestArrow != null )
                from.QuestArrow.Stop();

            if( trackAtt == null || trackAtt.Target == null || trackAtt.Target.Deleted )
                e.Mobile.SendMessage( "You do not have a target marked for tracking." );

            else if( info.Callback != null )
            {
                if( from.NextSkillTime <= DateTime.Now && from.Spell == null )
                {
                    from.DisruptiveAction();

                    from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

                    if( from.InRange( trackAtt.Target.Location, range ) && TrackWhoGump.CheckDifficulty( from, trackAtt.Target ) )
                    {
                        from.QuestArrow = new TrackArrow( from, trackAtt.Target, range * 2 );
                        return;
                    }

                    e.Mobile.SendMessage( "You have failed to locate your mark." );
                }

                else
                    from.SendSkillMessage();
            }                
        }

		public static TimeSpan OnUse( Mobile m )
		{
			m.SendLocalizedMessage( 1011350 ); // What do you wish to track?

			m.CloseGump( typeof( TrackWhatGump ) );
			m.CloseGump( typeof( TrackWhoGump ) );
			m.SendGump( new TrackWhatGump( m ) );

			return TimeSpan.FromSeconds( 10.0 ); // 10 second delay before beign able to re-use a skill
		}

		public class TrackingInfo
		{
			public Mobile m_Tracker;
			public Mobile m_Target;
			public Point2D m_Location;
			public Map m_Map;

			public TrackingInfo( Mobile tracker, Mobile target )
			{
				m_Tracker = tracker;
				m_Target = target;
				m_Location = new Point2D( target.X, target.Y );
				m_Map = target.Map;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public static void AddInfo( Mobile tracker, Mobile target )
		{
			TrackingInfo info = new TrackingInfo( tracker, target );
			m_Table[tracker] = info;
		}

		public static double GetStalkingBonus( Mobile tracker, Mobile target )
		{
			// Note: This is not reset as of publish 35.

			TrackingInfo info = m_Table[tracker] as TrackingInfo;

			if ( info == null || info.m_Target != target || info.m_Map != target.Map )
				return 0.0;

			int xDelta = info.m_Location.X - target.X;
			int yDelta = info.m_Location.Y - target.Y;

			return Math.Sqrt( (xDelta * xDelta) + (yDelta * yDelta) );
		}

		public static void ClearTrackingInfo( Mobile tracker )
		{
            if( m_Table.Contains( tracker ) )
			    m_Table.Remove( tracker );
		}
	}

	public class TrackWhatGump : Gump
	{
		private Mobile m_From;
		private bool m_Success;

		public TrackWhatGump( Mobile from ) : base( 20, 30 )
		{
			m_From = from;
			m_Success = from.CheckSkill( SkillName.Tracking, 0.0, 21.1 );
			
			if( from is PlayerMobile && !m_Success )
			{
				PlayerMobile tracker = from as PlayerMobile;
				
				if( tracker.HasHuntingHoundBonus )
				{
					Dog dog = tracker.HuntingHound as Dog;
					
					if( dog != null && Utility.Random( 100 ) < dog.Level )
						m_Success = true;
				}
			}

			AddPage( 0 );

			AddBackground( 0, 0, 440, 135, 5054 );

			AddBackground( 10, 10, 420, 75, 2620 );
			AddBackground( 10, 85, 420, 25, 3000 );

			AddItem( 20, 20, 9682 );
			AddButton( 20, 110, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 20, 90, 100, 20, 1018087, false, false ); // Animals

			AddItem( 120, 20, 9607 );
			AddButton( 120, 110, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 120, 90, 100, 20, 1018088, false, false ); // Monsters

			AddItem( 220, 20, 8454 );
			AddButton( 220, 110, 4005, 4007, 3, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 220, 90, 100, 20, 1018089, false, false ); // Human NPCs

			AddItem( 320, 20, 8455 );
			AddButton( 320, 110, 4005, 4007, 4, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 320, 90, 100, 20, 1018090, false, false ); // Players
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID >= 1 && info.ButtonID <= 4 )
				TrackWhoGump.DisplayTo( m_Success, m_From, info.ButtonID - 1 );
		}
	}

	public delegate bool TrackTypeDelegate( Mobile m );

	public class TrackWhoGump : Gump
	{
		private Mobile m_From;
		private int m_Range;

		private static TrackTypeDelegate[] m_Delegates = new TrackTypeDelegate[]
			{
				new TrackTypeDelegate( IsAnimal ),
				new TrackTypeDelegate( IsMonster ),
				new TrackTypeDelegate( IsHumanNPC ),
				new TrackTypeDelegate( IsPlayer )
			};

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

				Mobile a = x as Mobile;
				Mobile b = y as Mobile;

				if ( a == null || b == null )
					throw new ArgumentException();

				return m_From.GetDistanceToSqrt( a ).CompareTo( m_From.GetDistanceToSqrt( b ) );
			}
		}

        public static int GetTrackingRange( Mobile from )
        {
            int range = 25 + (int)( from.Skills[SkillName.Tracking].Base );

            if( from is PlayerMobile )
            {
                PlayerMobile tracker = from as PlayerMobile;
                range += tracker.Feats.GetFeatLevel( FeatList.EnhancedTracking ) * 25;
                IPooledEnumerable eable = from.Map.GetMobilesInRange(from.Location, 20);
                foreach (Mobile m in eable)
                {
                    if (m is BirdOfPrey)
                    {
                        if ((m as BirdOfPrey).Controlled)
                        {
                            if ((m as BirdOfPrey).ControlMaster != null && !(m as BirdOfPrey).ControlMaster.Deleted && (m as BirdOfPrey).ControlMaster == from)
                                range += ((m as BirdOfPrey).Level * (m as BirdOfPrey).XPScale);
                        }
                    }
                }
                eable.Free();

                range += BaseWeapon.GetRacialMountAbility(tracker, typeof(GallowayHorse)) > 0 ? (tracker.Mount as BaseMount).Level * (tracker.Mount as BaseMount).XPScale : 0;
            }

            return range;
        }

		public static void DisplayTo( bool success, Mobile from, int type )
		{
			if ( !success )
			{
				from.SendLocalizedMessage( 1018092 ); // You see no evidence of those in the area.
				return;
			}

			Map map = from.Map;

			if ( map == null )
				return;

			TrackTypeDelegate check = m_Delegates[type];

            int range = GetTrackingRange( from );

			ArrayList list = new ArrayList();

			foreach ( Mobile m in from.GetMobilesInRange( range ) )
			{
				// Ghosts can no longer be tracked 
				if ( m != from && (!Core.AOS || m.Alive) && (!m.Hidden || m.AccessLevel == AccessLevel.Player || from.AccessLevel > m.AccessLevel) && check( m ) && CheckDifficulty( from, m ) )
					list.Add( m );
			}

			if ( list.Count > 0 )
			{
				list.Sort( new InternalSorter( from ) );

				from.SendGump( new TrackWhoGump( from, list, range ) );
				from.SendLocalizedMessage( 1018093 ); // Select the one you would like to track.
			}
			else
			{
				if ( type == 0 )
					from.SendLocalizedMessage( 502991 ); // You see no evidence of animals in the area.
				else if ( type == 1 )
					from.SendLocalizedMessage( 502993 ); // You see no evidence of creatures in the area.
				else
					from.SendLocalizedMessage( 502995 ); // You see no evidence of people in the area.
			}
		}

		// Tracking players uses tracking and detect hidden vs. hiding and stealth 
		public static bool CheckDifficulty( Mobile from, Mobile m )
		{
			if ( !Core.AOS || !m.Player )
				return true;

			int mBonus = 0;
			int fromBonus = 0;
			
			if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.Nation == Nation.Western )
					mBonus = 25;

                if( pm.IsVampire && pm.Feats.GetFeatLevel( FeatList.Obfuscate ) > 0 )
                    mBonus = 100;
				
				mBonus += pm.Feats.GetFeatLevel(FeatList.EnhancedStealth) * 25;
			}
			
			if( from is PlayerMobile )
			{
				PlayerMobile pm = from as PlayerMobile;

                if (pm.Nation == Nation.Mhordul)
                    fromBonus = 25;

				fromBonus += pm.Feats.GetFeatLevel(FeatList.EnhancedTracking) * 50;

                IPooledEnumerable eable = pm.Map.GetMobilesInRange(pm.Location, 20);
                foreach (Mobile mob in eable)
                {
                    if (mob is BirdOfPrey)
                    {
                        if ((mob as BirdOfPrey).Controlled && (mob as BirdOfPrey).ControlMaster != null && !(mob as BirdOfPrey).ControlMaster.Deleted && (mob as BirdOfPrey).ControlMaster == pm)
                            fromBonus += ((mob as BirdOfPrey).Level / (6 - (mob as BirdOfPrey).XPScale));
                    }
                }
                eable.Free();
			}
			
			int tracking = (int)(from.Skills[SkillName.Tracking].Base) * 2;
			int detectHidden = (int)(from.Skills[SkillName.DetectHidden].Base);
			int total = (50 * (tracking + detectHidden + fromBonus)) + BaseWeapon.GetRacialMountAbility(from, typeof(GallowayHorse));

			int hiding = (int)(m.Skills[SkillName.Hiding].Base);
			int stealth = (int)(m.Skills[SkillName.Stealth].Base);
			int divisor = hiding + stealth + mBonus;
			
			if( m.Hidden )
				divisor += 25;

			int chance = 100;
			
			if( divisor > 0 )
				chance = total / divisor;

            bool success = chance > Utility.Random( 100 );

			return success;
		}

        public static void NewMark( Mobile tracker, Mobile target )
        {
            XmlTracking trackAtt = XmlAttach.FindAttachment( tracker, typeof( XmlTracking ) ) as XmlTracking;

            if( trackAtt == null )
            {
                trackAtt = new XmlTracking();
                XmlAttach.AttachTo( tracker, trackAtt );
            }

            trackAtt.Target = target;
        }

		private static bool IsAnimal( Mobile m )
		{
			return ( !m.Player && m.Body.IsAnimal );
		}

		private static bool IsMonster( Mobile m )
		{
			return ( !m.Player && m.Body.IsMonster );
		}

		private static bool IsHumanNPC( Mobile m )
		{
			return ( !m.Player && m.Body.IsHuman );
		}

		private static bool IsPlayer( Mobile m )
		{
			return m.Player;
		}

		private ArrayList m_List;

		private TrackWhoGump( Mobile from, ArrayList list, int range ) : base( 20, 30 )
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
				Mobile m = (Mobile)list[i];

				AddItem( 20 + ((i % 4) * 100), 20 + ((i / 4) * 155), ShrinkTable.Lookup( m ) );
				AddButton( 20 + ((i % 4) * 100), 130 + ((i / 4) * 155), 4005, 4007, i + 1, GumpButtonType.Reply, 0 );

                if( m.Name != null )
                {
                    string name = m.Name;

                    if( m is PlayerMobile )
                    {
                        PlayerMobile pm = m as PlayerMobile;
                        name = GetWeightTag( pm ) + " & " + GetDistanceTag( from, m );
                    }

                    AddHtml( 20 + ( ( i % 4 ) * 100 ), 90 + ( ( i / 4 ) * 155 ), 90, 40, name, false, false );
                }
			}
		}

        public static string GetDistanceTag( Mobile tracker, Mobile target )
        {
            string distance = "Extra Far";
            int sqrs = (int)(tracker.GetDistanceToSqrt( target.Location ));

            if( sqrs < 10 )
                distance = "Extra Close";
            else if( sqrs < 20 )
                distance = "Very Close";
            else if( sqrs < 35 )
                distance = "Close";
            else if( sqrs < 50 )
                distance = "Quite Close";
            else if( sqrs < 75 )
                distance = "Quite Far";
            else if( sqrs < 100 )
                distance = "Far";
            else if( sqrs < 125 )
                distance = "Very Far";

            return distance;
        }

        public static string GetWeightTag( PlayerMobile pm )
        {
            string tag = "Heavy";

            if( pm.Weight < 85 )
                tag = "Tiny";
            else if( pm.Weight < 90 )
                tag = "Small";
            else if( pm.Weight < 95 )
                tag = "Light";
            else if( pm.Weight < 100 )
                tag = "Limber";
            else if( pm.Weight < 105 )
                tag = "Average";
            else if( pm.Weight < 110 )
                tag = "Stout";
            else if( pm.Weight < 115 )
                tag = "Bulky";
            else if( pm.Weight < 120 )
                tag = "Large";

            return tag;
        }

		public override void OnResponse( NetState state, RelayInfo info )
		{
			int index = info.ButtonID - 1;

			if ( index >= 0 && index < m_List.Count && index < 12 )
			{
				Mobile m = (Mobile)m_List[index];

				m_From.QuestArrow = new TrackArrow( m_From, m, m_Range * 2 );

				if ( Core.SE )
					Tracking.AddInfo( m_From, m );

                NewMark( m_From, m );
			}
		}
	}

	public class TrackArrow : QuestArrow
	{
		private Mobile m_From;
		private Timer m_Timer;

		public TrackArrow( Mobile from, Mobile target, int range ) : base( from )
		{
			m_From = from;
			m_Timer = new TrackTimer( from, target, range, this );
			m_Timer.Start();
		}

		public override void OnClick( bool rightClick )
		{
			if ( rightClick )
			{
				Tracking.ClearTrackingInfo( m_From );

				m_From = null;

				Stop();
			}
		}

		public override void OnStop()
		{
			m_Timer.Stop();

			if ( m_From != null )
			{
				Tracking.ClearTrackingInfo( m_From );

				m_From.SendLocalizedMessage( 503177 ); // You have lost your quarry.
			}
		}
	}

	public class TrackTimer : Timer
	{
		private Mobile m_From, m_Target;
		private int m_Range;
		private int m_LastX, m_LastY;
		private QuestArrow m_Arrow;

		public TrackTimer( Mobile from, Mobile target, int range, QuestArrow arrow ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 2.5 ) )
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
			else if ( m_From.NetState == null || m_From.Deleted || m_Target.Deleted || m_From.Map != m_Target.Map || !m_From.InRange( m_Target, m_Range ) || !TrackWhoGump.CheckDifficulty(m_From, m_Target) )
			{
				m_From.Send( new CancelArrow() );
				m_From.SendLocalizedMessage( 503177 ); // You have lost your quarry.

				Tracking.ClearTrackingInfo( m_From );

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
