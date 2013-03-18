using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PetFeats : BaseFeat
	{
		public override string Name{ get{ return "Pet Feats"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PetFeats; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Veterinary }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.PetEvolution, FeatList.RetrainPet, FeatList.ExtraPetFeats }; } }
		
		public override string FirstDescription{ get{ return "With this skill, the creatures you breed will be randomly assigned 3 skill points."; } }
		public override string SecondDescription{ get{ return "The number of skill points increases to 6."; } }
		public override string ThirdDescription{ get{ return "The number of skill points increases to 9."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PetFeats()); }
		
		public PetFeats() {}
	}
}
