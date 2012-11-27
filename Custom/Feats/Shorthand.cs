using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Shorthand : BaseFeat
	{
		public override string Name{ get{ return "Shorthand"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Shorthand; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Linguistics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With the knowledge acquired via this skill, you will be able to communicate with " +
					"others that share this same knowledge via discreet hand gestures. [4 tiles]"; } }
		public override string SecondDescription{ get{ return "[8 tiles]"; } }
		public override string ThirdDescription{ get{ return "[12 tiles]"; } }

		public override string FirstCommand{ get{ return ".SpeakLanguage Shorthand"; } }
		public override string SecondCommand{ get{ return ".SpeakLanguage Shorthand"; } }
		public override string ThirdCommand{ get{ return ".SpeakLanguage Shorthand"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Shorthand()); }
		
		public Shorthand() {}
	}
}
