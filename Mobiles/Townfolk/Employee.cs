using System;
using System.Collections.Generic;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Harvest;
using Server.Gumps;
using Server.Targeting;
using Server.Prompts;
using Server.Engines.Craft;

namespace Server.Mobiles
{
    public class Employee : BaseKhaerosMobile
    {
        public static TimeSpan EmployeePayInterval { get { return TimeSpan.FromHours(24); } }

        #region Payment Variables
        private int m_HeldCopper;
        private int m_HeldFood;
        private DateTime m_LastPaid;
        private int m_Wage = 0;
        private int m_WagesPaid = 0;
        private bool m_IsSlave;
        #endregion

        #region Resource Gathering Variables
        private CraftResource m_ToGather = CraftResource.Copper;
        private int m_HeldResource;
        private EmployeeGatherTimer m_Timer;
        private ResourceNode m_Node;
        #endregion

        #region Public Get/Sets
        [CommandProperty(AccessLevel.GameMaster)]
        public int HeldResources { get { return m_HeldResource; } set { m_HeldResource = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int HeldCopper { get { return m_HeldCopper; } set { m_HeldCopper = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int HeldFood { get { return m_HeldFood; } set { m_HeldFood = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource ToGather 
        { 
            get { return m_ToGather; } 
            set 
            {
                if (value != m_ToGather)
                    m_HeldResource = 0;
                m_ToGather = value;

                switch (m_ToGather)
                {
                    case CraftResource.Copper:
                    case CraftResource.Tin:
                    case CraftResource.Iron:
                    //case CraftResource.Silver:
                    //case CraftResource.Gold:
                        {
                            Item toRemove = FindItemOnLayer(Layer.FirstValid);
                            if (toRemove == null)
                                toRemove = FindItemOnLayer(Layer.TwoHanded);
                            if (toRemove != null && !toRemove.Deleted)
                                toRemove.Delete();
                            EquipItem(new Pickaxe());
                            break;
                        }
                    case CraftResource.Cotton:
                    case CraftResource.Wool:
                    case CraftResource.Silk:
                    case CraftResource.RegularLeather:
                    case CraftResource.ThickLeather:
                    case CraftResource.BeastLeather:
                    case CraftResource.ScaledLeather:
                        {
                            Item toRemove = FindItemOnLayer(Layer.FirstValid);
                            if (toRemove == null)
                                toRemove = FindItemOnLayer(Layer.TwoHanded);
                            if (toRemove != null && !toRemove.Deleted)
                                toRemove.Delete();
                            EquipItem(new SkinningKnife());
                            break;
                        }
                    case CraftResource.Oak:
                    case CraftResource.Yew:
                    case CraftResource.Redwood:
                    case CraftResource.Ash:
                        {
                            Item toRemove = FindItemOnLayer(Layer.FirstValid);
                            if (toRemove == null)
                                toRemove = FindItemOnLayer(Layer.TwoHanded);
                            if (toRemove != null && !toRemove.Deleted)
                                toRemove.Delete();
                            EquipItem(new Hatchet());
                            break;
                        }
                }
            } 
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int GatherRate
        {
            get
            {
                if (m_Node == null || m_Node.Deleted)
                    return 0;
                if (TradeInfo.GetType(m_ToGather) != m_Node.Resource)
                    return 0;

                int baseRate = 0;
                double nodePerc = 0;
                double resMult = 1;
                int compDiv = 1;

                #region baseRate: Calculating the base rate for the employee, determined by wage, slavery, soldiers nearby, hunger... etc.
                if (!m_IsSlave)
                {
                    baseRate = m_Wage;
                }
                else
                {
                    IPooledEnumerable eable = Map.GetMobilesInRange(Location, 16);
                    foreach (Mobile m in eable)
                    {
                        if (m is Soldier)
                        {
                            Soldier sol = m as Soldier;
                            if (sol.Government != null && !sol.Government.Deleted)
                            {
                                if (m_Node.Government != null && !m_Node.Government.Deleted)
                                {
                                    if (sol.Government == m_Node.Government)
                                        baseRate += 5;
                                }
                            }
                        }
                    }
                    eable.Free();
                    baseRate += Hunger;
                }
                #endregion

                #region nodePerc: Calculating the percentage-adjustment the employe receives to their gahtering rate, based on the node's regular output.
                if (m_Node != null && !m_Node.Deleted)
                    nodePerc = (m_Node.ProductionRate * 0.002);
                else
                    return 0;
                #endregion

                #region compDiv: Counting up the competing employees in the area.
                foreach (Mobile m in Map.GetMobilesInRange(this.Location, 16))
                {
                    if (m is Employee && (m as Employee).Node == Node && m != this)
                        compDiv++;
                }
                #endregion

                #region resMult: Calculating the resource-amount-adjustment, based on the type of resource they are gathering.
                switch (m_ToGather)
                {
                    case CraftResource.Copper: resMult = 10; break;
                    case CraftResource.Tin: resMult = 10; break;
                    case CraftResource.Iron: resMult = 20; break;
                    case CraftResource.Silver: resMult = 1; break;
                    case CraftResource.Gold: resMult = 0.1; break;
                    case CraftResource.Cotton: resMult = 25; break;
                    case CraftResource.Wool: resMult = 25; break;
                    case CraftResource.Silk: resMult = 3; break;
                    case CraftResource.RegularLeather: resMult = 20; break;
                    case CraftResource.ThickLeather: resMult = 16; break;
                    case CraftResource.BeastLeather: resMult = 12; break;
                    case CraftResource.ScaledLeather: resMult = 8; break;
                    case CraftResource.Oak: resMult = 20; break;
                    case CraftResource.Yew: resMult = 18; break;
                    case CraftResource.Redwood: resMult = 16; break;
                    case CraftResource.Ash: resMult = 14; break;
                    default: break;
                }
                #endregion

                return (int)(((baseRate * resMult) * nodePerc) / compDiv);
            }
        }
        public EmployeeGatherTimer GatherTimer { get { return m_Timer; } set { m_Timer = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public ResourceNode Node { get { return m_Node; } set { m_Node = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsSlave { get { return m_IsSlave; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Wage { get { if (IsSlave) return 0; return m_Wage; } set { m_Wage = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Tax
        {
            get
            {
                if (Government == null || Government.Deleted)
                    return 0;
                else
                {
                    if (m_IsSlave)
                    {
                        return 0;
                    }
                    else
                    {
                        if (Government.TradeInformation.FlatTax)
                        {
                            return Government.TradeInformation.Taxes;
                        }
                        else
                        {
                            return (int)(m_Wage * (0.01 * Government.TradeInformation.Taxes));
                        }
                    }
                }
            }
        }
        #endregion

        public Employee(PlayerMobile owner, EmployeeContract contract) : base(contract.Government.Nation)
        {
            Government = contract.Government;
            Government.Employees.Add(this);
            m_IsSlave = contract.IsSlave;
            m_LastPaid = DateTime.Now;

            if (m_IsSlave)
            {
                m_HeldFood = 20 + (Government.TradeInformation.SlavePrice / 2);
                Hunger = 40;
            }
            else
            {
                m_HeldCopper = 20 + (Government.TradeInformation.WageEarnerPrice / 2);
                m_Wage = 1;
            }

            #region Control Assignment
            ControlSlots = 0;
            Lives = 1;
            ControlMaster = owner;
            Controlled = true;
            #endregion

            #region Title Assignment
            switch (Government.Nation)
            {
                case Nation.Southern:
                    {
                        if(IsSlave)
                            Title = "the Slave";
                        else
                            Title = "the Artisan";
                        break;
                    }
                case Nation.Western:
                    {
                        if(IsSlave)
                            Title = "the Toiler";
                        else
                            Title = "the Worker";
                        break;
                    }
                case Nation.Haluaroc:
                    {
                        if(IsSlave)
                            Title = "the Slave";
                        else
                            Title = "the Laborer";
                        break;
                    }
                case Nation.Mhordul:
                    {
                        if(IsSlave)
                            Title = "the Thrall";
                        else
                            Title = "the Hireling";
                        break;
                    }
                case Nation.Tirebladd:
                    {
                        if (IsSlave)
                            Title = "the Peon";
                        else
                            Title = "the Hired Hand";
                        break;
                    }
                case Nation.Northern:
                    {
                        if(IsSlave)
                            Title = "the Laborer";
                        else
                            Title = "the Peasant";
                        break;
                    }
 /*                case Nation.Imperial:
                    {
                        if(IsSlave)
                            Title = "the Vassal";
                        else
                            Title = "the Prole";
                        break;
                    }
                case Nation.Sovereign:
                    {
                        if(IsSlave)
                            Title = "the Bondservant";
                        else
                            Title = "the Roustabout";
                        break;
                    } */
                default: break;
            }
            #endregion

            if(!m_IsSlave)
                BaseKhaerosMobile.RandomCrafterClothes(this, Government.Nation);
            else
                BaseKhaerosMobile.RandomPoorClothes(this, Government.Nation);
        }

        #region Payment & Resource Handling
        public bool TryToPay()
        {
            if (m_LastPaid + EmployeePayInterval < DateTime.Now)
            {
                if (m_IsSlave)
                {
                    if (Hunger < 20)
                    {
                        if (m_HeldFood > 0)
                        {
                            if (m_HeldFood - (20 - Hunger) >= 0)
                            {
                                m_HeldFood -= (20 - Hunger);
                                Hunger = 20;
                            }
                            else
                            {
                                Hunger += m_HeldFood;
                                m_HeldFood = 0;
                            }
                            m_LastPaid = DateTime.Now;
                            return true;
                        }
                        else if ( Hunger > 0 )
                        {
                            m_LastPaid = DateTime.Now;
                            return true;
                        }
                    }

                    if (Hunger <= 0)
                    {
                        Kill();
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    if (HeldCopper >= ( m_Wage + Tax ) )
                    {
                        HeldCopper -= ( m_Wage + Tax );
                        Government.Treasury.AddItem(new Copper(Tax));
                        m_LastPaid = DateTime.Now;
                        return true;
                    }
                    else
                    {
                        Delete();
                        return false;
                    }
                }
            }
            return false;
        }

        public static void DoGatherAnimation(Mobile m, CraftResource r)
        {
            switch (r)
            {
                case CraftResource.Copper:
                case CraftResource.Tin:
                case CraftResource.Iron:
                //case CraftResource.Silver:
                //case CraftResource.Gold:
                    {
                            if (!m.Mounted)
                            {
                                m.Animate(11, 5, 1, true, false, 0);
                                m.PlaySound(0x33B);
                                if (Utility.RandomBool())
                                    m.PlaySound(0x125);
                                else
                                    m.PlaySound(0x126);
                            }
                        break;
                    }
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather:
                    {
                            if (!m.Mounted)
                            {
                                m.Animate(32, 5, 1, true, false, 0);
                                m.PlaySound(0x3E3);
                            }
                        break;
                    }
                case CraftResource.Oak:
                case CraftResource.Redwood:
                case CraftResource.Yew:
                case CraftResource.Ash:
                    {
                            if (!m.Mounted)
                            {
                                m.Animate(13, 5, 1, true, false, 0);
                                m.PlaySound(0x13E);
                            }

                        
                        break;
                    }
            }
        }

        public void TryToGather()
        {
            if (m_Node == null || m_Node.Deleted || !this.InRange(m_Node.Location, 20))
                return;

            int amount = 0;
            Say(amount.ToString());
            #region Finding Node; Acquiring Amount of Resource to Gather
            switch (m_ToGather)
            {
                case CraftResource.Copper:
                case CraftResource.Tin:
                case CraftResource.Iron:
                //case CraftResource.Silver:
                //case CraftResource.Gold:
                    {
                        if (m_Node.Resource == ResourceType.Metals)
                        {
                            amount += (GatherRate);
                            DoGatherAnimation(this, m_ToGather);
                        }
                        break;
                    }
                case CraftResource.Cotton:
                case CraftResource.Wool:
                case CraftResource.RegularLeather:
                case CraftResource.ThickLeather:
                case CraftResource.BeastLeather:
                case CraftResource.ScaledLeather:
                    {
                        if (m_Node.Resource == ResourceType.Cloth)
                        {
                            amount += (GatherRate);
                            DoGatherAnimation(this, m_ToGather);
                        }
                        break;
                    }
                case CraftResource.Oak:
                case CraftResource.Redwood:
                case CraftResource.Yew:
                case CraftResource.Ash:
                    {
                        if (m_Node.Resource == ResourceType.Wood)
                        {
                            amount += (GatherRate);
                            DoGatherAnimation(this, m_ToGather);
                        }
                        break;
                    }
            }
            #endregion
            Say(amount.ToString());
            HeldResources += amount;
        }

        public void WithdrawResources(Mobile to, int amt)
        {
            if (to == null || to.Deleted || !to.Alive)
                return;
            int amount = amt;
            if (HeldResources < amount)
                amount = HeldResources;
            if (amount < 1)
            {
                to.SendMessage("There is nothing to take.");
            }
            else
            {
                Item resource = Activator.CreateInstance(TradeInfo.GetResourceObject(ToGather), amount) as Item;
                to.AddToBackpack(resource);
                HeldResources -= amount;
                to.SendMessage("You successly withdraw " + amount.ToString() + " " + TradeInfo.GetString(ToGather) + " from " + Name + ". " +
                    (this.Female ? "She" : "He") + " has " + HeldResources.ToString() + " " + TradeInfo.GetString(ToGather) + " left.");
                switch (TradeInfo.GetType(m_ToGather))
                {
                    case ResourceType.Metals: to.PlaySound(0x0EE); break;
                    case ResourceType.Cloth: to.PlaySound(0x059); break;
                    case ResourceType.Wood: to.PlaySound(0x04F); break;
                }
            }
        }
        #endregion

        #region Utilities & Speech

        public override void OnDeath(Container c)
        {
            if (!m_IsSlave)
            {
                Government.Resources[ResourceType.Influence] -= 100;
                Government.Resources[TradeInfo.GetType(m_ToGather)] -= Utility.RandomMinMax(0, GatherRate);

                if (Government.Resources[ResourceType.Influence] < 0)
                    Government.Resources[ResourceType.Influence] = 0;

                if (Government.Resources[TradeInfo.GetType(m_ToGather)] < 0)
                    Government.Resources[TradeInfo.GetType(m_ToGather)] = 0;
            }

            if (Government != null && !Government.Deleted && Government.Employees.Contains(this))
                Government.Employees.Remove(this);

            if (m_WagesPaid > 0)
            {
                int payout = (int)(m_WagesPaid * (Utility.RandomMinMax(1, 50) * 0.01));
                if (payout > 0)
                    c.AddItem(new Copper(payout));
            }
            if (m_HeldResource > 0)
            {
                Item resource = Activator.CreateInstance(TradeInfo.GetResourceObject(ToGather), m_HeldResource) as Item;
                c.AddItem(resource);
            }

            if(m_Timer != null && m_Timer.Running)
                m_Timer.Stop();

            base.OnDeath(c);
        }

        public override void OnDelete()
        {
            if (Government != null && !Government.Deleted && Government.Employees.Contains(this))
                Government.Employees.Remove(this);

            if (m_Timer != null && m_Timer.Running)
                m_Timer.Stop();

            base.OnDelete();
        }

        public override bool OnDragDrop( Mobile from, Item item )
		{
			if( from is PlayerMobile)
            {
                if( item is Copper || item is Silver || item is Gold )
                {
                    if(m_IsSlave)
                    {
                        this.Say("*looks confused*");
                        return false;
                    }
                    else
                    {
                        if(item is Copper)
                            HeldCopper += item.Amount;
                        else if (item is Silver)
                            HeldCopper += (item.Amount * 10);
                        else if (item is Gold)
                            HeldCopper += (item.Amount * 100);

                        item.Delete();

                        this.Say("I'll put this to good use in " + ( ( m_LastPaid + TimeSpan.FromDays(1) ) - DateTime.Now).Hours.ToString() + " hours.");
                        return true;
                    }
                }
                else if (item is CustomFood || item is Food)
                {
                    if(!m_IsSlave)
                    {
                        this.Say("I can pay for my own food, thank you!");
                        return false;
                    }
                    else
                    {
                        this.Say("*accepts the food from " + (from.Female ? "her" : "him") + "*");
                        if(item is CustomFood)
                            HeldFood += ( (int)(item as CustomFood).Quality * 2 );
                        else
                            HeldFood += item.Amount;

                        item.Consume();
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 16))
            {
                return true;
            }

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile && AIObject.WasNamed(e.Speech) && Controlled && ControlMaster != null && !ControlMaster.Deleted && ( ControlMaster == e.Mobile || GroupInfo.IsGroupLeader(this, e.Mobile as PlayerMobile)))
            {
                if (e.Speech.ToLower().Contains("work"))
                {
                    e.Mobile.Target = new EmployeeWorkTarget(this);
                    e.Mobile.SendMessage("Target a resource node for " + this.Name + " to work. The node's resource must match the material you are having this employee gather.");
                    return;
                }
                else if (e.Speech.ToLower().Contains("manage"))
                {
                    e.Mobile.SendGump(new EmployeeGump(e.Mobile as PlayerMobile, this));
                    return;
                }
                else if (e.Speech.ToLower().Contains("help"))
                {
                    e.Mobile.SendMessage("Say 'work' to choose a node and have the employee begin working; say 'manage' to view and manage this employee's settings.");
                    return;
                }
            }

            base.OnSpeech(e);
        }
        #endregion

        public Employee(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_HeldCopper);
            writer.Write((int)m_HeldFood);
            writer.Write((DateTime)m_LastPaid);
            writer.Write((int)m_Wage);
            writer.Write((int)m_WagesPaid);
            writer.Write((bool)m_IsSlave);

            writer.Write((int)m_ToGather);
            writer.Write((int)m_HeldResource);
            writer.Write((ResourceNode)m_Node);
            writer.Write((bool)(m_Timer != null && m_Timer.Running)); // Checking to see if we should restart the timer on deserialization.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_HeldCopper = reader.ReadInt();
                        m_HeldFood = reader.ReadInt();
                        m_LastPaid = reader.ReadDateTime();
                        m_Wage = reader.ReadInt();
                        m_WagesPaid = reader.ReadInt();
                        m_IsSlave = reader.ReadBool();

                        m_ToGather = (CraftResource)reader.ReadInt();
                        m_HeldResource = reader.ReadInt();
                        m_Node = (ResourceNode)reader.ReadItem();
                        bool startTimer = reader.ReadBool();
                        if (startTimer)
                        {
                            m_Timer = new EmployeeGatherTimer(this);
                            m_Timer.Start();
                        }
                        break;
                    }
            }
        }
    }

    public class EmployeeContract : Item
    {
        private GovernmentEntity m_Government;
        private bool m_IsSlave;

        public GovernmentEntity Government { get { return m_Government; } }
        public bool IsSlave { get { return m_IsSlave; } }

        public EmployeeContract(GovernmentEntity source, bool slave) : base(0x14F0)
        {
            m_Government = source;
            m_IsSlave = slave;

            if (m_IsSlave)
                Name = "A Contract of Labor from " + m_Government.Name;
            else
                Name = "A Contract of Wage Employment from " + m_Government.Name;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Government == null || m_Government.Deleted)
                return;

            int empCount = 0;
            foreach (Mobile m in m_Government.Employees)
            {
                if (m is Employee)
                    empCount++;
            }

            if (m_IsSlave)
            {
                if (empCount >= m_Government.TradeInformation.MaxSlaves)
                {
                    from.SendMessage("This would exceed the maximum number of laborers " + m_Government.Name + " current allows.");
                    return;
                }
            }
            else
            {
                if (empCount >= m_Government.TradeInformation.MaxWageEarners)
                {
                    from.SendMessage("This would exceed the maximum number of wage-earners " + m_Government.Name + " current allows.");
                    return;
                }
            }

            Employee e = new Employee(from as PlayerMobile, this);
            e.MoveToWorld(from.Location, from.Map);
            this.Delete();

            base.OnDoubleClick(from);
        }

        public EmployeeContract(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((GovernmentEntity)m_Government);
            writer.Write((bool)m_IsSlave);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_Government = (GovernmentEntity)reader.ReadItem();
                        m_IsSlave = reader.ReadBool();
                        break;
                    }
            }
        }
    }

    public class EmployeeGatherTimer : Timer
        {
            Employee m_Employee;

            public EmployeeGatherTimer(Employee e)
                : base(TimeSpan.FromSeconds(300), TimeSpan.FromHours(6))
            {
                m_Employee = e;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Employee == null || m_Employee.Deleted)
                {
                    Stop();
                    return;
                }

                if (!m_Employee.Alive)
                {
                    Stop();
                    return;
                }

                if (!m_Employee.Controlled || m_Employee.ControlMaster == null || m_Employee.ControlMaster.Deleted)
                {
                    Stop();
                    return;
                }

                if (m_Employee.TryToPay())
                {
                    if (m_Employee.Combatant != null && !m_Employee.Combatant.Deleted)
                        return;
                    Employee.DoGatherAnimation(m_Employee, m_Employee.ToGather);
                    m_Employee.TryToGather();
                }
                else
                {
                    if (m_Employee.Combatant != null && !m_Employee.Combatant.Deleted)
                        return;
                    Employee.DoGatherAnimation(m_Employee, m_Employee.ToGather);
                }

                base.OnTick();
            }
        }

    public class EmployeeWorkTarget : Target
        {
            private Employee m_Emp;

            public EmployeeWorkTarget(Employee e) : base(16, true, TargetFlags.None)
            {
                m_Emp = e;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Emp == null || m_Emp.Deleted)
                    return;
                if (!m_Emp.Alive)
                    return;

                if (targeted is ResourceNode)
                {
                    m_Emp.Node = targeted as ResourceNode;
                    m_Emp.GatherTimer = new EmployeeGatherTimer(m_Emp);
                    m_Emp.GatherTimer.Start();
                    if (m_Emp.Wage > 0)
                        m_Emp.Say("*nods at " + (from.Female ? "her" : "him*"));
                    else
                        m_Emp.Say("*nods at " + (from.Female ? "her" : "him*"));
                    m_Emp.Home = (targeted as ResourceNode).Location;
                    m_Emp.RangeHome = 2;
                    m_Emp.ControlOrder = OrderType.None;
                    m_Emp.AIObject.Action = ActionType.Wander;
                }

                base.OnTarget(from, targeted);
            }
        }

    public class EmployeeGump : Gump
    {
        private enum EmpButton
        {
            Cancel,
            Okay,
            PreviousRes,
            NextRes,
            WithdrawRes
        }
        private PlayerMobile m_Viewer;
        private Employee m_Emp;
        private Dictionary<CraftResource, string> ResourceDictionary
        {
            get
            {
                Dictionary<CraftResource, string> list = new Dictionary<CraftResource, string>();
                list.Add(CraftResource.Copper, "Copper");
                list.Add(CraftResource.Tin, "Tin");
                list.Add(CraftResource.Iron, "Iron");
                //list.Add(CraftResource.Silver, "Silver");
                //list.Add(CraftResource.Gold, "Gold");
                list.Add(CraftResource.Cotton, "Cotton");
                list.Add(CraftResource.Wool, "Wool");
                list.Add(CraftResource.RegularLeather, "Leather");
                list.Add(CraftResource.ThickLeather, "Thick Leather");
                list.Add(CraftResource.BeastLeather, "Beast Leather");
                list.Add(CraftResource.ScaledLeather, "Scaled Leather");
                list.Add(CraftResource.Oak, "Oak");
                list.Add(CraftResource.Yew, "Yew");
                list.Add(CraftResource.Redwood, "Redwood");
                list.Add(CraftResource.Ash, "Ash");
                return list;
            }
        }
        private List<CraftResource> ResourceList
        {
            get
            {
                List<CraftResource> list = new List<CraftResource>();
                list.Add(CraftResource.Copper);
                list.Add(CraftResource.Tin);
                list.Add(CraftResource.Iron);
                //list.Add(CraftResource.Silver);
                //list.Add(CraftResource.Gold);
                list.Add(CraftResource.Cotton);
                list.Add(CraftResource.Wool);
                list.Add(CraftResource.RegularLeather);
                list.Add(CraftResource.ThickLeather);
                list.Add(CraftResource.BeastLeather);
                list.Add(CraftResource.ScaledLeather);
                list.Add(CraftResource.Oak);
                list.Add(CraftResource.Yew);
                list.Add(CraftResource.Redwood);
                list.Add(CraftResource.Ash);
                return list;
            }
        }

        public EmployeeGump(PlayerMobile viewer, Employee e) :base(0,0)
        {
            m_Viewer = viewer;
            m_Emp = e;
            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(EmployeeGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(300, 159, 200, 330, 9270);
            AddBackground(315, 173, 170, 30, 9350);
            AddLabel(400 - (int)(m_Emp.Name.Length * 2.9), 178, 0, m_Emp.Name);
            AddBackground(315, 208, 170, 30, 9350);
            if (m_Emp.IsSlave)
            {
                AddLabel(385, 213, 37, "Laborer");
            }
            else
            {
                AddLabel(325, 213, 247, "Daily Wage:");
                AddTextEntry(400, 213, 77, 20, 0, 1, m_Emp.Wage.ToString());
            }
            AddBackground(315, 243, 170, 30, 9350);
            AddLabel(325, 248, 0, "... After Taxes: " + (m_Emp.Wage + m_Emp.Tax).ToString());
            AddBackground(315, 278, 170, 30, 9350);
            if (m_Emp.IsSlave)
            {
                AddLabel(325, 283, 247, "Food Held: " + m_Emp.HeldFood.ToString());
            }
            else
            {
                AddLabel(325, 283, 247, "Copper Held: " + m_Emp.HeldCopper.ToString());
            }
            AddBackground(315, 313, 170, 30, 9350);
            AddLabel(400 - (int)(ResourceDictionary[m_Emp.ToGather].Length * 2.9), 318, 0, ResourceDictionary[m_Emp.ToGather]);
            AddButton(325, 321, 5603, 5607, (int)EmpButton.PreviousRes, GumpButtonType.Reply, 0);
            AddButton(455, 321, 5601, 5605, (int)EmpButton.NextRes, GumpButtonType.Reply, 0);
            AddBackground(315, 348, 170, 30, 9350);
            AddLabel(324, 353, 247, "Gathering Rate: " + m_Emp.GatherRate.ToString());
            AddBackground(315, 382, 170, 30, 9350);
            AddLabel(325, 388, 0, "Gathered: " + m_Emp.HeldResources.ToString());
            AddButton(449, 388, 1154, 1155, (int)EmpButton.WithdrawRes, GumpButtonType.Reply, 0);
            AddBackground(315, 415, 170, 30, 9350);
            AddLabel(400 - (int)(m_Emp.Government.Name.Length * 2.75), 420, 0, m_Emp.Government.Name);
            AddButton(315, 450, 249, 248, (int)EmpButton.Okay, GumpButtonType.Reply, 0);
            AddButton(420, 450, 243, 241, (int)EmpButton.Cancel, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                return;

            switch (info.ButtonID)
            {
                case (int)EmpButton.Cancel:
                    return;
                case (int)EmpButton.Okay:
                    {
                        if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                            return;
                        if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                            return;
                        if (!m_Emp.IsSlave)
                        {
                            int val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry(1).Text, "Wage", ref val))
                            {
                                if (val < 1)
                                    val = 1;
                                m_Emp.Wage = val;
                            }

                        }
                        return;
                    }
                case (int)EmpButton.PreviousRes:
                    {
                        if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                            return;
                        if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                            return;
                        if (!m_Emp.IsSlave)
                        {
                            int val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry(1).Text, "Wage", ref val))
                            {
                                if (val < 1)
                                    val = 1;
                                m_Emp.Wage = val;
                            }
                        }

                        if(m_Emp.HeldResources > 0)
                            m_Emp.WithdrawResources(m_Viewer, m_Emp.HeldResources);

                        int index = 0;
                        for (int i = 0; i < ResourceList.Count; i++)
                        {
                            if (ResourceList[i] == m_Emp.ToGather)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == 0)
                        {
                            m_Emp.ToGather = ResourceList[ResourceList.Count - 1];
                        }
                        else
                        {
                            m_Emp.ToGather = ResourceList[index - 1];
                        }

                        m_Viewer.SendGump(new EmployeeGump(m_Viewer, m_Emp));
                        return;
                    }
                case (int)EmpButton.NextRes:
                    {
                        if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                            return;
                        if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                            return;
                        if (!m_Emp.IsSlave)
                        {
                            int val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry(1).Text, "Wage", ref val))
                            {
                                if (val < 1)
                                    val = 1;
                                m_Emp.Wage = val;
                            }
                        }

                        if (m_Emp.HeldResources > 0)
                            m_Emp.WithdrawResources(m_Viewer, m_Emp.HeldResources);

                        int index = 0;
                        for (int i = 0; i < ResourceList.Count; i++)
                        {
                            if (ResourceList[i] == m_Emp.ToGather)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == ResourceList.Count - 1)
                        {
                            m_Emp.ToGather = ResourceList[0];
                        }
                        else
                        {
                            m_Emp.ToGather = ResourceList[index + 1];
                        }

                        m_Viewer.SendGump(new EmployeeGump(m_Viewer, m_Emp));
                        return;
                    }
                case (int)EmpButton.WithdrawRes:
                    {
                        if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                            return;
                        if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                            return;
                        m_Viewer.Prompt = new WithdrawResourcePrompt(m_Emp);
                        m_Viewer.SendMessage("Enter an amount to withdraw from your employee.");
                        return;
                    }
                default: return;
            }

            base.OnResponse(sender, info);
        }

        public bool ValidateInt(PlayerMobile m, string st, string name, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
            {
                m.SendMessage("Field \"" + name + "\" needs to be a valid number.");
                return false;
            }

            return true;
        }

        private class WithdrawResourcePrompt : Prompt
        {
            private Employee m_Emp;

            public WithdrawResourcePrompt(Employee e)
                : base()
            {
                m_Emp = e;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (from == null || from.Deleted || !from.Alive)
                    return;
                if (m_Emp == null || m_Emp.Deleted || !m_Emp.Alive || m_Emp.IsDeadBondedPet || m_Emp.IsDeadPet)
                    return;

                if (!from.InRange(m_Emp.Location, 3))
                {
                    from.SendMessage("You are not within sufficient range of " + m_Emp.Name + ".");
                    return;
                }

                int val = 0;
                if (Int32.TryParse(text, out val))
                {
                    m_Emp.WithdrawResources(from, val);
                    from.SendGump(new EmployeeGump(from as PlayerMobile, m_Emp));
                }
                else
                {
                    from.SendMessage("You must enter a valid integer to withdraw.");
                    from.SendGump(new EmployeeGump(from as PlayerMobile, m_Emp));
                }

                base.OnResponse(from, text);
            }

            public override void OnCancel(Mobile from)
            {
                base.OnCancel(from);
            }
        }
    }
}