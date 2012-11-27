using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ArmouredStealth : BaseFeat
	{
		public override string Name{ get{ return "Armoured Stealth"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ArmouredStealth; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.EnhancedStealth }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to stealth while wearing light or medium armour, as " +
					"long as the total penalty from the pieces you are wearing do not exceed 2."; } }
		public override string SecondDescription{ get{ return "Increases the penalty tolerance to 5."; } }
		public override string ThirdDescription{ get{ return "Increases the penalty tolerance to 8."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ArmouredStealth()); }
		
		public ArmouredStealth() {}
	}
}
