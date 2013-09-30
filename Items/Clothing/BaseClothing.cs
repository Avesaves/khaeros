using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Engines.Craft;
using Server.Factions;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Commands;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public enum ClothingQuality
	{
        Low = 1,
        Regular = 3,
        Exceptional = 5,
        Masterwork = 9,
        Extraordinary = 7,
        Poor = 0,
        Inferior = 2,
        Superior = 4,
        Remarkable = 6,
        Antique = 8,
        Legendary = 10 
	}

	public interface IArcaneEquip
	{
		bool IsArcane{ get; }
		int CurArcaneCharges{ get; set; }
		int MaxArcaneCharges{ get; set; }
	}

	public abstract class BaseClothing : Item, IDyable, IScissorable, IFactionItem, ICraftable, IWearableDurability
	{
		#region Factions
		private FactionItem m_FactionState;

		public FactionItem FactionItemState
		{
			get{ return m_FactionState; }
			set
			{
				m_FactionState = value;

				if ( m_FactionState == null )
					Hue = 0;

				LootType = ( m_FactionState == null ? LootType.Regular : LootType.Blessed );
			}
		}
		#endregion


		private int m_MaxHitPoints;
		private int m_HitPoints;
		private Mobile m_Crafter;
		private ClothingQuality m_Quality;
		private bool m_PlayerConstructed;
		protected CraftResource m_Resource;
		private int m_StrReq = -1;

		private AosAttributes m_AosAttributes;
		private AosArmorAttributes m_AosClothingAttributes;
		private AosSkillBonuses m_AosSkillBonuses;
		private AosElementAttributes m_AosResistances;
		private string m_CraftersOriginalName;
        private Disease m_Disease;

        private bool m_OldCloth = true;

        [CommandProperty(AccessLevel.GameMaster)]
        public Disease Disease
        {
            get { return m_Disease; }
            set { m_Disease = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public string CraftersOriginalName
		{
			get{ return m_CraftersOriginalName; }
			set{ m_CraftersOriginalName = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxHitPoints
		{
			get{ return m_MaxHitPoints; }
			set{ m_MaxHitPoints = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitPoints
		{
			get 
			{
				return m_HitPoints;
			}
			set 
			{
				if ( value != m_HitPoints && MaxHitPoints > 0 )
				{
					m_HitPoints = value;

					if ( m_HitPoints < 0 )
						Delete();
					else if ( m_HitPoints > MaxHitPoints )
						m_HitPoints = MaxHitPoints;

					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int StrRequirement
		{
			get{ return ( m_StrReq == -1 ? (Core.AOS ? AosStrReq : OldStrReq) : m_StrReq ); }
			set{ m_StrReq = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ClothingQuality Quality
		{
			get{ return m_Quality; }
			set{ m_Quality = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PlayerConstructed
		{
			get{ return m_PlayerConstructed; }
			set{ m_PlayerConstructed = value; }
		}

		public virtual CraftResource DefaultResource{ get{ return CraftResource.None; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosArmorAttributes ClothingAttributes
		{
			get{ return m_AosClothingAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosSkillBonuses SkillBonuses
		{
			get{ return m_AosSkillBonuses; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get{ return m_AosResistances; }
			set{}
		}

		public virtual int BasePhysicalResistance{ get{ return 0; } }
        public virtual int BaseFireResistance
        {
            get
			{ 
				if( this.Resource == CraftResource.Linen )
					return 2;
				
				else if( this.Resource == CraftResource.Cotton )
					return 5;
					
				else if( this.Resource == CraftResource.Satin )
					return 5;
					
				else if( this.Resource == CraftResource.Velvet )
					return 5;					
				
				return 0; 
			} 
        }
        public virtual int BaseColdResistance
        {
            get
            {
				if( this.Resource == CraftResource.Linen )
					return 2;
				
				else if( this.Resource == CraftResource.Wool )
					return 5;
					
				else if( this.Resource == CraftResource.Velvet )
					return 5;					

                return 0;
            }
        }
		public virtual int BasePoisonResistance{ get{ return 0; } }
		public virtual int BaseEnergyResistance{ get{ return 0; } }
		
		public virtual int BaseBluntResistance
		{ 
			get
			{ 
				if( this.Resource == CraftResource.Cotton )
					return 1;
					
				else if( this.Resource == CraftResource.Wool )
					return 2;	

				else if( this.Resource == CraftResource.ScaledLeather )
					return 2;
				
				return 0; 
			} 
		}
		
		public virtual int BaseSlashingResistance
		{ 
			get
			{ 
				if( this.Resource == CraftResource.Linen )
					return 1;
				
				else if( this.Resource == CraftResource.RegularLeather )
					return 1;
				
				else if( this.Resource == CraftResource.BeastLeather )
					return 2;
				
				return 0; 
			} 
		}
		
		public virtual int BasePiercingResistance
		{ 
			get
			{ 
				if( this.Resource == CraftResource.Silk )
					return 1;
					
				else if( this.Resource == CraftResource.Satin )
					return 1;
					
				else if( this.Resource == CraftResource.Velvet )
					return 1;
				
				else if( this.Resource == CraftResource.ThickLeather )
					return 2;
				
				else if( this.Resource == CraftResource.RegularLeather )
					return 1;
				
				return 0; 
			} 
		}

		public override int PhysicalResistance{ get{ return BasePhysicalResistance + m_AosResistances.Physical; } }
		public override int FireResistance{ get{ return BaseFireResistance + m_AosResistances.Fire; } }
		public override int ColdResistance{ get{ return BaseColdResistance + m_AosResistances.Cold; } }
		public override int PoisonResistance{ get{ return BasePoisonResistance + m_AosResistances.Poison; } }
		public override int EnergyResistance{ get{ return BaseEnergyResistance + m_AosResistances.Energy; } }
		
		public override int BluntResistance{ get{ return BaseBluntResistance + m_AosResistances.Blunt; } }
		public override int SlashingResistance{ get{ return BaseSlashingResistance + m_AosResistances.Slashing; } }
		public override int PiercingResistance{ get{ return BasePiercingResistance + m_AosResistances.Piercing; } }

		public virtual int ArtifactRarity{ get{ return 0; } }

		public virtual int BaseStrBonus{ get{ return 0; } }
		public virtual int BaseDexBonus{ get{ return 0; } }
		public virtual int BaseIntBonus { get { return 0; } }

		public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			if ( !Ethics.Ethic.CheckTrade( from, to, newOwner, this ) )
				return false;

			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}

		public virtual Race RequiredRace { get { return null; } }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( IsChildOf( from ) )
			{
				if ( Parent != from )
				{
					list.Add( new EquipEntry( this ) );
					list.Add( new ChangeLayerEntry( this ) );
				}
			}
		}

		private class EquipEntry : ContextMenuEntry
		{
			private Item m_Item;

			public EquipEntry( Item item ) : base( 6132, 0 ) // Use
			{
				m_Item = item;
			}

			public override void OnClick()
			{
				if ( m_Item.IsChildOf( Owner.From ) )
					Owner.From.EquipItem( m_Item );
			}
		}
		
		private class ChangeLayerEntry : ContextMenuEntry
		{
			private Item m_Item;

			public ChangeLayerEntry( Item item ) : base( 6174, 0 ) // Rebind
			{
				m_Item = item;
			}

			public override void OnClick()
			{
				if ( m_Item.IsChildOf( Owner.From ) )
					Owner.From.SendGump( new ChangeLayerGump( m_Item ) );
			}
		}
		
		public override bool CanEquip( Mobile from )
		{
			if ( !Ethics.Ethic.CheckEquip( from, this ) )
				return false;

			if( from.AccessLevel < AccessLevel.GameMaster )
			{
				if( RequiredRace != null && from.Race != RequiredRace )
				{
					if( RequiredRace == Race.Elf )
						from.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
					else
						from.SendMessage( "Only {0} may use this.", RequiredRace.PluralName );

					return false;
				}
				else if( !AllowMaleWearer && !from.Female )
				{
					if( AllowFemaleWearer )
						from.SendLocalizedMessage( 1010388 ); // Only females can wear this.
					else
						from.SendMessage( "You may not wear this." );

					return false;
				}
				else if( !AllowFemaleWearer && from.Female )
				{
					if( AllowMaleWearer )
						from.SendLocalizedMessage( 1063343 ); // Only males can wear this.
					else
						from.SendMessage( "You may not wear this." );

					return false;
				}
				else
				{
					int strBonus = ComputeStatBonus( StatType.Str );
					int strReq = ComputeStatReq( StatType.Str );

					if( from.Str < strReq || (from.Str + strBonus) < 1 )
					{
						from.SendLocalizedMessage( 500213 ); // You are not strong enough to equip that.
						return false;
					}
				}
			}

			return base.CanEquip( from );
		}

		public virtual int AosStrReq{ get{ return 0; } }
		public virtual int OldStrReq{ get{ return 0; } }

		public virtual int InitMinHits{ get{ return 50; } }
		public virtual int InitMaxHits{ get{ return 70; } }

		public virtual bool AllowMaleWearer{ get{ return true; } }
		public virtual bool AllowFemaleWearer{ get{ return true; } }
		public virtual bool CanBeBlessed{ get{ return true; } }

		public int ComputeStatReq( StatType type )
		{
			int v;

			//if ( type == StatType.Str )
				v = StrRequirement;

			return AOS.Scale( v, 100 - GetLowerStatReq() );
		}

        public int ComputeStatBonus( StatType type )
        {
            if( type == StatType.Str )
                return Attributes.BonusStr;
            else if( type == StatType.Dex )
                return Attributes.BonusDex;
            else if( type == StatType.Int )
                return Attributes.BonusInt;
            else if( type == StatType.HitsMax )
                return Attributes.BonusHitsMax;
            else if( type == StatType.StamMax )
                return Attributes.BonusStamMax;
            else
                return Attributes.BonusManaMax;
        }

		public virtual void AddStatBonuses( Mobile parent )
		{
			if ( parent == null )
				return;

            int hitsBonus = ComputeStatBonus( StatType.HitsMax );
            int stamBonus = ComputeStatBonus( StatType.StamMax );
            int manaBonus = ComputeStatBonus( StatType.ManaMax );
            int strBonus = ComputeStatBonus( StatType.Str );
            int dexBonus = ComputeStatBonus( StatType.Dex );
            int intBonus = ComputeStatBonus( StatType.Int );

            if( parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsBonus != 0 || stamBonus != 0 || manaBonus != 0 ) )
            {
                Mobile m = (Mobile)Parent;

                string modName = Serial.ToString();

                if( hitsBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.HitsMax, modName + "Hits", hitsBonus, TimeSpan.Zero ) );

                if( stamBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.StamMax, modName + "Stam", stamBonus, TimeSpan.Zero ) );

                if( manaBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.ManaMax, modName + "Mana", manaBonus, TimeSpan.Zero ) );

                if( strBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

                if( dexBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

                if( intBonus != 0 )
                    m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
            }
		}

		public static void ValidateMobile( Mobile m )
		{
			for ( int i = m.Items.Count - 1; i >= 0; --i )
			{
				if ( i >= m.Items.Count )
					continue;

				Item item = m.Items[i];

				if ( item is BaseClothing )
				{
					BaseClothing clothing = (BaseClothing)item;

					if( clothing.RequiredRace != null && m.Race != clothing.RequiredRace )
					{
						if( clothing.RequiredRace == Race.Elf )
							m.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
						else
							m.SendMessage( "Only {0} may use this.", clothing.RequiredRace.PluralName );

						m.AddToBackpack( clothing );
					}
					else if ( !clothing.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster )
					{
						if ( clothing.AllowFemaleWearer )
							m.SendLocalizedMessage( 1010388 ); // Only females can wear this.
						else
							m.SendMessage( "You may not wear this." );

						m.AddToBackpack( clothing );
					}
					else if ( !clothing.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster )
					{
						if ( clothing.AllowMaleWearer )
							m.SendLocalizedMessage( 1063343 ); // Only males can wear this.
						else
							m.SendMessage( "You may not wear this." );

						m.AddToBackpack( clothing );
					}
				}
			}
		}

		public int GetLowerStatReq()
		{
			if ( !Core.AOS )
				return 0;

			return m_AosClothingAttributes.LowerStatReq;
		}

		public override void OnAdded( object parent )
		{
			Mobile mob = parent as Mobile;

			if ( mob != null )
			{
				if ( Core.AOS )
					m_AosSkillBonuses.AddTo( mob );

				AddStatBonuses( mob );
				mob.CheckStatTimers();
			}

			base.OnAdded( parent );
			
			if( parent is PlayerMobile )
			{
				PlayerMobile pm = parent as PlayerMobile;
				
				if( pm.HasGump( typeof( CharInfoGump ) ) && pm.m_CharInfoTimer == null )
				{
					pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( pm );
					pm.m_CharInfoTimer.Start();
				}
			}
		}

		public override void OnRemoved( object parent )
		{
			Mobile m = parent as Mobile;

			if ( m != null )
			{
				if ( Core.AOS )
					m_AosSkillBonuses.Remove();

				string modName = this.Serial.ToString();

                m.RemoveStatMod( modName + "Str" );
                m.RemoveStatMod( modName + "Dex" );
                m.RemoveStatMod( modName + "Int" );
                m.RemoveStatMod( modName + "Hits" );
                m.RemoveStatMod( modName + "Stam" );
                m.RemoveStatMod( modName + "Mana" );

				m.CheckStatTimers();
			}

			base.OnRemoved( parent );
			
			if( parent is PlayerMobile )
			{
				PlayerMobile pm = parent as PlayerMobile;
				
				if( pm.HasGump( typeof( CharInfoGump ) ) && pm.m_CharInfoTimer == null )
				{
					pm.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( pm );
					pm.m_CharInfoTimer.Start();
				}
			}
		}

        public override void OnDoubleClick(Mobile from)
        {
            
        }

		public virtual int OnHit( BaseWeapon weapon, int damageTaken )
		{
			int Absorbed = Utility.RandomMinMax( 1, 4 );

			damageTaken -= Absorbed;

			if ( damageTaken < 0 ) 
				damageTaken = 0;

			if ( 25 > Utility.Random( 100 ) ) // 25% chance to lower durability
			{
				if ( Core.AOS && m_AosClothingAttributes.SelfRepair > Utility.Random( 10 ) )
				{
					HitPoints += 2;
				}
				else
				{
					int wear;

					if ( weapon.Type == WeaponType.Bashing )
						wear = Absorbed / 2;
					else
						wear = Utility.Random( 2 );

					if ( wear > 0 && m_MaxHitPoints > 0 )
					{
						if ( m_HitPoints >= wear )
						{
							HitPoints -= wear;
							wear = 0;
						}
						else
						{
							wear -= HitPoints;
							HitPoints = 0;
						}

						if ( wear > 0 )
						{
							if ( m_MaxHitPoints > wear )
							{
								MaxHitPoints -= wear;

								if ( Parent is Mobile )
									((Mobile)Parent).LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061121 ); // Your equipment is severely damaged.
							}
							else
							{
								Delete();
							}
						}
					}
				}
			}

			return damageTaken;
		}

		public BaseClothing( int itemID, Layer layer ) : this( itemID, layer, 0 )
		{
		}

		public BaseClothing( int itemID, Layer layer, int hue ) : base( itemID )
		{
			Layer = layer;
			Hue = hue;

			m_Resource = DefaultResource;
			m_Quality = ClothingQuality.Regular;

			m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );

			m_AosAttributes = new AosAttributes( this );
			m_AosClothingAttributes = new AosArmorAttributes( this );
			m_AosSkillBonuses = new AosSkillBonuses( this );
			m_AosResistances = new AosElementAttributes( this );
		}

		public BaseClothing( Serial serial ) : base( serial )
		{
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( base.AllowEquipedCast( from ) )
				return true;

			return ( m_AosAttributes.SpellChanneling != 0 );
		}

		public override bool CheckPropertyConfliction( Mobile m )
		{
			if ( base.CheckPropertyConfliction( m ) )
				return true;

			if ( Layer == Layer.Pants )
				return ( m.FindItemOnLayer( Layer.InnerLegs ) != null );

			if ( Layer == Layer.Shirt )
				return ( m.FindItemOnLayer( Layer.InnerTorso ) != null );
				
			if ( Layer == Layer.Ring )
				return ( m.FindItemOnLayer( Layer.Gloves ) != null );
				
			if ( Layer == Layer.Neck )
				return ( m.FindItemOnLayer( Layer.MiddleTorso ) != null );
				
			if ( Layer == Layer.Bracelet )
				return ( m.FindItemOnLayer( Layer.Cloak ) != null );
			
			if ( Layer == Layer.Arms )
				return ( m.FindItemOnLayer( Layer.Waist ) != null );

			return false;
		}

		private string GetNameString()
		{
			string name = this.Name;

			if ( name == null )
				name = String.Format( "#{0}", LabelNumber );

			return name;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_CraftersOriginalName != null )
				list.Add( 1050043, m_CraftersOriginalName ); // crafted by ~1_NAME~

			#region Factions
			if ( m_FactionState != null )
				list.Add( 1041350 ); // faction item
			#endregion

			if ( m_Quality == ClothingQuality.Exceptional )
				list.Add( 1060659, "Quality\tExceptional" );
			
			if ( m_Quality == ClothingQuality.Masterwork )
				list.Add( 1060659, "Quality\tMasterwork" );
			
			if ( m_Quality == ClothingQuality.Extraordinary )
				list.Add( 1060659, "Quality\tExtraordinary" );

			if( RequiredRace == Race.Elf )
				list.Add( 1075086 ); // Elves Only

			if ( m_AosSkillBonuses != null )
				m_AosSkillBonuses.GetProperties( list );

			int prop;

			if ( (prop = ArtifactRarity) > 0 )
				list.Add( 1061078, prop.ToString() ); // artifact rarity ~1_val~

			if ( (prop = m_AosAttributes.WeaponDamage) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( (prop = m_AosAttributes.DefendChance) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusDex) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( (prop = m_AosAttributes.EnhancePotions) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( (prop = m_AosAttributes.CastRecovery) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( (prop = m_AosAttributes.CastSpeed) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( (prop = m_AosAttributes.AttackChance) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusHits) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( (prop = m_AosAttributes.BonusInt) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( (prop = m_AosAttributes.LowerManaCost) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( (prop = m_AosAttributes.LowerRegCost) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( (prop = m_AosClothingAttributes.LowerStatReq) != 0 )
				list.Add( 1060435, prop.ToString() ); // lower requirements ~1_val~%

			if ( (prop = m_AosAttributes.Luck) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( (prop = m_AosClothingAttributes.MageArmor) != 0 )
				list.Add( 1060437 ); // mage armor

			if ( (prop = m_AosAttributes.BonusMana) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( (prop = m_AosAttributes.RegenMana) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( (prop = m_AosAttributes.NightSight) != 0 )
				list.Add( 1060441 ); // night sight

			if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( (prop = m_AosAttributes.RegenStam) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( (prop = m_AosAttributes.RegenHits) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( (prop = m_AosClothingAttributes.SelfRepair) != 0 )
				list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

			if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( (prop = m_AosAttributes.SpellDamage) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusStam) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( (prop = m_AosAttributes.BonusStr) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( (prop = m_AosAttributes.WeaponSpeed) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

			//base.AddResistanceProperties( list );
			
			
			
			int v = PhysicalResistance;

			if ( v != 0 )
				list.Add( 1060448, v.ToString() ); // physical resist ~1_val~%

			v = FireResistance;

			if ( v != 0 )
				list.Add( 1060447, v.ToString() ); // fire resist ~1_val~%

			v = ColdResistance;

			if ( v != 0 )
				list.Add( 1060445, v.ToString() ); // cold resist ~1_val~%

			v = PoisonResistance;

			if ( v != 0 )
				list.Add( 1060449, v.ToString() ); // poison resist ~1_val~%

			v = EnergyResistance;

			if ( v != 0 )
				list.Add( 1060446, v.ToString() ); // energy resist ~1_val~%

            v = BluntResistance;

			if ( v != 0 )
				list.Add( 1060526, v.ToString() ); // cold resist ~1_val~%
           
            v = SlashingResistance;

			if ( v != 0 )
				list.Add( 1060527, v.ToString() ); // cold resist ~1_val~%

            v = PiercingResistance;

			if ( v != 0 )
				list.Add( 1060528, v.ToString() ); // cold resist ~1_val~%
			
			string layer = "";
			for (int i=0; i<ChangeLayerGump.LayerArray.Length; i++)
				if (ChangeLayerGump.LayerArray[i].Key == Layer)
					layer = ChangeLayerGump.LayerArray[i].Value;
			
			if (layer != "")
				list.Add( 1060847, "{0}\t{1}", "Layer:", layer ); // ~1_val~ ~2_val~

			if ( (prop = m_AosClothingAttributes.DurabilityBonus) > 0 )
				list.Add( 1060410, prop.ToString() ); // durability ~1_val~%

			if ( (prop = ComputeStatReq( StatType.Str )) > 0 )
				list.Add( 1061170, prop.ToString() ); // strength requirement ~1_val~

			if ( m_HitPoints >= 0 && m_MaxHitPoints > 0 )
				list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~
		}

		public override void OnSingleClick( Mobile from )
		{
			ArrayList attrs = new ArrayList();

			if ( DisplayLootType )
			{
				if ( LootType == LootType.Blessed )
					attrs.Add( new EquipInfoAttribute( 1038021 ) ); // blessed
				else if ( LootType == LootType.Cursed )
					attrs.Add( new EquipInfoAttribute( 1049643 ) ); // cursed
			}

			#region Factions
			if ( m_FactionState != null )
				attrs.Add( new EquipInfoAttribute( 1041350 ) ); // faction item
			#endregion

			if ( m_Quality == ClothingQuality.Exceptional )
				attrs.Add( new EquipInfoAttribute( 1018305 - (int)m_Quality ) );

			int number;

			if ( Name == null )
			{
				number = LabelNumber;
			}
			else
			{
				this.LabelTo( from, Name );
				number = 1041000;
			}

			if ( attrs.Count == 0 && Crafter == null && Name != null )
				return;

			EquipmentInfo eqInfo = new EquipmentInfo( number, m_Crafter, false, (EquipInfoAttribute[])attrs.ToArray( typeof( EquipInfoAttribute ) ) );

			from.Send( new DisplayEquipmentInfo( this, eqInfo ) );
		}

		#region Serialization
		private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
		{
			if ( setIf )
				flags |= toSet;
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( (flags & toGet) != 0 );
		}

		[Flags]
		private enum SaveFlag
		{
			None				= 0x00000000,
			Resource			= 0x00000001,
			Attributes			= 0x00000002,
			ClothingAttributes	= 0x00000004,
			SkillBonuses		= 0x00000008,
			Resistances			= 0x00000010,
			MaxHitPoints		= 0x00000020,
			HitPoints			= 0x00000040,
			PlayerConstructed	= 0x00000080,
			Crafter				= 0x00000100,
			Quality				= 0x00000200,
			StrReq				= 0x00000400
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 12 ); // version

			writer.Write( (bool) m_OldCloth );
			
            writer.Write( (int) m_Disease );
			
			writer.Write( (string) m_CraftersOriginalName );

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.Resource,			m_Resource != DefaultResource );
			SetSaveFlag( ref flags, SaveFlag.Attributes,		!m_AosAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.ClothingAttributes,!m_AosClothingAttributes.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.SkillBonuses,		!m_AosSkillBonuses.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.Resistances,		!m_AosResistances.IsEmpty );
			SetSaveFlag( ref flags, SaveFlag.MaxHitPoints,		m_MaxHitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.HitPoints,			m_HitPoints != 0 );
			SetSaveFlag( ref flags, SaveFlag.PlayerConstructed,	m_PlayerConstructed != false );
			SetSaveFlag( ref flags, SaveFlag.Crafter,			m_Crafter != null );
			SetSaveFlag( ref flags, SaveFlag.Quality,			m_Quality != ClothingQuality.Regular );
			SetSaveFlag( ref flags, SaveFlag.StrReq,			m_StrReq != -1 );

			writer.WriteEncodedInt( (int) flags );

			if ( GetSaveFlag( flags, SaveFlag.Resource ) )
				writer.WriteEncodedInt( (int) m_Resource );

			if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
				m_AosAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.ClothingAttributes ) )
				m_AosClothingAttributes.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
				m_AosSkillBonuses.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
				m_AosResistances.Serialize( writer );

			if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
				writer.WriteEncodedInt( (int) m_MaxHitPoints );

			if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
				writer.WriteEncodedInt( (int) m_HitPoints );

			if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
				writer.Write( (Mobile) m_Crafter );

			if ( GetSaveFlag( flags, SaveFlag.Quality ) )
				writer.WriteEncodedInt( (int) m_Quality );

			if ( GetSaveFlag( flags, SaveFlag.StrReq ) )
				writer.WriteEncodedInt( (int) m_StrReq );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 12: m_OldCloth = reader.ReadBool(); goto case 11;
                case 11: m_Disease = (Disease)reader.ReadInt(); goto case 10;
                case 10: goto case 9;
				case 9:	goto case 8;
				case 8:	goto case 7;
				case 7:	goto case 6;
				case 6:
				{
					m_CraftersOriginalName = reader.ReadString();
					goto case 5;
				}
				case 5:
				{
					SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

					if ( GetSaveFlag( flags, SaveFlag.Resource ) )
						m_Resource = (CraftResource)reader.ReadEncodedInt();
					else
						m_Resource = DefaultResource;

					if ( GetSaveFlag( flags, SaveFlag.Attributes ) )
						m_AosAttributes = new AosAttributes( this, reader );
					else
						m_AosAttributes = new AosAttributes( this );

					if ( GetSaveFlag( flags, SaveFlag.ClothingAttributes ) )
						m_AosClothingAttributes = new AosArmorAttributes( this, reader );
					else
						m_AosClothingAttributes = new AosArmorAttributes( this );

					if ( GetSaveFlag( flags, SaveFlag.SkillBonuses ) )
						m_AosSkillBonuses = new AosSkillBonuses( this, reader );
					else
						m_AosSkillBonuses = new AosSkillBonuses( this );

					if ( GetSaveFlag( flags, SaveFlag.Resistances ) )
						m_AosResistances = new AosElementAttributes( this, reader );
					else
						m_AosResistances = new AosElementAttributes( this );

					if ( GetSaveFlag( flags, SaveFlag.MaxHitPoints ) )
						m_MaxHitPoints = reader.ReadEncodedInt();

					if ( GetSaveFlag( flags, SaveFlag.HitPoints ) )
						m_HitPoints = reader.ReadEncodedInt();

					if ( GetSaveFlag( flags, SaveFlag.Crafter ) )
						m_Crafter = reader.ReadMobile();

					if ( GetSaveFlag( flags, SaveFlag.Quality ) )
						m_Quality = (ClothingQuality)reader.ReadEncodedInt();
					else
						m_Quality = ClothingQuality.Regular;

					if ( GetSaveFlag( flags, SaveFlag.StrReq ) )
						m_StrReq = reader.ReadEncodedInt();
					else
						m_StrReq = -1;

					if ( GetSaveFlag( flags, SaveFlag.PlayerConstructed ) )
						m_PlayerConstructed = true;

					break;
				}
				case 4:
				{
					m_Resource = (CraftResource)reader.ReadInt();

					goto case 3;
				}
				case 3:
				{
					m_AosAttributes = new AosAttributes( this, reader );
					m_AosClothingAttributes = new AosArmorAttributes( this, reader );
					m_AosSkillBonuses = new AosSkillBonuses( this, reader );
					m_AosResistances = new AosElementAttributes( this, reader );

					goto case 2;
				}
				case 2:
				{
					m_PlayerConstructed = reader.ReadBool();
					goto case 1;
				}
				case 1:
				{
					m_Crafter = reader.ReadMobile();
					m_Quality = (ClothingQuality)reader.ReadInt();
					break;
				}
				case 0:
				{
					m_Crafter = null;
					m_Quality = ClothingQuality.Regular;
					break;
				}
			}

			if ( version < 2 )
				m_PlayerConstructed = true; // we don't know, so, assume it's crafted

			if ( version < 3 )
			{
				m_AosAttributes = new AosAttributes( this );
				m_AosClothingAttributes = new AosArmorAttributes( this );
				m_AosSkillBonuses = new AosSkillBonuses( this );
				m_AosResistances = new AosElementAttributes( this );
			}

			if ( version < 4 )
				m_Resource = DefaultResource;

			if ( m_MaxHitPoints == 0 && m_HitPoints == 0 )
				m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );

			Mobile parent = Parent as Mobile;

			if ( parent != null )
			{
				if ( Core.AOS )
					m_AosSkillBonuses.AddTo( parent );

				AddStatBonuses( parent );
				parent.CheckStatTimers();
			}
			
			if( version < 7 && this.Hue == 2992 )
				this.Hue = 2990;
			
			if( version < 8 && Quality == ClothingQuality.Masterwork )
			{
				this.Resistances.Blunt += 1;
				this.Resistances.Slashing += 1;
				this.Resistances.Piercing += 1;
			}
			
			if( version < 9 && this.Crafter != null )
			{
				this.CraftersOriginalName = this.Crafter.Name;
			}

            if( version < 10 )
            {
                Resistances.Fire = 0;
                Resistances.Cold = 0;
            }
		}
		#endregion

		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 502437 ); // Items you wish to cut must be in your backpack.
				return false;
			}

			if ( this != null )
			{
				Bandage bandage = new Bandage( 2 );
				
				if( this.Weight > 5.0 )
					bandage.Amount += 2;
				
				if( this.Weight > 10.0 )
					bandage.Amount += 2;
				
				bandage.Hue = this.Hue;
				
				Container pack = from.Backpack;
				
				pack.DropItem( bandage );
				
				from.SendMessage( "You rip the garment apart and create some bandages out of it" );
				
				this.Delete();
			}

			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}
		
		public override void AddNameProperty( ObjectPropertyList list )
		{
			int oreType;

			switch ( m_Resource )
			{
				case CraftResource.Copper:			oreType = 1053106; break; // copper
				case CraftResource.Bronze:			oreType = 1053105; break; // bronze
				case CraftResource.Iron:			oreType = 1062226; break; // iron
				case CraftResource.Gold:			oreType = 1053104; break; // golden
				case CraftResource.Silver:			oreType = 1053107; break; // agapite
				case CraftResource.Obsidian:		oreType = 1053103; break; // verite
				case CraftResource.Steel:	    	oreType = 1053102; break; // valorite
				case CraftResource.Tin:	    		oreType = 1053101; break; // valorite
				case CraftResource.Starmetal:	   	oreType = 1053108; break; // valorite
				
				case CraftResource.RegularLeather:	oreType = 1062235; break; // leather
				case CraftResource.ThickLeather:	oreType = 1061116; break; // Thick  
				case CraftResource.BeastLeather:	oreType = 1061117; break; // Beast
				case CraftResource.ScaledLeather:	oreType = 1061118; break; // Scaled
				
				case CraftResource.RedScales:		oreType = 1060814; break; // red
				case CraftResource.YellowScales:	oreType = 1060818; break; // yellow
				case CraftResource.BlackScales:		oreType = 1060820; break; // black
				case CraftResource.GreenScales:		oreType = 1060819; break; // green
				case CraftResource.WhiteScales:		oreType = 1060821; break; // white
				case CraftResource.BlueScales:		oreType = 1060815; break; // blue
				
				case CraftResource.Oak:				oreType = 1063511; break; // yellow
				case CraftResource.Yew:				oreType = 1063512; break; // black
				case CraftResource.Redwood:			oreType = 1063513; break; // green
				case CraftResource.Ash:				oreType = 1063514; break; // white
				case CraftResource.Greenheart:		oreType = 1063515; break; // blue

                case CraftResource.Cotton:          oreType = 1063525; break; // cloth
				case CraftResource.Linen:			oreType = 1063526; break; // linen
                case CraftResource.Silk:            oreType = 1063539; break; // silk
                case CraftResource.Satin:           oreType = 1063541; break; // satin
                case CraftResource.Velvet:          oreType = 1063543; break; // velvet
                case CraftResource.Wool:            oreType = 1063545; break; // wool
				
				default: oreType = 0; break;
			}

			if ( Name == null )
				list.Add( LabelNumber );
			else
				list.Add( Name );
			
			if ( oreType != 0 )
				list.Add( oreType );
		}
		
		private void MasterworkBonuses( PlayerMobile m )
		{
			int offset = 3 - (m.Masterwork.BluntResist + m.Masterwork.SlashingResist + m.Masterwork.PiercingResist);
			
			if( offset > 0 )
				DistributeBonuses( offset );
			
			this.Resistances.Blunt += m.Masterwork.BluntResist;
			this.Resistances.Slashing += m.Masterwork.SlashingResist;
			this.Resistances.Piercing += m.Masterwork.PiercingResist; 
		}
		
		public void DistributeBonuses( int amount )
		{
			for ( int i = 0; i < amount; ++i )
			{
				switch ( Utility.RandomMinMax( 1, 3 ) )
				{
					case 1: this.Resistances.Blunt += 1; break;
					case 2: this.Resistances.Slashing += 1; break;
					case 3: this.Resistances.Piercing += 1; break;
				}
			}

			InvalidateProperties();
		}

		#region ICraftable Members

		public virtual int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Quality = (ClothingQuality)quality;
            m_OldCloth = false;
			Crafter = from;			
			if ( makersMark )
			{
				this.CraftersOriginalName = from.Name;
			}

			PlayerConstructed = true;

			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			Resource = CraftResources.GetFromType( resourceType );

			CraftContext context = craftSystem.GetContext( from );

			if ( context != null && context.DoNotColor )
				Hue = 0;
			
			PlayerMobile m = from as PlayerMobile;
			bool masterwork = false;
			
			if( from is PlayerMobile )
			{
				if ( quality == 5 )
				{
					bool MWpieces = false;
					bool EOpieces = false;
					
					if( m.Masterwork.HasClothingPieces && Resource == m.Masterwork.ClothingResource )
					{
						if( m.Masterwork.MasterworkClothing )
							MWpieces = true;
						
						else
							EOpieces = true;
						
						m.Masterwork.HasClothingPieces = false;
					}
				
					if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 2 && !EOpieces )
					{
						double chance = 0.002 + GetCraftingSpecBonus( m );
						double roll = Utility.RandomDouble();
						
						if( m.AccessLevel > AccessLevel.Player )
							m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
						
						if( MWpieces || chance >= roll )
						{
							this.Quality = ClothingQuality.Masterwork;
							MasterworkBonuses( m );
							m.SendMessage( 60, "You have created a masterwork item." );
							masterwork = true;
							this.MaxHitPoints += 50;
							this.HitPoints += 50;
							m.Crafting = true;
							LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), 500 ) );
							LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), 100 ) );
							m.Crafting = false;

                            if( m.Backpack != null )
                                ((BaseContainer)m.Backpack).DropAndStack( new RewardToken(2) );
						}
					}
					
					if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 0 && !masterwork )
					{
						double chance = GetCraftingSpecBonus( m ) * 5;
						double roll = Utility.RandomDouble();
						
						if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 1 )
							chance += 0.002;
						
						else if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 2 )
							chance += 0.005;
						
						else chance += 0.01;
						
						if( m.AccessLevel > AccessLevel.Player )
							m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
						
						if( EOpieces || chance >= roll )
						{
							this.Quality = ClothingQuality.Extraordinary;
							m.SendMessage( 60, "You have created an extraordinary item." );
							this.MaxHitPoints += 25;
							this.HitPoints += 25;
							m.Crafting = true;
							LevelSystem.AwardExp( m, Math.Min( ( m.Int * 100 ), 250 ) );
							LevelSystem.AwardCP( m, Math.Min( ( m.Int * 20 ), 50 ) );
							m.Crafting = false;

                            if( m.Backpack != null && Utility.RandomBool() )
                                ( (BaseContainer)m.Backpack ).DropAndStack( new RewardToken() );
						}
					}
				}
				
				MaxHitPoints += ( MaxHitPoints / 10 ) * ( m.Feats.GetFeatLevel(FeatList.DurableCrafts) * 2 );
				HitPoints = MaxHitPoints;
			}
			
			if( Resource == CraftResource.Linen )
			{
/* 				if( m.Feats.GetFeatLevel(FeatList.Linen) < 3 )
				{
					from.SendMessage( 60, "You do not know how to use linen and have ruined your craft." );
			    	this.Delete();
				} */
				
				MaxHitPoints += ( MaxHitPoints / 2 );
				HitPoints = MaxHitPoints;
			}

			return quality;
		}
		
		public double GetCraftingSpecBonus( PlayerMobile m )
		{
			double bonus = 0.0;
			
			if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) > 0 && !String.IsNullOrEmpty(m.CraftingSpecialization) )
			{
				if( m.CraftingSpecialization == "Tailoring" )
				{
					if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 1 )
						bonus = 0.0004;
					
					if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 2 )
						bonus = 0.001;
					
					else
						bonus = 0.002;
				}
				
				else
					bonus = -0.001;
			}
			
			return bonus;
		}

		#endregion
	}
}
