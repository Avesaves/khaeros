using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class MilitaryWayPoint : WayPoint
    {
        private GovernmentEntity m_Government;

        public GovernmentEntity Government { get { return m_Government; } set { m_Government = value; } }

        public override string DefaultName
        {
            get { return ("A Waypoint of " + m_Government.Name); }
        }

        public MilitaryWayPoint( GovernmentEntity government ) : base()
        {
            m_Government = government;
            Movable = false;
            government.WayPoints.Add( this );
        }

        public virtual bool CanSeeMe( PlayerMobile m )
        {
            if( Government != null && !Government.Deleted && CustomGuildStone.IsGuildMilitary(m,Government))
                return true;

            return m.AccessLevel > AccessLevel.Player;
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( from is PlayerMobile && Government != null && !Government.Deleted && 
                CustomGuildStone.IsGuildMilitary( (PlayerMobile)from, Government) )
            {
                from.SendMessage( "Target the next way point in the sequence." );

                from.Target = new NextPointTarget( this );
            }

            else
                base.OnDoubleClick( from );
        }

        public override void Delete()
        {
            if (Government != null && !Government.Deleted && Government.WayPoints.Contains(this))
                Government.WayPoints.Remove(this);
            
            base.Delete();
        }

        public override void OnDelete()
        {
            if( Government != null && !Government.Deleted && Government.WayPoints.Contains( this ) )
                Government.WayPoints.Remove( this );

            base.OnDelete();
        }

        public MilitaryWayPoint( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
            writer.Write( (GovernmentEntity)m_Government );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
            m_Government = (GovernmentEntity)reader.ReadItem();

            if (m_Government == null || m_Government.Deleted)
                Delete();
        }
    }
}
