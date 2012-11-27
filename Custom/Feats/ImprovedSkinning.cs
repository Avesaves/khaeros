using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ImprovedSkinning : BaseFeat
	{
		public override string Name{ get{ return "Improved Skinning"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ImprovedSkinning; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Skinning }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Bone }; } }
		
		public override string FirstDescription{ get{ return "You know how to get the most material out of an animal. [You harvest 2 more " +
					"units when skinning and shearing animals]"; } }
		public override string SecondDescription{ get{ return "[You harvest 4 more units when skinning and shearing animals]"; } }
		public override string ThirdDescription{ get{ return "[You harvest 6 more units when skinning and shearing animals]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ImprovedSkinning()); }
		
		public ImprovedSkinning() {}
	}
}
