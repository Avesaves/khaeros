using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Targets;
using Server.Regions;
using Server.ContextMenus;
using Server.Engines.Quests;
using MoveImpl=Server.Movement.MovementImpl;
using Server.Misc;
using Server.TimeSystem;

namespace Server.Mobiles
{        


    public enum Armament
        {
            None = 0,
            Light = 1,
            Medium = 2,
            Heavy = 3,
            Ranged = 4,
            LightCavalry = 5,
            HeavyCavalry = 6
        }

    public enum Training
    {
        None = 0,
        Assault = 1,
        Endurance = 2,
        Strategy = 3,
        Ranged = 4,
        Dragoon = 5
    }

    public class Soldier : BaseKhaerosMobile
    {
        #region PRIVATE VARIABLE DECLARATION

        /* Much of this information is pulled from the gump that creates the soldier. */

        private Armament m_Armaments;        
        private Training m_Training;
        private int m_PayRate;
        private Direction m_FaceDirection;
        private DateTime m_speechInterval;
        private String m_BaseName;
        private ReportTimer m_ReportTimer;

        #endregion

        #region GETTERS & SETTERS

        public Armament Armaments { get { return m_Armaments; } set { m_Armaments = value; } }        
        public Training Training { get { return m_Training; } set { m_Training = value; } }
        public int PayRate { get { return m_PayRate; } set { m_PayRate = value; } }
        public Direction FaceDirection { get { return m_FaceDirection; } set { m_FaceDirection = value; } }
        public DateTime SpeechInterval { get { return m_speechInterval; } set { m_speechInterval = value; } }
        public String BaseName { get { return m_BaseName; } set { m_BaseName = value; } }
        public ReportTimer ReportTimer { get { return m_ReportTimer; } set { m_ReportTimer = value; } }

        #endregion

        #region CONSTRUCTORS

        [Constructable]
        public Soldier()
            : this(Nation.None) { }

        [Constructable]
        public Soldier(Nation n)
            : this(n, Utility.RandomMinMax(1, 4)) { }

        [Constructable]
        public Soldier(Nation n, int arms)
            : this(n, arms, 0) { }

        [Constructable]
        public Soldier(Nation n, int arms, int training)
            : this(n, arms, training, GovernmentEntity.Governments[Utility.Random(GovernmentEntity.Governments.Count)]) { }

        [Constructable]
        public Soldier(Nation n, int arms, int training, GovernmentEntity gov) : this(n, arms, training, gov.Name) { }

        [Constructable]
        public Soldier(Nation n, int arms, int training, string govName)
            : base(n)
        {
            Armaments = (Armament)arms;
            Training = (Training)training;

            foreach (GovernmentEntity g in GovernmentEntity.Governments)
            {
                if (g.Name == govName)
                {
                    Government = g;
                    break;
                }
            }
            if (Government == null || Government.Deleted)
                Delete();

            Nation = Government.Nation;

            BaseName = Name;

            SpeechInterval = DateTime.Now;

            FightMode = FightMode.Aggressor;
            AI = AIType.AI_Melee;

            EquipSoldier(Nation, m_Armaments);

            if (Training > 0)
                SpecialTraining(Training);
        }

        #endregion

        #region Equipment
        public void EquipSoldier(Nation n, Armament a)
        {
            if ((int)a > 6 || (int)a < 1)
                a = (Armament)Utility.RandomMinMax(1, 4);

            switch (n)
            {
                case Nation.Southern: SetRank(1); TrainSoldier(a); SetDex(Dex + 20); EquipSouthern(a, this); break;
                case Nation.Western: SetRank(1); TrainSoldier(a); SetDex(Dex + 20); EquipWestern(a, this); break;
                case Nation.Khemetar: SetRank(1); TrainSoldier(a); SetInt(Int + 20); EquipKhemetar(a, this); break;
                case Nation.Mhordul: SetRank(1); TrainSoldier(a); SetStr(Str + 20); EquipMhordul(a, this); break;
                case Nation.Tyrean: SetRank(1); TrainSoldier(a); SetStr(Str + 20); EquipTyrean(a, this); break;
                case Nation.Northern: SetRank(1); TrainSoldier(a); SetHits(Hits + 20); EquipNorthern(a, this); break;
                case Nation.Imperial: SetRank(1); TrainSoldier(a); SetHits(Hits + 10); SetStr(Str + 10); EquipImperial(a, this); break;
                case Nation.Sovereign: TrainSoldier(a); SetStr(Str + 10); SetDex(Dex + 10); EquipSovereign(a, this); SetRank(1); break;
                case Nation.Society: SetRank(1); TrainSoldier(a); SetStr(Str + 10); SetInt(Int + 10); EquipSociety(a, this); break;
				case Nation.Insularii: SetRank(1); TrainSoldier(a); SetDex(Dex + 20); SetHits(Hits + 20); SetStr(Str + 20); SetInt(Int + 20); EquipInsularii(a, this); break;

                default: TrainSoldier(a);   EquipFreeSoldier(a, this);      break;
            }
        }

        public static void EquipSouthern(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {                            
                        Gladius sword = new Gladius();
                        sword.Resource = CraftResource.Iron;                            
                        m.EquipItem(sword);

                        StuddedChest chest = new StuddedChest();
                        chest.Resource = CraftResource.RegularLeather;
                        m.EquipItem(chest);

                            m.EquipItem(new WoodlandBelt(2605));
                            m.EquipItem(new Sandals(2605));
							
							
                        BronzeShield shield = new BronzeShield();
                        shield.Resource = CraftResource.Bronze;
                        m.EquipItem(shield);  
						
                        HalfPlateGloves arms = new HalfPlateGloves();
                        arms.Resource = CraftResource.Bronze;
                        m.EquipItem(arms);
						
                        break;
                    }
                case Armament.Medium:
                    {                        
                        Spear spear = new Spear();
                        spear.Resource = CraftResource.Iron;
                        m.EquipItem(spear);

                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Resource = CraftResource.Bronze;
						
                        NorseHelm head = new NorseHelm();
                        head.Resource = CraftResource.Bronze;

                        HalfPlateGloves arms = new HalfPlateGloves();
                        arms.Resource = CraftResource.Bronze;

                        SplintedMailGorget gorget = new SplintedMailGorget();
                        gorget.Resource = CraftResource.Bronze;

                            m.EquipItem(new WoodlandBelt(2605));
                            m.EquipItem(new Sandals(2605));


                        m.EquipItem(chest);
                        m.EquipItem(head);
                        m.EquipItem(arms);
                        m.EquipItem(gorget);
						
                        break;
                    }
                case Armament.Heavy:
                    {                            
                        WoodenKiteShield shield = new WoodenKiteShield();
                        shield.Resource = CraftResource.Bronze;
                        m.EquipItem(shield);       

                        Falcata sabre = new Falcata();
                        sabre.Resource = CraftResource.Iron;
                        m.EquipItem(sabre);

                        HalfPlateChest chest = new HalfPlateChest();
                        chest.Resource = CraftResource.Bronze;
                        m.EquipItem(chest);

                        SplintedMailLegs legs = new SplintedMailLegs();
                        legs.Resource = CraftResource.Bronze;
                        m.EquipItem(legs);

                        SplintedMailArms arms = new SplintedMailArms();
                        arms.Resource = CraftResource.Bronze;
                        m.EquipItem(arms);

                        HalfPlateGorget gorget = new HalfPlateGorget();
                        gorget.Resource = CraftResource.Bronze;
                        m.EquipItem(gorget);

                        HalfPlateGloves gloves = new HalfPlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        m.EquipItem(gloves);
						
						Sallet helmet = new Sallet();
                        helmet.Resource = CraftResource.Bronze;
                        m.EquipItem(helmet);

                        m.EquipItem(new ElegantCloak(2587));

                      m.EquipItem(new WoodlandBelt(2605));
                      m.EquipItem(new Sandals(2605));

                        break;
                    }
                case Armament.Ranged:
                    {
                        CompositeLongbow com = new CompositeLongbow();
                        com.Resource = CraftResource.Ash;
                        com.Hue = 2935;
						m.EquipItem(com);	

                        SoftLeatherTunic tunic = new SoftLeatherTunic();
                        tunic.Resource = CraftResource.Bronze;
                        m.EquipItem(tunic);		
						
                        m.EquipItem(new WoodlandBelt(2605));
                        m.EquipItem(new Sandals());

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }   
        }

