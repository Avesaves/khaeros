using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Commands;
using Server.Misc;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HolyWater : Item
    {
        private int m_Power;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

        [Constructable]
        public HolyWater()
            : base( 0xE24 )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "Holy Water";
            Hue = 2701;
        }

        public override void OnDoubleClick( Mobile from )
        {
        	if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed )
        		return;
        	
        	Container pack = from.Backpack;
        	
        	if( pack != null && this.ParentEntity == pack )
        	{
        		PlayerMobile m = from as PlayerMobile;
        		m.Target = new HolyWaterTarget( this );
        		m.SendMessage( 60, "Choose a target for your Holy Water." );
        	}
        	
        	else
        		from.SendMessage( "That needs to be in your backpack for you to use it." );
        }

        private class HolyWaterTarget : Target
        {
            private HolyWater water;

            public HolyWaterTarget( HolyWater holywater )
                : base( 5, false, TargetFlags.None )
            {
                water = holywater;
            }

            protected override void OnTarget( Mobile from, object obj )
            {
            	if( from == null || water == null || !( obj is BaseCreature || obj is BaseWeapon) || !( from is PlayerMobile ) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed )
        			return;
            	
            	Container pack = from.Backpack;
            	
            	if( obj is BaseWeapon )
            	{
            		BaseWeapon weapon = obj as BaseWeapon;
            		
            		if( weapon.RootParentEntity == null || weapon.RootParentEntity != from )
            			from.SendMessage( "The weapon needs to be in your possession to be consecrated." );
            		
            		else if( weapon.Resource == CraftResource.Silver )
            		{
            			XmlHolyWater hwatt = XmlAttach.FindAttachment( weapon, typeof(XmlHolyWater) ) as XmlHolyWater;
	            
			            if( hwatt != null )
			            	from.SendMessage( "That weapon has already been consecrated." );
			            
			            else if( pack != null && water.ParentEntity == pack )
			            {
			            	XmlHolyWater newatt = new XmlHolyWater( Math.Max( 1, (int)(water.Power * 0.1) ), 600.0 );
		            		XmlAttach.AttachTo( weapon, newatt );
		            		from.SendMessage( "This weapon will remain consecrated for the next ten minutes." );
		            		water.Delete();
		        			Pitcher pitcher = new Pitcher();
		        			weapon.InvalidateProperties();
		        			pack.DropItem( pitcher );
			            }
            		}
            		
            		else
            			from.SendMessage( "Only silver weapons can be consecrated with holy water." );
            		
            		return;
            	}
            	
            	if( !( obj is IAbyssal ) && !( obj is IUndead ) && (!(obj is PlayerMobile) && !((PlayerMobile)obj).IsVampire) )
            	{
            		from.SendMessage( "Holy Water is only effective against demons and the undead." );
            		return;
            	}
        	
            	BaseCreature bc = obj as BaseCreature;
            	
            	if( bc.Deleted || from.Map != bc.Map || !bc.Alive || !from.CanSee( bc ) || !from.InLOS( bc ) || !from.CanBeHarmful( bc ) )
            		return;
            	
            	PlayerMobile m = from as PlayerMobile;
	        	
	        	if( pack != null && water.ParentEntity == pack )
	        	{
	        		if( BaseWeapon.CheckStam( m, 6 ) )
	        		{
	        			new BaseCustomSpell.SpellDamageTimer( m, bc, 0.5, water.Power ).Start();
	        			from.Emote( "*throws a vial of holy water on " + bc.Name + "*" );
	        			Effects.SendLocationParticles( EffectItem.Create(   bc.Location, bc.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
	        			bc.PlaySound( 868 );
	        			water.Delete();
	        			Pitcher pitcher = new Pitcher();
	        			pack.DropItem( pitcher );
	        		}
	        	}
	        	
	        	else
        			from.SendMessage( "That needs to be in your backpack for you to use it." );
            }
        }
  
        public HolyWater( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 );
            
            writer.Write( (int) m_Power );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            
            m_Power = reader.ReadInt();
        }
    }
}
