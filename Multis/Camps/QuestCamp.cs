using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Multis
{
	public class QuestCamp : BaseCamp
	{
		private Mobile m_Prisoner;
        private Mobile goons;
        private Mobile goons2;
        private Mobile goons3;
        private Mobile goons4;
        
		private BaseDoor m_Gate;

		[Constructable]
		public QuestCamp() : base( 0x25 )
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
                        m_Prisoner = new Quest1();
                        goons = new HobgoblinWarrior();
                        goons2 = new HobgoblinWarrior();
                        goons3 = new HobgoblinWarrior();
                        goons4 = new HobgoblinWarrior(); 
                        break;
                    }
                case 1:
                    {
                        m_Prisoner = new Quest2();
                        goons = new ConstrictingVine();
                        goons2 = new ConstrictingVine();
                        goons3 = new ConstrictingVine();
                        goons4 = new ConstrictingVine();
                        break;
                    }
                case 2:
                    {
                        m_Prisoner = new Quest3();
                        goons = new CrystalElemental();
                        goons2 = new CrystalElemental();
                        goons3 = new CrystalElemental();
                        goons4 = new CrystalElemental();
                        break;
                    }
                case 3:
                    {
                        m_Prisoner = new Quest4();
                        goons = new DisplacerBeast();
                        goons2 = new DisplacerBeast();
                        goons3 = new DisplacerBeast();
                        goons4 = new DisplacerBeast();
                        break;
                    }
                case 4:
                    {
                        m_Prisoner = new Quest5();
                        goons = new HookHorror();
                        goons2 = new HookHorror();
                        goons3 = new HookHorror();
                        goons4 = new HookHorror(); 
                        break;
                    }
                case 5:
                    {
                        m_Prisoner = new Quest6();
                        m_Prisoner.Name = "A star creature"; 
                        goons = new CaveTroll();
                        goons2 = new CaveTroll();
                        goons3 = new CaveTroll();
                        goons4 = new CaveTroll();
                        BaseCreature gons = goons as BaseCreature;
                        BaseCreature gons2 = goons2 as BaseCreature;
                        BaseCreature gons3 = goons3 as BaseCreature;
                        BaseCreature gons4 = goons4 as BaseCreature;
                        gons.WikiConfig = "trollhound";
                        gons.LoadWikiConfig = true;
                        gons.Name = "A crystalline entity"; 
                        gons2.WikiConfig = "trollhound";
                        gons2.LoadWikiConfig = true;
                        gons2.Name = "A crystalline entity"; 
                        gons3.WikiConfig = "trollhound";
                        gons3.LoadWikiConfig = true;
                        gons3.Name = "A crystalline entity"; 
                        gons4.WikiConfig = "trollhound";
                        gons4.LoadWikiConfig = true;
                        gons4.Name = "A crystalline entity"; 
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
		public QuestCamp( Serial serial ) : base( serial )
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
