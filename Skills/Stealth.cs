using System;
using Server.Items;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class Stealth
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Stealth].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			if ( !m.Hidden )
			{
				m.SendLocalizedMessage( 502725 ); // You must hide first
			}
			else if ( m.Skills[SkillName.Hiding].Base < ((Core.SE) ? 50.0 : 80.0) )
			{
				m.SendLocalizedMessage( 502726 ); // You are not hidden well enough.  Become better at hiding.
				m.RevealingAction();
			}
			else if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.HeavyPenalty > 0 || ((pm.Feats.GetFeatLevel(FeatList.ArmouredStealth) * 3) < pm.TotalPenalty) ||
				   (pm.Feats.GetFeatLevel(FeatList.ArmouredStealth) < 1 && (pm.MediumPieces > 0 || pm.LightPieces > 0)) )
				{
					m.SendLocalizedMessage( 502727 ); // You could not hope to move quietly wearing this much armor.
					m.RevealingAction();
				}
				else if ( m.CheckSkill( SkillName.Stealth, -20.0 + (pm.TotalPenalty * 3), (Core.AOS ? 60.0 : 80.0) + (pm.TotalPenalty * 3) ) )
				{
					int steps = (int)(m.Skills[SkillName.Stealth].Value / (Core.AOS ? 5.0 : 10.0));

					if ( steps < 1 )
						steps = 1;

					m.AllowedStealthSteps = steps;
					
					foreach( Mobile mob in m.GetMobilesInRange( 3 ) )
						if( mob.Hidden && (mob is Wolf || mob is Dog) && ((BaseCreature)mob).ControlMaster == m )
							mob.AllowedStealthSteps = steps;

					//m.SendLocalizedMessage( 502730 ); // You begin to move quietly.

					return TimeSpan.FromSeconds( 10.0 );
				}
				else
				{
					m.SendLocalizedMessage( 502731 ); // You fail in your attempt to move unnoticed.
					m.RevealingAction();
				}
			}

			return TimeSpan.FromSeconds( 10.0 );
		}
	}
}
