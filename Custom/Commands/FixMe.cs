using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
    public class FixMe
    {
        public static void Initialize()
        {
            CommandSystem.Register( "FixMe", AccessLevel.Player, new CommandEventHandler( FixMe_OnCommand ) );
        }

        [Usage( "FixMe" )]
        [Description( "Forces the server to resend all draw info to your client." )]
        private static void FixMe_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.NextAllowedFixMe > DateTime.Now )
                return;

            m.SendEverything();
            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
            m.InvalidateProperties();
            m.NextAllowedFixMe = DateTime.Now + TimeSpan.FromMinutes( 1.0 );
        }
    }
}
