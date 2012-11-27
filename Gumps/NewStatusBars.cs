using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
    public class NewStatusBarsGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("NewStatus", AccessLevel.Player, new CommandEventHandler(NewStatus_OnCommand));
        }

        [Usage("NewStatus")]
        [Description("Opens the new status bars gump.")]
        public static void NewStatus_OnCommand(CommandEventArgs e)
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        	   return;
        	   
            PlayerMobile caller = e.Mobile as PlayerMobile;

            if( caller.HasGump(typeof(NewStatusBarsGump)) )
                caller.CloseGump( typeof(NewStatusBarsGump) );
            
            caller.SendGump( new NewStatusBarsGump(caller) );
        }

        public NewStatusBarsGump( PlayerMobile from ) : base( 0, 0 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddImage(321, 231, 115);
			
			int id = 2054;
			
			if( from.Poisoned )
				id = 2056;
			
			if( from.Blessed )
				id = 2057;
			
			AddImageTiled(355, 242, GetSize(from.Hits, from.HitsMax), 11, id);
			AddImageTiled(355, 255, GetSize(from.Mana, from.ManaMax), 11, 2054);
			AddImageTiled(355, 268, GetSize(from.Stam, from.StamMax), 11, 2054);
        }
        
        public static int GetSize( double stat, double statmax )
        {
        	if( stat < 1.0 )
        		return 0;
        	
        	if( stat == statmax )
        		return 109;

        	double perc = 0;
        	
        	try{ perc = stat / (statmax / 109.0); }
        	catch{ return 0; }
        	
        	return (int)perc;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
        }
    }
}
