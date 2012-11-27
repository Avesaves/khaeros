using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Professor : BaseFeat
	{
		public override string Name{ get{ return "Professor"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Professor; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Teaching }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }

		public override string FirstDescription{ get{ return "You can now teach up to four characters at a time up until level 30."; } }
		public override string SecondDescription{ get{ return "You can now teach up to five characters at a time up until level 40."; } }
		public override string ThirdDescription{ get{ return "[You can now teach up to six characters at a time, of any level."; } }

		public override string FirstCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		public override string SecondCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		public override string ThirdCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Professor()); }
		
		public Professor() {}
	}
}
