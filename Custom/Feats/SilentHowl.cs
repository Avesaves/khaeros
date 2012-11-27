using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SilentHowl : BaseFeat
	{
		public override string Name{ get{ return "Silent Howl"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SilentHowl; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Learning the art of the predator means learning patience and planning, " +
					"which you use to great effect. [+3% Attack Chance, +3% Defense Chance, +3% Speed, +3% Damage]"; } }
        public override string SecondDescription { get { return "[+6% Attack Chance, +6% Defense Chance, +6% Speed, +6% Damage]"; } }
        public override string ThirdDescription { get { return "[+9% Attack Chance, +9% Defense Chance, +9% Speed, +9% Damage]"; } }

		public override string FirstCommand{ get{ return ".SilentHowl"; } }
		public override string SecondCommand{ get{ return ".SilentHowl"; } }
		public override string ThirdCommand{ get{ return ".SilentHowl"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SilentHowl()); }
		
		public SilentHowl() {}
	}
}
