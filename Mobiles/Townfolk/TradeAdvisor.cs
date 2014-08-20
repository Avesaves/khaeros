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
using Server.Gumps;
using Server.Prompts;

namespace Server.Mobiles
{
    public class TradeAdvisor : BaseKhaerosMobile
    {
        [Constructable]
        public TradeAdvisor(GovernmentEntity g) : base(g.Nation)
        {
            Blessed = true;
            Government = g;
            Nation = g.Nation;
            RandomRichClothes(this, this.Nation);

            string advisorTitle = "";

            switch(Nation)
            {
                case Nation.Southern: advisorTitle = "Argentarii"; break;
                case Nation.Western: advisorTitle = "Makipuray"; break;
                case Nation.Haluaroc: advisorTitle = "Pochteca"; break;
                case Nation.Mhordul: advisorTitle = "Qudaldughacin"; break;
                case Nation.Tirebladd: advisorTitle = "Suirbheir"; break;
                case Nation.Northern: advisorTitle = "Bursar"; break;
/*                 case Nation.Imperial: advisorTitle = "Agent"; break;
                case Nation.Sovereign: if(Female) { advisorTitle = "Quartermistress"; } else { advisorTitle = "Quartermaster"; } break;
				case Nation.Insularii: if(Female) { advisorTitle = "Epistolary"; } else { advisorTitle = "Epistolary"; } break; */
            }
            
            Name = advisorTitle + " " + Name; 
            Title = "of " + Government.Name.ToString();
        }

        public void SellVendorDeed(PlayerMobile from)
        {
            int totalCopper = 0;

            if (from.Backpack != null)
                totalCopper += from.Backpack.GetAmount(typeof(Copper));

            if (from.BankBox != null)
                totalCopper += from.BankBox.GetAmount(typeof(Copper));

            if (totalCopper < Government.TradeInformation.VendorPrice)
            {
                this.Say("You cannot afford this.");
                return;
            }
            else
            {
                int leftPrice = Government.TradeInformation.VendorPrice;

                if (from.Backpack != null)
                    leftPrice -= from.Backpack.ConsumeUpTo(typeof(Copper), leftPrice);

                if (leftPrice > 0 && from.BankBox != null)
                    from.BankBox.ConsumeUpTo(typeof(Copper), leftPrice);

                from.SendMessage("The deed is placed in your pack");
                from.Backpack.DropItem(new GovernmentPlayerVendorDeed(Government));
            }

        }

        public void SellEmployeeContract(PlayerMobile from, bool slave)
        {
            int totalCopper = 0;
            int price = slave ? Government.TradeInformation.SlavePrice : Government.TradeInformation.WageEarnerPrice;

            if (from.Backpack != null)
                totalCopper += from.Backpack.GetAmount(typeof(Copper));

            if (from.BankBox != null)
                totalCopper += from.BankBox.GetAmount(typeof(Copper));

            if (totalCopper < price)
            {
                this.Say("You cannot afford this.");
                return;
            }
            else
            {
                int leftPrice = price;

                if (from.Backpack != null)
                    leftPrice -= from.Backpack.ConsumeUpTo(typeof(Copper), leftPrice);

                if (leftPrice > 0 && from.BankBox != null)
                    from.BankBox.ConsumeUpTo(typeof(Copper), leftPrice);

                from.SendMessage("The contract is placed in your pack.");
                from.Backpack.DropItem(new EmployeeContract(Government, slave));
            }
        }

        public bool CheckEmployeeLimit(bool slave)
        {
            int empCount = 0;
            foreach (Mobile m in Government.Employees)
            {
                if (m is Employee)
                {
                    if (slave && (m as Employee).IsSlave)
                        empCount++;
                    else if (!slave && !(m as Employee).IsSlave)
                        empCount++;
                }
            }

            if (slave)
            {
                if (empCount >= Government.TradeInformation.MaxSlaves)
                    return false;
                else
                    return true;
            }
            else
            {
                if (empCount >= Government.TradeInformation.MaxWageEarners)
                    return false;
                else
                    return true;
            }
        }

