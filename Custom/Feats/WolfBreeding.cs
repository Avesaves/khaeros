using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class WolfBreeding : BaseFeat
	{
		public override string Name{ get{ return "Wolf Breeding"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WolfBreeding; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DogBreeding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "The knowledge acquired from this skill will enable you to breed wolves."; } }
		public override string SecondDescription{ get{ return "This skill will grant +3 additional lives (initial and maximum) to the wolves you breed."; } }
		public override string ThirdDescription{ get{ return "This skill level will grant +3 additional lives (initial and maximum) to the wolves you breed."; } }

		public override string FirstCommand{ get{ return ".Mate"; } }
		public override string SecondCommand{ get{ return ".Mate"; } }
		public override string ThirdCommand{ get{ return ".Mate"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WolfBreeding()); }
		
		public WolfBreeding() {}
	}
}
