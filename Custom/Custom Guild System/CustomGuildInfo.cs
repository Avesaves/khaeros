using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Commands;
using System.Collections.Generic;

namespace Server.Misc
{
    [PropertyObject]
    public class CustomGuildInfo
    {
        public static void Initialize()
        {
            CommandSystem.Register("Title", AccessLevel.Player, new CommandEventHandler(ActiveTitle_OnCommand));
        }

        [Usage("Title")]
        [Description("Changes which organization, if any, your title is active on.")]
        private static void ActiveTitle_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Mobile.Deleted)
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            if (e.ArgString == null || e.ArgString.Length < 1)
            {
                foreach (KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in m.CustomGuilds)
                {
                    if (m.CustomGuilds[kvp.Key].ActiveTitle)
                        m.CustomGuilds[kvp.Key].ActiveTitle = false;
                }
                m.SendMessage("All titles are now inactive!");
            }

            else if (e.ArgString != null && e.ArgString.Length > 0)
            {
                foreach (KeyValuePair<CustomGuildStone, CustomGuildInfo> kvp in m.CustomGuilds)
                {
                    if (m.CustomGuilds[kvp.Key].ActiveTitle)
                        m.CustomGuilds[kvp.Key].ActiveTitle = false;
                }

                string guild = e.ArgString.Trim().ToLower();

                foreach (CustomGuildStone g in m.CustomGuilds.Keys)
                {
                    if (g.Name.ToLower() == guild)
                    {
                        m.CustomGuilds[g].ActiveTitle = true;
                        m.SendMessage("Title for " + g.Name + " now active!");
                        continue;
                    }
                }
            }

            m.InvalidateProperties();
            m.Delta(MobileDelta.Name);
        }

        public override string ToString()
		{
			return "...";
        }

        private CustomGuildStone m_GuildStone;
		public CustomGuildStone GuildStone{ get{ return m_GuildStone; } set{ m_GuildStone = value; } }

		private int m_RankID;
		public int RankID{ get{ return m_RankID; } set{ m_RankID = value; } }
		
		private int m_Balance;
		public int Balance{ get{ return m_Balance; } set{ m_Balance = value; } }
		
		public GuildRankInfo RankInfo
		{
			get
			{
				if( GuildStone == null || !GuildStone.Ranks.ContainsKey(RankID) )
					return null;
				
				return GuildStone.Ranks[RankID];
			}
		}

		private string m_RegistrationName;
		public string RegistrationName{ get{ return m_RegistrationName; } set{ m_RegistrationName = value; } }
		
		private bool m_ActiveTitle;
		public bool ActiveTitle{ get{ return m_ActiveTitle; } set{ m_ActiveTitle = value; } }

		public string GuildName{ get{ return (m_GuildStone == null ? null : m_GuildStone.Name); } }

        public CustomGuildInfo()
		{
		}

		public CustomGuildInfo( GenericReader reader )
		{
			int version = reader.ReadInt();
			int test = 0;
			
			if( version < 1 )
			{
				m_GuildStone = (CustomGuildStone)reader.ReadItem();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				test = reader.ReadInt();
				m_RegistrationName = reader.ReadString();
			}
			
			else
			{
				m_GuildStone = (CustomGuildStone)reader.ReadItem();
				m_RankID = reader.ReadInt();
				m_RegistrationName = reader.ReadString();
				m_ActiveTitle = reader.ReadBool();
				
				if( version > 1 )
					m_Balance = reader.ReadInt();
			}
		}

        public static void Serialize( GenericWriter writer, CustomGuildInfo info )
		{
			writer.Write( (int) 2 ); // version
			writer.Write( (CustomGuildStone) info.GuildStone );
			writer.Write( (int) info.RankID );
			writer.Write( (string) info.RegistrationName );
			writer.Write( (bool) info.ActiveTitle );
			writer.Write( (int) info.Balance );
		}
    }
}
