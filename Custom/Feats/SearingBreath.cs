using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SearingBreath : BaseFeat
	{
		public override string Name{ get{ return "Searing Breath"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SearingBreath; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Your attacks follow the teachings of the dragon spirit, lowering your defences " +
                    "but allowing you to dispatch your enemies with brutal, if slow, force. [+15% Damage, -10% Speed, -5% Defense Chance]"; } }
        public override string SecondDescription { get { return "[+30% Damage, -20% Speed, -10% Defense Chance]"; } }
        public override string ThirdDescription { get { return "[+45% Damage, -30% Speed, -15% Defense Chance]"; } }

		public override string FirstCommand{ get{ return ".SearingBreath"; } }
		public override string SecondCommand{ get{ return ".SearingBreath"; } }
		public override string ThirdCommand{ get{ return ".SearingBreath"; } }

		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SearingBreath()); }
		
		public SearingBreath() {}
	}
}
