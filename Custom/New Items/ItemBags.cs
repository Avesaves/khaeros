using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;

namespace Server.Items
{ 
	public class WeaponBag : Bag 
	{
        public static List<Type> Types = GetAllClassesFrom( "Server.Items", typeof(BaseWeapon) );

        public static List<Type> GetAllClassesFrom( string nameSpace, Type toFind )
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<Type> list = new List<Type>();

            foreach( Type type in asm.GetTypes() )
            {
                if( type.Namespace == nameSpace && type.IsClass && type.IsSubclassOf( toFind ) )
                {
                    ConstructorInfo[] ctors = type.GetConstructors();
                    bool foundCtor = false;

                    for( int i = 0; i < ctors.Length; ++i )
                    {
                        ConstructorInfo ctor = ctors[i];

                        if( !Commands.Add.IsConstructable( ctor ) )
                            continue;

                        foundCtor = true;
                        break;
                    }

                    if( foundCtor )
                        list.Add( type );
                }
            }

            return list;
        }

        public void AddItems()
        {
            foreach( Type type in Types )
            {
                Item item = (Item)Activator.CreateInstance( type );
                DropItem( item );
            }
        }

		[Constructable] 
		public WeaponBag() 
		{
            AddItems();
		} 

		public WeaponBag( Serial serial ) : base( serial ) 
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

    public class ClothingBag : Bag
    {
        public static List<Type> Types = WeaponBag.GetAllClassesFrom( "Server.Items", typeof( BaseClothing ) );

        public void AddItems()
        {
            foreach( Type type in Types )
            {
                Item item = (Item)Activator.CreateInstance( type );
                DropItem( item );
            }
        }

        [Constructable]
        public ClothingBag()
        {
            AddItems();
        }

        public ClothingBag( Serial serial )
            : base( serial )
        {
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

    public class ArmourBag : Bag
    {
        public static List<Type> Types = WeaponBag.GetAllClassesFrom( "Server.Items", typeof( BaseArmor ) );

        public void AddItems()
        {
            foreach( Type type in Types )
            {
                Item item = (Item)Activator.CreateInstance( type );
                DropItem( item );
            }
        }

        [Constructable]
        public ArmourBag()
        {
            AddItems();
        }

        public ArmourBag( Serial serial )
            : base( serial )
        {
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
