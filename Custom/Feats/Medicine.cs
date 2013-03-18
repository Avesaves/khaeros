using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Medicine : BaseFeat
    {
        public override string Name{ get{ return "Medicine"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Medicine; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Alchemy }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Surgery }; } }
		
		public override string FirstDescription{ get{ return "The study of medicine will allow you to heal injuries much faster than time would normally allow. [+5 Seconds to Heal Attempts]"; } }
		public override string SecondDescription{ get{ return "[+10 Seconds to Heal Attempts]"; } }
		public override string ThirdDescription{ get{ return "[+15 Seconds to Heal Attempts]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Medicine()); }
		
		public Medicine() {}
    }
}
