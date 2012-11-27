using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Ventriloquism : BaseFeat
	{
		public override string Name{ get{ return "Ventriloquism"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Ventriloquism; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Linguistics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to throw your voice and make others believe it comes " +
					"from elsewhere. [2 tiles]"; } }
		public override string SecondDescription{ get{ return "[4 tiles]"; } }
		public override string ThirdDescription{ get{ return "[6 tiles]"; } }

		public override string FirstCommand{ get{ return ".Say"; } }
		public override string SecondCommand{ get{ return ".Say"; } }
		public override string ThirdCommand{ get{ return ".Say"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Ventriloquism()); }
		
		public Ventriloquism() {}
	}
}
