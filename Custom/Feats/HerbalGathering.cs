using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HerbalGathering : BaseFeat
	{
		public override string Name{ get{ return "Herbal Gathering"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HerbalGathering; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.HerbalLore }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Pusantia }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to extract more herbs whenever you harvest them. " +
					"[+1 more herb]"; } }
		public override string SecondDescription{ get{ return "[+2 more herbs]"; } }
		public override string ThirdDescription{ get{ return "[+3 more herbs]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HerbalGathering()); }
		
		public HerbalGathering() {}
	}
}
