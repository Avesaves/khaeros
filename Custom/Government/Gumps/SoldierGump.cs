using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Targets;

namespace Server.Gumps
{
    public class SoldierGump : Gump
    {
        //private int m_Armament;
        //private int m_Training;
        //private Direction m_Dir;

        //private int m_ArmsCost;
        //private int m_TrainingCost;

        //public int Armament { get { return m_Armament; } set { m_Armament = value; } }
        //public int Training { get { return m_Training; } set { m_Training = value; } }
        //public Direction Dir { get { return m_Dir; } set { m_Dir = value; } }

        //public PlayerMobile m_Viewer;
        //public GovernmentEntity m_Government;
        //public MilitarySpawner m_Spawner;
        //public int m_CurrentNav;
        //public int m_NavScroll;
        //public int m_MainScroll;
        //public int m_CurrentMain;
        //public int m_ItemsPerPage;
        //public int m_Start;
        //public int m_Y;
        //public int m_Current;
        //public int m_LineHeight;
        //public virtual string[] m_NavItems
        //{
        //    get
        //    {
        //        return new string[]
        //{ 
        //    "Overview",
        //    "Armament",
        //    "Training",
        //    "Direction",
        //    "Deployment"
        //};
        //    }
        //}

        //public enum Navigation
        //{
        //    Overview,
        //    Armament,
        //    Training,
        //    Direction,
        //    Deployment
        //}

        //public enum Buttons
        //{
        //    Close, NavUp, NavDown, MainUp, MainDown, Okay, Cancel,
        //    MB1, MB2, MB3, MB4, MB5, MB6, MB7, MB8,
        //    MB9, M10, MB11, MB12, MB13, MB14, MB15, MB16,
        //    MB17, MB18, MB19, MB20, MB21, MB22, MB23, MB24,
        //    NB1, NB2, NB3, NB4, NB5, NB6, NB7, NB8, NB9
        //}

        //public void AddCheckBox(int id, string label, bool initialState)
        //{
        //    if(initialState)
        //        AddButton(300, (m_Y + 2), 2511, 2510, id, GumpButtonType.Reply, 0);
        //    else
        //        AddButton(300, (m_Y + 2), 2510, 2511, id, GumpButtonType.Reply, 0);

        //    AddLabel(325, m_Y, 0, label);
        //    m_Y += m_LineHeight;
        //}

        //public virtual bool HandleMainPageUse(RelayInfo info)
        //{
        //    switch (m_CurrentNav)
        //    {
        //        case (int)Navigation.Overview: OverviewOnResponse(info.ButtonID); break;
        //        case (int)Navigation.Armament: ArmamentOnResponse(info.ButtonID); return true;
        //        case (int)Navigation.Training: TrainingOnResponse(info.ButtonID); return true;
        //        case (int)Navigation.Direction: DirectionOnResponse(info.ButtonID); break;
        //        case (int)Navigation.Deployment: DeploymentOnResponse(info.ButtonID); break;
        //    }

        //    return false;
        //}

        //public SoldierGump(PlayerMobile from) : this(from, null) { }

        //public SoldierGump(PlayerMobile from, GovernmentEntity gov) : this(from, gov, null) { }

        //public SoldierGump(PlayerMobile from, GovernmentEntity gov, MilitarySpawner spawner) : this(from, gov, spawner, 0, 0, 0, 0, 0, 0, 0, 0, (Direction)Utility.Random(8)) { }

        //public SoldierGump(PlayerMobile from, GovernmentEntity gov, MilitarySpawner spawner, int currentNav, int navScroll, int currentMain, 
        //    int mainScroll, int armaments, int armscost, int training, int trainingcost, Direction d) : base(0,0)
        //{
        //    if (!GovernmentEntity.IsGuildMilitary(from, gov))
        //        return;

        //    Armament = armaments;
        //    m_ArmsCost = armscost;
        //    Training = training;
        //    m_TrainingCost = trainingcost;
        //    Dir = d;

        //    m_Spawner = spawner;

        //    InitialSetup(from, gov, spawner, currentNav, navScroll, currentMain, mainScroll);
        //    AddNavigation();
        //    AddMain();
        //}

