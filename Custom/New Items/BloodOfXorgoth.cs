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
    public class BloodOfXorgoth : Item
    {
        private int m_Power;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

        [Constructable]
        public BloodOfXorgoth()
            : base( 0xE24 )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "Blood of Xorgoth";
            Hue = 2701;
        }

        public override void OnDoubleClick( Mobile from )
        {
        	if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed )
        		return;
        	
        	PlayerMobile pm = from as PlayerMobile;
        	
        	if( pm.BloodOfXorgoth != null )
        	{
        		from.SendMessage( "You are already under the effect of Blood of Xorgoth." );
        		return;
        	}

        	if( from.Backpack != null && this.ParentEntity == from.Backpack )
        	{
        		BasePotion.PlayDrinkEffectNoBottle( from );
        		from.Emote( "*lets out a roar after drinking the Blood of Xorgoth*" );
        		pm.BloodOfXorgoth = new BloodOfXorgothTimer( pm, this.Power );
        		pm.BloodOfXorgoth.Start();
        		this.Delete();
        		Pitcher pitcher = new Pitcher();
	        	from.Backpack.DropItem( pitcher );
        	}
        	
        	else
        		from.SendMessage( "That needs to be in your backpack for you to use it." );
        }
  
        public BloodOfXorgoth( Serial serial )
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
        
        public class BloodOfXorgothTimer : Timer
        {
            private PlayerMobile m;

            public BloodOfXorgothTimer( PlayerMobile from, int featLevel )
            	: base( TimeSpan.FromMinutes( featLevel ) )
            {
                m = from;
            }

            protected override void OnTick()
            {
            	if( m != null )
            		m.BloodOfXorgoth = null;
            }
        }
    }
}
