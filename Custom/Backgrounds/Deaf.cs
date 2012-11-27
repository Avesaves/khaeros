using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Deaf : BaseBackground
	{
		public override int Cost{ get{ return -2000; } }
		public override string Name{ get{ return "Deaf"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Deaf; } }
		public override string Description{ get{ return "This flaw will render you unable to hear when others speak. You will only " +
					"be able to see emotes."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Deaf()); }
		
		public Deaf() {}
	}
}
