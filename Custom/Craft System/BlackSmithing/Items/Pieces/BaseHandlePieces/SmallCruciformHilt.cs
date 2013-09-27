using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
    public class SmallCruciformHilt : BaseHandlePiece
    {
        [Constructable]
        public SmallCruciformHilt()
            : base(4023)
        {
            Weight = 2.0;
            Name = "A Small Cruciform Hilt";
            ResourceAmount = 2;
        }

        public SmallCruciformHilt(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1)
                from.Target = new SmallCruciformHiltTarget(this);
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


    public class SmallCruciformHiltTarget : Target
    {
        private SmallCruciformHilt m_Hilt;
        private BaseAttackPiece m_Blade;

        public SmallCruciformHiltTarget(SmallCruciformHilt owner)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = owner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is BaseAttackPiece))
                return;

            m_Blade = targeted as BaseAttackPiece;

            if (m_Blade is ShortBlade)
            {
                Dagger weapon = new Dagger();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.6 + m_Hilt.Durability * 0.4);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
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
            if (m_Blade is CurvedBlade && ((PlayerMobile)from).Nation == Nation.Haluaroc)
                from.Prompt = new SCrucHCurvedBladePrompt(from, m_Hilt, (CurvedBlade)m_Blade);
            if (m_Blade is MediumBlade)
            {
                if (((PlayerMobile)from).Nation == Nation.Northern)
                    from.Prompt = new SCrucHMediumBladePrompt(from, m_Hilt, (MediumBlade)m_Blade);
                else
                {
                    Shortsword weapon = new Shortsword();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.65 + m_Hilt.Durability * 0.35);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.65 + m_Hilt.Quality * 0.35);
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
            if (m_Blade is LongBlade)
            {
                if (((PlayerMobile)from).Nation == Nation.Tirebladd)
                    from.Prompt = new SCrucHLongBladePrompt(from, m_Hilt, (LongBlade)m_Blade);
                else
                {
                    Longsword weapon = new Longsword();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.8 + m_Hilt.Durability * 0.2);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
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
    }

    #region LongBlade

    public class SCrucHLongBladePrompt : Prompt
    {
        private SmallCruciformHilt m_Hilt;
        private LongBlade m_Blade;

        public SCrucHLongBladePrompt(Mobile from, SmallCruciformHilt hilt, LongBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the weapon you would like to make:");
            from.SendMessage(" 1 - Longsword ");
            from.SendMessage(" 2 - broadsword ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 2)
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                if (index == 1)
                {
                    Longsword weapon = new Longsword();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.8 + m_Hilt.Durability * 0.2);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
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
                else if (index == 2)
                {
                    Broadsword weapon = new Broadsword();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.8 + m_Hilt.Durability * 0.2);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
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
    }

    #endregion LongBlade

    #region MediumBlade

    public class SCrucHMediumBladePrompt : Prompt
    {
        private SmallCruciformHilt m_Hilt;
        private MediumBlade m_Blade;

        public SCrucHMediumBladePrompt(Mobile from, SmallCruciformHilt hilt, MediumBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the sword you would like to make:");
            from.SendMessage(" 1 - Shortsword ");
            from.SendMessage(" 2 - gladius ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 2)
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                if (index == 1)
                {
                    Shortsword weapon = new Shortsword();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.65 + m_Hilt.Durability * 0.35);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.65 + m_Hilt.Quality * 0.35);
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

                if (index == 2)
                {
                    Gladius weapon = new Gladius();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.65 + m_Hilt.Durability * 0.35);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.65 + m_Hilt.Quality * 0.35);
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
    }

    #endregion MediumBlade

    #region CurvedBlade

    public class SCrucHCurvedBladePrompt : Prompt
    {
        private SmallCruciformHilt m_Hilt;
        private CurvedBlade m_Blade;

        public SCrucHCurvedBladePrompt(Mobile from, SmallCruciformHilt hilt, CurvedBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type the code for the type of sword you would like to make:");
            from.SendMessage(" 1 - khopesh ");
            from.SendMessage(" 2 - scimitar ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 2)
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                if (index == 1)
                {
                    Khopesh weapon = new Khopesh();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                    weapon.Resource = m_Blade.Resource;
                    int quality = (int)(m_Blade.Durability * 0.7 + m_Hilt.Durability * 0.3);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.7 + m_Hilt.Quality * 0.3);
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
                if (index == 2)
                {
                    Scimitar weapon = new Scimitar();
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
    }

#endregion CurvedBlade


}

