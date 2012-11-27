using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAosAttribute : XmlAttachment
    {
        private AosAttribute m_Attribute;
        private int m_Value;

        public AosAttribute Attribute { get { return m_Attribute; } set { m_Attribute = value; } }
        public int Value { get { return m_Value; } set { m_Value = value; } }

        public XmlAosAttribute(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlAosAttribute(AosAttribute attr, int value)
        {
            Attribute = attr;
            m_Value = value;
            Name = Attribute.ToString() + " " + (Value <= 0 == true ? ("+" + Value.ToString()) : (Value.ToString()));
        }

        [Attachable]
        public XmlAosAttribute(AosAttribute attr, int value, int duration)
        {
            Attribute = attr;
            m_Value = value;
            Expiration = TimeSpan.FromMinutes(duration);
            Name = Attribute.ToString() + " " + (Value >= 0 ? ("+" + Value.ToString()) : (Value.ToString()));
        }

        public static void CleanUp(Mobile m)
        {
            List<XmlAosAttribute> removeAtt = new List<XmlAosAttribute>();
            foreach (XmlAosAttribute bAtt in XmlAttach.FindAttachments(m, typeof(XmlAosAttribute)))
                removeAtt.Add(bAtt);
            for (int i = removeAtt.Count - 1; i > -1; i--)
                removeAtt[i].Delete();
        }

        public override void OnAttach()
        {
            if (!(AttachedTo is Mobile))
                Delete();

            base.OnAttach();
        }

        public static int GetValue(Mobile m, AosAttribute attr)
        {
            if (m == null || m.Deleted)
                return 0;

            ArrayList attachments = XmlAttach.FindAttachments(m, typeof(XmlAosAttribute));
            int value = 0;

            if (attachments == null)
                return 0;

            if (attachments.Count > 0)
            {
                foreach (XmlAosAttribute att in attachments)
                {
                    if (att.Attribute == attr)
                        value += att.Value;
                }
            }
            return value;
        }
    }
}