using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Misc;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSpiritSummoning : XmlAttachment
    {
        private Mobile m_Summoned;
        private int m_ManaCost = 50;
        private TimeSpan m_CoolDown = TimeSpan.FromMinutes( 5.0 );
        private DateTime m_NextUseAllowed;

        public Mobile Summoned 
        { 
            get 
            {
                if( m_Summoned != null && (!m_Summoned.Alive || m_Summoned.Deleted) )
                    m_Summoned = null;

                return m_Summoned; 
            } 

            set { m_Summoned = value; } 
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ManaCost { get { return m_ManaCost; } set { m_ManaCost = Math.Max( 0, value ); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public TimeSpan CoolDown { get { return m_CoolDown; } set { m_CoolDown = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextUseAllowed { get { return m_NextUseAllowed; } set { m_NextUseAllowed = value; } }

        public XmlSpiritSummoning( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlSpiritSummoning()
        {
        }

        public override void OnActivatedBy( Mobile mob )
        {
            if( DateTime.Now < NextUseAllowed )
                mob.SendMessage( "It's too early to use this ability again." );
                
            else if( Summoned != null )
                mob.SendMessage( "You already have a spirit under the effect of this spell." );

            else
                BaseCustomSpell.SpellInitiator( new SpiritSummoning( mob, 3, this ) );

            base.OnActivatedBy( mob );
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );

            writer.Write( (Mobile)m_Summoned );
            writer.Write( (int)m_ManaCost );
            writer.Write( (TimeSpan)m_CoolDown );
            writer.Write( (DateTime)m_NextUseAllowed );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_Summoned = reader.ReadMobile();
            m_ManaCost = reader.ReadInt();
            m_CoolDown = reader.ReadTimeSpan();
            m_NextUseAllowed = reader.ReadDateTime();
        }
    }
}
