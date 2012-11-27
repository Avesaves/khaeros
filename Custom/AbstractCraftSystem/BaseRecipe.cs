using System;
using System.Collections.Generic;

namespace Server.Items
{
	public abstract class BaseRecipe : Item
	{
		private Type[] m_ComponentTypes;

		public Type[] ComponentTypes{ get{ return m_ComponentTypes; } set{ m_ComponentTypes = value; } }

		[Constructable]
		public BaseRecipe( int itemID ) : base( itemID )
		{
		}

		public BaseRecipe( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from )
			{
				// build the list
				Dictionary<Type, int> components = new Dictionary<Type, int>();
				if ( m_ComponentTypes != null )
				{
					foreach ( Type type in m_ComponentTypes )
					{
						if ( type != null )
						{
							if ( !components.ContainsKey(type) )
								components[type] = 1;
							else
								components[type]++;
						}
					}
				}

				// build the message
				string message = "";
				foreach ( KeyValuePair<Type, int> kvp in components )
					message += (message == "" ? " " : ", ") + ((Item)Activator.CreateInstance(kvp.Key)).Name + " (" + kvp.Value + ")";

				if ( message == "" )
					message = "This recipe is blank.";
				else
					message = "The components listed in this recipe are:" + message + ".";
				from.SendMessage( 35, message );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			if ( m_ComponentTypes == null )
				writer.Write( 0 );
			else
			{
				writer.Write( m_ComponentTypes.Length );
				foreach ( Type element in m_ComponentTypes )
				{
					if ( element == null )
						writer.Write( (string) "" );
					else
						writer.Write( (string) element.ToString() );
				}
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 0:
				{
					int length = reader.ReadInt();
					if ( length > 0 )
						m_ComponentTypes = new Type[length];
					for ( int i=0; i<length; i++ )
					{
						string str = reader.ReadString();
						if ( str != "" )
							m_ComponentTypes[i] = Type.GetType( str );
					}
					
					break;
				}
			}
		}
	}
}
