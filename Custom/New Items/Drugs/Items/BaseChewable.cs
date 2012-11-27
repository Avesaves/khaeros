using System;
using System.Collections;
using Server.Multis;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public enum Chewable
    {
        Qat = 0
    }
    /* 	
        BaseSnortable that does not implement content reduction. Basis for pipes and (stackable) stalks of weed
        Override OnSnort to handle content reduction and possibly item deletion
    */
    public abstract class BaseChewable : Item
    {
        private int m_ChewableRemaining;
        private Chewable m_Chewable;

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int ChewableRemaining
        {
            get { return m_ChewableRemaining; }
            set { m_ChewableRemaining = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual Chewable Chewable
        {
            get { return m_Chewable; }
            set { m_Chewable = value; }
        }

        [Constructable]
        public BaseChewable(int itemID, int chewableTotal)
            : base(itemID)
        {
            m_ChewableRemaining = chewableTotal;
        }

        public BaseChewable(Serial serial)
            : base(serial)
        {
        }

        public virtual void OnChew(Mobile from)
        {
            if (m_Chewable == Chewable.Qat)
                ChewTimer.BeginChew(from as PlayerMobile, 15);

            from.Emote("*chews*");            

            int chewSound = Utility.RandomMinMax(58, 60);
            from.PlaySound(chewSound);

            from.SendMessage("You feel a narcotic rush.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (RootParent == from && from is PlayerMobile)
            {
                if (m_ChewableRemaining > 0)
                    OnChew(from);
                else
                    from.SendMessage("There's nothing left to chew.");
            }
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        public override bool StackWith(Mobile from, Item dropped)
        {
            if (dropped is BaseChewable && ((BaseChewable)dropped).ChewableRemaining == ChewableRemaining)
                return base.StackWith(from, dropped);

            else
                return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write((int)m_Chewable);
            writer.Write((int)m_ChewableRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        m_Chewable = (Chewable)reader.ReadInt();
                        goto case 0;
                    }

                case 0:
                    {
                        m_ChewableRemaining = reader.ReadInt();
                        break;
                    }
            }
        }
    }

    public class ChewTimer : Timer
    {
        private static Hashtable m_Table = new Hashtable();

        public static bool IsChewing(PlayerMobile m)
        {
            return m_Table.Contains(m);
        }

        public static void BeginChew(PlayerMobile m, int duration)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
                t.Stop();

            t = new ChewTimer(m, duration);

            m_Table[m] = t;

            t.Start();
        }

        public static void EndChew(PlayerMobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
                t.Stop();

            m_Table.Remove(m);
            m.SendMessage("You spit the chew out.");
            m.Emote("*spits!*");

            if (m.Female)
            {
                m.PlaySound(820);
            }
            else
            {
                m.PlaySound(1094);
            }
        }

        private PlayerMobile m_Chewer;
        private int m_Duration;

        //Timespan between chews
        public ChewTimer(PlayerMobile from, int duration)
            : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            Priority = TimerPriority.OneSecond;
            m_Chewer = from;
            m_Duration = duration;
        }

        protected override void OnTick()
        {
            m_Duration -= 1;
            if (m_Duration <= 0)
            {
                EndChew(m_Chewer);
                return;
            }

            m_Chewer.Emote("*chews*");
            int chewSound = Utility.RandomMinMax(58, 60);
            m_Chewer.PlaySound(chewSound);

            //this.Interval = this.Delay = TimeSpan.FromSeconds(1);
            this.Interval = this.Delay = TimeSpan.FromSeconds(15);
        }
    }
}
