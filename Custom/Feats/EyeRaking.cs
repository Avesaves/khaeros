using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EyeRaking : BaseFeat
	{
		public override string Name{ get{ return "Eye Raking"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EyeRaking; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to viciously attack your opponent's eyes, blinding them " +
					"temporarily. [Target is blinded for 2 seconds and suffers -50% Hit Chance]"; } }
		public override string SecondDescription{ get{ return "[Target is blinded for 4 seconds and suffers -50% Hit Chance]"; } }
		public override string ThirdDescription{ get{ return "[Target is blinded for 6 seconds and suffers -50% Hit Chance]"; } }

		public override string FirstCommand{ get{ return ".EyeRaking"; } }
		public override string SecondCommand{ get{ return ".EyeRaking"; } }
		public override string ThirdCommand{ get{ return ".EyeRaking"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EyeRaking()); }
		
		public EyeRaking() {}
	}
}
