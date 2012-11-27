using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class WidespreadCommand : BaseFeat
	{
		public override string Name{ get{ return "Widespread Command"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WidespreadCommand; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.LingeringCommand }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.CombinedCommandsI }; } }
		
		public override string FirstDescription{ get{ return "This skill will expand the area of effect of all your commands. [+2 range]"; } }
		public override string SecondDescription{ get{ return "[+4 range]"; } }
		public override string ThirdDescription{ get{ return "[+6 range]"; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WidespreadCommand()); }
		
		public WidespreadCommand() {}
	}
}
