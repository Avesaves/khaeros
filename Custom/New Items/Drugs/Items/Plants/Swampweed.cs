using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class Swampweed : BaseIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		public override int SkillRequired { get { return 500; } }

		[Constructable]
		public Swampweed( int amount ) : base( 3976, amount )
		{
			Name = "Swampweed";
			Hue = 251;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from is PlayerMobile && from.Backpack != null && IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "Target a piece of paper to produce a joint, or a pipe to fill." );
				from.Target = new PickTarget( this );
			}
		}

		[Constructable]
		public Swampweed() : this( 1 )
		{
		}

		public Swampweed( Serial serial ) : base( serial )
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
		
		private class PickTarget : Target
		{
			private Swampweed m_Weed;
			public PickTarget( Swampweed item ) : base( 15, false, TargetFlags.None )
			{
				m_Weed = item;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( from is PlayerMobile && from.Backpack != null && m_Weed.IsChildOf( from.Backpack ) )
				{
					if ( targ is OldPieceOfPaper || targ is PieceOfPaper || targ is RoughScroll || targ is OrnateScroll )
					{
						Item paper = targ as Item;
						if ( paper.IsChildOf( from.Backpack ) )
						{
							paper.Consume( 1 );
							m_Weed.Consume( 1 );
							StalkOfSwampweed stalk = new StalkOfSwampweed();
							from.Backpack.DropItem( stalk );
							from.SendMessage( "You roll a joint." );
						}
						else
							from.SendMessage( "That needs to be in your pack." );
					}
					else if ( targ is Pipe )
					{
						Pipe pipe = targ as Pipe;
						if ( pipe.RootParent == from )
						{
							if ( pipe.ContentRemaining > 0 )
								from.SendMessage( "That pipe still contains some substance. Empty it first." );
							else
							{
								m_Weed.Consume( 1 );
								pipe.ContentRemaining = 3;
								pipe.ContentType = ContentType.Swampweed;
								from.SendMessage( "You refill your pipe." );
							}
						}
						else
							from.SendMessage( "That needs to be in your pack." );
					}
					else
						from.SendMessage( "You must target some paper, or a smoking pipe." );
				}
			}
		}
	}
}
