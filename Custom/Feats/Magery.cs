using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Magery : BaseFeat
	{
		public override string Name{ get{ return "Black Magic"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Magery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Magery }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.CureFamine }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Invocation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Meditation, FeatList.DeathI, FeatList.MatterI, 
				FeatList.MindI, FeatList.ForcesI, FeatList.CureFamine }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Black Magic, which will " +
					"give you access to low-level mage powers. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
		{
			
            	if (m.Feats.GetFeatLevel(FeatList.Faith) > 0 )
               		return false;
                if (m.Feats.GetFeatLevel(FeatList.RedMagic) > 0)
                    return false;
               	
               	return base.MeetsOurRequirements( m );
                
		}
		
		public static void Initialize(){ WriteWebpage(new Magery()); }
		
		public Magery() {}
	}
}
