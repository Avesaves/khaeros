using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HeavyLifting : BaseFeat
	{
		public override string Name{ get{ return "Heavy Lifting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HeavyLifting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Anatomy }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Having spent an absurd amount of time moving resources around you have become a " +
					"true packrat. [Adds 50 petrae to your max carrying capacity]"; } }
		public override string SecondDescription{ get{ return "[Adds 100 petrae to your max carrying capacity]"; } }
		public override string ThirdDescription{ get{ return "[Adds 150 petrae to your max carrying capacity]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HeavyLifting()); }
		
		public HeavyLifting() {}
	}
}
