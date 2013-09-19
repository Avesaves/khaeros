using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Prompts;
using Server.Gumps;
using Server.Targeting;

namespace Server.Commands
{
    public class DisguiseCommands
    {

        public static void Initialize()
        {
            CommandSystem.Register( "ListDisguises", AccessLevel.Player, new CommandEventHandler( ListDisguises_OnCommand ) );
            CommandSystem.Register( "SaveDisguise", AccessLevel.Player, new CommandEventHandler( SaveDisguise_OnCommand ) );
            CommandSystem.Register( "LoadDisguise", AccessLevel.Player, new CommandEventHandler( LoadDisguise_OnCommand ) );
            CommandSystem.Register( "DisguiseProfile", AccessLevel.Player, new CommandEventHandler( DisguiseProfile_OnCommand ) );
            CommandSystem.Register( "DisguiseAge", AccessLevel.Player, new CommandEventHandler( DisguiseAge_OnCommand ) );
            CommandSystem.Register( "DisguiseLooks", AccessLevel.Player, new CommandEventHandler( DisguiseLooks_OnCommand ) );
            CommandSystem.Register( "DisguiseTitle", AccessLevel.Player, new CommandEventHandler( DisguiseTitle_OnCommand ) );
            CommandSystem.Register( "DisguisePrefix", AccessLevel.Player, new CommandEventHandler( DisguisePrefix_OnCommand ) );
            CommandSystem.Register( "DisguiseName", AccessLevel.Player, new CommandEventHandler( DisguiseName_OnCommand ) );
            CommandSystem.Register( "NoDisguise", AccessLevel.Player, new CommandEventHandler( NoDisguise_OnCommand ) );
            CommandSystem.Register( "Disguise", AccessLevel.Player, new CommandEventHandler( Disguise_OnCommand ) );
        }