        public static void EquipWestern(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {
                        BlackLeatherBoots sandals = new BlackLeatherBoots();
                        sandals.Resource = CraftResource.BeastLeather;
                        sandals.Hue = 2581;
                        m.EquipItem(sandals);

                        LeatherLegs ll = new LeatherLegs();
                        ll.Resource = CraftResource.ScaledLeather;
                        m.EquipItem(ll);

                        PlateGloves lg = new PlateGloves();
                        lg.Resource = CraftResource.Steel;
                        lg.Hue = 2581;
                        m.EquipItem(lg);
                        
                        ScaleArmorArms arm = new ScaleArmorArms();
                        arm.Hue = 2935;
                        m.EquipItem(arm);
                        
                        Robe rober = new Robe();
                        rober.Hue = 2581;
                        m.EquipItem(rober);
                        
                        Cloak cloa = new Cloak();
                        cloa.Hue = 2935;
                        m.EquipItem(cloa);
                        
                        WaistCloth cloth = new WaistCloth();
                        cloth.Hue = 2581;
                        m.EquipItem(cloth);

                        SkullMask mask = new SkullMask();
                        mask.Hue = 2935;
                        m.EquipItem(mask);

                        Kukri sword = new Kukri();
                       	
                        if (Utility.Random(100) + 1 > 99)
                            sword.Resource = CraftResource.Obsidian;
                        else
                            sword.Resource = CraftResource.Bronze;
                        m.EquipItem(sword);

                        Buckler shield = new Buckler();
                        shield.Resource = CraftResource.Bronze;
                        shield.Hue = 2935;

                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Medium:
                    {
                        BlackLeatherBoots sandals = new BlackLeatherBoots();
                        sandals.Resource = CraftResource.BeastLeather;
                        sandals.Hue = 2581;
                        m.EquipItem(sandals);

                        ScaleArmorLegs lb = new ScaleArmorLegs();
                        lb.Resource = CraftResource.Bronze;
                        lb.Hue = 2935;
                        m.EquipItem(lb);

                        PlateGloves lg = new PlateGloves();
                        lg.Resource = CraftResource.Steel;
                        lg.Hue = 2581;
                        m.EquipItem(lg);
                        
                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Hue = 2935;
                        m.EquipItem(chest);
                        
                        Cloak cloa = new Cloak();
                        cloa.Hue = 2935;
                        m.EquipItem(cloa);
                        
                        WaistCloth cloth = new WaistCloth();
                        cloth.Hue = 2581;
                        m.EquipItem(cloth);
                        

                        
                        ScaleArmorArms arm = new ScaleArmorArms();
                        arm.Hue = 2935;
                        m.EquipItem(arm);

                        SkullMask mask = new SkullMask();
                        mask.Layer = Layer.Earrings;
                        mask.Hue = 2935;
                        m.EquipItem(mask);
                        
                        Cowl cow = new Cowl();
                        cow.Hue = 2581;
                        m.EquipItem(cow);

                        Flamberge sword = new Flamberge();
                       	
                        if (Utility.Random(100) + 1 > 99)
                            sword.Resource = CraftResource.Obsidian;
                        else
                            sword.Resource = CraftResource.Bronze;
                        m.EquipItem(sword);

                        MetalShield shield = new MetalShield();
                        shield.Resource = CraftResource.Bronze;                       
                        shield.Hue = 2935;

                        m.EquipItem(shield);
                        break;
                    }
                case Armament.Heavy:
                    {
                        BlackLeatherBoots sandals = new BlackLeatherBoots();
                        sandals.Resource = CraftResource.BeastLeather;
                        sandals.Hue = 2581;
                        m.EquipItem(sandals);

                        ScaleArmorLegs lb = new ScaleArmorLegs();
                        lb.Resource = CraftResource.Bronze;
                        lb.Hue = 2581;
                        m.EquipItem(lb);

                        PlateGloves lg = new PlateGloves();
                        lg.Resource = CraftResource.Steel;
                        lg.Hue = 2581;
                        m.EquipItem(lg);
                        
                        SpikedChest chest = new SpikedChest();
                        chest.Hue = 2581;
                        m.EquipItem(chest);
                        
                        Cloak cloa = new Cloak();
                        cloa.Hue = 2935;
                        m.EquipItem(cloa);
                        
                        HalfPlateArms arm = new HalfPlateArms();
                        arm.Hue = 2581;
                        m.EquipItem(arm);
                        
                        WaistCloth cloth = new WaistCloth();
                        cloth.Hue = 2581;
                        m.EquipItem(cloth);
                        


                        SkullMask mask = new SkullMask();
                        mask.Hue = 2935;
                        m.EquipItem(mask);

                        HeavyKhopesh sword = new HeavyKhopesh();
                       	
                        if (Utility.Random(100) + 1 > 99)
                            sword.Resource = CraftResource.Obsidian;
                        else
                            sword.Resource = CraftResource.Bronze;
                        m.EquipItem(sword);


                        break;

                        break;
                    }
                case Armament.Ranged:
                    {
                        HardenedThighBoots sandals = new HardenedThighBoots();
                        sandals.Resource = CraftResource.BeastLeather;
                        sandals.Hue = 2935;
                        m.EquipItem(sandals);
                        
                        ScaleArmorLegs lb = new ScaleArmorLegs();
                        lb.Resource = CraftResource.Bronze;
                        lb.Hue = 2581;
                        m.EquipItem(lb);
                        

                        FancyGlasses fancy = new FancyGlasses();
                        fancy.Hue = 2581;
                        m.EquipItem(fancy);
                        
                        PaddedVest pad = new PaddedVest();
                        pad.Hue = 2935;
                        m.EquipItem(pad);
                        
                        LargeScarf scar = new LargeScarf();
                        scar.Hue = 2581;
                        m.EquipItem(scar);
                        
                        WaistCloth cloth = new WaistCloth();
                        cloth.Hue = 2581;
                        m.EquipItem(cloth);
                        
                        CompositeBow com = new CompositeBow();
                        com.Resource = CraftResource.Ash;
                        com.Hue = 2935;

                        m.EquipItem(com);
                        
                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;

                            if (m.Female)
                                bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }

        }

        public static void EquipKhemetar(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {
                        Sandals sand = new Sandals();
                        sand.Resource = CraftResource.BeastLeather;
                        sand.Hue = 2947;
                        m.EquipItem(sand);

                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2947;
                        m.EquipItem(chest);

                        ScaleArmorLegs legs = new ScaleArmorLegs();
                        legs.Resource = CraftResource.Bronze;
                        legs.Hue = 2947;
                        m.EquipItem(legs);

                        ScaleArmorHelmet helmet = new ScaleArmorHelmet();
                        helmet.Resource = CraftResource.Bronze;
                        helmet.Hue = 2947;
                        m.EquipItem(helmet);

                        Kukri k = new Kukri();
                        k.Resource = CraftResource.Iron;
                        m.EquipItem(k);

                        WoodenShield shield = new WoodenShield();
                        shield.Resource = CraftResource.Redwood;
                        m.EquipItem(shield);

                        if (m.Female)
                        {
                            ElegantWaistCloth waist = new ElegantWaistCloth();
                            waist.Hue = 2795;
                            m.EquipItem(waist);
                        }

                        else
                        {
                            WaistSash sash = new WaistSash();
                            sash.Hue = 2795;
                            m.EquipItem(sash);
                        }

                        break;
                    }
                case Armament.Medium:
                    {
                        ThighBoots boots = new ThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2947;
                        m.EquipItem(boots);

                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2947;
                        m.EquipItem(chest);

                        ScaleArmorLegs legs = new ScaleArmorLegs();
                        legs.Resource = CraftResource.Bronze;
                        legs.Hue = 2947;
                        m.EquipItem(legs);

                        ScaleArmorArms arms = new ScaleArmorArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2947;
                        m.EquipItem(arms);

                        RingmailGloves gloves = new RingmailGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2947;
                        m.EquipItem(gloves);

                        ScaleArmorHelmet helmet = new ScaleArmorHelmet();
                        helmet.Resource = CraftResource.Bronze;
                        helmet.Hue = 2947;
                        m.EquipItem(helmet);

                        Khopesh sword = new Khopesh();
                        sword.Resource = CraftResource.Bronze;
                        m.EquipItem(sword);

                        MetalShield shield = new MetalShield();
                        shield.Resource = CraftResource.Bronze;
                        shield.Hue = 2947;
                        m.EquipItem(shield);

                        if (m.Female)
                        {
                            ElegantWaistCloth waist = new ElegantWaistCloth();
                            waist.Hue = 2795;
                            m.EquipItem(waist);
                        }

                        else
                        {
                            WaistSash sash = new WaistSash();
                            sash.Hue = 2795;
                            m.EquipItem(sash);
                        }

                        break;
                    }
                case Armament.Heavy:
                    {
                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2947;
                        m.EquipItem(chest);

                        PlateLegs legs = new PlateLegs();
                        legs.Resource = CraftResource.Bronze;
                        legs.Hue = 2947;
                        m.EquipItem(legs);

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2947;
                        m.EquipItem(arms);

                        PlateGorget gorget = new PlateGorget();
                        gorget.Resource = CraftResource.Bronze;
                        gorget.Hue = 2947;
                        m.EquipItem(gorget);

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2947;
                        m.EquipItem(gloves);

                        ScaleArmorHelmet helmet = new ScaleArmorHelmet();
                        helmet.Resource = CraftResource.Bronze;
                        helmet.Hue = 2947;
                        m.EquipItem(helmet);

                        RunicCloak cloak = new RunicCloak();
                        cloak.Hue = 2795;
                        m.EquipItem(cloak);

                        Tabarzin axe = new Tabarzin();
                        axe.Resource = CraftResource.Bronze;
                        m.EquipItem(axe);

                        if (m.Female)
                        {
                            ElegantWaistCloth waist = new ElegantWaistCloth();
                            waist.Hue = 2795;
                            m.EquipItem(waist);
                        }

                        else
                        {
                            WaistSash sash = new WaistSash();
                            sash.Hue = 2795;
                            m.EquipItem(sash);
                        }

                        break;
                    }
                case Armament.Ranged:
                    {
                        Turban turban = new Turban();
                        turban.Resource = CraftResource.Cotton;
                        turban.Hue = 2795;
                        m.EquipItem(turban);

                        BaggyPants pants = new BaggyPants();
                        pants.Resource = CraftResource.Cotton;
                        pants.Hue = 2795;
                        m.EquipItem(pants);

                        ElegantCloak cloak = new ElegantCloak();
                        cloak.Resource = CraftResource.Cotton;
                        cloak.Hue = 2795;
                        m.EquipItem(cloak);

                        m.EquipItem(new Sandals());

                        ScaleArmorChest chest = new ScaleArmorChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2947;
                        m.EquipItem(chest);

                        ScaleArmorArms arms = new ScaleArmorArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2947;
                        m.EquipItem(arms);

                        Hijazi bow = new Hijazi();
                        bow.Resource = CraftResource.Redwood;
                        m.EquipItem(bow);

                        if (m.Female)
                        {
                            ElegantWaistCloth waist = new ElegantWaistCloth();
                            waist.Hue = 2795;
                            m.EquipItem(waist);
                        }

                        else
                        {
                            WaistSash sash = new WaistSash();
                            sash.Hue = 2795;
                            m.EquipItem(sash);
                        }


                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }
        }

        public static void EquipMhordul(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            Sandals sandals = new Sandals();
            sandals.Resource = CraftResource.BeastLeather;
            sandals.Hue = 1194;
            m.EquipItem(sandals);

            BoneArms mba = new BoneArms();
            mba.Hue = 2101;
            m.EquipItem(mba);

            switch (a)
            {
                case Armament.Light:
                    {
                        BoneHelm mbh = new BoneHelm();
                        mbh.Hue = 2101;
                        m.EquipItem(mbh);

                        StuddedLegs sl = new StuddedLegs();
                        sl.Resource = CraftResource.BeastLeather;
                        m.EquipItem(sl);

                        if (m.Female)
                        {
                            m.EquipItem(new SmallRaggedSkirt(1194));
                            m.EquipItem(new RaggedBra(1194));
                        }
                        else
                        {
                            MedicineManBoneChest bc = new MedicineManBoneChest();
                            bc.Hue = 2101;
                            m.EquipItem(bc);
                        }

                        BoneSword mbs = new BoneSword();
                        m.EquipItem(mbs);

                        BoneShield shield = new BoneShield();
                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Medium:
                    {
                        BoneHelm mbh = new BoneHelm();
                        mbh.Hue = 2101;
                        m.EquipItem(mbh);

                        BoneLegs mbl = new BoneLegs();
                        mbl.Hue = 2101;
                        m.EquipItem(mbl);

                        BoneGloves mbg = new BoneGloves();
                        mbg.Hue = 2101;
                        m.EquipItem(mbg);

                        StuddedChest chest = new StuddedChest();
                        chest.Hue = 2101;
                        m.EquipItem(chest);

                        if (m.Female)
                        {
                            m.EquipItem(new RaggedSkirt(1194));
                        }
                        else
                            m.EquipItem(new WaistCloth(1194));

                        if (Utility.RandomBool())
                            m.EquipItem(new BoneSpear());
                        else
                            m.EquipItem(new BoneScythe());

                        break;
                    }
                case Armament.Heavy:
                    {
                        MhordulHornedSkullHelm mhsh = new MhordulHornedSkullHelm();
                        mhsh.Hue = 2101;
                        m.EquipItem(mhsh);

                        BoneChest mbc = new BoneChest();
                        mbc.Hue = 2101;
                        m.EquipItem(mbc);

                        BoneLegs mbl = new BoneLegs();
                        mbl.Hue = 2101;
                        m.EquipItem(mbl);

                        BoneGloves mbg = new BoneGloves();
                        mbg.Hue = 2101;
                        m.EquipItem(mbg);

                        BoneShield mbs = new BoneShield();
                        mbs.Hue = 2101;
                        m.EquipItem(mbs);
                        
                        if(m.Female)
                            m.EquipItem(new SmallRaggedSkirt(1194));           

                        if (Utility.RandomBool())
                        {
                            WarFork mwf = new WarFork();
                            mwf.Resource = CraftResource.Iron;
                            m.EquipItem(mwf);
                        }

                        else
                            m.EquipItem(new BoneAxe());

                        break;
                    }
                case Armament.Ranged:
                    {
                        BoneHelm mbh = new BoneHelm();
                        mbh.Hue = 2101;
                        m.EquipItem(mbh);

                        m.EquipItem(new BoneBow());

                        if (m.Female)
                        {
                            m.EquipItem(new SmallRaggedSkirt(1194));
                            m.EquipItem(new RaggedBra(1194));
                        }

                        else
                            m.EquipItem(new RaggedPants(1194));

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }
        }

        public static void EquipTyrean(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            Surcoat coat = new Surcoat();
            coat.ItemID = 15477;
            coat.Name = "A Surcoat of the Jarlsgaard";
            coat.Hue = 2741;
            m.EquipItem(coat);

            switch (a)
            {
                case Armament.Light:
                    {
                        StuddedChest lc = new StuddedChest();
                        lc.Resource = CraftResource.BeastLeather;
                        lc.Hue = 1899;
                        m.EquipItem(lc);

                        StuddedLegs ll = new StuddedLegs();
                        ll.Resource = CraftResource.BeastLeather;
                        ll.Hue = 1899;
                        m.EquipItem(ll);

                        StuddedArms la = new StuddedArms();
                        la.Resource = CraftResource.BeastLeather;
                        la.Hue = 1899;
                        m.EquipItem(ll);

                        StuddedGloves lg = new StuddedGloves();
                        lg.Resource = CraftResource.BeastLeather;
                        lg.Hue = 1899;
                        m.EquipItem(lg);

                        StuddedGorget lo = new StuddedGorget();
                        lo.Resource = CraftResource.BeastLeather;
                        lo.Hue = 1899;
                        m.EquipItem(lo);

                        BearMask mask = new BearMask();
                        mask.Hue = 1899;
                        m.EquipItem(mask);

                        FurBoots boots = new FurBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2741;
                        m.EquipItem(boots);

                        ThrowingAxe tta = new ThrowingAxe();
                        tta.Resource = CraftResource.Bronze;
                        m.EquipItem(tta);

                        LeatherShield ls = new LeatherShield();
                        ls.Resource = CraftResource.Yew;
                        m.EquipItem(ls);

                        break;
                    }
                case Armament.Medium:
                    {
                        ChainChest cc = new ChainChest();
                        cc.Resource = CraftResource.Bronze;
                        cc.Hue = 1899;
                        m.EquipItem(cc);

                        ChainLegs cl = new ChainLegs();
                        cl.Resource = CraftResource.Bronze;
                        cl.Hue = 1899;
                        m.EquipItem(cl);

                        RingmailArms ra = new RingmailArms();
                        ra.Resource = CraftResource.Bronze;
                        ra.Hue = 1899;
                        m.EquipItem(ra);

                        RingmailGloves rg = new RingmailGloves();
                        rg.Resource = CraftResource.Bronze;
                        rg.Hue = 1899;
                        m.EquipItem(rg);

                        HornedHelm thh = new HornedHelm();
                        thh.Resource = CraftResource.Bronze;
                        thh.Hue = 1899;
                        m.EquipItem(thh);

                        FurBoots boots = new FurBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2741;
                        m.EquipItem(boots);

                        bool WeaponChance = Utility.RandomBool();

                        if (WeaponChance)
                        {
                            Angon weapon = new Angon();
                            weapon.Resource = CraftResource.Bronze;

                            m.EquipItem(weapon);
                        }

                        else
                        {
                            HeavyBattleAxe weapon = new HeavyBattleAxe();
                            weapon.Resource = CraftResource.Bronze;

                            m.EquipItem(weapon);
                        }

                        break;
                    }
                case Armament.Heavy:
                    {
                        HalfPlateChest thpc = new HalfPlateChest();
                        thpc.Resource = CraftResource.Bronze;
                        thpc.Hue = 1899;
                        m.EquipItem(thpc);

                        HalfPlateLegs thpl = new HalfPlateLegs();
                        thpl.Resource = CraftResource.Bronze;
                        thpl.Hue = 1899;
                        m.EquipItem(thpl);

                        HalfPlateSabatons thps = new HalfPlateSabatons();
                        thps.Resource = CraftResource.Bronze;
                        thps.Hue = 1899;
                        m.EquipItem(thps);

                        HalfPlateArms thpa = new HalfPlateArms();
                        thpa.Resource = CraftResource.Bronze;
                        thpa.Hue = 1899;
                        m.EquipItem(thpa);

                        HalfPlateGloves thpg = new HalfPlateGloves();
                        thpg.Resource = CraftResource.Bronze;
                        thpg.Hue = 1899;
                        m.EquipItem(thpg);

                        HalfPlateGorget thpo = new HalfPlateGorget();
                        thpo.Resource = CraftResource.Bronze;
                        thpo.Hue = 1899;
                        m.EquipItem(thpo);

                        DragonKiteShield tks = new DragonKiteShield();
                        tks.Resource = CraftResource.Bronze;
                        tks.Hue = 1899;
                        m.EquipItem(tks);

                        m.EquipItem(new Cloak(1445));

                        WingedHelm twh = new WingedHelm();
                        twh.Resource = CraftResource.Bronze;
                        twh.Hue = 1899;
                        m.EquipItem(twh);

                        bool WeaponChance = Utility.RandomBool();
                        if (WeaponChance)
                        {
                            BroadAxe axe = new BroadAxe();
                            axe.Resource = CraftResource.Bronze;

                            m.EquipItem(axe);
                        }

                        else
                        {
                            OrnateAxe axe = new OrnateAxe();
                            axe.Resource = CraftResource.Bronze;

                            m.EquipItem(axe);
                        }

                        break;
                    }
                case Armament.Ranged:
                    {
                        FancyShirt shirt = new FancyShirt();
                        shirt.Resource = CraftResource.Wool;
                        shirt.Hue = 1899;
                        m.EquipItem(shirt);

                        LeatherChest lc = new LeatherChest();
                        lc.Resource = CraftResource.BeastLeather;
                        lc.Hue = 1899;
                        m.EquipItem(lc);

                        LeatherLegs ll = new LeatherLegs();
                        ll.Resource = CraftResource.BeastLeather;
                        ll.Hue = 1899;
                        m.EquipItem(ll);

                        LeatherGloves lg = new LeatherGloves();
                        lg.Resource = CraftResource.BeastLeather;
                        lg.Hue = 1899;
                        m.EquipItem(lg);

                        FurBoots boots = new FurBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2741;
                        m.EquipItem(boots);

                        m.EquipItem(new Cloak(1445));

                        RecurveLongBow bow = new RecurveLongBow();
                        bow.Resource = CraftResource.Redwood;
                        m.EquipItem(bow);

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }
        }

        public static void EquipNorthern(Armament a, Mobile m)
       {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            Surcoat coat = new Surcoat();
            coat.ItemID = 15477;
            coat.Name = "A Surcoat of the Temple of the Father";
            coat.Hue = 2723;
            m.EquipItem(coat);

            switch (a)
            {
                case Armament.Light:
                    {

                        ChainChest cc = new ChainChest();
                        cc.Resource = CraftResource.Iron;
                        cc.Hue = 2985;
                        m.EquipItem(cc);

                        ChainLegs cl = new ChainLegs();
                        cl.Resource = CraftResource.Iron;
                        cl.Hue = 2101;
                        m.EquipItem(cl);

                        RingmailArms ra = new RingmailArms();
                        ra.Resource = CraftResource.Iron;
                        ra.Hue = 2101;
                        m.EquipItem(ra);

                        RingmailGloves rg = new RingmailGloves();
                        rg.Resource = CraftResource.Iron;
                        rg.Hue = 2101;
                        m.EquipItem(rg);

                        LeatherBoots boots = new LeatherBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2101;

                        m.EquipItem(boots);

                        WoodenKiteShield ws = new WoodenKiteShield();
                        ws.Resource = CraftResource.Iron;
                        m.EquipItem(ws);


                            Maul sword = new Maul();
                            sword.Resource = CraftResource.Iron;
                            m.EquipItem(sword);


                        break;
                    }
                case Armament.Medium:
                    {
                        ChainChest cc = new ChainChest();
                        cc.Resource = CraftResource.Iron;
                        cc.Hue = 1899;
                        m.EquipItem(cc);

                        ChainLegs cl = new ChainLegs();
                        cl.Resource = CraftResource.Iron;
                        cl.Hue = 1899;
                        m.EquipItem(cl);

                        NorseHelm co = new NorseHelm();
                        co.Resource = CraftResource.Iron;
                        co.Hue = 1899;
                        m.EquipItem(co);

                        HalfPlateArms ra = new HalfPlateArms();
                        ra.Resource = CraftResource.Iron;
                        ra.Hue = 1899;
                        m.EquipItem(ra);

                        HalfPlateGloves rg = new HalfPlateGloves();
                        rg.Resource = CraftResource.Iron;
                        rg.Hue = 1899;
                        m.EquipItem(rg);

                        LeatherBoots boots = new LeatherBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 1899;
                        m.EquipItem(boots);

                        Halberd hally = new Halberd();
                        hally.Resource = CraftResource.Iron;
                        m.EquipItem(hally);

                        break;
                    }
                case Armament.Heavy:
                    {
                        PlateLegs vopl = new PlateLegs();
                        vopl.Resource = CraftResource.Steel;
                        vopl.Hue = 1899;
                        m.EquipItem(vopl);

                        OrnatePlateGorget vopo = new OrnatePlateGorget();
                        vopo.Resource = CraftResource.Steel;
                        vopo.Hue = 1899;
                        m.EquipItem(vopo);

                        PlateSabatons ps = new PlateSabatons();
                        ps.Resource = CraftResource.Steel;
                        ps.Hue = 1899;
                        m.EquipItem(ps);

                        PlateChest thpc = new PlateChest();
                        thpc.Resource = CraftResource.Steel;
                        thpc.Hue = 1899;
                        m.EquipItem(thpc);

                        HalfPlateArms thpa = new HalfPlateArms();
                        thpa.Resource = CraftResource.Steel;
                        thpa.Hue = 1899;
                        m.EquipItem(thpa);

                        PlateGloves thpg = new PlateGloves();
                        thpg.Resource = CraftResource.Steel;
                        thpg.Hue = 1899;
                        m.EquipItem(thpg);

                        WingedHelm twh = new WingedHelm();
                        twh.Resource = CraftResource.Steel;
                        twh.Hue = 1899;
                        m.EquipItem(twh);

                        OrnateKiteShield voks = new OrnateKiteShield();
                        voks.Resource = CraftResource.Steel;
                        voks.Hue = 2102;
                        m.EquipItem(voks);
                       

                            HandAndAHalfSword sword = new HandAndAHalfSword();
                            sword.Resource = CraftResource.Steel;
                            m.EquipItem(sword);

                        m.EquipItem(new ElegantCloak(2751));

                        break;
                    }
                case Armament.Ranged:
                    {
                        LeatherBoots boots = new LeatherBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2101;
                        m.EquipItem(boots);

                        LeatherChest lc = new LeatherChest();
                        lc.Resource = CraftResource.RegularLeather;
                        lc.Hue = 2101;
                        m.EquipItem(lc);

                        Quiver qv = new Quiver();
                        qv.Layer = Layer.Earrings;
                        m.EquipItem(qv);

                        LongPants lp = new LongPants();
                        lp.Resource = CraftResource.Cotton;
                        lp.Hue = 2101;
                        m.EquipItem(lp);

                        LeatherArms la = new LeatherArms();
                        la.Resource = CraftResource.RegularLeather;
                        la.Hue = 2101;
                        m.EquipItem(la);

                        LeatherGloves lg = new LeatherGloves();
                        lg.Resource = CraftResource.RegularLeather;
                        lg.Hue = 2101;
                        m.EquipItem(lg);

                        RecurveLongBow bow = new RecurveLongBow();
                        bow.Resource = CraftResource.Yew;
                        m.EquipItem(bow);

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                       
                        break;
                    }
            }
        }

        public static void EquipImperial(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            Surcoat coat = new Surcoat();
            coat.ItemID = 15477;
            coat.Name = "A Surcoat of the True Church of the North";
            coat.Hue = 2751;
            m.EquipItem(coat);

            switch (a)
            {
                case Armament.Light:
                    {

                        ChainCoif co = new ChainCoif();
                        co.Resource = CraftResource.Iron;
                        co.Hue = 2101;
                        m.EquipItem(co);

                        ChainChest cc = new ChainChest();
                        cc.Resource = CraftResource.Iron;
                        cc.Hue = 2101;
                        m.EquipItem(cc);

                        ChainLegs cl = new ChainLegs();
                        cl.Resource = CraftResource.Iron;
                        cl.Hue = 2101;
                        m.EquipItem(cl);

                        RingmailArms ra = new RingmailArms();
                        ra.Resource = CraftResource.Iron;
                        ra.Hue = 2101;
                        m.EquipItem(ra);

                        RingmailGloves rg = new RingmailGloves();
                        rg.Resource = CraftResource.Iron;
                        rg.Hue = 2101;
                        m.EquipItem(rg);

                        LeatherBoots boots = new LeatherBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2101;

                        m.EquipItem(boots);

                        WoodenKiteShield ws = new WoodenKiteShield();
                        ws.Resource = CraftResource.Iron;
                        m.EquipItem(ws);


                            Maul sword = new Maul();
                            sword.Resource = CraftResource.Iron;
                            m.EquipItem(sword);

                        m.EquipItem(new Cloak(1133));

                       if (m.Female)
                        {
                            m.Body = 0x190;
                            m.Female = false;
                        m.Name = "Brother " + RandomName(Nation.Northern, m.Female) + RandomSurname(Nation.Northern, m.Female);
                        }
                        else
                        { 
                            m.Body = 0x190;
                        }

                        break;
                    }
                case Armament.Medium:
                    {
                        ChainChest cc = new ChainChest();
                        cc.Resource = CraftResource.Iron;
                        cc.Hue = 1899;
                        m.EquipItem(cc);

                        ChainLegs cl = new ChainLegs();
                        cl.Resource = CraftResource.Iron;
                        cl.Hue = 1899;
                        m.EquipItem(cl);

                        NorseHelm co = new NorseHelm();
                        co.Resource = CraftResource.Iron;
                        co.Hue = 1899;
                        m.EquipItem(co);

                        HalfPlateArms ra = new HalfPlateArms();
                        ra.Resource = CraftResource.Iron;
                        ra.Hue = 1899;
                        m.EquipItem(ra);

                        HalfPlateGloves rg = new HalfPlateGloves();
                        rg.Resource = CraftResource.Iron;
                        rg.Hue = 1899;
                        m.EquipItem(rg);

                        LeatherBoots boots = new LeatherBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 1899;
                        m.EquipItem(boots);

                        NorseHelm norse = new NorseHelm();
                        norse.Resource = CraftResource.Iron;
                        norse.Hue = 1899;
                        m.EquipItem(norse);

                        Halberd hally = new Halberd();
                        hally.Resource = CraftResource.Iron;
                        m.EquipItem(hally);

                        m.EquipItem(new Cloak(1133));

                       if (m.Female)
                        {
                            m.Body = 0x190;
                            m.Female = false;
                        m.Name = "Brother " + RandomName(Nation.Northern, m.Female) + RandomSurname(Nation.Northern, m.Female);
                        }
                        else
                        { 
                            m.Body = 0x190;
                        }

                        break;
                    }
                case Armament.Heavy:
                    {
                        OrnatePlateLegs vopl = new OrnatePlateLegs();
                        vopl.Resource = CraftResource.Iron;
                        vopl.Hue = 1899;
                        m.EquipItem(vopl);

                        OrnatePlateGorget vopo = new OrnatePlateGorget();
                        vopo.Resource = CraftResource.Iron;
                        vopo.Hue = 1899;
                        m.EquipItem(vopo);

                        PlateSabatons ps = new PlateSabatons();
                        ps.Resource = CraftResource.Iron;
                        ps.Hue = 1899;
                        m.EquipItem(ps);

                        OrnatePlateChest thpc = new OrnatePlateChest();
                        thpc.Resource = CraftResource.Iron;
                        thpc.Hue = 1899;
                        m.EquipItem(thpc);

                        OrnatePlateArms thpa = new OrnatePlateArms();
                        thpa.Resource = CraftResource.Iron;
                        thpa.Hue = 1899;
                        m.EquipItem(thpa);

                        OrnatePlateGloves thpg = new OrnatePlateGloves();
                        thpg.Resource = CraftResource.Iron;
                        thpg.Hue = 1899;
                        m.EquipItem(thpg);

                        WingedHelm twh = new WingedHelm();
                        twh.Resource = CraftResource.Iron;
                        twh.Hue = 1899;
                        m.EquipItem(twh);

                        OrnateKiteShield voks = new OrnateKiteShield();
                        voks.Resource = CraftResource.Iron;
                        voks.Hue = 2102;
                        m.EquipItem(voks);
                       

                            HandAndAHalfSword sword = new HandAndAHalfSword();
                            sword.Resource = CraftResource.Iron;
                            m.EquipItem(sword);

                        m.EquipItem(new ElegantCloak(2751));

                       if (m.Female)
                        {
                            m.Body = 0x190;
                            m.Female = false;
                        m.Name = "Brother " + RandomName(Nation.Northern, m.Female) + RandomSurname(Nation.Northern, m.Female);
                        }
                        else
                        { 
                            m.Body = 0x190;
                        }

                        break;
                    }
                case Armament.Ranged:
                    {
                        FurBoots boots = new FurBoots();
                        boots.Resource = CraftResource.RegularLeather;
                        boots.Hue = 1899;
                        m.EquipItem(boots);

                        ExpensiveHat bandana = new ExpensiveHat();
                        bandana.Hue = 1899;
                        m.EquipItem(bandana);

                        Scarf scarf = new Scarf();
                        scarf.Hue = 1899;
                        scarf.Layer = Layer.Neck;
                        m.EquipItem(scarf);

                        LeatherChest lc = new LeatherChest();
                        lc.Resource = CraftResource.RegularLeather;
                        lc.Hue = 1899;
                        m.EquipItem(lc);

                        Quiver qv = new Quiver();
                        qv.Layer = Layer.Earrings;
                        m.EquipItem(qv);

                        LeatherLegs rp = new LeatherLegs();
                        rp.Resource = CraftResource.RegularLeather;
                        rp.Hue = 1899;
                        m.EquipItem(rp);

                        LeatherGloves lg = new LeatherGloves();
                        lg.Resource = CraftResource.RegularLeather;
                        lg.Hue = 1899;
                        m.EquipItem(lg);

                        LeatherArms la = new LeatherArms();
                        lg.Resource = CraftResource.RegularLeather;
                        lg.Hue = 1899;
                        m.EquipItem(la);

                        HeavyCrossbow bow = new HeavyCrossbow();
                        bow.Resource = CraftResource.Yew;
                        m.EquipItem(bow);

                       if (m.Female)
                        {
                            m.Body = 0x190;
                            m.Female = false;
                        m.Name = "Brother " + RandomName(Nation.Northern, m.Female) + RandomSurname(Nation.Northern, m.Female);
                        }
                        else
                        { 
                            m.Body = 0x190;
                        }

                        m.EquipItem(new Cloak(1133));

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Bolt(Utility.RandomMinMax(20, 30)));
                        }

                        break;
                    }
            }
        }

        public static void EquipSovereign(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {                            
                        Claymore sword = new Claymore();
                        sword.Resource = CraftResource.Bronze;                            
                        m.EquipItem(sword);

                        StuddedChest chest = new StuddedChest();
                        chest.Resource = CraftResource.RegularLeather;
                        m.EquipItem(chest);

                        StuddedLegs legs = new StuddedLegs();
                        legs.Resource = CraftResource.RegularLeather;
                        m.EquipItem(legs);

                        if (m.Female)
                        {
                            m.EquipItem(new ElegantFemaleKilt(2587));
                            m.EquipItem(new MetallicBra());
                            m.EquipItem(new ElegantShoes());
                        }
                        else
                        {
                            m.EquipItem(new OrnateKilt(2587));
                            m.EquipItem(new Sandals());
                        }

                        break;
                    }
                case Armament.Medium:
                    {                        
                        RoundShield shield = new RoundShield();
                        shield.Resource = CraftResource.Bronze;
                        m.EquipItem(shield);

                        ArmingSword sword = new ArmingSword();
                        sword.Resource = CraftResource.Bronze;
                        m.EquipItem(sword);

                        SplintedMailChest chest = new SplintedMailChest();
                        chest.Resource = CraftResource.Bronze;

                        SplintedMailLegs legs = new SplintedMailLegs();
                        legs.Resource = CraftResource.Bronze;

                        SplintedMailArms arms = new SplintedMailArms();
                        arms.Resource = CraftResource.Bronze;

                        SplintedMailGorget gorget = new SplintedMailGorget();
                        gorget.Resource = CraftResource.Bronze;

                        HardenedFurBoots boots = new HardenedFurBoots();

                        PlainKilt kilt = new PlainKilt(2587);
                        kilt.Resource = CraftResource.Wool;

                        Beret beret = new Beret(2587);
                        beret.Resource = CraftResource.Wool;

                        m.EquipItem(chest);
                        m.EquipItem(legs);
                        m.EquipItem(arms);
                        m.EquipItem(gorget);
                        m.EquipItem(boots);
                        m.EquipItem(kilt);
                        m.EquipItem(beret);

                        break;
                    }
                case Armament.Heavy:
                    {                            
                        NotchedShield shield = new NotchedShield();
                        shield.Resource = CraftResource.Bronze;
                        m.EquipItem(shield);       

                        Falcata sabre = new Falcata();
                        sabre.Resource = CraftResource.Bronze;
                        m.EquipItem(sabre);

                        SplintedMailChest chest = new SplintedMailChest();
                        chest.Resource = CraftResource.Bronze;
                        m.EquipItem(chest);

                        PlateLegs legs = new PlateLegs();
                        legs.Resource = CraftResource.Bronze;
                        m.EquipItem(legs);

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        m.EquipItem(arms);

                        PlateGorget gorget = new PlateGorget();
                        gorget.Resource = CraftResource.Bronze;
                        m.EquipItem(gorget);

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        m.EquipItem(gloves);

                        m.EquipItem(new RunicCloak(2587));

                        if (m.Female)
                            m.EquipItem(new FemaleKilt(2587));
                        else
                            m.EquipItem(new ElegantKilt(2587));

                        break;

                    }
                case Armament.Ranged:
                    {
                        bool WeaponChance = Utility.RandomBool();
                        if (WeaponChance)
                        {
                            OrnateLongBow bow = new OrnateLongBow();
                            bow.Resource = CraftResource.Redwood;
                            m.EquipItem(bow);
                        }
                        else
                        {
                            WarBow bow = new WarBow();
                            bow.Resource = CraftResource.Redwood;
                            m.EquipItem(bow);
                        }

                        if (m.Female)
                        {
                            m.EquipItem(new ElegantKilt(2587));
                            m.EquipItem(new MetallicBra());
                        }
                        else
                            m.EquipItem(new PlainKilt(2587));

                        m.EquipItem(new Sandals());

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }
        }

        public static void EquipSociety(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {
                        LeatherChest chest = new LeatherChest();
                        chest.Resource = CraftResource.BeastLeather;
                        chest.Hue = 2830;

                        LeatherArms arms = new LeatherArms();
                        arms.Resource = CraftResource.BeastLeather;
                        arms.Hue = 2830;

                        LeatherLegs legs = new LeatherLegs();
                        legs.Resource = CraftResource.BeastLeather;
                        legs.Hue = 2830;

                        LeatherGorget gorget = new LeatherGorget();
                        gorget.Resource = CraftResource.BeastLeather;
                        gorget.Hue = 2830;

                        LeatherGloves gloves = new LeatherGloves();
                        gloves.Resource = CraftResource.BeastLeather;
                        gloves.Hue = 2830;

                        ThighBoots boots = new ThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2830;

                        m.EquipItem(chest);
                        m.EquipItem(arms);
                        m.EquipItem(legs);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);

                        Shortsword ss = new Shortsword();
                        ss.Resource = CraftResource.Bronze;
                        m.EquipItem(ss);

                        KiteShield shield = new KiteShield();
                        shield.Resource = CraftResource.Bronze;
                        shield.Name = "Society of Rymaliel Kite Shield";
                        shield.Hue = 2413;
                        shield.ItemID = 15726;
                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Medium:
                    {
                        ChainChest chest = new ChainChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2830;

                        ChainArms arms = new ChainArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2830;

                        ChainLegs legs = new ChainLegs();
                        legs.Resource = CraftResource.Bronze;
                        legs.Hue = 2830;

                        ChainGorget gorget = new ChainGorget();
                        gorget.Resource = CraftResource.Bronze;
                        gorget.Hue = 2830;

                        ChainGloves gloves = new ChainGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2830;

                        KiteShield shield = new KiteShield();
                        shield.Resource = CraftResource.Bronze;
                        shield.Name = "Society of Rymaliel Kite Shield";
                        shield.Hue = 2413;
                        shield.ItemID = 15726;
                        m.EquipItem(shield);

                        FlangedMace fm = new FlangedMace();
                        fm.Resource = CraftResource.Bronze;
                        m.EquipItem(fm);

                        ThighBoots boots = new ThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2989;

                        m.EquipItem(chest);
                        m.EquipItem(arms);
                        m.EquipItem(legs);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(shield);
                        m.EquipItem(fm);
                        m.EquipItem(boots);

                        break;
                    }
                case Armament.Heavy:
                    {
                        PlateChest chest = new PlateChest();
                        chest.Resource = CraftResource.Bronze;
                        chest.Hue = 2830;

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2830;

                        PlateLegs legs = new PlateLegs();
                        legs.Resource = CraftResource.Bronze;
                        legs.Hue = 2830;

                        PlateGorget gorget = new PlateGorget();
                        gorget.Resource = CraftResource.Bronze;
                        gorget.Hue = 2830;

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2830;

                        CloseHelm helm = new CloseHelm();
                        helm.Resource = CraftResource.Bronze;
                        helm.Hue = 2830;

                        KiteShield shield = new KiteShield();
                        shield.Resource = CraftResource.Bronze;
                        shield.Name = "Society of Rymaliel Kite Shield";
                        shield.Hue = 2413;
                        shield.ItemID = 15726;
                        m.EquipItem(shield);

                        Longsword sword = new Longsword();
                        sword.Resource = CraftResource.Iron;

                        m.EquipItem(chest);
                        m.EquipItem(arms);
                        m.EquipItem(legs);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(shield);
                        m.EquipItem(helm);
                        m.EquipItem(sword);

                        m.EquipItem(new ElegantCloak(2751));

                        break;
                    }
                case Armament.Ranged:
                    {
                        LeatherChest chest = new LeatherChest();
                        chest.Resource = CraftResource.BeastLeather;
                        chest.Hue = 2830;

                        LongPants legs = new LongPants();
                        legs.Resource = CraftResource.Wool;
                        legs.Hue = 2830;

                        LeatherGorget gorget = new LeatherGorget();
                        gorget.Resource = CraftResource.BeastLeather;
                        gorget.Hue = 2830;

                        LeatherGloves gloves = new LeatherGloves();
                        gloves.Resource = CraftResource.BeastLeather;
                        gloves.Hue = 2830;

                        ThighBoots boots = new ThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2830;

                        WingedHelm helm = new WingedHelm();
                        helm.Resource = CraftResource.Copper;

                        CompositeBow bow = new CompositeBow();
                        bow.Resource = CraftResource.Ash;

                        m.EquipItem(chest);
                        m.EquipItem(legs);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);
                        m.EquipItem(helm);
                        m.EquipItem(bow);

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
                        }

                        break;
                    }
            }

