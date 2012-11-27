using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Misc;

namespace Server.Gumps
{
    public class CustomPrayerStripGump : Gump
    {
        private enum PrayerButton
        {
            Clear = 0,
            Accept = 1,
            PrevEffect = 2,
            NextEffect = 3,
            PrevComponent = 4,
            NextComponent = 5
        }

        private enum PrayerText
        {
            Name,
            Intensity,
            Seconds,
            Range,
            Area,
            Repetitions,
            Delay,
            Emote,
            Speech,
            TargetMessage,
            SoundID
        }

        private string GetEffectName
        {
            get
            {
                switch (m_Strip.Prayer.Effect)
                {
                    case PrayerEffect.Strength: return "Strength";
                    case PrayerEffect.Dexterity: return "Dexterity";
                    case PrayerEffect.Intelligence: return "Intelligence";
                    case PrayerEffect.RawHitPoints: return "Raw Hit Points";
                    case PrayerEffect.CurrentHitPoints: return "Current Hit Points";
                    case PrayerEffect.RawStamina: return "Raw Stamina";
                    case PrayerEffect.CurrentStamina: return "Current Stamina";
                    case PrayerEffect.RawMana: return "Raw Mana";
                    case PrayerEffect.CurrentMana: return "Current Mana";
                    case PrayerEffect.BluntResistance: return "Blunt Resistance";
                    case PrayerEffect.PiercingResistance: return "Piercing Resistance";
                    case PrayerEffect.SlashingResistance: return "Slashing Resistance";
                    case PrayerEffect.ColdResistance: return "Cold Resistance";
                    case PrayerEffect.FireResistance: return "Fire Resistance";
                    case PrayerEffect.PoisonResistance: return "Poison Resistance";
                    case PrayerEffect.EnergyResistance: return "Energy Resistance";
                    case PrayerEffect.AttackChance: return "Attack Chance";
                    case PrayerEffect.DefendChance: return "Defend Chance";
                    default: return "None";
                }
            }
        }

        private string GetComponentName
        {
            get
            {
                Type comp = m_Strip.Prayer.Component;

                if (comp == typeof(Frankincense))
                    return "Frankincense";
                else if (comp == typeof(Myrrh))
                    return "Myrrh";
                else if (comp == typeof(Patchouli))
                    return "Patchouli";
                else if (comp == typeof(Saffron))
                    return "Saffron";
                else if (comp == typeof(Sandelwood))
                    return "Sandelwood";
                else if (comp == typeof(Dragonsblood))
                    return "Dragonsblood";
                else if (comp == typeof(Thyme))
                    return "Thyme";
                else if (comp == typeof(Clove))
                    return "Clove";
                else if (comp == typeof(Opium))
                    return "Opium";
                else if (comp == typeof(Swampweed))
                    return "Swampweed";
                else if (comp == typeof(BanestoneAsh))
                    return "Ash";
                else if (comp == typeof(Bone))
                    return "Bone";
                else if (comp == typeof(Head))
                    return "Human Head";
                else if (comp == typeof(HolyWater))
                    return "Holy Water";
                else if (comp == null)
                    return "None";

                return "Unavailable";
            }
        }

        private PlayerMobile m_Viewer;
        private CustomPrayerStrip m_Strip;

        public CustomPrayerStripGump(PlayerMobile viewer, CustomPrayerStrip strip)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Strip = strip;
            if (m_Strip.Prayer == null)
                m_Strip.Prayer = new CustomFaithSpell();
            InitialSetup();
        }

