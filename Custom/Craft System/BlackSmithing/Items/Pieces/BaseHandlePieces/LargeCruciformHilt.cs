using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Items
{
    public class LargeCruciformHilt : BaseHandlePiece
    {
        [Constructable]
        public LargeCruciformHilt()
            : base(4023)
        {
            Weight = 3.0;
            Name = "A Large Cruciform Hilt";
            ResourceAmount = 3;
        }

        public LargeCruciformHilt(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1 && ((PlayerMobile)from).Nation == Nation.Southern)
                from.Target = new LargeCruciformHiltTarget(this);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }


    public class LargeCruciformHiltTarget : Target
    {
        private LargeCruciformHilt m_Hilt;

        public LargeCruciformHiltTarget(LargeCruciformHilt owner) : base(2, false, TargetFlags.None)
        {
            m_Hilt = owner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is LongBlade))
                return;

            LongBlade blade = targeted as LongBlade;
            from.Prompt = new LargeCruciformHiltPrompt(from, m_Hilt, blade);
        }
    }

    public class LargeCruciformHiltPrompt : Prompt
    {
        private LargeCruciformHilt m_Hilt;
        private LongBlade m_Blade;

        public LargeCruciformHiltPrompt(Mobile from, LargeCruciformHilt hilt, LongBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type the code for the kind of sword you would like to make:");
            from.SendMessage(" 1 - claymore ");
            from.SendMessage(" 2 - arming sword ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index, quality = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 2)
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1:
                        MakeClaymore(from);
                        break;
                    case 2:
                        MakeArmingSword(from);
                        break;
                }
            }
        }

        public void MakeClaymore(Mobile from)
        {
            Claymore weapon = new Claymore();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.75 + m_Hilt.Durability * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.75 + m_Hilt.Quality * 0.25);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Antique;
            if (quality < 400)
                weapon.Quality = WeaponQuality.Extraordinary;
            if (quality < 350)
                weapon.Quality = WeaponQuality.Remarkable;
            if (quality < 300)
                weapon.Quality = WeaponQuality.Exceptional;
            if (quality < 250)
                weapon.Quality = WeaponQuality.Superior;
            if (quality < 200)
                weapon.Quality = WeaponQuality.Regular;
            if (quality < 150)
                weapon.Quality = WeaponQuality.Inferior;
            if (quality < 100)
                weapon.Quality = WeaponQuality.Low;
            if (quality < 50)
                weapon.Quality = WeaponQuality.Poor;
            weapon.BetaNerf = true;
            weapon.InvalidateProperties();
            from.AddToBackpack(weapon);
            weapon.AddItem(m_Blade);
            weapon.AddItem(m_Hilt);
        }

        public void MakeArmingSword(Mobile from)
        {
            ArmingSword weapon = new ArmingSword();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.85 + m_Hilt.Durability * 0.15);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.85 + m_Hilt.Quality * 0.15);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Antique;
            if (quality < 400)
                weapon.Quality = WeaponQuality.Extraordinary;
            if (quality < 350)
                weapon.Quality = WeaponQuality.Remarkable;
            if (quality < 300)
                weapon.Quality = WeaponQuality.Exceptional;
            if (quality < 250)
                weapon.Quality = WeaponQuality.Superior;
            if (quality < 200)
                weapon.Quality = WeaponQuality.Regular;
            if (quality < 150)
                weapon.Quality = WeaponQuality.Inferior;
            if (quality < 100)
                weapon.Quality = WeaponQuality.Low;
            if (quality < 50)
                weapon.Quality = WeaponQuality.Poor;
            weapon.BetaNerf = true;
            weapon.InvalidateProperties();
            from.AddToBackpack(weapon);
            weapon.AddItem(m_Blade);
            weapon.AddItem(m_Hilt);
        }
    }

}

