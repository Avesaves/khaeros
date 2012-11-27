using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public enum PrayerEffect
    {
        None,
        Strength,
        Dexterity,
        Intelligence,
        RawHitPoints,
        CurrentHitPoints,
        RawStamina,
        CurrentStamina,
        RawMana,
        CurrentMana,
        BluntResistance,
        PiercingResistance,
        SlashingResistance,
        ColdResistance,
        FireResistance,
        PoisonResistance,
        EnergyResistance,
        AttackChance,
        DefendChance
    }

    [PropertyObject]
    public class CustomFaithSpell : BaseCustomSpell
    {
        public static void Initialize()
        {
            CommandSystem.Register("ListFaithful", AccessLevel.GameMaster, new CommandEventHandler(ListFaithful_OnCommand));
        }

        [Usage("ListFaithful")]
        [Description("Lists all players who are both active and in possession of faith powers.")]
        private static void ListFaithful_OnCommand(CommandEventArgs e)
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile)
                {
                    PlayerMobile pm = m as PlayerMobile;
                    if (pm.Feats.GetFeatLevel(FeatList.Faith) > 0 || pm.CanBeFaithful)
                    {
                        if (pm.LastOnline < DateTime.Now - TimeSpan.FromDays(30))
                        {
                            e.Mobile.SendMessage(pm.RawName);
                            if (pm.RawName != pm.Name)
                            {
                                e.Mobile.SendMessage(pm.RawName + " is disguised as " + pm.Name + ".");
                            }
                        }
                    }
                }
            }
        }

        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override bool UsesFaith { get { return true; } }
        public override FeatList Feat { get { return FeatList.Faith; } }
        public override string Name
        {
            get 
            {
                if (String.IsNullOrEmpty(m_Name))
                    m_Name = "A Prayer";

                return m_Name; 
            }
        }
        public override int BaseCost { get { return ManaCost; } }
        public override int BaseDuration { get { return m_Duration; } }
        public override int TotalCost { get { return BaseCost; } }
        public override int TotalDuration { get { return BaseDuration; } }
        public override int BaseRange { get { return m_Range; } }
        public override SkillName GetSkillName
        {
            get
            {
                return SkillName.Faith;
            }
        }

        #region Variable Declaration; Get/Sets
        private string m_Name;
        private PrayerEffect m_Effect;
        private int m_Intensity;
        private int m_Duration;
        private int m_Range;
        private int m_Area;
        private int m_Repetitions;
        private int m_RepetitionDelay;
        private Type m_Component;
        private string m_Emote;
        private string m_Speech;
        private string m_Message;
        private int m_SoundID = -1;

        public PrayerEffect Effect { get { return m_Effect; } set { m_Effect = value; } }
        public int Intensity 
        { 
            get { return m_Intensity; } 
            set 
            { 
                m_Intensity = value;
                if (m_Intensity < 0)
                    m_Intensity = 0;
            } 
        }
        public int Duration
        {
            get { return m_Duration; }
            set
            {
                m_Duration = value;
                if (m_Duration < 0)
                    m_Duration = 0;
            }
        }
        public int Range 
        { 
            get { return m_Range; } 
            set 
            { 
                m_Range = value;
                if (m_Range < 1)
                    m_Range = 1;
            } 
        }
        public int Area 
        { 
            get { return m_Area; } 
            set 
            { 
                m_Area = value;
                if (m_Area < 0)
                    m_Area = 0;
            } 
        }
        public int Repetitions 
        { 
            get { return m_Repetitions; } 
            set 
            { 
                m_Repetitions = value;
                if (m_Repetitions < 0)
                    m_Repetitions = 0;
            } 
        }
        public int RepetitionDelay
        {
            get { return m_RepetitionDelay; }
            set
            {
                m_RepetitionDelay = value;
                if (m_RepetitionDelay < 0)
                    m_RepetitionDelay = 0;
            }
        }
        public Type Component { get { return m_Component; } set { m_Component = value; } }
        public string Emote 
        { 
            get 
            {
                if (String.IsNullOrEmpty(m_Emote))
                    return "utters a prayer to the gods";
                return m_Emote; 
            } 
            set 
            { 
                m_Emote = value;
                if (String.IsNullOrEmpty(m_Emote))
                    m_Emote = "";
            } 
        }
        public string Speech 
        { 
            get { return m_Speech; } 
            set 
            { 
                m_Speech = value;
                if (String.IsNullOrEmpty(m_Speech))
                    m_Speech = "";
            } 
        }
        public string Message 
        { 
            get { return m_Message; } 
            set 
            { 
                m_Message = value;
                if (String.IsNullOrEmpty(m_Message))
                    m_Message = "";
            } 
        }
        public int SoundID
        {
            get { return m_SoundID; }
            set { m_SoundID = value; }
        }
        #endregion

        public void SetName(string name)
        {
            m_Name = name;
        }

        public static List<Type> ValidComponents
        {
            get
            {
                List<Type> list = new List<Type>();
                list.Add(null);
                list.Add(typeof(Frankincense));
                list.Add(typeof(Myrrh));
                list.Add(typeof(Patchouli));
                list.Add(typeof(Saffron));
                list.Add(typeof(Sandelwood));
                list.Add(typeof(Dragonsblood));
                list.Add(typeof(Thyme));
                list.Add(typeof(Clove));
                list.Add(typeof(Opium));
                list.Add(typeof(Swampweed));
                list.Add(typeof(BanestoneAsh));
                list.Add(typeof(Bone));
                list.Add(typeof(Head));
                list.Add(typeof(HolyWater));
                return list;
            }
        }

        public int ManaCost
        {
            get
            {
                int effects = 0;
                double cost = 0;

                if ( Intensity > 0 )
                {
                    effects++;
                    cost += (m_Intensity * ((m_Intensity * 0.005)));
                }
                if ( Repetitions > 0 )
                {
                    effects++;
                    //cost += ( m_Intensity * ((m_Intensity * 0.005)) ) * (1.5 - (RepetitionDelay * 0.05));
                    cost += (m_Intensity * ((Repetitions * 0.010) * (1.5 - (RepetitionDelay * 0.01) )));
                }
                if (Range > 0)
                {
                    effects++;
                    cost += (Range * 2);
                }
                if (Duration > 0)
                {
                    effects++;
                    cost += (Duration * 0.1);
                }
                if (Area > 0)
                {
                    effects++;
                    cost += ((m_Intensity * ((m_Intensity * 0.005))) * (1 + (0.5 * Area)));
                }
                if (Component != null)
                {
                    cost -= (cost * 0.15);
                }

                return (int)(cost + (effects * effects));
            }
        }

        public int PowerBonus(PlayerMobile from)
        {
            int init = from.Feats.GetFeatLevel(FeatList.Faith) * from.Int;
            return (int)(m_Intensity * (init * 0.001));
        }

        public CustomFaithSpell()
            : this(null, 1)
        {

        }

        public CustomFaithSpell(Mobile caster, int featlevel)
            : base(caster, featlevel)
        {

        }

        public override void InitialAnimation()
        {
            Caster.RevealingAction();
            (Caster as IKhaerosMobile).Fizzled = false;

            if (!Caster.Mounted)
                Caster.Animate(17, 5, 1, true, false, 0);

            Caster.PlaySound(0x24A);

            if (!String.IsNullOrEmpty(m_Emote))
                Caster.DoSpeech("*" + m_Emote + "*", new int[] { -1 }, MessageType.Emote, Caster.EmoteHue);

            if (!String.IsNullOrEmpty(m_Speech))
                Caster.DoSpeech(m_Speech, new int[] { -1 }, MessageType.Regular, Caster.SpeechHue);

            if ((Caster as IKhaerosMobile).StunnedTimer == null)
            {
                XmlFreeze freeze = new XmlFreeze(1.0);
                freeze.Name = "icast";
                XmlAttach.AttachTo(Caster, freeze);
            }
        }

        public bool TryEffectTarget(PlayerMobile from, Mobile target, bool isAreaEffect)
        {
            if (from == null || from.Deleted || !from.Alive || from.IsDeadBondedPet)
                return false;
            if (target == null || target.Deleted || !target.Alive || target.IsDeadBondedPet)
                return false;

            if (isAreaEffect)
            {
                if (from.IsAllyOf(target))
                {
                    if (Repetitions > 0)
                    {
                        RecurrentPrayerTimer timer = new RecurrentPrayerTimer(from, target, this);
                        timer.Start();
                    }
                    return DoPrayerEffect(from, target);
                }
                else
                    return false;
            }
            else
            {
                if (target.InRange(target.Location, m_Range) && from.InLOS(target))
                {
                    SpellHelper.Turn(from, target);
                    if (Repetitions > 0)
                    {
                        RecurrentPrayerTimer timer = new RecurrentPrayerTimer(from, target, this);
                        timer.Start();
                    }
                    return DoPrayerEffect(from, target);
                }
                else
                {
                    from.SendMessage("You are too far away.");
                    return false;
                }
            }

            return false;
        }

        public bool DoPrayerEffect(PlayerMobile from, Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || target.IsDeadBondedPet)
                return false;
            if (from == null || from.Deleted)
                return false;

            if (!String.IsNullOrEmpty(m_Message))
                target.SendMessage(m_Message);
            target.SendSound(SoundID);
            int offset = m_Intensity + PowerBonus(from);
            switch (m_Effect)
            {
                case PrayerEffect.Strength:
                    {
                        #region Strength
                        
                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Str, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.Str, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Str, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;
                        #endregion
                    }
                case PrayerEffect.Dexterity:
                    {
                        #region Dexterity
                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Dex, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.Dex, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Dex, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;
                        #endregion
                    }
                case PrayerEffect.Intelligence:
                    {
                        #region Intelligence

                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Int, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.Int, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.Int, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;

                        #endregion
                    }
                case PrayerEffect.RawHitPoints:
                    {
                        #region RawHits

                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.HitsMax, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.HitsMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.HitsMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;

                        #endregion
                    }
                case PrayerEffect.CurrentHitPoints:
                    {
                        target.Hits += offset;
                        return true;
                    }
                case PrayerEffect.RawStamina:
                    {
                        #region RawStam

                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.StamMax, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.StamMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.StamMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;

                        #endregion
                    }
                case PrayerEffect.CurrentStamina:
                    {
                        target.Stam += offset;
                        return true;
                    }
                case PrayerEffect.RawMana:
                    {
                        #region RawMana

                        string name = String.Format("[Prayer] {0} Offset", m_Effect);

                        StatMod mod = target.GetStatMod(name);

                        //one is negative and the other is positive, so adding up
                        if (mod != null && ((mod.Offset <= 0 && offset > 0) || (offset < 0 && mod.Offset >= 0)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.ManaMax, name, mod.Offset + offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //nothing to replace, just adding
                        else if (mod == null)
                        {
                            target.AddStatMod(new StatMod(StatType.ManaMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }
                        //replacing the current mod with a larger one
                        else if (mod != null && ((mod.Offset <= 0 && offset < mod.Offset) || (mod.Offset >= 0 && mod.Offset < offset)))
                        {
                            target.RemoveStatMod(name);
                            target.AddStatMod(new StatMod(StatType.ManaMax, name, offset, TimeSpan.FromSeconds(m_Duration)));
                            return true;
                        }

                        return false;

                        #endregion
                    }
                case PrayerEffect.CurrentMana:
                    {
                        target.Mana += offset;
                        return true;
                    }
                case PrayerEffect.BluntResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Blunt, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.PiercingResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Piercing, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.SlashingResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Slashing, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.ColdResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Cold, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.FireResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Fire, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.PoisonResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Poison, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.EnergyResistance:
                    {
                        ResistancePrayerTimer timer = new ResistancePrayerTimer(target, ResistanceType.Energy, offset, m_Duration);
                        timer.Start();
                        return true;
                    }
                case PrayerEffect.AttackChance:
                    {
                        XmlAosAttribute att = new XmlAosAttribute(AosAttribute.AttackChance, offset, m_Duration);
                        att.Name = String.Format("[Prayer] {0} Offset", m_Effect);
                        XmlAttach.AttachTo(target, att);
                        return true;
                    }
                case PrayerEffect.DefendChance:
                    {
                        XmlAosAttribute att = new XmlAosAttribute(AosAttribute.DefendChance, offset, m_Duration);
                        att.Name = String.Format("[Prayer] {0} Offset", m_Effect);
                        XmlAttach.AttachTo(target, att);
                        return true;
                    }
                default: return false;
            }
        }

        public void FinishPray(Mobile from)
        {
            from.Mana -= ManaCost;
            IKhaerosMobile caster = from as IKhaerosMobile;
            caster.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds(Math.Max(2, Convert.ToInt32((60 - caster.Level) * 0.2)));

            if (caster.StunnedTimer == null)
            {
                XmlFreeze freeze = new XmlFreeze(1.0);
                freeze.Name = "fcast";
                XmlAttach.AttachTo(from, freeze);
            }

            if (Caster.AccessLevel < AccessLevel.GameMaster && caster is PlayerMobile)
                ((PlayerMobile)Caster).SpeedHack = false;
        }

        public static CustomFaithSpell DupeCustomFaithSpell(CustomFaithSpell spell)
        {
            CustomFaithSpell prayer = new CustomFaithSpell();

            prayer.SetName(spell.Name);
            prayer.Effect = spell.Effect;
            prayer.Intensity = spell.Intensity;
            prayer.Duration = spell.Duration;
            prayer.Range = spell.Range;
            prayer.Area = spell.Area;
            prayer.Repetitions = spell.Repetitions;
            prayer.RepetitionDelay = spell.RepetitionDelay;
            prayer.Component = spell.Component;
            prayer.Emote = spell.Emote;
            prayer.Speech = spell.Speech;
            prayer.Message = spell.Message;
            prayer.SoundID = spell.SoundID;

            return prayer;
        }

        public override void CastCallback()
        {
            if (BadCasting)
                return;

            if (Caster.Mana >= this.ManaCost)
            {
                Caster.Target = new PrayerTarget(Caster as PlayerMobile, this);
            }
            else
            {
                Caster.SendMessage("You lack the will to do this.");
            }
        }

        public bool CheckComponent()
        {
            if (m_Component != null)
            {
                if (Caster.Backpack == null || Caster.Backpack.Deleted)
                    return false;

                Item toConsume = null;
                foreach (Item i in Caster.Backpack.Items)
                {
                    if (i.GetType() == Component)
                    {
                        toConsume = i;
                        continue;
                    }
                }

                if (toConsume == null)
                {
                    Caster.SendMessage("You do not have the necessary components to do that.");
                    return false;
                }
                else
                {
                    toConsume.Consume();
                    return true;
                }
            }
            else
                return true;
        }

        private class ResistancePrayerTimer : Timer
        {
            private ResistanceMod m_ResistanceMod;
            private Mobile m_Target;

            public ResistancePrayerTimer(Mobile target, ResistanceType type, int intensity, int seconds)
                : base(TimeSpan.FromSeconds(seconds))
            {
                m_Target = target;
                m_ResistanceMod = new ResistanceMod(type, intensity);
                target.AddResistanceMod(m_ResistanceMod);
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted)
                {
                    Stop();
                    return;
                }

                m_Target.RemoveResistanceMod(m_ResistanceMod);
                base.OnTick();
            }
        }

        private class RecurrentPrayerTimer : Timer
        {
            private int m_RepeatCount = 0;
            private PlayerMobile m_Caster;
            private Mobile m_Target;
            private CustomFaithSpell m_Prayer;

            public RecurrentPrayerTimer(PlayerMobile from, Mobile to, CustomFaithSpell prayer)
                : base(TimeSpan.FromSeconds(prayer.RepetitionDelay), TimeSpan.FromSeconds(prayer.RepetitionDelay))
            {
                m_Caster = from;
                m_Target = to;
                m_Prayer = prayer;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted)
                {
                    Stop();
                    return;
                }
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive || m_Target.IsDeadBondedPet)
                {
                    Stop();
                    return;
                }

                if (m_Prayer == null)
                {
                    Stop();
                    return;
                }

                if (m_RepeatCount >= m_Prayer.Repetitions)
                {
                    Stop();
                    return;
                }

                m_Prayer.DoPrayerEffect(m_Caster, m_Target);
                m_RepeatCount++;

                base.OnTick();
            }
        }

        private class PrayerTarget : Target
        {
            private PlayerMobile m_Caster;
            private CustomFaithSpell m_Prayer;

            public PrayerTarget(PlayerMobile pm, CustomFaithSpell prayer)
                : base(prayer.Range, true, TargetFlags.None)
            {
                m_Prayer = prayer;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Prayer.BadCasting)
                    return;

                if (!m_Prayer.CheckComponent())
                    return;

                if (targeted == null)
                    return;

                if (from == null || from.Deleted || !from.Alive)
                    return;

                if (from == targeted)
                {
                    from.SendMessage("You cannot target yourself.");
                    return;
                }

                if (m_Prayer.Area > 0)
                {
                    if (targeted is Mobile)
                    {
                        Mobile targ = targeted as Mobile;
                        if (targ != null && !targ.Deleted && targ.Alive && from.CanSee(targ))
                        {
                            if (targ == m_Caster)
                            {
                                from.SendMessage("You cannot pray for yourself.");
                                return;
                            }

                            IPooledEnumerable eable = targ.Map.GetMobilesInRange(targ.Location, m_Prayer.Area);
                            foreach (Mobile m in eable)
                            {
                                if (m != from)
                                {
                                    m_Prayer.CheckCasting(m_Prayer.Caster.Skills[SkillName.Invocation].Base, m_Prayer.IsHarmful);
                                    m_Prayer.TryEffectTarget(from as PlayerMobile, m, true);
                                }
                            }
                            eable.Free();
                            m_Prayer.FinishPray(from);
                        }
                        else
                            from.SendMessage("You cannot pray for that.");
                    }
                    else if (targeted is StaticTarget)
                    {
                        StaticTarget targ = targeted as StaticTarget;
                        if (targ != null && from.CanSee(targ))
                        {
                            IPooledEnumerable eable = from.Map.GetMobilesInRange(targ.Location, m_Prayer.Area);
                            foreach (Mobile m in eable)
                            {
                                m_Prayer.CheckCasting(m_Prayer.Caster.Skills[SkillName.Invocation].Base, m_Prayer.IsHarmful);
                                m_Prayer.TryEffectTarget(from as PlayerMobile, m, true);
                            }
                            eable.Free();
                            m_Prayer.FinishPray(from);
                        }
                        else
                            from.SendMessage("You cannot target that.");
                    }
                }
                else
                {
                    if (targeted is Mobile)
                    {
                        Mobile targ = targeted as Mobile;

                        if (targ != null && !targ.Deleted && targ.Alive && from.CanSee(targ))
                        {
                            if (targ == m_Caster)
                            {
                                from.SendMessage("You cannot pray for yourself.");
                                return;
                            }
                            m_Prayer.CheckCasting(m_Prayer.Caster.Skills[SkillName.Invocation].Base, m_Prayer.IsHarmful);
                            m_Prayer.TryEffectTarget(from as PlayerMobile, targeted as Mobile, false);
                            m_Prayer.FinishPray(from);
                        }
                        else
                            from.SendMessage("You cannot target that.");
                    }
                }

                base.OnTarget(from, targeted);
            }
        }

        public static void Serialize(GenericWriter writer, CustomFaithSpell spell)
        {
            writer.Write((int)0); // version

            writer.Write((string)spell.Name);
            writer.Write((int)spell.Effect);
            writer.Write((int)spell.Intensity);
            writer.Write((int)spell.Duration);
            writer.Write((int)spell.Range);
            writer.Write((int)spell.Area);
            writer.Write((int)spell.Repetitions);
            writer.Write((int)spell.RepetitionDelay);
            if (spell.Component == null)
                writer.Write((bool)false);
            else
            {
                writer.Write((bool)true);
                writer.Write((string)spell.Component.GetType().ToString());
            }
            writer.Write((string)spell.Emote);
            writer.Write((string)spell.Speech);
            writer.Write((string)spell.Message);
            writer.Write((int)spell.SoundID);
        }

        public static void Deserialize(GenericReader reader, CustomFaithSpell spell)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        spell.SetName(reader.ReadString());
                        spell.Effect = (PrayerEffect)reader.ReadInt();
                        spell.Intensity = reader.ReadInt();
                        spell.Duration = reader.ReadInt();
                        spell.Range = reader.ReadInt();
                        spell.Area = reader.ReadInt();
                        spell.Repetitions = reader.ReadInt();
                        spell.RepetitionDelay = reader.ReadInt();
                        bool hasComponent = reader.ReadBool();
                        if (hasComponent)
                            spell.Component = ScriptCompiler.FindTypeByName(reader.ReadString());
                        else
                            spell.Component = null;
                        spell.Emote = reader.ReadString();
                        spell.Speech = reader.ReadString();
                        spell.Message = reader.ReadString();
                        spell.SoundID = reader.ReadInt();
                        break;
                    }
            }
        }
    }
}