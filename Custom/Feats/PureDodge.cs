using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PureDodge : BaseFeat
	{
		public override string Name{ get{ return "Pure Dodge"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PureDodge; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.EnhancedDodge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ArmouredDodge, FeatList.Evade }; } }
		
		public override string FirstDescription{ get{ return "By doing risky moves, you increase the effectiveness of your dodge, but ignore " +
					"all bonuses from clothes. [+2% to all physical resists]"; } }
		public override string SecondDescription{ get{ return "[+5% to all physical resists]"; } }
		public override string ThirdDescription{ get{ return "[+10% to all physical resists]"; } }

		public override string FirstCommand{ get{ return ".PureDodge"; } }
		public override string SecondCommand{ get{ return ".PureDodge"; } }
		public override string ThirdCommand{ get{ return ".PureDodge"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PureDodge()); }
		
		public PureDodge() {}

        public override void OnLevelChanged( PlayerMobile owner )
		{
            base.OnLevelChanged( owner );

            owner.ComputeResistances();

            if( owner.HasGump( typeof( Gumps.CharInfoGump ) ) && owner.m_CharInfoTimer == null )
            {
                owner.m_CharInfoTimer = new Gumps.CharInfoGump.CharInfoTimer( owner );
                owner.m_CharInfoTimer.Start();
            }
		}
	}
}
