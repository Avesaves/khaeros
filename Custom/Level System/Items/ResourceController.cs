using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Spells;

namespace Server.Items
{
	public enum ControlledResource
	{
		None,
		Copper,
		Tin,
		Iron,
		Obsidian,
		Silver,
		Gold,
		Citrine,
		Tourmaline,
		Sapphire,
		Amethyst,
		Emerald,
		Ruby,
		StarSapphire,
		Diamond,
		Oak,
		Yew,
		Redwood,
		Ash,
		Greenheart,
		Amber,
        Coal,
        Cinnabar
	}
	
	public enum VeinIntensity
	{
		Low,
		Average,
		Full
	}
	
	public class ResourceController : BaseTrap
	{
		private int m_Range;
		private VeinIntensity m_Intensity;
		private ControlledResource m_ControlledResource;
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int Range
        {
            get { return m_Range; }
            set { m_Range = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public VeinIntensity Intensity
        {
            get { return m_Intensity; }
            set { m_Intensity = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public ControlledResource ControlledResource
        {
            get { return m_ControlledResource; }
            set { m_ControlledResource = value; }
        }
		
		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return m_Range; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromSeconds( 10 ); } }

		public override void OnTrigger( Mobile from )
		{
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				m.UniqueSpot = this;
			}
		}
		
		[Constructable]
		public ResourceController() : base( 0xDDA )
		{
            Name = "Resource Controller";
            Movable = false;
            Visible = false;
            Range = 1;
            Intensity = VeinIntensity.Low;
		}

		public ResourceController( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
            base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			writer.Write( (int) m_Range );
			writer.Write( (int) m_Intensity );
			writer.Write( (int) m_ControlledResource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Range = reader.ReadInt();
			m_Intensity = (VeinIntensity)reader.ReadInt();
			m_ControlledResource = (ControlledResource)reader.ReadInt();
		}
	}
}
