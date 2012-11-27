using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ChainEffect : BaseFeat
	{
		public override string Name{ get{ return "Chain Effect"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ChainEffect; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Magery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to create custom spells effecting up to 2 additional " +
					"targets, with up to 5 squares in range between them, and with up to 25 chained damage."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to create custom spells effecting up to 4 additional " +
					"targets, with up to 10 squares in range between them, and with up to 50 chained damage."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to create custom spells effecting up to 6 additional " +
					"targets, with up to 15 squares in range between them, and with limitless chained damage."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ChainEffect()); }
		
		public ChainEffect() {}
	}
}
