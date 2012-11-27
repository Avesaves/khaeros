using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Misc;
using Server.Targeting;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBlinkEffect : XmlAttachment
    {
        private DateTime m_NextUseAllowed;

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime NextUseAllowed { get { return m_NextUseAllowed; } set { m_NextUseAllowed = value; } }

        public XmlBlinkEffect( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlBlinkEffect()
        {
        }

        public override void OnActivatedBy( Mobile mob )
        {
            if( DateTime.Now < NextUseAllowed )
                mob.SendMessage( "It's too early to use this ability again." );

            else
                mob.Target = new BlinkTarget( this );

            base.OnActivatedBy( mob );
        }

        private class BlinkTarget : Target
        {
            private XmlBlinkEffect m_blink;

            public BlinkTarget( XmlBlinkEffect blink )
                : base( 15, true, TargetFlags.None )
            {
                m_blink = blink;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if( m == null || obj == null )
                    return;

                if( obj is Mobile )
                {
                    Mobile target = obj as Mobile;

                    if( target.Alive && !target.Blessed && m.CanSee( target ) )
                    {
                        Map map = target.Map;
                        bool validLocation = false;
                        Point3D loc = target.Location;

                        for( int j = 0; !validLocation && j < 10; ++j )
                        {
                            int x = target.X;
                            int y = target.Y;

                            if( Utility.RandomBool() )
                                x--;
                            else
                                x++;
                            if( Utility.RandomBool() )
                                y--;
                            else
                                y++;

                            int z = map.GetAverageZ( x, y );

                            if( validLocation = map.CanFit( x, y, target.Z, 16, false, false ) )
                                loc = new Point3D( x, y, target.Z );
                            else if( validLocation = map.CanFit( x, y, z, 16, false, false ) )
                                loc = new Point3D( x, y, z );
                        }

                        m.Location = loc;
                        m.Warmode = true;
                        m.Combatant = target;
                        m.PlaySound( 1154 );

                        if( m_blink != null )
                            m_blink.NextUseAllowed = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

                        XmlAttach.AttachTo( m, new XmlSolidHueMod( 12345678, 2.5 ) );
                        return;
                    }
                }

                if( obj is IPoint3D )
                {
                    Point3D loc =  new Point3D( ( (IPoint3D)obj ).X, ( (IPoint3D)obj ).Y, ( (IPoint3D)obj ).Z );

                    if( m.InLOS( loc ) )
                    {
                        m.Location = loc;
                        XmlAttach.AttachTo( m, new XmlSolidHueMod( 12345678, 2.5 ) );
                        m.PlaySound( 1154 );

                        if( m_blink != null )
                            m_blink.NextUseAllowed = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

                        return;
                    }
                }

                m.SendMessage( "Invalid target." );
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
