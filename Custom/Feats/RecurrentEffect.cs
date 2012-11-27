using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RecurrentEffect : BaseFeat
	{
		public override string Name{ get{ return "Recurrent Effect"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RecurrentEffect; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Magery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to create custom spells with up to 2 repetitions, " +
					"with a delay of 11-15 seconds between them, and with up to 25 recurrent damage."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to create custom spells with up to 4 repetitions, " +
					"with a delay of 6-15 seconds between them, and with up to 50 recurrent damage."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to create custom spells with up to 6 repetitions, " +
					"with a delay of 1-15 seconds between them, and with limitless recurrent damage."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RecurrentEffect()); }
		
		public RecurrentEffect() {}
	}
}
