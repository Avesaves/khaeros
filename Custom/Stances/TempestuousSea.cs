using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class TempestuousSea : BaseStance
	{
        public override string Name{ get{ return "Tempestuous Sea"; } }
		public override bool MartialArtistStance{ get{ return true; } }

		public override string TurnedOnEmote{ get{ return "*begins staggering about, seemingly drunk and out of control*"; } }
		public override string TurnedOffEmote{ get{ return "*steadies and goes back to a regular fighting stance*"; } }

        public int accBonus = Utility.RandomMinMax(-15, 15);
        public int defBonus = Utility.RandomMinMax(-15, 15);
        public double spdBonus = (Utility.RandomMinMax(-15, 15) / 2);
        public double dmgBonus = (Utility.RandomMinMax(-15, 15) / 2);

		public override int AccuracyBonus{ get{ return accBonus; } }
        public override double DamageBonus { get { return dmgBonus; } }
        public override double SpeedBonus { get { return spdBonus; } }
		public override int DefensiveBonus{ get{ return defBonus;  } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }
		
		public TempestuousSea( int featlevel ) : base( featlevel )
		{
		}
		
		public TempestuousSea() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.TempestuousSea) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.TempestuousSea);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "TempestuousSea", AccessLevel.Player, new CommandEventHandler( TempestuousSea_OnCommand ) );
		}
		
		[Usage( "TempestuousSea" )]
        [Description( "Allows the user to change their combat stance to Tyreans' racial style." )]
        private static void TempestuousSea_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m.CanPerformAttack(m.Feats.GetFeatLevel(FeatList.TempestuousSea)))
            {
                TempestuousSea newSea = new TempestuousSea(m.Feats.GetFeatLevel(FeatList.TempestuousSea));
                string seaEmote1 = "";
                string seaEmote2 = "";

                if (!(m.Stance is TempestuousSea))
                {
                    //HCI/DCI: telling the mobile how they compare in this incarnation of Tempestuous Sea
                    if (newSea.AccuracyBonus > newSea.DefensiveBonus)
                    {
                        seaEmote1 = "making headway";
                    }
                    else if (newSea.AccuracyBonus < newSea.DefensiveBonus)
                    {
                        seaEmote1 = "between a demon and the deep blue sea";
                    }
                    else if (newSea.AccuracyBonus == newSea.DefensiveBonus)
                    {
                        seaEmote1 = "back and filling";
                    }
                    else
                    {
                        seaEmote1 = "ERROR";
                    }

                    //Damage/Speed:  telling the mobile how they compare in this incarnation of Tempestuous Sea
                    if (newSea.DamageBonus > newSea.SpeedBonus)
                    {
                        seaEmote2 = "going by the board";
                    }
                    else if (newSea.DamageBonus < newSea.SpeedBonus)
                    {
                        seaEmote2 = "three sheets to the wind";
                    }
                    else if (newSea.DamageBonus == newSea.SpeedBonus)
                    {
                        seaEmote2 = "toeing the line";
                    }
                    else
                    {
                        seaEmote1 = "ERROR";
                    }

                    //For testing purposes; do the stats listed here match up with the if/else above that reads to the mobile?
                    //m.SendMessage("ACCURACY: " + newSea.AccuracyBonus);
                    //m.SendMessage("DEFENSE: " + newSea.DefensiveBonus);
                    //m.SendMessage("SPEED: " + newSea.SpeedBonus);
                    //m.SendMessage("DAMAGE: " + newSea.DamageBonus);

                    m.ChangeStance(newSea);
                    m.SendMessage("You are " + seaEmote1 + " and " + seaEmote2 + ".");
                }
                else
                {
                    m.ChangeStance(newSea); // return to normal stance
                }
            }
        }
	}
}
