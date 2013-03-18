using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Barbery : BaseFeat
    {
        public override string Name{ get{ return "Barbery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Barbery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Healing }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Barbery is the combined knowledge of basic healing arts and the art of hair-styling. [20% Healing]"; } }
		public override string SecondDescription{ get{ return "[50% Healing]"; } }
		public override string ThirdDescription{ get{ return "[100% Healing]"; } }

		public override string FirstCommand{ get{ return ".HairStyling"; } }
		public override string SecondCommand{ get{ return ".HairStyling"; } }
		public override string ThirdCommand{ get{ return ".HairStyling"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Barbery()); }
		
		public Barbery() {}
    }
}
