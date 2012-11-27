using System;
using Server;
using Server.Movement;
using CalcMoves = Server.Movement.Movement;

namespace Server
{
	public class PathFollower
	{
		// Should we use pathfinding? 'false' for not
		private static bool Enabled = true;

		private Mobile m_From;
		private IPoint3D m_Goal;
		private bool m_EscapeGoal;
		private int m_EscapeDistance;
		private MovementPath m_Path;
		private int m_Index;
		private Point3D m_Next, m_LastGoalLoc;
		private DateTime m_LastPathTime;
		private MoveMethod m_Mover;

		public bool EscapeGoal { get{ return m_EscapeGoal; } }
		public MoveMethod Mover
		{
			get{ return m_Mover; }
			set{ m_Mover = value; }
		}

		public IPoint3D Goal
		{
			get{ return m_Goal; }
		}

		public PathFollower( Mobile from, IPoint3D goal ) : this( from, goal, -1 )
		{
		}
		public PathFollower( Mobile from, IPoint3D goal, int goalDist )
		{
			m_From = from;
			m_Goal = goal;
			if ( goalDist != -1 )
			{
				m_EscapeGoal = true;
				m_EscapeDistance = goalDist;
			}
		}

		public MoveResult Move( Direction d )
		{
			if ( m_Mover == null )
				return ( m_From.Move( d ) ? MoveResult.Success : MoveResult.Blocked );

			return m_Mover( d );
		}

		public Point3D GetGoalLocation()
		{
			if ( m_Goal is Item )
				return ((Item)m_Goal).GetWorldLocation();

			return new Point3D( m_Goal );
		}

		private static TimeSpan RepathDelay = TimeSpan.FromSeconds( 2.0 );

		public void Advance( ref Point3D p, int index )
		{
			if ( m_Path != null && m_Path.Success )
			{
				Direction[] dirs = m_Path.Directions;

				if ( index >= 0 && index < dirs.Length )
				{
					int x = p.X, y = p.Y;

					CalcMoves.Offset( dirs[index], ref x, ref y );

					p.X = x;
					p.Y = y;
				}
			}
		}

		public void ForceRepath()
		{
			m_Path = null;
		}

		public bool CheckPath()
		{
			if ( !Enabled )
				return false;

			bool repath = false;

			Point3D goal = GetGoalLocation();

			if ( m_Path == null )
				repath = true;
			else if ( (!m_Path.Success || goal != m_LastGoalLoc) && (m_LastPathTime + RepathDelay) <= DateTime.Now )
				repath = true;
			else if ( m_Path.Success && Check( m_From.Location, m_LastGoalLoc, 0 ) )
				repath = true;

			if ( !repath )
				return false;

			m_LastPathTime = DateTime.Now;
			m_LastGoalLoc = goal;

			if ( m_EscapeGoal )
				m_Path = new MovementPath( m_From, goal, m_EscapeDistance );
			else
				m_Path = new MovementPath( m_From, goal );

			m_Index = 0;
			m_Next = m_From.Location;

			Advance( ref m_Next, m_Index );

			return true;
		}

		public bool Check( Point3D loc, Point3D goal, int range )
		{
			if ( !m_EscapeGoal && !Utility.InRange( loc, goal, range ) )
				return false;
			else if ( m_EscapeGoal && Utility.InRange( loc, goal, m_EscapeDistance ) )
				return false;

			if ( ( range <= 1 && Math.Abs( loc.Z - goal.Z ) >= 16 ) && !m_EscapeGoal )
				return false;

			return true;
		}

		public bool Follow( bool run, int range )
		{
			Point3D goal = GetGoalLocation();
			Direction d;

			if ( Check( m_From.Location, goal, range ) )
				return true;

			bool repathed = CheckPath();

			if ( !Enabled || !m_Path.Success )
			{
				// path failed somehow, let's use a cheap method
				if ( !m_EscapeGoal ) // move towards
					d = m_From.GetDirectionTo( goal );
				else // move away
					d = Utility.GetDirection( new Point2D( goal.X, goal.Y ), new Point2D( m_From.Location.X, m_From.Location.Y ) );

				if ( run )
					d |= Direction.Running;

				m_From.SetDirection( d );
				Move( d );

				return Check( m_From.Location, goal, range );
			}

			d = m_From.GetDirectionTo( m_Next );

			if ( run )
				d |= Direction.Running;

			m_From.SetDirection( d );

			MoveResult res = Move( d );

			if ( res == MoveResult.Blocked )
			{
				if ( repathed )
					return false;

				m_Path = null;
				CheckPath();

				if ( !m_Path.Success )
				{
					d = m_From.GetDirectionTo( goal );

					if ( run )
						d |= Direction.Running;

					m_From.SetDirection( d );
					Move( d );

					return Check( m_From.Location, goal, range );
				}

				d = m_From.GetDirectionTo( m_Next );

				if ( run )
					d |= Direction.Running;

				m_From.SetDirection( d );

				res = Move( d );

				if ( res == MoveResult.Blocked )
					return false;
			}

			if ( m_From.X == m_Next.X && m_From.Y == m_Next.Y )
			{
				if ( m_From.Z == m_Next.Z )
				{
					++m_Index;
					Advance( ref m_Next, m_Index );
				}
				else
				{
					m_Path = null;
				}
			}

			return Check( m_From.Location, goal, range );
		}
	}
}
