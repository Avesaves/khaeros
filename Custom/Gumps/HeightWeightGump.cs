using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
    public class HeightWeightGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register( "HeightWeight", AccessLevel.Player, new CommandEventHandler(HeightWeight_OnCommand) );
        }

        [Usage("HeightWeight")]
        [Description( "Opens the Height & Weight gump." )]
        public static void HeightWeight_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !(e.Mobile is PlayerMobile) )
        		return;
        			
            PlayerMobile from = e.Mobile as PlayerMobile;
            from.SendGump( new HeightWeightGump(from, (from.Forging || from.Reforging), Convert.ToDouble(from.Height), Convert.ToDouble(from.Weight)) );
        }

        private bool m_Editing;
        
        public HeightWeightGump( PlayerMobile from, bool editing, double height, double weight ) : base( 0, 0 )
        {
        	height = Math.Max( height, 30 );
        	weight = Math.Max( weight, 30 );
        	m_Editing = editing;
            this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			
			from.CloseGump( typeof(HeightWeightGump) );
			
			double heightInCm = PetraeToCm( height );
			double heightInM = CmToMeter( heightInCm );
			string heightInFt = PetraeToFt( height ).ToString() + "' " + PetraeToInches( height, PetraeToFt(height) ).ToString() + "\"";
			
			double weightInKg = PetraeToKg( weight );
			double weightInLb = PetraeToLb( weight );
			
			double minNH = GetMinNationalHeight( from.Nation );
			double avgNH = GetAverageNationalHeight( from.Nation );
			double maxNH = GetMaxNationalHeight( from.Nation );
			
			double minNW = GetMinMaleWeight( CmToMeter(PetraeToCm(minNH)) );
			double avgNW = GetAvgMaleWeight( CmToMeter(PetraeToCm(avgNH)) );
			double maxNW = GetMaxMaleWeight( CmToMeter(PetraeToCm(maxNH)) );
			
			double minH = GetMinMaleHeight( PetraeToKg(weight) );
			double avgH = GetAvgMaleHeight( PetraeToKg(weight) );
			double maxH = GetMaxMaleHeight( PetraeToKg(weight) );
			
			double minW = GetMinMaleWeight( CmToMeter(PetraeToCm(height)) );
			double avgW = GetAvgMaleWeight( CmToMeter(PetraeToCm(height)) );
			double maxW = GetMaxMaleWeight( CmToMeter(PetraeToCm(height)) );
			
			if( from.Female )
			{
				minNW = GetMinFemaleWeight( CmToMeter(PetraeToCm(minNH)) );
				avgNW = GetAvgFemaleWeight( CmToMeter(PetraeToCm(avgNH)) );
				maxNW = GetMaxFemaleWeight( CmToMeter(PetraeToCm(maxNH)) );
				
				minH = GetMinFemaleHeight( PetraeToKg(weight) );
				avgH = GetAvgFemaleHeight( PetraeToKg(weight) );
				maxH = GetMaxFemaleHeight( PetraeToKg(weight) );
				
				minW = GetMinFemaleWeight( CmToMeter(PetraeToCm(height)) );
				avgW = GetAvgFemaleWeight( CmToMeter(PetraeToCm(height)) );
				maxW = GetMaxFemaleWeight( CmToMeter(PetraeToCm(height)) );
			}

			int x1 = 305;
			int x2 = 525;
			
			AddPage( 0 );
			AddBackground( 260, 147, 502, 312, 9270 );
			AddBackground( 277, 163, 469, 279, 3500 );
			AddLabel( 455, 175, 2010, @"Height & Weight" );
			AddImageTiled( 379, 200, 264, 3, 96 );
			AddLabel( x1, 245, 0, @"This Height's Min Weight: " + Convert.ToInt32(KgToPetrae(minW)).ToString() );
			AddLabel( x2, 245, 0, @"This Weight's Min Height: " + Convert.ToInt32(MeterToPetrae(minH)).ToString() );
			AddLabel( x1, 275, 0, @"This Height's Average Weight: " + Convert.ToInt32(KgToPetrae(avgW)).ToString() );
			AddLabel( x2, 275, 0, @"This Weight's Average Height: " + Convert.ToInt32(MeterToPetrae(avgH)).ToString() );
			AddLabel( x1, 305, 0, @"This Height's Max Weight: " + Convert.ToInt32(KgToPetrae(maxW)).ToString() );
			AddLabel( x2, 305, 0, @"This Weight's Max Height: " + Convert.ToInt32(MeterToPetrae(maxH)).ToString() );
			AddLabel( x1, 335, 0, @"Racial Min/Avg/Max: " + minNH.ToString() + "/" + avgNH.ToString() + "/" + maxNH.ToString() );
			AddLabel( x2, 335, 0, @"Racial Min/Avg/Max: " + Convert.ToInt32(KgToPetrae(minNW)).ToString() + "/" + 
			         Convert.ToInt32(KgToPetrae(avgNW)).ToString() + "/" + Convert.ToInt32(KgToPetrae(maxNW)).ToString() );
			AddLabel( x1, 365, 0, @"Height (cm): " + Convert.ToInt32(heightInCm).ToString() );
			AddLabel( x2, 365, 0, @"Weight (kg): " + Convert.ToInt32(weightInKg).ToString() );
			AddLabel( x1, 395, 0, @"Height (ft): " + heightInFt );
			AddLabel( x2, 395, 0, @"Weight (lb): " + Convert.ToInt32(weightInLb).ToString() );
			
			if( editing )
			{
				AddLabel( x1, 215, 0, @"Height (Petrae): " );
				AddLabel( x2, 215, 0, @"Weight (Petrae): " );
				AddTextEntry( 410, 215, 40, 20, 0, 1, @"" + height.ToString());
				AddTextEntry( 634, 215, 40, 20, 0, 2, @"" + weight.ToString() );
				AddButton(666, 176, 1153, 1155, 1, GumpButtonType.Reply, 0);
			}
			
			else
			{
				AddLabel( x1, 215, 0, @"Height (Petrae): " + height.ToString() );
				AddLabel( x2, 215, 0, @"Weight (Petrae): " + weight.ToString() );
			}
			
			AddButton(696, 176, 1150, 1152, 0, GumpButtonType.Reply, 0);
        }

        public static int GetAverageNationalHeight( Nation nation )
        {
        	if( nation == Nation.Western )
        		return 90;
        	if( nation == Nation.Haluaroc )
        		return 93;
        	if( nation == Nation.Southern )
        		return 103;
        	if( nation == Nation.Tirebladd )
        		return 106;
        	if( nation == Nation.Mhordul )
        		return 109;
        	
        	return 100;
        }
        
        public static double GetMinNationalHeight( Nation nation ){ return (GetAverageNationalHeight(nation) - 9); }
        public static double GetMaxNationalHeight( Nation nation ){ return (GetAverageNationalHeight(nation) + 9); }
        
        public static double CmToMeter( double cm ){ return (double)(cm * 0.01); }
        public static double MeterToCm( double meter ){ return (double)(meter * 100.0); }
        public static double PetraeToCm( double petrae ){ return (double)(petrae * 1.8); }
        public static int PetraeToFt( double petrae ){ return (int)((PetraeToCm(petrae) * 0.393700787) / 12); }
        public static int PetraeToInches( double petrae, int feet ){ return (int)((PetraeToCm(petrae) * 0.393700787) % 12); }
        public static double PetraeToKg( double petrae ){ return (double)(petrae * 0.8); }
        public static double PetraeToLb( double petrae ){ return (double)(PetraeToKg(petrae) * 2.20462262); }
        public static double MeterToPetrae( double meter ){ return (double)(meter / 0.018); }
        public static double KgToPetrae( double kg ){ return (double)(kg / 0.8); }
        
        public static double GetMinMaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 50.0)); }
        public static double GetMinFemaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 45.0)); }
        public static double GetAvgMaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 23.0)); }
        public static double GetAvgFemaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 20.0)); }
        public static double GetMaxMaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 18.0)); }
        public static double GetMaxFemaleHeight( double weightInKg ){ return (double)(Math.Sqrt(weightInKg / 16.0)); }
        
        public static double GetMinMaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 18.0); }
        public static double GetMinFemaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 16.0); }
        public static double GetAvgMaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 23.0); }
        public static double GetAvgFemaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 20.0); }
        public static double GetMaxMaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 50.0); }
        public static double GetMaxFemaleWeight( double heightInM ){ return (double)(Math.Pow(heightInM, 2.0) * 45.0); }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
        	if( !m_Editing || sender.Mobile == null || sender.Mobile.Deleted || !(sender.Mobile is PlayerMobile) )
        		return;
        	
            PlayerMobile from = sender.Mobile as PlayerMobile;
            
            if( info.ButtonID > 0 )
			{
				string height = info.GetTextEntry( 1 ).Text;
				string weight = info.GetTextEntry( 2 ).Text;
				
				int h = 0;
				int w = 0;
				
				if( String.IsNullOrEmpty(height) || String.IsNullOrEmpty(weight) )
					from.SendMessage( "Height and Weight cannot be blank." );
				
				else if( !int.TryParse(height, out h) || !int.TryParse(weight, out w) )
					from.SendMessage( "Please enter only numbers for Height and Weight." );
				
				else
				{
					if( h > GetMaxNationalHeight(from.Nation) )
						from.SendMessage( "Your height cannot be greater than your race's max height." );
					
					else if( h < GetMinNationalHeight(from.Nation) )
						from.SendMessage( "Your height cannot be lower your race's min height." );
					
					else
					{
						double maxW = GetMaxMaleWeight( CmToMeter(PetraeToCm(from.Height)) );
						double minW = GetMinMaleWeight( CmToMeter(PetraeToCm(from.Height)) );
						
						double wInKg = PetraeToKg( from.Weight );
						
						if( from.Female )
						{
							maxW = GetMaxFemaleWeight( CmToMeter(PetraeToCm(from.Height)) );
							minW = GetMinFemaleWeight( CmToMeter(PetraeToCm(from.Height)) );
						}
						
						if( wInKg > maxW )
							from.SendMessage( "Your weight cannot be greater than your height's max weight." );
						
						else if( wInKg < minW )
							from.SendMessage( "Your weight cannot be lower your height's min weight." );
						
						else
						{
							from.Height = h;
							from.Weight = w;
							from.SendMessage( "Height and Weight successfully changed." );
							
							if( from.HasGump( typeof( CharInfoGump ) ) )
								from.SendGump( new CharInfoGump(from) );
						}
						
						from.SendGump( new HeightWeightGump(from, m_Editing, Convert.ToDouble(h), Convert.ToDouble(w)) );
						return;
					}
				}
				
				from.SendGump( new HeightWeightGump(from, true, Convert.ToDouble(from.Height), Convert.ToDouble(from.Weight)) );
            }
        }
    }
}
