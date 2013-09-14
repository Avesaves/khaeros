using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Scripts.Commands;
using Server.Misc;
using Server.Items;
using Server.Commands;
using Server.Targeting;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAddiction : XmlAttachment
    {
        public static void Initialize()
        {
            CommandSystem.Register( "CheckAddiction", AccessLevel.GameMaster, new CommandEventHandler(CheckAddiction_OnCommand) );
        }

        [Usage("CheckAddiction")]
        [Description("Checks a player's addictions..")]
        public static void CheckAddiction_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new CheckAddictionTarget();
        }

        private class CheckAddictionTarget : Target
        {
            public CheckAddictionTarget() : base(100, true, TargetFlags.None)
            {

            }

            protected override void  OnTarget(Mobile from, object targeted)
            {
                if(from.AccessLevel < AccessLevel.GameMaster)
                    return;

                if(targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;
                    XmlAddiction addictions = XmlAttach.FindAttachment(pm, typeof(XmlAddiction)) as XmlAddiction;

                    if(addictions != null)
                    {
                        foreach(AddictionTimer timer in addictions.Addictions)
                        {
                            from.SendMessage(timer.Crave + ": " + timer.Hunger + "/" + timer.HungerMax + "; Recovery Count: " + timer.RecoveryCount);
                        }
                    }
                    else
                    {
                        from.SendMessage("Target has no XmlAddiction.");
                    }
                }
                else
                    from.SendMessage("That is not a player.");

 	             base.OnTarget(from, targeted);
            }
        }

        private List<AddictionTimer> m_Addictions = new List<AddictionTimer>();

        public List<AddictionTimer> Addictions { get { return m_Addictions; } set { m_Addictions = value; } }

        public XmlAddiction()
        {

        }

        public override void OnDelete()
        {
            foreach (AddictionTimer timer in m_Addictions)
            {
                if (timer.Running)
                    timer.Stop();
            }
 
            base.OnDelete();
        }

        public override void OnRemoved(object parent)
        {
            foreach (AddictionTimer timer in m_Addictions)
            {
                if (timer.Running)
                    timer.Stop();
            }

            base.OnRemoved(parent);
        }

        public static void Fix(Mobile from, string type)
        {
            if (from == null || from.Deleted)
                return;

            if (type == null)
                return;

            if (!(from is PlayerMobile))
                return;

            PlayerMobile pm = from as PlayerMobile;

            if (pm.Reforging)
                return;

            XmlAddiction AddAtt = XmlAttach.FindAttachment(pm, typeof(XmlAddiction)) as XmlAddiction;

            if (AddAtt == null)
            {
                AcquireAddiction(from, type);
            }
            else
            {
                bool addicted = false;
                foreach (AddictionTimer addiction in AddAtt.Addictions)
                {
                    if (addiction.Crave == type)
                    {
                        addiction.Hunger += Utility.RandomMinMax(1, addiction.HungerMax);
                        if (Utility.RandomMinMax(1, 100) > 50 - (addiction.HungerMax - addiction.Hunger))
                            addiction.HungerMax++;

                        if (addiction.Hunger < addiction.HungerMax / 2)
                        {
                            pm.SendMessage("You consume the " + type.ToLower() + " but still crave it.");
                        }
                        else if (addiction.Hunger >= addiction.HungerMax)
                        {
                            pm.SendMessage("You feel content.");
                            DrugBonus(pm, addiction.Crave, addiction.HungerMax, addiction.HungerMax * pm.Stam);
                        }
                        addicted = true;
                        continue;
                    }
                }
                if (!addicted)
                {
                    AcquireAddiction(from, type);
                }
            }
        }

        public static void AcquireAddiction(Mobile from, string type)
        {
            if (from == null || from.Deleted)
                return;

            if (type == null)
                return;

            if (!(from is PlayerMobile))
                return;

            if (Utility.RandomMinMax(1, 100) > Utility.RandomMinMax(from.StamMax, from.StamMax * 2))
            {
                XmlAddiction addAtt = XmlAttach.FindAttachment(from, typeof(XmlAddiction)) as XmlAddiction;
                if (addAtt == null)
                {
                    addAtt = new XmlAddiction();
                    XmlAttach.AttachTo(from, addAtt);
                }

                AddictionTimer addTimer = new AddictionTimer(from as PlayerMobile, type);
                addAtt.Addictions.Add(addTimer);
                addTimer.Start();
            }
        }

        public static void DrugBonus(Mobile from, string type, int degree, int duration)
        {
            if (from == null || from.Deleted)
                return;

            if (type == null)
                return;

            if (!(from is PlayerMobile))
                return;

            if (degree < 1)
                degree = 1;
            if (duration < 10)
                duration = 10;

            from.RemoveStatMod("Severe Withdrawal");

            if (type == "Tobacco")
            {
                from.RemoveStatMod("Tobacco Withdrawal");
                from.RemoveStatMod("Tobacco");

                degree = (int)(degree * (1 - (0.05 * ( degree / 10 ))));
                if(degree < 0)
                    degree = 0;

                XmlDex drugBonus = new XmlDex(degree, duration);
                drugBonus.Name = "Tobacco";

                XmlAttach.AttachTo(from, new XmlDex(degree, duration));
            }
            else if (type == "Swampweed")
            {
                from.RemoveStatMod("Swampweed Withdrawal");
                from.RemoveStatMod("Swampweed");

                degree = (int)(degree * (1 - (0.05 * (degree / 10))));
                if (degree < 0)
                    degree = 0;

                XmlDex drugBonus = new XmlDex(degree, duration);
                drugBonus.Name = "Swampweed";

                XmlAttach.AttachTo(from, new XmlInt(degree, duration));
            }
            else if (type == "Qat")
            {
                from.RemoveStatMod("Qat Withdrawal");
                from.RemoveStatMod("Qat");

                degree = (int)(degree * (1 - (0.05 * (degree / 10))));
                if (degree < 0)
                    degree = 0;

                XmlDex drugBonus = new XmlDex(degree, duration);
                drugBonus.Name = "Qat";

                XmlAttach.AttachTo(from, new XmlStr(degree, duration));
            }
            else if (type == "Banestone")
            {
                from.RemoveStatMod("Banestone Withdrawal");
                from.RemoveStatMod("Banestone");

                degree = (int)(degree * (1 - (0.05 * (degree / 10))));
                if (degree < 0)
                    degree = 0;

                XmlDex drugBonus = new XmlDex(degree, duration);
                drugBonus.Name = "Banestone";

                XmlAttach.AttachTo(from, new XmlMana(degree, duration));
            }
            else if (type == "Opium")
            {
                from.RemoveStatMod("Opium Withdrawal");
                from.RemoveStatMod("Opium");

                degree = (int)(degree * (1 - (0.05 * (degree / 10))));
                if (degree < 0)
                    degree = 0;

                XmlDex drugBonus = new XmlDex(degree, duration);
                drugBonus.Name = "Opium";

                XmlAttach.AttachTo(from, new XmlHits(degree, duration));
            }
        }

        public static void CleanUp(Mobile m)
        {
            if (m == null || m.Deleted)
                return;

            XmlAddiction addAtt = XmlAttach.FindAttachment(m, typeof(XmlAddiction)) as XmlAddiction;

            if (addAtt == null || addAtt.Deleted)
                return;

            m.RemoveStatMod("Tobacco Withdrawal");
            m.RemoveStatMod("Tobacco");
            m.RemoveStatMod("Swampweed Withdrawal");
            m.RemoveStatMod("Swampweed");
            m.RemoveStatMod("Qat Withdrawal");
            m.RemoveStatMod("Qat");
            m.RemoveStatMod("Banestone Withdrawal");
            m.RemoveStatMod("Banestone");
            m.RemoveStatMod("Opium Withdrawal");
            m.RemoveStatMod("Opium");
        }

        #region Serialization
        public XmlAddiction(ASerial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Addictions = new List<AddictionTimer>();
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                PlayerMobile addict = (PlayerMobile)reader.ReadMobile();
                string addictionType = reader.ReadString();
                AddictionTimer newTimer = new AddictionTimer(addict, addictionType);
                newTimer.Hunger = reader.ReadInt();
                newTimer.HungerMax = reader.ReadInt();
                newTimer.RecoveryCount = reader.ReadInt();
                Addictions.Add(newTimer);
                newTimer.Start();
            }
            if (m_Addictions.Count <= 0)
                Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((int)Addictions.Count);
            foreach (AddictionTimer timer in Addictions)
            {
                writer.Write((PlayerMobile)timer.Addict);
                writer.Write((string)timer.Crave);
                writer.Write((int)timer.Hunger);
                writer.Write((int)timer.HungerMax);
                writer.Write((int)timer.RecoveryCount);
            }
        }
        #endregion

    }

    public class AddictionTimer : Timer
    {
        private PlayerMobile m_Addict;
        public PlayerMobile Addict { get { return m_Addict; } set { m_Addict = value; } }

        private string m_Crave;
        public string Crave { get { return m_Crave; } set { m_Crave = value; } }

        private int m_Hunger;
        public int Hunger { get { return m_Hunger; } set { m_Hunger = value; if (m_Hunger > m_HungerMax) m_Hunger = m_HungerMax; } }

        private int m_HungerMax;
        public int HungerMax { get { return m_HungerMax; } set { m_HungerMax = value; } }

        private int m_RecoveryCount;
        public int RecoveryCount { get { return m_RecoveryCount; } set { m_RecoveryCount = value; } }

        public AddictionTimer(PlayerMobile m, string t)
            : base(TimeSpan.FromMinutes(Utility.RandomMinMax(1, 10)), TimeSpan.FromMinutes(m.StamMax))
        {
            m_Addict = m;
            m_Crave = t;
            m_Hunger = 10;
            m_RecoveryCount = 0;

            int h = m.StamMax / 10;
            if (10 - h < 1)
                h = 1;
            m_HungerMax = Utility.RandomMinMax(h, h + 10);

            Priority = TimerPriority.OneMinute;
        }
            

        protected override void OnTick()
        {
            if (m_Addict == null || m_Addict.Deleted)
            {
                Stop();
                return;
            }

            if (!m_Addict.Alive)
                return;

            if (m_Addict.Map == Map.Internal)
                return;

            if (m_Addict is PlayerMobile && (m_Addict as PlayerMobile).Reforging)
                return;

            if (m_RecoveryCount >= HungerMax)
            {
                m_Addict.SendMessage(2006, "You have successfully recovered from your addiction to " + m_Crave.ToLower() + ".");

                XmlAddiction addAtt = XmlAttach.FindAttachment(m_Addict, typeof(XmlAddiction)) as XmlAddiction;
                if (addAtt != null)
                {
                    if (addAtt.Addictions.Contains(this))
                        addAtt.Addictions.Remove(this);
                }

                Stop();
                return;
            }

            if (Hunger < HungerMax / 2)
            {
                int statLoss = HungerMax - m_Addict.ManaMax - m_Addict.StamMax;
                if (statLoss < 5)
                    statLoss = 5;

                if (Hunger <= 0)
                {
                    XmlHits withdrawal1 = new XmlHits(0 - statLoss, HungerMax * 10); withdrawal1.Name = "Severe Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal1);
                    XmlHue withdrawal2 = new XmlHue(2419, HungerMax * 10); withdrawal2.Name = "Severe Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal2);

                    m_Addict.SendMessage(37, "You grow weak and pale with your desire for " + m_Crave.ToLower() + ".");
                    m_RecoveryCount++;
                }

                if (m_Crave == "Tobacco")
                {
                    XmlDex withdrawal = new XmlDex(0 - statLoss, HungerMax * 1); withdrawal.Name = "Tobacco Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal);

                    m_Addict.SendMessage(37, "You've developed the shakes!");
                }
                else if (m_Crave == "Swampweed")
                {
                    XmlInt withdrawal = new XmlInt(0 - statLoss, HungerMax * 10); withdrawal.Name = "Swampweed Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal);

                    m_Addict.SendMessage(37, "You have a splitting migraine!");
                }
                else if (m_Crave == "Qat")
                {
                    XmlStam withdrawal = new XmlStam(0 - statLoss, HungerMax * 10); withdrawal.Name = "Qat Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal);

                    m_Addict.SendMessage(37, "You feel lethargic.");
                }
                else if (m_Crave == "Banestone")
                {
                    XmlMana withdrawal1 = new XmlMana(0 - statLoss, HungerMax * 10); withdrawal1.Name = "Banestone Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal1);
                    XmlInt withdrawal2 = new XmlInt(0 - statLoss, HungerMax * 10); withdrawal2.Name = "Banestone Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal2);

                    if (Utility.RandomBool())
                    {
                        new Unhorse.UnhorseTimer(m_Addict, 3);
                        m_Addict.Emote("*collapses, thrashing violently!*");
                    }
                    else
                    {
                        int damage = (HungerMax - Hunger);
                        if(damage < 1)
                            damage = 1;
                        if(m_Addict.HitsMax / 10 < 1)
                            damage = 1;
                        else
                            damage = damage / (m_Addict.HitsMax / 10);

                        m_Addict.PlaySound(0x133);
                        m_Addict.Damage(damage, m_Addict);
                        Blood blood = new Blood();
                        blood.ItemID = Utility.Random(0x122A, 5);
                        blood.MoveToWorld(m_Addict.Location, m_Addict.Map);
                        m_Addict.Emote("*blood flows from " + (m_Addict.Female ? "her" : "his") + " nose as " + (m_Addict.Female ? "her" : "his") +
                            " eyes roll to the back of " + (m_Addict.Female ? "her" : "his") + " head!*");
                    }

                    m_Addict.SendMessage(1609, "You are wracked with seizures!");
                }
                else if (m_Crave == "Opium")
                {
                    XmlDex withdrawal = new XmlDex(0 - statLoss, HungerMax * 10); withdrawal.Name = "Opium Withdrawal";
                    XmlAttach.AttachTo(m_Addict, withdrawal);
                    m_Addict.Hunger--;

                    m_Addict.SendMessage(37, "You feel a gnawing hunger.");
                }
            }

            Hunger--;
            if (Hunger < 0)
                Hunger = 0;

            base.OnTick();
        }
    }
}