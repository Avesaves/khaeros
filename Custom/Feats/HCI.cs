﻿using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class HCI : BaseFeat
    {
        public override string Name{ get{ return "HCI"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HCI; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.WeaponSmithing, FeatList.Masterwork }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }

        public override string FirstDescription { get { return "This skill increases your chance to add HCI to weapons at the cost of the others. [10% chance to add HCI to weapons] "; } }
        public override string SecondDescription { get { return "[20% chance]"; } }
        public override string ThirdDescription { get { return "[30% chance]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HCI()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.Feats.GetFeatLevel(FeatList.Damage) > 0 || m.Feats.GetFeatLevel(FeatList.Speed) > 0 || m.Feats.GetFeatLevel(FeatList.DCI) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }
		
		public HCI() {}
    }
}
