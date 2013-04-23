using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.Items
{
    public class FootTrap : BaseTrap, IEasyCraft
    {
        private int m_SkillLevel;
        private int m_FeatLevel = 1;
        private Mobile m_Owner;
        private bool m_Armed;
        private bool m_InUse;

        private DateTime m_CreationDate;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Aging
        { get { return (DateTime.Now - m_CreationDate); } }

        public DateTime CreationDate
        {
            get { return m_CreationDate; }
            set { m_CreationDate = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SkillLevel
        {
            get { return m_SkillLevel; }
            set { m_SkillLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Armed
        {
            get { return m_Armed; }
            set { m_Armed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool InUse
        {
            get { return m_InUse; }
            set { m_InUse = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FeatLevel
        {
            get { return m_FeatLevel; }
            set { m_FeatLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        [Constructable]
        public FootTrap()
            : base(0x1374)
        {
            Name = "Foot Trap";
            Movable = true;
        }

        public override bool PassivelyTriggered { get { return false; } }
        public override TimeSpan PassiveTriggerDelay { get { return TimeSpan.Zero; } }
        public override int PassiveTriggerRange { get { return m_FeatLevel; } }
        public override TimeSpan ResetDelay { get { return TimeSpan.Zero; } }

        public override void OnTrigger(Mobile from)
        {
            TimeSpan maxage = new TimeSpan(0, m_FeatLevel * 10, 0);

            if (TimeSpan.Compare(maxage, this.Aging) < 0 && this.Armed)
            {
                this.Delete();
                return;
            }

            if (!from.Alive || from.Blessed || !this.Armed)
                return;

            if (from is IHuge) 

             {  
              from.Emote("*crushes a foot trap*");
              this.Delete();
              return; 
              }


            if (from is IIncorporeal) 

             {  
              from.Emote("*passes through the trap*");
              return; 
              }


            if (from is ITooSmart) 

             {  
              from.Emote("*steps over a trap*");
              return; 
              }

            PlayerMobile pm = from as PlayerMobile;

            int detecthidden = Convert.ToInt32(from.Skills[SkillName.DetectHidden].Fixed);

            int chancetodetect = detecthidden - m_SkillLevel;

            if (chancetodetect > (45 / m_FeatLevel) + 15)
            {
                chancetodetect = (45 / m_FeatLevel) + 15;
            }

            if (chancetodetect < (15 / m_FeatLevel))
            {
                chancetodetect = (15 / m_FeatLevel);
            }

            int attackroll = Utility.Random(100);

            if (chancetodetect < attackroll)
            {
                if (from is PlayerMobile)
                    if (((PlayerMobile)from).Evaded())
                    {
                        from.Emote("*evaded a trap*");
                        return;
                    }

                from.CantWalk = true;
                from.Emote("*got {0} foot stuck in a foot trap*", from.Female == true ? "her" : "his");

                if (from is PlayerMobile)
                {
                    Container pack = pm.Backpack;
                    pm.SendMessage(60, "You got caught in a foot trap. Double-click the trap that has been placed inside your pack to try to break free from it.");
                    this.Movable = true;
                    pack.DropItem(this);
                    this.Movable = false;
                    this.Visible = true;
                }

                else
                {
                    new FootTrapTimer(from, m_FeatLevel).Start();
                    this.Delete();
                }
            }
            from.PlaySound(0x387);
        }

        public class FootTrapTimer : Timer
        {
            private Mobile m_m;

            public FootTrapTimer(Mobile m, int featlevel)
                : base(TimeSpan.FromSeconds(featlevel * 15))
            {
                m_m = m;
            }

            protected override void OnTick()
            {
                m_m.CantWalk = false;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;
            Item weapon = pm.FindItemOnLayer(Layer.FirstValid) as Item;
            Item shield = pm.FindItemOnLayer(Layer.TwoHanded) as Item;

            if (from is PlayerMobile)
            {
                if (this.IsChildOf(pm.Backpack))
                {
                    if (weapon == null && shield == null)
                    {
                        if (from.Mounted && from.CantWalk == false)
                        {
                                from.SendMessage(60, "You can think of no possible way to arm this trap while mounted");
                            return;
                        }

                        if (!this.Armed && pm.Feats.GetFeatLevel(FeatList.NonLethalTraps) > 0)
                        {
                            if (!this.InUse)
                                new ArmTrapTimer(from, this, m_FeatLevel).Start();
                        }

                        else if (!this.InUse && !this.Movable)
                            new InUseTimer(from, this, m_FeatLevel).Start();

                        else
                        {
                            pm.SendMessage("You need at least one level of Non-Lethal Traps in order to arm this.");
                            return;
                        }
                    }

                    else
                        from.SendMessage(60, "You need to empty both your hands before trying to use this.");
                }

                else
                    from.SendMessage(60, "This must be in your backpack for you to use it.");
            }
        }

        public class ArmTrapTimer : Timer
        {
            private Mobile m_m;
            private FootTrap m_trap;
            private Point3D m_loc;

            public ArmTrapTimer(Mobile m, FootTrap trap, int featlevel)
                : base(TimeSpan.FromSeconds(6 / featlevel))
            {
                m_m = m;
                m_trap = trap;
                trap.InUse = true;
                m.RevealingAction();
                m_m.SendMessage(60, "You begin arming the foot trap.");
                m.Animate(32, 5, 1, true, false, 0);
                m_loc = m.Location;
                m_trap.SkillLevel = Convert.ToInt32(m_m.Skills[SkillName.ArmDisarmTraps].Fixed);
                m_trap.Owner = m_m;
                m_trap.Movable = false;
            }

            protected override void OnTick()
            {
                if (m_m != null && !m_m.Deleted && m_loc == m_m.Location && m_m.Alive)
                {
                    m_m.SendMessage(60, "You armed the foot trap.");
                    m_trap.InUse = false;
                    m_trap.Armed = true;
                    m_trap.DropToWorld(m_m, m_m.Location);
                    m_trap.Visible = false;
                    m_trap.CreationDate = DateTime.Now;
                }

                else
                {
                    m_m.SendMessage(60, "You failed to arm the trap.");
                    m_trap.InUse = false;
                    m_trap.Movable = true;
                }
            }
        }

        public class InUseTimer : Timer
        {
            private Mobile m_m;
            private FootTrap m_trap;

            public InUseTimer(Mobile m, FootTrap trap, int featlevel)
                : base(TimeSpan.FromSeconds(featlevel * 2))
            {
                m_m = m;
                m_trap = trap;
                trap.InUse = true;
                m.Emote("*tries to break free from the foot trap*");
            }

            protected override void OnTick()
            {
                CheckBreakFree(m_m, m_trap);
                m_trap.InUse = false;
            }
        }

        public static void CheckBreakFree(Mobile from, FootTrap trap)
        {
            int str = from.Str;

            int chancetobreakfree = str - trap.m_SkillLevel;

            if (chancetobreakfree > (100 - (25 * trap.m_FeatLevel)))
            {
                chancetobreakfree = (100 - (25 * trap.m_FeatLevel));
            }

            if (chancetobreakfree < (50 - (10 * trap.m_FeatLevel)))
            {
                chancetobreakfree = (50 - (10 * trap.m_FeatLevel));
            }

            int attackroll = Utility.Random(100);

            if (attackroll <= chancetobreakfree)
                SetFree(from, trap);

            else
                from.Emote("*failed to break free from the foot trap*");
        }

        public static void SetFree(Mobile from, FootTrap trap)
        {
            from.Emote("*broke free from the foot trap*");
            from.CantWalk = false;
            trap.Delete();
        }

        public static void CheckAge(FootTrap trap)
        {
            TimeSpan maxage = new TimeSpan(0, trap.m_FeatLevel, 0);

            if (TimeSpan.Compare(maxage, trap.Aging) < 0)
            {
                trap.Delete();
            }
        }

        public FootTrap(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((int)m_SkillLevel);
            writer.Write((int)m_FeatLevel);
            writer.Write((Mobile)m_Owner);
            writer.Write((bool)m_Armed);
            writer.Write((DateTime)m_CreationDate);
            writer.Write((bool)m_InUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_SkillLevel = reader.ReadInt();
            m_FeatLevel = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_Armed = reader.ReadBool();
            m_CreationDate = reader.ReadDateTime();
            m_InUse = reader.ReadBool();
        }
    }
}
