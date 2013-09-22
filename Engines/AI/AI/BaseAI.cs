using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Targets;
using Server.Network;
using Server.Regions;
using Server.ContextMenus;
using Server.Engines.Quests;
using MoveImpl=Server.Movement.MovementImpl;
using Server.Misc;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
	public enum AIType
	{
		AI_Use_Default,
		AI_Melee,
		AI_Animal,
		AI_Archer,
		AI_Healer,
		AI_Vendor,
		AI_Mage,
		AI_Berserk,
		AI_Predator,
		AI_Thief,
		AI_Lobotomized
	}

	public enum ActionType
	{
		Wander,
		Combat,
		Guard,
		Flee,
		Backoff,
		Interact
	}

	public abstract class BaseAI
	{
		public Timer m_Timer;
		protected ActionType m_Action;
		private DateTime m_NextStopGuard;

		public BaseCreature m_Mobile;

        public Mobile m_Speaker;

		public BaseAI( BaseCreature m )
		{
			m_Mobile = m;

			m_Timer = new AITimer( this );

			bool activate;

			if( !m.PlayerRangeSensitive )
				activate = true;
			else if( World.Loading )
				activate = false;
			else if( m.Map == null || m.Map == Map.Internal || !m.Map.GetSector( m ).Active )
				activate = false;
			else
				activate = true;

			if( activate )
				m_Timer.Start();

			Action = ActionType.Wander;
		}

		public ActionType Action
		{
			get
			{
				return m_Action;
			}
			set
			{
				m_Action = value;
				OnActionChanged();
			}
		}

		public virtual bool WasNamed( string speech )
		{
			string[] name = m_Mobile.Name.ToLower().Split(' ');
            string[] spoken = speech.ToLower().Split(' ');

            foreach (string word in spoken)
            {
                foreach (string n in name)
                {
                    if (n == word)
                        return true;
                }
            }

            return false;
		}

		private class InternalEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseCreature m_Mobile;
			private BaseAI m_AI;
			private OrderType m_Order;

			public InternalEntry( Mobile from, int number, int range, BaseCreature mobile, BaseAI ai, OrderType order )
				: base( number, range )
			{
				m_From = from;
				m_Mobile = mobile;
				m_AI = ai;
				m_Order = order;

				if( mobile.IsDeadPet && (order == OrderType.Guard || order == OrderType.Attack || order == OrderType.Transfer || order == OrderType.Drop) )
					Enabled = false;
			}

			public override void OnClick()
			{
				if( !m_Mobile.Deleted && m_Mobile.Controlled && m_From.CheckAlive() )
				{
					if( m_Mobile.IsDeadPet && (m_Order == OrderType.Guard || m_Order == OrderType.Attack || m_Order == OrderType.Transfer || m_Order == OrderType.Drop) )
						return;

					bool isOwner = (m_From == m_Mobile.ControlMaster);
					bool isFriend = (!isOwner && m_Mobile.IsPetFriend( m_From ));

                    if (!isOwner && m_From is PlayerMobile)
                    {
                        if (m_Mobile is Soldier)
                            isOwner = Soldier.IsGovernmentSuperior(m_Mobile, m_From as PlayerMobile);
                        else if (GroupInfo.HasGroup(m_Mobile))
                            isOwner = (GroupInfo.IsGroupLeader(m_Mobile, m_From as PlayerMobile));
                    }

                    if (!isOwner && !isFriend)
                        return;
                    else if (isFriend && m_Order != OrderType.Follow && m_Order != OrderType.Stay && m_Order != OrderType.Stop)
                        return;
                    else if (m_Mobile is Soldier && (m_Order == OrderType.Friend || m_Order == OrderType.Release || m_Order == OrderType.Transfer || m_Order == OrderType.Unfriend))
                        return;

					switch( m_Order )
					{
						case OrderType.Follow:
						case OrderType.Attack:
						case OrderType.Transfer:
						case OrderType.Friend:
						case OrderType.Unfriend:
						{
							if( m_Order == OrderType.Transfer && m_From.HasTrade )
								m_From.SendLocalizedMessage( 1010507 ); // You cannot transfer a pet with a trade pending
							else if( m_Order == OrderType.Friend && m_From.HasTrade )
								m_From.SendLocalizedMessage( 1070947 ); // You cannot friend a pet with a trade pending
							else
								m_AI.BeginPickTarget( m_From, m_Order );

							break;
						}
						case OrderType.Release:
						{
                            if (m_Mobile is Soldier)
                                break;
							if( m_Mobile.Summoned )
								goto default;
							else
								m_From.SendGump( new Gumps.ConfirmReleaseGump( m_From, m_Mobile ) );

							break;
						}
						default:
						{
							m_Mobile.ControlOrder = m_Order;

							break;
						}
					}
				}
			}
		}

		public virtual void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if( from.Alive && m_Mobile.Controlled && from.InRange( m_Mobile, 14 ) )
			{
				if( from == m_Mobile.ControlMaster )
				{
					list.Add( new InternalEntry( from, 6107, 14, m_Mobile, this, OrderType.Guard ) );  // Command: Guard
					list.Add( new InternalEntry( from, 6108, 14, m_Mobile, this, OrderType.Follow ) ); // Command: Follow

					if( m_Mobile.CanDrop )
						list.Add( new InternalEntry( from, 6109, 14, m_Mobile, this, OrderType.Drop ) );   // Command: Drop

					list.Add( new InternalEntry( from, 6111, 14, m_Mobile, this, OrderType.Attack ) ); // Command: Kill

					list.Add( new InternalEntry( from, 6112, 14, m_Mobile, this, OrderType.Stop ) );   // Command: Stop
					list.Add( new InternalEntry( from, 6114, 14, m_Mobile, this, OrderType.Stay ) );   // Command: Stay

					if( !m_Mobile.Summoned )
					{
						list.Add( new InternalEntry( from, 6110, 14, m_Mobile, this, OrderType.Friend ) ); // Add Friend
						list.Add( new InternalEntry( from, 6099, 14, m_Mobile, this, OrderType.Unfriend ) ); // Remove Friend
						list.Add( new InternalEntry( from, 6113, 14, m_Mobile, this, OrderType.Transfer ) ); // Transfer
					}

					list.Add( new InternalEntry( from, 6118, 14, m_Mobile, this, OrderType.Release ) ); // Release
				}
				else if( m_Mobile.IsPetFriend( from ) )
				{
					list.Add( new InternalEntry( from, 6108, 14, m_Mobile, this, OrderType.Follow ) ); // Command: Follow
					list.Add( new InternalEntry( from, 6112, 14, m_Mobile, this, OrderType.Stop ) );   // Command: Stop
					list.Add( new InternalEntry( from, 6114, 14, m_Mobile, this, OrderType.Stay ) );   // Command: Stay
				}
                else if (Soldier.IsGovernmentSuperior(m_Mobile, from as PlayerMobile))
                {
                    list.Add(new InternalEntry(from, 6107, 14, m_Mobile, this, OrderType.Guard));  // Command: Guard
                    list.Add(new InternalEntry(from, 6108, 14, m_Mobile, this, OrderType.Follow)); // Command: Follow
                    list.Add(new InternalEntry(from, 6111, 14, m_Mobile, this, OrderType.Attack)); // Command: Kill
                    list.Add(new InternalEntry(from, 6112, 14, m_Mobile, this, OrderType.Stop));   // Command: Stop
                    list.Add(new InternalEntry(from, 6114, 14, m_Mobile, this, OrderType.Stay));   // Command: Stay
                }
			}
		}

		public virtual void BeginPickTarget( Mobile from, OrderType order )
		{
            m_Mobile.DebugSay("Begin Pick Target; order = " + order.ToString());

            bool isOwner = false;
            bool isFriend = false;

            if (m_Mobile is Soldier)
            {
                if (m_Mobile.Deleted || !from.InRange(m_Mobile, 14) || from.Map != m_Mobile.Map)
                    return;            
                
                if(Soldier.IsGovernmentSuperior(m_Mobile, (PlayerMobile)from))
                    isOwner = true;
            }
            else
            {
			    if( m_Mobile.Deleted || !m_Mobile.Controlled|| !from.InRange( m_Mobile, 14 ) || from.Map != m_Mobile.Map)
				    return;			
                
                isOwner = (from == m_Mobile.ControlMaster);
			    isFriend = (!isOwner && m_Mobile.IsPetFriend( from ));
                if (!isOwner && from is PlayerMobile)
                    isOwner = GroupInfo.IsGroupLeader(m_Mobile, from as PlayerMobile);
            }

            m_Mobile.DebugSay("BPT isOwner == " + isOwner.ToString());

			if( !isOwner && !isFriend )
				return;
			else if( isFriend && order != OrderType.Follow && order != OrderType.Stay && order != OrderType.Stop )
				return;

            if ((order == OrderType.Transfer || order == OrderType.Friend || order == OrderType.Unfriend) && m_Mobile is Soldier)
                return;

			if( from.Target == null )
			{
				if( order == OrderType.Transfer )
					from.SendLocalizedMessage( 502038 ); // Click on the person to transfer ownership to.
				else if( order == OrderType.Friend )
					from.SendLocalizedMessage( 502020 ); // Click on the player whom you wish to make a co-owner.
				else if( order == OrderType.Unfriend )
					from.SendLocalizedMessage( 1070948 ); // Click on the player whom you wish to remove as a co-owner.

				from.Target = new AIControlMobileTarget( this, order );
			}
			else if( from.Target is AIControlMobileTarget )
			{
				AIControlMobileTarget t = (AIControlMobileTarget)from.Target;

				if( t.Order == order )
					t.AddAI( this );
			}
		}

		public virtual void OnAggressiveAction( Mobile aggressor )
		{
			Mobile currentCombat = m_Mobile.Combatant;

			if( currentCombat != null && !aggressor.Hidden && currentCombat != aggressor && m_Mobile.GetDistanceToSqrt( currentCombat ) > m_Mobile.GetDistanceToSqrt( aggressor ) )
				m_Mobile.Combatant = aggressor;
		}

		public virtual void EndPickTarget( Mobile from, Mobile target, OrderType order )
		{
            bool isOwner = false;
            bool isFriend = false;

            if (m_Mobile is Soldier)
            {
                if (m_Mobile.Deleted || !from.InRange(m_Mobile, 14) || from.Map != m_Mobile.Map)
                    return;

                if (Soldier.IsGovernmentSuperior(m_Mobile, (PlayerMobile)from))
                    isOwner = true;
            }
            else
            {
                if (m_Mobile.Deleted || !m_Mobile.Controlled || !from.InRange(m_Mobile, 14) || from.Map != m_Mobile.Map)
                    return;

                isOwner = (from == m_Mobile.ControlMaster);
                isFriend = (!isOwner && m_Mobile.IsPetFriend(from));
                if (!isOwner && from is PlayerMobile)
                    isOwner = GroupInfo.IsGroupLeader(m_Mobile, from as PlayerMobile);
            }

			if( !isOwner && !isFriend )
				return;
			else if( isFriend && order != OrderType.Follow && order != OrderType.Stay && order != OrderType.Stop )
				return;

            if ((order == OrderType.Transfer || order == OrderType.Friend || order == OrderType.Unfriend) && m_Mobile is Soldier)
                return;

			m_Mobile.ControlTarget = target;
			m_Mobile.ControlOrder = order;
		}

		public virtual bool HandlesOnSpeech( Mobile from )
		{
			if( from.AccessLevel >= AccessLevel.GameMaster || ((m_Mobile is IUndead || m_Mobile.CreatureGroup == CreatureGroup.Undead) &&
                from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.ControlUndead) > 0) )
				return true;

			if( from.Alive && m_Mobile.Controlled && m_Mobile.Commandable && (from == m_Mobile.ControlMaster || m_Mobile.IsPetFriend( from )) )
				return true;

            if (from.Alive && from is PlayerMobile && m_Mobile.Commandable && Soldier.IsGovernmentSuperior(m_Mobile, (PlayerMobile)from))
            {
                m_Speaker = from;
                return true;
            }

            if (from is PlayerMobile && from.Alive && m_Mobile.Controlled && m_Mobile.Commandable && GroupInfo.IsGroupLeader(m_Mobile, from as PlayerMobile))
                return true;

			return (from.Alive && from.InRange( m_Mobile.Location, 3 ) && m_Mobile.IsHumanInTown());
		}

		private static SkillName[] m_KeywordTable = new SkillName[]
			{
				SkillName.Parry,
				SkillName.Healing,
				SkillName.Hiding,
				SkillName.Stealing,
				SkillName.Alchemy,
				SkillName.AnimalLore,
				SkillName.Appraisal,
				SkillName.Craftsmanship,
				SkillName.Riding,
				SkillName.Blacksmith,
				SkillName.Fletching,
				SkillName.Peacemaking,
				SkillName.Camping,
				SkillName.Carpentry,
				SkillName.Cartography,
				SkillName.Cooking,
				SkillName.DetectHidden,
				SkillName.Throwing,//??
				SkillName.Invocation,
				SkillName.Fishing,
				SkillName.Dodge,
				SkillName.Lockpicking,
				SkillName.Magery,
				SkillName.MagicResist,
				SkillName.Tactics,
				SkillName.Snooping,
				SkillName.ArmDisarmTraps,
				SkillName.Musicianship,
				SkillName.Poisoning,
				SkillName.Archery,
				SkillName.Linguistics,
				SkillName.Tailoring,
				SkillName.AnimalTaming,
				SkillName.HerbalLore,
				SkillName.Tinkering,
				SkillName.Veterinary,
				SkillName.Forensics,
				SkillName.AnimalHusbandry,
				SkillName.Tracking,
				SkillName.Stealth,
				SkillName.Inscribe,
				SkillName.Swords,
				SkillName.Macing,
				SkillName.Fencing,
				SkillName.UnarmedFighting,
				SkillName.Lumberjacking,
				SkillName.Mining,
				SkillName.Meditation
			};

		public virtual void OnSpeech( SpeechEventArgs e )
		{
            OrderInfo.OnSpeech(m_Mobile, e.Mobile, e);
            m_Mobile.DebugSay("OrderInfo check...");
            if (!(m_Mobile is Soldier) && !m_Mobile.Controlled && OrderInfo.WasOrdered(e, m_Mobile))
            {
                m_Mobile.DebugSay("OrderInfo check successful!");
                e.Mobile.SendGump(new ViewOrdersGump(e.Mobile as PlayerMobile, m_Mobile));
            }

            if( ( m_Mobile.Controlled ) || ( e.Mobile is PlayerMobile && Soldier.IsGovernmentSuperior(m_Mobile, (PlayerMobile)e.Mobile ) ) )
			{
				m_Mobile.DebugSay( "Listening..." );
                m_Speaker = e.Mobile;

                bool isOwner = false;
                bool isFriend = false;

                if (m_Mobile is Soldier)
                {
                    isOwner = (e.Mobile is PlayerMobile && Soldier.IsGovernmentSuperior(m_Mobile, (PlayerMobile)e.Mobile));
                }
                else
                {
                    isOwner = (e.Mobile == m_Mobile.ControlMaster);
                    isFriend = (!isOwner && m_Mobile.IsPetFriend(e.Mobile));

                    if (!isOwner && e.Mobile is PlayerMobile)
                        isOwner = GroupInfo.IsGroupLeader(m_Mobile, e.Mobile as PlayerMobile);
                }

				if( e.Mobile.Alive && (isOwner || isFriend) )
				{
					m_Mobile.DebugSay( "It's from my master" );

					int[] keywords = e.Keywords;
					string speech = e.Speech;

                    if (isOwner && !(m_Mobile is Soldier) && OrderInfo.WasOrdered(e, m_Mobile))
                        e.Mobile.SendGump(new ViewOrdersGump(e.Mobile as PlayerMobile, m_Mobile));

                    if (GroupInfo.TryGetGroupGump(m_Mobile, e))
                        e.Mobile.SendGump(new GroupGump(e.Mobile as PlayerMobile, (e.Mobile as PlayerMobile).Group));

					// First, check the all*
					for( int i = 0; i < keywords.Length; ++i )
					{
						int keyword = keywords[i];

						switch( keyword )
						{
							case 0x016A: // all report
							{
								//Only the owner should be able to order the creature to switch weapons.
								if( !isOwner )
									break;
								
								//We need a backpack to find the items to switch.
								if( m_Mobile.Backpack == null )
									break;
								
								BaseShield shield = null;
								BaseWeapon weapon = null;
								bool needsshield = false;
								
								//Looking for a weapon to switch.
								try
								{
									weapon = m_Mobile.Backpack.FindItemByType( typeof( BaseWeapon ) ) as BaseWeapon;
								}
								
								catch
								{
									break;
								}
								
								if( weapon == null )
									break;
								
								if( weapon.StrRequirement > m_Mobile.Str )
									break;
								
								//Looking for a shield if the weapon is one-handed
								if( weapon.Layer == Layer.FirstValid || weapon.Layer == Layer.OneHanded )
								{
									needsshield = true;
									
									try
									{
										shield = m_Mobile.Backpack.FindItemByType( typeof( BaseShield ) ) as BaseShield;
									}
									
									catch
									{
									}
								}
								
								//Clearing both hands because we found a weapon.
								m_Mobile.ClearHands();
								
								//Adding the shield first for compability.
								if( needsshield && shield != null )
									m_Mobile.EquipItem( shield );
								
								//Adding the weapon.
								m_Mobile.Emote( "*switches to another weapon*" );
								m_Mobile.EquipItem( weapon );
								
								//Changing to AI_Archer if we added a ranged weapon.
								if( weapon is BaseRanged && m_Mobile.AI == AIType.AI_Melee )
								{
									m_Mobile.AI = AIType.AI_Archer;
								}
								
								//Chaning to AI_Melee if we added a melee weapon.
								else if( weapon is BaseMeleeWeapon && m_Mobile.AI == AIType.AI_Archer )
								{
									m_Mobile.AI = AIType.AI_Melee;
								}
								
								//Trying to recover a shield that the mobile was wearing before.
								if( shield == null && needsshield )
								{
									try
									{
										shield = m_Mobile.Backpack.FindItemByType( typeof( BaseShield ) ) as BaseShield;
									}
									
									catch
									{
										break;
									}
									
									if( shield != null )
										m_Mobile.EquipItem( shield );
								}
								
								return;
							}
								
							case 0x164: // all come
							{
								if( !isOwner )
									break;

								m_Mobile.ControlTarget = null;
								m_Mobile.ControlOrder = OrderType.Come;

								return;
							}
							case 0x165: // all follow
							{
							
														if( m_Mobile.CreatureGroup == CreatureGroup.Militia )
							break;
							
							else if( m_Mobile.CreatureGroup != CreatureGroup.Militia )
							
								BeginPickTarget( e.Mobile, OrderType.Follow );
								return;
							}
							case 0x166: // all guard
							{
								if( !isOwner )
									break;
									
									if( m_Mobile.CreatureGroup == CreatureGroup.Militia )
							break;
							
							else if( m_Mobile.CreatureGroup != CreatureGroup.Militia )

								m_Mobile.ControlTarget = null;
								m_Mobile.ControlOrder = OrderType.Guard;
								
								return;
							}
							case 0x167: // all stop
							{
								m_Mobile.ControlTarget = null;
								m_Mobile.ControlOrder = OrderType.Stop;
                                m_Mobile.Aggressors.Clear();
                                m_Mobile.Combatant = null;
                                m_Mobile.Warmode = false;
                                m_Mobile.FocusMob = null;
								
								return;
							}
							case 0x168: // all kill
							case 0x169: // all attack
							{
								if( !isOwner )
									break;

								BeginPickTarget( e.Mobile, OrderType.Attack );
								return;
							}
							case 0x16B: // all guard me
							{
								if( !isOwner )
									break;
																							if( m_Mobile.CreatureGroup == CreatureGroup.Militia )
							break;
							
							else if( m_Mobile.CreatureGroup != CreatureGroup.Militia )

								m_Mobile.ControlTarget = e.Mobile;
								m_Mobile.ControlOrder = OrderType.Guard;
								
								return;
							}
							case 0x16C: // all follow me
							{
								if( m_Mobile.CreatureGroup == CreatureGroup.Militia )
							break;
							
							else if( m_Mobile.CreatureGroup != CreatureGroup.Militia )
                                m_Mobile.DebugSay("All Follow " + e.Mobile.Name);
								m_Mobile.ControlTarget = e.Mobile;
								m_Mobile.ControlOrder = OrderType.Follow;
								
								return;
							}
							case 0x170: // all stay
							{
							
								m_Mobile.ControlTarget = null;
								m_Mobile.ControlOrder = OrderType.Stay;
								
								return;
							}
						}
					}

                    // Next check for group commands
                    
                    if (e.Mobile is PlayerMobile && GroupInfo.IsGroupMember(m_Mobile, e.Mobile as PlayerMobile))
                    {
                        string groupSpeech = speech.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("-", "");
                        if ( !groupSpeech.Contains("your") && (groupSpeech.Contains("group") || groupSpeech.Contains("squad") || groupSpeech.Contains((e.Mobile as PlayerMobile).Group.Name.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("-", ""))))
                        {
                            if (groupSpeech.Contains("come"))
                            {
                                m_Mobile.ControlTarget = null;
                                m_Mobile.ControlOrder = OrderType.Come;
                                return;
                            }
                            else if (groupSpeech.Contains("follow") && !groupSpeech.Contains("me"))
                            {
							if( m_Mobile.CreatureGroup == CreatureGroup.Militia )
							return;
							
							else if( m_Mobile.CreatureGroup != CreatureGroup.Militia )
                                if( WasNamed( speech ) )
									BeginPickTarget( e.Mobile, OrderType.Follow );
                                return;
                            }
                            else if (groupSpeech.Contains("guard"))
                            {
                                if( !m_Mobile.IsDeadPet)
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Guard;
								}
                                return;
                            }
                            else if (groupSpeech.Contains("kill") || groupSpeech.Contains("attack") || groupSpeech.Contains("strike"))
                            {
                                if( !m_Mobile.IsDeadPet )
									BeginPickTarget( e.Mobile, OrderType.Attack );

								return;
                            }
                            else if (groupSpeech.Contains("stop"))
                            {
                                m_Mobile.ControlTarget = null;
                                m_Mobile.ControlOrder = OrderType.Stop;
                                m_Mobile.Aggressors.Clear();
                                m_Mobile.Combatant = null;
                                m_Mobile.Warmode = false;

                                return;
                            }
                            else if (groupSpeech.Contains("follow") && groupSpeech.Contains("me"))
                            {
                                m_Mobile.DebugSay("I was named, will follow " + e.Mobile.Name);
                                m_Mobile.ControlTarget = e.Mobile;
                                m_Mobile.ControlOrder = OrderType.Follow;
                                return;
                            }
                            else if (groupSpeech.Contains("stay"))
                            {
                                m_Mobile.ControlTarget = null;
                                m_Mobile.ControlOrder = OrderType.Stay;
                                return;
                            }
                        }
                    }

					// No all*, no squad, so check *command
					for( int i = 0; i < keywords.Length; ++i )
					{
						int keyword = keywords[i];

						switch( keyword )
						{
							case 0x0160: // report
							{
								//Only the owner should be able to order the creature to switch weapons.
								if( !isOwner )
									break;
								
								//We need a backpack to find the items to switch.
								if( m_Mobile.Backpack == null )
									break;
								
								BaseShield shield = null;
								BaseWeapon weapon = null;
								bool needsshield = false;
								
								//Looking for a weapon to switch.
								try
								{
									weapon = m_Mobile.Backpack.FindItemByType( typeof( BaseWeapon ) ) as BaseWeapon;
								}
								
								catch
								{
									break;
								}
								
								if( weapon == null )
									break;
								
								if( weapon.StrRequirement > m_Mobile.Str )
									break;
								
								//Looking for a shield if the weapon is one-handed
								if( weapon.Layer == Layer.FirstValid || weapon.Layer == Layer.OneHanded )
								{
									needsshield = true;
									
									try
									{
										shield = m_Mobile.Backpack.FindItemByType( typeof( BaseShield ) ) as BaseShield;
									}
									
									catch
									{
									}
								}
								
								//Clearing both hands because we found a weapon.
								m_Mobile.ClearHands();
								
								//Adding the shield first for compability.
								if( needsshield && shield != null )
									m_Mobile.EquipItem( shield );
								
								//Adding the weapon.
								m_Mobile.Emote( "*switches to another weapon*" );
								m_Mobile.EquipItem( weapon );
								
								//Changing to AI_Archer if we added a ranged weapon.
								if( weapon is BaseRanged && m_Mobile.AI == AIType.AI_Melee )
								{
									m_Mobile.AI = AIType.AI_Archer;
								}
								
								//Chaning to AI_Melee if we added a melee weapon.
								if( weapon is BaseMeleeWeapon && m_Mobile.AI == AIType.AI_Archer )
								{
									m_Mobile.AI = AIType.AI_Melee;
								}
								
								//Trying to recover a shield that the mobile was wearing before.
								if( shield == null && needsshield )
								{
									try
									{
										shield = m_Mobile.Backpack.FindItemByType( typeof( BaseShield ) ) as BaseShield;
									}
									
									catch
									{
										break;
									}
									
									if( shield != null )
										m_Mobile.EquipItem( shield );
								}
								
								return;
							}
								
							case 0x155: // *come
							{
								if( !isOwner )
									break;

								if( WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Come;
								}

								return;
							}
							case 0x156: // *drop
							{
								if( !isOwner )
									break;

								if( !m_Mobile.IsDeadPet && !m_Mobile.Summoned && WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Drop;
								}

								return;
							}
							case 0x15A: // *follow
							{
								if( WasNamed( speech ) )
									BeginPickTarget( e.Mobile, OrderType.Follow );

								return;
							}
							case 0x15B: // *friend
							{
								if( !isOwner )
									break;

								if( WasNamed( speech ) )
								{
									if( m_Mobile.Summoned )
										e.Mobile.SendLocalizedMessage( 1005481 ); // Summoned creatures are loyal only to their summoners.
									else if( e.Mobile.HasTrade )
										e.Mobile.SendLocalizedMessage( 1070947 ); // You cannot friend a pet with a trade pending
									else
										BeginPickTarget( e.Mobile, OrderType.Friend );
								}

								return;
							}
							case 0x15C: // *guard
							{
								if( !isOwner )
									break;

								if( !m_Mobile.IsDeadPet && WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Guard;
								}

								return;
							}
							case 0x15D: // *kill
							case 0x15E: // *attack
							{
								if( !isOwner )
									break;

								if( !m_Mobile.IsDeadPet && WasNamed( speech ) )
									BeginPickTarget( e.Mobile, OrderType.Attack );

								return;
							}
							case 0x15F: // *patrol
							{
								if( !isOwner )
									break;

								if( WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Patrol;
								}

								return;
							}
							case 0x161: // *stop
							{
								if( WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Stop;
                                    m_Mobile.Aggressors.Clear();
                                    m_Mobile.Combatant = null;
                                    m_Mobile.Warmode = false;
								}

								return;
							}
							case 0x163: // *follow me
							{
                                if (WasNamed(speech))
                                {
                                    m_Mobile.DebugSay("I was named, will follow " + e.Mobile.Name);
                                    m_Mobile.ControlTarget = e.Mobile;
                                    m_Mobile.ControlOrder = OrderType.Follow;
                                }
                                else
                                    m_Mobile.DebugSay("I was not named");

								return;
							}
							case 0x16D: // *release
							{
								if( !isOwner )
									break;

								if( WasNamed( speech ) )
								{
									if( !m_Mobile.Summoned )
									{
										e.Mobile.SendGump( new Gumps.ConfirmReleaseGump( e.Mobile, m_Mobile ) );
									}
									else
									{
										m_Mobile.ControlTarget = null;
										m_Mobile.ControlOrder = OrderType.Release;
									}
								}

								return;
							}
							case 0x16E: // *transfer
							{
								if( !isOwner )
									break;

								if( !m_Mobile.IsDeadPet && WasNamed( speech ) )
								{
									if( m_Mobile.Summoned )
										e.Mobile.SendLocalizedMessage( 1005487 ); // You cannot transfer ownership of a summoned creature.
									else if( e.Mobile.HasTrade )
										e.Mobile.SendLocalizedMessage( 1010507 ); // You cannot transfer a pet with a trade pending
									else
										BeginPickTarget( e.Mobile, OrderType.Transfer );
								}

								return;
							}
							case 0x16F: // *stay
							{
								if( WasNamed( speech ) )
								{
									m_Mobile.ControlTarget = null;
									m_Mobile.ControlOrder = OrderType.Stay;
								}

								return;
							}
						}
					}
				}
			}
			else
			{
				if( e.Mobile.AccessLevel >= AccessLevel.GameMaster || ((m_Mobile.CreatureGroup == CreatureGroup.Undead || m_Mobile is IUndead) && 
                    e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.ControlUndead) > 0) )
				{
                    if( e.Mobile is PlayerMobile && e.Mobile.AccessLevel < AccessLevel.GameMaster )
                    {
                        int featlevel = ( (PlayerMobile)e.Mobile ).Feats.GetFeatLevel( FeatList.ControlUndead );

                        if( featlevel < 3 )
                        {
                            if( featlevel < 2 && m_Mobile.Fame > 5000 )
                                return;
                            else if( m_Mobile.Fame > 15000 )
                                return;
                        }
                    }

					m_Mobile.DebugSay( "It's from a GM" );

					if( m_Mobile.FindMyName( e.Speech, true ) )
					{
						string[] str = e.Speech.Split( ' ' );
						int i;

						for( i=0; i < str.Length; i++ )
						{
							string word = str[i];

							if( Insensitive.Equals( word, "obey" ) )
							{
								m_Mobile.SetControlMaster( e.Mobile );

								if( m_Mobile.Summoned )
									m_Mobile.SummonMaster = e.Mobile;

								return;
							}
						}
					}
				}
			}
		}

		public virtual bool Think()
		{
			if( m_Mobile.Deleted )
				return false;
				
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_Mobile );
			if ( csa.AttackTimer != null || csa.DefenseTimer != null )
				return true;

			if( CheckFlee() )
				return true;

			switch( Action )
			{
				case ActionType.Wander:
				m_Mobile.OnActionWander();
				return DoActionWander();

				case ActionType.Combat:
				m_Mobile.OnActionCombat();
				return DoActionCombat();

				case ActionType.Guard:
				m_Mobile.OnActionGuard();
				return DoActionGuard();

				case ActionType.Flee:
				m_Mobile.OnActionFlee();
				return DoActionFlee();

				case ActionType.Interact:
				m_Mobile.OnActionInteract();
				return DoActionInteract();

				case ActionType.Backoff:
				m_Mobile.OnActionBackoff();
				return DoActionBackoff();

				default:
				return false;
			}
		}

		public virtual void OnActionChanged()
		{
			switch( Action )
			{
				case ActionType.Wander:
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				m_Mobile.FocusMob = null;
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				break;

				case ActionType.Combat:
				m_Mobile.Warmode = true;
				m_Mobile.FocusMob = null;
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				break;

				case ActionType.Guard:
				m_Mobile.Warmode = true;
				m_Mobile.FocusMob = null;
				m_Mobile.Combatant = null;
				m_NextStopGuard = DateTime.Now + TimeSpan.FromSeconds( 10 );
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				break;

				case ActionType.Flee:
				m_Mobile.Warmode = true;
				m_Mobile.FocusMob = null;
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				break;

				case ActionType.Interact:
				m_Mobile.Warmode = false;
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				break;

				case ActionType.Backoff:
				m_Mobile.Warmode = false;
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				break;
			}
		}

		public virtual bool OnAtWayPoint()
		{
			return true;
		}

		public virtual bool DoActionWander()
		{
			if( CheckHerding() )
			{
				m_Mobile.DebugSay( "Praise the shepherd!" );
			}
			else if( m_Mobile.CurrentWayPoint != null )
			{
				WayPoint point = m_Mobile.CurrentWayPoint;
				if( (point.X != m_Mobile.Location.X || point.Y != m_Mobile.Location.Y) && point.Map == m_Mobile.Map && point.Parent == null && !point.Deleted )
				{
					m_Mobile.DebugSay( "I will move towards my waypoint." );
					DoMove( m_Mobile.GetDirectionTo( m_Mobile.CurrentWayPoint ) );
				}
				else if( OnAtWayPoint() )
				{
					m_Mobile.DebugSay( "I will go to the next waypoint" );
					m_Mobile.CurrentWayPoint = point.NextPoint;
					if( point.NextPoint != null && point.NextPoint.Deleted )
						m_Mobile.CurrentWayPoint = point.NextPoint = point.NextPoint.NextPoint;
				}
			}
            else if ((m_Mobile.CurrentWayPoint == null || m_Mobile.CurrentWayPoint.Deleted) && m_Mobile is Soldier && m_Mobile.RangeHome > 0)
            {
                // Adding to try and get this guy to return home when he's just wandering.
                m_Mobile.Move(m_Mobile.GetDirectionTo(m_Mobile.Home));
            }
            else if (m_Mobile.IsAnimatedDead)
            {
                // animated dead follow their master
                Mobile master = m_Mobile.SummonMaster;

                if (master != null && master.Map == m_Mobile.Map && master.InRange(m_Mobile, m_Mobile.RangePerception))
                    MoveTo(master, false, 1);
                else
                    WalkRandomInHome(2, 2, 1);
            }
            else if (CheckMove())
            {
                if (!m_Mobile.CheckIdle())
                    WalkRandomInHome(2, 2, 1);
            }

			if( m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted && m_Mobile.Combatant.Alive && !m_Mobile.Combatant.IsDeadBondedPet )
			{
				m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Mobile.Combatant );
			}

			return true;
		}

		public virtual bool DoActionCombat()
		{
			Mobile c = m_Mobile.Combatant;

			if( c == null || c.Deleted || c.Map != m_Mobile.Map || !c.Alive || c.IsDeadBondedPet )
				Action = ActionType.Wander;
			else
				m_Mobile.Direction = m_Mobile.GetDirectionTo( c );

			return true;
		}

		public virtual bool DoActionGuard()
		{
			if( DateTime.Now < m_NextStopGuard )
			{
				m_Mobile.DebugSay( "I am on guard" );
				//m_Mobile.Turn( Utility.Random(0, 2) - 1 );
			}
			else
			{
				m_Mobile.DebugSay( "I stop being on guard" );
				Action = ActionType.Wander;
			}

			return true;
		}

		public virtual bool DoActionFlee()
		{
			Mobile from = m_Mobile.FocusMob;

			if( from == null || from.Deleted || from.Map != m_Mobile.Map )
			{
				m_Mobile.DebugSay( "I have lost him" );
				Action = ActionType.Guard;
				return true;
			}

			if( WalkMobileRange( from, 1, true, m_Mobile.RangePerception*2, m_Mobile.RangePerception*3 ) )
			{
				m_Mobile.DebugSay( "I have fled" );
				Action = ActionType.Guard;
				return true;
			}
			else
			{
				m_Mobile.DebugSay( "I am fleeing!" );
			}

			return true;
		}

		public virtual bool DoActionInteract()
		{
			return true;
		}

		public virtual bool DoActionBackoff()
		{
			return true;
		}

		public virtual bool Obey()
		{
            m_Mobile.DebugSay("Obey() entered");
			if( m_Mobile.Deleted )
				return false;

			switch( m_Mobile.ControlOrder )
			{
				case OrderType.None:
				return DoOrderNone();

				case OrderType.Come:
				return DoOrderCome();

				case OrderType.Drop:
				return DoOrderDrop();

				case OrderType.Friend:
				return DoOrderFriend();

				case OrderType.Unfriend:
				return DoOrderUnfriend();

				case OrderType.Guard:
				return DoOrderGuard();

				case OrderType.Attack:
				return DoOrderAttack();

				case OrderType.Patrol:
				return DoOrderPatrol();

				case OrderType.Release:
				return DoOrderRelease();

				case OrderType.Stay:
				return DoOrderStay();

				case OrderType.Stop:
				return DoOrderStop();

				case OrderType.Follow:
				return DoOrderFollow();

				case OrderType.Transfer:
				return DoOrderTransfer();

				default:
				return false;
			}
		}

		public virtual void OnCurrentOrderChanged()
		{
            if (m_Mobile.Deleted)
                return;
            if (m_Mobile.ControlMaster != null && m_Mobile.ControlMaster.Deleted)
                return;
            if (m_Mobile.ControlMaster == null && !(m_Mobile is Soldier))
				return;

            m_Mobile.DebugSay("OnCurrentOrderChanged = " + m_Mobile.ControlOrder.ToString());

			switch( m_Mobile.ControlOrder )
			{
				case OrderType.None:

                    if (!(m_Mobile is Soldier))
                        m_Mobile.ControlMaster.RevealingAction();
                    else
                    { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }

				m_Mobile.Home = m_Mobile.Location;
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Come:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Drop:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = true;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Friend:
				case OrderType.Unfriend:
				m_Mobile.ControlMaster.RevealingAction();

				break;
				case OrderType.Guard:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = true;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Attack:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );

				m_Mobile.Warmode = true;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Patrol:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }

				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Release:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Stay:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Stop:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );
				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
                m_Mobile.Aggressors.Clear();
				break;

				case OrderType.Follow:

                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                { if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction(); }
				m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );

				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;

				case OrderType.Transfer:
                if (!(m_Mobile is Soldier))
                    m_Mobile.ControlMaster.RevealingAction();
                else
                {
                    if (m_Speaker != null && !m_Speaker.Deleted) m_Speaker.RevealingAction();
                    m_Mobile.CurrentSpeed = m_Mobile.PassiveSpeed;
                }
				m_Mobile.PlaySound( m_Mobile.GetIdleSound() );

				m_Mobile.Warmode = false;
				m_Mobile.Combatant = null;
				break;
			}
		}

		public virtual bool DoOrderNone()
		{
            if (m_Mobile is Soldier)
                Think();
            else if((m_Mobile.Combatant == null || m_Mobile.Combatant.Deleted) && (!m_Mobile.Controlled))
            {
                WalkRandomInHome(3, 2, 1);
                m_Mobile.DebugSay("I have no order");
            }
            else if (m_Mobile is Employee && (m_Mobile.Combatant == null || m_Mobile.Combatant.Deleted))
            {
                Employee e = m_Mobile as Employee;
                if (e.Node != null && !e.Node.Deleted && e.InRange(e.Node.Location, 20))
                {
                    e.Home = e.Node.Location;
                    e.RangeHome = 15;
                    WalkRandomInHome(50, 1, 1);
                }
            }

			if( m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted && m_Mobile.Combatant.Alive && !m_Mobile.Combatant.IsDeadBondedPet )
			{
				m_Mobile.Warmode = true;
				m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Mobile.Combatant );
                if (m_Mobile.Combatant != m_Mobile.ControlMaster && m_Mobile.CanSee(m_Mobile.Combatant) && m_Mobile.CanBeHarmful(m_Mobile.Combatant, false) && m_Mobile.Combatant.Map == m_Mobile.Map)
                {
                    m_Mobile.DebugSay("No orders; acquiring target...");
                    m_Mobile.FocusMob = m_Mobile.Combatant;
                    Action = ActionType.Combat;
                    Think();
                }
			}
			else
			{
				m_Mobile.Warmode = false;
			}

			return true;
		}

		public virtual bool DoOrderCome()
		{
			if(( m_Mobile.ControlMaster != null && !m_Mobile.ControlMaster.Deleted ) || (m_Mobile is Soldier))
			{
                int iCurrDist = 0;

                if (!(m_Mobile is Soldier))
                    iCurrDist = (int)m_Mobile.GetDistanceToSqrt(m_Mobile.ControlMaster);
                else
                {
                    if (m_Speaker != null && !m_Speaker.Deleted)
                        iCurrDist = (int)m_Mobile.GetDistanceToSqrt(m_Speaker);
                }

				if( iCurrDist > m_Mobile.RangePerception )
				{
					m_Mobile.DebugSay( "I have lost my master. I stay here" );
					m_Mobile.ControlTarget = null;
					m_Mobile.ControlOrder = OrderType.None;
				}
				else
				{
					m_Mobile.DebugSay( "My master told me come" );

					// Not exactly OSI style, but better than nothing.
					bool bRun = (iCurrDist > 5);

					if( WalkMobileRange( m_Mobile.ControlMaster, 1, bRun, 0, 1 ) )
					{
						if( m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted && m_Mobile.Combatant.Alive && !m_Mobile.Combatant.IsDeadBondedPet )
						{
							m_Mobile.Warmode = true;
							m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Mobile.Combatant );
						}
						else
						{
							m_Mobile.Warmode = false;
						}
					}
				}
			}

			return true;
		}

		public virtual bool DoOrderDrop()
		{
			if( m_Mobile.IsDeadPet || !m_Mobile.CanDrop )
				return true;

			m_Mobile.DebugSay( "I drop my stuff for my master" );

			Container pack = m_Mobile.Backpack;

			if( pack != null )
			{
				List<Item> list = pack.Items;

				for( int i = list.Count - 1; i >= 0; --i )
					if( i < list.Count )
						list[i].MoveToWorld( m_Mobile.Location, m_Mobile.Map );
			}

			m_Mobile.ControlTarget = null;
			m_Mobile.ControlOrder = OrderType.None;

			return true;
		}

		public virtual bool CheckHerding()
		{
			Point2D target = m_Mobile.TargetLocation;

			if( target == Point2D.Zero )
				return false; // Creature is not being herded

			double distance = m_Mobile.GetDistanceToSqrt( target );

			if( distance < 1 || distance > 20 )
			{
				m_Mobile.TargetLocation = Point2D.Zero;
				return false; // At the target or too far away
			}

			DoMove( m_Mobile.GetDirectionTo( target ) );

			return true;
		}

		public virtual bool DoOrderFollow()
		{
			if( CheckHerding() )
			{
				m_Mobile.DebugSay( "Praise the shepherd!" );
			}
			else if( m_Mobile.ControlTarget != null && !m_Mobile.ControlTarget.Deleted && m_Mobile.ControlTarget != m_Mobile )
			{
				int iCurrDist = (int)m_Mobile.GetDistanceToSqrt( m_Mobile.ControlTarget );

				if( iCurrDist > m_Mobile.RangePerception )
				{
					m_Mobile.DebugSay( "I have lost the one to follow. I stay here" );

					if( m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted && m_Mobile.Combatant.Alive && !m_Mobile.Combatant.IsDeadBondedPet )
					{
						m_Mobile.Warmode = true;
						m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Mobile.Combatant );
					}
					else
					{
						m_Mobile.Warmode = false;
					}
				}
				else
				{
					m_Mobile.DebugSay( "My master told me to follow: {0}", m_Mobile.ControlTarget.Name );

					// Not exactly OSI style, but better than nothing.
					bool bRun = (iCurrDist > 5);

					if( WalkMobileRange( m_Mobile.ControlTarget, 1, bRun, 0, 1 ) )
					{
						if( m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted && m_Mobile.Combatant.Alive && !m_Mobile.Combatant.IsDeadBondedPet )
						{
							m_Mobile.Warmode = true;
							m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Mobile.Combatant );
						}
						else
						{
							m_Mobile.Warmode = false;
						}
					}
				}
			}
			else
			{
				m_Mobile.DebugSay( "I have nobody to follow" );
				m_Mobile.ControlTarget = null;
				m_Mobile.ControlOrder = OrderType.None;
			}

			return true;
		}

		public virtual bool DoOrderFriend()
		{
			Mobile from = m_Mobile.ControlMaster;
			Mobile to = m_Mobile.ControlTarget;

			if( from == null || to == null || from == to || from.Deleted || to.Deleted || !to.Player )
			{
				m_Mobile.PublicOverheadMessage( MessageType.Regular, 0x3B2, 502039 ); // *looks confused*
			}
			else
			{
				bool youngFrom = from is PlayerMobile ? ((PlayerMobile)from).Young : false;
				bool youngTo = to is PlayerMobile ? ((PlayerMobile)to).Young : false;

				if( youngFrom && !youngTo )
				{
					from.SendLocalizedMessage( 502040 ); // As a young player, you may not friend pets to older players.
				}
				else if( !youngFrom && youngTo )
				{
					from.SendLocalizedMessage( 502041 ); // As an older player, you may not friend pets to young players.
				}
				else if( from.CanBeBeneficial( to, true ) )
				{
					NetState fromState = from.NetState, toState = to.NetState;

					if( fromState != null && toState != null )
					{
						if( from.HasTrade )
						{
							from.SendLocalizedMessage( 1070947 ); // You cannot friend a pet with a trade pending
						}
						else if( to.HasTrade )
						{
							to.SendLocalizedMessage( 1070947 ); // You cannot friend a pet with a trade pending
						}
						else if( m_Mobile.IsPetFriend( to ) )
						{
							from.SendLocalizedMessage( 1049691 ); // That person is already a friend.
						}
						else if( !m_Mobile.AllowNewPetFriend )
						{
							from.SendLocalizedMessage( 1005482 ); // Your pet does not seem to be interested in making new friends right now.
						}
						else
						{
							// ~1_NAME~ will now accept movement commands from ~2_NAME~.
							from.SendLocalizedMessage( 1049676, String.Format( "{0}\t{1}", m_Mobile.Name, to.Name ) );

							/* ~1_NAME~ has granted you the ability to give orders to their pet ~2_PET_NAME~.
							 * This creature will now consider you as a friend.
							 */
							to.SendLocalizedMessage( 1043246, String.Format( "{0}\t{1}", from.Name, m_Mobile.Name ) );

							m_Mobile.AddPetFriend( to );

							m_Mobile.ControlTarget = to;
							m_Mobile.ControlOrder = OrderType.Follow;

							return true;
						}
					}
				}
			}

			m_Mobile.ControlTarget = from;
			m_Mobile.ControlOrder = OrderType.Follow;

			return true;
		}

		public virtual bool DoOrderUnfriend()
		{
			Mobile from = m_Mobile.ControlMaster;
			Mobile to = m_Mobile.ControlTarget;

			if( from == null || to == null || from == to || from.Deleted || to.Deleted || !to.Player )
			{
				m_Mobile.PublicOverheadMessage( MessageType.Regular, 0x3B2, 502039 ); // *looks confused*
			}
			else if( !m_Mobile.IsPetFriend( to ) )
			{
				from.SendLocalizedMessage( 1070953 ); // That person is not a friend.
			}
			else
			{
				// ~1_NAME~ will no longer accept movement commands from ~2_NAME~.
				from.SendLocalizedMessage( 1070951, String.Format( "{0}\t{1}", m_Mobile.Name, to.Name ) );

				/* ~1_NAME~ has no longer granted you the ability to give orders to their pet ~2_PET_NAME~.
				 * This creature will no longer consider you as a friend.
				 */
				to.SendLocalizedMessage( 1070952, String.Format( "{0}\t{1}", from.Name, m_Mobile.Name ) );

				m_Mobile.RemovePetFriend( to );
			}

			m_Mobile.ControlTarget = from;
			m_Mobile.ControlOrder = OrderType.Follow;

			return true;
		}

		public virtual bool DoOrderGuard()
		{
			if( m_Mobile.IsDeadPet )
				return true;

            Mobile controlMaster;

            if (m_Mobile is Soldier && m_Speaker != null)
                controlMaster = m_Speaker;
            else if (GroupInfo.HasGroup(m_Mobile))
                controlMaster = GroupInfo.GetGroup(m_Mobile).Leader;
            else
                controlMaster = m_Mobile.ControlMaster;

			if( controlMaster == null || controlMaster.Deleted )
				return true;

			Mobile combatant = m_Mobile.Combatant;

			List<AggressorInfo> aggressors = controlMaster.Aggressors;

			if( aggressors.Count > 0 )
			{
				for( int i = 0; i < aggressors.Count; ++i )
				{
					AggressorInfo info = aggressors[i];
					Mobile attacker = info.Attacker;

					if( attacker != null && !attacker.Deleted && attacker.GetDistanceToSqrt( m_Mobile ) <= m_Mobile.RangePerception )
					{
						if( combatant == null || attacker.GetDistanceToSqrt( controlMaster ) < combatant.GetDistanceToSqrt( controlMaster ) )
							combatant = attacker;
					}
				}

				if( combatant != null )
					m_Mobile.DebugSay( "Crap, my master has been attacked! I will atack one of those bastards!" );
			}

			if( combatant != null && combatant != m_Mobile && combatant != m_Mobile.ControlMaster && !combatant.Deleted && combatant.Alive && !combatant.IsDeadBondedPet && m_Mobile.CanSee( combatant ) && m_Mobile.CanBeHarmful( combatant, false ) && combatant.Map == m_Mobile.Map )
			{
				m_Mobile.DebugSay( "Guarding from target..." );

				m_Mobile.Combatant = combatant;
				m_Mobile.FocusMob = combatant;
				Action = ActionType.Combat;
				
				/*
				 * We need to call Think() here or spell casting monsters will not use
				 * spells when guarding because their target is never processed.
				 */

				Think();
			}
			else
			{
				m_Mobile.DebugSay( "Nothing to guard from" );

				m_Mobile.Warmode = false;

				WalkMobileRange( controlMaster, 1, false, 0, 1 );
			}

			return true;
		}

		public virtual bool DoOrderAttack()
		{
			if( m_Mobile.IsDeadPet )
				return true;

			if( m_Mobile.ControlTarget == null || m_Mobile.ControlTarget.Deleted || m_Mobile.ControlTarget.Map != m_Mobile.Map || !m_Mobile.ControlTarget.Alive || m_Mobile.ControlTarget.IsDeadBondedPet )
			{
				m_Mobile.DebugSay( "I think he might be dead. He's not anywhere around here at least. That's cool. I'm glad he's dead." );

				m_Mobile.ControlTarget = null;
				m_Mobile.ControlOrder = OrderType.None;

				if( m_Mobile.FightMode == FightMode.Closest || m_Mobile.FightMode == FightMode.Aggressor )
				{
					Mobile newCombatant = null;
					double newScore = 0.0;

					List<AggressorInfo> list = m_Mobile.Aggressors;

					for( int i = 0; i < list.Count; ++i )
					{
						Mobile aggr = list[i].Attacker;

						if( aggr.Map != m_Mobile.Map || !aggr.InRange( m_Mobile.Location, m_Mobile.RangePerception ) || !m_Mobile.CanSee( aggr ) )
							continue;

						if( aggr.IsDeadBondedPet || !aggr.Alive )
							continue;

						double aggrScore = m_Mobile.GetValueFrom( aggr, FightMode.Closest, false );

						if( (newCombatant == null || aggrScore > newScore) && m_Mobile.InLOS( aggr ) )
						{
							newCombatant = aggr;
							newScore = aggrScore;
						}
					}

					if( newCombatant != null )
					{
						m_Mobile.ControlTarget = newCombatant;
						m_Mobile.ControlOrder = OrderType.Attack;
						m_Mobile.Combatant = newCombatant;
						m_Mobile.DebugSay( "But -that- is not dead. Here we go again..." );
						Think();
					}
				}
			}
			else
			{
				m_Mobile.DebugSay( "Attacking target..." );
				Think();
			}

			return true;
		}

		public virtual bool DoOrderPatrol()
		{
			m_Mobile.DebugSay( "This order is not yet coded" );
			return true;
		}

		public virtual bool DoOrderRelease()
		{
            if (m_Mobile is Soldier)
                return false;

			m_Mobile.DebugSay( "I have been released" );

			m_Mobile.PlaySound( m_Mobile.GetAngerSound() );

			if( m_Mobile.VanishTime != DateTime.MinValue && m_Mobile.ControlMaster != null )
				m_Mobile.ControlMaster.SendMessage( "You dismiss the creature and it is sent back to whence it came." );
				                                   
			else if( m_Mobile.ControlMaster != null )
				m_Mobile.ControlMaster.SendMessage( "Unless it is tamed again, this creature will be killed during the next world cleansing." );
			
			m_Mobile.MarkedForTermination = true;
			m_Mobile.ReleaseTime = DateTime.Now;
			m_Mobile.SetControlMaster( null );
			m_Mobile.SummonMaster = null;

			m_Mobile.BondingBegin = DateTime.MinValue;
			m_Mobile.OwnerAbandonTime = DateTime.MinValue;
			m_Mobile.IsBonded = false;

			SpawnEntry se = m_Mobile.Spawner as SpawnEntry;
			if( se != null && se.Home != Point3D.Zero )
			{
				m_Mobile.Home = se.Home;
				m_Mobile.RangeHome = se.Range;
			}

			if( m_Mobile.DeleteOnRelease || m_Mobile.IsDeadPet )
				m_Mobile.Delete();

			return true;
		}

		public virtual bool DoOrderStay()
		{
			if( CheckHerding() )
				m_Mobile.DebugSay( "Praise the shepherd!" );
			else
				m_Mobile.DebugSay( "My master told me to stay" );

            m_Mobile.Home = m_Mobile.Location;
            m_Mobile.ControlTarget = null;

            m_Mobile.ControlOrder = OrderType.None;

			return true;
		}

		public virtual bool DoOrderStop()
		{
			if(( m_Mobile.ControlMaster == null || m_Mobile.ControlMaster.Deleted) && !(m_Mobile is Soldier))
				return true;

			m_Mobile.DebugSay( "My master told me to stop." );

            if (GroupInfo.HasGroup(m_Mobile))
                m_Mobile.Direction = m_Mobile.GetDirectionTo(GroupInfo.GetGroup(m_Mobile).Leader);
            else if (!(m_Mobile is Soldier) && m_Mobile.ControlMaster != null)
                m_Mobile.Direction = m_Mobile.GetDirectionTo(m_Mobile.ControlMaster);
            else if (m_Mobile is Soldier)
            {
                if (m_Speaker != null && !m_Speaker.Deleted)
                m_Mobile.Direction = m_Mobile.GetDirectionTo(m_Speaker);
            }

			m_Mobile.ControlTarget = null;

			return true;
		}

		private class TransferItem : Item
		{
			public static bool IsInCombat( BaseCreature creature )
			{
				return (creature != null && (creature.Aggressors.Count > 0 || creature.Aggressed.Count > 0));
			}

			private BaseCreature m_Creature;

			public TransferItem( BaseCreature creature )
				: base( ShrinkTable.Lookup( creature ) )
			{
				m_Creature = creature;

				Movable = false;

				if( !Core.AOS )
				{
					Name = creature.Name;
				}
				else if( this.ItemID == ShrinkTable.DefaultItemID ||  creature.GetType().IsDefined( typeof( FriendlyNameAttribute ), false ) )
					Name = FriendlyNameAttribute.GetFriendlyNameFor( creature.GetType() ).ToString();

				//(As Per OSI)No name.  Normally, set by the ItemID of the Shrink Item unless we either explicitly set it with an Attribute, or, no lookup found
			}

			public TransferItem( Serial serial )
				: base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int)0 ); // version
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				Delete();
			}

			public override void GetProperties( ObjectPropertyList list )
			{
				base.GetProperties( list );

				list.Add( 1041603 ); // This item represents a pet currently in consideration for trade
				list.Add( 1041601, m_Creature.Name ); // Pet Name: ~1_val~

				if ( m_Creature.ControlMaster != null )
					list.Add( 1041602, m_Creature.ControlMaster.Name ); // Owner: ~1_val~
			}

			public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
			{
                if (m_Creature is Soldier)
                    return false;

				if( !base.AllowSecureTrade( from, to, newOwner, accepted ) )
					return false;

				if( Deleted || m_Creature == null || m_Creature.Deleted || m_Creature.ControlMaster != from || !from.CheckAlive() || !to.CheckAlive() )
					return false;

				if( from.Map != m_Creature.Map || !from.InRange( m_Creature, 14 ) )
					return false;

				bool youngFrom = from is PlayerMobile ? ((PlayerMobile)from).Young : false;
				bool youngTo = to is PlayerMobile ? ((PlayerMobile)to).Young : false;

				if( accepted && youngFrom && !youngTo )
				{
					from.SendLocalizedMessage( 502051 ); // As a young player, you may not transfer pets to older players.
				}
				else if( accepted && !youngFrom && youngTo )
				{
					from.SendLocalizedMessage( 502052 ); // As an older player, you may not transfer pets to young players.
				}
				else if( accepted && !m_Creature.CanBeControlledBy( to ) )
				{
					string args = String.Format( "{0}\t{1}\t ", to.Name, from.Name );

					from.SendLocalizedMessage( 1043248, args ); // The pet refuses to be transferred because it will not obey ~1_NAME~.~3_BLANK~
					to.SendLocalizedMessage( 1043249, args ); // The pet will not accept you as a master because it does not trust you.~3_BLANK~

					return false;
				}
				else if( accepted && !m_Creature.CanBeControlledBy( from ) )
				{
					string args = String.Format( "{0}\t{1}\t ", to.Name, from.Name );

					from.SendLocalizedMessage( 1043250, args ); // The pet refuses to be transferred because it will not obey you sufficiently.~3_BLANK~
					to.SendLocalizedMessage( 1043251, args ); // The pet will not accept you as a master because it does not trust ~2_NAME~.~3_BLANK~
				}
				else if( accepted && (to.Followers + m_Creature.ControlSlots) > to.FollowersMax )
				{
					to.SendLocalizedMessage( 1049607 ); // You have too many followers to control that creature.

					return false;
				}
				else if( accepted && IsInCombat( m_Creature ) )
				{
					from.SendMessage( "You may not transfer a pet that has recently been in combat." );
					to.SendMessage( "The pet may not be transfered to you because it has recently been in combat." );

					return false;
				}

				return true;
			}

			public override void OnSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
			{
				if( Deleted )
					return;

                if (m_Creature is Soldier)
                    return;

				Delete();

				if( m_Creature == null || m_Creature.Deleted || m_Creature.ControlMaster != from || !from.CheckAlive() || !to.CheckAlive() )
					return;

				if( from.Map != m_Creature.Map || !from.InRange( m_Creature, 14 ) )
					return;

				if( accepted )
				{
					if( m_Creature.SetControlMaster( to ) )
					{
						if( m_Creature.Summoned )
							m_Creature.SummonMaster = to;

						m_Creature.ControlTarget = to;
						m_Creature.ControlOrder = OrderType.Follow;

						m_Creature.BondingBegin = DateTime.MinValue;
						m_Creature.OwnerAbandonTime = DateTime.MinValue;
						m_Creature.IsBonded = false;

						m_Creature.PlaySound( m_Creature.GetIdleSound() );

						string args = String.Format( "{0}\t{1}\t{2}", from.Name, m_Creature.Name, to.Name );

						from.SendLocalizedMessage( 1043253, args ); // You have transferred your pet to ~3_GETTER~.
						to.SendLocalizedMessage( 1043252, args ); // ~1_NAME~ has transferred the allegiance of ~2_PET_NAME~ to you.
					}
				}
			}
		}

		public virtual bool DoOrderTransfer()
		{
			if( m_Mobile.IsDeadPet )
				return true;

            if (m_Mobile is Soldier)
                return true;

			Mobile from = m_Mobile.ControlMaster;
			Mobile to = m_Mobile.ControlTarget;

			if( from != to && from != null && !from.Deleted && to != null && !to.Deleted && to.Player )
			{
				m_Mobile.DebugSay( "Begin transfer with {0}", to.Name );

				bool youngFrom = from is PlayerMobile ? ((PlayerMobile)from).Young : false;
				bool youngTo = to is PlayerMobile ? ((PlayerMobile)to).Young : false;

				if( youngFrom && !youngTo )
				{
					from.SendLocalizedMessage( 502051 ); // As a young player, you may not transfer pets to older players.
				}
				else if( !youngFrom && youngTo )
				{
					from.SendLocalizedMessage( 502052 ); // As an older player, you may not transfer pets to young players.
				}
				else if( !m_Mobile.CanBeControlledBy( to ) )
				{
					string args = String.Format( "{0}\t{1}\t ", to.Name, from.Name );

					from.SendLocalizedMessage( 1043248, args ); // The pet refuses to be transferred because it will not obey ~1_NAME~.~3_BLANK~
					to.SendLocalizedMessage( 1043249, args ); // The pet will not accept you as a master because it does not trust you.~3_BLANK~
				}
				else if( !m_Mobile.CanBeControlledBy( from ) )
				{
					string args = String.Format( "{0}\t{1}\t ", to.Name, from.Name );

					from.SendLocalizedMessage( 1043250, args ); // The pet refuses to be transferred because it will not obey you sufficiently.~3_BLANK~
					to.SendLocalizedMessage( 1043251, args ); // The pet will not accept you as a master because it does not trust ~2_NAME~.~3_BLANK~
				}
				else if( TransferItem.IsInCombat( m_Mobile ) )
				{
					from.SendMessage( "You may not transfer a pet that has recently been in combat." );
					to.SendMessage( "The pet may not be transfered to you because it has recently been in combat." );
				}
				else
				{
					NetState fromState = from.NetState, toState = to.NetState;

					if( fromState != null && toState != null )
					{
						if( from.HasTrade )
						{
							from.SendLocalizedMessage( 1010507 ); // You cannot transfer a pet with a trade pending
						}
						else if( to.HasTrade )
						{
							to.SendLocalizedMessage( 1010507 ); // You cannot transfer a pet with a trade pending
						}
						else
						{
							Container c = fromState.AddTrade( toState );
							c.DropItem( new TransferItem( m_Mobile ) );
						}
					}
				}
			}

			m_Mobile.ControlTarget = null;
			m_Mobile.ControlOrder = OrderType.Stay;

			return true;
		}

		public virtual bool DoBardPacified()
		{
			if( DateTime.Now < m_Mobile.BardEndTime )
			{
				m_Mobile.DebugSay( "I am pacified, I wait" );
				m_Mobile.Combatant = null;
				m_Mobile.Warmode = false;

			}
			else
			{
				m_Mobile.DebugSay( "I'm not pacified any longer" );
				m_Mobile.BardPacified = false;
			}

			return true;
		}

		public virtual bool DoBardProvoked()
		{
			if( DateTime.Now >= m_Mobile.BardEndTime && (m_Mobile.BardMaster == null || m_Mobile.BardMaster.Deleted || m_Mobile.BardMaster.Map != m_Mobile.Map || m_Mobile.GetDistanceToSqrt( m_Mobile.BardMaster ) > m_Mobile.RangePerception) )
			{
				m_Mobile.DebugSay( "I have lost my provoker" );
				m_Mobile.BardProvoked = false;
				m_Mobile.BardMaster = null;
				m_Mobile.BardTarget = null;

				m_Mobile.Combatant = null;
				m_Mobile.Warmode = false;
			}
			else
			{
				if( m_Mobile.BardTarget == null || m_Mobile.BardTarget.Deleted || m_Mobile.BardTarget.Map != m_Mobile.Map || m_Mobile.GetDistanceToSqrt( m_Mobile.BardTarget ) > m_Mobile.RangePerception )
				{
					m_Mobile.DebugSay( "I have lost my provoke target" );
					m_Mobile.BardProvoked = false;
					m_Mobile.BardMaster = null;
					m_Mobile.BardTarget = null;

					m_Mobile.Combatant = null;
					m_Mobile.Warmode = false;
				}
				else
				{
					m_Mobile.Combatant = m_Mobile.BardTarget;
					m_Action = ActionType.Combat;

					m_Mobile.OnThink();
					Think();
				}
			}

			return true;
		}

		public virtual void WalkRandom( int iChanceToNotMove, int iChanceToDir, int iSteps )
		{
			if( m_Mobile.Deleted || m_Mobile.DisallowAllMoves )
				return;

			for( int i=0; i<iSteps; i++ )
			{
				if( Utility.Random( 8 * iChanceToNotMove ) <= 8 )
				{
					int iRndMove = Utility.Random( 0, 8 + (9*iChanceToDir) );

					switch( iRndMove )
					{
						case 0:
						DoMove( Direction.Up );
						break;
						case 1:
						DoMove( Direction.North );
						break;
						case 2:
						DoMove( Direction.Left );
						break;
						case 3:
						DoMove( Direction.West );
						break;
						case 5:
						DoMove( Direction.Down );
						break;
						case 6:
						DoMove( Direction.South );
						break;
						case 7:
						DoMove( Direction.Right );
						break;
						case 8:
						DoMove( Direction.East );
						break;
						default:
						DoMove( m_Mobile.Direction );
						break;
					}
				}
			}
		}

		public double TransformMoveDelay( double delay )
		{
			bool isPassive = (delay == m_Mobile.PassiveSpeed);
			bool isControlled = (m_Mobile.Controlled || m_Mobile.Summoned);

            if (m_Mobile is BirdOfPrey && (m_Mobile as BirdOfPrey).Retrieving)
                delay -= 0.075;

            if (!isPassive)
                delay = delay * 0.75;

			if( m_Mobile.Controlled )
			{
				if( m_Mobile.ControlOrder == OrderType.Follow && m_Mobile.ControlTarget == m_Mobile.ControlMaster )
					delay *= 0.5;
			}
            else if (m_Mobile is Soldier)
            {
                if (m_Mobile.ControlOrder == OrderType.Follow && m_Speaker != null && m_Mobile.ControlTarget == m_Speaker)
                    delay *= 0.5;
            }

            if( !m_Mobile.NoWoundedMovePenalty )
            {
                double offset = (double)(m_Mobile.Hits / m_Mobile.HitsMax);

                if( offset < 0.0 )
                    offset = 0.0;
                else if( offset > 1.0 )
                    offset = 1.0;

                offset = 1.0 - offset;

                delay += ( offset * 0.33 );
            }

			if( delay < 0.0 )
				delay = 0.0;

            if (m_Mobile.AI == AIType.AI_Berserk)
                delay = 0.0;

			return delay;
		}

		private DateTime m_NextMove;

		public DateTime NextMove
		{
			get { return m_NextMove; }
			set { m_NextMove = value; }
		}

		public virtual bool CheckMove()
		{
			return (DateTime.Now >= m_NextMove);
		}

		public virtual bool DoMove( Direction d )
		{
			return DoMove( d, false );
		}

		public virtual bool DoMove( Direction d, bool badStateOk )
		{
			MoveResult res = DoMoveImpl( d );

			return (res == MoveResult.Success || res == MoveResult.SuccessAutoTurn || (badStateOk && res == MoveResult.BadState));
		}

		private static Queue m_Obstacles = new Queue();

		public virtual MoveResult DoMoveImpl( Direction d )
		{
			if( m_Mobile.Deleted || m_Mobile.Frozen || m_Mobile.Paralyzed || (m_Mobile.Spell != null && m_Mobile.Spell.IsCasting) || m_Mobile.DisallowAllMoves )
				return MoveResult.BadState;
			else if( !CheckMove() )
				return MoveResult.BadState;

			// This makes them always move one step, never any direction changes
			m_Mobile.Direction = d;

			TimeSpan delay = TimeSpan.FromSeconds( TransformMoveDelay( m_Mobile.CurrentSpeed ) );

			m_NextMove += delay;

			if( m_NextMove < DateTime.Now )
				m_NextMove = DateTime.Now;

			m_Mobile.Pushing = false;

			MoveImpl.IgnoreMovableImpassables = (m_Mobile.CanMoveOverObstacles && !m_Mobile.CanDestroyObstacles);

			if( (m_Mobile.Direction & Direction.Mask) != (d & Direction.Mask) )
			{
				bool v = m_Mobile.Move( d );

				MoveImpl.IgnoreMovableImpassables = false;
				return (v ? MoveResult.Success : MoveResult.Blocked);
			}
			else if( !m_Mobile.Move( d ) )
			{
				bool wasPushing = m_Mobile.Pushing;

				bool blocked = true;

				bool canOpenDoors = m_Mobile.CanOpenDoors;
				bool canDestroyObstacles = m_Mobile.CanDestroyObstacles;

				if( canOpenDoors || canDestroyObstacles )
				{
					m_Mobile.DebugSay( "My movement was blocked, I will try to clear some obstacles." );

					Map map = m_Mobile.Map;

					if( map != null )
					{
						int x = m_Mobile.X, y = m_Mobile.Y;
						Movement.Movement.Offset( d, ref x, ref y );

						int destroyables = 0;

						IPooledEnumerable eable = map.GetItemsInRange( new Point3D( x, y, m_Mobile.Location.Z ), 1 );

						foreach( Item item in eable )
						{
							if( canOpenDoors && item is BaseDoor && (item.Z + item.ItemData.Height) > m_Mobile.Z && (m_Mobile.Z + 16) > item.Z )
							{
								if( item.X != x || item.Y != y )
									continue;

								BaseDoor door = (BaseDoor)item;

								if( !door.Locked || !door.UseLocks() )
									m_Obstacles.Enqueue( door );

								if( !canDestroyObstacles )
									break;
							}
							else if( canDestroyObstacles && item.Movable && item.ItemData.Impassable && (item.Z + item.ItemData.Height) > m_Mobile.Z && (m_Mobile.Z + 16) > item.Z )
							{
								if( !m_Mobile.InRange( item.GetWorldLocation(), 1 ) )
									continue;

								m_Obstacles.Enqueue( item );
								++destroyables;
							}
						}

						eable.Free();

						if( destroyables > 0 )
							Effects.PlaySound( new Point3D( x, y, m_Mobile.Z ), m_Mobile.Map, 0x3B3 );

						if( m_Obstacles.Count > 0 )
							blocked = false; // retry movement

						while( m_Obstacles.Count > 0 )
						{
							Item item = (Item)m_Obstacles.Dequeue();

							if( item is BaseDoor )
							{
								m_Mobile.DebugSay( "Little do they expect, I've learned how to open doors. Didn't they read the script??" );
								m_Mobile.DebugSay( "*twist*" );

								((BaseDoor)item).Use( m_Mobile );
							}
							else
							{
								m_Mobile.DebugSay( "Ugabooga. I'm so big and tough I can destroy it: {0}", item.GetType().Name );

								if( item is Container )
								{
									Container cont = (Container)item;

									for( int i = 0; i < cont.Items.Count; ++i )
									{
										Item check = cont.Items[i];

										if( check.Movable && check.ItemData.Impassable && (item.Z + check.ItemData.Height) > m_Mobile.Z )
											m_Obstacles.Enqueue( check );
									}

									cont.Destroy();
								}
								else
								{
									item.Delete();
								}
							}
						}

						if( !blocked )
							blocked = !m_Mobile.Move( d );
					}
				}

				if( blocked )
				{
					int offset = (Utility.RandomDouble() >= 0.6 ? 1 : -1);

					for( int i = 0; i < 2; ++i )
					{
						m_Mobile.TurnInternal( offset );

						if( m_Mobile.Move( m_Mobile.Direction ) )
						{
							MoveImpl.IgnoreMovableImpassables = false;
							return MoveResult.SuccessAutoTurn;
						}
					}

					MoveImpl.IgnoreMovableImpassables = false;
					return (wasPushing ? MoveResult.BadState : MoveResult.Blocked);
				}
				else
				{
					MoveImpl.IgnoreMovableImpassables = false;
					return MoveResult.Success;
				}
			}

			MoveImpl.IgnoreMovableImpassables = false;
			return MoveResult.Success;
		}

		public virtual void WalkRandomInHome( int iChanceToNotMove, int iChanceToDir, int iSteps )
		{
			if( m_Mobile.Deleted || m_Mobile.DisallowAllMoves )
				return;

			if( m_Mobile.Home == Point3D.Zero )
			{
				if( m_Mobile.Spawner is SpawnEntry )
				{
					Region region = ((SpawnEntry)m_Mobile.Spawner).Region;

					if( m_Mobile.Region.AcceptsSpawnsFrom( region ) )
					{
						m_Mobile.WalkRegion = region;
						WalkRandom( iChanceToNotMove, iChanceToDir, iSteps );
						m_Mobile.WalkRegion = null;
					}
					else
					{
						if( region.GoLocation != Point3D.Zero && Utility.Random( 10 ) > 5 )
						{
							DoMove( m_Mobile.GetDirectionTo( region.GoLocation ) );
						}
						else
						{
							WalkRandom( iChanceToNotMove, iChanceToDir, 1 );
						}
					}
				}
				else
				{
					WalkRandom( iChanceToNotMove, iChanceToDir, iSteps );
				}
			}
			else
			{
				for( int i=0; i<iSteps; i++ )
				{
					if( m_Mobile.RangeHome != 0 )
					{
						int iCurrDist = (int)m_Mobile.GetDistanceToSqrt( m_Mobile.Home );

						if( iCurrDist < m_Mobile.RangeHome * 2 / 3 )
						{
							WalkRandom( iChanceToNotMove, iChanceToDir, 1 );
						}
						else if( iCurrDist > m_Mobile.RangeHome )
						{
							DoMove( m_Mobile.GetDirectionTo( m_Mobile.Home ) );
						}
						else
						{
							if( Utility.Random( 10 ) > 5 )
							{
								DoMove( m_Mobile.GetDirectionTo( m_Mobile.Home ) );
							}
							else
							{
								WalkRandom( iChanceToNotMove, iChanceToDir, 1 );
							}
						}
					}
					else
					{
						if( m_Mobile.Location != m_Mobile.Home )
						{
							DoMove( m_Mobile.GetDirectionTo( m_Mobile.Home ) );
						}
					}
				}
			}
		}

		public virtual bool CheckFlee()
		{
			if( m_Mobile.CheckFlee() )
			{
				Mobile combatant = m_Mobile.Combatant;

				if( combatant == null )
				{
					WalkRandom( 1, 2, 1 );
				}
				else
				{
					Direction d = combatant.GetDirectionTo( m_Mobile );

					d = (Direction)((int)d + Utility.RandomMinMax( -1, +1 ));

					m_Mobile.Direction = d;
					m_Mobile.Move( d );
				}

				return true;
			}

			return false;
		}

		protected PathFollower m_Path;

		public virtual void OnTeleported()
		{
			if( m_Path != null )
			{
				m_Mobile.DebugSay( "Teleported; repathing" );
				m_Path.ForceRepath();
			}
		}

		public virtual bool MoveTo( Mobile m, bool run, int range )
		{
			if( m_Mobile.Deleted || m_Mobile.DisallowAllMoves || m == null || m.Deleted )
				return false;

			if( m_Mobile.InRange( m, range ) )
			{
				m_Path = null;
				return true;
			}

			if( m_Path != null && m_Path.Goal == m )
			{
				if( m_Path.Follow( run, 1 ) )
				{
					m_Path = null;
					return true;
				}
			}
			else if( !DoMove( m_Mobile.GetDirectionTo( m ), true ) )
			{
				m_Path = new PathFollower( m_Mobile, m );
				m_Path.Mover = new MoveMethod( DoMoveImpl );

				if( m_Path.Follow( run, 1 ) )
				{
					m_Path = null;
					return true;
				}
			}
			else
			{
				m_Path = null;
				return true;
			}

			return false;
		}

		/*
		 *  Walk at range distance from mobile
		 * 
		 *	iSteps : Number of steps
		 *	bRun   : Do we run
		 *	iWantDistMin : The minimum distance we want to be
		 *  iWantDistMax : The maximum distance we want to be
		 * 
		 */
		public virtual bool WalkMobileRange( Mobile m, int iSteps, bool bRun, int iWantDistMin, int iWantDistMax )
		{
			if( m_Mobile.Deleted || m_Mobile.DisallowAllMoves )
				return false;

			if( m != null )
			{
				for( int i=0; i<iSteps; i++ )
				{
					// Get the curent distance
					int iCurrDist = (int)m_Mobile.GetDistanceToSqrt( m );

					if( iCurrDist < iWantDistMin || iCurrDist > iWantDistMax )
					{
						bool needCloser = (iCurrDist > iWantDistMax);
						bool needFurther = !needCloser;

						if( needCloser && m_Path != null && m_Path.Goal == m && !m_Path.EscapeGoal )
						{
							if( m_Path.Follow( bRun, 1 ) )
								m_Path = null;
						}
						else if ( needFurther && m_Path != null && m_Path.Goal == m && m_Path.EscapeGoal )
						{
							if( m_Path.Follow( bRun, 1 ) )
								m_Path = null;
						}
						else
						{
							Direction dirTo;

							if( iCurrDist > iWantDistMax )
								dirTo = m_Mobile.GetDirectionTo( m );
							else
								dirTo = m.GetDirectionTo( m_Mobile );

							// Add the run flag
							if( bRun )
								dirTo = dirTo | Direction.Running;

							//if( !DoMove( dirTo, true ) )
							//{
								if ( needCloser )
								{
									m_Path = new PathFollower( m_Mobile, m );
									m_Path.Mover = new MoveMethod( DoMoveImpl );

									if( m_Path.Follow( bRun, 1 ) )
										m_Path = null;
								}
								else
								{
									m_Path = new PathFollower( m_Mobile, m, iWantDistMax );
									m_Path.Mover = new MoveMethod( DoMoveImpl );

									if( m_Path.Follow( bRun, 1 ) )
										m_Path = null;
								}
							//}
							//else
							//{
							//	m_Path = null;
							//}
						}
					}
					else
					{
						return true;
					}
				}

				// Get the curent distance
				int iNewDist = (int)m_Mobile.GetDistanceToSqrt( m );

				if( iNewDist >= iWantDistMin && iNewDist <= iWantDistMax )
					return true;
				else
					return false;
			}

			return false;
		}

		/*
		 * Here we check to acquire a target from our surronding
		 * 
		 *  iRange : The range
		 *  acqType : A type of acquire we want (closest, strongest, etc)
		 *  bPlayerOnly : Don't bother with other creatures or NPCs, want a player
		 *  bFacFriend : Check people in my faction
		 *  bFacFoe : Check people in other factions
		 * 
		 */
		public static bool IsLargerThanPrey( BaseCreature predator, BaseCreature prey )
		{
			if( predator is ILargePredator )
				return true;
			
			if( predator is IMediumPredator && !( prey is ILargePrey ) )
				return true;
			
			if( predator is ISmallPredator && prey is ISmallPrey )
				return true;
			
			return false;
		}

        public virtual bool AcquireFocusMob(int iRange, FightMode acqType, bool bPlayerOnly, bool bFacFriend, bool bFacFoe)
        {
            m_Mobile.DebugSay("Entered AcquireFocusMob");
            if (m_Mobile.Deleted)
                return false;

            XmlAwe awe = XmlAttach.FindAttachment(m_Mobile, typeof(XmlAwe)) as XmlAwe;

            if (awe != null)
                return false;

            if (m_Mobile is Soldier && m_Mobile.ControlOrder == OrderType.Stop)
                return false;
            
            if ((m_Mobile.Controlled) || (m_Mobile is Soldier && m_Mobile.ControlOrder == OrderType.Attack))
            {
                if (m_Mobile.Combatant != null && !m_Mobile.Combatant.Deleted)
                {
                    m_Mobile.ControlTarget = m_Mobile.Combatant;
                }

                if (m_Mobile.ControlTarget == null || m_Mobile.ControlTarget.Deleted || !m_Mobile.ControlTarget.Alive || m_Mobile.ControlTarget.IsDeadBondedPet || !m_Mobile.InRange(m_Mobile.ControlTarget, m_Mobile.RangePerception * 2))
                {
                    m_Mobile.FocusMob = null;
                    return false;
                }
                else
                {
                    m_Mobile.FocusMob = m_Mobile.ControlTarget;
                    return (m_Mobile.FocusMob != null);
                }
            }

            if (m_Mobile.ConstantFocus != null)
            {
                m_Mobile.FocusMob = m_Mobile.ConstantFocus;
                return true;
            }

            if (m_Mobile.MarkedForTermination)
            {
                m_Mobile.FocusMob = null;
                return false;
            }

            if (m_Mobile.Aggressors.Count == 0 && m_Mobile.Aggressed.Count == 0 && m_Mobile.FactionAllegiance == null && m_Mobile.EthicAllegiance == null)
            {
                if (acqType == FightMode.Aggressor)
                {
                    if (!m_Mobile.IsPredator && !(m_Mobile is IMagicalForestCreature) && !(m_Mobile is IRacialGuard) && !(m_Mobile is Soldier))
                    {
                        m_Mobile.FocusMob = null;
                        return false;
                    }
                }

                if (m_Mobile is IPeacefulPredator && m_Mobile.Hunger > 9)
                {
                    m_Mobile.FocusMob = null;
                    return false;
                }
            }

            if (m_Mobile.NextReacquireTime > DateTime.Now)
            {
                m_Mobile.FocusMob = null;
                return false;
            }

            if (acqType == FightMode.None)
            {
                m_Mobile.FocusMob = null;
                return false;
            }

            m_Mobile.NextReacquireTime = DateTime.Now + m_Mobile.ReacquireDelay;

            m_Mobile.DebugSay("Acquiring...");

            Map map = m_Mobile.Map;

            if (map != null)
            {
                Mobile newFocusMob = null;
                double val = double.MinValue;
                double theirVal;
                bool originalbPlayerOnly = bPlayerOnly;
                bool originalbFacFoe = bFacFoe;

                IPooledEnumerable eable = map.GetMobilesInRange(m_Mobile.Location, iRange);

                foreach (Mobile m in eable)
                {
                    if (m.Deleted || m.Blessed)
                        continue;

                    // Let's not target ourselves...
                    if (m == m_Mobile)
                        continue;

                    // Dead targets are invalid.
                    if (!m.Alive || m.IsDeadBondedPet)
                        continue;

                    if (m_Mobile.Summoned && m_Mobile.SummonMaster != null)
                    {
                        // If this is a summon, it can't target its controller.
                        if (m == m_Mobile.SummonMaster)
                            continue;

                        // It also must abide by harmful spell rules.
                        if (!Server.Spells.SpellHelper.ValidIndirectTarget(m_Mobile.SummonMaster, m))
                            continue;

                        // Animated creatures cannot attack players directly.
                        if (m is PlayerMobile && m_Mobile.IsAnimatedDead)
                            continue;

                    }

                    bool bValid = false;

                    // We will attack aggressors, or people we've aggressed priorly, regardless of our ethics.

                    for (int a = 0; !bValid && a < m_Mobile.Aggressors.Count; ++a)
                        bValid = (m_Mobile.Aggressors[a].Attacker == m);

                    for (int a = 0; !bValid && a < m_Mobile.Aggressed.Count; ++a)
                        bValid = (m_Mobile.Aggressed[a].Defender == m);

                    if (bValid)
                    { // we were aggressed, or fighting something -- we'll show them, player or not.
                        bPlayerOnly = false;
                        bFacFoe = false;
                    }
                    
                    else // this guy didn't do anything wrong to us, let's see if we want to ignore him
                    {
                        if (!m_Mobile.CanSee(m))
                            continue;

                        if (m_Mobile is BaseCreature && m is BaseCreature && ((BaseCreature)m_Mobile).Team != ((BaseCreature)m).Team)
                        {
                            bValid = true;
                            bPlayerOnly = false;
                            bFacFoe = false;                        
                        }
                        else if (AreAllies(m_Mobile, m))
                            continue;                        

                        // Prevent controlled mounts from being targeted
                        if (m is BaseMount && ((BaseMount)m).Controlled && ((BaseMount)m).ControlMaster != null)
                            continue;

                        if (m_Mobile is ICelestial && m is BaseCreature && !(m is IAbyssal))
                            continue;

                        if (m_Mobile is IMinotaur && m is IMinotaur)
                            continue;

                        if (acqType == FightMode.FindThreat) // look for things that might harm us
                        {

                            if (m.Body.Type == BodyType.Human) // flee from humans
                            {
                                bValid = true;
                                bPlayerOnly = false;
                                bFacFoe = false;
                            }
                            else if (m_Mobile is BaseCreature && ((BaseCreature)m_Mobile).IsPrey && m is BaseCreature && (((BaseCreature)m).IsPredator || m is PlayerMobile))
                            {
                                if (IsLargerThanPrey(((BaseCreature)m), ((BaseCreature)m_Mobile)) || m is PlayerMobile)
                                { // run away from predators if they're bigger than us, we're prey
                                    bValid = true;
                                    bPlayerOnly = false;
                                    bFacFoe = false;
                                }
                            }
                        }

                        if (m_Mobile is BaseCreature && ((BaseCreature)m_Mobile).IsPredator && m is PlayerMobile)
                        {
                            BaseCreature bc = m_Mobile as BaseCreature;
                            PlayerMobile pm = m as PlayerMobile;

                            if (bc is IJungleCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;

                            if (bc is IForestCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;

                            if (bc is IDesertCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;

                            if (bc is IPlainsCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;

                            if (bc is ICaveCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;

                            if (bc is ITundraCreature && pm.Backgrounds.BackgroundDictionary[BackgroundList.AnimalEmpathy].Level > 0)
                                continue;
                        }

                        if (m_Mobile is IUndead)
                        {
                            if (m is IUndead && !bValid)
                                continue;

                            if ((m_Mobile is ClericScorpion || m_Mobile is LesserClericScorpion || m_Mobile is GreaterClericScorpion) && 
                                ( (m is PlayerMobile && (m as PlayerMobile).Nation == Nation.Haluaroc) || 
                                (m is BaseCreature && (m as BaseCreature).Nation == Nation.Haluaroc) ) )
                                continue;

                            bValid = true;
                            bPlayerOnly = false;
                            bFacFoe = false;
                        }

                        if (m_Mobile is BaseCreature && ((BaseCreature)m_Mobile).IsPredator && m is BaseCreature && ((BaseCreature)m).IsPrey)
                        {
                            if ((m_Mobile.Hunger < 10 || m_Mobile is IAlwaysHungry) && IsLargerThanPrey(((BaseCreature)m_Mobile), ((BaseCreature)m)))
                            {
                                bValid = true;
                                bPlayerOnly = false;
                                bFacFoe = false;
                            }
                        }

                        if ((m_Mobile is IRacialGuard) || (m_Mobile is Soldier))
                        {
                            bPlayerOnly = false;
                            bFacFoe = false;
                        }
                    }

                    if (!bValid)
                    {
                        /*
                        #region Ethics & Faction checks
                        if ( !bValid )
                            bValid = ( m_Mobile.GetFactionAllegiance( m ) == BaseCreature.Allegiance.Enemy || m_Mobile.GetEthicAllegiance( m ) == BaseCreature.Allegiance.Enemy );
                        #endregion
                        */
                        if (!bValid && acqType == FightMode.Evil)
                        {
                            if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster != null)
                                bValid = (((BaseCreature)m).ControlMaster.Karma < 0);
                            else
                                bValid = (m.Karma < 0);
                        }
                        if (!bValid && m_Mobile is IMagicalForestCreature && m is PlayerMobile && !bValid)
                        {
                            PlayerMobile pm = m as PlayerMobile;

                            if (pm.EnemyOfNature)
                                bValid = true;
                        }
                        if (!bValid && m_Mobile is Soldier && (m_Mobile as Soldier).Government != null && !(m_Mobile as Soldier).Government.Deleted && Soldier.VisionCheck(m, m_Mobile, 16))
                        {
                            Soldier soldier = m_Mobile as Soldier;
                            soldier.WillArrest = false;
                            soldier.DebugSay("I am a soldier!");
                            XmlAttachment attachment = null;
                            attachment = XmlAttach.FindAttachment(m, typeof(XmlCriminal), soldier.Nation.ToString());

                            if (!bValid && m is BaseCreature && IsDangerous(m as BaseCreature, soldier))
                                bValid = true;
                            else if (!bValid && soldier.Government.MilitaryPolicies.AttackOnSight(m))
                            {
                                bValid = true;

                                if (soldier.Government.MilitaryPolicies.KillIndividualOnSight.Contains(m.Name) || soldier.Government.MilitaryPolicies.JailIndividualOnSight.Contains(m.Name))
                                {
                                    ReportInfo newReport = new ReportInfo(m.Name + " sighted at " + soldier.Location + ".");
                                    newReport.ReporterName = soldier.Name;
                                    soldier.ReportTimer = new ReportTimer(newReport, soldier);
                                    soldier.ReportTimer.Start();
                                }
                                else if ( m is PlayerMobile && (soldier.Government.MilitaryPolicies.KillNationOnSight.Contains((m as PlayerMobile).Nation) || soldier.Government.MilitaryPolicies.JailNationOnSight.Contains((m as PlayerMobile).Nation)) )
                                {
                                    ReportInfo newReport = new ReportInfo( (m as PlayerMobile).Nation.ToString() + " sighted at " + soldier.Location + ".");
                                    newReport.ReporterName = soldier.Name;
                                    soldier.ReportTimer = new ReportTimer(newReport, soldier);
                                    soldier.ReportTimer.Start();
                                }
                                else if (m is BaseCreature && (soldier.Government.MilitaryPolicies.KillNationOnSight.Contains((m as BaseCreature).Nation) || soldier.Government.MilitaryPolicies.JailNationOnSight.Contains((m as BaseCreature).Nation)) )
                                {
                                    ReportInfo newReport = new ReportInfo((m as BaseCreature).Nation.ToString() + " sighted at " + soldier.Location + ".");
                                    newReport.ReporterName = soldier.Name;
                                    soldier.ReportTimer = new ReportTimer(newReport, soldier);
                                    soldier.ReportTimer.Start();
                                }
                            }
                            else if (!bValid && (attachment == null) && (m.AccessLevel < AccessLevel.Counselor))
                            {
                                if (m is PlayerMobile && Soldier.ThiefCheck((PlayerMobile)m, soldier))
                                {
                                    Soldier.RecognizeCrime(m, soldier);
                                    bValid = true;
                                }
                                else if (Soldier.AssaultCheck(m, soldier))
                                {
                                    Soldier.RecognizeCrime(m, soldier);
                                    bValid = true;
                                }
                            }
                            else if (!bValid && attachment != null && soldier.Combatant == null && m.AccessLevel < AccessLevel.Counselor)
                            {
                                bValid = true;
                            }

                            if (bValid)
                            {
                                string speech = Soldier.RandomAttackSpeech(soldier);
                                if (speech != null)
                                    soldier.Say(speech);
                                m.RevealingAction();
                            }
                        }

                        if (!bValid && m_Mobile is BaseCreature && ((BaseCreature)m_Mobile).IsPredator)
                        {
                            if (m_Mobile.Hunger < 10 || m_Mobile is IAlwaysHungry)
                            {
                                if (m is PlayerMobile && (m_Mobile is ILargePredator || m_Mobile is IMediumPredator || m_Mobile is ISerpent))
                                    bValid = true;
                            }
                        }

                        if (!bValid)
                        {
                            if (acqType != FightMode.Aggressor && acqType != FightMode.Evil && acqType != FightMode.FindThreat)
                            {
                                bValid = true; // other modes will attack anyway
                                bPlayerOnly = false;
                                bFacFoe = false;
                            }
                            else
                                continue;
                        }
                    }

                    // Does it have to be a player?
                    if (bPlayerOnly && !m.Player)
                        continue;

                    // If we only want faction friends, make sure it's one.
                    if (bFacFriend && !m_Mobile.IsFriend(m))
                        continue;

                    // Same goes for faction enemies.
                    if (bFacFoe && !m_Mobile.IsEnemy(m))
                        continue;

                    bPlayerOnly = originalbPlayerOnly;

                    // If it's an enemy factioned mobile, make sure we can be harmful to it.
                    if (bFacFoe && !bFacFriend && !m_Mobile.CanBeHarmful(m, false))
                        continue;

                    theirVal = m_Mobile.GetValueFrom(m, acqType, bPlayerOnly);

                    if (theirVal > val && m_Mobile.InLOS(m))
                    {
                        newFocusMob = m;
                        val = theirVal;
                    }

                    bFacFoe = originalbFacFoe;
                }
                eable.Free();
                m_Mobile.FocusMob = newFocusMob;
            }
            return (m_Mobile.FocusMob != null);
        }
		
		private bool Disguised( PlayerMobile m, BaseKhaerosMobile guard )
		{
			if( guard is IHaluaroc && m.RPTitle.ToLower().Contains( "Haluaroc" ) )
				return true;
			
			if( guard is IMhordul && m.RPTitle.ToLower().Contains( "mhordul" ) )
				return true;
			
			if( guard is ITirebladd && m.RPTitle.ToLower().Contains( "Tirebladd" ) )
				return true;
			
			if( guard is INorthern && m.RPTitle.ToLower().Contains( "north" ) )
				return true;
			
			if( guard is IWestern && m.RPTitle.ToLower().Contains( "west" ) )
				return true;
			
			if( guard is ISouthern && m.RPTitle.ToLower().Contains( "south" ) )
				return true;
			
			return false;
		}
		
		/*public static bool ShouldAttack( Mobile attacker, Mobile defender )
		{
			
			return false;
		}*/
		
		public static bool IsDangerous( BaseCreature bc, BaseKhaerosMobile guard )
		{
			if ( bc.ControlMaster != null || bc.Controlled )
				return false;
				
			if( ( bc is IMediumPredator || bc is ILargePredator || bc is ISerpent ) && !bc.Controlled )
				return true;
			
			if( bc is IUndead && !( guard is Soldier && guard.Nation == Nation.Haluaroc && ( bc is ClericScorpion || bc is LesserClericScorpion || bc is GreaterClericScorpion ) ) )
				return true;
				
			if( bc.Team != guard.Team )
				return true;
			
			if( bc is DisplacerBeast || 
                bc is IBrigand || 
                bc is GelatinousBlobSpawn || 
                bc is JellyOozeSpawn || 
                bc is BlackPuddingSpawn || 
                bc is IGoblin )
				return true;

            if (bc.FightMode != FightMode.Aggressor && bc.FightMode != FightMode.FindThreat && bc.FightMode != FightMode.None)
                return true;
				
			if( bc is IInsularii )
				return true;
			
			return false;
		}
		
		public static bool PlayerAndCreatureAllies( PlayerMobile player, BaseCreature creature )
		{
            if (creature is DesertScorpion && player.Nation == Nation.Haluaroc)
				return true;
			
			if( creature.ControlMaster != null && creature.ControlMaster is PlayerMobile && ((PlayerMobile)creature.ControlMaster).IsAllyOf(player) )
				return true;

            if (creature.CreatureGroup != CreatureGroup.None)
            {
                XmlAttachment playeratt = XmlAttach.FindAttachment(player, typeof(XmlAlly), creature.CreatureGroup.ToString());
                if (playeratt != null)
                    return true;

                foreach (Item i in player.Items)
                {
                    XmlAttachment attachment = XmlAttach.FindAttachment(i, typeof(XmlAlly), creature.CreatureGroup.ToString());
                    if (attachment != null)
                        return true;
                }
            }
			
			switch( creature.CreatureGroup )
			{
				case CreatureGroup.None: break;

				case CreatureGroup.Brigand: 
				{
                    if (player.Friendship.Brigand > 0)
                        return true;

					break;
				}
					
				case CreatureGroup.Goblin: 
				{
					if( player.Friendship.Goblin > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Beastman: 
				{
					if( player.Friendship.Beastman > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Ogre: 
				{
					if( player.Friendship.Ogre > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Minotaur: 
				{
					if( player.Friendship.Minotaur > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Undead: 
				{
					if( player.Friendship.Undead > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.YuanTi: 
				{
					if( player.Friendship.YuanTi > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Giant: 
				{
					if( player.Friendship.Giant > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Draconic: 
				{
					if( player.Friendship.Draconic > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Canine: 
				{
					if( player.Friendship.Canine > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Serpent: 
				{
					if( player.Friendship.Serpent > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Rodent: 
				{
					if( player.Friendship.Rodent > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Goatman: 
				{
					if( player.Friendship.Goatman > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Troll: 
				{
					if( player.Friendship.Troll > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Troglin: 
				{
					if( player.Friendship.Troglin > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Spider: 
				{
					if( player.Friendship.Spider > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Drider: 
				{
					if( player.Friendship.Drider > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Formian: 
				{
					if( player.Friendship.Formian > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Kobold: 
				{
					if( player.Friendship.Kobold > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Beholder: 
				{
					if( player.Friendship.Beholder > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Feline: 
				{
					if( player.Friendship.Feline > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Bear: 
				{
					if( player.Friendship.Bear > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Abyssal: 
				{
					if( player.Friendship.Abyssal > 0 )
						return true;
					
					break;
				}
				
				case CreatureGroup.Celestial: 
				{
					if( player.Friendship.Abyssal == 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Elemental: 
				{
					if( player.Friendship.Elemental > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.Reptile: 
				{
					if( player.Friendship.Reptile > 0 )
						return true;
					
					break;
				}
					
				case CreatureGroup.GiantBug: 
				{
					if( player.Friendship.GiantBug > 0 )
						return true;
					
					break;
				}
				
				case CreatureGroup.Brotherhood: 
				{
					if( player.Friendship.Brotherhood > 0 )
						return true;
					
					break;
				}
				
				case CreatureGroup.Society: 
				{
					if( player.Friendship.Society > 0 )
						return true;
					
					break;
				}
				
				case CreatureGroup.Insularii: 
				{
					if( player.Friendship.Insularii > 0 )
						return true;
					
					break;
				}
			}
			
			return false;
		}
		
		public static bool AreAllies( Mobile one, Mobile two )
		{
			if( one == two )
				return true;

            if (one is Soldier)
            {
                if ((one as Soldier).Government != null && !(one as Soldier).Government.Deleted)
                {
                    if (two is Soldier)
                    {
                        if ((two as Soldier).Government != null && !(two as Soldier).Government.Deleted)
                        {
                            if ((one as Soldier).Government == (two as Soldier).Government)
                                return true;

                            if ((one as Soldier).Government.AlliedGuilds.Contains((two as Soldier).Government))
                                return true;
                        }
                    }

                    if (two is PlayerMobile)
                    {
                        if ((one as Soldier).Government.Members.Contains(two))
                            if ((two as PlayerMobile).CustomGuilds[(one as Soldier).Government].RankInfo.Rank > 1)
                                return true;

                        foreach (CustomGuildStone g in (one as Soldier).Government.AlliedGuilds)
                        {
                            if (g.Members.Contains(two))
                                if (CustomGuildStone.IsGuildOfficer(two as PlayerMobile, g))
                                    return true;
                        }
                    }
                }
            }

            if (two is Soldier)
            {
                if ((two as Soldier).Government != null && !(two as Soldier).Government.Deleted)
                {
                    if (one is Soldier)
                    {
                        if ((one as Soldier).Government != null && !(one as Soldier).Government.Deleted)
                        {
                            if ((two as Soldier).Government == (one as Soldier).Government)
                                return true;

                            if ((two as Soldier).Government.AlliedGuilds.Contains((one as Soldier).Government))
                                return true;
                        }
                    }

                    if (one is PlayerMobile)
                    {
                        if ((two as Soldier).Government.Members.Contains(one))
                            if ((one as PlayerMobile).CustomGuilds[(two as Soldier).Government].RankInfo.Rank > 1)
                                return true;

                        foreach (CustomGuildStone g in (two as Soldier).Government.AlliedGuilds)
                        {
                            if (g.Members.Contains(one))
                                if (CustomGuildStone.IsGuildOfficer(one as PlayerMobile, g))
                                    return true;
                        }
                    }
                }
            }

            if (one is PlayerMobile)
                if (two is Soldier && (two as Soldier).Government != null && !(two as Soldier).Government.Deleted)
                    if ((two as Soldier).Government.Members.Contains(one))
                        return true;

            if (two is PlayerMobile)
                if (one is Soldier && (one as Soldier).Government != null && !(one as Soldier).Government.Deleted)
                    if ((one as Soldier).Government.Members.Contains(two))
                        return true;
			
			if( one is BaseCreature && two is PlayerMobile )
				return (PlayerAndCreatureAllies( (PlayerMobile)two, (BaseCreature)one ) && !HasAggressed( two, one ));
			
			if( one is PlayerMobile )
				return ((PlayerMobile)one).AllyList.Contains( two );

			if( one is BaseCreature && two is BaseCreature )
				return ( CreatureAllies((BaseCreature)one, (BaseCreature)two) && !HasAggressed(two, one) );
		
			return false;
		}
		
		public static bool CreatureAllies( BaseCreature one, BaseCreature two )
		{
			return ( ShareCreatureGroup(one, two) || ShareType(one, two) || ShareMaster(one, two) );
		}
		
		public static bool ShareCreatureGroup( BaseCreature one, BaseCreature two )
		{
			return ( one.CreatureGroup == two.CreatureGroup );
		}
		
		public static bool ShareType( BaseCreature one, BaseCreature two )
		{
			return ( one.GetType().ToString() == two.GetType().ToString() && !one.GetType().ToString().ToLower().Contains("generic") );
		}
		
		public static bool ShareMaster( BaseCreature one, BaseCreature two )
		{
			return ( one.ControlMaster != null && one.ControlMaster == two.ControlMaster );
		}
		
		public static bool HasAggressed( Mobile aggressor, Mobile aggressed )
		{
			if( aggressed.Aggressors.Count > 0 )
			{
				for( int i = 0; i < aggressed.Aggressors.Count; ++i )
				{
					AggressorInfo info = aggressed.Aggressors[i];
					Mobile attacker = info.Attacker;

					if( attacker != null && !attacker.Deleted && attacker == aggressor )
						return true;
				}
			}
			
			return false;
		}

		public virtual void DetectHidden()
		{
			if( m_Mobile.Deleted || m_Mobile.Map == null )
				return;

			m_Mobile.DebugSay( "Checking for hidden players" );

			double srcSkill = m_Mobile.Skills[SkillName.DetectHidden].Value;

			if( srcSkill <= 0 )
				return;

			foreach( Mobile trg in m_Mobile.GetMobilesInRange( m_Mobile.RangePerception ) )
			{
				if( trg != m_Mobile && trg.Player && trg.Alive && trg.Hidden && trg.AccessLevel == AccessLevel.Player && m_Mobile.InLOS( trg ) )
				{
					m_Mobile.DebugSay( "Trying to detect {0}", trg.Name );

					double trgHiding = trg.Skills[SkillName.Hiding].Value / 2.9;
					double trgStealth = trg.Skills[SkillName.Stealth].Value / 1.8;

					double chance = srcSkill / 1.2 - Math.Min( trgHiding, trgStealth );

					if( chance < srcSkill / 10 )
						chance = srcSkill / 10;

					chance /= 100;

					if( chance > Utility.RandomDouble() )
					{
						trg.RevealingAction();
						trg.SendLocalizedMessage( 500814 ); // You have been revealed!
					}
				}
			}
		}

		public virtual void Deactivate()
		{
			if( m_Mobile.PlayerRangeSensitive )
			{
				m_Timer.Stop();

				SpawnEntry se = m_Mobile.Spawner as SpawnEntry;

				if( se != null && se.ReturnOnDeactivate && !m_Mobile.Controlled )
				{
					if( se.Home == Point3D.Zero )
					{
						if( !m_Mobile.Region.AcceptsSpawnsFrom( se.Region ) )
						{
							Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ReturnToHome ) );
						}
					}
					else if( !m_Mobile.InRange( se.Home, se.Range ) )
					{
						Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ReturnToHome ) );
					}
				}
			}
		}

		private void ReturnToHome()
		{
			SpawnEntry se = m_Mobile.Spawner as SpawnEntry;

			if( se != null )
			{
				Point3D loc = se.RandomSpawnLocation( 16, !m_Mobile.CantWalk, m_Mobile.CanSwim );

				if( loc != Point3D.Zero )
				{
					m_Mobile.MoveToWorld( loc, se.Region.Map );
					return;
				}
			}
		}

		public virtual void Activate()
		{
			if( !m_Timer.Running )
			{
				m_Timer.Delay = TimeSpan.Zero;
				m_Timer.Start();
			}
		}

		/*
		 *  The mobile changed it speed, we must ajust the timer
		 */
		public virtual void OnCurrentSpeedChanged()
		{
			m_Timer.Stop();
			m_Timer.Delay = TimeSpan.FromSeconds( Utility.RandomDouble() );
			m_Timer.Interval = TimeSpan.FromSeconds( Math.Max( 0.0, m_Mobile.CurrentSpeed ) );
			m_Timer.Start();
		}

		/*
		 *  The Timer object
		 */
		private class AITimer : Timer
		{
			private BaseAI m_Owner;
			private DateTime m_NextDetectHidden;
			private bool m_bDetectHidden;

			public AITimer( BaseAI owner )
				: base( TimeSpan.FromSeconds( Utility.RandomDouble() ), TimeSpan.FromSeconds( Math.Max( 0.0, owner.m_Mobile.CurrentSpeed ) ) )
			{
				m_Owner = owner;

				m_bDetectHidden = false;
				m_NextDetectHidden = DateTime.Now;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
                m_Owner.m_Mobile.DebugSay("OnTick()");

				if( m_Owner.m_Mobile.Deleted )
				{
					Stop();
					return;
				}
				else if( m_Owner.m_Mobile.Map == null || m_Owner.m_Mobile.Map == Map.Internal )
				{
					return;
				}
				else if( m_Owner.m_Mobile.PlayerRangeSensitive )//have to check this in the timer....
				{
					Sector sect = m_Owner.m_Mobile.Map.GetSector( m_Owner.m_Mobile );
					if( !sect.Active )
					{
						m_Owner.Deactivate();
						return;
					}
				}

				m_Owner.m_Mobile.OnThink();

				if( m_Owner.m_Mobile.Deleted )
				{
					Stop();
					return;
				}
				else if( m_Owner.m_Mobile.Map == null || m_Owner.m_Mobile.Map == Map.Internal )
				{
					return;
				}

				if( m_Owner.m_Mobile.BardPacified )
				{
					m_Owner.DoBardPacified();
				}
				else if( m_Owner.m_Mobile.BardProvoked )
				{
					m_Owner.DoBardProvoked();
				}
				else
				{
					if(( !m_Owner.m_Mobile.Controlled ) && (!(m_Owner.m_Mobile is Soldier)))
					{
						if( !m_Owner.Think() )
						{
							Stop();
							return;
						}
					}
					else
					{
                        m_Owner.m_Mobile.DebugSay("Action: " + m_Owner.Action.ToString());
                        m_Owner.m_Mobile.DebugSay("ControlOrder: " + m_Owner.m_Mobile.ControlOrder.ToString());

						if( !m_Owner.Obey() )
						{
                            m_Owner.m_Mobile.DebugSay("Obey() is false");
							Stop();
							return;
						}
					}
				}

				if( m_bDetectHidden && DateTime.Now > m_NextDetectHidden )
				{
					m_Owner.DetectHidden();

					// Not exactly OSI style, approximation.
					int delay = (15000 / m_Owner.m_Mobile.Int);

					if( delay > 60 )
						delay = 60;

					int min = delay * (9 / 10); // 13s at 1000 int, 33s at 400 int, 54s at <250 int
					int max = delay * (10 / 9); // 16s at 1000 int, 41s at 400 int, 66s at <250 int

					m_NextDetectHidden = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( min, max ) );
				}
				else
				{
					m_bDetectHidden = m_Owner.m_Mobile.Skills[SkillName.DetectHidden].Value > 0;
				}
			}
		}
	}
}
