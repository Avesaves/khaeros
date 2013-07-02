using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Masterwork : BaseFeat
	{
		public override string Name{ get{ return "Masterwork"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Masterwork; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DurableCrafts }; } }
        public override FeatList[] Allows { get { return new FeatList[] { FeatList.RenownedMasterwork }; } }
		
		public override string FirstDescription{ get{ return "Having spent years honing your skills you are able to craft items of extraordinary " +
					"quality. [0.33% chance of an extraordinary item when you would craft an exceptional item. 3 random bonuses]"; } }
		public override string SecondDescription{ get{ return "[0.66% chance of an extraordinary item when you would craft an exceptional item. " +
					"3 random bonuses]"; } }
		public override string ThirdDescription{ get{ return "[1% chance of an extraordinary and a 0.2% chance of a masterwork item when you " +
					"would craft an exceptional item. 3 random bonuses]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Masterwork()); }
		
		public Masterwork() {}
	}
}
