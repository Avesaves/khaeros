using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Cutpurse : BaseFeat
	{
		public override string Name{ get{ return "Cutpurse"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Cutpurse; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to steal a player's coinpurse without having to snoop " +
					"first. [Chance of being caught is (135 - 10 - Stealing)]>"; } }
		public override string SecondDescription{ get{ return "Increased chance of success. [Chance of being caught is (135 - 20 - Stealing)]"; } }
		public override string ThirdDescription{ get{ return "Increased chance of success. [Chance of being caught is (135 - 30 - Stealing)]"; } }

		public override string FirstCommand{ get{ return ".Cutpurse"; } }
		public override string SecondCommand{ get{ return ".Cutpurse"; } }
		public override string ThirdCommand{ get{ return ".Cutpurse"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Cutpurse()); }
		
		public Cutpurse() {}
	}
}
