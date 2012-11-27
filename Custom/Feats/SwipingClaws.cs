using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SwipingClaws : BaseFeat
	{
		public override string Name{ get{ return "Swiping Claws"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SwipingClaws; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You have learned how to move like the jaguar, attacking blindingly fast without " +
					"regard for tactics or strategy. [+15% Speed, -10% Attack Chance, -5% Defense Chance]"; } }
		public override string SecondDescription{ get{ return "[+30% Speed, -20% Attack Chance, -10% Defense Chance]"; } }
		public override string ThirdDescription{ get{ return "[+45% Speed, -30% Attack Chance, -15% Defense Chance]"; } }

		public override string FirstCommand{ get{ return ".SwipingClaws"; } }
		public override string SecondCommand{ get{ return ".SwipingClaws"; } }
		public override string ThirdCommand{ get{ return ".SwipingClaws"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SwipingClaws()); }
		
		public SwipingClaws() {}
	}
}
