using System;
using Server.Items;
using Server.Mobiles;

namespace Server.ContextMenus
{
	public class OpenBankEntry : ContextMenuEntry
	{
		private Mobile m_Banker;

		public OpenBankEntry( Mobile from, Mobile banker ) : base( 6105, 12 )
		{
			m_Banker = banker;
		}

		public override void OnClick()
		{
			if ( !Owner.From.CheckAlive() )
				return;

			BankBox box = this.Owner.From.BankBox;
			PlayerMobile pm = this.Owner.From as PlayerMobile;
			Banker banker = m_Banker as Banker;
			
			if( this.Owner.From != null && m_Banker != null && this.Owner.From is PlayerMobile && m_Banker is Banker && pm.Nation != banker.Nation )
			{
				banker.Say( "You do not have an account here, foreigner. You can buy a storage box in the storage room if you want though." );
				return;
			}

			if ( box != null )
				box.Open();
		}
	}
}
