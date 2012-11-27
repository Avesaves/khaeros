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
		public override string Name{ get{ return "Magery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Magery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Magery }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Invocation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Meditation, FeatList.DamagingEffect, FeatList.RangedEffect, 
				FeatList.ExplosiveEffect, FeatList.StatusEffect, FeatList.RecurrentEffect, FeatList.ChainEffect, FeatList.EnchantArmour, 
				FeatList.EnchantWeapon, FeatList.EnchantClothing, FeatList.EnchantRing, FeatList.EnchantTrinket, FeatList.EnchantBracelet }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Magery skill, which will " +
					"give you access to mage powers. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
		{
			if( !m.CanBeMage )
				return false;
			
			return base.MeetsOurRequirements( m );
		}
		
		public static void Initialize(){ WriteWebpage(new Magery()); }
		
		public Magery() {}
	}
}