        //public virtual void InitialSetup(PlayerMobile from, GovernmentEntity gov, MilitarySpawner spawner, int currentNav, int navScroll, int currentMain, int mainScroll)
        //{
        //    Closable = true;
        //    Disposable = true;
        //    Dragable = true;
        //    Resizable = false;
        //    m_Government = gov;
        //    m_CurrentNav = currentNav;
        //    m_NavScroll = navScroll;
        //    m_CurrentMain = currentMain;
        //    m_MainScroll = mainScroll;
        //    m_ItemsPerPage = 8;
        //    m_Start = m_ItemsPerPage * m_NavScroll;
        //    m_Y = 228;
        //    m_LineHeight = 28;
        //    m_Viewer = from;
        //    m_Government = gov;
        //    m_Spawner = spawner;
        //    m_Viewer.CloseGump(typeof(SoldierGump));
        //    m_Viewer.CloseGump(typeof(GovernmentGump));
        //    AddPage(0);
        //    AddBackground(64, 114, 671, 55, 9270);
        //    AddLabel(327, 132, 2010, @"Khaeros' Soldier Deployment System");
        //    AddBackground(64, 169, 203, 338, 9270);
        //    AddBackground(267, 169, 468, 338, 9270);
        //    AddBackground(283, 185, 435, 305, 3500);
        //    AddBackground(80, 185, 170, 305, 3500);
        //    AddBackground(64, 507, 671, 55, 9270);
        //    AddImage(703, 61, 10441);
        //    AddImage(16, 61, 10440);

        //    if (m_Government != null && !String.IsNullOrEmpty(m_Government.Name))
        //        AddLabel(82, 525, 2010, "Government Organization:   " + m_Government.Name.Replace(" ", "   "));
        //}

        //public virtual void AddNavigation()
        //{
        //    AddLabel( 127, 198, 2010, @"Navigation" );
        	
        //    for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < m_NavItems.Length; m_Current++ )
        //    {
        //        AddLabel( 117, (m_Y + (m_Current * m_LineHeight)), 0, m_NavItems[m_Start + m_Current] );
        //        AddButton( 97, (m_Y + 2 + (m_Current * m_LineHeight)), (m_Start + m_Current) == m_CurrentNav ? 2511 : 2510, 2511, 
        //                  ((int)Buttons.NB1 + m_Current), GumpButtonType.Reply, 0 );
        //    }
        	
        //    if( m_NavItems.Length > (m_Start + m_Current + 1) )
        //        AddButton( 222, 462, 2648, 2649, (int)Buttons.NavDown, GumpButtonType.Reply, 0 );
        	
        //    if( m_NavScroll > 0 )
        //        AddButton( 222, 193, 2650, 2651, (int)Buttons.NavUp, GumpButtonType.Reply, 0 );
        //}

        //public virtual void AddMain()
        //{
        //    switch (m_CurrentNav)
        //    {
        //        case (int)Navigation.Overview: AddOverview(); break;
        //        case (int)Navigation.Armament: AddArmament(); break;
        //        case (int)Navigation.Training: AddTraining(); break;
        //        case (int)Navigation.Direction: AddDirection(); break;
        //        case (int)Navigation.Deployment: AddDeployment(); break;
        //    }
        //}

        //public void AddMainScroll(int total, int next)
        //{
        //    if (total > next)
        //        AddButton(690, 462, 2648, 2649, (int)Buttons.MainDown, GumpButtonType.Reply, 0);

        //    if (m_MainScroll > 0)
        //        AddButton(690, 193, 2650, 2651, (int)Buttons.MainUp, GumpButtonType.Reply, 0);
        //}

        //public void AddOverview()
        //{
        //    AddLabel(315, 198, 2010, "Military Review of " + m_Government.Name);
        //    m_Y = 253;
        //    int x = 315;

        //    if (m_Government == null)
        //        return;

        //    int payroll = 0;
        //    int count = m_Government.MilitarySpawners.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (m_Government != null && m_Government.MilitarySpawners[i] != null && m_Government.MilitarySpawners[i].PayRate != null)
        //        {
        //            int thisPay = (m_Government.MilitarySpawners[i].PayRate - m_Government.ResourceBudgetContribution());
        //            if (thisPay < 0)
        //                thisPay = 0;
        //            payroll += thisPay;
        //        }
        //    }

        //    AddLabel(x, m_Y, 0, "Active Soldiers: " + m_Government.MilitarySpawners.Count); m_Y += m_LineHeight;
        //    AddLabel(x, m_Y, 0, "Military Payroll: " + payroll); m_Y += m_LineHeight;
        //    AddLabel(x, m_Y, 0, "Government Balance: " + m_Government.GetTreasuryBalance().ToString()); m_Y += m_LineHeight;
        //}

