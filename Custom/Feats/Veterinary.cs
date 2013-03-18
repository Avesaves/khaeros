using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Veterinary : BaseFeat
	{
		public override string Name{ get{ return "Veterinary"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Veterinary; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Veterinary, SkillName.AnimalHusbandry }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.AnimalLore }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DogBreeding, FeatList.HorseBreeding, FeatList.PetFeats,
		FeatList.Beartalker, FeatList.Snakecharmer, FeatList.AvianBreeding }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Veterinary skill, which will " +
					"allow you allow you to apply bandages to your pets with greater efficiency. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Veterinary()); }
		
		public Veterinary() {}
	}
}
