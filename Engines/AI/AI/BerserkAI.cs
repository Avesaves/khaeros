using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    public class BerserkAI : MeleeAI
    {
        public BerserkAI( BaseCreature m )
            : base( m )
        {
        }

        public override bool DoActionCombat()
        {
            if( CombatSystemAttachment.GetCSA( m_Mobile ).DefenseTimer != null )
                return true;

            Mobile combatant = m_Mobile.Combatant;

            if( combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet )
            {
                m_Mobile.DebugSay( "My combatant is gone, so my guard is up" );

                Action = ActionType.Guard;

                return true;
            }

            if( !m_Mobile.InRange( combatant, Math.Max( m_Mobile.RangeFight, m_Mobile.Weapon.MaxRange ) ) )
            {
                // They are somewhat far away, can we find something else?

                if( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
                {
                    m_Mobile.Combatant = m_Mobile.FocusMob;
                    m_Mobile.FocusMob = null;
                }
                else if( !m_Mobile.InRange( combatant, m_Mobile.RangePerception * 3 ) )
                {
                    m_Mobile.Combatant = null;
                }

                combatant = m_Mobile.Combatant;

                if( combatant == null )
                {
                    m_Mobile.DebugSay( "My combatant has fled, so I am on guard" );
                    Action = ActionType.Guard;

                    return true;
                }
            }

            return base.DoActionCombat();
        }
    }
}
