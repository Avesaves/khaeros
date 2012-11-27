using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EscortPrisoner : BaseFeat
	{
		public override string Name{ get{ return "Escort Prisoner"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EscortPrisoner; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.RopeTrick }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the transport of a tied up individual under close watch. " +
					"Target must be tied up and secure before escort prisoner can work. [Escort prisoner at walking speed]"; } }
		public override string SecondDescription{ get{ return "[Escort prisoner at running speed]"; } }
		public override string ThirdDescription{ get{ return "[Escort prisoner on horseback]"; } }

		public override string FirstCommand{ get{ return ".EscortPrisoner"; } }
		public override string SecondCommand{ get{ return ".EscortPrisoner"; } }
		public override string ThirdCommand{ get{ return ".EscortPrisoner"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EscortPrisoner()); }
		
		public EscortPrisoner() {}
	}
}
