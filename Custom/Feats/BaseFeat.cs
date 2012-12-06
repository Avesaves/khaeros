using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Items;

namespace Server.FeatInfo
{
	public enum FeatCost
	{
		None,
		Low,
		Medium,
		High
	}
	
	public abstract class BaseFeat : object
	{
		private int m_Level;
		public int Level{ get{ return m_Level; } set{ m_Level = value; } }
		public abstract FeatCost CostLevel{ get; }
		public virtual int BaseCost{ get{ return (CostLevel == FeatCost.Low ? 500 : (CostLevel == FeatCost.Medium ? 1000 : 2000)); } }
		public virtual int CostToRaise{ get{ return ((Level + 1) * BaseCost); } }
		public static bool OldPages = false;
		public static string WikiFolder = Server.Misc.StatusPage.LiveServer == true ? @"\Wiki\data\pages\skills\" : @"\Feats\";
		public abstract string Name{ get; }
		public abstract FeatList ListName{ get; }
		public virtual string Cost1{ get{ return CostLevel == FeatCost.None ? "0" : BaseCost.ToString(); } }
		public virtual string Cost2{ get{ return CostLevel == FeatCost.None ? "0" : (2 * BaseCost).ToString(); } }
		public virtual string Cost3{ get{ return CostLevel == FeatCost.None ? "0" : (3 * BaseCost).ToString(); } }
		public virtual double SkillLevel
		{ 
			get
			{
				if( Level == 3 )
					return 100.0;
				if( Level == 2 )
					return 50.0;
				if( Level == 1 )
					return 20.0;
				
				return 0.0;
			}
		}
		
		public abstract SkillName[] AssociatedSkills{ get; }
		public abstract FeatList[] AssociatedFeats{ get; }
		
		public abstract FeatList[] Requires{ get; }
		public abstract FeatList[] Allows{ get; }
		
		public abstract string FirstDescription{ get; }
		public abstract string SecondDescription{ get; }
		public abstract string ThirdDescription{ get; }
		
		public abstract string FirstCommand{ get; }
		public abstract string SecondCommand{ get; }
		public abstract string ThirdCommand{ get; }
		
		public abstract string FullDescription{ get; }
		
		public virtual bool IsRacialFeat{ get{ return (AllowedNations.Length > 0); } }
		public virtual Nation[] AllowedNations{ get{ return new Nation[]{ }; } }

        public virtual bool IgnoreThisFeatWhenRemovingParent( PlayerMobile m )
        {
            return false;
        }
		
		public static string GetRacialRequirements( BaseFeat skill )
		{
			string requirements = "All races";
			
			if( skill.IsRacialFeat )
			{
				for( int i = 0; i < skill.AllowedNations.Length; i++ )
				{
					if( requirements == "All races" )
						requirements = skill.AllowedNations[i].ToString();
					
					else
						requirements = requirements + "<br>" + skill.AllowedNations[i].ToString();
					
					if( skill.AllowedNations[i] != Nation.Mhordul && skill.AllowedNations[i] != Nation.Khemetar )
						requirements = requirements + "s";
				}
			}
			
			return requirements;
		}
		
		public static string GetRequirements( BaseFeat skill )
		{
			return GetRequirements( skill, false );
		}
		
		public static string GetRequirements( BaseFeat skill, bool wiki )
		{
			string requirements = "None";
			
			for( int i = 0; i < skill.Requires.Length; i++ )
			{
				if( requirements == "None" )
					requirements = "";
				
				else
					requirements = requirements + "<br>";
				
				string name = Feats.ConvertFeatListToFeatObject( skill.Requires[i] ).Name;
				
				if( wiki )
					requirements = requirements + "[[guides:skills:" + name.Replace(" ", "_").ToLower() + "|" + name + " Level 3]]";
					
				else
					requirements = requirements + Feats.ConvertFeatListToFeatObject( skill.Requires[i] ).Name + " Level 3";
			}
			
			return requirements;
		}
		
