using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AnimalTraining : BaseFeat
	{
		public override string Name{ get{ return "Animal Training"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AnimalTraining; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.AnimalTaming }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Breedable creatures you control will receive 50% more experience when hunting near you."; } }
		public override string SecondDescription{ get{ return "Increases the bonus to 100%."; } }
		public override string ThirdDescription{ get{ return "Increases the bonus to 150%."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AnimalTraining()); }
		
		public AnimalTraining() {}
	}
}
