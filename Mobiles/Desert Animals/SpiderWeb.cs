using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.Items
{
	public class SpiderWeb : BaseTrap
	{
        private int m_SkillLevel = 80;
        private int m_FeatLevel = 6;
        private Mobile m_Owner;
        private bool m_Armed;
        private bool m_InUse;

        private DateTime m_CreationDate;

        [CommandProperty( AccessLevel.GameMaster )]
        public TimeSpan Aging
        { get { return ( DateTime.Now - m_CreationDate ); } }

        public DateTime CreationDate
        {
            get { return m_CreationDate; }
            set { m_CreationDate = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SkillLevel
        {
            get { return m_SkillLevel; }
            set { m_SkillLevel = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Armed
        {
            get { return m_Armed; }
            set { m_Armed = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool InUse
        {
            get { return m_InUse; }
            set { m_InUse = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int FeatLevel
        {
            get { return m_FeatLevel; }
            set { m_FeatLevel = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

		[Constructable]
		public SpiderWeb() : base( 0x10D2 )
		{
            Name = "a Spider Web";
            Movable = false;
            Armed = true;
            CreationDate = DateTime.Now;
		}

		public override bool PassivelyTriggered{ get{ return false; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return m_FeatLevel; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

		public override void OnTrigger( Mobile from )
		{
			if( from == null || !from.Alive || from is ISpider || from is IDrider )
				return;
			
            TimeSpan maxage = new TimeSpan( 0, m_FeatLevel, 0 );

            if ( TimeSpan.Compare( maxage, this.Aging ) < 0 && this.Armed )
            {
                this.Delete();
                from.Emote( "*cuts through an old spider web*" );
                return;
            }

			if ( !from.Alive || from.Blessed || !this.Armed )
				return;

            PlayerMobile pm = from as PlayerMobile;

            int detecthidden = Convert.ToInt32( from.Skills[SkillName.Tactics].Fixed );
            
            int chancetodetect = detecthidden - m_SkillLevel;

            if( chancetodetect > ( 45 / m_FeatLevel ) + 15 )
            {
                chancetodetect = ( 45 / m_FeatLevel ) + 15 ;
            }

            if( chancetodetect < ( 15 / m_FeatLevel ) )
            {
                chancetodetect = ( 15 / m_FeatLevel );
            }

            int attackroll = Utility.Random( 100 );

            if( chancetodetect < attackroll )
            {
                if( from is PlayerMobile )
                    if( ( (PlayerMobile)from ).Evaded() )
                    {
                        from.Emote( "*avoided getting stuck in a spider web*" );
                        return;
                    }

                from.Emote( "*got stuck in a spider web*" );
                ((IKhaerosMobile)from).FreezeTimer = new SpiderWebTimer( from, m_FeatLevel, this );
                ((IKhaerosMobile)from).FreezeTimer.Start();
                this.Armed = false;
                this.Owner = from;
            }
            
			from.PlaySound( 901 );
		}

        public class SpiderWebTimer : Timer
        {
            private Mobile m_m;
            private SpiderWeb m_sw;

            public SpiderWebTimer( Mobile m, int featlevel, SpiderWeb sw )
                : base( TimeSpan.FromSeconds( featlevel ) )
            {
                m_m = m;
                m_sw = sw;
            }

            protected override void OnTick()
            {
            	if( m_sw.Owner != null && m_m != null )
            	{
		            m_m.Emote( "*broke free from a spider web*" );
		            m_sw.Owner = null;
		            m_sw.Delete();
		            
		            if( ((IKhaerosMobile)m_m).FreezeTimer != null )
		            {
		            	((IKhaerosMobile)m_m).FreezeTimer.Stop();
		            	((IKhaerosMobile)m_m).FreezeTimer = null;
		            }
            	}
            }
        }

        public static void CheckAge( SpiderWeb trap )
        {
            TimeSpan maxage = new TimeSpan( 0, trap.m_FeatLevel, 0 );

            if( TimeSpan.Compare( maxage, trap.Aging ) < 0 )
            {
            	if( trap.Owner != null )
            	{
	                Mobile m = World.FindMobile( trap.Owner.Serial );
	                
	                if( m != null )
	                {
		                if( !m.Alive || m.Location == trap.Location )
		                {
		                	m.Emote( "*broke free from a spider web*" );
            				m.CantWalk = false;
		                }
	                }
            	}
            	
            	trap.Delete();
            }
        }

		public SpiderWeb( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize( writer );

			writer.Write( (int) 2 ); // version

            writer.Write( (int) m_SkillLevel );
            writer.Write( (int) m_FeatLevel );
            writer.Write( (Mobile) m_Owner );
            writer.Write( (bool) m_Armed );
            writer.Write( (DateTime) m_CreationDate );
            writer.Write( (bool) m_InUse );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_SkillLevel = reader.ReadInt();
            m_FeatLevel = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_Armed = reader.ReadBool();
            m_CreationDate = reader.ReadDateTime();
            m_InUse = reader.ReadBool();
		}
	}
}
