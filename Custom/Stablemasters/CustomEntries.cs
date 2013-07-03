using System;
using Server.Mobiles;
using Server.Items;
using Server.Multis;

namespace Server.ContextMenus
{
    public class HoldEntry : ContextMenuEntry
    {
        private BaseCreature m_Mobile;
        private Mobile m_From;

        public HoldEntry( BaseCreature m, Mobile from )
            : base( 6173 )
        {
            m_Mobile = m;
            m_From = from;
        }

        public override void OnClick()
        {
            if( m_From == null || m_Mobile == null || m_From.Deleted || m_Mobile.Deleted )
                return;

            if( !( m_Mobile.Controlled && m_Mobile.ControlMaster == m_From ) )
                m_From.SendMessage( "You do not control that creature." );

            if (!m_Mobile.Alive || m_Mobile.IsDeadBondedPet || m_Mobile.IsDeadPet)
            {
                m_From.SendMessage("That creature is dead!");
                return;
            }

            if( m_Mobile is ITinyPet && m_From.Backpack != null && !m_From.Backpack.Deleted )
            {
                if( !m_From.InRange( m_Mobile.Location, 2 ) || !m_From.CanSee( m_Mobile ) || !m_From.InLOS( m_Mobile ) )
                    m_From.SendMessage( "You are too far away from the creature." );

                else
                {
                    m_From.SendMessage( "You grab your pet and place it in your backpack." );
                    m_From.Backpack.DropItem( new MiniaturePet( m_Mobile.MiniatureID, m_Mobile, m_From ) );
                }
            }
        }
    }

    public class HouseEntry : ContextMenuEntry
    {
        private BaseCreature m_Mobile;
        private Mobile m_From;

        public HouseEntry( BaseCreature m, Mobile from )
            : base( 6170 )
        {
            m_Mobile = m;
            m_From = from;
        }

        public override void OnClick()
        {
            if( m_From == null || m_Mobile == null || m_From.Deleted || m_Mobile.Deleted )
                return;

            if( !m_From.InRange( m_Mobile, 2 ) || !m_From.CanSee( m_Mobile ) || !m_From.InLOS( m_Mobile ) )
            {
                m_From.SendMessage( "You are too far away from the creature." );
                return;
            }

            BaseHouse house = BaseHouse.FindHouseAt( m_Mobile );

            if( house == null || !house.IsCoOwner( m_From ) )
                m_From.SendMessage( "Your pet needs to be inside a house that you own or co-own in order to use this command." );

            else if( m_Mobile.Controlled && m_Mobile.ControlMaster == m_From )
            {
                m_From.SendMessage( "You set your pet inside your house and tell it to stay there." );
                m_Mobile.SetControlMaster( null );
                m_Mobile.ControlOrder = OrderType.Stay;
                m_Mobile.ControlTarget = null;
                m_Mobile.Loyalty = BaseCreature.MaxLoyalty;
                m_Mobile.Blessed = true;
                m_Mobile.Frozen = true;
                m_Mobile.Tamable = false;
            }

            else if( m_Mobile.Blessed && m_Mobile.Frozen && (m_From.Followers + m_Mobile.ControlSlots) <= m_From.FollowersMax )
            {
                m_From.SendMessage( "You retrieve your pet and tell it to follow you." );
                m_Mobile.SetControlMaster( m_From );
                m_Mobile.ControlOrder = OrderType.Follow;
                m_Mobile.ControlTarget = m_From;
                m_Mobile.Blessed = false;
                m_Mobile.Frozen = false;
                m_Mobile.Tamable = true;

                if (m_Mobile is BaseBreedableCreature)
                {
                    BaseBreedableCreature pet = m_Mobile as BaseBreedableCreature;
                    pet.UpdateSpeeds();
                }
            }

            else if( m_Mobile.Blessed && m_Mobile.Frozen )
                m_From.SendMessage( "You lack enough follower slots to retrieve that pet." );

            else
                m_From.SendMessage( "You do not control that creature." );
        }
    }
}
