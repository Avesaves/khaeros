using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Mute : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Mute"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Mute; } }
		public override string Description{ get{ return "This flaw will render you unable to communicate via speech. Only " +
					"emotes will be available to you."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Mute()); }
		
		public Mute() {}
	}
}
