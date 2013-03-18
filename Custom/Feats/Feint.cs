using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Feint : BaseFeat
	{
		public override string Name{ get{ return "Feint"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Feint; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "When activated, the next attack will be feigned (i.e. only animation, no effect). " +
					"This 'attack' will not cause a delay, so if the opponent attempts to parry the feigned attack, the player can initiate a " +
					"real attack right off the bat. Should you get an attack of opportunity while performing a feigned attack (e.g. the " +
					"enemy turns their back) the attack will become real and hit them. [2 times slower animation for the feigned attack]"; } }
		public override string SecondDescription{ get{ return "[1.5 times slower animation for the feigned attack]"; } }
		public override string ThirdDescription{ get{ return "[same animation speed as your regular attack (and thus indistinguishable)]"; } }

		public override string FirstCommand{ get{ return ".Feint"; } }
		public override string SecondCommand{ get{ return ".Feint"; } }
		public override string ThirdCommand{ get{ return ".Feint"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Feint()); }
		
		public Feint() {}
	}
}
