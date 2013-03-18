using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedEndurance : BaseFeat
	{
		public override string Name{ get{ return "Mounted Endurance"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedEndurance; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.Trample }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Riding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.MountedCharge, FeatList.Trample }; } }
		
		public override string FirstDescription{ get{ return "You know how to preserve your mount's stamina, allowing him to " +
					"carry you further. [Whenever your mount would regen stamina it will regen an extra 0-1 stamina]"; } }
		public override string SecondDescription{ get{ return "[Whenever your mount would regen stamina it will regen an extra 0-2 stamina]"; } }
		public override string ThirdDescription{ get{ return "[Whenever your mount would regen stamina it will regen an extra 0-3 stamina]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }

        public override void FixAddOns( PlayerMobile owner )
        {
            owner.Feats.SetFeatLevel( FeatList.Trample, Level );
        }
		
		public static void Initialize(){ WriteWebpage(new MountedEndurance()); }
		
		public MountedEndurance() {}
	}
}
