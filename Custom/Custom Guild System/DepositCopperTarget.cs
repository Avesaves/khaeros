using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;

namespace Server.Misc
{		
	public class DepositCopperTarget : Target
    {
		private CustomGuildStone g;
		
        public DepositCopperTarget( PlayerMobile m, CustomGuildStone guild )
            : base( 8, false, TargetFlags.None )
        {
        	g = guild;
        	m.SendMessage( "Target a pile of copper in your backpack to deposit it into your organizaiton's treasury." );
        }

        protected override void OnTarget( Mobile m, object obj )
        {
        	if( m == null || m.Deleted || g == null || g.Deleted || !(m is PlayerMobile) )
        		return;
        	
        	if( !m.InRange(g, 3) )
        	{
				m.SendMessage( "You need to be near your organization's stone in order to do that." );
				return;
			}
        	
        	Copper copper = obj as Copper;
        	PlayerMobile from = m as PlayerMobile;
        	
        	if( obj is Copper && copper.RootParentEntity == from )
        	{
        		if( !CustomGuildStone.IsGuildMember(from, g) || !g.HasTreasury(from, true) )
					return;
			
				from.CustomGuilds[g].Balance += copper.Amount;
				((BaseContainer)g.Treasury).DropAndStack( copper );
				from.SendMessage( "You have deposited some copper into your organization's treasury. Your current balance is now: " +
				              from.CustomGuilds[g].Balance.ToString() + " copper." );
        	}
            
        	else
            	m.SendMessage( "Invalid target." );
    	}
    }
}
