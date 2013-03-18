using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Alertness : BaseFeat
	{
		public override string Name{ get{ return "Alertness"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Alertness; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DetectHidden }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Your senses are keener than normal, allow you to sense and spot hidden people " +
					"from a greater distance, and even flanking or behind you. [2 square radius]"; } }
		public override string SecondDescription{ get{ return "[4 square radius]"; } }
		public override string ThirdDescription{ get{ return "[6 square radius]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Alertness()); }
		
		public Alertness() {}
	}
}
