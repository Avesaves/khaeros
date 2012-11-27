using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class GemEmbedding : BaseFeat
	{
		public override string Name{ get{ return "Gem Embedding"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.GemEmbedding; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.JewelryCrafting }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Counterfeiting }; } }
		
		public override string FirstDescription{ get{ return "You are able to embed common gems into weapons or armour, and work with cinnabar."; } }
		public override string SecondDescription{ get{ return "You are able to embed rare gems into weapons or armour."; } }
		public override string ThirdDescription{ get{ return "You are able to embed the rarest gems into weapons or armour."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new GemEmbedding()); }
		
		public GemEmbedding() {}
	}
}
