using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class StatusEffect : BaseFeat
	{
		public override string Name{ get{ return "Status Effect"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.StatusEffect; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Magery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to create custom spells with a status effect " +
					"that freezes mobiles for up to 2 seconds."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to create custom spells with a status effect " +
					"that prevents mobiles from attacking for up to 4 seconds."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to create custom spells with a status effect " +
					"that both freezes mobiles and prevents them from attacking for up to 6 seconds."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new StatusEffect()); }
		
		public StatusEffect() {}
	}
}
