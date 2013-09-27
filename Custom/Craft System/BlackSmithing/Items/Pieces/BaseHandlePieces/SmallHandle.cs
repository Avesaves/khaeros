using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
    public class SmallHandle : BaseHandlePiece
    {
        [Constructable]
        public SmallHandle()
            : base(4269)
        {
            Weight = 3.0;
            Name = "A Small Handle";
            ResourceAmount = 3;
        }

        public SmallHandle(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1)
                from.Target = new SmallHandleTarget(this);
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


    public class SmallHandleTarget : Target
    {
        private SmallHandle m_Hilt;
        private BaseAttackPiece m_Blade;

        public SmallHandleTarget(SmallHandle owner)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = owner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is BaseAttackPiece))
                return;

            m_Blade = targeted as BaseAttackPiece;

            if (m_Blade is CurvedBlade)
                from.Prompt = new SHCurvedBladePrompt(from, m_Hilt, (CurvedBlade)m_Blade);
            if (m_Blade is MediumBlade)
            {
                Kryss weapon = new Kryss();
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
            if (m_Blade is MaceHead)
                from.Prompt = new SHMaceHeadPrompt(from, m_Hilt, (MaceHead)m_Blade);
            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Western)
            {
                Tepatl weapon = new Tepatl();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.9 + m_Hilt.Durability * 0.1);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.9 + m_Hilt.Quality * 0.1);
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
            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Northern)
            {
                Lance weapon = new Lance();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.9 + m_Hilt.Durability * 0.1);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.9 + m_Hilt.Quality * 0.1);
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
            if (m_Blade is ShortBlade)
                from.Prompt = new SHShortBladePrompt(from, m_Hilt, (ShortBlade)m_Blade);
        }
    }

