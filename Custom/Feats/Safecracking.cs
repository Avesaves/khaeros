﻿using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Safecracking : BaseFeat
    {
        public override string Name { get { return "Safecracking"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Safecracking; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.Locksmith, FeatList.Tinkering }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription
        {
            get
            {
                return "This skill will make you an apprentice of the Safecracking skill, which will " +
                    "allow you to attempt to crack locked safes.";
            }
        }
        public override string SecondDescription { get { return "Journeyman Safecracker"; } }
        public override string ThirdDescription { get { return "Master Safecracker"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new Safecracking()); }

        public override bool MeetsOurRequirements( PlayerMobile m )
		{
			if( !m.CanBeThief )
				return false;

            if(((PlayerMobile)m).Feats.GetFeatLevel(FeatList.Tinkering) < 3)
                return false;

            if (((PlayerMobile)m).Feats.GetFeatLevel(FeatList.Locksmith) < 3)
                return false;
			
			return base.MeetsOurRequirements( m );
		}

        public Safecracking() { }
    }
}
