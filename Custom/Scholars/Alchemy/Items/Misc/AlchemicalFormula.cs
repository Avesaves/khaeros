using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class AlchemicalFormula : Item
	{
		private string m_PotionName = null;
		private PotionType m_PotionType;
		private Type[] m_IngredientTypes = new Type [10];
		private int m_Bottle = 3626;

		public int Bottle{ get{ return m_Bottle; } set { m_Bottle = value; } }
		public string PotionName{ get{ return m_PotionName; } set { m_PotionName = value; } }
		public PotionType PotionType{ get{ return m_PotionType; } set { m_PotionType = value; } }
		public Type[] IngredientTypes{ get{ return m_IngredientTypes; } }

		[Constructable]
		public AlchemicalFormula() : base( 0x14F0 )
		{
			Name = "blank alchemical formula";
			Hue = 884;
			Weight = 1.0;
		}

		public AlchemicalFormula( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from )
			{
				// build the list
				Dictionary<Type, int> ingredients = new Dictionary<Type, int>();
				foreach ( Type type in m_IngredientTypes )
				{
					if ( type != null )
					{
						if ( !ingredients.ContainsKey(type) )
							ingredients[type] = 1;
						else
							ingredients[type]++;
					}
				}

				// build the message
				string message = "";
				foreach ( KeyValuePair<Type, int> kvp in ingredients )
					message += (message == "" ? " " : ", ") + ((Item)Activator.CreateInstance(kvp.Key)).Name + " (" + kvp.Value + ")";

				if ( message == "" )
					message = "This formula is blank.";
				else
					message = "The ingredients listed in this formula are:" + message + ".";
				from.SendMessage( 35, message );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
				
			writer.Write( (int) m_Bottle );

			writer.Write( (string) m_PotionName );
			writer.Write( (int) m_PotionType );

			foreach ( Type element in m_IngredientTypes )
			{
				if ( element == null )
					writer.Write( (string) "" );
				else
					writer.Write( (string) element.ToString() );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 1:
				{
					m_Bottle = reader.ReadInt();
					goto case 0;
				}
				
				case 0:
				{
					m_PotionName = reader.ReadString();
					m_PotionType = (PotionType) reader.ReadInt();

					for ( int i=0; i<m_IngredientTypes.Length; i++ )
					{
						string str = reader.ReadString();
						if ( str != "" )
							m_IngredientTypes[i] = Type.GetType( str );
					}
					
					break;
				}
			}
		}
	}
}
