using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.Items
{
    public class CustomPrayerStrip : Item, IEasyCraft
    {
        private CustomFaithSpell m_Prayer = new CustomFaithSpell();
        public CustomFaithSpell Prayer { get { return m_Prayer; } set { m_Prayer = value; } }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from is PlayerMobile && ((PlayerMobile)from).CanBeFaithful && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Faith) > 0)
                list.Add(new MenuEntry(this, (PlayerMobile)from));
        }

        private class MenuEntry : ContextMenuEntry
        {
            private CustomPrayerStrip m_Strip;
            private PlayerMobile m_From;

            public MenuEntry(CustomPrayerStrip strip, PlayerMobile from)
                : base(5101) // 3006132  2132
            {
                m_Strip = strip;
                m_From = from;
            }
            
            public override void OnClick()
            {
                if (m_From == null || m_Strip == null || m_From.Deleted || m_Strip.Deleted || !m_From.Alive || m_From.Paralyzed)
                    return;

                if (!m_Strip.IsChildOf(m_From.Backpack))
                {
                    m_From.SendMessage("That needs to be in your backpack for you to edit it.");
                    return;
                }

                if (m_From.Feats.GetFeatLevel(FeatList.Faith) > 0 && m_From.CanBeFaithful)
                    m_From.SendGump(new CustomPrayerStripGump(m_From, m_Strip));
            }
        }

        [Constructable]
        public CustomPrayerStrip()
            : base(Utility.RandomList( new int[] { 10305, 10304 } ))
        {
            Hue = 1045;
            Name = "A Prayer Strip";
            Stackable = false;
            
        }

        public CustomPrayerStrip(Serial serial)
            : base(serial)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
                return;

            if (from is PlayerMobile)
            {
                PlayerMobile pm = from as PlayerMobile;

                if (pm.CanBeFaithful && pm.Feats.GetFeatLevel(FeatList.Faith) > 0)
                {
                    if (Prayer.ManaCost > 0)
                        CastCustomFaithSpell(from, Prayer);
                    else
                        pm.SendMessage("This prayer is incomplete.");
                }
                else
                    pm.SendMessage("You do not understand this scroll.");
            }

            else
                from.SendMessage("This scroll appears blank.");

            base.OnDoubleClick(from);
        }

        public void CastCustomFaithSpell(Mobile m, CustomFaithSpell spell)
        {
            if (m is PlayerMobile)
            {
                if (!(m as PlayerMobile).CanBeFaithful)
                    return;
            }
            else
                return;

            CustomFaithSpell prayer = CustomFaithSpell.DupeCustomFaithSpell(spell);
            prayer.Caster = m;
            BaseCustomSpell.SpellInitiator(prayer);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version

            if (m_Prayer == null)
                m_Prayer = new CustomFaithSpell();
            CustomFaithSpell.Serialize(writer, m_Prayer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Prayer = new CustomFaithSpell();
                        CustomFaithSpell.Deserialize(reader, m_Prayer);
                        break;
                    }
            }
        }
    }
}