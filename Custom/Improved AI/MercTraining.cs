using System; 
using Server.Misc;
using Server.Mobiles;
using Server.Commands;
using Server.Prompts;
using Server.Targeting;

namespace Server.Misc.ImprovedAI 
{
	public class MercTraining
	{ 
		public static void SetFeat( Mobile mob, int index, int newvalue )
		{
			IKhaerosMobile m = mob as IKhaerosMobile;
			
			switch( index )
			{
				case 1: m.Feats.SetFeatLevel(FeatList.BruteStrength, newvalue); break;
				case 2: m.Feats.SetFeatLevel(FeatList.QuickReflexes, newvalue); break;
				case 3: m.Feats.SetFeatLevel(FeatList.Cleave, newvalue); break;
				case 4: m.Feats.SetFeatLevel(FeatList.DamageIgnore, newvalue); break;
				case 5: m.Feats.SetFeatLevel(FeatList.FastHealing, newvalue); break;
				case 6: m.Feats.SetFeatLevel(FeatList.CriticalStrike, newvalue); break;
				case 7: m.Feats.SetFeatLevel(FeatList.SavageStrike, newvalue); break;
				case 8: m.Feats.SetFeatLevel(FeatList.CripplingBlow, newvalue); break;
				case 9: m.Feats.SetFeatLevel(FeatList.FlurryOfBlows, newvalue); break;
				case 10: m.Feats.SetFeatLevel(FeatList.FocusedAttack, newvalue); break;
				case 11: m.Feats.SetFeatLevel(FeatList.DefensiveStance, newvalue); break;
				case 12: m.Feats.SetFeatLevel(FeatList.FlashyAttack, newvalue); break;
				case 13: m.Feats.SetFeatLevel(FeatList.ShieldMastery, newvalue); break;
				case 14: m.Feats.SetFeatLevel(FeatList.GreatweaponFighting, newvalue); break;
				case 15: m.Feats.SetFeatLevel(FeatList.BowMastery, newvalue); break;
				case 16: m.Feats.SetFeatLevel(FeatList.CrossbowMastery, newvalue); break;
				case 17: m.Feats.SetFeatLevel(FeatList.SwiftShot, newvalue); break;
				case 18: m.Feats.SetFeatLevel(FeatList.FocusedShot, newvalue); break;
				case 19: m.Feats.SetFeatLevel(FeatList.CriticalShot, newvalue); break;
				case 20: m.Feats.SetFeatLevel(FeatList.CripplingShot, newvalue); break;
			}
		}
		
		public static int GetFeat( Mobile mob, int index )
		{
			IKhaerosMobile m = mob as IKhaerosMobile;
			
			switch( index )
			{
				case 1: return m.Feats.GetFeatLevel(FeatList.BruteStrength);
				case 2: return m.Feats.GetFeatLevel(FeatList.QuickReflexes);
				case 3: return m.Feats.GetFeatLevel(FeatList.Cleave);
				case 4: return m.Feats.GetFeatLevel(FeatList.DamageIgnore);
				case 5: return m.Feats.GetFeatLevel(FeatList.FastHealing);
				case 6: return m.Feats.GetFeatLevel(FeatList.CriticalStrike);
				case 7: return m.Feats.GetFeatLevel(FeatList.SavageStrike);
				case 8: return m.Feats.GetFeatLevel(FeatList.CripplingBlow);
				case 9: return m.Feats.GetFeatLevel(FeatList.FlurryOfBlows);
				case 10: return m.Feats.GetFeatLevel(FeatList.FocusedAttack);
				case 11: return m.Feats.GetFeatLevel(FeatList.DefensiveStance);
				case 12: return m.Feats.GetFeatLevel(FeatList.FlashyAttack);
				case 13: return m.Feats.GetFeatLevel(FeatList.ShieldMastery);
				case 14: return m.Feats.GetFeatLevel(FeatList.GreatweaponFighting);
				case 15: return m.Feats.GetFeatLevel(FeatList.BowMastery);
				case 16: return m.Feats.GetFeatLevel(FeatList.CrossbowMastery);
				case 17: return m.Feats.GetFeatLevel(FeatList.SwiftShot);
				case 18: return m.Feats.GetFeatLevel(FeatList.FocusedShot);
				case 19: return m.Feats.GetFeatLevel(FeatList.CriticalShot);
				case 20: return m.Feats.GetFeatLevel(FeatList.CripplingShot);
			}
			
			return 0;
		}
		
