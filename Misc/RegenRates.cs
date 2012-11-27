using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.ExoticWeaponry;
using Server.Mobiles;
using Server.Misc;

namespace Server.Misc
{
	public class RegenRates
	{
		[CallPriority( 10 )]
		public static void Configure()
		{
			Mobile.DefaultHitsRate = TimeSpan.FromSeconds( 15.0 );
			Mobile.DefaultStamRate = TimeSpan.FromSeconds( 15.0 );
			Mobile.DefaultManaRate = TimeSpan.FromSeconds( 15.0 );

			Mobile.ManaRegenRateHandler = new RegenRateHandler( Mobile_ManaRegenRate );

			if ( Core.AOS )
			{
				Mobile.StamRegenRateHandler = new RegenRateHandler( Mobile_StamRegenRate );
				Mobile.HitsRegenRateHandler = new RegenRateHandler( Mobile_HitsRegenRate );
			}
		}

		private static void CheckBonusSkill( Mobile m, int cur, int max, SkillName skill )
		{
			if ( !m.Alive )
				return;

			double n = (double)cur / max;
			double v = Math.Sqrt( m.Skills[skill].Value * 0.005 );

			n *= (1.0 - v);
			n += v;

			m.CheckSkill( skill, n );
		}

		private static bool CheckTransform( Mobile m, Type type )
		{
			return TransformationSpell.UnderTransformation( m, type );
		}

		private static bool CheckAnimal( Mobile m, Type type )
		{
			return AnimalForm.UnderTransformation( m, type );
		}

		private static TimeSpan Mobile_HitsRegenRate( Mobile from )
		{
			double points = (double)AosAttributes.GetValue( from, AosAttribute.RegenHits );

			if ( from is BaseCreature && !((BaseCreature)from).IsAnimatedDead )
				points += 1.0;

			if ( (from is BaseCreature && ((BaseCreature)from).IsParagon) || from is IRegenerativeCreature )
				points += 2.0;

			if ( CheckTransform( from, typeof( HorrificBeastSpell ) ) )
				points += 20.0;

			if( Core.ML && from.Race == Race.Human )
				points += 2.0;
			
			double extra = 0.0;
			
			if( extra > 10.0 )
				extra = 10.0;
			
			points += extra;
			
			if( from is IKhaerosMobile )
			{			
				points += (double)( (IKhaerosMobile)from ).Feats.GetFeatLevel(FeatList.FastHealing);
				
				if( from is PlayerMobile )
				{
                    points += (double)((PlayerMobile)from).GetBackgroundLevel(BackgroundList.QuickHealer);
					points -= (double)( (PlayerMobile)from ).GetBackgroundLevel(BackgroundList.SlowHealer);
					points += (double)( (PlayerMobile)from ).Fortitude;
					points += ((PlayerMobile)from).GetVampiricRegenBonus();
				}
			}

			return TimeSpan.FromSeconds( 1.0 / (0.1 * (1.0 + points)) );
		}

		private static TimeSpan Mobile_StamRegenRate( Mobile from )
		{
			if ( from.Skills == null )
				return Mobile.DefaultStamRate;

			CheckBonusSkill( from, from.Stam, from.StamMax, SkillName.Concentration );

			int points = AosAttributes.GetValue( from, AosAttribute.RegenStam );
			
			if( from is PlayerMobile )
			{
				if( ( (PlayerMobile)from ).Nation != Nation.None )
				{				
					points += ( (PlayerMobile)from ).GetBackgroundLevel(BackgroundList.Resilient);
					points -= ( (PlayerMobile)from ).GetBackgroundLevel(BackgroundList.OutOfShape);
					points++; // Adding 1 point to increase stam regen somewhat.
				}
				
				if( from.Mounted && from.Mount != null )
				{
					Mobile mount = from.Mount as Mobile;
					
					if( mount != null )
						mount.Stam += Math.Max( 0, ( ( (PlayerMobile)from ).Feats.GetFeatLevel(FeatList.MountedEndurance) - Utility.RandomMinMax( 0, 3 ) ) );
				}
				
				points += ( (PlayerMobile)from ).Fortitude;
				
				if( ((PlayerMobile)from).IsVampire )
				{
					int extra = PlayerMobile.GetVampireTimeOffset( ((PlayerMobile)from).GetHour() );
					
					if( extra > 0 )
						points += extra * 3;
				}
			}

			if ( CheckTransform( from, typeof( VampiricEmbraceSpell ) ) )
				points += 15;

			if ( (from is BaseCreature && ((BaseCreature)from).IsParagon) )
				points += 40;

			if ( points < -1 )
				points = -1;

			return TimeSpan.FromSeconds( 1.0 / (0.1 * (2 + points)) );
		}
		