		public static string GetAllowed( BaseFeat skill )
		{
			string allowed = "None";
			
			for( int i = 0; i < skill.Allows.Length; i++ )
			{
				if( allowed == "None" )
					allowed = "";
				
				else
					allowed = allowed + "<br>";
				
				string name = Feats.ConvertFeatListToFeatObject( skill.Allows[i] ).Name;
				allowed = allowed + "[[guides:skills:" + name.Replace(" ", "_").ToLower() + "|" + name + "]]";
			}
			
			return allowed;
		}
		
		public bool MeetsTheRacialRequirement( PlayerMobile m )
		{
			for( int i = 0; i < AllowedNations.Length; i++ )
			{
				if( AllowedNations[i] == m.Nation )
					return true;
			}
			
			return false;
		}
		
		public virtual bool MeetsOurRequirements( PlayerMobile m )
		{
			if( IsRacialFeat && !MeetsTheRacialRequirement(m) )
				return false;
			
			if( Requires.Length < 1 )
				return true;
			
			for( int i = 0; i < Requires.Length; i++ )
			{
				FeatList skill = Requires[i];
				
				if( m.Feats.GetFeatLevel(skill) > 2 )
					return true;
			}
			
			return false;
		}
		
		public bool HasThisFeat( PlayerMobile m )
		{
			return (m.Feats.GetFeatLevel(ListName) > 2);
		}
		
		public virtual void AttemptRemoval( PlayerMobile m, int level, bool freeRemoval )
		{
			if( level != Level )
				m.SendMessage( "Please remove the higher levels of this skill first." );
			
			else if( CanRemoveThisFeat(m, freeRemoval) )
			{
				m.CP += Level * BaseCost;
				m.CPSpent -= Level * BaseCost;
				m.FeatSlots -= Level * BaseCost;
				
				if( m.HasGump( typeof(CharInfoGump) ) )
					m.SendGump( new CharInfoGump(m) );
				
				Level--;
				OnLevelChanged( m );
                OnLevelLowered( m );
				
				for( int i = 0; i < AssociatedFeats.Length; i++ )
					m.Feats.SetFeatLevel( AssociatedFeats[i], Level, m );
	
				for( int i = 0; i < AssociatedSkills.Length; i++ )
					m.Skills[AssociatedSkills[i]].Base = SkillLevel;
			}
		}
		
		public virtual void OnLevelChanged( PlayerMobile owner )
		{
			FixAddOns( owner );
		}

        public virtual void OnLevelLowered( PlayerMobile owner )
        {
        }

        public virtual void OnLevelRaised( PlayerMobile owner )
        {
        }
		
		public virtual void FixAddOns( PlayerMobile owner )
		{
		}
		
		public virtual bool CanBeRemovedFrom( PlayerMobile m )
		{
			return true;
		}

        public virtual bool ShouldDisplayTo( PlayerMobile m )
        {
            return true;
        }
		
		public bool CanRemoveThisFeat( PlayerMobile m, bool freeRemoval )
		{
			if( !CanBeRemovedFrom(m) )
				return false;
			
			for( int i = 0; i < Allows.Length; i++ )
			{
				if( m.Feats.GetFeatLevel(Allows[i]) > 0 && !m.Feats.FeatDictionary[Allows[i]].CanKeepThisFeatWithout(m, this) )
				{
					m.SendMessage( "If you wish to remove this skill, first you will have to remove " + m.Feats.FeatDictionary[Allows[i]].Name + " or " +
					              "acquire another skill that works as a requirement for it, if possible." );
					
					return false;
				}
			}
			
			if( !freeRemoval )
			{
				if( m.Backpack != null && m.Backpack.ConsumeTotal(typeof(Copper), (Level * BaseCost)) )
				   return true;
				   
				else
					m.SendMessage( "In order to remove this skill, you will need to have " + (Level * BaseCost).ToString() + " copper coins in your backpack." );
				
				return false;
			}
			
			return true;
		}
		