        #region Speech and Dialogue
        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 12))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public static CraftResource Resource(string s)
        {
            switch (s)
            {
                case "copper": return CraftResource.Copper;
                case "tin": return CraftResource.Tin;
                case "iron": return CraftResource.Iron;
                case "silver": return CraftResource.Silver;
                case "gold": return CraftResource.Gold;
                case "cotton": return CraftResource.Cotton;
                case "wool": return CraftResource.Wool;
                case "silk": return CraftResource.Silk;
                case "leather": return CraftResource.RegularLeather;
                case "thick": return CraftResource.ThickLeather;
                case "beast": return CraftResource.BeastLeather;
                case "scaled": return CraftResource.ScaledLeather;
                case "oak": return CraftResource.Oak;
                case "yew": return CraftResource.Yew;
                case "redwood": return CraftResource.Redwood;
                case "ash": return CraftResource.Ash;
                default: return CraftResource.None;
            }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.InRange(this.Location, 2) && e.Mobile is PlayerMobile && AIObject.WasNamed(e.Speech))
            {

                PlayerMobile speaker = e.Mobile as PlayerMobile;

                if (!TradeInfo.CanDoBusiness(Government, speaker))
                {
                    this.Say("I'll have nothing to do with you.");
                    return;
                }

                string speech = e.Speech.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace("?", "").Replace(":", "").Replace(";", "").Replace("-", "");
                string[] split = speech.Split(' ');

                if (speech.Contains("manage"))
                {
                    if (CustomGuildStone.IsGuildEconomic(speaker, Government))
                    {
                        if (!speaker.HasGump(typeof(TradeGump)))
                            speaker.SendGump(new TradeGump(speaker, Government));
                        return;
                    }
                    else
                    {
                        Say("You do not have any authority over me!");
                        return;
                    }
                }
                else if (speech.Contains("face"))
                {
                    if (speech.Contains("north"))
                        this.Direction = Server.Direction.North;
                    else if (speech.ToLower().Contains("northeast"))
                        this.Direction = Server.Direction.Right;
                    else if (speech.ToLower().Contains("east"))
                        this.Direction = Server.Direction.East;
                    else if (speech.ToLower().Contains("southeast"))
                        this.Direction = Server.Direction.Down;
                    else if (speech.ToLower().Contains("south"))
                        this.Direction = Server.Direction.South;
                    else if (speech.ToLower().Contains("southwest"))
                        this.Direction = Server.Direction.Left;
                    else if (speech.ToLower().Contains("west"))
                        this.Direction = Server.Direction.West;
                    else if (speech.ToLower().Contains("northwest"))
                        this.Direction = Server.Direction.Up;
                    return;
                }
                else if (speech.Contains("prices"))
                    {
                         e.Mobile.SendMessage("Selling Metal @ " + Government.TradeInformation.MetalSalesPrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Buying Metal @ " + Government.TradeInformation.MetalPurchasePrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Selling Cloth @ " + Government.TradeInformation.ClothSalesPrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Buying Cloth @ " + Government.TradeInformation.ClothPurchasePrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Selling Wood @ " + Government.TradeInformation.WoodSalesPrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Buying Wood @ " + Government.TradeInformation.WoodPurchasePrice.ToString() + " copper a unit.");
                         e.Mobile.SendMessage("Selling vendors at " + Government.TradeInformation.VendorPrice.ToString() + " copper a contract; vendor wages are " + Government.TradeInformation.VendorWages.ToString() + " copper a month.");
                         if (Government.TradeInformation.SellsSlaves)
                            e.Mobile.SendMessage("Selling slaves at " + Government.TradeInformation.SlavePrice.ToString() + " copper a head.");
                        if (Government.TradeInformation.SellsWageEarners)
                            e.Mobile.SendMessage("Selling wage-earner contracts at " + Government.TradeInformation.WageEarnerPrice.ToString() + " copper a contract.");
                    }
                    else if (speech.Contains("rates"))
                    {
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Copper).ToString() + " " + TradeInfo.GetString(CraftResource.Copper) + " per metal unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Tin).ToString() + " " + TradeInfo.GetString(CraftResource.Tin) + " per metal unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Iron).ToString() + " " + TradeInfo.GetString(CraftResource.Iron) + " per metal unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Silver).ToString() + " " + TradeInfo.GetString(CraftResource.Silver) + " per metal unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Gold).ToString() + " " + TradeInfo.GetString(CraftResource.Gold) + " per metal unit.");

                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Cotton).ToString() + " " + TradeInfo.GetString(CraftResource.Cotton) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Wool).ToString() + " " + TradeInfo.GetString(CraftResource.Wool) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Silk).ToString() + " " + TradeInfo.GetString(CraftResource.Silk) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.RegularLeather).ToString() + " " + TradeInfo.GetString(CraftResource.RegularLeather) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.ThickLeather).ToString() + " " + TradeInfo.GetString(CraftResource.ThickLeather) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.BeastLeather).ToString() + " " + TradeInfo.GetString(CraftResource.BeastLeather) + " per cloth unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.ScaledLeather).ToString() + " " + TradeInfo.GetString(CraftResource.ScaledLeather) + " per cloth unit.");

                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Oak).ToString() + " " + TradeInfo.GetString(CraftResource.Oak) + " per wood unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Yew).ToString() + " " + TradeInfo.GetString(CraftResource.Yew) + " per wood unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Redwood).ToString() + " " + TradeInfo.GetString(CraftResource.Redwood) + " per wood unit.");
                        e.Mobile.SendMessage(TradeInfo.GetDivisor(CraftResource.Ash).ToString() + " " + TradeInfo.GetString(CraftResource.Ash) + " per wood unit.");
                    }
                else if (speech.Contains("help"))
                {
                
                        e.Mobile.SendMessage("Say 'prices' for information on this trader's prices.");
                        e.Mobile.SendMessage("Or, say 'rates' to get the exchange rates between resource types and craft resources.");
                        e.Mobile.SendMessage("Say 'buy,' the resource you want to buy (cotton, copper, etc.) and the amount in any order to buy a resource.");
                        e.Mobile.SendMessage("Say 'buy' and 'vendor' to purchase a vendor contract.");
                        e.Mobile.SendMessage("Say 'sell,' the resource, and the amount to sell a resource to the trader.");                        e.Mobile.SendMessage("Use 'donate' to donate resources to the trader's government.");
                        if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildEconomic(e.Mobile as PlayerMobile, Government))
                        {
                            e.Mobile.SendMessage("Say 'manage' to manage the trader's prices, ban lists, and other details of your government's economy.");
                            e.Mobile.SendMessage("Say 'face' and a direction to change the direction the trader is facing.");
                        }
                }
                else if (speech.Contains("withdraw"))
                {
                    if (CustomGuildStone.IsGuildEconomic(speaker, this.Government))
                    {
                        ResourceType resource = ResourceType.None;

                        if (speech.Contains("food"))
                            resource = ResourceType.Food;
                        else if (speech.Contains("water"))
                            resource = ResourceType.Water;

                        int amt = 0;

                        foreach (string findInt in split)
                        {
                            int val = 0;
                            if (ValidateInt(findInt, ref val))
                                amt = val;
                        }

                        if (amt > 0)
                        {
                            if (ResourceExistsInDictionary(resource))
                            {
                                if (Government.Resources[resource] >= amt)
                                {
                                    ResourceBit bit = new ResourceBit(resource, amt);
                                    Government.Resources[resource] -= amt;
                                    speaker.AddToBackpack(bit);
                                    speaker.SendMessage(amt.ToString() + " " + resource.ToString() + " is placed in your backpack.");
                                    speaker.PlaySound(0x057);
                                }
                            }
                        }
                            
                    }
                    else
                        this.Say("You cannot do that.");
                }
                else
                {
                    foreach (string word in split)
                    {
                        switch (word)
                        {
                            case "buy":
                                {
                                    if (e.Speech.ToLower().Contains("vendor"))
                                    {
                                        SellVendorDeed(speaker);
                                        break;
                                    }
                                    else if (e.Speech.ToLower().Contains("laborer"))
                                    {
                                        if (CheckEmployeeLimit(true))
                                            SellEmployeeContract(speaker, true);
                                        else
                                            this.Say("That would exceed the number of laborers we allow.");
                                        break;
                                    }
                                    else if (e.Speech.ToLower().Contains("worker") || e.Speech.ToLower().Contains("wage-earner"))
                                    {
                                        if (CheckEmployeeLimit(false))
                                            SellEmployeeContract(speaker, false);
                                        else
                                            this.Say("That would exceed the number of wage-earners we allow.");
                                        break;
                                    }
                                    else
                                    {
                                        int amt = 0;
                                        CraftResource res = CraftResource.None;
                                        foreach (string findInt in split)
                                        {
                                            int val = 0;
                                            if (ValidateInt(findInt, ref val))
                                                amt = val;
                                        }

                                        if (amt != 0 && amt > 0)
                                        {
                                            foreach (string findRes in split)
                                            {
                                                if (Resource(findRes) != CraftResource.None)
                                                {
                                                    res = Resource(findRes);
                                                    continue;
                                                }
                                            }
                                        }

                                        if (res != CraftResource.None)
                                        {
                                            TradeInfo.SellResource(this, speaker, res, amt);
                                            continue;
                                        }
                                        break;
                                    }
                                }
                            case "purchase":
                                 case "sell":
                                 {
                                     int amt = 0;
                                     CraftResource res = CraftResource.None;
                                     foreach (string findInt in split)
                                     {
                                         int val = 0;
                                         if (ValidateInt(findInt, ref val))
                                             amt = val;
                                     }
 
                                     if (amt != 0 && amt > 0)
                                     {
                                         foreach (string findRes in split)
                                         {
                                             if (Resource(findRes) != CraftResource.None)
                                             {
                                                 res = Resource(findRes);
                                                continue;
                                             }
                                         }
                                     }
 
                                     if (res != CraftResource.None)
                                     {
                                         TradeInfo.BuyResource(this, speaker, res, amt);
                                         continue;
                                     }
                                     break;
                                 }
                            case "donate":
                            case "give":
                                {
                                    int amt = 0;
                                    CraftResource res = CraftResource.None;
                                    foreach (string findInt in split)
                                    {
                                        int val = 0;
                                        if (ValidateInt(findInt, ref val))
                                            amt = val;
                                    }

                                    if (amt != 0 && amt > 0)
                                    {
                                        foreach (string findRes in split)
                                        {
                                            if (Resource(findRes) != CraftResource.None)
                                            {
                                                res = Resource(findRes);
                                                continue;
                                            }
                                        }
                                    }

                                    if (res != CraftResource.None)
                                    {
                                        TradeInfo.DonateResource(this, speaker, res, amt);
                                        continue;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            base.OnSpeech(e);
        }

        public bool ValidateInt(string st, ref int parsed)
        {
            if (int.TryParse(st, out parsed))
                return true;
            return false;
        }
        #endregion

        private bool ResourceExistsInDictionary(ResourceType resource)
        {
            return this.Government.Resources.ContainsKey(resource);
        }

        #region Serialization/Deserialization & Utilities

        public override void OnDelete()
        {
            Government.TradeAdvisor = null;

            base.OnDelete();
        }

        public TradeAdvisor(Serial serial) : base(serial)
		{

		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

        public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
        }
        #endregion
    }

    public class AddAdvisorTarget : Target
    {
        private GovernmentEntity m_Government;

        public AddAdvisorTarget(GovernmentEntity gov)
            : base(4, true, TargetFlags.None)
        {
            m_Government = gov;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {

            if (!(m_Government.TradeAdvisor == null || m_Government.TradeAdvisor.Deleted))
            {
                from.SendMessage("You already have a trade advisor deployed.");
                return;
            }

            TradeAdvisor newAdvisor = new TradeAdvisor(m_Government);

            if (targeted is Item)
                newAdvisor.MoveToWorld((targeted as Item).Location, (targeted as Item).Map);
            else if (targeted is Mobile)
                newAdvisor.MoveToWorld((targeted as Mobile).Location, (targeted as Mobile).Map);
            else if (targeted is LandTarget)
                newAdvisor.MoveToWorld((targeted as LandTarget).Location, from.Map);
            else
                newAdvisor.MoveToWorld(from.Location, from.Map);
                
            m_Government.TradeAdvisor = newAdvisor;
            newAdvisor.Home = newAdvisor.Location;            
            newAdvisor.RangeHome = 0;

            base.OnTarget(from, targeted);
        }
    }

    public class RemoveAdvisorTarget : Target
    {
        private GovernmentEntity m_Government;

        public RemoveAdvisorTarget(GovernmentEntity gov)
            : base(4, false, TargetFlags.None)
        {
            m_Government = gov;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {

            if (m_Government.TradeAdvisor == null || m_Government.TradeAdvisor.Deleted)
            {
                from.SendMessage("You do not have an advisor deployed by " + m_Government.Name.ToString() + ".");
                return;
            }

            if (!(targeted is TradeAdvisor))
            {
                from.SendMessage("That is not a trade advisor.");
                return;
            }

            if ((targeted as TradeAdvisor).Government != m_Government)
            {
                from.SendMessage("You may only remove trade advisors belonging to " + m_Government.Name.ToString() + ".");
                return;
            }

            (targeted as TradeAdvisor).Delete();
            m_Government.TradeAdvisor = null;

            base.OnTarget(from, targeted);
        }
    }

    public class TradeGump : Gump
    {
        private PlayerMobile m_Viewer;
        private GovernmentEntity m_Government;
        private int m_CurrentBan;
        private const int m_MaxBanNames = 5;

        private enum TradeButton
        {
            Okay,
            BanList,
            ScrollUp,
            ScrollDown,
            SellSlaves,
            SellWageEarners,
            FlatTax,
            BanNation
        }

        private enum Text
        {
            Budget,
            MSellAt,
            MLimit,
            MBuyAt,
            CSellAt,
            CLimit,
            CBuyAt,
            WSellAt,
            WLimit,
            WBuyAt,
            BanName,
            SlaveCost,
            SlaveLimit,
            WageCost,
            WageLimit,
            VendorCost,
            VendorWage,
            TaxRate
        }

        public TradeGump(PlayerMobile from, GovernmentEntity gov)
            : this(from, gov, 0)
        {

        }

        public TradeGump(PlayerMobile from, GovernmentEntity gov, int currentBan)
            : base(0, 0)
        {
            m_Viewer = from;
            m_Government = gov;
            m_CurrentBan = currentBan;
            InitialSetup();
        }

        #region Gump Setup
        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(TradeGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddResourcePage();
            AddBanPage();
            AddEmployeePage();

            AddBackground(305, 482, 189, 48, 9270);
            AddButton(371, 495, 249, 248, (int)TradeButton.Okay, GumpButtonType.Reply, 0);
        }

        public void AddResourcePage()
        {
            AddBackground(331, 101, 328, 193, 9270);
            AddBackground(431, 118, 212, 32, 9350);
            AddLabel(469, 125, m_Government.ClothingHue, "ECONOMIC ADVISORY");
            AddBackground(345, 118, 77, 52, 9350);
            AddLabel(358, 123, 247, "Budget:");
            AddTextEntry(352, 143, 61, 20, 0, (int)Text.Budget, m_Government.TradeInformation.BudgetMinimum.ToString());

            AddBackground(346, 174, 75, 31, 9350);
            AddLabel(356, 179, 247, "METALS:");
            AddBackground(346, 209, 75, 31, 9350);
            AddLabel(356, 214, 247, "CLOTH:");
            AddBackground(346, 244, 75, 31, 9350);
            AddLabel(356, 249, 247, "WOOD:");

            AddLabel(431, 151, 247, "Sell At");
            AddLabel(515, 151, 247, "Limit");
            AddLabel(596, 151, 247, "Buy At");

            AddBackground(431, 174, 45, 31, 9350);
            AddTextEntry(437, 179, 32, 20, 0, (int)Text.MSellAt, m_Government.TradeInformation.MetalSalesPrice.ToString());
            AddBackground(431, 209, 45, 31, 9350);
            AddTextEntry(437, 214, 32, 20, 0, (int)Text.CSellAt, m_Government.TradeInformation.ClothSalesPrice.ToString());
            AddBackground(431, 244, 45, 31, 9350);
            AddTextEntry(437, 249, 32, 20, 0, (int)Text.WSellAt, m_Government.TradeInformation.WoodSalesPrice.ToString());

            AddBackground(506, 174, 55, 31, 9350);
            AddTextEntry(513, 179, 39, 20, 0, (int)Text.MLimit, m_Government.TradeInformation.MetalMinimum.ToString());
            AddBackground(506, 209, 55, 31, 9350);
            AddTextEntry(513, 214, 39, 20, 0, (int)Text.CLimit, m_Government.TradeInformation.ClothMinimum.ToString());
            AddBackground(506, 244, 55, 31, 9350);
            AddTextEntry(513, 249, 39, 20, 0, (int)Text.WLimit, m_Government.TradeInformation.WoodMinimum.ToString());

            AddBackground(596, 174, 47, 31, 9350);
            AddTextEntry(602, 179, 39, 20, 0, (int)Text.MBuyAt, m_Government.TradeInformation.MetalPurchasePrice.ToString());
            AddBackground(596, 209, 47, 31, 9350);
            AddTextEntry(602, 214, 39, 20, 0, (int)Text.CBuyAt, m_Government.TradeInformation.ClothPurchasePrice.ToString());
            AddBackground(596, 244, 47, 31, 9350);
            AddTextEntry(602, 249, 39, 20, 0, (int)Text.WBuyAt, m_Government.TradeInformation.WoodPurchasePrice.ToString());
        }

        public void AddBanPage()
        {
            AddBackground(145, 101, 189, 384, 9270);
            AddLabel(168, 115, 247, "Banned from Business!");
            AddBackground(159, 136, 139, 30, 9350);
            AddTextEntry(164, 141, 126, 20, 0, (int)Text.BanName, "");
            AddButton(301, 142, 1896, 1895, (int)TradeButton.BanList, GumpButtonType.Reply, 0);
            AddBackground(159, 169, 161, 121, 9350);
            AddImageTiled(308, 177, 14, 107, 2712);
            AddButton(308, 169, 250, 251, (int)TradeButton.ScrollUp, GumpButtonType.Reply, 0);
            AddButton(308, 270, 252, 253, (int)TradeButton.ScrollDown, GumpButtonType.Reply, 0);
            int m_Y = 175;
            for (int i = m_CurrentBan * m_MaxBanNames; i < m_Government.TradeInformation.NoBusinessList.Count && i < (m_CurrentBan * m_MaxBanNames) + m_MaxBanNames; i++)
            {
                AddLabel(167, m_Y, 0, m_Government.TradeInformation.NoBusinessList[i]);
                m_Y += 22;
            }

            int m_X = 164;
            m_Y = 296;
            for (int i = 1; i < 7; i++)
            {
                AddButton(m_X, m_Y, m_Government.TradeInformation.NoBusinessNations.Contains((Nation)i) ? 9027 : 9026, m_Government.TradeInformation.NoBusinessNations.Contains((Nation)i) ? 9026 : 9027, (int)TradeButton.BanNation + i, GumpButtonType.Reply, 0);
                AddLabel(m_X + 30, m_Y, m_Government.TradeInformation.NoBusinessNations.Contains((Nation)i) ? 37 : 985, ((Nation)i).ToString());
                m_Y += 30;
            }
        }

        public void AddEmployeePage()
        {
            AddBackground(331, 291, 328, 192, 9270);

            #region Employee Section
            AddBackground(347, 307, 298, 86, 9350);
            AddLabel(375, 311, 247, "SELL?");
            AddLabel(493, 311, 247, "PRICE");
            AddLabel(584, 311, 247, "LIMIT");
            AddButton(352, 336, m_Government.TradeInformation.SellsSlaves ? 9027 : 9026, m_Government.TradeInformation.SellsSlaves ? 9026 : 9027, (int)TradeButton.SellSlaves, GumpButtonType.Reply, 0);
            AddLabel(375, 336, m_Government.TradeInformation.SellsSlaves ? 37 : 0, "Laborers");
            if (m_Government.TradeInformation.SellsSlaves)
            {
                AddTextEntry(489, 336, 45, 20, 0, (int)Text.SlaveCost, m_Government.TradeInformation.SlavePrice.ToString());
                AddTextEntry(579, 336, 45, 20, 0, (int)Text.SlaveLimit, m_Government.TradeInformation.MaxSlaves.ToString());
            }
            /* AddButton(352, 366, m_Government.TradeInformation.SellsWageEarners ? 9027 : 9026, m_Government.TradeInformation.SellsWageEarners ? 9026 : 9027, (int)TradeButton.SellWageEarners, GumpButtonType.Reply, 0); */
            AddLabel(375, 366, m_Government.TradeInformation.SellsWageEarners ? 37 : 0, "(Workers)");
            if (m_Government.TradeInformation.SellsWageEarners)
            {
                AddTextEntry(489, 366, 45, 20, 0, (int)Text.WageCost, m_Government.TradeInformation.WageEarnerPrice.ToString());
                AddTextEntry(579, 366, 45, 20, 0, (int)Text.WageLimit, m_Government.TradeInformation.MaxWageEarners.ToString());
            }
            #endregion

            #region Vendor Section
            AddBackground(347, 396, 147, 74, 9350);
            AddLabel(355, 409, 247, "Vendor Price:");
            AddTextEntry(444, 409, 41, 20, 0, (int)Text.VendorCost, m_Government.TradeInformation.VendorPrice.ToString());
            AddLabel(355, 440, 247, "Vendor Wage:");
            AddTextEntry(444, 440, 41, 20, 0, (int)Text.VendorWage, m_Government.TradeInformation.VendorWages.ToString());
            #endregion

            #region Taxation Section

            AddBackground(497, 396, 147, 74, 9350);
            AddLabel(507, 409, 247, "Tax Rate:");
            AddTextEntry(575, 408, 58, 20, 0, (int)Text.TaxRate, m_Government.TradeInformation.Taxes.ToString());
            AddButton(507, 440, m_Government.TradeInformation.FlatTax ? 9027 : 9026, m_Government.TradeInformation.FlatTax ? 9026 : 9027, (int)TradeButton.FlatTax, GumpButtonType.Reply, 0);
            AddLabel(534, 440, m_Government.TradeInformation.FlatTax ? 37 : 247, m_Government.TradeInformation.FlatTax ? "Flat Tax" : "% Tax");

            #endregion
        }
        #endregion

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID < (int)TradeButton.BanNation)
            {
                switch (info.ButtonID)
                {
                    case (int)TradeButton.Okay:
                        {
                            int val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.Budget).Text, "Budget", ref val))
                                m_Government.TradeInformation.BudgetMinimum = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.MSellAt).Text, "Metal Sell At", ref val))
                                m_Government.TradeInformation.MetalSalesPrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.CSellAt).Text, "Cloth Sell At", ref val))
                                m_Government.TradeInformation.ClothSalesPrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.WSellAt).Text, "Wood Sell At", ref val))
                                m_Government.TradeInformation.WoodSalesPrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.MLimit).Text, "Metal Limit", ref val))
                                m_Government.TradeInformation.MetalMinimum = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.CLimit).Text, "Cloth Limit", ref val))
                                m_Government.TradeInformation.ClothMinimum = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.WLimit).Text, "Wood Limit", ref val))
                                m_Government.TradeInformation.WoodMinimum = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.MBuyAt).Text, "Metal Buy At", ref val))
                                m_Government.TradeInformation.MetalPurchasePrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.CBuyAt).Text, "Cloth Buy At", ref val))
                                m_Government.TradeInformation.ClothPurchasePrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.WBuyAt).Text, "Wood Buy At", ref val))
                                m_Government.TradeInformation.WoodPurchasePrice = val;
                            val = 0;
                            if (m_Government.TradeInformation.SellsSlaves && info.ButtonID != (int)TradeButton.SellSlaves)
                            {
                                val = 0;
                                if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.SlaveCost).Text, "Laborer Cost", ref val))
                                    m_Government.TradeInformation.SlavePrice = val;
                                val = 0;
                                if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.SlaveLimit).Text, "Laborer Limit", ref val))
                                    m_Government.TradeInformation.MaxSlaves = val;
                                if (m_Government.TradeInformation.MaxSlaves > m_Government.Resources[ResourceType.Influence] / 50)
                                {
                                    m_Viewer.SendMessage("You cannot have more laborers in existence than (your Influence / 50).");
                                    m_Government.TradeInformation.MaxSlaves = m_Government.Resources[ResourceType.Influence] / 50;
                                }
                            }
                            if (m_Government.TradeInformation.SellsWageEarners && info.ButtonID != (int)TradeButton.SellWageEarners)
                            {
                                val = 0;
                                if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.WageCost).Text, "Wage-Earner Cost", ref val))
                                    m_Government.TradeInformation.WageEarnerPrice = val;
                                val = 0;
                                if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.WageLimit).Text, "Wage-Earner Limit", ref val))
                                    m_Government.TradeInformation.MaxWageEarners = val;
                                if (m_Government.TradeInformation.MaxWageEarners > m_Government.Resources[ResourceType.Influence] / 50)
                                {
                                    m_Viewer.SendMessage("You cannot have more wage-earners in existence than (your Influence / 50).");
                                    m_Government.TradeInformation.MaxWageEarners = m_Government.Resources[ResourceType.Influence] / 50;
                                }
                            }
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.VendorCost).Text, "Vendor Price", ref val))
                                m_Government.TradeInformation.VendorPrice = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.VendorWage).Text, "Vendor Wage", ref val))
                                m_Government.TradeInformation.VendorWages = val;
                            val = 0;
                            if (ValidateInt(m_Viewer, info.GetTextEntry((int)Text.TaxRate).Text, "Tax Rate", ref val))
                                m_Government.TradeInformation.Taxes = val;

                            if (info.ButtonID == (int)TradeButton.Okay)
                                return;
                            else
                            {
                                m_Viewer.SendGump(new TradeGump(m_Viewer, m_Government, m_CurrentBan));
                                return;
                            }
                        }
                    case (int)TradeButton.BanList:
                        {
                            string enteredText = info.GetTextEntry((int)Text.BanName).Text;
                            if (m_Government.TradeInformation.NoBusinessList.Contains(enteredText))
                            {
                                m_Government.TradeInformation.NoBusinessList.Remove(enteredText);
                                m_Viewer.SendMessage(enteredText + " removed from the No Business List.");
                            }
                            else
                            {
                                m_Government.TradeInformation.NoBusinessList.Add(enteredText);
                                m_Viewer.SendMessage(enteredText + " added to the No Business List.");
                            }

                            goto case (int)(TradeButton.Okay);
                        }
                    case (int)TradeButton.ScrollUp:
                        {
                            if (m_CurrentBan > 0)
                                m_CurrentBan--;
                            goto case (int)(TradeButton.Okay);
                        }
                    case (int)TradeButton.ScrollDown:
                        {
                            if ((m_CurrentBan + 1) * m_MaxBanNames < m_Government.TradeInformation.NoBusinessList.Count)
                                m_CurrentBan++;
                            goto case (int)(TradeButton.Okay);
                        }
                    case (int)TradeButton.SellSlaves:
                        {
                            if (m_Government.TradeInformation.SellsSlaves)
                                m_Government.TradeInformation.SellsSlaves = false;
                            else
                                m_Government.TradeInformation.SellsSlaves = true;
                            goto case (int)(TradeButton.Okay);
                        }
                    case (int)TradeButton.SellWageEarners:
                        {
                            if (m_Government.TradeInformation.SellsWageEarners)
                                m_Government.TradeInformation.SellsWageEarners = false;
                            else
                                m_Government.TradeInformation.SellsWageEarners = true;
                            goto case (int)(TradeButton.Okay);
                        }
                    case (int)TradeButton.FlatTax:
                        {
                            if (m_Government.TradeInformation.FlatTax)
                                m_Government.TradeInformation.FlatTax = false;
                            else
                                m_Government.TradeInformation.FlatTax = true;
                            goto case (int)(TradeButton.Okay);
                        }
                }
            }
            else
            {
                if (m_Government.TradeInformation.NoBusinessNations.Contains((Nation)(info.ButtonID - (int)TradeButton.BanNation)))
                    m_Government.TradeInformation.NoBusinessNations.Remove((Nation)(info.ButtonID - (int)TradeButton.BanNation));
                else
                    m_Government.TradeInformation.NoBusinessNations.Add((Nation)(info.ButtonID - (int)TradeButton.BanNation));
                m_Viewer.SendGump(new TradeGump(m_Viewer, m_Government, m_CurrentBan));
                return;
            }
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

        public void SendNewGump()
        {
            m_Viewer.SendGump(new TradeGump(m_Viewer, m_Government));
        }
    }
}
