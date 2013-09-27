using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
    public class SmallCrescentHilt : BaseHandlePiece
    {
        [Constructable]
        public SmallCrescentHilt()
            : base(2424)
        {
            Weight = 2.0;
            Name = "A Small Crescent Hilt";
            ResourceAmount = 2;
        }

        public SmallCrescentHilt(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1)
                from.Target = new SmallCrescentHiltTarget(this);
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


    public class SmallCrescentHiltTarget : Target
    {
        private SmallCrescentHilt m_Hilt;
        private BaseAttackPiece m_Blade;

        public SmallCrescentHiltTarget(SmallCrescentHilt owner)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = owner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is BaseAttackPiece))
                return;

            m_Blade = targeted as BaseAttackPiece;

            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Western)// )
            {
                Flamberge weapon = new Flamberge();
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
            if (m_Blade is MediumBlade && ((PlayerMobile)from).Nation == Nation.Northern)
            {
                CavalrySword weapon = new CavalrySword();
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
            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Northern)
            {
                BastardSword weapon = new BastardSword();
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
            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Tirebladd)
            {
                HandAndAHalfSword weapon = new HandAndAHalfSword();
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
            if (m_Blade is CurvedBlade && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) >= 3 && from.Backpack.ConsumeTotal(typeof(Bone), 10))
            {
                BoneSword weapon = new BoneSword();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage + 2;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed + 4;
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
            if (m_Blade is ShortBlade && ((PlayerMobile)from).Nation == Nation.Western)
            {
                Kutar weapon = new Kutar();
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
        }
    }   
    

}

