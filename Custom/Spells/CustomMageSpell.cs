using System;
using System.Collections;
using System.Text;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	[PropertyObject]
	public class CustomMageSpell : BaseCustomSpell
	{
        public virtual CustomMageSpell GetNewInstance()
        {
            return new CustomMageSpell();
        }

        public virtual bool CustomScripted { get { return false; } }
        public virtual Type ScrollType { get { return typeof( CustomSpellScroll ); } }
		public override string ToString() { return "..."; }
		
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool IsHarmful{ get{ return true; } }
		public override bool UsesTarget{ get{ return (Range > 0); } }
		public override bool UsesFullEffect{ get{ return !(TargetMobile is PlayerMobile); } }
		public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }
		public override string Name{ get{ return (CustomName ?? "a Custom Mage Spell"); } }
		public override int TotalCost{ get{ return ManaCost; } }
		public override double FullEffect{ get{ return (HadFirstEffect == true ? ChainedDamage : Damage); } }
		public override double PartialEffect{ get{ return (int)(FullEffect * 0.75); } }
		public override int BaseRange{ get{ return Range; } }

		
		private Mobile m_NextTarget;
		private string m_CustomName;
		private int m_Damage;
		private int m_Range;
		private int m_ChainedTargets;
		private int m_ChainedDamage;
		private int m_ChainedRange;
		private int m_ExplosionDamage;
		private int m_ExplosionArea;
		private int m_Reps;
		private int m_RepDelay;
		private int m_RepDamage;
		private int m_StatusType;
		private int m_StatusDuration;
		private int m_EffectID;
		private int m_EffectHue;
		private int m_EffectSound;
		private int m_ExplosionID;
		private int m_ExplosionHue;
		private int m_ExplosionSound;
		private int m_IconID = 6016;
		private bool m_HadFirstEffect;
		private double m_DamageDelay;
		
		public Mobile NextTarget{ get{ return m_NextTarget; } set{ m_NextTarget = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string CustomName{ get{ return m_CustomName; } set{ m_CustomName = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ManaCost
		{ 
			get { return CalculateManaCost(); }
		}

	    int CalculateManaCost()
	    {
	        int effects = 0;
	        double cost = 0;

	        if (Damage > 0)
	        {
	            effects++;
	            cost += GetDamageCost(Damage);
	        }

	        if (Reps > 0 && RepDelay > 0)
	        {
	            effects++;
	            cost += ((Reps*Math.Max(1, ChainedTargets))*(GetDamageCost(RepDamage)*(1.5 - (RepDelay*0.05))));
	        }

	        if (Range > 0)
	        {
	            effects++;
	            cost += (Range*2);
	        }

	        if (ChainedTargets > 0)
	        {
	            effects++;
	            cost += (ChainedTargets*GetDamageCost(ChainedDamage));
	            cost += (ChainedRange*2);
	        }

	        if (ExplosionArea > 0)
	        {
	            effects++;
	            cost += (GetDamageCost(ExplosionDamage)*(1 + (0.5*ExplosionArea)));
	        }

	        if (StatusType > 0)
	        {
	            effects++;
	            cost += (StatusType*(StatusDuration*5));
	        }

	        return (int) (cost + (effects*effects));
	    }

	    public double GetDamageCost( int damage )
		{
            if (this.IsTouchspell())
                return ((damage * (damage * 0.005))*6);
			return (damage * (damage * 0.005));
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage
        {
            get
            {
                return m_Damage;
            }
            set
            {
                m_Damage = Math.Max(0, value);
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Range{ get{ return m_Range; } set{ m_Range = Math.Max( 0, Math.Min( 15, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ChainedTargets{ get{ return m_ChainedTargets; } set{ m_ChainedTargets = Math.Max( 0, Math.Min( 6, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ChainedDamage{ get{ return m_ChainedDamage; } set{ m_ChainedDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ChainedRange{ get{ return m_ChainedRange; } set{ m_ChainedRange = Math.Max( 0, Math.Min( 15, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExplosionDamage{ get{ return m_ExplosionDamage; } set{ m_ExplosionDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExplosionArea{ get{ return m_ExplosionArea; } set{ m_ExplosionArea = Math.Max( 0, Math.Min( 6, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Reps{ get{ return m_Reps; } set{ m_Reps = Math.Max( 0, Math.Min( 6, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int RepDelay{ get{ return m_RepDelay; } set{ m_RepDelay = Math.Max( 0, Math.Min( 15, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int RepDamage{ get{ return m_RepDamage; } set{ m_RepDamage = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int StatusType{ get{ return m_StatusType; } set{ m_StatusType = Math.Max( 0, Math.Min( 3, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int StatusDuration{ get{ return m_StatusDuration; } set{ m_StatusDuration = Math.Max( 0, Math.Min( 6, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int EffectID{ get{ return m_EffectID; } set{ m_EffectID = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int EffectHue{ get{ return m_EffectHue; } set{ m_EffectHue = Math.Max( 0, Math.Min( 2999, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int EffectSound{ get{ return m_EffectSound; } set{ m_EffectSound = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExplosionID{ get{ return m_ExplosionID; } set{ m_ExplosionID = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExplosionHue{ get{ return m_ExplosionHue; } set{ m_ExplosionHue = Math.Max( 0, Math.Min( 2999, value ) ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int ExplosionSound{ get{ return m_ExplosionSound; } set{ m_ExplosionSound = Math.Max( 0, value ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int IconID
		{
			get{ return m_IconID; }
			set
			{
				int newValue = Math.Max( 6016, Math.Min( 6185, value ) );
				
				if( newValue > 6047 && newValue < 6057 )
					newValue = 6057;
				
				m_IconID = newValue;;
			}
		}
		
		public bool HadFirstEffect{ get{ return m_HadFirstEffect; } set{ m_HadFirstEffect = value; } }
		public double DamageDelay{ get{ return m_DamageDelay; } set{ m_DamageDelay = value; } }

        public CustomMageSpell()
            : this( null, 1 )
        {
        }

		public CustomMageSpell( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}

        private bool IsTouchspell()
        {
            return this.Range == 0;
        }

        public bool HasRequiredArcanas( FeatList[] requirements )
        {
            if( Caster == null || !( Caster is PlayerMobile ) )
                return false;

            PlayerMobile pm = (PlayerMobile)Caster;
            bool failed = false;

            for( int i = 0; i < requirements.Length; i++ )
            {
                if( pm.Feats.GetFeatLevel( requirements[i] ) < 3 )
                {
                    StringBuilder arcana = new StringBuilder();
                    arcana.Append( requirements[i].ToString() );

                    if( arcana.ToString().EndsWith( "II" ) )
                        arcana.Insert( arcana.Length - 2, " " );

                    else
                        arcana.Insert( arcana.Length - 1, " " );

                    pm.SendMessage( "You need the 3rd level of " + arcana + " in order to cast this spell." );
                    failed = true;
                }
            }

            return !failed;
        }
		
		public override void Effect()
		{
			if( CasterHasEnoughMana )
			{
				Caster.Mana -= TotalCost;
				
				if( Range > 0 && TargetCanBeAffected )
				{
					if( StatusType > 0 )
	            	{
	            		TargetMobile.PlaySound( 0x204 );
	            		TargetMobile.FixedParticles( 0x37C4, 1, 8, 9916, 39, 3, EffectLayer.Head );
						TargetMobile.FixedParticles( 0x37C4, 1, 8, 9502, 39, 4, EffectLayer.Head );
	            		new StatusTimer( TargetMobile, StatusType, StatusDuration ).Start();
	            	}
					
					HandleEffect( true );
					Success = true;
				}
				
				else if( ExplosionArea > 0 && Range < 1 && Damage < 1 && ChainedTargets < 1 && StatusType < 1 && RepDamage < 1 )
				{
					Explode( Caster );
					Success = true;
				}
				
				else if( Range < 1 && Caster is PlayerMobile )
				{
					((PlayerMobile)Caster).TouchSpell = this;
					Success = true;
				}
			}
		}
		
		public virtual void HandleEffect( bool first )
		{
			Mobile from = Caster;
			
			if( NextTarget != null && NextTarget.Alive && !NextTarget.Blessed )
				ChangeTarget( ref from );
			
			else if( !first )
				return;
			
			if( first && Range < 1 )
				from = TargetMobile;
			
			ProduceEffect( from, first );
		}
		
		public virtual void ChangeTarget( ref Mobile from )
		{
			from = TargetMobile;
			TargetMobile = NextTarget;
			NextTarget = null;
		}
		
		public virtual void AcquireNewTarget()
		{
			foreach( Mobile mob in TargetMobile.GetMobilesInRange( ChainedRange ) )
			{
				if( mob != TargetMobile && !((IKhaerosMobile)Caster).IsAllyOf(mob) && mob.Alive && mob.InLOS(TargetMobile) && Caster.CanSee(mob) && !mob.Blessed )
				{
					NextTarget = mob;
					ChainedTargets--;
					break;
				}
			}
		}
		
		public virtual void Explode( Mobile from )
		{
			if( ExplosionSound > 0 )
				from.PlaySound( ExplosionSound );
			
			ArrayList list = new ArrayList();
			
			foreach( Mobile mob in from.GetMobilesInRange( ExplosionArea ) )
				if( !((IKhaerosMobile)Caster).IsAllyOf(mob) && mob.Alive && mob.InLOS(Caster) && Caster.CanSee(mob) && !mob.Blessed )
					list.Add( mob );
			
			for( int i = 0; i < list.Count; i++ )
			{
				Mobile m = list[i] as Mobile;
				m.FixedParticles( ExplosionID, 10, 30, 5052, ExplosionHue, 0, EffectLayer.LeftFoot );
                if (m is PlayerMobile)
				    AOS.Damage( m, Caster, ExplosionDamage, false, 0, 0, 0, 0, 100, 0, 0, 0, false );
                else
                    AOS.Damage(m, Caster, ExplosionDamage*2, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
			}
		}
		
		public virtual void ProduceEffect( Mobile from, bool first )
		{
			if( first && EffectSound > 0 )
				Caster.PlaySound( EffectSound );
			
			if( EffectID > 0 )
				from.MovingEffect( TargetMobile, EffectID, 5, 0, false, true, EffectHue, 0 );
			
			DamageDelay = 0.1 * from.GetDistanceToSqrt( TargetMobile.Location );
			new MageChainEffectTimer( this ).Start();
		}
		
		public class RecurrentDamageTimer : Timer
        {
            private Mobile m_target;
            private CustomMageSpell m_spell;

            public RecurrentDamageTimer( Mobile target, CustomMageSpell spell )
            	: base( TimeSpan.FromSeconds( spell.RepDelay ) )
            {
                m_target = target as Mobile;
                m_spell = spell;
            }

            protected override void OnTick()
            {
            	if( m_target == null || m_spell == null || m_spell.Caster == null )
            		return;
            	
            	if( !m_target.Alive || m_target.Blessed || m_target.Deleted )
            		return;
            	
            	if(m_target is PlayerMobile)
                    AOS.Damage( m_target, m_spell.Caster, m_spell.RepDamage, false, 0, 0, 0, 0, 100, 0, 0, 0, false );
                else
                    AOS.Damage(m_target, m_spell.Caster, m_spell.RepDamage*2, false, 0, 0, 0, 0, 100, 0, 0, 0, false);
            	m_target.FixedParticles( 0x374A, 10, 15, 5013, m_spell.EffectHue, 0, EffectLayer.Waist );
				m_target.PlaySound( 0x1F1 );
            	m_spell.Reps--;
            	
            	if( m_spell.Reps > 0 && m_target.Alive )
            		new RecurrentDamageTimer( m_target, m_spell ).Start();
            }
        }
		
		public class MageChainEffectTimer : Timer
        {
            private CustomMageSpell spell;

            public MageChainEffectTimer( CustomMageSpell cms )
            	: base( TimeSpan.FromSeconds( cms.DamageDelay ) )
            {
                spell = cms;
            }

            protected override void OnTick()
            {
            	if( spell == null )
            		return;
            	
            	if( !spell.TargetCanBeAffected )
            		return;

            	if( spell.ChainedTargets > 0 )
					spell.AcquireNewTarget();
            	
            	if( spell.ExplosionArea > 0 && !spell.HadFirstEffect )
					spell.Explode( spell.TargetMobile );
            	
            	if( spell.Damage > 0 && spell.TargetCanBeAffected )
            	{
                    int damage = 100;

	            	Mobile toDamage = spell.TargetMobile;
	            	spell.HandleEffect( false );
                    if(toDamage is PlayerMobile)
	            	    AOS.Damage( toDamage, spell.Caster, Convert.ToInt32( spell.TotalEffect ), false, 0, 0, 0, 0, damage, 0, 0, 0, false );
                    else
                        AOS.Damage(toDamage, spell.Caster, Convert.ToInt32(spell.TotalEffect)*2, false, 0, 0, 0, 0, damage, 0, 0, 0, false);
	            	
	            	if( toDamage.Alive && spell.Reps > 0 && spell.RepDelay > 0 )
	            		new RecurrentDamageTimer( toDamage, spell ).Start();
            	}

            	spell.HadFirstEffect = true;
            }
        }
		
		public class StatusTimer : Timer
        {
            private Mobile m;
            private int effect;

            public StatusTimer( Mobile from, int status, int time )
                : base( TimeSpan.FromSeconds( time ) )
            {
                m = from;
                effect = status;
                
                if( status == 1 || status == 3 )
                {
                	((IKhaerosMobile)m).FreezeTimer = new EmptyTimer();
                	m.SendMessage( "You are frozen." );
                }
                
                if( status == 2 || status == 3 )
                {
                	((IKhaerosMobile)m).DisabledRightArmTimer = new EmptyTimer();
                	m.SendMessage( "You are unable to attack." );
                }
            }

            protected override void OnTick()
            {
            	if( m == null )
            		return;
            	
            	IKhaerosMobile mob = m as IKhaerosMobile;
            	
            	if( ( effect == 1 || effect == 3) && mob.FreezeTimer != null )
            	{
            		mob.FreezeTimer.Stop();
            		mob.FreezeTimer = null;
            		m.SendMessage( "You are no longer frozen." );
            	}
            	
            	if( effect > 1 && mob.DisabledRightArmTimer != null )
            	{
            		mob.DisabledRightArmTimer.Stop();
            		mob.DisabledRightArmTimer = null;
            		m.SendMessage( "You are no longer unable to attack." );
            	}
            }
        }
		
		public class EmptyTimer : Timer { public EmptyTimer() : base( TimeSpan.FromDays( 1 ) ) {} }
		
		public static void Initialize()
		{
			CommandSystem.Register( "MageSpell", AccessLevel.Player, new CommandEventHandler( MageSpell_OnCommand ) );
		}
		
		[Usage( "MageSpell" )]
        [Description( "Casts a Mage Spell." )]
        private static void MageSpell_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null && e.ArgString != null )
        	{
        		PlayerMobile m = e.Mobile as PlayerMobile;
        		
        		if( m.SpellBook == null )
        			m.SendMessage( "Please place your spell book in your backpack, double-click it and close it. Then try this command again." );
        		
        		else if( m.SpellBook.ContainsSpell(e.ArgString) )
        			CustomSpellScroll.CastCustomMageSpell( m, m.SpellBook.GetSpellByName(e.ArgString) );
        	}
        }
	}
}
