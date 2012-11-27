using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class AllowGhoulingGump : Gump
	{
		private Mobile m_Vamp;
		private Mobile m_Victim;

		public AllowGhoulingGump( Mobile vamp, Mobile victim ) : base( 50, 50 )
		{
			m_Vamp = vamp;
			m_Victim = victim;

			AddPage( 0 );

			AddBackground( 0, 0, 270, 120, 5054 );
			AddBackground( 10, 10, 250, 100, 3000 );

			if( vamp != null )
				AddHtml( 20, 15, 230, 60, "Will you accept to drink the blood " + vamp.Name + " is offering you?", true, true );
			
			AddButton( 20, 80, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 55, 80, 75, 20, 1011011, false, false ); // CONTINUE

			AddButton( 135, 80, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 170, 80, 75, 20, 1011012, false, false ); // CANCEL
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 2 )
			{
				if( m_Vamp == null || m_Victim == null || !(m_Vamp is PlayerMobile) || !(m_Victim is PlayerMobile) )
					return;
				
				PlayerMobile vamp = (PlayerMobile)m_Vamp;
				PlayerMobile victim = (PlayerMobile)m_Victim;
				
				if( !vamp.Alive || !victim.Alive || vamp.Paralyzed || victim.Paralyzed || victim.IsVampire || !vamp.InRange(victim, 1) || vamp.BPs < 1 )
					return;
				
				vamp.BPs--;
				victim.LastTimeGhouled = DateTime.Now;
				victim.HandleGhoulStatOffsets();
				victim.PlaySound( 1228 );
				victim.Emote( "*drinks the blood " + vamp.Name + " has offered " + victim.GetReflexivePronoun() + "*" );
			}
		}
	}
}
