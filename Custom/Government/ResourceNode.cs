using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Government;
using Server.Network;
using Server.Engines.XmlSpawner2;
using Server.TimeSystem;

namespace Server.Items
{
    public class ResourceNode : Item
    {
        #region Private Variable Declaration
        private Timer m_ClaimTimer;

        private ResourceType m_Resource;
        private GovernmentEntity m_Government;
        private bool m_Owned;
        private DateTime m_ProductionDate;
        private int m_ProductionRate;
        private string BaseName;

        private PlayerMobile m_Claimant;
        private GovernmentEntity m_ClaimantGovernment;
        private DateTime m_LastCapture;

        private ResourceTimer m_Timer;
        #endregion

        #region GET/SET
        [CommandProperty(AccessLevel.GameMaster)]
        public ResourceType Resource { get { return m_Resource; } set { m_Resource = value; SetNodeAppearance(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public GovernmentEntity Government { get { return m_Government; } set { m_Government = value; } }

        public bool Owned { get { return m_Owned; } set { m_Owned = value; } }
        
        public DateTime ProductionDate { get { return m_ProductionDate; } set { m_ProductionDate = value; } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int ProductionRate { get { return m_ProductionRate; } set { m_ProductionRate = value; } }

        public PlayerMobile Claimant { get { return m_Claimant; } set { m_Claimant = value; } }
        public GovernmentEntity ClaimantGovernment { get { return m_ClaimantGovernment; } set { m_ClaimantGovernment = value; } }

        public TimeSpan ClaimTime
        {
            get
            {
                double minutes = 30.0;
                if (m_ClaimTimer.Running)
                {
                    if (ClaimantGovernment != null && !ClaimantGovernment.Deleted)
                    {
                        IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 15);
                        foreach (Mobile m in eable)
                        {
                            if (m is PlayerMobile)
                            {
                                if (CustomGuildStone.IsGuildMember(m as PlayerMobile, ClaimantGovernment))
                                    minutes -= 2.0;
                            }
                            else if (m is BaseKhaerosMobile)
                            {
                                BaseKhaerosMobile s = m as BaseKhaerosMobile;
                                if (s != null && !s.Deleted && s.Alive && s.Government != null && !s.Government.Deleted)
                                    if (s.Government == ClaimantGovernment)
                                        minutes -= 1.0;
                            }
                        }
                        eable.Free();
                        if (minutes < 10.0)
                            minutes = 10.0;
                        return TimeSpan.FromMinutes(minutes);
                    }
                    else
                    {
                        m_ClaimTimer.Stop();
                        m_ClaimTimer = null;
                        return TimeSpan.FromHours(24);
                    }
                }
                return TimeSpan.FromHours(24);
            }
        }
        #endregion

        [Constructable]
        public ResourceNode() : this(0, 0) { }

        [Constructable]
        public ResourceNode(ResourceType r, int rate)
            : base()
        {
            Resource = r;
            ProductionRate = rate;
            Government = null;
            Owned = false;
            BaseName = "";
            Movable = false;

            SetNodeAppearance();
        }

        public void SendResource()
        {
            if ((Government == null) || (Government.Deleted))
            {
                Lose();
                return;
            }
            Government.Resources[this.Resource] += ProductionRate;
            ProductionDate = DateTime.Now.AddDays(1);
        }
        
        public void SetNodeAppearance()
        {
            switch (m_Resource)
            {
                case ResourceType.None: ItemID = 0x0DDA; Visible = false; BaseName = "Resource Node"; break;
                case ResourceType.Metals:
                    if (Utility.RandomBool())
                    {
                        ItemID = 0x1A83;

                        EmptyJar front = new EmptyJar(0x1A86);
                        front.Movable = false;
                        front.Name = "";
                        front.MoveToWorld(this.Location, this.Map);
                        front.Y--;

                        EmptyJar back = new EmptyJar(0x1A82);
                        back.Movable = false;
                        back.Name = "";
                        front.MoveToWorld(this.Location, this.Map);
                        back.Y++;
                    }
                    else
                    {
                        ItemID = 0x1A88;

                        EmptyJar front = new EmptyJar(0x1A8B);
                        front.Movable = false;
                        front.Name = "";
                        front.MoveToWorld(this.Location, this.Map);
                        front.X--;

                        EmptyJar back = new EmptyJar(0x1A87);
                        back.Movable = false;
                        back.Name = "";
                        back.MoveToWorld(this.Location, this.Map);
                        back.X++;
                    }
                    BaseName = "Metal Resources";
                    break;
                case ResourceType.Food: ItemID = 0x0E5B; BaseName = "Food Resources"; break;
                case ResourceType.Water: ItemID = 0x0E7B; BaseName = "Water Resources"; break;
                case ResourceType.Cloth: ItemID = 0x0DEF; BaseName = "Cloth Resources"; break;
                case ResourceType.Wood: { if (Utility.RandomBool()) ItemID = 0x1BDF; else ItemID = 0x1BE2; } BaseName = "Wood Resources"; break;
                case ResourceType.Influence: BaseName = "Influence Resources"; break;
                default: goto case 0;
            }

            Name = BaseName;
            if (m_Government != null && !m_Government.Deleted)
                Name = BaseName + " [" + m_Government.Name.ToString() + "]";
        }

        public void Release(PlayerMobile pm, GovernmentEntity g)
        {
            if( !(GovernmentEntity.IsGuildOfficer(pm, g)) )
            {
                pm.SendMessage("Only officers may order a resource to be abandoned.");
                return;
            }

            pm.RevealingAction();
            pm.SendMessage("You order the " + this.Resource.ToString() + " to be abandoned by " + this.Government.Name.ToString() + ".");
            Owned = false;
            Name = BaseName;
            Government = null;
        }

        public void Lose()
        {
            if (Government != null && !Government.Deleted)
            {
                ReportInfo newReport = new ReportInfo( "Lost contact with " + this.Resource.ToString() + " at X: " + this.X.ToString() + " and Y: " + this.Y.ToString() + ".");
                newReport.TimeOfReport = Formatting.GetTimeFormat(this, Format.Time);
                Government.Reports.Add(newReport);
            }

            Owned = false;
            Government = null;
            Name = BaseName;

            if (m_Timer != null && m_Timer.Running)
                m_Timer.Stop();
        }

        public void Claim(PlayerMobile pm, GovernmentEntity g)
        {
            if (Government != null && !Government.Deleted && Government == g)
            {
                pm.SendMessage(g.Name.ToString() + " already owns this resource.");
                return;
            }

            if (m_ClaimantGovernment != null && !m_ClaimantGovernment.Deleted && m_ClaimantGovernment == g)
            {
                pm.SendMessage(g.Name.ToString() + " is already laying claim to this resource.");
                return;
            }

            if (Owned && (DateTime.Now < m_LastCapture + TimeSpan.FromDays(1)) )
            {
                pm.SendMessage("It is too soon to claim this resource.");
                return;
            }

            if (Owned && ( DateTime.Now < (pm.LastDeath + TimeSpan.FromHours(1)) ) )
            {
                pm.SendMessage("You have been injured in the last hour. Try resting before attempting to claim these resources.");
                return;
            }
            
            if (!CustomGuildStone.IsGuildOfficer(pm, g))
            {
                pm.SendMessage("Only officers of " + g.Name.ToString() + " may claim resources.");
                return;
            }

            pm.RevealingAction();

            m_Claimant = pm;
            m_ClaimantGovernment = g;
            m_ClaimTimer = new ClaimResourceTimer(this);
            m_ClaimTimer.Start();

            if (m_Government != null && !m_Government.Deleted)
            {
                bool nodeThief = true;

                foreach (CustomGuildStone c in CustomGuildStone.Guilds)
                {
                    if (CustomGuildStone.IsGuildMember(pm, c))
                    {
                        if (m_Government.AlliedGuilds.Contains(c))
                        {
                            nodeThief = false;
                            continue;
                        }
                    }
                }

                if (nodeThief)
                {
                    XmlAttachment attachment = null;
                    attachment = XmlAttach.FindAttachmentOnMobile(pm, typeof(XmlCriminal), m_Government.Nation.ToString());

                    if (attachment == null)
                    {
                        XmlAttach.AttachTo(pm, new XmlCriminal(m_Government.Nation));
                        pm.SendMessage(Soldier.CriminalAlertMessage(m_Government.Nation));

                        pm.RevealingAction();
                    }
                }
            }

            m_Claimant.SendMessage("You begin laying claim to the " + Resource.ToString() + " in the name of " + m_ClaimantGovernment.Name.ToString() + ".");
        }

        public void Capture()
        {
            if (m_ClaimantGovernment == null || m_ClaimantGovernment.Deleted)
                return;

            if (Government != null && !Government.Deleted && Government != m_ClaimantGovernment)
            {
                ReportInfo newReport = new ReportInfo("Lost contact with " + this.Resource.ToString() + " at X: " + this.X.ToString() + " and Y: " + this.Y.ToString() + ".");
                newReport.TimeOfReport = Formatting.GetTimeFormat(this, Format.Time);
                Government.Reports.Add(newReport);

                List<MilitarySpawner> nearbySpawners = new List<MilitarySpawner>();
                IPooledEnumerable nearbyItems = this.Map.GetItemsInRange(this.Location, 15);
                foreach (Item item in nearbyItems)
                {
                    if (item is MilitarySpawner && (item as MilitarySpawner).Government != null && !(item as MilitarySpawner).Government.Deleted)
                    {
                        MilitarySpawner spawner = item as MilitarySpawner;

                        if (spawner.Government == Government)
                            nearbySpawners.Add(spawner);
                        else if (Government.AlliedGuilds.Contains(spawner.Government))
                            nearbySpawners.Add(spawner);
                        else if (m_ClaimantGovernment.EnemyGuilds.Contains(spawner.Government))
                            nearbySpawners.Add(spawner);
                    }
                }
                nearbyItems.Free();

                for (int i = (nearbySpawners.Count - 1); i > -1; i--)
                    nearbySpawners[i].Delete();
                nearbySpawners.Clear();
            }

            IPooledEnumerable eable = Map.GetItemsInRange(this.Location, 15);
            List<MilitarySpawner> removeSpawner = new List<MilitarySpawner>();
            foreach (Item i in eable)
            {
                if (i is MilitarySpawner)
                {
                    MilitarySpawner spawner = i as MilitarySpawner;
                    if (spawner.Government != null && !spawner.Government.Deleted)
                        if (m_ClaimantGovernment.EnemyGuilds.Contains(spawner.Government))
                            removeSpawner.Add(spawner);
                }
            }
            eable.Free();
            for (int i = (removeSpawner.Count - 1); i > -1; i--)
                removeSpawner[i].Delete();
            removeSpawner.Clear();

            Government = m_ClaimantGovernment;
            Owned = true;
            ProductionDate = DateTime.Now.AddDays(1);
            Name = BaseName + " [" + Government.Name.ToString() + "]";

            if(m_LastCapture + TimeSpan.FromDays(7.0) <= DateTime.Now)
            {
                eable = this.Map.GetMobilesInRange(this.Location, 15);
                foreach (Mobile m in eable)
                {
                    if (m is PlayerMobile && Government != null && !Government.Deleted && Government.Members.Contains(m))
                    {
                        m.SendMessage("You have successfully claimed the " + this.Resource.ToString() + "!");
                        int reward = ProductionRate * Utility.RandomMinMax( Utility.Random(Government.Members.Count) + 1, Government.Members.Count + 2 );
                        LevelSystem.AwardExp(m as PlayerMobile, Utility.RandomMinMax(ProductionRate, reward));
                        LevelSystem.AwardCP(m as PlayerMobile, Utility.RandomMinMax(ProductionRate, reward));
                    }

                    if (m is BaseCreature && 
                        Government != null && 
                        !Government.Deleted && 
                        ((m as BaseCreature).Government != null && 
                        (m as BaseCreature).Government == Government || 
                        Government.Members.Contains((m as BaseCreature).ControlMaster)))
                    {
                        LevelSystem.AwardExp(m as BaseCreature, Utility.RandomMinMax(ProductionRate + 1, (ProductionRate * (Utility.Random(25) + 1))));
                    }
                }
                eable.Free();
            }

            m_LastCapture = DateTime.Now;
            m_Claimant = null;
            m_ClaimantGovernment = null;
            m_ClaimTimer.Stop();

            m_Timer = new ResourceTimer(this);
            m_Timer.Start();
        }

        public bool SearchForOwners()
        {
            bool OwnerNearby = false;

            if (m_Owned) //If the resource node is currently owned...
            {
                IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 14);
                foreach (Mobile m in eable)
                {
                    if (m is PlayerMobile && ( DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(1)) && m_Government.Members.Contains(m))
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (m is BaseKhaerosMobile &&
                        (m as BaseKhaerosMobile).Government != null &&
                        !(m as BaseKhaerosMobile).Government.Deleted &&
                        (m as BaseKhaerosMobile).Government == m_Government)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(12)))
                        {
                            if (CustomGuildStone.IsGuildMember(m as PlayerMobile, g) && m_Government.AlliedGuilds.Contains(g))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                        else if (m is BaseKhaerosMobile)
                        {
                            if ((m as BaseKhaerosMobile).Government != null
                                && !(m as BaseKhaerosMobile).Government.Deleted
                                && m_Government.AlliedGuilds.Contains((m as BaseKhaerosMobile).Government))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                    }

                    if (m is BaseCreature 
                        && (m as BaseCreature).Controlled 
                        && (m as BaseCreature).ControlMaster != null 
                        && CustomGuildStone.IsGuildMember((m as BaseCreature).ControlMaster as PlayerMobile, m_Government)
                        && !(m as BaseCreature).IsDeadBondedPet
                        && !(m as BaseCreature).IsDeadPet)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (OwnerNearby)
                        continue;
                }
                eable.Free();
            }
            return OwnerNearby;
        }

        public bool SearchForOwnersRT()
        {
            bool OwnerNearby = false;
            bool SpawnerNearby = false;

            if (m_Owned) //If the resource node is currently owned...
            {
                if ((DateTime.Now < m_LastCapture + TimeSpan.FromDays(1)))
                    return true;

                IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 14);
                foreach (Mobile m in eable)
                {
                    if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(1)) && m_Government.Members.Contains(m))
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (m is BaseKhaerosMobile &&
                        (m as BaseKhaerosMobile).Government != null &&
                        !(m as BaseKhaerosMobile).Government.Deleted &&
                        (m as BaseKhaerosMobile).Government == m_Government)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(12)))
                        {
                            if (CustomGuildStone.IsGuildMember(m as PlayerMobile, g) && m_Government.AlliedGuilds.Contains(g))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                        else if (m is BaseKhaerosMobile)
                        {
                            if ((m as BaseKhaerosMobile).Government != null
                                && !(m as BaseKhaerosMobile).Government.Deleted
                                && m_Government.AlliedGuilds.Contains((m as BaseKhaerosMobile).Government))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                    }

                    if (m is BaseCreature
                        && (m as BaseCreature).Controlled
                        && (m as BaseCreature).ControlMaster != null
                        && CustomGuildStone.IsGuildMember((m as BaseCreature).ControlMaster as PlayerMobile, m_Government)
                        && !(m as BaseCreature).IsDeadBondedPet
                        && !(m as BaseCreature).IsDeadPet)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (OwnerNearby)
                        continue;
                }
                eable.Free();

