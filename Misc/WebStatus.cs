using System;
using System.IO;
using System.Text;
using System.Collections;
using Server;
using Server.Network;
using Server.Guilds;
using Server.Mobiles;

namespace Server.Misc
{
	public class StatusPage : Timer
	{
		public static bool LiveServer = true;
		public static string WebFolder = LiveServer == true ? @"C:\Inetpub\wwwroot" : @"C:\Khaeros\Web";
		
		public static void Initialize()
		{
			new StatusPage().Start();
		}

		public StatusPage() : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromMinutes( 5.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
		}

		private static string Encode( string input )
		{
			StringBuilder sb = new StringBuilder( input );

			sb.Replace( "&", "&amp;" );
			sb.Replace( "<", "&lt;" );
			sb.Replace( ">", "&gt;" );
			sb.Replace( "\"", "&quot;" );
			sb.Replace( "'", "&apos;" );

			return sb.ToString();
		}
		
		public static void PublishWikiStatus()
		{
			using ( StreamWriter op = new StreamWriter( WebFolder + @"\Wiki\data\pages\status.txt" ) )
			{
				int players = 0;
				
				foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;

					if ( m != null && m is PlayerMobile && !( (PlayerMobile)m ).HideStatus )
						players++;
				}
				
				op.WriteLine( @"~~NOCACHE~~" );
				op.WriteLine( @"====== Shard Status ======" );
				op.WriteLine( @"\\" );
				op.WriteLine( @"\\" );
				op.WriteLine( @"Online Players: " + players.ToString() + @"\\" );
				op.WriteLine( @"\\" );
				op.WriteLine( @"**Player List** \\" );
				op.WriteLine( @"\\" );

				foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;

					if ( m != null && m is PlayerMobile && !( (PlayerMobile)m ).HideStatus )
						op.WriteLine( @"" + m.Name + ", " + ((PlayerMobile)m).RPTitle + @"\\" );
				}
				
