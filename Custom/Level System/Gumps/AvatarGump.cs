using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Gumps
{
	public class AvatarGump : Gump
	{
		private PlayerMobile m_self = null;
		private int m_page;
		private BaseCreature m_bc = null;
		private Item m_item = null;
		
		int a1pg1 = 1000;
		int a2pg1 = 1001;
		int a3pg1 = 1002;
		int a4pg1 = 1040;
		int a5pg1 = 1041;
		int a6pg1 = 1042;
		int a1pg2 = 1003;
		int a2pg2 = 1004;
		int a3pg2 = 1005;
		int a4pg2 = 1043;
		int a5pg2 = 1044;
		int a6pg2 = 1045;
		int a1pg3 = 1006;
		int a2pg3 = 1007;
		int a3pg3 = 1008;
		int a4pg3 = 1046;
		int a5pg3 = 1047;
		int a6pg3 = 1048;
		int a1pg4 = 1009;
		int a2pg4 = 1010;
		int a3pg4 = 1011;
		int a4pg4 = 1049;
		int a5pg4 = 1050;
		int a6pg4 = 1051;		
		int a1pg5 = 1012;
		int a2pg5 = 1013;
		int a3pg5 = 1014;
		int a4pg5 = 1052;
		int a5pg5 = 1053;
		int a6pg5 = 1054;
		int a1pg6 = 1015;
		int a2pg6 = 1016;
		int a3pg6 = 1017;
		int a4pg6 = 1055;
		int a5pg6 = 1056;
		int a6pg6 = 1057;
		int a1pg7 = 1018;
		int a2pg7 = 1019;
		int a3pg7 = 1020;
		int a4pg7 = 1058;
		int a5pg7 = 1059;
		int a6pg7 = 1060;		
		int a1pg8 = 1021;
		int a2pg8 = 1022;
		int a3pg8 = 1023;
		int a4pg8 = 1061;
		int a5pg8 = 1062;
		int a6pg8 = 1063;
		int a1pg9 = 1024;
		int a2pg9 = 1025;
		int a3pg9 = 1026;
		int a4pg9 = 1064;
		int a5pg9 = 1065;
		int a6pg9 = 1066;			
		int a1pg10 = 1027;
		int a2pg10 = 1028;
		int a3pg10 = 1029;
		int a4pg10 = 1067;
		int a5pg10 = 1068;
		int a6pg10 = 1069;
		int a1pg11 = 1030;
		int a2pg11 = 1031;
		int a3pg11 = 1032;
		int a4pg11 = 1070;
		int a5pg11 = 1071;
		int a6pg11 = 1072;
		int a1pg12 = 1033;
		int a2pg12 = 1034;
		int a3pg12 = 1035;
		int a4pg12 = 1073;
		int a5pg12 = 1074;
		int a6pg12 = 1075;			
		
		int a1pg13 = 1001;			
		int a2pg13 = 1001;			
		int a3pg13 = 1001;			
		int a4pg13 = 1001;			
		int a5pg13 = 1001;			
		int a6pg13 = 1001;			
		
		
		public AvatarGump( PlayerMobile from, int page ): base( 0, 0 )
		{
			string description = "";
			m_page = page;
			
			from.CloseGump( typeof( AvatarGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			if (from.CustomAvatarID1 != 0)
			a1pg13 = from.CustomAvatarID1;		
			if (from.CustomAvatarID2 != 0)
			a2pg13 = from.CustomAvatarID2;		
			if (from.CustomAvatarID3 != 0)			
			a3pg13 = from.CustomAvatarID3;			


			AddPage(0);
			AddBackground(54, 31, 476, 549, 9270);
			AddImage(4, 10, 10440);
			AddImage(494, 10, 10441);
			AddBackground(74, 47, 139, 203, 9270);
			AddImage(86, 59, 30510, 2999);
			AddBackground(156, 0, 264, 34, 9270);
			AddLabel(244, 7, 148, @"Avatar Chooser");
			AddBackground(222, 47, 139, 203, 9270);
			AddImage(234, 59, 30510, 2999);
			AddBackground(370, 48, 139, 203, 9270);
			AddImage(382, 60, 30510, 2999);
			AddBackground(74, 47, 139, 203, 9270);
			AddImage(86, 59, 30510, 2999);
			AddBackground(222, 259, 139, 203, 9270);
			AddImage(234, 271, 30510, 2999);
			AddBackground(370, 260, 139, 203, 9270);
			AddImage(382, 272, 30510, 2999);
			AddBackground(74, 259, 139, 203, 9270);
			AddImage(86, 271, 30510, 2999);
			AddLabel(125, 475, 148, @"Click on an avatar to save it as your character portrait");
			AddLabel(111, 488, 148, @"Use the buttons below to view more pages or reward avatars");
			//AddButton(255, 517, 12006, 12008, 666, GumpButtonType.Reply, 0);
			AddButton(71, 537, 2640, 2641, 0, GumpButtonType.Page, 1);
			AddLabel(82, 517, 148, @"1");
			AddButton(102, 537, 2640, 2641, 0, GumpButtonType.Page, 2);
			AddLabel(113, 517, 148, @"2");
			AddButton(133, 537, 2640, 2641, 0, GumpButtonType.Page, 3);
			AddLabel(144, 517, 148, @"3");
			AddButton(164, 537, 2640, 2641, 0, GumpButtonType.Page, 4);
			AddLabel(175, 517, 148, @"4");
			AddButton(194, 537, 2640, 2641, 0, GumpButtonType.Page, 5);
			AddLabel(205, 517, 148, @"5");
			AddButton(225, 537, 2640, 2641, 0, GumpButtonType.Page, 6);
			AddLabel(236, 517, 148, @"6");
			AddButton(255, 537, 2640, 2641, 0, GumpButtonType.Page, 7);
			AddLabel(266, 517, 148, @"7");
			AddButton(286, 537, 2640, 2641, 0, GumpButtonType.Page, 8);
			AddLabel(297, 517, 148, @"8");
			AddButton(316, 537, 2640, 2641, 0, GumpButtonType.Page, 9);
			AddLabel(327, 517, 148, @"9");
			AddButton(347, 537, 2640, 2641, 0, GumpButtonType.Page, 10);
			AddLabel(354, 517, 148, @"10");
			AddButton(378, 537, 2640, 2641, 0, GumpButtonType.Page, 11);
			AddLabel(386, 517, 148, @"11");
			AddButton(409, 537, 2640, 2641, 0, GumpButtonType.Page, 12);
			AddLabel(415, 517, 148, @"12");
			AddButton(470, 537, 2640, 2641, 0, GumpButtonType.Page, 13);
			AddLabel(458, 517, 148, @"Rewards");
			
			
////BEGIN PAGES////
			
			AddPage(1);			
			AddButton(86, 59, a1pg1, a1pg1, 1, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg1, a2pg1, 2, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg1, a3pg1, 3, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg1, a4pg1, 4, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg1, a5pg1, 5, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg1, a6pg1, 6, GumpButtonType.Reply, 0);

			
			AddPage(2);			
			AddButton(86, 59, a1pg2, a1pg2, 7, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg2, a2pg2, 8, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg2, a3pg2, 9, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg2, a4pg2, 10, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg2, a5pg2, 11, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg2, a6pg2, 12, GumpButtonType.Reply, 0);

			
			AddPage(3);			
			AddButton(86, 59, a1pg3, a1pg3, 13, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg3, a2pg3, 14, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg3, a3pg3, 15, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg3, a4pg3, 16, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg3, a5pg3, 17, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg3, a6pg3, 18, GumpButtonType.Reply, 0);			

			
			
			AddPage(4);			
			AddButton(86, 59, a1pg4, a1pg4, 19, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg4, a2pg4, 20, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg4, a3pg4, 21, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg4, a4pg4, 22, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg4, a5pg4, 23, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg4, a6pg4, 24, GumpButtonType.Reply, 0);    

			
			AddPage(5);			
			AddButton(86, 59, a1pg5, a1pg5, 25, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg5, a2pg5, 26, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg5, a3pg5, 27, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg5, a4pg5, 28, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg5, a5pg5, 29, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg5, a6pg5, 30, GumpButtonType.Reply, 0);

			
			AddPage(6);			
			AddButton(86, 59, a1pg6, a1pg6, 31, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg6, a2pg6, 32, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg6, a3pg6, 33, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg6, a4pg6, 34, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg6, a5pg6, 35, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg6, a6pg6, 36, GumpButtonType.Reply, 0);

			
			AddPage(7);			
			AddButton(86, 59, a1pg7, a1pg7, 37, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg7, a2pg7, 38, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg7, a3pg7, 39, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg7, a4pg7, 40, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg7, a5pg7, 41, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg7, a6pg7, 42, GumpButtonType.Reply, 0);

			
			AddPage(8);			
			AddButton(86, 59, a1pg8, a1pg8, 43, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg8, a2pg8, 44, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg8, a3pg8, 45, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg8, a4pg8, 46, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg8, a5pg8, 47, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg8, a6pg8, 48, GumpButtonType.Reply, 0);

			
			AddPage(9);			
			AddButton(86, 59, a1pg9, a1pg9, 49, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg9, a2pg9, 50, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg9, a3pg9, 51, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg9, a4pg9, 52, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg9, a5pg9, 53, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg9, a6pg9, 54, GumpButtonType.Reply, 0);

			
			AddPage(10);			
			AddButton(86, 59, a1pg10, a1pg10, 55, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg10, a2pg10, 56, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg10, a3pg10, 57, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg10, a4pg10, 58, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg10, a5pg10, 59, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg10, a6pg10, 60, GumpButtonType.Reply, 0);			

			
			AddPage(11);			
			AddButton(86, 59, a1pg11, a1pg11, 61, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg11, a2pg11, 62, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg11, a3pg11, 63, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg11, a4pg11, 64, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg11, a5pg11, 65, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg11, a6pg11, 66, GumpButtonType.Reply, 0);		

			
			AddPage(12);			
			AddButton(86, 59, a1pg12, a1pg12, 67, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg12, a2pg12, 68, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg12, a3pg12, 69, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg12, a4pg12, 70, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg12, a5pg12, 71, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg12, a6pg12, 72, GumpButtonType.Reply, 0);			
			
			AddPage(13);			
			AddButton(86, 59, a1pg13, a1pg13, 73, GumpButtonType.Reply, 0);
			AddButton(234, 59, a2pg13, a2pg13, 74, GumpButtonType.Reply, 0);
			AddButton(382, 60, a3pg13, a3pg13, 75, GumpButtonType.Reply, 0);
			AddButton(86, 271, a4pg13, a4pg13, 76, GumpButtonType.Reply, 0);
			AddButton(234, 271, a5pg13, a5pg13, 77, GumpButtonType.Reply, 0);
			AddButton(382, 272, a6pg13, a6pg13, 78, GumpButtonType.Reply, 0);
        }

        

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile m = sender.Mobile as PlayerMobile;
			PlayerMobile m_self = sender.Mobile as PlayerMobile;
			
			switch(info.ButtonID)
            {
				//page1
                case 1:		m.AvatarID = a1pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 2:		m.AvatarID = a2pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 3:		m.AvatarID = a3pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 4:		m.AvatarID = a4pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 5:		m.AvatarID = a5pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 6:		m.AvatarID = a6pg1;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;		
				//page2
                case 7:		m.AvatarID = a1pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 8:		m.AvatarID = a2pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 9:		m.AvatarID = a3pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 10:	m.AvatarID = a4pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 11:	m.AvatarID = a5pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 12:	m.AvatarID = a6pg2;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;		
				//page3
                case 13:		m.AvatarID = a1pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 14:		m.AvatarID = a2pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 15:		m.AvatarID = a3pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 16:	m.AvatarID = a4pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 17:	m.AvatarID = a5pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 18:	m.AvatarID = a6pg3;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page4
                case 19:		m.AvatarID = a1pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 20:		m.AvatarID = a2pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 21:		m.AvatarID = a3pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 22:	m.AvatarID = a4pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 23:	m.AvatarID = a5pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 24:	m.AvatarID = a6pg4;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;								
				//page5
                case 25:		m.AvatarID = a1pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 26:		m.AvatarID = a2pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 27:		m.AvatarID = a3pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 28:	m.AvatarID = a4pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 29:	m.AvatarID = a5pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 30:	m.AvatarID = a6pg5;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;								
				//page6
                case 31:		m.AvatarID = a1pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 32:		m.AvatarID = a2pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 33:		m.AvatarID = a3pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 34:	m.AvatarID = a4pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 35:	m.AvatarID = a5pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 36:	m.AvatarID = a6pg6;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;							
				//page7
                case 37:		m.AvatarID = a1pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 38:		m.AvatarID = a2pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 39:		m.AvatarID = a3pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 40:	m.AvatarID = a4pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 41:	m.AvatarID = a5pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 42:	m.AvatarID = a6pg7;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page8
                case 43:		m.AvatarID = a1pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 44:		m.AvatarID = a2pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 45:		m.AvatarID = a3pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 46:	m.AvatarID = a4pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 47:	m.AvatarID = a5pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 48:	m.AvatarID = a6pg8;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page9
                case 49:		m.AvatarID = a1pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 50:		m.AvatarID = a2pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 51:		m.AvatarID = a3pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 52:	m.AvatarID = a4pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 53:	m.AvatarID = a5pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 54:	m.AvatarID = a6pg9;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page10
                case 55:		m.AvatarID = a1pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 56:		m.AvatarID = a2pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 57:		m.AvatarID = a3pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 58:	m.AvatarID = a4pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 59:	m.AvatarID = a5pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 60:	m.AvatarID = a6pg10;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page11
                case 61:		m.AvatarID = a1pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 62:		m.AvatarID = a2pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 63:		m.AvatarID = a3pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 64:	m.AvatarID = a4pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 65:	m.AvatarID = a5pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 66:	m.AvatarID = a6pg11;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
				//page12
                case 67:		m.AvatarID = a1pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 68:		m.AvatarID = a2pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 69:		m.AvatarID = a3pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 70:	m.AvatarID = a4pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 71:	m.AvatarID = a5pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 72:	m.AvatarID = a6pg12;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;	
							
				//page13
                case 73:		m.AvatarID = a1pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 74:		m.AvatarID = a2pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 75:		m.AvatarID = a3pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
				case 76:	m.AvatarID = a4pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 77:	m.AvatarID = a5pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;
                case 78:	m.AvatarID = a6pg13;	
							m.CloseGump( typeof( AvatarGump ) );m.CloseGump( typeof( LookGump ) );m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );break;									
			}
				
				
			if( info.ButtonID == 666 )
				{
				m_page = 1;
				m.CloseGump( typeof( AvatarGump ) );
				m.CloseGump( typeof( LookGump ) );
				m.SendGump( new LookGump( m, m_bc, m_self, m_item, m_page ) );
				}	
			
			

			if( info.ButtonID == 900 )
				{
					m_page = m_page - 1;
					
					if( m_page < 1 )
						m_page = 3; //lastpage
					
					m.CloseGump( typeof( AvatarGump ) );
					m.SendGump( new AvatarGump( m, m_page ) );
				}
				
				if( info.ButtonID == 100 )
				{
					m_page = m_page + 1;
					
					if( m_page > 3 )//last page
						m_page = 1;
					
					m.CloseGump( typeof( AvatarGump ) );
					m.SendGump( new AvatarGump( m, m_page ) );
				}
        }
    }
}