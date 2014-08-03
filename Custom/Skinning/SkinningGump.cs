using System;
using Server;
using Server.Gumps;
using Server.Network;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Gumps
{
	public class SkinningGump : Gump
	{
		private BodyPartsContainer m_BPC;
		private Corpse m_Corpse;
		private Mobile m_Mobile;
		private Item[] m_Items;
		private SkinningTimer m_Timer;
		public SkinningGump(Corpse corpse, BodyPartsContainer bpc, Mobile mobile) : base( 0, 0 )
		{
			m_Corpse = corpse;
			m_Mobile = mobile;
			m_BPC = bpc;

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			
			this.AddBackground(38, 27, 327, 361, 9380);
			this.AddBackground(52, 57, 303, 304, 9270);
			this.AddImage(172, 344, 5576);
			this.AddLabel(174, 32, 2654, "Skinning");
			
			this.AddLabel(94, 72, 32, "Which part do you want to remove?");
			
			if ( !(m_Mobile is PlayerMobile) )
				return;
				
			PlayerMobile pm  = m_Mobile as PlayerMobile;
			int skinning = pm.Feats.GetFeatLevel(FeatList.Skinning);
			int i = 0;
			int j = 1;
			List<Item> items = new List<Item>();
			if (m_BPC != null && !m_BPC.Deleted)
			{
				foreach (Item item in m_BPC.Items)
				{
					if ( item == null || item.Deleted )
						continue;
						
					bool canCarve = false;
					
					if ( skinning >= 1 && item is CookableFood )
						canCarve = true;
					else if ( skinning >= 2 && ( item is Wool || item is BaseHides || item is Fur || item is Feather ) )
						canCarve = true;
					else if ( skinning >= 3 )
					{
						canCarve = true;
						/*if ( item is BaseToxinIngredient )
						{
							if ( pm.Skills[SkillName.Poisoning].Fixed < ((BaseToxinIngredient)item).SkillRequired )
								canCarve = false;
						} */  
						if ( item is Bone )
						{
							if ( pm.Feats.GetFeatLevel(FeatList.Bone) < 1 )
								canCarve = false;
						}
					}
					
					string displayedName = null;
					
					if ( item is BaseHides ) // handled differently
					{
						if ( item is Hides )
							displayedName = item.Amount > 1 ? item.Amount + " hides" : "hide";
						else if ( item is ThickHides )
							displayedName = item.Amount > 1 ? item.Amount + " thick hides" : "thick hide";
						else if	( item is BeastHides )
							displayedName = item.Amount > 1 ? item.Amount + " beast hides" : "beast hide";
						else if ( item is ScaledHides )
							displayedName = item.Amount > 1 ? item.Amount + " scaled hides" : "scaled hide";
					}
					else if (!String.IsNullOrEmpty(item.Name))
						displayedName = (item.Amount > 1 ? item.Amount + " " : "") + item.Name.ToLower();
						
					if ( !String.IsNullOrEmpty(displayedName) )
						this.AddLabel(100, 101 + i*20, canCarve ? 469 : 950, displayedName);
					else // these should never display (all skinnable items should have a name), but it is easier to spot them if they do with the different color
						this.AddHtmlLocalized( 100, 101 + i*20, 250, 18, item.LabelNumber, canCarve ? 0x777777 : 0x77FF77, false, false );
					if (canCarve)
					{
						this.AddButton(78, 104 + i*20, 5601, 5605, j, GumpButtonType.Reply, 0);
						items.Add(item);
						j++;
					}
					i++;
				}
			}
			m_Items = new Item[items.Count];
			i = 0;
			foreach (Item item in items)
				m_Items[i++] = item;
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			
			if ( m_Timer != null && m_Timer.Running ) {
				from.SendMessage( "You stop your skinning attempt." );
				m_Timer.Stop();
			}
			
			if ( info.ButtonID == 0 ) // close
				return;
			
			int index = info.ButtonID - 1;
			if (m_BPC == null || m_BPC.Deleted || index < 0 || index >= m_Items.Length || m_Items[index].Parent != m_BPC)
				return;
			
			Item desiredItem = m_Items[index];
			m_Timer = new SkinningTimer( from, m_Corpse, desiredItem, m_BPC );
			m_Timer.Start();
			
			from.SendMessage( "You begin skinning the corpse." );
			
			from.SendGump( this );
		}
		
		private class SkinningTimer : Timer
		{
			private Mobile m_From;
			private Corpse m_Corpse;
			private Item m_Item;
			private BodyPartsContainer m_BPC;
			
			public SkinningTimer( Mobile from, Corpse corpse, Item item, BodyPartsContainer bpc ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				Priority = TimerPriority.OneSecond;
				m_From = from;
				m_Corpse = corpse;
				m_Item = item;
				m_BPC = bpc;
			}
			
			protected override void OnTick()
			{
				PlayerMobile m = m_From as PlayerMobile;
				if ( m == null || m.Deleted || m_Item == null || m_Item.Deleted || m_Corpse == null || m_Corpse.Deleted )
				{
					Stop();
					m_From.CloseGump( typeof( SkinningGump ) );
					return;
				}
				
				if ( !m.InRange( m_Corpse.Location, 1 ) )
				{
					m_From.SendMessage( "You have moved too far away to be able to skin the corpse." );
					Stop();
					m_From.CloseGump( typeof( SkinningGump ) );
					return;
				}
				
				if ( m_Item.Parent is BodyPartsContainer )
				{
					m.Crafting = true;	
					LevelSystem.AwardMinimumXP( m, 1 );
					m.Crafting = false;
					
					// also increase some parts amount
					
					if( m_Item is Wool || m_Item is BaseHides || m_Item is Fur || m_Item is Feather )
						m_Item.Amount += m.Feats.GetFeatLevel(FeatList.ImprovedSkinning) * 2;
					
					else if( m_Item is Bone )
						m_Item.Amount += m.Feats.GetFeatLevel(FeatList.Bone) * 2;
					
					new Blood( 0x122D ).MoveToWorld( m_Corpse.Location, m_Corpse.Map );
					
					m_Corpse.DropItem( m_Item );
					
					m_From.SendMessage( "You carve away the part, it is now on the corpse." );
				}
				else
					m_From.SendMessage( "It appears someone carved it from the corpse before you could." );
				Stop();
				m_From.CloseGump( typeof( SkinningGump ) );
				if ( m_BPC != null && !m_BPC.Deleted && m_BPC.TotalItems > 0 )
					m_From.SendGump( new SkinningGump( m_Corpse, m_BPC, m_From ) );
			}
		}
	}
}