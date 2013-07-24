using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
	public abstract class BaseOre : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		string ICommodity.Description
		{
			get
			{
				return String.Format( "{0} {1} ore", Amount, CraftResources.GetName( m_Resource ).ToLower() );
			}
		}

		public abstract BaseIngot GetIngot();

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				case 1:
				{
					m_Resource = (CraftResource)reader.ReadInt();
					break;
				}
				case 0:
				{
					OreInfo info;

					switch ( reader.ReadInt() )
					{
						case 0: info = OreInfo.Iron; break;
						case 1: info = OreInfo.Copper; break;
						case 2: info = OreInfo.Bronze; break;
						case 3: info = OreInfo.Gold; break;
						case 4: info = OreInfo.Silver; break;
						case 5: info = OreInfo.Obsidian; break;
						case 6: info = OreInfo.Steel; break;
						case 7: info = OreInfo.Tin; break;
						case 8: info = OreInfo.Starmetal; break;
						case 9: info = OreInfo.Electrum; break;
						default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
			
			if( version < 2 )
			{
				if( this.Hue == 2413 )
					this.Resource = CraftResource.Copper;
				
				if( this.Hue == 0 )
					this.Resource = CraftResource.Iron;
				
				if( this.Hue == 2418 )
					this.Resource = CraftResource.Bronze;
				if( this.Hue == 2669 )
					this.Resource = CraftResource.Electrum;
			}
		}

		public BaseOre( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseOre( CraftResource resource, int amount ) : base( 0x19B9 )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseOre( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1026583 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1026583 ); // ore
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}

		public override int LabelNumber
		{
			get
			{
				if ( m_Resource >= CraftResource.Iron && m_Resource <= CraftResource.Electrum )
					return 1042845 + (int)(m_Resource - CraftResource.Copper);

				return 1042853; // iron ore;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;
			
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				
				if( this is ObsidianOre && m.Feats.GetFeatLevel(FeatList.Obsidian) < 2 )
				{
					m.SendMessage( 60, "You do not know yet how to refine this material." );
					return;
				}
				
				if( this is SteelOre && m.Feats.GetFeatLevel(FeatList.Steel) < 2 )
				{
					m.SendMessage( 60, "You do not know yet how to refine this material." );
					return;
				}
			}

			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.SendLocalizedMessage( 501971 ); // Select the forge on which to smelt the ore, or another pile of ore with which to combine it.
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 501976 ); // The ore is too far away.
			}
		}

		private class InternalTarget : Target
		{
			private BaseOre m_Ore;

			public InternalTarget( BaseOre ore ) :  base ( 2, false, TargetFlags.None )
			{
				m_Ore = ore;
			}

			private bool IsForge( object obj )
			{
				if ( obj.GetType().IsDefined( typeof( ForgeAttribute ), false ) )
					return true;

				int itemID = 0;

				if ( obj is Item )
					itemID = ((Item)obj).ItemID;
				else if ( obj is StaticTarget )
					itemID = ((StaticTarget)obj).ItemID & 0x3FFF;

				return ( itemID == 4017 || (itemID >= 6522 && itemID <= 6569) );
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Ore.Deleted )
					return;

				if ( !from.InRange( m_Ore.GetWorldLocation(), 2 ) )
				{
					from.SendLocalizedMessage( 501976 ); // The ore is too far away.
					return;
				}
				
				if( ( targeted is CopperOre && m_Ore is TinOre ) || ( targeted is TinOre && m_Ore is CopperOre ) )
				{
					bool anvil, forge;
					
					DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

					if ( !forge )
					{
						from.SendMessage( 60, "You must be standing near a forge in order to do that" );
						return;
					}
					
					if( from.CheckTargetSkill( SkillName.Mining, targeted, 20.0, 80.0 ) )
					{
						int toConsume = m_Ore.Amount;

						if ( toConsume <= 0 )
						{
							from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						}
						else
						{
							if ( toConsume > 30000 )
								toConsume = 30000;
							
							toConsume = Math.Min( m_Ore.Amount, ( (BaseOre) targeted ).Amount );

							BronzeIngot ingot = new BronzeIngot();
							ingot.Amount = toConsume + toConsume;

							m_Ore.Consume( toConsume );
							( (BaseOre)targeted ).Consume( toConsume );
							from.AddToBackpack( ingot );
							from.PlaySound( 43 );


							from.SendLocalizedMessage( 501988 ); // You smelt the ore removing the impurities and put the metal in your backpack.
						}
					}
				}
				if( ( targeted is SilverOre && m_Ore is GoldOre ) || ( targeted is GoldOre && m_Ore is SilverOre ) )
				{
					bool anvil, forge;
					
					DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

					if ( !forge )
					{
						from.SendMessage( 60, "You must be standing near a forge in order to do that" );
						return;
					}
					
					if( from.CheckTargetSkill( SkillName.Mining, targeted, 20.0, 80.0 ) )
					{
						int toConsume = m_Ore.Amount;

						if ( toConsume <= 0 )
						{
							from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						}
						else
						{
							if ( toConsume > 30000 )
								toConsume = 30000;
							
							toConsume = Math.Min( m_Ore.Amount, ( (BaseOre) targeted ).Amount );

							ElectrumIngot ingot = new ElectrumIngot();
							ingot.Amount = toConsume + toConsume;

							m_Ore.Consume( toConsume );
							( (BaseOre)targeted ).Consume( toConsume );
							from.AddToBackpack( ingot );
							from.PlaySound( 43 );


							from.SendLocalizedMessage( 501988 ); // You smelt the ore removing the impurities and put the metal in your backpack.
						}
					}
				}
				if( targeted is Coal && m_Ore is IronOre )
				{
					if( from is PlayerMobile )
					{
						if( ( (PlayerMobile)from ).Feats.GetFeatLevel(FeatList.Steel) < 1 )
						{
							from.SendLocalizedMessage( 501986 ); // You have no idea how to smelt this strange ore!
							return;
						}
					}
					bool anvil, forge;
					
					DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

					if ( !forge )
					{
						from.SendMessage( 60, "You must be standing near a forge in order to do that" );
						return;
					}
					
					if( from.CheckTargetSkill( SkillName.Mining, targeted, 20.0, 80.0 ) )
					{
						int toConsume = m_Ore.Amount;

						if ( toConsume <= 0 )
						{
							from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						}
						else
						{
							if ( toConsume > 30000 )
								toConsume = 30000;
							
							toConsume = Math.Min( m_Ore.Amount, ( (Coal) targeted ).Amount );

							SteelOre steelore = new SteelOre();
							steelore.Amount = toConsume * 2;

							m_Ore.Consume( toConsume );
							( (Coal)targeted ).Consume( toConsume );
							from.AddToBackpack( steelore );
							from.PlaySound( 43 );


							from.SendLocalizedMessage( 501988 ); // You smelt the ore removing the impurities and put the metal in your backpack.
						}
					}
				}

				if ( IsForge( targeted ) )
				{
					if( m_Ore is SteelOre && ( (PlayerMobile)from ).Feats.GetFeatLevel(FeatList.Steel) < 2 )
					{
						from.SendLocalizedMessage( 501986 ); // You have no idea how to smelt this strange ore!
						return;
					}
					
					double difficulty;

					switch ( m_Ore.Resource )
					{
						default: difficulty = 0.0; break;
						case CraftResource.Copper: difficulty = 0.0; break;
						case CraftResource.Bronze: difficulty = 50.0; break;
						case CraftResource.Gold: difficulty = 90.0; break;
						case CraftResource.Silver: difficulty = 80.0; break;
						case CraftResource.Obsidian: difficulty = 70.0; break;
						case CraftResource.Steel: difficulty = 70.0; break;
						case CraftResource.Tin: difficulty = 0.0; break;
						case CraftResource.Iron: difficulty = 70.0; break;
						case CraftResource.Starmetal: difficulty = 100.0; break;
						case CraftResource.Electrum: difficulty = 75.0; break;
					}

					double minSkill = difficulty - 25.0;
					double maxSkill = difficulty + 25.0;
					
					if ( difficulty > 50.0 && difficulty > from.Skills[SkillName.Mining].Value )
					{
						from.SendLocalizedMessage( 501986 ); // You have no idea how to smelt this strange ore!
						return;
					}

					if ( from.CheckTargetSkill( SkillName.Mining, targeted, minSkill, maxSkill ) )
					{
						int toConsume = m_Ore.Amount;

						if ( toConsume <= 0 )
						{
							from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						}
						else
						{
							if ( toConsume > 30000 )
								toConsume = 30000;

							BaseIngot ingot = m_Ore.GetIngot();
							ingot.Amount = toConsume;

							m_Ore.Consume( toConsume );
							from.AddToBackpack( ingot );
							from.PlaySound( 43 );


							from.SendLocalizedMessage( 501988 ); // You smelt the ore removing the impurities and put the metal in your backpack.
						}
					}
					else if ( m_Ore.Amount < 2 )
					{
						from.SendLocalizedMessage( 501989 ); // You burn away the impurities but are left with no useable metal.
						m_Ore.Delete();
					}
					else
					{
						from.SendLocalizedMessage( 501990 ); // You burn away the impurities but are left with less useable metal.
						m_Ore.Amount /= 2;
					}
				}
			}
		}
	}

	public class IronOre : BaseOre
	{
		[Constructable]
		public IronOre() : this( 1 )
		{
		}

		[Constructable]
		public IronOre( int amount ) : base( CraftResource.Iron, amount )
		{
		}

		public IronOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new IronIngot();
		}
	}

	public class CopperOre : BaseOre
	{
		[Constructable]
		public CopperOre() : this( 1 )
		{
		}

		[Constructable]
		public CopperOre( int amount ) : base( CraftResource.Copper, amount )
		{
		}

		public CopperOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new CopperIngot();
		}
	}

	public class BronzeOre : BaseOre
	{
		[Constructable]
		public BronzeOre() : this( 1 )
		{
		}

		[Constructable]
		public BronzeOre( int amount ) : base( CraftResource.Bronze, amount )
		{
		}

		public BronzeOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new BronzeIngot();
		}
	}

	public class ElectrumOre : BaseOre
	{
		[Constructable]
		public ElectrumOre() : this( 1 )
		{
		}

		[Constructable]
		public ElectrumOre( int amount ) : base( CraftResource.Electrum, amount )
		{
		}

		public ElectrumOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new ElectrumIngot();
		}
	}
	public class GoldOre : BaseOre
	{
		[Constructable]
		public GoldOre() : this( 1 )
		{
		}

		[Constructable]
		public GoldOre( int amount ) : base( CraftResource.Gold, amount )
		{
		}

		public GoldOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new GoldIngot();
		}
	}
	public class SilverOre : BaseOre
	{
		[Constructable]
		public SilverOre() : this( 1 )
		{
		}

		[Constructable]
		public SilverOre( int amount ) : base( CraftResource.Silver, amount )
		{
		}

		public SilverOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new SilverIngot();
		}
	}

	public class ObsidianOre : BaseOre
	{
		//[Constructable]
		public ObsidianOre() : this( 1 )
		{
		}

		//[Constructable]
		public ObsidianOre( int amount ) : base( CraftResource.Obsidian, amount )
		{
		}

		public ObsidianOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		

		public override BaseIngot GetIngot()
		{
			return new ObsidianIngot();
		}
	}

	public class SteelOre : BaseOre
	{
		[Constructable]
		public SteelOre() : this( 1 )
		{
		}

		[Constructable]
		public SteelOre( int amount ) : base( CraftResource.Steel, amount )
		{
		}

		public SteelOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		public override BaseIngot GetIngot()
		{
			return new SteelIngot();
		}
	}
	
	public class TinOre : BaseOre
	{
		[Constructable]
		public TinOre() : this( 1 )
		{
		}

		[Constructable]
		public TinOre( int amount ) : base( CraftResource.Tin, amount )
		{
		}

		public TinOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		public override BaseIngot GetIngot()
		{
			return new TinIngot();
		}
	}
	
	public class StarmetalOre : BaseOre
	{
		[Constructable]
		public StarmetalOre() : this( 1 )
		{
		}

		[Constructable]
		public StarmetalOre( int amount ) : base( CraftResource.Starmetal, amount )
		{
		}

		public StarmetalOre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 2.0;
		}

		public override BaseIngot GetIngot()
		{
			return new StarmetalIngot();
		}
	}
}
