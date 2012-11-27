using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBackground : XmlAttachment
    {
        private BackgroundList m_Background;
        private int m_Level = 0;

        public BackgroundList Background { get { return m_Background; } set { m_Background = value; } }
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [Attachable]
        public XmlBackground(BackgroundList b, int addlevel)
        {
            m_Background = b;
            m_Level = addlevel;

            if (m_Level < 0)
                Name = (m_Background.ToString() + " " + m_Level.ToString());
            else
                Name = (m_Background.ToString() + " +" + m_Level.ToString());
        }

        [Attachable]
        public XmlBackground(BackgroundList b, int addlevel, int duration)
        {
            m_Background = b;
            m_Level = addlevel;
            Expiration = TimeSpan.FromMinutes(duration);

            if (m_Level < 0)
                Name = (m_Background.ToString() + " " + m_Level.ToString());
            else
                Name = (m_Background.ToString() + " +" + m_Level.ToString());
        }

        public static int GetLevel(Mobile m, BackgroundList bg)
        {
            int bLevel = 0;

            if (m == null)
                return 0;

            ArrayList backgroundAttachments = XmlAttach.FindAttachments(m, typeof(XmlBackground));

            if (backgroundAttachments != null)
            {
                foreach (XmlBackground att in backgroundAttachments)
                {
                    if (att.Background == bg)
                        bLevel += att.Level;
                }
            }

            if (m.Items != null)
            {
                foreach (Item item in m.Items)
                {
                    ArrayList itemAtts = XmlAttach.FindAttachments(item, typeof(XmlBackground));
                    if (itemAtts != null)
                    {
                        foreach (XmlBackground iAtt in itemAtts)
                        {
                            if (iAtt.Background == bg)
                                bLevel += iAtt.Level;
                        }
                    }
                }
            }

            return bLevel;
        }

        public static void CleanUp(PlayerMobile m, BackgroundList ListName)
        {
                List<XmlBackground> removeAtt = new List<XmlBackground>();
                foreach (XmlBackground att in XmlAttach.FindAttachments(m, typeof(XmlBackground)))
                {
                    if (att.Background == ListName)
                        removeAtt.Add(att);
                }
                for (int i = removeAtt.Count - 1; i > -1; i--)
                {
                    removeAtt[i].Delete();
                }
        }

        public static void CleanUp(PlayerMobile m)
        {
            List<XmlBackground> removeAtt = new List<XmlBackground>();
            foreach (XmlBackground bAtt in XmlAttach.FindAttachments(m, typeof(XmlBackground)))
                removeAtt.Add(bAtt);
            for (int i = removeAtt.Count - 1; i > -1; i--)
                removeAtt[i].Delete();
        }

        public XmlBackground(ASerial serial)
            : base(serial)
        {

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Background = (BackgroundList)reader.ReadInt();
            m_Level = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); //version
            writer.Write((int)m_Background);
            writer.Write((int)m_Level);
        }
    }
}
