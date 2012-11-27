using System;
using Server;
using Server.Gumps;
using Server.Engines.Craft;
using Server.Mobiles;

namespace Server.Items
{
    public class BlackSmithingHammer: BaseTool
    {
        public override CraftSystem CraftSystem { get { return DefBlacksmithy.CraftSystem; } } // not used

        [Constructable]
        public BlackSmithingHammer()
            : base(0x13E3)
        {
            Name = "Weapon Smith Hammer";
            Weight = 8.0;
            Layer = Layer.OneHanded;
        }

        [Constructable]
        public BlackSmithingHammer(int uses)
            : base(uses, 0x13E3)
        {
            Name = "Smith Hammer";
            Weight = 8.0;
            Layer = Layer.OneHanded;
        }

        public BlackSmithingHammer(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckAnvilAndForge(from))
                from.SendGump(new BStartGump((PlayerMobile)from, new BlackSmithingCraftState(from, this), this, null));
            else
                from.SendLocalizedMessage(1044267); // You must be near an anvil and a forge to smith items.
        }

        private static Type typeofAnvil = typeof(AnvilAttribute);
        private static Type typeofForge = typeof(ForgeAttribute);

        public static bool CheckAnvilAndForge(Mobile from)
        {
            bool anvil = false;
            bool forge = false;
            int range = 3;

            Map map = from.Map;

            if (map == null)
                return false;

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, range);

            foreach (Item item in eable)
            {
                Type type = item.GetType();

                bool isAnvil = (type.IsDefined(typeofAnvil, false) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6);
                bool isForge = (type.IsDefined(typeofForge, false) || item.ItemID == 4017 || (item.ItemID >= 6522 && item.ItemID <= 6569) || item.ItemID == 0x2DD8);

                if (isAnvil || isForge)
                {
                    if ((from.Z + 16) < item.Z || (item.Z + 16) < from.Z || !from.InLOS(item))
                        continue;

                    anvil = anvil || isAnvil;
                    forge = forge || isForge;

                    if (anvil && forge)
                        break;
                }
            }

            eable.Free();

            for (int x = -range; (!anvil || !forge) && x <= range; ++x)
            {
                for (int y = -range; (!anvil || !forge) && y <= range; ++y)
                {
                    Tile[] tiles = map.Tiles.GetStaticTiles(from.X + x, from.Y + y, true);

                    for (int i = 0; (!anvil || !forge) && i < tiles.Length; ++i)
                    {
                        int id = tiles[i].ID & 0x3FFF;

                        bool isAnvil = (id == 4015 || id == 4016 || id == 0x2DD5 || id == 0x2DD6);
                        bool isForge = (id == 4017 || (id >= 6522 && id <= 6569) || id == 0x2DD8);

                        if (isAnvil || isForge)
                        {
                            if ((from.Z + 16) < tiles[i].Z || (tiles[i].Z + 16) < from.Z || !from.InLOS(new Point3D(from.X + x, from.Y + y, tiles[i].Z + (tiles[i].Height / 2) + 1)))
                                continue;

                            anvil = anvil || isAnvil;
                            forge = forge || isForge;
                        }
                    }
                }
            }

            if (anvil && forge)
                return true;
            else
                return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
