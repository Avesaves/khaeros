using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class BackToBack : BaseFeat
	{
		public override string Name{ get{ return "Back to Back"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.BackToBack; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Having trained extensively in foot group combat you are better prepared " +
					"to defend yourself when fighting with a comrade at your side. [When standing no more than one tile apart from an " +
					"ally who also has this skill you gain +10% Defence Chance Increase, and cannot receive attacks of opportunity from behind.]"; } }
		public override string SecondDescription{ get{ return "[+20% Defence Chance Increase]"; } }
		public override string ThirdDescription{ get{ return "[+30% Defence Chance Increase]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new BackToBack()); }
		
		public BackToBack() {}
	}
}
