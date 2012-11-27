using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class OilPotion : CustomPotion
	{
		private int m_Duration;
		private int m_Corrosivity;

		public int Duration { get { return m_Duration; } set { m_Duration = value; } }
		public int Corrosivity { get { return m_Corrosivity; } set { m_Corrosivity = value; } }
		public override bool RequireFreeHand{ get{ return false; } }

		public OilPotion( int itemID ) : base( itemID )
		{
		}

		public OilPotion( Serial serial ) : base( serial )
		{
		}

		public override void Drink( Mobile from )
		{
			from.SendMessage( "What will you apply this on?" );
			from.Target = new PickWeaponTarget( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060847, "{0}\t{1}", "Potion Type:", "Oil" ); // ~1_val~ ~2_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Duration );

			writer.Write( (int) m_Corrosivity );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Duration = reader.ReadInt();

			m_Corrosivity = reader.ReadInt();
		}

		private class PickWeaponTarget : Target
		{
			private OilPotion m_Potion;

			public PickWeaponTarget( OilPotion potion ) : base( 15, false, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( targ is BaseMeleeWeapon )
				{
					BaseMeleeWeapon weapon = targ as BaseMeleeWeapon;
					OilAttachment attachment = XmlAttach.FindAttachment( weapon, typeof( OilAttachment ) ) as OilAttachment;
					if ( weapon.RootParent != from )
						from.SendMessage( "The item must be in your pack." );
					else if ( attachment != null )
						from.SendMessage( "There's already oil present on that weapon. Remove it with an oil cloth first." );
					else
					{
						attachment = new OilAttachment( m_Potion.Effects, m_Potion.Duration, m_Potion.Corrosivity );
						XmlAttach.AttachTo( weapon, attachment );
						from.PlaySound( 0x4F );
						Bottle emptybottle = new Bottle();
						from.AddToBackpack( emptybottle );
						m_Potion.Consume( 1 );
						from.SendMessage( "You apply the oil onto the weapon." );
					}
				}
				else
					from.SendMessage( "You cannot apply oil to that." );
			}
		}
	}
}
