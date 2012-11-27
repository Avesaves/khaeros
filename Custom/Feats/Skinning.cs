using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Skinning : BaseFeat
	{
		public override string Name{ get{ return "Skinning"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Skinning; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ImprovedSkinning }; } }
		
		public override string FirstDescription{ get{ return "You know how to remove the meat from fallen creatures."; } }
		public override string SecondDescription{ get{ return "You know how to remove leather, fur and feathers from fallen creatures."; } }
		public override string ThirdDescription{ get{ return "You know how to extract alchemical and venomous ingredients from creatures, as well as any other parts that might be of interest."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Skinning()); }
		
		public Skinning() {}
	}
}
