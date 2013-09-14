using System;
using Server.Multis;
using Server.Targeting;
using Server.Items;
using Server.Regions;
using Server.Factions;
using Server.Mobiles;
using Server.Misc;

namespace Server.SkillHandlers
{
	public class DetectHidden
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.DetectHidden].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile src )
		{
			src.SendLocalizedMessage( 500819 );//Where will you search?
			src.Target = new InternalTarget();

			return TimeSpan.FromSeconds( 1.0 );
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( 12, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile src, object targ )
			{
				bool foundAnyone = false;

				Point3D p;
				if ( targ is Mobile )
					p = ((Mobile)targ).Location;
				else if ( targ is Item )
					p = ((Item)targ).Location;
				else if ( targ is IPoint3D )
					p = new Point3D( (IPoint3D)targ );
				else 
					p = src.Location;

				double srcSkill = src.Skills[SkillName.DetectHidden].Value;
				int range = 0;

                if( src is PlayerMobile )
                    range = ( (PlayerMobile)src ).Feats.GetFeatLevel( FeatList.DetectHidden );

				if ( range > 0 )
				{
					IPooledEnumerable inRange = src.Map.GetMobilesInRange( p, range );

					foreach ( Mobile trg in inRange )
					{
						if ( trg.Hidden && src != trg )
						{
							double bonus = 0.0;
							
							if( trg is PlayerMobile )
							{
								PlayerMobile pm = trg as PlayerMobile;
								
								if( pm.Nation == Nation.Western )
									bonus = 5.0;

                                if( pm.IsVampire && pm.Feats.GetFeatLevel( FeatList.Obfuscate ) > 0 )
                                    bonus = 10.0;
							}
							
							double ss = srcSkill + Utility.Random( 21 ) - 10;
							double ts = trg.Skills[SkillName.Hiding].Value + bonus + Utility.Random( 21 ) - 10;

							if ( src.AccessLevel >= trg.AccessLevel && ss >= ts )
							{
								trg.RevealingAction();
								trg.SendLocalizedMessage( 500814 ); // You have been revealed!
								foundAnyone = true;
							}
						}
					}

					inRange.Free();
				}

				if ( !foundAnyone )
				{
					src.SendLocalizedMessage( 500817 ); // You can see nothing hidden there.
				}
			}
		}
	}
}
