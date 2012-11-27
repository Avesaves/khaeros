using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RetrainPet : BaseFeat
	{
		public override string Name{ get{ return "Retrain Pet"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RetrainPet; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PetFeats }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "After taking this skill, you will be able to reassign the points of one of your " +
					"creature's skills into other skills (only works on creatures bred after this skill was taken)."; } }
		public override string SecondDescription{ get{ return "Increases the amount of reassignable skills to 2."; } }
		public override string ThirdDescription{ get{ return "Increases the amount of reassignable skills to 3."; } }

		public override string FirstCommand{ get{ return ".RetrainPet"; } }
		public override string SecondCommand{ get{ return ".RetrainPet"; } }
		public override string ThirdCommand{ get{ return ".RetrainPet"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RetrainPet()); }
		
		public RetrainPet() {}
	}
}
