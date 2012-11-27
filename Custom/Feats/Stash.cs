using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Stash : BaseFeat
	{
		public override string Name{ get{ return "Stash"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Stash; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Hiding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to keep one secret stash that will last for up to 5 real life days."; } }
		public override string SecondDescription{ get{ return "Increases the stash's duration to 10 days."; } }
		public override string ThirdDescription{ get{ return "Increases the stash's duration to 15 days."; } }

		public override string FirstCommand{ get{ return ".Stash"; } }
		public override string SecondCommand{ get{ return ".Stash"; } }
		public override string ThirdCommand{ get{ return ".Stash"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Stash()); }
		
		public Stash() {}
	}
}
