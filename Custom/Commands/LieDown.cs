using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Prompts;
using Server.Engines.XmlSpawner2;

namespace Server.Commands
{
    public class LieDown
    {
        public static void Initialize()
        {
            CommandSystem.Register( "LieDown", AccessLevel.Player, new CommandEventHandler( LieDown_OnCommand ) );
        }

        [Usage( "LieDown" )]
        [Description( "Allows you to lie down or to get up." )]
        private static void LieDown_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m );

            if( m.TrippedTimer != null )
            {
                m.TrippedTimer.Stop();
                m.TrippedTimer.Interval = TimeSpan.FromSeconds( 0.1 );
                m.TrippedTimer.Delay = TimeSpan.FromSeconds( 0.1 );
                ( (Misc.Unhorse.UnhorseTimer)m.TrippedTimer ).m_Repeat = false;
                ( (Misc.Unhorse.UnhorseTimer)m.TrippedTimer ).m_Stage = 1;
                m.TrippedTimer.Start();
            }

            else
            {
                csa.DoTrip( 3 );
                ( (Misc.Unhorse.UnhorseTimer)m.TrippedTimer ).m_Repeat = true;
            }
        }
    }
}
