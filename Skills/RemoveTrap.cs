using System;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Factions;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class RemoveTrap
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.ArmDisarmTraps].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			XmlAttachment freeze = XmlAttach.FindAttachment( m, typeof( XmlFreeze ) );
		             
			if ( m.Skills[SkillName.ArmDisarmTraps].Value < 2.0 )
				m.SendMessage( "You lack the appropriate knowledge of this skill." );

			else if( freeze != null && ( freeze.Name == "disarming" ) )
	        	m.SendMessage( "You are already disarming a trap." );

			else
			{
				m.Target = new InternalTarget();

				m.SendLocalizedMessage( 502368 ); // Wich trap will you attempt to disarm?
			}

			return TimeSpan.FromSeconds( 10.0 ); // 10 second delay before beign able to re-use a skill
		}

		private class InternalTarget : Target
		{
			public InternalTarget() :  base ( 2, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				XmlAttachment freeze = XmlAttach.FindAttachment( from, typeof( XmlFreeze ) );
				
				if ( targeted is Mobile )
					from.SendLocalizedMessage( 502816 ); // You feel that such an action would be inappropriate
				
				else if( freeze != null && ( freeze.Name == "disarming" ) )
	        		from.SendMessage( "You are already disarming a trap." );

				else if( targeted is BaseTrap && from is PlayerMobile )
					((BaseTrap)targeted).OnDisarmAttempt( (PlayerMobile)from );
				
				else if ( targeted is TrapableContainer )
				{
					TrapableContainer targ = (TrapableContainer)targeted;

					from.Direction = from.GetDirectionTo( targ );

					if ( targ.TrapType == TrapType.None )
					{
						from.SendLocalizedMessage( 502373 ); // That doesn't appear to be trapped
						return;
					}

					from.PlaySound( 0x241 );
					
					if ( from.CheckTargetSkill( SkillName.ArmDisarmTraps, targ, targ.TrapPower, targ.TrapPower + 30 ) )
					{
						targ.TrapPower = 0;
						targ.TrapLevel = 0;
						targ.TrapType = TrapType.None;
						from.SendLocalizedMessage( 502377 ); // You successfully render the trap harmless
					}
					
					else
						from.SendLocalizedMessage( 502372 ); // You fail to disarm the trap... but you don't set it off
				}
				else if ( targeted is BaseFactionTrap )
				{
					BaseFactionTrap trap = (BaseFactionTrap) targeted;
					Faction faction = Faction.Find( from );

					FactionTrapRemovalKit kit = ( from.Backpack == null ? null : from.Backpack.FindItemByType( typeof( FactionTrapRemovalKit ) ) as FactionTrapRemovalKit );

					bool isOwner = ( trap.Placer == from || ( trap.Faction != null && trap.Faction.IsCommander( from ) ) );

					if ( faction == null )
						from.SendLocalizedMessage( 1010538 ); // You may not disarm faction traps unless you are in an opposing faction
					
					else if ( faction == trap.Faction && trap.Faction != null && !isOwner )
						from.SendLocalizedMessage( 1010537 ); // You may not disarm traps set by your own faction!

					else if ( !isOwner && kit == null )
						from.SendLocalizedMessage( 1042530 ); // You must have a trap removal kit at the base level of your pack to disarm a faction trap.

					else
					{
						if ( from.CheckTargetSkill( SkillName.ArmDisarmTraps, trap, 80.0, 100.0 ) && from.CheckTargetSkill( SkillName.Tinkering, trap, 80.0, 100.0 ) )
						{
							from.PrivateOverheadMessage( MessageType.Regular, trap.MessageHue, trap.DisarmMessage, from.NetState );

							if ( !isOwner )
							{
								int silver = faction.AwardSilver( from, trap.SilverFromDisarm );

								if ( silver > 0 )
									from.SendLocalizedMessage( 1008113, true, silver.ToString( "N0" ) ); // You have been granted faction silver for removing the enemy trap :
							}

							trap.Delete();
						}
						
						else
							from.SendLocalizedMessage( 502372 ); // You fail to disarm the trap... but you don't set it off

						if ( !isOwner && kit != null )
							kit.ConsumeCharge( from );
					}
				}
				
				else
					from.SendLocalizedMessage( 502373 ); // That does'nt appear to be trapped
			}
		}
	}
}
