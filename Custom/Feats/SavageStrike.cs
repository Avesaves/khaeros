using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SavageStrike : BaseFeat
	{
		public override string Name{ get{ return "Savage Strike"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SavageStrike; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Macing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You can lunge at the opponent, swinging at them with such " +
					"savagery that your attack is hard to parry. [+10% HCI]"; } }
		public override string SecondDescription{ get{ return "[+20% HCI]"; } }
		public override string ThirdDescription{ get{ return "[+30% HCI]"; } }

		public override string FirstCommand{ get{ return ".SavageStrike"; } }
		public override string SecondCommand{ get{ return ".SavageStrike"; } }
		public override string ThirdCommand{ get{ return ".SavageStrike"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SavageStrike()); }
		
		public SavageStrike() {}
	}
}
