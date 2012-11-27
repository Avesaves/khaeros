using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x108F, 0x1091 )]
	public class ElevatorSwitch : Item 
	{ 
		private BaseAddon m_Elevator;
		private bool m_InUse;
		private int m_Height;
		private int m_Ticks;
		private bool m_Peaked;
		private int m_OutOfUseID;
		private int m_InUseID;
		private Point3D m_NorthWestEdge;
		private int m_SizeNorthSouth;
		private int m_SizeEastWest;
		private bool m_Start;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BaseAddon Elevator
		{
			get{ return m_Elevator; }
			set{ m_Elevator = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Start
		{
			get{ return m_Start; }
			set
			{ 
				if( value == true )
				{
					if( this.InUse && this.Elevator != null )
					{
						this.ItemID = InUseID;
						
						this.InUse = true;
						
			            BeginMove( TimeSpan.FromSeconds( 0.2 ) );
					}
				}
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool InUse
		{
			get{ return m_InUse; }
			set{ m_InUse = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Height
		{
			get{ return m_Height; }
			set{ m_Height = value; }
		}
		
		public int Ticks
		{
			get{ return m_Ticks; }
			set{ m_Ticks = value; }
		}

		public bool Peaked
		{
			get{ return m_Peaked; }
			set{ m_Peaked = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int OutOfUseID
		{
			get{ return m_OutOfUseID; }
			set{ m_OutOfUseID = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int InUseID
		{
			get{ return m_InUseID; }
			set{ m_InUseID = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D NorthWestEdge
		{
			get{ return m_NorthWestEdge; }
			set{ m_NorthWestEdge = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int SizeNorthSouth
		{
			get{ return m_SizeNorthSouth; }
			set{ m_SizeNorthSouth = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int SizeEastWest
		{
			get{ return m_SizeEastWest; }
			set{ m_SizeEastWest = value; }
		}

        private Timer m_MoveTimer;
        private DateTime m_MoveTime;
		
		[Constructable] 
		public  ElevatorSwitch() : base( 0x108F ) 
		{ 
        	Name = "Switch"; 
        	Height = 20;
        	SizeEastWest = 3;
        	SizeNorthSouth = 3;
        	OutOfUseID = 4239;
        	InUseID = 4240;
        	Movable = false;
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			PlayerMobile from = m as PlayerMobile;
			
			if( m == null || !( from is PlayerMobile ) || this.InUse || !m.InRange( this.Location, 2 ) )
			{
				return;
			}
			
			if( Elevator == null )
			{
				if( from.AccessLevel > AccessLevel.Player )
					m.SendMessage( 60, "First, you must link this switch to an elevator." );
				
				return;
			}

			this.ItemID = InUseID;
			
			from.PlaySound( 0x51 );
			
			this.InUse = true;
			
            BeginMove( TimeSpan.FromSeconds( 0.2 ) );
        }
		
		public void BeginMove( TimeSpan delay )
        {
            if( m_MoveTimer != null )
                m_MoveTimer.Stop();
            
            if( this.Elevator == null || this.Elevator.Deleted )
            	return;

            m_MoveTime = DateTime.Now + delay;

            m_MoveTimer = new InternalTimer( this, delay );
            m_MoveTimer.Start();
        }
		
		public override void OnAfterDelete()
		{
			if ( m_MoveTimer != null )
				m_MoveTimer.Stop();

			m_MoveTimer = null;
		}
		
		private class InternalTimer : Timer
        {
            private ElevatorSwitch m_switch;

            public InternalTimer( ElevatorSwitch s, TimeSpan delay )
                : base( delay )
            {
            	if( s == null || s.Deleted )
            		return;
            	
                m_switch = s;
            }

            protected override void OnTick()
            {
            	if( m_switch == null || m_switch.Deleted )
            		return;
            	
            	if( m_switch.Elevator == null && m_switch.Elevator.Deleted )
            	{
            		m_switch.Elevator = null;
            		m_switch.m_MoveTimer.Stop();
            		m_switch.m_MoveTimer = null;
            		return;
            	}
            	
            	if( !m_switch.Peaked )
            	{
	                m_switch.Elevator.Z++;
	                m_switch.Ticks++;
	                m_switch.MoveObjects( true );
	                
	                if( m_switch.Height == m_switch.Ticks )
	                {
	                	m_switch.InUse = false;
	                	m_switch.ItemID = m_switch.OutOfUseID;
	                	m_switch.Peaked = true;
	                	m_switch.m_MoveTimer = null;
	                	return;
	                }
	                
	                else
	                	m_switch.BeginMove( TimeSpan.FromSeconds( 0.2 ) );
            	}
            	
            	else
            	{
	                m_switch.Elevator.Z--;
	                m_switch.Ticks--;
	                m_switch.MoveObjects( false );
	                
	                if( m_switch.Ticks == 0 )
	                {
	                	m_switch.InUse = false;
	                	m_switch.ItemID = m_switch.OutOfUseID;
	                	m_switch.Peaked = false;
	                	m_switch.m_MoveTimer = null;
	                	return;
	                }
	                
	                else
	                	m_switch.BeginMove( TimeSpan.FromSeconds( 0.2 ) );
            	}
            }
        }
		
		public void MoveObjects( bool Ascending )
		{
			ArrayList toMove = new ArrayList();
			
			IPooledEnumerable eable = Map.GetObjectsInBounds( new Rectangle2D( NorthWestEdge.X, NorthWestEdge.Y, SizeEastWest, SizeNorthSouth ) );

			foreach ( object o in eable )
			{
				if ( o is Item )
				{
					Item item = (Item)o;

					if ( item.Visible )
					{
						if( item is AddonComponent )
						{
							if( this.Elevator.Components.Contains( ( (AddonComponent)item ) ) )
								continue;
						}
						
						if( item.Z + 1 < this.Elevator.Z )
							continue;
						
						toMove.Add( item );
					}
						
				}
				else if ( o is Mobile )
				{
					if( ( (Mobile)o ).Z + 1 < this.Elevator.Z )
						continue;
					
					toMove.Add( o );
				}
			}

			eable.Free();
			
			for ( int i = 0; i < toMove.Count; ++i )
			{
				object o = toMove[i];

				if ( o is Item )
				{
					if( Ascending )
					{
						((Item)o).Z++;
					}
					
					else
						((Item)o).Z--;
				}
				else if ( o is Mobile )
				{
					if( Ascending )
					{
						((Mobile)o).Z++;
					}
					
					else
						((Mobile)o).Z--;
				}
			}
		}

  		public  ElevatorSwitch( Serial serial ) : base( serial ) 
  		{ 
 		} 

 		public override void Serialize( GenericWriter writer ) 
  		{ 
	     	base.Serialize( writer ); 
	
	     	writer.Write( (int) 1 ); // version 
	     	
	     	writer.Write( (BaseAddon) m_Elevator );
	     	writer.Write( (bool) m_InUse );
	     	writer.Write( (int) m_Height );
	     	writer.Write( (int) m_Ticks );
	     	writer.Write( (bool) m_Peaked );
	     	writer.Write( (int) m_OutOfUseID );
	     	writer.Write( (int) m_InUseID );
	     	writer.Write( (Point3D) m_NorthWestEdge );
	     	writer.Write( (int) m_SizeNorthSouth );
	     	writer.Write( (int) m_SizeEastWest );

            writer.Write( m_MoveTimer != null );

            if( m_MoveTimer != null )
                writer.WriteDeltaTime( m_MoveTime );
  		} 

  		public override void Deserialize( GenericReader reader ) 
  		{ 
	     	base.Deserialize( reader ); 
	
	     	int version = reader.ReadInt();      	
	     	
	     	m_Elevator = (BaseAddon)reader.ReadItem();
	     	m_InUse = reader.ReadBool();
	     	m_Height = reader.ReadInt();
	     	m_Ticks = reader.ReadInt();
	     	m_Peaked = reader.ReadBool();
	     	m_OutOfUseID = reader.ReadInt();
	     	m_InUseID = reader.ReadInt();
	     	m_NorthWestEdge = reader.ReadPoint3D();
	     	m_SizeNorthSouth = reader.ReadInt();
	     	m_SizeEastWest = reader.ReadInt();

            if( reader.ReadBool() )
                BeginMove( reader.ReadDeltaTime() - DateTime.Now );
     	
  		} 
   	} 
} 
