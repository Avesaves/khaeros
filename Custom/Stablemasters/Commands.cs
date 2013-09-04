using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.Targeting;
using Server.Prompts;

namespace Server.Misc.BreedingSystem
{
	public class Commands
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Mate", AccessLevel.Player, new CommandEventHandler( Mate_OnCommand ) );
			CommandSystem.Register( "RetrainPet", AccessLevel.Player, new CommandEventHandler( RetrainPet_OnCommand ) );
			CommandSystem.Register( "Geld", AccessLevel.Player, new CommandEventHandler( Geld_OnCommand ) );
			CommandSystem.Register( "Brand", AccessLevel.Player, new CommandEventHandler( Brand_OnCommand ) );
		}
		
		[Usage( "Mate" )]
        [Description( "Allows you to have two animals mate." )]
        private static void Mate_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new MateTarget( null );
        }
        
        private class MateTarget : Target
		{
        	private BaseBreedableCreature creat;
        	
			public MateTarget( BaseBreedableCreature bbc ) : base( -1, false, TargetFlags.None )
			{
				creat = bbc;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseBreedableCreature )
				{
					if( creat != null && creat.Breed == ((BaseBreedableCreature)targeted).Breed )
						Utilities.TryToMate( (PlayerMobile)from, creat, (BaseBreedableCreature)targeted );
					
					else if( creat == null )
						from.Target = new MateTarget( (BaseBreedableCreature)targeted );
				}
			}
		}
        
        [Usage( "RetrainPet" )]
        [Description( "Allows you to retrain some of your pet's feats." )]
        private static void RetrainPet_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your pets do you wish to train?" );
        	m.Target = new RetrainPetTarget();
        }
        
        private class RetrainPetTarget : Target
        {
            public RetrainPetTarget()
                : base( 2, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	BaseBreedableCreature bbc = obj as BaseBreedableCreature;
            	
            	if( obj is BaseBreedableCreature && bbc.CanBeHandledBy(m) )
            	{
            		if( bbc.TrainingRemaining > 0 )
            			m.Prompt = new RetrainPetPrompt( m, bbc, true );
            		
            		else if( bbc.TrainingPoints > 0 )
	            		m.Prompt = new RetrainPetPrompt( m, bbc, false );
            		
            		else
            			m.SendMessage( "That pet has already been trained as much as possible." );
            		
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        [Usage( "Geld" )]
        [Description( "Allows you to geld your animal, to prevent it from mating." )]
        private static void Geld_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.Skills[SkillName.AnimalLore].Base < 100.0 )
        	{
    			m.SendMessage( "You need 100% Animal Lore in order to use this command." );
    			return;
        	}
        	
        	m.SendMessage( "Which of your male pets do you wish to geld?" );
        	m.Target = new GeldTarget();
        }
        
        private class GeldTarget : Target
        {
            public GeldTarget()
                : base( 2, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	BaseBreedableCreature bbc = obj as BaseBreedableCreature;
            	
            	if( obj is BaseBreedableCreature && bbc.CanBeHandledBy(m) )
            	{
            		if( bbc.Gelt )
            			m.SendMessage( "That pet has already been gelt." );
            		
            		else if( !bbc.Female )
            		{
            			bbc.Gelt = true;
            			m.SendMessage( "Your pet has been gelt." );
            			bbc.InvalidateProperties();
            		}
            		
            		else
            			m.SendMessage( "That is not a male pet." );
            		
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        [Usage( "Brand" )]
        [Description( "Allows you to brand an animal." )]
        private static void Brand_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.Skills[SkillName.AnimalTaming].Base < 100.0 )
        	{
    			m.SendMessage( "You need 100% Animal Taming in order to use this command." );
    			return;
        	}
        	
        	if( e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        	{
        		m.SendMessage( "Correct usage example: .Brand \"Northern Royal Stables\"." );
        		m.SendMessage( "Reminder: make sure to use \"s for composite names, such as the one on the example above." );
        		return;
        	}
        	
        	m.SendMessage( "Which of your pets do you wish to brand?" );
        	m.Target = new BrandTarget( e.Arguments[0] );
        }
        
        private class BrandTarget : Target
        {
        	string m_Brand;
        	
            public BrandTarget( string brand )
                : base( 2, false, TargetFlags.None )
            {
            	m_Brand = brand;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	BaseBreedableCreature bbc = obj as BaseBreedableCreature;
            	
            	if( obj is BaseBreedableCreature && bbc.CanBeHandledBy(m) )
            	{
            		if( bbc.Brand != null )
            			m.SendMessage( "That pet has already been branded." );
            		
            		else
            		{
            			bbc.Brand = m_Brand;
            			m.SendMessage( "You have branded your pet." );
            			bbc.InvalidateProperties();
            		}
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        private class RetrainPetPrompt : Prompt
		{
			private BaseBreedableCreature bbc;
			private bool Add;
	
			public RetrainPetPrompt( Mobile from, BaseBreedableCreature creat, bool Remove )
			{
				bbc = creat;
				Add = !Remove;
				
				if( Remove )
					from.SendMessage( "Please type the code for the feat you wish to REMOVE from this pet." );
				
				else
					from.SendMessage( "This pet has currently {0} unassigned feat points. Please type the code for the feat you wish to INCREASE by one.", 
					                 bbc.TrainingPoints.ToString() );
				
				from.SendMessage( 60, "Code - Feat Name - Feat Level" );
				
				for( int i = 1; i < 16; i++ )
				{
					if( (Remove && bbc.GetFeat(i) > 0) || (Add && bbc.GetFeat(i) < 3 ) )
						from.SendMessage( 5 * i, "{0} - {1} - {2}", i.ToString(), Utilities.GetBBCFeatName(i), bbc.GetFeat(i) );
				}
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				if( !(text != null && bbc != null && from != null && !bbc.Deleted && !from.Deleted && bbc.CanBeHandledBy(from)) )
					return;
				
				int index = 0;
				
				if( int.TryParse(text, out index) )
				{
					if( index > 15 || index < 1 )
					{
						from.SendMessage( "Invalid code." );
						return;
					}
				
					if( Add )
					{
						if( bbc.GetFeat(index) > 2 )
							from.SendMessage( "The chosen feat ({0}) is already at level 3.", Utilities.GetBBCFeatName(index) );
						
						else
						{
							bbc.IncFeat(index);
							bbc.TrainingPoints--;
							from.SendMessage( "The chosen feat ({0}) has been raised by 1 and is now at level {1}.", 
							                 Utilities.GetBBCFeatName(index), bbc.GetFeat(index).ToString() );
						}
					}
					
					else
					{
						if( bbc.GetFeat(index) < 1 )
							from.SendMessage( "The chosen feat ({0}) is already at level 0.", Utilities.GetBBCFeatName(index) );
						
						else
						{
							bbc.TrainingPoints += bbc.GetFeat(index);
							bbc.SetFeat(index, 0);
							bbc.TrainingRemaining--;
							from.SendMessage( "The chosen feat ({0}) has been set to 0 and this pet now has {1} unassigned feat points.", 
							                 Utilities.GetBBCFeatName(index), bbc.TrainingPoints.ToString() );
						}
					}
				}
				
				else
					from.SendMessage( "Invalid code." );
			}
        }
	}
}
