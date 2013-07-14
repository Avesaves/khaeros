using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Misc.BreedingSystem;

namespace Server.Mobiles
{
	public class BaseBreedableCreature : BaseCreature
	{
		private int m_MatesXPScale;
		private int m_MatesStatScale;
		private int m_MatesSkillScale;
		private string m_NewBreed;
		private string m_Breed;
		private bool m_Pregnant;
		private DateTime m_Conception;
		private int m_HueGroup;
		private int m_MatesHueGroup;
		private int m_MatesLevel;
        private int m_TrainingRemaining;
        private int m_TrainingPoints;
        private int m_PetEvolution;
        private List<int> m_MatesFeats;
        private int m_ExtraLives;
        private int m_CubsExtraLives;
        private bool m_Gelt;
        private string m_Brand;
        private int m_InitialFeats;

		public virtual int MaxCubs{ get{ return 1; } }
		public virtual int[] Hues{ get{ return new int[]{0,0,0}; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveFeatAmount
		{
			get{ return false; }
			set
			{
				if( value == true )
				{
					List<int> list = this.ListFeats();
					this.Say( "I have {0} feat{1}.", list.Count == 0 ? "no" : list.Count.ToString(), list.Count == 1 ? "" : "s" );
				}
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ForceLabour
		{
			get{ return false; }
			set
			{
				if( value == true && this.Pregnant )
					this.Labour();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int GiveXRandomFeats
		{
			get{ return 0; }
			set
			{
				if( value > 0 )
					this.RaiseXRandomFeats(value);
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int RemoveXRandomFeats
		{
			get{ return 0; }
			set
			{
				if( value > 0 )
					this.LowerXRandomFeats(value);
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ListMyFeats
		{
			get{ return false; }
			set
			{
				if( value == true )
				{
					List<int> list = this.ListFeats();
					
					foreach( int i in list )
						this.Say( "Feat " + i.ToString() + "." );
					
					if( list.Count < 1 )
						this.Say( "I have no feats." );
				}
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool RemoveRandomFeat
		{
			get{ return false; }
			set
			{
				if( value == true )
					this.LowerRandomFeat();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveRandomFeat
		{
			get{ return false; }
			set
			{
				if( value == true )
					this.RaiseRandomFeat();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int InitialFeats
		{
			get{ return m_InitialFeats; }
			set{ m_InitialFeats = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string Brand
		{
			get{ return m_Brand; }
			set{ m_Brand = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Gelt
		{
			get{ return m_Gelt; }
			set{ m_Gelt = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExtraLives
		{
			get{ return m_ExtraLives; }
			set{ m_ExtraLives = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int CubsExtraLives
		{
			get{ return m_CubsExtraLives; }
			set{ m_CubsExtraLives = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public string NewBreed
        {
            get{ return m_NewBreed; }
            set{ SetBreedsTraits( value, -1 ); }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string Breed
        {
            get{ return m_Breed; }
        }
        
        public string ChangeBreed
        {
            get{ return m_Breed; }
            set{ m_Breed = value; }
        }
        
        public int HueGroup
        {
            get{ return m_HueGroup; }
            set
            {
            	if( value > 2 )
            		value = 2;
            	
            	if( value < 0 )
            		value = 0;
            	
            	this.Hue = this.Hues[value];
            	m_HueGroup = value;
            }
        }
        
        public int MatesHueGroup
        {
            get{ return m_MatesHueGroup; }
            set{ m_MatesHueGroup = value; }
        }
        
        public int MatesLevel
        {
            get{ return m_MatesLevel; }
            set{ m_MatesLevel = value; }
        }
        
        public int MatesXPScale
        {
            get{ return m_MatesXPScale; }
            set{ m_MatesXPScale = value; }
        }
        
        public int MatesStatScale
        {
            get{ return m_MatesStatScale; }
            set{ m_MatesStatScale = value; }
        }
        
        public int MatesSkillScale
        {
            get{ return m_MatesSkillScale; }
            set{ m_MatesSkillScale = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool Pregnant
        {
            get { return m_Pregnant; }
            set { m_Pregnant = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime Conception
        {
            get { return m_Conception; }
            set { m_Conception = value; }
        }
        
        public int TrainingRemaining
        {
            get{ return m_TrainingRemaining; }
            set{ m_TrainingRemaining = value; }
        }
        
        public int TrainingPoints
        {
            get{ return m_TrainingPoints; }
            set{ m_TrainingPoints = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int PetEvolution
        {
            get{ return m_PetEvolution; }
            set{ m_PetEvolution = value; }
        }

        public void UpdateSpeeds()
        {
            if (this is Wolf)
            {
                PassiveSpeed = 0.350;
                ActiveSpeed = 0.1;
            }
            else if (this is Serpent)
            {
                PassiveSpeed = 0.350;
                ActiveSpeed = 0.175;
            }
            else if (this is Bear)
            {
                PassiveSpeed = 0.4;
                ActiveSpeed = 0.2;
            }
            else if (this is WorkHorse)
            {
                ActiveSpeed = 0.2;
                PassiveSpeed = 0.4;
            }
        }

        public List<int> MatesFeats
        {
            get
            {
            	if( m_MatesFeats == null )
            		m_MatesFeats = new List<int>();
            	
            	return m_MatesFeats; 
            }
            set{ m_MatesFeats = value; }
        }
			
        public BaseBreedableCreature( AIType aiType, FightMode fightMode, int rangePerception, int rangeFight, double activeSpeed, double passiveSpeed ) : base ( aiType, fightMode, rangePerception, rangeFight, activeSpeed, passiveSpeed )
		{
        	Female = Utility.RandomBool();
        	Lives = 1;
		}
		
		public virtual void SetBreedsTraits( string breed, int group )
		{
		}
		
		public virtual void Impregnate( BaseBreedableCreature male, PlayerMobile owner, int featlevel )
		{
			this.CubsExtraLives = (featlevel - 1) * 3;
			this.Pregnant = true;
			this.Conception = DateTime.Now;
			this.MatesXPScale = male.XPScale;
			this.MatesStatScale = male.StatScale;
			this.MatesSkillScale = male.SkillScale;
			this.MatesHueGroup = male.HueGroup;
			this.MatesLevel = male.Level;
			this.Feats.SetFeatLevel(FeatList.PetFeats, owner.Feats.GetFeatLevel(FeatList.PetFeats));
			this.Feats.SetFeatLevel(FeatList.RetrainPet, owner.Feats.GetFeatLevel(FeatList.RetrainPet));
			this.Feats.SetFeatLevel(FeatList.ExtraPetFeats, owner.Feats.GetFeatLevel(FeatList.ExtraPetFeats));
			this.Feats.SetFeatLevel(FeatList.PetEvolution, owner.Feats.GetFeatLevel(FeatList.PetEvolution));
			this.MatesFeats = male.ListFeats();
			this.InvalidateProperties();
		}
		
		public List<int> ListFeats()
		{
			List<int> list = new List<int>();
			
			for( int index = 0; index < 16; index++ )
			{
				int featValue = GetFeat(index);
				
				for( int rep = 0; rep < featValue; rep++ )
					list.Add(index);
			}
			
			return list;
		}
		
		public bool CanBeHandledBy( Mobile owner )
		{
			return ( (this.ControlMaster != null && this.ControlMaster == owner) || (this.Friends != null && this.Friends.Contains(owner)) );
		}
		
		public virtual void Labour()
		{
			int cubs = 1;
			
			//More cubs based on the mother's level.
			for( int i = 1; i < this.MaxCubs; i++ )
				if( this.Level >= Utility.RandomMinMax(1, 50) )
				   cubs++;
			
			this.Emote( "*gives birth to " + cubs + " animal{0}*", cubs > 1 ? "s" : "" );
			
			for( int a = 0; a < cubs; a++ )
				PrepareToGiveBirth();
			
			this.Pregnant = false;
			this.InvalidateProperties();
		}
		
		public virtual void PrepareToGiveBirth()
		{
		}
		
		public virtual void GiveBirth( BaseBreedableCreature bbc )
		{
			bool validLocation = false;
			Point3D loc = this.Location;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = X + Utility.Random( 3 ) - 1;
				int y = Y + Utility.Random( 3 ) - 1;
				int z = this.Map.GetAverageZ( x, y );

				if ( validLocation = this.Map.CanFit( x, y, this.Z, 16, false, false ) )
					loc = new Point3D( x, y, Z );
				else if ( validLocation = this.Map.CanFit( x, y, z, 16, false, false ) )
					loc = new Point3D( x, y, z );
			}

			bbc.FixScales();
			bbc.MoveToWorld( loc, this.Map );
			bbc.SetTraits(this);
		}
		
		public virtual void SetTraits( BaseBreedableCreature mother )
		{
			this.Lives += mother.CubsExtraLives;
			this.ExtraLives = mother.CubsExtraLives;
			this.XPScale = mother.MatesXPScale;
			this.StatScale = mother.MatesStatScale;
			this.SkillScale = mother.MatesSkillScale;
			
			//One scale from the mother, the other two from the father.
			switch( Utility.RandomMinMax( 1, 3 ) )
			{
				case 1: this.XPScale = mother.XPScale; break;
				case 2: this.StatScale = mother.StatScale; break;
				case 3: this.SkillScale = mother.SkillScale; break;
			}

            if (this is Serpent)
            {
                this.SetBreedsTraits(this.Breed, -1);
            }
            else
            {
                if (mother.HueGroup == mother.MatesHueGroup)
                    this.HueGroup = mother.HueGroup;

                else if (Utility.RandomBool())
                    this.HueGroup = mother.MatesHueGroup;

                else
                    this.HueGroup = mother.HueGroup;
            }
			
			//Chance to change scales for better or for worse based on the father's level.
			this.XPScale = Utilities.NewScale( this.XPScale, mother.MatesLevel );
			this.StatScale = Utilities.NewScale( this.StatScale, mother.MatesLevel );
			this.SkillScale = Utilities.NewScale( this.SkillScale, mother.MatesLevel );
			this.BirthFeats( mother );
			this.PetEvolution = 3 * mother.Feats.GetFeatLevel(FeatList.PetEvolution);
			this.MarkedForTermination = true;
			this.ReleaseTime = DateTime.Now;
		}
		
		public void BirthFeats( BaseBreedableCreature mother )
		{
			this.TrainingRemaining = mother.Feats.GetFeatLevel(FeatList.RetrainPet);
			this.InitialFeats = (mother.Feats.GetFeatLevel(FeatList.PetFeats) + Utility.RandomMinMax( 0, mother.Feats.GetFeatLevel(FeatList.ExtraPetFeats) )) * 3;
			AddFeats( mother.MatesFeats );
		}
		
		public void AddFeats( List<int> list )
		{
			foreach( int i in list )
				IncFeat( i );
			
			int randomRemove = 0;
			
			//Randomly removing 0-3 feats from the father's pool if the pool has at least 3 feats
			if( list.Count > 2 )
			{
				randomRemove = Utility.RandomMinMax( 0, 3 );
				LowerXRandomFeats( randomRemove );
			}
			
			int current = this.ListFeats().Count;
			int toAdd = this.InitialFeats - current;
			
			//Adding more feats in case the father's pool wasn't enough
			if( toAdd > 0 )
				RaiseXRandomFeats( toAdd );
			
			//Removing more feats in case the father's pool was too much
			if( toAdd < 0 )
				LowerXRandomFeats( -toAdd );
		}
		
		public void RaiseRandomFeat()
		{
			List<int> list = new List<int>();
			
			for( int i = 1; i < 16; i++ )
				list.Add( i );
			
			RaiseRandomFeat( list );
		}
		
		public void RaiseXRandomFeats( int amount )
		{
			for( int i = 0; i < amount; i++ )
				RaiseRandomFeat();
		}
		
		public void RaiseRandomFeat( List<int> list )
		{
			int position = Utility.RandomMinMax(0, (list.Count - 1));
			int posValue = list[position];
			
			if( !TryToRaise(posValue) )
			{
				list.RemoveAt(position);
				
				if( list.Count > 0 )
					RaiseRandomFeat( list );
			}
		}
		
		public bool TryToRaise( int position )
		{
			if( GetFeat(position) < 3 )
			{
				IncFeat(position);
				return true;
			}
			
			return false;
		}
		
		public void LowerXRandomFeats( int amount )
		{
			for( int i = 0; i < amount; i++ )
				LowerRandomFeat();
		}
		
		public void LowerRandomFeat()
		{
			List<int> list = new List<int>();
			
			for( int i = 1; i < 16; i++ )
				list.Add( i );
			
			LowerRandomFeat( list );
		}
		
		public void LowerRandomFeat( List<int> list )
		{
			int position = Utility.RandomMinMax(0, (list.Count - 1));
			int posValue = list[position];
			
			if( !TryToLower(posValue) )
			{
				list.RemoveAt(position);
				
				if( list.Count > 0 )
					LowerRandomFeat( list );
			}
		}
		
		public bool TryToLower( int position )
		{
			if( GetFeat(position) > 0 )
			{
				DecFeat(position);
				return true;
			}
			
			return false;
		}
		
		public void IncFeat( int index )
		{
			switch( index )
			{
				case 1: this.Feats.SetFeatLevel(FeatList.BruteStrength, this.Feats.GetFeatLevel(FeatList.BruteStrength) + 1); break;
				case 2: this.Feats.SetFeatLevel(FeatList.QuickReflexes, this.Feats.GetFeatLevel(FeatList.QuickReflexes) + 1); break;
				case 3: this.Feats.SetFeatLevel(FeatList.Cleave, this.Feats.GetFeatLevel(FeatList.Cleave) + 1); break;
				case 4: this.Feats.SetFeatLevel(FeatList.Evade, this.Feats.GetFeatLevel(FeatList.Evade) + 1); break;
				case 5: this.Feats.SetFeatLevel(FeatList.DamageIgnore, this.Feats.GetFeatLevel(FeatList.DamageIgnore) + 1); break;
				case 6: this.Feats.SetFeatLevel(FeatList.FastHealing, this.Feats.GetFeatLevel(FeatList.FastHealing) + 1); break;
				case 7: this.Feats.SetFeatLevel(FeatList.CriticalStrike, this.Feats.GetFeatLevel(FeatList.CriticalStrike) + 1); break;
				case 8: this.Feats.SetFeatLevel(FeatList.SavageStrike, this.Feats.GetFeatLevel(FeatList.SavageStrike) + 1); break;
				case 9: this.Feats.SetFeatLevel(FeatList.CripplingBlow, this.Feats.GetFeatLevel(FeatList.CripplingBlow) + 1); break;
				case 10: this.Feats.SetFeatLevel(FeatList.EnhancedDodge, this.Feats.GetFeatLevel(FeatList.EnhancedDodge) + 1); break;
				case 11: this.Feats.SetFeatLevel(FeatList.Buildup, this.Feats.GetFeatLevel(FeatList.Buildup) + 1); break;
				case 12: this.Feats.SetFeatLevel(FeatList.FlurryOfBlows, this.Feats.GetFeatLevel(FeatList.FlurryOfBlows) + 1); break;
				case 13: this.Feats.SetFeatLevel(FeatList.FocusedAttack, this.Feats.GetFeatLevel(FeatList.FocusedAttack) + 1); break;
				case 14: this.Feats.SetFeatLevel(FeatList.DefensiveStance, this.Feats.GetFeatLevel(FeatList.DefensiveStance) + 1); break;
				case 15: this.Feats.SetFeatLevel(FeatList.Rage, this.Feats.GetFeatLevel(FeatList.Rage) + 1); break;
			}
		}
		
		public void DecFeat( int index )
		{
			switch( index )
			{
				case 1: this.Feats.SetFeatLevel(FeatList.BruteStrength, this.Feats.GetFeatLevel(FeatList.BruteStrength) - 1); break;
				case 2: this.Feats.SetFeatLevel(FeatList.QuickReflexes, this.Feats.GetFeatLevel(FeatList.QuickReflexes) - 1); break;
				case 3: this.Feats.SetFeatLevel(FeatList.Cleave, this.Feats.GetFeatLevel(FeatList.Cleave) - 1); break;
				case 4: this.Feats.SetFeatLevel(FeatList.Evade, this.Feats.GetFeatLevel(FeatList.Evade) - 1); break;
				case 5: this.Feats.SetFeatLevel(FeatList.DamageIgnore, this.Feats.GetFeatLevel(FeatList.DamageIgnore) - 1); break;
				case 6: this.Feats.SetFeatLevel(FeatList.FastHealing, this.Feats.GetFeatLevel(FeatList.FastHealing) - 1); break;
				case 7: this.Feats.SetFeatLevel(FeatList.CriticalStrike, this.Feats.GetFeatLevel(FeatList.CriticalStrike) - 1); break;
				case 8: this.Feats.SetFeatLevel(FeatList.SavageStrike, this.Feats.GetFeatLevel(FeatList.SavageStrike) - 1); break;
				case 9: this.Feats.SetFeatLevel(FeatList.CripplingBlow, this.Feats.GetFeatLevel(FeatList.CripplingBlow) - 1); break;
				case 10: this.Feats.SetFeatLevel(FeatList.EnhancedDodge, this.Feats.GetFeatLevel(FeatList.EnhancedDodge) - 1); break;
				case 11: this.Feats.SetFeatLevel(FeatList.Buildup, this.Feats.GetFeatLevel(FeatList.Buildup) - 1); break;
				case 12: this.Feats.SetFeatLevel(FeatList.FlurryOfBlows, this.Feats.GetFeatLevel(FeatList.FlurryOfBlows) - 1); break;
				case 13: this.Feats.SetFeatLevel(FeatList.FocusedAttack, this.Feats.GetFeatLevel(FeatList.FocusedAttack) - 1); break;
				case 14: this.Feats.SetFeatLevel(FeatList.DefensiveStance, this.Feats.GetFeatLevel(FeatList.DefensiveStance) - 1); break;
				case 15: this.Feats.SetFeatLevel(FeatList.Rage, this.Feats.GetFeatLevel(FeatList.Rage) - 1); break;
			}
		}
		
		public void SetFeat( int index, int newvalue )
		{
			switch( index )
			{
				case 1: this.Feats.SetFeatLevel(FeatList.BruteStrength, newvalue); break;
				case 2: this.Feats.SetFeatLevel(FeatList.QuickReflexes, newvalue); break;
				case 3: this.Feats.SetFeatLevel(FeatList.Cleave, newvalue); break;
				case 4: this.Feats.SetFeatLevel(FeatList.Evade, newvalue); break;
				case 5: this.Feats.SetFeatLevel(FeatList.DamageIgnore, newvalue); break;
				case 6: this.Feats.SetFeatLevel(FeatList.FastHealing, newvalue); break;
				case 7: this.Feats.SetFeatLevel(FeatList.CriticalStrike, newvalue); break;
				case 8: this.Feats.SetFeatLevel(FeatList.SavageStrike, newvalue); break;
				case 9: this.Feats.SetFeatLevel(FeatList.CripplingBlow, newvalue); break;
				case 10: this.Feats.SetFeatLevel(FeatList.EnhancedDodge, newvalue); break;
				case 11: this.Feats.SetFeatLevel(FeatList.Buildup, newvalue); break;
				case 12: this.Feats.SetFeatLevel(FeatList.FlurryOfBlows, newvalue); break;
				case 13: this.Feats.SetFeatLevel(FeatList.FocusedAttack, newvalue); break;
				case 14: this.Feats.SetFeatLevel(FeatList.DefensiveStance, newvalue); break;
				case 15: this.Feats.SetFeatLevel(FeatList.Rage, newvalue); break;
			}
		}
		
		public int GetFeat( int index )
		{
			switch( index )
			{
				case 1: return this.Feats.GetFeatLevel(FeatList.BruteStrength);
				case 2: return this.Feats.GetFeatLevel(FeatList.QuickReflexes);
				case 3: return this.Feats.GetFeatLevel(FeatList.Cleave);
				case 4: return this.Feats.GetFeatLevel(FeatList.Evade);
				case 5: return this.Feats.GetFeatLevel(FeatList.DamageIgnore);
				case 6: return this.Feats.GetFeatLevel(FeatList.FastHealing);
				case 7: return this.Feats.GetFeatLevel(FeatList.CriticalStrike);
				case 8: return this.Feats.GetFeatLevel(FeatList.SavageStrike);
				case 9: return this.Feats.GetFeatLevel(FeatList.CripplingBlow);
				case 10: return this.Feats.GetFeatLevel(FeatList.EnhancedDodge);
				case 11: return this.Feats.GetFeatLevel(FeatList.Buildup);
				case 12: return this.Feats.GetFeatLevel(FeatList.FlurryOfBlows);
				case 13: return this.Feats.GetFeatLevel(FeatList.FocusedAttack);
				case 14: return this.Feats.GetFeatLevel(FeatList.DefensiveStance);
				case 15: return this.Feats.GetFeatLevel(FeatList.Rage);
			}
			
			return 0;
		}

        public static List<FeatList> GetAllowedPetFeats()
        {
            List<FeatList> feats = new List<FeatList>();

            feats.Add(FeatList.BruteStrength);
			feats.Add(FeatList.QuickReflexes);
			feats.Add(FeatList.Cleave);
			feats.Add(FeatList.Evade);
			feats.Add(FeatList.DamageIgnore);
			feats.Add(FeatList.FastHealing);
			feats.Add(FeatList.CriticalStrike);
			feats.Add(FeatList.SavageStrike);
			feats.Add(FeatList.CripplingBlow);
			feats.Add(FeatList.EnhancedDodge);
			feats.Add(FeatList.Buildup);
			feats.Add(FeatList.FlurryOfBlows);
			feats.Add(FeatList.FocusedAttack);
			feats.Add(FeatList.DefensiveStance);
			feats.Add(FeatList.Rage);

            return feats;
        }
		
		public override void OnThink()
		{
			if( this.Pregnant && this.ControlMaster != null && this.ControlMaster.InRange( this, 1 ) && DateTime.Compare( DateTime.Now, (this.Conception + TimeSpan.FromDays(5)) ) > 0 )
			{
				if( this.ControlMaster is PlayerMobile && ((PlayerMobile)this.ControlMaster).XPFromCrafting )
					LevelSystem.AwardCraftXP( ((PlayerMobile)this.ControlMaster), true );
				
				this.Labour();
			}
	
			base.OnThink();
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Pregnant )
				list.Add( 1060847, "{0}\t{1}", "  " + "Pregnant", " " ); // ~1_val~ ~2_val~
			
			else if ( m_Gelt )
				list.Add( 1060847, "{0}\t{1}", "  " + "Gelt", " " ); // ~1_val~ ~2_val~
			
			if( m_Brand != null && m_Brand.Length > 0 )
				list.Add( 1060658, "{0}\t{1}", "  " + "Branded" , m_Brand ); // ~1_val~ ~2_val~
		}

		public BaseBreedableCreature( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 20 ); // version

			writer.Write( (int) m_PetEvolution );
			writer.Write( (int) m_InitialFeats );
			writer.Write( (string) m_Brand );
			writer.Write( (bool) m_Gelt );
			writer.Write( (int) m_CubsExtraLives );
			writer.Write( (int) m_ExtraLives );
			writer.Write( (int) MatesFeats.Count );
			
			foreach( int i in MatesFeats )
				writer.Write( (int) i );
			
			writer.Write( (int) m_TrainingPoints );
			writer.Write( (int) m_TrainingRemaining );
			writer.Write( (int) m_MatesXPScale );
			writer.Write( (int) m_MatesStatScale );
			writer.Write( (int) m_MatesSkillScale );
			writer.Write( (int) m_MatesLevel );
			writer.Write( (int) m_HueGroup );
			writer.Write( (int) m_MatesHueGroup );
			writer.Write( (string) m_NewBreed );
			writer.Write( (string) m_Breed );
			writer.Write( (bool) m_Pregnant );
			writer.Write( (DateTime) m_Conception );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version < 20)
            {
                if (Level >= 20 && (this is Wolf || this is Serpent || this is WorkHorse))
                {
                    if (this is Wolf)
                    {
                        this.ActiveSpeed = 0.1;
                        this.PassiveSpeed = 0.350;
                    }
                    else if (this is Serpent)
                    {
                        this.ActiveSpeed = 0.175;
                        this.PassiveSpeed = 0.350;
                    }
                    else if (this is WorkHorse)
                    {
                        this.ActiveSpeed = 0.2;
                        this.PassiveSpeed = 0.4;
                    }     
                }
            }
            if (version < 19)
            {
                if (Level >= 20 && this is Bear)
                {
                    this.ActiveSpeed = 0.2;
                    this.PassiveSpeed = 0.4;
                }
            }
			if( version < 18 )
				LevelSystem.FixStatsAndSkills( this );
				
			if( version > 16 )
				m_PetEvolution = reader.ReadInt();

			if( version > 15 )
				m_InitialFeats = reader.ReadInt();
			
			if( version > 14 )
				m_Brand = reader.ReadString();
			
			if( version > 13 )
				m_Gelt = reader.ReadBool();
			
			if( version > 10 )
				m_CubsExtraLives = reader.ReadInt();
			
			if( version > 9 )
				m_ExtraLives = reader.ReadInt();
			
			if( version > 8 )
			{
				int count = reader.ReadInt();
				
				for( int i = 0; i < count; i++ )
					MatesFeats.Add( reader.ReadInt() );
			}
			
			if( version > 7 )
				m_TrainingPoints = reader.ReadInt();
			
			if( version > 6 )
				m_TrainingRemaining = reader.ReadInt();
			
			if( version < 13 )
			{
				TimeOfDeath = reader.ReadDeltaTime();
	
	            if( reader.ReadBool() )
	            	BeginRess( reader.ReadDeltaTime() - DateTime.Now, this.Corpse );

				Lives = reader.ReadInt();
			}

			if( version > 4 )
			{
				m_MatesXPScale = reader.ReadInt();
				m_MatesStatScale = reader.ReadInt();
				m_MatesSkillScale = reader.ReadInt();
			}
			
			if( version > 2 )
				m_MatesLevel = reader.ReadInt();
			
			if( version > 1 )
			{
				m_HueGroup = reader.ReadInt();
				m_MatesHueGroup = reader.ReadInt();
			}
			
			if( version > 0 )
			{
				m_NewBreed = reader.ReadString();
				m_Breed = reader.ReadString();
				m_Pregnant = reader.ReadBool();
				m_Conception = reader.ReadDateTime();
			}
			
			if( version < 12 )
				this.HueGroup = Utility.Random(3);
		}
	}
}
