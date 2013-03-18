using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FateII : BaseFeat
	{
		public override string Name{ get{ return "Fate II"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FateII; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.FateI }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to cast fate-based spells from ancient " +
					"scrolls you may find. It will also increase your mana total."; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FateII()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
		{
			if( !m.CanBeMage )
				return false;
			
			return base.MeetsOurRequirements( m );
		}
		
		public FateII() {}

        public override void OnLevelLowered( PlayerMobile owner )
        {
            base.OnLevelLowered( owner );

            if( Level == 0 )
                owner.RawMana -= 1;
            else if( Level == 1 )
                owner.RawMana -= 2;
            else if( Level == 2 )
                owner.RawMana -= 3;
        }

        public override void OnLevelRaised( PlayerMobile owner )
        {
            base.OnLevelRaised( owner );

            if( Level == 1 )
                owner.RawMana += 1;
            else if( Level == 2 )
                owner.RawMana += 2;
            else if( Level == 3 )
                owner.RawMana += 3;
        }
	}
}
