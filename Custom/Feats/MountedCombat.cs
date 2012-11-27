using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedCombat : BaseFeat
	{
		public override string Name{ get{ return "Mounted Combat"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedCombat; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MountedCharge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Being well conditioned in mounted combat, war horses will now attack " +
                    "your enemies when you enter combat with them. Additionally, you take less time to recover from a charge, " +
					"allowing you to resume running speed faster. This only works while mounted. " +
					"[No running period after a charge is reduced by 1/8]"; } }
		public override string SecondDescription{ get{ return "War horses gain a bonus to their damage based on your mana. " + 
                    "[No running period after a charge is reduced by 1/4]"; } }
		public override string ThirdDescription{ get{ return "War horses gain a bonus to their damage based on your intelligence. " + 
                    "[No running period after a charge is reduced by 1/2]"; } }

		public override string FirstCommand{ get{ return ".Charge"; } }
		public override string SecondCommand{ get{ return ".Charge"; } }
		public override string ThirdCommand{ get{ return ".Charge"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MountedCombat()); }
		
		public MountedCombat() {}
	}
}
