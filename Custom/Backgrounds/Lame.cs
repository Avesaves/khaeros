using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Lame : BaseBackground
	{
		public override int Cost{ get{ return -3000; } }
		public override string Name{ get{ return "Lame"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Lame; } }
		public override string Description{ get{ return "This flaw will render you unable to run on foot. You will still be able to run " +
					"on horseback, however."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Lame()); }
		
		public Lame() {}
	}
}
