using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RacialFightingStyle : BaseFeat
	{
		public override string Name{ get{ return "Racial Fighting Style"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RacialFightingStyle; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You have learned the fighting style unique to your culture. " +
					"Sharing your forest with one of the deadliest predators you have learned to emulate their devastating attacks. " +
					"[(+5% Attack Chance * skill level) (+2 Speed * skill level)]<br>" +
					"You have learned how to move like the jaguar, attacking blindingly fast while repelling any attempts to hit you. " +
					"[(+10% Defence Chance * skill level) (-5% Attack Chance * skill level) (+1 Speed * skill level) (+1 Damage * skill level)] <br>" +
					"Like the slithering snakes of the scorching sands you discard all thought of defence in favour of deadly attacks. [(+15% " +
					"Attack Chance * skill level) (-10% Defence Chance * skill level) (+3 Speed * skill level) (-1 Damage * skill level)] <br>" +
					"Your attacks follow the teachings of the dragon spirit, lowering your defences but allowing you to dispatch your enemies" +
					" with brutal force. [(+10% Attack Chance * skill level) (-5% Defence Chance * skill level) (+6 Damage * skill level) (-2 " +
					"Speed * skill level)] <br> Raising your fists like one of the Hydra's heads you deliver forceful blows to your enemies " +
					"while keeping yourself well defended. [(+5% Defence Chance * skill level) (+4 Damage * skill level) (-1 Speed * skill " +
					"level)] <br>Closing your defences your fists become like a warhorse's hooves. [(+15% Defence Chance * skill level) " +
					"(-10% Attack Chance * skill level) (+2 Damage * skill level)]"; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }

		public override string FirstCommand{ get{ return ".SilentHowl | .SwipingClaws | .VenomousWay | .SearingBreath | .TempestuousSea | .ThunderingHooves"; } }
		public override string SecondCommand{ get{ return ".SilentHowl | .SwipingClaws | .VenomousWay | .SearingBreath | .TempestuousSea | .ThunderingHooves"; } }
		public override string ThirdCommand{ get{ return ".SilentHowl | .SwipingClaws | .VenomousWay | .SearingBreath | .TempestuousSea | .ThunderingHooves"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RacialFightingStyle()); }
		
		public RacialFightingStyle() {}
	}
}
