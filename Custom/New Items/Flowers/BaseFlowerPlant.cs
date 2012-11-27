using System;
using Server;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseFlowerPlant : BasePlant
	{
		public BaseFlowerPlant( int itemID ) : this( itemID, 1 )
		{
		}

		public BaseFlowerPlant( int itemID, int amount ) : base( itemID, amount )
		{
		}

		public BaseFlowerPlant( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( from.InRange( this.Location, 1 ) )
			{
				if ( !from.Mounted )
				{
					Flower flower = Activator.CreateInstance( Ingredient ) as Flower;
					if ( flower != null )
					{
						Server.Spells.SpellHelper.Turn( from, this );
						from.Animate( 32, 5, 1, true, false, 0 );
						from.PlaySound( 79 );

						from.SendMessage( "You have picked the flower." );
						flower.Hue = this.Hue;
						from.AddToBackpack( flower );
						((PlayerMobile)from).Crafting = true;	
						Misc.LevelSystem.AwardMinimumXP( (PlayerMobile)from, 1 );
						((PlayerMobile)from).Crafting = false;
						Delete();
					}
				}
				else
					from.SendMessage( "You can't do that while mounted." );
			}
			else
				from.SendMessage( "You are too far away." );
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
}