using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Government;
using Server.Network;
using Server.Targets;
using Server.Targeting;

namespace Server.Items
{
    public class CoinMint : Item
    {
        private int m_MintedCoins;
        public int MintedCoins { get { return m_MintedCoins; } set { m_MintedCoins = value; } }

        [Constructable]
        public CoinMint()
            : base(0x2AFD)
        {
            Name = "a mint";
            Movable = false;
        }

        public override void OnDoubleClick(Mobile from)
        {
		
			 //from.SendMessage("This is currently disabled.");
            if( (from as PlayerMobile).Feats.GetFeatLevel(FeatList.Tinkering) >= 3)
            {
                from.Target = new MintTarget(this);
                from.SendMessage("Coins Minted: " + MintedCoins.ToString());
                from.SendMessage("Target copper, silver, or gold ingots to mint coins; or, target coins to melt them back into ingots.");
            }
            else
                from.SendMessage("You aren't sure how to use a mint.");

            base.OnDoubleClick(from);
        }
                
        public CoinMint(Serial serial) : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
 	         base.Serialize(writer);

             writer.Write((int)0); // version

             writer.Write((int)m_MintedCoins);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_MintedCoins = reader.ReadInt();
        }
    }

    public class MintTarget : Target
    {
        private CoinMint m_ThisMint;

        public MintTarget(CoinMint m)
            : base(2, false, TargetFlags.None)
        {
            m_ThisMint = m;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is CopperIngot)
            {
                from.SendMessage("You mint " + (targeted as CopperIngot).Amount.ToString() + " copper ingots into " +
                    ((targeted as CopperIngot).Amount * 10).ToString() + " copper coins.");
                from.AddToBackpack(new Copper((targeted as CopperIngot).Amount * 10));
                m_ThisMint.MintedCoins += (targeted as CopperIngot).Amount * 10;
                (targeted as CopperIngot).Consume((targeted as CopperIngot).Amount);
            }
            else if (targeted is SilverIngot)
            {
                from.SendMessage("You mint " + (targeted as SilverIngot).Amount.ToString() + " silver ingots into " +
                    ((targeted as SilverIngot).Amount * 10).ToString() + " silver coins.");
                from.AddToBackpack(new Silver((targeted as SilverIngot).Amount * 10));
                m_ThisMint.MintedCoins += (targeted as SilverIngot).Amount * 100;
                (targeted as SilverIngot).Consume((targeted as SilverIngot).Amount);
            }
            else if (targeted is GoldIngot)
            {
                from.SendMessage("You mint " + (targeted as GoldIngot).Amount.ToString() + " gold ingots into " +
                    ((targeted as GoldIngot).Amount * 10).ToString() + " gold coins.");
                from.AddToBackpack(new Gold((targeted as GoldIngot).Amount * 10));
                m_ThisMint.MintedCoins += (targeted as GoldIngot).Amount * 1000;
                (targeted as GoldIngot).Consume((targeted as GoldIngot).Amount);
            }
            else if ((targeted is Copper) && (targeted as Copper).Amount >= 10)
            {
                if ((targeted as Copper).Amount >= 10)
                {
                    from.SendMessage("You melt down " + (targeted as Copper).Amount.ToString() + " copper coins into " +
                        ((targeted as Copper).Amount / 10).ToString() + " copper ingots.");

                    int coinamount = (targeted as Copper).Amount;
                    int ingots = 0;
                    while ((targeted as Copper).Amount >= 10)
                    {
                        (targeted as Copper).Consume(10);
                        m_ThisMint.MintedCoins -= 10;
                        ingots++;
                    }

                    if (ingots > 0)
                        from.AddToBackpack(new CopperIngot(ingots));
                }
                else
                    from.SendMessage("You need at least ten coins to produce an ingot.");
            }
            else if (targeted is Silver)
            {
                if ((targeted as Silver).Amount >= 10)
                {
                    from.SendMessage("You melt down " + (targeted as Silver).Amount.ToString() + " silver coins into " +
                        ((targeted as Silver).Amount / 10).ToString() + " silver ingots.");

                    int coinamount = (targeted as Silver).Amount;
                    int ingots = 0;
                    while ((targeted as Silver).Amount >= 10)
                    {
                        (targeted as Silver).Consume(10);
                        m_ThisMint.MintedCoins -= 100;
                        ingots++;
                    }

                    if (ingots > 0)
                        from.AddToBackpack(new SilverIngot(ingots));
                }
                else
                    from.SendMessage("You need at least ten coins to produce an ingot.");
            }
            else if (targeted is Gold)
            {
                if ((targeted as Gold).Amount >= 10)
                {
                    from.SendMessage("You melt down " + (targeted as Gold).Amount.ToString() + " gold coins into " +
                        ((targeted as Gold).Amount / 10).ToString() + " gold ingots.");

                    int coinamount = (targeted as Gold).Amount;
                    int ingots = 0;
                    while ((targeted as Gold).Amount >= 10)
                    {
                        (targeted as Gold).Consume(10);
                        m_ThisMint.MintedCoins -= 1000;
                        ingots++;
                    }

                    if (ingots > 0)
                        from.AddToBackpack(new GoldIngot(ingots));
                }
                else
                    from.SendMessage("You need at least ten coins to produce an ingot.");
            }
            else
            {
                from.SendMessage("You may only mint copper, silver, or gold ingots in official coinage.");
            }

            base.OnTarget(from, targeted);
        }
    }
}