using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class LeadershipMastery : BaseFeat
    {
        public override string Name { get { return "Leadership Mastery"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.LeadershipMastery; } }
        public override FeatCost CostLevel { get { return FeatCost.High; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.LingeringCommand }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have mastered leading animals and men, and gain one additional follower slot."; } }
        public override string SecondDescription { get { return "You are a renowned commander and leader, and now have two additional follower slots."; } }
        public override string ThirdDescription { get { return "You are a paragon of leadership and command, and now have three additional follower slots."; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public static void Initialize() { WriteWebpage(new LeadershipMastery()); }

        public override void AttemptPurchase(PlayerMobile m, int level, bool freeRemoval)
        {
            m.SendMessage("This feat has been disabled, you cannot purchase it.");
        }

        public override void OnLevelLowered(PlayerMobile owner)
        {
            owner.FollowersMax--;
            if(owner.Followers > owner.FollowersMax)
            {
                owner.Followers = 0;
                foreach( Mobile m in World.Mobiles.Values )
			    {
				    if( m is BaseCreature )
				    {
					    BaseCreature bc = (BaseCreature)m;

					    if (bc.Controlled && bc.ControlMaster == owner)
                        {
                            if ((bc.ControlSlots + owner.Followers) > owner.FollowersMax)
                            {
                                bc.Controlled = false;
                                bc.ControlMaster = null;
                                owner.SendMessage("You has lost control of " + bc.Name + ".");
                            }
                            else
                                owner.Followers += bc.ControlSlots;
                        }
				    }
			    }

                owner.FixControlSlots();
            }

            owner.SendMessage(2118, "You now have the use of up to " + owner.FollowersMax + " followers.");
            base.OnLevelLowered(owner);
        }

        public override void OnLevelRaised(PlayerMobile owner)
        {
            owner.FollowersMax++;
            owner.SendMessage(2212, "You now have the use of up to " + owner.FollowersMax + " followers.");
            base.OnLevelRaised(owner);
        }

        public LeadershipMastery() { }
    }
}
