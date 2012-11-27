using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AdvancedLumberjacking : BaseFeat
	{
		public override string Name{ get{ return "Advanced Lumberjkacing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AdvancedLumberjacking; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Lumberjacking }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Greenheart }; } }
		
		public override string FirstDescription{ get{ return "You know the small tricks of lumberjacking that allow you to gather wood at a " +
					"faster rate. [Shortens the lumberjacking animation by 1 swing]"; } }
		public override string SecondDescription{ get{ return "[Shortens the lumberjacking animation by 2 swings]"; } }
		public override string ThirdDescription{ get{ return "[Shortens the lumberjacking animation by 3 swings]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AdvancedLumberjacking()); }
		
		public AdvancedLumberjacking() {}
	}
}
