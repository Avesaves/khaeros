using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Commands;
using Server.Gumps;

namespace Server.Items
{
	public class FinishCreationPortal : BaseTrap
	{
		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 0; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.Zero; } }

		[Constructable]
		public FinishCreationPortal() : base( 0xF6C )
		{
            Name = "Finish Character Creation";
            Movable = false;
		}
		
		public override void OnTrigger( Mobile m )
		{
			bool finished = true;
			PlayerMobile from = m as PlayerMobile;
			
			if( m is PlayerMobile )
			{
	        	if( from.Nation == Nation.None )
				{
					from.SendMessage( 60, "You must choose a race first." );
					finished = false;
				}
				
				if( from.Hue == 0 )
				{
					from.SendMessage( 60, "You must customize your character's appearance first." );
					finished = false;
				}
				
				if( ( from.HairItemID > 0 && from.HairHue < 1 ) || ( from.FacialHairItemID > 0 && from.FacialHairHue < 1 ) )
				{
					from.SendMessage( 60, "Please pick a hair hue." );
					finished = false;
				}
				
				if( (from.Forging || from.OldMapChar) && !from.Account.AcceptedNames.Contains(from.Name) )
				{
					from.SendMessage( 60, "Your character's name has not yet been accepted by an Overseer. Please make sure that you have applied for this character and that your application has been accepted. Contact an Overseer if you need help." );
					finished = false;
				}
				
				if( finished )
				{
					if( from.Account.AcceptedNames.Contains(from.Name) )
						from.Account.AcceptedNames.Remove(from.Name);
					
					from.SendMessage( 60, "Character creation complete." );
					from.SendMessage( 70, "Please remember to use .look on yourself and write a profile." );
					from.SendMessage( 80, "Enjoy your stay in Khaeros!" );
					from.CloseGump( typeof(RaceGump) );
					from.CloseGump( typeof(InitialStatsGump) );
					from.CloseGump( typeof(HeightWeightGump) );
					from.CloseGump( typeof(ChosenDeityGump) );
					from.CloseGump( typeof(UniversalFeatsGump) );
					from.CloseGump( typeof(CharCustomGump) );
					from.CloseGump( typeof(BackgroundsGump) );
					
					if( !from.Reforging )
					{
						LevelSystemCommands.RemoveEquippedItems( from );
						BaseKhaerosMobile.RandomPoorClothes( from, from.Nation );
						
						if( from.Nation == Nation.Western )
							m.Location = new Point3D( 5352, 487, 0 );
						
						else if( from.Nation == Nation.Southern )
							m.Location = new Point3D( 5352, 479, 0 );
						
						else if( from.Nation == Nation.Haluaroc )
							m.Location = new Point3D( 5363, 487, 0 );
						
						else if( from.Nation == Nation.Mhordul )
							m.Location = new Point3D( 5363, 480, 0 );
						
						else if( from.Nation == Nation.Tirebladd )
							m.Location = new Point3D( 5362, 471, 0 );
						
						else if( from.Nation == Nation.Northern )
							m.Location = new Point3D( 5352, 468, 0 );
						
						from.MaxAge = Utility.RandomMinMax( 0, 10 ) + GetMaxAge( from ) - Utility.RandomMinMax( 0, 10 );
						from.NextBirthday = DateTime.Now + TimeSpan.FromDays( 90 );
						from.NextAllowance = DateTime.Now + TimeSpan.FromHours( 80 );
						from.DayOfBirth = Convert.ToString( TimeSystem.Data.Day );
						from.MonthOfBirth = Convert.ToString( TimeSystem.Data.Month );
						from.YearOfBirth = Convert.ToString( ( TimeSystem.Data.Year - from.Age ) );
					}
					
					else
					{
						if( from.OldMapChar )
						{
							from.Location = new Point3D( 2388, 1507, 0 );
							from.Map = Map.Felucca;
							from.LogPetsIn();
							from.MaxAge = Utility.RandomMinMax( 0, 10 ) + GetMaxAge( from ) - Utility.RandomMinMax( 0, 10 );
							from.NextBirthday = DateTime.Now + TimeSpan.FromDays( 90 );
							from.DayOfBirth = Convert.ToString( TimeSystem.Data.Day );
							from.MonthOfBirth = Convert.ToString( TimeSystem.Data.Month );
							from.YearOfBirth = Convert.ToString( ( TimeSystem.Data.Year - from.Age ) );
						}
						
						else
						{
							from.Location = from.ReforgeLocation;
							from.Map = from.ReforgeMap;
						}
					}
					
					from.Blessed = false;
					from.Forging = false;
					from.Reforging = false;
					from.OldMapChar = false;
					from.RecreateCP = 0;
					from.RecreateXP = 0;
					from.DisplayGuildTitle = true;
				}
			}
		}
		
		public static int GetMaxAge( PlayerMobile from )
		{
			int racialmaxage = 100;
			
			if( from.Nation == Nation.Western )
				racialmaxage -= 25;
			
			if( from.Nation == Nation.Haluaroc )
				racialmaxage -= 10;
			
			if( from.Nation == Nation.Mhordul )
				racialmaxage -= 40;
			
			if( from.Nation == Nation.Tirebladd )
				racialmaxage -= 30;
			
			if( from.Nation == Nation.Northern )
				racialmaxage -= 20;
			
			return racialmaxage;
		}

		public FinishCreationPortal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