                if (!OwnerNearby) //No owners? Check for spawners.
                {
                    IPooledEnumerable nearbyItems = this.Map.GetItemsInRange(this.Location, 15);
                    foreach (Item item in nearbyItems)
                    {
                        if (item is MilitarySpawner && (item as MilitarySpawner).Government != null && !(item as MilitarySpawner).Government.Deleted)
                        {
                            MilitarySpawner spawner = item as MilitarySpawner;

                            if (spawner.Government == Government)
                                SpawnerNearby = true;
                            else if (Government.AlliedGuilds.Contains(spawner.Government))
                                SpawnerNearby = true;
                        }
                    }
                    nearbyItems.Free();
                }

                if (!OwnerNearby && SpawnerNearby) //Spawners but no owners? Let's check for hostiles that have broken contact
                {
                    eable = this.Map.GetMobilesInRange(this.Location, 14);
                    foreach (Mobile m in eable)
                    {
                        if ( m.Body == 400 || m.Body == 401 )
                            SpawnerNearby = false;
                        if ((m is ILargePredator || m is IMediumPredator && !(m is IPeacefulPredator) ) || m is IUndead || m is IDraconic || m is IBeastman || m is IGoatman)
                            SpawnerNearby = false;
                        if (!SpawnerNearby)
                            continue;
                    }
                    eable.Free();
                }
                
            }
            if (!OwnerNearby && SpawnerNearby)
                OwnerNearby = true;
            return OwnerNearby;
        }

        public bool SearchForOwnersDC()
        {
            bool OwnerNearby = false;

            if (m_Owned) //If the resource node is currently owned...
            {
                IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 14);
                foreach (Mobile m in eable)
                {
                    if (m is PlayerMobile && !m.Hidden && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(1)) && m_Government.Members.Contains(m))
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (m is BaseKhaerosMobile &&
                        (m as BaseKhaerosMobile).Government != null &&
                        !(m as BaseKhaerosMobile).Government.Deleted &&
                        (m as BaseKhaerosMobile).Government == m_Government)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(12)))
                        {
                            if (CustomGuildStone.IsGuildMember(m as PlayerMobile, g) && m_Government.AlliedGuilds.Contains(g))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                        else if (m is BaseKhaerosMobile)
                        {
                            if ((m as BaseKhaerosMobile).Government != null
                                && !(m as BaseKhaerosMobile).Government.Deleted
                                && m_Government.AlliedGuilds.Contains((m as BaseKhaerosMobile).Government))
                            {
                                OwnerNearby = true;
                                m.RevealingAction();
                                continue;
                            }
                        }
                    }

                    if (m is BaseCreature
                        && (m as BaseCreature).Controlled
                        && (m as BaseCreature).ControlMaster != null
                        && CustomGuildStone.IsGuildMember((m as BaseCreature).ControlMaster as PlayerMobile, m_Government)
                        && !(m as BaseCreature).IsDeadBondedPet
                        && !(m as BaseCreature).IsDeadPet)
                    {
                        OwnerNearby = true;
                        m.RevealingAction();
                        continue;
                    }

                    if (OwnerNearby)
                        continue;
                }
                eable.Free();
            }
            return OwnerNearby;
        }

        public bool SearchForClaimants()
        {
            if (SearchForOwners())
                return false;

            if(m_Claimant == null || m_Claimant.Deleted)
                return false;

            if(m_ClaimantGovernment == null || m_ClaimantGovernment.Deleted)
                return false;

            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 14);
            bool ClaimantNearby = false;

            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(1)) && m_ClaimantGovernment.Members.Contains(m))
                {
                    ClaimantNearby = true;
                    m.RevealingAction();
                    continue;
                }

                if (m is BaseCreature && (m as BaseCreature).Government == m_Government)
                {
                    ClaimantNearby = true;
                    m.RevealingAction();
                    continue;
                }

                foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                {
                    if (m is PlayerMobile && (DateTime.Now > (m as PlayerMobile).LastDeath + TimeSpan.FromHours(12)))
                    {
                        if (CustomGuildStone.IsGuildMember(m as PlayerMobile, g) && g.AlliedGuilds.Contains(g))
                        {
                            ClaimantNearby = true;
                            m.RevealingAction();
                            continue;
                        }
                    }
                    else if (m is BaseKhaerosMobile)
                    {
                        if ((m as BaseKhaerosMobile).Government != null
                            && !(m as BaseKhaerosMobile).Government.Deleted
                            && g.AlliedGuilds.Contains((m as BaseKhaerosMobile).Government))
                        {
                            ClaimantNearby = true;
                            m.RevealingAction();
                            continue;
                        }
                    }
                }

                if (m is BaseCreature
                    && (m as BaseCreature).Controlled
                    && (m as BaseCreature).ControlMaster != null
                    && CustomGuildStone.IsGuildMember((m as BaseCreature).ControlMaster as PlayerMobile, m_ClaimantGovernment)
                    && !(m as BaseCreature).IsDeadBondedPet
                    && !(m as BaseCreature).IsDeadPet)
                {
                    ClaimantNearby = true;
                    m.RevealingAction();
                    continue;
                }

                if (ClaimantNearby)
                    break;
            }
            eable.Free();
            if (!ClaimantNearby && (m_ClaimTimer != null))
            {
                m_Claimant.SendMessage("You failed to capture the " + this.Resource.ToString() + "!");
                m_ClaimantGovernment = null;
                m_Claimant = null;
                m_ClaimTimer.Stop();
            }

            return ClaimantNearby;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                if ((m_Claimant != null && from == m_Claimant) || (m_ClaimantGovernment != null && m_ClaimantGovernment.Members.Contains(from)))
                {
                    from.SendMessage("This resource is already being claimed by " + m_ClaimantGovernment.Name.ToString() + ".");
                    return;
                }

                if (m_Owned && SearchForOwnersDC() && (Government != null && !Government.Members.Contains(from)))
                {
                    from.SendMessage("You cannot take control of this resource while its current owners are nearby.");
                    return;
                }
            }

            from.SendGump(new ResourceNodeGump((PlayerMobile)from, this));

            base.OnDoubleClick(from);
        }

        public override void OnDelete()
        {
            if ((m_ClaimTimer != null) && (m_ClaimTimer.Running))
            {
                m_ClaimTimer.Stop();
                m_ClaimTimer = null;
            }

            if (m_Timer != null && m_Timer.Running)
            {
                m_Timer.Stop();
                m_Timer = null;
            }

            base.OnDelete();
        }

        public ResourceNode(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);

            writer.Write((int)m_Resource);
            writer.Write((GovernmentEntity)m_Government);
            writer.Write((bool)m_Owned);
            writer.Write((DateTime)m_ProductionDate);
            writer.Write((int)m_ProductionRate);
            writer.Write((string)BaseName);
            writer.Write((PlayerMobile)m_Claimant);
            writer.Write((GovernmentEntity)m_ClaimantGovernment);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Resource = (ResourceType)reader.ReadInt();
            m_Government = (GovernmentEntity)reader.ReadItem();
            m_Owned = reader.ReadBool();
            m_ProductionDate = reader.ReadDateTime();
            m_ProductionRate = reader.ReadInt();
            BaseName = reader.ReadString();
            m_Claimant = (PlayerMobile)reader.ReadMobile();
            m_ClaimantGovernment = (GovernmentEntity)reader.ReadItem();

            if (version < 1)
                ProductionDate = DateTime.Now.AddDays(1);

            if (m_Owned)
            {
                m_Timer = new ResourceTimer(this);
                m_Timer.Start();
            }

            if (m_ClaimantGovernment != null && !m_ClaimantGovernment.Deleted)
            {
                m_ClaimTimer = new ClaimResourceTimer(this);
                m_ClaimTimer.Start();
            }
        }
    }

    public class ResourceTimer : Timer
    {
        private ResourceNode m_Node;
        private GovernmentEntity m_Government;

        public ResourceTimer(ResourceNode n) : base(TimeSpan.FromMinutes(1), TimeSpan.FromHours(1))
        {
            m_Node = n;
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (m_Node.ProductionDate <= DateTime.Now)
            {
                if (m_Node.SearchForOwnersRT())
                    m_Node.SendResource();
            }

            if (!m_Node.SearchForOwnersRT() && !m_Node.SearchForClaimants())
                m_Node.Lose();

            base.OnTick();
        }
    }

    public class ClaimResourceTimer : Timer
    {
        private ResourceNode m_Node;
        private DateTime m_StartTime;

        public ClaimResourceTimer(ResourceNode rn) : base(TimeSpan.Zero, TimeSpan.FromMinutes(1))
        {
            m_Node = rn;
            Priority = TimerPriority.FiveSeconds;

            m_StartTime = DateTime.Now;
        }

        protected override void OnTick()
        {
            if (m_Node == null || m_Node.Deleted)
                Stop();

            if (m_Node.SearchForClaimants())
            {
                if (m_Node.Owned)
                {
                    if (DateTime.Now >= m_StartTime + m_Node.ClaimTime)
                        m_Node.Capture();
                }
                else
                {
                    if (DateTime.Now >= m_StartTime + ( TimeSpan.FromMinutes ( m_Node.ClaimTime.Minutes / 2 ) ))
                        m_Node.Capture();
                }
            }
            else
            {
                m_Node.Claimant = null;
                m_Node.ClaimantGovernment = null;
                this.Stop();
            }

            base.OnTick();
        }
    }
}