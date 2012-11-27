using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;
using Server.Items.Crops;
using System.Reflection;

namespace Server.Items
{
    public class Seed : Item
    {
        private Type m_ParentPlant;
        public Type ParentPlant { get { return m_ParentPlant; } set { m_ParentPlant = value; } }

        [Constructable]
        public Seed(string parent)
            : base(0x1B22)
        {
            Weight = 0.1;
            SetSeedProperties(this,parent);
        }

        [Constructable]
        public Seed()
            : this(1)
        {
        }

        public Seed(Serial serial)
            : base(serial)
        {
        }
       
        public static void SetSeedProperties(Seed s, string parent)
        {
            string Name = "";
            int Hue = 0;
            Type type = typeof(BaseCrop);

            string parentString = parent;

            switch (parent)
            {
                case "AlmondTree":
                    Name = "Almond Seeds";
                    Hue = 1801;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "AppleTree":
                    Name = "Apple Seeds";
                    Hue = 1802;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "ApricotTree":
                    Name = "Apricot Seeds";
                    Hue = 1803;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "AsparagusCrop":         
                    Name = "Asparagus Seeds";
                    Hue = 1804;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "AvocadoTree":          
                    Name = "Avocado Seeds";
                    Hue = 1805;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BananaTree":
                    Name = "Banana Seeds";
                    Hue = 1806;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BarleyCrop":
                    Name = "Barley Seeds";
                    Hue = 1807;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BeetCrop":
                    Name = "Beet Seeds";
                    Hue = 1808;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BitterHopsCrop":
                    Name = "Bitter Hops Seeds";
                    Hue = 1809;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BlackberryTree":
                    Name = "Blackberry Seeds";
                    Hue = 1810;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BlackRaspberryTree": 
                    Name = "Black Raspberry Seeds";
                    Hue = 1811;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BlueberryTree":
                    Name = "Blueberry Seeds";
                    Hue = 1812;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BroccoliCrop":
                    Name = "Broccoli Seeds";
                    Hue = 1813;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CabbageCrop":
                    Name = "Cabbage Seeds";
                    Hue = 1814;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CantaloupeCrop":
                    Name = "Cantaloupe Seeds";
                    Hue = 1815;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CarrotCrop":
                    Name = "Carrot Seeds";
                    Hue = 1816;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CauliflowerCrop":
                    Name = "Cauliflower Seeds";
                    Hue = 1817;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CeleryCrop":
                    Name = "Celery Seeds";
                    Hue = 1818;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CherryTree":
                    Name = "Cherry Seeds";
                    Hue = 1819;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "ChiliPepperCrop":
                    Name = "Chili Pepper Seeds";
                    Hue = 1820;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CocoaTree":
                    Name = "Cocoa Seeds";
                    Hue = 1821;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CoconutPalm":
                    Name = "Coconut Seeds";
                    Hue = 1822;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CoffeeCrop":
                    Name = "Coffee Seeds";
                    Hue = 1823;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CornCrop":
                    Name = "Corn Seeds";
                    Hue = 1824;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CottonCrop":
                    Name = "Cotton Seeds";
                    Hue = 1825;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CranberryTree":
                    Name = "Cranberry Seeds";
                    Hue = 1826;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CucumberCrop":
                    Name = "Cucumber Seeds";
                    Hue = 1827;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "DatePalm":
                    Name = "Date Seeds";
                    Hue = 1828;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "EggplantCrop":
                    Name = "Eggplant Seeds";
                    Hue = 1829;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "FieldCornCrop":
                    Name = "Field Corn Seeds";
                    Hue = 1830;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "FlaxCrop":
                    Name = "Flax Seeds";
                    Hue = 1831;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GarlicCrop":
                    Name = "Garlic Seeds";
                    Hue = 1832;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GingerCrop":
                    Name = "Ginger Seeds";
                    Hue = 1833;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GinsengCrop":
                    Name = "Ginseng Seeds";
                    Hue = 1834;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GrapefruitTree":
                    Name = "Grapefruit Seeds";
                    Hue = 1835;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GreenBeanCrop":
                    Name = "Green Bean Seeds";
                    Hue = 1836;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GreenPepperCrop":
                    Name = "Green Pepper Seeds";
                    Hue = 1837;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GreenSquashCrop":
                    Name = "Green Squash Seeds";
                    Hue = 1838;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "HayCrop":
                    Name = "Hay Seeds";
                    Hue = 1839;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "HoneydewMelonCrop":
                    Name = "Honeydew Melon Seeds";
                    Hue = 1840;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "KiwiCrop": 
                    Name = "Kiwi Seeds";
                    Hue = 1841;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "LemonTree":
                    Name = "Lemon Seeds";
                    Hue = 1191;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "LettuceCrop": 
                    Name = "Lettuce Seeds";
                    Hue = 1842;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "LimeTree":
                    Name = "Lime Seeds";
                    Hue = 2207;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "MaltCrop":
                    Name = "Malt Seeds";
                    Hue = 1843;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "MandrakeCrop":
                    Name = "Mandrake Seeds";
                    Hue = 1844;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "MangoTree":
                    Name = "Mango Seeds";
                    Hue = 1845;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "NightshadeCrop":
                    Name = "Nightshade Seeds";
                    Hue = 1846;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "OatsCrop":
                    Name = "Oat Seeds";
                    Hue = 1847;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "OnionCrop":
                    Name = "Onion Seeds";
                    Hue = 1848;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "OrangePepperCrop":
                    Name = "Orange Pepper Seeds";
                    Hue = 1849;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "OrangeTree":
                    Name = "Orange Seeds";
                    Hue = 1850;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PeachTree":
                    Name = "Peach Seeds";
                    Hue = 1851;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PeanutCrop":
                    Name = "Peanut Seeds";
                    Hue = 1852;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PearTree":
                    Name = "Pear Seeds";
                    Hue = 1853;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PeasCrop":
                    Name = "Pea Seeds";
                    Hue = 1854;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PineappleTree":
                    Name = "Pineapple Seeds";
                    Hue = 1855;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PistacioTree":
                    Name = "Pistacio Seeds";
                    Hue = 1856;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PomegranateTree":
                    Name = "Pomegranate Seeds";
                    Hue = 1857;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PotatoCrop":
                    Name = "Potato Seeds";
                    Hue = 1858;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PumpkinCrop":
                    Name = "Pumpkin Seeds";
                    Hue = 1859;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "RadishCrop":
                    Name = "Radish Seeds";
                    Hue = 1860;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "RedPepperCrop":
                    Name = "Red Pepper Seeds";
                    Hue = 1861;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "RedRaspberryCrop":
                    Name = "Red Raspberry Seeds";
                    Hue = 1862;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "RiceCrop":
                    Name = "Rice Seeds";
                    Hue = 1863;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SnowHopsCrop":
                    Name = "Snow Hops Seeds";
                    Hue = 1864;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SnowPeasCrop":
                    Name = "Snow Pea Seeds";
                    Hue = 1865;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SoyCrop":
                    Name = "Soy Seeds";
                    Hue = 1866;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SpinachCrop":
                    Name = "Spinach Seeds";
                    Hue = 1867;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SquashCrop":
                    Name = "Squash Seeds";
                    Hue = 1868;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "StrawberryCrop":
                    Name = "Strawberry Seeds";
                    Hue = 1869;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SugarCrop":
                    Name = "Sugar Cane Seeds";
                    Hue = 1870;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SunflowerCrop":
                    Name = "Sunflower Seeds";
                    Hue = 1871;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SweetHopsCrop":
                    Name = "Sweet Hops Seeds";
                    Hue = 1872;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SweetPotatoCrop":
                    Name = "Sweet Potato Seeds";
                    Hue = 1872;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "TeaCrop":
                    Name = "Tea Seeds";
                    Hue = 1873;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "TomatoCrop":
                    Name = "Tomato Seeds";
                    Hue = 1874;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "TurnipCrop":
                    Name = "Turnip Seeds";
                    Hue = 1875;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "WatermelonCrop":
                    Name = "Watermelon Seeds";
                    Hue = 1876;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "WheatCrop":
                    Name = "Wheat Seeds";
                    Hue = 1877;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "QatPlant":
                    Name = "Qat Seeds";
                    Hue = 1878;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PoppyPlant":
                    Name = "Poppy Seeds";
                    Hue = 1879;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SwampweedPlant":
                    Name = "Swampweed Seeds";
                    Hue = 1880;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "TobaccoPlant":
                    Name = "Tobacco Seeds";
                    Hue = 1881;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Pusantia":
                    Name = "Pusantia Seeds";
                    Hue = 1882;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "WillowBark":
                    Name = "Willow Bark Seeds";
                    Hue = 1883;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "WolfLichen":
                    Name = "Wolf Lichen Seeds";
                    Hue = 1884;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Yarrow":
                    Name = "Yarrow Seeds";
                    Hue = 18785;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "AlpineSorrel":
                    Name = "Alpine Sorrel Seeds";
                    Hue = 1886;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "MyrrhaTree":
                    Name = "Myrrha Seeds";
                    Hue = 1887;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "DesertSage":
                    Name = "Desert Sage Seeds";
                    Hue = 1888;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Chia":
                    Name = "Chia Seeds";
                    Hue = 1889;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Cliffrose":
                    Name = "Cliffrose Seeds";
                    Hue = 1890;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Datura":
                    Name = "Datura Seeds";
                    Hue = 1891;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Dogbane":
                    Name = "Dogbane Seeds";
                    Hue = 1892;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Agrimony":
                    Name = "Agrimony Seeds";
                    Hue = 1893;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Echinacea":
                    Name = "Echinacea Seeds";
                    Hue = 1894;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Goldenseal":
                    Name = "Goldenseal Seeds";
                    Hue = 1895;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Mullein":
                    Name = "Mullein Seeds";
                    Hue = 1896;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SkullcapMushroom":
                    Name = "Skullcap Mushroom Seeds";
                    Hue = 1897;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Hyssop":
                    Name = "Hyssop Seeds";
                    Hue = 1898;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SphagnumMoss":
                    Name = "Sphagnum Seeds";
                    Hue = 1899;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "MarshMallow":
                    Name = "Marsh Mallow Seeds";
                    Hue = 1900;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "GingkoTree":
                    Name = "Gingko Seeds";
                    Hue = 1901;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Ginger":
                    Name = "Ginger Seeds";
                    Hue = 1902;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "PinonTree":
                    Name = "Pinon Seeds";
                    Hue = 1903;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CopalTree":
                    Name = "Copal Seeds";
                    Hue = 1904;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "SacraTree":
                    Name = "Sacra Seeds";
                    Hue = 1905;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "JuniperBush":
                    Name = "Juniper Seeds";
                    Hue = 1906;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "RedValerian":
                    Name = "Red Valerian Seeds";
                    Hue = 1172;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Belladonna":
                    Name = "Belladonna Seeds";
                    Hue = 1907;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Comfrey":
                    Name = "Comfrey Seeds";
                    Hue = 1908;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Purslane":
                    Name = "Purslane Seeds";
                    Hue = 1401;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Aloe":
                    Name = "Aloe Seeds";
                    Hue = 1402;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Lousewort":
                    Name = "Lousewort Seeds";
                    Hue = 1403;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "BlueLily":
                    Name = "Blue Lily Seeds";
                    Hue = 1404;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CatsClaw":
                    Name = "Cats Claw Seeds";
                    Hue = 1405;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Damiana":
                    Name = "Damiana Seeds";
                    Hue = 1406;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Foxglove":
                    Name = "Foxglove Seeds";
                    Hue = 1407;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Wormwood":
                    Name = "Wormwood Seeds";
                    Hue = 1408;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Passionflower":
                    Name = "Passionflower Seeds";
                    Hue = 2828;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "ChaulmoograTree":
                    Name = "Chaulmoogra Seeds";
                    Hue = 1454;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "CamphorTree":
                    Name = "Camphor Seeds";
                    Hue = 1453;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
                case "Cinchona":
                    Name = "Cinchona Seeds";
                    Hue = 1452;
                    type = (Type)ScriptCompiler.FindTypeByName(parentString);
                    break;
            }

                s.Name = Name;
                s.Hue = Hue;
                s.ParentPlant = type;
        }

