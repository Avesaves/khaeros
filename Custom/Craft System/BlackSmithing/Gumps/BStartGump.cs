using Server;
using Server.Items;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using System;

namespace Server.Gumps
{
    public class BStartGump : Gump
    {
        private enum Buttons
        {
            Start,
            Cancel,
            Resource,
            Armor,
            Weapons,
            AttackPieces,
            Hilts,
            //Embellishments,
            Piece1,
            Piece2,
            Piece3,
            Piece4,
            Piece5,
            Piece6,
            Piece7,
            Piece8,
            Back
        }

        private BlackSmithingCraftState m_CraftState;
        private PlayerMobile m_Crafter;
        private BaseTool m_Tool;
        private BStartContext m_Context;

        public BaseTool Tool{ get { return m_Tool; }  }        

        //private string m_PieceName = "None";
        //private int m_PiecePage = 1;

        //private BaseIngot m_Ingot;

        public BStartGump(PlayerMobile crafter, BlackSmithingCraftState craftstate, BaseTool tool, BStartContext context)
            : base( 0, 0)
        {
            m_CraftState = craftstate;
            m_Crafter = crafter;
            m_Tool = tool;

            if (context == null)
                m_Context = new BStartContext();
            else
                m_Context = context;

            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);

            InitialSetUp();

            if (m_Context.Ingot != null)
                AddResourceImage(m_Context.Ingot);

            switch (m_Context.Page)
            {
                case 1: InitialPiecePage(); break;
                case 2: ArmorPiecePage(); break;
                case 3: WeaponPiecePage(); break;
                case 4: AttackPiecePage(); break;
                case 5: HiltsPage(); break;
                //case 6: EmbellishmentsPage(); break;
            }
        }

