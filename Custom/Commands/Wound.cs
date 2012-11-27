using System; 
using System.Text;
using Server; 
using Server.Network; 
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Commands 
{ 
	public class Wound 
	{ 
		public static void Initialize() 
		{ 
			CommandSystem.Register( "Wound", AccessLevel.Player, new CommandEventHandler( Wound_OnCommand ) ); 
		} 

		public static void Wound_OnCommand( CommandEventArgs e ) 
		{ 
			Mobile from = e.Mobile;
			from.Target = new WoundTarget();
		} 
	}
   
	public class WoundTarget : Target 
	{ 
		
		public WoundTarget() : base( 2, false, TargetFlags.None ) 
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if(UseDelaySystem.CheckContext(from,DelayContextType.WoundCommand))	
			{
				from.SendMessage("You must wait an hour before inflicting a wound upon another player.");		
				return;
			}
			
				
			if ( targeted is Corpse )
			{
				PlayerMobile fr = from as PlayerMobile;
				if (fr.IsHardcore)
				{
				Corpse corpse = targeted as Corpse;
				if ( corpse.Deleted )
				return;
				if (targeted is ICarvable && corpse.Owner is PlayerMobile)
				((ICarvable)targeted).Carve( from, corpse );
				UseDelaySystem.AddContext(from, DelayContextType.WoundCommand,TimeSpan.FromHours(1));
				}
				else
				from.SendMessage ("You do not possess the Hardcore merit, and thus, you cannot use this command.");
			}

			else
			from.SendMessage( "You cannot wound that!" );	
			return;
		}
	}
}