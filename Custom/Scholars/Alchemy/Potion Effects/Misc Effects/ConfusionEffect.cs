using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ConfusionEffect : CustomPotionEffect
    {
        public override string Name { get { return "Confusion"; } }

        public override void ApplyEffect(Mobile to, Mobile source, int intensity, Item itemSource)
        {
            HealthAttachment.TryTreatDisease(to, Disease.Bile, intensity);
            if (Utility.RandomMinMax(1, 100) < intensity)
            {
                ConfusionTimer CamphorConfusion = new ConfusionTimer(to, TimeSpan.FromSeconds(intensity * Utility.Random(5, 15)));
                CamphorConfusion.Start();
            }
        }

        public override bool CanDrink(Mobile mobile)
        {
            return true;
        }

        private class ConfusionTimer : Timer
        {
            private Mobile m_Afflicted;
            private DateTime m_Expiration;

            public ConfusionTimer(Mobile m, TimeSpan duration)
                : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(Utility.Random(3, 7)))
            {
                m_Afflicted = m;
                m_Expiration = DateTime.Now + duration;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Afflicted == null || m_Afflicted.Deleted || !m_Afflicted.Alive)
                {
                    Stop();
                    return;
                }

                if (m_Expiration < DateTime.Now)
                {
                    m_Afflicted.SendMessage("You have regained your senses.");
                    Stop();
                    return;
                }

                m_Afflicted.Warmode = false;
                CombatSystemAttachment.GetCSA(m_Afflicted).DisableAutoCombat();
                Direction stagger = (Direction)Utility.RandomMinMax(0, 7);
                m_Afflicted.Move(stagger);
                int[] blankKey = { -1 };
                m_Afflicted.DoSpeech("*staggers about in confusion*", blankKey, Network.MessageType.Emote, m_Afflicted.EmoteHue);

                base.OnTick();
            }
        }
    }
}