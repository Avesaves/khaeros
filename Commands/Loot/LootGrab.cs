using System;
using System.Collections.Generic;
using Server.Items;
using System.IO;

namespace Server
{
    [Flags]
    public enum LootFlag
    {
        Coins = 0x0001
    }

    public class LootGrab
    {
        internal static string PersistencePath = "Saves\\Loot";
        internal static string PersistenceFile = Path.Combine(PersistencePath, "Loot.bin");

        private static Dictionary<Mobile, LootOptions> _optTable = new Dictionary<Mobile, LootOptions>();

        public static Dictionary<Mobile, LootOptions> OptionsTable { get { return _optTable; } }

        public static void Configure()
        {
            EventSink.WorldLoad += new WorldLoadEventHandler(event_worldLoad);
            EventSink.WorldSave += new WorldSaveEventHandler(event_worldSave);
        }

        #region +static string GetContainerName( Mobile, GrabFlag )
        public static string GetContainerName(Mobile m, LootFlag flag)
        {
            LootOptions options = GetOptions(m);
            Container cont = options.GetPlacementContainer(flag);
            string res = "(not set)";

            if (cont == null || !cont.IsChildOf(m.Backpack))
                res = "(not set)";
            else if (cont == m.Backpack)
                res = "(main backpack)";
            else if (!String.IsNullOrEmpty(cont.Name))
                res = cont.Name;
            else
                res = cont.ItemData.Name;

            return res;
        }
        #endregion

        #region +static GrabFlag ParseInt32( int )
        public static LootFlag ParseInt32(int num)
        {
            LootFlag flag = (LootFlag)0;

            switch (num)
            {
                case 1: flag = LootFlag.Coins; break;
            }

            return flag;
        }
        #endregion

        #region +static GrabFlag ParseType( Item )
        public static LootFlag ParseType(Item i)
        {
            LootFlag flag = LootFlag.Coins;

            if (i is Gold || i is Silver || i is Copper || i is RewardToken)
                flag = LootFlag.Coins;
            return flag;
        }
        #endregion

        #region LootOptions operations
        public static LootOptions GetOptions(Mobile m)
        {
            if (!OptionsTable.ContainsKey(m) || OptionsTable[m] == null)
                OptionsTable[m] = new LootOptions(m);

            return OptionsTable[m];
        }

        public static void SaveOptions(Mobile m, LootOptions options)
        {
            OptionsTable[m] = options;
        }
        #endregion

        #region persistence
        private static void event_worldLoad()
        {
            if (!File.Exists(PersistenceFile))
                return;

            using (FileStream stream = new FileStream(PersistenceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFileReader reader = new BinaryFileReader(new BinaryReader(stream));

                int count = reader.ReadInt();

                for (int i = 0; i < count; i++)
                {
                    int serial = reader.ReadInt();

                    if (serial > -1)
                    {
                        Mobile m = World.FindMobile(serial);
                        LootOptions options = new LootOptions(reader);

                        if (m != null && !m.Deleted)
                        {
                            if (options != null && !OptionsTable.ContainsKey(m))
                                OptionsTable.Add(m, options);
                        }
                    }
                }

                reader.Close();
            }
        }

        private static void event_worldSave(WorldSaveEventArgs args)
        {
            if (!Directory.Exists(PersistencePath))
                Directory.CreateDirectory(PersistencePath);

            BinaryFileWriter writer = new BinaryFileWriter(PersistenceFile, true);

            writer.Write(OptionsTable.Count);

            if (OptionsTable.Count > 0)
            {
                foreach (KeyValuePair<Mobile, LootOptions> kvp in OptionsTable)
                {
                    if (kvp.Key == null || kvp.Key.Deleted || kvp.Value == null)
                    {
                        writer.Write((int)-1);
                    }
                    else
                    {
                        writer.Write((int)kvp.Key.Serial);
                        kvp.Value.Serialize(writer);
                    }
                }
            }

            writer.Close();
        }
        #endregion
    }
}