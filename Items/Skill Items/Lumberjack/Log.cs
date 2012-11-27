using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseLog : Item, ICommodity
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
			get { return 1.0; }
		}
		
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} {1} log" : "{0} {1} logs", Amount, CraftResources.GetName( m_Resource ).ToLower() );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
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
						case 301: info = OreInfo.Oak; break;
						case 302: info = OreInfo.Yew; break;
						case 303: info = OreInfo.Redwood; break;
						case 304: info = OreInfo.Ash; break;
						case 305: info = OreInfo.Greenheart; break;
						default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
		}

		public BaseLog( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseLog( CraftResource resource, int amount ) : base( 0x1BDD )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseLog( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1063516 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1063516 ); // logs
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
				if( m_Resource == CraftResource.Oak )
					return 1063517;
				
				if( m_Resource == CraftResource.Yew )
					return 1063518;
				
				if( m_Resource == CraftResource.Redwood )
					return 1063519;
				
				if( m_Resource == CraftResource.Ash )
					return 1063520;
				
				if( m_Resource == CraftResource.Greenheart )
					return 1063521;
				
				return 1063517;
			}
		}
	}
	
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class Log : BaseLog
	{
		[Constructable]
		public Log() : this( 1 )
		{
		}

		[Constructable]
		public Log( int amount ) : base( CraftResource.Oak, amount )
		{
			Weight = 1.0;
		}

		public Log( Serial serial ) : base( serial )
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
				Weight = 1.0;
		}
	}
	
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class YewLog : BaseLog
	{
		[Constructable]
		public YewLog() : this( 1 )
		{
		}

		[Constructable]
		public YewLog( int amount ) : base( CraftResource.Yew, amount )
		{
			Weight = 1.0;
		}

		public YewLog( Serial serial ) : base( serial )
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
				Weight = 1.0;
		}
	}
	
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class RedwoodLog : BaseLog
	{
		[Constructable]
		public RedwoodLog() : this( 1 )
		{
		}

		[Constructable]
		public RedwoodLog( int amount ) : base( CraftResource.Redwood, amount )
		{
			Weight = 1.0;
		}

		public RedwoodLog( Serial serial ) : base( serial )
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
				Weight = 1.0;
		}
	}
	
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class AshLog : BaseLog
	{
		[Constructable]
		public AshLog() : this( 1 )
		{
		}

		[Constructable]
		public AshLog( int amount ) : base( CraftResource.Ash, amount )
		{
			Weight = 1.0;
		}

		public AshLog( Serial serial ) : base( serial )
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
				Weight = 1.0;
		}
	}
	
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class GreenheartLog : BaseLog
	{
		[Constructable]
		public GreenheartLog() : this( 1 )
		{
		}

		[Constructable]
		public GreenheartLog( int amount ) : base( CraftResource.Greenheart, amount )
		{
			Weight = 1.0;
		}

		public GreenheartLog( Serial serial ) : base( serial )
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
				Weight = 1.0;
		}
	}
}
