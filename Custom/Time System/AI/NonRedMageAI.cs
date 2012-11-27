//#define UseNonRedMageAI

using System;
using Server.Mobiles;
using Server.TimeSystem;

namespace Server.Mobiles
{
	public class NonRedMageAI : MageAI
	{
		public NonRedMageAI( BaseCreature m ) : base( m )
		{
		}

        public override bool AcquireFocusMob(int iRange, FightMode acqType, bool bPlayerOnly, bool bFacFriend, bool bFacFoe)
        {
            if (m_Mobile.Deleted)
                return false;

            if (m_Mobile.BardProvoked)
            {
                if (m_Mobile.BardTarget == null || m_Mobile.BardTarget.Deleted)
                {
                    m_Mobile.FocusMob = null;
                    return false;
                }
                else
                {
                    m_Mobile.FocusMob = m_Mobile.BardTarget;
                    return (m_Mobile.FocusMob != null);
                }
            }
            else if (m_Mobile.Controlled)
            {
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
                m_Mobile.DebugSay("Acquired my constant focus");
                m_Mobile.FocusMob = m_Mobile.ConstantFocus;
                return true;
            }

            if (acqType == FightMode.None)
            {
                m_Mobile.FocusMob = null;
                return false;
            }

            if (acqType == FightMode.Aggressor && m_Mobile.Aggressors.Count == 0 && m_Mobile.Aggressed.Count == 0 && m_Mobile.FactionAllegiance == null && m_Mobile.EthicAllegiance == null)
            {
                m_Mobile.FocusMob = null;
                return false;
            }

            if (m_Mobile.NextReacquireTime > DateTime.Now)
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

                    // Staff members cannot be targeted.
                    if (m.AccessLevel > AccessLevel.Player)
                        continue;

                    // Does it have to be a player?
                    if (bPlayerOnly && !m.Player)
                        continue;

                    // Can't acquire a target we can't see.
                    if (!m_Mobile.CanSee(m))
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

                    // If we only want faction friends, make sure it's one.
                    if (bFacFriend && !m_Mobile.IsFriend(m))
                        continue;

                    // Same goes for faction enemies.
                    if (bFacFoe && !m_Mobile.IsEnemy(m))
                        continue;

#if (UseNonRedMageAI)

                    MobileObject mo = Support.GetMobileObject(m);

                    if (acqType == FightMode.NonRed && mo != null & !mo.CanBeAttackedByEvilSpirit)
                        continue;

#endif

                    if (acqType == FightMode.Aggressor || acqType == FightMode.Evil)
                    {
                        // Only acquire this mobile if it attacked us, or if it's evil.
                        bool bValid = false;

                        for (int a = 0; !bValid && a < m_Mobile.Aggressors.Count; ++a)
                            bValid = (m_Mobile.Aggressors[a].Attacker == m);

                        for (int a = 0; !bValid && a < m_Mobile.Aggressed.Count; ++a)
                            bValid = (m_Mobile.Aggressed[a].Defender == m);

                        #region Ethics & Faction checks
                        if (!bValid)
                            bValid = (m_Mobile.GetFactionAllegiance(m) == BaseCreature.Allegiance.Enemy || m_Mobile.GetEthicAllegiance(m) == BaseCreature.Allegiance.Enemy);
                        #endregion

                        if (acqType == FightMode.Evil && !bValid)
                        {
                            if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster != null)
                                bValid = (((BaseCreature)m).ControlMaster.Karma < 0);
                            else
                                bValid = (m.Karma < 0);
                        }

                        if (!bValid)
                            continue;
                    }

                    // If it's an enemy factioned mobile, make sure we can be harmful to it.
                    if (bFacFoe && !bFacFriend && !m_Mobile.CanBeHarmful(m, false))
                        continue;

                    theirVal = m_Mobile.GetValueFrom(m, acqType, bPlayerOnly);

                    if (theirVal > val && m_Mobile.InLOS(m))
                    {
                        newFocusMob = m;
                        val = theirVal;
                    }
                }

                eable.Free();

                m_Mobile.FocusMob = newFocusMob;
            }

            return (m_Mobile.FocusMob != null);
        }
    }
}
