using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Scripts.Commands;
using Server.Misc;
using Server.Gumps;
using Server.Commands;
using Server.Targets;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Engines.XmlSpawner2
{
    

    public enum Injury //The injuries that a PlayerMobile can acquire through being knocked the hell out, and some special Feats.
    {
        None = 0,
        Winded = 1,
        Bruised = 2,
        MinorCut = 3,
        MinorConcussion = 4,
        Bloodied = 5,
        Exhausted = 6,
        MajorConcussion = 7,
        FracturedLeftArm = 8,
        FracturedRightArm = 9,
        FracturedLeftLeg = 10,
        FracturedRightLeg = 11,
        FracturedRibs = 12,
        FracturedSkull = 13,
        DeepCut = 14,
        InternalBleeding = 15,
        LaceratedTorso = 16,
        BrokenLeftArm = 17,
        BrokenRightArm = 18,
        BrokenLeftLeg = 19,
        BrokenRightLeg = 20,
        BrokenJaw = 21,
        BrokenNeck = 22,
        BrokenBack = 23,
        BrokenSkull = 24,
        MassiveBleeding = 25
    }

    public enum Disease
    {
        None = 0,
        Influenza = 1,
        HundredDaysCough = 2, // Pertussis
        Diptheria = 3,
        Dysentery = 4,
        Consumption = 5, // Tuberculosis
        WesternFever = 6, // Malaria
        Bile = 7, // Cholera
        Leprosy = 8,
        LoveDisease = 9 // Syphilis
    }

    public class HealthAttachment : XmlAttachment
    {
        public static void Initialize()
        {
            CommandSystem.Register("GiveDisease", AccessLevel.GameMaster, new CommandEventHandler(GiveDisease_OnCommand));
            CommandSystem.Register("DiseaseInfo", AccessLevel.GameMaster, new CommandEventHandler(DiseaseInfo_OnCommand));
            CommandSystem.Register("ResetHealthAttachments", AccessLevel.Owner, new CommandEventHandler(ResetHealthAttachments_OnCommand));
            CommandSystem.Register("WipeHealthAttachments", AccessLevel.Owner, new CommandEventHandler(WipeHealthAttachments_OnCommand));
            CommandSystem.Register("GetDiseases", AccessLevel.Owner, new CommandEventHandler(GetDiseases_OnCommand));
            CommandSystem.Register("GiveInjury", AccessLevel.GameMaster, new CommandEventHandler(GiveInjury_OnCommand));
        }

        [Usage("GiveDisease")]
        [Description("Gives a disease to target player.")]
        public static void GiveDisease_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return;

            Disease arg = Disease.None;

            for(int i = 1; i < 10; i++)
            {
                if(e.Arguments[0].Trim().ToLower() == ((Disease)i).ToString().ToLower())
                {
                    arg = ((Disease)i);
                    continue;
                }
            }

            if(arg != Disease.None)
                e.Mobile.Target = new GiveDiseaseTarget(arg);
        }

        [Usage("DiseaseInfo")]
        [Description("Retrieves information on your diseases.")]
        public static void DiseaseInfo_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return;

            int i = 1;

            foreach (DiseaseTimer timer in HealthAttachment.GetHA(e.Mobile).CurrentDiseases)
            {
                e.Mobile.SendMessage("Disease #" + i.ToString() + ": " + timer.Disease.ToString() + ": Recovery Count: " + timer.RecoveryCount.ToString());
            }
        }

        [Usage("GetDiseases")]
        [Description("Retrieves information on target's diseases.")]
        public static void GetDiseases_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return;

            e.Mobile.Target = new GetDiseasesTarget();
        }

        public class GetDiseasesTarget : Target
        {
            public GetDiseasesTarget()
                : base(15, true, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if(targeted is Mobile)
                {
                    if (HealthAttachment.GetHA(targeted as Mobile).CurrentDiseases.Count < 1)
                        from.SendMessage("No diseases on target.");
                    else
                    {
                        foreach (DiseaseTimer dis in HealthAttachment.GetHA(targeted as Mobile).CurrentDiseases)
                        {
                            from.SendMessage(dis.Disease.ToString() + ": " + dis.RecoveryCount.ToString() + "/" + DiseaseTimer.RecoveryTarget(dis.Disease).ToString());
                        }
                    }
                }
                

                base.OnTarget(from, targeted);
            }
        }

        [Usage("ResetHealthAttachments")]
        [Description("Resets all existing health attachments, wiping injury and disease information clear.")]
        public static void ResetHealthAttachments_OnCommand(CommandEventArgs e)
        {
            int i = 0;
            foreach (Mobile m in World.Mobiles.Values)
            {
                HealthAttachment HA = HealthAttachment.GetHA(m);
                HA.Delete();
                XmlAttach.AttachTo(m, new HealthAttachment());
                i++;
                HA = null;
            }
            e.Mobile.SendMessage(i.ToString() + " attachments reset.");
        }

        [Usage("WipeHealthAttachments")]
        [Description("Wipes health attachments from most mob types.")]
        public static void WipeHealthAttachments_OnCommand(CommandEventArgs e)
        {
            int i = 0;
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (!(m is PlayerMobile) && !(m is BaseKhaerosMobile) && !(m is Soldier) && !(m is BaseCreature && (m as BaseCreature).Controlled) && !(m.GetType().IsSubclassOf(typeof(BaseKhaerosMobile))))
                {
                    HealthAttachment.GetHA(m).Delete();
                }
            }
 
        }

        [Usage("GiveInjury")]
        [Description("Gives an injury to a target player")]
        public static void GiveInjury_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return;

            Injury arg = Injury.None;

            for (int i = 1; i < 26; i++)
            {
                if (e.Arguments[0].Trim().ToLower() == ((Injury)i).ToString().ToLower())
                {
                    arg = ((Injury)i);
                    continue;
                }
            }

            if (arg != Injury.None)
                e.Mobile.Target = new GiveInjuryTarget(arg);
        }

        private class GiveDiseaseTarget : Target
        {
            private Disease m_Dis;

            public GiveDiseaseTarget(Disease d)
                : base(20, true, TargetFlags.None)
            {
                m_Dis = d;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile m = targeted as Mobile;
                    if (!HealthAttachment.GetHA(m).HasDisease(m_Dis))
                    {
                        DiseaseTimer timer = new DiseaseTimer(m, m_Dis);
                        timer.Start();
                        HealthAttachment.GetHA(m).CurrentDiseases.Add(timer);
                        from.SendMessage("You have successfully added " + m_Dis.ToString() + " to " + m.Name + "'s diseases.");
                    }
                }

                base.OnTarget(from, targeted);
            }
        }

        private class GiveInjuryTarget : Target
        {
            private Injury m_Inj;

            public GiveInjuryTarget(Injury i)
                : base(20, true, TargetFlags.None)
            {
                m_Inj = i;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile m = targeted as Mobile;
                    if (!HealthAttachment.GetHA(m).HasInjury(m_Inj))
                    {
                        InjuryTimer timer = new InjuryTimer(m_Inj, HealthAttachment.GetHA(m));
                        timer.Start();
                        HealthAttachment.GetHA(m).CurrentInjuries.Add(timer);
                        from.SendMessage("You have successfully added " + m_Inj.ToString() + " to " + m.Name + "'s injuries.");
                    }
                }

                base.OnTarget(from, targeted);
            }
        }

        #region Private Variables & Get/Sets
        private Mobile m_Player;
        private List<InjuryTimer> m_CurrentInjuries = new List<InjuryTimer>();
        private DateTime m_LastInjury;

        public Mobile Player { get { return m_Player; } }
        public List<InjuryTimer> CurrentInjuries { get { return m_CurrentInjuries; } set { m_CurrentInjuries = value; } }

        public int InjuryLevel //This only compiles an integer to represent the level of injury in CurrentInjuries.
        {
            get
            {
                int iLevel = 0;
                foreach (InjuryTimer iT in CurrentInjuries)
                {
                    iLevel += (int)(iT.Injury);
                }
                iLevel += CurrentInjuries.Count;

                if (iLevel > 100)
                    iLevel = 100;
                else if (iLevel < 0)
                    iLevel = 0;

                return iLevel;
            }
        }
        public int RecoveryTime //The average number of minutes needed to recover from an injury; used in InjuryTimer. ((17 - 58 HOURS))
        {
            get
            {

                int rTime = Utility.RandomMinMax( (60 + InjuryLevel) * 12 , (120 + InjuryLevel) * 24); //(17 - 58 HOURS))

                if (m_Player != null && !m_Player.Deleted && m_Player is PlayerMobile)
                {
                    PlayerMobile pm = m_Player as PlayerMobile;
                    //Negative factors that increase recovery time.
                    if (pm.GetBackgroundLevel(BackgroundList.Frail) > 0)
                        rTime += 5;
                    if (pm.GetBackgroundLevel(BackgroundList.OutOfShape) > 0)
                        rTime += 5;
                    if (pm.GetBackgroundLevel(BackgroundList.SlowHealer) > 0)
                        rTime += 10;
                    if (pm.GetBackgroundLevel(BackgroundList.Weak) > 0)
                        rTime += 5;
                    if (pm.GetBackgroundLevel(BackgroundList.Unlucky) > 0)
                        rTime += 10;
					if (pm.IsHardcore) //Added for HC merit, + 3 days worth of heal time for any injury
                        rTime *= 2;	
                   // rTime += ((int)(m_Player.StamMax / m_Player.Stam) - 1);

                    //Positve factors that decrease recovery time.
                    if (pm.GetBackgroundLevel(BackgroundList.Faithful) > 0)
                        rTime -= 5;
                    if (pm.GetBackgroundLevel(BackgroundList.Fit) > 0)
                        rTime -= 5;
                    if (pm.GetBackgroundLevel(BackgroundList.IronWilled) > 0)
                        rTime -= 5;
                    if (pm.GetBackgroundLevel(BackgroundList.Lucky) > 0)
                        rTime -= 10;
                    if (pm.GetBackgroundLevel(BackgroundList.QuickHealer) > 0)
                        rTime -= 10;
                    if (pm.GetBackgroundLevel(BackgroundList.Tough) > 0)
                        rTime -= 5;
                    if (pm.GetBackgroundLevel(BackgroundList.Strong) > 0)
                        rTime -= 5;
                    rTime -= (int)((m_Player.Stam + m_Player.ManaMax) / 10);

                    rTime -= (pm.Feats.GetFeatLevel(FeatList.FastHealing) * 5);

                    if (pm.IsVampire)
                        rTime /= 2;
                }

                if (rTime < 10)
                    rTime = Utility.RandomMinMax(rTime, 10);
                if (rTime < 1)
                    rTime = 1;
                //Console.WriteLine("rTime = " + rTime.ToString() + " minutes.");
                return rTime;
            }
        }
        #endregion

        [Attachable]
        public HealthAttachment() : base()
        {
            m_DiseaseImmunities = new Dictionary<Disease, int>();
            for (int i = 1; i <= 9; i++)
            {
                m_DiseaseImmunities.Add((Disease)i, 0);
            }

            m_LastCaught = new Dictionary<Disease, DateTime>();
            for (int i = 1; i <= 9; i++)
            {
                m_LastCaught.Add((Disease)i, DateTime.Now - TimeSpan.FromDays(30));
            }
        }

        public override void  OnAttach()
        {
            base.OnAttach();

            if (AttachedTo is Mobile)
                m_Player = AttachedTo as Mobile;
            else
                Delete();
        }

        #region Injury System

        #region The Injuring Process
        private int CalcChanceOfInjury(Mobile from)
        {
            int chance = Utility.Random(100);
            chance += InjuryLevel; //Ok, this means there is more of a chance to get injured if you're already injured.
            if(m_Player is PlayerMobile)
                chance -= ((m_Player as PlayerMobile).GetBackgroundLevel(BackgroundList.Lucky) * Utility.RandomMinMax(0,20));//- chance if they are LUCKY
            if(from is BaseCreature)
                chance += (from as BaseCreature).Deadly;  //+ chance if a creature is deadly
            if (from is PlayerMobile)
                if ((from as PlayerMobile).IsVampire)
                    chance /= 2; //Half the chance as a vampire
			if (from is PlayerMobile)
                if (((PlayerMobile)m_Player).LastDeath + TimeSpan.FromMinutes(5) > DateTime.Now)
                     chance += (Utility.RandomMinMax(0,25));// + 0 - 15% chance of injury if Hardcore

            return chance;
        }

        public void TryInjure(Mobile from, FeatList feat, int damage) // For feats that injure a player without knocking them out, and for deadly basecreatures.
        {
            if (from is BaseCreature)
            {

            }
            else if (from is PlayerMobile)
            {

            }
        }

        public void TryInjure(Mobile from) //Called on being knocked out in PlayerMobile's OnDeath method.
        {
            if (from is BaseCreature &&!(from is Soldier))
            {
                if ((from as BaseCreature).Deadly <= 0)
                    return;
                else if (Utility.RandomMinMax(1, 100) > (from as BaseCreature).Deadly)
                    return;
            }

            if (m_Player is PlayerMobile && (m_Player as PlayerMobile).Level < 20) // Characters below level 20 are still considered 'new', and needing time to learn the system.
                return;

            if (m_LastInjury + TimeSpan.FromMinutes(5) > DateTime.Now) //If the player was injured in the last five minutes; they cannot receive yet another injury.
                return;

            if (m_CurrentInjuries.Count == 25) //No more injuries can be acquired -- this guy has them all!
                return;

            bool injuryAcquired = false;
            while (!injuryAcquired)
            {
                int newInjury = Utility.RandomMinMax(1, Utility.RandomMinMax(InjuryLevel, InjuryLevel + ( m_Player is PlayerMobile ? (30 - ((m_Player as PlayerMobile).Level / 2)) : 5)));

                // Check to make sure the injury is between integers 1 and 25 -- as the Injury enumerator is between 1 and 25, and the integer must correspond to an injury.
                if (newInjury >= 1 && newInjury <= 25)
                    injuryAcquired = true;

                // Check to see if you already have the injury.
                if (injuryAcquired)
                {
                    foreach (InjuryTimer iT in CurrentInjuries)
                    {
                        if (iT.Injury == (Injury)newInjury)
                        {
                            // You have the injury; you can't get it again!
                            injuryAcquired = false;
                            continue;
                        }
                    }
                }

                // You have a valid injury; time to add it to your attachment!
                if (injuryAcquired)
                    AddInjury((Injury)newInjury);
            }
        }
		
		public void NormalWound(Mobile from) //Called on when an HC player uses .wound on a NON HC character
        {
			
			 if (m_Player is PlayerMobile && (m_Player as PlayerMobile).Level < 20) // Characters below level 20 are still considered 'new', and needing time to learn the system.
                return;
			
			if (m_LastInjury + TimeSpan.FromMinutes(5) > DateTime.Now) //If the player was injured in the last five minutes; they cannot receive yet another injury.
                return;

            if (m_CurrentInjuries.Count == 25) //No more injuries can be acquired -- this guy has them all!
                return;

            bool injuryAcquired = false;
			
            while (!injuryAcquired)
            {
                int newInjury = Utility.RandomList(1,2,2,4,5); //Lower tiered injuries inflicted on Non HC players.

                // Check to make sure the injury is between integers 1 and 25 -- as the Injury enumerator is between 1 and 25, and the integer must correspond to an injury.
                if (newInjury >= 1 && newInjury <= 25)
                    injuryAcquired = true;

                // Check to see if you already have the injury.
                if (injuryAcquired)
                {
                    foreach (InjuryTimer iT in CurrentInjuries)
                    {
                        if (iT.Injury == (Injury)newInjury)
                        {
                            // You have the injury; you can't get it again!
                            injuryAcquired = false;
                            continue;
                        }
                    }
                }

                // You have a valid injury; time to add it to your attachment!
                if (injuryAcquired)
                    AddInjury((Injury)newInjury);
            }
        }		
		
		
		public void HCWound(Mobile from) //Called on when an HC player greviously wounds another HC player
        {
		
			if (m_LastInjury + TimeSpan.FromMinutes(5) > DateTime.Now) //If the player was injured in the last five minutes; they cannot receive yet another injury.
                return;

            if (m_CurrentInjuries.Count == 25) //No more injuries can be acquired -- this guy has them all!
                return;

            bool injuryAcquired = false;
            while (!injuryAcquired)
            {
                int newInjury = Utility.RandomList(15,16,17,18,19,20,21,22,23,24,25);

                // Check to make sure the injury is between integers 1 and 25 -- as the Injury enumerator is between 1 and 25, and the integer must correspond to an injury.
                if (newInjury >= 1 && newInjury <= 25)
                    injuryAcquired = true;

                // Check to see if you already have the injury.
                if (injuryAcquired)
                {
                    foreach (InjuryTimer iT in CurrentInjuries)
                    {
                        if (iT.Injury == (Injury)newInjury)
                        {
                            // You have the injury; you can't get it again!
                            injuryAcquired = false;
                            continue;
                        }
                    }
                }

                // You have a valid injury; time to add it to your attachment!
                if (injuryAcquired)
                    AddInjury((Injury)newInjury);
            }
        }

        private void AddInjury(Injury inj)
        {
            InjuryTimer addInjury = new InjuryTimer(inj, this);
            m_CurrentInjuries.Add(addInjury);
            addInjury.Start();
            m_LastInjury = DateTime.Now;

            if (m_Player is PlayerMobile && ((PlayerMobile)m_Player).Feats.GetFeatLevel(FeatList.Justice) > 0)
            {
                if (((PlayerMobile)m_Player).LastDeath + TimeSpan.FromSeconds(90) > DateTime.Now)
                {
                    if (m_Player.LastKiller != null && !m_Player.LastKiller.Deleted && m_Player.LastKiller.Alive)
                    {
                        HealthAttachment.GetHA(m_Player.LastKiller).TryInjure(m_Player);
                    }
                }
            }

            switch (inj)
            {
                case Injury.Bloodied: m_Player.SendMessage(37, "You have been bloodied!"); break;
                case Injury.BrokenBack: m_Player.SendMessage(37, "Your back is broken!"); break;
                case Injury.BrokenJaw: m_Player.SendMessage(37, "Your jaw is broken!"); break;
                case Injury.BrokenLeftArm: m_Player.SendMessage(37, "Your left arm is broken!"); break;
                case Injury.BrokenLeftLeg: m_Player.SendMessage(37, "Your left leg is broken!"); break;
                case Injury.BrokenNeck: m_Player.SendMessage(37, "Your neck is broken!"); break;
                case Injury.BrokenRightArm: m_Player.SendMessage(37, "Your right arm is broken!"); break;
                case Injury.BrokenRightLeg: m_Player.SendMessage(37, "Your right leg is broken!"); break;
                case Injury.Bruised: m_Player.SendMessage(37, "You are bruised!"); break;
                case Injury.DeepCut: m_Player.SendMessage(37, "You have been deeply cut!"); break;
                case Injury.Exhausted: m_Player.SendMessage(37, "You are exhausted!"); break;
                case Injury.FracturedLeftArm: m_Player.SendMessage(37, "Your left arm is fractured!"); break;
                case Injury.FracturedLeftLeg: m_Player.SendMessage(37, "Your left leg is fractured!"); break;
                case Injury.FracturedRibs: m_Player.SendMessage(37, "Your ribs are fractured!"); break;
                case Injury.FracturedRightArm: m_Player.SendMessage(37, "Your right arm is fractured!"); break;
                case Injury.FracturedRightLeg: m_Player.SendMessage(37, "Your right leg is fractured!"); break;
                case Injury.FracturedSkull: m_Player.SendMessage(37, "Your skull is fractured!"); break;
                case Injury.InternalBleeding: m_Player.SendMessage(37, "You're bleeding internally!"); break;
                case Injury.LaceratedTorso: m_Player.SendMessage(37, "Your torso is lacerated!"); break;
                case Injury.MajorConcussion: m_Player.SendMessage(37, "You are majorly concussed!"); break;
                case Injury.MassiveBleeding: m_Player.SendMessage(37, "You are experiencing massive bleeding!"); break;
                case Injury.MinorConcussion: m_Player.SendMessage(37, "You have a minor concussion!"); break;
                case Injury.MinorCut: m_Player.SendMessage(37, "You have a few minor cuts!"); break;
                case Injury.BrokenSkull: m_Player.SendMessage(37, "Your skull is broken!"); break;
                case Injury.Winded: m_Player.SendMessage(37, "You are winded!"); break;
                default: break;
            }
        }
        #endregion

        public void DoInjury(Injury inj)
        {
            if (m_Player == null || m_Player.Deleted)
            {
                Delete();
                return;
            }

            if (m_Player is PlayerMobile && (m_Player as PlayerMobile).RessSick)
                return;

            bool hasInjury = false;
            foreach (InjuryTimer iT in CurrentInjuries)
            {
                if (iT.Injury == inj)
                {
                    hasInjury = true;
                    continue;
                }
            }
            if (!hasInjury)
                return;

            switch (inj)
            {
                case Injury.Bloodied:
                    {
                        int dmg = Utility.RandomMinMax(1, 5);
                        if (m_Player.Hits - dmg < 1)
                            dmg = m_Player.Hits - (m_Player.Hits - 1);
                        m_Player.Damage(dmg);
                        Blood newBlood = new Blood();
                        newBlood.MoveToWorld(m_Player.Location, m_Player.Map);
                        m_Player.SendMessage(37, "You're bleeding.");
                        break;
                    }
                case Injury.BrokenBack:
                    {
                        int pMin = 120 - m_Player.StamMax - m_Player.ManaMax;
                        if (pMin < 15)
                            pMin = 15;
                        XmlParalyze newParalysis = new XmlParalyze(pMin);
                        newParalysis.Name = "Injury Paralysis";
                        XmlAttach.AttachTo(m_Player, newParalysis);
                        m_Player.SendMessage(37, "You are wracked with paralytic back spasms and cannot move!");
                        m_Player.Emote("*spasms!*");
                        break;
                    }
                case Injury.BrokenJaw:
                    {
                        ArrayList muteAtt = XmlAttach.FindAttachments(m_Player, typeof(XmlBackground));
                        bool attached = false;
                        foreach (XmlBackground att in muteAtt)
                        {
                            if (att.Background == BackgroundList.Mute)
                                attached = true;
                        }
                        if (!attached)
                        {
                            int muteTime = 180 - m_Player.StamMax - m_Player.ManaMax;
                            if (muteTime < 20)
                                muteTime = 20;
                            XmlBackground newMute = new XmlBackground(BackgroundList.Mute, 1, muteTime);
                            newMute.Name = "Injury Mute";
                            XmlAttach.AttachTo(m_Player, newMute);
                            m_Player.SendMessage(37, "Your jaw is broken, causing you great pain and rendering you unable to speak.");
                        }
                        break;
                    }
                case Injury.BrokenLeftArm:
                    {
                        Item LeftArmItem = m_Player.FindItemOnLayer(Layer.TwoHanded);
                        if (LeftArmItem != null && !LeftArmItem.Deleted && m_Player.Alive)
                        {
                            LeftArmItem.MoveToWorld(m_Player.Location, m_Player.Map);
                            m_Player.PlaySound(LeftArmItem.GetDropSound());
                            if (LeftArmItem.Name != null && LeftArmItem.Name != "")
                                m_Player.Emote("*drops " + LeftArmItem.Name + " due to " + (m_Player.Female ? "her" : "his") + " wounds*");
                        }
                        //Remainder is handled in PlayerMobile.cs
                        break;
                    }
                case Injury.BrokenLeftLeg:
                    {
                        ArrayList lameAtts = XmlAttach.FindAttachments(m_Player, typeof(XmlBackground));
                        bool attached = false;
                        foreach (XmlBackground att in lameAtts)
                        {
                            if (att.Background == BackgroundList.Lame)
                                attached = true;
                        }
                        if (!attached)
                        {
                            int lameTime = 100 - m_Player.StamMax - m_Player.ManaMax;
                            if (lameTime < 5)
                                lameTime = 5;
                            XmlBackground lameAtt = new XmlBackground(BackgroundList.Lame, 1, lameTime);
                            lameAtt.Name = "Injury Left Lame";
                            XmlAttach.AttachTo(m_Player, lameAtt);
                            m_Player.SendMessage(37, "Your left leg is broken, hindering your movement.");
                        }
                        break;
                    }
                case Injury.BrokenNeck:
                    {
                        if (Utility.RandomMinMax(1, 100) > Utility.RandomMinMax(1 + (m_Player.ManaMax / 2), 100 + m_Player.ManaMax))
                        {
                            int pMin = 120 - m_Player.StamMax - m_Player.ManaMax;
                            if (pMin < 15)
                                pMin = 15;
                            XmlParalyze parAtt = new XmlParalyze(pMin);
                            parAtt.Name = "Injury Paralysis Neck";
                            XmlAttach.AttachTo(m_Player, parAtt);
                            m_Player.SendMessage(37, "Your neck is broken, and your body is paralyzed with pain!");
                            m_Player.Emote("*spasms!*");
                        }
                        break;
                    }
                case Injury.BrokenRightArm:
                    {
                        Item RightArmItem = m_Player.FindItemOnLayer(Layer.OneHanded);
                        if (RightArmItem == null || RightArmItem.Deleted)
                            RightArmItem = m_Player.FindItemOnLayer(Layer.FirstValid);
                        if ((RightArmItem == null || RightArmItem.Deleted) && m_Player.FindItemOnLayer(Layer.TwoHanded) is BaseWeapon)
                            RightArmItem = m_Player.FindItemOnLayer(Layer.TwoHanded);

                        if (RightArmItem != null && !RightArmItem.Deleted && m_Player.Alive)
                        {
                            RightArmItem.MoveToWorld(m_Player.Location, m_Player.Map);
                            m_Player.PlaySound(RightArmItem.GetDropSound());
                            if (RightArmItem.Name != null && RightArmItem.Name != "")
                                m_Player.Emote("*drops " + RightArmItem.Name + " due to " + (m_Player.Female ? "her" : "his") + " wounds*");
                        }
                        //Remainder is handled in PlayerMobile.cs
                        break;
                    }
                case Injury.BrokenRightLeg:
                    {
                        ArrayList lameAtts = XmlAttach.FindAttachments(m_Player, typeof(XmlBackground));
                        bool attached = false;
                        foreach (XmlBackground att in lameAtts)
                        {
                            if (att.Background == BackgroundList.Lame)
                                attached = true;
                        }
                        if (!attached)
                        {
                            int lameTime = 100 - m_Player.StamMax - m_Player.ManaMax;
                            if (lameTime < 5)
                                lameTime = 5;
                            XmlBackground lameAtt = new XmlBackground(BackgroundList.Lame, 1, lameTime);
                            lameAtt.Name = "Injury Right Lame";
                            XmlAttach.AttachTo(m_Player, lameAtt);
                            m_Player.SendMessage(37, "Your right leg is broken, hindering your movement.");
                        }
                        break;
                    }
                case Injury.Bruised:
                    {
                        bool alreadyBruised = false;
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.ToLower().Contains("bruise"))
                            {
                                alreadyBruised = true;
                                continue;
                            }
                        }
                        if (!alreadyBruised)
                        {
                            int bruiseTime = 180 - m_Player.StamMax - m_Player.ManaMax;
                            if (bruiseTime < 20)
                                bruiseTime = 20;
                            XmlHits bruising = new XmlHits(Utility.RandomMinMax(1, 10) * -1, bruiseTime);
                            bruising.Name = "Bruise";
                            XmlAttach.AttachTo(m_Player, bruising);
                            m_Player.SendMessage("You are badly bruised.");
                        }
                        break;
                    }
                case Injury.DeepCut:
                    {
                        int HPLoss = m_Player.HitsMax / Utility.RandomMinMax(10, 20);

                        if (m_Player.Hits - HPLoss <= 0)
                            break;
                        else if ( m_Player.Hits - HPLoss > 0 )
                        {
                            m_Player.Damage(HPLoss);
                            m_Player.SendMessage(37, "You have been cut deeply, and continue to bleed.");
                            Blood newBlood = new Blood();
                            newBlood.MoveToWorld(m_Player.Location, m_Player.Map);
                            m_Player.PlaySound(0x050);
                        }
                        break;
                    }
                case Injury.Exhausted:
                     {
                        m_Player.Mana -= Utility.RandomMinMax(1, 2);

                        int stamDmg = Utility.RandomMinMax(1, 2);
                        if (m_Player.Stam - stamDmg > 7)
                            m_Player.Stam -= stamDmg;
                            if(Utility.RandomMinMax(1,200) < 2)
                            m_Player.SendMessage(37, "You feel exhausted.");
                        break;
                    }
					
                case Injury.FracturedLeftArm:
                    {
                        //Handled in BaseWeapon and BaseShield.cs
                        break;
                    }
                case Injury.FracturedLeftLeg:
                    {
                        if (Utility.RandomBool() && m_Player.Stam - 1 > 0)
                            m_Player.Stam--;

                        //Handled in PlayerMobile.cs
                        break;
                    }
                case Injury.FracturedRibs:
                    {
                        int damage = Utility.RandomMinMax(1, 10);
                        if (m_Player.Hits - damage > 0)
                        {
                            m_Player.Damage(damage);
                            if(Utility.RandomMinMax(1,100) < 25)
                                m_Player.SendMessage(37, "Your ribs are fractured, making movement painful and even debilitating!");
                        }
                        break;
                    }
                case Injury.FracturedRightArm:
                    {
                        //Handled in BaseWeapon.cs
                        break;
                    }
                case Injury.FracturedRightLeg:
                    {
                        if (Utility.RandomBool() && m_Player.Stam - 1 > 0)
                            m_Player.Stam--;

                        //Handled in PlayerMobile.cs
                        break;
                    }
                case Injury.FracturedSkull:
                    {
                        m_Player.Mana = 0;
                        int intLoss = (-1 * (m_Player.RawInt - m_Player.ManaMax)) / 10;
                        if(intLoss > 0)
                            intLoss = 0;
                        XmlAttach.AttachTo(m_Player, new XmlInt(intLoss, InjuryLevel * 100));
                        m_Player.SendMessage(37, "Your skull is fractured, causing you to feel dizzy and confused!");
                        break;
                    }
                case Injury.InternalBleeding:
                    {
                        int HPLoss = m_Player.HitsMax / Utility.RandomMinMax(10, 20);
                        if (m_Player.Hits - HPLoss > 0)
                        {
                            m_Player.Damage(HPLoss);
                        }
                        m_Player.SendMessage(37, "You are bleeding internally!");

                        m_Player.PlaySound(m_Player.Female ? 813 : 1087);
                        m_Player.Say("*pukes*");
                        if (!m_Player.Mounted)
                            m_Player.Animate(32, 5, 1, true, false, 0);
                        Point3D p = new Point3D(m_Player.Location);
                        switch (m_Player.Direction)
                        {
                            case Direction.North:
                                p.Y--; break;
                            case Direction.South:
                                p.Y++; break;
                            case Direction.East:
                                p.X++; break;
                            case Direction.West:
                                p.X--; break;
                            case Direction.Right:
                                p.X++; p.Y--; break;
                            case Direction.Down:
                                p.X++; p.Y++; break;
                            case Direction.Left:
                                p.X--; p.Y++; break;
                            case Direction.Up:
                                p.X--; p.Y--; break;
                            default:
                                break;
                        }
                        p.Z = m_Player.Map.GetAverageZ(p.X, p.Y);

                        bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, m_Player.Map, 12, false);

                        if (canFit)
                        {
                            Puke puke = new Puke();
                            puke.Name = "blood";
                            puke.Hue = Utility.RandomList(1157, 1609, 2206, 2778, 2795);
                            puke.Map = m_Player.Map;
                            puke.Location = p;
                        }

                        if (CombatSystemAttachment.GetCSA(m_Player).CruiseControl)
                        {
                            CombatSystemAttachment.GetCSA(m_Player).DisableAutoCombat();
                            m_Player.SendMessage("It is difficult for you focus on combat right now.");
                        }

                        break;
                    }
                case Injury.LaceratedTorso:
                    {
                        int HPLoss = m_Player.HitsMax / Utility.RandomMinMax(10, 20);
                        if (m_Player.Hits - HPLoss > 0)
                        {
                            m_Player.Damage(HPLoss);
                            m_Player.SendMessage(37, "You are severely injured!");
                        }
                        break;
                    }
                case Injury.MajorConcussion:
                    {
                        m_Player.Mana = 0;
                        if (m_Player.Warmode)
                            m_Player.Warmode = false;

                        if (m_Player.Mount != null && !(m_Player.Mount as BaseCreature).Deleted && Utility.RandomMinMax(1, 100) > ( 50 - ( m_Player.HitsMax - m_Player.Hits)))
                        {
                            IMount mount = m_Player.Mount;
                            mount.Rider = null;

                            if (m_Player is PlayerMobile)
                            {
                                PlayerMobile pm = m_Player as PlayerMobile;

                                if (pm.DismountedTimer != null)
                                    pm.DismountedTimer.Stop();

                                pm.DismountedTimer = new Misc.Dismount.DismountTimer(pm, 1);
                                pm.DismountedTimer.Start();

                                Spells.SpellHelper.Damage(TimeSpan.FromTicks(1), pm, pm, Utility.RandomMinMax(1, 6));
                                pm.Emote("*falls from {0} mount*", pm.Female == true ? "her" : "his");
                            }
                            else
                            {
                                m_Player.Emote("*falls off " + (m_Player.Female == true ? "her" : "his") + " mount!*");
                                if (m_Player is Soldier)
                                    (mount as BaseCreature).Kill();
                            }
                        }

                        if (Utility.RandomMinMax(1, 100) > 95)
                        {
                            CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(m_Player);
                            csa.DoTrip(1);
                        }

/*                         if (Utility.RandomMinMax(1, 100) > 25)
                            XmlAttach.AttachTo(m_Player, new XmlBackground(BackgroundList.Deaf, 1, 5)); */

                        if(Utility.RandomMinMax(1,100) > 95)
                            m_Player.SendMessage(37, "You are severely concussed!");

                        break;
                    }
                case Injury.MassiveBleeding:
                    {
                        foreach (Item item in m_Player.Items)
                        {
                            if (Utility.RandomMinMax(1, 100) > 75)
                            {
                                XmlAttach.AttachTo(item, new XmlHue(1157, 30));
                                continue;
                            }
                        }

                        XmlAttach.AttachTo(m_Player, new XmlHue(2419, 30));

                        XmlAttach.AttachTo(m_Player, new XmlHits(-1 * Utility.RandomMinMax(1, InjuryLevel), InjuryLevel * 100));
                        Blood newBlood = new Blood();
                        newBlood.MoveToWorld(m_Player.Location, m_Player.Map);

                        m_Player.Mana = 0;

                        break;
                    }
                case Injury.MinorConcussion:
                    {
                        m_Player.Mana = 0;
                        switch(Utility.RandomMinMax(1,3))
                        {
                            case 1:
                                if (m_Player.Warmode)
                                {
                                    m_Player.Warmode = false;
                                    if(Utility.RandomMinMax(1,100) > 90)
                                        m_Player.SendMessage(37, "You are concussed!");
                                }
                                break;
                            case 2:
                                {
                                    if (m_Player.Mount != null && !(m_Player.Mount as BaseCreature).Deleted && Utility.RandomMinMax(1, 100) > ( 50 - ( m_Player.HitsMax - m_Player.Hits)))
                                    {
                                        IMount mount = m_Player.Mount;
                                        mount.Rider = null;

                                        if (m_Player is PlayerMobile)
                                        {
                                            PlayerMobile pm = m_Player as PlayerMobile;

                                            if (pm.DismountedTimer != null)
                                                pm.DismountedTimer.Stop();

                                            pm.DismountedTimer = new Misc.Dismount.DismountTimer(pm, 1);
                                            pm.DismountedTimer.Start();

                                            Spells.SpellHelper.Damage(TimeSpan.FromTicks(1), pm, pm, Utility.RandomMinMax(1, 6));
                                            pm.Emote("*falls from {0} mount*", pm.Female == true ? "her" : "his");
                                        }
                                        else
                                        {
                                            m_Player.Emote("*falls off " + (m_Player.Female == true ? "her" : "his") + " mount!*");
                                            if (m_Player is Soldier)
                                                (mount as BaseCreature).Kill();
                                        }
                                    }

                                    break;
                                }
/*                             case 3:
                                {
                                    if (Utility.RandomMinMax(1, 100) > 95)
                                    {
                                        CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(m_Player);
                                        csa.DoTrip(1);
                                        m_Player.SendMessage(37, "You are concussed!");
                                    }
                                    break;
                                } */
                        }
                        break;
                    }
                case Injury.MinorCut:
                    {
                        int dmg = Utility.RandomMinMax(1, 3);
                        if (m_Player.Hits - dmg > 0)
                        {
                            m_Player.Damage(dmg);
                            m_Player.SendMessage(37, "You have been cut, and are still bleeding.");
                        }
                        
                        break;
                    }
                case Injury.BrokenSkull:
                    {
                        m_Player.Mana = 0;
                        int intLoss = (-1 * (m_Player.RawInt - m_Player.ManaMax)) / 10;
                        if (intLoss > 0)
                            intLoss = 0;
                        XmlAttach.AttachTo(m_Player, new XmlInt(intLoss, InjuryLevel * 100));

                        new EyeRaking.EyeRakingTimer(m_Player, 10);
                        m_Player.SendMessage("You have been blinded!");

                        m_Player.SendMessage(37, "Your skull is broken, causing your perception to fade in and out!");

                        break;
                    }
                case Injury.Winded:
                    {
                        m_Player.Mana -= Utility.RandomMinMax(1, 10);

                        int stamDmg = Utility.RandomMinMax(1, 5);
                        if (m_Player.Stam - stamDmg > 0)
                            m_Player.Stam -= stamDmg;
                        else
                            m_Player.Stam = 1;

                        m_Player.SendMessage(37, "You're winded.");
                        break;
                    }
                default: break;
            }
        }

        public static void TryHealInjury(PlayerMobile from, Mobile targ, Injury inj, TryHealInjuryGump.HealInfo info)
        {
            if (from == null || from.Deleted || !from.Alive)
                return;
            if (targ == null || targ.Deleted || !targ.Alive || targ.IsDeadBondedPet)
                return;
            if (info == null)
                return;

            if (HasHealthAttachment(targ))
            {
                if (HealthAttachment.GetHA(targ).HasInjury(inj))
                {
                    if (TryHealInjuryGump.HealInjuryRequirements.MeetsRequirements(from, targ, info, inj))
                    {
                        from.SendMessage("You succcessfully heal " + targ.Name + "'s " + GetName(inj));
                        targ.SendMessage(from.Name + " has successfully healed your " + GetName(inj));
                        Misc.LevelSystem.AwardCraftXP(from, true);

                        InjuryTimer removeTimer = null;
                        foreach (InjuryTimer iT in HealthAttachment.GetHA(targ).CurrentInjuries)
                        {
                            if (iT.Injury == inj)
                            {
                                removeTimer = iT;
                                continue;
                            }
                        }
                        if (removeTimer != null && HealthAttachment.GetHA(targ).CurrentInjuries.Contains(removeTimer))
                        {
                            removeTimer.Stop();
                            HealthAttachment.GetHA(targ).CurrentInjuries.Remove(removeTimer);
                        }
                    }
                    else
                    {
                        from.SendMessage("You have failed to treat the wound properly!");
                        targ.SendMessage("You feel an intense pain!");
                        TryHealInjuryGump.FailToHeal(from, targ);
                        Misc.LevelSystem.AwardCraftXP(from, false);
                    }
                }
                else
                    from.SendMessage(targ.Name + " no longer has that injury.");
            }
            else
            {
                from.SendMessage("ERROR: Target has no Health Attachment.");
                return;
            }
        }

        public bool HasInjury(Injury inj)
        {
            bool hasInjury = false;
            foreach (InjuryTimer i in m_CurrentInjuries)
            {
                if (i.Injury == inj)
                {
                    hasInjury = true;
                    continue;
                }
            }
            return hasInjury;
        }

        public static string GetName(Injury inj)
        {
            switch (inj)
            {
                case Injury.Bloodied: return "Bloodied";
                case Injury.BrokenBack: return "Broken Back";
                case Injury.BrokenJaw: return "Broken Jaw";
                case Injury.BrokenLeftArm: return "Broken Left Arm";
                case Injury.BrokenLeftLeg: return "Broken Left Leg";
                case Injury.BrokenNeck: return "Broken Neck";
                case Injury.BrokenRightArm: return "Broken Right Arm";
                case Injury.BrokenRightLeg: return "Broken Right Leg";
                case Injury.Bruised: return "Bruising";
                case Injury.DeepCut: return "A Deep Cut";
                case Injury.Exhausted: return "Exhaustion";
                case Injury.FracturedLeftArm: return "Fractured Left Arm";
                case Injury.FracturedLeftLeg: return "Fractured Left Leg";
                case Injury.FracturedRibs: return "Fractured Ribs";
                case Injury.FracturedRightArm: return "Fractured Right Arm";
                case Injury.FracturedRightLeg: return "Fractured Right Leg";
                case Injury.FracturedSkull: return "A Skull Fracture";
                case Injury.InternalBleeding: return "Internal Bleeding";
                case Injury.LaceratedTorso: return "Torso Lacerations";
                case Injury.MajorConcussion: return "A Major Concussion";
                case Injury.MassiveBleeding: return "Massive Bleeding";
                case Injury.MinorConcussion: return "A Minor Concussion";
                case Injury.MinorCut: return "A Minor Cut";
                case Injury.BrokenSkull: return "Broken Skull";
                case Injury.Winded: return "Winded";
                default: return "An Injury";
            }
        }

        public static string GetStatusString(Mobile m)
        {
            if (HasHealthAttachment(m))
            {
                int i = GetHA(m).InjuryLevel;
                if (i <= 0)
                {
                    return "Unharmed";
                }
                else if (i >= 1 && i <= 20)
                {
                    return "Minor Injuries";
                }
                else if (i >= 21 && i <= 40)
                {
                    return "Wounded";
                }
                else if (i >= 41 && i <= 60)
                {
                    return "Severe Wounds";
                }
                else if (i >= 61 && i <= 80)
                {
                    return "Critical";
                }
                else if (i > 80)
                {
                    return "Near Death";
                }
                else
                    return "Not Available";
            }
            else
                return "Not Available";
        }

        public static int GetStatusHue(string status)
        {
            switch (status)
            {
                case "Unharmed": return 0;
                case "Minor Injuries": return 61;
                case "Wounded": return 247;
                case "Severe Wounds": return 522;
                case "Critical": return 437;
                case "Near Death": return 1156;
                default: return 0;
            }
        }

        public static bool PermanentDeathCheck(Mobile m)
        {
            HealthAttachment m_HA = HealthAttachment.GetHA(m);

            if (!(m is PlayerMobile))
                return false;

            if (m_HA == null || m_HA.Deleted)
                return false;

            if (m is PlayerMobile && (m as PlayerMobile).GetBackgroundLevel(BackgroundList.Unlucky) > 0)
            {
                if (m_HA.InjuryLevel < 75)
                    return false;
            }
            else if (m_HA.InjuryLevel < 90)
                return false;

            if (m is PlayerMobile && (m as PlayerMobile).GetBackgroundLevel(BackgroundList.Lucky) > 0 && Utility.RandomBool())
                return false;

            if (Utility.RandomMinMax(1, 100) < m.StamMax)
                return false;

            return true;
        }

        #endregion

        #region Disease System

        private List<DiseaseTimer> m_CurrentDiseases = new List<DiseaseTimer>();
        public List<DiseaseTimer> CurrentDiseases { get { return m_CurrentDiseases; } set { m_CurrentDiseases = value; } }

        private int m_Disfigurement = 0;
        private DateTime m_LastAppearanceRecovery = DateTime.Now;
        public int Disfigurement 
        { 
            get { return m_Disfigurement; } 
            set 
            { 
                m_Disfigurement = value;
                if (m_Disfigurement < 0)
                    m_Disfigurement = 0;
            } 
        }
        public DateTime LastAppearanceRecovery { get { return m_LastAppearanceRecovery; } set { m_LastAppearanceRecovery = value; } }

        private Dictionary<Disease, int> m_DiseaseImmunities = new Dictionary<Disease, int>();
        public Dictionary<Disease, int> DiseaseImmunities { get { return m_DiseaseImmunities; } set { m_DiseaseImmunities = value; } }

        private Dictionary<Disease, DateTime> m_LastCaught = new Dictionary<Disease, DateTime>();
        public bool CanCatch(Disease d)
        {
            switch (d)
            {
                case Disease.Influenza:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(6) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.HundredDaysCough:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(3) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.Diptheria:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(9) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.Dysentery:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(14) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.Consumption:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(28) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.WesternFever:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(20) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.Bile:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(28) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.Leprosy:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(30) < DateTime.Now)
                            return true;
                        return false;
                    }
                case Disease.LoveDisease:
                    {
                        if (m_LastCaught[d] + TimeSpan.FromDays(45) < DateTime.Now)
                            return true;
                        return false;
                    }
                default: return true;
            }
        }

        #region Disease Methods
        public int DiseaseResistance(Disease toResist)
        {
            if (m_Player == null || m_Player.Deleted)
                return 0;
            int baseResist = (m_Player.HitsMax + m_Player.StamMax + m_Player.ManaMax) / 3;
            int backgroundResist = 0;
            if (m_Player is PlayerMobile)
            {
                #region Background Resistance
                PlayerMobile pm = m_Player as PlayerMobile;

                if (pm.GetBackgroundLevel(BackgroundList.Frail) > 0)
                    backgroundResist -= 5;
                if (pm.GetBackgroundLevel(BackgroundList.OutOfShape) > 0)
                    backgroundResist -= 5;
                if (pm.GetBackgroundLevel(BackgroundList.SlowHealer) > 0)
                    backgroundResist -= 5;
                if (pm.GetBackgroundLevel(BackgroundList.Weak) > 0)
                    backgroundResist -= 5;
                if (pm.GetBackgroundLevel(BackgroundList.Unlucky) > 0)
                    backgroundResist -= 15;

                if (pm.GetBackgroundLevel(BackgroundList.Fit) > 0)
                    backgroundResist += 5;
                if (pm.GetBackgroundLevel(BackgroundList.IronWilled) > 0)
                    backgroundResist += 5;
                if (pm.GetBackgroundLevel(BackgroundList.Lucky) > 0)
                    backgroundResist += 15;
                if (pm.GetBackgroundLevel(BackgroundList.QuickHealer) > 0)
                    backgroundResist += 5;
                if (pm.GetBackgroundLevel(BackgroundList.Tough) > 0)
                    backgroundResist += 5;
                if (pm.GetBackgroundLevel(BackgroundList.Strong) > 0)
                    backgroundResist += 5;
                #endregion
            }
            double clothingResist = 0;
            foreach (Item i in m_Player.Items)
            {
                #region Clothing Resistance Bonuses
                if (i is RogueMask)
                    clothingResist += 0.2;
                else if (i is SurgicalMask)
                    clothingResist += 0.5;
                else if (i is LargeScarf)
                    clothingResist += 0.2;
                else if (i is Scarf)
                    clothingResist += 0.15;
                else if (i is SmallScarf)
                    clothingResist += 0.1;
                else if (i is Turban)
                    clothingResist += 0.33;
                else if (i is HatWithMask)
                    clothingResist += 0.25;
                #endregion
            }

            baseResist = baseResist + (int)(baseResist * clothingResist) + m_DiseaseImmunities[toResist];

            return (baseResist + backgroundResist -  ( InjuryLevel * (int)toResist) );
        }

        public static void TrySpreadDiease(Disease dis, object source)
        {
            /*
            if (dis == Disease.Dysentery || dis == Disease.Bile)
                return;

            if (source is Item)
            {
                Item infected = source as Item;

                if (infected == null || infected.Deleted || ( infected.RootParentEntity != null && infected.RootParentEntity is Mobile && ( !(infected.RootParentEntity as Mobile).Alive || (infected.RootParentEntity as Mobile).IsDeadBondedPet || (infected.RootParentEntity as Mobile).Map == Map.Internal) ) )
                    return;

                IPooledEnumerable eable = infected.Map.GetMobilesInRange(infected.Location, 10 - (int)dis);
                foreach (Mobile m in eable)
                {
                    if(m.CanSee(infected))
                        GetHA(m).TryCatchDisease(dis);
                }
                eable.Free();
            }
            else if (source is Mobile)
            {
                PlayerMobile player = source as PlayerMobile;
                if (player != null)
                    return;
                Mobile infected = source as Mobile;
                if (infected == null || infected.Deleted || !infected.Alive || infected.IsDeadBondedPet || infected.Map == Map.Internal)
                    return;

                if (infected == null || infected.Deleted || infected.IsDeadBondedPet || !infected.Alive)
                    return;

                IPooledEnumerable eable = infected.Map.GetMobilesInRange(infected.Location, 10 - (int)dis);
                foreach (Mobile m in eable)
                {
                    if(m != infected && m != null && !m.Deleted && m.Alive && !m.IsDeadBondedPet && m.CanSee(infected) && m.InLOS(infected))
                        GetHA(m).TryCatchDisease(dis);
                }
                eable.Free();

                foreach (Item i in infected.Items)
                {
                    TryContaminate(infected, i);
                }
                if (infected.Backpack != null && !infected.Backpack.Deleted)
                {
                    foreach (Item i in infected.Backpack.Items)
                    {
                        if(i != null && !i.Deleted)
                            TryContaminate(infected, i);
                    }
                }
            }*/
        }

        public void TryCatchDisease(Disease dis)
        {
            if (m_Player == null || m_Player.Deleted)
            {
                Delete();
                return;
            }

            if (m_Player is PlayerMadeStatue)
                return;

            if (m_Player.IsDeadBondedPet)
                return;

            if (m_Player.Blessed)
                return;

            if (m_Player.Frozen)
                return;

            if (!m_Player.Alive)
                return;

            if (m_Player is PlayerMobile && (m_Player as PlayerMobile).IsVampire)
                return;

            if (dis == Disease.None)
                return;

            if (HasDisease(dis))
                return;

            if (!CanCatch(dis))
                return;

            if (Utility.RandomMinMax(1, 100) < DiseaseResistance(dis))
                return;

            DiseaseTimer newTimer = new DiseaseTimer(m_Player, dis);
            newTimer.Start();
            m_CurrentDiseases.Add(newTimer);
            m_LastCaught[dis] = DateTime.Now;
        }

        public static void TryContaminate(Mobile m, Item i)
        {
            if (m == null || m.Deleted || !m.Alive || m.IsDeadBondedPet || m.Map == Map.Internal)
                return;
            if (i == null || i.Deleted || i.Map == Map.Internal)
                return;

            if (!(m.CanSee(i) || m.Items.Contains(i) || (m.Backpack != null && !m.Backpack.Deleted && m.Backpack.Items.Contains(i))))
                return;

            foreach (DiseaseTimer dis in HealthAttachment.GetHA(m).CurrentDiseases)
            {
                switch (dis.Disease)
                {
                    case Disease.Influenza: goto case Disease.Diptheria;
                    case Disease.HundredDaysCough: goto case Disease.Diptheria;
                    case Disease.Diptheria:
                        {
                            if (i is BaseClothing)
                            {
                                (i as BaseClothing).Disease = dis.Disease;
                                continue;
                            }
                            break;
                        }
                    case Disease.Dysentery: goto case Disease.Bile;
                    case Disease.Bile:
                        {
                            if (i is Food || i is CustomFood)
                            {
                                (i as Food).Disease = dis.Disease;
                                continue;
                            }
                            else if (i is Pitcher)
                            {
                                if (!(i as Pitcher).IsEmpty)
                                {
                                    (i as Pitcher).Disease = dis.Disease;
                                    continue;
                                }
                            }
                            break;
                        }
                    default: break;
                }
            }
        }

        public static void TryTreatDisease(Mobile m, Disease d, int intensity)
        {
            if (HealthAttachment.GetHA(m).HasDisease(d))
            {
                HealthAttachment.GetHA(m).GetDisease(d).RecoveryCount += Utility.RandomMinMax(0, intensity);
            }
        }

        public static void SpeedDiseaseRecovery(Mobile m)
        {
            foreach (DiseaseTimer dis in HealthAttachment.GetHA(m).CurrentDiseases)
            {
                switch (dis.Disease)
                {
                    case Disease.Dysentery: dis.RecoveryCount++; break;
                    case Disease.Influenza: dis.RecoveryCount++; break;
                    case Disease.Diptheria: dis.RecoveryCount++; break;
                    case Disease.WesternFever: dis.RecoveryCount++; break;
                    case Disease.HundredDaysCough: dis.RecoveryCount++; break;
                    default: break;
                }
            }
        }

        public bool DoDisease(Disease dis)
        {
            if (m_Player == null || m_Player.Deleted || !m_Player.Alive || (m_Player is PlayerMobile && (m_Player as PlayerMobile).RessSick))
                return false;

            if (m_Player.Map == Map.Internal)
                return false;

            if (!HasDisease(dis))
                return false;

            switch (dis)
            {
                case Disease.Influenza:
                    {
                        List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[Influenza]"))
                                removeMod.Add(mod);

                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if(m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);


                        int amount = (m_Player.StamMax / 2) * -1;
                        m_Player.Emote("*sweats profusely*");
                        XmlStam influenza = new XmlStam(amount, 300);
                        influenza.Name = " [Disease] [Influenza] ";
                        XmlAttach.AttachTo(m_Player, influenza);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.HundredDaysCough:
                    {
                        List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[HundredDaysCough]"))
                                removeMod.Add(mod);

                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if(m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        m_Player.Emote("*cough cough cough!*");
                        if (m_Player.Female)
                        {
                            if (Utility.RandomBool())
                                m_Player.PlaySound(0x311);
                            else
                                m_Player.PlaySound(0x312);
                        }
                        else
                        {
                            if (Utility.RandomBool())
                                m_Player.PlaySound(0x420);
                            else
                                m_Player.PlaySound(0x421);
                        }
                        XmlStam cough = new XmlStam(-5, 60);
                        XmlHits ache = new XmlHits(-5, 300);
                        cough.Name = " [Disease] [HundredDaysCough] [1] ";
                        ache.Name = " [Disease] [HundredDaysCough] [2] ";
                        XmlAttach.AttachTo(m_Player, cough);
                        XmlAttach.AttachTo(m_Player, ache);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.Diptheria:
                    {
                         List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[Diptheria]"))
                                removeMod.Add(mod);

                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if(m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        int amount = (m_Player.Dex / 3) * -1;
                        m_Player.Emote("*drools over " + (m_Player.Female ? "her" : "his") + " swollen neck*");
                        XmlDex diptheria = new XmlDex(amount, 600);
                        diptheria.Name = " [Disease] [Diptheria] ";
                        XmlAttach.AttachTo(m_Player, diptheria);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.Dysentery:
                    {
                         List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[Dysentery]"))
                                removeMod.Add(mod);

                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if(m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        m_Player.Emote("*doubles over in pain!*");
                        m_Player.Hunger -= 10;
                        if (m_Player.Hunger < 0)
                            m_Player.Hunger = 0;
                        m_Player.Thirst -= 10;
                        if (m_Player.Thirst < 0)
                            m_Player.Thirst = 0;
                        XmlHits dysentery = new XmlHits(-10, 3600);
                        dysentery.Name = " [Disease] [Dysentery] ";
                        XmlAttach.AttachTo(m_Player, dysentery);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.Consumption:
                    {
                         List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[Consumption]"))
                                removeMod.Add(mod);

                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if(m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        m_Player.Emote("*cough cough cough!*");
                        if (m_Player.Female)
                        {
                            if(Utility.RandomBool())
                                m_Player.PlaySound(0x311);
                            else
                                m_Player.PlaySound(0x312);
                        }
                        else
                        {
                            if (Utility.RandomBool())
                                m_Player.PlaySound(0x420);
                            else
                                m_Player.PlaySound(0x421);
                        }

                        int hitsamount = (m_Player.HitsMax / 4) * -1;
                        int stamamount = (m_Player.StamMax / 4) * -1;

                        XmlHits consumption1 = new XmlHits(hitsamount, 1200);
                        XmlStam consumption2 = new XmlStam(stamamount, 1200);
                        consumption1.Name = " [Disease] [Consumption] [1] ";
                        consumption2.Name = " [Disease] [Consumption] [2] ";
                        XmlAttach.AttachTo(m_Player, consumption1);
                        XmlAttach.AttachTo(m_Player, consumption2);

                        if (!m_Player.Mounted)
                            m_Player.Animate(32, 5, 1, true, false, 0);
                        Point3D p = new Point3D(m_Player.Location);
                        switch (m_Player.Direction)
                        {
                            case Direction.North:
                                p.Y--; break;
                            case Direction.South:
                                p.Y++; break;
                            case Direction.East:
                                p.X++; break;
                            case Direction.West:
                                p.X--; break;
                            case Direction.Right:
                                p.X++; p.Y--; break;
                            case Direction.Down:
                                p.X++; p.Y++; break;
                            case Direction.Left:
                                p.X--; p.Y++; break;
                            case Direction.Up:
                                p.X--; p.Y--; break;
                            default:
                                break;
                        }
                        p.Z = m_Player.Map.GetAverageZ(p.X, p.Y);

                        bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, m_Player.Map, 12, false);

                        if (canFit)
                        {
                            Puke puke = new Puke();
                            puke.Name = "blood";
                            puke.Hue = Utility.RandomList(1157, 1609, 2206, 2778, 2795);
                            puke.Map = m_Player.Map;
                            puke.Location = p;
                        }
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.WesternFever:
                    {
                        List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[WesternFever]"))
                                removeMod.Add(mod);
                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if (m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        m_Player.Emote("*sweats profusely!*");
                        if(m_Player is PlayerMobile)
                            HallucinationEffect.BeginHallucinating(m_Player as PlayerMobile, 120);
                        XmlHue yellowFever = new XmlHue(1052, 120);

                        int stamamount =(m_Player.StamMax / 3) * -1;
                        int stramount = (m_Player.Str / 4) * -1;
                        XmlStam WesternStam = new XmlStam(stamamount, 60);
                        XmlStr WesternStr = new XmlStr(stramount, 60);

                        yellowFever.Name = " [Disease] [WesternFever] [1] ";
                        WesternStam.Name = " [Disease] [WesternFever] [2] ";
                        WesternStr.Name = " [Disease] [WesternFever] [3] ";

                        XmlAttach.AttachTo(m_Player, yellowFever);
                        XmlAttach.AttachTo(m_Player, WesternStam);
                        XmlAttach.AttachTo(m_Player, WesternStr);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.Bile:
                    {
                        List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[Bile]"))
                                removeMod.Add(mod);
                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if (m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        m_Player.Emote("*doubles over in pain, losing control of " + (m_Player.Female ? "her" : "his") + " bodily functions*");
                        m_Player.Hunger -= 20;
                        if (m_Player.Hunger < 0)
                            m_Player.Hunger = 0;
                        m_Player.Thirst -= 20;
                        if (m_Player.Thirst < 0)
                            m_Player.Thirst = 0;
                        XmlHits dysentery = new XmlHits(-20, 3600);
                        dysentery.Name = " [Disease] [Bile] ";
                        XmlAttach.AttachTo(m_Player, dysentery);

                        if (!m_Player.Mounted)
                            m_Player.Animate(32, 5, 1, true, false, 0);
                        Point3D p = new Point3D(m_Player.Location);
                        switch (m_Player.Direction)
                        {
                            case Direction.North:
                                p.Y--; break;
                            case Direction.South:
                                p.Y++; break;
                            case Direction.East:
                                p.X++; break;
                            case Direction.West:
                                p.X--; break;
                            case Direction.Right:
                                p.X++; p.Y--; break;
                            case Direction.Down:
                                p.X++; p.Y++; break;
                            case Direction.Left:
                                p.X--; p.Y++; break;
                            case Direction.Up:
                                p.X--; p.Y--; break;
                            default:
                                break;
                        }
                        p.Z = m_Player.Map.GetAverageZ(p.X, p.Y);

                        bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, m_Player.Map, 12, false);

                        if (canFit)
                        {
                            Puke puke = new Puke();
                            puke.Name = "vomit";
                            puke.Hue = 2964;
                            puke.Map = m_Player.Map;
                            puke.Location = p;

                            if (m_Player.Female)
                                m_Player.PlaySound(0x32D);
                            else
                                m_Player.PlaySound(0x43F);
                        }
                        if (m_Player.Female)
                            m_Player.PlaySound(0x318);
                        else
                            m_Player.PlaySound(0x428);
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.Leprosy:
                    {
                        XmlAosAttribute leprosy = new XmlAosAttribute(AosAttribute.RegenHits, -25, 15);
                        leprosy.Name = " [Disease] [Leprosy] ";
                        XmlAttach.AttachTo(m_Player, leprosy);
                        Disfigurement++;
                        DiseaseMessage(dis);
                        return true;
                    }
                case Disease.LoveDisease:
                    {
                        List<StatMod> removeMod = new List<StatMod>();
                        foreach (StatMod mod in m_Player.StatMods)
                        {
                            if (mod.Name.Contains("[LoveDisease]"))
                                removeMod.Add(mod);
                        }

                        for (int i = 0; i < removeMod.Count; i++)
                            if (m_Player.StatMods.Contains(removeMod[i]))
                                m_Player.StatMods.Remove(removeMod[i]);

                        XmlAosAttribute Tirebladd = new XmlAosAttribute(AosAttribute.RegenHits, -50, 15);
                        Tirebladd.Name = " [Disease] [LoveDisease] ";
                        XmlAttach.AttachTo(m_Player, Tirebladd);

                        int amount = (m_Player.Int / 2) * -1;
                        XmlInt madness = new XmlInt(amount, 600);
                        madness.Name = " [Disease] [LoveDisease] ";
                        XmlAttach.AttachTo(m_Player, madness);

                        Disfigurement++;
                        DiseaseMessage(dis);
                        return true;
                    }
            }

            return false;
        }

        public void DiseaseMessage(Disease d)
        {
            switch (d)
            {
                case Disease.Influenza: m_Player.SendMessage("You feel warm..."); return;
                case Disease.HundredDaysCough: m_Player.SendMessage("Your throat aches..."); return;
                case Disease.Diptheria: m_Player.SendMessage("It's hard to breathe..."); return;
                case Disease.Dysentery: m_Player.SendMessage("Your stomache aches..."); return;
                case Disease.Consumption: m_Player.SendMessage("You feel fatigued..."); return;
                case Disease.WesternFever: m_Player.SendMessage("You feel hot..."); return;
                case Disease.Bile: m_Player.SendMessage("You feel sick to your stomache..."); return;
                case Disease.Leprosy: m_Player.SendMessage("You feel numb..."); return;
                case Disease.LoveDisease: m_Player.SendMessage("You feel dizzy..."); return;
                default: return;
            }
        }

        public bool HasDisease(Disease d)
        {
            foreach (DiseaseTimer timer in CurrentDiseases)
            {
                if (timer.Disease == d)
                    return true;
            }

            return false;
        }

        public DiseaseTimer GetDisease(Disease d)
        {
            foreach (DiseaseTimer dt in m_CurrentDiseases)
            {
                if (dt.Disease == d)
                    return dt;
            }
            return null;
        }

        public static string GetAppearance(PlayerMobile viewer, PlayerMobile viewed)
        {
            string appearance = "Appears average-looking";
            if (viewer != viewed && viewed.Disguise != null && viewed.Disguise.Looks != null)
                appearance = viewed.Disguise.Looks;
            else
            {
                #region Background Appearance Setting
                if (viewed.GetBackgroundLevel(BackgroundList.Attractive) > 0)
                    appearance = "Appears attractive";

                else if (viewed.GetBackgroundLevel(BackgroundList.GoodLooking) > 0)
                    appearance = "Appears good-looking";

                else if (viewed.GetBackgroundLevel(BackgroundList.Gorgeous) > 0)
                    appearance = "Appears gorgeous";

                else if (viewed.GetBackgroundLevel(BackgroundList.Homely) > 0)
                    appearance = "Appears homely";

                else if (viewed.GetBackgroundLevel(BackgroundList.Ugly) > 0)
                    appearance = "Appears ugly";

                else if (viewed.GetBackgroundLevel(BackgroundList.Hideous) > 0)
                    appearance = "Appears disfigured";
                #endregion
            }

            #region Disfigure from Disease
            if (GetHA(viewed).Disfigurement > 0)
            {
                if (!GetHA(viewed).HasDisease(Disease.LoveDisease) && !GetHA(viewed).HasDisease(Disease.Leprosy))
                {
                    while (GetHA(viewed).LastAppearanceRecovery + TimeSpan.FromHours(48) < DateTime.Now)
                    {
                        GetHA(viewed).LastAppearanceRecovery += TimeSpan.FromHours(48);
                        GetHA(viewed).Disfigurement--;
                    }
                }

                switch (appearance)
                {
                    case "Appears disfigured":
                        {
                            break;
                        }
                    case "Appears ugly":
                        {
                            if (GetHA(viewed).Disfigurement > 5)
                                appearance = "Appears disfigured";
                            break;
                        }
                    case "Appears homely":
                        {
                            if (GetHA(viewed).Disfigurement < 6)
                                appearance = "Appears ugly";
                            else
                                appearance = "Appears ugly";
                            break;
                        }
                    case "Appears average-looking":
                        {
                            if (GetHA(viewed).Disfigurement < 6)
                                appearance = "Appears homely";
                            else if (GetHA(viewed).Disfigurement < 12)
                                appearance = "Appears ugly";
                            else
                                appearance = "Appears disfigured";
                            break;
                        }
                    
                    case "Appears attractive":
                        {
                            if (GetHA(viewed).Disfigurement < 6)
                                appearance = "Appears average-looking";
                            else if (GetHA(viewed).Disfigurement < 12)
                                appearance = "Appears homely";
                            else if (GetHA(viewed).Disfigurement < 18)
                                appearance = "Appears ugly";
                            else
                                appearance = "Appears disfigured";
                            break;
                        }
                    case "Appears good-looking":
                        {
                            if (GetHA(viewed).Disfigurement < 6)
                                appearance = "Appears attractive";
                            else if (GetHA(viewed).Disfigurement < 12)
                                appearance = "Appears average-looking";
                            else if (GetHA(viewed).Disfigurement < 18)
                                appearance = "Appears homely";
                            else if (GetHA(viewed).Disfigurement < 24)
                                appearance = "Appears ugly";
                            else
                                appearance = "Appears disfigured";
                            break;
                        }
                    case "Appears gorgeous":
                        {
                            if (GetHA(viewed).Disfigurement < 6)
                                appearance = "Appears good-looking";
                            else if (GetHA(viewed).Disfigurement < 12)
                                appearance = "Appears attractive";
                            else if (GetHA(viewed).Disfigurement < 18)
                                appearance = "Appears average-looking";
                            else if (GetHA(viewed).Disfigurement < 24)
                                appearance = "Appears homely";
                            else if (GetHA(viewed).Disfigurement < 30)
                                appearance = "Appears ugly";
                            else
                                appearance = "Appears disfigured";
                            break;
                        }
                }
            }
            #endregion

            return appearance;
        }
        #endregion

        #endregion

        #region Utilities & Serialization

        public static HealthAttachment GetHA(Mobile m)
        {
            if (m == null)
                return null;

            HealthAttachment ha = XmlAttach.FindAttachment(m, typeof(HealthAttachment)) as HealthAttachment;
            if (ha == null)
            {
                ha = new HealthAttachment();
                XmlAttach.AttachTo(m, ha);
            }

            return ha;
        }

        public static bool HasHealthAttachment(Mobile m)
        {
            if (m == null)
                return false;

            if (GetHA(m) == null || GetHA(m).Deleted)
                return false;
            else
                return true;
        }

        public override void OnDelete()
        {
            foreach (InjuryTimer timer in m_CurrentInjuries)
                timer.Stop();
            m_CurrentInjuries.Clear();

            foreach (DiseaseTimer timer in m_CurrentDiseases)
                timer.Stop();
            m_CurrentDiseases.Clear();

            base.OnDelete();
        }

        public override void OnRemoved(object parent)
        {
            foreach (InjuryTimer timer in m_CurrentInjuries)
                timer.Stop();
            m_CurrentInjuries.Clear();

            foreach (DiseaseTimer timer in m_CurrentDiseases)
                timer.Stop();
            m_CurrentDiseases.Clear();

            base.OnRemoved(parent);
        }

        public HealthAttachment(ASerial serial)
            : base(serial)
        {

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        #region Version 2

                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Disease dis = (Disease)reader.ReadInt();
                            DateTime last = reader.ReadDateTime();
                            m_LastCaught.Add(dis, last);
                        }

                        int dcount = reader.ReadInt();
                        for (int i = 0; i < dcount; i++)
                        {
                            Disease dis = (Disease)reader.ReadInt();
                            int immu = reader.ReadInt();
                            m_DiseaseImmunities.Add(dis, immu);
                        }

                        #endregion
                        goto case 1;
                    }
                case 1:
                    {
                        #region Version 1
                        m_Disfigurement = reader.ReadInt();
                        m_LastAppearanceRecovery = reader.ReadDateTime();
                        int count = reader.ReadInt();
                        if (m_CurrentDiseases == null)
                            m_CurrentDiseases = new List<DiseaseTimer>();
                        for (int i = 0; i < count; i++)
                        {
                            Disease newDisease = (Disease)reader.ReadInt();
                            Mobile newInfected = reader.ReadMobile();
                            int newRecovCount = reader.ReadInt();
                            TimeSpan newIncuLeft = reader.ReadTimeSpan();

                            DiseaseTimer newTimer = new DiseaseTimer(newInfected, newDisease, newIncuLeft);
                            newTimer.RecoveryCount = newRecovCount;
                            newTimer.Start();
                            m_CurrentDiseases.Add(newTimer);
                        }
                        #endregion
                        goto case 0;
                    }
                case 0:
                    {
                        #region Version 0
                        m_Player = (Mobile)reader.ReadMobile();
                        m_LastInjury = reader.ReadDateTime();
                        int count = reader.ReadInt();
                        if(m_CurrentInjuries == null)
                            m_CurrentInjuries = new List<InjuryTimer>();
                        for (int i = 0; i < count; i++)
                        {
                            Injury timerInjury = (Injury)reader.ReadInt();
                            DateTime timerStartTime = reader.ReadDateTime();
                            InjuryTimer newInjuryTimer = new InjuryTimer(timerInjury, this);
                            newInjuryTimer.StartTime = timerStartTime;
                            m_CurrentInjuries.Add(newInjuryTimer);
                            newInjuryTimer.Start();
                        }
                        #endregion
                        break;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2);

            //version 2
            writer.Write((int)m_LastCaught.Count);
            foreach (KeyValuePair<Disease, DateTime> kvp in m_LastCaught)
            {
                writer.Write((int)kvp.Key);
                writer.Write((DateTime)kvp.Value);
            }

            writer.Write((int)m_DiseaseImmunities.Count);
            foreach (KeyValuePair<Disease, int> kvp in m_DiseaseImmunities)
            {
                writer.Write((int)kvp.Key);
                writer.Write((int)kvp.Value);
            }

            //version 1
            writer.Write((int)m_Disfigurement);
            writer.Write((DateTime)m_LastAppearanceRecovery);

            writer.Write((int)m_CurrentDiseases.Count);
            foreach (DiseaseTimer timer in m_CurrentDiseases)
            {
                writer.Write((int)timer.Disease);
                writer.Write((Mobile)timer.Infected);
                writer.Write((int)timer.RecoveryCount);
                writer.Write((TimeSpan)timer.IncubationLeft);
            }

            //version 0
            writer.Write((Mobile)m_Player);
            writer.Write((DateTime)m_LastInjury);
            writer.Write((int)m_CurrentInjuries.Count);
            foreach (InjuryTimer timer in m_CurrentInjuries)
            {
                writer.Write((int)timer.Injury);
                writer.Write((DateTime)timer.StartTime);
            }
        }
        #endregion
    }

    public class InjuryTimer : Timer
    {
        private HealthAttachment m_HA;
        private Injury m_Injury;
        private DateTime m_StartTime;
        private int m_RecoveryTime;

        public Injury Injury { get { return m_Injury; } set { m_Injury = value; } }
        public DateTime StartTime { get { return m_StartTime; } set { m_StartTime = value; } }

        public InjuryTimer(Injury i, HealthAttachment ha)
            : base(TimeSpan.FromSeconds(Utility.Random(300)), TimeSpan.FromMinutes(1))
        {
            m_Injury = i;
            m_HA = ha;
            m_RecoveryTime = m_HA.RecoveryTime;
            Priority = TimerPriority.OneMinute;
            m_StartTime = DateTime.Now;            
        }

        protected override void OnTick()
        {            
            if (m_HA == null || m_HA.Deleted)
            {
                Stop();
                return;
            }
            
            if (m_HA.Player == null || m_HA.Player.Deleted) //If the Player isn't properly instantiated, stop this; there's nobody to affect.
            {
                m_HA.Delete();
                Stop();
                return;
            }

            if (m_HA.InjuryLevel == 0) //If you have no injuries left, stop this timer.
            {
                Stop();
                return;
            }

            if (m_HA.Player is PlayerMobile && (m_HA.Player as PlayerMobile).RessSick)
                return;

            if (m_HA.Player is PlayerMobile && (!(m_HA.Player as PlayerMobile).Alive || (m_HA.Player as PlayerMobile).HasGump(typeof(DeathGump))))
                return;

            if(m_HA.Player.AccessLevel > AccessLevel.Counselor)
                return;

            if (Utility.RandomMinMax(1, 100) > (75 - Utility.Random(m_HA.InjuryLevel)))
            {
                m_HA.DoInjury(m_Injury);
            }
            
            if (m_StartTime + TimeSpan.FromMinutes(m_RecoveryTime) < DateTime.Now)
            {
                m_HA.Player.SendMessage("You have recovered from " + HealthAttachment.GetName(m_Injury) + ".");
                m_HA.CurrentInjuries.Remove(this);
                Stop();
                return;
            }
            else
            {
                int intervalRandomization = m_HA.InjuryLevel;
                if (intervalRandomization > 30)
                    intervalRandomization = 30;

                Interval = TimeSpan.FromSeconds(Utility.RandomMinMax(45, 120) - Utility.RandomMinMax((-1 * intervalRandomization), intervalRandomization));
            }

            base.OnTick();
        }
    }

    public class DiseaseTimer : Timer
    {
        private Disease m_Disease;
        private Mobile m_Infected;
        private int m_RecoveryCount = 0;
        private DateTime m_DateInfected;

        public Disease Disease { get { return m_Disease; } set { m_Disease = value; } }
        public Mobile Infected { get { return m_Infected; } set { m_Infected = value; } }
        public int RecoveryCount { get { return m_RecoveryCount; } set { m_RecoveryCount = value; } }
        public TimeSpan IncubationLeft { get { return (m_DateInfected + IncubationPeriod(m_Disease) - DateTime.Now); } }

        public static int RecoveryTarget(Disease d)
        {
            switch (d)
                {
                    case Disease.Influenza: return 20;
                    case Disease.HundredDaysCough: return 500;
                    case Disease.Diptheria: return 20;
                    case Disease.Dysentery: return 40;
                    case Disease.Consumption: return 150;
                    case Disease.WesternFever: return 30;
                    case Disease.Bile: return 50;
                    case Disease.Leprosy: return 250;
                    case Disease.LoveDisease: return 350;
                    default: return 10;
                }
        }
        public bool HealsOverTime
        {
            get
            {
                switch (m_Disease)
                {
                    case Disease.Influenza: return true;
                    case Disease.HundredDaysCough: return true;
                    case Disease.Diptheria: return true;
                    case Disease.Dysentery: return true;
                    case Disease.Consumption: return false;
                    case Disease.WesternFever: return true;
                    case Disease.Bile: return true;
                    case Disease.Leprosy: return false;
                    case Disease.LoveDisease: return false;
                    default: return true;
                }
            }
        }
        public static TimeSpan IncubationPeriod(Disease d)
        {
            switch (d)
            {
                case Disease.Influenza: return TimeSpan.FromDays(1);
                case Disease.HundredDaysCough: return TimeSpan.FromDays(1);
                case Disease.Diptheria: return TimeSpan.FromDays(2);
                case Disease.Dysentery: return TimeSpan.FromHours(6);
                case Disease.Consumption: return TimeSpan.FromDays(Utility.RandomMinMax(15, 30));
                case Disease.WesternFever: return TimeSpan.FromHours(1);
                case Disease.Bile: return TimeSpan.FromHours(Utility.RandomMinMax(6, 12));
                case Disease.Leprosy: return TimeSpan.FromDays(Utility.RandomMinMax(15, 30));
                case Disease.LoveDisease: return TimeSpan.FromDays(Utility.RandomMinMax(30, 60));
                default: return TimeSpan.FromDays(1);
            }

        }
        public string Diagnosis
        {
            get
            {
                string diag = "";

                switch (m_Disease)
                {
                    case Disease.Influenza:
                        {
                            diag = "Subject suffers from high temperature; cold sweats; congestion of the sinuses; occasional vomiting; and minor delirium.";
                            break;
                        }
                    case Disease.HundredDaysCough:
                        {
                            diag = "Subject suffers from violent, prolonged coughing fits and has difficulty breathing.";
                            break;
                        }
                    case Disease.Diptheria:
                        {
                            diag = "Subject suffers from chronic coughing fits, sore throat, swollen neck and face.";
                            break;
                        }
                    case Disease.Dysentery:
                        {
                            diag = "Subject suffers from abdominal cramps, vomiting, dehydration.";
                            break;
                        }
                    case Disease.Consumption:
                        {
                            diag = "Subject suffers from violent, prolonged coughing fits; has difficulty breathing; blood and other matter may be found in expelled fluids.";
                            break;
                        }
                    case Disease.WesternFever:
                        {
                            diag = "Subject suffers from yellow, discolored skin and eyes; intense sweating; delirium; weakness; pain in joints.";
                            break;
                        }
                    case Disease.Bile:
                        {
                            diag = "Subject suffers from vomiting, diarrhea, dehydration, and malnutrition.";
                            break;
                        }
                    case Disease.Leprosy:
                        {
                            diag = "Subject suffers from various growths and infections apparent throughout the subject's skin; subject appears to feel little pain.";
                            break;
                        }
                    case Disease.LoveDisease:
                        {
                            diag = "Subject suffers from painless lesions on hands, face, and sensitive regions of the body; subject appears weakened.";
                            break;
                        }
                    default: diag = "Subject displays no symptoms."; break;
                }
                return diag;
            }
        }
        public string Name
        {
            get
            {
                string name = "Unknown";

                switch (m_Disease)
                {
                    case Disease.Influenza:
                        {
                            name = "Influenza";
                            break;
                        }
                    case Disease.HundredDaysCough:
                        {
                            name = "The 100 Days' Cough";
                            break;
                        }
                    case Disease.Diptheria:
                        {
                            name = "Diptheria";
                            break;
                        }
                    case Disease.Dysentery:
                        {
                            name = "Dysentery";
                            break;
                        }
                    case Disease.Consumption:
                        {
                            name = "Consumption";
                            break;
                        }
                    case Disease.WesternFever:
                        {
                            name = "Western Fever";
                            break;
                        }
                    case Disease.Bile:
                        {
                            name = "Bile";
                            break;
                        }
                    case Disease.Leprosy:
                        {
                            name = "Leprosy";
                            break;
                        }
                    case Disease.LoveDisease:
                        {
                            name = "Love Disease";
                            break;
                        }
                    default: break;
                }

                return name;
            }
        }
        public static TimeSpan Virulance(Disease d)
        {
            switch (d)
            {
                case Disease.Influenza: return TimeSpan.FromMinutes(Utility.RandomMinMax(15, 30));
                case Disease.HundredDaysCough: return TimeSpan.FromMinutes(Utility.RandomMinMax(5, 10));
                case Disease.Diptheria: return TimeSpan.FromHours(Utility.RandomMinMax(3, 6));
                case Disease.Dysentery: return TimeSpan.FromHours(Utility.RandomMinMax(1, 2));
                case Disease.Consumption: return TimeSpan.FromMinutes(Utility.RandomMinMax(10, 20));
                case Disease.WesternFever: return TimeSpan.FromMinutes(Utility.RandomMinMax(3, 5));
                case Disease.Bile: return TimeSpan.FromHours(Utility.RandomMinMax(3, 6));
                case Disease.Leprosy: return TimeSpan.FromDays(Utility.RandomMinMax(1, 3));
                case Disease.LoveDisease: return TimeSpan.FromDays(Utility.RandomMinMax(6, 12));
                default: return TimeSpan.FromDays(1);
            }

        }

        public DiseaseTimer(Mobile m, Disease d)
            : this(m, d, DiseaseTimer.IncubationPeriod(d))
        {
            
        }

        public DiseaseTimer(Mobile m, Disease d, TimeSpan incubation)
            : base(incubation, DiseaseTimer.Virulance(d))
        {
            m_Infected = m;
            m_Disease = d;
            Priority = TimerPriority.OneMinute;
            m_DateInfected = DateTime.Now;
        }

        protected override void OnTick()
        {
            if (m_Infected == null || m_Infected.Deleted)
            {
                Stop();
                return;
            }

            if (m_Infected.Map == Map.Internal)
                return;

            HealthAttachment HA = HealthAttachment.GetHA(m_Infected);
            if (!HA.CurrentDiseases.Contains(this))
                HA.CurrentDiseases.Add(this);

            if (m_RecoveryCount > RecoveryTarget(Disease))
            {
                if (HA.CurrentDiseases.Contains(this))
                    HA.CurrentDiseases.Remove(this);
                HA.Player.SendMessage("You feel healthier.");
                HA.DiseaseImmunities[m_Disease] += Utility.RandomMinMax(0, 10);
                Stop();
                return;
            }
            else if (HA.DoDisease(Disease))
            {
                HealthAttachment.TrySpreadDiease(Disease, HA.Player);
                if(HealsOverTime)
                    m_RecoveryCount++;
            }

            base.OnTick();
        }
    }

    public class DiseaseSource : Item
    {
        private Disease m_Disease;
        private int m_Range;
        
        [Constructable]
        public DiseaseSource(Disease d) : this(d, 5)
        {

        }

        [Constructable]
        public DiseaseSource(Disease d, int range)
        {
            m_Disease = d;
            m_Range = range;
            Visible = false;
            Movable = false;
            CanBeGrabbed = false;
            Stackable = false;
        }

        public override void OnAfterSpawn()
        {
            if(m_Disease == Disease.None)
                Delete();

            if (Spawner is XmlSpawner)
            {
                IPooledEnumerable eable = Map.GetMobilesInRange((Spawner as XmlSpawner).Location, m_Range);
                foreach (Mobile m in eable)
                {
                    HealthAttachment.GetHA(m).TryCatchDisease(m_Disease);
                }
                eable.Free();
            }

            if(this != null && !this.Deleted)
                Delete();

            base.OnAfterSpawn();
        }

        public DiseaseSource(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Delete();
        }
    }

    public class HealthGump : Gump
    {
        private enum InjuryButton
        {
            Exit,
            ScrollUp,
            ScrollDown,
            DiseasesAndAilments,
            Injury
        }

        private PlayerMobile m_Viewer;
        private Mobile m_Viewed;
        private int m_Current;
        private const int m_MaxEntries = 12;

        public HealthGump(PlayerMobile viewer, Mobile viewed)
            : this(viewer, viewed, 0)
        {
        }

        public HealthGump(PlayerMobile viewer, Mobile viewed, int current)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Viewed = viewed;
            m_Current = current;
            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(HealthGump));
            m_Viewer.CloseGump(typeof(TryHealInjuryGump));
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            AddBackground(160, 131, 450, 347, 9270); // Main background pane.
            AddButton(175, 148, 1151, 1152, (int)InjuryButton.Exit, GumpButtonType.Reply, 0); // Exit button
            
            #region Paper Doll View
            AddBackground(411, 147, 184, 313, 2620); // Black background for paper doll anatomy.
            AddBackground(416, 156, 172, 77, 5120); // Stone background for name, health, status.
            AddLabel(425, 160, 247, m_Viewed.Name.ToString());
            AddLabel(425, 180, 247, "Health: " + m_Viewed.Hits.ToString() + "/" + m_Viewed.HitsMax.ToString());
            AddLabel(425, 200, 247, "Status:");

            if (m_Viewed.BodyValue == 400 || m_Viewed.BodyValue == 401)
            {
                AddLabel(478, 200, HealthAttachment.GetStatusHue(HealthAttachment.GetStatusString(m_Viewed)), HealthAttachment.GetStatusString(m_Viewed));
                AddImage(400, 175, m_Viewed.Female ? 1888 : 1889, m_Viewed.Hue);
            }
            #endregion

            #region Injury List

            AddBackground(247, 146, 93, 26, 5120);
            AddLabel(271, 148, 247, "Injuries");
            AddBackground(175, 175, 228, 252, 3000);

            int b_X = 185;
            int b_Y = 185;
            int t_X = 205;
            int t_Y = 182;
            if (HealthAttachment.HasHealthAttachment(m_Viewed))
            {
                if (HealthAttachment.GetHA(m_Viewed).CurrentInjuries.Count > 0)
                {
                    List<Injury> injList = new List<Injury>();
                    foreach (InjuryTimer i in HealthAttachment.GetHA(m_Viewed).CurrentInjuries)
                    {
                        injList.Add(i.Injury);
                    }

                    int m_Start = m_Current * m_MaxEntries;
                    for (int i = m_Start; i < (m_Start) + m_MaxEntries && i < injList.Count; i++)
                    {
                        if ((m_Viewed.BodyValue == 400 || m_Viewed.BodyValue == 401) && m_Viewer.Feats.GetFeatLevel(FeatList.Medicine) > 0)
                        {
                            AddButton(b_X, b_Y, 10740, 10742, (int)InjuryButton.Injury + (int)injList[i], GumpButtonType.Reply, 0); b_Y += 20;
                        }
                        else if ((m_Viewed.BodyValue == 400 || m_Viewed.BodyValue == 401) && m_Viewer.Feats.GetFeatLevel(FeatList.Veterinary) > 0)
                        {
                            AddButton(b_X, b_Y, 10740, 10742, (int)InjuryButton.Injury + (int)injList[i], GumpButtonType.Reply, 0); b_Y += 20;
                        }
                        
                        AddLabel(t_X, t_Y, 0, HealthAttachment.GetName(injList[i])); t_Y += 20;
                    }
                }
                else
                {
                    AddLabel(t_X, t_Y, 0, "Uninjured");
                }
            }
            else
            {
                AddLabel(205, 182, 0, "Uninjured");
            }

            #endregion

            #region Scrollbar

            AddImageTiled(386, 181, 11, 237, 2712);
            AddButton(384, 177, 250, 251, (int)InjuryButton.ScrollUp, GumpButtonType.Reply, 0);
            AddButton(384, 403, 252, 253, (int)InjuryButton.ScrollDown, GumpButtonType.Reply, 0);

            #endregion

            AddButton(209, 436, 2440, 2440, (int)InjuryButton.DiseasesAndAilments, GumpButtonType.Reply, 0);
            AddLabel(227, 438, 247, "Diseases and Ailments");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                return;
            if (m_Viewed == null || m_Viewed.Deleted || !m_Viewed.Alive || m_Viewed.IsDeadBondedPet)
                return;

            if(info.ButtonID >= (int)InjuryButton.Injury)
            {
                if (m_Viewer.InRange(m_Viewed.Location, 1))
                {
                    if (m_Viewed is PlayerMobile)
                    {
                        m_Viewed.SendGump(new MedicalTreatmentConfirmGump(m_Viewer, m_Viewed as PlayerMobile, (Injury)(info.ButtonID - (int)InjuryButton.Injury)));
                        return;
                    }
                    else
                    {
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Viewed, (Injury)(info.ButtonID - (int)InjuryButton.Injury)));
                        return;
                    }
                }
                else
                {
                    m_Viewer.SendMessage("You are too far away.");
                    return;
                }
            }
            else
            {
                switch (info.ButtonID)
                {
                    case (int)InjuryButton.Exit:
                        {
                            return;
                        }
                    case (int)InjuryButton.ScrollUp:
                        {
                            if (m_Current > 0)
                            {
                                m_Current--;
                            }
                            m_Viewer.SendGump(new HealthGump(m_Viewer, m_Viewed, m_Current));
                            return;
                        }
                    case (int)InjuryButton.ScrollDown:
                        {
                            if (HealthAttachment.HasHealthAttachment(m_Viewed) && m_Current * m_MaxEntries < HealthAttachment.GetHA(m_Viewed).CurrentInjuries.Count)
                            {
                                m_Current++;
                            }
                            m_Viewer.SendGump(new HealthGump(m_Viewer, m_Viewed, m_Current));
                            return;
                        }
                    case (int)InjuryButton.DiseasesAndAilments:
                        {
                            if (m_Viewer.Feats.GetFeatLevel(FeatList.Pathology) > 0)
                            {
                                if (HealthAttachment.GetHA(m_Viewed).CurrentDiseases.Count <= 0)
                                {
                                    m_Viewer.SendMessage(m_Viewed.Name + " does not seem to have any ailments.");
                                    m_Viewer.SendGump(new HealthGump(m_Viewer, m_Viewed, m_Current));
                                    return;
                                }
                                else
                                {
                                    m_Viewer.SendMessage("You examine their appearance and behavior...");
                                    m_Viewer.SendGump(new DiseaseGump(m_Viewer, m_Viewed, 0));
                                    return;
                                }
                            }
                            else
                            {
                                m_Viewer.SendMessage("You do not know enough about diseases and ailments.");
                                m_Viewer.SendGump(new HealthGump(m_Viewer, m_Viewed, m_Current));
                                return;
                            }
                        }
                }
            }

            base.OnResponse(sender, info);
        }
    }

    public class MedicalTreatmentConfirmGump : Gump
    {
        private PlayerMobile m_Healer;
        private PlayerMobile m_Target;
        private Injury m_Injury;

        public MedicalTreatmentConfirmGump(PlayerMobile from, PlayerMobile target, Injury injury)
            : base(0, 0)
        {
            m_Healer = from;
            m_Target = target;
            m_Injury = injury;

            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Healer.CloseGump(typeof(HealthGump));
            m_Healer.CloseGump(typeof(TryHealInjuryGump));
            m_Target.CloseGump(typeof(MedicalTreatmentConfirmGump));
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            AddBackground(304, 242, 228, 106, 9270);
            AddBackground(314, 255, 206, 81, 5120);

            AddLabel(424 - (int)(m_Healer.Name.Length * 3.35), 258, 247, m_Healer.Name.ToString());
            AddLabel(364, 282, 247, "wants to heal you.");

            AddButton(322, 312, 12000, 12002, 1, GumpButtonType.Reply, 0);
            AddButton(438, 312, 12018, 12020, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    {
                        m_Healer.SendMessage(m_Target.Name + " has refused your treatment.");
                        return;
                    }
                case 1:
                    {
                        m_Healer.SendMessage(m_Target.Name + " has accepted your treatment.");
                        m_Healer.SendGump(new TryHealInjuryGump(m_Healer, m_Target, m_Injury));
                        return;
                    }
                default:
                    {
                        m_Healer.SendMessage(m_Target.Name + " has refused your treatment.");
                        return;
                    }
            }

            base.OnResponse(sender, info);
        }
    }

    public class TryHealInjuryGump : Gump
    {
        private enum HealButton
        {
            Refuse,
            Accept,
            iCut,
            dCut,
            iSew,
            dSew,
            iHeat,
            dHeat,
            iCool,
            dCool,
            iBleed,
            dBleed
        }

        #region Variable Definition
        private PlayerMobile m_Viewer;
        private Mobile m_Injured;
        private Injury m_Injury;
        private HealInfo m_Info;
        private HealTimer m_Timer;

        //private const int m_TimeBarMax = 109;
        #endregion

        #region Constructors
        public TryHealInjuryGump(PlayerMobile viewer, Mobile injured, Injury injury) : this(viewer, injured, injury, new HealInfo()) { }

        public TryHealInjuryGump(PlayerMobile viewer, Mobile injured, Injury injury, HealInfo info) : this(viewer, injured, injury, info, new HealTimer(viewer, injured, injury, info)) { }

        public TryHealInjuryGump(PlayerMobile viewer, Mobile injured, Injury injury, HealInfo info, HealTimer timer)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Injured = injured;
            m_Injury = injury;
            m_Info = info;
            m_Timer = timer;
            if (!m_Timer.Running)
                m_Timer.Start();

            InitialSetup();
        }
        #endregion

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(HealthGump));
            m_Viewer.CloseGump(typeof(TryHealInjuryGump));
            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = false;
            AddPage(0);

            AddBackground(286, 149, 217, 301, 9270);
            AddBackground(302, 166, 182, 29, 5120);
            AddLabel(399 - (int)(HealthAttachment.GetName(m_Injury).Length * 3), 169, 247, HealthAttachment.GetName(m_Injury));

            #region HealInfo Controls
            AddBackground(303, 207, 182, 196, 5120);
            int m_X = 405;
            int m_Y = 220;
            AddLabel(m_X, m_Y, 0, "CUT"); m_Y += 25;
            AddItem(425, 224, 5110);
            AddLabel(m_X, m_Y, 0, "SEW"); m_Y += 25;
            AddItem(426, 249, 4001);
            AddLabel(m_X, m_Y, 0, "HEAT"); m_Y += 25;
            AddItem(433, 271, 2575);
            AddLabel(m_X, m_Y, 0, "COOL"); m_Y += 25;
            AddItem(429, 297, 4088);
            AddLabel(m_X, m_Y, 0, "BLEED");
            AddItem(449, 326, 3921);

            m_X = 375;
            m_Y = 220;

            AddButton(m_X, m_Y, 9760, 9761, (int)HealButton.iCut, GumpButtonType.Reply, 0); m_Y += 25;
            AddButton(m_X, m_Y, 9760, 9761, (int)HealButton.iSew, GumpButtonType.Reply, 0); m_Y += 25;
            AddButton(m_X, m_Y, 9760, 9761, (int)HealButton.iHeat, GumpButtonType.Reply, 0); m_Y += 25;
            AddButton(m_X, m_Y, 9760, 9761, (int)HealButton.iCool, GumpButtonType.Reply, 0); m_Y += 25;
            AddButton(m_X, m_Y, 9760, 9761, (int)HealButton.iBleed, GumpButtonType.Reply, 0); m_Y += 25;

            m_X = 353;
            m_Y = 220;

            AddLabel(m_X - (m_Info.Cut.ToString().Length), m_Y, 0, m_Info.Cut.ToString()); m_Y += 25;
            AddLabel(m_X - (m_Info.Sew.ToString().Length), m_Y, 0, m_Info.Sew.ToString()); m_Y += 25;
            AddLabel(m_X - (m_Info.Heat.ToString().Length), m_Y, 0, m_Info.Heat.ToString()); m_Y += 25;
            AddLabel(m_X - (m_Info.Cool.ToString().Length), m_Y, 0, m_Info.Cool.ToString()); m_Y += 25;
            AddLabel(m_X - (m_Info.Bleed.ToString().Length), m_Y, 0, m_Info.Bleed.ToString()); m_Y += 25;

            switch(m_Viewer.Feats.GetFeatLevel(FeatList.Surgery))
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        m_X = 325;
                        m_Y = 220;
                        AddButton(m_X, m_Y + 25, 9764, 9765, (int)HealButton.dSew, GumpButtonType.Reply, 0);
                        AddButton(m_Y, m_Y + 50, 9764, 9765, (int)HealButton.dHeat, GumpButtonType.Reply, 0);
                        break;
                    }
                case 2:
                    {
                        m_X = 325;
                        m_Y = 220;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dCut, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dSew, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dHeat, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dCool, GumpButtonType.Reply, 0); m_Y += 25;
                        break;
                    }
                case 3:
                    {
                        m_X = 325;
                        m_Y = 220;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dCut, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dSew, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dHeat, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dCool, GumpButtonType.Reply, 0); m_Y += 25;
                        AddButton(m_X, m_Y, 9764, 9765, (int)HealButton.dBleed, GumpButtonType.Reply, 0); m_Y += 25;
                        break;
                    }
            }
            #endregion

            #region Time Display

            AddLabel(346, 352, 247, "Time Remaining");
            m_Viewer.SendGump(new HealTimeBar(m_Viewer, m_Timer));

            #endregion

            #region Accept/Refuse

            AddButton(309, 413, 12000, 12002, (int)HealButton.Accept, GumpButtonType.Reply, 0);
            AddButton(402, 413, 12018, 12020, (int)HealButton.Refuse, GumpButtonType.Reply, 0);

            #endregion

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)HealButton.Refuse:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.SendGump(new HealthGump(m_Viewer, m_Injured));
                        m_Timer.Stop();
                        return;
                    }
                case (int)HealButton.Accept:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        HealthAttachment.TryHealInjury(m_Viewer, m_Injured, m_Injury, m_Info);
                        m_Viewer.SendGump(new HealthGump(m_Viewer, m_Injured));
                        m_Timer.Stop();
                        return;
                    }
                case (int)HealButton.iCut:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.Target = new CutTarget(m_Injured, m_Injury, m_Info, m_Timer);
                        return;
                    }
                case (int)HealButton.dCut:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Info.Cut--;
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Injured, m_Injury, m_Info, m_Timer));
                        return;
                    }
                case (int)HealButton.iSew:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.Target = new SewTarget(m_Injured, m_Injury, m_Info, m_Timer);
                        return;
                    }
                case (int)HealButton.dSew:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Info.Sew--;
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Injured, m_Injury, m_Info, m_Timer));
                        return;
                    }
                case (int)HealButton.iHeat:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.Target = new HeatTarget(m_Injured, m_Injury, m_Info, m_Timer);
                        return;
                    }
                case (int)HealButton.dHeat:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Info.Heat--;
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Injured, m_Injury, m_Info, m_Timer));
                        return;
                    }
                case (int)HealButton.iCool:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.Target = new CoolTarget(m_Injured, m_Injury, m_Info, m_Timer);
                        return;
                    }
                case (int)HealButton.dCool:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Info.Cool--;
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Injured, m_Injury, m_Info, m_Timer));
                        return;
                    }
                case (int)HealButton.iBleed:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Viewer.Target = new BleedTarget(m_Injured, m_Injury, m_Info, m_Timer);
                        return;
                    }
                case (int)HealButton.dBleed:
                    {
                        m_Viewer.CloseGump(typeof(HealTimeBar));
                        m_Info.Bleed--;
                        m_Viewer.SendGump(new TryHealInjuryGump(m_Viewer, m_Injured, m_Injury, m_Info, m_Timer));
                        return;
                    }
            }

            base.OnResponse(sender, info);
        }

        private class HealTimeBar : Gump
        {
            private HealTimer m_Timer;
            private int m_BarMax = 109;

            public HealTimeBar(PlayerMobile from, HealTimer timer)
                : base(0, 0)
            {
                m_Timer = timer;

                from.CloseGump(typeof(HealTimeBar));
                Closable = false;
                Disposable = false;
                Dragable = false;
                Resizable = false;
                //AddPage(0);

                AddImageTiled(340, 375, m_BarMax, 10, 2053);
                AddImageTiled(340, 375, (int)((double)m_BarMax * m_Timer.PercentLeft), 10, 2056);
            }
        }

        #region Surgery Targets
        private class CutTarget : Target
        {
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_Info;
            private HealTimer m_Timer;

            public CutTarget(Mobile injured, Injury injury, HealInfo info, HealTimer timer)
                : base(1, true, TargetFlags.None)
            {
                m_Injured = injured;
                m_Injury = injury;
                m_Info = info;
                m_Timer = timer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (IsValidCutObject(targeted))
                {
                    from.PlaySound(0x248);
                    m_Info.Cut += AddValue(targeted);
                    m_Info.Sew--;
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    return;
                }
                else
                {
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    from.SendMessage("You cannot use that.");
                    return;
                }

                base.OnTarget(from, targeted);
            }

            #region Target Catching - Sending the Gump Back to the Viewer
            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if(cancelType != TargetCancelType.Disconnected)
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetCancel(from, cancelType);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnNonlocalTarget(from, targeted);
            }
            #endregion

            public static int AddValue(object o)
            {
                if (o is Item)
                {
                    Item i = o as Item;

                    if (o is ButcherKnife)
                    {
                        (o as ButcherKnife).HitPoints--;
                        return 3;
                    }
                    if (o is Cleaver)
                    {
                        (o as Cleaver).HitPoints--;
                        return 4;
                    }
                    if (o is SkinningKnife)
                    {
                        (o as SkinningKnife).HitPoints--;
                        return 2;
                    }
                    if (o is Scissors)
                    {
                        return 1;
                    }
                }
                return 0;
            }

            public static bool IsValidCutObject(object o)
            {
                if (o is Item)
                {
                    if (o is ButcherKnife)
                    {
                        
                        return true;
                    }
                    if (o is Cleaver)
                    {
                        
                        return true;
                    }
                    if (o is SkinningKnife)
                    {
                        
                        return true;
                    }
                    if (o is Scissors)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class SewTarget : Target
        {
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_Info;
            private HealTimer m_Timer;

            public SewTarget(Mobile injured, Injury injury, HealInfo info, HealTimer timer)
                : base(1, true, TargetFlags.None)
            {
                m_Injured = injured;
                m_Injury = injury;
                m_Info = info;
                m_Timer = timer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (IsValidSewObject(targeted))
                {
                    from.PlaySound(0x059);
                    m_Info.Sew += AddValue(targeted);
                    m_Info.Cut--;
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    return;
                }
                else
                {
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    from.SendMessage("You cannot use that.");
                    return;
                }

                base.OnTarget(from, targeted);
            }

            #region Target Catching - Sending the Gump Back to the Viewer
            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetCancel(from, cancelType);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnNonlocalTarget(from, targeted);
            }
            #endregion

            public static int AddValue(object o)
            {
                if (o is Item)
                {
                    if (o is SpoolOfThread)
                    {
                        (o as SpoolOfThread).Consume(1);
                        return 2;
                    }
                    if (o is Bandage)
                    {
                        (o as Bandage).Consume(1);
                        return 1;
                    }
                }
                return 0;
            }

            public static bool IsValidSewObject(object o)
            {
                if (o is Item)
                {
                    if (o is SpoolOfThread)
                    {
                        return true;
                    }
                    if (o is Bandage)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class HeatTarget : Target
        {
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_Info;
            private HealTimer m_Timer;

            public HeatTarget(Mobile injured, Injury injury, HealInfo info, HealTimer timer)
                : base(1, true, TargetFlags.None)
            {
                m_Injured = injured;
                m_Injury = injury;
                m_Info = info;
                m_Timer = timer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (IsValidHeatObject(targeted))
                {
                    from.PlaySound(0x02B);
                    m_Info.Heat += AddValue(targeted);
                    m_Info.Cool--;
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    return;
                }
                else
                {
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    from.SendMessage("You cannot use that.");
                    return;
                }

                base.OnTarget(from, targeted);
            }

            #region Target Catching - Sending the Gump Back to the Viewer
            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetCancel(from, cancelType);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnNonlocalTarget(from, targeted);
            }
            #endregion

            public static int AddValue(object o)
            {
                if (o is Item)
                {
                    Item i = o as Item;

                    if (o is CandleShort)
                    {
                        return 1;
                    }
                    if (o is Candle)
                    {
                        return 2;
                    }
                    if (o is CandleSkull)
                    {
                        return 3;
                    }
                    if (o is CandleLong)
                    {
                        return 4;
                    }
                    if (o is CandleLarge)
                    {
                        return 5;
                    }
                    if (o is Lantern)
                    {
                        return 6;
                    }
                    if (o is Torch)
                    {
                        return 7;
                    }
                    if (o is Campfire)
                    {
                        return 8;
                    }
                }
                return 0;
            }

            public static bool IsValidHeatObject(object o)
            {
                if (o is Item)
                {
                    if (o is Candle || o is CandleShort || o is CandleLarge || o is CandleLong || o is CandleSkull)
                        return true;
                    if (o is Torch)
                        return true;
                    if (o is Lantern)
                        return true;
                    if (o is Campfire)
                        return true;
                }

                return false;
            }
        }

        private class CoolTarget : Target
        {
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_Info;
            private HealTimer m_Timer;

            public CoolTarget(Mobile injured, Injury injury, HealInfo info, HealTimer timer)
                : base(1, true, TargetFlags.None)
            {
                m_Injured = injured;
                m_Injury = injury;
                m_Info = info;
                m_Timer = timer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (IsValidCoolObject(targeted))
                {
                    from.PlaySound(0x23F);
                    m_Info.Cool += AddValue(targeted);
                    m_Info.Heat--;
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    return;
                }
                else
                {
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    from.SendMessage("You cannot use that.");
                    return;
                }

                base.OnTarget(from, targeted);
            }

            #region Target Catching - Sending the Gump Back to the Viewer
            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetCancel(from, cancelType);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnNonlocalTarget(from, targeted);
            }
            #endregion

            public static int AddValue(object o)
            {
                if (o is Item)
                {
                    if (o is Pitcher)
                    {
                        (o as Pitcher).Quantity--;
                        return 1;
                    }
                    if (o is WetCloth)
                    {
                        (o as WetCloth).Consume(1);
                        return 2;
                    }
                }
                return 0;
            }

            public static bool IsValidCoolObject(object o)
            {
                if (o is Pitcher && (o as Pitcher).Content == BeverageType.Water && !(o as Pitcher).IsEmpty)
                    return true;

                if (o is WetCloth)
                    return true;

                return false;
            }
        }

        private class BleedTarget : Target
        {
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_Info;
            private HealTimer m_Timer;

            public BleedTarget(Mobile injured, Injury injury, HealInfo info, HealTimer timer)
                : base(1, true, TargetFlags.None)
            {
                m_Injured = injured;
                m_Injury = injury;
                m_Info = info;
                m_Timer = timer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (IsValidBleedObject(targeted))
                {
                    from.PlaySound(0x2D6);
                    int add = AddValue(targeted);
                    m_Info.Bleed += add;
                    m_Injured.Damage(Utility.RandomMinMax(1, 5) * add);
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    return;
                }
                else
                {
                    from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                    from.SendMessage("You cannot use that.");
                    return;
                }
            }

            #region Target Catching - Sending the Gump Back to the Viewer
            protected override void OnCantSeeTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnCantSeeTarget(from, targeted);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetCancel(from, cancelType);
            }

            protected override void OnTargetDeleted(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetDeleted(from, targeted);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfLOS(from, targeted);
            }

            protected override void OnTargetUntargetable(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetUntargetable(from, targeted);
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetNotAccessible(from, targeted);
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnTargetOutOfRange(from, targeted);
            }

            protected override void OnNonlocalTarget(Mobile from, object targeted)
            {
                from.SendGump(new TryHealInjuryGump(from as PlayerMobile, m_Injured, m_Injury, m_Info, m_Timer));
                base.OnNonlocalTarget(from, targeted);
            }
            #endregion

            public static int AddValue(object o)
            {
                if (o is Item)
                {
                    if (o is Dagger)
                    {
                        (o as Dagger).HitPoints--;
                        return 2;
                    }
                    if (o is Leech)
                    {
                        (o as Leech).Consume(1);
                        return 1;
                    }
                }
                return 0;
            }

            public static bool IsValidBleedObject(object o)
            {
                if (o is Dagger)
                {
                    return true;
                }
                if (o is Leech)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        public static void FailToHeal(PlayerMobile healer, Mobile injured)
        {
            if (injured is PlayerMobile && (injured as PlayerMobile).IsHardcore)
				{
				if ((healer.Feats.GetFeatLevel(FeatList.Surgery) * 10) + healer.RawInt > Utility.RandomMinMax(1, 150))
                return;
				}
		
			if ((healer.Feats.GetFeatLevel(FeatList.Surgery) * 10) + healer.RawInt > Utility.RandomMinMax(1, 100))//Potential of 30 to 180 chance)
                return;

            if (HealthAttachment.HasHealthAttachment(injured))
            {
                HealthAttachment.GetHA(injured).TryInjure(healer);
            }
        }
     
        public class HealTimer : Timer
        {
            private PlayerMobile m_Healer;
            private Mobile m_Injured;
            private Injury m_Injury;
            private HealInfo m_HealInfo;

            private DateTime m_Expiration;
            public DateTime Expiration { get { return m_Expiration; } set { m_Expiration = value; } }

            public TimeSpan TimeLeft { get { return (m_Expiration - DateTime.Now); } }
            public TimeSpan MaxTime { get { return (m_Expiration - m_StartTime); } }

            private DateTime m_StartTime;
            public DateTime StartTime { get { return m_StartTime; } set { m_StartTime = value; } }

            public double PercentLeft { get { return ( TimeLeft.TotalSeconds / MaxTime.TotalSeconds ); } }

            public HealTimer(PlayerMobile healer, Mobile injured, Injury injury, HealInfo info) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            {
                m_Healer = healer;
                m_Injured = injured;
                m_Injury = injury;
                m_HealInfo = info;

                int medVal = 15;
                if (healer != null && !healer.Deleted & healer.Alive)
                {
                    medVal += (healer.Feats.GetFeatLevel(FeatList.Medicine) * 5);
                    medVal += (healer.Feats.GetFeatLevel(FeatList.Surgery) * 10);
                }

                if(injured != null && !injured.Deleted && injured.Alive)
                    medVal += injured.Stam / 10;

                m_StartTime = DateTime.Now;
                m_Expiration = DateTime.Now.AddSeconds(medVal);
            }

            protected override void OnTick()
            {
                #region Null Catch
                if (m_Healer == null || m_Healer.Deleted || !m_Healer.Alive)
                {
                    Stop();
                    return;
                }

                if (m_Injured == null || m_Injured.Deleted || !m_Injured.Alive || m_Injured.IsDeadBondedPet)
                {
                    Stop();
                    return;
                }
                #endregion

                if (!m_Healer.InRange(m_Injured.Location, 1))
                {
                    if (m_Healer.Target != null && (m_Healer.Target is CutTarget || m_Healer.Target is SewTarget || m_Healer.Target is HeatTarget || m_Healer.Target is CoolTarget || m_Healer.Target is BleedTarget))
                    {
                        m_Healer.Target.Cancel(m_Healer, TargetCancelType.Disconnected);
                    }
                    m_Healer.SendMessage(m_Injured.Name + " is too far away.");
                    TryHealInjuryGump.FailToHeal(m_Healer, m_Injured);
                    m_Healer.CloseGump(typeof(TryHealInjuryGump));
                    m_Healer.CloseGump(typeof(HealTimeBar));
                    m_Healer.SendGump(new HealthGump(m_Healer, m_Injured));
                    Stop();
                    return;
                }
                else if (m_Expiration <= DateTime.Now)
                {
                    if (m_Healer.Target != null && (m_Healer.Target is CutTarget || m_Healer.Target is SewTarget || m_Healer.Target is HeatTarget || m_Healer.Target is CoolTarget || m_Healer.Target is BleedTarget))
                    {
                        m_Healer.Target.Cancel(m_Healer, TargetCancelType.Disconnected);
                    }
                    m_Healer.SendMessage("You have failed to treat the injury.");
                    TryHealInjuryGump.FailToHeal(m_Healer, m_Injured);
                    m_Healer.CloseGump(typeof(TryHealInjuryGump));
                    m_Healer.CloseGump(typeof(HealTimeBar));
                    m_Healer.SendGump(new HealthGump(m_Healer, m_Injured));
                    Stop();
                    return;
                }
                else
                {
                    if (m_Healer.Target == null || (m_Healer.Target != null && !(m_Healer.Target is CutTarget || m_Healer.Target is SewTarget || m_Healer.Target is HeatTarget || m_Healer.Target is CoolTarget || m_Healer.Target is BleedTarget)))
                    {
                        m_Healer.CloseGump(typeof(HealTimeBar));
                        m_Healer.SendGump(new HealTimeBar(m_Healer, this));
                    }
                }
                
                base.OnTick();
            }
        }

        [PropertyObject]
        public class HealInfo
        {
            private int m_Cut;
            private int m_Sew;
            private int m_Heat;
            private int m_Cool;
            private int m_Bleed;

            public int Cut { get { return m_Cut; } set { m_Cut = value; if (m_Cut < 0) m_Cut = 0; } }
            public int Sew { get { return m_Sew; } set { m_Sew = value; if (m_Sew < 0) m_Sew = 0;  } }
            public int Heat { get { return m_Heat; } set { m_Heat = value; if (m_Heat < 0) m_Heat = 0; } }
            public int Cool { get { return m_Cool; } set { m_Cool = value; if (m_Cool < 0) m_Cool = 0; } }
            public int Bleed { get { return m_Bleed; } set { m_Bleed = value; if (m_Bleed < 0) m_Bleed = 0; } }

            public HealInfo() : this(0, 0, 0, 0, 0) { }

            public HealInfo(int cut, int sew, int heat, int cool, int bleed)
            {
                m_Cut = cut;
                m_Sew = sew;
                m_Heat = heat;
                m_Cool = cool;
                m_Bleed = bleed;
            }
        }

        [PropertyObject]
        public class HealInjuryRequirements
        {
            private enum Severity
            {
                None = 0,
                Minor = 1,
                Moderate = 2,
                Severe = 3,
                Critical = 4,
                Deadly = 5
            }

            #region Variable Declaration, Get/Setters
            private int m_Cut;
            private int m_Sew;
            private int m_Heat;
            private int m_Cool;
            private int m_Bleed;

            public int Cut { get { return m_Cut; } set { m_Cut = value; } }
            public int Sew { get { return m_Sew; } set { m_Sew = value; } }
            public int Heat { get { return m_Heat; } set { m_Heat = value; } }
            public int Cool { get { return m_Cool; } set { m_Cool = value; } }
            public int Bleed { get { return m_Bleed; } set { m_Bleed = value; } }
            #endregion

            public HealInjuryRequirements()
            {

            }

            public static bool MeetsRequirements(PlayerMobile healer, Mobile injured, HealInfo info, Injury injury)
            {
                HealInjuryRequirements req = HealInjuryRequirements.GetRequirements(injury);

                int MOE = (healer.Feats.GetFeatLevel(FeatList.Surgery) > 2 ? 1 : 0); // Margin of error from having max Surgery.

                if (!(info.Cut >= req.Cut - MOE && info.Cut <= req.Cut + MOE))
                    return false;
                if (!(info.Sew >= req.Sew - MOE && info.Sew <= req.Sew + MOE))
                    return false;
                if (!(info.Heat >= req.Heat - MOE && info.Heat <= req.Heat + MOE))
                    return false;
                if (!(info.Cool >= req.Cool - MOE && info.Cool <= req.Cool + MOE))
                    return false;
                if (!(info.Bleed >= req.Bleed - MOE && info.Bleed <= req.Bleed + MOE))
                    return false;

                return true;
            }

            public static HealInjuryRequirements GetRequirements(Injury i)
            {
                HealInjuryRequirements reqs = new HealInjuryRequirements();
                reqs.Cut = 0;
                reqs.Sew = 0;
                reqs.Cool = 0;
                reqs.Heat = 0;
                reqs.Bleed = 0;

                switch (i)
                {
                    case Injury.Bloodied:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.BrokenBack:
                        {
                            reqs.Cut = (int)Severity.Severe;
                            reqs.Sew = (int)Severity.Critical;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.BrokenJaw:
                        {
                            reqs.Cut = (int)Severity.Severe;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.BrokenLeftArm:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.BrokenLeftLeg:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.BrokenNeck:
                        {
                            reqs.Cut = (int)Severity.Moderate;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Severe;
                            break;
                        }
                    case Injury.BrokenRightArm:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.BrokenRightLeg:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.Bruised:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.DeepCut:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.Exhausted:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.FracturedLeftArm:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.FracturedLeftLeg:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.Minor;
                            break;
                        }
                    case Injury.FracturedRibs:
                        {
                            reqs.Cut = (int)Severity.Moderate;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.FracturedRightArm:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.FracturedRightLeg:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.Minor;
                            break;
                        }
                    case Injury.FracturedSkull:
                        {
                            reqs.Cut = (int)Severity.Moderate;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.Moderate;
                            break;
                        }
                    case Injury.InternalBleeding:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.Severe;
                            break;
                        }
                    case Injury.LaceratedTorso:
                        {
                            reqs.Cut = (int)Severity.Minor;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.Moderate;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.MajorConcussion:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Moderate;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.Critical;
                            break;
                        }
                    case Injury.MassiveBleeding:
                        {
                            reqs.Cut = (int)Severity.Moderate;
                            reqs.Sew = (int)Severity.Severe;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.Deadly;
                            break;
                        }
                    case Injury.MinorConcussion:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.Severe;
                            break;
                        }
                    case Injury.MinorCut:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.Minor;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                    case Injury.BrokenSkull:
                        {
                            reqs.Cut = (int)Severity.Severe;
                            reqs.Sew = (int)Severity.Moderate;
                            reqs.Cool = (int)Severity.None;
                            reqs.Heat = (int)Severity.Minor;
                            reqs.Bleed = (int)Severity.Deadly;
                            break;
                        }
                    case Injury.Winded:
                        {
                            reqs.Cut = (int)Severity.None;
                            reqs.Sew = (int)Severity.None;
                            reqs.Cool = (int)Severity.Minor;
                            reqs.Heat = (int)Severity.None;
                            reqs.Bleed = (int)Severity.None;
                            break;
                        }
                }

                return reqs;
            }
        }
    }

    public class DiseaseGump : Gump
    {
        private enum DiseaseButton
        {
            Okay,
            Prev,
            Next
        }

        private PlayerMobile m_Viewer;
        private Mobile m_Viewed;
        private int m_Current;

        public DiseaseGump(PlayerMobile viewer, Mobile viewed, int current)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Viewed = viewed;
            m_Current = current;
            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(HealthGump));
            m_Viewer.CloseGump(typeof(TryHealInjuryGump));
            m_Viewer.CloseGump(typeof(DiseaseGump));
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            AddBackground(305, 179, 198, 262, 5120);
            AddBackground(319, 186, 173, 29, 9350);
            AddBackground(319, 221, 173, 188, 9350);

            if (HealthAttachment.GetHA(m_Viewed).CurrentDiseases.Count > m_Current)
            { 
                AddLabel(408 - (int)(HealthAttachment.GetHA(m_Viewed).CurrentDiseases[m_Current].Name.Length * 2.9), 192, 247, HealthAttachment.GetHA(m_Viewed).CurrentDiseases[m_Current].Name);
                WriteDiagnosis();
                AddButton(321, 416, 5603, 5607, (int)DiseaseButton.Prev, GumpButtonType.Reply, 0);
                AddButton(471, 416, 5601, 5605, (int)DiseaseButton.Next, GumpButtonType.Reply, 0);
            }
                
            AddButton(375, 412, 249, 248, (int)DiseaseButton.Okay, GumpButtonType.Reply, 0);
            
        }

        public void WriteDiagnosis()
        {
            int MaxCharacters = 20;
            string[] diagnosis = HealthAttachment.GetHA(m_Viewed).CurrentDiseases[m_Current].Diagnosis.Split(' ');
            string line = "";
            int m_X = 327;
            int m_Y = 229;
            for (int i = 0; i < diagnosis.Length; i++)
            {
                line += diagnosis[i] + " ";
                if (line.Length >= MaxCharacters)
                {
                    AddLabel(m_X, m_Y, 0, line);
                    line = "";
                    m_Y += 20;
                }
            }
            if (line != "")
            {
                AddLabel(m_X, m_Y, 0, line);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer.Deleted || !m_Viewer.Alive)
                return;
            if (m_Viewed == null || m_Viewed.Deleted || !m_Viewed.Alive || m_Viewed.IsDeadBondedPet)
                return;
            if (!m_Viewer.InRange(m_Viewed.Location, 5))
            {
                m_Viewer.SendMessage("You are too far away to observe that.");
                return;
            }

            switch (info.ButtonID)
            {
                case (int)DiseaseButton.Prev:
                {
                    if (m_Current > 0)
                        m_Current--;
                    m_Viewer.SendGump(new DiseaseGump(m_Viewer, m_Viewed, m_Current));
                    return;
                }
                case (int)DiseaseButton.Next:
                {
                    if (m_Current < HealthAttachment.GetHA(m_Viewed).CurrentDiseases.Count - 1)
                        m_Current++;
                    m_Viewer.SendGump(new DiseaseGump(m_Viewer, m_Viewed, m_Current));
                    return;
                }
                case (int)DiseaseButton.Okay:
                {
                    m_Viewer.SendGump(new HealthGump(m_Viewer, m_Viewed));
                    return;
                }
            }

            base.OnResponse(sender, info);
        }
    }

}