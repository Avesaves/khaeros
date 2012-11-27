using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class TripFoe : BaseFeat
	{
		public override string Name{ get{ return "Trip Foe"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.TripFoe; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PolearmsMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Using the polearm's size to your advantage, you perform a trip attack with the " +
					"blunt end of the weapon. A tripped opponent lays on the ground for 2/4/6 seconds, and cannot attack during that time, " +
					"but they can still parry blows. Mounted attackers will have a chance to be dismounted instead. This can only be performed " +
					"while on foot. [Opponent remains on the ground for 2 seconds]"; } }
		public override string SecondDescription{ get{ return "[Opponent remains on the ground for 4 seconds]"; } }
		public override string ThirdDescription{ get{ return "[Opponent remains on the ground for 6 seconds]"; } }

		public override string FirstCommand{ get{ return ".TripFoe"; } }
		public override string SecondCommand{ get{ return ".TripFoe"; } }
		public override string ThirdCommand{ get{ return ".TripFoe"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new TripFoe()); }
		
		public TripFoe() {}
	}
}
