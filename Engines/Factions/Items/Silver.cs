using System;
using Server;

namespace Server.Items
{
    public class Tin : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        [Constructable]
        public Tin()
            : this( 1000 )
        {
        }

        [Constructable]
        public Tin(int amountFrom, int amountTo)
            : this( Utility.RandomMinMax( amountFrom, amountTo ) )
        {
        }

        [Constructable]
        public Tin(int amount)
            : base( 0xEF0 )
        {
            Stackable = true;
            Amount = amount;
            Name = "Tin Coin";
            Hue = 2401;
        }

        public Tin(Serial serial)
            : base( serial )
        {
        }

        public override int GetDropSound()
        {
            if( Amount <= 1 )
                return 0x2E4;
            else if( Amount <= 5 )
                return 0x2E5;
            else
                return 0x2E6;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

	public class Silver : Item
	{
		public override double DefaultWeight
		{
			get { return 0.02; }
		}

		[Constructable]
		public Silver() : this( 1 )
		{
		}

		[Constructable]
		public Silver( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Silver( int amount ) : base( 0xEF0 )
		{
			Stackable = true;
			Amount = amount;
		}

		public Silver( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
				return 0x2E4;
			else if ( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

/*    public class Copper : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        [Constructable]
        public Copper()
            : this( 1 )
        {
        }

        [Constructable]
        public Copper(int amountFrom, int amountTo)
            : this( Utility.RandomMinMax( amountFrom, amountTo ) )
        {
        }

        [Constructable]
        public Copper(int amount)
            : base( 0xEF0 )
        {
            Stackable = true;
            Amount = amount;
            Name = "Copper Coin";
            Hue = 1453;
        }

        public Copper(Serial serial)
            : base( serial )
        {
        }

        public override int GetDropSound()
        {
            if( Amount <= 1 )
                return 0x2E4;
            else if( Amount <= 5 )
                return 0x2E5;
            else
                return 0x2E6;
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
        }
    }
    */
    public class ForgedGold : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        private DateTime m_TimeOfCreation;

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        private Timer m_DecayTimer;
        private DateTime m_DecayTime;

        private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes( 30.0 );

        public void BeginDecay( TimeSpan delay )
        {
            if( m_DecayTimer != null )
                m_DecayTimer.Stop();

            m_DecayTime = DateTime.Now + delay;

            m_DecayTimer = new InternalTimer( this, delay );
            m_DecayTimer.Start();
        }

        public override void OnAfterDelete()
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTimer = null;
		}

        private class InternalTimer : Timer
        {
            private ForgedGold m_ForgedGold;

            public InternalTimer(ForgedGold c, TimeSpan delay)
                : base( delay )
            {
                m_ForgedGold = c;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                m_ForgedGold.ItemID = 0xEF0;
                m_ForgedGold.Name = "Forged Gold Coin";
                m_ForgedGold.Hue = 2401;
            }
        }

        [Constructable]
        public ForgedGold()
            : this( 1 )
        {
        }

        [Constructable]
        public ForgedGold( int amountFrom, int amountTo )
            : this( Utility.RandomMinMax( amountFrom, amountTo ) )
        {
        }

        [Constructable]
        public ForgedGold( int amount )
            : base( 0xEED )
        {
            Stackable = true;
            Amount = amount;
            Name = "Gold Coin";
            m_TimeOfCreation = DateTime.Now;
            BeginDecay( m_DefaultDecayTime );
        }

        public ForgedGold( Serial serial )
            : base( serial )
        {
        }

        public override int GetDropSound()
        {
            if( Amount <= 1 )
                return 0x2E4;
            else if( Amount <= 5 )
                return 0x2E5;
            else
                return 0x2E6;
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version

            writer.WriteDeltaTime( m_TimeOfCreation );

            writer.Write( m_DecayTimer != null );

            if( m_DecayTimer != null )
                writer.WriteDeltaTime( m_DecayTime );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_TimeOfCreation = reader.ReadDeltaTime();

            if( reader.ReadBool() )
                BeginDecay( reader.ReadDeltaTime() - DateTime.Now );
        }
    }

    public class ForgedSilver : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        private DateTime m_TimeOfCreation;

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        private Timer m_DecayTimer;
        private DateTime m_DecayTime;

        private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes( 30.0 );

        public void BeginDecay(TimeSpan delay)
        {
            if( m_DecayTimer != null )
                m_DecayTimer.Stop();

            m_DecayTime = DateTime.Now + delay;

            m_DecayTimer = new InternalTimer( this, delay );
            m_DecayTimer.Start();
        }

        public override void OnAfterDelete()
        {
            if( m_DecayTimer != null )
                m_DecayTimer.Stop();

            m_DecayTimer = null;
        }

        private class InternalTimer : Timer
        {
            private ForgedSilver m_ForgedSilver;

            public InternalTimer(ForgedSilver c, TimeSpan delay)
                : base( delay )
            {
                m_ForgedSilver = c;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                m_ForgedSilver.Name = "Forged Gold Coin";
                m_ForgedSilver.Hue = 2401;
            }
        }

        [Constructable]
        public ForgedSilver()
            : this( 1 )
        {
        }

        [Constructable]
        public ForgedSilver(int amountFrom, int amountTo)
            : this( Utility.RandomMinMax( amountFrom, amountTo ) )
        {
        }

        [Constructable]
        public ForgedSilver(int amount)
            : base( 0xEF0 )
        {
            Stackable = true;
            Amount = amount;
            Name = "Silver Coin";
            m_TimeOfCreation = DateTime.Now;
            BeginDecay( m_DefaultDecayTime );
        }

        public ForgedSilver(Serial serial)
            : base( serial )
        {
        }

        public override int GetDropSound()
        {
            if( Amount <= 1 )
                return 0x2E4;
            else if( Amount <= 5 )
                return 0x2E5;
            else
                return 0x2E6;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version

            writer.WriteDeltaTime( m_TimeOfCreation );

            writer.Write( m_DecayTimer != null );

            if( m_DecayTimer != null )
                writer.WriteDeltaTime( m_DecayTime );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_TimeOfCreation = reader.ReadDeltaTime();

            if( reader.ReadBool() )
                BeginDecay( reader.ReadDeltaTime() - DateTime.Now );
        }
    }

    public class ForgedCopper : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        private DateTime m_TimeOfCreation;

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        private Timer m_DecayTimer;
        private DateTime m_DecayTime;

        private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes( 30.0 );

        public void BeginDecay(TimeSpan delay)
        {
            if( m_DecayTimer != null )
                m_DecayTimer.Stop();

            m_DecayTime = DateTime.Now + delay;

            m_DecayTimer = new InternalTimer( this, delay );
            m_DecayTimer.Start();
        }

        public override void OnAfterDelete()
        {
            if( m_DecayTimer != null )
                m_DecayTimer.Stop();

            m_DecayTimer = null;
        }

        private class InternalTimer : Timer
        {
            private ForgedCopper m_ForgedCopper;

            public InternalTimer(ForgedCopper c, TimeSpan delay)
                : base( delay )
            {
                m_ForgedCopper = c;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                int amount = m_ForgedCopper.Amount;
                Tin tin = new Tin( amount );

                if( m_ForgedCopper.RootParentEntity != null )
                {
                    try
                    {
                        Mobile parent = World.FindMobile( m_ForgedCopper.RootParentEntity.Serial );
                        parent.Backpack.DropItem( tin );
                    }

                    catch
                    {
                        Container parent = m_ForgedCopper.Parent as Container;
                        parent.DropItem( tin );
                    }
                }

                else
                {
                    tin.Map = m_ForgedCopper.Map;
                    tin.Location = m_ForgedCopper.Location;
                }

                m_ForgedCopper.Delete();
            }
        }

        [Constructable]
        public ForgedCopper()
            : this( 1 )
        {
        }

        [Constructable]
        public ForgedCopper(int amountFrom, int amountTo)
            : this( Utility.RandomMinMax( amountFrom, amountTo ) )
        {
        }

        [Constructable]
        public ForgedCopper(int amount)
            : base( 0xEF0 )
        {
            Stackable = true;
            Amount = amount;
            Name = "Copper Coin";
            Hue = 1453;
            m_TimeOfCreation = DateTime.Now;
            BeginDecay( m_DefaultDecayTime );
        }

        public ForgedCopper(Serial serial)
            : base( serial )
        {
        }

        public override int GetDropSound()
        {
            if( Amount <= 1 )
                return 0x2E4;
            else if( Amount <= 5 )
                return 0x2E5;
            else
                return 0x2E6;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version

            writer.WriteDeltaTime( m_TimeOfCreation );

            writer.Write( m_DecayTimer != null );

            if( m_DecayTimer != null )
                writer.WriteDeltaTime( m_DecayTime );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_TimeOfCreation = reader.ReadDeltaTime();

            if( reader.ReadBool() )
                BeginDecay( reader.ReadDeltaTime() - DateTime.Now );
        }
    }
}
