using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DurableCrafts : BaseFeat
	{
		public override string Name{ get{ return "Durable Crafts"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DurableCrafts; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Masterwork }; } }
		
		public override string FirstDescription{ get{ return "You know how to better prepare the materials you work with, thus granting your " +
					"craft a higher durability. [+20% Durability]"; } }
		public override string SecondDescription{ get{ return "[+40% Durability]"; } }
		public override string ThirdDescription{ get{ return "[+60% Durability]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DurableCrafts()); }
		
		public DurableCrafts() {}
	}
}
