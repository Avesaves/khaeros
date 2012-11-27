using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class MiniaturePet : Item
    {
        private BaseCreature m_Creature;
        public BaseCreature Creature { get { return m_Creature; } set { m_Creature = value; } }

        [Constructable]
        public MiniaturePet( int itemid, BaseCreature creature, Mobile owner )
            : base( itemid )
        {
            m_Creature = creature;
            creature.IsStabled = true;
            creature.ControlTarget = null;
            creature.ControlOrder = OrderType.Stay;
            creature.StabledOwner = owner;
            creature.Internalize();
            creature.SetControlMaster( null );
            creature.SummonMaster = null;
            creature.Loyalty = BaseCreature.MaxLoyalty;
            Weight = 10;
            Name = creature.Name;
            Movable = false;
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( Creature != null && from != null && !Creature.Deleted )
            {
                if( (from.Followers + Creature.ControlSlots) <= from.FollowersMax )
                {
                    Creature.SetControlMaster( from );
                    Creature.MoveToWorld( from.Location, from.Map );
                    Creature.ControlTarget = from;
                    Creature.ControlOrder = OrderType.Follow;
                    Creature.IsStabled = false;
                    Delete();
                }

                else
                    from.SendMessage( "You lack enough follower slots to retrieve that pet." );
            }
        }

        public MiniaturePet( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
            
			writer.Write( (int) 0 );
            writer.Write( (BaseCreature)m_Creature );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            m_Creature = (BaseCreature)reader.ReadMobile();
		}
    }
}
