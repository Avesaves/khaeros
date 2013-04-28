using System;
using Server;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class MagicStoneWall : BaseWall
    {
        private Mobile m_trapper;

        [Constructable]
        public MagicStoneWall(Mobile trapper) : base( 0x82 )
        {
            m_trapper = trapper;
            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(Expire));
        }



        private void Expire()
        {
            if (this == null || this.Deleted)
                return;

            else
            {
                if (m_trapper != null)
                    m_trapper.SendMessage("Your walls crumble...");

                this.Delete();
            }
        }


        public MagicStoneWall(Serial serial)
            : base(serial)
        {
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

            if (ItemID == 0x3735)
                Expire();
        }
    }
}