using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RedMagic : BaseFeat
	{
		public override string Name{ get{ return "Red Magic"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RedMagic; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Invocation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.EnchantArmour, FeatList.EnchantWeapon, FeatList.EnchantTrinket, FeatList.EnchantClothing }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to perform Red Magic, the art of enchantment."; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }

		public override string FirstCommand{ get{ return ".bloodmagic"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RedMagic()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
		{

            if (m.Feats.GetFeatLevel(FeatList.Faith) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Magery) > 0)
                return false;
			
			return base.MeetsOurRequirements( m );
		}
		
		public RedMagic() {}
	}
}
