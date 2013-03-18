using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PoisonResistance : BaseFeat
	{
		public override string Name{ get{ return "Poison Resistance"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PoisonResistance; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Anatomy, FeatList.Alchemy }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Poisoning }; } }
		
		public override string FirstDescription{ get{ return "The assassin, having imbibed small doses of poison over many years of practice, " +
					"develops a natural resistance to their effects. [+20% poison resistance]"; } }
		public override string SecondDescription{ get{ return "[+40% poison resistance]"; } }
		public override string ThirdDescription{ get{ return "[+60% poison resistance]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PoisonResistance()); }
		
		public PoisonResistance() {}
	}
}