        public Type GetParentType() {
            return m_ParentPlant; }

        public override void OnDoubleClick(Mobile from)
        {
            int eFarmingFeatLevel = ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.EnhancedHarvesting);

            if (from is PlayerMobile && from.Backpack != null && IsChildOf(from.Backpack))
            {
                if (eFarmingFeatLevel > 1)
                {
                    from.SendMessage("Choose some tilled soil in which to plant your " + this.Name + ".");
                    from.Target = new SeedTarget(this);
                }
                else
                    from.SendMessage("You don't know enough about agriculture to use this.");
            }
        }

        public static void PickCropSeed(Mobile m, string type)
        {
            if (m is PlayerMobile)
            {
                PlayerMobile from = m as PlayerMobile;
                int FarmingLevel = ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Farming);
                int HarvestLevel = ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.EnhancedHarvesting);
                int seedChance = (FarmingLevel + HarvestLevel) * ((from.RawInt / Utility.RandomMinMax(2, from.RawInt)));

                if (FarmingLevel > 2 && Utility.Random(100) < seedChance)
                {
                    from.SendMessage("You harvest some seeds.");
                    if (from is PlayerMobile && (from as PlayerMobile).CraftContainer != null && !(from as PlayerMobile).CraftContainer.Deleted && (from as PlayerMobile).CraftContainer.IsChildOf(from))
                            (from as PlayerMobile).CraftContainer.AddItem(new Seed(type));
                    else
                        from.AddToBackpack(new Seed(type));
                }
            }
        }

        public static void PickPlantSeed(Mobile m, string type)
        {
            if (!(m is PlayerMobile))
                return;

            PlayerMobile from = m as PlayerMobile;

            int seedChance = from.Feats.GetFeatLevel(FeatList.HerbalGathering);
            seedChance += from.Feats.GetFeatLevel(FeatList.Farming);
            seedChance += from.RawInt / 10;

            if (from.Feats.GetFeatLevel(FeatList.Farming) > 0 && Utility.Random(100) < seedChance)
            {
                from.SendMessage("You harvest some seeds.");
                if (from.CraftContainer != null && !from.CraftContainer.Deleted && from.CraftContainer.IsChildOf(from))
                    from.CraftContainer.AddItem(new Seed(type));
                else
                    from.AddToBackpack(new Seed(type));
            }

        }

        private class SeedTarget : Target
        {
            private Seed thisSeed;

            public SeedTarget(Seed item)
                : base(1, false, TargetFlags.None)
            {
                thisSeed = item;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ is FarmSoil)
                {
                    FarmSoil t = (FarmSoil)targ;
                    from.Direction = from.GetDirectionTo(t);

                    if (t.getOccupied())
                        from.SendMessage("That has already been seeded.");
                    else
                    {
                        t.SeedSoil(from, thisSeed);
                        thisSeed.Consume();
                    }
                }
                else
                    from.SendMessage("You can only plant a seed in tilled soil.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(m_ParentPlant == null ? null : m_ParentPlant.FullName);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            string type = reader.ReadString();

            if (type != null)
                m_ParentPlant = ScriptCompiler.FindTypeByFullName(type);

            int version = reader.ReadInt();
        }
    }
}