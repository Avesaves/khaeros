using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Fit : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Fit"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Fit; } }
		public override string Description{ get{ return "This merit will raise both your current Stamina and also your Stamina cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Fit()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Unenergetic, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawStam < 10 )
            {
                m.SendMessage( "That would lower your current Stamina too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawStam += 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawStam -= 5;
		}
		
		public Fit() {}
	}
}
