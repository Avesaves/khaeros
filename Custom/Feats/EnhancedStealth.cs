using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnhancedStealth : BaseFeat
	{
		public override string Name{ get{ return "Enhanced Stealth"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnhancedStealth; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealth }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ArmouredStealth, FeatList.Backstab }; } }
		
		public override string FirstDescription{ get{ return "This skill allows you to run while stealthing."; } }
		public override string SecondDescription{ get{ return "This improved level allows you to run for a longer time and with more ease " +
					"between every skill check."; } }
		public override string ThirdDescription{ get{ return "This improved level allows you to run for a longer time and with more ease " +
					"between every skill check."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnhancedStealth()); }
		
		public EnhancedStealth() {}
	}
}
