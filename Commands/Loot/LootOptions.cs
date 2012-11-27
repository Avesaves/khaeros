using System.Collections.Generic;
using Server.Items;

namespace Server
{
    public class LootOptions
    {
        private Dictionary<LootFlag, Container> _containerTable;
        private LootFlag _flags;
        private Mobile _mobile;

        public Dictionary<LootFlag, Container> ContainerTable { get { return _containerTable; } protected set { _containerTable = value; } }
        public LootFlag Flags { get { return _flags; } protected set { _flags = value; } }
        public Mobile Mobile { get { return _mobile; } protected set { _mobile = value; } }

        public LootOptions(Mobile m) : this(m, LootFlag.Coins) { }

        public LootOptions(Mobile m, LootFlag flags)
        {
            _containerTable = new Dictionary<LootFlag, Container>();
            _flags = flags;
            _mobile = m;
        }

        #region +bool IsLootable( Item )
        public bool IsLootable(Item i)
        {

            if (i is Gold && GetFlag(LootFlag.Coins) || i is Silver && GetFlag(LootFlag.Coins) || i is Copper && GetFlag(LootFlag.Coins) || i is RewardToken && GetFlag(LootFlag.Coins))
                return true;

            return false;
        }
        #endregion

        #region +bool IsToken( Item )
        public bool IsToken(Item i)
        {

            if (i is RewardToken)
                return true;

            return false;
        }
        #endregion

        #region GrabFlag operations
        public bool GetFlag(LootFlag flag)
        {
            return ((Flags & flag) != 0);
        }

        public void SetFlag(LootFlag flag, bool value)
        {
            if (value)
            {
                Flags |= flag;
            }
            else
            {
                Flags &= flag;
            }
        }

        public void ResetFlags()
        {
            Flags = 0;
        }
        #endregion

        #region PlacementContainer operations
        public Container GetPlacementContainer(LootFlag flag)
        {
            if (ContainerTable.ContainsKey(flag))
                return ContainerTable[flag];

            return null;
        }

        public void SetPlacementContainer(LootFlag flag, Container cont)
        {
            ContainerTable[flag] = cont;
        }
        #endregion

        public virtual void Serialize(BinaryFileWriter writer)
        {
            writer.Write((int)0);

            //version 0
            writer.Write((int)ContainerTable.Count);

            if (ContainerTable.Count > 0)
            {
                foreach (KeyValuePair<LootFlag, Container> kvp in ContainerTable)
                {
                    writer.Write((int)kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            writer.Write((int)Flags);
            writer.Write(Mobile);
        }

        public LootOptions(BinaryFileReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        int tblCount = reader.ReadInt();
                        ContainerTable = new Dictionary<LootFlag, Container>(tblCount);

                        for (int i = 0; i < tblCount; i++)
                        {
                            ContainerTable.Add((LootFlag)reader.ReadInt(), (Container)reader.ReadItem());
                        }

                        Flags = (LootFlag)reader.ReadInt();
                        Mobile = reader.ReadMobile();

                        break;
                    }
            }
        }
    }
}