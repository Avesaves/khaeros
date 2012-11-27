using System;
using Server;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Gumps;
using Server.Multis;
using Server.Engines.Help;
using Server.ContextMenus;

namespace Server.Gumps
{
    public class BlindnessGump : Gump
    {
        public BlindnessGump()
            : base( 0, 0 )
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = false;
            this.Resizable = false;
            this.AddPage( 0 );
            this.AddImageTiled( 0, 0, 1280, 1024, 2702 );
        }
    }
}
