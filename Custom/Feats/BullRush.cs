using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class BullRush : BaseFeat
	{
		public override string Name{ get{ return "Bull Rush"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.BullRush; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You let out a war cry and begin rushing in a straight line while whacking with " +
					"your weapon at anything that is in your path and sending them flying to the sides. This attack can only be performed on " +
					"foot. Once activated, you will have 2 seconds to run in a straight line. Any changes in direction will interrupt this " +
					"attack. The attack direction is always swing, and it can be parried. If it is parried, the defender takes no damage, " +
					"but is still pushed to the side. While rushing, you have mounted speed. Allies are not damaged, but are still flung to " +
					"the side. [Rushes for 2 tiles, 1 tile push]"; } }
		public override string SecondDescription{ get{ return "[Rushes for 4 tiles, 2 tile push]"; } }
		public override string ThirdDescription{ get{ return "[Rushes for 6 tiles, 3 tile push]"; } }
		
		public override string FirstCommand{ get{ return ".BullRush"; } }
		public override string SecondCommand{ get{ return ".BullRush"; } }
		public override string ThirdCommand{ get{ return ".BullRush"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new BullRush()); }
		
		public BullRush() {}
	}
}