        //public void OverviewOnResponse(int id)
        //{

        //}

        //public void AddArmament()
        //{
        //    AddLabel(315, 198, 2010, "Requisition Armaments for " + m_Government.Nation.ToString() + " Soldiers");
        //    m_Y = 253;

        //    switch (Armament)
        //    {
        //        case 0:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 1:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", true);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 2:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", true);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 3:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", true);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 4:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", true);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 5:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", true);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", false);
        //                break;
        //            }
        //        case 6:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "Light Infantry", false);
        //                AddCheckBox((int)Buttons.MB2, "Medium Infantry", false);
        //                AddCheckBox((int)Buttons.MB3, "Heavy Infantry", false);
        //                AddCheckBox((int)Buttons.MB4, "Ranged Infantry", false);
        //                AddCheckBox((int)Buttons.MB5, "Light Cavalry", false);
        //                AddCheckBox((int)Buttons.MB6, "Heavy Cavalry", true);
        //                break;
        //            }

        //        default: goto case 0;
        //    }
        //}

        //public void ArmamentOnResponse(int id)
        //{
        //    if (id == (int)Buttons.MB1)
        //    {
        //        m_ArmsCost = 1000;
        //        Armament = 1;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB2)
        //    {
        //        m_ArmsCost = 1500;
        //        Armament = 2;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB3)
        //    {
        //        m_ArmsCost = 2000;
        //        Armament = 3;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB4)
        //    {
        //        m_ArmsCost = 1500;
        //        Armament = 4;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB5)
        //    {
        //        m_ArmsCost = 2500;
        //        Armament = 5;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB6)
        //    {
        //        m_ArmsCost = 3500;
        //        Armament = 6;
        //        SendNewGump();
        //    }
        //}

        //public void AddTraining()
        //{
        //    AddLabel(315, 198, 2010, "Elite Training for " + m_Government.Nation.ToString() + " Soldiers");
        //    m_Y = 253;

        //    switch (Training)
        //    {
        //        case 0:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", true);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", false);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", false);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", false);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", false);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", false);
        //                break;
        //            }
        //        case 1:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", false);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", true);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", false);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", false);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", false);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", false);
        //                break;
        //            }
        //        case 2:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", false);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", false);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", true);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", false);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", false);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", false);
        //                break;
        //            }
        //        case 3:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", false);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", false);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", false);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", true);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", false);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", false);
        //                break;
        //            }
        //        case 4:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", false);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", false);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", false);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", false);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", true);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", false);
        //                break;
        //            }
        //        case 5:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "No Elite Training", false);
        //                AddCheckBox((int)Buttons.MB2, "Assault Training", false);
        //                AddCheckBox((int)Buttons.MB3, "Endurance Training", false);
        //                AddCheckBox((int)Buttons.MB4, "Tactical Training", false);
        //                AddCheckBox((int)Buttons.MB5, "Ranged Training", false);
        //                AddCheckBox((int)Buttons.MB6, "Dragoon Training", true);
        //                break;
        //            }

        //        default: goto case 0;
        //    }
        //}

        //public void TrainingOnResponse(int id)
        //{
        //    if (id == (int)Buttons.MB1)
        //    {
        //        m_TrainingCost = 0;
        //        Training = 0;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB2)
        //    {
        //        m_TrainingCost = 1000;
        //        Training = 1;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB3)
        //    {
        //        m_TrainingCost = 1000;
        //        Training = 2;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB4)
        //    {
        //        m_TrainingCost = 1000;
        //        Training = 3;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB5)
        //    {
        //        m_TrainingCost = 1000;
        //        Training = 4;
        //        SendNewGump();
        //    }

        //    if (id == (int)Buttons.MB6)
        //    {
        //        m_TrainingCost = 1000;
        //        Training = 5;
        //        SendNewGump();
        //    }
        //}

        //public void AddDirection()
        //{
        //    AddLabel(315, 198, 2010, "Watch Duty for " + m_Government.Nation.ToString() + " Soldiers");
        //    m_Y = 253;

