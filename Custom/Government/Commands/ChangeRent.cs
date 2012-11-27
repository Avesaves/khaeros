using System;
using Server.Mobiles;
using Server.Network;
using Knives.TownHouses;
using Server.Items;
using Server.Targeting;
using Server.Multis;

namespace Server.Commands
{
    public class ChangeRent
    {
        public static void Initialize()
        {
            CommandSystem.Register( "ChangeRent", AccessLevel.Player, new CommandEventHandler( ChangeRent_OnCommand ) );
        }

        [Usage( "ChangeRent" )]
        [Description( "Allows the Owner or Leader of the organization that controls the rent of a house to change its price." )]
        private static void ChangeRent_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            int price = 0;

            if( e.Arguments.Length < 1 || String.IsNullOrEmpty( e.Arguments[0] ) || !int.TryParse( e.Arguments[0], out price ) )
            {
                m.SendMessage( "This command requires an argument to regulate the price. Example: \".ChangeRent 1250\", to change a house's rent to 1250." );
                return;
            }

            m.Target = new ChangeRentTarget( price );
        }

        private class ChangeRentTarget : Target
        {
            private int m_newPrice;

            public ChangeRentTarget( int price )
                : base( 10, false, TargetFlags.None )
            {
                m_newPrice = price;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if( from == null || targeted == null || !( from is PlayerMobile ) || targeted == null )
                    return;

                TownHouseSign townsign = null;

                if( targeted is HouseSign && ( (HouseSign) targeted ).TownHouseSign == null )
                    from.SendMessage( "This sign is bugged. Please post its location on the bug forums." );

                else if( targeted is HouseSign && ( (HouseSign)targeted ).TownHouseSign != null )
                    townsign = ( (HouseSign)targeted ).TownHouseSign;

                else if( targeted is TownHouseSign )
                    townsign = targeted as TownHouseSign;

                if( townsign != null )
                {
                    if( townsign.Treasury == null )
                        from.SendMessage( "This house is not part of any city." );

                    else if( ( (Treasury)townsign.Treasury ).ControllingGuild == null )
                        from.SendMessage( "The city to which this house belongs is not currently controlled by any organization." );

                    else if( !CustomGuildStone.IsGuildLeader( (PlayerMobile)from, ( (Treasury)townsign.Treasury ).ControllingGuild ) )
                        from.SendMessage( "Only the leader or owner of the organization controlling this city can change the rent of this house." );

                    else
                    {
                        from.SendMessage( "New rent set." );
                        townsign.Price = m_newPrice;
                        townsign.InvalidateProperties();

                        if( townsign.House != null && townsign.House.Sign != null )
                            townsign.House.Sign.InvalidateProperties();
                    }
                }
            }
        }
    }
}
