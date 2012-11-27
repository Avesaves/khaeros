using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ArmourEnameling : BaseFeat
	{
		public override string Name{ get{ return "Armour Enameling"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ArmourEnameling; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.RacialEnameling }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Blacksmithing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.RacialEnameling }; } }
		
		public override string FirstDescription{ get{ return "You know how to mix hues in to metal during the process of crafting items. [10 hues]"; } }
		public override string SecondDescription{ get{ return "[20 hues]"; } }
		public override string ThirdDescription{ get{ return "[30 hues]"; } }

		public override string FirstCommand{ get{ return ".Enamel"; } }
		public override string SecondCommand{ get{ return ".Enamel"; } }
		public override string ThirdCommand{ get{ return ".Enamel"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ArmourEnameling()); }
		
		public ArmourEnameling() {}
	}
}
