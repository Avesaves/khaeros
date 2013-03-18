using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Technique : BaseFeat
	{
		public override string Name{ get{ return "Technique"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Technique; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.UnarmedFighting }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		
		public override string FirstDescription{ get{ return "Through your Martial Arts studies, you have learnt how to change the kind of " +
					"damage you deal with your fists, being now able to even cut or pierce through flesh. [can change 50% of the damage you " +
					"deal to Slashing or Piercing, rather than just Blunt]"; } }
		public override string SecondDescription{ get{ return "[can change 75%]"; } }
		public override string ThirdDescription{ get{ return "[can change 100%]"; } }

		public override string FirstCommand{ get{ return ".Technique <Piercing, Slashing or Blunt (to turn it off)>"; } }
		public override string SecondCommand{ get{ return ".Technique <Piercing, Slashing or Blunt (to turn it off)>"; } }
		public override string ThirdCommand{ get{ return ".Technique <Piercing, Slashing or Blunt (to turn it off)>"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Technique()); }
		
		public Technique() {}
	}
}