            Surcoat coat = new Surcoat();
            coat.ItemID = 15483;
            coat.Name = "A Surcoat of the Society of Rymaliel";
            m.EquipItem(coat);
        }
		
		
		public static void EquipInsularii(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            switch (a)
            {
                case Armament.Light:
                    {
                        MaleDress dress = new MaleDress();
                        dress.Name = "A Ceremonial Robe";
						dress.Layer = Layer.InnerTorso;
                        dress.Hue = 2990;

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2990;

                        ElegantDoublet doub = new ElegantDoublet();
                        doub.Name = "A Ceremonial Tunic";
                        doub.Hue = 2707;

                        Turban gorget = new Turban();
                        gorget.Hue = 2990;

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2990;

                        HardenedThighBoots boots = new HardenedThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2990;
						
						BeltPouch belt = new BeltPouch();
                        belt.Hue = 2990;	

						FancyGlasses glasses = new FancyGlasses();
                        glasses.Hue = 2707;							

                        m.EquipItem(dress);
                        m.EquipItem(arms);
                        m.EquipItem(doub);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);
						m.EquipItem(belt);
						m.EquipItem(glasses);

                        Shortsword ss = new Shortsword();
                        ss.Resource = CraftResource.Iron;
                        m.EquipItem(ss);

                        Buckler shield = new Buckler();
                        shield.Resource = CraftResource.Bronze;
                        shield.Hue = 2707;
                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Medium:
                    {
                        MaleDress dress = new MaleDress();
                        dress.Name = "A Ceremonial Robe";
						dress.Layer = Layer.InnerTorso;
                        dress.Hue = 2990;

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2990;

                        ElegantDoublet doub = new ElegantDoublet();
                        doub.Name = "A Ceremonial Tunic";
						doub.ItemID = 15502;
                        doub.Hue = 2707;

                        Turban gorget = new Turban();
                        gorget.Layer = Layer.Neck;
                        gorget.Hue = 2990;

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2990;

                        HardenedThighBoots boots = new HardenedThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2990;
						
						HornedPlateHelm helm = new HornedPlateHelm();
                        helm.Resource = CraftResource.Bronze;
						helm.Name = "A Horned Helmet";
                        helm.Hue = 2990;
						
						BeltPouch belt = new BeltPouch();
                        belt.Hue = 2990;	

						FancyGlasses glasses = new FancyGlasses();
                        glasses.Hue = 2707;							

                        m.EquipItem(dress);
                        m.EquipItem(arms);
                        m.EquipItem(doub);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);
						m.EquipItem(helm);
						m.EquipItem(belt);
						m.EquipItem(glasses);

                        Machete ss = new Machete();
                        ss.Resource = CraftResource.Iron;
                        m.EquipItem(ss);
						
						MetalShield shield = new MetalShield();
                        shield.Resource = CraftResource.Bronze;
                        shield.Hue = 2707;
                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Heavy:
                    {
                        MaleDress dress = new MaleDress();
                        dress.Name = "A Ceremonial Robe";
						dress.Layer = Layer.InnerTorso;
                        dress.Hue = 2707;
						
						HalfPlateChest chest = new HalfPlateChest();
                        chest.Name = "A Ceremonial Breastplate";
						chest.Layer = Layer.MiddleTorso;
                        chest.Hue = 2990;

                        PlateArms arms = new PlateArms();
                        arms.Resource = CraftResource.Bronze;
                        arms.Hue = 2990;

                        LargeScarf gorget = new LargeScarf();
                        gorget.Layer = Layer.Neck;
                        gorget.Hue = 2990;

                        PlateGloves gloves = new PlateGloves();
                        gloves.Resource = CraftResource.Bronze;
                        gloves.Hue = 2990;

                        HardenedThighBoots boots = new HardenedThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2990;
						
						HornedHelm helm = new HornedHelm();
                        helm.Resource = CraftResource.Bronze;
						helm.Name = "A Horned Helmet";
                        helm.Hue = 2707;
						
						BeltPouch belt = new BeltPouch();
                        belt.Hue = 2990;	
						
						FemaleLoinCloth lc = new FemaleLoinCloth();
                        lc.Hue = 2990;	
						lc.Name = "A Ceremonial Loin Cloth";

						FancyGlasses glasses = new FancyGlasses();
                        glasses.Hue = 2707;							

                        m.EquipItem(dress);
						m.EquipItem(chest);
                        m.EquipItem(arms);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);
						m.EquipItem(helm);
						m.EquipItem(belt);
						m.EquipItem(lc);
						m.EquipItem(glasses);

                        SerratedSword ss = new SerratedSword();
                        ss.Resource = CraftResource.Iron;
                        m.EquipItem(ss);

                        HeaterShield shield = new HeaterShield();
                        shield.Resource = CraftResource.Iron;
                        shield.Hue = 2990;
                        m.EquipItem(shield);

                        break;
                    }
                case Armament.Ranged:
                    {
                        MaleDress dress = new MaleDress();
                        dress.Name = "A Ceremonial Robe";
						dress.Layer = Layer.InnerTorso;
                        dress.Hue = 2990;

                        ElegantDoublet doub = new ElegantDoublet();
                        doub.Name = "A Ceremonial Tunic";
                        doub.Hue = 2707;

                        Turban gorget = new Turban();
                        gorget.Layer = Layer.Neck;
                        gorget.Hue = 2990;

                        FancyGloves gloves = new FancyGloves();
                        gloves.Hue = 2990;

                        HardenedThighBoots boots = new HardenedThighBoots();
                        boots.Resource = CraftResource.BeastLeather;
                        boots.Hue = 2990;
						
						ScaleArmorHelmet helm = new ScaleArmorHelmet();
                        helm.Resource = CraftResource.Bronze;
						helm.Name = "A Ceremonial Helmet";
                        helm.Hue = 2990;
						
						BeltPouch belt = new BeltPouch();
                        belt.Hue = 2990;	

						FancyGlasses glasses = new FancyGlasses();
                        glasses.Hue = 2707;							

                        m.EquipItem(dress);
                        m.EquipItem(doub);
                        m.EquipItem(gorget);
                        m.EquipItem(gloves);
                        m.EquipItem(boots);
						m.EquipItem(helm);
						m.EquipItem(belt);
						m.EquipItem(glasses);

                        WarBow bow = new WarBow();
                        bow.Resource = CraftResource.Oak;

                        m.EquipItem(bow);

                        if (m is BaseCreature)
                        {
                            BaseCreature bc = m as BaseCreature;
                            bc.AI = AIType.AI_Archer;
                            bc.PackItem(new Arrow(Utility.RandomMinMax(35, 55)));
                        }

                        break;
                    }
            }
        }

        public static void EquipFreeSoldier(Armament a, Mobile m)
        {
            if (a == Armament.LightCavalry)
                a = (Armament)1;
            else if (a == Armament.HeavyCavalry)
                a = (Armament)3;

            int chance = Utility.RandomMinMax(1, 6);
            Nation nation = Nation.Northern;

            switch (chance)
            {
                case 1: nation = Nation.Southern; break;
                case 2: nation = Nation.Western; break;
                case 3: nation = Nation.Khemetar; break;
                case 4: nation = Nation.Mhordul; break;
                case 5: nation = Nation.Tyrean; break;
                case 6: nation = Nation.Northern; break;
            }

            m.Language = "Common";
            m.Female = Utility.RandomBool();

            if (m.Female)
            {
                m.Body = 0x191;
                (m as Soldier).BaseName = RandomName(nation, true) + RandomSurname(nation, true);
            }

            else
            {
                m.Body = 0x190;
                (m as Soldier).BaseName = BaseKhaerosMobile.RandomName(nation, false) + RandomSurname(nation, false);
            }

            m.Hue = BaseKhaerosMobile.AssignRacialHue(nation);
            m.HairItemID = BaseKhaerosMobile.AssignRacialHair(nation, m.Female);
            int hairhue = BaseKhaerosMobile.AssignRacialHairHue(nation);
            m.HairHue = hairhue;

            if (!m.Female)
            {
                m.FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair(nation);
                m.FacialHairHue = hairhue;
            }
            else
                m.FacialHairItemID = 0;

            if (m.Backpack == null)
                m.AddItem(new Backpack());

            m.Name = ((m as Soldier).BaseName + " the Free Soldier");

            ChainChest cc = new ChainChest();
            m.EquipItem(cc);
            PlateArms pa = new PlateArms();
            m.EquipItem(pa);
            HalfPlateGloves thpg = new HalfPlateGloves();
            m.EquipItem(thpg);
            PlateGorget pg = new PlateGorget();
            m.EquipItem(pg);
            PlateLegs pl = new PlateLegs();
            m.EquipItem(pl);
            m.EquipItem(new Cloak(Utility.RandomMinMax(1873, 1908)));
            m.EquipItem(new Tunic(Utility.RandomMinMax(1873, 1908)));

            int RandomWeapons = Utility.Random(3);

            switch (RandomWeapons)
            {
                case 0: Broadsword sword = new Broadsword(); m.EquipItem(sword); MetalShield shield = new MetalShield(); m.EquipItem(shield); break;
                case 1: WarHammer wh = new WarHammer(); m.EquipItem(wh); break;
                case 2: Bow b = new Bow(); m.EquipItem(b); m.AddToBackpack(new Arrow(Utility.Random(20))); (m as BaseCreature).AI = AIType.AI_Archer; break;
            }

        }

        #endregion

        #region Soldier Rank & Training
        public void TrainSoldier(Armament a)
        {
            switch (a)
            {
                case Armament.Light:
                    {
                        SetStr(Utility.Random(120, 130));
                        SetDex(Utility.Random(160, 180));
                        SetInt(Utility.Random(65, 85));
                        SetHits(Utility.Random(125, 130));
                        SetStam(Utility.Random(90, 110));

                        SetResistance(ResistanceType.Blunt, 50);
                        SetResistance(ResistanceType.Piercing, 50);
                        SetResistance(ResistanceType.Slashing, 50);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 75;
                        SetDamage(15, 20);

                        PassiveSpeed = 0.4;
                        ActiveSpeed = 0.2;

                        PayRate = 1000;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }

                        Fame = 5000;

                        break;
                    }
                case Armament.Medium:
                    {
                        SetStr(Utility.Random(140, 160));
                        SetDex(Utility.Random(130, 150));
                        SetInt(Utility.Random(90, 110));
                        SetHits(Utility.Random(135, 155));
                        SetStam(Utility.Random(100, 125));

                        SetResistance(ResistanceType.Blunt, 60);
                        SetResistance(ResistanceType.Piercing, 60);
                        SetResistance(ResistanceType.Slashing, 60);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 90;
                        SetDamage(20, 25);

                        PassiveSpeed = 0.45;
                        ActiveSpeed = 0.25;

						PayRate = 1500;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }

                        Fame = 8000;

                        break;

                    }
                case Armament.Heavy:
                    {
                        SetStr(Utility.Random(160, 180));
                        SetDex(Utility.Random(90, 110));
                        SetInt(Utility.Random(115, 135));
                        SetHits(Utility.Random(175, 200));
                        SetStam(Utility.Random(200, 225));

                        SetResistance(ResistanceType.Blunt, 70);
                        SetResistance(ResistanceType.Piercing, 70);
                        SetResistance(ResistanceType.Slashing, 70);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 100;
                        SetDamage(25, 30);

                        PassiveSpeed = 0.5;
                        ActiveSpeed = 0.3;

                        PayRate = 2000;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }

                        Fame = 10000;

                        break;
                    }
                case Armament.Ranged:
                    {
                        SetStr(Utility.Random(120, 130));
                        SetDex(Utility.Random(150, 250));
                        SetInt(Utility.Random(90, 110));
                        SetHits(Utility.Random(100, 120));
                        SetStam(Utility.Random(100, 125));

                        SetResistance(ResistanceType.Blunt, 40);
                        SetResistance(ResistanceType.Piercing, 40);
                        SetResistance(ResistanceType.Slashing, 40);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 100;
                        SetDamage(1, 25);

                        PassiveSpeed = 0.4;
                        ActiveSpeed = 0.2;

                        PayRate = 1500;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }

                        Fame = 8000;

                        break;
                    }
                case Armament.LightCavalry:
                    {
                        SetStr(Utility.Random(140, 160));
                        SetDex(Utility.Random(130, 150));
                        SetInt(Utility.Random(90, 110));
                        SetHits(Utility.Random(135, 155));
                        SetStam(Utility.Random(100, 125));

                        SetResistance(ResistanceType.Blunt, 60);
                        SetResistance(ResistanceType.Piercing, 60);
                        SetResistance(ResistanceType.Slashing, 60);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 90;
                        SetDamage(25, 30);

                        PassiveSpeed = 0.3;
                        ActiveSpeed = 0.1;

						PayRate = 2500;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }						

                        Fame = 7000;

                        MountSoldier();

                        GiveFeat = "Riding 3";

                        break;
                    }
                case Armament.HeavyCavalry:
                    {
                        SetStr(Utility.Random(180, 200));
                        SetDex(Utility.Random(100, 120));
                        SetInt(Utility.Random(125, 135));
                        SetHits(Utility.Random(190, 200));
                        SetStam(Utility.Random(200, 225));

                        SetResistance(ResistanceType.Blunt, 70);
                        SetResistance(ResistanceType.Piercing, 70);
                        SetResistance(ResistanceType.Slashing, 70);
                        VirtualArmor = 0;

                        SetDamageType(ResistanceType.Blunt, 100);
                        CombatSkills = 100;
                        SetDamage(30, 35);

                        PassiveSpeed = 0.4;
                        ActiveSpeed = 0.2;

                        PayRate = 3500;
						
						if (Nation == Nation.Insularii )
                        {
							SetResistance( ResistanceType.Energy, 70, 70 );
							SetSkill( SkillName.MagicResist, 100, 100 );
                        }						

                        Fame = 12000;

                        MountSoldier();

                        GiveFeat = "Riding 3";

                        break;
                    }
            }
        }

        public void SpecialTraining(Training training)
        {
            switch (training)
            {
                case Training.Assault:
                    {
                        //Assault Training
                        CombatSkills = 100;

                        GiveFeat = "FlurryOfBlows 3";
                        GiveFeat = "FlashyAttack 3";

                        FavouriteStance = "FlurryOfBlows";
                        FavouriteManeuver = "FlashyAttack";

                        SetStr(Str + 50);
                        SetDex(Dex + 50);

                        PassiveSpeed -= 0.05;
                        ActiveSpeed -= 0.1;

                        if (Nation == Nation.Mhordul || Nation == Nation.Western || Nation == Nation.Insularii)
                        {
                            GiveFeat = "QuickReflexes 3";
                            GiveFeat = "BruteStrength 3";
                            SetHits(Hits + 50);
                        }

                        PayRate += 1000;

                        SetRank(2);

                        break;
                    }
                case Training.Endurance:
                    {
                        //Endurance Training
                        CombatSkills = 100;

                        GiveFeat = "DefensiveStance 3";
                        GiveFeat = "PoisonRestance 3";
                        GiveFeat = "MagicResistance 3";
                        GiveFeat = "ShieldMastery 3";

                        FavouriteStance = "DefensiveStance";

                        SetHits(Hits + 100);
                        SetStam(Stam + 100);

                        if (Nation == Nation.Tyrean || Nation == Nation.Insularii)
                        {
                            GiveFeat = "DamageIgnore 3";
                            GiveFeat = "FastHealing 3";
                            SetHits(Hits + 100);
                        }

                        PayRate += 1000;

                        SetRank(2);

                        break;
                    }
                case Training.Strategy:
                    {
                        //Strategy Training
                        CombatSkills = 100;

                        GiveFeat = "FocusedAttack 3";
                        GiveFeat = "BleedingStrike 3";
                        GiveFeat = "BruteStrength 3";

                        FavouriteStance = "FocusedAttack";
                        FavouriteManeuver = "BleedingStrike";

                        SetInt(Int + 50);
                        SetDex(Dex + 25);
                        SetMana(Mana + 50);

                        if (Nation == Nation.Khemetar || Nation == Nation.Insularii )
                        {
                            GiveFeat = "QuickReflexes 3";
                            GiveFeat = "DamageIgnore 3";
                        }

                        PayRate += 1000;

                        SetRank(2);

                        break;
                    }
                case Training.Ranged:
                    {
                        //Ranged Combat Training
                        CombatSkills = 100;

                        GiveFeat = "BowMastery 3";
                        GiveFeat = "FarShot 3";
                        GiveFeat = "SwiftShot 3";
                        GiveFeat = "CripplingShot 3";

                        FavouriteStance = "SwiftShot";
                        FavouriteManeuver = "CripplingShot";

                        SetStr(Str + 25);
                        SetDex(Dex + 25);
                        SetInt(Int + 100);

                        if (Nation == Nation.Southern || Nation == Nation.Insularii )
                        {
                            GiveFeat = "Evade 3";
                            GiveFeat = "Dodge 3";
                            GiveFeat = "EnhancedDodge 3";
                            SetDex(Dex + 25);
                        }

                        PayRate += 1000;

                        SetRank(2);

                        break;
                    }
                case Training.Dragoon:
                    {
                        //Dragoon Training
                        CombatSkills = 100;

                        GiveFeat = "MountedEndurance 3";
                        GiveFeat = "MountedDefence 3";
                        GiveFeat = "MountedCharge 3";
                        GiveFeat = "MountedCombat 3";
                        GiveFeat = "MountedMomentum 3";

                        FavouriteManeuver = "Charge";
                        FavouriteStance = "FocusedAttack";

                        SetStr(Str + 15);
                        SetHits(Hits + 50);
                        SetDex(Dex + 15);
                        SetInt(Int + 15);
                        SetStam(Stam + 50);

                        if (Nation == Nation.Northern || Nation == Nation.Insularii )
                        {
                            SetStr(Str + 50);
                            SetHits(Hits + 100);
                            VirtualArmor += 10;
                        }

                        PayRate += 1000;

                        SetRank(2);

                        break;
                    }
                default: break;
            }
        }

        public void SetRank(int rank)
        {
            switch (Nation)
            {
                case Nation.Southern:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Watchman " + BaseName; break;
                            case 2: Name = "Milite " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Western:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Keeper " + BaseName; break;
                            case 2: Name = "Keeper " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Khemetar:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Mnemeran " + BaseName; break;
                            case 2: Name = "High Mnemeran " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Mhordul:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Abaci " + BaseName; break;
                            case 2: Name = "Danji " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Tyrean:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Varnefr " + BaseName; break;
                            case 2: Name = "Varnardr " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Northern:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Guard " + BaseName; break;
                            case 2: Name = "Templar " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Imperial:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Brother " + BaseName; break;
                            case 2: Name = "Brother " + BaseName; break;
                        }
                        break;
                    }
                case Nation.Sovereign:
                    {
                        switch (Armaments)
                        {
                            case Armament.Light:
                                    switch (rank)
                                    {
                                        case 1: Name = "Newyddian " + BaseName; break;
                                        case 2: Name = "Milwyr " + BaseName; break;
                                    }
                                    break;
                            case Armament.Medium:            
                                    switch (rank)
                                    {
                                        case 1: Name = "Abaci " + BaseName; break;
                                        case 2: Name = "Danji " + BaseName; break;
                                    }
                                    break;
                            case Armament.Heavy:
                                switch (rank)
                                {
                                    case 1: Name = "Abaci " + BaseName; break;
                                    case 2: Name = "Danji " + BaseName; break;
                                }
                                break;
                            case Armament.Ranged:          
                                    switch (rank)
                                    {
                                        case 1: Name = "Newyddian " + BaseName; break;
                                        case 2: Name = "Milwyr " + BaseName; break;
                                    }
                                    break;
                            case Armament.LightCavalry:
                                switch (rank)
                                {
                                    case 1: Name = "Newyddian " + BaseName; break;
                                    case 2: Name = "Milwyr " + BaseName; break;
                                }
                                break;
                            case Armament.HeavyCavalry:
                                switch (rank)
                                {
                                    case 1: Name = "Abaci " + BaseName; break;
                                    case 2: Name = "Danji " + BaseName; break;
                                }
                                break;
                        }
                        break;
                    }
                case Nation.Society:
                    {
                        switch (rank)
                        {
                            case 1: Name = "Sentinel " + BaseName; break;
                            case 2: Name = "Defender " + BaseName; break;
                        }
                        break;
                    }
				case Nation.Insularii:
                    {
                        switch (rank)
                        {
                            case 1: Name = "a Voxi"; break;
                            case 2: Name = "a Voxi"; break;
                        }
                        break;
                    }

                default: return;
            }
        }

        public void MountSoldier()
        {
            switch (Nation)
            {
                case Nation.Southern:
                    {
                        SouthernHorse Southernhorse = new SouthernHorse();
                        Southernhorse.Rider = this;
                        break;
                    }
                case Nation.Western:
                    {
                        WesternHorse Westernhorse = new WesternHorse();
                        Westernhorse.Rider = this;
                        break;
                    }
                case Nation.Khemetar:
                    {
                        KhemetarHorse khemetarhorse = new KhemetarHorse();
                        khemetarhorse.Rider = this;
                        break;
                    }
                case Nation.Mhordul:
                    {
                        MhordulHorse mhordulhorse = new MhordulHorse();
                        mhordulhorse.Rider = this;
                        break;
                    }
                case Nation.Tyrean:
                    {
                        TyreanHorse tyreanhorse = new TyreanHorse();
                        tyreanhorse.Rider = this;
                        break;
                    }
                case Nation.Northern:
                    {
                        if (Utility.RandomBool())
                        {
                            NorthernHorse Northernhorse = new NorthernHorse();
                            Northernhorse.Rider = this;
                        }
                        else
                        {
                            WarHorse warhorse = new WarHorse();
                            warhorse.Rider = this;
                        }
                        break;
                    }
                case Nation.Imperial:
                    {
                        if (Utility.RandomBool())
                        {
                            NorthernHorse vhorse = new NorthernHorse();
                            vhorse.Rider = this;
                        }
                        else
                        {
                            TyreanHorse thorse = new TyreanHorse();
                            thorse.Rider = this;
                        }
                        break;
                    }
                case Nation.Sovereign:
                    {
                        if (Utility.RandomBool())
                        {
                            MhordulHorse mhorse = new MhordulHorse();
                            mhorse.Rider = this;
                        }
                        else
                        {
                            SouthernHorse ahorse = new SouthernHorse();
                            ahorse.Rider = this;
                        }
                        break;
                    }
                case Nation.Society:
                    {
                        if (Utility.RandomBool())
                        {
                            KhemetarHorse khorse = new KhemetarHorse();
                            khorse.Rider = this;
                        }
                        else
                        {
                            WesternHorse azhorse = new WesternHorse();
                            azhorse.Rider = this;
                        }
                        break;
                    }
                case Nation.Insularii:
                    {
                        if (Utility.RandomBool())
                        {
                            MhordulHorse imhorse = new MhordulHorse();
                            imhorse.Rider = this;
                        }
                        else
                        {
                            WarHorse iwarhorse = new WarHorse();
                            iwarhorse.Rider = this;
                        }
                        break;
                    }
            }
        }
        #endregion

        #region Mounted Soldiers - Animations and Death
        public override bool OnBeforeDeath()
        {
            if (this.Mount != null && this.Mount is BaseCreature && !(this.Mount as BaseCreature).Deleted)
            {
                BaseCreature mount = this.Mount as BaseCreature;
                mount.Kill();
            }

            return base.OnBeforeDeath();
        }

        public override void Animate(int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay, bool external)
        {
            if (this.CheckIdle() && this.Mount != null)
                return;

            base.Animate(action, frameCount, repeatCount, forward, repeat, delay, external);
        }
        #endregion

        #region CRIMINAL RECOGNITION SYSTEM

        public override void OnReceivedAttack(bool melee, bool parried, Mobile attacker)
        {
            if (this != null && !this.Deleted && attacker != null && !attacker.Deleted)
            {
                if (ControlOrder == OrderType.Stay || ControlOrder == OrderType.Stop)
                {
                    ControlOrder = OrderType.None;
                    AIObject.Action = ActionType.Combat;
                }

                if (attacker is PlayerMobile)
                {
                    PlayerMobile pm = attacker as PlayerMobile;
                    bool attack = true;

                    if (CustomGuildStone.IsGuildOfficer(pm, Government))
                    {
                        attack = false;
                    }
                    else
                    {
                        foreach (CustomGuildStone g in this.Government.AlliedGuilds)
                        {
                            if (CustomGuildStone.IsGuildOfficer(pm, g))
                            {
                                attack = false;
                                continue;
                            }
                        }
                    }
                    if(attack)
                        Soldier.RecognizeCrime(attacker, this);
                }
                else if (attacker is BaseCreature)
                {
                    if (attacker is Soldier)
                    {
                        if ((attacker as Soldier).Government != this.Government && !this.Government.AlliedGuilds.Contains((attacker as Soldier).Government))
                            Soldier.RecognizeCrime(attacker, this);
                    }
                    else if ((attacker as BaseCreature).Controlled
                        && (attacker as BaseCreature).ControlMaster != null
                        && !CustomGuildStone.IsGuildMember((attacker as BaseCreature).ControlMaster as PlayerMobile, this.Government))
                        Soldier.RecognizeCrime((attacker as BaseCreature).ControlMaster, this);
                    else if (attacker is BaseCreature)
                        Soldier.RecognizeCrime(attacker, this);
                }
            }

            base.OnReceivedAttack(melee, parried, attacker);
        }

        public static bool VisionCheck(Mobile m, BaseCreature s, int range) // A check to see if the seen mobile meets all the conditions for identifying criminal activity.
        {
            if ((m.Combatant != s) && s.CanSee(m) && s.InLOS(m) && (m.AccessLevel < AccessLevel.Counselor))
            {
                if (m.Hidden)
                    return (s.InFieldOfVision(m) && m.InRange(s, range));
                else
                    return m.InRange(s, range);
            }
            else
                return false;
        }

        public static String CriminalAlertMessage(Nation n)
        {
            String alertMessage = "The authorities have been alerted of your deeds!";

            switch (n)
            {
                case Nation.Southern:    alertMessage = "The Southerners have been alerted of your deeds!";     break;
                case Nation.Western:    alertMessage = "The Keepers have been alerted of your deeds!";     break;
                case Nation.Khemetar:   alertMessage = "The Khemetar have been alerted of your deeds!";     break;
                case Nation.Mhordul:    alertMessage = "The Mhordul have been alerted of your deeds!";      break;
                case Nation.Tyrean:     alertMessage = "The Tyreans have been alerted of your deeds!";      break;
                case Nation.Northern:  alertMessage = "The Northerners have been alerted of your deeds!";   break;
                case Nation.Imperial:   alertMessage = "The Northerners have been alerted of your deeds!";        break;
                case Nation.Sovereign:  alertMessage = "The Sovereign have been alerted of your deeds!";    break;
                case Nation.Society:    alertMessage = "The Society has been alerted of your deeds!";       break;
				case Nation.Insularii:  alertMessage = "The Insularii has been alerted of your deeds!";       break;
            }

            return alertMessage;
        }

        public static void RecognizeCrime(Mobile m, Soldier s)
        {
            if (m.Deleted || m == null || !m.Alive)
                return;

            if (s.Deleted || s == null || !s.Alive)
                return;

            if (s.Government == null || s.Government.Deleted)
                return;

            if (m is PlayerMobile && CustomGuildStone.IsGuildOfficer(m as PlayerMobile, s.Government))
                return;

            if (m is Soldier && (m as Soldier).Government != null && !(m as Soldier).Government.Deleted && (m as Soldier).Government == s.Government)
                return;

            if (m is PlayerMobile && GroupInfo.IsGroupLeader(s, m as PlayerMobile))
                return;

            XmlAttachment attachment = null;
            attachment = XmlAttach.FindAttachmentOnMobile(m, typeof(XmlCriminal), s.Nation.ToString());

            if (attachment == null)
            {
                XmlAttach.AttachTo(m, new XmlCriminal(s));
                
                m.SendMessage(CriminalAlertMessage(s.Nation));

                if (m is PlayerMobile)
                {
                    if (((PlayerMobile)m).CriminalActivity)
                    {
                        ((PlayerMobile)m).CriminalActivity = false;
                        ReportInfo newReport = new ReportInfo(m, true, false);
                        newReport.ReporterName = s.Name;
                        s.ReportTimer = new ReportTimer(newReport, s);
                    }
                    else
                    {
                        ReportInfo newReport = new ReportInfo(m, false, true);
                        newReport.ReporterName = s.Name;
                        s.ReportTimer = new ReportTimer(newReport, s);
                    }
                    
                    s.ReportTimer.Start();
                }

                m.RevealingAction();
                s.OnThink();
            }
        }

        /*
        public static bool AssaultCheck(Mobile m, Soldier s)
        {
            if (m == null || m.Deleted || s == null || s.Deleted || s.Government == null || s.Government.Deleted || !CustomGuildStone.Guilds.Contains(s.Government))
                return false;            
            
            if (m.AccessLevel > AccessLevel.Player) // Don't attack staff!
                return false;
                        
            // Is this a controlled pet that is committing the assault? Then we'll recognize its master as the true enemy.
            if ((m is BaseCreature) && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster != null && !((BaseCreature)m).ControlMaster.Deleted)
                m = ((BaseCreature)m).ControlMaster;

            if (s.ControlOrder != null && s.ControlOrder != OrderType.Stop && m.Combatant == null)
            {
                if (m is Soldier && (m as Soldier).Government != null && !(m as Soldier).Government.Deleted)
                {
                    if (s.Government != null && !s.Government.Deleted && !s.Government.MilitaryPolicies.Exceptions.Contains(m.Name))
                    {
                        if (s.Government.EnemyGuilds.Contains((m as Soldier).Government))
                            return true;
                    }
                }
                else if (m is PlayerMobile)
                {
                    if (s.Government != null && !s.Government.Deleted && !s.Government.MilitaryPolicies.Exceptions.Contains(m.Name))
                    {
                        foreach (CustomGuildStone enemy in s.Government.EnemyGuilds)
                        {
                            if (CustomGuildStone.IsGuildMember(m as PlayerMobile, enemy) && (m as PlayerMobile).CustomGuilds[enemy].ActiveTitle)
                                return true;
                        }
                    }
                }
            }

            if (m.Combatant == null)
                return false;
            
            if (m.Combatant != null && !m.Combatant.Deleted && s.ControlOrder != null && s.ControlOrder != OrderType.Stop)
            {
                Mobile combatant = m.Combatant;

                if (combatant == null) //If the combatant isn't in combat, then there's nothing to do here.
                    return false;
                else if (combatant is PlayerMobile)
                {
                    PlayerMobile attackedMobile = (PlayerMobile)combatant; // Casting the person you're attacking as a Player.

                    if ( (attackedMobile is PlayerMobile && attackedMobile.Spar) || ( m is PlayerMobile && ((PlayerMobile)m).Spar) ) // Are you guys just sparring? Aw, heck, go on ahead then.
                        return false;

                    if (s.Government != null && !s.Government.Deleted)
                    {
                        #region Enemy Check

                        if (!s.Government.MilitaryPolicies.Exceptions.Contains(m.Name))
                        {
                            // Are one of you in an enemy guild?
                            int enemyCount = s.Government.EnemyGuilds.Count;
                            for (int i = 0; i < enemyCount; i++) // Cycling through my government's list of enemy guilds...
                            {
                                CustomGuildStone thisGuild = (CustomGuildStone)s.Government.EnemyGuilds[i]; // Here's one!

                                if (m is PlayerMobile)
                                {
                                    if (CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild) && (m as PlayerMobile).CustomGuilds[thisGuild].ActiveTitle) // Are you in this enemy guild?
                                        return true; // If so, I'll attack you!
                                    else if (CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild) && !(attackedMobile).Disguised && (attackedMobile).CustomGuilds[thisGuild].ActiveTitle) // Is the defender in this enemy guild?
                                        return false; // If so, I won't bother you!
                                }
                                else if (m is BaseCreature)
                                {
                                    if (((BaseCreature)m).Government == thisGuild) // Are you in this enemy government?
                                        return true; // If so, I'll attack you!
                                    if (((BaseCreature)m).Organization == thisGuild) // Are you in this enemy guild?
                                        return true; // If so, I'll attack you!
                                    else if (attackedMobile is PlayerMobile && CustomGuildStone.IsGuildMember(attackedMobile, thisGuild)) // Is the defender in this enemy guild?
                                        return false; // Then I'll leave you be.
                                }
                            }
                        }
                        #endregion

                        #region Government Check
                        if (CustomGuildStone.IsGuildMember(attackedMobile, s.Government)) // Are you attacking someone who is in my government?
                        {
                            if ( (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government) && (m as PlayerMobile).CustomGuilds[s.Government].ActiveTitle && !(m as PlayerMobile).Disguised) || (m is BaseCreature && (((BaseCreature)m).Government == s.Government))) // Are you in my government?
                            {
                                // Are you (an NPC) attacking a high-ranking citizen?
                                if ((attackedMobile.CustomGuilds[s.Government].RankInfo.Rank > 1) && (m is BaseCreature))
                                    return true; // Then I'll attack you!
                                else if ((m is Soldier) && ((BaseCreature)m).Government == s.Government) // Are you you a soldier of my government attacking someone of low rank?
                                    return false; // Go right ahead!

                                // Are you (a player) attacking someone whom you outrank?
                                if (m is PlayerMobile && CustomGuildStone.Outranks((PlayerMobile)m, (PlayerMobile)attackedMobile, (CustomGuildStone)s.Government))
                                    return false; // Then I won't attack you.
                                else // Or do they outrank you?
                                    return true; // Then I -will- attack you!
                            }
                            else // Well, are you an ally of my government?
                            {
                                int num = s.Government.AlliedGuilds.Count;
                                for (int i = 0; i < num; i++) // Let me look at the list of allied guilds.
                                {
                                    CustomGuildStone thisGuild = (CustomGuildStone)s.Government.AlliedGuilds[i]; // Here's one!

                                    //m is PlayerMobile
                                    if (m is PlayerMobile && (CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild) && (m as PlayerMobile).CustomGuilds[thisGuild].ActiveTitle && !(m as PlayerMobile).Disguised)) // Are you in an allied guild?
                                    {
                                        int attackerRank = ((PlayerMobile)m).CustomGuilds[thisGuild].RankInfo.Rank; // Well, what's your rank?
                                        int defenderRank = ((PlayerMobile)attackedMobile).CustomGuilds[s.Government].RankInfo.Rank; // And what's the defender's?

                                        if (attackerRank > defenderRank) // If you outrank the defender, I won't attack you.
                                            return false;
                                        else
                                            return true; // But I will if you don't outrank the person you're attacking!
                                    }
                                }
                            }

                            // You are attacking someone in my government and you're neither in my government nor an ally. Have at thee!
                            return true;
                        }

                        #endregion

                        #region Ally Check

                        //Are you attacking someone who is an ally of my government?
                        int allyCount = s.Government.AlliedGuilds.Count;
                        for (int i = 0; i < allyCount; i++) // Cycling through my government's allied guilds...
                        {
                            CustomGuildStone thisGuild = (CustomGuildStone)s.Government.AlliedGuilds[i]; // Here's one!

                            if (attackedMobile is PlayerMobile && CustomGuildStone.IsGuildMember(attackedMobile as PlayerMobile, thisGuild)
                                && attackedMobile.CustomGuilds[thisGuild].ActiveTitle && !attackedMobile.Disguised) // Is the person you're attacking in that allied guild?
                            {
                                // They are, but you're part of my government!
                                if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government))
                                    return false;
                                // They are, but you're part of my government and they are of low rank.
                                if (m is BaseCreature && ((BaseCreature)m).Government == s.Government && attackedMobile.CustomGuilds[thisGuild].RankInfo.Rank <= 1)
                                    return false;

                                // They are, but you outrank them in that guild.
                                if (m is PlayerMobile && CustomGuildStone.Outranks((PlayerMobile)m, (PlayerMobile)attackedMobile, (CustomGuildStone)s.Government))
                                    return false;

                                //Are you ALSO part of an allied guild, though???
                                int otherAlly = s.Government.AlliedGuilds.Count;
                                for (int q = 0; q < otherAlly; q++) // Cycling through my allied guilds again...
                                {
                                    CustomGuildStone otherGuild = (CustomGuildStone)s.Government.AlliedGuilds[q]; // Here's one!

                                    if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, otherGuild)) // Are you in that other allied guild?
                                    {
                                        int attackerRank = ((PlayerMobile)m).CustomGuilds[otherGuild].RankInfo.Rank; // Well, what's your rank?
                                        int defenderRank = ((PlayerMobile)attackedMobile).CustomGuilds[thisGuild].RankInfo.Rank; // And what's the defender's?

                                        if (attackerRank > defenderRank) // If you outrank the defender, I won't attack you.
                                            return false;
                                    }
                                    // Are you an NPC of the allied guild?
                                    else if ((m is BaseCreature) && (((BaseCreature)m).Government == otherGuild || ((BaseCreature)m).Organization == otherGuild))
                                    {
                                        int defenderRank = ((PlayerMobile)attackedMobile).CustomGuilds[thisGuild].RankInfo.Rank; // What's the defender's rank?

                                        if (defenderRank <= 1) // If the defender is of low rank, you can attack him/her, no problem.
                                            return false;
                                        else // If the defender is of high rank, I'll attack you.
                                            return true;
                                    }
                                }

                                // You aren't in my govt, you don't outrank them in their guild, and you aren't of an equal or higher rank in an allied guild.
                                // You're dead!
                                return true;
                            }
                        }
                        #endregion
                    }

                    #region Nation Check

                    if (attackedMobile.Nation == s.Nation || (attackedMobile is PlayerMobile && attackedMobile.Disguised && attackedMobile.GetDisguisedNation() == s.Nation)) // If you're attacking someone with the same nation as I...
                    {
                        if (((m is PlayerMobile && ((PlayerMobile)m).Nation != s.Nation) && ((PlayerMobile)m).GetDisguisedNation() != s.Nation) || (m is BaseCreature && ((BaseCreature)m).Nation != s.Nation)) // And you aren't of the same nation as I...
                        {
                            if ((m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government)) || (m is BaseCreature && ((BaseCreature)m).Government != s.Government)) // And you aren't in my government...
                            {
                                int count = s.Government.AlliedGuilds.Count;
                                for (int i = 0; i < count; i++) // Are you in a guild allied to my government? Let me check...
                                {
                                    CustomGuildStone thisGuild = (CustomGuildStone)s.Government.AlliedGuilds[i];

                                    if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild)) // Well, are you???
                                    {
                                        if (((PlayerMobile)m).CustomGuilds[thisGuild].RankInfo.Rank > 1) // You are? And you're of high rank?
                                            if (((PlayerMobile)m).CustomGuilds[thisGuild].ActiveTitle)
                                                return false; // Then I won't attack.
                                    }
                                    else if ((m is BaseCreature) && ((BaseCreature)m).Government == thisGuild) // Are you an NPC of an allied guild?
                                    {
                                        return false; // Then I won't attack you.
                                    }
                                }

                                // You're attacking someone of my nation and you aren't of the same nation. Turns out you aren't in an allied
                                // organization or you didn't have a high enough rank. I'l attack you!
                                return true;
                            }
                        }
                    }

                    #endregion
                }
                else if (combatant is BaseCreature)
                {
                    BaseCreature attackedMobile = (BaseCreature)combatant;

                    if (s.Government != null && !s.Government.Deleted)
                    {
                        #region Enemy Check
                        if (!s.Government.MilitaryPolicies.Exceptions.Contains(m.Name))
                        {
                            // Are one of you in an enemy guild?
                            int enemyCount = s.Government.EnemyGuilds.Count;
                            foreach (CustomGuildStone thisGuild in s.Government.EnemyGuilds) // Cycling through my government's list of enemy guilds...
                            {
                                if (m is PlayerMobile)
                                {
                                    if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild)) // Are you in this enemy guild?
                                    {
                                        if (!(m as PlayerMobile).Disguised && (m as PlayerMobile).CustomGuilds[thisGuild].ActiveTitle)
                                            return true; // If so, I'll attack you!
                                    }
                                    else if (attackedMobile.Government == thisGuild) // Is the defender in this enemy guild?
                                        return false; // If so, I won't bother you!
                                }
                                else if (m is BaseCreature)
                                {
                                    if (((BaseCreature)m).Government == thisGuild) // Are you in this enemy government?
                                        return true; // If so, I'll attack you!
                                    if (((BaseCreature)m).Organization == thisGuild) // Are you in this enemy guild?
                                        return true; // If so, I'll attack you!
                                    else if (attackedMobile.Government == thisGuild) // Is the defender in this enemy guild?
                                        return false; // Then I'll leave you be.
                                }
                            }
                        }
                        #endregion

                        #region Government Check

                        if (attackedMobile.Government == s.Government) // Are you attacking someone who is in my government?
                        {
                            if( (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government) ) || ((m is BaseCreature) && (((BaseCreature)m).Government == s.Government))) // Are you in my government?
                            {
                                // Are you ( a player ) of high enough rank to attack an NPC of the government?
                                if (m is PlayerMobile && ((PlayerMobile)m).CustomGuilds[s.Government].RankInfo.Rank > 1)
                                    if(!(m as PlayerMobile).Disguised && (m as PlayerMobile).CustomGuilds[s.Government].ActiveTitle)
                                        return false; // You are; go ahead.
                                else if (m is PlayerMobile && ((PlayerMobile)m).CustomGuilds[s.Government].RankInfo.Rank <= 1)
                                    return true; // You are not of high enough rank! Have at thee!

                                // Are you ( an NPC ) of high enough rank to attack an NPC of the government?
                                if (m is Soldier)
                                    return false; // If you're a soldier, carry on with your work
                                else
                                    return true; // Otherwise, you're doing something only soldiers can do!
                            }
                            else // Well, are you an ally of my government?
                            {
                                foreach (CustomGuildStone thisGuild in s.Government.AlliedGuilds) // Let me look at the list of allied guilds.
                                {
                                    //m is PlayerMobile
                                    if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild)) // Are you in an allied guild?
                                    {
                                        if (!(m as PlayerMobile).Disguised && (m as PlayerMobile).CustomGuilds[thisGuild].ActiveTitle)
                                        {
                                            int attackerRank = ((PlayerMobile)m).CustomGuilds[thisGuild].RankInfo.Rank; // Well, what's your rank?

                                            if (attackerRank > 1) // If you outrank the defender, I won't attack you.
                                                return false;
                                            else
                                                return true; // But I will if you don't outrank the person you're attacking!
                                        }
                                    }
                                    else if ((m is BaseCreature) && ((BaseCreature)m).Government == thisGuild) // Are you an NPC of that allied guild?
                                    {
                                        if ((m is Soldier) && !(attackedMobile is Soldier)) //Are you a soldier attacking a non-military unit of my govt?
                                            return false; //If so, proceed.
                                        else
                                            return true; //If not, I'll attack!
                                    }
                                }
                            }

                            // You are attacking someone in my government and you're neither in my government nor an ally. Have at thee!
                            return true;
                        }

                        #endregion

                        #region Ally Check

                        //Are you attacking someone who is an ally of my government?
                        foreach(CustomGuildStone thisGuild in s.Government.AlliedGuilds) // Cycling through my government's allied guilds...
                        {
                            if (attackedMobile.Government == thisGuild) // Is the person you're attacking in that allied guild?
                            {
                                // They are, but you're part of my government!
                                if ((m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government)
                                    && !(m as PlayerMobile).Disguised 
                                    && (m as PlayerMobile).CustomGuilds[thisGuild].ActiveTitle) || 
                                        (m is BaseCreature 
                                        && ((BaseCreature)m).Government == s.Government))
                                    return false;

                                // They are, but you're in the same guild.
                                if ((m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild)) || ((m is BaseCreature && ((BaseCreature)m).Government == thisGuild)))
                                    return false;

                                //Are you ALSO part of an allied guild, though???
                                foreach (CustomGuildStone otherGuild in s.Government.AlliedGuilds) // Cycling through my allied guilds again...
                                {
                                    if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, otherGuild) && !(m as PlayerMobile).Disguised && (m as PlayerMobile).CustomGuilds[otherGuild].ActiveTitle) // Are you in that other allied guild?
                                        return false; // Then I won't interfere.

                                    // Are you an NPC of the allied guild?
                                    else if ((m is BaseCreature && (((BaseCreature)m).Government == otherGuild || ((BaseCreature)m).Organization == otherGuild)))
                                        return false; // You are, and so I won't interfere.
                                }

                                // You aren't in my govt, you don't outrank them in their guild, and you aren't of an equal or higher rank in an allied guild.
                                // You're dead!
                                return true;
                            }
                        }
                        #endregion
                    }

                    #region Nation Check

                    if (attackedMobile.Nation == s.Nation) // If you're attacking someone with the same nation as I...
                    {
                        if (((m is PlayerMobile && ((PlayerMobile)m).Nation != s.Nation) && ((PlayerMobile)m).GetDisguisedNation() != s.Nation) || (m is BaseCreature && ((BaseCreature)m).Nation != s.Nation)) // And you aren't of the same nation as I...
                        {
                            if (s.Government != null && !s.Government.Deleted)
                            {
                                if ((m is PlayerMobile && !CustomGuildStone.IsGuildMember(m as PlayerMobile, s.Government)) || (m is BaseCreature && ((BaseCreature)m).Government != s.Government)) // And you aren't in my government...
                                {
                                    foreach(CustomGuildStone thisGuild in s.Government.AlliedGuilds) // Are you in a guild allied to my government? Let me check...
                                    {
                                        if (m is PlayerMobile && CustomGuildStone.IsGuildMember(m as PlayerMobile, thisGuild)) // Well, are you???
                                        {
                                            if (((PlayerMobile)m).CustomGuilds[thisGuild].RankInfo.Rank > 1) // You are? And you're of high rank?
                                                if(((PlayerMobile)m).CustomGuilds[thisGuild].ActiveTitle)
                                                    return false; // Then I won't attack.
                                        }
                                        else if ((m is BaseCreature) && ((BaseCreature)m).Government == thisGuild) // Are you an NPC of an allied guild?
                                        {
                                            return false; // Then I won't attack you.
                                        }
                                    }

                                    // You're attacking someone of my nation and you aren't of the same nation. Turns out you aren't in an allied
                                    // organization or you didn't have a high enough rank. I'l attack you!
                                    return true;
                                }
                            }

                            // I have no government and you're attacking someone of my nation; so, I'll attack you!
                            return true;
                        }

                        return true;
                    }

                    #endregion

                }
            }

            return false;
        }
        */
        public static bool AssaultCheck(Mobile m, Soldier s)
        {
            if (m == null || m.Deleted)
                return false;
            if (!m.Alive || m.IsDeadBondedPet)
                return false;
            if (s == null || s.Deleted )
                return false;
            if (!s.Alive)
                return false;
            if (s.Government == null || s.Government.Deleted)
                return false;
            if (s.ControlOrder == OrderType.Stop)
                return false;

            Soldier soldier = s;
            bool bValid = false;
            XmlAttachment attachment = null;
            attachment = XmlAttach.FindAttachment(m, typeof(XmlCriminal), soldier.Nation.ToString());

            #region Enemy Guild Handling
            if (Soldier.VisionCheck(m, soldier, 16) && !soldier.Government.MilitaryPolicies.Exceptions.Contains(m.Name))
            {
                soldier.DebugSay("You are not on the Exceptions list.");

                if (m is BaseKhaerosMobile && (m as BaseKhaerosMobile).Government != null && !(m as BaseKhaerosMobile).Deleted)
                {
                    soldier.DebugSay("You are a soldier too.");
                    if (soldier.Government.EnemyGuilds.Contains((m as BaseKhaerosMobile).Government))
                        bValid = true;
                }
                else if (m is PlayerMobile)
                {
                    soldier.DebugSay("You are a player.");
                    foreach (CustomGuildStone enemyGuild in soldier.Government.EnemyGuilds)
                    {
                        if (CustomGuildStone.IsGuildMember(m as PlayerMobile, enemyGuild) && (m as PlayerMobile).CustomGuilds[enemyGuild].ActiveTitle)
                        {
                            soldier.DebugSay("You are part of an enemy guild!");
                            bValid = true;
                            continue;
                        }
                    }
                }
            }
            #endregion

            if (Soldier.VisionCheck(m, soldier, 15) && !bValid && attachment == null && m.Combatant != null && !m.Combatant.Deleted && m.Combatant.Alive)
            {
                soldier.DebugSay("I can see you!");
                if (!(m is PlayerMobile && CustomGuildStone.IsGuildOfficer(m as PlayerMobile, soldier.Government)))
                {
                    Mobile attacker = m;

                    #region The defender is a controlled creature.
                    if (attacker.Combatant is BaseCreature && (attacker.Combatant as BaseCreature).Controlled && (attacker.Combatant as BaseCreature).ControlMaster != null && !(attacker.Combatant as BaseCreature).ControlMaster.Deleted && (attacker.Combatant as BaseCreature).ControlMaster is PlayerMobile)
                    {
                        PlayerMobile defender = (attacker.Combatant as BaseCreature).ControlMaster as PlayerMobile;
                        if (!defender.Spar) // If the defender isn't sparring...
                        {
                            soldier.DebugSay("You are not sparring.");
                            #region Is the defender in an enemy guild?
                            bool defenderIsEnemy = false;
                            foreach (CustomGuildStone enemyGuild in soldier.Government.EnemyGuilds)
                            {
                                if (CustomGuildStone.IsGuildMember(defender, enemyGuild) && defender.CustomGuilds[enemyGuild].ActiveTitle)
                                {
                                    soldier.DebugSay("The person you are attacking is in an enemy guild!");
                                    defenderIsEnemy = true; // The defender is part of an enemy guild; that's no reason to defend them!
                                    continue;
                                }
                            }
                            #endregion

                            if (!defenderIsEnemy)
                            {
                                #region Ally Handling
                                foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                {
                                    if (!(attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government))) // If the attacker isn't a member of my guild...
                                    {
                                        if (CustomGuildStone.IsGuildMember(defender, allyGuild) && defender.CustomGuilds[allyGuild].ActiveTitle) // ... and the defender is an ally ... 
                                        {
                                            soldier.DebugSay("The defender is a player ally!");
                                            bool attackerIsAlly = false;
                                            foreach (CustomGuildStone otherAllyGuild in soldier.Government.AlliedGuilds)
                                            {
                                                if (attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, otherAllyGuild)) // Is the attacker ALSO an ally?
                                                {
                                                    soldier.DebugSay("You are an ally.");
                                                    attackerIsAlly = true; // The attacker is an ally as well; I can't decide!
                                                    continue;
                                                }
                                                else if (attacker is Soldier && (attacker as Soldier).Government != null && (attacker as Soldier).Government.Deleted && (attacker as Soldier).Government == otherAllyGuild) // Is the attacker ALSO an ally?
                                                {
                                                    soldier.DebugSay("You are an ally.");
                                                    attackerIsAlly = true; // The attacker is an ally as well; I can't decide!
                                                    continue;
                                                }
                                            }

                                            if (!attackerIsAlly) // The attacker is not an ally.
                                            {
                                                soldier.DebugSay("You are not an ally, I will attack!");
                                                bValid = true; // The defender is an ally! I'll defend him from this unknown.
                                                continue;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Government Handling
                                if (!bValid) // So the defender wasn't an ally, nor was the attacker...
                                {
                                    if (CustomGuildStone.IsGuildMember(defender, soldier.Government) && defender.CustomGuilds[soldier.Government].ActiveTitle) // Is the defender a member of my government?
                                    {
                                        soldier.DebugSay("The defender is a member of my government.");
                                        if (attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government)) // Yes, and so is the attacker!
                                        {
                                            soldier.DebugSay("You are a member of my guild!");
                                            if ((attacker as PlayerMobile).CustomGuilds[soldier.Government].RankID < defender.CustomGuilds[soldier.Government].RankID) // So who outranks who? I'm with the authority on this one!  
                                                bValid = true; // Both are members of my government, but the defender outranks the attacker; defend the high-ranking member!
                                        }
                                        else
                                        {
                                            soldier.DebugSay("The defender is in my government, but you are neither in my government nor an ally. I will attack you.");
                                            bValid = true; // The defender is in my government, but the attacker is neither an ally nor in my government; therefore, I will attack him.
                                        }
                                    }
                                }
                                #endregion

                                #region Nation Handling

                                if (!bValid && !(attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government))) // If the attacker is not an officer of my guild...
                                {
                                    #region Checking to make certain the attacker is not an ally.
                                    bool attackerIsAlly = false;
                                    if (attacker is PlayerMobile || attacker is Soldier)
                                    {
                                        if (attacker is Soldier && (attacker as Soldier).Government != null && (attacker as Soldier).Government.Deleted && (attacker as Soldier).Government == soldier.Government)
                                        {
                                            soldier.DebugSay("The attacker is a soldier and an ally!");
                                            attackerIsAlly = true;
                                        }
                                        else
                                        {
                                            foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                            {
                                                if (attacker is PlayerMobile)
                                                {
                                                    if (CustomGuildStone.IsGuildMember(attacker as PlayerMobile, allyGuild))
                                                    {
                                                        soldier.DebugSay("You are an ally!");
                                                        attackerIsAlly = true;
                                                        continue;
                                                    }
                                                }
                                                else if (attacker is Soldier && (attacker as Soldier).Government != null && !(attacker as Soldier).Government.Deleted)
                                                {
                                                    if ((attacker as Soldier).Government == allyGuild)
                                                    {
                                                        soldier.DebugSay("You are an ally!");
                                                        attackerIsAlly = true;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (!attackerIsAlly && defender.GetDisguisedNation() == soldier.Government.Nation) // ... and the defender is of my own nation ...
                                    {
                                        soldier.DebugSay("The attacker is not an ally, and the defender is of my nation.");
                                        if (attacker is PlayerMobile)
                                        {
                                            if ((attacker as PlayerMobile).GetDisguisedNation() != soldier.Government.Nation)
                                            {
                                                soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                bValid = true;
                                            }
                                        }
                                        else
                                        {
                                            if (attacker is Soldier)
                                            {
                                                if ((attacker as Soldier).Nation != soldier.Government.Nation)
                                                {
                                                    soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                    bValid = true;
                                                }
                                            }
                                            else
                                            {
                                                soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                bValid = true;
                                            }
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    #endregion

                    #region The defender is a player.
                    else if (attacker.Combatant is PlayerMobile)
                    {
                        soldier.DebugSay("You are a player.");
                        PlayerMobile defender = attacker.Combatant as PlayerMobile;
                        if (!defender.Spar) // If the defender isn't sparring...
                        {
                            soldier.DebugSay("You are not sparring.");
                            #region Is the defender in an enemy guild?
                            bool defenderIsEnemy = false;
                            foreach (CustomGuildStone enemyGuild in soldier.Government.EnemyGuilds)
                            {
                                if (CustomGuildStone.IsGuildMember(defender, enemyGuild) && defender.CustomGuilds[enemyGuild].ActiveTitle)
                                {
                                    soldier.DebugSay("The person you are attacking is in an enemy guild!");
                                    defenderIsEnemy = true; // The defender is part of an enemy guild; that's no reason to defend them!
                                    continue;
                                }
                            }
                            #endregion

                            if (!defenderIsEnemy)
                            {
                                #region Ally Handling
                                foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                {
                                    if (!(attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government))) // If the attacker isn't a member of my guild...
                                    {
                                        if (CustomGuildStone.IsGuildMember(defender, allyGuild) && defender.CustomGuilds[allyGuild].ActiveTitle) // ... and the defender is an ally ... 
                                        {
                                            soldier.DebugSay("The defender is a player ally!");
                                            bool attackerIsAlly = false;
                                            foreach (CustomGuildStone otherAllyGuild in soldier.Government.AlliedGuilds)
                                            {
                                                if (attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, otherAllyGuild)) // Is the attacker ALSO an ally?
                                                {
                                                    soldier.DebugSay("You are an ally.");
                                                    attackerIsAlly = true; // The attacker is an ally as well; I can't decide!
                                                    continue;
                                                }
                                                else if (attacker is Soldier && (attacker as Soldier).Government != null && (attacker as Soldier).Government.Deleted && (attacker as Soldier).Government == otherAllyGuild) // Is the attacker ALSO an ally?
                                                {
                                                    soldier.DebugSay("You are an ally.");
                                                    attackerIsAlly = true; // The attacker is an ally as well; I can't decide!
                                                    continue;
                                                }
                                            }

                                            if (!attackerIsAlly) // The attacker is not an ally.
                                            {
                                                soldier.DebugSay("You are not an ally, I will attack!");
                                                bValid = true; // The defender is an ally! I'll defend him from this unknown.
                                                continue;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Government Handling
                                if (!bValid) // So the defender wasn't an ally, nor was the attacker...
                                {
                                    if (CustomGuildStone.IsGuildMember(defender, soldier.Government) && defender.CustomGuilds[soldier.Government].ActiveTitle) // Is the defender a member of my government?
                                    {
                                        soldier.DebugSay("The defender is a member of my government.");
                                        if (attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government)) // Yes, and so is the attacker!
                                        {
                                            soldier.DebugSay("You are a member of my guild!");
                                            if ((attacker as PlayerMobile).CustomGuilds[soldier.Government].RankID < defender.CustomGuilds[soldier.Government].RankID) // So who outranks who? I'm with the authority on this one!  
                                                bValid = true; // Both are members of my government, but the defender outranks the attacker; defend the high-ranking member!
                                        }
                                        else
                                        {
                                            soldier.DebugSay("The defender is in my government, but you are neither in my government nor an ally. I will attack you.");
                                            bValid = true; // The defender is in my government, but the attacker is neither an ally nor in my government; therefore, I will attack him.
                                        }
                                    }
                                }
                                #endregion

                                #region Nation Handling

                                if (!bValid && !(attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government))) // If the attacker is not an officer of my guild...
                                {
                                    #region Checking to make certain the attacker is not an ally.
                                    bool attackerIsAlly = false;
                                    if (attacker is PlayerMobile || attacker is Soldier)
                                    {
                                        if (attacker is Soldier && (attacker as Soldier).Government != null && (attacker as Soldier).Government.Deleted && (attacker as Soldier).Government == soldier.Government)
                                        {
                                            soldier.DebugSay("The attacker is a soldier and an ally!");
                                            attackerIsAlly = true;
                                        }
                                        else
                                        {
                                            foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                            {
                                                if (attacker is PlayerMobile)
                                                {
                                                    if (CustomGuildStone.IsGuildMember(attacker as PlayerMobile, allyGuild))
                                                    {
                                                        soldier.DebugSay("You are an ally!");
                                                        attackerIsAlly = true;
                                                        continue;
                                                    }
                                                }
                                                else if (attacker is Soldier && (attacker as Soldier).Government != null && !(attacker as Soldier).Government.Deleted)
                                                {
                                                    if ((attacker as Soldier).Government == allyGuild)
                                                    {
                                                        soldier.DebugSay("You are an ally!");
                                                        attackerIsAlly = true;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (!attackerIsAlly && defender.GetDisguisedNation() == soldier.Government.Nation) // ... and the defender is of my own nation ...
                                    {
                                        soldier.DebugSay("The attacker is not an ally, and the defender is of my nation.");
                                        if (attacker is PlayerMobile)
                                        {
                                            if ((attacker as PlayerMobile).GetDisguisedNation() != soldier.Government.Nation)
                                            {
                                                soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                bValid = true;
                                            }
                                        }
                                        else
                                        {
                                            if (attacker is Soldier)
                                            {
                                                if ((attacker as Soldier).Nation != soldier.Government.Nation)
                                                {
                                                    soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                    bValid = true;
                                                }
                                            }
                                            else
                                            {
                                                soldier.DebugSay("But you are not of my nation, nor are you an ally; I will attack.");
                                                bValid = true;
                                            }
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    #endregion

                    #region The defender is a BaseCreature.
                    else if (attacker.Combatant is BaseCreature)
                    {
                        soldier.DebugSay("The defender is a BaseCreature!");
                        BaseCreature defender = attacker.Combatant as BaseCreature;
                        if (defender.FightMode != FightMode.Berserk && defender.FightMode != FightMode.Closest)
                        {
                            soldier.DebugSay("The defender is not aggressive, so why are you fighting?");
                            if (!(attacker is PlayerMobile && CustomGuildStone.IsGuildMember(attacker as PlayerMobile, soldier.Government)))
                            {
                                soldier.DebugSay("The attacker is not a member of my guild.");
                                #region Defending NPC is a soldier.
                                if (defender is BaseKhaerosMobile)
                                {
                                    soldier.DebugSay("The defender is a BaseKhaerosMobile.");
                                    BaseKhaerosMobile khaerosDefender = defender as BaseKhaerosMobile;
                                    if (khaerosDefender.Government != null && !khaerosDefender.Government.Deleted)
                                    {
                                        if (!soldier.Government.EnemyGuilds.Contains(khaerosDefender.Government)) // If the defender isn't part of an enemy government, I'll consider defending him.
                                        {
                                            #region Is the defending soldier part of my government?
                                            if (khaerosDefender.Government == soldier.Government &&
                                                !(attacker is Soldier &&
                                                (attacker as Soldier).Government != null &&
                                                !(attacker as Soldier).Government.Deleted &&
                                                (attacker as Soldier).Government == soldier.Government))
                                            {
                                                soldier.DebugSay("The defender is part of my government, I will attack you!");
                                                bValid = true;
                                            }
                                            #endregion
                                            #region Is the defending soldier an ally of my government?
                                            else
                                            {
                                                bool attackerIsAlly = false;
                                                foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                                {
                                                    if (attacker is PlayerMobile)
                                                    {
                                                        if (CustomGuildStone.IsGuildMember(attacker as PlayerMobile, allyGuild))
                                                        {
                                                            soldier.DebugSay("The attacker is an ally.");
                                                            attackerIsAlly = true;
                                                            continue;
                                                        }
                                                    }
                                                    else if (attacker is BaseKhaerosMobile)
                                                    {
                                                        if ((attacker as BaseKhaerosMobile).Government != null && !(attacker as BaseKhaerosMobile).Government.Deleted &&
                                                            ((attacker as BaseKhaerosMobile).Government == allyGuild || (attacker as BaseKhaerosMobile).Government == soldier.Government ||
                                                            (attacker as BaseKhaerosMobile).Government == khaerosDefender.Government))
                                                        {
                                                            soldier.DebugSay("The attacker is an ally.");
                                                            attackerIsAlly = true;
                                                            continue;
                                                        }
                                                    }
                                                }
                                                if (!attackerIsAlly) // If the attacker has proven to not be an ally of my guild...
                                                {
                                                    foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                                    {
                                                        if (khaerosDefender.Government == allyGuild) // ... and you ARE an ally of my guild ...
                                                        {
                                                            soldier.DebugSay("The attacker is not an ally and the defender IS an ally.");
                                                            bValid = true; // I will attack the attacker and defend you, defender.
                                                            continue;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region Defending NPC Nation Handling
                                if (!bValid)
                                {
                                    if (defender.Nation == soldier.Nation &&
                                        !(attacker is BaseCreature && (attacker as BaseCreature).Nation == soldier.Nation) &&
                                        !(attacker is PlayerMobile && (attacker as PlayerMobile).Nation == soldier.Nation))
                                    {
                                        soldier.DebugSay("The defender's nation is the same as mine.");
                                        bool attackerIsAlly = false;
                                        foreach (CustomGuildStone allyGuild in soldier.Government.AlliedGuilds)
                                        {
                                            if (attacker is BaseKhaerosMobile && (attacker as BaseKhaerosMobile).Government != null && !(attacker as BaseKhaerosMobile).Government.Deleted)
                                            {
                                                if ((attacker as BaseKhaerosMobile).Government == allyGuild)
                                                {
                                                    soldier.DebugSay("The attacker is an ally.");
                                                    attackerIsAlly = true;
                                                    continue;
                                                }
                                            }
                                            else if (attacker is PlayerMobile)
                                            {
                                                if (CustomGuildStone.IsGuildMember(attacker as PlayerMobile, allyGuild) && (attacker as PlayerMobile).CustomGuilds[allyGuild].ActiveTitle)
                                                {
                                                    soldier.DebugSay("The attacker is an ally.");
                                                    attackerIsAlly = true;
                                                    continue;
                                                }
                                            }
                                        }

                                        if (!attackerIsAlly)
                                        {
                                            soldier.DebugSay("The attacker is not an ally and the defender is of my Nation. I will attack.");
                                            bValid = true;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                    }
                    #endregion
                }
            }
            return bValid;
        }

        public static bool ThiefCheck(PlayerMobile m, Soldier s) // Checks to see if the player is committing a crime and is not a superior or ally.
        {
            if (m == null || m.Deleted || !m.Alive)
                return false;

            if (s == null || s.Deleted || !s.Alive)
                return false;

            if (m.AccessLevel > AccessLevel.Player) // Don't attack staff!
                return false;

            if (s.Government == null || s.Government.Deleted)
                return false;

            if (m.CriminalActivity && Soldier.VisionCheck(m, s, 15))
            {
                if (CustomGuildStone.IsGuildOfficer(m, s.Government) && m.CustomGuilds[s.Government].ActiveTitle)
                    return false;

                int count = s.Government.AlliedGuilds.Count;
                for (int i = 0; i < count; i++)
                {
                    CustomGuildStone thisGuild = (CustomGuildStone)s.Government.AlliedGuilds[i];

                    if (CustomGuildStone.IsGuildOfficer(m, thisGuild) && m.CustomGuilds[thisGuild].ActiveTitle)
                        return false;
                }

                return true;
            }

            return false;
        }
        
        public override void OnThink()
        {
            if  (ControlOrder != OrderType.Stop && (Combatant == null || (Combatant != null && (Combatant.Deleted || !Combatant.Alive || Combatant.IsDeadBondedPet))))
            {
                IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 15);
                foreach (Mobile m in eable)
                {
                    if (Soldier.VisionCheck(m, this, 14))
                    {
                        XmlAttachment attachment = null;
                        attachment = XmlAttach.FindAttachment(m, typeof(XmlCriminal), this.Nation.ToString());

                        if ((attachment == null || attachment.Deleted) && (m.AccessLevel < AccessLevel.Counselor))// if there is no Criminal attachment and they aren't staff
                        {
                            if ((m is PlayerMobile) && Soldier.ThiefCheck((PlayerMobile)m, this)) // Is the seen PlayerMobile participating in criminal activity?
                                RecognizeCrime(m, this);
                            else if (AssaultCheck(m, this))
                                RecognizeCrime(m, this);
                        }

                        // The mobile is added to the guard's aggressors list if the mobile has that nation's Criminal attachment if the guard is not 
                        // currently in battle, and if the mobile is not staff.
                        if ((attachment != null) && (this.Combatant == null) && (m.AccessLevel < AccessLevel.Counselor))
                        {
                            m.RevealingAction();

                            if (m is BaseCreature && ((BaseCreature)m).Controlled && (m as BaseCreature).ControlMaster != null && this.InRange((m as BaseCreature).ControlMaster.Location, 14))
                                this.AddAggressor = ((BaseCreature)m).ControlMaster;

                            string speech = RandomAttackSpeech(this);
                            if (speech != null)
                                this.Say(speech);

                            if (ControlOrder == OrderType.Stay || ControlOrder == OrderType.Guard)
                                ControlOrder = OrderType.Stop;                            
                            
                            this.AddAggressor = m;

                            continue;
                        }
                    }
                }
                eable.Free();
            }

            base.OnThink();
        }
        
        // A random attacking phrase generator; the timer is set so that guards do not endlessly spout there attack phrases.
        public static String RandomAttackSpeech(Soldier s)
        {
            int randomAttackPhrase = Utility.RandomMinMax(1, 16);
            switch (s.Nation)
            {
                case Nation.Southern:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "To victory!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "I'll clear the way of you!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "With a red hand!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Your day has come!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "Sons of the hounds, come here and get flesh!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "There'll be no one to sing your name!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "Your story ends here, blighter."; }
                            }
                        }

                        return null;
                    }
                case Nation.Western:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "For the Dawn Princess!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "This will form my Balance."; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "For the preservation of Light!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Forward!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "You'll know only darkness."; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Death! Death! Death!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "I will sweep you away like the wind."; }
                            }
                        }

                        return null;
                    }
                case Nation.Khemetar:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Mah'tet wills it."; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "Today is an auspicious day for death!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "I will sweep you away like the wind."; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "The vultures take you!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "Black sand!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "You should know your place!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "Ignorant sow!"; }
                            }
                        }

                        return null;
                    }
                case Nation.Mhordul:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Honorless slave!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "Betrayer!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "Boghul! You will regret this!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Fool!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "Your death will please Xorgoth."; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Your bones will make fine armor!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "You're already dead."; }
                            }
                        }

                        return null;
                    }
                case Nation.Tyrean:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "For the fatherland, forward!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "Remember Tyris!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "Cut them down, sons of the north!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Fire at your balls!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "Follow me, heroes!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Unworthy swine."; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "Beardless fool!"; }
                            }
                        }

                        return null;
                    }
                case Nation.Northern:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Awake the iron!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "Long live Vhaluran!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Halt, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "Charge, my brothers!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "For the Father!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "To arms, to arms!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Halfborn bastard of a Mhordul!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "*snooore... snr-rr-snort?!*"; }
                            }
                        }

                        return null;
                    }
                case Nation.Imperial:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Idolator!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "In the name of our Father!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Slay them!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "For the glory of our Mother!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Run them through!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "Cleanse the ground with their blood!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "To the abyss!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "Come to meet my reaper?"; }
                            }
                        }

                        return null;
                    }
                case Nation.Sovereign:
                    {
                        if (DateTime.Now >= (s.SpeechInterval + TimeSpan.FromSeconds(Utility.RandomMinMax(120, 600))))
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Freedom or death!"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "No quarter!"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "Brothers! Trust and go forward!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "Honorless slave."; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "With a red hand!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Our gods protect us!"; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "Your evil ends here."; }
                            }
                        }

                        return null;
                    }
                case Nation.Society:
                    {
                        if ( DateTime.Now >= ( s.SpeechInterval + TimeSpan.FromSeconds( Utility.RandomMinMax(120,600) ) ) )
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "Mah'tet willed our meeting."; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "Xipotec has revealed your sins."; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "Xorgoth guide my hand!"; }
                                case 5: { s.SpeechInterval = DateTime.Now; return "By the many dicks of Ohlm!"; }
                                case 6: { s.SpeechInterval = DateTime.Now; return "By Arianthyt's grace!"; }
                                case 7: { s.SpeechInterval = DateTime.Now; return "Elysia have mercy on you."; }
                                case 8: { s.SpeechInterval = DateTime.Now; return "For the Six!!!"; }
                            }
                        }

                        return null;
                    }
                case Nation.Insularii:
                    {
                        if ( DateTime.Now >= ( s.SpeechInterval + TimeSpan.FromSeconds( Utility.RandomMinMax(120,600) ) ) )
                        {
                            switch (randomAttackPhrase)
                            {
                                case 1: { s.SpeechInterval = DateTime.Now; return "*readies their weapon*"; }
                                case 2: { s.SpeechInterval = DateTime.Now; return "*growls softly*"; }
                                case 3: { s.SpeechInterval = DateTime.Now; return "*readies for battle*"; }
                                case 4: { s.SpeechInterval = DateTime.Now; return "*chuckles lightly*"; }
                            }
                        }

                        return null;
                    }
            }

            return null;
        }

        #endregion

        #region Utilities & Serialization/Deserialization

        public override void OnDeath(Container c)
        {
            if (Government != null && !Government.Deleted)
            {
                ReportInfo newReport = new ReportInfo(this.Name + " died in the line of duty, stationed at X: " + (Spawner as MilitarySpawner).X + ", Y: " + (Spawner as MilitarySpawner).Y + ".");
                newReport.TimeOfReport = Formatting.GetTimeFormat(this, Format.Time);
                Government.Reports.Add(newReport);

                if (Government.Employees.Contains(this))
                    Government.Employees.Remove(this);
            }

            base.OnDeath(c);
        }

        public override void OnDelete()
        {
            if (Government.Employees.Contains(this))
                Government.Employees.Remove(this);

            base.OnDelete();
        }

        public override void OnAfterSpawn()
        {
            if (Government != null && !Government.Deleted && !Government.Employees.Contains(this))
            {
                Government.Employees.Add(this);
                DyeClothes = Government.ClothingHue;
                DyeArmour = Government.ArmourHue;
                foreach (Item i in Items)
                {
                    if (i is Surcoat)
                    {
                        i.Name = "A Surcoat of " + Government.Name.ToString();
                    }
                }
            }

            base.OnAfterSpawn();
        }

        public static int CalculateSoldierPay(Armament arm, Training train)
        {
            int cost = 0;

            if (train != Training.None)
                cost += 1;

            switch (arm)
            {
                case Armament.Light: cost += 100; break;
                case Armament.Medium: cost += 150; break;
                case Armament.Heavy: cost += 200; break;
                case Armament.Ranged: cost += 150; break;
                case Armament.LightCavalry: cost += 250; break;
                case Armament.HeavyCavalry: cost += 350; break;
                default: break;
            }

            return cost;
        }

        public static double CalculateResourceBonus(GovernmentEntity gov, Armament arm)
        {
            if (gov == null || gov.Deleted)
                return 0;

            double bonus = 0;
            double resDiv = (gov.MilitarySpawners.Count);
            resDiv -= (gov.Resources[ResourceType.Influence] / 1000);

            foreach (KeyValuePair<ResourceType, int> kvp in gov.Resources)
            {
                if (kvp.Value > 0)
                    resDiv -= 0.75;
            }

            if (resDiv < 1)
                resDiv = 1;

            switch (arm)
            {
                case Armament.Light:
                    {
                        bonus += ( gov.Resources[ResourceType.Cloth] / resDiv );
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                case Armament.Medium:
                    {
                        bonus += (gov.Resources[ResourceType.Metals] / (resDiv * 2));
                        bonus += (gov.Resources[ResourceType.Cloth] / (resDiv * 2));
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                case Armament.Heavy:
                    {
                        bonus += (gov.Resources[ResourceType.Metals] / resDiv);
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                case Armament.Ranged:
                    {
                        bonus += (gov.Resources[ResourceType.Wood] / resDiv);
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                case Armament.LightCavalry:
                    {
                        bonus += (gov.Resources[ResourceType.Cloth] / resDiv);
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                case Armament.HeavyCavalry:
                    {
                        bonus += (gov.Resources[ResourceType.Metals] / resDiv);
                        bonus += (gov.Resources[ResourceType.Food] / resDiv);
                        bonus += (gov.Resources[ResourceType.Water] / resDiv);
                        break;
                    }
                default: break;
            }

            return bonus;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 16))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.InRange(this.Location, 14) && e.Mobile.InLOS(this) && e.Mobile is PlayerMobile && (Soldier.IsGovernmentSuperior(this, e.Mobile as PlayerMobile) || e.Mobile.AccessLevel > AccessLevel.GameMaster))
            {
                string speech = e.Speech.ToLower();
                speech.Replace(".", ""); speech.Replace(",", ""); speech.Replace("!", ""); speech.Replace("?", "");
                speech.Replace(":", ""); speech.Replace(";", ""); speech.Replace("-", "");
                string[] heardSpeech = speech.Split(' ');

                bool called = false;
                string[] splitName = Name.Split(' ');
                
                foreach (string sp in heardSpeech)
                {
                    if (sp.ToLower() == "all")
                    {
                        called = true;
                        continue;
                    }
                    
                    foreach (string name in splitName)
                    {
                        if (sp.ToLower() == name.ToLower())
                        {
                            called = true;
                            continue;
                        }
                    }
                }

                if (!called && (speech.Contains(Name.ToLower()) || speech.Contains(BaseName.ToLower())))
                    called = true;

                if (called || ( GroupInfo.IsGroupLeader(this, e.Mobile as PlayerMobile) && ( e.Speech.ToLower().Contains("squad") || e.Speech.ToLower().Contains("group") || e.Speech.ToLower().Contains((e.Mobile as PlayerMobile).Group.Name.ToLower()))))
                {
                    for (int i = 0; i < heardSpeech.Length; i++)
                    {
                        if (heardSpeech[i].ToLower() == "patrol")
                        {
                            if (e.Mobile.Target == null)
                            {
                                e.Mobile.Target = new SoldierPatrolTarget(this);
                                e.Mobile.SendMessage("Target a waypoint to order " + this.Name.ToString() + " to begin patrolling there.");
                            }
                            continue;
                        }
                        else if (heardSpeech[i].ToLower() == "return")
                        {
                            ControlOrder = OrderType.None;
                            Home = (Spawner as Item).Location;
                            AIObject.Action = ActionType.Wander;
                            continue;
                        }
                        else if (heardSpeech[i].ToLower() == "equip")
                        {
                            if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildMilitary(e.Mobile as PlayerMobile, Government))
                            {
                                e.Mobile.Target = new SoldierEquipTarget(this);
                                e.Mobile.SendMessage("Target a piece of equipment to have " + this.Name.ToString() + " equip it.");
                            }
                            continue;
                        }
                        else if (heardSpeech[i].ToLower() == "unequip")
                        {
                            if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildMilitary(e.Mobile as PlayerMobile, Government))
                            {
                                e.Mobile.Target = new SoldierUnequipTarget(this);
                                e.Mobile.SendMessage("Target a piece of equipment on " + this.Name.ToString() + " to have them unequip it.");
                            }
                            continue;
                        }
                        else if (heardSpeech[i].ToLower() == "orders" || heardSpeech[i].ToLower() == "order")
                        {
                            if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildMilitary(e.Mobile as PlayerMobile, Government))
                            {
                                e.Mobile.SendGump(new ViewOrdersGump(e.Mobile as PlayerMobile, this));
                            }

                            continue;
                        }
                        else if (heardSpeech[i].ToLower() == "face")
                        {
                            if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildMilitary(e.Mobile as PlayerMobile, Government))
                            {
                                if (e.Speech.ToLower().Contains("north"))
                                    this.Direction = Server.Direction.North;
                                else if (e.Speech.ToLower().Contains("northeast"))
                                    this.Direction = Server.Direction.Right;
                                else if (e.Speech.ToLower().Contains("east"))
                                    this.Direction = Server.Direction.East;
                                else if (e.Speech.ToLower().Contains("southeast"))
                                    this.Direction = Server.Direction.Down;
                                else if (e.Speech.ToLower().Contains("south"))
                                    this.Direction = Server.Direction.South;
                                else if (e.Speech.ToLower().Contains("southwest"))
                                    this.Direction = Server.Direction.Left;
                                else if (e.Speech.ToLower().Contains("west"))
                                    this.Direction = Server.Direction.West;
                                else if (e.Speech.ToLower().Contains("northwest"))
                                    this.Direction = Server.Direction.Up;
                            }

                            continue;
                        }
                    }
                }

            }
            base.OnSpeech(e);
        }

        public static bool IsGovernmentSuperior(Mobile m, PlayerMobile pm)
        {
            if(m is Soldier)
            {
                Soldier s = m as Soldier;

                if (GroupInfo.HasGroup(s))
                {
                    return GroupInfo.IsGroupLeader(s, pm);
                }
                else
                {
                    if  ( (s.Government != null) && (s.Government.Members.Contains(pm)) && ( GovernmentEntity.IsGuildOfficer(pm, s.Government) || 
                        GovernmentEntity.IsGuildLeader(pm, s.Government) || GovernmentEntity.IsGuildOwner(pm, s.Government) ) ) 
                        return true;
                }
            }

            return false;
        }

        public Soldier(Serial serial) : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)m_Armaments);
            writer.Write((int)m_PayRate);
            writer.WriteDeltaTime(m_speechInterval);
            writer.Write((int)m_Training);
            writer.Write((String)m_BaseName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Armaments = (Armament)reader.ReadInt();
                        m_PayRate = reader.ReadInt();
                        m_speechInterval = reader.ReadDeltaTime();
                        m_Training = (Training)reader.ReadInt();
                        m_BaseName = reader.ReadString();

                        break;
                    }
            }

            if (Government != null && Government.Deleted && !Government.Employees.Contains(this))
                Government.Employees.Add(this);
        }
        #endregion
    }

    public class SoldierPatrolTarget : Target
    {
        private Soldier m_Sol;

        public SoldierPatrolTarget(Soldier s) : base(12, true, TargetFlags.None)
        {
            m_Sol = s;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is MilitaryWayPoint && (targeted as MilitaryWayPoint).Government == m_Sol.Government)
            {
                m_Sol.CurrentWayPoint = targeted as MilitaryWayPoint;
                m_Sol.ControlOrder = OrderType.None;
                m_Sol.AIObject.Action = ActionType.Wander;
                (m_Sol.Spawner as XmlSpawner).WayPoint = targeted as MilitaryWayPoint;
            }
            else if (!(targeted is MilitaryWayPoint))
                from.SendMessage("Soldiers can only be ordered to patrol using military way points.");
            else if (targeted is MilitaryWayPoint && (targeted as MilitaryWayPoint).Government != m_Sol.Government)
                from.SendMessage("That is not a waypoint belonging to " + m_Sol.Name.ToString() + "'s government, " + m_Sol.Government.Name.ToString() + ".");

            base.OnTarget(from, targeted);
        }
    }

    public class SoldierEquipTarget : Target
    {
        private Soldier m_Soldier;
        private PlayerMobile m_Commander;
        private Item m_CurrentEquip;
        private Item m_NewEquip;

        public SoldierEquipTarget(Soldier s) : base(14, true, TargetFlags.None)
        {
            m_Soldier = s;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Soldier == null || m_Soldier.Deleted || !m_Soldier.Alive)
                return;

            if (m_Soldier.Government == null || m_Soldier.Government.Deleted)
                return;

            if (from is PlayerMobile)
                m_Commander = from as PlayerMobile;
            else
                return;

            if (CustomGuildStone.IsGuildMilitary(m_Commander, m_Soldier.Government))
            {
                if (targeted is Item)
                {
                    if ((targeted as Item).Movable && (targeted as Item).Visible)
                    {
                        if ((targeted as Item).IsChildOf(m_Commander))
                        {
                            if (m_Commander.InRange(m_Soldier.Location, 2))
                            {
                                EquipTarget(targeted as Item);
                            }
                            else
                                m_Commander.SendMessage("You are too far away to do that.");
                        }
                        else
                        {
                            if (m_Soldier.InRange((targeted as Item).Location, 2))
                            {
                                EquipTarget(targeted as Item);
                            }
                            else
                                m_Soldier.Say("I am too far away to equip that.");
                        }
                    }
                }
                else
                    m_Soldier.Say("I can't equip that.");
            }
            else
            {
                m_Commander.SendMessage("Only military officers of " + m_Soldier.Government.Name.ToString() + " may do that.");
                m_Soldier.Say("You don't have that authority.");
            }
            
            base.OnTarget(from, targeted);
        }

        private void EquipTarget(Item i)
        {
            if (i == null || i.Deleted)
                return;

            if (m_Commander.CustomGuilds[m_Soldier.Government].ActiveTitle)
            {
                if (m_Commander.TitlePrefix != null && m_Commander.TitlePrefix != "")
                    m_Soldier.Say("Yes, " + m_Commander.TitlePrefix + ".");
            }

            if (i.Layer != Layer.Invalid && i.Layer != Layer.Mount && i.Layer != Layer.ShopBuy && i.Layer != Layer.ShopResale && i.Layer != Layer.ShopSell)
            {
                m_NewEquip = i;
                if (m_NewEquip.Layer == Layer.FirstValid && m_NewEquip is BaseWeapon)
                {
                    if (m_Soldier.FindItemOnLayer(Layer.TwoHanded) is BaseWeapon)
                        m_CurrentEquip = m_Soldier.FindItemOnLayer(Layer.TwoHanded);
                    else
                        m_CurrentEquip = m_Soldier.FindItemOnLayer(Layer.OneHanded);
                }
                else
                    m_CurrentEquip = m_Soldier.FindItemOnLayer(i.Layer);

                if (m_CurrentEquip != null && !m_CurrentEquip.Deleted)
                {
                    if (m_NewEquip.Layer == Layer.TwoHanded)
                    {
                        Item oneHanded = m_Soldier.FindItemOnLayer(Layer.OneHanded);
                        if (oneHanded == null)
                            oneHanded = m_Soldier.FindItemOnLayer(Layer.FirstValid);

                        if (oneHanded != null)
                        {
                            if (m_Commander.InRange(m_Soldier.Location, 3))
                            {
                                m_Commander.AddToBackpack(oneHanded);
                                //m_Soldier.Say("Here is the " + oneHanded.Name.ToString() + ".");
                                m_Commander.PlaySound(oneHanded.GetDropSound());
                            }
                            else
                            {
                                oneHanded.DropToWorld(m_Soldier, m_Soldier.Location);
                                m_Soldier.PlaySound(oneHanded.GetDropSound());
                            }
                        }
                    }

                    if (m_Commander.InRange(m_Soldier.Location, 3))
                    {
                        m_Commander.AddToBackpack(m_CurrentEquip);
                        //m_Soldier.Say("Here is the " + m_CurrentEquip.Name.ToString() + ".");
                        m_Commander.PlaySound(m_CurrentEquip.GetDropSound());
                    }
                    else
                    {
                        m_CurrentEquip.DropToWorld(m_Soldier, m_Soldier.Location);
                        m_Soldier.PlaySound(m_CurrentEquip.GetDropSound());
                    }
                }
                m_Soldier.EquipItem(m_NewEquip);
                m_Soldier.Emote("*changes equipment*");
                if (m_NewEquip is BaseMeleeWeapon)
                    m_Soldier.AI = AIType.AI_Melee;
                else if (m_NewEquip is BaseRanged)
                    m_Soldier.AI = AIType.AI_Archer;
                m_Soldier.PlaySound(m_NewEquip.GetLiftSound(m_Soldier));
            }
            else
                m_Soldier.Say("I can't equip that.");
        }
    }

    public class SoldierUnequipTarget : Target
    {
        private Soldier m_Soldier;
        private PlayerMobile m_Commander;


        public SoldierUnequipTarget(Soldier s)
            : base(1000000, true, TargetFlags.None)
        {
            m_Soldier = s;
            AllowNonlocal = true;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Soldier == null || m_Soldier.Deleted)
                return;
            if (m_Soldier.Government == null || m_Soldier.Government.Deleted)
                return;

            if (from is PlayerMobile && CustomGuildStone.IsGuildMilitary(from as PlayerMobile, m_Soldier.Government))
            {
                if (!from.InRange(m_Soldier.Location, 14))
                {
                    from.SendMessage("You are too far away to issue that order.");
                    return;
                }

                if (targeted is Item)
                {
                    Item thisItem = targeted as Item;
                    if (thisItem.IsChildOf(m_Soldier))
                    {
                        m_Soldier.Emote("*sheds some equipment*");

                        if(from.InRange(m_Soldier.Location, 3))
                        {
                            from.AddToBackpack(thisItem);
                            from.PlaySound(thisItem.GetDropSound());
                        }
                        else
                        {
                            thisItem.DropToWorld(m_Soldier, m_Soldier.Location);
                            m_Soldier.PlaySound(thisItem.GetDropSound());
                        }
                    }
                    else
                        from.SendMessage(m_Soldier.Name + " can only unequip items that are already equipped.");
                }
                else
                    from.SendMessage("You must target an item.");
            }
            else
                from.SendMessage("Only military officers of " + m_Soldier.Government.Name + " may do that.");

            base.OnTarget(from, targeted);
        }
    }

    [PropertyObject]
    public class ReportInfo
    {
        #region Variables, Properties, and Get/Set
        private bool m_IsPlayer;
        private bool m_IsSoldier;
        private string m_Name;
        private bool m_IsFemale;
        private int m_Height;
        private int m_Weight;
        private string m_Age;
        private Nation m_Nation;
        private string m_Description;
        private string m_Guild;
        private Point3D m_Location;
        private bool m_Thievery;
        private bool m_Assault;
        private bool m_IsOptional;
        private string m_ReporterName;

        private string m_Information;
        private string m_TimeOfReport;

        public bool IsPlayer { get { return m_IsPlayer; } set { m_IsPlayer = value; } }
        public bool IsSoldier { get { return m_IsSoldier; } set { m_IsSoldier = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public bool IsFemale { get { return m_IsFemale; } set { m_IsFemale = value; } }
        public int Height { get { return m_Height; } set { m_Height = value; } }
        public int Weight { get { return m_Weight; } set { m_Weight = value; } }
        public string Age { get { return m_Age; } set { m_Age = value; } }
        public Nation Nation { get { return m_Nation; } set { m_Nation = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public string Guild { get { return m_Guild; } set { m_Guild = value; } }
        public Point3D Location { get { return m_Location; } set { m_Location = value; } }
        public bool Thievery { get { return m_Thievery; } set { m_Thievery = value; } }
        public bool Assault { get { return m_Assault; } set { m_Assault = value; } }
        public bool IsOptional { get { return m_IsOptional; } set { m_IsOptional = value; } }
        public string ReporterName { get { return m_ReporterName; } set { m_ReporterName = value; } }

        public string Information { get { return m_Information; } set { m_Information = value; } }
        public string TimeOfReport { get { return m_TimeOfReport; } set { m_TimeOfReport = value; } }

        #endregion

        public ReportInfo(string optionalString)
        {
            m_Information = optionalString;
            m_IsOptional = true;
        }

        public ReportInfo(Mobile m, bool thievery, bool assault)
        {
            if (m == null)
                return;

            m_IsOptional = false;
            m_Thievery = thievery;
            m_Assault = assault;
            m_IsFemale = m.Female;
            m_Location = m.Location;
            m_Location.X += Utility.RandomMinMax(-15,15);
            m_Location.Y += Utility.RandomMinMax(-15,15);
            if (m is PlayerMobile)
            {
                PlayerMobile pm = m as PlayerMobile;
                m_IsPlayer = true;

                m_Height = pm.Height + Utility.RandomMinMax(Utility.RandomMinMax(-10, -1), Utility.RandomMinMax(1, 10));
                m_Weight = pm.Weight + Utility.RandomMinMax(Utility.RandomMinMax(-10, -1), Utility.RandomMinMax(1, 10));

                int sightedAge = pm.Age += Utility.RandomMinMax(Utility.RandomMinMax(-10,-1),Utility.RandomMinMax(1,10));
                

                if (sightedAge < 20)
                    Age = "teenager";
                else if (sightedAge < 26)
                    Age = "early twenties";
                else if (sightedAge < 30)
                    Age = "late twenties";
                else if (sightedAge < 36)
                    Age = "early thirties";
                else if (sightedAge < 40)
                    Age = "late thirties";
                else if (sightedAge < 55)
                    Age = "middle-aged";
                else if (sightedAge < 75)
                    Age = "elderly";
                else if (sightedAge < 100)
                    Age = "extremely old";
                else
                    Age = "ancient";
                
                m_Description = pm.Description;
                m_Nation = pm.Nation;

                if (pm.Disguised)
                {
                    m_Nation = pm.GetDisguisedNation();
                    if (pm.Disguise.Age != null)
                        Age = pm.Disguise.Age;
                    if (pm.Disguise.Description1 != null)
                        m_Description = pm.Disguise.Description1;
                }

                foreach (KeyValuePair<CustomGuildStone,CustomGuildInfo> g in pm.CustomGuilds)
                {
                    if (pm.CustomGuilds[g.Key].ActiveTitle)
                    {
                        m_Guild = g.Key.Name.ToString();
                        continue;
                    }
                }
            }
            else if (m is BaseCreature)
            {
                BaseCreature bc = m as BaseCreature;
                m_IsPlayer = false;

                if (bc is Soldier)
                {
                    IsSoldier = true;
                    m_Guild = (bc as Soldier).Government.Name.ToString();
                }
                else
                {
                    IsSoldier = false;
                    m_Name = bc.Name.ToString();
                }
            }

            Information = ReportInfo.WriteReport(this);
        }

        public static string WriteReport(ReportInfo info)
        {
            string reportString = "";

            if (info.IsPlayer)
            {
                if (info.IsFemale)
                    reportString += "A female ";
                else
                    reportString += "A male ";

                reportString += info.Nation + ", " + info.Age + ", weight: " + info.Weight + " petrae, height: " + info.Height + " petrae. ";
                if (info.Guild != null)
                    reportString += "<br> The subject bore the insignia of " + info.Guild + ". ";
                if (Utility.RandomMinMax(1, 100) > 75)
                    reportString += "<br> The following description was provided: " + info.Description;
            }
            else
            {
                if (info.IsSoldier)
                    reportString += "soldier under the command of " + info.Guild + ". ";
                else
                    reportString = info.Name + ".";
            }

            if (info.Thievery)
                reportString = "Spotted engaging in suspicious and illegal activity at " + info.Location + ": " + reportString;
            else if (info.Assault)
                reportString = "Spotted engaging in violent activity at " + info.Location + ": " + reportString;

            return reportString;
        }

        public static void Serialize(GenericWriter writer, ReportInfo info)
        {
            writer.Write((bool)info.IsPlayer);
            writer.Write((bool)info.IsSoldier);
            writer.Write((string)info.Name);
            writer.Write((bool)info.IsFemale);
            writer.Write((int)info.Height);
            writer.Write((int)info.Weight);
            writer.Write((string)info.Age);
            writer.Write((int)info.Nation);
            writer.Write((string)info.Description);
            writer.Write((string)info.Guild);
            writer.Write((Point3D)info.Location);
            writer.Write((bool)info.Thievery);
            writer.Write((bool)info.Assault);
            writer.Write((bool)info.IsOptional);
            writer.Write((string)info.ReporterName);
            writer.Write((string)info.Information);
            writer.Write((string)info.TimeOfReport);
        }

        public static void Deserialize(GenericReader reader, ReportInfo info)
        {
            info.IsPlayer = reader.ReadBool();
            info.IsSoldier = reader.ReadBool();
            info.Name = reader.ReadString();
            info.IsFemale = reader.ReadBool();
            info.Height = reader.ReadInt();
            info.Weight = reader.ReadInt();
            info.Age = reader.ReadString();
            info.Nation = (Nation)reader.ReadInt();
            info.Description = reader.ReadString();
            info.Guild = reader.ReadString();
            info.Location = reader.ReadPoint3D();
            info.Thievery = reader.ReadBool();
            info.Assault = reader.ReadBool();
            info.IsOptional = reader.ReadBool();
            info.ReporterName = reader.ReadString();
            info.Information = reader.ReadString();
            info.TimeOfReport = reader.ReadString();
        }
    }

    public class ReportTimer : Timer
    {
        private ReportInfo m_Report;
        private Soldier m_Soldier;
        private DateTime m_TimerStarted;

        public ReportTimer(ReportInfo r, Soldier s)
            : base(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(30))
        {
            Priority = TimerPriority.OneMinute;
            m_Report = r;
            m_Soldier = s;
            m_Report.Name = m_Soldier.Name.ToString();
            m_TimerStarted = DateTime.Now;
        }

        protected override void OnTick()
        {
            if (m_Soldier == null || m_Soldier.Deleted || !m_Soldier.Alive)
                Stop();

            if (m_TimerStarted + TimeSpan.FromHours(Utility.RandomMinMax(1,4)) <= DateTime.Now)
            {
                if (m_Soldier != null && !m_Soldier.Deleted && m_Soldier.Alive && m_Soldier.Government != null && !m_Soldier.Government.Deleted)
                {
                    m_Report.TimeOfReport = Formatting.GetTimeFormat(m_Soldier, Format.Time);
                }

                m_Soldier.Government.Reports.Add(m_Report);

                Stop();
            }
            base.OnTick();
        }
    }
}
