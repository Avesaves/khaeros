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

                        goons = new GenericWarrior();
                        goons2 = new GenericWarrior();
                        goons3 = new GenericWarrior();
                        goons4 = new GenericWarrior();

                        BaseCreature mop = m_Prisoner as BaseCreature;
                        BaseCreature gons = goons as BaseCreature;
                        BaseCreature gons2 = goons2 as BaseCreature;
                        BaseCreature gons3 = goons3 as BaseCreature;
                        BaseCreature gons4 = goons4 as BaseCreature;
                        gons.WikiConfig = "bloodspirit";
                        gons2.WikiConfig = "bloodspirit";
                        gons3.WikiConfig = "bloodspirit";
                        gons4.WikiConfig = "bloodspirit";
                        mop.WikiConfig = "bloodspirit";
                        gons2.Team = 1;
                        gons3.Team = 1;
                        gons4.Team = 1;
                        gons.Team = 1; 
                        mop.LoadWikiConfig = true; 
                        gons.LoadWikiConfig = true;
                        gons2.LoadWikiConfig = true;
                        gons3.LoadWikiConfig = true;
                        gons4.LoadWikiConfig = true;
                        break;
                    }
                case 1:
                    {
                        m_Prisoner = new Quest9();
                        BaseCreature mop = m_Prisoner as BaseCreature;
                        mop.WikiConfig = "rym_headmaster";
                        mop.LoadWikiConfig = true;
                        m_Prisoner.Name = "A spirit of hate";
                        m_Prisoner.BodyValue = 400;
                        mop.AI = AIType.AI_Mage;
                        goons = new WesternBrigand();
                        goons2 = new SouthernBrigand();
                        goons3 = new NorthernBrigand();
                        goons4 = new SouthernNoble();
                        break;
                    }
                case 2:
                    {
                        m_Prisoner = new Quest9();
                        BaseCreature mop = m_Prisoner as BaseCreature;
                        mop.WikiConfig = "graftedspiderabomination";
                        mop.LoadWikiConfig = true; 
                        goons = new SkeletalSoldier();
                        goons2 = new SkeletalSoldier();
                        goons3 = new SkeletalSoldier();
                        goons4 = new SkeletalSoldier();
                        BaseCreature gons = goons as BaseCreature;
                        BaseCreature gons2 = goons2 as BaseCreature;
                        BaseCreature gons3 = goons3 as BaseCreature;
                        BaseCreature gons4 = goons4 as BaseCreature;
                        gons2.Team = 1;
                        gons3.Team = 1;
                        gons4.Team = 1;
                        gons.Team = 1; 
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

                        BaseCreature gons = goons as BaseCreature;
                        BaseCreature gons2 = goons2 as BaseCreature;
                        BaseCreature gons3 = goons3 as BaseCreature;
                        BaseCreature gons4 = goons4 as BaseCreature;
                        gons.WikiConfig = "cavetrollrager";
                        gons2.WikiConfig = "cavetrollrager";
                        gons3.WikiConfig = "cavetrollrager";
                        gons4.WikiConfig = "cavetrollrager";
                        gons2.Team = 1;
                        gons3.Team = 1;
                        gons4.Team = 1;
                        gons.Team = 1; 
                        gons.LoadWikiConfig = true;
                        gons2.LoadWikiConfig = true;
                        gons3.LoadWikiConfig = true;
                        gons4.LoadWikiConfig = true;

                        break;
                    }
                case 5:
                    {
                        m_Prisoner = new Quest7();
                        m_Prisoner.Name = "A star wyrm"; 
                        goons = new Wyvern();
                        goons2 = new Wyvern();
                        goons3 = new Wyvern();
                        goons4 = new Wyvern();

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
