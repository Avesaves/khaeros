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
			if( m.Nation == Nation.Alyrian )
				m.SpokenLanguage = KnownLanguage.Alyrian;
			else if( m.Nation == Nation.Azhuran )
				m.SpokenLanguage = KnownLanguage.Azhuran;
			else if( m.Nation == Nation.Khemetar )
				m.SpokenLanguage = KnownLanguage.Khemetar;
			else if( m.Nation == Nation.Mhordul )
				m.SpokenLanguage = KnownLanguage.Mhordul;				
			else if( m.Nation == Nation.Tyrean )
				m.SpokenLanguage = KnownLanguage.Tyrean;	
			else if( m.Nation == Nation.Vhalurian )
				m.SpokenLanguage = KnownLanguage.Vhalurian;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.IsXenophobe = false;
			m.SpokenLanguage = KnownLanguage.Common;
		}
		
		public Xenophobe() {}
	}
}*/
