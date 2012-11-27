using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    [DynamicFliping]
    [FlipableAttribute(0xe80, 0x9a8)]
    public class AlmsBox : LockableContainer
    {
        public override int MaxWeight { get { return 10000; } }
        public override int DefaultGumpID { get { return 0x4B; } }

        private Timer m_bonusTimer;

        [Constructable]
        public AlmsBox()
            : base(0x09a8)
        {
            Name = "An Alms Box";
            LockLevel = 250;
            MaxLockLevel = 250;
            RequiredSkill = 250;
            MaxItems = 999999;
            Weight = 200;
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            TrapableContainer thisBox = this as TrapableContainer;

            if (from.AccessLevel < AccessLevel.GameMaster && this.Locked)
            {
                if ((dropped is Copper) || (dropped is Silver) || (dropped is Gold))
                {
                    int bonusMessage = Utility.Random(2);
                    switch (bonusMessage)
                    {
                        case 0: from.SendMessage("You believe the gods will guide you for your charity."); break;
                        case 1: from.SendMessage("You believe the gods will protect you for your charity."); break;
                    }
                    this.DropItem(dropped);
                    return true;
                }
                else
                {
                    from.SendMessage("That will not fit into the alms box.");
                    return false;
                }
            }
            else
            {
                from.SendLocalizedMessage(501747); // It appears to be locked.
                return false;
            }

            return true;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            TrapableContainer thisBox = this as TrapableContainer;

            if (from.AccessLevel < AccessLevel.GameMaster && this.Locked && !((from is PlayerMobile && ((PlayerMobile)from).BreakLock)))
            {
                if ((item is Copper) || (item is Silver) || (item is Gold))
                {
                    int bonusMessage = Utility.Random(2);
                    switch (bonusMessage)
                    {
                        case 0: from.SendMessage("You believe the gods will guide you for your charity."); break;
                        case 1: from.SendMessage("You believe the gods will protect you for your charity."); break;
                    }
                    this.DropItem(item);
                    return true;
                }
                else
                {
                    from.SendMessage("That will not fit into the alms box.");
                }
            }
            else
            {                    
                from.SendLocalizedMessage(501747); // It appears to be locked.
                return false;
            }

            return true;
        }

        public AlmsBox(Serial serial)
            : base(serial)
        {
            m_bonusTimer = null;
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