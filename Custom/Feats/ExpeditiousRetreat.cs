using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ExpeditiousRetreat : BaseFeat
	{
		public override string Name{ get{ return "Expeditious Retreat"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ExpeditiousRetreat; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Leadership }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to inspire your allies to expedite their retreat. " +
					"[as the name suggests, this effect does not work in combat, only when running from danger]"; } }
		public override string SecondDescription{ get{ return "Reduced stamina cost."; } }
		public override string ThirdDescription{ get{ return "Reduced stamina cost."; } }

		public override string FirstCommand{ get{ return ".ExpeditiousRetreat | .CancelCommand"; } }
		public override string SecondCommand{ get{ return ".ExpeditiousRetreat | .CancelCommand"; } }
		public override string ThirdCommand{ get{ return ".ExpeditiousRetreat | .CancelCommand"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ExpeditiousRetreat()); }
		
		public ExpeditiousRetreat() {}
	}
}
