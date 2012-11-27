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
using Server.Gumps;

namespace Server.Items
{
    public class TattooArtistTool : Item
    {
        [Constructable]
        public TattooArtistTool() : base( 0x102E )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "tattoo artist's tool";
            Hue = 1023;
        }

        public override void OnDoubleClick( Mobile from )
        {
        	if( from == null || !( from is PlayerMobile ) || from.Deleted || !from.Alive )
        		return;
        	
        	Container pack = from.Backpack;
        	
        	if( pack != null && this.ParentEntity == pack )
        	{
        		PlayerMobile m = from as PlayerMobile;
				if ( m.Feats.GetFeatLevel(FeatList.TattooArtist) > 0 )
				{
					m.Target = new TattooTarget( this );
					m.SendMessage( 60, "Choose a target." );
				}
				else
				{
					from.SendMessage( "You don't know how to use that." );
					return;
				}
        	}
        	
        	else
        		from.SendMessage( "That needs to be in your backpack for you to use it." );
        }

        private class TattooTarget : Target
        {
            private TattooArtistTool m_Tool;

            public TattooTarget( TattooArtistTool tool ) : base( 5, false, TargetFlags.None )
            {
                m_Tool = tool;
            }

            protected override void OnTarget( Mobile from, object obj )
            {
            	if( from == null )
        			return;
            	
            	Container pack = from.Backpack;
            	
				PlayerMobile targeted = obj as PlayerMobile;
            	if (targeted == null || targeted.Deleted)
					return;
            	
            	PlayerMobile m = from as PlayerMobile;
	        	
	        	if( pack != null && m_Tool.ParentEntity == pack )
	        	{
	        		m.SendGump( new TattooArtistGump(targeted, from as PlayerMobile) );
	        	}
	        	
	        	else
        			from.SendMessage( "That needs to be in your backpack for you to use it." );
            }
        }
  
        public TattooArtistTool( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
