using System;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseIngot : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		public override double DefaultWeight
		{
			get { return 2.0; }
		}
		
		string ICommodity.Description
		{
			get
			{
				if( CraftResources.GetName( m_Resource ).ToLower() == "obsidian" )
					return String.Format( Amount == 1 ? "{0} obsidan shard" : "{0} obsidian shards", Amount );

				else
					return String.Format( Amount == 1 ? "{0} {1} ingot" : "{0} {1} ingots", Amount, CraftResources.GetName( m_Resource ).ToLower() );
			}
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( this is ObsidianIngot )
				return;
			
			if ( !Movable )
				return;

			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.SendMessage( "Select a forge to turn these ingots back into ore form." );
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendMessage( "That is too far away." );
			}
		}
		
		private class InternalTarget : Target
		{
			private BaseIngot m_Ingot;

			public InternalTarget( BaseIngot ingot ) :  base ( 2, false, TargetFlags.None )
			{
				m_Ingot = ingot;
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
				if ( m_Ingot == null || m_Ingot.Deleted )
					return;

				if ( !from.InRange( m_Ingot.GetWorldLocation(), 2 ) )
				{
					from.SendMessage( "That is too far away." );
					return;
				}

				if ( IsForge( targeted ) )
				{
					if( m_Ingot is CopperIngot )
						from.AddToBackpack( new CopperOre( m_Ingot.Amount ) );
					
					if( m_Ingot is TinIngot )
						from.AddToBackpack( new TinOre( m_Ingot.Amount ) );
					
					if( m_Ingot is IronIngot )
						from.AddToBackpack( new IronOre( m_Ingot.Amount ) );
					
					if( m_Ingot is BronzeIngot )
					{
						if( m_Ingot.Amount < 2 )
						{
							from.SendMessage( "Not enough bronze to smelt." );
							return;
						}
						
						from.AddToBackpack( new CopperOre( m_Ingot.Amount / 2 ) );
						from.AddToBackpack( new TinOre( m_Ingot.Amount / 2 ) );
					}
					if( m_Ingot is ElectrumIngot )
					{
						if( m_Ingot.Amount < 2 )
						{
							from.SendMessage( "Not enough Electrum to smelt." );
							return;
						}
						
						from.AddToBackpack( new GoldOre( m_Ingot.Amount / 2 ) );
						from.AddToBackpack( new SilverOre( m_Ingot.Amount / 2 ) );
					}
					if( m_Ingot is SteelIngot )
						from.AddToBackpack( new SteelOre( m_Ingot.Amount ) );
					
					if( m_Ingot is GoldIngot )
						from.AddToBackpack( new GoldOre( m_Ingot.Amount ) );
					
					if( m_Ingot is SilverIngot )
						from.AddToBackpack( new SilverOre( m_Ingot.Amount ) );
					
					if( m_Ingot is StarmetalIngot )
						from.AddToBackpack( new StarmetalIngot( m_Ingot.Amount ) );
					
					from.PlaySound( 43 );
					from.SendMessage( "You resmelt the ingots into ore." );
					m_Ingot.Delete();
				}
				
				else
					from.SendMessage( "Invalid target." );
			}
		}

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

		public BaseIngot( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseIngot( CraftResource resource, int amount ) : base( 0x1BF2 )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
			Weight = 2.0;
		}

		public BaseIngot( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if( this is ObsidianIngot && Amount > 1 )
				list.Add( "{0} shards", Amount );
			else if( this is ObsidianIngot )
				list.Add( "shards" );
			else if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1027154 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1027154 ); // ingots
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
				if( m_Resource == CraftResource.Iron )
					return 1042692;
				
				if( m_Resource == CraftResource.Copper )
					return 1042686;
				
				if( m_Resource == CraftResource.Bronze )
					return 1042687;
				
				if( m_Resource == CraftResource.Gold )
					return 1042688;
				
				if( m_Resource == CraftResource.Silver )
					return 1042684;
				
				if( m_Resource == CraftResource.Obsidian )
					return 1042689;
				
				if( m_Resource == CraftResource.Steel )
					return 1042690;
				
				if( m_Resource == CraftResource.Tin )
					return 1042691;
				
				if( m_Resource == CraftResource.Starmetal )
					return 1042685;
				if( m_Resource == CraftResource.Electrum )
					return 1038042;	
				
				
				return 1042692;
			}
		}
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class IronIngot : BaseIngot
	{
		[Constructable]
		public IronIngot() : this( 1 )
		{
		}

		[Constructable]
		public IronIngot( int amount ) : base( CraftResource.Iron, amount )
		{
		}

		public IronIngot( Serial serial ) : base( serial )
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
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class CopperIngot : BaseIngot
	{
		[Constructable]
		public CopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public CopperIngot( int amount ) : base( CraftResource.Copper, amount )
		{
		}

		public CopperIngot( Serial serial ) : base( serial )
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
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class BronzeIngot : BaseIngot
	{
		[Constructable]
		public BronzeIngot() : this( 1 )
		{
		}

		[Constructable]
		public BronzeIngot( int amount ) : base( CraftResource.Bronze, amount )
		{
		}

		public BronzeIngot( Serial serial ) : base( serial )
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
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class GoldIngot : BaseIngot
	{
		[Constructable]
		public GoldIngot() : this( 1 )
		{
		}

		[Constructable]
		public GoldIngot( int amount ) : base( CraftResource.Gold, amount )
		{
		}

		public GoldIngot( Serial serial ) : base( serial )
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
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class SilverIngot : BaseIngot
	{
		[Constructable]
		public SilverIngot() : this( 1 )
		{
		}

		[Constructable]
		public SilverIngot( int amount ) : base( CraftResource.Silver, amount )
		{
		}

		public SilverIngot( Serial serial ) : base( serial )
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
	}
	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ElectrumIngot : BaseIngot
	{
		[Constructable]
		public ElectrumIngot() : this( 1 )
		{
		}

		[Constructable]
		public ElectrumIngot( int amount ) : base( CraftResource.Electrum, amount )
		{
		}

		public ElectrumIngot( Serial serial ) : base( serial )
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
	}
	//[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ObsidianIngot : BaseIngot
	{
		[Constructable]
		public ObsidianIngot() : this( 1 )
		{
		}

		[Constructable]
		public ObsidianIngot( int amount ) : base( CraftResource.Obsidian, amount )
		{
			ItemID = 3977;
		}

		public ObsidianIngot( Serial serial ) : base( serial )
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
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class SteelIngot : BaseIngot
	{
		[Constructable]
		public SteelIngot() : this( 1 )
		{
		}

		[Constructable]
		public SteelIngot( int amount ) : base( CraftResource.Steel, amount )
		{
		}

		public SteelIngot( Serial serial ) : base( serial )
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
	}
	
	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class TinIngot : BaseIngot
	{
		[Constructable]
		public TinIngot() : this( 1 )
		{
		}

		[Constructable]
		public TinIngot( int amount ) : base( CraftResource.Tin, amount )
		{
		}

		public TinIngot( Serial serial ) : base( serial )
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
	}
	
	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class StarmetalIngot : BaseIngot
	{
		[Constructable]
		public StarmetalIngot() : this( 1 )
		{
		}

		[Constructable]
		public StarmetalIngot( int amount ) : base( CraftResource.Starmetal, amount )
		{
		}

		public StarmetalIngot( Serial serial ) : base( serial )
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
	}
}
