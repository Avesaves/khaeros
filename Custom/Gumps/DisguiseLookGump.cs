using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
    public class DisguiseLookGump : Gump
    {
        private BaseCreature m_bc = null;
        private PlayerMobile m_self = null;
        private Item m_item = null;
        private int m_page = 1;

        public DisguiseLookGump( PlayerMobile self, int page )
            : base( 0, 0 )
        {
            string description = "";
            m_page = page;

            self.CloseGump( typeof( DisguiseLookGump ) );
            this.Closable=false;
            this.Disposable=false;
            this.Dragable=true;
            this.Resizable=false;

            this.AddPage( 0 );
            this.AddBackground( 54, 31, 400, 383, 9270 );
            this.AddBackground( 71, 192, 364, 202, 3500 );
            this.AddImage( 4, 10, 10440 );
            this.AddImage( 423, 10, 10441 );
            this.AddImage( 183, 50, 29 );
            //this.AddImage(215, 80, 9000);
            this.AddLabel( 218, 47, 2010, @"Description" );

            if( self != null )
            {
                this.AddButton( 396, 130, 4005, 4007, 102, GumpButtonType.Reply, 0 );
                this.AddButton( 359, 130, 4014, 4016, 101, GumpButtonType.Reply, 0 );

                string beauty = "Average-Looking";

                if( self.Disguise.Looks != null )
                    beauty = self.Disguise.Looks;

                
                else if( self.GetBackgroundLevel(BackgroundList.Attractive) > 0 )
                    beauty = "Attractive";

                else if( self.GetBackgroundLevel(BackgroundList.GoodLooking) > 0 )
                    beauty = "Good-Looking";

                else if( self.GetBackgroundLevel(BackgroundList.Gorgeous) > 0 )
                    beauty = "Gorgeous";

                else if( self.GetBackgroundLevel(BackgroundList.Homely) > 0 )
                    beauty = "Homely";

                else if( self.GetBackgroundLevel(BackgroundList.Ugly) > 0 )
                    beauty = "Ugly";

                else if( self.GetBackgroundLevel(BackgroundList.Hideous) > 0 )
                    beauty = "Hideous";

                string age = "Teenager";

                if( self.Disguise.Age != null )
                    age = self.Disguise.Age;

                else if( self.Age > 19 && self.Age < 26 )
                    age = "Early twenties";

                else if( self.Age > 25 && self.Age < 30 )
                    age = "Late twenties";

                else if( self.Age > 29 && self.Age < 36 )
                    age = "Early thirties";

                else if( self.Age > 35 && self.Age < 40 )
                    age = "Late thirties";

                else if( self.Age > 39 && self.Age < 60 )
                    age = "Middle-aged";

                else if( self.Age > 59 && self.Age < 80 )
                    age = "Elderly";

                else if( self.Age > 79 && self.Age < 100 )
                    age = "Extremely old";

                else if( self.Age > 99 )
                    age = "Ancient";

                m_self = self;

                if( page == 1 )
                    description = self.Disguise.Description1;

                else if( page == 2 )
                    description = self.Disguise.Description2;

                else if( page == 3 )
                    description = self.Disguise.Description3;

                this.AddLabel( 88, 97, 2010, @"Height: " + self.Height + " petrae" );
                this.AddLabel( 88, 117, 2010, @"Weight: " + self.Weight + " petrae" );
                this.AddLabel( 88, 137, 2010, beauty );
                this.AddLabel( 88, 157, 2010, age );
            }

            /*this.AddLabel(126, 84, 1149, @"Western");
            this.AddLabel(126, 121, 1149, @"Haluaroc");
            this.AddLabel(126, 158, 1149, @"Southern");
            this.AddLabel(317, 84, 1149, @"Mhordul");
            this.AddLabel(317, 121, 1149, @"Tirebladd");
            this.AddLabel(317, 158, 1149, @"Northern");*/
            this.AddButton( 404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0 );
            this.AddButton( 371, 46, 1153, 1155, 1, GumpButtonType.Reply, 0 );

            //this.AddHtml( 99, 219, 307, 147, @"" + description, (bool)true, (bool)true);
            this.AddTextEntry( 99, 219, 307, 347, 0, 2, @"" + description );
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            PlayerMobile m = sender.Mobile as PlayerMobile;

            if( m == null )
                return;

            if( info.ButtonID > 0 )
            {
                TextRelay target = info.GetTextEntry( 2 );

                if( m_page == 1 )
                    m.Disguise.Description1 = target.Text;

                else if( m_page == 2 )
                    m.Disguise.Description2 = target.Text;

                else if( m_page == 3 )
                    m.Disguise.Description3 = target.Text;

                if( info.ButtonID == 101 )
                {
                    m_page--;

                    if( m_page < 1 )
                        m_page = 3;

                    m.SendGump( new DisguiseLookGump( m, m_page ) );
                }

                if( info.ButtonID == 102 )
                {
                    m_page++;

                    if( m_page > 3 )
                        m_page = 1;

                    m.SendGump( new DisguiseLookGump( m, m_page ) );
                }
            }
        }
    }
}
