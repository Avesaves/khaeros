using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Shipwright : BaseFeat
	{
		public override string Name{ get{ return "Shipwright"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Shipwright; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Carpentry }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You have learned how to build seaworthy vessels of small scale."; } }
		public override string SecondDescription{ get{ return "You have learned how to build seaworthy vessels of medium scale."; } }
		public override string ThirdDescription{ get{ return "You have learned how to build seaworthy vessels of large scale."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Shipwright()); }
		
		public Shipwright() {}
	}
}
