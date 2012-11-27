using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class DrinkPotion : CustomPotion
	{
		private bool m_Enhanced = false;
		public bool Enhanced { get { return m_Enhanced ; } set { m_Enhanced  = value; } }
		
		public DrinkPotion( int itemID ) : base( itemID )
		{
		}

		public DrinkPotion( Serial serial ) : base( serial )
		{
		}

		public override void Drink( Mobile from )
		{
			bool candrink = false;
			foreach ( KeyValuePair<CustomEffect, int> kvp in Effects )  // if none of the effects can be applied, the potion won't be drank
			{
				if ( kvp.Value > 0 ) // only effects count, not side-effects
				{
					CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
					if ( effect != null )
					{
						candrink = effect.CanDrink( from );
						if ( candrink == true )
							break;
					}
				}
			}

			if ( !candrink )
				from.SendMessage( "Drinking this potion would yield no effect in your current state." );
			else
			{
				PoisonedFoodAttachment attachment = XmlAttach.FindAttachment( this, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
				
				if ( attachment != null )
					attachment.OnConsumed( from );
				
				foreach ( KeyValuePair<CustomEffect, int> kvp in Effects ) 
				{
					CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
					if ( effect != null )
						effect.ApplyEffect( from, from, kvp.Value, this );
				}

				from.RevealingAction();
				from.PlaySound( 0x2D6 );
				Bottle emptybottle = new Bottle();
				from.AddToBackpack( emptybottle );

				if ( from.Body.IsHuman )
					from.Animate( 34, 5, 1, true, false, 0 );
				this.Consume( 1 );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			if ( m_Enhanced )
				list.Add( 1060847, "{0}\t{1}", "  Enhanced", " " ); // ~1_val~ ~2_val~
			list.Add( 1060658, "{0}\t{1}", "Potion Type", "Drink" ); // ~1_val~: ~2_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (bool) m_Enhanced );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Enhanced = reader.ReadBool();
		}
	}
}