        //    switch (Dir)
        //    {
        //        case Direction.North:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", true);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.Right:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", true);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.East:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", true);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.Down:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", true);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.South:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", true);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.Left:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", true);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.West:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", true);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", false);
        //                break;
        //            }
        //        case Direction.Up:
        //            {
        //                AddCheckBox((int)Buttons.MB1, "North", false);
        //                AddCheckBox((int)Buttons.MB2, "Northeast", false);
        //                AddCheckBox((int)Buttons.MB3, "East", false);
        //                AddCheckBox((int)Buttons.MB4, "Southeast", false);
        //                AddCheckBox((int)Buttons.MB5, "South", false);
        //                AddCheckBox((int)Buttons.MB6, "Southwest", false);
        //                AddCheckBox((int)Buttons.MB7, "West", false);
        //                AddCheckBox((int)Buttons.MB8, "Northwest", true);
        //                break;
        //            }
        //    }
        //}

        //public void DirectionOnResponse(int id)
        //{
        //    if (id == (int)Buttons.MB1) { Dir = Direction.North; SendNewGump(); }

        //    if (id == (int)Buttons.MB2) { Dir = Direction.Right; SendNewGump(); }

        //    if (id == (int)Buttons.MB3) { Dir = Direction.East; SendNewGump(); }

        //    if (id == (int)Buttons.MB4) { Dir = Direction.Down; SendNewGump(); }

        //    if (id == (int)Buttons.MB5) { Dir = Direction.South; SendNewGump(); }

        //    if (id == (int)Buttons.MB6) { Dir = Direction.Left; SendNewGump(); }

        //    if (id == (int)Buttons.MB7) { Dir = Direction.West; SendNewGump(); }

        //    if (id == (int)Buttons.MB8) { Dir = Direction.Up; SendNewGump(); }
        //}

        //public void AddDeployment()
        //{
        //    AddLabel(315, 198, 2010, "Prepare to Train and Deploy a " + m_Government.Nation.ToString() + " Soldier");
        //    m_Y = 253;
        //    int x = 315;

        //    String arms = null;
        //    switch (Armament)
        //    {
        //        case 1: arms = "Light Infantry"; break;
        //        case 2: arms = "Medium Infantry"; break;
        //        case 3: arms = "Heavy Infantry"; break;
        //        case 4: arms = "Ranged Infantry"; break;
        //        case 5: arms = "Light Cavalry"; break;
        //        case 6: arms = "Heavy Cavalry"; break;
        //        default: Armament = 1; m_ArmsCost = 1000; goto case 1;
        //    }

        //    String trainingString = null;
        //    switch (Training)
        //    {
        //        case 0: trainingString = "No Special Training"; m_TrainingCost = 0; break;
        //        case 1: trainingString = "Assault Training"; break;
        //        case 2: trainingString = "Endurance Training"; break;
        //        case 3: trainingString = "Tactical Training"; break;
        //        case 4: trainingString = "Ranged Training"; break;
        //        case 5: trainingString = "Dragoon Training"; break;
        //    }

        //    String dirString = null;
        //    switch (Dir)
        //    {
        //        case Direction.North: dirString = "North"; break;
        //        case Direction.Right: dirString = "Northeast"; break;
        //        case Direction.East: dirString = "East"; break;
        //        case Direction.Down: dirString = "Southeast"; break;
        //        case Direction.South: dirString = "South"; break;
        //        case Direction.Left: dirString = "Southwest"; break;
        //        case Direction.West: dirString = "West"; break;
        //        case Direction.Up: dirString = "Northwest"; break;
        //    }

        //    AddLabel(x, m_Y, 0, "Government Organization: " + m_Government.Name); m_Y += m_LineHeight;
        //    AddLabel(x, m_Y, 0, "Armaments: " + arms); m_Y += m_LineHeight;
        //    AddLabel(x, m_Y, 0, "Elite Training: " + trainingString); m_Y += m_LineHeight;
        //    AddLabel(x, m_Y, 0, "Watch: " + dirString); m_Y += (m_LineHeight * 2);
        //    AddLabel(x, m_Y, 0, "Cost: " + MilitarySpawner.Salary(m_TrainingCost, m_ArmsCost, m_Government ) + " Copper"); m_Y += m_LineHeight;

        //    AddButton(436, 453, 2311, 2312, (int)Buttons.MB1, GumpButtonType.Reply, 0);
        //    AddButton(505, 453, 243, 241, (int)Buttons.MB2, GumpButtonType.Reply, 0);

        //}

        //public void DeploymentOnResponse(int id)
        //{
        //    if (id == (int)Buttons.MB1)
        //        TryStationSoldier();