		public bool CanKeepThisFeatWithout( PlayerMobile m, BaseFeat toRemove )
		{
            if( IgnoreThisFeatWhenRemovingParent( m ) )
                return true;

			if( CostLevel == FeatCost.None )
			{
				for( int i = 0; i < toRemove.AssociatedFeats.Length; i++ )
				{
					if( toRemove.AssociatedFeats[i] == ListName )
						return true;
				}
			}
			
			for( int i = 0; i < Requires.Length; i++ )
			{
				if( Feats.Catalogue[Requires[i]].ListName != toRemove.ListName && m.Feats.GetFeatLevel(Requires[i]) > 2 )
					return true;
			}
			
			return false;
		}
		
		public virtual void AttemptPurchase( PlayerMobile m, int level, bool freeRemoval )
		{
			if( CostLevel == FeatCost.None )
				m.SendMessage( "This skill cannot be directly purchased." );
			
			if( !MeetsOurRequirements(m) )
				m.SendMessage( "You do not meet the requirements for this skill." );
			
			else if( Level >= level )
				AttemptRemoval( m, level, freeRemoval );
			
			else if( (level - Level) != 1 )
				m.SendMessage( "You cannot acquire a skill level before purchasing all previous levels." );
			
			else if( m.CP < CostToRaise )
				m.SendMessage( "You do not have enough CPs to acquire this skill level." );
			
			else if( LevelSystem.CanSpendCP(m, CostToRaise) )
			{
				m.CP -= CostToRaise;
				m.CPSpent += CostToRaise;
				m.FeatSlots += CostToRaise;
				Level++;
				OnLevelChanged( m );
                OnLevelRaised( m );
				m.SendMessage( "You have purchased " + Name + " Level " + Level.ToString() + "." );
				
				for( int i = 0; i < AssociatedFeats.Length; i++ )
					m.Feats.SetFeatLevel( AssociatedFeats[i], Level, m );
	
				for( int i = 0; i < AssociatedSkills.Length; i++ )
					m.Skills[AssociatedSkills[i]].Base = SkillLevel;
				
				if( m.HasGump( typeof(CharInfoGump) ) )
					m.SendGump( new CharInfoGump(m) );
			}
		}

		public static string GetFullDescription( BaseFeat skill )
		{
			string description = "<b>" + skill.Name + " Level 1</b><br><br>" +
				"<i>Description:</i> " +
				skill.FirstDescription + "<br><br>" +
				"<i>Requires any of the following:</i><br><br>" +
				GetRequirements( skill ) + "<br><br>" +
				"<i>Available to:</i><br><br>" +
				GetRacialRequirements( skill ) + "<br><br>" +
				"<i>Cost:</i> " + skill.Cost1 + " CPs<br><br>" +
				"<i>Command:</i> " + skill.FirstCommand + "<br><br>" +
				"<b>" + skill.Name + " Level 2</b><br><br>" +
				"<i>Description: (Upgrade)</i> " +
				skill.SecondDescription + "<br><br>" +
				"<i>Cost:</i> " + skill.Cost2 + " CPs<br><br>" +
				"<i>Command:</i> " + skill.SecondCommand + "<br><br>" +
				"<b>" + skill.Name + " Level 3</b><br><br>" +
				"<i>Description: (Final)</i> " +
				skill.ThirdDescription + "<br><br>" +
				"<i>Cost:</i> " + skill.Cost3 + " CPs<br><br>" +
				"<i>Command:</i> " + skill.ThirdCommand + "<br><br>";
			
			return description;
		}
		
