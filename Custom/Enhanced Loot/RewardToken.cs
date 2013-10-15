using System;
using Server.Mobiles;

namespace Server.Items
{
	public class RewardToken : Item
	{
        [Constructable]
        public RewardToken() : this( 1 )
        {
        }

        [Constructable]
        public RewardToken( int amount ) : base( 7154 )
        {
            Amount = amount;
            Hue = 2999;
            Weight = 0.0;
            Name = "Rare Materials";
            Stackable = true;
        }

        public RewardToken( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}
