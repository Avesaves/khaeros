using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class LightArmour : BaseFeat
	{
		public override string Name{ get{ return "Light Armour"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.LightArmour; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ArmourFocus }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the player to wear light grade armour. [You can wear light " +
					"armours]"; } }
		public override string SecondDescription{ get{ return "[Reduces dexterity and stamina penalty per piece by 1]"; } }
		public override string ThirdDescription{ get{ return "[Reduces dexterity and stamina penalty per piece by 2]"; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new LightArmour()); }
		
		public LightArmour() {}
	}
}
