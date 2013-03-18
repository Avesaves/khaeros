using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CripplingBlow : BaseFeat
	{
		public override string Name{ get{ return "Crippling Blow"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CripplingBlow; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Swordsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to cripple your opponents and prevent them from running " +
					"or knock them off their mounts. [Lasts 1.5 seconds, freezes creatures, dismounts riders]"; } }
		public override string SecondDescription{ get{ return "[Lasts 3 seconds, freezes creatures, dismounts riders]"; } }
		public override string ThirdDescription{ get{ return "[Lasts 4.5 seconds, freezes creatures, dismounts riders]"; } }

		public override string FirstCommand{ get{ return ".CripplingBlow"; } }
		public override string SecondCommand{ get{ return ".CripplingBlow"; } }
		public override string ThirdCommand{ get{ return ".CripplingBlow"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CripplingBlow()); }
		
		public CripplingBlow() {}
	}
}
