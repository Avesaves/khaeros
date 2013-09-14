using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedDefence : BaseFeat
	{
		public override string Name{ get{ return "Mounted Defence"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedDefence; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Riding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.HorseArcher, FeatList.KudaRider, FeatList.Clibanarii, FeatList.SteppeRaider, FeatList.Skirmisher, FeatList.HeavyCavalry }; } }
		
		public override string FirstDescription{ get{ return "You are able to defend yourself from melee attacks by using " +
					"your mount, however, this will damage it. [5% damage is transfered to mount if the mount is above 75% health]"; } }
		public override string SecondDescription{ get{ return "[10% damage is transfered to mount if the mount is above 55% health]"; } }
		public override string ThirdDescription{ get{ return "[15% damage is transfered to mount if the mount is above 35% health]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MountedDefence()); }
		
		public MountedDefence() {}
	}
}
