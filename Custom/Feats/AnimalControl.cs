using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AnimalControl : BaseFeat
	{
		public override string Name{ get{ return "Animal Control"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AnimalControl; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will increase the maximum amount of followers you can have by 1."; } }
		public override string SecondDescription{ get{ return "Grants 1 additional follower slot."; } }
		public override string ThirdDescription{ get{ return "Grants 1 additional follower slot."; } }
	
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AnimalControl()); }
		
		public AnimalControl() {}
	}
}
