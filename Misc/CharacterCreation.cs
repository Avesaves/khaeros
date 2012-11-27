using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;

namespace Server.Misc
{
	public class CharacterCreation
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.CharacterCreated += new CharacterCreatedEventHandler( EventSink_CharacterCreated );
		}

		private static void AddBackpack( Mobile m )
		{
			Container pack = m.Backpack;
			
			PlayerMobile pm = m as PlayerMobile;
			
			pm.RawStr = 10;
			pm.AvatarID = 1076;
			pm.RawDex = 10;
			pm.RawInt = 10;
			pm.MaxHits = 10;
			pm.MaxStam = 10;
			pm.MaxMana = 10;
			pm.Hits = 10;
			pm.Stam = 10;
			pm.Mana = 10;
			pm.Criminal = true;
				
			if ( pack == null )
			{
				pack = new ArmourBackpack();
				pack.Movable = false;

				m.AddItem( pack );
			}
			
			PackItem( new RedBook( "a book", m.Name, 20, true ) );
			PackItem( new Copper( Utility.Random( 75, 150 ) ) ); // Starting gold can be customized here
			PackItem( new Dagger() );
			PackItem( new Candle() );
		}

		private static Item MakeNewbie( Item item )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Newbied;

			return item;
		}

		private static void PlaceItemIn( Container parent, int x, int y, Item item )
		{
			parent.AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}

		private static void FillBankAOS( Mobile m )
		{

		}

		private static void FillBankbox( Mobile m )
		{

		}

		private static void AddPowerScrolls( BankBox bank )
		{

		}

		private static void AddShirt( Mobile m, int shirtHue )
		{
			int hue = Utility.ClipDyedHue( shirtHue & 0x3FFF );

			switch ( Utility.Random( 3 ) )
			{
				case 0: EquipItem( new Shirt( hue ), true ); break;
				case 1: EquipItem( new FancyShirt( hue ), true ); break;
				case 2: EquipItem( new Doublet( hue ), true ); break;
			}
		}

		private static void AddPants( Mobile m, int pantsHue )
		{
			int hue = Utility.ClipDyedHue( pantsHue & 0x3FFF );

			if ( m.Female )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: EquipItem( new Skirt( hue ), true ); break;
					case 1: EquipItem( new Kilt( hue ), true ); break;
				}
			}
			else
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: EquipItem( new LongPants( hue ), true ); break;
					case 1: EquipItem( new ShortPants( hue ), true ); break;
				}
			}
		}

		private static void AddShoes( Mobile m )
		{
			EquipItem( new Shoes( Utility.RandomYellowHue() ), true );
		}

		private static Mobile CreateMobile( Account a )
		{
			if ( a.Count >= a.Limit )
				return null;

			for ( int i = 0; i < a.Length; ++i )
			{
				if ( a[i] == null )
					return (a[i] = new PlayerMobile());
			}

			return null;
		}

		private static void EventSink_CharacterCreated( CharacterCreatedEventArgs args )
		{
			if ( !VerifyProfession( args.Profession ) )
				args.Profession = 0;

			Mobile newChar = CreateMobile( args.Account as Account );

			if ( newChar == null )
			{
				Console.WriteLine( "Login: {0}: Character creation failed, account full", args.State );
				return;
			}
			
			newChar.Race = Race.Human;

			args.Mobile = newChar;
			m_Mobile = newChar;

			newChar.Player = true;
			newChar.AccessLevel = args.Account.AccessLevel;
			newChar.Female = args.Female;
			//newChar.Body = newChar.Female ? 0x191 : 0x190;

			/*if( Core.Expansion >= args.Race.RequiredExpasnion )
				newChar.Race = args.Race;	//Sets body
			else*/
				newChar.Race = Race.DefaultRace;

			//newChar.Hue = Utility.ClipSkinHue( args.Hue & 0x3FFF ) | 0x8000;
			newChar.Hue = newChar.Race.ClipSkinHue( args.Hue & 0x3FFF ) | 0x8000;

			newChar.Hunger = 20;
			newChar.Thirst = 20;

			//bool young = false;

			if ( newChar is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) newChar;

				pm.Profession = args.Profession;

				//if ( pm.AccessLevel == AccessLevel.Player && ((Account)pm.Account).Young )
					//young = pm.Young = true;
			}

			SetName( newChar, args.Name );

			AddBackpack( newChar );

			SetStats( newChar, args.Str, args.Dex, args.Int );
			SetSkills( newChar, args.Skills, args.Profession );

			Race race = newChar.Race;

			if( race.ValidateHair( newChar, args.HairID ) )
			{
				newChar.HairItemID = args.HairID;
				newChar.HairHue = race.ClipHairHue( args.HairHue & 0x3FFF );
			}

			if( race.ValidateFacialHair( newChar, args.BeardID ) )
			{
				newChar.FacialHairItemID = args.BeardID;
				newChar.FacialHairHue = race.ClipHairHue( args.BeardHue & 0x3FFF );
			}

			AddShirt( newChar, args.ShirtHue );
			AddPants( newChar, args.PantsHue );
			AddShoes( newChar );

			if( newChar.BankBox != null )
			{
				BankBox bankbox = newChar.FindItemOnLayer( Layer.Bank ) as BankBox;
				bankbox.Delete();
			}

			PlayerBankBox pbb = new PlayerBankBox( newChar );
			newChar.AddItem( pbb );
			newChar.Blessed = true;

			CityInfo city = GetStartLocation( args, false );
			//CityInfo city = new CityInfo( "Britain", "Sweet Dreams Inn", 1496, 1628, 10, Map.Felucca );

			newChar.MoveToWorld( city.Location, city.Map );

			Console.WriteLine( "Login: {0}: New character being created (account={1})", args.State, args.Account.Username );
			Console.WriteLine( " - Character: {0} (serial={1})", newChar.Name, newChar.Serial );
			Console.WriteLine( " - Started: {0} {1} in {2}", city.City, city.Location, city.Map.ToString() );

			new WelcomeTimer( newChar ).Start();
		}

		public static bool VerifyProfession( int profession )
		{
			if ( profession < 0 )
				return false;
			else if ( profession < 4 )
				return true;
			else if ( Core.AOS && profession < 6 )
				return true;
			else if ( Core.SE && profession < 8 )
				return true;
			else
				return false;
		}

		private class BadStartMessage : Timer
		{
			Mobile m_Mobile;
			int m_Message;
			public BadStartMessage( Mobile m, int message ) : base( TimeSpan.FromSeconds ( 3.5 ) )
			{
				m_Mobile = m;
				m_Message = message;
				this.Start();
			}

			protected override void OnTick()
			{
				m_Mobile.SendLocalizedMessage( m_Message );
			}
		}
		private static CityInfo GetStartLocation( CharacterCreatedEventArgs args, bool isYoung )
		{
			bool useHaven = isYoung;

			int flags = args.State == null ? 0 : args.State.Flags;
			Mobile m = args.Mobile;

			return new CityInfo( "Creation", "Creation Chamber", 5449, 1238, -95, Map.Felucca );

		}

		private static void FixStats( ref int str, ref int dex, ref int intel )
		{
			int vStr = str - 10;
			int vDex = dex - 10;
			int vInt = intel - 10;

			if ( vStr < 0 )
				vStr = 0;

			if ( vDex < 0 )
				vDex = 0;

			if ( vInt < 0 )
				vInt = 0;

			int total = vStr + vDex + vInt;

			if ( total == 0 || total == 50 )
				return;

			double scalar = 50 / (double)total;

			vStr = (int)(vStr * scalar);
			vDex = (int)(vDex * scalar);
			vInt = (int)(vInt * scalar);

			FixStat( ref vStr, (vStr + vDex + vInt) - 50 );
			FixStat( ref vDex, (vStr + vDex + vInt) - 50 );
			FixStat( ref vInt, (vStr + vDex + vInt) - 50 );

			str = vStr + 10;
			dex = vDex + 10;
			intel = vInt + 10;
		}

		private static void FixStat( ref int stat, int diff )
		{
			stat += diff;

			if ( stat < 0 )
				stat = 0;
			else if ( stat > 50 )
				stat = 50;
		}

		private static void SetStats( Mobile m, int str, int dex, int intel )
		{
			FixStats( ref str, ref dex, ref intel );

			if ( str < 10 || str > 60 || dex < 10 || dex > 60 || intel < 10 || intel > 60 || (str + dex + intel) != 80 )
			{
				str = 10;
				dex = 10;
				intel = 10;
			}

			m.InitStats( str, dex, intel );
		}

		private static void SetName( Mobile m, string name )
		{
			name = name.Trim();

			if ( !NameVerification.Validate( name, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote ) )
				name = "Generic Player";

			m.Name = name;
		}

		private static bool ValidSkills( SkillNameValue[] skills )
		{
			int total = 0;

			for ( int i = 0; i < skills.Length; ++i )
			{
				if ( skills[i].Value < 0 || skills[i].Value > 50 )
					return false;

				total += skills[i].Value;

				for ( int j = i + 1; j < skills.Length; ++j )
				{
					if ( skills[j].Value > 0 && skills[j].Name == skills[i].Name )
						return false;
				}
			}

			return ( total == 100 );
		}

		private static Mobile m_Mobile;

		private static void SetSkills( Mobile m, SkillNameValue[] skills, int prof )
		{
			switch ( prof )
			{
				case 1: // Warrior
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Anatomy, 30 ),
							new SkillNameValue( SkillName.Healing, 45 ),
							new SkillNameValue( SkillName.Swords, 35 ),
							new SkillNameValue( SkillName.Tactics, 50 )
						};

					break;
				}
				case 2: // Magician
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Invocation, 30 ),
							new SkillNameValue( SkillName.UnarmedFighting, 30 ),
							new SkillNameValue( SkillName.Magery, 50 ),
							new SkillNameValue( SkillName.Meditation, 50 )
						};

					break;
				}
				case 3: // Blacksmith
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Mining, 30 ),
							new SkillNameValue( SkillName.Craftsmanship, 30 ),
							new SkillNameValue( SkillName.Blacksmith, 50 ),
							new SkillNameValue( SkillName.Tinkering, 50 )
						};

					break;
				}
				case 4: // Necromancer
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Polearms, 50 ),
							new SkillNameValue( SkillName.Concentration, 30 ),
							new SkillNameValue( SkillName.Linguistics, 30 ),
							new SkillNameValue( SkillName.Swords, 30 ),
							new SkillNameValue( SkillName.Tactics, 20 )
						};

					break;
				}
				case 5: // Paladin
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Faith, 51 ),
							new SkillNameValue( SkillName.Swords, 49 ),
							new SkillNameValue( SkillName.Concentration, 30 ),
							new SkillNameValue( SkillName.Tactics, 30 )
						};

					break;
				}
				case 6:	//Samurai
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Leadership, 50 ),
							new SkillNameValue( SkillName.Swords, 50 ),
							new SkillNameValue( SkillName.Anatomy, 30 ),
							new SkillNameValue( SkillName.Healing, 30 )
					};
					break;
				}
				case 7:	//Ninja
				{
					skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.ExoticWeaponry, 50 ),
							new SkillNameValue( SkillName.Hiding, 50 ),
							new SkillNameValue( SkillName.Fencing, 30 ),
							new SkillNameValue( SkillName.Stealth, 30 )
						};
					break;
				}
				default:
				{
					if ( !ValidSkills( skills ) )
						return;

					break;
				}
			}
		}

		private static void EquipItem( Item item )
		{
			EquipItem( item, false );
		}

		private static void EquipItem( Item item, bool mustEquip )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Newbied;

			if ( m_Mobile != null && m_Mobile.EquipItem( item ) )
				return;

			Container pack = m_Mobile.Backpack;

			if ( !mustEquip && pack != null )
				pack.DropItem( item );
			else
				item.Delete();
		}

		private static void PackItem( Item item )
		{
			if ( !Core.AOS )
				item.LootType = LootType.Newbied;

			Container pack = m_Mobile.Backpack;

			if ( pack != null )
				pack.DropItem( item );
			else
				item.Delete();
		}

		private static void PackInstrument()
		{

		}

		private static void PackScroll( int circle )
		{

		}

		private static Item NecroHue( Item item )
		{
			item.Hue = 0x2C3;

			return item;
		}

		private static void AddSkillItems( SkillName skill, Mobile m )
		{
		}
	}
}
