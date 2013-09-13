using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class MadnessEffect : CustomPotionEffect
    {
        public override string Name { get { return "Madness"; } }

        public override void ApplyEffect(Mobile to, Mobile source, int intensity, Item itemSource)
        {
            HealthAttachment.TryTreatDisease(to, Disease.LoveDisease, intensity);
            if(Utility.RandomMinMax(1,100) < ( intensity * 2 ) )
            {
                MadnessTimer MercuryPoisoning = new MadnessTimer(to, TimeSpan.FromSeconds(intensity * Utility.RandomMinMax(15, 60)));
                to.SendMessage("You don't feel like yourself.");
                MercuryPoisoning.Start();

                if (to is PlayerMobile)
                {
                    HallucinationEffect.HallucinationTimer MercuryHallucinations = new HallucinationEffect.HallucinationTimer(to as PlayerMobile, intensity * Utility.RandomMinMax(15, 60));
                    MercuryHallucinations.Start();
                }
            }
        }

        public override bool CanDrink(Mobile mobile)
        {
            return true;
        }

        private class MadnessTimer : Timer
        {
            private Mobile m_Afflicted;
            private DateTime m_Expiration;
            private int m_Emote;

            public MadnessTimer(Mobile m, TimeSpan duration)
                : base(TimeSpan.FromMinutes(Utility.RandomMinMax(1, 3)), TimeSpan.FromSeconds(15))
            {
                m_Afflicted = m;
                m_Expiration = DateTime.Now + duration;
                Priority = TimerPriority.OneSecond;
                m_Emote = Utility.RandomMinMax(1, 8);
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

                switch (m_Emote)
                {
                    case 1: m_Afflicted.Emote("*gibbers madly*"); break;
                    case 2: m_Afflicted.Emote("*froths at the mouth*"); break;
                    case 3: m_Afflicted.Emote("*" + (m_Afflicted.Female ? "her" : "his") + " eyes whirl ceaselessly*"); break;
                    case 4: m_Afflicted.Emote("*twitches and mutters darkly*"); break;
                    case 5: m_Afflicted.Emote("*stops and cocks an ear, as if listening to a voice*"); break;
                    case 6: m_Afflicted.Emote("*nervously itches at " + (m_Afflicted.Female ? "her" : "his") + " limbs*"); break;
                    case 7: m_Afflicted.Emote("*growls and barks!*"); break;
                    case 8: m_Afflicted.Emote("*laughs loud and long, until " + (m_Afflicted.Female ? "her" : "his") + " voice breaks*"); break;
                    default: break;
                }

                switch (Utility.RandomBool())
                {
                    case true:
                        {
                            m_Afflicted.Emote("*staggers about*");
                            m_Afflicted.Move((Direction)Utility.RandomMinMax(0, 7));

                            IPooledEnumerable eable = m_Afflicted.Map.GetMobilesInRange(m_Afflicted.Location, 1);
                            foreach (Mobile m in eable)
                            {
                                if (m != m_Afflicted)
                                {
                                    if (Utility.RandomMinMax(1, 100) < Utility.RandomMinMax(1, 100))
                                    {
                                        if(!m_Afflicted.Warmode)
                                            m_Afflicted.Warmode = true;
                                        m_Afflicted.Combatant = m;
                                        CombatSystemAttachment.GetCSA(m_Afflicted).BeginAttack(AttackType.Swing);
                                        continue;
                                    }
                                }
                            }
                            eable.Free();
                            break;
                        }
                    case false:
                        {
                            m_Afflicted.Emote("*staggers about*");
                            m_Afflicted.Move((Direction)Utility.RandomMinMax(0, 7));
                            break;
                        }
                }



                base.OnTick();
            }

        }
    }
}