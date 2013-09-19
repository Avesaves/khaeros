using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Commands;

namespace Server.Items
{
    public class GovernmentEntity : CustomGuildStone
    {
        public static void Initialize()
        {
            CommandSystem.Register("ResetGovernmentEmployees", AccessLevel.Owner, new CommandEventHandler(ResetGovernmentEmployees_OnCommand));
        }

        [Usage("ResetGovernmentEmployees")]
        [Description("Reloads all government employees into the m_Employees property on GovernmentEntity stones.")]
        private static void ResetGovernmentEmployees_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Mobile.Deleted)
                return;

            foreach(GovernmentEntity g in GovernmentEntity.Governments)
            {
                e.Mobile.SendMessage("Clearing " + g.Name.ToString());
                g.Employees.Clear();
                g.Employees = new List<Mobile>();
            }

            e.Mobile.SendMessage("Adding soldiers & employees...");
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is Soldier)
                {
                    Soldier sol = m as Soldier;
                    if (sol.Government != null && !sol.Government.Deleted)
                        sol.Government.Employees.Add(sol);
                }
                else if (m is Employee)
                {
                    Employee emp = m as Employee;
                    if (emp.Government != null && !emp.Government.Deleted)
                        emp.Government.Employees.Add(emp);
                }
                else if (m is PlayerVendor)
                {
                    PlayerVendor vendor = m as PlayerVendor;
                    if (vendor.Government != null && !vendor.Government.Deleted)
                        vendor.Government.Employees.Add(vendor);
                }
            }

            e.Mobile.SendMessage("Employees updated.");
        }

        #region Private Variables & Gets/Sets
        public static List<GovernmentEntity> Governments = new List<GovernmentEntity>();

        private bool m_CityGovernment = false;
        private Nation m_Nation = Nation.None;
        private List<MilitarySpawner> m_MilitarySpawners = new List<MilitarySpawner>();
        private List<MilitaryWayPoint> m_WayPoints = new List<MilitaryWayPoint>();
        private Dictionary<ResourceType, int> m_Resources = new Dictionary<ResourceType, int>();
        private TradeAdvisor m_TradeAdvisor = null;
        private MilitaryAdvisor m_MilitaryAdvisor = null;
        private List<ReportInfo> m_Reports = new List<ReportInfo>();
        private MilitaryInfo m_MilitaryPolicies;
        private List<Mobile> m_Employees = new List<Mobile>();
        private TradeInfo m_TradeInformation;
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CityGovernment { get { return m_CityGovernment; } set { m_CityGovernment = value; } }        
        [CommandProperty(AccessLevel.GameMaster)]
        public Nation Nation { get { return m_Nation; } set { m_Nation = value; } }
        public List<MilitarySpawner> MilitarySpawners { get { return m_MilitarySpawners; } set { m_MilitarySpawners = value; } }
        public List<MilitaryWayPoint> WayPoints { get { return m_WayPoints; } set { m_WayPoints = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public Dictionary<ResourceType, int> Resources { get { return m_Resources; } set { m_Resources = value; } }
        public TradeAdvisor TradeAdvisor { get { return m_TradeAdvisor; } set { m_TradeAdvisor = value; } }
        public MilitaryAdvisor MilitaryAdvisor { get { return m_MilitaryAdvisor; } set { m_MilitaryAdvisor = value; } }
        public List<ReportInfo> Reports { get { return m_Reports; } set { m_Reports = value; } }
        public MilitaryInfo MilitaryPolicies { get { return m_MilitaryPolicies; } set { m_MilitaryPolicies = value; } }
        public List<Mobile> Employees { get { return m_Employees; } set { m_Employees = value; } }
        public TradeInfo TradeInformation 
        { 
            get 
            {
                if (m_TradeInformation == null)
                    m_TradeInformation = new TradeInfo();
                return m_TradeInformation; 
            } 
            set { m_TradeInformation = value; } }
        public int EmployeesForHire
        {
            get
            {
                if (Resources[ResourceType.Influence] < 1)
                    return 0;

                int count = (100 - Members.Count);
                if(count < 1)
                    count = 1;
                int forhire = (Resources[ResourceType.Influence] / count);
                count = 0;
                foreach(Mobile m in Employees)
                    if(m is Employee)
                        count++;
                forhire -= count;
                if (forhire < 0)
                    forhire = 0;

                return forhire;
            }
        }
        #endregion

        [Constructable]
        public GovernmentEntity() : base()
        {
            Name = "New Government";
            Governments.Add( this );

            Resources = new Dictionary<ResourceType, int>();
            Resources.Add(ResourceType.Metals, 0);
            Resources.Add(ResourceType.Food, 0);
            Resources.Add(ResourceType.Water, 0);
            Resources.Add(ResourceType.Cloth, 0);
            Resources.Add(ResourceType.Wood, 0);
            Resources.Add(ResourceType.Influence, 0);

            m_MilitarySpawners = new List<MilitarySpawner>();
            m_WayPoints = new List<MilitaryWayPoint>();
            m_MilitaryPolicies = new MilitaryInfo(this);
        }

        public override void DoPayment()
        {
            base.DoPayment();

            if (Treasury == null || Treasury.Deleted)
                return;

            int count = MilitarySpawners.Count;
            for (int i = 0; i < count; i++) //Pay for spawners/soldiers.
            {
                if (Treasury.GetAmount(typeof(Copper)) >= ( MilitarySpawners[i].PayRate - ResourceBudgetContribution()))
                {
                    int copperPay = MilitarySpawners[i].PayRate - ResourceBudgetContribution();
                    if(copperPay < 0)
                        copperPay = 1;
                    Treasury.ConsumeUpTo(typeof(Copper), copperPay);
                    MilitarySpawners[i].IsInactivated = false;
                }
                else
                    MilitarySpawners[i].IsInactivated = true;
            }

            //Resource degradation.
/* 
            int MemberDivisor = Members.Count / 2;
            if (MemberDivisor < 1)
                MemberDivisor = 1;
            int SpendRes =  (MilitarySpawners.Count * (100 - Members.Count)) / MemberDivisor;

            for (int i = 1; i <= 6; i++)
            {
                ResourceType res = (ResourceType)i;
                if (Resources[res] > 0)
                {
                    Resources[res] -= ((Resources[res] / 2) + SpendRes);
                }

                if (Resources[res] < 0)
                    Resources[res] = 0;
            } */
        }

        public int Budget()
        {
            int budg = 0;

            foreach (MilitarySpawner spawner in MilitarySpawners)
            {
                budg += (int)(spawner.PayRate - Soldier.CalculateResourceBonus(this, (Armament)spawner.Armaments));
            }

            foreach (Mobile m in Employees)
            {
                if(m is PlayerVendor)
                    budg += (TradeInformation.VendorWages / 2);
            }

            foreach (PlayerMobile pm in Members)
            {
                budg += pm.CustomGuilds[this].RankInfo.Pay;
            }

            return budg;
        }

        public int GetTreasuryBalance()
        {
            int amount = 0;

            if (Treasury != null)
            {
                foreach (Item item in Treasury.Items)
                {
                    if (item is Copper)
                        amount += item.Amount;
                    else if (item is Silver)
                        amount += item.Amount * 10;
                    else if (item is Gold)
                        amount += item.Amount * 100;
                }
            }

            return amount;
        }

        public int ResourceBudgetContribution()
        {
            int totalReduction = 0;
            double divisor = (this.MilitarySpawners.Count * 0.5);

            foreach (KeyValuePair<ResourceType, int> kvp in m_Resources)
            {
                totalReduction += kvp.Value;
                if (kvp.Value > 0)
                    divisor += 1;
            }

            divisor -= this.Members.Count;

            if (divisor < 1)
                divisor = 1;

            totalReduction = totalReduction / (int)divisor;

            return totalReduction;
        }
            
        public override void OnDoubleClick( Mobile m )
        {
            if( m != null && this != null && !m.Deleted && !this.Deleted && m is PlayerMobile )
            {
                if( HasViewingRights( (PlayerMobile)m, this ) )
                    m.SendGump( new GovernmentGump( (PlayerMobile)m, this ) );

                else
                    TryToApply( (PlayerMobile)m );
            }
        }

        public override void OnDelete()
        {
            if( Governments.Contains( this ) )
                Governments.Remove( this );

            if((TradeAdvisor != null) && !(TradeAdvisor.Deleted))
                TradeAdvisor.Delete();

            if (MilitaryAdvisor != null && !MilitaryAdvisor.Deleted)
                MilitaryAdvisor.Delete();

            if (Employees != null && Employees.Count > 0)
            {
                for (int i = (Employees.Count - 1); i > -1; i--)
                {
                    Employees[i].Delete();
                }
            }

            if (MilitarySpawners != null && MilitarySpawners.Count > 0)
            {
                for (int i = (MilitarySpawners.Count - 1); i > -1; i--)
                {
                    MilitarySpawners[i].Delete();
                }
            }

            if (WayPoints != null && WayPoints.Count > 0)
            {
                for (int i = (WayPoints.Count - 1); i > -1; i--)
                {
                    WayPoints[i].Delete();
                }
            }

            base.OnDelete();
        }

        public static void ChangeHue(CustomGuildStone g, int newHue, bool clothes)
        {
            if (g is GovernmentEntity)
            {
                GovernmentEntity gov = g as GovernmentEntity;

                if (clothes)
                {
                    if (gov.Employees != null)
                    {
                        foreach (Mobile m in gov.Employees)
                        {
                            if (!m.Deleted && m is BaseCreature)
                            {
                                (m as BaseCreature).DyeClothes = newHue;
                            }
                        }
                    }
                    if (gov.TradeAdvisor != null && !gov.TradeAdvisor.Deleted)
                        gov.TradeAdvisor.DyeClothes = newHue;
                    if (gov.MilitaryAdvisor != null && !gov.MilitaryAdvisor.Deleted)
                        gov.MilitaryAdvisor.DyeClothes = newHue;
                }
                else
                {
                    if (gov.Employees != null)
                    {
                        foreach (Mobile m in gov.Employees)
                        {
                            if (!m.Deleted && m is BaseCreature)
                            {
                                (m as BaseCreature).DyeArmour = newHue;
                            }
                        }
                    }
                    if (gov.TradeAdvisor != null && !gov.TradeAdvisor.Deleted)
                        gov.TradeAdvisor.DyeArmour = newHue;
                    if (gov.MilitaryAdvisor != null && !gov.MilitaryAdvisor.Deleted)
                        gov.MilitaryAdvisor.DyeArmour = newHue;
                }
            }
        }

        public GovernmentEntity( Serial serial ) : base( serial ) 
  		{ 
 		}

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)7 );

            #region Version 7

            TradeInfo.Serialize(writer, TradeInformation);

            #endregion

            #region Version 6
            if (m_Employees == null)
                m_Employees = new List<Mobile>();
            writer.Write((int)m_Employees.Count);
            foreach (Mobile m in m_Employees)
                writer.Write((Mobile)m);

            #endregion

            #region Version 5
            writer.Write((TradeAdvisor)TradeAdvisor);

            writer.Write((int)MilitarySpawners.Count);
            for (int i = 0; i < MilitarySpawners.Count; i++)
                writer.Write((MilitarySpawner)MilitarySpawners[i]);

            writer.Write((int)WayPoints.Count);
            for (int i = 0; i < WayPoints.Count; i++)
                writer.Write((MilitaryWayPoint)WayPoints[i]);

            writer.Write((int)m_Nation);

            writer.Write((bool)m_CityGovernment);

            writer.Write(m_Resources.Count);
            foreach (KeyValuePair<ResourceType, int> kvp in m_Resources)
            {
                writer.Write((int)kvp.Key);
                writer.Write((int)kvp.Value);
            }

            writer.Write((int)m_Reports.Count);
            foreach (ReportInfo r in m_Reports)
            {
                ReportInfo.Serialize(writer, r);
            }

            writer.Write((MilitaryAdvisor)m_MilitaryAdvisor);

            MilitaryInfo.Serialize(writer, m_MilitaryPolicies);
            #endregion
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
            switch (version)
            {
                case 7:
                    {
                        #region Version 7
                        m_TradeInformation = new TradeInfo();
                        TradeInfo.Deserialize(reader, m_TradeInformation);
                        #endregion
                        goto case 6;
                    }
                case 6:
                    {
                        #region Version 6
                        m_Employees = new List<Mobile>();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Mobile e = reader.ReadMobile();
                            if(!m_Employees.Contains(e))
                                m_Employees.Add(e);
                        }
                        #endregion
                        goto case 5;
                    }
                case 5:
                    {
                        #region Version 5
                        TradeAdvisor = (TradeAdvisor)reader.ReadMobile();
                        int count = 0;

                        m_MilitarySpawners = new List<MilitarySpawner>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            object o = reader.ReadItem();
                            if (o != null)
                                m_MilitarySpawners.Add((MilitarySpawner)o);
                        }

                        m_WayPoints = new List<MilitaryWayPoint>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            object o = reader.ReadItem();
                            if (o != null)
                                m_WayPoints.Add((MilitaryWayPoint)o);
                        }

                        m_Nation = (Nation)reader.ReadInt();
                        m_CityGovernment = reader.ReadBool();

                        m_Resources = new Dictionary<ResourceType, int>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            ResourceType r = (ResourceType)reader.ReadInt();
                            int a = reader.ReadInt();
                            m_Resources.Add(r, a);
                        }

                        m_Reports = new List<ReportInfo>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            ReportInfo r = new ReportInfo(null, false, false);
                            ReportInfo.Deserialize(reader, r);
                            m_Reports.Add(r);
                        }

                        m_MilitaryAdvisor = (MilitaryAdvisor)reader.ReadMobile();

                        m_MilitaryPolicies = new MilitaryInfo(this);
                        MilitaryInfo.Deserialize(reader, m_MilitaryPolicies);

                        Governments.Add(this);
                        #endregion Version 5
                        break;
                    }
            }
        }
    }

    [PropertyObject]
    public class MilitaryInfo
    {
        private GovernmentEntity m_Government;
        private List<string> m_KillIndividualOnSight = new List<string>(); //The list of individuals who will be attacked on being sighted.
        private List<string> m_JailIndividualOnSight = new List<string>(); //The list of individuals who will be jailed on being sighted.
        private List<Nation> m_KillNationOnSight = new List<Nation>(); //The list of Nations that will be attacked on being sighted.
        private List<Nation> m_JailNationOnSight = new List<Nation>(); //The list of Nations that will be jailed on being sighted.
        private List<string> m_Exceptions = new List<string>(); //The list of individuals who are exceptions to the Nation kill/jail lists.

        public GovernmentEntity Government { get { return m_Government; } set { m_Government = value; } }
        public List<string> KillIndividualOnSight { get { return m_KillIndividualOnSight; } set { m_KillIndividualOnSight = value; } }
        public List<string> JailIndividualOnSight { get { return m_JailIndividualOnSight; } set { m_JailIndividualOnSight = value; } }
        public List<Nation> KillNationOnSight { get { return m_KillNationOnSight; } set { m_KillNationOnSight = value; } }
        public List<Nation> JailNationOnSight { get { return m_JailNationOnSight; } set { m_JailNationOnSight = value; } }
        public List<string> Exceptions { get { return m_Exceptions; } set { m_Exceptions = value; } }

        public MilitaryInfo(GovernmentEntity g)
        {
            m_Government = g;
        }

        public bool AttackOnSight(Mobile m)
        {
            if (m == null || m.Deleted || !m.Alive)
                return false;

            if (m.AccessLevel > AccessLevel.Player)
                return false;

            if (m is PlayerMobile)
            {
                PlayerMobile pm = m as PlayerMobile;

                if(m_Exceptions.Contains(pm.Name))
                    return false;

                foreach(CustomGuildStone g in Government.AlliedGuilds)
                {
                    if(CustomGuildStone.IsGuildMember(pm, g))
                        return false;
                }

                if(m_KillIndividualOnSight.Contains(pm.Name.ToString()) || m_JailIndividualOnSight.Contains(pm.Name.ToString()))
                    return true;
                else if(m_KillNationOnSight.Contains(pm.Nation) || m_JailNationOnSight.Contains(pm.Nation))
                    return true;
                else
                {
                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if (g is GovernmentEntity && CustomGuildStone.IsGuildMember(pm, g))
                        {
                            if( pm.CustomGuilds[g].ActiveTitle && (m_KillNationOnSight.Contains((g as GovernmentEntity).Nation) || m_JailNationOnSight.Contains((g as GovernmentEntity).Nation)))
                                return true;
                        }
                    }
                }

                return false;
            }
            else if (m is BaseCreature)
            {
                if( m is Soldier && (m as Soldier).Government != null && !(m as Soldier).Government.Deleted)
                    if( m_KillNationOnSight.Contains( (m as Soldier).Government.Nation ) || m_JailNationOnSight.Contains( (m as Soldier).Government.Nation ) )
                        if( !Government.AlliedGuilds.Contains((m as Soldier).Government) )
                            return true;

                if( !(m as BaseCreature).Controlled )
                {
                    if( m_KillNationOnSight.Contains( (m as BaseCreature).Nation ) || m_JailNationOnSight.Contains( (m as BaseCreature).Nation ) )
                        return true;

                    if(m_KillIndividualOnSight.Contains(m.Name) || m_JailIndividualOnSight.Contains(m.Name))
                        return true;
                }

                return false;
            }

            return false;
        }

        public static void Serialize(GenericWriter writer, MilitaryInfo info)
        {
            writer.Write((int)0); //version

            #region version 0
            writer.Write((GovernmentEntity)info.Government);

            writer.Write((int)info.KillIndividualOnSight.Count);
            for (int i = 0; i < info.KillIndividualOnSight.Count; i++)
                writer.Write((string)info.KillIndividualOnSight[i]);
            writer.Write((int)info.JailIndividualOnSight.Count);
            for (int i = 0; i < info.JailIndividualOnSight.Count; i++)
                writer.Write((string)info.JailIndividualOnSight[i]);
            writer.Write((int)info.KillNationOnSight.Count);
            for(int i = 0; i < info.KillNationOnSight.Count; i++)
                writer.Write((int)info.KillNationOnSight[i]);
            writer.Write((int)info.JailNationOnSight.Count);
            for(int i = 0; i < info.JailNationOnSight.Count; i++)
                writer.Write((int)info.JailNationOnSight[i]);
            writer.Write((int)info.Exceptions.Count);
            for (int i = 0; i < info.Exceptions.Count; i++)
                writer.Write((string)info.Exceptions[i]);
            #endregion
        }
        public static void Deserialize(GenericReader reader, MilitaryInfo info)
        {
            int version = reader.ReadInt(); //version

            switch (version)
            {
                case 0:
                    {
                        info.Government = (GovernmentEntity)reader.ReadItem();

                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.KillIndividualOnSight.Add(reader.ReadString());
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.JailIndividualOnSight.Add(reader.ReadString());
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.KillNationOnSight.Add((Nation)reader.ReadInt());
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.JailNationOnSight.Add((Nation)reader.ReadInt());
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.Exceptions.Add(reader.ReadString());

                        break;
                    }
            }
        }
    }

    [PropertyObject]
    public class TradeInfo
    {
        #region Private Variable Declaration
        private List<string> m_NoBusinessList = new List<string>();
        private List<Nation> m_NoBusinessNations = new List<Nation>();
        private int m_BudgetMinimum;
        private int m_MetalSalesPrice;
        private int m_MetalPurchasePrice;
        private int m_MetalMinimum;
        private int m_ClothSalesPrice;
        private int m_ClothPurchasePrice;
        private int m_ClothMinimum;
        private int m_WoodSalesPrice;
        private int m_WoodPurchasePrice;
        private int m_WoodMinimum;

        private int m_VendorPrice;
        private int m_WageEarnerPrice;
        private int m_MaxWageEarners;
        private int m_SlavePrice;
        private int m_MaxSlaves;

        private bool m_SellsSlaves;
        private bool m_SellsWageEarners;

        private int m_VendorWages;
        private int m_Taxes;
        private bool m_FlatTax;

        #endregion

        #region Get/Sets
        public List<string> NoBusinessList { get { return m_NoBusinessList; } set { m_NoBusinessList = value; } }
        public List<Nation> NoBusinessNations { get { return m_NoBusinessNations; } set { m_NoBusinessNations = value; } }
        public int BudgetMinimum { get { return m_BudgetMinimum; } set { m_BudgetMinimum = value; } }
        public int MetalSalesPrice { get { return m_MetalSalesPrice; } set { m_MetalSalesPrice = value; } }
        public int MetalPurchasePrice { get { return m_MetalPurchasePrice; } set { m_MetalPurchasePrice = value; } }
        public int MetalMinimum { get { return m_MetalMinimum; } set { m_MetalMinimum = value; } }
        public int ClothSalesPrice { get { return m_ClothSalesPrice; } set { m_ClothSalesPrice = value; } }
        public int ClothPurchasePrice { get { return m_ClothPurchasePrice; } set { m_ClothPurchasePrice = value; } }
        public int ClothMinimum { get { return m_ClothMinimum; } set { m_ClothMinimum = value; } }
        public int WoodSalesPrice { get { return m_WoodSalesPrice; } set { m_WoodSalesPrice = value; } }
        public int WoodPurchasePrice { get { return m_WoodPurchasePrice; } set { m_WoodPurchasePrice = value; } }
        public int WoodMinimum { get { return m_WoodMinimum; } set { m_WoodMinimum = value; } }

        public int VendorPrice { get { return m_VendorPrice; } set { m_VendorPrice = value; } }
        public int WageEarnerPrice { get { return m_WageEarnerPrice; } set { m_WageEarnerPrice = value; } }
        public int MaxWageEarners { get { if(!m_SellsWageEarners) return 0; return m_MaxWageEarners; } set { m_MaxWageEarners = value; } }
        public int SlavePrice { get { return m_SlavePrice; } set { m_SlavePrice = value; } }
        public int MaxSlaves 
        { 
            get 
            { 
                if (!m_SellsSlaves) 
                    return 0; 
                return m_MaxSlaves; 
            } 
            set 
            { 
                m_MaxSlaves = value;
            } 
        }

        public bool SellsSlaves { get { return m_SellsSlaves; } set { m_SellsSlaves = value; } }
        public bool SellsWageEarners { get { return m_SellsWageEarners; } set { m_SellsWageEarners = value; } }

        public int VendorWages { get { return m_VendorWages; } set { m_VendorWages = value; } }
        public int Taxes { get { return m_Taxes; } set { m_Taxes = value; } }
        public bool FlatTax { get { return m_FlatTax; } set { m_FlatTax = value; } }

        #endregion

        public TradeInfo()
        {
            m_BudgetMinimum = 0;
            m_MetalSalesPrice = 100;
            m_MetalPurchasePrice = 0;
            m_MetalMinimum = 0;
            m_ClothSalesPrice = 100;
            m_ClothPurchasePrice = 0;
            m_ClothMinimum = 0;
            m_WoodSalesPrice = 100;
            m_WoodPurchasePrice = 0;
            m_WoodMinimum = 0;
            m_WageEarnerPrice = 1000;
            m_MaxWageEarners = 0;
            m_SlavePrice = 1000;
            m_MaxSlaves = 0;
            m_SellsSlaves = false;
            m_SellsWageEarners = false;
            m_VendorPrice = 1000;
            m_VendorWages = 100;
            m_Taxes = 0;
            m_FlatTax = true;

            m_NoBusinessList = new List<string>();
            m_NoBusinessNations = new List<Nation>();
        }

        public static ResourceType GetType(CraftResource r)
        {
            switch (r)
            {
                case CraftResource.Copper:
                case CraftResource.Tin:
                case CraftResource.Iron:
                case CraftResource.Silver:
                case CraftResource.Gold: return ResourceType.Metals;
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.Silk:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather: return ResourceType.Cloth;
                case CraftResource.Oak:
                case CraftResource.Yew:
                case CraftResource.Redwood:
                case CraftResource.Ash: return ResourceType.Wood;
                default: return ResourceType.None;
            }
        }

        public static Type GetResourceObject(CraftResource r)
        {
            switch (r)
            {
                case CraftResource.Copper: return typeof(CopperIngot);
                case CraftResource.Tin: return typeof(TinIngot);
                case CraftResource.Iron: return typeof(IronIngot);
                case CraftResource.Silver: return typeof(SilverIngot);
                case CraftResource.Gold: return typeof(GoldIngot);
                case CraftResource.Cotton: return typeof(Cotton);
                case CraftResource.Wool: return typeof(Wool);
                case CraftResource.Silk: return typeof(SpidersSilk);
                case CraftResource.RegularLeather: return typeof(Leather);
                case CraftResource.ThickLeather: return typeof(ThickLeather);
                case CraftResource.BeastLeather: return typeof(BeastLeather);
                case CraftResource.ScaledLeather: return typeof(ScaledLeather);
                case CraftResource.Oak: return typeof(Log);
                case CraftResource.Yew: return typeof(YewLog);
                case CraftResource.Redwood: return typeof(RedwoodLog);
                case CraftResource.Ash: return typeof(AshLog);
                default: return null;
            }
        }

        public static double GetDivisor(CraftResource r)
        {
            double div = 1;

            switch (r)
            {
                case CraftResource.Copper: div = 5; break;
                case CraftResource.Tin: div = 5; break;
                case CraftResource.Iron: div = 10; break;
                case CraftResource.Silver: div = 1; break;
                case CraftResource.Gold: div = .1; break;
                case CraftResource.Cotton: div = 12; break;
                case CraftResource.Wool: div = 12; break;
                case CraftResource.Silk: div = 4; break;
                case CraftResource.RegularLeather: div = 10; break;
                case CraftResource.ThickLeather: div = 8; break;
                case CraftResource.BeastLeather: div = 6; break;
                case CraftResource.ScaledLeather: div = 2; break;
                case CraftResource.Oak: div = 10; break;
                case CraftResource.Yew: div = 9; break;
                case CraftResource.Redwood: div = 8; break;
                case CraftResource.Ash: div = 7; break;
            }

            return div;
        }

        public static string GetString(CraftResource r)
        {
            switch (r)
            {
                case CraftResource.Copper: return "copper";
                case CraftResource.Tin: return "tin";
                case CraftResource.Iron: return "iron";
                case CraftResource.Silver: return "silver";
                case CraftResource.Gold: return "gold";
                case CraftResource.Cotton: return "cotton";
                case CraftResource.Wool: return "wool";
                case CraftResource.Silk: return "silk";
                case CraftResource.RegularLeather: return "regular leather";
                case CraftResource.ThickLeather: return "thick leather";
                case CraftResource.BeastLeather: return "beast leather";
                case CraftResource.ScaledLeather: return "scaled leather";
                case CraftResource.Oak: return "oak logs";
                case CraftResource.Yew: return "yew logs";
                case CraftResource.Redwood: return "redwood logs";
                case CraftResource.Ash: return "ash logs";
                default: return "null";
            }
        }

        public static int GetMinimum(CraftResource r, GovernmentEntity g)
        {
            switch (r)
            {
                case CraftResource.Copper: 
                case CraftResource.Tin:
                case CraftResource.Iron:
                case CraftResource.Silver:
                case CraftResource.Gold: return g.TradeInformation.MetalMinimum;
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.Silk:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather: return g.TradeInformation.ClothMinimum;
                case CraftResource.Oak:
                case CraftResource.Yew:
                case CraftResource.Redwood:
                case CraftResource.Ash: return g.TradeInformation.WoodMinimum;
                default: return 0;
            }
        }

        public static int GetSellPrice(CraftResource r, GovernmentEntity g)
        {
            switch (r)
            {
                case CraftResource.Copper:
                case CraftResource.Tin:
                case CraftResource.Iron:
                case CraftResource.Silver:
                case CraftResource.Gold: return g.TradeInformation.MetalSalesPrice;
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.Silk:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather: return g.TradeInformation.ClothSalesPrice;
                case CraftResource.Oak:
                case CraftResource.Yew:
                case CraftResource.Redwood:
                case CraftResource.Ash: return g.TradeInformation.WoodSalesPrice;
                default: return 0;
            }
        }

        public static int GetBuyPrice(CraftResource r, GovernmentEntity g)
        {
            switch (r)
            {
                case CraftResource.Copper:
                case CraftResource.Tin:
                case CraftResource.Iron:
                case CraftResource.Silver:
                case CraftResource.Gold: return g.TradeInformation.MetalPurchasePrice;
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.Silk:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather: return g.TradeInformation.ClothPurchasePrice;
                case CraftResource.Oak:
                case CraftResource.Yew:
                case CraftResource.Redwood:
                case CraftResource.Ash: return g.TradeInformation.WoodSalesPrice;
                default: return 0;
            }
        }

        public static bool CanDoBusiness(GovernmentEntity g, PlayerMobile pm)
        {
            if (CustomGuildStone.IsGuildEconomic(pm, g))
                return true;

            foreach(string name in g.TradeInformation.NoBusinessList)
            {
                if(name.ToLower() == pm.Name.ToLower())
                    return false;
            }
            foreach(Nation n in g.TradeInformation.NoBusinessNations)
            {
                if(n == pm.GetDisguisedNation())
                    return false;
            }
            if(g.MilitaryPolicies.KillIndividualOnSight.Contains(pm.Name))
                return false;
            if(g.MilitaryPolicies.JailIndividualOnSight.Contains(pm.Name))
                return false;
            if(!g.MilitaryPolicies.Exceptions.Contains(pm.Name))
            {
                if(g.MilitaryPolicies.JailNationOnSight.Contains(pm.GetDisguisedNation()))
                    return false;
                if(g.MilitaryPolicies.KillNationOnSight.Contains(pm.GetDisguisedNation()))
                    return false;
            }
            foreach(CustomGuildStone enemy in g.EnemyGuilds)
            {
                if(CustomGuildStone.IsGuildMember(pm, enemy) && pm.CustomGuilds[enemy].ActiveTitle)
                    return false;
            }
            return true;
        }

        public static bool SellResource(TradeAdvisor trader, PlayerMobile buyer, CraftResource r, int amount)
        {
            if(trader.Government == null || trader.Government.Deleted)
                return false;

            if (trader.Government.TradeInformation == null)
                return false;

            if (!CanDoBusiness(trader.Government, buyer))
            {
                trader.Say("I cannot do business with you.");
                return false;
            }

            #region Amount Checking
            if (trader.Government.Resources[GetType(r)] < 1)
            {
                trader.Say("I have no " + GetType(r).ToString() + " in stock.");
                return false;
            }
            else if (amount / GetDivisor(r) > trader.Government.Resources[GetType(r)])
            {
                trader.Say("I only have " + trader.Government.Resources[GetType(r)] + " units of " + GetType(r).ToString() + " in stock.");
                return false;
            }
            else if (amount < GetDivisor(r))
            {
                trader.Say("You must purchase at least " + GetDivisor(r).ToString() + " " + GetString(r) + ".");
                return false;
            }
            else if (r != CraftResource.Gold && amount % GetDivisor(r) > 0)
            {
                trader.Say(r.ToString() + " is handled in units of " + GetDivisor(r).ToString() + ".");
                return false;
            }
            #endregion

            if (CustomGuildStone.IsGuildEconomic(buyer, trader.Government))
            {
                Item res = (Item)Activator.CreateInstance(GetResourceObject(r), amount);
                buyer.AddToBackpack(res);
                trader.Say("Of course you may freely access " + trader.Government.Name + "'s resources.");
                trader.Government.Resources[GetType(r)] -= (int)(amount / GetDivisor(r));
                return true;
            }
            else
            {
                if (trader.Government.Resources[GetType(r)] - (amount / GetDivisor(r)) < GetMinimum(r, trader.Government))
                {
                    trader.Say("I have been ordered not to go below " + GetMinimum(r, trader.Government).ToString() + " units of " + GetType(r).ToString() + " in my stock.");
                    return false;

                }
                else if (buyer.Backpack.GetAmount(typeof(Copper)) < ((amount / GetDivisor(r)) * GetSellPrice(r, trader.Government)))
                {
                    trader.Say("You don't have enough coin to buy " + (amount / GetDivisor(r)) + " units of " + GetType(r).ToString() + ".");
                    return false;
                }
                else if (buyer.Backpack.ConsumeTotal(typeof(Copper), ((int)(amount / GetDivisor(r)) * GetSellPrice(r, trader.Government))))
                {
                    #region Successful Sale of Resources to a Player
                    switch (Utility.Random(3))
                    {
                        case 0: buyer.PlaySound(0x2E5); break;
                        case 1: buyer.PlaySound(0x2E6); break;
                        case 2: buyer.PlaySound(0x2E7); break;
                    }

                    Item res = (Item)Activator.CreateInstance(GetResourceObject(r), amount);
                    buyer.AddToBackpack(res);
                    trader.Government.Resources[GetType(r)] -= (int)(amount / GetDivisor(r));
                    return true;
                    #endregion
                }
                else
                {
                    trader.Say("You do not have enough copper coins.");
                    return false;
                }
            }
        }

        public static bool BuyResource(TradeAdvisor trader, PlayerMobile seller, CraftResource r, int amount)
        {
            if (!CanDoBusiness(trader.Government, seller))
            {
                trader.Say("I cannot do business with you.");
                return false;
            }

            int itemCount = 0;
            foreach (Item i in seller.Backpack.Items)
            {
                if (i.GetType() == GetResourceObject(r))
                    itemCount += i.Amount;
            }

            if (itemCount <= 0)
            {
                trader.Say("You do not have any " + GetString(r));
                return false;
            }
            else if (itemCount < amount)
                amount = itemCount;

            if (amount < GetDivisor(r))
            { 
                trader.Say("You must have " + GetDivisor(r).ToString() + " " + GetString(r) + " to make a sale; you only have " + amount.ToString() + " " + GetString(r) + ".");
                return false;
            }
            else if (amount / GetDivisor(r) < 1)
            { 
                trader.Say(amount.ToString() + " " + GetString(r) + " is not enough to make a sale.");
                return false;
            }
            else if (r != CraftResource.Gold && amount % GetDivisor(r) > 0) 
            { 
                trader.Say(GetString(r) + " is handled in units of " + GetDivisor(r).ToString() + ".");
                return false;
            }
            else if ((trader.Government.GetTreasuryBalance() - ((amount / GetDivisor(r)) * GetBuyPrice(r, trader.Government))) < trader.Government.TradeInformation.BudgetMinimum)
            { 
                trader.Say("I've been ordered to buy for no more than " + trader.Government.TradeInformation.BudgetMinimum.ToString() + " copper.");
                return false;
            }
            else if ((amount / GetDivisor(r)) * GetBuyPrice(r, trader.Government) > trader.Government.GetTreasuryBalance())
            { 
                trader.Say("We've not enough in our treasury to make that purchase.");
                return false;
            }

            switch (Utility.Random(3))
            {
                case 0: seller.PlaySound(0x2E4); break;
                case 1: seller.PlaySound(0x2E5); break;
                case 2: seller.PlaySound(0x2E6); break;
            }

            seller.Backpack.ConsumeTotal(GetResourceObject(r), amount);
            trader.Government.Resources[GetType(r)] += (int)(amount / GetDivisor(r));

            int giveCopper = (int)((amount / GetDivisor(r)) * GetBuyPrice(r, trader.Government));
            if (giveCopper > 60000)
            {
                while (giveCopper > 60000)
                {
                    seller.Backpack.AddItem(new Copper(trader.Government.WithdrawFromTreasury(giveCopper / 2)));
                    giveCopper -= (giveCopper / 2);
                }

                if (giveCopper > 0)
                    seller.Backpack.AddItem(new Copper(trader.Government.WithdrawFromTreasury(giveCopper)));
            }
            else
                seller.Backpack.AddItem(new Copper(trader.Government.WithdrawFromTreasury(giveCopper)));

            return true;
        }

        public static bool DonateResource(TradeAdvisor trader, PlayerMobile giver, CraftResource r, int amount)
        {
            if (!CanDoBusiness(trader.Government, giver))
            {
                trader.Say("I cannot do business with you.");
                return false;
            }

            int itemCount = 0;
            foreach (Item i in giver.Backpack.Items)
            {
                if (i.GetType() == GetResourceObject(r))
                    itemCount += i.Amount;
            }

            if (itemCount < amount)
            {
                amount = itemCount;
            }

            if (amount / GetDivisor(r) < 1)
            {
                trader.Say("You don't have enough " + GetString(r) + " to make a donation.");
                return false;
            }
            else if (amount < GetDivisor(r))
            {
                trader.Say("You must have " + GetDivisor(r).ToString() + " to make a donation.");
                return false;
            }
            else if (r != CraftResource.Gold && (amount % GetDivisor(r)) > 0)
            {
                trader.Say(GetString(r).ToUpper() + " is handled in units of " + GetDivisor(r).ToString() + ".");
                return false;
            }

            giver.Backpack.ConsumeTotal(GetResourceObject(r), amount);
            trader.Government.Resources[GetType(r)] += (int)(amount / GetDivisor(r));
            trader.Say(trader.Government.Name + " thanks you for your generosity.");
            return true;
        }

        public static void Serialize(GenericWriter writer, TradeInfo info)
        {
            writer.Write((int)0); // version

            writer.Write((int)info.BudgetMinimum);
            writer.Write((int)info.MetalSalesPrice);
            writer.Write((int)info.MetalPurchasePrice);
            writer.Write((int)info.MetalMinimum);
            writer.Write((int)info.ClothSalesPrice);
            writer.Write((int)info.ClothPurchasePrice);
            writer.Write((int)info.ClothMinimum);
            writer.Write((int)info.WoodSalesPrice);
            writer.Write((int)info.WoodPurchasePrice);
            writer.Write((int)info.WoodMinimum);
            writer.Write((int)info.VendorPrice);
            writer.Write((int)info.WageEarnerPrice);
            writer.Write((int)info.MaxWageEarners);
            writer.Write((int)info.SlavePrice);
            writer.Write((int)info.MaxSlaves);
            writer.Write((bool)info.SellsSlaves);
            writer.Write((bool)info.SellsWageEarners);
            writer.Write((int)info.VendorWages);
            writer.Write((int)info.Taxes);
            writer.Write((bool)info.FlatTax);

            writer.Write((int)info.NoBusinessList.Count);
            foreach (string name in info.NoBusinessList)
                writer.Write((string)name);

            writer.Write((int)info.NoBusinessNations.Count);
            foreach (Nation n in info.NoBusinessNations)
                writer.Write((int)n);
        }

        public static void Deserialize(GenericReader reader, TradeInfo info)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        info.BudgetMinimum = reader.ReadInt();
                        info.MetalSalesPrice = reader.ReadInt();
                        info.MetalPurchasePrice = reader.ReadInt();
                        info.MetalMinimum = reader.ReadInt();
                        info.ClothSalesPrice = reader.ReadInt();
                        info.ClothPurchasePrice = reader.ReadInt();
                        info.ClothMinimum = reader.ReadInt();
                        info.WoodSalesPrice = reader.ReadInt();
                        info.WoodPurchasePrice = reader.ReadInt();
                        info.WoodMinimum = reader.ReadInt();
                        info.VendorPrice = reader.ReadInt();
                        info.WageEarnerPrice = reader.ReadInt();
                        info.MaxWageEarners = reader.ReadInt();
                        info.SlavePrice = reader.ReadInt();
                        info.MaxSlaves = reader.ReadInt();
                        info.SellsSlaves = reader.ReadBool();
                        info.SellsWageEarners = reader.ReadBool();
                        info.VendorWages = reader.ReadInt();
                        info.Taxes = reader.ReadInt();
                        info.FlatTax = reader.ReadBool();

                        int count = 0;
                        info.NoBusinessList = new List<string>();
                        count = reader.ReadInt();
                        for(int i = 0; i < count; i++)
                        {
                            string name = reader.ReadString();
                            info.NoBusinessList.Add(name);
                        }
                        info.NoBusinessNations = new List<Nation>();
                        count = reader.ReadInt();
                        for(int i = 0; i < count; i++)
                        {
                            Nation nat = (Nation)reader.ReadInt();
                            info.NoBusinessNations.Add(nat);
                        }
                        break;
                    }
            }
        }
    }
}
