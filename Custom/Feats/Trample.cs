using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Trample : BaseFeat
	{
		public override string Name{ get{ return "Trample"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Trample; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MountedEndurance }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You can spur your mount to ride over your enemies and trampling them " +
					"underneath its hooves. [Hit chance is an opposing tactics + dexterity check. Min hit chance is 10% and max 20%. " +
					"Damage dealt is 5-10. In order to be able to trample you must be on full stamina]"; } }
		public override string SecondDescription{ get{ return "[Hit chance is an opposing tactics + dexterity check. Min hit chance is " +
					"20% and max 40%. Damage dealt is 10-20. In order to be able to trample you must be on full stamina]"; } }
		public override string ThirdDescription{ get{ return "[Hit chance is an opposing tactics + dexterity check. Min hit chance is " +
					"30% and max 60%. Damage dealt is 15-30. In order to be able to trample you must be on full stamina]"; } }

		public override string FirstCommand{ get{ return ".Trample"; } }
		public override string SecondCommand{ get{ return ".Trample"; } }
		public override string ThirdCommand{ get{ return ".Trample"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Trample()); }
		
		public Trample() {}
	}
}
