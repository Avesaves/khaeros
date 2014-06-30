using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnchantTrinket : BaseFeat
	{
		public override string Name{ get{ return "Special Enchantment"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnchantTrinket; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.RedMagic }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This unlocks .CreateEcho, which will let you put a verbal message on an item."; } }
		public override string SecondDescription{ get{ return "This unlocks .Root, which will allow you to lockdown items outside a house (can be retrieved by anyone with .grab)."; } }
		public override string ThirdDescription{ get{ return "This unlocks .CastRune, which will allow you to create a visible trap at your feet."; } }

		public override string FirstCommand{ get{ return ".CreateEcho"; } }
		public override string SecondCommand{ get{ return ".Root"; } }
		public override string ThirdCommand{ get{ return ".CastRune"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnchantTrinket()); }
        public override bool MeetsOurRequirements(PlayerMobile m)
        {

            return base.MeetsOurRequirements(m);
        }
		public EnchantTrinket() {}
	}
}
