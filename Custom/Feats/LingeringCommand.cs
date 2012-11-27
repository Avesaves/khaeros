using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class LingeringCommand : BaseFeat
	{
		public override string Name{ get{ return "Lingering Command"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.LingeringCommand; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Leadership }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.WidespreadCommand, FeatList.LeadershipMastery }; } }
		
		public override string FirstDescription{ get{ return "This skill will extend the duration of all your commands. [Extra 10 seconds]"; } }
		public override string SecondDescription{ get{ return "[Extra 20 seconds]"; } }
		public override string ThirdDescription{ get{ return "[Extra 30 seconds]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new LingeringCommand()); }
		
		public LingeringCommand() {}
	}
}
