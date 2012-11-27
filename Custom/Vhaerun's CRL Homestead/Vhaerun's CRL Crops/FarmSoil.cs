using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Engines.Harvest;
using Server.Engines.Craft;
using Server.Items.Crops;
using Server.TimeSystem;
using System.Collections.Generic;


namespace Server.Items
{
    public class FarmSoil : Item
    {
        public static PlantTimer UniversalPlantTimer;

        private Mobile m_Owner;
        private String m_SeedName;
        private Type m_PlantType;
        private bool m_Occupied;        
        private bool m_FullGrown;
        private DateTime m_Seeded;
        private DateTime m_Bloom;        
        private TimeSpan m_GrowthDelay;        
        private DateTime m_LastPlantRefresh;
        private DateTime m_PlantDeath;
        private Item m_Plant;

        #region Public Get/Sets
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public String SeedName { get { return m_SeedName; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Type PlantType { get { return m_PlantType; } set { m_PlantType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Occupied { get { return m_Occupied; } set { m_Occupied = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FullGrown { get { return m_FullGrown; } set { m_FullGrown = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Seeded { get { return m_Seeded; } set { m_Seeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Bloomed { get { return m_Bloom; } set { m_Bloom = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan GrowthDelay { get { return m_GrowthDelay; } set { m_GrowthDelay = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastPlantRefreshCheck { get { return m_LastPlantRefresh; } set { m_LastPlantRefresh = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PlantDeathCheck { get { return m_PlantDeath; } set { m_PlantDeath = value; } }

        public Item Plant { get { return m_Plant; } set { m_Plant = value; } }
        #endregion

        [Constructable]
        public FarmSoil(Mobile owner)
            : base(0x0914)
        {
            owner.Animate(32, 5, 1, true, false, 0);
            owner.PlaySound(0x365);

            Name = "tilled soil";
            Movable = false;

            Owner = owner;
            m_SeedName = "";
            PlantType = typeof(BaseCrop);
            Occupied = false;
            FullGrown = false;
            m_Seeded = DateTime.Now;
            m_Bloom = DateTime.Now;
            m_GrowthDelay = TimeSpan.FromDays(1);

            m_LastPlantRefresh = DateTime.Now;
            m_PlantDeath = m_LastPlantRefresh + TimeSpan.FromDays(((IKhaerosMobile)m_Owner).Feats.GetFeatLevel(FeatList.Farming));
            WaterHueCheck();
        }

        public override void OnDelete()
        {
            if(Plant != null && !Plant.Deleted)
                Plant.Delete();

            base.OnDelete();
        }

        public void setOwner(Mobile owner) { m_Owner = owner; }

        public void setOccupied(bool x)
        {
            Occupied = x;
        }

        public bool getOccupied()
        {
            return Occupied;
        }

        public void RefreshSoil(Mobile from, int days)
        {
            if (m_LastPlantRefresh + TimeSpan.FromDays(3) < DateTime.Now)
            {
                from.SendMessage("You water the soil, refreshing it.");
                from.PlaySound(0x2D6);
                m_LastPlantRefresh = DateTime.Now;
                m_PlantDeath = m_PlantDeath + TimeSpan.FromDays(days + (from as PlayerMobile).Feats.GetFeatLevel(FeatList.EnhancedHarvesting));
            }
            else
                from.SendMessage("This looks like it has enough water for now.");

            WaterHueCheck();
        }

        public void SeedSoil(Mobile from, Seed s)
        {
            if (from != null && !from.Deleted)
            {
                //planting gfx and sfx
                from.Animate(32, 5, 1, true, false, 0);
                from.PlaySound(0x368);

                //editing the targeted soil
                setOccupied(true);
                setOwner(from);
                PlantType = s.GetParentType();
                m_SeedName = s.Name;
                int enhancedHarvestingLevel = ((IKhaerosMobile)m_Owner).Feats.GetFeatLevel(FeatList.EnhancedHarvesting);

                //tracking when the soil was seeded and when it will bloom, and creating a delay for growth based on that.
                m_Seeded = DateTime.Now;
                m_Bloom = m_Seeded.AddDays(7 - enhancedHarvestingLevel);
                m_GrowthDelay = m_Bloom - m_Seeded;

                //reseting the deletion date of the soil and attached plants now that it has been refreshed through seeding.
                m_LastPlantRefresh = m_Seeded;
                m_PlantDeath = m_LastPlantRefresh + TimeSpan.FromDays(3 + enhancedHarvestingLevel);

                if (UniversalPlantTimer == null || !UniversalPlantTimer.Running)
                {
                    UniversalPlantTimer = new PlantTimer();
                    UniversalPlantTimer.Start();
                }
            }
            else
            {
                from.SendMessage("ERROR: Seed's parent is " + s.GetParentType());
                return;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Warmode)
            {
                from.Direction = from.GetDirectionTo(this);
                from.SendMessage("You uproot the plant and destroy the farmland!");
                from.Animate(9, 7, 1, true, false, 0);
                from.PlaySound(0x059);
                if (from != m_Owner)
                {
                    bool crime = true;

                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if(g.Members.Contains(m_Owner) && g.Members.Contains(from))
                            crime = false;

                        if (!crime)
                            break;
                    }

                    if(crime)
                        (from as PlayerMobile).CriminalActivity = true;
                }

                this.Delete();
                return;
            }

            int enhancedHarvestingLevel = ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.EnhancedHarvesting);

            if (enhancedHarvestingLevel < 1)
                return;

            if (enhancedHarvestingLevel > 0)
            {
                if (this.Occupied)
                    from.SendMessage("This soil is seeded with " + this.m_SeedName + ".");
                else
                    from.SendMessage("This soil is not seeded.");
            }

            if (enhancedHarvestingLevel > 1)
                from.SendMessage("This soil was last cared for on " + this.m_LastPlantRefresh + ".");

            if (enhancedHarvestingLevel > 2)
                from.SendMessage("Unless cared for, this soil will be reclaimed by nature on " + this.m_PlantDeath + ".");

            WaterHueCheck();

            base.OnDoubleClick(from);
        }

        public void WaterHueCheck()
        {
            if ( m_PlantDeath.DayOfYear - DateTime.Now.DayOfYear >= 3)
            {
                this.Hue = 1190;
            }
            else if (m_PlantDeath.DayOfYear - DateTime.Now.DayOfYear >= 2)
            {
                this.Hue = 0;
            }
            else
                this.Hue = 2307;
        }

        public FarmSoil(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {            
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((Mobile)m_Owner);
            writer.Write((string)m_SeedName);

            if (m_PlantType != null)
            {
                writer.Write((bool)true);
                writer.Write((string)m_PlantType.Name);
            }
            else
                writer.Write((bool)false);

            writer.Write((bool)m_Occupied);        
            writer.Write((bool)m_FullGrown);
            writer.Write((DateTime)m_Seeded);
            writer.Write((DateTime)m_Bloom);        
            writer.Write((TimeSpan)m_GrowthDelay);        
            writer.Write((DateTime)m_LastPlantRefresh);
            writer.Write((DateTime)m_PlantDeath);
            writer.Write((Item)m_Plant);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Owner = (Mobile)reader.ReadMobile();
                        m_SeedName = (String)reader.ReadString();

                        if (reader.ReadBool())
                            m_PlantType = (Type)ScriptCompiler.FindTypeByName(reader.ReadString());

                        m_Occupied = (bool)reader.ReadBool();
                        m_FullGrown = (bool)reader.ReadBool();
                        m_Seeded = (DateTime)reader.ReadDateTime();
                        m_Bloom = (DateTime)reader.ReadDateTime();
                        m_GrowthDelay = (TimeSpan)reader.ReadTimeSpan();
                        m_LastPlantRefresh = (DateTime)reader.ReadDateTime();
                        m_PlantDeath = (DateTime)reader.ReadDateTime();
                        if (version < 1)
                            reader.ReadBool();

                        if (version < 1)
                        {
                            if (reader.ReadBool())
                                m_Plant = (Item)reader.ReadItem();
                        }
                        else
                            m_Plant = (Item)reader.ReadItem();
                        goto case 0;
                    }
                case 0: break;
            }

            if (UniversalPlantTimer == null || !UniversalPlantTimer.Running)
            {
                UniversalPlantTimer = new PlantTimer();
                UniversalPlantTimer.Start();
            }

            WaterHueCheck();
        }
    }

    public class PlantTimer : Timer
    {
        private Type parent;

        public PlantTimer()
            : base(TimeSpan.FromMinutes(15), TimeSpan.FromHours(12))
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            CheckPlantGrowth();

            base.OnTick();
        }

        public void CheckPlantGrowth()
        {
            List<FarmSoil> removeSoil = new List<FarmSoil>();
            List<FarmSoil> bloomSoil = new List<FarmSoil>();

            foreach (Item i in World.Items.Values)
            {
                if (i is FarmSoil)
                {
                    FarmSoil cropSoil = i as FarmSoil;

                    cropSoil.WaterHueCheck();

                    if (!cropSoil.FullGrown && cropSoil.Occupied && (cropSoil.Bloomed <= DateTime.Now))
                    {
                        if (cropSoil.PlantType != null)
                        {
                            bloomSoil.Add(cropSoil);
                        }
                    }
                    else if (cropSoil.FullGrown)
                    {
                        if (DateTime.Now > cropSoil.PlantDeathCheck)
                        {
                            removeSoil.Add(cropSoil);
                        }
                    }
                }
            }

            for (int i = bloomSoil.Count - 1; i > -1; i--)
            {
                FarmSoil cropSoil = bloomSoil[i];
                Type parent = cropSoil.PlantType;
                #region Blooming Plant & Moving It To the Soil
                if (parent == typeof(AlmondTree))
                {
                    AlmondTree newCrop = new AlmondTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(AppleTree))
                {
                    AppleTree newCrop = new AppleTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(ApricotTree))
                {
                    ApricotTree newCrop = new ApricotTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(AsparagusCrop))
                {
                    AsparagusCrop newCrop = new AsparagusCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(AvocadoTree))
                {
                    AvocadoTree newCrop = new AvocadoTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BananaTree))
                {
                    BananaTree newCrop = new BananaTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BarleyCrop))
                {
                    BarleyCrop newCrop = new BarleyCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BeetCrop))
                {
                    BeetCrop newCrop = new BeetCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BitterHopsCrop))
                {
                    BitterHopsCrop newCrop = new BitterHopsCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BlackberryTree))
                {
                    BlackberryTree newCrop = new BlackberryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BlackRaspberryTree))
                {
                    BlackRaspberryTree newCrop = new BlackRaspberryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BlueberryTree))
                {
                    BlueberryTree newCrop = new BlueberryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(BroccoliCrop))
                {
                    BroccoliCrop newCrop = new BroccoliCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CabbageCrop))
                {
                    CabbageCrop newCrop = new CabbageCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CantaloupeCrop))
                {
                    CantaloupeCrop newCrop = new CantaloupeCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CarrotCrop))
                {
                    CarrotCrop newCrop = new CarrotCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CauliflowerCrop))
                {
                    CauliflowerCrop newCrop = new CauliflowerCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CeleryCrop))
                {
                    CeleryCrop newCrop = new CeleryCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CherryTree))
                {
                    CherryTree newCrop = new CherryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(ChiliPepperCrop))
                {
                    ChiliPepperCrop newCrop = new ChiliPepperCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CocoaTree))
                {
                    CocoaTree newCrop = new CocoaTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CoconutPalm))
                {
                    CoconutPalm newCrop = new CoconutPalm();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CoffeeCrop))
                {
                    CoffeeCrop newCrop = new CoffeeCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CornCrop))
                {
                    CornCrop newCrop = new CornCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CottonCrop))
                {
                    CottonCrop newCrop = new CottonCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CranberryTree))
                {
                    CranberryTree newCrop = new CranberryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(CucumberCrop))
                {
                    CucumberCrop newCrop = new CucumberCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(DatePalm))
                {
                    DatePalm newCrop = new DatePalm();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(EggplantCrop))
                {
                    EggplantCrop newCrop = new EggplantCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(FieldCornCrop))
                {
                    FieldCornCrop newCrop = new FieldCornCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(FlaxCrop))
                {
                    FlaxCrop newCrop = new FlaxCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GarlicCrop))
                {
                    GarlicCrop newCrop = new GarlicCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GingerCrop))
                {
                    GingerCrop newCrop = new GingerCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GinsengCrop))
                {
                    GinsengCrop newCrop = new GinsengCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GrapefruitTree))
                {
                    GrapefruitTree newCrop = new GrapefruitTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GreenBeanCrop))
                {
                    GreenBeanCrop newCrop = new GreenBeanCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GreenPepperCrop))
                {
                    GreenPepperCrop newCrop = new GreenPepperCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(GreenSquashCrop))
                {
                    GreenSquashCrop newCrop = new GreenSquashCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(HayCrop))
                {
                    HayCrop newCrop = new HayCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(HoneydewMelonCrop))
                {
                    HoneydewMelonCrop newCrop = new HoneydewMelonCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(KiwiCrop))
                {
                    KiwiCrop newCrop = new KiwiCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(LemonTree))
                {
                    LemonTree newCrop = new LemonTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(LettuceCrop))
                {
                    LettuceCrop newCrop = new LettuceCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(LimeTree))
                {
                    LimeTree newCrop = new LimeTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(MaltCrop))
                {
                    MaltCrop newCrop = new MaltCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(MandrakeCrop))
                {
                    MandrakeCrop newCrop = new MandrakeCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(MangoTree))
                {
                    MangoTree newCrop = new MangoTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(NightshadeCrop))
                {
                    NightshadeCrop newCrop = new NightshadeCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(OatsCrop))
                {
                    OatsCrop newCrop = new OatsCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(OnionCrop))
                {
                    OnionCrop newCrop = new OnionCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(OrangePepperCrop))
                {
                    OrangePepperCrop newCrop = new OrangePepperCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(OrangeTree))
                {
                    OrangeTree newCrop = new OrangeTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PeachTree))
                {
                    PeachTree newCrop = new PeachTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PeanutCrop))
                {
                    PeanutCrop newCrop = new PeanutCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PearTree))
                {
                    PearTree newCrop = new PearTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PeasCrop))
                {
                    PeasCrop newCrop = new PeasCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PineappleTree))
                {
                    PineappleTree newCrop = new PineappleTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PistacioTree))
                {
                    PistacioTree newCrop = new PistacioTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PomegranateTree))
                {
                    PomegranateTree newCrop = new PomegranateTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PotatoCrop))
                {
                    PotatoCrop newCrop = new PotatoCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(PumpkinCrop))
                {
                    PumpkinCrop newCrop = new PumpkinCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(RadishCrop))
                {
                    RadishCrop newCrop = new RadishCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(RedPepperCrop))
                {
                    RedPepperCrop newCrop = new RedPepperCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(RedRaspberryTree))
                {
                    RedRaspberryTree newCrop = new RedRaspberryTree(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(RiceCrop))
                {
                    RiceCrop newCrop = new RiceCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SnowHopsCrop))
                {
                    SnowHopsCrop newCrop = new SnowHopsCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SnowPeasCrop))
                {
                    SnowPeasCrop newCrop = new SnowPeasCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SoyCrop))
                {
                    SoyCrop newCrop = new SoyCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SpinachCrop))
                {
                    SpinachCrop newCrop = new SpinachCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SquashCrop))
                {
                    SquashCrop newCrop = new SquashCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(StrawberryCrop))
                {
                    StrawberryCrop newCrop = new StrawberryCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SugarCrop))
                {
                    SugarCrop newCrop = new SugarCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SunFlowerCrop))
                {
                    SunFlowerCrop newCrop = new SunFlowerCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SweetHopsCrop))
                {
                    SweetHopsCrop newCrop = new SweetHopsCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(SweetPotatoCrop))
                {
                    SweetPotatoCrop newCrop = new SweetPotatoCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(TeaCrop))
                {
                    TeaCrop newCrop = new TeaCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(TomatoCrop))
                {
                    TomatoCrop newCrop = new TomatoCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(TurnipCrop))
                {
                    TurnipCrop newCrop = new TurnipCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(WatermelonCrop))
                {
                    WatermelonCrop newCrop = new WatermelonCrop(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(WheatCrop))
                {
                    WheatCrop newCrop = new WheatCrop(cropSoil.Owner);
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                }
                else if (parent == typeof(QatPlant))
                {
                    QatPlant newCrop = new QatPlant();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(PoppyPlant))
                {
                    PoppyPlant newCrop = new PoppyPlant();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(SwampweedPlant))
                {
                    SwampweedPlant newCrop = new SwampweedPlant();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(TobaccoPlant))
                {
                    TobaccoPlant newCrop = new TobaccoPlant();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Pusantia))
                {
                    Pusantia newCrop = new Pusantia();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(WillowBark))
                {
                    WillowBark newCrop = new WillowBark();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(WolfLichen))
                {
                    WolfLichen newCrop = new WolfLichen();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Yarrow))
                {
                    Yarrow newCrop = new Yarrow();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(AlpineSorrel))
                {
                    AlpineSorrel newCrop = new AlpineSorrel();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(MyrrhaTree))
                {
                    MyrrhaTree newCrop = new MyrrhaTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(DesertSage))
                {
                    DesertSage newCrop = new DesertSage();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Chia))
                {
                    Chia newCrop = new Chia();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Cliffrose))
                {
                    Cliffrose newCrop = new Cliffrose();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Datura))
                {
                    Datura newCrop = new Datura();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Dogbane))
                {
                    Dogbane newCrop = new Dogbane();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Agrimony))
                {
                    Agrimony newCrop = new Agrimony();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Echinacea))
                {
                    Echinacea newCrop = new Echinacea();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Goldenseal))
                {
                    Goldenseal newCrop = new Goldenseal();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Mullein))
                {
                    Mullein newCrop = new Mullein();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(SkullcapMushroom))
                {
                    SkullcapMushroom newCrop = new SkullcapMushroom();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Hyssop))
                {
                    Hyssop newCrop = new Hyssop();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(SphagnumMoss))
                {
                    SphagnumMoss newCrop = new SphagnumMoss();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(MarshMallow))
                {
                    MarshMallow newCrop = new MarshMallow();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(GingkoTree))
                {
                    GingkoTree newCrop = new GingkoTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Ginger))
                {
                    Ginger newCrop = new Ginger();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(PinonTree))
                {
                    PinonTree newCrop = new PinonTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(CopalTree))
                {
                    CopalTree newCrop = new CopalTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(SacraTree))
                {
                    SacraTree newCrop = new SacraTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(JuniperBush))
                {
                    JuniperBush newCrop = new JuniperBush();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(RedValerian))
                {
                    RedValerian newCrop = new RedValerian();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Belladonna))
                {
                    Belladonna newCrop = new Belladonna();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Comfrey))
                {
                    Comfrey newCrop = new Comfrey();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Purslane))
                {
                    Purslane newCrop = new Purslane();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Aloe))
                {
                    Aloe newCrop = new Aloe();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Cinchona))
                {
                    Cinchona newCrop = new Cinchona();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Lousewort))
                {
                    Lousewort newCrop = new Lousewort();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(BlueLily))
                {
                    BlueLily newCrop = new BlueLily();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(CatsClaw))
                {
                    CatsClaw newCrop = new CatsClaw();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Damiana))
                {
                    Damiana newCrop = new Damiana();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Foxglove))
                {
                    Foxglove newCrop = new Foxglove();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Wormwood))
                {
                    Wormwood newCrop = new Wormwood();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(Passionflower))
                {
                    Passionflower newCrop = new Passionflower();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(ChaulmoograTree))
                {
                    ChaulmoograTree newCrop = new ChaulmoograTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else if (parent == typeof(CamphorTree))
                {
                    CamphorTree newCrop = new CamphorTree();
                    newCrop.MoveToWorld(cropSoil.Location, cropSoil.Map);
                    cropSoil.Plant = newCrop;
                    (cropSoil.Plant as BasePlant).Planted = true;
                    (cropSoil.Plant as BasePlant).Soil = cropSoil;
                }
                else
                    removeSoil.Add(cropSoil);

                //editing the planted soil
                if (cropSoil != null && !cropSoil.Deleted && !removeSoil.Contains(cropSoil))
                {
                    cropSoil.PlantType = parent;
                    cropSoil.FullGrown = true;
                }
                #endregion
            }

            for (int i = removeSoil.Count - 1; i > -1; i--)
            {
                removeSoil[i].Delete();
            }
        }
    }
}