        private void InitialSetup()
        {
            if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                return;

            if (m_Strip == null || m_Strip.Deleted || m_Strip.RootParentEntity != m_Viewer)
                return;

            m_Viewer.CloseGump(typeof(CustomPrayerStripGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            #region Background, Buttons, Immutables
            AddBackground(6, 5, 318, 565, 9390);
            AddLabel(68, 8, 247, "Khaeros' Custom Prayer Writer");
            AddLabel(113, 547, 247, "Mana Cost: " + m_Strip.Prayer.ManaCost);
            AddButton(32, 549, 12000, 12002, (int)PrayerButton.Accept, GumpButtonType.Reply, 0);
            AddButton(223, 549, 12003, 12005, (int)PrayerButton.Clear, GumpButtonType.Reply, 0);
            #endregion

            #region Prayer Stats
            AddBackground(32, 45, 268, 30, 9350);
            AddLabel(38, 50, 0, "Prayer Name:");
            AddTextEntry(133, 50, 158, 20, 0, (int)PrayerText.Name, m_Strip.Prayer.Name);

            AddBackground(32, 80, 268, 30, 9350);
            AddButton(36, 84, 5537, 5539, (int)PrayerButton.PrevEffect, GumpButtonType.Reply, 0);
            AddButton(272, 84, 5540, 5542, (int)PrayerButton.NextEffect, GumpButtonType.Reply, 0);
            AddLabel(169 - (int)(GetEffectName.Length * 3.1), 86, 0, GetEffectName);

            AddBackground(32, 115, 127, 30, 9350);
            AddLabel(38, 119, 0, "Intensity:");
            AddTextEntry(103, 119, 49, 20, 0, (int)PrayerText.Intensity, m_Strip.Prayer.Intensity.ToString());

            AddBackground(32, 150, 127, 30, 9350);
            AddLabel(38, 154, 0, "Range:");
            AddTextEntry(88, 155, 62, 20, 0, (int)PrayerText.Range, m_Strip.Prayer.Range.ToString());

            AddBackground(32, 185, 127, 30, 9350);
            AddLabel(38, 190, 0, "Repetitions:");
            AddTextEntry(117, 190, 32, 20, 0, (int)PrayerText.Repetitions, m_Strip.Prayer.Repetitions.ToString());

            AddBackground(172, 115, 127, 30, 9350);
            AddLabel(178, 119, 0, "Seconds:");
            AddTextEntry(238, 119, 53, 20, 0, (int)PrayerText.Seconds, m_Strip.Prayer.Duration.ToString());

            AddBackground(172, 150, 127, 30, 9350);
            AddLabel(178, 155, 0, "Area:");
            AddTextEntry(219, 155, 72, 20, 0, (int)PrayerText.Area, m_Strip.Prayer.Area.ToString());

            AddBackground(172, 185, 127, 30, 9350);
            AddLabel(178, 190, 0, "Rep. Delay:");
            AddTextEntry(254, 190, 38, 20, 0, (int)PrayerText.Delay, m_Strip.Prayer.RepetitionDelay.ToString());

            AddBackground(32, 220, 268, 30, 9350);
            AddButton(36, 224, 5537, 5539, (int)PrayerButton.PrevComponent, GumpButtonType.Reply, 0);
            AddButton(272, 224, 5540, 5542, (int)PrayerButton.NextComponent, GumpButtonType.Reply, 0);
            AddLabel(169 - (int)(GetComponentName.Length * 3.1), 226, 0, GetComponentName);
            #endregion

            #region Flavor Text
            AddBackground(32, 255, 268, 74, 9350);
            AddLabel(40, 260, 0, "Emote:");
            AddTextEntry(41, 280, 252, 43, 0, (int)PrayerText.Emote, m_Strip.Prayer.Emote);

            AddBackground(32, 338, 268, 74, 9350);
            AddLabel(40, 344, 0, "Speech:");
            AddTextEntry(40, 364, 252, 43, 0, (int)PrayerText.Speech, m_Strip.Prayer.Speech);

            AddBackground(32, 421, 268, 74, 9350);
            AddLabel(40, 426, 0, "Target Message:");
            AddTextEntry(40, 447, 252, 43, 0, (int)PrayerText.TargetMessage, m_Strip.Prayer.Message);

            AddBackground(32, 503, 268, 30, 9350);
            AddLabel(41, 509, 0, "Target Sound ID:");
            AddTextEntry(151, 508, 140, 20, 0, (int)PrayerText.SoundID, m_Strip.Prayer.SoundID.ToString());
            #endregion
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Strip == null || m_Strip.Deleted)
                return;
            if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                return;
            if (m_Strip.RootParentEntity != m_Viewer)
            {
                m_Viewer.SendMessage("That must be in your backpack for you to edit it.");
                return;
            }

            switch (info.ButtonID)
            {
                case (int)PrayerButton.Accept:
                    {
                        m_Strip.Prayer.SetName(info.GetTextEntry((int)PrayerText.Name).Text);
                        m_Strip.Name = m_Strip.Prayer.Name;

                        m_Strip.Prayer.Emote = info.GetTextEntry((int)PrayerText.Emote).Text;
                        m_Strip.Prayer.Speech = info.GetTextEntry((int)PrayerText.Speech).Text;
                        m_Strip.Prayer.Message = info.GetTextEntry((int)PrayerText.TargetMessage).Text;

                        int val = 0;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Intensity).Text, ref val))
                            m_Strip.Prayer.Intensity = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Seconds).Text, ref val))
                            m_Strip.Prayer.Duration = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Range).Text, ref val))
                            m_Strip.Prayer.Range = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Area).Text, ref val))
                            m_Strip.Prayer.Area = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Repetitions).Text, ref val))
                            m_Strip.Prayer.Repetitions = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.Delay).Text, ref val))
                            m_Strip.Prayer.RepetitionDelay = val;

                        if (ValidateInt(info.GetTextEntry((int)PrayerText.SoundID).Text, ref val))
                            m_Strip.Prayer.SoundID = val;

                        if(info.ButtonID != (int)PrayerButton.Accept)
                            SendNewGump();
                        return;
                    }
                case (int)PrayerButton.Clear:
                    {
                        return;
                    }
                case (int)PrayerButton.PrevEffect:
                    {
                        if (m_Strip.Prayer.Effect == PrayerEffect.Strength)
                            m_Strip.Prayer.Effect = PrayerEffect.DefendChance;
                        else
                        {
                            m_Strip.Prayer.Effect--;
                        }
                        goto case (int)PrayerButton.Accept;
                    }
                case (int)PrayerButton.NextEffect:
                    {
                        if (m_Strip.Prayer.Effect == PrayerEffect.DefendChance)
                            m_Strip.Prayer.Effect = PrayerEffect.Strength;
                        else
                        {
                            m_Strip.Prayer.Effect++;
                        }
                        goto case (int)PrayerButton.Accept;
                    }
                case (int)PrayerButton.PrevComponent:
                    {
                        int index = CustomFaithSpell.ValidComponents.IndexOf(m_Strip.Prayer.Component);

                        if (index == 0)
                            m_Strip.Prayer.Component = CustomFaithSpell.ValidComponents[CustomFaithSpell.ValidComponents.Count - 1];
                        else
                            m_Strip.Prayer.Component = CustomFaithSpell.ValidComponents[index - 1];
                        goto case (int)PrayerButton.Accept;
                    }
                case (int)PrayerButton.NextComponent:
                    {
                        int index = CustomFaithSpell.ValidComponents.IndexOf(m_Strip.Prayer.Component);

                        if (index == CustomFaithSpell.ValidComponents.Count - 1)
                            m_Strip.Prayer.Component = CustomFaithSpell.ValidComponents[0];
                        else
                            m_Strip.Prayer.Component = CustomFaithSpell.ValidComponents[index + 1];
                        goto case (int)PrayerButton.Accept;
                    }
                default: break;
            }

            base.OnResponse(sender, info);
        }

        private void SendNewGump()
        {
            m_Viewer.SendGump(new CustomPrayerStripGump(m_Viewer, m_Strip));
        }

        private bool ValidateInt(string st, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
                return false;

            return true;
        }
    }
}