		public static string GetMercFeatName( int index )
		{
			switch( index )
			{
				case 1: return "Brute Strength";
				case 2: return "Quick Reflexes";
				case 3: return "Cleave";
				case 4: return "Damage Ignore";
				case 5: return "Fast Healing";
				case 6: return "Critical Strike";
				case 7: return "Savage Strike";
				case 8: return "Crippling Blow";
				case 9: return "Flurry of Blows";
				case 10: return "Focused Attack";
				case 11: return "Defensive Stance";
				case 12: return "Flashy Attack";
				case 13: return "Shield Mastery";
				case 14: return "Greatweapon Fighting";
				case 15: return "Bow Mastery";
				case 16: return "Crossbow Mastery";
				case 17: return "Swift Shot";
				case 18: return "Focused Shot";
				case 19: return "Critical Shot";
				case 20: return "Crippling Shot";
			}
			
			return "Invalid";
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "MercTraining", AccessLevel.Player, new CommandEventHandler( MercTraining_OnCommand ) );
		}
		
		[Usage( "MercTraining" )]
        [Description( "Allows you to teach your mercenary some of the feats you know." )]
        private static void MercTraining_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your mercenaries do you wish to train?" );
        	m.Target = new MercTrainingTarget();
        }
        
        private class MercTrainingTarget : Target
        {
            public MercTrainingTarget()
                : base( 2, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	Mercenary merc = obj as Mercenary;
            	PlayerMobile pm = m as PlayerMobile;
            	
            	if( obj is Mercenary && merc.ControlMaster == m )
            	{
            		int minlevel = merc.TrainingReceived * 5;
            		
            		if( (pm.Feats.GetFeatLevel(FeatList.MercTraining) * 2) > merc.TrainingReceived )
            		{
            			if( merc.Level >= minlevel )
            				m.Prompt = new MercTrainingPrompt( m, merc );
            			
            			else
            				m.SendMessage( "That mercenary needs to get to level {0} before {1} can learn a new feat.", 
            				              minlevel.ToString(), merc.GetPersonalPronoun() );
            		}
            		
            		else
            			m.SendMessage( "You have trained that mercenary as much as you can." );
            		
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        private class MercTrainingPrompt : Prompt
		{
			private Mercenary merc;
	
			public MercTrainingPrompt( Mobile from, Mercenary mercenary )
			{
				merc = mercenary;
				PlayerMobile m = from as PlayerMobile;
				
				from.SendMessage( "Please type the code for the feat you wish to teach to this mercenary." );
				from.SendMessage( 60, "Code - Feat Name - Feat Level" );
				
				for( int i = 1; i < 21; i++ )
				{
					if( MercTraining.GetFeat( m, i ) > 0 && MercTraining.GetFeat( merc, i ) < 1 )
						from.SendMessage( 5 * i, "{0} - {1}", i.ToString(), MercTraining.GetMercFeatName(i) );
				}
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				if( !(text != null && merc != null && from != null && !merc.Deleted && !from.Deleted && merc.ControlMaster == from ) )
					return;
				
				int index = 0;
				
				if( int.TryParse(text, out index) )
				{
					if( index > 20 || index < 1 )
					{
						from.SendMessage( "Invalid code." );
						return;
					}

					if( MercTraining.GetFeat( merc, index ) > 2 )
						from.SendMessage( "The chosen feat ({0}) is already at level 3.", MercTraining.GetMercFeatName(index) );
					
					else if( MercTraining.GetFeat( from, index ) < 1 )
						from.SendMessage( "You do not possess the chosen feat ({0}).", MercTraining.GetMercFeatName(index) );
					
					else
					{
						MercTraining.SetFeat( merc, index, 3 );
						merc.TrainingReceived++;
						from.SendMessage( "The chosen feat ({0}) has been set to level 3.", MercTraining.GetMercFeatName(index) );
					}
				}
				
				else
					from.SendMessage( "Invalid code." );
			}
        }
	} 
}
