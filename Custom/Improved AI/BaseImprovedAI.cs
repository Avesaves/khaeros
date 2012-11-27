using System;
using Server.Mobiles;
using Server.Misc;

namespace Server.Misc.ImprovedAI
{
	public class BaseImprovedAI : object
	{
		#region Stance AI
		public virtual void HandleStance( BaseCreature bc )
		{
			if( bc.Warmode && bc.SetStance != null && bc.Stance.GetType() != bc.SetStance.GetType() )
				ChangeToSetStance(bc);
			
			else if( !bc.Warmode && bc.Stance.FeatLevel > 0 )
				TurnStanceOff(bc);
		}
		
		public virtual void ChangeToSetStance( BaseCreature bc )
		{
			bc.Stance = bc.SetStance;
		}
		
		public virtual void TurnStanceOff( BaseCreature bc )
		{
			bc.Stance = null;
		}
		#endregion
		
		#region Maneuver AI
		public virtual void HandleManeuver( BaseCreature bc )
		{
			if( bc.Warmode && bc.SetManeuver != null && bc.CombatManeuver.GetType() != bc.SetManeuver.GetType() && bc.CanUseSpecial )
				ChangeToSetManeuver(bc);
		}
		
		public virtual void ChangeToSetManeuver( BaseCreature bc )
		{
			bc.CombatManeuver = bc.SetManeuver;
			bc.OffensiveFeat = bc.CombatManeuver.ListedName;
			bc.ManeuverAccuracyBonus = bc.CombatManeuver.AccuracyBonus * bc.CombatManeuver.FeatLevel;
    		bc.ManeuverDamageBonus = bc.CombatManeuver.DamageBonus * bc.CombatManeuver.FeatLevel;
		}
		#endregion
		
		#region Rage AI
		public virtual void HandleRage( BaseCreature bc )
		{
			if( DateTime.Compare( DateTime.Now, bc.NextRage ) > 0 && bc.Feats.GetFeatLevel(FeatList.Rage) > 0 && bc.RageTimer == null && bc.Warmode && bc.Hits < (int)(bc.HitsMax * 0.75) )
				TurnRageOn(bc);
		}
		
		public virtual void TurnRageOn( BaseCreature bc )
		{
			bc.RageTimer = new Server.Commands.LevelSystemCommands.RageTimer( bc, bc.Feats.GetFeatLevel(FeatList.Rage) );
            bc.RageFeatLevel = bc.Feats.GetFeatLevel(FeatList.Rage);
            bc.RageTimer.Start();
		}
		#endregion
		
		public BaseImprovedAI()
		{
		}
	}
}
