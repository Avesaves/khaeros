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
    public class BlacksmithingGump : Gump
    {
        private PlayerMobile m_Crafter;
        private BlackSmithingCraftState m_CraftState;

        public BlacksmithingGump(PlayerMobile crafter, BlackSmithingCraftState craftstate): base(0, 0)
        {
            m_Crafter = crafter;
            m_CraftState = craftstate;

            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            InitialSetup();
        }

        public void InitialSetup()
        {
            AddBackground(260, 89, 288, 394, 9270);
            AddBackground(275, 105, 257, 36, 9350);
            AddLabel(334, 113, 0, "Khaeros Blacksmithing");

            //WORK section
            AddBackground(275, 148, 75, 72, 9350);
            AddLabel(291, 158, 987, "WORK");
            AddLabel(299, 187, 987, m_CraftState.Work.ToString());

            AddBackground(355, 148, 177, 72, 9350);
            AddButton(362, 155, 6145, 6072, (int)Buttons.WorkLight, GumpButtonType.Reply, 0);
            AddLabel(369, 199, 0, "Soft");
            AddButton(421, 155, 6145, 6072, (int)Buttons.WorkMedium, GumpButtonType.Reply, 0);
            AddLabel(421, 199, 832, "Medium");
            AddButton(480, 155, 6145, 6072, (int)Buttons.WorkHeavy, GumpButtonType.Reply, 0);
            AddLabel(480, 199, 1898, "Hard");

            //HEAT section
            AddBackground(275, 225, 75, 72, 9350);
            AddLabel(295, 234, 48, "HEAT");
            AddLabel(299, 262, 48, m_CraftState.Heat.ToString());

            AddBackground(355, 225, 177, 72, 9350);
            AddButton(362, 232, 6141, 6184, (int)Buttons.HeatLight, GumpButtonType.Reply, 0);
            AddLabel(368, 275, 50, "Light");
            AddButton(421, 232, 6141, 6184, (int)Buttons.HeatMedium, GumpButtonType.Reply, 0);
            AddLabel(421, 275, 48, "Medium");
            AddButton(480, 232, 6141, 6184, (int)Buttons.HeatHeavy, GumpButtonType.Reply, 0);
            AddLabel(490, 275, 42, "Hot");

            //Quench, Variable Details
            AddBackground(472, 302, 60, 72, 9350);
            AddButton(480, 309, 6140, 6139, (int)Buttons.Quench, GumpButtonType.Reply, 0);
            AddLabel(380, 351, 97, "Quench");

            AddBackground(275, 302, 192, 33, 9350);
            AddLabel(283, 309, 0, "QUALITY:  " + m_CraftState.Piece.Quality.ToString());
            AddLabel(383, 309, 0, "RND:  " + m_CraftState.Round.ToString() + "/" + m_CraftState.TotalComplete.ToString());

            AddBackground(275, 340, 192, 33, 9350);
            AddLabel(283, 347, 0, "DURABILITY: " + m_CraftState.Piece.Durability.ToString());
			AddLabel(405, 347, 0, "TRN: " + m_CraftState.TotalTurns.ToString());

            if(m_CraftState.Piece is BaseArmorPiece)
                AddArmorDetails();
            else
                AddWeaponDetails();

            //AddButton(368, 472, 12000, 12002, (int)Buttons.Accept, GumpButtonType.Reply, 0);
        }

        public void AddArmorDetails()
        {
            BaseArmorPiece a = m_CraftState.Piece as BaseArmorPiece;

            AddBackground(278, 384, 253, 81, 9200);
            AddLabel(287, 390, 0, "Crafting... " + m_CraftState.Piece.Name); // Wraithe -- will this work? The player should be aware of the item he is crafting.
            AddLabel(287, 415, 0, "Piercing: " + a.Pierce.ToString());
            AddLabel(287, 440, 0, "Slashing: " + a.Slash.ToString());
            AddLabel(400, 415, 0, "Blunt: " + a.Blunt.ToString());
        }

        public void AddWeaponDetails()
        {
            BaseWeaponPiece w = m_CraftState.Piece as BaseWeaponPiece;

            AddBackground(278, 384, 253, 81, 9200);
            AddLabel(287, 390, 0, "Crafting..." + w.Name);
            AddLabel(287, 415, 0, "Damage:  " + w.Damage.ToString());
            AddLabel(287, 440, 0, "Speed:  " + w.Speed.ToString());
            AddLabel(400, 415, 0, "Hit Chance:  " + w.Attack.ToString());
            AddLabel(400, 440, 0, "Defend Chance:  " + w.Defense.ToString());
        }

        public enum Buttons
        {
            WorkHeavy = 1,
            WorkMedium,
            WorkLight,
            HeatHeavy,
            HeatMedium,
            HeatLight,
            Quench           
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)Buttons.WorkHeavy: this.m_CraftState.DoWork(3); new InternalTimer(m_Crafter, 0).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.WorkMedium: this.m_CraftState.DoWork(2); new InternalTimer(m_Crafter, 2).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.WorkLight: this.m_CraftState.DoWork(1); new InternalTimer(m_Crafter, 3).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.HeatHeavy: this.m_CraftState.AddHeat(3); new InternalTimer3(m_Crafter, 0).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.HeatMedium: this.m_CraftState.AddHeat(2); new InternalTimer3(m_Crafter, 1).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.HeatLight: this.m_CraftState.AddHeat(1); new InternalTimer3(m_Crafter, 2).Start(); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;
                case (int)Buttons.Quench: this.m_CraftState.DoQuench(); m_Crafter.PlaySound(0x5AF); m_Crafter.CloseGump(typeof(BlacksmithingGump)); break;                
            }
        }

        private class InternalTimer : Timer
        {
            private PlayerMobile m_From;
            private int m_turn;

            public InternalTimer(PlayerMobile from, int turn)
                : base(TimeSpan.FromSeconds(1.4))
            {
                if (from.Body.Type == BodyType.Human && !from.Mounted)
                    from.Animate(9, (5 - turn), 1, true, false, 0);

                m_From = from;
                m_turn = turn;
            }

            protected override void OnTick()
            {
                m_From.PlaySound(0x2A);

                if (m_turn < 4)
                    new InternalTimer2(m_From, m_turn + 1).Start();
            }
        }

        private class InternalTimer2 : Timer
        {
            private PlayerMobile m_From;
            private int m_turn;

            public InternalTimer2(PlayerMobile from, int turn)
                : base(TimeSpan.FromSeconds(1.6))
            {
                m_From = from;
                m_turn = turn;
            }

            protected override void OnTick()
            {
                new InternalTimer(m_From, m_turn).Start();
            }
        }

        private class InternalTimer3 : Timer
        {
            private PlayerMobile m_From;
            private int m_turn;

            public InternalTimer3(PlayerMobile from, int turn)
                : base(TimeSpan.FromSeconds(1.4))
            {
                m_From = from;
                m_turn = turn;
            }

            protected override void OnTick()
            {
                m_From.PlaySound(0x2B);

                if (m_turn < 3)
                    new InternalTimer3(m_From, m_turn + 1).Start();
            }
        }
    }
}
