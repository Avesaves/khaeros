using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseCloth : Item, ICommodity, IScissorable
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
			get { return 0.1; }
		}
		
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} {1} cloth" : "{0} {1} cloth", Amount, CraftResources.GetName( m_Resource ).ToLower() );
			}
		}
		
		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) ) return false;

			base.ScissorHelper( from, new Bandage(), 1 );

			return true;
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
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
						case 401: info = OreInfo.Cotton; break;
						case 402: info = OreInfo.Linen; break;
						default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
		}

		public BaseCloth( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseCloth( CraftResource resource, int amount ) : base( 0x1766 )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseCloth( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1063527 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1063527 ); // cloth
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
				if( m_Resource == CraftResource.Cotton )
					return 1063525;
				
				if( m_Resource == CraftResource.Linen )
					return 1063526;

                if( m_Resource == CraftResource.Satin )
                    return 1063531;

                if( m_Resource == CraftResource.Velvet )
                    return 1063543;

                if( m_Resource == CraftResource.Wool )
                    return 1063545;
				
				return 1063525;
			}
		}
	}
	
	[FlipableAttribute( 0x1766, 0x1768 )]
	public class Cloth : BaseCloth
	{
		[Constructable]
		public Cloth() : this( 1 )
		{
		}

		[Constructable]
		public Cloth( int amount ) : base( CraftResource.Cotton, amount )
		{
		}

		public Cloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute( 0x1766, 0x1768 )]
	public class Linen : BaseCloth
	{
		[Constructable]
		public Linen() : this( 1 )
		{
		}

		[Constructable]
		public Linen( int amount ) : base( CraftResource.Linen, amount )
		{
		}

		public Linen( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    [FlipableAttribute( 0x1766, 0x1768 )]
    public class Satin : BaseCloth
    {
        [Constructable]
        public Satin()
            : this( 1 )
        {
        }

        [Constructable]
        public Satin( int amount )
            : base( CraftResource.Satin, amount )
        {
        }

        public Satin( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( from != null && this.RootParentEntity == from && from.Backpack != null && !from.Backpack.Deleted && from.Backpack is BaseContainer )
            {
                this.Consume( 1 );
                from.SendMessage( "You process the satin and turn it into velvet." );
                ( (BaseContainer)from.Backpack ).DropAndStack( new Velvet() );
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    [FlipableAttribute( 0x1766, 0x1768 )]
    public class Velvet : BaseCloth
    {
        [Constructable]
        public Velvet()
            : this( 1 )
        {
        }

        [Constructable]
        public Velvet( int amount )
            : base( CraftResource.Velvet, amount )
        {
        }

        public Velvet( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( from != null && this.RootParentEntity == from && from.Backpack != null && !from.Backpack.Deleted && from.Backpack is BaseContainer )
            {
                this.Consume( 1 );
                from.SendMessage( "You process the velvet and turn it back into silk." );
                ( (BaseContainer)from.Backpack ).DropAndStack( new SpidersSilk( 10 ) );
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
