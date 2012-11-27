using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Commands;

namespace Server.Items
{
    public interface IEasyCraft
    {
    }

	public class RopeTrickCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register( "RopeTrick", AccessLevel.Player, new CommandEventHandler( RopeTrick_OnCommand ) );
    	}
		
		[Usage( "RopeTrick" )]
        [Description( "Attempts to find and use a rope in your backpack." )]
        private static void RopeTrick_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	foreach( Item item in m.Backpack.Items )
        	{
        		if( item is Rope )
        		{
        			item.OnDoubleClick( m );
        			return;
        		}
        	}
        	
        	e.Mobile.SendMessage( "No rope was found in your backpack." );
        }
	}
    public class Rope : Item, IEasyCraft
    {
        private Mobile m_RestrainedTarget;

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile RestrainedTarget
        {
            get
            {
            	if( m_RestrainedTarget == null || ( m_RestrainedTarget != null && m_RestrainedTarget.Deleted ) )
            		ClearRopeInfo();
            	
            	return m_RestrainedTarget;
            }
            
            set { m_RestrainedTarget = value; }
        }

        [Constructable]
        public Rope()
            : base( 0x14F8 )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "a rope";
        }
        
        public void ClearRopeInfo()
        {
        	this.Movable = true;
    		this.Name = "a rope";
    		this.m_RestrainedTarget = null;
        }

        public override void OnDoubleClick( Mobile from )
        {
        	if( from == null || from.Deleted || !from.Alive || from.Paralyzed || !(from is PlayerMobile) )
            	return;
        	
            PlayerMobile pm = from as PlayerMobile;
            Item weapon = pm.FindItemOnLayer( Layer.FirstValid ) as Item;
            Item shield = pm.FindItemOnLayer( Layer.TwoHanded ) as Item;

            if( this.RestrainedTarget != null && this.IsChildOf( pm.Backpack ) )
            {
                Mobile m = this.RestrainedTarget;
                m.Emote( "*was set free by {0}*", from.Name );
                
                if( ((IKhaerosMobile)m).StunnedTimer != null )
                {
	                ((IKhaerosMobile)m).StunnedTimer.Stop();
				    ((IKhaerosMobile)m).StunnedTimer = null;
                }
                
                if( m is PlayerMobile )
                {
                	m.Paralyzed = false;
                	Item item = m.Backpack.FindItemByType( typeof( Lasso ) );
                	
                	if( item != null )
                    	item.Delete();
                }
                
                ClearRopeInfo();
            }
            
            else if( pm.RopeTrick )
            	pm.SendMessage( 60, "You are already lassoing someone else." );
            
            else if( pm.Target != null )
            	pm.SendMessage( 60, "Cancel your current target first." );

            else if( (weapon == null && (shield == null || !(shield is BaseWeapon))) || shield == null )
                if( pm.Feats.GetFeatLevel(FeatList.RopeTrick) > 0 && this.IsChildOf( pm.Backpack ) )
                    new RopeTrickTimer( pm, ( 4 - pm.Feats.GetFeatLevel(FeatList.RopeTrick) ), this ).Start();

            else
                pm.SendMessage( 60, "You need a free hand in order to use this." );
        }

        public static bool CheckContainerForRope( Container cont )
        {
        	foreach( Item item in cont.Items )
    		{
        		if( item is Container && CheckContainerForRope( (Container)item ) )
        			return true;
        		
        		else if( CheckForRope( item ) )
        			return true;
    		}
        	
        	return false;
        }
        
        public static bool CheckForRope( Item item )
        {
        	if( item is Rope && ( (Rope)item ).RestrainedTarget != null )
        		return true;
        	
        	return false;
        }

        public class RopeTrickTimer : Timer
        {
            private PlayerMobile m_pm;
            private Rope m_Rope;

            public RopeTrickTimer(PlayerMobile pm, int featlevel, Rope rope)
                : base( TimeSpan.FromSeconds( featlevel ) )
            {
                m_pm = pm;
                m_Rope = rope;
                pm.RevealingAction();
                pm.Emote( "*starts swinging a lasso*" );
            }

            protected override void OnTick()
            {
                if( m_pm != null && !m_pm.Deleted && m_pm.Alive && m_Rope.RestrainedTarget == null )
                {
                	if( m_pm.Warmode && m_pm.Combatant != null && !m_pm.Combatant.Deleted && m_pm.InRange(m_pm.Combatant, (m_pm.Feats.GetFeatLevel(FeatList.RopeTrick) + 1)) )
                		RopeTrickTarget.FinishTarget( m_pm, m_pm.Combatant, m_Rope );
                	
                	else
                    	m_pm.Target = new RopeTrickTarget( m_pm, m_Rope );
                }
            }
        }

        public class ParalyzeTimer : Timer
        {
            private Rope m_Rope;
            private Mobile m_From;

            public ParalyzeTimer( Mobile from, Rope rope, double featlevel )
                : base( TimeSpan.FromSeconds( featlevel ) )
            {
                m_Rope = rope;
                m_From = from;
            }

            protected override void OnTick()
            {
            	if( m_Rope != null && !m_Rope.Deleted )
            		m_Rope.ClearRopeInfo();
            	
            	if( m_From != null && !m_From.Deleted )
            	{
                	m_From.Emote( "*has broken free from the lasso*" );
                	((IKhaerosMobile)m_From).StunnedTimer = null;
            	}
            }
        }

        private class RopeTrickTarget : Target
        {
            private Rope m_Rope;

            public RopeTrickTarget( Mobile m, Rope rope )
                : base( 8, false, TargetFlags.None )
            {
                m_Rope = rope;
                m.SendMessage( 60, "Choose a target for your rope trick." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	FinishTarget( m, obj, m_Rope );
            }
            
            public static void FinishTarget( Mobile m, object obj, Rope m_Rope )
            {
            	if( m == null || obj == null || m.Deleted )
            		return;
            	
                Mobile mob = obj as Mobile;
                PlayerMobile pm = m as PlayerMobile;
                int featlevel = pm.Feats.GetFeatLevel(FeatList.RopeTrick) + 1;

                if( obj is Mobile && obj != m && m_Rope.IsChildOf( pm.Backpack ) )
                {
                    if( BaseWeapon.CheckStam( pm, pm.Feats.GetFeatLevel(FeatList.RopeTrick), false, true ) )
                    {
                        if( pm.CanBeHarmful( mob ) && pm.InRange( mob, featlevel ) && !mob.Deleted && mob.Map == pm.Map && mob.Alive && pm.CanSee( mob ) && pm.InLOS( mob ) )
                        {
                            if( !mob.Paralyzed )
                            {
                                CheckLasso( pm, mob, m_Rope );
                                
                                if( mob is BaseCreature )
                                	((BaseCreature)mob).AggressiveAction( pm, false );
                            }

                            else
                                m.SendMessage( 60, "{0} is already restrained.", mob.Name );
                        }

                        else
                        {
                            pm.SendMessage( 60, "You cannot reach {0}.", mob.Name );
                            pm.Emote( "*stops swinging a lasso*" );
                        }
                    }

                    else
                        pm.Emote( "*stops swinging a lasso*" );
                }

                else
                    m.SendMessage( 60, "You cannot leash that." );
            }

            public static void CheckLasso( PlayerMobile pm, Mobile m, Rope rope )
            {
            	int featlevel = (10 * pm.Feats.GetFeatLevel(FeatList.RopeTrick)) + 30;
                int attackroll = Utility.RandomMinMax( 1, 100 );
                bool ressSick = (m is PlayerMobile && ((PlayerMobile)m).RessSick == true);

                if( featlevel >= attackroll || ressSick )
                {
                	if( !ressSick && ((IKhaerosMobile)m).Evaded() )
                	{
                	   m.Emote( "*evaded the attack*" );
                       return;
                    }

                    rope.RestrainedTarget = m;
                    m.Emote( "*has been lassoed by " + pm.Name + "*" );
                    rope.Movable = false;
                    rope.Name = "a rope restraining " + m.Name +"";

                    if( ressSick )
                    {
                        Container pack = m.Backpack;
                        m.SendMessage( 60, "You have been lassoed. When you are no longer under the effects of KO penalties, double-click on the lasso that has been placed inside your backpack to try to break free from it." );
                        Lasso lasso = new Lasso();
                        pack.DropItem( lasso );
                        lasso.RestrainedTarget = m;
                        lasso.Attacker = pm;
                        lasso.Rope = rope;
                        lasso.Movable = false;
                        m.Paralyzed = true;
                    }

                    else
                    {
                    	((IKhaerosMobile)m).StunnedTimer = new ParalyzeTimer( m, rope, (double)(pm.Feats.GetFeatLevel(FeatList.RopeTrick) * 1.5) );
                    	((IKhaerosMobile)m).StunnedTimer.Start();
                    }
                }

                else
                    m.Emote( "*avoided being lassoed by " + pm.Name + "*" );
            }
        }

        public Rope( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 3 );

            writer.Write( (Mobile) m_RestrainedTarget );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            m_RestrainedTarget = reader.ReadMobile();
        }
    }

    public class Lasso : Item
    {
        private Mobile m_RestrainedTarget;
        private Mobile m_Attacker;
        private Item m_Rope;
        private bool m_InUse;

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile RestrainedTarget
        {
            get { return m_RestrainedTarget; }
            set { m_RestrainedTarget = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile Attacker
        {
            get { return m_Attacker; }
            set { m_Attacker = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public Item Rope
        {
            get { return m_Rope; }
            set { m_Rope = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool InUse
        {
            get { return m_InUse; }
            set { m_InUse = value; }
        }

        [Constructable]
        public Lasso()
            : base( 0x14F8 )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "Lasso";
        }

        public override void OnDoubleClick( Mobile from )
        {
            PlayerMobile defender = from as PlayerMobile;
            Rope rope = m_Rope as Rope;

            if( from is PlayerMobile )
            {
            	if( defender.RessSick )
            		from.SendMessage( "You are too tired to do that." );
            	
            	else if( from.Paralyzed && !this.InUse && this.IsChildOf( defender.Backpack ) )
                	new LassoTimer( defender, this, rope ).Start();
            }
        }

        public class LassoTimer : Timer
        {
            private PlayerMobile m_defender;
            private Lasso m_lasso;
            private Rope m_rope;

            public LassoTimer( PlayerMobile defender, Lasso lasso, Rope rope )
                : base( TimeSpan.FromSeconds( 2 ) )
            {
                m_defender = defender;
                m_rope = rope;
                m_lasso = lasso;
                lasso.InUse = true;
                defender.Emote( "*tries to break free from the lasso*" );
            }

            protected override void OnTick()
            {
            	if( m_defender != null )
            	{
	                m_defender.Emote( "*has broken free from the lasso*" );
	                m_defender.Paralyzed = false;
            	}
                
                if( m_rope != null )
                	m_rope.ClearRopeInfo();
                
                if( m_lasso != null )
               		m_lasso.Delete();
            }
        }
        
        public override void OnDelete()
        {
        	if( this.Rope != null && !this.Rope.Deleted )
        		((Rope)this.Rope).ClearRopeInfo();
        	
        	if( this.RestrainedTarget != null && !this.RestrainedTarget.Deleted )
        		this.RestrainedTarget.Paralyzed = false;
        }

        public Lasso( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 3 );

            writer.Write( (Mobile) m_RestrainedTarget );
            writer.Write( (Mobile) m_Attacker );
            writer.Write( (Item) m_Rope );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_RestrainedTarget = reader.ReadMobile();
            m_Attacker = reader.ReadMobile();
            m_Rope = reader.ReadItem();
        }
    }
}
