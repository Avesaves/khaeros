using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AuraOfProtection : BaseFeat
	{
		public override string Name{ get{ return "Aura of Protection"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AuraOfProtection; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Sanctuary }; } }
		
		public override string FirstDescription{ get{ return "This spell provides a creature with physical protection for some time. [Adds 5 to " +
					"all physical protections, or 3 to self. Costs 15 mana and lasts for 16 minutes]"; } }
		public override string SecondDescription{ get{ return "[Adds 10 to all physical protections, or 6 to self. Costs 30 mana and lasts for 16 minutes]"; } }
		public override string ThirdDescription{ get{ return "[Adds 15 to all physical protections, or 9 to self. Costs 45 mana and lasts for 16 minutes]"; } }

		public override string FirstCommand{ get{ return ".AuraOfProtection | .AuraOfProtection 1"; } }
		public override string SecondCommand{ get{ return ".AuraOfProtection | .AuraOfProtection 2"; } }
		public override string ThirdCommand{ get{ return ".AuraOfProtection | .AuraOfProtection 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AuraOfProtection()); }
		
		public AuraOfProtection() {}
	}
}
