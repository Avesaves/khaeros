using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Khaeros.Scripts.Khaeros.Mobiles.Unique_Monsters
{
    [CorpseName("Shae's corpse.")]
    public class ShaeMob : BaseCreature
    {
        const int BaseDamage = 200;
        int numberOfKills = 0;

        public ShaeMob(Serial serial) : base(serial)
        {
        }
        
        [Constructable]
        public ShaeMob() : base(AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "SHAE";
            BodyValue = 61;
            BaseSoundID = 362;

            SetStr(350, 400);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(5500, 5500);

            SetDamage(BaseDamage, BaseDamage);

            SetDamageType(ResistanceType.Slashing, 100);

            SetResistance(ResistanceType.Blunt, 60, 65);
            SetResistance(ResistanceType.Piercing, 60, 60);
            SetResistance(ResistanceType.Slashing, 60, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 65, 70);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Invocation, 30.1, 40.0);
            SetSkill(SkillName.Magery, 30.1, 40.0);
            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.UnarmedFighting, 90.1, 92.5);
            SetSkill(SkillName.Macing, 90.1, 92.5);

            this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 50;
        }

        public int NumberOfKills { get { return numberOfKills; } set { numberOfKills = value; } }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.10 >= Utility.RandomDouble())
                Deafen(defender);
            if (0.10 >= Utility.RandomDouble())
                Mute(defender);
            if (0.10 >= Utility.RandomDouble())
                Blind(defender);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (willKill)
            {
                Hits += 250;
                NumberOfKills++;
            }

            base.OnDamage(amount, from, willKill);
        }

        protected override bool OnMove(Direction d)
        {
            ReduceDamageBasedOnNumberOfPlayersClose();

            return base.OnMove(d);
        }

        void ReduceDamageBasedOnNumberOfPlayersClose()
        {
            int numberOfEnemiesInRange = GetEnemiesInRange().Count;

            if (numberOfEnemiesInRange > 0)
                SetDamage(CalculateReducedDamage(numberOfEnemiesInRange), CalculateReducedDamage(numberOfEnemiesInRange));
            else
                SetDamage(BaseDamage, BaseDamage);
        }

        int CalculateReducedDamage(int numberOfEnemiesInRange)
        {
            int newDamage = BaseDamage - (numberOfEnemiesInRange * 25);

            if (newDamage < 25)
                newDamage = 25;

            return newDamage;
        }

        List<Mobile> GetEnemiesInRange()
        {
            List<Mobile> list = new List<Mobile>();

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m.Player)
                    list.Add(m);
            }

            return list;
        }

        void Blind(Mobile defender)
        {
            IKhaerosMobile defplayer = defender as IKhaerosMobile;

            if (defplayer.BlindnessTimer != null)
            {
                defplayer.BlindnessTimer.Stop();
            }

            defplayer.BlindnessTimer = new ShaeMobBlindnessTimer(defender, 10, "You feel as if something is very wrong ...");
            defplayer.BlindnessTimer.Start();
        }

        void Mute(Mobile defender)
        {
            IKhaerosMobile defplayer = defender as IKhaerosMobile;

            if (defplayer.MutenessTimer != null)
            {
                defplayer.MutenessTimer.Stop();
            }

            defplayer.MutenessTimer = new ShaeMobBlindnessTimer(defender, 30, "You feel as if something is very wrong ...");
            defplayer.MutenessTimer.Start();
        }

        void Deafen(Mobile defender)
        {
            IKhaerosMobile defplayer = defender as IKhaerosMobile;

            if (defplayer.DeafnessTimer != null)
            {
                defplayer.DeafnessTimer.Stop();
            }

            defplayer.DeafnessTimer = new ShaeMobBlindnessTimer(defender, 30, "You feel as if something is very wrong ...");
            defplayer.DeafnessTimer.Start();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(NumberOfKills);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            NumberOfKills = reader.ReadInt();
        }

        public class ShaeMobBlindnessTimer : Timer
        {
            private Mobile target;

            public ShaeMobBlindnessTimer(Mobile target, double duration, string message)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
                this.target.SendMessage(60, message);
            }

            public ShaeMobBlindnessTimer(Mobile target, double duration)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
            }

            protected override void OnTick()
            {
                if (this.target == null)
                    return;

                ((IKhaerosMobile)this.target).BlindnessTimer = null;
            }
        }

        public class ShaeMobDeafnessTimer : Timer
        {
            private Mobile target;

            public ShaeMobDeafnessTimer(Mobile target, double duration, string message)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
                this.target.SendMessage(60, message);
            }

            public ShaeMobDeafnessTimer(Mobile target, double duration)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
            }

            protected override void OnTick()
            {
                if (this.target == null)
                    return;

                ((IKhaerosMobile)this.target).StunnedTimer = null;
            }
        }

        public class ShaeMobMutenessTimer : Timer
        {
            private Mobile target;

            public ShaeMobMutenessTimer(Mobile target, double duration, string message)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
                this.target.SendMessage(60, message);
            }

            public ShaeMobMutenessTimer(Mobile target, double duration)
                : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
            }

            protected override void OnTick()
            {
                if (this.target == null)
                    return;

                ((IKhaerosMobile)this.target).MutenessTimer = null;
            }
        }
    }
}
