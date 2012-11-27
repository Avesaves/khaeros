using System;
using Server.Items;

namespace Server.Gumps
{
    public class LootOptionsGump : Gump
    {
        internal int LabelHue = 0;

        public LootOptionsGump(Mobile m)
            : base(10, 10)
        {
            LootOptions options = LootGrab.GetOptions(m);

            AddPage(1);
            AddBackground(0, 0, 330, 200, 9270);
            AddBackground(12, 12, 308, 177, 3000);
            AddLabel(120, 15, LabelHue, "Loot Options");
            AddImageTiled(15, 35, 300, 4, 9151);

            //AddAlphaRegion( 15, 45, 160, 20 );
            AddLabel(15, 45, LabelHue, "Items Looted");
            //AddAlphaRegion( 180, 45, 135, 20 );
            AddLabel(180, 45, LabelHue, "Placement Container");

            string[] types = Enum.GetNames(typeof(LootFlag));

            for (int i = 0, y = 75; i < types.Length; i++, y += 25)
            {
                LootFlag flag = (LootFlag)Enum.Parse(typeof(LootFlag), types[i], true);

                AddCheck(15, y, 210, 211, options.GetFlag(flag), (i + 1));
                AddLabel(40, y, LabelHue, "Coins and Tokens");

                AddLabelCropped(185, y, 100, 20, LabelHue, LootGrab.GetContainerName(m, flag));
                AddButton(295, y, 9762, 9763, (i + 1), GumpButtonType.Reply, 0);
            }

            AddButton(15, 165, 4020, 4022, 100, GumpButtonType.Reply, 0);
            AddLabel(50, 165, LabelHue, "Cancel");
            AddButton(285, 165, 4023, 4025, 105, GumpButtonType.Reply, 0);
            AddLabel(190, 165, LabelHue, "Apply Changes");
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;
            LootOptions options = LootGrab.GetOptions(m);

            if (m == null || info.ButtonID <= 0 || info.ButtonID == 100)
                return;

            //store flags
            options.ResetFlags();

            if (info.Switches.Length > 0)
            {
                for (int i = 0; i < info.Switches.Length; i++)
                {
                    if (info.Switches[i] == 1)
                    {
                        options.SetFlag(LootFlag.Coins, true);
                        break;
                    }

                    options.SetFlag(LootGrab.ParseInt32(info.Switches[i]), true);
                }
            }
            else
            {
                options.SetFlag(LootFlag.Coins, true);
            }

            //handle buttons
            if (info.ButtonID == 105) //OK
            {
                LootGrab.SaveOptions(m, options);
                m.SendMessage("You have updated your loot options.");
            }
            else //placement container selection
            {
                m.SendMessage("Select the container to place your loot in.");
                m.BeginTarget(-1, false, Server.Targeting.TargetFlags.None, new TargetStateCallback(OnContainerSelect), info.ButtonID);
            }
        }

        private void OnContainerSelect(Mobile from, object target, object state)
        {
            if (target is Container)
            {
                Container cont = (Container)target;

                if (!cont.IsChildOf(from.Backpack) && cont != from.Backpack)
                {
                    from.SendMessage("You may only drop loot into containers within your pack.");
                }
                else
                {
                    LootOptions options = LootGrab.GetOptions(from);
                    LootFlag containerFlag = LootGrab.ParseInt32((int)state);

                    options.SetPlacementContainer(containerFlag, cont);

                    from.SendMessage("You have selected a new container for your coins and tokens.");
                }
            }
            else
            {
                from.SendMessage("Loot can only be dropped into containers.");
            }

            from.SendGump(new LootOptionsGump(from));
        }
    }
}