using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Fireworks : BaseFeat
	{
		public override string Name{ get{ return "Fireworks"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Fireworks; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BlackPowder }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to create Low Quality Fireworks by using Tinker Tools."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to create Medium Quality Fireworks by using Tinker Tools."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to create Excellent Quality Fireworks by using Tinker Tools."; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Fireworks()); }
		
		public Fireworks() {}
	}
}
