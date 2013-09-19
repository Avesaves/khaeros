using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps
{
	public class LookGump : Gump
	{
		private BaseCreature m_bc = null;
		private PlayerMobile m_self = null;
		private Item m_item = null;
		private int m_page = 1;
		
	public static class ExtensionMethods
	{
		public static int RoundOff (int i)
		{
			return ((int)Math.Round(i / 10.0)) * 10;
		}
	}

		public static string Color( string text, string color )
				
		{
		return String.Format( "<BASEFONT COLOR=#{0}><center>{1}</center></BASEFONT>", color, text );
		}
		
		public LookGump( PlayerMobile from, BaseCreature bc, PlayerMobile self, Item item, int page)
			: base( 0, 0 )
		{
			string description = "";
			m_page = page;
			
			from.CloseGump( typeof( LookGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 458, 9270);//Background
			this.AddBackground(121, 0, 264, 34, 9270);//Background Descript Title
			this.AddBackground(72, 257, 364, 202, 3500);//White Main Descript
			this.AddImage(4, 10, 10440); //Dragon Left
			this.AddImage(423, 10, 10441);//Dragon Right
			this.AddBackground(74, 47, 139, 203, 9270);//Avatar Background
			this.AddImage(86, 58, 30520, 2999);//Avatar Black Background
			this.AddLabel(202, 7, 148, @"Description Window");
			this.AddButton(365, 44, 12006, 12008, 0, GumpButtonType.Reply, 0); //Close

            bool myself = false;

            if( from == self )
			{
                myself = true;
				this.AddBackground(54, 487, 401, 66, 9270);//Background
				this.AddButton(303, 522, 12000, 12002, 1, GumpButtonType.Reply, 0); //Checkmark
				this.AddLabel(127, 498, 148, @"Avatar Chooser");
				this.AddLabel(287, 498, 148, @"Save Descriptions");
				this.AddImage(257, 30, 29);//Khaeros Button
				this.AddButton(135, 522, 12012, 12014, 666, GumpButtonType.Reply, 0); //Chooser								
			}
			
			
			if( bc != null )
			{
				m_bc = bc;
				description = bc.Description;
				this.AddImage(257, 75, 29);//Khaeros Button
			}
			
			if( item != null )
			{
				m_item = item;
				description = item.Description;
				this.AddImage(257, 75, 29);//Khaeros Button
                this.AddButton(303, 462, 12000, 12002, 1, GumpButtonType.Reply, 0); //Checkmark
			}
			
			if( self != null )
			{
				this.AddButton(303, 460, 12009,12011, 102, GumpButtonType.Reply, 0);//page buttons
            	this.AddButton(135, 460, 12015, 12017, 101, GumpButtonType.Reply, 0);
				this.AddImage(257, 30, 29);//Khaeros Button
				
				string beauty = "Appears average-looking";
                beauty = HealthAttachment.GetAppearance(from, self);

                //if( !myself && self.Disguise.Looks != null )
                //    beauty = self.Disguise.Looks;
				
                    
                //else if( self.GetBackgroundLevel(BackgroundList.Attractive) > 0 )
                //    beauty = "Appears attractive";
				
                //else if( self.GetBackgroundLevel(BackgroundList.GoodLooking) > 0 )
                //    beauty = "Appears good-looking";
				
                //else if( self.GetBackgroundLevel(BackgroundList.Gorgeous) > 0 )
                //    beauty = "Appears gorgeous";
				
                //else if( self.GetBackgroundLevel(BackgroundList.Homely) > 0 )
                //    beauty = "Appears homely";
				
                //else if( self.GetBackgroundLevel(BackgroundList.Ugly) > 0 )
                //    beauty = "Appears ugly";
				
                //else if( self.GetBackgroundLevel(BackgroundList.Hideous) > 0 )
                //    beauty = "Appears disfigured";
				
				string age = "Looks to be a teenager";
				
				if( self.Disguise.Age != null )
                    age = self.Disguise.Age;
				
				else if( self.Age > 19 && self.Age < 26 )
					age = "Is in their early twenties";
				
				else if( self.Age > 25 && self.Age < 30 )
					age = "Is in their late twenties";
				
				else if( self.Age > 29 && self.Age < 36 )
					age = "Is in their early thirties";
				
				else if( self.Age > 35 && self.Age < 40 )
					age = "Is in their late thirties";
				
				else if( self.Age > 39 && self.Age < 60 )
					age = "Looks to be middle-aged";
				
				else if( self.Age > 59 && self.Age < 80 )
					age = "Looks to be elderly";
				
				else if( self.Age > 79 && self.Age < 100 )
					age = "Looks extremely old";
				
				else if( self.Age > 99 )
					age = "Looks to be ancient";
					
				else if( self.Age < 13 )
					age = "Looks to be a child";

                string race = "Exhibits no racial qualities";
				
				//if( !myself && self.Disguise.Nation != null )
                    //race = "Appears to be of " + self.Disguise.Nation + " ancestory";
		if (self.GetDisguisedNation().ToString() == "Northern")
			race = "Exhibits Northern qualities";
			
		else if (self.GetDisguisedNation().ToString() == "Western")
			race = "Exhibits Western qualities";

		else if (self.GetDisguisedNation().ToString() == "Southern")
			race = "Exhibits Southern qualities";
			
/* 		else if (self.GetDisguisedNation().ToString() == "Mhordul")
			race = "Exhibits Mhordul qualities"; */			
                	//race = "Exhibits " + self.GetDisguisedNation().ToString() + " qualities";

                //if (self.Nation == Nation.Northern)
                //    race = "Exhibits Northern qualities";

                //else if (self.Nation == Nation.Southern)
                //    race = "Exhibits Southern qualities";

                //else if (self.Nation == Nation.Western)
                //    race = "Exhibits Western qualities";

                //else if (self.Nation == Nation.Haluaroc)
                //    race = "Exhibits Haluaroc qualities";

                //else if (self.Nation == Nation.Mhordul)
                //    race = "Exhibits Mhordul qualities";

                //else if (self.Nation == Nation.Tirebladd)
                //    race = "Exhibits Tirebladd qualities";
				
				m_self = self;
				
				
				int rawweight = self.Weight;
				int rawheight = self.Height;
				int roundedweight = ExtensionMethods.RoundOff(rawweight);
				int roundedheight = ExtensionMethods.RoundOff(rawheight); 
				
				string height = "Looks around " + roundedheight + " petrae tall";
				string weight = "Looks about " + roundedweight + " petrae heavy";


                    if (page == 1)
                    {
                        if (myself)
                            description = self.BaseDescription1;

                        else
                            description = self.Description;
                    }

                    else if (page == 2)
                    {
                        if (myself)
                            description = self.BaseDescription2;

                        else
                            description = self.Description2;
                    }

                    else if (page == 3)
                    {
                        if (myself)
                            description = self.BaseDescription3;

                        else
                            description = self.Description3;
                    }
				
				AddHtml( 225, 139, 200, 20, Color(race,"FF9912"), false, false);
				AddHtml( 225, 161, 200, 20, Color(age,"FF9912"), false, false);
				AddHtml( 225, 183, 200, 20, Color(beauty,"FF9912"), false, false);
				AddHtml( 225, 205, 200, 20, Color(height,"FF9912"), false, false);
				AddHtml( 225, 227, 200, 20, Color(weight,"FF9912"), false, false);
				//this.AddLabel(271, 180, 2010, @age );
				//this.AddLabel(271, 198, 2010, @beauty);
				//this.AddLabel(271, 216, 2010, @"Height: " + self.Height + " Petrae");
				//this.AddLabel(271, 235, 2010, @"Weight: " + self.Weight + " Petrae" );
				this.AddImage(86, 58, self.AvatarID);//Avatar Test
				
				
			}
			this.AddTextEntry( 92, 308, 324, 92, 0, 2, @"" + description);
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if ( m == null )
				return;
			
			if( info.ButtonID > -1 )
			{
				TextRelay target = info.GetTextEntry( 2 );

                if (info.ButtonID == 1)
                {
                    if (m_item != null && target.Text != null)
                    {
                        if(m != null && !m.Deleted && m.Alive && m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) > 2)
                        {
                            if(m_item is BaseArmor && (m_item as BaseArmor).Crafter == m && (m_item as BaseArmor).Quality == ArmorQuality.Masterwork)
                                m_item.Description = target.Text;
                            else if (m_item is BaseClothing && (m_item as BaseClothing).Crafter == m && (m_item as BaseClothing).Quality >= ClothingQuality.Masterwork)
                                m_item.Description = target.Text;
                            else if (m_item is BaseWeapon && (m_item as BaseWeapon).Crafter == m && (m_item as BaseWeapon).Quality == WeaponQuality.Masterwork)
                                m_item.Description = target.Text;
                            else if (m_item is BaseJewel && (m_item as BaseJewel).Crafter == m && (m_item as BaseJewel).Quality >= WeaponQuality.Masterwork)
                                m_item.Description = target.Text;
                        }

                        if (m_item is CustomFood && (m_item as CustomFood).Crafter == m && (m_item as CustomFood).Quality == FoodQuality.Masterpiece)
                            m_item.Description = target.Text;
                    }

                    if (m_bc != null && (m.AccessLevel > AccessLevel.Player || m_bc.ControlMaster == m))
                        m_bc.Description = target.Text;

                    if (m_self != null && m != null && target.Text != null)
                    {
                        if (m_page == 1)
                        {
                            if (m_self != null && m_self == m)
                                m_self.Description = target.Text;
                        }

                        else if (m_page == 2)
                        {
                            if (m_self != null && m_self == m)
                                m_self.Description2 = target.Text;
                        }

                        else if (m_page == 3)
                        {
                            if (m_self != null && m_self == m)
                                m_self.Description3 = target.Text;
                        }
                    }

                    m.SendGump(new LookGump(m, m_bc, m_self, m_item, m_page));
                    return;
                }
				
				if( info.ButtonID == 101 )
				{
					m_page--;
					
					if( m_page < 1 )
						m_page = 3;
					
					m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );
				}
				
				if( info.ButtonID == 102 )
				{
					m_page++;
					
					if( m_page > 3 )
						m_page = 1;
					
					m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );
				}
				
				if( info.ButtonID == 666  && m_item == null)
				{
					m.SendGump( new AvatarGump( m, m_page ) );
				}
                else if (info.ButtonID == 666 && m_item != null)
                {
                    m.SendGump(new LookGump(m, m_bc, m_self, m_item, m_page));
                }
			}
		}
	}
}
