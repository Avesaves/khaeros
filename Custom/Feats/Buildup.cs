using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Buildup : BaseFeat
	{
		public override string Name{ get{ return "Buildup"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Buildup; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.SilentHowl, FeatList.SwipingClaws, FeatList.VenomousWay, 
				FeatList.SearingBreath, FeatList.TempestuousSea, FeatList.ThunderingHooves }; } }
		
		public override string FirstDescription{ get{ return "You have great insight into how to combine your attacks for maximum efficiency. " +
					"Each successful blow will decrease your unarmed attack connect time. These bonuses are nullified whenever your attack is " +
					"parried, or if 20 seconds elapse between attacks. [decreases attack connect time by 0.05s]"; } }
		public override string SecondDescription{ get{ return "[decreases attack connect time by 0.1s]"; } }
		public override string ThirdDescription{ get{ return "[decreases attack connect time by 0.15s]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Buildup()); }
		
		public Buildup() {}
	}
}
