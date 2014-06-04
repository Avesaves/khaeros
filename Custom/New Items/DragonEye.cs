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
    public class DragonEye : Item
    {
        private int m_Power;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

        [Constructable]
        public DragonEye()
            : base( 0x23E )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "A draconian eyeball";

        }

        public override void OnDoubleClick( Mobile from )
        {
        	if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed )
        		return;
        	
        	PlayerMobile pm = from as PlayerMobile;
        	

        	if( from.Backpack != null && this.ParentEntity == from.Backpack )
        	{

        		from.Emote( "*Eats a dragon's eye, with a squelch*" );

        		this.Delete();
                pm.WikiConfig = "dragon";
                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(Flare));
        	}
        	
        	else
        		from.SendMessage( "That needs to be in your backpack for you to use it." );
        }
        private void Flare( Mobile from )
        {

            PlayerMobile pm = from as PlayerMobile;
            pm.WikiConfig = null;
            pm.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*A light fades from their eyes...*");


        }
        public DragonEye( Serial serial )
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
