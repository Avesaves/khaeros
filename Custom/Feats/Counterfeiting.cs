using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Counterfeiting : BaseFeat
	{
		public override string Name{ get{ return "Counterfeiting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Counterfeiting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.VerifyCurrency }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.GemEmbedding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.VerifyCurrency }; } }
		
		public override string FirstDescription{ get{ return "This skill will grant you the ability to forge copper coins."; } }
		public override string SecondDescription{ get{ return "This skill will grant you the ability to forge silver coins."; } }
		public override string ThirdDescription{ get{ return "This skill will grant you the ability to forge gold coins."; } }

		public override string FirstCommand{ get{ return ".Forge Copper"; } }
		public override string SecondCommand{ get{ return ".Forge Copper | .Forge Silver"; } }
		public override string ThirdCommand{ get{ return ".Forge Copper | .Forge Silver | .Forge Gold"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Counterfeiting()); }
		
		public Counterfeiting() {}
	}
}
