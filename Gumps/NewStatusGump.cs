using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using Server.Misc;
using Server.Items;

namespace Server.Gumps
{
    public class NewStatusGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register( "NewStatusGump", AccessLevel.Player, new CommandEventHandler(NewStatusGump_OnCommand) );
        }

        [Usage("NewStatusGump")]
        [Description("Opens the new status gump.")]
        public static void NewStatusGump_OnCommand(CommandEventArgs e)
        {
        	if( e.Mobile == null || e.Mobile.Deleted || !(e.Mobile is PlayerMobile) )
        		return;

        	PlayerMobile caller = e.Mobile as PlayerMobile;
        	
            if ( caller.HasGump(typeof(NewStatusGump)) )
                caller.CloseGump( typeof(NewStatusGump) );
            
            caller.SendGump( new NewStatusGump(caller) );
        }

        public NewStatusGump( PlayerMobile from ) : base( 0, 0 )
        {
        	if( from == null || from.Deleted )
        		return;
        	
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			int min = 0, max = 0, copper = 0, silver = 0, gold = 0;

			if ( from.Weapon != null )
				from.Weapon.GetStatusDamage( from, out min, out max );
			
			if( from.Backpack != null && !from.Backpack.Deleted )
				CountCoins( from.Backpack, ref copper, ref silver, ref gold );
			
			int cpcap = 175000;
			
			if( from.Advanced != Advanced.None )
				cpcap += 75000;
				
			AddPage(0);
			AddImage(298, 52, 114);
			AddLabel(371, 163, 0, from.Name);
			AddLabel(368, 181, 0, LevelSystem.SubclassName( from ));
			AddLabel(365, 199, 0, from.Nation.ToString());
			AddLabel(548, 181, 0, from.Level.ToString());
			AddLabel(578, 199, 0, from.Lives.ToString());
			AddLabel(556, 218, 0, from.Weight.ToString());
			AddLabel(462, 218, 0, from.Height.ToString());
			AddLabel(359, 218, 0, from.Age.ToString());
			AddLabel(385, 255, 0, from.Str.ToString());
			AddLabel(477, 255, 0, from.Dex.ToString());
			AddLabel(578, 255, 0, from.Int.ToString());
			AddLabel(362, 273, 0, from.HitsMax.ToString());
			AddLabel(471, 273, 0, from.StamMax.ToString());
			AddLabel(551, 273, 0, from.ManaMax.ToString());
			AddLabel(384, 310, 0, from.SlashingResistance.ToString());
			AddLabel(473, 310, 0, from.PiercingResistance.ToString());
			AddLabel(549, 310, 0, from.BluntResistance.ToString());
			AddLabel(360, 328, 0, from.FireResistance.ToString());
			AddLabel(425, 328, 0, from.ColdResistance.ToString());
			AddLabel(503, 328, 0, from.PoisonResistance.ToString());
			AddLabel(584, 328, 0, from.EnergyResistance.ToString());
			AddLabel(382, 365, 0, (min.ToString() + "-" + max.ToString()));
			AddLabel(437, 383, 0, ((Mobile.BodyWeight + from.TotalWeight).ToString() + "/" + from.MaxWeight.ToString()));
			AddLabel(377, 402, 0, copper.ToString());
			AddLabel(458, 402, 0, silver.ToString());
			AddLabel(543, 402, 0, gold.ToString());
			AddLabel(378, 420, 0, from.Hunger.ToString());
			AddLabel(462, 420, 0, from.Thirst.ToString());
			AddLabel(569, 420, 0, (from.Followers.ToString() + "/" + from.FollowersMax.ToString()));
			AddLabel(415, 438, 0, from.CP.ToString());
			AddLabel(429, 456, 0, from.CPSpent.ToString());
			AddLabel(420, 476, 0, (cpcap + from.CPCapOffset).ToString());
			AddImageTiled(442, 500, GetSize(from), 11, 116);
        }
        
        public static int GetSize( PlayerMobile m )
        {
        	int totalxpneeded = m.Level * 1000;
			int thislevelsxp = m.XP - ( m.NextLevel - ( m.Level * 1000 ) );
		
			double divisor = (double)totalxpneeded / 158.0;
			double offset = 0;
			
			if( divisor > 0.0 )
				offset = (double)thislevelsxp / divisor;
			
			if( m.Level > 49 )
				offset = 158;
        	
			return (int)offset;
        }
        
        public static void CountCoins( Container pack, ref int copper, ref int silver, ref int gold )
        {
        	foreach( Item item in pack.Items )
        	{
        		if( item is Copper )
        			copper += item.Amount;
        		
        		else if( item is Silver )
        			silver += item.Amount;
        		
        		else if( item is Gold )
        			gold += item.Amount;
        		
        		else if( item is Container )
        			CountCoins( ((Container)item), ref copper, ref silver, ref gold );
        	}
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
        }
    }
}
