using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public abstract class BaseStaff : BaseMeleeWeapon
	{
		private Mobile m_Owner;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}
		
		public override int DefHitSound{ get{ return 0x233; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override SkillName DefSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefType{ get{ return WeaponType.Staff; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash2H; } }

		public BaseStaff( int itemID ) : base( itemID )
		{
		}

		public BaseStaff( Serial serial ) : base( serial )
		{
		}
		
		public override bool CanEquip( Mobile from )
		{
			if( Owner != null )
			{
				if( !(from is PlayerMobile) || ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.ConsecrateItem) < 2 )
					return false;
			}
			
			return base.CanEquip( from );
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Blacksmithing) > 1 && !(this is PriestStaff) && !(this is ShamanStaff) && !(this is BoneStaff))
                from.Target = new StaffTarget(this);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write ( (Mobile) m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version > 0 )
				m_Owner = reader.ReadMobile();
		}

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			base.OnHit( attacker, defender );

			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
		}
	}

    public class StaffTarget : Target
    {
        private BaseStaff m_Hilt;
        private BaseAttackPiece m_Blade;

        public StaffTarget(BaseStaff owner)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = owner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is BaseAttackPiece))
                return;

            m_Blade = targeted as BaseAttackPiece;

            if (m_Blade is AxeHead)
            {
                Halberd weapon = new Halberd();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.15 + m_Hilt.MaxHitPoints * 0.85);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.85 + ((int)m_Hilt.Quality * 50) * 0.15);
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

            if (m_Blade is CurvedBlade)
                from.Prompt = new StaffCurvedBladePrompt(from, m_Hilt, (CurvedBlade)m_Blade);
            if (m_Blade is HammerHead && (((PlayerMobile)from).Nation == Nation.Haluaroc || ((PlayerMobile)from).Nation == Nation.Mhordul))
            {
                if (((PlayerMobile)from).Nation == Nation.Haluaroc)
                {
                    PriestStaff weapon = new PriestStaff();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                    weapon.Resource = m_Hilt.Resource;
                    int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.MaxHitPoints * 0.8);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.2 + ((int)m_Hilt.Quality * 50) * 0.8);
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
                else
                {
                    ShamanStaff weapon = new ShamanStaff();
                    weapon.NewCrafting = true;
                    weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                    weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                    weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                    weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                    weapon.Resource = m_Hilt.Resource;
                    int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.MaxHitPoints * 0.8);
                    weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                    weapon.Crafter = from;
                    weapon.CraftersOriginalName = from.Name;
                    quality = (int)(m_Blade.Quality * 0.2 + ((int)m_Hilt.Quality * 50) * 0.8);
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

            if (m_Blade is LongBlade && ((PlayerMobile)from).Nation == Nation.Western)
            {
                Glaive weapon = new Glaive();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.4 + m_Hilt.MaxHitPoints * 0.6);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.6 + ((int)m_Hilt.Quality * 50) * 0.4);
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

            if (m_Blade is MediumBlade && ((PlayerMobile)from).Nation == Nation.Western)
            {
                PrimitiveSpear weapon = new PrimitiveSpear();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                weapon.Resource = m_Blade.Resource;
                int quality = (int)(m_Blade.Durability * 0.4 + m_Hilt.MaxHitPoints * 0.6);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.6 + ((int)m_Hilt.Quality * 50) * 0.4);
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
                from.Prompt = new StaffShortBladePrompt(from, m_Hilt, (ShortBlade)m_Blade);

            if (m_Blade is MaceHead && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) > 2 && from.Backpack.ConsumeTotal(typeof(Bone), 10) )
            {
                BoneStaff weapon = new BoneStaff();
                weapon.NewCrafting = true;
                weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
                weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
                weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
                weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
                weapon.Resource = m_Hilt.Resource;
                int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.MaxHitPoints * 0.75);
                weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(m_Blade.Quality * 0.25 + ((int)m_Hilt.Quality * 50) * 0.75);
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

    #region CurvedBlade

    public class StaffCurvedBladePrompt : Prompt
    {
        private BaseStaff m_Hilt;
        private CurvedBlade m_Blade;

        public StaffCurvedBladePrompt(Mobile from, BaseStaff hilt, CurvedBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the polearm you would like to make:");
            from.SendMessage(" 1 - Bardiche ");
            from.SendMessage(" 2 - Scythe ");
            from.SendMessage(" 3 - Spear ");
            from.SendMessage(" 4 - Pitchfork ");
            
            if (((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) > 2)
            {
                from.SendMessage(" 5 - Mhordul Bladed Bone Staff ");
                from.SendMessage(" 6 - bone Spear ");
                from.SendMessage(" 7 - bone Scythe ");
            }
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 7 || (index > 4 && index < 8 && ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Bone) < 3))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1: MakeBardiche(from); break;
                    case 2: MakeScythe(from); break;
                    case 3: MakeSpear(from); break;
                    case 4:
                        from.SendMessage("Target another curved blade");
                        from.Target = new ForkTarget(m_Hilt, m_Blade);
                        break;
                    case 5:
                        if (from.Backpack.ConsumeTotal(typeof(Bone), 10))
                        {
                            from.SendMessage("Target another curved blade.");
                            from.Target = new MhordulBladedStaffTarget(m_Hilt, m_Blade);
                        }
                        break;
                    case 6: 
                        if (from.Backpack.ConsumeTotal(typeof(Bone), 10))
                            MakeBoneSpear(from); 
                        break;
                    case 7: 
                        if (from.Backpack.ConsumeTotal(typeof(Bone), 10))
                            MakeBoneScythe(from); 
                        break;
                        /*from.Target = new DoubleBladedStaffTarget(m_Hilt, m_Blade);
                        break;*/
                }
            }
        }

        public void MakeBardiche(Mobile from)
        {
            Bardiche weapon = new Bardiche();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.4 + m_Hilt.MaxHitPoints * 0.6);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.6 + ((int)m_Hilt.Quality * 50) * 0.4);
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

        public void MakeSpear(Mobile from)
        {
            Spear weapon = new Spear();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.2 + m_Hilt.MaxHitPoints * 0.8);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.8 + ((int)m_Hilt.Quality * 50) * 0.2);
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

        public void MakeScythe(Mobile from)
        {
            Scythe weapon = new Scythe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.45 + m_Hilt.MaxHitPoints * 0.55);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.55 + ((int)m_Hilt.Quality * 50) * 0.45);
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

        public void MakeBoneSpear(Mobile from)
        {
            BoneSpear weapon = new BoneSpear();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.3 + m_Hilt.MaxHitPoints * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + ((int)m_Hilt.Quality * 50) * 0.3);
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

        public void MakeBoneScythe(Mobile from)
        {
            BoneScythe weapon = new BoneScythe();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.45 + m_Hilt.MaxHitPoints * 0.55);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.55 + ((int)m_Hilt.Quality * 50) * 0.45);
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

    public class ForkTarget : Target
    {
        private BaseStaff m_Hilt;
        private CurvedBlade m_Blade;

        public ForkTarget(BaseStaff hilt, CurvedBlade blade)
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
            from.SendMessage("Target one more curved blade to complete the pitchfork");
            from.Target = new PitchforkTarget(m_Hilt, m_Blade, xtraBlade);
        }
    }

    public class PitchforkTarget : Target
    {
        private BaseStaff m_Hilt;
        private CurvedBlade m_Blade1;
        private CurvedBlade m_Blade2;

        public PitchforkTarget(BaseStaff hilt, CurvedBlade blade1, CurvedBlade blade2)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = hilt;
            m_Blade1 = blade1;
            m_Blade2 = blade2;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is CurvedBlade))
                return;
            if (targeted == m_Blade1)
                return;
            if (targeted == m_Blade2)
                return;

            CurvedBlade m_Blade3 = targeted as CurvedBlade;

            Pitchfork weapon = new Pitchfork();
            weapon.NewCrafting = true;
            int quality = (int)(m_Blade1.Damage * 0.35 + m_Blade2.Damage * 0.35 + m_Blade3.Damage * 0.35);
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + quality;
            quality = (int)(m_Blade1.Speed * 0.3 + m_Blade2.Speed * 0.3 + m_Blade3.Speed * 0.3);
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + quality;
            quality = (int)(m_Blade1.Attack * 0.35 + m_Blade2.Attack * 0.35 + m_Blade3.Attack * 0.35);
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + quality;
            quality = (int)(m_Blade1.Defense * 0.3 + m_Blade2.Defense * 0.3 + m_Blade3.Defense * 0.3);
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + quality;
            weapon.Resource = m_Hilt.Resource;
            quality = (int)(m_Blade1.Durability * 0.05 + m_Blade2.Durability * 0.05 + m_Blade3.Durability * 0.05 + m_Hilt.MaxHitPoints * 0.85);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade1.Quality * 0.2 + m_Blade2.Quality * 0.2 + m_Blade3.Quality * 0.2 + ((int)m_Hilt.Quality * 50) * 0.4);
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
            weapon.AddItem(m_Blade1);
            weapon.AddItem(m_Blade2);
            weapon.AddItem(m_Blade3);
            weapon.AddItem(m_Hilt);
        }
    }

    public class MhordulBladedStaffTarget : Target
    {
        private BaseStaff m_Hilt;
        private CurvedBlade m_Blade;

        public MhordulBladedStaffTarget(BaseStaff hilt, CurvedBlade blade)
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

            MhordulBladedBoneStaff weapon = new MhordulBladedBoneStaff();
            weapon.NewCrafting = true;
            int quality = (int)(m_Blade.Damage * 0.5 + xtraBlade.Damage * 0.5);
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + quality + 2;
            quality = (int)(m_Blade.Speed * 0.5 + xtraBlade.Speed * 0.5);
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + quality + 4;
            quality = (int)(m_Blade.Attack * 0.5 + xtraBlade.Attack * 0.5);
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + quality;
            quality = (int)(m_Blade.Defense * 0.5 + xtraBlade.Defense * 0.5);
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + quality;
            weapon.Resource = m_Hilt.Resource;
            quality = (int)(m_Blade.Durability * 0.2 + xtraBlade.Durability * 0.2 + m_Hilt.MaxHitPoints * 0.6);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.3 + xtraBlade.Quality * 0.3 + ((int)m_Hilt.Quality * 50) * 0.4);
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

    #region ShortBlade

    public class StaffShortBladePrompt : Prompt
    {
        private BaseStaff m_Hilt;
        private ShortBlade m_Blade;

        public StaffShortBladePrompt(Mobile from, BaseStaff hilt, ShortBlade blade)
        {
            m_Hilt = hilt;
            m_Blade = blade;

            from.SendMessage("Please type in the code for the type of polearm you would like to make:");
            from.SendMessage(" 1 - Double Bladed Staff ");
            from.SendMessage(" 2 - Pike ");
            from.SendMessage(" 3 - Short Spear ");

            if (((PlayerMobile)from).Nation == Nation.Mhordul)
                from.SendMessage(" 4 - barbarian's spear ");
            if (((PlayerMobile)from).Nation == Nation.Tirebladd)
                from.SendMessage(" 5 - angon ");
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == null)
                return;

            int index = 0;

            if (int.TryParse(text, out index))
            {
                if (index < 1 || index > 5 || (index == 4 && ((PlayerMobile)from).Nation != Nation.Mhordul) || (index == 5 && ((PlayerMobile)from).Nation != Nation.Tirebladd))
                {
                    from.SendMessage("Invalid code.");
                    return;
                }

                switch (index)
                {
                    case 1:
                        from.SendMessage("Target another short blade.");
                        from.Target = new DoubleBladedStaffTarget(m_Hilt, m_Blade);
                        break;
                    case 2: MakePike(from); break;
                    case 3: MakeShortSpear(from); break;
                    case 4: MakeBarbarianSpear(from); break;
                    case 5: MakeAngon(from); break;
                }
            }
        }

        public void MakeAngon(Mobile from)
        {
            Angon weapon = new Angon();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.MaxHitPoints * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + ((int)m_Hilt.Quality * 50) * 0.3);
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

        public void MakePike(Mobile from)
        {
            Pike weapon = new Pike();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.MaxHitPoints * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + ((int)m_Hilt.Quality * 50) * 0.3);
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

        public void MakeShortSpear(Mobile from)
        {
            ShortSpear weapon = new ShortSpear();
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.MaxHitPoints * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + ((int)m_Hilt.Quality * 50) * 0.3);
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

        public void MakeBarbarianSpear(Mobile from)
        {
            BarbarianSpear weapon = new BarbarianSpear();            
            weapon.NewCrafting = true;
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + m_Blade.Damage;
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + m_Blade.Speed;
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Attack;
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + m_Blade.Defense;
            weapon.Resource = m_Blade.Resource;
            int quality = (int)(m_Blade.Durability * 0.25 + m_Hilt.MaxHitPoints * 0.75);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.7 + ((int)m_Hilt.Quality * 50) * 0.3);
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

    public class DoubleBladedStaffTarget : Target
    {
        private BaseStaff m_Hilt;
        private ShortBlade m_Blade;

        public DoubleBladedStaffTarget(BaseStaff hilt, ShortBlade blade)
            : base(2, false, TargetFlags.None)
        {
            m_Hilt = hilt;
            m_Blade = blade;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is ShortBlade) || targeted == m_Blade)
                return;

            ShortBlade xtraBlade = targeted as ShortBlade;
            DoubleBladedStaff weapon = new DoubleBladedStaff();
            weapon.NewCrafting = true;
            int quality = (int)(m_Blade.Damage * 0.5 + xtraBlade.Damage * 0.5);
            weapon.QualityDamage = m_Hilt.GetDamageBonus() / 2 + quality;
            quality = (int)(m_Blade.Speed * 0.5 + xtraBlade.Speed * 0.5);
            weapon.QualitySpeed = m_Hilt.GetSpeedBonus() / 2 + quality;
            quality = (int)(m_Blade.Attack * 0.5 + xtraBlade.Attack * 0.5);
            weapon.QualityAccuracy = m_Hilt.GetHitChanceBonus() / 2 + quality;
            quality = (int)(m_Blade.Defense * 0.5 + xtraBlade.Defense * 0.5);
            weapon.QualityDefense = m_Hilt.GetHitChanceBonus() / 2 + quality;
            weapon.Resource = m_Hilt.Resource;
            quality = (int)(m_Blade.Durability * 0.15 + xtraBlade.Durability * 0.15 + m_Hilt.MaxHitPoints * 0.7);
            weapon.MaxHitPoints = quality; weapon.HitPoints = quality;
            weapon.Crafter = from;
            weapon.CraftersOriginalName = from.Name;
            quality = (int)(m_Blade.Quality * 0.35 + xtraBlade.Quality * 0.35 + ((int)m_Hilt.Quality * 50) * 0.3);
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

#endregion ShortBlade

}