				op.WriteLine( @"\\" );
				op.WriteLine( @"\\" );
				op.WriteLine( @"====== Current Time ======" );
				op.WriteLine( @"The time, according to [[guides:universal_shard_time|UST]], is currently: " );
				op.WriteLine( @"<php>" );
				op.WriteLine( @"echo '<B>';" );
				op.WriteLine( @";echo date('l jS \of F Y h:i:s A');" );
				op.WriteLine( @";echo '</B>';" );
				op.WriteLine( @"</php>" );
			}
		}

		protected override void OnTick()
		{
			if( LiveServer )
				PublishWikiStatus();
			
			using ( StreamWriter op = new StreamWriter( WebFolder + @"\status.htm" ) )
			{
				int players = 0;
				
				foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;

					if ( m != null && m is PlayerMobile && !( (PlayerMobile)m ).HideStatus )
						players++;
				}
				
				op.WriteLine( "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" );
				op.WriteLine( "<html xmlns=\"http://www.w3.org/1999/xhtml\">" );
				op.WriteLine( "<head>" );
				op.WriteLine( "<title>Khaeros</title>" );
				op.WriteLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" );
				op.WriteLine( "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />" );
				op.WriteLine( "<link href=\"tutorsty.css\" rel=\"stylesheet\" type=\"text/css\" />" );
				op.WriteLine( "<link href=\"flexcrollstyles.css\" rel=\"stylesheet\" type=\"text/css\" />" );
				op.WriteLine( "<style type=\"text/css\">" );
				op.WriteLine( ".p3 {font-size:10pt; color:#ffffff;}" );
				op.WriteLine( "</style></head>" );
				op.WriteLine( "<script type='text/javascript' src=\"flexcroll.js\"></script>" );
				op.WriteLine( "<body background=\"khaeros_arquivos/bg.gif\" text=\"#ffffff\">" );
				op.WriteLine( "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\"><tbody><tr valign=\"top\"><td align=\"center\">" );
				op.WriteLine( "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"600\" width=\"760\">" );
				op.WriteLine( "<tbody><tr><td colspan=\"5\"><img src=\"khaeros_arquivos/bdr_top.gif\" alt=\"\" height=\"15\" width=\"760\"></td></tr>" );
				op.WriteLine( "<tr><td colspan=\"5\"><img src=\"khaeros_arquivos/header.gif\" alt=\"\" height=\"110\" width=\"760\"></td></tr>" );
				op.WriteLine( "<tr valign=\"top\">" );
				op.WriteLine( "<td><img src=\"khaeros_arquivos/bdr_lft.gif\" alt=\"\" height=\"460\" width=\"15\"></td>" );
				op.WriteLine( "<!-- MENU -->	<td><img src=\"khaeros_arquivos/menu.gif\" usemap=\"#menumap\" border=\"0\" height=\"460\" width=\"120\"></td>" );
				op.WriteLine( "<td><img src=\"khaeros_arquivos/bdr_ctr.gif\" alt=\"\" height=\"460\" width=\"15\"></td>" );
				op.WriteLine( "<td align=\"center\" height=\"460\" width=\"595\">" );
				op.WriteLine( "<!-- MAIN -->" );
				op.WriteLine( "<div id='mycustomscroll' class='flexcroll'>" );
				op.WriteLine( "<div class='lipsum'>" );
				op.WriteLine( "<p class=\"MsoNormal\" style=\"text-align: justify\">" );
				op.WriteLine( "<font color=\"#CC9900\" size=\"4\">Status</font></p>" );
				op.WriteLine( "<p class=\"MsoNormal\" style=\"text-align: justify\">" );
				op.WriteLine( "&nbsp;</p>" );
				op.WriteLine( "<p class=\"MsoNormal\" style=\"text-align: justify\">" );
				//op.WriteLine( "<span style=\"font-size: 10.0pt\">- Status section.</span></p>" );
				op.WriteLine( "       Online Players: " + players + "<br>" );
				op.WriteLine( "      <table width=\"100%\">" );
				op.WriteLine( "         <tr>" );
				op.WriteLine( "            <td bgcolor=\"black\"><b><font color=\"#CC9900\">Player List</font></b></td>" );
				op.WriteLine( "         </tr>" );
				

				foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;

					if ( m != null && m is PlayerMobile && !( (PlayerMobile)m ).HideStatus )
					{
						op.Write( "         <tr><td>" );

						op.Write( Encode( m.Name + ", " + ( (PlayerMobile)m ).RPTitle ) );

					}
				}

				op.WriteLine( "         <tr>" );
				op.WriteLine( "      </table>" );
				op.WriteLine( "</div>" );
				op.WriteLine( "</div>" );
				op.WriteLine( "</td>" );
				op.WriteLine( "<td><img src=\"khaeros_arquivos/bdr_rgt.gif\" alt=\"\" height=\"460\" width=\"15\"></td>" );
				op.WriteLine( "</tr>" );
				op.WriteLine( "<tr><td colspan=\"5\" background=\"khaeros_arquivos/bdr_btm.gif\" height=\"15\" width=\"760\"></td></tr>" );
				op.WriteLine( "</tbody></table>" );
				op.WriteLine( "</td></tr></tbody></table>" );
				op.WriteLine( "<map name=\"menumap\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 40, 120, 65\" target=\"_self\" href=\"http://www.khaeros.com/khaeros.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 65, 120, 85\" target=\"_self\" href=\"http://www.khaeros.com/rules.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 85, 120, 105\" target=\"_self\" href=\"http://www.khaeros.com/forums\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 105, 120, 125\" target=\"_self\" href=\"http://server.khaeros.com/Status.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 125, 120, 147\" target=\"_self\" href=\"http://www.khaeros.com/classes.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 195, 120, 215\" target=\"_self\" href=\"http://www.khaeros.com/Northern.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 215, 120, 240\" target=\"_self\" href=\"http://www.khaeros.com/Tirebladd.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 240, 120, 260\" target=\"_self\" href=\"http://www.khaeros.com/Southern.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 260, 120, 283\" target=\"_self\" href=\"http://www.khaeros.com/Haluaroc.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 283, 120, 305\" target=\"_self\" href=\"http://www.khaeros.com/Western.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 305, 120, 325\" target=\"_self\" href=\"http://www.khaeros.com/mhordul.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 350, 120, 370\" target=\"_self\" href=\"http://www.khaeros.com/shop.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 370, 120, 390\" target=\"_self\" href=\"http://www.khaeros.com/donations.htm\">" );
				op.WriteLine( "<area shape=\"rect\" coords=\"0, 390, 120, 415\" target=\"_self\" href=\"http://www.khaeros.com/staff.htm\">" );
				op.WriteLine( "</map></body></html>" );

				
			}			
		}
	}
}
