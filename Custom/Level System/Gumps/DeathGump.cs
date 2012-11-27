using System;
using Server;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Gumps;
using Server.Multis;
using Server.Engines.Help;
using Server.ContextMenus;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps
{
    public class DeathGump : Gump
    {
        public Container m_corpse;

        public DeathGump( Mobile m, int time, Container corpse )
            : base( 0, 0 )
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = false;
            this.Resizable = false;
            this.AddPage( 0 );
            this.AddImageTiled( 0, 0, 1280, 1024, 2702 );
            m_corpse = corpse;
            
            if( m is PlayerMobile )
            {
            	( (PlayerMobile)m ).m_DeathTimer = new DeathTimer( m, time, corpse );
            	( (PlayerMobile)m ).m_DeathTimer.Start();
            }

            if( corpse != null )
                this.AddLabel( 300, 300, 2656, "You have been knocked out, but you are not dead. You will wake up shortly. Please, wait patiently." );
        }
    }
    
    public class KOTimer : Timer
    {
        public PlayerMobile pm;

        public KOTimer( PlayerMobile m )
            : base( TimeSpan.FromMinutes( 5 ) )
        {
            pm = m;
        }

        protected override void OnTick()
        {
        	if( pm == null || pm.Deleted )
        		return;
        	
        	pm.m_KOPenalty = null;
        }
    }
    
    public class InvulTimer : Timer
    {
        public Mobile m_mob;

        public InvulTimer( Mobile m )
            : base( TimeSpan.FromSeconds( 20 ) )
        {
            m_mob = m;
            m.Blessed = true;
        }

        protected override void OnTick()
        {
        	if( m_mob == null || m_mob.Deleted )
        		return;
        	
        	m_mob.Blessed = false;
        	
        	if( m_mob is PlayerMobile )
            	( (PlayerMobile)m_mob ).m_InvulTimer = null;
        }
    }
    
    public class DeathTimer : Timer
    {
        public Mobile m_mob;
        public Container m_corpse;

        public DeathTimer( Mobile m, int seconds, Container corpse )
            : base( TimeSpan.FromSeconds( seconds ) )
        {
            m_mob = m;
            m_corpse = corpse;
            m.Location = new Point3D( 6072, 267, 0 );
            m.Squelched = true;
            
            if( m is PlayerMobile && ( (PlayerMobile)m ).RageTimer != null )
            {
            	( (PlayerMobile)m ).RageTimer.Stop();
            	( (PlayerMobile)m ).RageTimer.Delay = TimeSpan.FromSeconds( 1 );
            	( (PlayerMobile)m ).RageTimer.Start();
            }
        }

        protected override void OnTick()
        {
        	if( m_mob == null || m_mob.Deleted || m_corpse == null || m_corpse.Deleted )
        		return;
        	
            m_mob.CloseGump( typeof( DeathGump ) );
            ArrayList list = new ArrayList();
            Container pack = m_mob.Backpack;
            m_corpse.Movable = true;

            m_mob.Resurrect();

            if( m_corpse.Parent != null )
            {
                try
                {
                    Mobile parent = World.FindMobile( m_corpse.RootParentEntity.Serial );
                        m_mob.Location = parent.Location;
                }

                catch
                {
                    Container parent = m_corpse.Parent as Container;
                    m_mob.Location = parent.Location;
                }
            }

            else
            {
                m_mob.Location = m_corpse.Location;
            }

            pack.DropItem( m_corpse );
            m_corpse.OnDoubleClick( m_mob );
            m_mob.Squelched = false;
            m_mob.Emote( "*slowly comes to*" );
            m_mob.Animate( 21, 6, 1, false, false, 0 );
            m_mob.SendMessage( "You will be invulnerable for a few seconds to be able to escape from creatures that might instantly kill you again. Additionally, you will be weakened for some minutes." );
            
            foreach ( Item item in m_corpse.Items )
                list.Add( item );

            if( list.Count > 0 )
            {
                for( int i = 0; i < list.Count; ++i )
                {
                    Item item = (Item)list[i];
                    pack.AddItem( item );
                }
            }
            
            m_corpse.Delete();
            
            if( m_mob is PlayerMobile )
            {
            	PlayerMobile m = m_mob as PlayerMobile;

                if (HealthAttachment.HasHealthAttachment(m))
                {
                    HealthAttachment.GetHA(m).TryInjure(m.LastKiller);
                }
            	
            	m.m_InvulTimer = new InvulTimer( m );
            	m.m_InvulTimer.Start();
            	m.m_DeathTimer = null;

            	XmlAttachment hitsatt = XmlAttach.FindAttachment( m, typeof( XmlHits ) );
            	XmlAttachment stamatt = XmlAttach.FindAttachment( m, typeof( XmlStam ) );
            	XmlAttachment manaatt = XmlAttach.FindAttachment( m, typeof( XmlMana ) );
            	
            	if( hitsatt != null )
            		hitsatt.Delete();
            	if( stamatt != null )
            		stamatt.Delete();
            	if( manaatt != null )
            		manaatt.Delete();
            		
            	XmlAttach.AttachTo( m, new XmlHits( ( 0 - ( m.RawHits / 4 ) ), 300.0 ) );
            	XmlAttach.AttachTo( m, new XmlStam( ( 0 - ( m.RawStam / 4 ) ), 300.0 ) );
            	XmlAttach.AttachTo( m, new XmlMana( ( 0 - ( m.RawMana / 4 ) ), 300.0 ) );

            	m.m_KOPenalty = new KOTimer( m );
            	m.m_KOPenalty.Start();


            }
        }
    }
}
