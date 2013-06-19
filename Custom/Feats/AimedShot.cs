using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AimedShot : BaseFeat
	{
		public override string Name{ get{ return "Aimed Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AimedShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.FocusedShot }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the archer to increase his accuracy with " +
					"crossbows by taking some time to aim before firing. The longer the archer aims, the better his accuracy will be. " +
					"When the bonus converges to the maximum, the archer will see a visual cue indicating that aiming any " +
					"longer will not increase his accuracy. Convergence speed is 1 second for all feat levels. This feat will work while mounted, but only if you have Mounted Archery level 3. " +
					"[50% hit chance maximum] <br>"; } }
		public override string SecondDescription{ get{ return "[75% hit chance maximum]"; } }
		public override string ThirdDescription{ get{ return "[100% hit chance maximum]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AimedShot()); }
		
		public AimedShot() {}
	}
}
