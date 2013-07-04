/*using System;
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
		public override string Description{ get{ return "Description: You are tough as nails, and don't need to prove it to anyone. As you know that death can be just around the corner you make every action in life count. You know the risks that are involved in pursuing perfection, and your years of reckless abandon in such endeavors have left you more susceptible to harm, and at times, even flirting with death itself. This flaw is a double-edged sword however, as you've come to learn how not only take such punishment, but also how to dish it out, sometimes with deadly results.<br><br>Mechanical Details:<br>Random 1 – 15% increased chance (random % calculated upon every knockout) in incurring an injury when knocked out by an NPC or PC.<br>All injuries sustained take twice as long to heal on their own if left untreated.<br>Increased difficulty for PCs to heal injuries your character has incurred.<br>Name will appear 'red” to all other characters in the game world.<br>Access to the “Finishing Wound” ability (.wound).<br><br>Finishing Wound:<br>The Finishing Wound ability is a new command (.wound) that can be used by Hardcore characters whom are involved in consensual player versus player combat with other players. Once a character is “knocked out”, if their assailant possess the Hardcore merit, they can use the Finishing Wound ability on the fallen corpse of their opponent to inflict an injury upon them.<br><br>A 'Finishing Wound' is inflicted upon a knocked out character if and when another character that possesses the Hardcore merit uses the .wound command on their knocked out corpse. Using this command on a knocked out player will inflict a random injury upon them. The Finishing Wound ability works differently on players whom do not have the Hardcore merit from those whom do have the Hardcore merit. Any character, no matter if they have taken the Hardcore merit or not, cannot be inflicted more than one Finishing Wound in a 24 hour period. In addition, the .wound command has a one hour cool down period before being able to be used again.<br><br>Finishing Wounds for Non-Hardcore Victims:<br>For characters that are the victim of a Finishing Wound that do NOT possess the Hardcore merit, a random “lesser” injury will be inflicted upon said character once the .wound command is successfully used on their knocked out corpse. A “lesser” injury can include any of the following minor injuries: Winded, Bruised, Minor Cut, Minor Concussion, and Bloodied. These minor injuries have small mechanical disadvantages, however can be easily healed and treated by a character whom possesses the proper traits. An injury will NOT be inflicted upon a target character if they are below level 20, if they have been injured in the past five minutes, if they have been the victim of a Finishing Wound in the last 24 hours, or if they possess all possible injuries already.<br><br>Finishing Wounds for Hardcore Victims:<br>Finishing Wounds are a more serious matter for characters whom possess the Hardcore merit. For characters that are the victim of a Finishing Wound that DO possess the Hardcore merit, a random “major” injury will be inflicted upon said character once the .wound command is successfully used on their knocked out corpse. A “major” injury includes all injuries from “Internal Bleeding” to “Massive Bleeding”, as well as including other major injuries such as broken appendages. These major injuries have very large mechanical disadvantages, and are quite difficult to treat even by players whom possesses the proper traits. An injury will not be inflicted upon a target character if they have been injured in the past five minutes, if they have been the victim of a Finishing Wound in the last 24 hours, or if they possess all possible injuries already.<br><br>Additionally, a Finishing Wound will leave permanent effects on a player whom has the Hardcore merit. For Hardcore characters, a Finishing Wound has 10% base chance to cause a victim to lose either 5 or 10 points (chosen at random) in Hits, Stam, and Mana. Each stat works off of it's own dice roll, and thus, an injured character will roll three times to determine if they take any or all of the deductions. In addition, each Finishing Wound a Hardcore character sustains will add a +10% chance increase to this stat loss for each Finishing Wound counter a character possess (for example, a character with two Finishing Wound counters will have a 30% chance of losing stat points, if they don't PD to begin with) . In addition, for every Finishing Wound a Hardcore character sustains, said wound is saved as a numeric value and added to a counter, which in turn increase the chance of said character being dealt a Fatal Wound. A Fatal Wound permanently ends the life of a victim. Finishing Wound counters raise the chance of a Hardcore character succumbing to a Fatal Wound when they are subjected to a Finishing Wound by 10% for each counter they possess. For example, a Hardcore character whom has had two Finishing Wounds inflicted upon them in their lifespan will have a 20% chance of succumbing to a Fatal Wound when a Finishing Wound is inflicted upon them. Finishing Wound counters for Hardcore players do not expire, and stay with a character for their entire lifespan."; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Hardcore()); }
		
        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.IsHardcore = true;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.IsHardcore = false;
		}
		
		public Hardcore() {}
	}
}*/
