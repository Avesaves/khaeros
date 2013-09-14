using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Commands;

namespace Server.Misc
{
	public class FeatSystem
	{
		public static void FeatTool( PlayerMobile m, int feat, string currentpage )
		{
			/*string featname = "";
			
			switch ( currentpage )
			{
				case "bountyhunter": 
				{
					switch( feat )
					{
						case 1: featname = "Shield Bash"; m.Feats.GetFeatLevel(FeatList.ShieldBash) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ShieldBash), featname, SkillName.Parry, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 2: featname = "Cleave"; m.Feats.GetFeatLevel(FeatList.Cleave) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cleave), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 3: featname = "Armour Focus"; m.Feats.GetFeatLevel(FeatList.ArmourFocus) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourFocus), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 4: featname = "Crippling Blow"; m.Feats.GetFeatLevel(FeatList.CripplingBlow) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CripplingBlow), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 5: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 6: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 7: featname = "Back to Back"; m.Feats.GetFeatLevel(FeatList.BackToBack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BackToBack), featname, SkillName.Tactics, 40.0, 30.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawInt, 20, 5 ); break;
						case 8: featname = "Rope Trick"; m.Feats.GetFeatLevel(FeatList.RopeTrick) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RopeTrick), featname, SkillName.Tactics, 25.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawDex, 25, 25 ); break;
						case 9: featname = "Non-Lethal Traps"; m.Feats.GetFeatLevel(FeatList.NonLethalTraps) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.NonLethalTraps), featname, SkillName.ArmDisarmTraps, 45.0, 15.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 10: featname = "Escort Prisoner"; m.Feats.GetFeatLevel(FeatList.EscortPrisoner) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EscortPrisoner), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, m.RawInt, 20, 5 ); break;
						case 11: featname = "Focused Attack"; m.Feats.GetFeatLevel(FeatList.FocusedAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 12: featname = "Flurry of Blows"; m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlurryOfBlows), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 25, 25 ); break;
						case 13: featname = "Enhanced Tracking"; m.Feats.GetFeatLevel(FeatList.EnhancedTracking) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedTracking), featname, SkillName.Tracking, 100.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 14: featname = "Alertness"; m.Feats.GetFeatLevel(FeatList.Alertness) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Alertness), featname, SkillName.DetectHidden, 100.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 15: featname = "Intimidate"; m.Feats.GetFeatLevel(FeatList.Intimidate) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Intimidate), featname, SkillName.Tactics, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, m.RawInt, 40, 5 ); break;
						case 16: featname = "Throwing Mastery"; m.Feats.GetFeatLevel(FeatList.ThrowingMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ThrowingMastery), featname, SkillName.Throwing, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "dragoon": 
				{
					switch( feat )
					{
						case 1: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 2: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 3: featname = "Cleave"; m.Feats.GetFeatLevel(FeatList.Cleave) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cleave), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 4: featname = "Armour Focus"; m.Feats.GetFeatLevel(FeatList.ArmourFocus) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourFocus), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 5: featname = "Critical Strike"; m.Feats.GetFeatLevel(FeatList.CriticalStrike) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CriticalStrike), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 6: featname = "Focused Attack"; m.Feats.GetFeatLevel(FeatList.FocusedAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 7: featname = "Flurry of Blows"; m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlurryOfBlows), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 25, 25 ); break;
						case 8: featname = "Greatweapon Fighting"; m.Feats.GetFeatLevel(FeatList.GreatweaponFighting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GreatweaponFighting), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 9: featname = "Mounted Charge"; m.Feats.GetFeatLevel(FeatList.MountedCharge) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedCharge), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Riding, 70.0, 10.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 10: featname = "Polearms Mastery"; m.Feats.GetFeatLevel(FeatList.PolearmsMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PolearmsMastery), featname, SkillName.Polearms, 100.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 11: featname = "Mounted Momentum"; m.Feats.GetFeatLevel(FeatList.MountedMomentum) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedMomentum), featname, SkillName.Riding, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 12: featname = "Mounted Combat"; m.Feats.GetFeatLevel(FeatList.MountedCombat) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedCombat), featname, SkillName.Riding, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 13: featname = "Mounted Endurance"; m.Feats.GetFeatLevel(FeatList.MountedEndurance) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedEndurance), featname, SkillName.Riding, 25.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, 0, 0, 0 ); break;
						case 14: featname = "Mounted Defence"; m.Feats.GetFeatLevel(FeatList.MountedDefence) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedDefence), featname, SkillName.Parry, 60.0, 20.0, SkillName.Riding, 60.0, 20.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Unhorse"; m.Feats.GetFeatLevel(FeatList.Unhorse) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Unhorse), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.PolearmsMastery), 1, 20, 5, m.RawInt, 20, 5 ); break;
						case 16: featname = "Defensive Stance"; m.Feats.GetFeatLevel(FeatList.DefensiveStance) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DefensiveStance), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Parry, 60.0, 20.0, 0, 0, 10, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "berserker": 
				{
					switch( feat )
					{
						case 1: featname = "Shield Bash"; m.Feats.GetFeatLevel(FeatList.ShieldBash) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ShieldBash), featname, SkillName.Parry, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 2: featname = "Cleave"; m.Feats.GetFeatLevel(FeatList.Cleave) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cleave), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 3: featname = "Armour Focus"; m.Feats.GetFeatLevel(FeatList.ArmourFocus) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourFocus), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 4: featname = "Focused Attack"; m.Feats.GetFeatLevel(FeatList.FocusedAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 5: featname = "Flurry of Blows"; m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlurryOfBlows), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 25, 25 ); break;
						case 6: featname = "Crippling Blow"; m.Feats.GetFeatLevel(FeatList.CripplingBlow) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CripplingBlow), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 7: featname = "Savage Strike"; m.Feats.GetFeatLevel(FeatList.SavageStrike) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SavageStrike), featname, SkillName.Anatomy, 50.0, 10.0, SkillName.Tactics, 50.0, 25.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 8: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 9: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 10: featname = "Greatweapon Fighting"; m.Feats.GetFeatLevel(FeatList.GreatweaponFighting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GreatweaponFighting), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 11: featname = "Damage Ignore"; m.Feats.GetFeatLevel(FeatList.DamageIgnore) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DamageIgnore), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStam, 90, 5 ); break;
						case 12: featname = "Rage"; m.Feats.GetFeatLevel(FeatList.Rage) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Rage), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 13: featname = "Tireless Rage"; m.Feats.GetFeatLevel(FeatList.TirelessRage) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.TirelessRage), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.Rage), 1, 10, 10, m.RawStam, 90, 5 ); break;
						case 14: featname = "Defensive Fury"; m.Feats.GetFeatLevel(FeatList.DefensiveFury) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DefensiveFury), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.Rage), 3, 25, 5, m.RawStr, 90, 5 ); break;
						case 15: featname = "Fast Healing"; m.Feats.GetFeatLevel(FeatList.FastHealing) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FastHealing), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawHits, 90, 5 ); break;
						case 16: featname = "Throwing Mastery"; m.Feats.GetFeatLevel(FeatList.ThrowingMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ThrowingMastery), featname, SkillName.Throwing, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
					}
					
					break;
				}
				
				case "martialartist": 
				{
					switch( feat )
					{
						case 1: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 2: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 3: featname = "Back to Back"; m.Feats.GetFeatLevel(FeatList.BackToBack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BackToBack), featname, SkillName.Tactics, 40.0, 30.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawInt, 20, 5 ); break;
						case 4: featname = "Evade"; m.Feats.GetFeatLevel(FeatList.Evade) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Evade), featname, SkillName.Dodge, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 5: featname = "Enhanced Dodge"; m.Feats.GetFeatLevel(FeatList.EnhancedDodge) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedDodge), featname, SkillName.Dodge, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 6: featname = "Stunning Blow"; m.Feats.GetFeatLevel(FeatList.StunningBlow) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.StunningBlow), featname, SkillName.Anatomy, 50.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 7: featname = "Catch Projectiles"; m.Feats.GetFeatLevel(FeatList.CatchProjectiles) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CatchProjectiles), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, m.RawDex, 90, 0 ); break;
						case 8: featname = "Technique"; m.Feats.GetFeatLevel(FeatList.Technique) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Technique), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Tactics, 60.0, 20.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 9: featname = "Eye Raking"; m.Feats.GetFeatLevel(FeatList.EyeRaking) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EyeRaking), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 10: featname = "Pure Dodge"; m.Feats.GetFeatLevel(FeatList.PureDodge) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PureDodge), featname, SkillName.Dodge, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 11: featname = "Throat Strike"; m.Feats.GetFeatLevel(FeatList.ThroatStrike) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ThroatStrike), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: featname = "Racial Fighting Style"; m.Feats.GetFeatLevel(FeatList.RacialFightingStyle) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialFightingStyle), featname, SkillName.UnarmedFighting, 100.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 13: featname = "Disarm"; m.Feats.GetFeatLevel(FeatList.Disarm) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Disarm), featname, SkillName.UnarmedFighting, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 14: featname = "Dismount"; m.Feats.GetFeatLevel(FeatList.Dismount) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Dismount), featname, SkillName.UnarmedFighting, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 15: featname = "Buildup"; m.Feats.GetFeatLevel(FeatList.Buildup) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Buildup), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 16: featname = "Martial Offence"; m.Feats.GetFeatLevel(FeatList.MartialOffence) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MartialOffence), featname, SkillName.UnarmedFighting, 60.0, 20.0, SkillName.Tactics, 60.0, 20.0, 0, 0, 20, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "weaponspecialist": 
				{
					switch( feat )
					{
						case 1: featname = "Cleave"; m.Feats.GetFeatLevel(FeatList.Cleave) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cleave), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 2: featname = "Focused Attack"; m.Feats.GetFeatLevel(FeatList.FocusedAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 3: featname = "Flurry of Blows"; m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlurryOfBlows), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 25, 25 ); break;
						case 4: featname = "Flashy Attack"; m.Feats.GetFeatLevel(FeatList.FlashyAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlashyAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawDex, 50, 25 ); break;
						case 5: featname = "Crippling Blow"; m.Feats.GetFeatLevel(FeatList.CripplingBlow) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CripplingBlow), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 6: featname = "Critical Strike"; m.Feats.GetFeatLevel(FeatList.CriticalStrike) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CriticalStrike), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 7: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 8: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 9: featname = "Back to Back"; m.Feats.GetFeatLevel(FeatList.BackToBack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BackToBack), featname, SkillName.Tactics, 40.0, 30.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawInt, 20, 5 ); break;
						case 10: featname = "Fighting Style"; m.Feats.GetFeatLevel(FeatList.FightingStyle) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FightingStyle), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 11: featname = "Weapon Specialization"; m.Feats.GetFeatLevel(FeatList.WeaponSpecialization) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.WeaponSpecialization), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.FightingStyle), 3, 20, 5, m.RawInt, 40, 5 ); break;
						case 12: featname = "Weapon Parrying"; m.Feats.GetFeatLevel(FeatList.WeaponParrying) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.WeaponParrying), featname, SkillName.Parry, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 13: featname = "Armour Focus"; m.Feats.GetFeatLevel(FeatList.ArmourFocus) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourFocus), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 14: featname = "Second Specialization"; m.Feats.GetFeatLevel(FeatList.SecondSpecialization) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SecondSpecialization), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.WeaponSpecialization), 3, 0, 0, 0, 0, 0 ); break;
						case 15: featname = "Pure Dodge"; m.Feats.GetFeatLevel(FeatList.PureDodge) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PureDodge), featname, SkillName.Dodge, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 16: featname = "Enhanced Dodge"; m.Feats.GetFeatLevel(FeatList.EnhancedDodge) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedDodge), featname, SkillName.Dodge, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "archer": 
				{
					switch( feat )
					{
						case 1: featname = "Bow Mastery"; m.Feats.GetFeatLevel(FeatList.BowMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BowMastery), featname, SkillName.Archery, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 2: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 3: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 4: featname = "Critical Shot"; m.Feats.GetFeatLevel(FeatList.CriticalShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CriticalShot), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Tactics, 50.0, 25.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 5: featname = "Crippling Shot"; m.Feats.GetFeatLevel(FeatList.CripplingShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CripplingShot), featname, SkillName.Anatomy, 25.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 6: featname = "Focused Shot"; m.Feats.GetFeatLevel(FeatList.FocusedShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedShot), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 7: featname = "Swift Shot"; m.Feats.GetFeatLevel(FeatList.SwiftShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SwiftShot), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 8: featname = "Hail of Arrows"; m.Feats.GetFeatLevel(FeatList.HailOfArrows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HailOfArrows), featname, SkillName.Archery, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 9: featname = "Far Shot"; m.Feats.GetFeatLevel(FeatList.FarShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FarShot), featname, SkillName.Archery, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 10: featname = "Traveling Shot"; m.Feats.GetFeatLevel(FeatList.TravelingShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.TravelingShot), featname, SkillName.Archery, 100.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, m.RawInt, 20, 5 ); break;
						case 11: featname = "Mounted Archery"; m.Feats.GetFeatLevel(FeatList.MountedArchery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MountedArchery), featname, SkillName.Riding, 65.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: featname = "Throwing Mastery"; m.Feats.GetFeatLevel(FeatList.ThrowingMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ThrowingMastery), featname, SkillName.Throwing, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 13: featname = "Reusable Ammunition"; m.Feats.GetFeatLevel(FeatList.ReusableAmmunition) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ReusableAmmunition), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 1, 10, 5, 0, 0, 0 ); break;
						case 14: featname = "Crossbow Mastery"; m.Feats.GetFeatLevel(FeatList.CrossbowMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CrossbowMastery), featname, SkillName.Archery, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Aimed Shot"; m.Feats.GetFeatLevel(FeatList.AimedShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AimedShot), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.CrossbowMastery), 3, 20, 5, 0, 0, 0 ); break;
						case 16: featname = "Quick Traveling Shot"; m.Feats.GetFeatLevel(FeatList.QuickTravelingShot) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickTravelingShot), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 30, 5, m.Feats.GetFeatLevel(FeatList.TravelingShot), 3, 0 ); break;
					}
							
					break;
				}
					
				case "fighter": 
				{
					switch( feat )
					{
						case 1: featname = "Shield Bash"; m.Feats.GetFeatLevel(FeatList.ShieldBash) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ShieldBash), featname, SkillName.Parry, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 2: featname = "Deflect Projectiles"; m.Feats.GetFeatLevel(FeatList.DeflectProjectiles) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DeflectProjectiles), featname, SkillName.Parry, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 3: featname = "Cleave"; m.Feats.GetFeatLevel(FeatList.Cleave) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cleave), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 4: featname = "Armour Focus"; m.Feats.GetFeatLevel(FeatList.ArmourFocus) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourFocus), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 5: featname = "Focused Attack"; m.Feats.GetFeatLevel(FeatList.FocusedAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FocusedAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawInt, 20, 5 ); break;
						case 6: featname = "Flurry of Blows"; m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlurryOfBlows), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 25, 25 ); break;
						case 7: featname = "Flashy Attack"; m.Feats.GetFeatLevel(FeatList.FlashyAttack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FlashyAttack), featname, SkillName.Tactics, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, m.RawDex, 50, 25 ); break;
						case 8: featname = "Crippling Blow"; m.Feats.GetFeatLevel(FeatList.CripplingBlow) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CripplingBlow), featname, SkillName.Anatomy, 25.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 9: featname = "Critical Strike"; m.Feats.GetFeatLevel(FeatList.CriticalStrike) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CriticalStrike), featname, SkillName.Anatomy, 25.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 10: featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 11: featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 12: featname = "Greatweapon Fighting"; m.Feats.GetFeatLevel(FeatList.GreatweaponFighting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GreatweaponFighting), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 13: featname = "Shield Mastery"; m.Feats.GetFeatLevel(FeatList.ShieldMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ShieldMastery), featname, SkillName.Parry, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 14: featname = "Feint"; m.Feats.GetFeatLevel(FeatList.Feint) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Feint), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Back to Back"; m.Feats.GetFeatLevel(FeatList.BackToBack) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BackToBack), featname, SkillName.Tactics, 40.0, 30.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawInt, 20, 5); break;
						case 16: featname = "Defensive Stance"; m.Feats.GetFeatLevel(FeatList.DefensiveStance) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DefensiveStance), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Parry, 60.0, 20.0, 0, 0, 10, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "metalworker": 
				{
					switch( feat )
					{
						case 1: featname = "Gem Embedding"; m.Feats.GetFeatLevel(FeatList.GemEmbedding) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GemEmbedding), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.GemHarvesting), 3, 0, 0, m.RawInt, 60, 20 ); break;
						case 2: featname = "Advanced Mining"; m.Feats.GetFeatLevel(FeatList.AdvancedMining) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AdvancedMining), featname, SkillName.Mining, 90.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 3: featname = "Armour Enameling"; m.Feats.GetFeatLevel(FeatList.ArmourEnameling) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmourEnameling), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 4: featname = "Heavy Lifting"; m.Feats.GetFeatLevel(FeatList.HeavyLifting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyLifting), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 60, 20 ); break;
						case 5: featname = "Durable Crafts"; m.Feats.GetFeatLevel(FeatList.DurableCrafts) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DurableCrafts), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 6: featname = "Racial Enameling"; m.Feats.GetFeatLevel(FeatList.RacialEnameling) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialEnameling), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.ArmourEnameling), 3, 20, 5, 0, 0, 0 ); break;
						case 7: featname = "Racial Resource"; m.Feats.GetFeatLevel(FeatList.RacialResource) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialResource), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 8: featname = "Potter"; m.Feats.GetFeatLevel(FeatList.Potter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Potter), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 9: featname = "Glass Blower"; m.Feats.GetFeatLevel(FeatList.GlassBlower) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GlassBlower), featname, SkillName.Craftsmanship, 50.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 0, 5, 0, 0, 0 ); break;
						case 10: featname = "Painter"; m.Feats.GetFeatLevel(FeatList.Painter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Painter), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 11: featname = "Sculptor"; m.Feats.GetFeatLevel(FeatList.Sculptor) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sculptor), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Mining, 50.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: featname = "Verify Currency"; m.Feats.GetFeatLevel(FeatList.VerifyCurrency) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.VerifyCurrency), featname, SkillName.Appraisal, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 13: featname = "Gem Harvesting"; m.Feats.GetFeatLevel(FeatList.GemHarvesting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GemHarvesting), featname, SkillName.Mining, 25.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, m.RawInt, 30, 10 ); break;
						case 14: featname = "Masterwork"; m.Feats.GetFeatLevel(FeatList.Masterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Masterwork), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Renowned Masterwork"; m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RenownedMasterwork), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.Masterwork), 3, 20, 5, 0, 0, 0 ); break;
						case 16: featname = "Jewelry Crafting"; m.Feats.GetFeatLevel(FeatList.JewelryCrafting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.JewelryCrafting), featname, SkillName.Tinkering, 30.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "woodworker": 
				{
					switch( feat )
					{
						case 1: featname = "Shipwright"; m.Feats.GetFeatLevel(FeatList.Shipwright) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Shipwright), featname, SkillName.Carpentry, 90.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
						case 2: featname = "Advanced Lumberjack"; m.Feats.GetFeatLevel(FeatList.AdvancedLumberjacking) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AdvancedLumberjacking), featname, SkillName.Lumberjacking, 90.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 3: featname = "Wood Staining"; m.Feats.GetFeatLevel(FeatList.WoodStaining) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.WoodStaining), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 4: featname = "Heavy Lifting"; m.Feats.GetFeatLevel(FeatList.HeavyLifting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyLifting), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 60, 20 ); break;
						case 5: featname = "Durable Crafts"; m.Feats.GetFeatLevel(FeatList.DurableCrafts) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DurableCrafts), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 6: featname = "Racial Staining"; m.Feats.GetFeatLevel(FeatList.RacialStaining) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialStaining), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.WoodStaining), 3, 20, 5, 0, 0, 0 ); break;
						case 7: featname = "Racial Resource"; m.Feats.GetFeatLevel(FeatList.RacialResource) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialResource), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 8: featname = "Potter"; m.Feats.GetFeatLevel(FeatList.Potter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Potter), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 9: featname = "Glass Blower"; m.Feats.GetFeatLevel(FeatList.GlassBlower) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GlassBlower), featname, SkillName.Craftsmanship, 50.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 0, 5, 0, 0, 0 ); break;
						case 10: featname = "Painter"; m.Feats.GetFeatLevel(FeatList.Painter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Painter), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 11: featname = "Sculptor"; m.Feats.GetFeatLevel(FeatList.Sculptor) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sculptor), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Mining, 50.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: featname = "Verify Currency"; m.Feats.GetFeatLevel(FeatList.VerifyCurrency) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.VerifyCurrency), featname, SkillName.Appraisal, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 13: featname = "Gem Harvesting"; m.Feats.GetFeatLevel(FeatList.GemHarvesting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GemHarvesting), featname, SkillName.Mining, 25.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, m.RawInt, 30, 10 ); break;
						case 14: featname = "Masterwork"; m.Feats.GetFeatLevel(FeatList.Masterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Masterwork), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Renowned Masterwork"; m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RenownedMasterwork), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.Masterwork), 3, 20, 5, 0, 0, 0 ); break;
						case 16: featname = "Jewelry Crafting"; m.Feats.GetFeatLevel(FeatList.JewelryCrafting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.JewelryCrafting), featname, SkillName.Tinkering, 30.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
				
				case "tailor": 
				{
					switch( feat )
					{
						case 1: featname = "Enhanced Harvesting"; m.Feats.GetFeatLevel(FeatList.EnhancedHarvesting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedHarvesting), featname, SkillName.Tailoring, 90.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 2: featname = "Improved Skinning"; m.Feats.GetFeatLevel(FeatList.ImprovedSkinning) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ImprovedSkinning), featname, SkillName.Tailoring, 90.0, 5.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 3: featname = "Advanced Dying"; m.Feats.GetFeatLevel(FeatList.AdvancedDying) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AdvancedDying), featname, SkillName.Craftsmanship, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 10, 0, 0, 0 ); break;
						case 4: featname = "Heavy Lifting"; m.Feats.GetFeatLevel(FeatList.HeavyLifting) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyLifting), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 60, 20 ); break;
						case 5: featname = "Durable Crafts"; m.Feats.GetFeatLevel(FeatList.DurableCrafts) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DurableCrafts), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 15, 5, 0, 0, 0 ); break;
						case 6: featname = "Racial Dyes"; m.Feats.GetFeatLevel(FeatList.RacialDyes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialDyes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.AdvancedDying), 3, 20, 5, 0, 0, 0 ); break;
						case 7: featname = "Racial Resource"; m.Feats.GetFeatLevel(FeatList.RacialResource) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialResource), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 8: featname = "Potter"; m.Feats.GetFeatLevel(FeatList.Potter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Potter), featname, SkillName.Craftsmanship, 80.0, 10.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 9: featname = "Glass Blower"; m.Feats.GetFeatLevel(FeatList.GlassBlower) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GlassBlower), featname, SkillName.Craftsmanship, 50.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 0, 5, 0, 0, 0 ); break;
						case 10: featname = "Painter"; m.Feats.GetFeatLevel(FeatList.Painter) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Painter), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 11: featname = "Sculptor"; m.Feats.GetFeatLevel(FeatList.Sculptor) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sculptor), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Mining, 50.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: featname = "Verify Currency"; m.Feats.GetFeatLevel(FeatList.VerifyCurrency) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.VerifyCurrency), featname, SkillName.Appraisal, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 13: featname = "Hair Styling"; m.Feats.GetFeatLevel(FeatList.HairStyling) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HairStyling), featname, SkillName.Tailoring, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 14: featname = "Masterwork"; m.Feats.GetFeatLevel(FeatList.Masterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Masterwork), featname, SkillName.Craftsmanship, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 15: featname = "Renowned Masterwork"; m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RenownedMasterwork), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.Masterwork), 3, 20, 5, 0, 0, 0 ); break;
						case 16: featname = "Leather Dying"; m.Feats.GetFeatLevel(FeatList.LeatherDying) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LeatherDying), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, m.Feats.GetFeatLevel(FeatList.AdvancedDying), 3, 0, 0, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "cleric": 
				{
					switch( feat )
					{
						case 1: featname = "First Racial Power"; m.Feats.GetFeatLevel(FeatList.SummonProtector) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SummonProtector), featname, 	SkillName.Faith, 50.0, 25.0, 	SkillName.Invocation, 50.0, 25.0, 	0, 0, 						10, 10, 0, 0, 0 ); break;
						case 2: featname = "Second Racial Power"; m.Feats.GetFeatLevel(FeatList.DivineConsecration) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DivineConsecration), featname, 	SkillName.Faith, 50.0, 25.0, 	SkillName.Invocation, 50.0, 25.0, 	m.Feats.GetFeatLevel(FeatList.ConsecrateItem), 3, 	20, 5, 0, 0, 0 ); break;
						case 3: featname = "Sanctuary"; m.Feats.GetFeatLevel(FeatList.Sanctuary) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sanctuary), featname, 			SkillName.Faith, 50.0, 25.0, 	SkillName.Magery, 	  0.0, 0.0, 	0, 0, 						5, 10, 0, 0, 0 ); break;
						case 4: featname = "Aura of Protection"; m.Feats.GetFeatLevel(FeatList.AuraOfProtection) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AuraOfProtection), featname, 	SkillName.Faith, 50.0, 25.0, 	SkillName.Magery,     0.0, 0.0, 	0, 0, 						5, 10, 0, 0, 0 ); break;
						case 5: featname = "Shield of Sacrifice"; m.Feats.GetFeatLevel(FeatList.ShieldOfSacrifice) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ShieldOfSacrifice), featname, 	SkillName.Faith, 100.0, 0.0, 	SkillName.Invocation, 0.0, 0.0, 	0, 0, 						15, 5, 0, 0, 0 ); break;
						case 6: featname = "Holy Strike"; m.Feats.GetFeatLevel(FeatList.HolyStrike) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HolyStrike), featname, 		SkillName.Faith, 100.0, 0.0, 	SkillName.Invocation, 100.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.HealWounds), 3, 		15, 5, 0, 0, 0 ); break;
						case 7: featname = "Sacred Blast"; m.Feats.GetFeatLevel(FeatList.SacredBlast) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SacredBlast), featname, 		SkillName.Faith, 100.0, 0.0, 	SkillName.Invocation, 100.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.InflictWounds), 3, 	15, 5, 0, 0, 0 ); break;
						case 8: featname = "Hold Person"; m.Feats.GetFeatLevel(FeatList.HoldPerson) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HoldPerson), featname, 		SkillName.Faith, 100.0, 0.0, 	SkillName.Invocation, 100.0, 0.0, 	0, 0, 						15, 5, 0, 0, 0 ); break;
						case 9: featname = "Mending"; m.Feats.GetFeatLevel(FeatList.Mending) += 						FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Mending), featname, 			SkillName.Faith, 25.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						0, 5, 0, 0, 0 ); break;
						case 10: featname = "Halo of Light"; m.Feats.GetFeatLevel(FeatList.HaloOfLight) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HaloOfLight), featname, 		SkillName.Faith, 25.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						5, 5, 0, 0, 0 ); break;
						case 11: featname = "Consecrate Item"; m.Feats.GetFeatLevel(FeatList.ConsecrateItem) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ConsecrateItem), featname, 	SkillName.Faith, 50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						10, 5, 0, 0, 0 ); break;
						case 12: featname = "Curse"; m.Feats.GetFeatLevel(FeatList.Curse) += 							FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Curse), featname, 				SkillName.Faith, 50.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						10, 5, 0, 0, 0 ); break;
						case 13: featname = "Bless"; m.Feats.GetFeatLevel(FeatList.Bless) += 							FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Bless), featname, 				SkillName.Faith, 50.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						10, 5, 0, 0, 0 ); break;
						case 14: featname = "Cure Famine"; m.Feats.GetFeatLevel(FeatList.CureFamine) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CureFamine), featname, 		SkillName.Faith, 25.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 		0, 0, 						0, 5, 0, 0, 0 ); break;
						case 15: featname = "Heal Wounds"; m.Feats.GetFeatLevel(FeatList.HealWounds) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HealWounds), featname, 		SkillName.Faith, 50.0, 25.0, 	SkillName.Invocation,  50.0, 25.0, 	0, 0, 						5, 5, 0, 0, 0 ); break;
						case 16: featname = "Inflict Wounds"; m.Feats.GetFeatLevel(FeatList.InflictWounds) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.InflictWounds), featname, 		SkillName.Faith, 50.0, 25.0, 	SkillName.Invocation,  50.0, 25.0, 	0, 0, 						5, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "thief": 
				{
					switch( feat )
					{
						case 1: 	featname = "Enhanced Dodge"; 	m.Feats.GetFeatLevel(FeatList.EnhancedDodge) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedDodge), 	featname, SkillName.Dodge, 			60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 10, 	0, 0, 0 ); break;
						case 2: 	featname = "Evade"; 			m.Feats.GetFeatLevel(FeatList.Evade) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Evade), 			featname, SkillName.Dodge, 			50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0,					 	10, 5,		0, 0, 0 ); break;
						case 3: 	featname = "Armoured Dodge"; 	m.Feats.GetFeatLevel(FeatList.ArmouredDodge) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmouredDodge), 	featname, SkillName.Dodge, 			100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.EnhancedDodge), 3, 	30, 5, 		0, 0, 0 ); break;
						case 4: 	featname = "Enhanced Stealth"; 	m.Feats.GetFeatLevel(FeatList.EnhancedStealth) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedStealth), 	featname, SkillName.Stealth, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		0, 0, 0 ); break;
						case 5: 	featname = "Armoured Stealth"; 	m.Feats.GetFeatLevel(FeatList.ArmouredStealth) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmouredStealth), 	featname, SkillName.Stealth, 		100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.EnhancedStealth), 3, 30, 5, 		0, 0, 0 ); break;
						case 6: 	featname = "Disguise Kit"; 		m.Feats.GetFeatLevel(FeatList.DisguiseKit) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseKit), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		m.RawInt, 50, 10 ); break;
						case 7: 	featname = "Disguise Others"; 	m.Feats.GetFeatLevel(FeatList.DisguiseOthers) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseOthers), 	featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.DisguiseKit), 3, 	30, 5, 		m.RawInt, 70, 10 ); break;
						case 8: 	featname = "Shorthand"; 		m.Feats.GetFeatLevel(FeatList.Shorthand) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Shorthand), 		featname, SkillName.Linguistics, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		m.RawInt, 70, 10 ); break;
						case 9: 	featname = "Plant Evidence"; 	m.Feats.GetFeatLevel(FeatList.PlantEvidence) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PlantEvidence), 	featname, SkillName.Stealing, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		0, 0, 0 ); break;
						case 10: 	featname = "Counterfeiting";	m.Feats.GetFeatLevel(FeatList.Counterfeiting) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Counterfeiting), 	featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5,		m.RawInt, 50, 10 ); break;
						case 11: 	featname = "Judge Wealth"; 		m.Feats.GetFeatLevel(FeatList.JudgeWealth) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.JudgeWealth), 		featname, SkillName.Snooping, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		m.RawInt, 30, 10 ); break;
						case 12: 	featname = "Stash"; 			m.Feats.GetFeatLevel(FeatList.Stash) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Stash), 			featname, SkillName.Hiding, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		0, 0, 0 ); break;
						case 13: 	featname = "Hideout"; 			m.Feats.GetFeatLevel(FeatList.Hideout) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Hideout), 			featname, SkillName.Camping, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						30, 5, 		0, 0, 0 ); break;
						case 14: 	featname = "Cutpurse"; 			m.Feats.GetFeatLevel(FeatList.Cutpurse) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cutpurse), 		featname, SkillName.Stealing, 		90.0, 5.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		0, 0, 0 ); break;
						case 15: 	featname = "Pet Stealing"; 		m.Feats.GetFeatLevel(FeatList.PetStealing) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PetStealing), 		featname, SkillName.Stealing, 		90.0, 5.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		0, 0, 0 ); break;
						case 16: 	featname = "Locksmith"; 		m.Feats.GetFeatLevel(FeatList.Locksmith) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Locksmith), 		featname, SkillName.Lockpicking, 	90.0, 5.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						25, 5, 		0, 0, 0 ); break;
						
					}
					
					break;
				}
					
				case "bard": 
				{
					switch( feat )
					{
						case 1: 	featname = "Enhanced Dodge"; 		m.Feats.GetFeatLevel(FeatList.EnhancedDodge) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedDodge), 		featname, SkillName.Dodge, 			60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 10, 	0, 0, 0 ); break;
						case 2: 	featname = "Evade"; 				m.Feats.GetFeatLevel(FeatList.Evade) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Evade), 				featname, SkillName.Dodge, 			50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0,					 	10, 5,		0, 0, 0 ); break;
						case 3: 	featname = "Enhanced Stealth"; 		m.Feats.GetFeatLevel(FeatList.EnhancedStealth) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedStealth), 		featname, SkillName.Stealth, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		0, 0, 0 ); break;
						case 4: 	featname = "Disguise Kit"; 			m.Feats.GetFeatLevel(FeatList.DisguiseKit) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseKit), 			featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		m.RawInt, 50, 10 ); break;
						case 5: 	featname = "Disguise Others"; 		m.Feats.GetFeatLevel(FeatList.DisguiseOthers) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseOthers), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.DisguiseKit), 3, 	30, 5, 		m.RawInt, 70, 10 ); break;
						case 6: 	featname = "Shorthand"; 			m.Feats.GetFeatLevel(FeatList.Shorthand) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Shorthand), 			featname, SkillName.Linguistics, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		m.RawInt, 70, 10 ); break;
						case 7: 	featname = "Ventriloquism"; 		m.Feats.GetFeatLevel(FeatList.Ventriloquism) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Ventriloquism), 		featname, SkillName.Linguistics, 	60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		0, 0, 0 ); break;
						case 8: 	featname = "Drums of War";			m.Feats.GetFeatLevel(FeatList.DrumsOfWar) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DrumsOfWar), 			featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5,  5,		0, 0, 0 ); break;
						case 9: 	featname = "Lingering Note"; 		m.Feats.GetFeatLevel(FeatList.LingeringCommand) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LingeringCommand), 		featname, SkillName.Musicianship, 	100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 10, 	0, 0, 0 ); break;
						case 10: 	featname = "Widespread Note"; 		m.Feats.GetFeatLevel(FeatList.WidespreadCommand) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.WidespreadCommand),		featname, SkillName.Musicianship, 	100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 10, 	0, 0, 0 ); break;
						case 11: 	featname = "Song of Retreat"; 		m.Feats.GetFeatLevel(FeatList.ExpeditiousRetreat) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ExpeditiousRetreat), 		featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		0, 0, 0 ); break;
						case 12: 	featname = "Song of Fortitude"; 	m.Feats.GetFeatLevel(FeatList.InspireFortitude) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.InspireFortitude), 		featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		0, 0, 0 ); break;
						case 13: 	featname = "Song of Heroes"; 		m.Feats.GetFeatLevel(FeatList.InspireHeroics) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.InspireHeroics), 		featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		0, 0, 0 ); break;
						case 14: 	featname = "Song of Martyrs"; 		m.Feats.GetFeatLevel(FeatList.InspireResilience) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.InspireResilience), 		featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 	5, 		0, 0, 0 ); break;
						case 15: 	featname = "Combined Songs I"; 		m.Feats.GetFeatLevel(FeatList.CombinedCommandsI) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CombinedCommandsI), 		featname, SkillName.Musicianship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 	0, 0, 0 ); break;
						case 16: 	featname = "Combined Songs II"; 	m.Feats.GetFeatLevel(FeatList.CombinedCommandsII) +=		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.CombinedCommandsII),		featname, SkillName.Musicianship, 	100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.CombinedCommandsI), 3,	25, 5, 	0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "scholar": 
				{
					switch( feat )
					{
						case 1: 	featname = "Lower Side Effects";	m.Feats.GetFeatLevel(FeatList.LowerSideEffects) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LowerSideEffects), 	featname, SkillName.Craftsmanship, 	60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 10, 	0, 0, 0 ); break;
						case 2: 	featname = "Fireworks"; 			m.Feats.GetFeatLevel(FeatList.Fireworks) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Fireworks), 			featname, SkillName.Alchemy, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0,					 	10, 5,		0, 0, 0 ); break;
						case 3: 	featname = "Black Powder"; 			m.Feats.GetFeatLevel(FeatList.BlackPowder) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BlackPowder), 			featname, SkillName.Alchemy, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		0, 0, 0 ); break;
						case 4: 	featname = "Herbal Gathering"; 		m.Feats.GetFeatLevel(FeatList.HerbalGathering) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HerbalGathering), 		featname, SkillName.HerbalLore, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		0, 0, 0 ); break;
						case 5: 	featname = "Heavy Lifting"; 		m.Feats.GetFeatLevel(FeatList.HeavyLifting) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyLifting), 		featname, SkillName.Magery, 		0.0,  0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0,					 	5,  5, 		m.RawStr, 60, 20 ); break;
						case 6: 	featname = "Business Mentor"; 		m.Feats.GetFeatLevel(FeatList.RacialStaining) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialStaining), 		featname, SkillName.Magery, 		0.0,  0.0,	 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.RacialResource), 4,	20, 5, 		0, 0, 0 ); break;
						case 7: 	featname = "Racial Resource"; 		m.Feats.GetFeatLevel(FeatList.RacialResource) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialResource), 		featname, SkillName.Craftsmanship,	60.0, 20.0,		SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 8: 	featname = "Potter";				m.Feats.GetFeatLevel(FeatList.Potter) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Potter), 				featname, SkillName.Craftsmanship, 	80.0, 10.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5,		0, 0, 0 ); break;
						case 9: 	featname = "Glass Blower"; 			m.Feats.GetFeatLevel(FeatList.GlassBlower) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GlassBlower), 			featname, SkillName.Craftsmanship, 	50.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 5, 		0, 0, 0 ); break;
						case 10: 	featname = "Painter"; 				m.Feats.GetFeatLevel(FeatList.Painter) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Painter),				featname, SkillName.Craftsmanship, 	60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 11: 	featname = "Sculptor"; 				m.Feats.GetFeatLevel(FeatList.Sculptor) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sculptor), 			featname, SkillName.Craftsmanship, 	60.0, 20.0, 	SkillName.Mining, 50.0, 0.0, 	0, 0, 						10,	5, 		0, 0, 0 ); break;
						case 12: 	featname = "Verify Currency"; 		m.Feats.GetFeatLevel(FeatList.VerifyCurrency) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.VerifyCurrency), 		featname, SkillName.Appraisal, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10,	5, 		0, 0, 0 ); break;
						case 13: 	featname = "Oil Making"; 			m.Feats.GetFeatLevel(FeatList.OilMaking) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.OilMaking), 			featname, SkillName.Alchemy, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20,	5, 		0, 0, 0 ); break;
						case 14: 	featname = "Enhance Potion"; 		m.Feats.GetFeatLevel(FeatList.EnhancePotion) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancePotion), 		featname, SkillName.Craftsmanship, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20,	5, 		0, 0, 0 ); break;
						case 15: 	featname = "Professor";		 		m.Feats.GetFeatLevel(FeatList.Professor) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Professor), 			featname, SkillName.Magery, 		0.0,  0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.Teaching), 3, 		50, 0, 		m.RawInt, 50, 25 ); break;
						case 16: 	featname = "Cryptography"; 			m.Feats.GetFeatLevel(FeatList.Cryptography) +=			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Cryptography),			featname, SkillName.Magery, 		0.0,  0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0,						15, 5, 		m.RawInt, 50, 25 ); break;
					}
						
					break;
				}
				
				case "stableworker": 
				{
					switch( feat )
					{
						case 1: 	featname = "Heavy Lifting"; 	m.Feats.GetFeatLevel(FeatList.HeavyLifting) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyLifting), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 5, 		m.RawStr, 60, 20 ); break;
						case 2: 	featname = "Potter"; 			m.Feats.GetFeatLevel(FeatList.Potter) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Potter), 				featname, SkillName.Craftsmanship, 	80.0, 10.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 3: 	featname = "Glass Blower"; 		m.Feats.GetFeatLevel(FeatList.GlassBlower) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.GlassBlower), 			featname, SkillName.Craftsmanship, 	50.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 5, 		0, 0, 0 ); break;
						case 4: 	featname = "Painter"; 			m.Feats.GetFeatLevel(FeatList.Painter) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Painter), 				featname, SkillName.Craftsmanship, 	60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 5: 	featname = "Sculptor"; 			m.Feats.GetFeatLevel(FeatList.Sculptor) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Sculptor), 			featname, SkillName.Craftsmanship, 	60.0, 20.0, 	SkillName.Mining, 50.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 6: 	featname = "Verify Currency"; 	m.Feats.GetFeatLevel(FeatList.VerifyCurrency) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.VerifyCurrency), 		featname, SkillName.Appraisal, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						
						case 7: 	featname = "Wolf Breeding"; 	m.Feats.GetFeatLevel(FeatList.WolfBreeding) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.WolfBreeding), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.DogBreeding), 3, 	15, 5, 		0, 0, 0 ); break;
						case 8: 	featname = "Dog Breeding"; 		m.Feats.GetFeatLevel(FeatList.DogBreeding) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DogBreeding), 			featname, SkillName.AnimalHusbandry,50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 5, 		0, 0, 0 ); break;
						case 9: 	featname = "Horse Breeding"; 	m.Feats.GetFeatLevel(FeatList.HorseBreeding) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HorseBreeding), 		featname, SkillName.AnimalHusbandry,50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 10: 	featname = "Racial Mounts"; 	m.Feats.GetFeatLevel(FeatList.RacialMounts) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RacialMounts), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.HorseBreeding), 3, 	10, 5, 		0, 0, 0 ); break;
						case 11: 	featname = "Pet Feats"; 		m.Feats.GetFeatLevel(FeatList.PetFeats) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PetFeats), 			featname, SkillName.AnimalHusbandry,50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 12: 	featname = "Extra Pet Feats"; 	m.Feats.GetFeatLevel(FeatList.ExtraPetFeats) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ExtraPetFeats), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.PetFeats), 3, 		20, 5, 		0, 0, 0 ); break;
						case 13: 	featname = "Pet Evolution"; 	m.Feats.GetFeatLevel(FeatList.PetEvolution) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PetEvolution), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.PetFeats), 3, 		20, 5, 		0, 0, 0 ); break;
						case 14: 	featname = "Retrain Pet"; 		m.Feats.GetFeatLevel(FeatList.RetrainPet) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RetrainPet), 			featname, SkillName.Magery, 		0.0, 0.0,		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.PetFeats), 3,		20, 5, 		0, 0, 0 ); break;
						case 15: 	featname = "Animal Training"; 	m.Feats.GetFeatLevel(FeatList.AnimalTraining) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AnimalTraining), 		featname, SkillName.AnimalLore, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 5, 		0, 0, 0 ); break;
						case 16: 	featname = "Animal Control"; 	m.Feats.GetFeatLevel(FeatList.AnimalControl) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.AnimalControl), 		featname, SkillName.AnimalLore, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 10, 	0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "mage": 
				{
					switch( feat )
					{
						case 1: 	featname = "Damaging Effect"; 	m.Feats.GetFeatLevel(FeatList.DamagingEffect) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DamagingEffect), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 2: 	featname = "Ranged Effect"; 	m.Feats.GetFeatLevel(FeatList.RangedEffect) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RangedEffect), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 3: 	featname = "Explosive Effect"; 	m.Feats.GetFeatLevel(FeatList.ExplosiveEffect) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ExplosiveEffect), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 4: 	featname = "Recurrent Effect";  m.Feats.GetFeatLevel(FeatList.RecurrentEffect) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.RecurrentEffect), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 5: 	featname = "Chain Effect"; 		m.Feats.GetFeatLevel(FeatList.ChainEffect) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ChainEffect), 			featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Mining, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 6: 	featname = "Status Effect"; 	m.Feats.GetFeatLevel(FeatList.StatusEffect) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.StatusEffect), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						
						case 7: 	featname = "Life I"; 			m.Feats.GetFeatLevel(FeatList.LifeI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LifeI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 					 	0, 0, 		0, 0, 0 ); break;
						case 8: 	featname = "Death I"; 			m.Feats.GetFeatLevel(FeatList.DeathI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DeathI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 9: 	featname = "Mind I"; 			m.Feats.GetFeatLevel(FeatList.MindI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MindI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 10: 	featname = "Matter I"; 			m.Feats.GetFeatLevel(FeatList.MatterI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MatterI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 					 	0, 0,  		0, 0, 0 ); break;
						case 11: 	featname = "Time I"; 			m.Feats.GetFeatLevel(FeatList.TimeI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.TimeI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 12: 	featname = "Space I"; 			m.Feats.GetFeatLevel(FeatList.SpaceI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SpaceI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 				 		0, 0, 		0, 0, 0 ); break;
						case 13: 	featname = "Fate I"; 			m.Feats.GetFeatLevel(FeatList.FateI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FateI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 				 		0, 0,  		0, 0, 0 ); break;
						case 14: 	featname = "Prime I"; 			m.Feats.GetFeatLevel(FeatList.PrimeI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PrimeI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 15: 	featname = "Forces I";	 		m.Feats.GetFeatLevel(FeatList.ForcesI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ForcesI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 16: 	featname = "Spirit I"; 			m.Feats.GetFeatLevel(FeatList.SpiritI) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SpiritI), 				featname, SkillName.Meditation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 	 	0, 0, 0 ); break;
					}
					
					break;
				}
					
				case "mage expanded": 
				{
					switch( feat )
					{
						case 1: 	featname = "Enchant Weapon"; 	m.Feats.GetFeatLevel(FeatList.EnchantWeapon) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantWeapon), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 2: 	featname = "Enchant Armour"; 	m.Feats.GetFeatLevel(FeatList.EnchantArmour) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantArmour), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 3: 	featname = "Enchant Ring";	 	m.Feats.GetFeatLevel(FeatList.EnchantRing) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantRing), 			featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 4: 	featname = "Enchant Clothing";  m.Feats.GetFeatLevel(FeatList.EnchantClothing) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantClothing), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 5: 	featname = "Enchant Self"; 		m.Feats.GetFeatLevel(FeatList.EnchantSelf) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantSelf), 			featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Mining, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 6: 	featname = "Enchant Others"; 	m.Feats.GetFeatLevel(FeatList.EnchantOthers) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnchantOthers), 		featname, SkillName.Magery, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						
						case 7: 	featname = "Life II"; 			m.Feats.GetFeatLevel(FeatList.LifeII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LifeII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 					 	0, 0, 		0, 0, 0 ); break;
						case 8: 	featname = "Death II"; 			m.Feats.GetFeatLevel(FeatList.DeathII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DeathII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 9: 	featname = "Mind II"; 			m.Feats.GetFeatLevel(FeatList.MindII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MindII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 		0, 0, 0 ); break;
						case 10: 	featname = "Matter II"; 		m.Feats.GetFeatLevel(FeatList.MatterII) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MatterII), 			featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 					 	0, 0,  		0, 0, 0 ); break;
						case 11: 	featname = "Time II"; 			m.Feats.GetFeatLevel(FeatList.TimeII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.TimeII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 12: 	featname = "Space II"; 			m.Feats.GetFeatLevel(FeatList.SpaceII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SpaceII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 				 		0, 0, 		0, 0, 0 ); break;
						case 13: 	featname = "Fate II"; 			m.Feats.GetFeatLevel(FeatList.FateII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.FateII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 				 		0, 0,  		0, 0, 0 ); break;
						case 14: 	featname = "Prime II"; 			m.Feats.GetFeatLevel(FeatList.PrimeII) += 					FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PrimeII), 				featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 15: 	featname = "Forces II";	 		m.Feats.GetFeatLevel(FeatList.ForcesII) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ForcesII), 			featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0,  		0, 0, 0 ); break;
						case 16: 	featname = "Spirit II"; 		m.Feats.GetFeatLevel(FeatList.SpiritII) += 				FeatRaise( m, m.Feats.GetFeatLevel(FeatList.SpiritII), 			featname, SkillName.Invocation, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						0, 0, 	 	0, 0, 0 ); break;
					}
					
					break;
				}
				
				case "assassin": 
				{
					switch( feat )
					{
						case 1: 	featname = "Enhanced Dodge"; 	m.Feats.GetFeatLevel(FeatList.EnhancedDodge) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedDodge), 	featname, SkillName.Dodge, 			60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 10, 	0, 0, 0 ); break;
						case 2: 	featname = "Evade"; 			m.Feats.GetFeatLevel(FeatList.Evade) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Evade), 			featname, SkillName.Dodge, 			50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0,					 	10, 5,		0, 0, 0 ); break;
						case 3: 	featname = "Armoured Dodge"; 	m.Feats.GetFeatLevel(FeatList.ArmouredDodge) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmouredDodge), 	featname, SkillName.Dodge, 			100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.EnhancedDodge), 3, 	30, 5, 		0, 0, 0 ); break;
						case 4: 	featname = "Enhanced Stealth"; 	m.Feats.GetFeatLevel(FeatList.EnhancedStealth) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.EnhancedStealth), 	featname, SkillName.Stealth, 		60.0, 20.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		0, 0, 0 ); break;
						case 5: 	featname = "Armoured Stealth"; 	m.Feats.GetFeatLevel(FeatList.ArmouredStealth) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ArmouredStealth), 	featname, SkillName.Stealth, 		100.0, 0.0, 	SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.EnhancedStealth), 3, 30, 5, 		0, 0, 0 ); break;
						case 6: 	featname = "Disguise Kit"; 		m.Feats.GetFeatLevel(FeatList.DisguiseKit) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseKit), 		featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						20, 5, 		m.RawInt, 50, 10 ); break;
						case 7: 	featname = "Disguise Others"; 	m.Feats.GetFeatLevel(FeatList.DisguiseOthers) += 	FeatRaise( m, m.Feats.GetFeatLevel(FeatList.DisguiseOthers), 	featname, SkillName.Magery, 		0.0, 0.0, 		SkillName.Magery, 0.0, 0.0, 	m.Feats.GetFeatLevel(FeatList.DisguiseKit), 3, 	30, 5, 		m.RawInt, 70, 10 ); break;
						case 8: 	featname = "Shorthand"; 		m.Feats.GetFeatLevel(FeatList.Shorthand) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Shorthand), 		featname, SkillName.Linguistics, 	50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 5, 		m.RawInt, 70, 10 ); break;
						case 9: 	featname = "Quick Reflexes"; m.Feats.GetFeatLevel(FeatList.QuickReflexes) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.QuickReflexes), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawDex, 90, 5 ); break;
						case 10: 	featname = "Brute Strength"; m.Feats.GetFeatLevel(FeatList.BruteStrength) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BruteStrength), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, m.RawStr, 90, 5 ); break;
						case 11: 	featname = "Feint"; m.Feats.GetFeatLevel(FeatList.Feint) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Feint), featname, SkillName.Tactics, 60.0, 20.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 12: 	featname = "Throwing Mastery"; m.Feats.GetFeatLevel(FeatList.ThrowingMastery) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.ThrowingMastery), featname, SkillName.Throwing, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 10, 0, 0, 0 ); break;
						case 13: 	featname = "Poison Resistance"; 			m.Feats.GetFeatLevel(FeatList.PoisonResistance) += 			FeatRaise( m, m.Feats.GetFeatLevel(FeatList.PoisonResistance), 			featname, SkillName.Poisoning, 		50.0, 25.0, 	SkillName.Magery, 0.0, 0.0, 	0, 0, 						15, 10, 		0, 0, 0 ); break;
						case 14: 	featname = "Finesse"; 			m.Feats.GetFeatLevel(FeatList.Finesse) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Finesse), 		featname, SkillName.Throwing, 		50.0, 25.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						5, 5, 		0, 0, 0 ); break;
						case 15: 	featname = "Bleeding Strike"; 		m.Feats.GetFeatLevel(FeatList.BleedingStrike) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.BleedingStrike), 		featname, SkillName.Anatomy, 		50.0, 25.0, 		SkillName.Magery, 0.0, 0.0, 	0, 0, 						10, 5, 		0, 0, 0 ); break;
						case 16: 	featname = "Backstab"; 		m.Feats.GetFeatLevel(FeatList.Backstab) += 		FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Backstab), 		featname, SkillName.Hiding, 	60.0, 15.0, 		SkillName.Stealth, 60.0, 15.0, 	0, 0, 						20, 10, 		0, 0, 0 ); break;
					}

					break;
				}
					
				case "general":
				{
					switch( feat )
					{
						case 1: featname = "Light Armour"; m.Feats.GetFeatLevel(FeatList.LightArmour) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.LightArmour), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 0, 5, 0, 0, 0 ); break;
						case 2: featname = "Medium Armour"; m.Feats.GetFeatLevel(FeatList.MediumArmour) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MediumArmour), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 5, 5, 0, 0, 0 ); break;
						case 3: featname = "Heavy Armour"; m.Feats.GetFeatLevel(FeatList.HeavyArmour) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.HeavyArmour), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 10, 5, 0, 0, 0 ); break;
						case 4: featname = "Teaching"; m.Feats.GetFeatLevel(FeatList.Teaching) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.Teaching), featname, SkillName.Magery, 0.0, 0.0, SkillName.Magery, 0.0, 0.0, 0, 0, 30, 10, 0, 0, 0 ); break;
						case 5: featname = "Merc Training"; m.Feats.GetFeatLevel(FeatList.MercTraining) += FeatRaise( m, m.Feats.GetFeatLevel(FeatList.MercTraining), featname, SkillName.Leadership, 50.0, 25.0, SkillName.Magery, 0.0, 0.0, 0, 0, 20, 5, 0, 0, 0 ); break;
					}
					
					break;
				}
				
			}
			
			if( featname == "Armour Focus" || featname == "Light Armour" || featname == "Medium Armour" || featname == "Heavy Armour" )
			{
				ArmourTest( Layer.InnerTorso, m );
				ArmourTest( Layer.InnerLegs, m );
				ArmourTest( Layer.TwoHanded, m );
				ArmourTest( Layer.Neck, m );
				ArmourTest( Layer.Gloves, m );
				ArmourTest( Layer.Helm, m );
				ArmourTest( Layer.Arms, m );
			}
		}
		
		public static int FeatRaise( PlayerMobile m, int featlevel, string featname, SkillName skill1, 
		                             double skillreq1, double skillinc1, SkillName skill2, double skillreq2, 
		                             double skillinc2, int featreqvalue, int featreq, int levelreq, int levelinc, 
		                             int statreqvalue, int statreq, int statinc )
		{
			string featnumber = "";
				
			switch( featlevel + 1 )
			{
				case 1: featnumber = "first"; break;
				case 2: featnumber = "second"; break;
				case 3: featnumber = "third"; break;
			}
			
			if( !LevelSystem.CanSpendCP( m, ( ( featlevel + 1 ) * 1000 ) ) )
			{
				return 0;
			}
				
			if( m.Level < ( levelreq + ( featlevel * levelinc ) ) )
			{
				m.SendMessage( 60, "You do not meet the level requirement." );
				return 0;
			}
			
			if( statreqvalue < ( statreq + ( statinc * featlevel ) ) )
			{
				m.SendMessage( 60, "You do not meet the stat requirement." );
				return 0;
			}
			
			if( m.Skills[skill1].Base < ( skillreq1 + ( skillinc1 * featlevel ) ) )
			{
				m.SendMessage( 60, "You do not meet the {0}", skillreq2 > 0 ? "first skill requirement." : "skill requirement." );
				return 0;
			}
		
			if( m.Skills[skill2].Base < ( skillreq2 + ( skillinc2 * featlevel ) ) )
			{
				m.SendMessage( 60, "You do not meet the second skill requirement." );
				return 0;
			}
	
			if( featreq > featreqvalue )
			{
				m.SendMessage( 60, "You do not meet the feat requirement." );
				return 0;
			}
			
			if( m.CP >= ( featlevel + 1 ) * 1000 )
			{
				m.CP -= ( featlevel + 1 ) * 1000;
				m.SendMessage( 60, "You have successfully learnt the " + featnumber + " level of " + featname + "." );
				
				if( featname == "Life II" || featname == "Death II" || featname == "Mind II" || featname == "Matter II" || featname == "Time II" || 
				   featname == "Space II" || featname == "Fate II" || featname == "Prime II" || featname == "Forces II" || featname == "Spirit II" )
					m.RawMana += (featlevel + 1);
				
				switch( featname )
				{						
					case "Racial Fighting Style":
					{
						switch( m.Nation )
						{
							case Nation.Southern:
							{
								m.Feats.GetFeatLevel(FeatList.SilentHowl)++;
								break;
							}
								
							case Nation.Western:
							{
								m.Feats.GetFeatLevel(FeatList.SwipingClaws)++;
								break;
							}
								
							case Nation.Haluaroc:
							{
								m.Feats.GetFeatLevel(FeatList.VenomousWay)++;
								break;
							}
								
							case Nation.Mhordul:
							{
								m.Feats.GetFeatLevel(FeatList.SearingBreath)++;
								break;
							}
								
							case Nation.Tirebladd:
							{
								m.Feats.GetFeatLevel(FeatList.TempestuousSea)++;
								break;
							}
								
							case Nation.Northern:
							{
								m.Feats.GetFeatLevel(FeatList.ThunderingHooves)++;
								break;
							}
						}
						break;
					}
						
					case "Racial Resource":
					{
						switch( m.Nation )
						{
							case Nation.Southern:
							{
								m.Feats.GetFeatLevel(FeatList.Greenheart)++;
								break;
							}
								
							case Nation.Western:
							{
								m.Feats.GetFeatLevel(FeatList.Obsidian)++;
								break;
							}
								
							case Nation.Haluaroc:
							{
								m.Feats.GetFeatLevel(FeatList.Linen)++;
								break;
							}
								
							case Nation.Mhordul:
							{
								m.Feats.GetFeatLevel(FeatList.Bone)++;
								break;
							}
								
							case Nation.Tirebladd:
							{
								m.Feats.GetFeatLevel(FeatList.Steel)++;
								break;
							}
								
							case Nation.Northern:
							{
								m.Feats.GetFeatLevel(FeatList.Pusantia)++;
								break;
							}
						}
						break;
					}
						
					case "Glass Blower":
					{
						if( m.SandMining )
							m.Glassblowing = true;
						
						else
							m.SandMining = true;
						
						break;
					}
						
					case "Sculptor":
					{
						if( m.StoneMining )
							m.Masonry = true;
						
						else
							m.StoneMining = true;
						
						break;
					}
						
					case "Renowned Masterwork":
					{
						m.Masterwork.WeaponPointsLeft++;
						m.Masterwork.ArmourPointsLeft++;
						
						break;
					}
						
					case "Animal Control":
					{
						m.FollowersMax++;
						
						break;
					}
					
					case "Poison Resistance":
					{
						m.InvalidateProperties();
						break;
					}
				}
				
				m.FeatSlots += ( ( featlevel + 1 ) * 1000 );
				m.CPSpent += ( ( featlevel + 1 ) * 1000 );
				
				if( m.HasGump( typeof( CharInfoGump ) ) )
					m.SendGump( new CharInfoGump( m ) );
				
				return 1;
			}
			
			int offset = ( ( featlevel + 1 ) * 1000 ) - m.CP;
			m.SendMessage( 60, "You need " + offset + " more CPs to learn the " + featnumber + " level of " + featname + "." );
			return 0;
		}
		
		public static void ArmourTest( Layer layer, PlayerMobile m )
		{
			Item item = null;
        	
        	if( m.FindItemOnLayer( layer ) != null )
        	{ 
        		item = m.FindItemOnLayer( layer );
        		
        		if( item is BaseArmor )
        		{
        			BaseArmor armor = item as BaseArmor;
        			
        			armor.OnRemoved( m );
        			armor.OnEquip( m );
        		}
        	}*/
		}
	}
}
