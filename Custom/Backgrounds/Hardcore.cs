using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Hardcore : BaseBackground
	{
		public override int Cost{ get{ return 0; } }
		public override string Name{ get{ return "Hardcore"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Hardcore; } }
		public override string Description{ get{ return "Description: This background is disabled."; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Hardcore()); }
		
        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.IsHardcore = false;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.IsHardcore = false;
		}
		
		public Hardcore() {}
	}
}
