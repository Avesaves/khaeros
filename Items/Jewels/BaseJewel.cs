using System;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
	public enum GemType
	{
		None,
		StarSapphire,
		Emerald,
		Sapphire,
		Ruby,
		Citrine,
		Amethyst,
		Tourmaline,
		Amber,
		Diamond,
        Cinnabar
	}

	public abstract class BaseJewel : Item, ICraftable
	{
        public static List<string> Seals = new List<string>();

		private AosAttributes m_AosAttributes;
		private AosElementAttributes m_AosResistances;
		private AosSkillBonuses m_AosSkillBonuses;
		private CraftResource m_Resource;
		private GemType m_GemType;
		private Mobile m_Crafter;
		private string m_CraftersOriginalName;
		private WeaponQuality m_Quality;
        private string m_Seal;
        
        private bool m_OldJewel = true;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter{ get{ return m_Crafter; } set{ m_Crafter = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string CraftersOriginalName{ get{ return m_CraftersOriginalName; } set{ m_CraftersOriginalName = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public WeaponQuality Quality{ get{ return m_Quality; } set{ m_Quality = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get{ return m_AosResistances; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosSkillBonuses SkillBonuses
		{
			get{ return m_AosSkillBonuses; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public GemType GemType
		{
			get{ return m_GemType; }
			set{ m_GemType = value; InvalidateProperties(); }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public string Seal { get { return m_Seal; } set { m_Seal = value; } }

		public override int PhysicalResistance{ get{ return m_AosResistances.Physical; } }
		public override int FireResistance{ get{ return m_AosResistances.Fire; } }
		public override int ColdResistance{ get{ return m_AosResistances.Cold; } }
		public override int PoisonResistance{ get{ return m_AosResistances.Poison; } }
		public override int EnergyResistance{ get{ return m_AosResistances.Energy; } }
		public virtual int BaseGemTypeNumber{ get{ return 0; } }

		public override int LabelNumber
		{
			get
			{
				if ( m_GemType == GemType.None )
					return base.LabelNumber;

				return BaseGemTypeNumber + (int)m_GemType - 1;
			}
		}

		public virtual int ArtifactRarity{ get{ return 0; } }

		public BaseJewel( int itemID, Layer layer ) : base( itemID )
		{
			m_AosAttributes = new AosAttributes( this );
			m_AosResistances = new AosElementAttributes( this );
			m_AosSkillBonuses = new AosSkillBonuses( this );
			m_Resource = CraftResource.Iron;
			m_GemType = GemType.None;

			Layer = layer;
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

		public override void OnAdded( object parent )
		{
			if ( Core.AOS && parent is Mobile )
			{
				Mobile m = (Mobile)parent;

				m_AosSkillBonuses.AddTo( m );

                int hitsBonus = ComputeStatBonus( StatType.HitsMax );
                int stamBonus = ComputeStatBonus( StatType.StamMax );
                int manaBonus = ComputeStatBonus( StatType.ManaMax );
                int strBonus = ComputeStatBonus( StatType.Str );
                int dexBonus = ComputeStatBonus( StatType.Dex );
                int intBonus = ComputeStatBonus( StatType.Int );

                if( parent is Mobile && ( strBonus != 0 || dexBonus != 0 || intBonus != 0 || hitsBonus != 0 || stamBonus != 0 || manaBonus != 0 ) )
                {
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

				m.CheckStatTimers();
			}
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !(from is PlayerMobile) || from.Deleted || !from.Alive)
                return;

            

            if (this.RootParentEntity == from)
            {
                if (!String.IsNullOrEmpty(m_Seal))
                {
                    from.Target = new SealTarget(m_Seal);
                    from.SendMessage("Target a candle to prepare " + m_Seal + "; or, target a sealmaker's tool to copy this seal to the tool.");
                }
            }
            else if (!String.IsNullOrEmpty(m_Seal))
                from.SendMessage("That must be in your backpack to use it.");

            base.OnDoubleClick(from);
        }

        private class SealTarget : Target
        {
            private string m_SealText;

            public SealTarget(string text)
                : base(1, true, TargetFlags.None)
            {
                m_SealText = text;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!String.IsNullOrEmpty(m_SealText))
                {
                    if (targeted is Candelabra || targeted is CandelabraStand || targeted is Candle || targeted is CandleLong || targeted is CandleShort || targeted is CandleSkull)
                    {
                        from.Target = new SealWaxTarget(m_SealText);
                        from.PlaySound(0x5AC);
                        from.SendMessage("You have placed wet wax on the seal; target a scroll, paper, or book to seal.");
                    }
                    else if (targeted is SealmakersTool)
                    {
                        if ((targeted as SealmakersTool).RootParentEntity == from)
                        {
                            (targeted as SealmakersTool).Seal = m_SealText;
                            from.SendMessage("The tool's seal is now " + m_SealText + ".");
                            from.PlaySound(0x241);
                            return;
                        }
                        else
                        {
                            from.SendMessage("The sealmaker's tool must be in your pack for you to change it's seal.");
                            return;
                        }
                    }
                    else
                        from.SendMessage("You must target a candle to ready the seal.");
                }
                else
                    from.SendMessage("There is no seal on that.");

                base.OnTarget(from, targeted);
            }
        }

        private class SealWaxTarget : Target
        {
            private string m_SealText;

            public SealWaxTarget(string text)
                : base(1, true, TargetFlags.None)
            {
                m_SealText = text;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is HTMLBook)
                {
                    if (!String.IsNullOrEmpty(m_SealText))
                    {
                        (targeted as HTMLBook).Seal = m_SealText;
                        from.PlaySound(0x249);
                        from.SendMessage("You have sealed the text with " + m_SealText + ".");
                        return;
                    }
                    else
                        from.SendMessage("Error: The seal name is not specified.");
                }
                else
                    from.SendMessage("You can't place a seal on that.");

                base.OnTarget(from, targeted);
            }
        }

		public override void OnRemoved( object parent )
		{
			if ( Core.AOS && parent is Mobile )
			{
				Mobile m = (Mobile)parent;

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
		}

		public BaseJewel( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_CraftersOriginalName != null )
				list.Add( 1050043, m_CraftersOriginalName ); // crafted by ~1_NAME~

            if (m_Quality == WeaponQuality.Poor)
                list.Add(1060659, "Quality\tPoor");

            if (m_Quality == WeaponQuality.Low)
                list.Add(1060659, "Quality\tLow");

            if (m_Quality == WeaponQuality.Inferior)
                list.Add(1060659, "Quality\tInferior");

            if (m_Quality == WeaponQuality.Superior)
                list.Add(1060659, "Quality\tExceptional");

            if (m_Quality == WeaponQuality.Exceptional)
                list.Add(1060659, "Quality\tExceptional");

            if (m_Quality == WeaponQuality.Remarkable)
                list.Add(1060659, "Quality\tRemarkable");

            if (m_Quality == WeaponQuality.Antique)
                list.Add(1060659, "Quality\tAntique");

            if (m_Quality == WeaponQuality.Masterwork)
                list.Add(1060659, "Quality\tMasterwork");

            if (m_Quality == WeaponQuality.Extraordinary)
                list.Add(1060659, "Quality\tExtraordinary");

            if (m_Quality == WeaponQuality.Legendary)
                list.Add(1060659, "Quality\tLegendary");
			
			if ( this.GemType != GemType.None )
				list.Add( 1060658, "Gem Type\t{0}", Commands.LevelSystemCommands.AddSpacesToString(this.GemType.ToString()) );
			
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

			if ( (prop = m_AosAttributes.Luck) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

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

            if (!String.IsNullOrEmpty(m_Seal))
                list.Add("Seal: " + m_Seal);
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 6 ); // version

            writer.Write( (bool) m_OldJewel );

            writer.Write((string)m_Seal);

			writer.Write( (Mobile) m_Crafter );
			writer.Write( (string) m_CraftersOriginalName );
			writer.Write( (int) m_Quality );
			writer.WriteEncodedInt( (int) m_Resource );
			writer.WriteEncodedInt( (int) m_GemType );

			m_AosAttributes.Serialize( writer );
			m_AosResistances.Serialize( writer );
			m_AosSkillBonuses.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 6: m_OldJewel = reader.ReadBool(); goto case 5;
                case 5: m_Seal = reader.ReadString(); goto case 4;
				case 4:
				case 3:
				{
					m_Crafter = reader.ReadMobile();
					m_CraftersOriginalName = reader.ReadString();
					m_Quality = (WeaponQuality)reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_Resource = (CraftResource)reader.ReadEncodedInt();
					m_GemType = (GemType)reader.ReadEncodedInt();

					goto case 1;
				}
				case 1:
				{
					m_AosAttributes = new AosAttributes( this, reader );
					m_AosResistances = new AosElementAttributes( this, reader );
					m_AosSkillBonuses = new AosSkillBonuses( this, reader );

					if ( Core.AOS && Parent is Mobile )
						m_AosSkillBonuses.AddTo( (Mobile)Parent );

					int strBonus = m_AosAttributes.BonusStr;
					int dexBonus = m_AosAttributes.BonusDex;
					int intBonus = m_AosAttributes.BonusInt;

					if ( Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
					{
						Mobile m = (Mobile)Parent;

						string modName = Serial.ToString();

						if ( strBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

						if ( dexBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

						if ( intBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
					}

					if ( Parent is Mobile )
						((Mobile)Parent).CheckStatTimers();

					break;
				}
				case 0:
				{
					m_AosAttributes = new AosAttributes( this );
					m_AosResistances = new AosElementAttributes( this );
					m_AosSkillBonuses = new AosSkillBonuses( this );

					break;
				}
			}

			if ( version < 2 )
			{
				m_Resource = CraftResource.Iron;
				m_GemType = GemType.None;
			}
			
			if( version < 4 )
				Hue = 0;

            if (!String.IsNullOrEmpty(m_Seal) && !Seals.Contains(m_Seal))
                Seals.Add(m_Seal);
		}

		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Crafter = from;
			
			if (makersMark)
				CraftersOriginalName = from.Name;
				
			Type resourceType = typeRes;
			quality = 3;
			PlayerMobile m = from as PlayerMobile;
            m_OldJewel = false;
			
			if( !(from is PlayerMobile) )
					return quality;
			
			double chance = 0.4 + (GetCraftingChance( m ) * 75.0);
			double roll = Utility.RandomDouble();
			
			if( m.AccessLevel > AccessLevel.Player )
				m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
				
			
			if( chance >= roll )
			{
				this.Quality = WeaponQuality.Exceptional;
				quality = 5;

				chance = GetCraftingChance( m );
				roll = Utility.RandomDouble();
				
				if( m.AccessLevel > AccessLevel.Player )
					m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
				
				if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 0 && chance >= roll )
				{
					this.Quality = WeaponQuality.Masterwork;
					from.SendMessage( 60, "You have created a masterwork item." );

                    if( from.Backpack != null )
                        ( (BaseContainer)from.Backpack ).DropAndStack( new RewardToken( 2 ) );
				}
				
				else if( m.Feats.GetFeatLevel(FeatList.Masterwork) > 0 )
				{
					chance = GetCraftingChance( m ) * 5.0;
					roll = Utility.RandomDouble();
				
					if( m.AccessLevel > AccessLevel.Player )
						m.SendMessage( "Chance: " + chance.ToString() + ". Rolled: " + roll.ToString() + "." );
					
					if( chance >= roll )
					{
						this.Quality = WeaponQuality.Extraordinary;
						from.SendMessage( 60, "You have created an extraordinary item." );

                        if( from.Backpack != null && Utility.RandomBool() )
                            ( (BaseContainer)from.Backpack ).DropAndStack( new RewardToken() );
					}
				}	
			}

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			Resource = CraftResources.GetFromType( resourceType );

			if ( 1 < craftItem.Ressources.Count )
			{
				resourceType = craftItem.Ressources.GetAt( 1 ).ItemType;

                if (resourceType == typeof(Cinnabar))
                {
                    GemType = GemType.Cinnabar;
                    Name = "Cinnabar " + Name;
                }
				else if ( resourceType == typeof( StarSapphire ) )
				{
					GemType = GemType.StarSapphire;
					Name = "Star Sapphire " + Name;
				}
				else if ( resourceType == typeof( Emerald ) )
				{
					GemType = GemType.Emerald;
					Name = "Emerald " + Name;
				}
				else if ( resourceType == typeof( Sapphire ) )
				{
					GemType = GemType.Sapphire;
					Name = "Sapphire " + Name;
				}
				else if ( resourceType == typeof( Ruby ) )
				{
					GemType = GemType.Ruby;
					Name = "Ruby " + Name;
				}
				else if ( resourceType == typeof( Citrine ) )
				{
					GemType = GemType.Citrine;
					Name = "Citrine " + Name;
				}
				else if ( resourceType == typeof( Amethyst ) )
				{
					GemType = GemType.Amethyst;
					Name = "Amethyst " + Name;
				}
				else if ( resourceType == typeof( Tourmaline ) )
				{
					GemType = GemType.Tourmaline;
					Name = "Tourmaline " + Name;
				}
				else if ( resourceType == typeof( Amber ) )
				{
					GemType = GemType.Amber;
					Name = "Amber " + Name;
				}
				else if ( resourceType == typeof( Diamond ) )
				{
					GemType = GemType.Diamond;
					Name = "Diamond " + Name;
				}
			}
			
			Hue = 0;

			return quality;
		}
		
		public Item getGem() {
			switch (GemType) {
				case GemType.None: return null; break; //oopsies, let's really hope this never happens!
				case GemType.StarSapphire: return new StarSapphire(); break;
				case GemType.Emerald: return new Emerald(); break;
				case GemType.Sapphire: return new Sapphire(); break;
				case GemType.Ruby: return new Ruby(); break;
				case GemType.Citrine: return new Citrine(); break;
				case GemType.Amethyst: return new Amethyst(); break;
				case GemType.Tourmaline: return new Tourmaline(); break;
				case GemType.Amber: return new Amber(); break;
				case GemType.Diamond: return new Diamond(); break;
                case GemType.Cinnabar: return new Cinnabar(); break;
				default: return null; break; //don't do this you ugly bastard!!!!
			}
		}
		
		public Item getMetal() {
			switch (Resource) {
				case CraftResource.Gold: return new GoldIngot(); break;
				case CraftResource.Silver: return new SilverIngot(); break;
				default: return null; break; //don't do this please
			}
		}
		
		public double GetCraftingChance( PlayerMobile m )
		{
			double bonus = 0.0;
			
			if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 1 )
				bonus = 0.0008;
			
			else if( m.Feats.GetFeatLevel(FeatList.Masterwork) == 2 )
				bonus = 0.002;
			
			else
				bonus = 0.004;

			if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) > 0 && !String.IsNullOrEmpty(m.CraftingSpecialization) )
			{
				if( m.CraftingSpecialization == "Tinkering" )
				{
					if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 1 )
						bonus += 0.0008;
					
					if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) == 2 )
						bonus += 0.002;
					
					else
						bonus += 0.004;
				}
				
				else
					bonus -= -0.002;
			}
			
			return bonus;
		}

		#endregion
	}
}
