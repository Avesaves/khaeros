using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using Server.Prompts;
using Server.Engines.XmlSpawner2;

namespace Server.Commands
{
    public class RollDice
    {
        public static void Initialize()
        {
            CommandSystem.Register( "RollDice", AccessLevel.Player, new CommandEventHandler( RollDice_OnCommand ) );
        }

        [Usage( "RollDice" )]
        [Description( "Allows you to roll up to 10 dice of up to 100 sides." )]
        private static void RollDice_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            if( e.Arguments.Length > 0 && e.Arguments[0].ToLower().Contains( "d" ) )
            {
                string[] args = e.Arguments[0].Split( new char[] { 'd' } );

                if( args.Length > 1 )
                {
                    int numberOfDice = 0;
                    int numberOfSides = 0;

                    if( int.TryParse( args[0], out numberOfDice ) && int.TryParse( args[1], out numberOfSides ) &&
                        numberOfSides > 0 && numberOfDice > 0 && numberOfDice < 11 && numberOfSides < 101 )
                    {
                        List<int> results = new List<int>();
                        int total = 0;
                        StringBuilder sb = new StringBuilder();

                        for( int i = 0; i < numberOfDice; i++ )
                        {
                            int rolled = Utility.RandomMinMax( 1, numberOfSides );
                            results.Add( rolled );
                            total += rolled;

                            if( sb.Length > 0 )
                                sb.Append( ", " );

                            sb.Append( rolled.ToString() );
                        }

                        sb.Append( "." );

                        string plural = numberOfDice > 1 ? "s" : "";

                        foreach( NetState ns in e.Mobile.GetClientsInRange( 12 ) )
                            ns.Mobile.SendMessage( e.Mobile.Name + " rolled " + numberOfDice.ToString() + "d" + numberOfSides + plural + 
                                ". Result" + plural + ": " + sb.ToString() + " Total: " + total.ToString() + "." );

                        return;
                    }
                }
            }

            e.Mobile.SendMessage( "This command requires one argument for the number of dice to roll and the amount of sides on the dice." );
            e.Mobile.SendMessage( "For example, to roll 1 die with 6 sides, use: \".RollDice 1d6\"." );
        }
    }
}
