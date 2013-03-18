using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CureFamine : BaseFeat
	{
		public override string Name{ get{ return "Cure Famine"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CureFamine; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Magery, FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This spell allows you to quench a creature's thirst. [Replenishes your " +
					"thirst, costs 5 mana]"; } }
		public override string SecondDescription{ get{ return "[Replenishes your hunger, costs 5 mana]"; } }
		public override string ThirdDescription{ get{ return "[Replenishes thirst and hunger of all allies within five squares, costs 20 mana]"; } }

		public override string FirstCommand{ get{ return ".CureFamine | .CureFamine 1"; } }
		public override string SecondCommand{ get{ return ".CureFamine | .CureFamine 2"; } }
		public override string ThirdCommand{ get{ return ".CureFamine | .CureFamine 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CureFamine()); }
		
		public CureFamine() {}
	}
}