        //    if (id == (int)Buttons.MB2)
        //        TryClearGump();
        //}

        //public void TryStationSoldier()
        //{
        //    //Put in a check to see if the government has enough cash to station this particular soldier; if not, send error message and return.
        //    m_Spawner.AddSoldier(m_Government, Armament, Training, Dir, m_TrainingCost + m_ArmsCost, m_Viewer);
        //    //m_Viewer.CloseAllGumps();
        //}

        //public void TryClearGump()
        //{
        //    m_Armament = 0;
        //    m_ArmsCost = 0;
        //    m_Training = 0;
        //    m_TrainingCost = 0;
        //    m_Dir = (Direction)Utility.Random(8);
        //    m_Viewer.SendGump(new SoldierGump(m_Viewer, m_Government, m_Spawner, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll, Armament, m_ArmsCost, Training, m_TrainingCost, Dir));
        //}

        //public override void OnResponse(NetState sender, RelayInfo info)
        //{
        //    if (m_Viewer == null || m_Viewer != sender.Mobile)
        //        return;

        //    if (info.ButtonID >= (int)Buttons.NB1 && info.ButtonID <= (int)Buttons.NB9)
        //    {
        //        m_CurrentNav = (m_NavScroll * m_ItemsPerPage) + (info.ButtonID - (int)Buttons.NB1);
        //        m_MainScroll = 0;
        //        m_CurrentMain = 0;
        //    }

        //    if (info.ButtonID == (int)Buttons.NavUp)
        //        m_NavScroll--;

        //    if (info.ButtonID == (int)Buttons.NavDown)
        //        m_NavScroll++;

        //    if (info.ButtonID == (int)Buttons.MainUp)
        //        m_MainScroll--;

        //    if (info.ButtonID == (int)Buttons.MainDown)
        //        m_MainScroll++;

        //    if (info.ButtonID >= (int)Buttons.MB1 && info.ButtonID <= (int)Buttons.MB24)
        //        if (HandleMainPageUse(info))
        //            return;

        //    if ( (info.ButtonID > (int)Buttons.Close) && !( m_CurrentNav == (int)Navigation.Deployment && info.ButtonID == (int)Buttons.MB1 ) )
        //        SendNewGump();
        //}

        //public virtual void SendNewGump()
        //{
        //    m_Viewer.SendGump(new SoldierGump(m_Viewer, m_Government, m_Spawner, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll, Armament, m_ArmsCost, Training, m_TrainingCost, Dir));
        //}

        private enum SoldierButton
        {
            Refuse,
            Accept,
            PrevArmament,
            NextArmament,
            PrevTraining,
            NextTraining,
            PrevDirection,
            NextDirection
        }

        #region Private Variables
        private PlayerMobile m_Viewer;
        private MilitarySpawner m_Spawner;
        private Armament m_Armament;
        private Training m_Training;
        private Direction m_Direction;
        #endregion

        private List<Armament> m_ArmamentList
        {
            get
            {
                List<Armament> list = new List<Armament>();
                list.Add(Armament.Light);
                list.Add(Armament.Medium);
                list.Add(Armament.Heavy);
                list.Add(Armament.Ranged);
                list.Add(Armament.LightCavalry);
                list.Add(Armament.HeavyCavalry);
                return list;
            }
        }
        private List<Training> m_TrainingList
        {
            get
            {
                List<Training> list = new List<Training>();
                list.Add(Training.None);
                list.Add(Training.Assault);
                list.Add(Training.Endurance);
                list.Add(Training.Strategy);
                list.Add(Training.Ranged);
                list.Add(Training.Dragoon);
                return list;
            }
        }
        private List<Direction> m_DirectionList
        {
            get
            {
                List<Direction> list = new List<Direction>();
                list.Add(Direction.North);
                list.Add(Direction.Right);
                list.Add(Direction.East);
                list.Add(Direction.Down);
                list.Add(Direction.South);
                list.Add(Direction.Left);
                list.Add(Direction.West);
                list.Add(Direction.Up);
                return list;
            }
        }
        private string DirectionString
        {
            get
            {
                switch (m_Direction)
                {
                    case Direction.Right: return "Northeast";
                    case Direction.Down: return "Southeast";
                    case Direction.Left: return "Southwest";
                    case Direction.Up: return "Northwest";
                    default: return m_Direction.ToString();
                }
            }
        }

