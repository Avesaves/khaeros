using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class AnimalEmpathy : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Animal Empathy"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.AnimalEmpathy; } }
		public override string Description{ get{ return "This merit will allow you to mingle with non-monstrous animals without " +
					"ever getting attacked by them."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AnimalEmpathy()); }
		
		public AnimalEmpathy() {}
	}
}
