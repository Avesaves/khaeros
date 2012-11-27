using System;
using System.Collections.Generic;
using Server.Prompts;
using Server.Commands;
using Server.Mobiles;

namespace Server.Items
{
    public class AtmosphereTile : Item, IDyable, IChopable
    {
        public static void Initialize()
        {
            CommandSystem.Register("AddAtmosphere", AccessLevel.Player, new CommandEventHandler(AddAtmosphere_OnCommand));
        }

        [Usage("AddAtmosphere")]
        [Description("Allows you to add an Atmosphere Tile in-game.")]
        private static void AddAtmosphere_OnCommand(CommandEventArgs e)
        {
            AtmosphereTile removeTile = null;
            foreach (AtmosphereTile tile in AtmosphereTiles)
            {
                if (tile.Owner == e.Mobile)
                {
                    removeTile = tile;
                    continue;
                }
            }
            if (removeTile != null)
            {
                removeTile.Delete();
                e.Mobile.SendMessage("You have an Atmosphere Tile currently in existence. It has been deleted.");
            }

            AtmosphereTile newTile = new AtmosphereTile("", e.Mobile);
            e.Mobile.AddToBackpack(newTile);
            e.Mobile.SendMessage("You have created a new Atmosphere Tile!");
        }

        public static List<AtmosphereTile> AtmosphereTiles = new List<AtmosphereTile>();

        private string m_Message;
        private Mobile m_Owner;
        private DateTime m_LastMessage = DateTime.Now;
        private int m_Refractory;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Message { get { return m_Message; } set { m_Message = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Refractory { get { return m_Refractory; } set { m_Refractory = value; if (m_Refractory < 1) m_Refractory = 1; } }

        [Constructable]
        public AtmosphereTile(string message, Mobile owner) : base(6099)
        {
            m_Message = message;
            m_Owner = owner;
            m_Refractory = 180;

            Name = "An Atmosphere Tile";
            
            Hue = 0;
            Stackable = false;
            Visible = false;
            Weight = 0;

            AtmosphereTiles.Add(this);
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (Message != null && Message.Length > 0)
            {
                if (m_LastMessage + TimeSpan.FromSeconds(m_Refractory) < DateTime.Now)
                {
                    m.SendMessage(Hue, Message);
                    m_LastMessage = DateTime.Now;
                }
            }

            return base.OnMoveOver(m);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Current Message:");
            from.SendMessage(Hue, Message);
            from.Prompt = new AtmosphereTileEditPrompt(from, this);

            base.OnDoubleClick(from);
        }

        public bool CanSeeMe(Mobile m)
        {
            if (m_Owner == m)
                return true;

            if (m.AccessLevel > AccessLevel.Player)
                return true;

            return false;
        }

        public override void OnDelete()
        {
            if (AtmosphereTiles.Contains(this))
                AtmosphereTiles.Remove(this);

            base.OnDelete();
        }

        public override bool OnDecay()
        {
            if (AtmosphereTiles.Contains(this))
                AtmosphereTiles.Remove(this);

            return base.OnDecay();
        }

        public void OnChop(Mobile from)
        {
            if (from == m_Owner || from.AccessLevel > AccessLevel.Player)
            {
                if (AtmosphereTiles.Contains(this))
                    AtmosphereTiles.Remove(this);

                from.SendLocalizedMessage(500461); // You destroy the item.
                Delete();
            }
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public AtmosphereTile(Serial serial)
            : base(serial)
        { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Message = reader.ReadString();
            m_Owner = reader.ReadMobile();

            m_LastMessage = DateTime.Now;

            AtmosphereTiles.Add(this);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
            writer.Write((string)m_Message);
            writer.Write((Mobile)m_Owner);
        }

        private class AtmosphereTileEditPrompt : Prompt
        {
            private AtmosphereTile m_AtmoTile;

            public AtmosphereTileEditPrompt(Mobile from, AtmosphereTile tile)
            {
                m_AtmoTile = tile;
                from.SendMessage("Do you wish to edit this Atmosphere Tile's message? (Yes / No) Or, enter 'time' or 'refractory' to edit the tile's refractory period.");
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text == null || m_AtmoTile == null || from == null || m_AtmoTile.Deleted || from.Deleted)
                    return;

                if (text.ToLower() == "yes" || text.ToLower() == "y")
                    from.Prompt = new AtmosphereTilePrompt(from, m_AtmoTile);
                else if (text.ToLower() == "time" || text.ToLower() == "refractory")
                    from.Prompt = new AtmosphereTileTimePrompt(from, m_AtmoTile);
                else
                    from.SendMessage("This Atmosphere Tile will not be edited.");
            }
        }

        private class AtmosphereTilePrompt : Prompt
        {
            private AtmosphereTile m_AtmoTile;

            public AtmosphereTilePrompt(Mobile from, AtmosphereTile tile)
            {
                m_AtmoTile = tile;
                from.SendMessage("Type and enter the text you wish the Atmosphere Tile to send to anyone who walks over it.");
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text == null || m_AtmoTile == null || from == null || m_AtmoTile.Deleted || from.Deleted)
                    return;

                m_AtmoTile.Message = text;

                from.SendMessage("Message successfully entered:");
                from.SendMessage(m_AtmoTile.Hue, m_AtmoTile.Message);
            }
        }

        private class AtmosphereTileTimePrompt : Prompt
        {
            private AtmosphereTile m_AtmoTile;

            public AtmosphereTileTimePrompt(Mobile from, AtmosphereTile tile)
            {
                m_AtmoTile = tile;
                from.SendMessage("Enter the number of seconds for this Atmosphere Tile's refractory period between messages.");
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text == null || m_AtmoTile == null || from == null || m_AtmoTile.Deleted || from.Deleted)
                    return;

                int val = 0;
                if (ValidateInt(text, ref val))
                {
                    m_AtmoTile.Refractory = val;
                    from.SendMessage("Refractory period successfully changed to " + m_AtmoTile.Refractory.ToString() + " seconds.");
                }
                else
                {
                    from.SendMessage("ERROR: Invalid integer.");
                }
            }

            private bool ValidateInt(string st, ref int parsed)
            {
                if (!int.TryParse(st, out parsed))
                    return false;

                return true;
            }
        }
    }
}