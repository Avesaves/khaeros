using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Tough : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Tough"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Tough; } }
		public override string Description{ get{ return "This merit will raise both your current Hit Points and also your Hit Points cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Tough()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Frail, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawHits < 10 )
            {
                m.SendMessage( "That would lower your current Hit Points too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawHits += 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawHits -= 5;
		}
		
		public Tough() {}
	}
}
