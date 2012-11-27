using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Strong : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Strong"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Strong; } }
		public override string Description{ get{ return "This merit will raise both your current Strength and also your Strength cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Strong()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Weak, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawStr < 10 )
            {
                m.SendMessage( "That would lower your current Strength too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawStr += 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawStr -= 5;
		}
		
		public Strong() {}
	}
}
