using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class OilMaking : BaseFeat
	{
		public override string Name{ get{ return "Oil Making"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.OilMaking; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Alchemy }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to craft Watery Oil via the Tinkering Menu."; } }
		public override string SecondDescription{ get{ return "This skill level will allow you to craft regular Oil."; } }
		public override string ThirdDescription{ get{ return "This skill level will allow you to craft Adhesive Oil."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new OilMaking()); }
		
		public OilMaking() {}
	}
}
