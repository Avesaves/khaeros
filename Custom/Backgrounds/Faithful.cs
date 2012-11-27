using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Faithful : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Faithful"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Faithful; } }
		public override string Description{ get{ return "This merit will enhance the effect of Faith spells cast upon you by " +
					"someone who worships the same deity as you do."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Faithful()); }
		
		public Faithful() {}
	}
}
