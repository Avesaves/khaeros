using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Misc;

namespace Server.Items
{
	public abstract class BaseTrap : Item
	{
		public virtual bool PassivelyTriggered{ get{ return false; } }
		public virtual TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public virtual int PassiveTriggerRange{ get{ return -1; } }
		public virtual TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }
		
		private int m_RequiredSkill = -1;
		private int m_TrapLevel = Utility.RandomMinMax( 1, 5 );
		private int m_MinDamage = -1;
		private int m_MaxDamage = -1;
		private int m_XPOnDisarm = -1;
		private PlayerMobile m_Disarming;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int RequiredSkill
		{
			get
			{
				if( m_RequiredSkill > 0 )
					return m_RequiredSkill;
				
				return (m_TrapLevel * 20);
			}
			set{ m_RequiredSkill = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int XPOnDisarm
		{
			get
			{
				if( m_XPOnDisarm > 0 )
					return m_XPOnDisarm;
				
				return (m_TrapLevel * 200);
			}
			set{ m_XPOnDisarm = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MinDamage
		{
			get
			{
				if( m_MinDamage > 0 )
					return m_MinDamage;
				
				return (m_TrapLevel * 10);
			}
			set{ m_MinDamage = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxDamage
		{
			get
			{
				if( m_MaxDamage > 0 )
					return m_MaxDamage;
				
				return (m_TrapLevel * 20);
			}
			set{ m_MaxDamage = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapLevel
		{
			get{ return m_TrapLevel; }
			set
			{
				if( value < 1 )
					m_TrapLevel = 1;
				
				else if( value > 5 )
					m_TrapLevel = 5;
				
				else
					m_TrapLevel = value;
			}
		}
		
		public virtual int GetDamage()
		{
			return Utility.RandomMinMax( MinDamage, MaxDamage );
		}

		private DateTime m_NextPassiveTrigger, m_NextActiveTrigger;

		public virtual void OnTrigger( Mobile from )
		{
		}
		
		public virtual void OnDisarmAttempt( PlayerMobile from )
		{
			if( m_Disarming != null )
			{
				from.SendMessage( "Someone is already trying to disarm that trap." );
				return;
			}
			
			from.RevealingAction();
			XmlFreeze freeze = new XmlFreeze( 2.0 );
			freeze.Name = "disarming";
			XmlAttach.AttachTo( from, freeze );
			from.Emote( "*starts disarming a trap*" );
			from.PlaySound( 996 );
			m_Disarming = from;
			new DisarmTimer( this ).Start();
		}
		
		public class DisarmTimer : Timer
        {
            private BaseTrap m_trap;

            public DisarmTimer( BaseTrap trap )
                : base( TimeSpan.FromSeconds(2.0) )
            {
                m_trap = trap;
            }

            protected override void OnTick()
            {
            	if( m_trap == null || m_trap.Deleted )
            		return;
            	
            	m_trap.OnDisarm();
            }
        }
		
		public virtual void OnDisarm()
		{
			if( m_Disarming == null )
				return;
			
			int needed = Utility.RandomMinMax( (int)(RequiredSkill * 0.5), RequiredSkill );
			int roll = Utility.RandomMinMax( (int)(m_Disarming.Skills[SkillName.ArmDisarmTraps].Base * 0.75), (int)m_Disarming.Skills[SkillName.ArmDisarmTraps].Base );
			
			if( roll >= needed )
			{
				m_Disarming.SendMessage( "You have successfully disarmed the trap!" );
				LevelSystem.AwardExp( m_Disarming, XPOnDisarm );
				LevelSystem.AwardCP( m_Disarming, (int)( m_Disarming.LastXP * 0.12 ) );
				this.Delete();
				return;
			}
			
			else
			{
				m_Disarming.SendMessage( "You accidentally activate the trap while trying to disarm it!" );
				OnTrigger( m_Disarming );
				m_Disarming = null;
			}
		}

		public override bool HandlesOnMovement{ get{ return true; } } // Tell the core that we implement OnMovement

		public virtual int GetEffectHue()
		{
			int hue = this.Hue & 0x3FFF;

			if ( hue < 2 )
				return 0;

			return hue - 1;
		}

		public bool CheckRange( Point3D loc, Point3D oldLoc, int range )
		{
			return CheckRange( loc, range ) && !CheckRange( oldLoc, range );
		}

		public bool CheckRange( Point3D loc, int range )
		{
			return ( (this.Z + 8) >= loc.Z && (loc.Z + 16) > this.Z )
				&& Utility.InRange( GetWorldLocation(), loc, range );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if ( m.Location == oldLocation )
				return;

			if ( CheckRange( m.Location, oldLocation, 0 ) && DateTime.Now >= m_NextActiveTrigger )
			{
				m_NextActiveTrigger = m_NextPassiveTrigger = DateTime.Now + ResetDelay;

				OnTrigger( m );
			}
			else if ( PassivelyTriggered && CheckRange( m.Location, oldLocation, PassiveTriggerRange ) && DateTime.Now >= m_NextPassiveTrigger )
			{
				m_NextPassiveTrigger = DateTime.Now + PassiveTriggerDelay;

				OnTrigger( m );
			}
		}

		public BaseTrap( int itemID ) : base( itemID )
		{
			Movable = false;
		}

		public BaseTrap( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write( (int) m_RequiredSkill );
			writer.Write( (int) m_TrapLevel );
			writer.Write( (int) m_MinDamage );
			writer.Write( (int) m_MaxDamage );
			writer.Write( (int) m_XPOnDisarm );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version > 0 )
			{
				m_RequiredSkill = reader.ReadInt();
				m_TrapLevel = reader.ReadInt();
				m_MinDamage = reader.ReadInt();
				m_MaxDamage = reader.ReadInt();
				m_XPOnDisarm = reader.ReadInt();
			}
		}
	}
}
