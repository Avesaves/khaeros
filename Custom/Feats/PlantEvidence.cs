using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PlantEvidence : BaseFeat
	{
		public override string Name{ get{ return "Plant Evidence"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PlantEvidence; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to attempt to drop things inside a player's backpack " +
					"while snooping. [33% chance]"; } }
		public override string SecondDescription{ get{ return "[66% chance]"; } }
		public override string ThirdDescription{ get{ return "[99% chance]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PlantEvidence()); }
		
		public PlantEvidence() {}
	}
}
