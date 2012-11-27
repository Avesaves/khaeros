using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Teaching : BaseFeat
	{
		public override string Name{ get{ return "Teaching"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Teaching; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Professor }; } }
		
		public override string FirstDescription{ get{ return "This skill allows you to teach a character via RP. Every thirty seconds, " +
					"when you speak or emote, your students will receive some XP and CP. During the duration of the teaching session and for " +
					"thirty minutes after it is over, neither you nor your students will be able to get XP or CP through usual means. To stop " +
					"teaching, use the command to clear your student list. To start teaching, have your student use the command to accept " +
					"teaching, then add them to your student list via the appropriate command and start RPing teaching them. This skill level " +
					"allows you to teach one character under level 10 at a time."; } }
		public override string SecondDescription{ get{ return "This skill level allows you to teach up to two characters under level 15 at a time."; } }
		public override string ThirdDescription{ get{ return "This skill level allows you to teach up to three characters under level 20 at a time."; } }

		public override string FirstCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		public override string SecondCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		public override string ThirdCommand{ get{ return ".Student | .Teach | .ClearStudentList"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Teaching()); }
		
		public Teaching() {}
	}
}