        public SoldierGump(PlayerMobile viewer, MilitarySpawner spawner) : this(viewer, spawner, Armament.Light, 0, (Direction)Utility.RandomMinMax(1,8))
        {

        }

        public SoldierGump(PlayerMobile viewer, MilitarySpawner spawner, Armament arms, Training training, Direction dir) : base(0,0)
        {
            m_Viewer = viewer;
            m_Spawner = spawner;
            m_Armament = arms;
            m_Training = training;
            m_Direction = dir;
            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(SoldierGump));
            m_Viewer.CloseGump(typeof(GovernmentGump));
            m_Viewer.CloseGump(typeof(OrganizationGump));
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            //Basic background.
            AddBackground(269, 120, 271, 48, 9270);
            AddLabel(340, 133, 47, "Station   a   Soldier");
            AddBackground(269, 167, 271, 332, 9270);

            //Government overhead
            AddBackground(284, 181, 241, 35, 9350);
            AddLabel(402 - (int)(m_Spawner.Government.Name.Length * 2.9), 189, 47, m_Spawner.Government.Name);

            //Armament Section
            AddBackground(284, 221, 241, 35, 9350);
            AddLabel(289, 227, 47, "Armament:");
            AddButton(367, 230, 5603, 5607, (int)SoldierButton.PrevArmament, GumpButtonType.Reply, 0);
            AddButton(498, 230, 5601, 5605, (int)SoldierButton.NextArmament, GumpButtonType.Reply, 0);
            AddLabel(444 - (int)(m_Armament.ToString().Length * 2.9), 228, 0, m_Armament.ToString());

            //Training Section
            AddBackground(284, 262, 241, 35, 9350);
            AddLabel(289, 268, 47, "Training:");
            AddButton(367, 272, 5603, 5607, (int)SoldierButton.PrevTraining, GumpButtonType.Reply, 0);
            AddButton(498, 272, 5601, 5605, (int)SoldierButton.NextTraining, GumpButtonType.Reply, 0);
            AddLabel(444 - (int)(m_Training.ToString().Length * 2.9), 270, 0, m_Training.ToString());

            //Direction Section
            AddBackground(284, 303, 241, 35, 9350);
            AddLabel(289, 310, 47, "Watch:");
            AddButton(367, 312, 5603, 5607, (int)SoldierButton.PrevDirection, GumpButtonType.Reply, 0);
            AddButton(498, 312, 5601, 5605, (int)SoldierButton.NextDirection, GumpButtonType.Reply, 0);
            AddLabel(444 - (int)(DirectionString.Length * 2.9), 311, 0, DirectionString);

            //Summary of Cost
            int thisPay = Soldier.CalculateSoldierPay(m_Armament, m_Training);
            int finalWage = thisPay - (int)Soldier.CalculateResourceBonus(m_Spawner.Government, m_Armament);
            if (finalWage < 0)
                finalWage = 0;
            AddBackground(284, 344, 241, 111, 9350);
            AddLabel(292, 353, 0, "Training Cost: " + thisPay.ToString() + " copper");
            AddLabel(292, 378, 0, "Seasonal Wage: " + finalWage.ToString() + " copper");
            AddLabel(292, 403, 818, "( Resource Bonus: " + (int)Soldier.CalculateResourceBonus(m_Spawner.Government, m_Armament) + " )");
            AddLabel(292, 428, 32, "New Seasonal Budget: " +  ( m_Spawner.Government.Budget() - finalWage ));

