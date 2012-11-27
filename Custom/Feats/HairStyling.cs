using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HairStyling : BaseFeat
	{
		public override string Name{ get{ return "Hair Styling"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HairStyling; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tailoring }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the character to style haircuts."; } }
		public override string SecondDescription{ get{ return "This skill allows the character to style facial hair cuts."; } }
		public override string ThirdDescription{ get{ return "This skill allows the character to dye hair and facial hair."; } }

		public override string FirstCommand{ get{ return ".HairStyling"; } }
		public override string SecondCommand{ get{ return ".HairStyling"; } }
		public override string ThirdCommand{ get{ return ".HairStyling"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HairStyling()); }
		
		public HairStyling() {}
	}
}
