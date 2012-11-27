using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps
{
	public class AllowFeedingGump : Gump
	{
		private Mobile m_Vamp;
		private Mobile m_Victim;

		public AllowFeedingGump( Mobile vamp, Mobile victim ) : base( 50, 50 )
		{
			m_Vamp = vamp;
			m_Victim = victim;

			AddPage( 0 );

			AddBackground( 0, 0, 270, 120, 5054 );
			AddBackground( 10, 10, 250, 100, 3000 );

			if( vamp != null )
				AddHtml( 20, 15, 230, 60, "Will you allow " + vamp.Name + " to drain your blood?", true, true );
			
			AddButton( 20, 80, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 55, 80, 75, 20, 1011011, false, false ); // CONTINUE

			AddButton( 135, 80, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 170, 80, 75, 20, 1011012, false, false ); // CANCEL
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 2 )
			{
				if ( m_Victim != null && !m_Victim.Blessed && m_Vamp != null && m_Vamp.InRange(m_Victim, 1) && m_Victim.Alive && 
				    m_Vamp.Alive && !m_Vamp.Paralyzed && m_Victim is PlayerMobile && !((PlayerMobile)m_Victim).IsVampire )
				{
					m_Vamp.Emote( "*feeds on {0}*", m_Victim.Name );
					m_Vamp.PlaySound( 49 );
					m_Victim.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Head );
					m_Victim.Damage( 10 );
					((PlayerMobile)m_Vamp).BPs += 2;
					((PlayerMobile)m_Vamp).NextFeedingAllowed = DateTime.Now + TimeSpan.FromSeconds( 5 );
                    foreach (DiseaseTimer timer in HealthAttachment.GetHA(m_Victim).CurrentDiseases)
                    {
                        if (!HealthAttachment.GetHA(m_Vamp).HasDisease(timer.Disease))
                        {
                            DiseaseTimer newDis = new DiseaseTimer(m_Vamp, timer.Disease);
                            HealthAttachment.GetHA(m_Vamp).CurrentDiseases.Add(newDis);
                            newDis.Start();
                        }
                    }
				}
			}
		}
	}
}
