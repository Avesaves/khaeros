using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Surgery : BaseFeat
    {
        public override string Name{ get{ return "Surgery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Surgery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Medicine }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "The study of surgery allows you to treat and heal injuries with greater precision. At this level, you may reduce Sewing and Heat during surgery. [+10 Seconds to Heal Attempts]"; } }
		public override string SecondDescription{ get{ return "At this level, you may reduce Sewing, Heat, Cutting, and Cooling during surgery. [+20 Seconds to Heal Attempts]"; } }
		public override string ThirdDescription{ get{ return "At this level, you may reduce Sewing, Heat, Cutting, Cooling, and Bleeding during surgery. Additionally, you gain a margin of error on these surgical techniques when attempting to heal a wound. [+30 Seconds to Heal Attempts]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Surgery()); }
		
		public Surgery() {}
    }
}