using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Blunt : BaseFeat
    {
        public override string Name{ get{ return "Blunt"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Blunt; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ArmourSmithing, FeatList.Masterwork }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill increases your chance to add blunt resistance to armor at the cost of the others. [10% chance to add blunt resistance to armor] "; } }
        public override string SecondDescription { get { return "[20% chance]"; } }
        public override string ThirdDescription { get { return "[30% chance]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Blunt()); }
		
		public Blunt() {}
    }
}
