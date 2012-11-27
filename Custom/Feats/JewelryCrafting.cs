using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class JewelryCrafting : BaseFeat
	{
		public override string Name{ get{ return "Jewelry Crafting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.JewelryCrafting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tinkering }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.GemEmbedding }; } }
		
		public override string FirstDescription{ get{ return "You have learned the delicate art of crafting bracelets."; } }
		public override string SecondDescription{ get{ return "You have learned the delicate art of crafting necklaces."; } }
		public override string ThirdDescription{ get{ return "You have learned the delicate art of crafting rings and earrings."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new JewelryCrafting()); }
		
		public JewelryCrafting() {}
	}
}
