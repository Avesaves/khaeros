/*using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Xenophobe : BaseBackground
	{
		public override int Cost{ get{ return -3000; } }
		public override string Name{ get{ return "Xenophobe"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Xenophobe; } }
		public override string Description{ get{ return "Whether you grew up amongst isolationists or just "+
		"under a rock, you have no knowledge of the Common tongue. [Cannot speak or understand Common until acquisition of Level 3 Linguistics.]"; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Xenophobe()); }

		public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.IsXenophobe = true;
			if( m.Nation == Nation.Southern )
				m.SpokenLanguage = KnownLanguage.Southern;
			else if( m.Nation == Nation.Western )
				m.SpokenLanguage = KnownLanguage.Western;
			else if( m.Nation == Nation.Haluaroc )
				m.SpokenLanguage = KnownLanguage.Haluaroc;
			else if( m.Nation == Nation.Mhordul )
				m.SpokenLanguage = KnownLanguage.Mhordul;				
			else if( m.Nation == Nation.Tirebladd )
				m.SpokenLanguage = KnownLanguage.Tirebladd;	
			else if( m.Nation == Nation.Northern )
				m.SpokenLanguage = KnownLanguage.Northern;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.IsXenophobe = false;
			m.SpokenLanguage = KnownLanguage.Common;
		}
		
		public Xenophobe() {}
	}
}*/