		public static double GetValue( int feat )
		{
			if( feat == 1 )
				return 0.2;
			
			if( feat == 2 )
				return 0.6;
			
			if( feat == 3 )
				return 1.2;
			
			return 0.0;
		}

		private static TimeSpan Mobile_ManaRegenRate( Mobile from )
		{
			if ( from.Skills == null )
				return Mobile.DefaultManaRate;

			if ( !from.Meditating )
				CheckBonusSkill( from, from.Mana, from.ManaMax, SkillName.Meditation );

			double rate;
			//double armorPenalty = GetArmorOffset( from );

			if ( Core.AOS )
			{
				double medPoints = from.Int + (from.Skills[SkillName.Meditation].Value * 3);

				medPoints *= ( from.Skills[SkillName.Meditation].Value < 100.0 ) ? 0.025 : 0.0275;

				CheckBonusSkill( from, from.Mana, from.ManaMax, SkillName.Concentration );

				double focusPoints = (int)(from.Skills[SkillName.Concentration].Value * 0.00);

				//if ( armorPenalty > 0 )
				//	medPoints = 0; // In AOS, wearing any meditation-blocking armor completely removes meditation bonus

				double totalPoints = AosAttributes.GetValue( from, AosAttribute.RegenMana ) +
					focusPoints + medPoints + (from.Meditating ? (medPoints > 13.0 ? 13.0 : medPoints) : 0.0);

				if ( CheckTransform( from, typeof( VampiricEmbraceSpell ) ) )
					totalPoints += 3;
				else if ( CheckTransform( from, typeof( LichFormSpell ) ) )
					totalPoints += 13;

				if ( (from is BaseCreature && ((BaseCreature)from).IsParagon) )
					totalPoints += 40;

				if ( totalPoints < -1 )
					totalPoints = -1;
				
				if( from is PlayerMobile )
				{
					if( ( (PlayerMobile)from ).Nation != Nation.None )
					{				
						totalPoints += ( (PlayerMobile)from ).GetBackgroundLevel(BackgroundList.FocusedMind);
                        totalPoints -= ((PlayerMobile)from).GetBackgroundLevel(BackgroundList.UnfocusedMind);
					}
					
					totalPoints += ( (PlayerMobile)from ).Fortitude;
				}
				
				if( from is IKhaerosMobile )
				{
					IKhaerosMobile mob = from as IKhaerosMobile;
					double fromFeats = GetValue(mob.Feats.GetFeatLevel(FeatList.LifeI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.DeathI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.MindI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.MatterI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.SpaceI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.TimeI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.FateI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.PrimeI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.ForcesI)) + GetValue(mob.Feats.GetFeatLevel(FeatList.SpiritI));
					fromFeats *= 0.5;
					totalPoints += fromFeats;
				}

				rate = 1.0 / (0.1 * (2 + totalPoints));
			}
			else
			{
				double medPoints = (from.Int + from.Skills[SkillName.Meditation].Value) * 0.5;

				if ( medPoints <= 0 )
					rate = 7.0;
				else if ( medPoints <= 100 )
					rate = 7.0 - (239*medPoints/2400) + (19*medPoints*medPoints/48000);
				else if ( medPoints < 120 )
					rate = 1.0;
				else
					rate = 0.75;

				//rate += armorPenalty;

				if ( from.Meditating )
					rate *= 0.5;

				if ( rate < 0.5 )
					rate = 0.5;
				else if ( rate > 7.0 )
					rate = 7.0;
			}

			return TimeSpan.FromSeconds( rate );
		}

		public static double GetArmorOffset( Mobile from )
		{
			double rating = 0.0;

			if ( !Core.AOS )
				rating += GetArmorMeditationValue( from.ShieldArmor as BaseArmor );

			rating += GetArmorMeditationValue( from.NeckArmor as BaseArmor );
			rating += GetArmorMeditationValue( from.HandArmor as BaseArmor );
			rating += GetArmorMeditationValue( from.HeadArmor as BaseArmor );
			rating += GetArmorMeditationValue( from.ArmsArmor as BaseArmor );
			rating += GetArmorMeditationValue( from.LegsArmor as BaseArmor );
			rating += GetArmorMeditationValue( from.ChestArmor as BaseArmor );

			return rating / 4;
		}

		private static double GetArmorMeditationValue( BaseArmor ar )
		{
			if ( ar == null || ar.ArmorAttributes.MageArmor != 0 || ar.Attributes.SpellChanneling != 0 )
				return 0.0;

			switch ( ar.MeditationAllowance )
			{
				default:
				case ArmorMeditationAllowance.None: return ar.BaseArmorRatingScaled;
				case ArmorMeditationAllowance.Half: return ar.BaseArmorRatingScaled / 2.0;
				case ArmorMeditationAllowance.All:  return 0.0;
			}
		}
	}
}
