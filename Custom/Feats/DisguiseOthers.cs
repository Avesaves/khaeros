using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DisguiseOthers : BaseFeat
	{
		public override string Name{ get{ return "Disguise Others"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DisguiseOthers; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DisguiseKit }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to make use of the same abilities granted by " +
					"Disguise Kit 1, but on others."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to make use of the same abilities granted by " +
					"Disguise Kit 2, but on others."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to make use of the same abilities granted by " +
					"Disguise Kit 3, but on others."; } }

		public override string FirstCommand{ get{ return ".Disguise | .NoDisguise | .DisguiseName"; } }
		public override string SecondCommand{ get{ return ".Disguise | .NoDisguise | .DisguiseName | .DisguiseAge | .DisguiseLooks"; } }
		public override string ThirdCommand{ get{ return ".Disguise | .NoDisguise | .DisguiseName | .DisguiseAge | .DisguiseLooks | " +
					".DisguiseTitle | .DisguisePrefix"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DisguiseOthers()); }
		
		public DisguiseOthers() {}
	}
}
