using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
    public class SealmakersTool : Item
    {
        public static bool SealExists(string proposal)
        {
            foreach (string seal in BaseJewel.Seals)
            {
                if (seal.ToLower().Trim() == proposal.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private string m_Seal = "None";
        public string Seal { get { return m_Seal; } set { m_Seal = value; } }

        [Constructable]
        public SealmakersTool()
            : base(Utility.RandomMinMax(11617, 11618))
        {
            Stackable = false;
            Weight = 4;
            Name = "A Sealmaker's Tool";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive )
        		return;
        	
        	Container pack = from.Backpack;

            if (pack != null && this.ParentEntity == pack)
            {
                if ( (from as PlayerMobile).Feats.GetFeatLevel(FeatList.Tinkering) > 2 )
                {
                    from.SendMessage(1423, "Current Seal: " + m_Seal);
                    from.SendMessage("Target a piece of jewelry to attach this seal; or, target this sealmaker's tool to set a new seal.");
                    from.Target = new SealmakersToolTarget(this);
                }
                else
                    from.SendMessage("You don't know enough about seals to do that.");
            }
            else { from.SendMessage("This must be in your backpack for you to use it."); }

            base.OnDoubleClick(from);
        }

        private class SealmakersToolTarget : Target
        {
            private SealmakersTool m_Tool;

            public SealmakersToolTarget(SealmakersTool tool)
                : base(1, true, TargetFlags.None)
            {
                m_Tool = tool;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive )
        		    return;

                if (m_Tool == null || m_Tool.Deleted)
                    return;
        	
        	    Container pack = from.Backpack;

                if (pack != null && m_Tool.ParentEntity == pack)
                {
                    if (targeted == m_Tool)
                    {
                        from.SendMessage("Enter the new seal's name.");
                        from.Prompt = new SetSeal(m_Tool);
                    }
                    else if (targeted is BaseJewel && String.IsNullOrEmpty((targeted as BaseJewel).Seal))
                    {
                        (targeted as BaseJewel).Seal = m_Tool.Seal;
                        from.SendMessage("You have successfully placed " + m_Tool.Seal + " on that jewelry.");
                    }
                    else if (targeted is BaseJewel && !String.IsNullOrEmpty((targeted as BaseJewel).Seal))
                    {
                        from.SendMessage("That already has a seal on it.");
                    }
                    else
                    {
                        from.SendMessage("Target the sealmaker's tool to reset the seal or a piece of jewelry to give that jewelry the seal.");
                    }
                }
                else
                    from.SendMessage("That must be in your backpack to use it.");

                base.OnTarget(from, targeted);
            }
        }

        public SealmakersTool(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((string)m_Seal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_Seal = reader.ReadString();
                        break;
                    }
            }
        }

        private class SetSeal : Prompt
        {
            private SealmakersTool m_Tool;

            public SetSeal(SealmakersTool tool)
            {
                m_Tool = tool;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (!String.IsNullOrEmpty(text))
                {
                    if (SealExists(text) && m_Tool.Seal != text)
                    {
                        from.SendMessage("You cannot re-create that seal without a duplicate.");
                    }
                    else
                    {
                        if (!BaseJewel.Seals.Contains(text))
                            BaseJewel.Seals.Add(text);

                        m_Tool.Seal = text;
                        from.PlaySound(0x241);
                        from.SendMessage("Seal Successfully Changed To: " + m_Tool.Seal);
                    }
                }
                else
                {
                    from.SendMessage(1423, "Current Seal: " + m_Tool.Seal);
                }

                base.OnResponse(from, text);
            }
        }
    }

    
}