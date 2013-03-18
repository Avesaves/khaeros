using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Poisoning : BaseFeat
	{
		public override string Name{ get{ return "Poisoning"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Poisoning; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Poisoning }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PoisonResistance }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Poisoning skill, which will allow " +
					"you to create poisons using a mixing set and venom you can extract by skinning venomous creatures. It will also improve the " +
					"duration of poisons you apply on weapons. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Poisoning()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
			{

           	 if(((PlayerMobile)m).Feats.GetFeatLevel(FeatList.Anatomy) < 3)
                	return false;

          	 if (((PlayerMobile)m).Feats.GetFeatLevel(FeatList.Alchemy) < 3)
               		 return false;

				return base.MeetsOurRequirements( m );
			}
		
		public Poisoning() {}
	}
}
