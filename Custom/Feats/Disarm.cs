using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Disarm : BaseFeat
	{
		public override string Name{ get{ return "Disarm"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Disarm; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Every time you interrupt your opponent's attack with your fists, you have a small " +
					"chance to disarm them. When successfully disarmed, there's a 50% chance that the weapon will end up back in the opponent's " +
					"pack, a 25% chance that it will end up on the ground, and a 25% chance that it will end up in your pack. [1% chance]"; } }
		public override string SecondDescription{ get{ return "[2% chance]"; } }
		public override string ThirdDescription{ get{ return "[3% chance]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Disarm()); }
		
		public Disarm() {}
	}
}
