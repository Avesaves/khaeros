using System;
using Server.Mobiles;

namespace Server.Items
{
	public enum ItemType
	{
		Weapon,
		Armour,
		Clothing
	}
	
	public class ItemPiece : Item
	{
		private CraftResource m_Resource;
		private ItemType m_Type;
		private bool m_Masterwork;
		private bool m_Assembled;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set
			{
				if( GoodMatch( value, m_Type ) )
				{
					m_Resource = value;
					FixHue();
					InvalidateProperties();
				}
			}
		}
		
		public void FixHue()
		{
			Hue = CraftResources.GetResourceHue( m_Resource );
					
			if( m_Resource == CraftResource.Iron )
				Hue = 2401;
			
			else if( m_Resource == CraftResource.Cotton )
				Hue = 2594;
			
			else if( m_Resource == CraftResource.RegularLeather )
				Hue = 1730;
			
			else if( m_Resource == CraftResource.Oak )
				Hue = 1735;
			
			else if( m_Resource == CraftResource.ThickLeather )
				Hue = 2311;
			
			else if( m_Resource == CraftResource.BeastLeather )
				Hue = 2405;
			
			else if( m_Resource == CraftResource.ScaledLeather )
				Hue = 1445;
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public ItemType Type
		{
			get{ return m_Type; }
			set
			{
				if( !GoodMatch( m_Resource, value ) )
				{
					if( value == ItemType.Weapon || value == ItemType.Armour )
						m_Resource = CraftResource.Copper;
					
					else
						m_Resource = CraftResource.Cotton;
				}
				
				m_Type = value;
				FixHue();
				InvalidateProperties();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Masterwork{ get{ return m_Masterwork; } set{ m_Masterwork = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Assembled{ get{ return m_Assembled; } set{ m_Assembled = value; InvalidateProperties(); } }
		
		public static bool GoodMatch( CraftResource resource, ItemType type )
		{
			if( resource >= CraftResource.Copper && resource <= CraftResource.Starmetal && resource != CraftResource.Tin )
			{
				if( type == ItemType.Armour && resource == CraftResource.Obsidian )
					return false;
				
				if( type != ItemType.Clothing )
					return true;
			}
			
			if( resource >= CraftResource.RegularLeather && resource <= CraftResource.ScaledLeather && type == ItemType.Armour )
				return true;
			
			if( resource >= CraftResource.Oak && resource <= CraftResource.Greenheart && type != ItemType.Clothing )
				return true;
			
			if( resource >= CraftResource.Cotton && type == ItemType.Clothing )
				return true;
			
			return false;
		}
		
		[Constructable]
		public ItemPiece() : this( 1 )
		{
		}

		[Constructable]
		public ItemPiece( int amount ) : base( 0x19BA )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = Math.Min( amount, 5 );
            Name = "Pieces of a Destroyed Item";
            Resource = CraftResource.Copper;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile m = from as PlayerMobile;
			
			if( m.Feats.GetFeatLevel(FeatList.Masterwork) < 3 )
			{
				m.SendMessage( "In order to tinker with this, you need to have the third level of the Masterwork feat." );
				return;
			}
			
			if( !this.IsChildOf( from.Backpack ) )
			{
				m.SendMessage( "This needs to be in your backpack for you to use it." );
				return;
			}
			
			if( !Assembled && Amount > 4 )
				TryToAssemble( m );
			
			else if( Assembled )
				HandleUse( m );
		}
		
		public void TryToAssemble( PlayerMobile m )
		{
			if( !(m.Backpack != null && !m.Backpack.Deleted && m.Alive && !m.Paralyzed) )
				return;
			
			Type type = CraftResources.GetResourceType( this.Resource );
			
			if( m.Backpack.ConsumeTotal( type, 100 ) )
			{
				m.SendMessage( "You assemble the item." );
				this.Amount = 1;
				this.Assembled = true;
				this.Name = "Assembled Item Pieces";
				this.ItemID = 6584;
				this.InvalidateProperties();
				m.PlaySound( 79 );
			}
			
			else
				m.SendMessage( "You need 100 " + Commands.LevelSystemCommands.AddSpacesToString( type.ToString().Replace("Server.Items.", "") ) + " in order to assemble this item." );
		}
		
		public void HandleUse( PlayerMobile m )
		{
			if( this.Type == ItemType.Weapon && !m.Masterwork.HasWeaponPieces )
			{
				m.Masterwork.HasWeaponPieces = true;
				m.Masterwork.MasterworkWeapon = this.Masterwork;
				m.Masterwork.WeaponResource = this.Resource;
			}
			
			else if( this.Type == ItemType.Armour && !m.Masterwork.HasArmourPieces )
			{
				m.Masterwork.HasArmourPieces = true;
				m.Masterwork.MasterworkArmour = this.Masterwork;
				m.Masterwork.ArmourResource = this.Resource;
			}
			
			else if( this.Type == ItemType.Clothing && !m.Masterwork.HasClothingPieces )
			{
				m.Masterwork.HasClothingPieces = true;
				m.Masterwork.MasterworkClothing = this.Masterwork;
				m.Masterwork.ClothingResource = this.Resource;
			}
			
			else
			{
				m.SendMessage( "You already have an assembled item of this type waiting to be crafted." );
				return;
			}
			
			m.PlaySound( 87 );
			m.SendMessage( "You ready the item to be fixed. The next time you craft an exceptional item of the same type and resource, it will be converted into the refurbished piece you just used." );
			this.Delete();
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060661, "{0}\t{1}", "Resource", Commands.LevelSystemCommands.AddSpacesToString( m_Resource.ToString() ) ); // ~1_val~ ~2_val~
			list.Add( 1060658, "{0}\t{1}", "Type", m_Type.ToString() ); // ~1_val~ ~2_val~
			
			if( Masterwork )
				list.Add( 1060660, "{0}\t{1}", "Quality", "Masterwork" ); // ~1_val~ ~2_val~
			
			else
				list.Add( 1060660, "{0}\t{1}", "Quality", "Extraordinary" ); // ~1_val~ ~2_val~
			
			if( Assembled )
				list.Add( 1060659, "{0}\t{1}", "Status", "double-click to use" ); // ~1_val~ ~2_val~
				
			else if( Amount > 4 )
				list.Add( 1060659, "{0}\t{1}", "Status", "double-click to assemble" ); // ~1_val~ ~2_val~
			
			else
				list.Add( 1060659, "{0}\t{1}", "Status", "drop " + (5 - this.Amount).ToString() + " more pieces on this pile" ); // ~1_val~ ~2_val~
		}
		
		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( this.Name == null )
				list.Add( LabelNumber );

			else
				list.Add( this.Name );
		}
		
		public override void OnAfterDuped( Item newItem )
		{
			if( newItem is ItemPiece )
			{
				ItemPiece piece = newItem as ItemPiece;
				piece.Type = this.Type;
				piece.Resource = this.Resource;
				piece.Masterwork = this.Masterwork;
			}
		}

		public override bool StackWith( Mobile from, Item dropped, bool playSound )
		{
			if ( Stackable && dropped.Stackable && dropped.GetType() == GetType() && dropped.ItemID == ItemID && dropped.Hue == Hue )
			{
				if( (dropped.Amount + Amount) > 5 )
				{
					from.SendMessage( "You cannot stack more than five of these pieces together." );
					return false;
				}
				
				if( ((ItemPiece)dropped).Resource != this.Resource || ((ItemPiece)dropped).Type != this.Type )
				{
					from.SendMessage( "You cannot stack pieces of different types or resources." );
					return false;
				}
				
				if( ((ItemPiece)dropped).Assembled || this.Assembled )
				{
					from.SendMessage( "You cannot stack pieces that are already assembled." );
					return false;
				}
				
				if( ((ItemPiece)dropped).Masterwork != this.Masterwork )
				{
					from.SendMessage( "You cannot stack pieces of different quality." );
					return false;
				}
				
				if ( LootType != dropped.LootType )
					LootType = LootType.Regular;

				Amount += dropped.Amount;
				dropped.Delete();

				if ( playSound && from != null )
				{
					int soundID = GetDropSound();

					if ( soundID == -1 )
						soundID = 0x42;

					from.SendSound( soundID, GetWorldLocation() );
				}

				return true;
			}

			return false;
		}
		
		public ItemPiece( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (int) m_Resource );
			writer.Write( (int) m_Type );
			writer.Write( (bool) m_Masterwork );
			writer.Write( (bool) m_Assembled );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Resource = (CraftResource)reader.ReadInt();
			m_Type = (ItemType)reader.ReadInt();
			m_Masterwork = reader.ReadBool();
			m_Assembled = reader.ReadBool();
		}
	}
}
