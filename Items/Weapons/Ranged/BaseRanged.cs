using System;
using System.Text;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Bushido;
using Server.Spells.ExoticWeaponry;
using Server.Factions;
using Server.Engines.Craft;
using System.Collections.Generic;
using Server.Commands;

namespace Server.Items
{
	public abstract class BaseRanged : BaseMeleeWeapon
	{
		public abstract int EffectID{ get; }
		public abstract Type AmmoType{ get; }
		public abstract Item Ammo{ get; }

		public override int DefHitSound{ get{ return 0x234; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override SkillName DefSkill{ get{ return SkillName.Archery; } }
		public override WeaponType DefType{ get{ return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootXBow; } }

		public override SkillName AccuracySkill{ get{ return SkillName.Archery; } }

		public BaseRanged( int itemID ) : base( itemID )
		{
		}

		public BaseRanged( Serial serial ) : base( serial )
		{
		}

        public TimeSpan RequiredStillTime(Mobile attacker)
        {
            double waitTime = 0.0;

            int range = this.DefMaxRange;

            if (range > 1)
                waitTime = ((double)range);

            if (!(this is Boomerang)) // Editing ARCHERY to be more of a support build.
            {
                double waitReduction = 0;

                if (attacker.RawInt > 20)
                    waitReduction += (attacker.RawInt / 50);
                else
                    waitReduction -= -1;

                if (attacker.RawDex > 20)
                    waitReduction += (attacker.RawDex / 100);
                else
                    waitReduction -= 1;

                if (attacker.RawStr > 20)
                    waitReduction += (attacker.RawStr / 100);
                else
                    waitReduction -= 1;

                if (this is HeavyCrossbow)
                    waitReduction += 1;

                if (this is Crossbow)
                    waitReduction += 2;

                waitReduction -= (this.Speed / 10);
                waitReduction += (this.GetSpeedBonus() / 5);

                switch (Quality)
                {
                    case WeaponQuality.Low: waitReduction -= 1; break;
                    case WeaponQuality.Exceptional: waitReduction += 0.5; break;
                    case WeaponQuality.Extraordinary: waitReduction += 1; break;
                    case WeaponQuality.Masterwork: waitReduction += 2; break;
                    default: break;
                }

                switch (Resource)
                {
                    case CraftResource.Oak: waitReduction += 0.2; break;
                    case CraftResource.Yew: waitReduction += 0.4; break;
                    case CraftResource.Redwood: waitReduction += 0.6; break;
                    case CraftResource.Ash: waitReduction += 0.8; break;
                    case CraftResource.Greenheart: waitReduction += 1.0; break;
                    default: break;
                }

                if (attacker is PlayerMobile)
                {
                    if( (attacker as PlayerMobile).Feats.GetFeatLevel(FeatList.ArmourFocus) > 2)
                        waitReduction -= (attacker as PlayerMobile).HeavyPieces * 0.25;
                    else
                        waitReduction -= (attacker as PlayerMobile).HeavyPieces * 0.5;

                    if ((attacker as PlayerMobile).Feats.GetFeatLevel(FeatList.ArmourFocus) > 1)
                        waitReduction -= (attacker as PlayerMobile).MediumPieces * 0.1;
                    else
                        waitReduction -= (attacker as PlayerMobile).MediumPieces * 0.2;
                }

                if (attacker.Mount != null)
                {
                    if (attacker is PlayerMobile)
                        waitReduction -= (2 - (attacker as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedArchery));
                    else if (attacker is IKhaerosMobile)
                        waitReduction -= (2 - (attacker as IKhaerosMobile).Feats.GetFeatLevel(FeatList.MountedArchery));
                }

                foreach (Item i in attacker.Items)
                {
                    if (i is Quiver)
                    {
                        waitReduction += 2;
                        continue;
                    }
                }
                
                waitTime -= waitReduction;

                if (waitTime < 1)
                    waitTime = 1;
            }
            else
                waitTime = 1;

            if (this is RepeatingCrossbow)
            {
                if (this.Quality == WeaponQuality.Masterwork)
                    waitTime = 0.0;
                else if (this.Quality == WeaponQuality.Extraordinary)
                    waitTime = 0.25;
                else if (this.Quality == WeaponQuality.Exceptional)
                    waitTime = 0.5;
            }

            if (((IKhaerosMobile)attacker).OffensiveFeat == FeatList.TravelingShot)
                waitTime = 0.0;

            return TimeSpan.FromSeconds(waitTime);
        }
		
		public override bool IsStill( Mobile attacker )
		{
			return ( DateTime.Now >= (attacker.LastMoveTime + RequiredStillTime( attacker )) );
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
            if( attacker.Weapon is Boomerang )
        	{
        		base.OnHit( attacker, defender, damageBonus );
        		return;
        	}
            
            double findChance = 0.3 + (0.2 * ( (IKhaerosMobile)attacker ).Feats.GetFeatLevel(FeatList.ReusableAmmunition));
            attacker.PlaySound( GetHitAttackSound( attacker, defender ) );
			
			if ( attacker.Player && defender is BaseCreature && findChance >= Utility.RandomDouble() )
				( (BaseCreature)defender ).PackItem( Ammo );

			base.OnHit( attacker, defender, damageBonus );
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
            if( attacker.Weapon is Boomerang )
        	{
        		base.OnMiss( attacker, defender );
        		return;
        	}
            
			attacker.PlaySound( 690 );
            double findChance = 0.3 + (0.2 * ( (IKhaerosMobile)attacker ).Feats.GetFeatLevel(FeatList.ReusableAmmunition));
			if ( attacker.Player && findChance >= Utility.RandomDouble() )
				Ammo.MoveToWorld( new Point3D( defender.X + Utility.RandomMinMax( -1, 1 ), defender.Y + Utility.RandomMinMax( -1, 1 ), defender.Z ), defender.Map );

			base.OnMiss( attacker, defender );
		}

		public override bool OnFired( Mobile attacker, Mobile defender )
		{
			Container pack = attacker.Backpack;

			if ( ( attacker.Player || attacker is Mercenary ) && (pack == null || !pack.ConsumeTotal( AmmoType, 1 )) )
				return false;

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				{
					MinDamage = -1;
					MaxDamage = -1;
					break;
				}
				case 2:
				case 1:
				{
					break;
				}
				case 0:
				{
					/*m_EffectID =*/ reader.ReadInt();
					break;
				}
			}

			if ( version < 2 )
			{
				WeaponAttributes.MageWeapon = 0;
				WeaponAttributes.UseBestSkill = 0;
			}
		}
	}
}
