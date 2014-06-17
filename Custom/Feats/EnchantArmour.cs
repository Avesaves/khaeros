using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnchantArmour : BaseFeat
	{
		public override string Name{ get{ return "Enchant Armour"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnchantArmour; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.RedMagic }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to enchant armour."; } }
        public override string SecondDescription { get { return "Better effects."; } }
        public override string ThirdDescription { get { return "Best effects."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnchantArmour()); }
        public override bool MeetsOurRequirements(PlayerMobile m)
        {

            if (!m.CanBeMage)
                return false;
        }
		public EnchantArmour() {}
	}
}
