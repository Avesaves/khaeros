using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PetStealing : BaseFeat
	{
		public override string Name{ get{ return "Pet Stealing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PetStealing; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to steal an animal that has been hitched to a post for " +
					"longer than 15 real life days."; } }
		public override string SecondDescription{ get{ return "Same as level 1, but the requirement now being that the animal has been hitched " +
					"for at least 10 real life days."; } }
		public override string ThirdDescription{ get{ return "Same as level 1, but the requirement now being that the animal has been hitched " +
					"for at least 5 real life days."; } }

		public override string FirstCommand{ get{ return ".PetStealing"; } }
		public override string SecondCommand{ get{ return ".PetStealing"; } }
		public override string ThirdCommand{ get{ return ".PetStealing"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PetStealing()); }
		
		public PetStealing() {}
	}
}