        [Usage("ListDisguises")]
        [Description("Lists the disguises you have saved.")]
        private static void ListDisguises_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Mobile.Deleted)
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) < 1)
            {
                m.SendMessage("You need at least the first level of the Disguise Kit skill in order to use this command.");
                return;
            }

            int hue = 0;
            switch(m.Nation)
            {
                case Nation.Southern: hue = 2001; break;
                case Nation.Western: hue = 1358; break;
                case Nation.Haluaroc: hue = 1057; break;
                case Nation.Mhordul: hue = 1157; break;
                case Nation.Tirebladd: hue = 2406; break;
                case Nation.Northern: hue = 1156; break;
            }

            for (int i = 0; i < m.MyDisguises.Disguises.Length; i++)
            {
                if (m.MyDisguises.Disguises[i].Name != null)
                {
                    m.SendMessage(hue, "Disguise #" + i.ToString() + ": " + m.MyDisguises.Disguises[i].TitlePrefix + " "
                        + m.MyDisguises.Disguises[i].Name + ", " + m.MyDisguises.Disguises[i].RPTitle + "; " +
                        m.MyDisguises.Disguises[i].Age + ".");
                }
            }
        }

        [Usage("SaveDisguise")]
        [Description( "Saves your current looks as a disguise." )]
        private static void SaveDisguise_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            int slot = 0;

            if( m.Feats.GetFeatLevel( FeatList.DisguiseKit ) < 1 )
                m.SendMessage( "You need at least the first level of the Disguise Kit skill in order to use this command." );

            else if( e.Length > 0 && e.Arguments[0].Trim().Length > 0 && int.TryParse( e.Arguments[0].Trim(), out slot ) &&
                slot > 0 && slot < 7 )
            {
                slot--;
                m.MyDisguises.Disguises[slot].Name = String.IsNullOrEmpty( m.Disguise.Name ) ? m.Name : m.Disguise.Name;
                m.MyDisguises.Disguises[slot].RPTitle = String.IsNullOrEmpty( m.Disguise.RPTitle ) ? m.RPTitle : m.Disguise.RPTitle;
                m.MyDisguises.Disguises[slot].TitlePrefix = String.IsNullOrEmpty( m.Disguise.TitlePrefix ) ? m.TitlePrefix : m.Disguise.TitlePrefix;
                m.MyDisguises.Disguises[slot].Age = m.Disguise.Age;
                m.MyDisguises.Disguises[slot].Looks = m.Disguise.Looks;
                m.MyDisguises.Disguises[slot].Description1 = m.Description;
                m.MyDisguises.Disguises[slot].Description2 = m.Description2;
                m.MyDisguises.Disguises[slot].Description3 = m.Description3;
                m.MyDisguises.Disguises[slot].HairItemID = m.Disguise.HairItemID > -1 ? m.Disguise.HairItemID : m.HairItemID;
                m.MyDisguises.Disguises[slot].HairHue = m.Disguise.HairHue > -1 ? m.Disguise.HairHue : m.HairHue;
                m.MyDisguises.Disguises[slot].FacialHairItemID = m.Disguise.FacialHairItemID > -1 ? m.Disguise.FacialHairItemID : m.FacialHairItemID;
                m.MyDisguises.Disguises[slot].FacialHairHue = m.Disguise.FacialHairHue > -1 ? m.Disguise.FacialHairHue : m.FacialHairHue;
                m.MyDisguises.Disguises[slot].Hue = m.Disguise.Hue > -1 ? m.Disguise.Hue : m.Hue;
                slot++;
                m.SendMessage( "Your current disguise has been saved to slot " + slot.ToString() + "." );
            }

            else
                m.SendMessage( "Please add a number from 1 to 6 as an argument for the command." );
        }

        [Usage( "LoadDisguise" )]
        [Description( "Loads a saved disguise." )]
        private static void LoadDisguise_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            int slot = 0;

            if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) < 1)
                m.SendMessage("You need at least the first level of the Disguise Kit skill in order to use this command.");

            else if (e.Length > 0 && e.Arguments[0].Trim().Length > 0 && int.TryParse(e.Arguments[0].Trim(), out slot) && slot > 0 &&
                slot < 7 && ( m.AccessLevel < AccessLevel.GameMaster && (m.LastDisguiseTime + TimeSpan.FromMinutes(20) <= DateTime.Now)))
            {
                slot--;
                m.Disguised = true;
                m.LastDisguiseTime = DateTime.Now;

                m.Disguise.Name = m.MyDisguises.Disguises[slot].Name;
                m.Disguise.Description1 = m.MyDisguises.Disguises[slot].Description1;
                m.Disguise.Description2 = m.MyDisguises.Disguises[slot].Description2;
                m.Disguise.Description3 = m.MyDisguises.Disguises[slot].Description3;
                m.Disguise.HairItemID = m.MyDisguises.Disguises[slot].HairItemID;
                m.Disguise.FacialHairItemID = m.MyDisguises.Disguises[slot].FacialHairItemID;

                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 1)
                {
                    m.Disguise.FacialHairHue = m.MyDisguises.Disguises[slot].FacialHairHue;
                    m.Disguise.Hue = m.MyDisguises.Disguises[slot].Hue;
                    m.Disguise.HairHue = m.MyDisguises.Disguises[slot].HairHue;
                }

                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 2)
                {
                    m.Disguise.RPTitle = m.MyDisguises.Disguises[slot].RPTitle;
                    m.Disguise.TitlePrefix = m.MyDisguises.Disguises[slot].TitlePrefix;
                    m.Disguise.Age = m.MyDisguises.Disguises[slot].Age;
                    m.Disguise.Looks = m.MyDisguises.Disguises[slot].Looks;
                }

                m.Delta(MobileDelta.Hue);
                m.Delta(MobileDelta.Hair);
                m.Delta(MobileDelta.FacialHair);
                m.InvalidateProperties();
                slot++;
                m.SendMessage("You have loaded the disguise saved on slot " + slot.ToString() + ".");
            }
            else if (m.AccessLevel < AccessLevel.GameMaster && (m.LastDisguiseTime + TimeSpan.FromMinutes(20)) >= DateTime.Now)
            {
                switch (m.Disguised)
                {
                    case true: m.SendMessage("It is too soon to change your disguise!"); break;
                    case false: m.SendMessage("It is too soon to disguise yourself again!"); break;
                }
            }
            else
                m.SendMessage("Please add a number from 1 to 6 as an argument for the command.");
        }

        [Usage( "NoDisguise" )]
        [Description( "Allows you to remove your disguise." )]
        private static void NoDisguise_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m.LastDisguiseTime + TimeSpan.FromMinutes(15) <= DateTime.Now)
            {
                if (m == null || !m.Alive || m.Paralyzed)
                    return;

                if (m.Female)
                    m.Emote("*removes her disguise*");
                else
                    m.Emote("*removes his disguise*");

                RemoveDisguise(m);
            }
            else
                m.SendMessage("It is too soon to change your disguise!");
        }

        public static void RemoveDisguise( PlayerMobile m )
        {
            m.Disguise = new DisguiseContext();
            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
            m.InvalidateProperties();
            m.Disguised = false;
        }

        [Usage( "Disguise" )]
        [Description( "Allows you to customize a mobile's appearance." )]
        private static void Disguise_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;

            if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 0)
            {
                string nationchoice = e.ArgString.Trim();
                nationchoice = nationchoice.ToLower();
                Nation nation = Nation.None;

                if (nationchoice == "southern")
                    nation = Nation.Southern;

                else if (nationchoice == "western")
                    nation = Nation.Western;

/*                 else if (nationchoice == "stranger")
                    nation = Nation.Mhordul; */

                else if (nationchoice == "northern")
                    nation = Nation.Northern;

                else
                {
                    m.SendMessage("Usage example: .Disguise Northern .Disguise Southern .Disguise Western");
                    return;
                }

                if (m.Nation != nation && m.Feats.GetFeatLevel(FeatList.DisguiseKit) < 2)
                {
                    m.SendMessage("You need the third level of the Disguise Kit feat before you can disguise yourself as a member of another nation.");
                    return;
                }

                m.Target = new DisguiseTarget(nation);
            }
            else
                m.SendMessage("You do not have the Disguise Kit feat.");
        }

        private class DisguiseTarget : Target
        {
            private Nation m_nation;

            public DisguiseTarget( Nation nation )
                : base( 8, false, TargetFlags.None )
            {
                m_nation = nation;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if( m == null || obj == null || !m.Alive || m.Paralyzed )
                    return;

                Mobile mob = obj as Mobile;
                PlayerMobile pm = m as PlayerMobile;

                if( !( obj is PlayerMobile ) && !( obj is Mercenary ) )
                {
                    m.SendMessage( "That is not a valid target." );
                    return;
                }

                if( obj != m && ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.DisguiseOthers ) < 1 )
                {
                    m.SendMessage( "You do not have the Disguise Others feat." );
                    return;
                }

                if( obj is PlayerMobile && ( (PlayerMobile)mob ).Nation != m_nation && pm.Feats.GetFeatLevel( FeatList.DisguiseKit ) < 2 )
                {
                    m.SendMessage( "You need the third level of the Disguise Others feat before you can disguise someone as a member of another nation." );
                    return;
                }

                Item apron = mob.FindItemOnLayer( Layer.MiddleTorso );

                if (apron != null && apron is HairStylingApron && m.CanSee(mob) && m.InLOS(mob) && m.InRange(mob, 1))
                {
                    if (mob is PlayerMobile)
                    {
                        if (((PlayerMobile)mob).LastDisguiseTime + TimeSpan.FromMinutes(30) <= DateTime.Now)
                        {
                            m.SendGump(new NPCCustomGump(mob, m_nation, 0));
                            ((PlayerMobile)mob).Disguised = true;
                            ((PlayerMobile)mob).LastDisguiseTime = DateTime.Now;
                        }
                        else
                            m.SendMessage("It is too soon to disguise " + mob.Name + " again.");
                    }
                    else
                        m.SendGump(new NPCCustomGump(mob, m_nation, 0));
                }
                else
                    m.SendMessage("Your target needs to be wearing a hair styling apron.");
            }
        }

        [Usage("DisguiseProfile")]
        [Description("Changes your disguise profile.")]
        private static void DisguiseProfile_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Mobile.Deleted)
                return;

            if ((e.Mobile as PlayerMobile).Feats.GetFeatLevel(FeatList.DisguiseKit) > 0)
            {
                if (((PlayerMobile)e.Mobile).Disguised)
                {
                    PlayerMobile m = e.Mobile as PlayerMobile;
                    m.SendGump(new DisguiseLookGump(m, 1));
                }            
                else
                    (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
            }
            else
                (e.Mobile as PlayerMobile).SendMessage("You do not have the appropriate Disguise Kit feat level.");
        }

        [Usage( "DisguiseAge" )]
        [Description( "Allows you to change your age description." )]
        private static void DisguiseAge_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;


                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 1)
                {
                    if (((PlayerMobile)e.Mobile).Disguised)
                    {
                        string newstring = e.ArgString.ToLower();

                        if (newstring == "teenager")
                            m.Target = new FineDisguiseTarget("Teenager", false, false, false, true, false);
                        else if (newstring == "early twenties")
                            m.Target = new FineDisguiseTarget("Early twenties", false, false, false, true, false);
                        else if (newstring == "late twenties")
                            m.Target = new FineDisguiseTarget("Late twenties", false, false, false, true, false);
                        else if (newstring == "early thirties")
                            m.Target = new FineDisguiseTarget("Early thirties", false, false, false, true, false);
                        else if (newstring == "late thirties")
                            m.Target = new FineDisguiseTarget("Late thirties", false, false, false, true, false);
                        else if (newstring == "middle-aged")
                            m.Target = new FineDisguiseTarget("Middled-aged", false, false, false, true, false);
                        else if (newstring == "extremely old")
                            m.Target = new FineDisguiseTarget("Extremely old", false, false, false, true, false);
                        else if (newstring == "ancient")
                            m.Target = new FineDisguiseTarget("Ancient", false, false, false, true, false);
                        else if (newstring == "elderly")
                            m.Target = new FineDisguiseTarget("Elderly", false, false, false, true, false);
                        else
                        {
                            m.SendMessage("Invalid usage. Options:");
                            m.SendMessage(".DisguiseAge teenager");
                            m.SendMessage(".DisguiseAge early twenties");
                            m.SendMessage(".DisguiseAge late twenties");
                            m.SendMessage(".DisguiseAge early thirties");
                            m.SendMessage(".DisguiseAge late thirties");
                            m.SendMessage(".DisguiseAge middle-aged");
                            m.SendMessage(".DisguiseAge elderly");
                            m.SendMessage(".DisguiseAge extremely old");
                            m.SendMessage(".DisguiseAge ancient");
                            return;
                        }
                    }
                    else
                        (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
                }
                else
                    m.SendMessage("You do not have the appropriate Disguise Kit feat level.");
        }

        [Usage( "DisguiseLooks" )]
        [Description( "Allows you to change your looks description." )]
        private static void DisguiseLooks_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;

                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 1)
                {
                    if (((PlayerMobile)e.Mobile).Disguised)
                    {
                        string newstring = e.ArgString.ToLower();

                        if (newstring == "hideous")
                            m.Target = new FineDisguiseTarget("Hideous", false, false, false, false, true);
                        else if (newstring == "ugly")
                            m.Target = new FineDisguiseTarget("Ugly", false, false, false, false, true);
                        else if (newstring == "homely")
                            m.Target = new FineDisguiseTarget("Homely", false, false, false, false, true);
                        else if (newstring == "average-looking")
                            m.Target = new FineDisguiseTarget("Average-Looking", false, false, false, false, true);
                        else if (newstring == "attractive")
                            m.Target = new FineDisguiseTarget("Attractive", false, false, false, false, true);
                        else if (newstring == "good-looking")
                            m.Target = new FineDisguiseTarget("Good-Looking", false, false, false, false, true);
                        else if (newstring == "gorgeous")
                            m.Target = new FineDisguiseTarget("Gorgeous", false, false, false, false, true);
                        else
                        {
                            m.SendMessage("Invalid usage. Options:");
                            m.SendMessage(".DisguiseLooks hideous");
                            m.SendMessage(".DisguiseLooks ugly");
                            m.SendMessage(".DisguiseLooks homely");
                            m.SendMessage(".DisguiseLooks average-looking");
                            m.SendMessage(".DisguiseLooks attractive");
                            m.SendMessage(".DisguiseLooks good-looking");
                            m.SendMessage(".DisguiseLooks gorgeous");
                            return;
                        }
                    }
                    else
                        (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
                }

                else
                    m.SendMessage("You do not have the appropriate Disguise Kit feat level.");
        }

        [Usage( "DisguiseTitle" )]
        [Description( "Allows you to change your title." )]
        private static void DisguiseTitle_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;

                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 2)
                {
                    if (((PlayerMobile)e.Mobile).Disguised)
                    {
                        string newstring = e.ArgString;

                        if (newstring.Length > 0)
                            m.Target = new FineDisguiseTarget(newstring, false, true, false, false, false);

                        else
                        {
                            m.SendMessage("Usage example: .DisguiseTitle the Northern Soldier");
                            return;
                        }
                    }
                    else                
                        (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
                }

                else
                    m.SendMessage("You do not have the appropriate Disguise Kit feat level.");
        }

        [Usage( "DisguisePrefix" )]
        [Description( "Allows you to change your prefix." )]
        private static void DisguisePrefix_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;


                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 2)
                {            
                    if (((PlayerMobile)e.Mobile).Disguised)
                        m.Target = new FineDisguiseTarget(e.ArgString, false, false, true, false, false);
                    else
                        (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
                }
                else
                    m.SendMessage("You do not have the appropriate Disguise Kit feat level.");
                
        }

        [Usage( "DisguiseName" )]
        [Description( "Allows you to change your name." )]
        private static void DisguiseName_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m == null || !m.Alive || m.Paralyzed )
                return;



                if (m.Feats.GetFeatLevel(FeatList.DisguiseKit) > 0)
                {
                    if (((PlayerMobile)e.Mobile).Disguised)
                    {
                        string newstring = e.ArgString;

                        if (newstring.Length > 0)
                            m.Target = new FineDisguiseTarget(newstring, true, false, false, false, false);

                        else
                        {
                            m.SendMessage("Usage example: .DisguiseName John Smith");
                            return;
                        }
                    }
                    else
                        (e.Mobile as PlayerMobile).SendMessage("You must .disguise yourself before you can add details to your disguise.");
                }

                else
                    m.SendMessage("You do not have the Disguise Kit feat.");

        }

        private class FineDisguiseTarget : Target
        {
            private string m_newstring;
            private bool m_name;
            private bool m_title;
            private bool m_prefix;
            private bool m_age;
            private bool m_looks;

            public FineDisguiseTarget( string newstring, bool name, bool title, bool prefix, bool age, bool looks )
                : base( 3, false, TargetFlags.None )
            {
                m_newstring = newstring;
                m_name = name;
                m_title = title;
                m_prefix = prefix;
                m_age = age;
                m_looks = looks;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if( m == null || obj == null || !m.Alive || m.Paralyzed )
                    return;

                if( !( obj is PlayerMobile ) )
                {
                    m.SendMessage( "Invalid target." );
                    return;
                }

                PlayerMobile pm = m as PlayerMobile;
                PlayerMobile disguised = obj as PlayerMobile;
                int featlevel = pm.Feats.GetFeatLevel( FeatList.DisguiseKit );

                if( pm != obj )
                {
                    if( pm.Feats.GetFeatLevel( FeatList.DisguiseOthers ) < 1 )
                    {
                        pm.SendMessage( "You lack the Disguise Others feat." );
                        return;
                    }

                    if( !pm.CanSee( disguised ) || !pm.InLOS( disguised ) || !disguised.Alive )
                    {
                        pm.SendMessage( "Target out of reach." );
                        return;
                    }

                    Item apron = disguised.FindItemOnLayer( Layer.MiddleTorso );

                    if( apron != null && apron is HairStylingApron )
                        featlevel = pm.Feats.GetFeatLevel( FeatList.DisguiseOthers );

                    else
                    {
                        pm.SendMessage( "Your target needs to be wearing a hair styling apron." );
                        return;
                    }
                }

                if( ( m_title || m_prefix ) && featlevel < 3 )
                {
                    m.SendMessage( "You lack the appropriate feat level." );
                    return;
                }

                if( ( m_age || m_looks ) && featlevel < 2 )
                {
                    m.SendMessage( "You lack the appropriate feat level." );
                    return;
                }

                if (m_name)
                {
                    disguised.Disguise.Name = m_newstring;
                }

                if (m_title)
                {
                    disguised.Disguise.RPTitle = m_newstring;
                }

                if (m_prefix)
                {
                    disguised.Disguise.TitlePrefix = m_newstring;
                }

                if (m_age)
                {
                    disguised.Disguise.Age = m_newstring;
                }

                if( m_looks )
                {
                    int looks = 0;
                    int newlooks = 0;

                    
                    if(  disguised.GetBackgroundLevel(BackgroundList.Hideous) > 0 )
                        looks = -3;
                    else if( disguised.GetBackgroundLevel(BackgroundList.Ugly) > 0 )
                        looks = -2;
                    else if( disguised.GetBackgroundLevel(BackgroundList.Homely) > 0 )
                        looks = -1;
                    else if( disguised.GetBackgroundLevel(BackgroundList.Attractive) > 0 )
                        looks = 1;
                    else if( disguised.GetBackgroundLevel(BackgroundList.GoodLooking) > 0 )
                        looks = 2;
                    else if( disguised.GetBackgroundLevel(BackgroundList.Gorgeous) > 0 )
                        looks = 3;

                    if( m_newstring == "Hideous" )
                        newlooks = -3;
                    else if( m_newstring == "Ugly" )
                        newlooks = -2;
                    else if( m_newstring == "Homely" )
                        newlooks = -1;
                    else if( m_newstring == "Average-Looking" )
                        newlooks = 0;
                    else if( m_newstring == "Attractive" )
                        newlooks = 1;
                    else if( m_newstring == "Good-Looking" )
                        newlooks = 2;
                    else if( m_newstring == "Gorgeous" )
                        newlooks = 3;

                    if( newlooks > looks )
                        m.SendMessage( "You cannot improve your target's looks, only worsen it." );

                    else
                        disguised.Disguise.Looks = m_newstring;
                }

                disguised.InvalidateProperties();
            }
        }
    }

    public class DisguiseCollection
    {
        public const int MaxDisguises = 6;

        private DisguiseContext[] m_Disguises = new DisguiseContext[MaxDisguises]{ new DisguiseContext(), new DisguiseContext(), new DisguiseContext(), 
            new DisguiseContext(), new DisguiseContext(), new DisguiseContext() };
        public DisguiseContext[] Disguises { get { return m_Disguises; } set { m_Disguises = value; } }

        public DisguiseCollection()
        {
        }

        public DisguiseCollection( GenericReader reader )
		{
            Deserialize( reader );
		}

        public void Deserialize( GenericReader reader )
        {
            int version = reader.ReadInt();
            int count = reader.ReadInt();

            for( int i = 0; i < count; i++ )
                Disguises[i].Deserialize( reader );
        }

        public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); // version
            writer.Write( (int) MaxDisguises );

            for( int i = 0; i < MaxDisguises; i++ )
                Disguises[i].Serialize( writer );
		}
    }

    public class DisguiseContext
    {
        private int m_HairItemID = -1;
        private int m_FacialHairItemID = -1;
        private int m_HairHue = -1;
        private int m_FacialHairHue = -1;
        private int m_Hue = -1;
        private string m_Description1;
        private string m_Description2;
        private string m_Description3;
        private string m_Name;
        private string m_RPTitle;
        private string m_TitlePrefix;
        private string m_Age;
        private string m_Looks;

        public int HairItemID { get { return m_HairItemID; } set { m_HairItemID = value; } }
        public int FacialHairItemID { get { return m_FacialHairItemID; } set { m_FacialHairItemID = value; } }
        public int HairHue { get { return m_HairHue; } set { m_HairHue = value; } }
        public int FacialHairHue { get { return m_FacialHairHue; } set { m_FacialHairHue = value; } }
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public string Description1 { get { return m_Description1; } set { m_Description1 = value; } }
        public string Description2 { get { return m_Description2; } set { m_Description2 = value; } }
        public string Description3 { get { return m_Description3; } set { m_Description3 = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string RPTitle { get { return m_RPTitle; } set { m_RPTitle = value; } }
        public string TitlePrefix { get { return m_TitlePrefix; } set { m_TitlePrefix = value; } }
        public string Age { get { return m_Age; } set { m_Age = value; } }
        public string Looks { get { return m_Looks; } set { m_Looks = value; } }

        public DisguiseContext()
        {
        }

        public DisguiseContext( GenericReader reader )
		{
            Deserialize( reader );
		}

        public void Deserialize( GenericReader reader )
        {
            int version = reader.ReadInt();
            m_HairItemID = reader.ReadInt();
            m_FacialHairItemID = reader.ReadInt();
            m_HairHue = reader.ReadInt();
            m_FacialHairHue = reader.ReadInt();
            m_Hue = reader.ReadInt();
            m_Description1 = reader.ReadString();
            m_Description2 = reader.ReadString();
            m_Description3 = reader.ReadString();
            m_Name = reader.ReadString();
            m_RPTitle = reader.ReadString();
            m_TitlePrefix = reader.ReadString();
            m_Age = reader.ReadString();
            m_Looks = reader.ReadString();
        }

        public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); // version
            writer.Write( (int) m_HairItemID );
            writer.Write( (int) m_FacialHairItemID );
            writer.Write( (int) m_HairHue );
            writer.Write( (int) m_FacialHairHue );
            writer.Write( (int) m_Hue );
            writer.Write( (string) m_Description1 );
            writer.Write( (string) m_Description2 );
            writer.Write( (string) m_Description3 );
            writer.Write( (string) m_Name );
            writer.Write( (string) m_RPTitle );
            writer.Write( (string) m_TitlePrefix );
            writer.Write( (string) m_Age );
            writer.Write( (string) m_Looks );
		}
    }
}
