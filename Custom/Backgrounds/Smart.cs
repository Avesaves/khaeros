using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Smart : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Smart"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Smart; } }
		public override string Description{ get{ return "This merit will raise both your current Intelligence and also your Intelligence cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Smart()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Feebleminded, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawInt < 10 )
            {
                m.SendMessage( "That would lower your current Intelligence too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawInt += 5;
			m.CPCapOffset += 1000;
			m.FeatSlots -= 1000;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawInt -= 5;
			m.CPCapOffset -= 1000;
			m.FeatSlots += 1000;
		}
		
		public Smart() {}
	}
}
