using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class Tobacco : BaseIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		public override int SkillRequired { get { return 500; } }

		[Constructable]
		public Tobacco( int amount ) : base( 3193, amount )
		{
			Name = "tobacco";
			Hue = 2581;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from is PlayerMobile && from.Backpack != null && IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "Target paper to produce a cigarette, or a pipe to fill" );
				from.Target = new PickTarget( this );
			}
		}

		[Constructable]
		public Tobacco() : this( 1 )
		{
		}

		public Tobacco( Serial serial ) : base( serial )
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
			private Tobacco m_Tobacco;
			public PickTarget( Tobacco item ) : base( 15, false, TargetFlags.None )
			{
				m_Tobacco = item;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( from is PlayerMobile && from.Backpack != null && m_Tobacco.IsChildOf( from.Backpack ) )
				{
					if ( targ is OldPieceOfPaper || targ is PieceOfPaper || targ is RoughScroll || targ is OrnateScroll )
					{
						Item paper = targ as Item;
						if ( paper.IsChildOf( from.Backpack ) )
						{
							paper.Consume( 1 );
							paper.Consume( 1 );
							Cigarette cigarette = new Cigarette();
							from.Backpack.DropItem( cigarette );
							from.SendMessage( "You roll a cigarette." );
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
								m_Tobacco.Consume( 1 );
								pipe.ContentRemaining = 3;
								pipe.ContentType = ContentType.Tobacco;
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
