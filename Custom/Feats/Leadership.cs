using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Leadership : BaseFeat
	{
		public override string Name{ get{ return "Leadership"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Leadership; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Leadership }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.LifeI }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.MercTraining, FeatList.LingeringCommand, FeatList.ExpeditiousRetreat, 
				FeatList.InspireResilience, FeatList.InspireHeroics, FeatList.InspireFortitude }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Leadership skill, which will " +
					"improve your pets' and mercenaries' fighting capabilities. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Leadership()); }

     		public override void AttemptPurchase(PlayerMobile m, int level, bool freeRemoval)
        	{
           	 m.SendMessage("This feat has been disabled, you cannot purchase it.");
        	}
		
		public Leadership() {}
	}
}
