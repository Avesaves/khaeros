using System;
using Server;
using Server.Gumps;
using System.Collections.Generic;
using Server.Network;

namespace Server.Gumps
{
	public class ChangeLayerGump : Gump
	{
		private Item m_Item;
		public static KeyValuePair<Layer, string>[] LayerArray = { 
			new KeyValuePair<Layer, string>(Layer.Helm, "Head"),
			new KeyValuePair<Layer, string>(Layer.Earrings, "Ear"),
			new KeyValuePair<Layer, string>(Layer.Neck, "Neck"),
			new KeyValuePair<Layer, string>(Layer.Shirt, "Shirt"),
			new KeyValuePair<Layer, string>(Layer.InnerTorso, "Inner Torso"), // armor layer
			new KeyValuePair<Layer, string>(Layer.MiddleTorso, "Middle Torso"),
			new KeyValuePair<Layer, string>(Layer.OuterTorso, "Outer Torso"),
			new KeyValuePair<Layer, string>(Layer.Cloak, "Cloak"),
			new KeyValuePair<Layer, string>(Layer.Arms, "Arms"),
			new KeyValuePair<Layer, string>(Layer.Gloves, "Hands"),
			new KeyValuePair<Layer, string>(Layer.Bracelet, "Wrist"),
			new KeyValuePair<Layer, string>(Layer.Ring, "Finger"),
			new KeyValuePair<Layer, string>(Layer.Waist, "Waist"),
			new KeyValuePair<Layer, string>(Layer.Pants, "Pants"),
			//new KeyValuePair<Layer, string>(Layer.InnerLegs, "Inner Legs"),
			new KeyValuePair<Layer, string>(Layer.OuterLegs, "Outer Legs"),
			new KeyValuePair<Layer, string>(Layer.Shoes, "Shoes")
		};
		
		
		public ChangeLayerGump( Item item ) : base( 0, 0 )
		{
			m_Item = item;
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			this.AddBackground(89, 86, 178, 371, 5120);
			this.AddLabel(107, 88, 2931, "Change to which layer?");
			
			for (int i=0; i < LayerArray.Length; i++)
			{
				int hue = 1158;
				if ( LayerArray[i].Key == m_Item.Layer )
					hue = 337;
				this.AddLabel(132, 110 + 20*i, hue, LayerArray[i].Value);
				this.AddButton(116, 115 + 20*i, 2103, 2104, 10+i, GumpButtonType.Reply, 0);
			}
			
			this.AddImage(89, 39, 10452);
			this.AddImage(88, 445, 10452);
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if ( info.ButtonID == 0 ) // close
				return;
			else
			{
				if ( !m_Item.IsChildOf( from ) || m_Item.Parent == from )
					return;
				int index = info.ButtonID - 10;
				if ( index < 0 || index >= LayerArray.Length )
					return;
				m_Item.Layer = LayerArray[index].Key;
				m_Item.InvalidateProperties();
				from.SendMessage( "You change the item's layer." );
				return;
			}
		}
	}
}