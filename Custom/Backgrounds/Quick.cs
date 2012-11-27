using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Quick : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Quick"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Quick; } }
		public override string Description{ get{ return "This merit will raise both your current Dexterity and also your Dexterity cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Quick()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Clumsy, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawDex < 10 )
            {
                m.SendMessage( "That would lower your current Dexterity too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawDex += 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawDex -= 5;
		}
		
		public Quick() {}
	}
}