#region CurvedBlade

    public class SHCurvedBladePrompt : Prompt
    {
        private SmallHandle m_Hilt;
        private CurvedBlade m_Blade;

        public SHCurvedBladePrompt(Mobile from, SmallHandle hilt, CurvedBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            if (((PlayerMobile)from).Nation != Nation.Southern && ((PlayerMobile)from).Nation != Nation.Haluaroc && ((PlayerMobile)from).Nation != Nation.Mhordul)
                OnResponse(from, "1");
            else
            {
                from.SendMessage("Please type in the code for the type of weapon you would like to make:");
                from.SendMessage(" 1 - Machete ");

                if (((PlayerMobile)from).Nation == Nation.Southern)
                {
                    from.SendMessage(" 2 - billhook ");
                    from.SendMessage(" 3 - falcata ");
                }
                if (((PlayerMobile)from).Nation == Nation.Haluaroc)
                {
                    from.SendMessage(" 4 - shamshir ");
                    from.SendMessage(" 5 - shamshir ");
                }
                if (((PlayerMobile)from).Nation == Nation.Mhordul)
                    from.SendMessage(" 6 - crescent blade ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 6 || ((index == 2 || index == 3) && ((PlayerMobile)from).Nation != Nation.Southern) || ((index == 4 || index == 5) && ((PlayerMobile)from).Nation != Nation.Haluaroc) || (index == 6 && ((PlayerMobile)from).Nation != Nation.Mhordul))
                {
                    from.SendMessage("Invalide code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeMachete(from); break;
                    case 2: MakeBillhook(from); break;
                    case 3: MakeFalcata(from); break;
                    case 4: MakeSabre(from); break;
                    case 5: MakeShamshir(from); break;
                    case 6: from.SendMessage("Add another curved blade"); from.Target = new CrescentBladeTarget(m_Hilt, m_Blade); break;
                }
            }
        }

        public void MakeMachete(Mobile from)
        {
            Machete weapon = new Machete();
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

        public void MakeBillhook(Mobile from)
        {
            Billhook weapon = new Billhook();
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

        public void MakeFalcata(Mobile from)
        {
            Falcata weapon = new Falcata();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.9 + m_Hilt.Durability * 0.1);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.9 + m_Hilt.Quality * 0.1);
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

        public void MakeSabre(Mobile from)
        {
            Sabre weapon = new Sabre();
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

        public void MakeShamshir(Mobile from)
        {
            Shamshir weapon = new Shamshir();
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

    public class CrescentBladeTarget : Target
    {
        private SmallHandle m_Hilt;
        private CurvedBlade m_Blade;

        public CrescentBladeTarget(SmallHandle hilt, CurvedBlade blade)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = hilt;
            m_Blade = blade;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is CurvedBlade) || targeted == m_Blade)
                return;

            CurvedBlade xtraBlade = targeted as CurvedBlade;
            CrescentBlade weapon = new CrescentBlade();
            weapon.NewCrafting = true;
            int quality = (int)(m_Blade.Damage * 0.6 + xtraBlade.Damage * 0.6 + m_Hilt.Damage * 0.9);
            weapon.QualityDamage = quality;
            quality = (int)(m_Blade.Speed * 0.6 + xtraBlade.Speed * 0.6 + m_Hilt.Speed  * 0.9);
            weapon.QualitySpeed = quality;
            quality = (int)(m_Blade.Attack * 0.7 + xtraBlade.Attack * 0.7 + m_Hilt.Attack * 0.9);
            weapon.QualityAccuracy =  quality;
            quality = (int)(m_Hilt.Defense * 0.8 + m_Blade.Defense * 0.5 + xtraBlade.Defense * 0.5);
            weapon.QualityDefense = quality;
            weapon.Resource = m_Hilt.Resource;
            quality = (int)(m_Blade.Durability * 0.4 + xtraBlade.Durability * 0.4 + m_Hilt.Durability * 0.2);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.4 + xtraBlade.Quality * 0.4 + m_Hilt.Quality * 0.2);
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
            weapon.AddItem(xtraBlade);
        }
    }

#endregion CurvedBlade

#region MaceHead

    public class SHMaceHeadPrompt : Prompt
    {
        private SmallHandle m_Hilt;
        private MaceHead m_Blade;

        public SHMaceHeadPrompt(Mobile from, SmallHandle hilt, MaceHead blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the mace you would like to make:");
            from.SendMessage(" 1 - Flanged Mace ");
            from.SendMessage(" 2 - Mace ");
            from.SendMessage(" 3 - War Mace");

            if (((PlayerMobile)from).Nation == Nation.Western)
            {
                from.SendMessage(" 4 - primitive mace ");
                from.SendMessage(" 5 - spiked mace ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 5 || ((index == 4 || index == 5) && ((PlayerMobile)from).Nation != Nation.Western))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeFlangedMace(from); break;
                    case 2: MakeMace(from); break;
                    case 3: MakeWarMace(from); break;
                    case 4: MakeWesternMace(from); break;
                    case 5: MakeSpikedMace(from); break;
                }
            }
        }

        public void MakeFlangedMace(Mobile from)
        {
            FlangedMace weapon = new FlangedMace();
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

        public void MakeMace(Mobile from)
        {
            Mace weapon = new Mace();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.45 + m_Hilt.Durability * 0.55);
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

        public void MakeWarMace(Mobile from)
        {
            WarMace weapon = new WarMace();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.55 + m_Hilt.Durability * 0.45);
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

        public void MakeWesternMace(Mobile from)
        {
            WesternMace weapon = new WesternMace();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
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

        public void MakeSpikedMace(Mobile from)
        {
            SpikedMace weapon = new SpikedMace();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.9 + m_Hilt.Durability * 0.1);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.9 + m_Hilt.Quality * 0.1);
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

#endregion MaceHead

#region ShortBlade

    public class SHShortBladePrompt : Prompt
    {
        private SmallHandle m_Hilt;
        private ShortBlade m_Blade;

        public SHShortBladePrompt(Mobile from, SmallHandle hilt, ShortBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the weapon you would like to make");
            from.SendMessage(" 1 - Butcher Knife ");
            from.SendMessage(" 2 - Cleaver ");
            from.SendMessage(" 3 - Skinning Knife ");

            if (((PlayerMobile)from).Nation == Nation.Haluaroc)
                from.SendMessage(" 4 - kukri ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text== null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 4 || (index == 4 && ((PlayerMobile)from).Nation != Nation.Haluaroc))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeButcherKnife(from); break;
                    case 2: MakeCleaver(from); break;
                    case 3: MakeSkinningKnife(from); break;
                    case 4: MakeKukri(from); break;
                }
            }
        }

        public void MakeButcherKnife(Mobile from)
        {
            ButcherKnife weapon = new ButcherKnife();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.75 + m_Hilt.Durability * 0.25);
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

        public void MakeCleaver(Mobile from)
        {
            Cleaver weapon = new Cleaver();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.95 + m_Hilt.Durability * 0.05);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.95 + m_Hilt.Quality * 0.05);
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

        public void MakeSkinningKnife(Mobile from)
        {
            SkinningKnife weapon = new SkinningKnife();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.9 + m_Hilt.Durability * 0.1);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.9 + m_Hilt.Quality * 0.1);
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

        public void MakeKukri(Mobile from)
        {
            Kukri weapon = new Kukri();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.8 + m_Hilt.Durability * 0.2);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
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

#endregion ShortBlade

}

