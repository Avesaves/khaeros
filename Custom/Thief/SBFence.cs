using System;
using System.Collections;

using Server.Items;

namespace Server.Mobiles
{
    public class SBFence : SBInfo
    {
        private ArrayList m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBFence()
        {
        }

        public override ArrayList BuyInfo
        {
            get { return m_BuyInfo; }
        }

        public override IShopSellInfo SellInfo
        {
            get { return m_SellInfo; }
        }
    }

    public class InternalBuyInfo : ArrayList
    {
        //Maybe he also sells a thief friendly item or too as well.
        public InternalBuyInfo()
        {
            Add(new GenericBuyInfo(typeof(Lockpick), 10, 40, 0x14FC, 0));
            //Add(new GenericBuyInfo(typeof(HairStylingApron), 200, 20, 0x153d, 0));
            Add(new GenericBuyInfo(typeof(DyeTub), 8, 5, 0xFAB, 0));
            Add(new GenericBuyInfo(typeof(MixingSet), 10, 2, 6237, 0));
            Add(new GenericBuyInfo(typeof(SewingKit), 10, 10, 0xF9D, 0));
            Add(new GenericBuyInfo(typeof(TinkerTools), 10, 10, 0x1EB8, 0));
        }
    }

    public class InternalSellInfo : GenericSellInfo
    {
        //Add in stuff here for the fence to buy.
        public InternalSellInfo()
        {
            //Tailoring Items
            Add(typeof(Scissors), 1);
            Add(typeof(SewingKit), 1);
            Add(typeof(DyeTub), 1);
            Add(typeof(FancyShirt), 1);
            Add(typeof(Shirt), 1);
            Add(typeof(ShortPants), 1);
            Add(typeof(LongPants), 1);
            Add(typeof(Cloak), 1);
            Add(typeof(FancyDress), 2);
            Add(typeof(Robe), 2);
            Add(typeof(PlainDress), 1);
            Add(typeof(Skirt), 1);
            Add(typeof(Kilt), 1);
            Add(typeof(Doublet), 1);
            Add(typeof(Tunic), 1);
            Add(typeof(FullApron), 1);
            Add(typeof(HalfApron), 1);
            Add(typeof(FloppyHat), 1);
            Add(typeof(WideBrimHat), 13);
            Add(typeof(Cap), 1);
            Add(typeof(SkullCap), 1);
            Add(typeof(Bandana), 1);
            Add(typeof(TallStrawHat), 1);
            Add(typeof(StrawHat), 1);
            Add(typeof(TallBrimHat), 1);
            Add(typeof(Bonnet), 1);
            Add(typeof(FeatheredHat), 1);
            Add(typeof(TricorneHat), 1);
            Add(typeof(Flax), 1);
            Add(typeof(Cotton), 1);
            Add(typeof(Wool), 1);
            Add(typeof(BoltOfCloth), 5);
            Add(typeof(Hides), 1);
            Add(typeof(ThighBoots), 2);
            Add(typeof(Shoes), 1);
            Add(typeof(Boots), 1);
            Add(typeof(Sandals), 1);
            Add(typeof(Leather), 2);
            Add(typeof(SpoolOfThread), 1);
            Add(typeof(DarkYarn), 1);
            Add(typeof(LightYarn), 1);
            Add(typeof(LightYarnUnraveled), 1);
            Add(typeof(StuddedArms), 3);
            Add(typeof(StuddedChest), 6);
            Add(typeof(StuddedGloves), 2);
            Add(typeof(StuddedGorget), 1);
            Add(typeof(StuddedLegs), 5);
            Add(typeof(FemaleStuddedChest), 4);
            Add(typeof(StuddedBustierArms), 3);
            Add(typeof(LeatherArms), 2);
            Add(typeof(LeatherChest), 5);
            Add(typeof(LeatherGloves), 1);
            Add(typeof(LeatherGorget), 1);
            Add(typeof(LeatherLegs), 4);
            Add(typeof(LeatherCap), 1);
            Add(typeof(FemaleLeatherChest), 4);
            Add(typeof(FemaleStuddedChest), 5);
            Add(typeof(LeatherShorts), 2);
            Add(typeof(LeatherSkirt), 2);
            Add(typeof(LeatherBustierArms), 3);
            Add(typeof(StuddedBustierArms), 3);

            //Jewelry Items
            Add(typeof(Amber), 3);
            Add(typeof(Amethyst), 6);
            Add(typeof(Citrine), 3);
            Add(typeof(Diamond), 10);
            Add(typeof(Emerald), 7);
            Add(typeof(Ruby), 3);
            Add(typeof(Sapphire), 7);
            Add(typeof(StarSapphire), 8);
            Add(typeof(Tourmaline), 3);
            Add(typeof(GoldRing), 5);
            Add(typeof(SilverRing), 2);
            Add(typeof(Necklace), 1);
            Add(typeof(GoldNecklace), 4);
            Add(typeof(GoldBeadNecklace), 5);
            Add(typeof(SilverNecklace), 2);
            Add(typeof(SilverBeadNecklace), 3);
            Add(typeof(Beads), 1);
            Add(typeof(GoldBracelet), 3);
            Add(typeof(SilverBracelet), 2);
            Add(typeof(GoldEarrings), 2);
            Add(typeof(SilverEarrings), 1);

            //Tavern Items
            Add(typeof(BeverageBottle), 1);
            Add(typeof(Jug), 1);
            Add(typeof(Pitcher), 1);
            Add(typeof(GlassMug), 1);
            Add(typeof(Candle), 1);
            Add(typeof(Chessboard), 1);
            Add(typeof(CheckerBoard), 1);
            Add(typeof(Backgammon), 1);
            Add(typeof(Dices), 1);

            //Carpentry Items
            Add(typeof(WoodenBox), 1);
            Add(typeof(SmallCrate), 1);
            Add(typeof(MediumCrate), 1);
            Add(typeof(LargeCrate), 2);
            Add(typeof(WoodenChest), 2);
            Add(typeof(LargeTable), 2);
            Add(typeof(Nightstand), 1);
            Add(typeof(YewWoodTable), 3);
            Add(typeof(Throne), 4);
            Add(typeof(WoodenThrone), 4);
            Add(typeof(Stool), 1);
            Add(typeof(FootStool), 1);
            Add(typeof(FancyWoodenChairCushion), 3);
            Add(typeof(WoodenChairCushion), 2);
            Add(typeof(WoodenChair), 1);
            Add(typeof(BambooChair), 1);
            Add(typeof(WoodenBench), 1);
            Add(typeof(Saw), 1);
            Add(typeof(Scorp), 1);
            Add(typeof(SmoothingPlane), 1);
            Add(typeof(DrawKnife), 1);
            Add(typeof(Froe), 1);
            Add(typeof(Hammer), 1);
            Add(typeof(Inshave), 1);
            Add(typeof(JointingPlane), 1);
            Add(typeof(MouldingPlane), 1);
            Add(typeof(DovetailSaw), 1);
            Add(typeof(Board), 1);
            Add(typeof(Axle), 1);
            Add(typeof(Club), 3);
            Add(typeof(Lute), 1);
            Add(typeof(LapHarp), 1);
            Add(typeof(Tambourine), 1);
            Add(typeof(Drums), 1);
            Add(typeof(Log), 1);
            Add(typeof(Board), 1);
            Add(typeof(FletcherTools), 1);
            Add(typeof(HeavyCrossbow), 7);
            Add(typeof(Bow), 5);
            Add(typeof(Crossbow), 5);
            Add(typeof(CompositeBow), 5);
            Add(typeof(RepeatingCrossbow), 12);
            Add(typeof(BlackStaff), 5);
            Add(typeof(GnarledStaff), 2);
            Add(typeof(QuarterStaff), 3);
            Add(typeof(ClericCrook), 3);
            Add(typeof(Nails), 1);
            Add(typeof(WoodenKiteShield), 3);

            //Blacksmith Items
            Add(typeof(IronIngot), 1);
            Add(typeof(Tongs), 1);
            Add(typeof(BattleAxe), 4);
            Add(typeof(BeardedDoubleAxe), 6);
            Add(typeof(HaftedAxe), 4);
            Add(typeof(LargeBattleAxe), 4);
            Add(typeof(Pickaxe), 1);
            Add(typeof(TwoHandedAxe), 4);
            Add(typeof(WarAxe), 2);
            Add(typeof(Axe), 2);
            Add(typeof(ButcherKnife), 1);
            Add(typeof(Cleaver), 1);
            Add(typeof(Dagger), 2);
            Add(typeof(SkinningKnife), 1);
            Add(typeof(Bardiche), 8);
            Add(typeof(Halberd), 8);
            Add(typeof(Spear), 7);
            Add(typeof(Pitchfork), 3);
            Add(typeof(ShortSpear), 4);
            Add(typeof(Broadsword), 4);
            Add(typeof(Cutlass), 3);
            Add(typeof(Kryss), 3);
            Add(typeof(Longsword), 4);
            Add(typeof(Scimitar), 6);
            Add(typeof(ThinLongsword), 3);
            Add(typeof(Broadsword), 7);
            Add(typeof(Scythe), 7);
            Add(typeof(HandScythe), 2);
            Add(typeof(BarbarianScepter), 3);
            Add(typeof(Glaive), 6);
            Add(typeof(Pike), 9);
            Add(typeof(DoubleBladedStaff), 7);
            Add(typeof(Lance), 11);
            Add(typeof(CrescentBlade), 8);
            Add(typeof(SmithHammer), 2);
            Add(typeof(RingmailArms), 2);
            Add(typeof(RingmailChest), 6);
            Add(typeof(RingmailGloves), 2);
            Add(typeof(RingmailLegs), 4);
            Add(typeof(PlateArms), 4);
            Add(typeof(PlateChest), 10);
            Add(typeof(PlateGloves), 2);
            Add(typeof(PlateGorget), 2);
            Add(typeof(PlateLegs), 8);
            Add(typeof(Buckler), 2);
            Add(typeof(BronzeShield), 3);
            Add(typeof(MetalShield), 6);
            Add(typeof(KiteShield), 6);
            Add(typeof(HeaterShield), 6);
            Add(typeof(ChainCoif), 2);
            Add(typeof(ChainChest), 7);
            Add(typeof(ChainLegs), 4);
            

            //Tinkerering Items
            Add(typeof(AxleGears), 2);
            Add(typeof(Springs), 1);
            Add(typeof(Gears), 1);
            Add(typeof(ClockFrame), 5);
            Add(typeof(Clock), 7);
            Add(typeof(Globe), 2);
            Add(typeof(Hinge), 1);
            Add(typeof(SextantParts), 1);
            Add(typeof(Spyglass), 3);
            Add(typeof(Sextant), 2);
            Add(typeof(ScribesPen), 1);
            Add(typeof(TinkerTools), 3);
            Add(typeof(MixingSet), 3);
        }
    }
}
