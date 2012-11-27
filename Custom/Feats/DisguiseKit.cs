using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DisguiseKit : BaseFeat
	{
		public override string Name{ get{ return "Disguise Kit"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DisguiseKit; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealth }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DisguiseOthers }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to change your name, as well as your hair and facial " +
					"hair styles via the use of wigs."; } }
		public override string SecondDescription{ get{ return "With this skill level, you will also be able to use special powders and dyes to " +
					"change your hair and skin hue."; } }
		public override string ThirdDescription{ get{ return "With this final skill level, you will be able to disguise yourself to look like a " +
					"member of another race. Additionally, you will be able to change your title and title prefix."; } }

		public override string FirstCommand{ get{ return ".Disguise | .NoDisguise | .DisguiseName | .DisguiseProfile | .SaveDisguise | .LoadDisguise"; } }
        public override string SecondCommand { get { return ".Disguise | .NoDisguise | .DisguiseName | .DisguiseAge | .DisguiseLooks | .DisguiseProfile | .SaveDisguise | .LoadDisguise"; } }
		public override string ThirdCommand{ get{ return ".Disguise | .NoDisguise | .DisguiseName | .DisguiseAge | .DisguiseLooks | " +
					".DisguiseTitle | .DisguisePrefix | .DisguiseProfile | .SaveDisguise | .LoadDisguise"; }
        }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DisguiseKit()); }
		
		public DisguiseKit() {}
	}
}