		public static string GetWebpageDescription( BaseFeat skill ){ return
				"<p id=\"skillName\">" + skill.Name + " Level 1</p>" +
				"<span id=\"boldTopic\">Description:</span> " +
				skill.FirstDescription + "<br><br>" +
				"<span id=\"boldTopic\">Requires any of the following:</span><br><br>" +
				GetRequirements( skill ) + "<br><br>" +
				"<span id=\"boldTopic\">Available to:</span><br><br>" +
				GetRacialRequirements( skill ) + "<br><br>" +
				"<span id=\"boldTopic\">Cost:</span> " + skill.Cost1 + " CPs<br><br>" +
				"<span id=\"boldTopic\">Command:</span> " + skill.FirstCommand + "<br><br>" +
				"<p id=\"skillName\">" + skill.Name + " Level 2</p>" +
				"<span id=\"boldTopic\">Description: (Upgrade)</span> " +
				skill.SecondDescription + "<br><br>" +
				"<span id=\"boldTopic\">Cost:</span> " + skill.Cost2 + " CPs<br><br>" +
				"<span id=\"boldTopic\">Command:</span> " + skill.SecondCommand + "<br><br>" +
				"<p id=\"skillName\">" + skill.Name + " Level 3</p>" +
				"<span id=\"boldTopic\">Description: (Final)</span> " +
				skill.ThirdDescription + "<br><br>" +
				"<span id=\"boldTopic\">Cost:</span> " + skill.Cost3 + " CPs<br><br>" +
				"<span id=\"boldTopic\">Command:</span> " + skill.ThirdCommand + "<br><br>"; }
		
		public static void PublishWikiPage( BaseFeat skill, StreamWriter op )
		{
			HandleString( "====== " + skill.Name + " Level 1 ======", op );
			HandleString( "**Description:** ", op );
			HandleString( skill.FirstDescription + "<br><br>", op );
			HandleString( "**Requires any of the following:**<br><br>", op );
			HandleString( GetRequirements( skill, true ) + "<br><br>", op );
			HandleString( "**Allows the following:**<br><br>", op );
			HandleString( GetAllowed( skill ) + "<br><br>", op );
			HandleString( "**Available to:**<br><br>", op );
			HandleString( GetRacialRequirements( skill ) + "<br><br>", op );
			HandleString( "**Cost:** " + skill.Cost1 + " CPs<br><br>", op );
			HandleString( "**Command:** " + skill.FirstCommand + "<br><br>", op );
			HandleString( "====== " + skill.Name + " Level 2 ======", op );
			HandleString( "**Description: (Upgrade)** ", op );
			HandleString( skill.SecondDescription + "<br><br>", op );
			HandleString( "**Cost:** " + skill.Cost2 + " CPs<br><br>", op );
			HandleString( "**Command:** " + skill.SecondCommand + "<br><br>", op );
			HandleString( "====== " + skill.Name + " Level 3 ======", op );
			HandleString( "**Description: (Final)** ", op );
			HandleString( skill.ThirdDescription + "<br><br>", op );
			HandleString( "**Cost:** " + skill.Cost3 + " CPs<br><br>", op );
			HandleString( "**Command:** " + skill.ThirdCommand, op ); 
		}
		
		public static void HandleString( string st, StreamWriter op )
		{
			string toPrint = st.Replace( "<br>", @"<break>\\ " );
			string[] lines = toPrint.Split( new string[]{"<break>"}, StringSplitOptions.None );
			
			for( int i = 0; i < lines.Length; i++ )
				op.WriteLine( lines[i] );
		}

		public static void WriteWebpage( BaseFeat skill )
		{
			string fileName = OldPages == true ? skill.Name.Replace(" ", "") : skill.Name.Replace(" ", "_").ToLower();
			string extension = OldPages == true ? ".htm" : ".txt";
			
			using ( StreamWriter op = new StreamWriter( Misc.StatusPage.WebFolder + WikiFolder + fileName + extension ) )
			{
				if( OldPages )
				{
					op.WriteLine( "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" );
					op.WriteLine( "<html xmlns=\"http://www.w3.org/1999/xhtml\">" );
					op.WriteLine( "<head>" );
					op.WriteLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" );
					op.WriteLine( "<title>" + skill.Name + "</title>" );
					op.WriteLine( "<link href=\"styles.css\" rel=\"stylesheet\" type=\"text/css\" />" );
					op.WriteLine( "<style type=\"text/css\"></style></head><body><div id=\"main\">" );
					op.WriteLine( GetWebpageDescription(skill) );
					op.WriteLine( "</div></body></html>" );
				}
				
				else
					PublishWikiPage( skill, op );
			}
		}
	}
}
