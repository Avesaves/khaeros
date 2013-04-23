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
                if (this is IAlyrian)
                    this.Nation = Nation.Alyrian;
                else if (this is IAzhuran)
                    this.Nation = Nation.Azhuran;
                else if (this is IKhemetar)
                    this.Nation = Nation.Khemetar;
                else if (this is IMhordul)
                    this.Nation = Nation.Mhordul;
                else if (this is ITyrean)
                    this.Nation = Nation.Tyrean;
                else if (this is IVhalurian)
                    this.Nation = Nation.Vhalurian;
                else if (this is IImperial)
                    this.Nation = Nation.Imperial;
            }

			SpeechHue = Utility.RandomDyedHue();

            Nation nation = n;

            switch (n)
            {
                case Nation.Imperial:
                    {
                        int raceChance = Utility.Random(7);
                        switch (raceChance)
                        {
                            case 0: nation = Nation.Vhalurian; break;
                            case 1: nation = Nation.Vhalurian; break;
                            case 2: nation = Nation.Vhalurian; break;
                            case 3: nation = Nation.Tyrean; break;
                            case 4: nation = Nation.Tyrean; break;
                            case 5: nation = Nation.Tyrean; break;
                            case 6: nation = Nation.Khemetar; break;

                            default: nation = Nation.Vhalurian; break;
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
                            case 3: nation = Nation.Alyrian; break;
                            case 4: nation = Nation.Alyrian; break;
                            case 5: nation = Nation.Alyrian; break;
                            case 6: nation = Nation.Azhuran; break;

                            default: nation = Nation.Mhordul; break;
                        }

                        break;
                    }
                case Nation.Society:
                    {
                        int raceChance = Utility.Random(6);

                        switch (raceChance)
                        {
                            case 0: nation = Nation.Alyrian; break;
                            case 1: nation = Nation.Azhuran; break;
                            case 2: nation = Nation.Khemetar; break;
                            case 3: nation = Nation.Mhordul; break;
                            case 4: nation = Nation.Tyrean; break;
                            case 5: nation = Nation.Vhalurian; break;

                            default: nation = Nation.Khemetar; break;
                        }

                        break;
                    }
				case Nation.Insularii:
                    {
                        int raceChance = Utility.Random(6);

                        switch (raceChance)
                        {
                            case 0: nation = Nation.Vhalurian; break;
                            case 1: nation = Nation.Vhalurian; break;
                            case 2: nation = Nation.Khemetar; break;
                            case 3: nation = Nation.Mhordul; break;
                            case 4: nation = Nation.Alyrian; break;
                            case 5: nation = Nation.Vhalurian; break;

                            default: nation = Nation.Vhalurian; break;
                        }

                        break;
                    }
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
				case Nation.Alyrian: options = 16; break;
				case Nation.Azhuran: options = 17; break;
				case Nation.Khemetar: options = 15; break;
				case Nation.Mhordul: options = 14; break;
				case Nation.Tyrean: options = 13; break;
				case Nation.Vhalurian: options = 13; break;
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
						case Nation.Alyrian: return 12738;
						case Nation.Azhuran: return 12738;
						case Nation.Khemetar: return 12738;
						case Nation.Mhordul: return 12747;
						case Nation.Tyrean: return 12749;
						case Nation.Vhalurian: return 12749;
					}
					break;
				}
				case 10:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12749;
						case Nation.Azhuran: return 12741;
						case Nation.Khemetar: return 12746;
						case Nation.Mhordul: return 12757;
						case Nation.Tyrean: return 12757;
						case Nation.Vhalurian: return 8261;
					}
					break;
				}
				case 11:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12750;
						case Nation.Azhuran: return 12748;
						case Nation.Khemetar: return 12748;
						case Nation.Mhordul: return 12241;
						case Nation.Tyrean: hair = 8262; break;
						case Nation.Vhalurian: hair = 8262; break;
					}
					break;
				}
				case 12:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12757;
						case Nation.Azhuran: return 12745;
						case Nation.Khemetar: return 12749;
						case Nation.Mhordul: return 8260;
						case Nation.Tyrean: hair = 12753; break;
						case Nation.Vhalurian: hair = 12753; break;
					}
					break;
				}
				case 13:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: hair = 8262; break;
						case Nation.Azhuran: return 12241;
						case Nation.Khemetar: return 12757;
						case Nation.Mhordul: hair = 8262; break;
						case Nation.Tyrean: hair = 12752; break;
						case Nation.Vhalurian: hair = 12752; break;
					}
					break;
				}
				case 14:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: hair = 12753; break;
						case Nation.Azhuran: return 8263;
						case Nation.Khemetar: hair = 8262; break;
						case Nation.Mhordul: hair = 12753; break;
						case Nation.Tyrean: return 0;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 15:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: hair = 12752; break;
						case Nation.Azhuran: return 8266;
						case Nation.Khemetar: hair = 12753; break;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 0;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 16:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 0;
						case Nation.Azhuran: hair = 8262; break;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 0;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 17:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 0;
						case Nation.Azhuran: hair = 12753; break;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 0;
						case Nation.Vhalurian: return 0;
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
				case Nation.Azhuran:
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
					
				case Nation.Khemetar:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2795;
						case 1:	return 2801;
						case 2:	return 2799;
						case 3:	return 2990;
						case 4:	return 1908;
						case 5:	return 1141;
						case 6:	return 1175;
						case 7:	return 1133;
						case 8:	return 1149;
						case 9:	return 2992;
					}
					break;
				}
					
				case Nation.Vhalurian:
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
					
				case Nation.Tyrean:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 2418;
						case 1:	return 2803;
						case 2:	return 2218;
						case 3:	return 2657;
						case 4:	return 2796;
						case 5:	return 1126;
						case 6:	return 2413;
						case 7:	return 1118;
						case 8:	return 2601;
						case 9:	return 1108;
					}
					break;
				}
					
				case Nation.Alyrian:
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
				case Nation.Azhuran:
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
			
				case Nation.Khemetar:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1815;
						case 1:	return 1814;
						case 2:	return 1813;
						case 3:	return 1812;
						case 4:	return 1851;
						case 5:	return 1850;
						case 6:	return 1849;
						case 7:	return 1822;
						case 8:	return 1823;
						case 9:	return 1824;
					}
					break;
				}
					
				case Nation.Vhalurian:
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
					
				case Nation.Tyrean:
				{
					switch( Utility.Random( 9 ) )
					{
						case 0:	return 1002;
						case 1:	return 1009;
						case 2:	return 1016;
						case 3:	return 1023;
						case 4:	return 1003;
						case 5:	return 1010;
						case 6:	return 1017;
						case 7:	return 1024;
						case 8:	return 1004;
						case 9:	return 1011;
					}
					break;
				}
					
				case Nation.Alyrian:
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
						case 1:	return 1147;
						case 2:	return 1145;
						case 3:	return 1062;
						case 4:	return 2426; 
						case 5:	return 1017;
						case 6:	return 1011;
						case 7:	return 2425; 
						case 8:	return 1813;
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
						case Nation.Alyrian: return 12725;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 12725;
						case Nation.Mhordul: return 12725;
						case Nation.Tyrean: return 12725;
						case Nation.Vhalurian: return 12725;
					}
					break;
				}
				case 12:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12726;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 12726;
						case Nation.Mhordul: return 12726;
						case Nation.Tyrean: return 12726;
						case Nation.Vhalurian: return 12726;
					}
					break;
				}
				case 13:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12737;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 12737;
						case Nation.Mhordul: return 12737;
						case Nation.Tyrean: return 12737;
						case Nation.Vhalurian: return 12737;
					}
					break;
				}
				case 14:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 8254;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 8254;
						case Nation.Tyrean: return 8254;
						case Nation.Vhalurian: return 8254;
					}
					break;
				}
				case 15:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 8268;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 8268;
						case Nation.Tyrean: return 8268;
						case Nation.Vhalurian: return 8268;
					}
					break;
				}
				case 16:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12731;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 12731;
						case Nation.Tyrean: return 12731;
						case Nation.Vhalurian: return 12731;
					}
					break;
				}
				case 17:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12730;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 12730;
						case Nation.Tyrean: return 12730;
						case Nation.Vhalurian: return 12730;
					}
					break;
				}
				case 18:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12733;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12733;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 19:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12735;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12735;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 20:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 12729;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12729;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 21:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 0;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12736;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 22:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 0;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12734;
						case Nation.Vhalurian: return 0;
					}
					break;
				}
				case 23:
				{
					switch ( nation )
					{		
						case Nation.Alyrian: return 0;
						case Nation.Azhuran: return 0;
						case Nation.Khemetar: return 0;
						case Nation.Mhordul: return 0;
						case Nation.Tyrean: return 12732;
						case Nation.Vhalurian: return 0;
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
				case Nation.Alyrian:
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
					
				case Nation.Azhuran:
				{
					if( female )
					{
						namelist = "Teolco,Tlilo,Xichua,Chihica,Malozua,Mella,Tamut," +
							"Tallit,Ildja,Kulla,Tisa,Dihya,Takirri,Zahrit,Zenet,Tiri," +
							"Chintal,Chintoci,Xuchtlia,Xichuatl,Tlinetzin,Huicinal," +
							"Miyahuel,Yolxau,Tantico,Malinali,Itzamna,Ixchel,Ixchup," +
							"Ixchab,Anacaona,Quilaco,Cusi,Canari,Viracocha,Catequil," +
							"Mayta,Huayna,Atahualpa,Sayri,Amaru,Anas-Collque," +
							"Cusi-Huarcay,Marca-Chimbo,Quispe-Sisa,Anta-Anclla," +
							"Pinca-Huaco,Cura-Ocllo,Varanca,Sahuara";
					}
					
					else
					{
						namelist = "Coatl,Toztin,Toltex,Opazin,Huemal,Acotochtl,Zimoc," +
							"Axocep,Xiuch,Telmoc,Xocitli,Ilchuitl,Oquitzin,Tzuicotl," +
							"Zantli,Xichotl,Xocatl,Xotin,Chipetl,Centzon,Tlican,Pactli," +
							"Hextli,Maxtli,Xiucan,Toa,Tangi,Tiru,Api,Piri,Kango,Temu," +
							"Kamo,Maki,Kenaru,Kaerune,Reku,Utran,Amenzu,Usan,Mezruc," +
							"Ikan,Menzu,Yun,Kannis,Zomalec,Apochtzin,Cuixico,Tipal,Ximal";
					}
					
					break;
				}
					
				case Nation.Khemetar:
				{
					if( female )
					{
						namelist = "Nesis,Nismet,Metrit,Sakhet,Sekeret,Sekhmet,Mehefra," +
							"Taret,Hekarit,Neret,Saherit,Nekeryt,Sibkhet,Khutah,Sura," +
							"Hura-Ta,Sera,Henem-Sep,Kethys,Setari,Marethys,Nem-Set,Usis," +
							"Mebt-Ep,Sentera,Maharet,Makare,Neferet,Reonet,Hath-Iunet," +
							"Taheret,Takhat,Abia,Almira,Badriyyah,Am-Ni,Siyah,Fadia," +
							"Haniyyah,Karida,Mariyah,Nazira,Qitarah,Rahna,Ruqayah," +
							"Shahara,Thara,Zuhaira,Duathor,Maneret";
					}
					
					else
					{
						namelist = "Imef,Nekhuem,Nemkasen,Kenses,Atep,Hesutis,Qemmut," +
							"Muris,Ahmunis,Nebertep,Shematep,Nofru,Pten,Ushetep,Senbek," +
							"Heseti,Anbis,Shubis,Imhattis,Illatis,Serel-Ut,Henkhen," +
							"Khepdera,Meshabti,Aminis,Onubiris,Am-Shu,Khen,Amontu,Batis," +
							"Setesh,Sareed,Saadir,Haqiim,Nakhir,Sakhir,Sartum,Ansis," +
							"Hakheru,Seti,Hassan,Iu-Khare,Aneb,Nebekh,Fakhir,Shakil," +
							"Ahmose,Ankhef,Sekhmire,Rekh-Tem";
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
					
				case Nation.Tyrean:
				{
					if( female )
					{
						namelist = "Asta,Astrid,Auda,Asvora,Brenda,Brynja,Eir,Elle," +
							"Ema,Erica,Fjorgyn,Frieda,Gerda,Gunnhilde,Haldana,Hallam," +
							"Helga,Hilde,Hulda,Idona,Inga,Inge,Ingrid,Kelda,Kirsten," +
							"Linnea,Liv,Mildri,Nanna,Norma,Olga,Ragnilde,Rana,Rona,Runa," +
							"Sigourney,Sigrid,Snora,Solveiq,Svanhilde,Ragna,Thora," +
							"Thordis,Tyra,Unne,Valda,Yngvild,Yule";
					}
					
					else
					{
						namelist = "Bodvar,Brian,Eric,Erland,Finn,Frey,Garrett,Garth," +
							"Geir,Gunnar,Gunther,Gus,Gustav,Hakan,Haldor,Harold,Hjordis," +
							"Holger,Howe,Ingemar,Ivar,Jorund,Kell,Kerr,Kirk,Latham," +
							"Logmar,Njord,Oddvar,Odell,Olaf,Ormar,Ranulf,Regin,Roscoe," +
							"Rothwell,Runolf,Somerled,Sorley,Stig,Sven,Swain,Tait,Tarn," +
							"Tate,Terje,Todd,Torvald,Yngvar";
					}
					
					break;
				}
					
				case Nation.Vhalurian:
				{
					if( female )
					{
						namelist = "Julia,Julianne,Marina,Maria,Nadia,Marianne," +
							"Teresa,Juliette,Vanessa,Angela,Emily,Hailey,Sarah," +
							"Jessica,Audrey,Sophia,Victoria,Olivia,Gabrielle,Rachel," +
							"Amanda,Isabella,Nicole,Hannah,Rebecca,Samantha,Jenna," +
							"Katelyn,Caroline,Ashley,Katherine,Elizabeth,Alexandra," +
							"Lily,Stephanie,Alyssa,Mia,Jennifer,Andrea,Melanie,Natalie," +
							"Angelina,Leah,Chloe,Diana,Michelle,Lillian,Molly,Ella," +
							"Lauren,Brooke,Madeline";
					}
					
					else
					{
						namelist = "Michael,Matthew,Ethan,Andrew,Daniel,Anthony," +
							"Christopher,Joseph,William,Alexander,David,Nicholas," +
							"James,John,Jonathan,Nathan,Samuel,Christian,Noah," +
							"Dylan,Benjamin,Brandon,Gabriel,Elijah,Kevin,Jack," +
							"Justin,Austin,Evan,Robert,Thomas,Luke,Mason,Aidan," +
							"Isaiah,Jordan,Connor,Jason,Cameron,Charles,Aaron,Lucas," +
							"Owen,Diego,Brian,Adam,Adrian,Kyle,Ian,Nathaniel,Alex," +
							"Julian,Sean,Carter,Cole,Wyatt,Steven,Timothy,Sebastian," +
							"Xavier,Seth,Richard";
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
                case Nation.Alyrian:
                    {
                        if (female)
                            surname += " verch ";
                        else
                            surname += " ap ";

                        surname += RandomName(nation, false);

                        return surname;
                    }
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
                case Nation.Tyrean:
                    {
                        surname += " " + RandomName(Nation.Tyrean, false);

                        if (female)
                            surname += "dottir";
                        else
                            surname += "son";

                        return surname;
                    }
                case Nation.Vhalurian:
                    {
                        String surname1 = "";
                        String surname2 = "";

                        //resources, places, people, products, etc.
                        string list1 = "Wood,Oak,Yew,Ash,Redwood,Metal,Copper,Bronze,Tin,Iron,Steel,Wool,Cotton,Crop,Egg,Chicken,Cattle,Horse,Fur,Hide,Leather,Wheat,Apple,Tree," +
                            "Orange,Sugar,Sand,Stone,Granite,Coal,Thick,Beast,Scale,Linen,Gold,Silver,Fish,Ale,Wine,Grape,Mountain,River,Plain,Village,Borough,City,North," +
                            "South,East,West,Road,Street,Forest,Sea,Ocean,Island,Cave,Jungle,Desert,Common,Coin,Noble,King,Queen,Knight,Sword,Spear,Dagger,Bow,Shield," +
                            "Helm,Plate,Chain,Arrow,Bolt,Chest,Quill,Book,Ink,Hammer,Tongs,Axe,Boot,Knife,Pie,Cloak,Bottle";

                        //actions,feelings,colors,adjectives
                        string list2 = "Add,Back,Bake,Beg,Bless,Blind,Boast,Boil,Burn,Call,Carry,Cheat,Claim,Chop,Crush,Chew,Dare,Dance,Dust,Drown,Earn,End," +
                            "Face,Fail,Fix,Fit,Fill,Fire,Fold,Follow,Gather,Gaze,Greet,Guard,Guide,Hand,Hate,Heal,Hope,Hunt,Jump,Kill,Kiss,Knot,Look,Love,Match,Mate," +
                            "Melt,Mend,Miss,Mourn,Move,Murder,Need,Note,Open,Pack,Pass,Peck,Pick,Please,Preach,Rain,Rhyme,Risk,Rot,Scare,Scold,Scorch,Seal,Share,Stay," +
                            "Switch,Talk,Tame,Tempt,Thank,Tie,Trade,Trap,Trust,Turn,Watch,Want,Wait,Free,Glee,Good,Bad,Strong,Ache,White,Black,Blue,Green,Yellow,Purple," +
                            "Red,Bright,Dark,Gray,Big,Little,Small,Long,High,Low,Soft,Hard,Top,Bottom";

                        if (Utility.RandomBool())
                        {
                            string[] firsthalf = list1.Split(',');
                            surname1 += firsthalf[Utility.Random(firsthalf.Length)];

                            string[] secondhalf = list2.Split(',');
                            surname2 += secondhalf[Utility.Random(secondhalf.Length)];

                            surname2 = surname2.ToLower();

                            surname = " " + surname1 + surname2;
                        }
                        else
                        {
                            string[] firsthalf = list2.Split(',');
                            surname1 += firsthalf[Utility.Random(firsthalf.Length)];

                            string[] secondhalf = list1.Split(',');
                            surname2 += secondhalf[Utility.Random(secondhalf.Length)];

                            surname2 = surname2.ToLower();

                            surname = " " + surname1 + surname2;
                        }

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
                case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Alyrian; } else { nation = Nation.Mhordul; } break; }
                case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Azhuran; } else { nation = Nation.Tyrean; } break; }
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
            }
			
			switch( nation )
			{
				case Nation.Alyrian:
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
					
				case Nation.Azhuran:
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
					
				case Nation.Khemetar:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new HalfApron() ); 
								m.EquipItem( new Sandals() ); 
								m.EquipItem( new GoldBeadNecklace() ); 
								m.EquipItem( new LongPlainDress( 2983 ) );
								break;
							}
							
							case 1:
							{
								m.EquipItem( new BaggyPants() ); 
								m.EquipItem( new FullApron() ); 
								m.EquipItem( new Shirt() ); 
								m.EquipItem( new Shoes() ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new Doublet() ); 
								m.EquipItem( new HalfApron() ); 
								m.EquipItem( new LongSkirt() ); 
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
								m.EquipItem( new BaggyPants() ); 
								m.EquipItem( new FullApron() ); 
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new Shoes() );  
								break;
							}
							
							case 1:
							{
								m.EquipItem( new LongSkirt() ); 
								m.EquipItem( new HalfApron() ); 
								m.EquipItem( new Sandals() ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new BaggyPants() );
								m.EquipItem( new Shoes() ); 
								m.EquipItem( new HalfApron() ); 
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
					
				case Nation.Tyrean:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0:
							{
								int hue = Utility.RandomNeutralHue();
								Bonnet bonnet = new Bonnet();
								bonnet.Hue = hue;
								m.EquipItem( bonnet ); 
								m.EquipItem( new PlainDress( hue ) ); 
								m.EquipItem( new HalfApron( hue ) );
								m.EquipItem( new FurBoots() );
								break;
							}
								
							case 1: 
							{
								m.EquipItem( new FurBoots() ); 
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new LongPants() );
								m.EquipItem( new LeatherGloves() );
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) );
								break;
							}
								
							case 2:
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new Skirt( hue ) ); 
								m.EquipItem( new HalfApron( hue ) );
								m.EquipItem( new Boots() );
								m.EquipItem( new FancyShirt() ); 
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
								m.EquipItem( new FurBoots() );
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 1:
							{
								m.EquipItem( new FurBoots() );
								m.EquipItem( new Shirt() ); 
								m.EquipItem( new FullApron( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new LongVest( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new Boots() ); 
								m.EquipItem( new LeatherGloves() ); 
								m.EquipItem( new Tunic( Utility.RandomNeutralHue() ) );
								m.EquipItem( new HalfApron( Utility.RandomNeutralHue() ) );
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
								break;
							}
						}
					}	
						
					break;
				}
					
				case Nation.Vhalurian:
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
                case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Alyrian; } else { nation = Nation.Mhordul; } break; }
                case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Azhuran; } else { nation = Nation.Tyrean; } break; }
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
            }

			switch( nation )
			{
				case Nation.Alyrian:
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
					
				case Nation.Azhuran:
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
								m.EquipItem( new AzhuranLeatherTunic() ); 
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
					
				case Nation.Khemetar:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: 
							{
								m.EquipItem( new ElegantShortDress() ); 
								m.EquipItem( new GoldRing() ); 
								m.EquipItem( new GoldBeadNecklace() ); 
								break;
							}
							
							case 1:
							{
								m.EquipItem( new GildedGown() ); 
								m.EquipItem( new FancyGloves() ); 
								m.EquipItem( new SilverBeadNecklace() ); 
								m.EquipItem( new SilverEarrings() ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new ElegantSkirt() ); 
								m.EquipItem( new FancyBra() ); 
								m.EquipItem( new GoldBracelet() ); 
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
								m.EquipItem( new ElegantTunic() ); 
								m.EquipItem( new BeltedPants() ); 
								m.EquipItem( new ThighBoots() ); 
								m.EquipItem( new GoldBeadNecklace() ); 
								break;
							}
							
							case 1:
							{
								m.EquipItem( new LongSkirt() ); 
								m.EquipItem( new GoldEarrings() ); 
								m.EquipItem( new GoldBracelet() ); 
								m.EquipItem( new GoldRing() ); 
								break;
							}
							
							case 2:
							{
								m.EquipItem( new BaggyPants() );
								m.EquipItem( new Shoes() ); 
								m.EquipItem( new Doublet() ); 
								m.EquipItem( new GoldRing() ); 
								m.EquipItem( new GoldBeadNecklace() ); 
								break;
							}
						}
						
						if( choice < 2 )
							m.EquipItem( new RunicCloak() );
						
						if( choice != 1 )
							m.EquipItem( new WaistSash() );
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
					
				case Nation.Tyrean:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0:
							{
								int hue = Utility.RandomSlimeHue();
								Bonnet bonnet = new Bonnet();
								bonnet.Hue = hue;
								m.EquipItem( bonnet ); 
								m.EquipItem( new LacedGown( hue ) ); 
								break;
							}
								
							case 1: 
							{
								int hue = Utility.RandomSlimeHue();
								FloppyHat floppy = new FloppyHat();
								floppy.Hue = hue;
								m.EquipItem( floppy ); 
								m.EquipItem( new LongOrnateDress( hue ) );
								break;
							}
								
							case 2:
							{
								m.EquipItem( new GoldRing() );
								m.EquipItem( new LongDress( Utility.RandomSlimeHue() ) ); 
								break;
							}
						}
						
						m.EquipItem( new ElegantShoes() ); 
						
						if( choice < 2 )
						{
							m.EquipItem( new GoldBracelet() );
							m.EquipItem( new GoldBeadNecklace() );
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0:
							{ 
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new ExpensiveShirt() );
								m.EquipItem( new ThighBoots() );
								m.EquipItem( new ElegantCloak( hue ) );
								m.EquipItem( new ElegantTunic( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 1:
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new ExpensiveShirt() ); 
								m.EquipItem( new ExpensiveCloak( hue ) ); 
								m.EquipItem( new ElegantSurcoat( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 2: 
							{
								int hue = Utility.RandomNeutralHue();
								m.EquipItem( new ExpensiveCloak( hue ) ); 
								m.EquipItem( new ExpensiveShirt() ); 
								m.EquipItem( new PaddedVest( Utility.RandomNeutralHue() ) );
								break;
							}
						}
						
						m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
						
						if( choice > 0 )
						{
							m.EquipItem( new BlackLeatherBoots() ); 
						}
					}	
						
					break;
				}
					
				case Nation.Vhalurian:
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
                case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Alyrian; } else { nation = Nation.Mhordul; } break; }
                case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Azhuran; } else { nation = Nation.Tyrean; } break; }
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
            }
			
			switch( nation )
			{
				case Nation.Alyrian:
				{
					GreenBeret greenberet = new GreenBeret();
					greenberet.Hue = 2587;
					m.EquipItem( greenberet );
						            
					switch( choice )
					{
						case 0:
						{
							AlyrianChainChest chest = new AlyrianChainChest();
							chest.Resource = CraftResource.Bronze;
							
							AlyrianChainLegs legs = new AlyrianChainLegs();
							legs.Resource = CraftResource.Bronze;
							
							AlyrianChainArms arms = new AlyrianChainArms();
							arms.Resource = CraftResource.Bronze;
							
							AlyrianChainGorget gorget = new AlyrianChainGorget();
							gorget.Resource = CraftResource.Bronze;
							
							m.EquipItem( chest );
							m.EquipItem( legs );
							m.EquipItem( arms );
							m.EquipItem( gorget ); 
							m.EquipItem( new Cloak( 2587 ) );
							            
				            if( m.Female )
				            {
				            	AlyrianLeafShield shield = new AlyrianLeafShield();
								shield.Resource = CraftResource.Bronze;
								
								AlyrianSabre sabre = new AlyrianSabre();
								sabre.Resource = CraftResource.Bronze;
				            	
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sabre ); 
				            	m.EquipItem( new FemaleKilt( 2587 ) );
				            }
				            
				            else
				            {
				            	AlyrianTwoHandedAxe axe = new AlyrianTwoHandedAxe();
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
				            	AlyrianRoundShield shield = new AlyrianRoundShield();
								shield.Resource = CraftResource.Bronze;
								
								AlyrianLongsword sword = new AlyrianLongsword();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            	m.EquipItem( new ElegantFemaleKilt( 2587 ) );
				            	m.EquipItem( new MetallicBra() );
				            	m.EquipItem( new ElegantShoes() );
				            }
				            
				            else
				            {
				            	AlyrianClaymore sword = new AlyrianClaymore();
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
				            	AlyrianLongbow bow = new AlyrianLongbow();
								bow.Resource = CraftResource.Redwood;
								
				            	m.EquipItem( bow ); 
				            	m.EquipItem( new ElegantKilt( 2587 ) );
				            	m.EquipItem( new MetallicBra() );
				            	
				            }
				            
				            else
				            {
				            	AlyrianGiantBow bow = new AlyrianGiantBow();
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
					
				case Nation.Azhuran:
				{
					switch( choice )
					{
						case 0:
						{
							AzhuranHelm helm = new AzhuranHelm();
							helm.Resource = CraftResource.Bronze;
							
							AzhuranSpikedChainChest chest = new AzhuranSpikedChainChest();
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
				            	AzhuranKiteShield shield = new AzhuranKiteShield();
								shield.Resource = CraftResource.Bronze;
								
								AzhuranShortsword sword = new AzhuranShortsword();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            	m.EquipItem( new ElegantWaistCloth( 2810 ) );
				            }
				            
				            else
				            {
				            	AzhuranRoundShield shield = new AzhuranRoundShield();
								shield.Resource = CraftResource.Bronze;
								
								AzhuranBroadsword sword = new AzhuranBroadsword();
								sword.Resource = CraftResource.Bronze;
								
				            	m.EquipItem( shield ); 
				            	m.EquipItem( sword ); 
				            }
				            
							break;
						}
							
						case 1:
						{ 
							AzhuranLeatherTunic chest = new AzhuranLeatherTunic();
							chest.Resource = CraftResource.BeastLeather;
							
							AzhuranLeatherPauldrons pauldrons = new AzhuranLeatherPauldrons();
							pauldrons.Resource = CraftResource.BeastLeather;
							
							AzhuranLeatherBoots boots = new AzhuranLeatherBoots();
							boots.Resource = CraftResource.BeastLeather;
							
							AzhuranLeatherLegs legs = new AzhuranLeatherLegs();
							legs.Resource = CraftResource.BeastLeather;
							
							LeatherGloves gloves = new LeatherGloves();
							gloves.Resource = CraftResource.BeastLeather;
							
							LeatherArms arms = new LeatherArms();
							arms.Resource = CraftResource.BeastLeather;
							
							AzhuranSpear spear = new AzhuranSpear();
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
								AzhuranShortbow bow = new AzhuranShortbow();
								bow.Resource = CraftResource.Redwood;
							
				            	m.EquipItem( bow ); 
				            	m.EquipItem( new MetallicBra() );
				            	m.EquipItem( new WaistCloth( 2810 ) );
				            }
							
							else
							{
								AzhuranBoomerang bow = new AzhuranBoomerang();
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
					
				case Nation.Khemetar:
				{
					switch( choice )
					{
						case 0:
						{
							KhemetarScaleChest chest = new KhemetarScaleChest();
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
							
							KhemetarScaleHelmet helmet = new KhemetarScaleHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							KhemetarAxe axe = new KhemetarAxe();
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
							
							KhemetarScaleChest chest = new KhemetarScaleChest();
							chest.Resource = CraftResource.Bronze;
							chest.Hue = 2947;
							m.EquipItem( chest );
							
							KhemetarScaleLegs legs = new KhemetarScaleLegs();
							legs.Resource = CraftResource.Bronze;
							legs.Hue = 2947;
							m.EquipItem( legs );
							
							KhemetarScaleArms arms = new KhemetarScaleArms();
							arms.Resource = CraftResource.Bronze;
							arms.Hue = 2947;
							m.EquipItem( arms );
							
							RingmailGloves gloves = new RingmailGloves();
							gloves.Resource = CraftResource.Bronze;
							gloves.Hue = 2947;
							m.EquipItem( gloves );
							
							KhemetarScaleHelmet helmet = new KhemetarScaleHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							KhemetarKhopesh sword = new KhemetarKhopesh();
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
							
							KhemetarScaleChest chest = new KhemetarScaleChest();
							chest.Resource = CraftResource.Bronze;
							chest.Hue = 2947;
							m.EquipItem( chest );
							
							KhemetarScaleLegs legs = new KhemetarScaleLegs();
							legs.Resource = CraftResource.Bronze;
							legs.Hue = 2947;
							m.EquipItem( legs );
							
							KhemetarScaleHelmet helmet = new KhemetarScaleHelmet();
							helmet.Resource = CraftResource.Bronze;
							helmet.Hue = 2947;
							m.EquipItem( helmet );
							
							RunicCloak cloak = new RunicCloak();
							cloak.Hue = 2795;
							m.EquipItem( cloak );
							
							KhemetarLongbow bow = new KhemetarLongbow();
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
					
					MhordulBoneArms mba = new MhordulBoneArms();
					mba.Hue = 2101;
					m.EquipItem( mba );
							
					switch( choice )
					{
						case 0:
						{
							MhordulHornedSkullHelm mhsh = new MhordulHornedSkullHelm();
							mhsh.Hue = 2101;
							m.EquipItem( mhsh );
							
							MhordulBoneChest mbc = new MhordulBoneChest();
							mbc.Hue = 2101;
							m.EquipItem( mbc );
							
							MhordulBoneLegs mbl = new MhordulBoneLegs();
							mbl.Hue = 2101;
							m.EquipItem( mbl );
							
							MhordulBoneGloves mbg = new MhordulBoneGloves();
							mbg.Hue = 2101;
							m.EquipItem( mbg );
							
							MhordulBoneShield mbs = new MhordulBoneShield();
							mbs.Hue = 2101;
							m.EquipItem( mbs );
							            
				            if( m.Female )
				            {
				            	m.EquipItem( new MhordulBoneSword() ); 
				            	m.EquipItem( new SmallRaggedSkirt( 1194 ) );
				            }
				            
				            else
				            {
				            	m.EquipItem( new MhordulBoneAxe() ); 
				            }
				            
							break;
						}
							
						case 1:
						{ 
			            	MhordulBoneHelm mbh = new MhordulBoneHelm();
							mbh.Hue = 2101;
							m.EquipItem( mbh );
							
							MhordulBoneLegs mbl = new MhordulBoneLegs();
							mbl.Hue = 2101;
							m.EquipItem( mbl );
							
							MhordulBoneGloves mbg = new MhordulBoneGloves();
							mbg.Hue = 2101;
							m.EquipItem( mbg );
							            
				            if( m.Female )
				            {
				            	m.EquipItem( new MhordulBoneSpear() ); 
				            	m.EquipItem( new SmallRaggedSkirt( 1194 ) );
				            	m.EquipItem( new RaggedBra( 1194 ) );
				            }
				            
				            else
				            {
				            	m.EquipItem( new MhordulBoneScythe() ); 
				            	m.EquipItem( new WaistCloth( 1194 ) );
				            }
				            
							break;
						}
							
						case 2:
						{ 
							MhordulBoneHelm mbh = new MhordulBoneHelm();
							mbh.Hue = 2101;
							m.EquipItem( mbh );
							
							m.EquipItem( new MhordulBoneBow() );
							
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
					
				case Nation.Tyrean:
				{
					Surcoat coat = new Surcoat();
					coat.ItemID = 15477;
					coat.Name = "Tyrean Military Surcoat";
					coat.Hue = 2741;
					m.EquipItem( coat );
							
					switch( choice )
					{
						case 0:
						{
							TyreanHalfPlateChest thpc = new TyreanHalfPlateChest();
							thpc.Resource = CraftResource.Bronze;
							thpc.Hue = 1899;
							m.EquipItem( thpc );
							
							TyreanHalfPlateLegs thpl = new TyreanHalfPlateLegs();
							thpl.Resource = CraftResource.Bronze;
                            thpl.Hue = 1899;
							m.EquipItem( thpl );
							
							TyreanHalfPlateSabatons thps = new TyreanHalfPlateSabatons();
							thps.Resource = CraftResource.Bronze;
                            thps.Hue = 1899;
							m.EquipItem( thps );
							
							TyreanHalfPlateArms thpa = new TyreanHalfPlateArms();
							thpa.Resource = CraftResource.Bronze;
                            thpa.Hue = 1899;
							m.EquipItem( thpa );
							
							TyreanHalfPlateGloves thpg = new TyreanHalfPlateGloves();
							thpg.Resource = CraftResource.Bronze;
                            thpg.Hue = 1899;
							m.EquipItem( thpg );
							
							TyreanHalfPlateGorget thpo = new TyreanHalfPlateGorget();
							thpo.Resource = CraftResource.Bronze;
                            thpo.Hue = 1899;
							m.EquipItem( thpo );
							
							TyreanKiteShield tks = new TyreanKiteShield();
							tks.Resource = CraftResource.Bronze;
                            tks.Hue = 1899;
							m.EquipItem( tks );
							
							m.EquipItem( new Cloak( 1445 ) );
							
							TyreanWingedHelm twh = new TyreanWingedHelm();
			            	twh.Resource = CraftResource.Bronze;
                            twh.Hue = 1899;
							m.EquipItem( twh );
							            
				            if( m.Female )
				            {
				            	TyreanWarAxe axe = new TyreanWarAxe();
								axe.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( axe ); 
				            }
				            
				            else
				            {
				            	TyreanOrnateAxe axe = new TyreanOrnateAxe();
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
							
							TyreanHornedHelm thh = new TyreanHornedHelm();
							thh.Resource = CraftResource.Bronze;
                            thh.Hue = 1899;
							m.EquipItem( thh );
							
							FurBoots boots = new FurBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2741;
						
							m.EquipItem( boots );
							            
				            if( m.Female )
				            {
				            	TyreanHarpoon weapon = new TyreanHarpoon();
								weapon.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( weapon ); 
				            }
				            
				            else
				            {
				            	TyreanBattleAxe weapon = new TyreanBattleAxe();
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
							
							TyreanCompositeBow bow = new TyreanCompositeBow();
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
					
				case Nation.Vhalurian:
				{
					Surcoat coat = new Surcoat();
					coat.Name = "Vhalurian Military Surcoat";
					coat.Hue = 1327;
					coat.ItemID = 15479;
					m.EquipItem( coat );
							
					switch( choice )
					{
						case 0:
						{
							VhalurianOrnatePlateChest vopc = new VhalurianOrnatePlateChest();
							vopc.Resource = CraftResource.Bronze;
							vopc.Hue = 2101;
							m.EquipItem( vopc );
							
							VhalurianOrnatePlateLegs vopl = new VhalurianOrnatePlateLegs();
							vopl.Resource = CraftResource.Bronze;
							vopl.Hue = 2101;
							m.EquipItem( vopl );
							
							VhalurianOrnatePlateGorget vopo = new VhalurianOrnatePlateGorget();
							vopo.Resource = CraftResource.Bronze;
							vopo.Hue = 2101;
							m.EquipItem( vopo );
							
							PlateSabatons ps = new PlateSabatons();
							ps.Resource = CraftResource.Bronze;
							ps.Hue = 2105;
							m.EquipItem( ps );
							
							VhalurianOrnatePlateArms vopa = new VhalurianOrnatePlateArms();
							vopa.Resource = CraftResource.Bronze;
							vopa.Hue = 2101;
							m.EquipItem( vopa );
							
							VhalurianOrnatePlateGloves vopg = new VhalurianOrnatePlateGloves();
							vopg.Resource = CraftResource.Bronze;
							vopg.Hue = 2101;
							m.EquipItem( vopg );
							
							VhalurianOrnateKiteShield voks = new VhalurianOrnateKiteShield();
							voks.Resource = CraftResource.Bronze;
							voks.Hue = 2102;
							m.EquipItem( voks );
							
							m.EquipItem( new Cloak( 1327 ) );
							            
				            if( m.Female )
				            {
				            	VhalurianMace mace = new VhalurianMace();
								mace.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( mace ); 
				            	
				            	VhalurianOrnateHelm voh = new VhalurianOrnateHelm();
				            	voh.Resource = CraftResource.Bronze;
								voh.Hue = 2102;
								m.EquipItem( voh );
				            }
				            
				            else
				            {
				            	VhalurianWarHammer mace = new VhalurianWarHammer();
								mace.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( mace );
				            	
				            	VhalurianOrnatePlateHelm voph = new VhalurianOrnatePlateHelm();
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
							
							VhalurianMetalKiteShield vmks = new VhalurianMetalKiteShield();
							vmks.Resource = CraftResource.Bronze;
							vmks.Hue = 2101;
							m.EquipItem( vmks );
							
							LeatherBoots boots = new LeatherBoots();
							boots.Resource = CraftResource.BeastLeather;
							boots.Hue = 2101;
							
							m.EquipItem( boots );
							            
				            if( m.Female )
				            {
				            	VhalurianGladius sword = new VhalurianGladius();
								sword.Resource = CraftResource.Bronze;
							
				            	m.EquipItem( sword );
				            }
				            
				            else
				            {
				            	VhalurianBroadsword sword = new VhalurianBroadsword();
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
							
							VhalurianLongbow bow = new VhalurianLongbow();
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
                case Nation.Imperial: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
                case Nation.Sovereign: { if (Utility.RandomBool()) { nation = Nation.Alyrian; } else { nation = Nation.Mhordul; } break; }
                case Nation.Society: { if (Utility.RandomBool()) { nation = Nation.Azhuran; } else { nation = Nation.Tyrean; } break; }
				case Nation.Insularii: { if (Utility.RandomBool()) { nation = Nation.Vhalurian; } else { nation = Nation.Khemetar; } break; }
            }

			switch( nation )
			{
				case Nation.Alyrian:
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
					
				case Nation.Azhuran:
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
					
				case Nation.Khemetar:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new LongPlainDress() ); break;
							case 1: m.EquipItem( new LongDress() ); break;
							case 2: m.EquipItem( new BaggyPants() ); break;
						}
						
						if( choice < 2 )
							m.EquipItem( new Sandals() );
						
						else
						{
							m.EquipItem( new WaistSash() );
							m.EquipItem( new Shirt() );
						}
					}
					
					else
					{
						switch( choice )
						{
							case 0: m.EquipItem( new LongSkirt() ); break;
							case 1: m.EquipItem( new ElegantWaistCloth() ); break;
							case 2: m.EquipItem( new BaggyPants() ); break;
						}
						
						if( choice < 2 )
							m.EquipItem( new Sandals() );
						
						else
						{
							m.EquipItem( new WaistSash() );
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
					
				case Nation.Tyrean:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new LacedGown( Utility.RandomSlimeHue() ) ); break;
							case 1: m.EquipItem( new RunedDress( Utility.RandomSlimeHue() ) ); break;
							case 2:
							{
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new Skirt( Utility.RandomSlimeHue() ) ); 
								m.EquipItem( new Boots() ); 
								break;
							}
						}
						
						if( choice < 2 )
							m.EquipItem( new FurBoots() ); 
					}
					
					else
					{
						switch( choice )
						{
							case 0:
							{
								m.EquipItem( new LongPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new FancyShirt() );
								m.EquipItem( new LongVest( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 1:
							{
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) ); 
								m.EquipItem( new ExpensiveShirt( Utility.RandomNeutralHue() ) ); 
								break;
							}
								
							case 2: 
							{
								m.EquipItem( new BeltedPants( Utility.RandomNeutralHue() ) );
								m.EquipItem( new Tunic( Utility.RandomNeutralHue() ) );
								break;
							}
						}
						
						m.EquipItem( new FurBoots() ); 
					}	
						
					break;
				}
					
				case Nation.Vhalurian:
				{
					if( m.Female )
					{
						switch( choice )
						{
							case 0: m.EquipItem( new BeltedDress( Utility.RandomSlimeHue() ) ); break;
							case 1: m.EquipItem( new ElegantGown( Utility.RandomSlimeHue() ) ); break;
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
