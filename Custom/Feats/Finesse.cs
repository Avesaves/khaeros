using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Finesse : BaseFeat
	{
		public override string Name{ get{ return "Finesse"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Finesse; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ThrowingMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "The assassin can hurl light weapons, such as daggers, with unerring accuracy, " +
					"greatly increasing the chance of a critical hit. [Chance for a critical hit is (60% * Feat Level) / (Weight * Weight)]"; } }
		public override string SecondDescription{ get{ return "Improved chance."; } }
		public override string ThirdDescription{ get{ return "Improved chance."; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Finesse()); }
		
		public Finesse() {}
	}
}