            //Buttons
            AddButton(288, 465, 12000, 12002, (int)SoldierButton.Accept, GumpButtonType.Reply, 0);
            AddButton(448, 465, 12018, 12020, (int)SoldierButton.Refuse, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)SoldierButton.Refuse:
                    return;
                case (int)SoldierButton.Accept:
                    {
                        m_Spawner.AddSoldier(m_Spawner.Government, (int)m_Armament, (int)m_Training, m_Direction, Soldier.CalculateSoldierPay(m_Armament, m_Training), m_Viewer);
                        m_Viewer.SendGump(new GovernmentGump(m_Viewer, m_Spawner.Government));
                        return;
                    }
                case (int)SoldierButton.PrevArmament:
                    {
                        int index = 0;
                        for (int i = 0; i < m_ArmamentList.Count; i++)
                        {
                            if (m_ArmamentList[i] == m_Armament)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == 0)
                        {
                            m_Armament = m_ArmamentList[m_ArmamentList.Count - 1];
                            SendNewGump();
                        }
                        else
                        {
                            m_Armament = m_ArmamentList[index - 1];
                            SendNewGump();
                        }
                        return;
                    }
                case (int)SoldierButton.NextArmament:
                    {
                        int index = 0;
                        for (int i = 0; i < m_ArmamentList.Count; i++)
                        {
                            if (m_ArmamentList[i] == m_Armament)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == m_ArmamentList.Count - 1)
                        {
                            m_Armament = m_ArmamentList[0];
                            SendNewGump();
                        }
                        else
                        {
                            m_Armament = m_ArmamentList[index + 1];
                            SendNewGump();
                        }
                        return;
                    }
                case (int)SoldierButton.PrevTraining:
                    {
                        int index = 0;
                        for (int i = 0; i < m_TrainingList.Count; i++)
                        {
                            if (m_TrainingList[i] == m_Training)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == 0)
                        {
                            m_Training = m_TrainingList[m_TrainingList.Count - 1];
                            SendNewGump();
                        }
                        else
                        {
                            m_Training = m_TrainingList[index - 1];
                            SendNewGump();
                        }
                        return;
                    }
                case (int)SoldierButton.NextTraining:
                    {
                        int index = 0;
                        for (int i = 0; i < m_TrainingList.Count; i++)
                        {
                            if (m_TrainingList[i] == m_Training)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == m_TrainingList.Count - 1)
                        {
                            m_Training = m_TrainingList[0];
                            SendNewGump();
                        }
                        else
                        {
                            m_Training = m_TrainingList[index + 1];
                            SendNewGump();
                        }
                        return;
                    }
                case (int)SoldierButton.PrevDirection:
                    {
                        int index = 0;
                        for (int i = 0; i < m_DirectionList.Count; i++)
                        {
                            if (m_DirectionList[i] == m_Direction)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == 0)
                        {
                            m_Direction = m_DirectionList[m_DirectionList.Count - 1];
                            SendNewGump();
                        }
                        else
                        {
                            m_Direction = m_DirectionList[index - 1];
                            SendNewGump();
                        }
                        return;
                    }
                case (int)SoldierButton.NextDirection:
                    {
                        int index = 0;
                        for (int i = 0; i < m_DirectionList.Count; i++)
                        {
                            if (m_DirectionList[i] == m_Direction)
                            {
                                index = i;
                                continue;
                            }
                        }

                        if (index == m_DirectionList.Count - 1)
                        {
                            m_Direction = m_DirectionList[0];
                            SendNewGump();
                        }
                        else
                        {
                            m_Direction = m_DirectionList[index + 1];
                            SendNewGump();
                        }
                        return;
                    }
                default: base.OnResponse(sender, info); return;
            }
        }

        public void SendNewGump()
        {
            m_Viewer.SendGump(new SoldierGump(m_Viewer, m_Spawner, m_Armament, m_Training, m_Direction));
        }

    }
    public class SoldierTarget : Target
    {
        private PlayerMobile m_Owner;
        private GovernmentEntity m_Government;

        public SoldierTarget(GovernmentEntity gov)
            : base(14,false,TargetFlags.None)
        {
            m_Government = gov;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (from == null || from.Deleted || !from.Alive)
                return;
            if (m_Government == null || m_Government.Deleted)
                return;
            if (!(from is PlayerMobile))
                return;

            m_Owner = from as PlayerMobile;

            if (target is MilitarySpawner)
            {
                MilitarySpawner spawner = target as MilitarySpawner;
                if (spawner.Government == null || spawner.Government.Deleted)
                {
                    spawner.Delete();
                    return;
                }
                else if (spawner.Government != m_Government)
                {
                    from.SendMessage(m_Government.Name + " does not control that spawner.");
                    return;
                }
                else if (spawner.Stationed)
                {
                    from.SendMessage("That spawner already has a soldier stationed at it; remove the spawner and a place a new one to station a new soldier.");
                    return;
                }
                else if (!GovernmentEntity.IsGuildMilitary(m_Owner, m_Government))
                {
                    from.SendMessage("You must be military personnel of " + m_Government.Name + " to do that.");
                    return;
                }
                else
                    m_Owner.SendGump(new SoldierGump(m_Owner, spawner));
            }
            else
                from.SendMessage("Target a military spawner to station a soldier there.");
            
            base.OnTarget(from, target);
        }
    }
}