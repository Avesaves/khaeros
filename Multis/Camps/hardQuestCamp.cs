using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Multis
{
	public class hardQuestCamp : BaseCamp
	{
		private Mobile m_Prisoner;
        private Mobile goons;
        private Mobile goons2;
        private Mobile goons3;
        private Mobile goons4;
        
		private BaseDoor m_Gate;

		[Constructable]
		public hardQuestCamp() : base( 0x25 )
		{
		}

		public override void AddComponents()
		{
			//IronGate gate = new IronGate( DoorFacing.EastCCW );
			//m_Gate = gate;

			//gate.KeyValue = Key.RandomValue();
			//gate.Locked = true;

			//AddItem( gate, -2, 1, 0 );
            GiantRock rock = new GiantRock(); 
			//MetalChest chest = new MetalChest();
            rock.ItemID = 0x3660;
            rock.Hue = 2832;
            rock.Movable = false; 
            AddItem(rock, 1, 0, 0); 
			//chest.ItemID = 0xE7C;
		//	chest.DropItem( new Key( KeyType.Iron, gate.KeyValue ) );

			//TreasureMapChest.Fill( chest, 2 );

			//AddItem( chest, 4, 4, 0 );

			//AddMobile( new GoblinArcher(), 15, 0, -2, 0 );
		//	AddMobile( new GoblinScavenger(), 15, 0,  1, 0 );
		//	AddMobile( new HobgoblinWarrior(), 15, 0, -1, 0 );
		//	AddMobile( new HobgoblinRider(), 15, 0,  0, 0 );

			switch ( Utility.Random( 6 ) )
			{
                case 0:
                    {
                        m_Prisoner = new Quest9();
                        m_Prisoner.WikiConfig = "bloodspirit";
                        m_Prisoner.LoadWikiConfig = true; 
                        goons = new GenericWarrior();
                        goons2 = new GenericWarrior();
                        goons3 = new GenericWarrior();
                        goons4 = new GenericWarrior();
                        goons.WikiConfig = "bloodspirit";
                        goons2.WikiConfig = "bloodspirit";
                        goons3.WikiConfig = "bloodspirit";
                        goons4.WikiConfig = "bloodspirit";
                        goons.LoadWikiConfig = true;
                        goons2.LoadWikiConfig = true;
                        goons3.LoadWikiConfig = true;
                        goons4.LoadWikiConfig = true;
                        break;
                    }
                case 1:
                    {
                        m_Prisoner = new Quest9();
                        m_Prisoner.WikiConfig = "rym_headmaster";
                        m_Prisoner.LoadWikiConfig = true;
                        m_Prisoner.Name = "A spirit of hate";
                        goons = new WesternBrigand();
                        goons2 = new SouthernBrigand();
                        goons3 = new NorthernBrigand();
                        goons4 = new SouthernNoble();
                        break;
                    }
                case 2:
                    {
                        m_Prisoner = new Quest9();
                        m_Prisoner.WikiConfig = "graftedspiderabomination";
                        m_Prisoner.LoadWikiConfig = true; 
                        goons = new SkeletalSoldier();
                        goons2 = new SkeletalSoldier();
                        goons3 = new SkeletalSoldier();
                        goons4 = new SkeletalSoldier();
                        break;
                    }
                case 3:
                    {
                        m_Prisoner = new Quest8();
                        m_Prisoner.Name = "An unknown horror";

                        goons = new WesternBrigand();
                        goons2 = new SouthernBrigand();
                        goons3 = new NorthernBrigand();
                        goons4 = new SouthernNoble();
                        break;
                    }
                case 4:
                    {
                        m_Prisoner = new Stalker();
                        goons = new CaveTroll();
                        goons2 = new CaveTroll();
                        goons3 = new CaveTroll();
                        goons4 = new CaveTroll();
                        goons.WikiConfig = "cavetrollrager";
                        goons2.WikiConfig = "cavetrollrager";
                        goons3.WikiConfig = "cavetrollrager";
                        goons4.WikiConfig = "cavetrollrager";
                        goons.LoadWikiConfig = true;
                        goons2.LoadWikiConfig = true;
                        goons3.LoadWikiConfig = true;
                        goons4.LoadWikiConfig = true;

                        break;
                    }
                case 5:
                    {
                        m_Prisoner = new Quest7();
                        m_Prisoner.Name = "A star wyrm"; 
                        goons = new wyvern();
                        goons2 = new wyvern();
                        goons3 = new wyvern();
                        goons4 = new wyvern();

                        break;
                    }
			}

			//m_Prisoner.YellHue = Utility.RandomList( 0x57, 0x67, 0x77, 0x87, 0x117 );
             
			AddMobile( m_Prisoner, 15, 0, 0, 0 );
            AddMobile(goons, 10, -2, 2, 0);
            AddMobile(goons2, 10, 2, 1, 0);
            AddMobile(goons3, 10, -2, -1, 0);
            AddMobile(goons4, 10, 2, -2, 0);
		}
        /*
		    public override void OnEnter( Mobile m )
		{
			base.OnEnter( m );

			if ( m.Player && m_Prisoner != null && m_Gate != null && m_Gate.Locked )
			{
				int number;

				switch ( Utility.Random( 4 ) )
				{
					default:
					case 0: number =  502264; break; // Help a poor prisoner!
					case 1: number =  502266; break; // Aaah! Help me!
					case 2: number = 1046000; break; // Help! These savages wish to end my life!
					case 3: number = 1046003; break; // Quickly! Kill them for me! HELP!!
				}

				m_Prisoner.Yell( number );
			}
		}
        */
		public hardQuestCamp( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Prisoner );
			writer.Write( m_Gate );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Prisoner = reader.ReadMobile();
					m_Gate = reader.ReadItem() as BaseDoor;
					break;
				}
			}
		}
	}
}