        public void InitialSetUp()
        {
            AddBackground(213, 201, 421, 212, 9270);
            AddLabel(235, 212, 0, "Blacksmithing - Khaeros");

            //Resource Section
            AddBackground(225, 236, 175, 167, 9270);
            AddLabel(281, 254, 0, "Resource");
            AddButton(287, 280, 9802, 9802, (int)Buttons.Resource, GumpButtonType.Reply, 0);
            AddLabel(243, 342, 0, "Piece :");
            AddLabel(295, 342, 0, m_Context.Name); //Label for type of piece being made.
            AddButton(241, 366, 247, 248, (int)Buttons.Start, GumpButtonType.Reply, 0);
            AddButton(317, 366, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

            //Piece Selection Section Background
            AddBackground(400, 211, 226, 192, 9270);
            AddBackground(412, 224, 203, 168, 9350);
        }

        public void AddResourceImage(BaseIngot ingot)
        {
            AddItem(295, 289, ingot.ItemID, ingot.Hue);
        }

        public void InitialPiecePage()
        {
            AddButton(424, 236, 4005, 4005, (int)Buttons.Armor, GumpButtonType.Reply, 0);
            AddButton(424, 289, 4005, 4005, (int)Buttons.Weapons, GumpButtonType.Reply, 0);
            AddLabel(464, 239, 0, "Armor");
            AddLabel(464, 292, 0, "Weapons");
        }

        public void ArmorPiecePage()
        {
            //Buttons
            AddButton(419, 231, 4005, 4005, (int)Buttons.Piece1, GumpButtonType.Reply, 0);
            AddButton(419, 257, 4005, 4005, (int)Buttons.Piece2, GumpButtonType.Reply, 0);
            AddButton(419, 283, 4005, 4005, (int)Buttons.Piece3, GumpButtonType.Reply, 0);
            AddButton(419, 310, 4005, 4005, (int)Buttons.Piece4, GumpButtonType.Reply, 0);
            AddButton(419, 334, 4005, 4005, (int)Buttons.Piece5, GumpButtonType.Reply, 0);
            AddButton(419, 360, 4005, 4005, (int)Buttons.Piece6, GumpButtonType.Reply, 0);
            AddButton(582, 369, 4014, 4014, (int)Buttons.Back, GumpButtonType.Reply, 0);

            //Labels
            AddLabel(460, 234, 0, "Small Plate");
            AddLabel(460, 260, 0, "Large Plate");            
            AddLabel(460, 286, 0, "Small Cylinders");
            AddLabel(460, 313, 0, "Large Cylinders");
            AddLabel(460, 337, 0, "Set of Rings");
            AddLabel(460, 363, 0, "Set of Scales");
        }

        public void WeaponPiecePage()
        {
            //Buttons
            AddButton(424, 236, 4005, 4005, (int)Buttons.AttackPieces, GumpButtonType.Reply, 0);
            AddButton(424, 289, 4005, 4005, (int)Buttons.Hilts, GumpButtonType.Reply, 0);
            //AddButton(424, 338, 4005, 4005, (int)Buttons.Embellishments, GumpButtonType.Reply, 0);
            AddButton(582, 369, 4014, 4014, (int)Buttons.Back, GumpButtonType.Reply, 0);

            //Labels
            AddLabel(464, 239, 0, "Attack Pieces");
            AddLabel(464, 292, 0, "Hilts & Handles");
            //AddLabel(464, 341, 0, "Embellishments");
        }

        public void AttackPiecePage()
        {
            //Buttons
            AddButton(419, 226, 4005, 4005, (int)Buttons.Piece1, GumpButtonType.Reply, 0);
            AddButton(419, 246, 4005, 4005, (int)Buttons.Piece2, GumpButtonType.Reply, 0);
            AddButton(419, 266, 4005, 4005, (int)Buttons.Piece3, GumpButtonType.Reply, 0);
            AddButton(419, 286, 4005, 4005, (int)Buttons.Piece4, GumpButtonType.Reply, 0);
            AddButton(419, 306, 4005, 4005, (int)Buttons.Piece5, GumpButtonType.Reply, 0);
            AddButton(419, 326, 4005, 4005, (int)Buttons.Piece6, GumpButtonType.Reply, 0);
            AddButton(419, 346, 4005, 4005, (int)Buttons.Piece7, GumpButtonType.Reply, 0);
            AddButton(419, 366, 4005, 4005, (int)Buttons.Piece8, GumpButtonType.Reply, 0);
            AddButton(582, 369, 4014, 4014, (int)Buttons.Back, GumpButtonType.Reply, 0);

            //Labels
            AddLabel(460, 229, 0, "Small Blade");
            AddLabel(460, 249, 0, "Medium Blade");
            AddLabel(460, 269, 0, "Large Blade");
            AddLabel(460, 289, 0, "Curved Blade");
            AddLabel(460, 309, 0, "Mace Head");
            AddLabel(460, 329, 0, "Hammer Head");
            AddLabel(460, 349, 0, "Axe Blade");
            AddLabel(460, 369, 0, "Dual Axe Blade");
        }

        public void HiltsPage()
        {
            //Buttons
            AddButton(419, 226, 4005, 4005, (int)Buttons.Piece1, GumpButtonType.Reply, 0);
            AddButton(419, 246, 4005, 4005, (int)Buttons.Piece2, GumpButtonType.Reply, 0);
            AddButton(419, 266, 4005, 4005, (int)Buttons.Piece3, GumpButtonType.Reply, 0);
            AddButton(419, 286, 4005, 4005, (int)Buttons.Piece4, GumpButtonType.Reply, 0);
            AddButton(419, 306, 4005, 4005, (int)Buttons.Piece5, GumpButtonType.Reply, 0);
            AddButton(419, 326, 4005, 4005, (int)Buttons.Piece6, GumpButtonType.Reply, 0);
            AddButton(419, 346, 4005, 4005, (int)Buttons.Piece7, GumpButtonType.Reply, 0);            
            AddButton(582, 369, 4014, 4014, (int)Buttons.Back, GumpButtonType.Reply, 0);

            //Labels
            AddLabel(460, 229, 0, "Small Cruciform Hilt");
            AddLabel(460, 249, 0, "Large Cruciform Hilt");
            AddLabel(460, 269, 0, "Basket Hilt");
            AddLabel(460, 289, 0, "Small Cresent Hilt");
            AddLabel(460, 309, 0, "Large Cresent Hilt");
            AddLabel(460, 329, 0, "Small Handle");
            AddLabel(460, 349, 0, "Large Handle");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)Buttons.Start: StartCrafting(); break;
                case (int)Buttons.Cancel: m_Crafter.CloseGump(typeof(BStartGump)); break;
                case (int)Buttons.Resource: SetResource(); break;
                case (int)Buttons.Armor: m_Context.Page = 2; m_Crafter.CloseGump(typeof(BStartGump)); m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case (int)Buttons.Weapons: m_Context.Page = 3; m_Crafter.CloseGump(typeof(BStartGump)); m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case (int)Buttons.AttackPieces: m_Context.Page = 4; m_Crafter.CloseGump(typeof(BStartGump)); m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case (int)Buttons.Hilts: m_Context.Page = 5; m_Crafter.CloseGump(typeof(BStartGump)); m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                //case (int)Buttons.Embellishments: m_Context.Page = 6; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case (int)Buttons.Piece1: SetPiece(1, m_Context.Page); break;
                case (int)Buttons.Piece2: SetPiece(2, m_Context.Page); break;
                case (int)Buttons.Piece3: SetPiece(3, m_Context.Page); break;
                case (int)Buttons.Piece4: SetPiece(4, m_Context.Page); break;
                case (int)Buttons.Piece5: SetPiece(5, m_Context.Page); break;
                case (int)Buttons.Piece6: SetPiece(6, m_Context.Page); break;
                case (int)Buttons.Piece7: SetPiece(7, m_Context.Page); break;
                case (int)Buttons.Piece8: SetPiece(8, m_Context.Page); break;
                case (int)Buttons.Back: GoBack(m_Context.Page); break;
            }
        }

        public void SetResource()
        {
            m_Crafter.Target = new BStartTarget(this, m_CraftState, m_Context);
        }

        public void SetPiece(int piece, int page)
        {
            int i = piece;

            if (page > 3)
                i += 6;
            if (page > 4)
                i += 8;

            switch (i)
            {
                case 1: m_CraftState.SetPieceType(new SmallPlate()); m_Context.Name = "Small Plate"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 2: m_CraftState.SetPieceType(new LargePlate()); m_Context.Name = "Large Plate"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 3: m_CraftState.SetPieceType(new SmallCylinders()); m_Context.Name = "Small Cylinders"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 4: m_CraftState.SetPieceType(new LargeCylinders()); m_Context.Name = "Large Cylinders"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 5: m_CraftState.SetPieceType(new SetOfRings()); m_Context.Name = "Set Of Rings"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 6: m_CraftState.SetPieceType(new SetOfScales()); m_Context.Name = "Set of Scales"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 7: m_CraftState.SetPieceType(new ShortBlade()); m_Context.Name = "Short Blade"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 8: m_CraftState.SetPieceType(new MediumBlade()); m_Context.Name = "Medium Blade"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 9: m_CraftState.SetPieceType(new LongBlade()); m_Context.Name = "Long Blade"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 10: m_CraftState.SetPieceType(new CurvedBlade()); m_Context.Name = "Curved Blade"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 11: m_CraftState.SetPieceType(new MaceHead()); m_Context.Name = "Mace Head"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 12: m_CraftState.SetPieceType(new HammerHead()); m_Context.Name = "Hammer Head"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 13: m_CraftState.SetPieceType(new AxeHead()); m_Context.Name = "Axe Head"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 14: m_CraftState.SetPieceType(new DualAxeHead()); m_Context.Name = "Dual Axe Head"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 15: m_CraftState.SetPieceType(new SmallCruciformHilt()); m_Context.Name = "Small Cruciform Hilt"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 16: m_CraftState.SetPieceType(new LargeCruciformHilt()); m_Context.Name = "Large Cruciform Hilt"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 17: m_CraftState.SetPieceType(new BasketHilt()); m_Context.Name = "Basket Hilt"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 18: m_CraftState.SetPieceType(new SmallCrescentHilt()); m_Context.Name = "Small Crescent Hilt"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 19: m_CraftState.SetPieceType(new LargeCrescentHilt()); m_Context.Name = "Large Crescent Hilt"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 20: m_CraftState.SetPieceType(new SmallHandle()); m_Context.Name = "Small Handle"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
                case 21: m_CraftState.SetPieceType(new LargeHandle()); m_Context.Name = "Large Handle"; m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context)); break;
            }
        }

        public void GoBack(int page)
        {
            if (page == 2 || page == 3)
                m_Context.Page = 1;
            if (page == 4 || page == 5)
                m_Context.Page = 3;

            m_Crafter.SendGump(new BStartGump(m_Crafter, m_CraftState, m_Tool, m_Context));
        }

        public void StartCrafting()
        {
            if (m_Context.Name == "None")
            {
                m_Crafter.SendMessage("You have not chosen a piece to make yet.");
                return;
            }
            else if (m_Context.Ingot == null)
            {
                m_Crafter.SendMessage("You have not chosen any materials to use yet.");
                return;
            }
            else if (m_Context.Ingot.Amount < m_CraftState.TotalComplete)
            {
                m_Crafter.SendMessage("You do not have enough material to make this item.");
                return;
            }
            else if (m_CraftState.Piece is BaseArmorPiece && m_Context.Ingot is ObsidianIngot)
            {
                m_Crafter.SendMessage("Obsidian can only be used to make weapons");
                return;
            }
            else
            {
                m_Crafter.CloseGump(typeof(BStartGump));
                m_Tool.UsesRemaining--;
                m_CraftState.StartCraftState();
            }
        }
    }

    public class BStartTarget : Target
    {
        private BlackSmithingCraftState m_Craft;
        private BStartGump m_Owner;
        private BStartContext m_Context;

        public BStartTarget(BStartGump from, BlackSmithingCraftState craftstate, BStartContext context)
            : base(2, false, TargetFlags.None)
        {
            m_Owner = from;
            m_Craft = craftstate;
            m_Context = context;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is BaseIngot)
            {
                BaseIngot i = targeted as BaseIngot;
                if ((i.Resource == CraftResource.Obsidian && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Obsidian) < 3) || (i.Resource == CraftResource.Steel && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Steel) < 3))                
                {
                    from.SendLocalizedMessage(1063491); // Your race cannot craft that item.
                    return;
                }
                else
                {
                    m_Craft.SetResourceType(i.Resource);
                    m_Context.Ingot = i;
                    from.SendGump(new BStartGump((PlayerMobile)from, m_Craft, m_Craft.Tool, m_Context));
                    return;
                } 
            }
            else
                from.SendMessage("You must target a metal resouce to use!");
            return;
        }
    }

    public class BStartContext
    {
        private string m_PieceName = "None";
        private int m_PiecePage = 1;
        private BaseIngot m_Ingot = null;

        public string Name
        {
            get { return m_PieceName; }
            set { m_PieceName = value; }
        }

        public int Page
        {
            get { return m_PiecePage; }
            set { m_PiecePage = value; }
        }

        public BaseIngot Ingot
        {
            get { return m_Ingot; }
            set { m_Ingot = value; }
        }

        public BStartContext()
        {
        }
    }
}
