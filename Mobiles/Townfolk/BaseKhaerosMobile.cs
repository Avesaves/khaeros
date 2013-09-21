using System; 
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
	public class BaseKhaerosMobile : BaseCreature 
	{
		private bool m_WillArrest;
		
		public bool WillArrest
		{
			get{ return m_WillArrest; }
			set{ m_WillArrest = value; }
		}

		public BaseKhaerosMobile( Nation n ) : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 ) 
		{
            if (!(this is Soldier))
            {
                if (this is ISouthern)
                    this.Nation = Nation.Southern;
                else if (this is IWestern)
                    this.Nation = Nation.Western;
                else if (this is IHaluaroc)
                    this.Nation = Nation.Haluaroc;
                else if (this is IMhordul)
                    this.Nation = Nation.Mhordul;
                else if (this is ITirebladd)
                    this.Nation = Nation.Tirebladd;
                else if (this is INorthern)
                    this.Nation = Nation.Northern;
/*                 else if (this is IImperial)
                    this.Nation = Nation.Imperial; */
            }

			SpeechHue = Utility.RandomDyedHue();

            Nation nation = n;

            switch (n)
            {
 /*                case Nation.Imperial:
                    {
                        int raceChance = Utility.Random(7);
                        switch (raceChance)
                        {
                            case 0: nation = Nation.Northern; break;
                            case 1: nation = Nation.Northern; break;
                            case 2: nation = Nation.Northern; break;
                            case 3: nation = Nation.Tirebladd; break;
                            case 4: nation = Nation.Tirebladd; break;
                            case 5: nation = Nation.Tirebladd; break;
                            case 6: nation = Nation.Haluaroc; break;

                            default: nation = Nation.Northern; break;
                        }

                        break;
                    }
                case Nation.Sovereign:        
                    {
                        int raceChance = Utility.Random(8);
                        switch (raceChance)
                        {
                            case 0: nation = Nation.Mhordul; break;
                            case 1: nation = Nation.Mhordul; break;
                            case 2: nation = Nation.Mhordul; break;
                            case 3: nation = Nation.Southern; break;
                            case 4: nation = Nation.Southern; break;
                            case 5: nation = Nation.Southern; break;
                            case 6: nation = Nation.Western; break;

                            default: nation = Nation.Mhordul; break;
                        }

                        break;
                    } */
 /*                case Nation.Society:
                    {
                        int raceChance = Utility.Random(6);

                        switch (raceChance)
                        {
                            case 0: nation = Nation.Southern; break;
                            case 1: nation = Nation.Western; break;
                            case 2: nation = Nation.Haluaroc; break;
                            case 3: nation = Nation.Mhordul; break;
                            case 4: nation = Nation.Tirebladd; break;
                            case 5: nation = Nation.Northern; break;

                            default: nation = Nation.Haluaroc; break;
                        }

                        break;
                    } */
/* 				case Nation.Insularii:
                    {
                        int raceChance = Utility.Random(6);

                        switch (raceChance)
                        {
                            case 0: nation = Nation.Northern; break;
                            case 1: nation = Nation.Northern; break;
                            case 2: nation = Nation.Haluaroc; break;
                            case 3: nation = Nation.Mhordul; break;
                            case 4: nation = Nation.Southern; break;
                            case 5: nation = Nation.Northern; break;

                            default: nation = Nation.Northern; break;
                        }

                        break;
                    } */
            }

			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 0x191;
                this.Name = RandomName(nation, true) + RandomSurname(nation, true);
			} 
			
			else 
			{ 
				this.Body = 0x190;
                this.Name = BaseKhaerosMobile.RandomName(nation, false) + RandomSurname(nation, false);
			} 

			SetStr( 50 );
			SetDex( 50 );
			SetInt( 50 );
			SetHits( 25 );

			SetDamage( 2, 3 );

			SetSkill( SkillName.Anatomy, 25.0 );
			SetSkill( SkillName.Fencing, 25.0 );
			SetSkill( SkillName.Macing, 25.0 );
			SetSkill( SkillName.Swords, 25.0 );
			SetSkill( SkillName.Tactics, 25.0 );
			SetSkill( SkillName.Polearms, 25.0 );
			SetSkill( SkillName.ExoticWeaponry, 25.0 );
			SetSkill( SkillName.Axemanship, 25.0 );

			VirtualArmor = 40;

			Hue = AssignRacialHue( nation );
			HairItemID = AssignRacialHair( nation, this.Female );
			int hairhue = AssignRacialHairHue( nation );
			HairHue = hairhue;
			
			Karma = -2000;
			Fame = 100;
			Criminal = true;

            if (!this.Female)
            {
                FacialHairItemID = AssignRacialFacialHair(nation);
                FacialHairHue = hairhue;
            }
            else
                FacialHairItemID = 0;
			
			if( this.Backpack == null )
				AddItem( new Backpack() );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
            //if( WillArrest && defender is PlayerMobile && defender != null )
            //{
            //    GuardBoard board = this.RacialGuardBoard;
            //    defender.Location = RacialGuardBoard.JailLocation;
            //    defender.SendMessage( "You are going to remain in jail for " + RacialGuardBoard.JailTime.TotalMinutes + " minutes. After that, you will be automatically released." );
            //    new PrisonTimer( defender, RacialGuardBoard.JailTime, RacialGuardBoard.SetFreeLocation ).Start();
				
            //    if( board.ArrestByNameList.Contains( defender.Name ) )
            //        board.ArrestByNameList.Remove( defender.Name );
            //}

            base.OnGaveMeleeAttack(defender);
		}
		
		public class PrisonTimer : Timer
        {
            private Mobile m_m;
            private Point3D m_location;

            public PrisonTimer( Mobile m, TimeSpan jailtime, Point3D location )
                : base( jailtime )
            {
                m_m = m;
                m_location = location;
            }

            protected override void OnTick()
            {
                m_m.Location = m_location;
            }
        }
		
		public static int AssignRacialHair( Nation nation, bool female )
		{
			int hair = 0;
			int options = 20;
			
			switch( nation )
			{
				case Nation.Southern: options = 16; break;
				case Nation.Western: options = 17; break;
				case Nation.Haluaroc: options = 15; break;
				case Nation.Mhordul: options = 14; break;
				case Nation.Tirebladd: options = 16; break;
				case Nation.Northern: options = 13; break;
			}
			
			switch( Utility.Random( options ) )
			{
				case 0:	return female == true ? 8251: 0;
				case 1:	return 8251;
				case 2:	return 8252;
				case 3:	return 8253;
				case 4:	return 8265;
				case 5:	return female == true ? 8252 : 8264;
				case 6:	return 12742;
				case 7:	return 12744;
				case 8:	return 12751;
				case 9:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12738;
						case Nation.Western: return 12738;
						case Nation.Haluaroc: return 12738;
						case Nation.Mhordul: return 12747;
						case Nation.Tirebladd: return 12738;
						case Nation.Northern: return 12749;
					}
					break;
				}
				case 10:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12749;
						case Nation.Western: return 12741;
						case Nation.Haluaroc: return 12746;
						case Nation.Mhordul: return 12757;
						case Nation.Tirebladd: return 12749;
						case Nation.Northern: return 8261;
					}
					break;
				}
				case 11:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12750;
						case Nation.Western: return 12748;
						case Nation.Haluaroc: return 12748;
						case Nation.Mhordul: return 12241;
						case Nation.Tirebladd: hair = 12750; break;
						case Nation.Northern: hair = 8262; break;
					}
					break;
				}
				case 12:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12757;
						case Nation.Western: return 12745;
						case Nation.Haluaroc: return 12749;
						case Nation.Mhordul: return 8260;
						case Nation.Tirebladd: hair = 12757; break;
						case Nation.Northern: hair = 12753; break;
					}
					break;
				}
				case 13:
				{
					switch ( nation )
					{		
						case Nation.Southern: hair = 8262; break;
						case Nation.Western: return 12241;
						case Nation.Haluaroc: return 12757;
						case Nation.Mhordul: hair = 8262; break;
						case Nation.Tirebladd: hair = 8262; break;
						case Nation.Northern: hair = 12752; break;
					}
					break;
				}
				case 14:
				{
					switch ( nation )
					{		
						case Nation.Southern: hair = 12753; break;
						case Nation.Western: return 8263;
						case Nation.Haluaroc: hair = 8262; break;
						case Nation.Mhordul: hair = 12753; break;
						case Nation.Tirebladd: return 12753;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 15:
				{
					switch ( nation )
					{		
						case Nation.Southern: hair = 12752; break;
						case Nation.Western: return 8266;
						case Nation.Haluaroc: hair = 12753; break;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 12752;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 16:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 0;
						case Nation.Western: hair = 8262; break;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 17:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 0;
						case Nation.Western: hair = 12753; break;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
			}
			
			if( !female )
				hair = 0;
			
			return hair;
		}
		
		public static int AssignRacialHairHue( Nation nation )
		{
			switch ( nation )
			{		
				case Nation.Western:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2801;
						case 1:	return 2411;
						case 2:	return 1140;
						case 3:	return 1109;
						case 4:	return 2990;
						case 5:	return 2406;
						case 6:	return 1175;
						case 7:	return 1881;
						case 8:	return 1989;
						case 9:	return 2992;
					}
					break;
				}
					
				case Nation.Haluaroc:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2801;
						case 1:	return 2411;
						case 2:	return 1140;
						case 3:	return 1109;
						case 4:	return 2990;
						case 5:	return 2406;
						case 6:	return 1175;
						case 7:	return 1881;
						case 8:	return 1989;
						case 9:	return 2992;
					}
					break;
				}
					
				case Nation.Northern:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2990;
						case 1:	return 1147;
						case 2:	return 1058;
						case 3:	return 1118;
						case 4:	return 1127;
						case 5:	return 1149;
						case 6:	return 1145;
						case 7:	return 1103;
						case 8:	return 1120;
						case 9:	return 1509;
					}
					break;
				}
					
				case Nation.Tirebladd:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1627;
						case 1:	return 1149;
						case 2:	return 1607;
						case 3:	return 1147;
						case 4:	return 2312;
						case 5:	return 1627;
						case 6:	return 1645;
						case 7:	return 2303;
						case 8:	return 1617;
						case 9:	return 2206;
					}
					break;
				}
					
				case Nation.Southern:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2107;
						case 1:	return 1149;
						case 2:	return 2413;
						case 3:	return 2418;
						case 4:	return 2312;
						case 5:	return 1627;
						case 6:	return 1830;
						case 7:	return 2303;
						case 8:	return 1845;
						case 9:	return 2206;
					}
					break;
				}
					
				case Nation.Mhordul:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1149;
						case 1:	return 1022;
						case 2:	return 1141;
						case 3:	return 2990;
						case 4:	return 1109;
						case 5:	return 1190;
						case 6:	return 2799;
						case 7:	return 2801;
						case 8:	return 2992;
						case 9:	return 1108;
					}
					break;
				}
			}
			return 0;
		}
		
		public static int AssignRacialHue( Nation nation )
		{
			switch ( nation )
			{		
				case Nation.Western:
				{
					switch( Utility.Random( 9 ) )
					{
						//case 0:	return 1133;
						//case 1:	return 1132;
						//case 2:	return 1131;
						//case 3:	return 1130;
						//case 4:	return 1190;
						case 0: return 1005;
						case 1: return 1850;
						case 2: return 1812;
						case 3: return 1849;
						case 4: return 1815;
						case 5:	return 1148;
						case 6:	return 1147; 
						case 7:	return 1146;
						case 8:	return 1145; 
						case 9:	return 1144;
					}
					break;
				}
			
				case Nation.Haluaroc:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1005;
						case 1:	return 1850;
						case 2:	return 1812;
						case 3:	return 1849;
						case 4:	return 1815;
						case 5:	return 1148;
						case 6:	return 1147;
						case 7:	return 1146;
						case 8:	return 1145;
						case 9:	return 1144;
					}
					break;
				}
					
				case Nation.Northern:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1018;
						case 1:	return 1025;
						case 2: return 1851;
						case 3: return 1145;
						//case 2:	return 1030;
					//	case 3:	return 1037;
						case 4:	return 1005;
						case 5:	return 1012;
						case 6:	return 1019;
						case 7:	return 1026;
						case 8:	return 1031;
						case 9:	return 1065;
					}
					break;
				}
					
				case Nation.Tirebladd:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1062;
						case 1:	return 1061;
						case 2:	return 1063;
						case 3:	return 1064;
						case 4:	return 1065;
						case 5:	return 1066;
						case 6:	return 1023;
						case 7:	return 1030;
						case 8:	return 1037;
						case 9:	return 1045;
					}
					break;
				}
					
				case Nation.Southern:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1062;
						case 1:	return 1061;
						case 2: return 1849;
						//case 2:	return 1063;
						case 3:	return 1064;
						case 4:	return 1065;
						case 5:	return 1066;
						case 6: return 1019;
						case 7: return 1003;
						//case 6:	return 1023;
						//case 7:	return 1030;
						case 8:	return 1037;
						case 9: return 1144;
						//case 9:	return 1045;
					}
					break;
				}
					
				case Nation.Mhordul:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1849;
						case 1:	return 2425;
						case 2:	return 1009;
						case 3:	return 1010;
						case 4:	return 2426; 
						case 5:	return 1017;
						case 6:	return 1011;
						case 7:	return 2425; 
						case 8:	return 2423;
						case 9:	return 1072; 
					}
					break;
				}
			}
			return 0;
		}
		
		public static int AssignRacialFacialHair( Nation nation )
		{
			switch( Utility.Random( 23 ) )
			{
				case 0:	return 0;
				case 1:	return 8257;
				case 2:	return 12720;
				case 3:	return 8256;
				case 4:	return 12721;
				case 5:	return 8269;
				case 6:	return 12722;
				case 7:	return 8255;
				case 8:	return 8267;
				case 9:	return 12727;
				case 10: return 12728;
				case 11:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12725;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 12725;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 12725;
					}
					break;
				}
				case 12:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12726;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 12726;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 12726;
					}
					break;
				}
				case 13:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12737;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 12737;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 12737;
					}
					break;
				}
				case 14:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 8254;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 8254;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 8254;
					}
					break;
				}
				case 15:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 8268;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 8268;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 8268;
					}
					break;
				}
				case 16:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12731;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 12731;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 12731;
					}
					break;
				}
				case 17:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12730;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 12730;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 12730;
					}
					break;
				}
				case 18:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12733;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 19:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12735;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 20:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 12729;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 21:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 0;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 22:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 0;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
				case 23:
				{
					switch ( nation )
					{		
						case Nation.Southern: return 0;
						case Nation.Western: return 0;
						case Nation.Haluaroc: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tirebladd: return 0;
						case Nation.Northern: return 0;
					}
					break;
				}
			}
			return 0;
		}
		
		public static string GiveInsulariiName( bool female )
		{
			string namelist = "";
			
			if( female )
			{
				namelist = "Tallia,Nintinia,Hala,Pola,Gratumna,Cassia,Sulvia,Quatia,Pellia,Tunia,Traenia," +
					"Plena,Lullia,Conia,Minia,Lina,Tillia,Pulvula,Sautia,Larania,Somaria,Purnia,Milinia," +
					"Agrinia,Clena,Velva,Stallia,Agrutia,Stera,Cinia,Cessulvia,Closia,Punia,Rutallia,Dama," +
					"Stravoria,Silia,Glatia,Pania,Ascia,Vala,Helina,Tramecia,Crilunia,Lutia," +
					"Pullia,Claenia,Drullecia,Laenia,Pellia";
			}
			
			else
			{
				namelist = "Quarto,Vintius,Varus,Vinnius,Quarrus,Nentius,Ganus,Quisius,Gaius,Caronius," +
					"Fectius,Egnitis,Vallitus,Cuvonius,Trucius,Sorius,Maelius,Selius,Crevian,Durio," +
					"Viralian,Stanus,Mectris,Aralius,Plasus,Ussius,Dentius,Frallerian,Delius,Atis," +
					"Trellius,Migius,Armatius,Trasucian,Gelacius,Malchio,Gragavius,Pivius,Marotius," +
					"Polimius,Allius,Aetirius,Castianus,Flagius,Audallirus,Atturus,Caerius," +
					"Selus,Parius,Brunis";
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
		}
		
		public static string RandomName( Nation nation, bool female )
		{
			string namelist = "";
			
			switch( nation )
			{
				case Nation.Southern:
				{
					if( female )
			{
				namelist = "Tallia,Nintinia,Hala,Pola,Gratumna,Cassia,Sulvia,Quatia,Pellia,Tunia,Traenia," +
					"Plena,Lullia,Conia,Minia,Lina,Tillia,Pulvula,Sautia,Larania,Somaria,Purnia,Milinia," +
					"Agrinia,Clena,Velva,Stallia,Agrutia,Stera,Cinia,Cessulvia,Closia,Punia,Rutallia,Dama," +
					"Stravoria,Silia,Glatia,Pania,Ascia,Vala,Helina,Tramecia,Crilunia,Lutia," +
					"Pullia,Claenia,Drullecia,Laenia,Pellia";
			}
			
			else
			{
				namelist = "Quarto,Vintius,Varus,Vinnius,Quarrus,Nentius,Ganus,Quisius,Gaius,Caronius," +
					"Fectius,Egnitis,Vallitus,Cuvonius,Trucius,Sorius,Maelius,Selius,Crevian,Durio," +
					"Viralian,Stanus,Mectris,Aralius,Plasus,Ussius,Dentius,Frallerian,Delius,Atis," +
					"Trellius,Migius,Armatius,Trasucian,Gelacius,Malchio,Gragavius,Pivius,Marotius," +
					"Polimius,Allius,Aetirius,Castianus,Flagius,Audallirus,Atturus,Caerius," +
					"Selus,Parius,Brunis";
			}
					
					break;
				}
					
				case Nation.Western:
				{
					if( female )
					{
						namelist = "Esra,Vara,Xi,Zi,Chih,Lozua,Mela,Anixa,Ana" +
							"Tali,Ildja,Kulla,Tisa,Dihya,Takirri,Zahrit,Zenet,Tiri," +
							"Atla,Ata,Dia,Essa,Extia,Faris,Izela,Ixela,Fazria,Raza," +
							"Hadia,Hala,Hani,Isa,Izra,Izza,Khadija,Lamia,Lamya,Khalifa," +
							"Canari,Layla,Lina,Lixtli,Luffi,Maha,Maali,Maram,Mirza," +
							"Muna,Nada,Nadya,Nura,Raisa,Ranya,Rana,Rua,Ruya,Safi,Sakina," +
							"Sana,Sani,Sara,Saria,Zelesi,Shahrazad,Shazi,Thana,Uzma,Thaone," +
							"Uzme,Exma,Yasmin,Zahra,Zaina,Zia,Aradia";
					}
					
					else
					{
						namelist = "Coatl,Ataxl,Camaxtli,Opazin,Coaxri,Atla,Diaxl," +
							"Farix,Fakri,Raz,Ixandar,Jafar,Janan,Khalil,Maali,Malik," +
							"Xalik,Mazin,Navid,Ra'lai,Raox,Raiz'ai,Ran'xia,Aran,Raotl" +
							"Toa,Tariq,Sain,Zain,Mazin,Tarix,Farix,Feran,Exiti,Zulfiqar," +
							"Usan,Maki,Zia,Thaon,Tirix,Canar,Aladl,Takir,Zenl,Aradl," +
							"Cuixico,Xomal,Mehen,Ma'alai,Xin'alai,Zain'al";
					}
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					if( female )
					{
						namelist = "Esra,Vara,Xi,Zi,Chih,Lozua,Mela,Anixa,Ana" +
							"Tali,Ildja,Kulla,Tisa,Dihya,Takirri,Zahrit,Zenet,Tiri," +
							"Atla,Ata,Dia,Essa,Extia,Faris,Izela,Ixela,Fazria,Raza," +
							"Hadia,Hala,Hani,Isa,Izra,Izza,Khadija,Lamia,Lamya,Khalifa," +
							"Canari,Layla,Lina,Lixtli,Luffi,Maha,Maali,Maram,Mirza," +
							"Muna,Nada,Nadya,Nura,Raisa,Ranya,Rana,Rua,Ruya,Safi,Sakina," +
							"Sana,Sani,Sara,Saria,Zelesi,Shahrazad,Shazi,Thana,Uzma,Thaone," +
							"Uzme,Exma,Yasmin,Zahra,Zaina,Zia,Aradia";
					}
					
					else
					{
						namelist = "Coatl,Ataxl,Camaxtli,Opazin,Coaxri,Atla,Diaxl," +
							"Farix,Fakri,Raz,Ixandar,Jafar,Janan,Khalil,Maali,Malik," +
							"Xalik,Mazin,Navid,Ra'lai,Raox,Raiz'ai,Ran'xia,Aran,Raotl" +
							"Toa,Tariq,Sain,Zain,Mazin,Tarix,Farix,Feran,Exiti,Zulfiqar," +
							"Usan,Maki,Zia,Thaon,Tirix,Canar,Aladl,Takir,Zenl,Aradl," +
							"Cuixico,Xomal,Mehen,Ma'alai,Xin'alai,Zain'al";
					}
					
					break;
				}
					
				case Nation.Mhordul:
				{
					if( female )
					{
						namelist = "Rhogal,Rakhel,Rhana,Jegira,Tizal,Tilya,Ghnissa," +
							"Jdira,Kessa,Tilla,Ilya,Zohna,Xerena,Xeressa,Enzela,Sinja," +
							"Ralle,Anae,Silrae,Nirel,Zhurana,Sharga,Kheniva,Ulurza," +
							"Arjella,Narga,Ruzga,Ashna,Shar,Mula,Ulura,Zaressa,Ora," +
							"Khavia,Ina,Xathal,Zyrkal,Gokrel,Vhera,Maeryn,Jhoa,Ila,Sharo," +
							"Magha,Urma,Grevral,Ulnae,Sharel,Gresha,Xaril";
					}
					
					else
					{
						namelist = "Jhago,Okhon,Kamor,Boren,Sukhan,Mogen,Mulgan,Durgha," +
							"Xentir,Zelan,Xanxu,Xintru,Zoren,Ath,Xokil,Xilmar,Tharok," +
							"Ilan,Zyran,Zoranok,Xirak,Khentir,Mantek,Geragh,Uruth,Dhaug," +
							"Thurl,Ghaulogh,Ograug,Kranth,Ilagh,Oglukh,Saurogh,Ghauluk," +
							"Kendar,Morcan,Vengal,Zengar,Vizrik,Utaros,Artum,Menkar," +
							"Anuros,Chremus,Yamus,Ikharon,Danoch,Garekh,Aaruz,Zedek";
					}
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					if( female )
					{
						namelist = "Aideen,Aine,Anna,Arianwen,Blair,Brenda,Briana, " +
							"Ceinwyn,Colleen,Dealla,Dervil,Devany,Eachna,Eavan,Ethna," +
							"Eithne,Elatha,Eleanor,Fionna,Grania,Gwynne,Isleen,Kaitlin," +
							"Keena,Liadan,Lynn,Maeve,Mell,Myrna,Naomh,Neala,Nessa," +
							"Nevina,Nia,Nila,Nola,Ryann,Sinead,Taillte,Tara,Tullia," +
							"Una,Vanora,Caireann,Alana";
					}
					
					else
					{
						namelist = "Bran,Brennus,Brian,Caley,Calhoun,Casey,Cass,Cathal," +
							"Ceallach,Conall,Conn,Craig,Culley,Daigh,Devine,Dolan,Donn," +
							"Doran,Dow,Dumnorix,Eachan,Fearghus,Finnegan,Fionn,Gaeth," +
							"Gair,Gallagher,Galloway,Gwent,Haley,Imar,Innis,Irv,Keelan," +
							"Keenan,Keller,Kelvin,Kenneth,Labhraidh,Labras,Leannan,Lonn," +
							"Mardbod,Murdock,Ossian,Quinn,Raegan,Ryan,Riddock,Sean,Trevor," +
							"Logan,Liam,Daegan";
					}
					
					break;
				}
					
				case Nation.Northern:
				{
					if( female )
					{
						namelist = "Alma,Alexis,Andrea,Angela,Anna,Annika,Barbara,Cecilia," +
							"Clara,Claudia,Diana,Doreen,Doris,Emilia,Emma,Erika,Elisabeth," +
							"Elise,Flora,Gabriele,Gretchen,Hannah,Heidi,Helena,Irene," +
							"Isabell,Julia,Katharine,Kristin,Lara,Leah,Lena,Lisa,Livia," +
							"Marie,Marina,Marlene,Miriam,Monica,Nadia,Nadine,Natalie,Nicole," +
							"Nina,Olivia,Paula,Rita,Sabrina,Sandra,Sara,Sasha,Simone,Sophia," +
							"Stephanie,Therese,Thora,Vanessa,Victoria,Yvonne";
					}
					
					else
					{
						namelist = "Adam,Alexander,Alfred,Anton,Arnold,Benjamin,Christoff,Daniel," +
                                                        "Dennis,Elias,Eric,Ernest,Felix,Frederick,Gabriel,Garret,Gunnar," +
                                                        "Harold,Herman,Jakob,Jan,Jonas,Karl,Kasper,Kirk,Konrad,Kurt,Leon," +
                                                        "Linus,Lukas,Luther,Marcel,Marcus,Martin,Matthias,Max,Michael,Nicholas," +
                                                        "Oliver,Oskar,Patrick,Paul,Peter,Philipp,Ralph,Richard,Robert,Roland,"+
                                                        "Ruben,Rudolf,Samuel,Sebastian,Simon,Sven,Thomas,Tristan,Tobias,"+
                                                        "Victor,Walter,William";
					}
					
					break;
				}
			}
			
			string[] names = namelist.Split( ',' );
			
			return names[ Utility.Random( names.Length ) ];
		}

        public static string RandomSurname(Nation nation, bool female)
        {
            String surname = "";

            switch (nation)
            {
                case Nation.Mhordul:
                    {
                        if (Utility.Random(100) > 24) // Only 25% of Mhordul will have the title.
                            return surname;

                        surname += " the ";

                        string titleslist = "Cruel,Hard,Wartorn,Bloody,White,Black,Red,Iron,Manic,Mad,Quick,Slow,Pained,Sharp,Tongue,Eye," +
                            "Heart,Mouth,Ear,Ass,Horse,Bear,Eagle,Raven,Crow,Wolf,Snake,Serpent,Fish,Whale,Arrow,Blade,Spear,Knife,Sun," +
                            "Moon,Sky,Star,Night,Rock,Flame,River,Even,Just,Patient,Impatient,Vengeful,Fist,Cannibal,Terror,Punished,Blind,Mute," +
                            "Deaf,Dumb,Stupid,Ugly,Rotten,Lost,Forgotten,Bronze,Obsidian,Oak,Yew,Ash,Axe,Sword,Dagger,Hammer,Shield,Angry,Sad," +
                            "Weeping,Hard,Uncaring,Ignorant,Enraged,Lover,Lusty,Beautiful,Wounded,Unconquered,Tyrant,Inspired,Boar,Elk,Drox," +
                            "Bull,North,South,East,West,Wind,Chicken,Panther,Cougar,Jaguar,Spider,Lynx,Frozen,Cold,Free,Charitable,Grim,Broken," +
                            "Unforgiving,Remorseless,Wrathful,Earthquake,Flood,Tornado,Tsunami,Pestilent,Drought,Mountain,Thorn,Volcano," +
                            "Voice,Intrepid,Proud,Hawk,Falcon,Tenacious,Dragon,Ape,Lion,Crimson,Heavy,Loud,Silent,Quiet,Lame,Wide-eyed," +
                            "Filty,Watchful,Grotesque,Still,Clever,Fast,Horrible,Impossible,Wrong,Arrogant,Clumsy,Defeated,Envious," +
                            "Fierce,Fool,Thoughtless,Hungry,Lazy,Wicked,Weary,Brave,Fair,Glorious,Good,Enchanted,Lucky,Zealous,Obedient," +
                            "Broad,Fat,High,Hollow,Low,Straight,Crooked,Puny,Tiny,Big,Hushed,Shrill,Thunder,Whisper,Brief,Swift,Young," +
                            "Late,Early,Bitter,Sour,Thirsty,Winter,Blizzard,Song,Dirty,Empty,Axe";

                        string[] names = titleslist.Split(',');
                        surname += names[Utility.Random(names.Length)];

                        return surname;
                    }
                case Nation.Tirebladd:
                    {
                        if (female)
                            surname += " verch ";
                        else
                            surname += " ap ";

                        surname += RandomName(nation, false);

                        return surname;
                    }
                case Nation.Northern:
                    {
                        surname += " ";

                        string titleslist = "Akers,Anderson,Anson,Beck,Bernard,Brant,Brewer,Burke,Cline,Dennel,Derrick,Engel," +
                            "Ericson,Falk,Fischer,Frank,Frost,Fuller,Garber,Garner,Hagen,Hall,Hanson,Hart,Hoffman,Holt,Ivarson,Janson,Keller," +
                            "Kramer,Krause,Kruger,Lambert,Lang,Lennart,Lowe,Lynch,Mann,Mason,Meyer,Miller,Olson,Powell,Peters,Renner," +
                            "Rasmus,Reed,Ritter,Samson,Schaefer,Snyder,Sommer,Sorenson,Stark,Wagner,Warner,Weber,Weiss,Westen,Winters,Wolf,Wright,Zimmerman";

                        string[] names = titleslist.Split(',');
                        surname += names[Utility.Random(names.Length)];

                        return surname;
                    }
            }

            return surname;
        }
		
		public static void RandomCrafterClothes( Mobile m, Nation nation )
		{
			if( m == null )
				return;
			
			int choice = Utility.RandomMinMax( 0, 2 );

            switch (nation)
            {
/*                 case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Southern; } else { nation = Nation.Mhordul; } break; }
                 case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Western; } else { nation = Nation.Tirebladd; } break; } 
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; } */
            }
			
			switch( nation )
			{
				case Nation.Southern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new FemaleKilt( Utility.RandomNeutralHue() ) );
								break;
							}
								
							case 1: 
							{
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new MetallicBra() );
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new MetallicBra() );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new ElegantFemaleKilt( Utility.RandomNeutralHue() ) );
								break;
							}
						}
						
						m.EquipItem( new ElegantShoes() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ElegantKilt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Sandals() );
								break;
							}
								
							case 1: 
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BlackLeatherBoots() );
								m.EquipItem( new BeltedPants() );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								break;
							}
						}
					}
					
					break;
				}
					
				case Nation.Western:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new MetallicBra() );
								break;
							}
							
							case 1:
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new LeatherGloves() );
								break;
							}
							
							case 2:
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new LoinCloth( Utility.RandomNeutralHue() ) );
								m.EquipItem( new MetallicBra() );
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() ); 
								break;
							}
							
							case 1:
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Boots() ); 
								m.EquipItem( new LeatherGloves() );
								break;
							}
						}
					}
					
					break;
				}
					
				case Nation.Haluaroc:
								{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new MetallicBra() );
								break;
							}

							case 1:
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new LeatherGloves() );
								break;
							}

							case 2:
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new LoinCloth( Utility.RandomNeutralHue() ) );
								m.EquipItem( new MetallicBra() );
								break;
							}
						}

						m.EquipItem( new Sandals() );
					}

					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() ); 
								break;
							}

							case 1:
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() ); 
								break;
							}

							case 2:
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Boots() ); 
								m.EquipItem( new LeatherGloves() );
								break;
							}
						}
					}

					break;
				}
					
				case Nation.Mhordul:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new Sandals() ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new SmallDress( Utility.RandomNeutralHue() ) );
								break;
							}
								
							case 1:
							{
								m.EquipItem( new Boots() ); 
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new RaggedBra( Utility.RandomNeutralHue() ) );
								m.EquipItem( new LeatherGloves() ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new SmallRaggedSkirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new RaggedBra( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Sandals() ); 
								break;
							}
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new LeatherGloves() );
								break;
							}
							case 1: 
							{
								m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() );
								break;
							}
							case 2: 
							{
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() );
								break;
							}
						}
					}
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new FemaleKilt( Utility.RandomNeutralHue() ) );
								break;
							}

							case 1: 
							{
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new MetallicBra() );
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) );
								break;
							}

							case 2: 
							{
								m.EquipItem( new MetallicBra() );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new ElegantFemaleKilt( Utility.RandomNeutralHue() ) );
								break;
							}
						}

						m.EquipItem( new ElegantShoes() );
					}

					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ElegantKilt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Sandals() );
								break;
							}

							case 1: 
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BlackLeatherBoots() );
								m.EquipItem( new BeltedPants() );
								break;
							}

							case 2: 
							{
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								break;
							}
						}
					}

					break;
				}
				
				case Nation.Northern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								int hue = Utility.RandomNeutralHue();
								Cap bonnet = new Cap();
								bonnet.Hue = hue;
								m.EquipItem( bonnet ); 
								m.EquipItem( new FullApron( hue ) ); 
								m.EquipItem( new LeatherGloves() );
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new Shirt() );
								m.EquipItem( new LongPants( Utility.RandomNeutralHue() ) );
								break;
							}
							
							case 1:
							{
								m.EquipItem( new BeltedDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new ElegantShoes() );
								break;
							}
							
							case 2:
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Skirt( hue ) );
								m.EquipItem( new FancyDoublet( hue ) );
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new ElegantShoes() );
								break;
							}
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ElegantShoes() );
								m.EquipItem( new FancyShirt() );
								break;
							}
								
							case 1:
							{
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BlackLeatherBoots() );
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new Tunic( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new LeatherBoots() );
								break;
							}
						}
					}
					break;
				}
			}
		}
		
		public static void RandomRichClothes( Mobile m, Nation nation )
		{
			if( m == null )
				return;
			
			int choice = Utility.RandomMinMax( 0, 2 );

            switch (nation)
            {
/*                 case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Southern; } else { nation = Nation.Mhordul; } break; }
                 case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Western; } else { nation = Nation.Tirebladd; } break; } 
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; } */
            }

			switch( nation )
			{
				case Nation.Southern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ElegantShortDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldNecklace() );
								break;
							}
								
							case 1: 
							{
								m.EquipItem( new PuffyDress( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverNecklace() );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new OrnateDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new SilverBracelet() );
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new MaleDress( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() );
								m.EquipItem( new GoldBracelet() );
								break;
							}
								
							case 1: 
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new ElegantTunic( hue ) ); 
								m.EquipItem( new ElegantShoes() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new ElegantCloak( hue ) );
								m.EquipItem( new GoldBeadNecklace() );
								break;
							}
								
							case 2: 
							{
								int hue = Utility.RandomNeutralHue();
								Bonnet bonnet = new Bonnet();
								bonnet.Hue = hue;
								m.EquipItem( bonnet ); 
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new RunicCloak( hue ) );
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new ElegantShirt() ); 
								break;
							}
						}
					}
					
					break;
				}
					
				case Nation.Western:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new Skirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverBracelet() );
								break;
							}
							
							case 1:
							{
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverBracelet() );
								m.EquipItem( new SilverNecklace() );
								m.EquipItem( new SilverRing() );
								break;
							}
							
							case 2:
							{
								m.EquipItem( new ShortPlainDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldBracelet() );
								m.EquipItem( new GoldBeadNecklace() );
								m.EquipItem( new GoldRing() );
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
						
						if( choice < 2 )
							m.EquipItem( new MetallicBra() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); 
								break;
							}
							
							case 1:
							{
								m.EquipItem( new OrnateWaistCloth( Utility.RandomNeutralHue() ) ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new SoftLeatherTunic() ); 
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
						m.EquipItem( new GoldBeadNecklace() );
						
						if( choice < 2 )
						{
							m.EquipItem( new GoldBracelet() );
							m.EquipItem( new GoldRing() );
						}
					}
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new Skirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverBracelet() );
								break;
							}

							case 1:
							{
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverBracelet() );
								m.EquipItem( new SilverNecklace() );
								m.EquipItem( new SilverRing() );
								break;
							}

							case 2:
							{
								m.EquipItem( new ShortPlainDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldBracelet() );
								m.EquipItem( new GoldBeadNecklace() );
								m.EquipItem( new GoldRing() );
								break;
							}
						}

						m.EquipItem( new Sandals() );

						if( choice < 2 )
							m.EquipItem( new MetallicBra() );
					}

					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); 
								break;
							}

							case 1:
							{
								m.EquipItem( new OrnateWaistCloth( Utility.RandomNeutralHue() ) ); 
								break;
							}

							case 2:
							{
								m.EquipItem( new SoftLeatherTunic() ); 
								break;
							}
						}

						m.EquipItem( new Sandals() );
						m.EquipItem( new GoldBeadNecklace() );

						if( choice < 2 )
						{
							m.EquipItem( new GoldBracelet() );
							m.EquipItem( new GoldRing() );
						}
					}

					break;
				}
					
				case Nation.Mhordul:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new LongRaggedBra() ); 
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldBeadNecklace() ); 
								break;
							}
								
							case 1:
							{
								m.EquipItem( new LongRaggedBra() ); 
								m.EquipItem( new Skirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldEarrings() ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new ShortPlainDress( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new GoldRing() ); 
								m.EquipItem( new GoldBeadNecklace() ); 
								break;
							}
						}
						
						m.EquipItem( new Sandals() ); 
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new MonsterMask() );
								break;
							}
							case 1: 
							{
								m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new WolfMask() );
								break;
							}
							case 2: 
							{
								m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BearMask() );
								m.EquipItem( new StuddedArms() );
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
						m.EquipItem( new GoldBeadNecklace() ); 
						m.EquipItem( new GoldRing() ); 
						
						if( choice < 2 )
						{
							m.EquipItem( new ElegantCloak( Utility.RandomNeutralHue() ) );
							m.EquipItem( new GoldBracelet() ); 
						}
					}
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ElegantShortDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldNecklace() );
								break;
							}
								
							case 1: 
							{
								m.EquipItem( new PuffyDress( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new SilverNecklace() );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new OrnateDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new SilverBracelet() );
								break;
							}
						}
						
						m.EquipItem( new Sandals() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new MaleDress( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new Sandals() );
								m.EquipItem( new GoldBracelet() );
								break;
							}
								
							case 1: 
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new ElegantTunic( hue ) ); 
								m.EquipItem( new ElegantShoes() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new ElegantCloak( hue ) );
								m.EquipItem( new GoldBeadNecklace() );
								break;
							}
								
							case 2: 
							{
								int hue = Utility.RandomNeutralHue();
								Bonnet bonnet = new Bonnet();
								bonnet.Hue = hue;
								m.EquipItem( bonnet ); 
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new BeltedPants() );
								m.EquipItem( new RunicCloak( hue ) );
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new ElegantShirt() ); 
								break;
							}
						}
					}
					
					break;
				}
										
				case Nation.Northern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ExpensiveLongGown( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new WhiteFeatheredHat() );
								m.EquipItem( new SilverBeadNecklace() );
								break;
							}
							
							case 1:
							{
								m.EquipItem( new GildedFancyDress( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldBeadNecklace() );
								m.EquipItem( new ExpensiveCloak() );
								break;
							}
							
							case 2:
							{
								m.EquipItem( new ExpensiveGown( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ExpensiveHat() );
								m.EquipItem( new GoldEarrings() );
								m.EquipItem( new ElegantShoes() );
								break;
							}
						}
						if( choice < 2 )
							m.EquipItem( new HighHeels() );
					}
					
					else
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new ExpensiveShirt( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ExpensiveCloak() );
								m.EquipItem( new WhiteFeatheredHat() );
								break;
							}
								
							case 1:
							{
								m.EquipItem( new ExtravagantShirt( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new ElegantShoes() ); 
								m.EquipItem( new FormalShirt( Utility.RandomNeutralHue() ) );
								m.EquipItem( new GoldRing() );
								break;
							}
						}
					}
					
					m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
					
					if( choice < 2 )
							m.EquipItem( new ThighBoots() );
					
					if( choice > 0 )
							m.EquipItem( new ElegantCloak( Utility.RandomNeutralHue() ) );
						
					break;
				}
			}
		}
		
		public static void RandomGuardEquipment( Mobile m, Nation nation, int choice )
		{
			if( m == null )
				return;
			
			if( choice > 3 || choice < 1 )
				choice = Utility.RandomMinMax( 0, 2 );
			else
				choice--;

            switch (nation)
            {
/*                 case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Southern; } else { nation = Nation.Mhordul; } break; }
                 case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Western; } else { nation = Nation.Tirebladd; } break; } 
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; } */
            }
			
			switch( nation )
			{
				case Nation.Southern:
				{
					GreenBeret greenberet = new GreenBeret();
					greenberet.Hue = 2587;
					m.EquipItem( greenberet );
						            
					switch( choice )
					{
						case 0:
						{
							SplintedMailChest chest = new SplintedMailChest();
							chest.Resource = CraftResource.Bronze;
							
							SplintedMailLegs legs = new SplintedMailLegs();
							legs.Resource = CraftResource.Bronze;
							
							SplintedMailArms arms = new SplintedMailArms();
							arms.Resource = CraftResource.Bronze;
							
							SplintedMailGorget gorget = new SplintedMailGorget();
							gorget.Resource = CraftResource.Bronze;
							
							m.EquipItem( chest );
							m.EquipItem( legs );
							m.EquipItem( arms );
							m.EquipItem( gorget ); 
							m.EquipItem( new Cloak( 2587 ) );
							            
				            if( m.Female )
				            {
				            	NotchedShield shield = new NotchedShield();
								shield.Resource = CraftResource.Bronze;
								
								Falcata sabre = new Falcata();
								sabre.Resource = CraftResource.Bronze;
				            	
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sabre ); 
				            	m.EquipItem( new FemaleKilt( 2587 ) );
				            }
				            
				            else
				            {
				            	HeavyDoubleAxe axe = new HeavyDoubleAxe();
								axe.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( axe ); 
				            	m.EquipItem( new ElegantKilt( 2587 ) );
				            }
				            
							break;
						}
							
						case 1:
						{ 
				            if( m.Female )
				            {
				            	RoundShield shield = new RoundShield();
								shield.Resource = CraftResource.Bronze;
								
								ArmingSword sword = new ArmingSword();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            	m.EquipItem( new ElegantFemaleKilt( 2587 ) );
				            	m.EquipItem( new MetallicBra() );
				            	m.EquipItem( new ElegantShoes() );
				            }
				            
				            else
				            {
				            	Claymore sword = new Claymore();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( sword ); 
				            	m.EquipItem( new OrnateKilt( 2587 ) );
				            	m.EquipItem( new Sandals() ); 
				            }
				            
							break;
						}
							
						case 2:
						{ 
				            if( m.Female )
				            {
				            	OrnateLongBow bow = new OrnateLongBow();
								bow.Resource = CraftResource.Redwood;
								
				            	m.EquipItem( bow ); 
				            	m.EquipItem( new ElegantKilt( 2587 ) );
				            	m.EquipItem( new MetallicBra() );
				            	
				            }
				            
				            else
				            {
				            	WarBow bow = new WarBow();
								bow.Resource = CraftResource.Redwood;
								
				            	m.EquipItem( bow ); 
				            	m.EquipItem( new PlainKilt( 2587 ) );
				            }
				            
				            m.EquipItem( new Sandals() );
				            
				            if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
					
				case Nation.Western:
				{
					switch( choice )
					{
						case 0:
						{
							EagleHelm helm = new EagleHelm();
							helm.Resource = CraftResource.Bronze;
							
							SpikedChest chest = new SpikedChest();
							chest.Resource = CraftResource.Bronze;
							
							RingmailArms arms = new RingmailArms();
							arms.Resource = CraftResource.Bronze;
							
							ChainLegs legs = new ChainLegs();
							legs.Resource = CraftResource.Bronze;
							
							RingmailGloves gloves = new RingmailGloves();
							gloves.Resource = CraftResource.Bronze;
							
							m.EquipItem( helm );
							m.EquipItem( chest );
							Sandals sandals = new Sandals();
							sandals.Resource = CraftResource.BeastLeather;
							sandals.Hue = 2810;
							m.EquipItem( sandals );
							m.EquipItem( gloves ); 
							m.EquipItem( arms ); 
							m.EquipItem( legs );
							m.EquipItem( new Cloak( 2810 ) );
							            
				            if( m.Female )
				            {
				            	TallFaceShield shield = new TallFaceShield();
								shield.Resource = CraftResource.Bronze;
								
								Kutar sword = new Kutar();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            	m.EquipItem( new ElegantWaistCloth( 2810 ) );
				            }
				            
				            else
				            {
				            	RoundedFaceShield shield = new RoundedFaceShield();
								shield.Resource = CraftResource.Bronze;
								
								Flamberge sword = new Flamberge();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            }
				            
							break;
						}
							
						case 1:
						{ 
							SoftLeatherTunic chest = new SoftLeatherTunic();
							chest.Resource = CraftResource.BeastLeather;
							
							SoftLeatherPauldrons pauldrons = new SoftLeatherPauldrons();
							pauldrons.Resource = CraftResource.BeastLeather;
							
							SoftLeatherBoots boots = new SoftLeatherBoots();
							boots.Resource = CraftResource.BeastLeather;
							
							SoftLeatherLegs legs = new SoftLeatherLegs();
							legs.Resource = CraftResource.BeastLeather;
							
							LeatherGloves gloves = new LeatherGloves();
							gloves.Resource = CraftResource.BeastLeather;
							
							LeatherArms arms = new LeatherArms();
							arms.Resource = CraftResource.BeastLeather;
							
							PrimitiveSpear spear = new PrimitiveSpear();
							spear.Resource = CraftResource.Bronze;
								
			            	m.EquipItem( chest ); 
			            	m.EquipItem( pauldrons ); 
			            	m.EquipItem( new Bandana( 2810 ) );
			            	m.EquipItem( spear );
			            	m.EquipItem( boots );
			            	m.EquipItem( legs );
			            	m.EquipItem( gloves );
			            	m.EquipItem( arms );
							break;
						}
							
						case 2:
						{ 
				            Sandals sandals = new Sandals();
				            sandals.Resource = CraftResource.BeastLeather;
							sandals.Hue = 2810;
							m.EquipItem( sandals );
				            
							if( m.Female )
				            {
								ShortBow bow = new ShortBow();
								bow.Resource = CraftResource.Redwood;
							
				            	m.EquipItem( bow ); 
				            	m.EquipItem( new MetallicBra() );
				            	m.EquipItem( new WaistCloth( 2810 ) );
				            }
							
							else
							{
								Boomerang bow = new Boomerang();
								bow.Resource = CraftResource.Redwood;
								
								m.EquipItem( bow ); 
								m.EquipItem( new LoinCloth( 2810 ) );
							}
							
							if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            
					            if( m.Female )
					            	bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					switch( choice )
					{
						case 0:
						{
							ScaleArmorChest chest = new ScaleArmorChest();
							chest.Resource = CraftResource.Bronze;
							chest.Hue = 2947;
							m.EquipItem( chest );
							
							PlateLegs legs = new PlateLegs();
							legs.Resource = CraftResource.Bronze;
							legs.Hue = 2947;
							m.EquipItem( legs );
							
							PlateArms arms = new PlateArms();
							arms.Resource = CraftResource.Bronze;
							arms.Hue = 2947;
							m.EquipItem( arms );
							
							PlateGorget gorget = new PlateGorget();
							gorget.Resource = CraftResource.Bronze;
							gorget.Hue = 2947;
							m.EquipItem( gorget );
							
							PlateGloves gloves = new PlateGloves();
							gloves.Resource = CraftResource.Bronze;
							gloves.Hue = 2947;
							m.EquipItem( gloves );
							
							ScaleArmorHelmet helmet = new ScaleArmorHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							Tabarzin axe = new Tabarzin();
							axe.Resource = CraftResource.Bronze;
							m.EquipItem( axe );
							            
				            if( m.Female )
				            {
				            	ElegantWaistCloth waist = new ElegantWaistCloth();
				            	waist.Hue = 2795;
				            	m.EquipItem( waist );
				            }
				            
				            else
				            {
				            	WaistSash sash = new WaistSash();
				            	sash.Hue = 2795;
				            	m.EquipItem( sash ); 
				            }
				            
							break;
						}
							
						case 1:
						{
							ThighBoots boots = new ThighBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2947;
							m.EquipItem( boots ); 
							
							ScaleArmorChest chest = new ScaleArmorChest();
							chest.Resource = CraftResource.Bronze;
							chest.Hue = 2947;
							m.EquipItem( chest );
							
							ScaleArmorLegs legs = new ScaleArmorLegs();
							legs.Resource = CraftResource.Bronze;
							legs.Hue = 2947;
							m.EquipItem( legs );
							
							ScaleArmorArms arms = new ScaleArmorArms();
							arms.Resource = CraftResource.Bronze;
							arms.Hue = 2947;
							m.EquipItem( arms );
							
							RingmailGloves gloves = new RingmailGloves();
							gloves.Resource = CraftResource.Bronze;
							gloves.Hue = 2947;
							m.EquipItem( gloves );
							
							ScaleArmorHelmet helmet = new ScaleArmorHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							Khopesh sword = new Khopesh();
							sword.Resource = CraftResource.Bronze;
							m.EquipItem( sword );
							
							MetalShield shield = new MetalShield();
							shield.Resource = CraftResource.Bronze;
							shield.Hue = 2947;
							m.EquipItem( shield );
			            	
			            	if( m.Female )
				            {
				            	ElegantWaistCloth waist = new ElegantWaistCloth();
				            	waist.Hue = 2795;
				            	m.EquipItem( waist );
				            }
				            
				            else
				            {
				            	WaistSash sash = new WaistSash();
				            	sash.Hue = 2795;
				            	m.EquipItem( sash ); 
				            }
							
							break;
						}
							
						case 2:
						{ 
							Sandals sandals = new Sandals();
							sandals.Resource = CraftResource.BeastLeather;
							sandals.Hue = 2947;
							m.EquipItem( sandals ); 
							
							ScaleArmorChest chest = new ScaleArmorChest();
							chest.Resource = CraftResource.Bronze;
							chest.Hue = 2947;
							m.EquipItem( chest );
							
							ScaleArmorLegs legs = new ScaleArmorLegs();
							legs.Resource = CraftResource.Bronze;
							legs.Hue = 2947;
							m.EquipItem( legs );
							
							ScaleArmorHelmet helmet = new ScaleArmorHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							Hijazi bow = new Hijazi();
							bow.Resource = CraftResource.Redwood;
							m.EquipItem( bow );
			            	
			            	if( m.Female )
				            {
				            	ElegantWaistCloth waist = new ElegantWaistCloth();
				            	waist.Hue = 2795;
				            	m.EquipItem( waist );
				            }
				            
				            else
				            {
				            	WaistSash sash = new WaistSash();
				            	sash.Hue = 2795;
				            	m.EquipItem( sash ); 
				            }

							
							if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
					
				case Nation.Mhordul:
				{
					Sandals sandals = new Sandals();
					sandals.Resource = CraftResource.BeastLeather;
					sandals.Hue = 1194;
					m.EquipItem( sandals );
					
					BoneArms mba = new BoneArms();
					mba.Hue = 2101;
					m.EquipItem( mba );
							
					switch( choice )
					{
						case 0:
						{
							MhordulHornedSkullHelm mhsh = new MhordulHornedSkullHelm();
							mhsh.Hue = 2101;
							m.EquipItem( mhsh );
							
							BoneChest mbc = new BoneChest();
							mbc.Hue = 2101;
							m.EquipItem( mbc );
							
							BoneLegs mbl = new BoneLegs();
							mbl.Hue = 2101;
							m.EquipItem( mbl );
							
							BoneGloves mbg = new BoneGloves();
							mbg.Hue = 2101;
							m.EquipItem( mbg );
							
							BoneShield mbs = new BoneShield();
							mbs.Hue = 2101;
							m.EquipItem( mbs );
							            
				            if( m.Female )
				            {
				            	m.EquipItem( new BoneSword() ); 
				            	m.EquipItem( new SmallRaggedSkirt( 1194 ) );
				            }
				            
				            else
				            {
				            	m.EquipItem( new BoneAxe() ); 
				            }
				            
							break;
						}
							
						case 1:
						{ 
			            	BoneHelm mbh = new BoneHelm();
							mbh.Hue = 2101;
							m.EquipItem( mbh );
							
							BoneLegs mbl = new BoneLegs();
							mbl.Hue = 2101;
							m.EquipItem( mbl );
							
							BoneGloves mbg = new BoneGloves();
							mbg.Hue = 2101;
							m.EquipItem( mbg );
							            
				            if( m.Female )
				            {
				            	m.EquipItem( new BoneSpear() ); 
				            	m.EquipItem( new SmallRaggedSkirt( 1194 ) );
				            	m.EquipItem( new RaggedBra( 1194 ) );
				            }
				            
				            else
				            {
				            	m.EquipItem( new BoneScythe() ); 
				            	m.EquipItem( new WaistCloth( 1194 ) );
				            }
				            
							break;
						}
							
						case 2:
						{ 
							BoneHelm mbh = new BoneHelm();
							mbh.Hue = 2101;
							m.EquipItem( mbh );
							
							m.EquipItem( new BoneBow() );
							
							if( m.Female )
							{
				            	m.EquipItem( new SmallRaggedSkirt( 1194 ) );
				            	m.EquipItem( new RaggedBra( 1194 ) );
							}
							
							else
								m.EquipItem( new RaggedPants( 1194 ) );
							
							if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					Surcoat coat = new Surcoat();
					coat.ItemID = 15477;
					coat.Name = "Tirebladd Military Surcoat";
					coat.Hue = 2741;
					m.EquipItem( coat );
							
					switch( choice )
					{
						case 0:
						{
							HalfPlateChest thpc = new HalfPlateChest();
							thpc.Resource = CraftResource.Bronze;
							thpc.Hue = 1899;
							m.EquipItem( thpc );
							
							HalfPlateLegs thpl = new HalfPlateLegs();
							thpl.Resource = CraftResource.Bronze;
                            thpl.Hue = 1899;
							m.EquipItem( thpl );
							
							HalfPlateSabatons thps = new HalfPlateSabatons();
							thps.Resource = CraftResource.Bronze;
                            thps.Hue = 1899;
							m.EquipItem( thps );
							
							HalfPlateArms thpa = new HalfPlateArms();
							thpa.Resource = CraftResource.Bronze;
                            thpa.Hue = 1899;
							m.EquipItem( thpa );
							
							HalfPlateGloves thpg = new HalfPlateGloves();
							thpg.Resource = CraftResource.Bronze;
                            thpg.Hue = 1899;
							m.EquipItem( thpg );
							
							HalfPlateGorget thpo = new HalfPlateGorget();
							thpo.Resource = CraftResource.Bronze;
                            thpo.Hue = 1899;
							m.EquipItem( thpo );
							
							DragonKiteShield tks = new DragonKiteShield();
							tks.Resource = CraftResource.Bronze;
                            tks.Hue = 1899;
							m.EquipItem( tks );
							
							m.EquipItem( new Cloak( 1445 ) );
							
							WingedHelm twh = new WingedHelm();
			            	twh.Resource = CraftResource.Bronze;
                            twh.Hue = 1899;
							m.EquipItem( twh );
							            
				            if( m.Female )
				            {
				            	BroadAxe axe = new BroadAxe();
								axe.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( axe ); 
				            }
				            
				            else
				            {
				            	OrnateAxe axe = new OrnateAxe();
								axe.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( axe ); 
				            }
				            
							break;
						}
							
						case 1:
						{ 
			            	ChainChest cc = new ChainChest();
			            	cc.Resource = CraftResource.Bronze;
                            cc.Hue = 1899;
							m.EquipItem( cc );
							
							ChainLegs cl = new ChainLegs();
							cl.Resource = CraftResource.Bronze;
                            cl.Hue = 1899;
							m.EquipItem( cl );
							
							RingmailArms ra = new RingmailArms();
							ra.Resource = CraftResource.Bronze;
                            ra.Hue = 1899;
							m.EquipItem( ra );
							
							RingmailGloves rg = new RingmailGloves();
							rg.Resource = CraftResource.Bronze;
                            rg.Hue = 1899;
							m.EquipItem( rg );
							
							HornedHelm thh = new HornedHelm();
							thh.Resource = CraftResource.Bronze;
                            thh.Hue = 1899;
							m.EquipItem( thh );
							
							FurBoots boots = new FurBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2741;
						
							m.EquipItem( boots );
							            
				            if( m.Female )
				            {
				            	Angon weapon = new Angon();
								weapon.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( weapon ); 
				            }
				            
				            else
				            {
				            	HeavyBattleAxe weapon = new HeavyBattleAxe();
								weapon.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( weapon ); 
				            }
				            
							break;
						}
							
						case 2:
						{ 
							LeatherChest lc = new LeatherChest();
							lc.Resource = CraftResource.BeastLeather;
							lc.Hue = 1899;
							m.EquipItem( lc );
							
							LeatherLegs ll = new LeatherLegs();
							ll.Resource = CraftResource.BeastLeather;
							ll.Hue = 1899;
							m.EquipItem( ll );
							
							LeatherArms la = new LeatherArms();
							la.Resource = CraftResource.BeastLeather;
							la.Hue = 1899;
							m.EquipItem( ll );
							
							LeatherGloves lg = new LeatherGloves();
							lg.Resource = CraftResource.BeastLeather;
							lg.Hue = 1899;
							m.EquipItem( lg );
							
							LeatherGorget lo = new LeatherGorget();
							lo.Resource = CraftResource.BeastLeather;
							lo.Hue = 1899;
							m.EquipItem( lo );
							
							LeatherCap lcap = new LeatherCap();
							lcap.Resource = CraftResource.BeastLeather;
							lcap.Hue = 1899;
							m.EquipItem( lcap );
							
							FurBoots boots = new FurBoots();
							boots.Resource = CraftResource.BeastLeather;
                            boots.Hue = 2741;
							m.EquipItem( boots );
							
							RecurveLongBow bow = new RecurveLongBow();
							bow.Resource = CraftResource.Redwood;
							m.EquipItem( bow );
							
							if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
					
				case Nation.Northern:
				{
					Surcoat coat = new Surcoat();
					coat.Name = "Northern Military Surcoat";
					coat.Hue = 1327;
					coat.ItemID = 15479;
					m.EquipItem( coat );
							
					switch( choice )
					{
						case 0:
						{
							OrnatePlateChest vopc = new OrnatePlateChest();
							vopc.Resource = CraftResource.Bronze;
							vopc.Hue = 2101;
							m.EquipItem( vopc );
							
							OrnatePlateLegs vopl = new OrnatePlateLegs();
							vopl.Resource = CraftResource.Bronze;
							vopl.Hue = 2101;
							m.EquipItem( vopl );
							
							OrnatePlateGorget vopo = new OrnatePlateGorget();
							vopo.Resource = CraftResource.Bronze;
							vopo.Hue = 2101;
							m.EquipItem( vopo );
							
							PlateSabatons ps = new PlateSabatons();
							ps.Resource = CraftResource.Bronze;
							ps.Hue = 2105;
							m.EquipItem( ps );
							
							OrnatePlateArms vopa = new OrnatePlateArms();
							vopa.Resource = CraftResource.Bronze;
							vopa.Hue = 2101;
							m.EquipItem( vopa );
							
							OrnatePlateGloves vopg = new OrnatePlateGloves();
							vopg.Resource = CraftResource.Bronze;
							vopg.Hue = 2101;
							m.EquipItem( vopg );
							
							OrnateKiteShield voks = new OrnateKiteShield();
							voks.Resource = CraftResource.Bronze;
							voks.Hue = 2102;
							m.EquipItem( voks );
							
							m.EquipItem( new Cloak( 1327 ) );
							            
				            if( m.Female )
				            {
				            	HorsemanMace mace = new HorsemanMace();
								mace.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( mace ); 
				            	
				            	OrnateHelm voh = new OrnateHelm();
				            	voh.Resource = CraftResource.Bronze;
								voh.Hue = 2102;
								m.EquipItem( voh );
				            }
				            
				            else
				            {
				            	HorsemanWarhammer mace = new HorsemanWarhammer();
								mace.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( mace );
				            	
				            	OrnatePlateHelm voph = new OrnatePlateHelm();
				            	voph.Resource = CraftResource.Bronze;
								voph.Hue = 2102;
								m.EquipItem( voph );
				            }
				            
							break;
						}
							
						case 1:
						{ 
			            	ChainChest cc = new ChainChest();
			            	cc.Resource = CraftResource.Bronze;
							cc.Hue = 2101;
							m.EquipItem( cc );
							
							ChainLegs cl = new ChainLegs();
							cl.Resource = CraftResource.Bronze;
							cl.Hue = 2101;
							m.EquipItem( cl );
							
							ChainCoif co = new ChainCoif();
							co.Resource = CraftResource.Bronze;
							co.Hue = 2101;
							m.EquipItem( co );
							
							RingmailArms ra = new RingmailArms();
							ra.Resource = CraftResource.Bronze;
							ra.Hue = 2101;
							m.EquipItem( ra );
							
							RingmailGloves rg = new RingmailGloves();
							rg.Resource = CraftResource.Bronze;
							rg.Hue = 2101;
							m.EquipItem( rg );
							
							KiteShield vmks = new KiteShield();
							vmks.Resource = CraftResource.Bronze;
							vmks.Hue = 2101;
							m.EquipItem( vmks );
							
							LeatherBoots boots = new LeatherBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2101;
							
							m.EquipItem( boots );
							            
				            if( m.Female )
				            {
				            	Gladius sword = new Gladius();
								sword.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( sword );
				            }
				            
				            else
				            {
				            	CavalrySword sword = new CavalrySword();
								sword.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( sword );
				            }
				            
							break;
						}
							
						case 2:
						{ 
							LeatherBoots boots = new LeatherBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2101;
							
							m.EquipItem( boots );
							
							Beret beret = new Beret();
							beret.Hue = 1327;
							m.EquipItem( beret );
							
							StuddedChest sc = new StuddedChest();
							sc.Resource = CraftResource.BeastLeather;
							sc.Hue = 2101;
							m.EquipItem( sc );
							
							StuddedLegs sl = new StuddedLegs();
							sl.Resource = CraftResource.BeastLeather;
							sl.Hue = 2101;
							m.EquipItem( sl );
							
							StuddedArms sa = new StuddedArms();
							sa.Resource = CraftResource.BeastLeather;
							sa.Hue = 2101;
							m.EquipItem( sa );
							
							StuddedGloves sg = new StuddedGloves();
							sg.Resource = CraftResource.BeastLeather;
							sg.Hue = 2101;
							m.EquipItem( sg );
							
							StuddedGorget so = new StuddedGorget();
							so.Resource = CraftResource.BeastLeather;
							so.Hue = 2101;
							m.EquipItem( so );
							
							LongBow bow = new LongBow();
							bow.Resource = CraftResource.Redwood;
							
							m.EquipItem( bow );
							
							if( m is BaseCreature )
				            {
					            BaseCreature bc = m as BaseCreature;
					            bc.AI = AIType.AI_Archer;
					            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
							}
				            
							break;
						}
					}
					
					break;
				}
			}
		}
		
		public static void RandomPoorClothes( Mobile m, Nation nation )
		{
			if( m == null )
				return;
			
			int choice = Utility.RandomMinMax( 0, 2 );

            switch (nation)
            {
/*                 case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Southern; } else { nation = Nation.Mhordul; } break; }
                 case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Western; } else { nation = Nation.Tirebladd; } break; } 
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Northern; } else { nation = Nation.Haluaroc; } break; } */
            }

			switch( nation )
			{
				case Nation.Southern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new FemaleKilt( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantFemaleKilt( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); break;
						}
						
						if( choice < 2 )
						{
							m.EquipItem( new MetallicBra() );
							m.EquipItem( new ElegantShoes() );
						}
						
						else
						{
							m.EquipItem( new Sandals() );
							m.EquipItem( new Shirt() );
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new LongPants( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new ShortPants( Utility.RandomNeutralHue() ) ); break;
						}
						
						if( choice > 0 )
						{
							m.EquipItem( new Sandals() );
	                                                m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) ); break;
						}
						
						else
						{
                                                        m.EquipItem( new Shirt( Utility.RandomNeutralHue() ) ); break;
						}
					}
					
					break;
				}
					
				case Nation.Western:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantWaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new LoinCloth( Utility.RandomNeutralHue() ) ); break;
						}
						
						m.EquipItem( new Sandals() );
						m.EquipItem( new MetallicBra() );
					}
					
					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantWaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new OrnateWaistCloth( Utility.RandomNeutralHue() ) ); break;
						}
						
						m.EquipItem( new Sandals() );
					}
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantWaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new LoinCloth( Utility.RandomNeutralHue() ) ); break;
						}

						m.EquipItem( new Sandals() );
						m.EquipItem( new MetallicBra() );
					}

					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantWaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new OrnateWaistCloth( Utility.RandomNeutralHue() ) ); break;
						}

						m.EquipItem( new Sandals() );
					}

					break;
				}
					
				case Nation.Mhordul:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new LongRaggedBra() ); 
								m.EquipItem( new RaggedSkirt() );
								break;
							}
								
							case 1:
							{
								m.EquipItem( new RaggedBra() ); 
								m.EquipItem( new SmallRaggedSkirt() );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new SmallDress( Utility.RandomNeutralHue() ) ); 
								break;
							}
						}
						
						m.EquipItem( new Sandals() ); 
					}
					
					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new RaggedPants( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new WaistCloth( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new LoinCloth( Utility.RandomNeutralHue() ) ); break;
						}
						
						m.EquipItem( new Sandals() );
					}
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new FemaleKilt( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new ElegantFemaleKilt( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new LongSkirt( Utility.RandomNeutralHue() ) ); break;
						}
						
						if( choice < 2 )
						{
							m.EquipItem( new MetallicBra() );
							m.EquipItem( new ElegantShoes() );
						}
						
						else
						{
							m.EquipItem( new Sandals() );
							m.EquipItem( new Shirt() );
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new ElegantKilt( Utility.RandomNeutralHue() ) ); break;
							case 1: m.EquipItem( new PlainKilt( Utility.RandomNeutralHue() ) ); break;
							case 2: m.EquipItem( new Kilt( Utility.RandomNeutralHue() ) ); break;
						}
						
						if( choice > 0 )
						{
							m.EquipItem( new Sandals() );
						}
						
						else
						{
							m.EquipItem( new LeatherBoots() );
						}
					}
					
					break;
				}
					
				case Nation.Northern:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new BeltedDress( Utility.RandomSlimeHue() ) ); break;
							case 1: m.EquipItem( new LongDress( Utility.RandomSlimeHue() ) ); break;
							case 2: m.EquipItem( new LongOrnateDress( Utility.RandomSlimeHue() ) ); break;
						}
						
						m.EquipItem( new Sandals() );
					}
					
					else
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new Boots() ); 
								m.EquipItem( new LongPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new FancyShirt( Utility.RandomSlimeHue() ) ); 
								break;
							}
								
							case 1:
							{
								m.EquipItem( new LeatherBoots() );
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ExpensiveShirt( Utility.RandomSlimeHue() ) ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new BlackLeatherBoots() ); 
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Tunic( Utility.RandomSlimeHue() ) );
								break;
							}
						}
					}
						
					break;
				}
			}
		}

		public override int Bones{ get{ return 6; } }
		public override int Meat{ get{ return 6; } }

		public BaseKhaerosMobile( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 5 ); // version 
			
			writer.Write( (bool) m_WillArrest );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
			
			int test = 0;
			
			if( version > 1 )
			{
				if( version < 3 )
				{
					test = reader.ReadInt();
					m_WillArrest = test > 0;
				}
					
				else
					m_WillArrest = reader.ReadBool();
			}

            if (version < 5)
            {
                reader.ReadItem();
            }
		} 
	} 
}
