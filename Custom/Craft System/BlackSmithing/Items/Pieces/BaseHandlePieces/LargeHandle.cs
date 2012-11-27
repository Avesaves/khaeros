using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Items
{
    public class LargeHandle : BaseHandlePiece
    {
        [Constructable]
        public LargeHandle()
            : base(4269)
        {
            Weight = 4.0;
            Name = "A Large Handle";
            ResourceAmount = 4;
        }

        public LargeHandle(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1)
                from.Target = new LargeHandleTarget(this);
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


    public class LargeHandleTarget : Target
    {
        private LargeHandle m_Hilt;
        private BaseAttackPiece m_Blade;

        public LargeHandleTarget(LargeHandle owner)
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
                from.Prompt = new LHCurvedBladePrompt(from, m_Hilt, (CurvedBlade)m_Blade);
            else if (m_Blade is HammerHead)
                from.Prompt = new LH_HammerHeadPrompt(from, m_Hilt, (HammerHead)m_Blade);
            else if (m_Blade is DualAxeHead)
                from.Prompt = new LHDualAxeHeadPrompt(from, m_Hilt, (DualAxeHead)m_Blade);
            else if (m_Blade is AxeHead)
                from.Prompt = new LHAxeHeadPrompt(from, m_Hilt, (AxeHead)m_Blade);
            else if (m_Blade is MaceHead)
                from.Prompt = new LHMaceHeadPrompt(from, m_Hilt, (MaceHead)m_Blade);
            else if (m_Blade is ShortBlade && ((PlayerMobile)from).Nation == Nation.Mhordul)
            {
                MhordulMace weapon = new MhordulMace();
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
                quality = (int)(m_Blade.Quality * 0.55 + m_Hilt.Quality * 0.45);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Illustrious;
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

    #region CurvedBlade

    public class LHCurvedBladePrompt : Prompt
    {
        private LargeHandle m_Hilt;
        private CurvedBlade m_Blade;

        public LHCurvedBladePrompt(Mobile from, LargeHandle hilt, CurvedBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            if (((PlayerMobile)from).Nation != Nation.Azhuran && ((PlayerMobile)from).Nation != Nation.Khemetar && ((PlayerMobile)from).Nation != Nation.Mhordul)
                OnResponse(from, "1");
            else
            {
                from.SendMessage("Please type the code for the kind of weapon you would like to make:");
                from.SendMessage(" 1 - Hand Scythe ");

                if (((PlayerMobile)from).Nation == Nation.Azhuran)
                    from.SendMessage(" 2 - Azhuran Hooked Club ");
                if (((PlayerMobile)from).Nation == Nation.Khemetar)
                {
                    from.SendMessage(" 3 - Khemetar Heavy Khopesh ");
                    from.SendMessage(" 4 - Khemetar Falchion ");
                    from.SendMessage(" 5 - Khemetar Large Crescent Sword ");
                }
                if (((PlayerMobile)from).Nation == Nation.Mhordul)
                    from.SendMessage(" 6 - Mhordul Warfork ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 6 || (index == 2 && ((PlayerMobile)from).Nation != Nation.Azhuran) || ((index > 2 && index < 6) && ((PlayerMobile)from).Nation != Nation.Khemetar) || (index == 6 && ((PlayerMobile)from).Nation != Nation.Mhordul))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1:
                        MakeHandScythe(from);
                        break;
                    case 2:
                        MakeAzhuranHookedClub(from);
                        break;
                    case 3:
                        MakeKhemetarHeavyKhopesh(from);
                        break;
                    case 4:
                        MakeKhemetarFalchion(from);
                        break;
                    case 5:
                        MakeKhemetarLargeCrescentSword(from);
                        break;
                    case 6:
                        from.SendMessage("Add another curved blade");
                        from.Target = new MhordulWarForkTarget(m_Hilt, m_Blade);
                        break;
                }
            }
        }

        public void MakeHandScythe(Mobile from)
        {
            HandScythe weapon = new HandScythe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeAzhuranHookedClub(Mobile from)
        {
            AzhuranHookedClub weapon = new AzhuranHookedClub();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.5 + m_Hilt.Durability * 0.5);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.5 + m_Hilt.Quality * 0.5);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeKhemetarHeavyKhopesh(Mobile from)
        {
            KhemetarHeavyKhopesh weapon = new KhemetarHeavyKhopesh();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeKhemetarFalchion(Mobile from)
        {
            KhemetarFalchion weapon = new KhemetarFalchion();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeKhemetarLargeCrescentSword(Mobile from)
        {
            KhemetarLargeCrescentSword weapon = new KhemetarLargeCrescentSword();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

    public class MhordulWarForkTarget : Target
    {
        private LargeHandle m_Hilt;
        private CurvedBlade m_Blade;

        public MhordulWarForkTarget(LargeHandle hilt, CurvedBlade blade)
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

            MhordulWarFork weapon = new MhordulWarFork();
            weapon.NewCrafting = true;
            int quality = (int)(m_Blade.Damage * 0.55 + xtraBlade.Damage * 0.55);
            weapon.QualityDamage = m_Hilt.Damage + quality;
            quality = (int)(m_Blade.Speed * 0.55 + xtraBlade.Speed * 0.55);
            weapon.QualitySpeed = m_Hilt.Speed + quality;
            quality = (int)(m_Blade.Attack * 0.6 + xtraBlade.Attack * 0.6);
            weapon.QualityAccuracy = m_Hilt.Attack + quality;
            quality = (int)(m_Blade.Defense * 0.5 + xtraBlade.Defense * 0.5);
            weapon.QualityDefense = m_Hilt.Defense + quality;
            weapon.Resource = m_Hilt.Resource;
            quality = (int)(m_Blade.Durability * 0.25 + xtraBlade.Durability * 0.25 + m_Hilt.Durability * 0.5);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.35 + xtraBlade.Quality * 0.35 + m_Hilt.Quality * 0.3);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

    #region HammerHead

    public class LH_HammerHeadPrompt : Prompt
    {
        private LargeHandle m_Hilt;
        private HammerHead m_Blade;

        public LH_HammerHeadPrompt(Mobile from, LargeHandle hilt, HammerHead blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type the code for the kind of weapon you would like to make:");
            from.SendMessage(" 1 - Hammer Pick ");
            from.SendMessage(" 2 - Light Hammer ");
            from.SendMessage(" 3 - War Hammer ");

            if (((PlayerMobile)from).Nation == Nation.Alyrian)
                from.SendMessage(" 4 - Alyrian BattleHammer ");
            if (((PlayerMobile)from).Nation == Nation.Tyrean)
                from.SendMessage(" 5 - Tyrean BattleHammer ");
            if (((PlayerMobile)from).Nation == Nation.Vhalurian)
            {
                from.SendMessage(" 6 - Vhalurian Heavy Maul ");
                from.SendMessage(" 7 - Vhalurian Maul ");
				from.SendMessage(" 8 - Vhalurian War Hammer ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 8 || (index == 4 && ((PlayerMobile)from).Nation != Nation.Alyrian) || (index == 5 && ((PlayerMobile)from).Nation != Nation.Tyrean) || ((index > 5 && index < 9) && ((PlayerMobile)from).Nation != Nation.Vhalurian))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeHammerPick(from); break;
                    case 2: MakeLightHammer(from); break;
                    case 3: MakeWarHammer(from); break;
                    case 4:
                        MakeAlyrianBattleHammer(from);
                        break;
                    case 5:
                        MakeTyreanBattleHammer(from);
                        break;
                    case 6: goto case 7;                       
                    case 7:
                        MakeVhalurianMaul(from, index);
                        break;
					case 8: MakeVhalurianWarHammer(from); break;
                }
            }
        }
		
		public void MakeVhalurianWarHammer(Mobile from)
		{
			VhalurianWarHammer weapon = new VhalurianWarHammer();
			weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeHammerPick(Mobile from)
        {
            HammerPick weapon = new HammerPick();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeLightHammer(Mobile from)
        {
            LightHammer weapon = new LightHammer();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeWarHammer(Mobile from)
        {
            WarHammer weapon = new WarHammer();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeAlyrianBattleHammer(Mobile from)
        {
            AlyrianBattleHammer weapon = new AlyrianBattleHammer();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.4 + m_Hilt.Durability * 0.6);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanBattleHammer(Mobile from)
        {
            TyreanBattleHammer weapon = new TyreanBattleHammer();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + m_Hilt.Quality * 0.2);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeVhalurianMaul(Mobile from, int index)
        {
            if (index == 3)
            {
                VhalurianHeavyMaul weapon = new VhalurianHeavyMaul();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
                weapon.Resource = m_Hilt.Resource;
                int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.Durability * 0.7);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.7 + m_Hilt.Quality * 0.3);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Illustrious;
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
            else
            {
                VhalurianMaul weapon = new VhalurianMaul();
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
                quality = (int)(m_Blade.Quality * 0.55 + m_Hilt.Quality * 0.45);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Illustrious;
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

    #endregion HammerHead

    #region MaceHead

    public class LHMaceHeadPrompt : Prompt
    {
        private LargeHandle m_Hilt;
        private MaceHead m_Blade;

        public LHMaceHeadPrompt(Mobile from, LargeHandle hilt, MaceHead blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            if (((PlayerMobile)from).Nation != Nation.Khemetar && ((PlayerMobile)from).Nation != Nation.Tyrean && ((PlayerMobile)from).Nation != Nation.Vhalurian)
                OnResponse(from, "1");
            else
            {
                from.SendMessage("Please type the code for the kind of mace would you like to make:");
                from.SendMessage(" 1 - Maul ");

                if (((PlayerMobile)from).Nation == Nation.Khemetar)
                    from.SendMessage(" 2 - Khemetar War Mace ");
                if (((PlayerMobile)from).Nation == Nation.Tyrean)
                    from.SendMessage(" 3 - Tyrean War Mace ");
                if (((PlayerMobile)from).Nation == Nation.Vhalurian)
                    from.SendMessage(" 4 - Vhalurian Mace ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 4 || (index == 2 && ((PlayerMobile)from).Nation != Nation.Khemetar) || (index == 3 && ((PlayerMobile)from).Nation != Nation.Tyrean) || (index == 4 && ((PlayerMobile)from).Nation != Nation.Vhalurian))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeMaul(from); break;
                    case 2: MakeKhemetarWarMace(from); break;
                    case 3: MakeTyreanWarMace(from); break;
                    case 4: MakeVhalurianMace(from); break;
                }
            }
        }

        public void MakeMaul(Mobile from)
        {
            Maul weapon = new Maul();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + m_Hilt.Quality * 0.3);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeKhemetarWarMace(Mobile from)
        {
            KhemetarWarMace weapon = new KhemetarWarMace();
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
            quality = (int)(m_Blade.Quality * 0.55 + m_Hilt.Quality * 0.45);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanWarMace(Mobile from)
        {
            TyreanWarMace weapon = new TyreanWarMace();
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
            quality = (int)(m_Blade.Quality * 0.55 + m_Hilt.Quality * 0.45);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeVhalurianMace(Mobile from)
        {
            VhalurianMace weapon = new VhalurianMace();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.15 + m_Hilt.Durability * 0.85);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + m_Hilt.Quality * 0.3);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

    #region DualAxeHead

    public class LHDualAxeHeadPrompt : Prompt
    {
        private LargeHandle m_Hilt;
        private DualAxeHead m_Blade;

        public LHDualAxeHeadPrompt(Mobile from, LargeHandle hilt, DualAxeHead blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type the code for the kind of axe would you like to make:");
            from.SendMessage(" 1 - Large Battle Axe ");
            from.SendMessage(" 2 - Two-Handed Axe ");
            

            if (((PlayerMobile)from).Nation == Nation.Alyrian)
                from.SendMessage(" 3 - Alyrian Two-Handed Axe ");
            if (((PlayerMobile)from).Nation == Nation.Khemetar)
                from.SendMessage(" 4 - Khemetar Axe ");
            if (((PlayerMobile)from).Nation == Nation.Mhordul)
            {
                from.SendMessage(" 5 - Mhordul Axe ");
                from.SendMessage(" 6 - Mhordul Heavy Battle Axe ");
            }
            if (((PlayerMobile)from).Nation == Nation.Tyrean)
            {
                from.SendMessage(" 7 - Tyrean Winged Axe ");
                from.SendMessage(" 8 - Tyrean Double Axe ");
            }
            
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 8 || (index == 3 && ((PlayerMobile)from).Nation != Nation.Alyrian) || (index == 4 && ((PlayerMobile)from).Nation != Nation.Khemetar) || ((index == 5 || index == 6) && ((PlayerMobile)from).Nation != Nation.Mhordul) || ((index == 8 || index == 7) && ((PlayerMobile)from).Nation != Nation.Tyrean) )
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeLargeBattleAxe(from); break;
                    case 2: MakeTwoHandedAxe(from); break;
                    case 3: MakeAlyrianTwoHandedAxe(from); break;
                    case 4: MakeKhemetarAxe(from); break;
                    case 5: MakeMhordulAxe(from); break;
                    case 6: MakeMhordulHeavyBattleAxe(from); break;
                    case 7: MakeTyreanWingedAxe(from); break;
                    case 8: MakeTyreanDoubleAxe(from); break;                    
                }
            }
        }

        public void MakeLargeBattleAxe(Mobile from)
        {
            LargeBattleAxe weapon = new LargeBattleAxe();
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
            quality = (int)(m_Blade.Quality * 0.55 + m_Hilt.Quality * 0.45);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTwoHandedAxe(Mobile from)
        {
            TwoHandedAxe weapon = new TwoHandedAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeAlyrianTwoHandedAxe(Mobile from)
        {
            AlyrianTwoHandedAxe weapon = new AlyrianTwoHandedAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeKhemetarAxe(Mobile from)
        {
            KhemetarAxe weapon = new KhemetarAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeMhordulAxe(Mobile from)
        {
            MhordulAxe weapon = new MhordulAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeMhordulHeavyBattleAxe(Mobile from)
        {
            MhordulHeavyBattleAxe weapon = new MhordulHeavyBattleAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanWingedAxe(Mobile from)
        {
            TyreanWingedAxe weapon = new TyreanWingedAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanDoubleAxe(Mobile from)
        {
            TyreanDoubleAxe weapon = new TyreanDoubleAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.Durability * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

#endregion DualAxeHead

    #region AxeHead

    public class LHAxeHeadPrompt : Prompt
    {
        private LargeHandle m_Hilt;
        private AxeHead m_Blade;

        public LHAxeHeadPrompt(Mobile from, LargeHandle hilt, AxeHead blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type the code for the kind of axe would you like to make:");
            from.SendMessage(" 1 - Axe ");
            from.SendMessage(" 2 - Battle Axe ");
            from.SendMessage(" 3 - War Axe ");

            if (((PlayerMobile)from).Nation == Nation.Azhuran)
                from.SendMessage(" 4 - Azhuran Axe ");
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) > 2)
                from.SendMessage(" 5 - Mhordul Bone Axe - (Requires 10 bones) ");
            if (((PlayerMobile)from).Nation == Nation.Tyrean)
            {
                from.SendMessage(" 6 - Tyrean Ornate Axe ");
                from.SendMessage(" 7 - Tyrean Throwing Axe ");
                from.SendMessage(" 8 - Tyrean War Axe ");
            }
            if (((PlayerMobile)from).Nation == Nation.Vhalurian)
                from.SendMessage(" 9 - Vhalurian War Axe ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 9 || (index == 4 && ((PlayerMobile)from).Nation != Nation.Azhuran) || (index == 5 && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) < 3) || ((index > 5 && index < 9) && ((PlayerMobile)from).Nation != Nation.Tyrean) || (index == 9 && ((PlayerMobile)from).Nation != Nation.Vhalurian))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeAxe(from); break;
                    case 2: MakeBattleAxe(from); break;
                    case 3: MakeWarAxe(from); break;
                    case 4: MakeAzhuranAxe(from); break;
                    case 5: MakeMhordulBoneAxe(from); break;
                    case 6: MakeTyreanOrnateAxe(from); break;
                    case 7: MakeTyreanThrowingAxe(from); break;
                    case 8: MakeTyreanWarAxe(from); break;
                    case 9: MakeVhalurianWarAxe(from); break;
                }
            }
        }

        public void MakeAxe(Mobile from)
        {
            Axe weapon = new Axe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.35 + m_Hilt.Durability * 0.65);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.65 + m_Hilt.Quality * 0.35);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeBattleAxe(Mobile from)
        {
            BattleAxe weapon = new BattleAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.Durability * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeWarAxe(Mobile from)
        {
            WarAxe weapon = new WarAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.4 + m_Hilt.Quality * 0.6);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeAzhuranAxe(Mobile from)
        {
            AzhuranAxe weapon = new AzhuranAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.Durability * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeMhordulBoneAxe(Mobile from)
        {
            if (from.Backpack.ConsumeTotal(typeof(Bone), 10))
            {
                MhordulBoneAxe weapon = new MhordulBoneAxe();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage +2;
                weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed + 4;
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
                    weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanOrnateAxe(Mobile from)
        {
            TyreanOrnateAxe weapon = new TyreanOrnateAxe();
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
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanThrowingAxe(Mobile from)
        {
            TyreanThrowingAxe weapon = new TyreanThrowingAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.Durability * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.65 + m_Hilt.Quality * 0.35);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeTyreanWarAxe(Mobile from)
        {
            TyreanWarAxe weapon = new TyreanWarAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.Durability * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

        public void MakeVhalurianWarAxe(Mobile from)
        {
            VhalurianWarAxe weapon = new VhalurianWarAxe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.Damage + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.Speed + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.Attack + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.Defense + m_Blade.Defense;
            weapon.Resource = m_Hilt.Resource;
            int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.Durability * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + m_Hilt.Quality * 0.4);
            if (quality == 500)
                weapon.Quality = WeaponQuality.Legendary;
            if (quality < 500)
                weapon.Quality = WeaponQuality.Masterwork;
            if (quality < 450)
                weapon.Quality = WeaponQuality.Illustrious;
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

#endregion AxeHead


}

