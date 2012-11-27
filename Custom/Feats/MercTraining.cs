using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MercTraining : BaseFeat
	{
		public override string Name{ get{ return "Merc Training"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MercTraining; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Leadership }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to teach up to 2 skills to your mercenaries (from a " +
					"list of 20 possible skills, but you must have at least the first level of the skill you wish to teach them)."; } }
		public override string SecondDescription{ get{ return "Increases the amount of skills you can teach to your mercenaries to 4."; } }
		public override string ThirdDescription{ get{ return "Increases the amount of skills you can teach to your mercenaries to 6."; } }

		public override string FirstCommand{ get{ return ".MercTraining | .SetStance | .SetManeuver"; } }
		public override string SecondCommand{ get{ return ".MercTraining | .SetStance | .SetManeuver"; } }
		public override string ThirdCommand{ get{ return ".MercTraining | .SetStance | .SetManeuver"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MercTraining()); }
		
		public MercTraining() {}
	}
}
