using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ThunderingHooves : BaseFeat
	{
		public override string Name{ get{ return "Thundering Hooves"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ThunderingHooves; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Closing your defences, your fists become like a warhorse's hooves. " +
					"[+15% Defense Chance, -10% Damage -5% Attack Chance]"; } }
        public override string SecondDescription { get { return "[+30% Defense Chance, -20% Damage, -10% Attack Chance]"; } }
        public override string ThirdDescription { get { return "[+45% Defense Chance, -30% Damage, -15% Attack Chance]"; } }

		public override string FirstCommand{ get{ return ".ThunderingHooves"; } }
		public override string SecondCommand{ get{ return ".ThunderingHooves"; } }
		public override string ThirdCommand{ get{ return ".ThunderingHooves"; } }

		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ThunderingHooves()); }
		
		public ThunderingHooves() {}
	}
}
