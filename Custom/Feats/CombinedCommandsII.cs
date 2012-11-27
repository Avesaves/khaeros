using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CombinedCommandsII : BaseFeat
	{
		public override string Name{ get{ return "Combined Commands II"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CombinedCommandsII; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.CombinedCommandsI }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Through this skill, you will become more adapt at merging the effects of your " +
					"commands. [each level reduces the cooldown to issue another command by 5 seconds (+5 seconds per level of lingering command)]"; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CombinedCommandsII()); }
		
		public CombinedCommandsII() {}
	}
}
