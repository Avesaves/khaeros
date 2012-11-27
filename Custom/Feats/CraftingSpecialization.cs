using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CraftingSpecialization : BaseFeat
	{
		public override string Name{ get{ return "Crafting Specialization"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CraftingSpecialization; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.RenownedMasterwork }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to choose a crafting field to specialize in, thus " +
					"enhancing your chances of getting extraordinary and masterwork items in that field, but lowering in your chances in " +
					"all other fields."; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }
		
		public override string FirstCommand{ get{ return ".CraftingSpecialization"; } }
		public override string SecondCommand{ get{ return ".CraftingSpecialization"; } }
		public override string ThirdCommand{ get{ return ".CraftingSpecialization"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CraftingSpecialization()); }
		
		public CraftingSpecialization() {}

        public override void OnLevelLowered( PlayerMobile owner )
        {
            owner.CraftingSpecialization = null;
        }
	}
}
