using System;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Misc
{
	public class BaseCustomSpell : object
	{
		public virtual bool IsMageSpell{ get{ return false; } }
		public virtual bool CanTargetSelf{ get{ return false; } }
		public virtual bool AffectsMobiles{ get{ return false; } }
		public virtual bool AffectsItems{ get{ return false; } }
		public virtual bool IsHarmful{ get{ return false; } }
		public virtual bool UsesTarget{ get{ return false; } }
		public virtual bool BackpackItemsOnly{ get{ return false; } }
		public virtual bool UsesFullEffect{ get{ return false; } }
		public virtual bool UsesFaith{ get{ return false; } }
		public virtual FeatList Feat{ get{ return FeatList.None; } }
		public virtual string Name{ get{ return "a spell"; } }
		public virtual double FullEffect{ get{ return 0; } }
		public virtual double PartialEffect{ get{ return 0; } }
		public virtual double FaithModifier
		{ 
			get
			{
				if( UsesFaith )
					return Faith == true ? 1.1 : 1;
				
				return 1;
			} 
		}
		public virtual int BaseCost{ get{ return 0; } }
		public virtual int BaseDuration{ get{ return 0; } }
		public virtual int TotalCost{ get{ return (BaseCost * FeatLevel); } }
		public virtual int TotalDuration{ get{ return (BaseDuration * FeatLevel); } }
		public virtual int TotalEffect
		{
			get
			{
				if( UsesFullEffect )
					return (int)(FullEffect * FeatLevel * FaithModifier);
				
				return (int)(PartialEffect * FeatLevel * FaithModifier);
			}
		}
		public virtual int BaseRange{ get{ return ((FeatLevel * 2) +4); } }
		public virtual SkillName GetSkillName
		{
			get
			{
				if( IsMageSpell )
					return SkillName.Magery;
				
				return SkillName.Faith;
			}
		}
		public virtual IEntity TargetLocation
		{
			get
			{
				if( TargetMobile != null )
					return TargetMobile;
				
				if( TargetItem != null )
				{
					if( TargetItem.RootParentEntity == null )
						return TargetItem;
					
					return TargetItem.RootParentEntity;
				}
				
				return m_Caster;
			}
		}
		public virtual bool BadCasting
		{
			get
			{
				if( m_Caster == null || m_Caster.Deleted || !m_Caster.Alive || m_Caster.Paralyzed )
					return true;

                XmlAwe awe = XmlAttach.FindAttachment( m_Caster, typeof( XmlAwe ) ) as XmlAwe;
                if( awe != null )
                    return true;
				
				return false;
			}
		}
		public virtual bool Faith
		{
			get
			{
				if( m_Caster == null || m_TargetMobile == null )
					return false;
				
				if( m_Caster is PlayerMobile && m_TargetMobile is PlayerMobile && ((PlayerMobile)m_Caster).ChosenDeity != ChosenDeity.None && 
				   ((PlayerMobile)m_Caster).ChosenDeity == ((PlayerMobile)m_TargetMobile).ChosenDeity && 
				   ((PlayerMobile)m_TargetMobile).Backgrounds.BackgroundDictionary[BackgroundList.Faithful].Level > 0 )
                	return true;
				
				return false;
			}
		}
		public virtual bool Self
		{
			get
			{
				if( m_Caster == null || m_TargetMobile == null )
					return false;
				
				if( m_Caster == m_TargetMobile )
                	return true;
				
				return false;
			}
		}
		public virtual bool TargetCanBeAffected
		{
			get
			{
				if( BadCasting )
					return false;
				
				if( AffectsMobiles && CanAffectTargetMobile )
					return true;
				
				if( AffectsItems && CanAffectTargetItem )
                	return true;
				
				return false;
			}
		}
		public virtual bool CanAffectTargetMobile
		{
			get
			{
				if( TargetMobile == null || TargetMobile.Deleted || !TargetMobile.Alive || TargetMobile.Blessed )
					return false;
				
				return CanTargetEntity(TargetMobile);
			}
		}
		public virtual bool CanAffectTargetItem
		{
			get
			{
				if( TargetItem == null || TargetItem.Deleted || (BackpackItemsOnly && m_TargetItem.RootParentEntity != m_Caster) )
					return false;
				
				return CanTargetEntity(TargetItem);
			}
		}

        FeatList requiredFeat = FeatList.Invocation;
        public virtual FeatList RequiredFeat { get { return requiredFeat; } set { requiredFeat = value; } }

		public virtual bool CanBeCast
		{
			get
			{
				if( BadCasting )
					return false;
				
				IKhaerosMobile caster = m_Caster as IKhaerosMobile;

                if (caster == null)
                    return false;

                if (!(caster.Feats.GetFeatLevel(RequiredFeat) > 0))
                {
                    m_Caster.SendMessage(String.Format("You cannot cast this spell without the {0} feat.", RequiredFeat));
                    return false;
                }

			    if( caster.IsTired() )
					return false;

				if( m_Caster.Target != null )
				{
					m_Caster.SendMessage( "Close your current target first." );
					return false;
				}
				
				XmlAttachment freeze = XmlAttach.FindAttachment( m_Caster, typeof( XmlFreeze ) );
		            
		        if( freeze != null && ( freeze.Name == "icast" || freeze.Name == "fcast" ) )
		        {
		        	m_Caster.SendMessage( "You are already casting a spell." );
					return false;
		        }
		        
		        if( FeatLevel < 1 )
		        {
		        	m_Caster.SendMessage( "You lack the appropriate feat." );
		        	return false;
		        }
		        
		        if( m_Caster is PlayerMobile && DateTime.Compare( caster.NextFeatUse, DateTime.Now ) > 0 )
		    	{
					TimeSpan waitingtime = caster.NextFeatUse - DateTime.Now;
		    		m_Caster.SendMessage( 60, "You need wait another " + waitingtime.Seconds + " seconds before using another ability." );
		    		return false;
		    	}
		        
		        return true;
			}
		}
		public virtual bool CasterHasEnoughMana
		{
			get
			{
				if( BadCasting )
					return false;
				
				else
					return HasEnoughMana( m_Caster, TotalCost );
			}
		}
		
		private Mobile m_Caster;
		private Mobile m_TargetMobile;
		private Item m_TargetItem;
		private int m_FeatLevel;
		private bool m_Success;
		
		public Mobile Caster{ get{ return m_Caster; } set{ m_Caster = value; } }
		public Mobile TargetMobile{ get{ return m_TargetMobile; } set{ m_TargetMobile = value; } }
		public Item TargetItem{ get{ return m_TargetItem; } set{ m_TargetItem = value; } }
		public int FeatLevel{ get{ return m_FeatLevel; } set{ m_FeatLevel = value; } }
		public bool Success{ get{ return m_Success; } set{ m_Success = value; } }
		
		public static bool HasEnoughMana( Mobile m, int amount )
		{
			if( m.Mana < amount )
			{
				m.SendMessage( "Not enough mana." );
				return false;
			}
			
			return true;
		}
		public virtual bool CanTargetEntity( IEntity entity )
		{
			if( entity == null || !(entity is object) || !m_Caster.CanSee( (object)entity ) || m_Caster.Map != entity.Map || !m_Caster.InLOS( entity ) )
				return false;
			
			return true;
		}
		
		public BaseCustomSpell( Mobile caster, int featLevel )
		{
			m_Caster = caster;
			m_FeatLevel = featLevel;
		}
		
		public virtual void Effect()
		{
		}
		
		public virtual void BeforeEffect()
		{
			if( BadCasting )
				return;
			
			FinalAnimation();

            if( !CheckProblems() )
            {
                Effect();

                if( TargetMobile != null && TargetMobile.Alive && !TargetMobile.Deleted && TargetMobile is BaseCreature && IsHarmful )
                {
                    ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpellHitInvokeOnMobile, Caster );
                    ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpellHit );
                }
            }

            else if( TargetMobile != null && TargetMobile.Alive && !TargetMobile.Deleted && TargetMobile is BaseCreature && IsHarmful )
            {
                ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpellMissedInvokeOnMobile, Caster );
                ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpellMissed );
            }

            if( TargetMobile != null && TargetMobile.Alive && !TargetMobile.Deleted && TargetMobile is BaseCreature && IsHarmful )
            {
                ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpellInvokeOnMobile, Caster );
                ( (BaseCreature)TargetMobile ).OnXMLEvent( XMLEventType.OnTargetedBySpell );
            }
			
			AfterHarmfulSpell();
			FinishCasting();
		}
		
		public virtual bool CheckProblems()
		{
			if( UsesTarget && TargetMobile != null && IsHarmful && ((IKhaerosMobile)TargetMobile).Evaded() )
				return true;
			
			if( ((IKhaerosMobile)Caster).Fizzled )
				return true;
			
			return false;
		}
		
		public virtual void AfterHarmfulSpell()
		{
			if( IsHarmful && TargetCanBeAffected && TargetMobile is BaseCreature )
				((BaseCreature)TargetMobile).AggressiveAction( Caster, false );
		}
		
		public virtual void FinishCasting()
		{
			if( BadCasting )
				return;
			
			IKhaerosMobile caster = m_Caster as IKhaerosMobile;
			caster.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( Math.Max( 2, Convert.ToInt32( ( 60 - caster.Level ) * 0.2 ) ) );
			
			if( caster.StunnedTimer == null )
			{
				XmlFreeze freeze = new XmlFreeze( 1.0 );
				freeze.Name = "fcast";
				XmlAttach.AttachTo( m_Caster, freeze );
			}
			
			if( !Success )
				Fizzled();

            if( Caster.AccessLevel < AccessLevel.GameMaster && caster is PlayerMobile )
                ( (PlayerMobile)Caster ).SpeedHack = false;
		}
		
		public virtual void InitialAnimation()
		{
			if( BadCasting )
				return;
				
			PlayerMobile pm = m_Caster as PlayerMobile;
			IKhaerosMobile caster = m_Caster as IKhaerosMobile;
			m_Caster.RevealingAction();
			caster.Fizzled = false;
			
			if (pm.CanBeMage == true && pm.RawInt >=100 && IsMageSpell)
				return;
			
			if( !m_Caster.Mounted && IsMageSpell )
				m_Caster.Animate( 16, 7, 1, true, false, 0 );
			
			else if( !m_Caster.Mounted )
				m_Caster.Animate( 17, 5, 1, true, false, 0 );
			
			else if( IsMageSpell )
				m_Caster.Emote( "*starts casting a spell*" );
			
			else
				m_Caster.Emote( "*starts casting a spell*" );
			
			if( caster.StunnedTimer == null )
			{
				XmlFreeze freeze = new XmlFreeze( 1.0 );
				freeze.Name = "icast";
				XmlAttach.AttachTo( m_Caster, freeze );
			}
		}
		
		public virtual void FinalAnimation()
		{
			PlayerMobile pm = m_Caster as PlayerMobile;
			Success = false;			
			IKhaerosMobile caster = m_Caster as IKhaerosMobile;
			m_Caster.RevealingAction();
			
			if (pm.CanBeMage == true && pm.RawInt >=100 && IsMageSpell)
				return;
			
			if( !m_Caster.Mounted && IsMageSpell )
				m_Caster.Animate( 16, 7, 1, true, false, 0 );
			
			else if( !m_Caster.Mounted )
				m_Caster.Animate( 17, 5, 1, true, false, 0 );
			
			if( !Self && TargetLocation != m_Caster )
				m_Caster.Direction = m_Caster.GetDirectionTo( TargetLocation );
		}
		
		public virtual void Fizzled()
		{
			if( BadCasting )
				return;
			
			m_Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
			m_Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502632 ); // The spell fizzles.
			m_Caster.PlaySound( 0x5C );
		}
		
		public static int GetSpellPower( string modlevel, int featlevel )
		{
			if( featlevel > 0 )
			{
				if( modlevel.Length == 0 )
					return featlevel;
				
				else if( modlevel == "1" )
					return 1;
				
				else if( modlevel == "2" && featlevel > 1 )
					return 2;
				
				else if( modlevel == "3" && featlevel > 2 )
					return 3;
			}
			
			return 0;
		}
		
		public virtual void CastCallback()
		{
			if( BadCasting )
				return;
			
			if( UsesTarget )
				m_Caster.Target = new CustomSpellTarget( m_Caster, this );
			
			else
			{
				CheckCasting( m_Caster.Skills[SkillName.Invocation].Base, false );
    			BeforeEffect();
			}
		}
		
		public virtual void CheckCasting( double chance, bool resist )
		{
			if( resist )
				chance -= m_TargetMobile.EnergyResistance;
			
			if( m_Caster.CheckSkill( GetSkillName, Math.Max(20.0, chance) ) )
    			CheckConcentration();
		}
		
		public virtual void CheckConcentration()
		{
			if( BadCasting )
				return;
			
			double threat = 0.0;
			Success = true;
    			
			foreach( Mobile mob in m_Caster.GetMobilesInRange( 4 ) )
			{
				if( mob != null && !mob.Deleted && mob.Alive && mob.CanSee( m_Caster ) && mob.Map == m_Caster.Map && mob.CanBeHarmful( m_Caster, false, false ) && mob.Warmode && mob.Combatant == m_Caster && mob.Weapon != null && ( (BaseWeapon)mob.Weapon ).MaxRange >= mob.GetDistanceToSqrt( m_Caster.Location ) )
					threat += 20.0;
			}
			
			if( threat > 0.0 )
			{
				double chance = m_Caster.Skills[SkillName.Concentration].Base;
    			SkillName skill = GetSkillName;
    			
    			chance -= threat;
    			
    			if( chance < 20.0 )
    				chance = 20.0;
    			
    			Success = m_Caster.CheckSkill( skill, chance );
			}
		}
		
		public static void SpellInitiator( BaseCustomSpell spell )
		{
			spell.Cast();
		}
		
		public virtual void Cast()
		{
			if( CanBeCast )
			{
				PlayerMobile pm = m_Caster as PlayerMobile;
				InitialAnimation();
				((IKhaerosMobile)m_Caster).CurrentSpell = Feat;
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( CastCallback ) );
				if ( pm.RawInt >=100 && IsMageSpell)
				m_Caster.SendMessage( "You silently begin casting " + Name + "." );
				else
				m_Caster.SendMessage( "You begin casting " + Name + "." );
			}

		}
		
		public class CustomSpellTarget : Target
        {
        	private Mobile m_caster;
        	private BaseCustomSpell m_spell;
        	
            public CustomSpellTarget( Mobile caster, BaseCustomSpell spell )
            	: base( spell.BaseRange, false, TargetFlags.None )
            {
            	m_caster = caster;
            	m_spell = spell;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m_spell.BadCasting )
            		return;
            	
                if( obj is Mobile && m_spell.AffectsMobiles )
                {
                	m_spell.TargetMobile = (Mobile)obj;
                	
                	if ( !m_spell.CanTargetSelf && m_spell.Self )
                	    m_caster.SendMessage( "You cannot target yourself." );
                	
                	else
                		m_spell.CheckCasting( m_spell.m_Caster.Skills[SkillName.Invocation].Base, m_spell.IsHarmful );
                	
                	m_spell.BeforeEffect();
                }
                
                else if( obj is Item && m_spell.AffectsItems )
                {
                	m_spell.TargetItem = (Item)obj;
                	m_spell.CheckCasting( m_spell.m_Caster.Skills[SkillName.Invocation].Base, false );
                	m_spell.BeforeEffect();
                }
                
                else
                	m_caster.SendMessage( "Invalid target." );
            }
        }
		
		public class SpellDamageTimer : Timer
        {
            private Mobile m_target;
            private Mobile m_caster;
            private double m_damage;

            public SpellDamageTimer( Mobile caster, Mobile target, double delay, double damage )
            	: base( TimeSpan.FromSeconds( delay ) )
            {
                m_target = target as Mobile;
                m_caster = caster;
                m_damage = damage;
            }

            protected override void OnTick()
            {
            	if( m_target == null || m_caster == null )
            		return;
            	
            	if( !m_target.Alive || m_target.Blessed || m_target.Deleted )
            		return;
            	
            	AOS.Damage( m_target, m_caster, Convert.ToInt32( m_damage ), false, 0, 0, 0, 0, 100, 0, 0, 0, false );
                
            }
        }
		
		public class SummoningTimer : Timer
        {
            private BaseCreature bc;

            public SummoningTimer( BaseCreature creat, int delay )
            	: base( TimeSpan.FromMinutes( delay ) )
            {
            	bc = creat;
            }

            protected override void OnTick()
            {
            	if( bc != null )
            	{
            		bc.Emote( bc.VanishEmote );
            		bc.Delete();
            	}
            }
        }
		
		public static void Summon( Mobile caster, BaseCreature summoned, int duration, int sound )
		{
			Summon( caster, summoned, duration, sound, true );
		}
		
		public static void Summon( Mobile caster, BaseCreature summoned, int duration, int sound, bool cooldown )
		{
			summoned.Controlled = true;
			summoned.ControlMaster = caster;
			Map map = caster.Map;
			bool validLocation = false;
			Point3D loc = caster.Location;

			for ( int j = 0; !validLocation && j < 10; ++j )
			{
				int x = caster.X + Utility.Random( 3 ) - 1;
				int y = caster.Y + Utility.Random( 3 ) - 1;
				int z = map.GetAverageZ( x, y );

				if ( validLocation = map.CanFit( x, y, caster.Z, 16, false, false ) )
					loc = new Point3D( x, y, caster.Z );
				else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
					loc = new Point3D( x, y, z );
			}

			summoned.MoveToWorld( loc, map );
			summoned.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
			caster.PlaySound( sound );
			
			if( caster is PlayerMobile && cooldown )
				((PlayerMobile)caster).NextSummoningAllowed = DateTime.Now + TimeSpan.FromMinutes( duration );
			
			else if( caster is BaseCreature )
				((BaseCreature)caster).NextFeatUse = DateTime.Now + TimeSpan.FromMinutes( 1 );
			
			summoned.m_SummoningTimer = new SummoningTimer( summoned, duration );
			summoned.m_SummoningTimer.Start();
			summoned.Lives = -100;
		}
	}
}
