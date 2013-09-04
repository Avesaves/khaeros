using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class Toxin : BasePotion
	{
		public static int MaxToxinDuration = 7*24*60*60;
		private int m_Duration;
		private int m_Corrosivity;
		private int m_ActingSpeed;
		private int m_PoisonDuration;
		private Dictionary<PoisonEffectEnum, int> m_Effects = new Dictionary<PoisonEffectEnum, int>(); // effect ID, intensity

		public Dictionary<PoisonEffectEnum, int> Effects { get { return m_Effects; } }

		public int Duration { get { return m_Duration; } set { m_Duration = value; } }
		public int PoisonDuration { get { return m_PoisonDuration; } set { m_PoisonDuration = value; } }
		public int Corrosivity { get { return m_Corrosivity; } set { m_Corrosivity = value; } }
		public int ActingSpeed { get { return m_ActingSpeed; } set { m_ActingSpeed = value; } }
		public override bool RequireFreeHand{ get{ return false; } }
		public override int LabelNumber{ get{ return 0; } }

		public Toxin( int itemID ) : base( itemID, PotionEffect.Nightsight )
		{
		}

		public Toxin( Serial serial ) : base( serial )
		{
		}
		
		public void AddEffect( PoisonEffectEnum ID, int intensity )
		{
			m_Effects.Add( ID, intensity );
		}

		public override void Drink( Mobile from )
		{
			from.SendMessage( "What will you apply this on?" );
			from.Target = new PickWeaponTarget( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060847, "{0}\t{1}", "Potion Type:", "Toxin" ); // ~1_val~ ~2_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( (int) m_PoisonDuration );
				
			writer.Write( (int) m_ActingSpeed );

			writer.Write( (int) m_Duration );

			writer.Write( (int) m_Corrosivity );
			
			writer.Write( (int) m_Effects.Count );

			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_Effects ) 
			{
				writer.Write( (int)kvp.Key );
				writer.Write( (int)kvp.Value );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_PoisonDuration = reader.ReadInt();
			
			m_ActingSpeed = reader.ReadInt();

			m_Duration = reader.ReadInt();

			m_Corrosivity = reader.ReadInt();
			
			int c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
				m_Effects.Add( (PoisonEffectEnum) reader.ReadInt(), reader.ReadInt() );
		}

		private class PickWeaponTarget : Target
		{
			private Toxin m_Potion;

			public PickWeaponTarget( Toxin potion ) : base( 15, false, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				Item item = targ as Item;
				if ( item == null )
				{
					from.SendMessage( "You cannot poison that." );
					return;
				}
				
				if ( item.RootParent != from )
				{
					from.SendMessage( "The item must be in your pack." );
					return;
				}
				
				OilAttachment attachment = XmlAttach.FindAttachment( item, typeof( OilAttachment ) ) as OilAttachment;
				PoisonedFoodAttachment attachment2 = XmlAttach.FindAttachment( item, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
				
				if ( attachment != null || attachment2 != null )
				{
					if ( item is BaseMeleeWeapon )
						from.SendMessage( "There's some substance present on that weapon already. Remove it with an oil cloth first." );
					else
						from.SendMessage( "There's some poisonous substance present on that item already." );
					
					return;
				}
				
				// passed all basic checks, now see what they're applying it to
				double skill = Math.Max( from.Skills[SkillName.Poisoning].Value, 0.1 );
				int actualOilDuration = (int)((skill/100.0) * m_Potion.m_Duration);
				ToxinAttachment toxAtt = new ToxinAttachment( m_Potion.Effects, m_Potion.PoisonDuration, m_Potion.ActingSpeed, actualOilDuration, m_Potion.Corrosivity );
				
				if ( (targ is BaseAxe || targ is BaseKnife || targ is BasePoleArm || targ is BaseSword || targ is BaseSpear) && !(targ is Boomerang) )
				{
					XmlAttach.AttachTo( targ, toxAtt );
					from.PlaySound( 0x4F );
					Bottle emptybottle = new Bottle();
					from.AddToBackpack( emptybottle );
					m_Potion.Consume( 1 );
					from.SendMessage( "You apply the toxin onto the weapon." );
				}
				else if ( targ is Food || targ is BaseBeverage || targ is DrinkPotion || targ is BlowGun )
				{
                    if( targ is BlowGun && ( (BlowGun)targ ).UsesRemaining < 1 )
                    {
                        from.SendMessage( "You cannot poison an empty blowgun." );
                        return;
                    }

					attachment2 = new PoisonedFoodAttachment( m_Potion.Effects, m_Potion.PoisonDuration, m_Potion.ActingSpeed, from );
					from.PlaySound( 0x4F );
					XmlAttach.AttachTo( item, attachment2 );
					from.SendMessage( "You apply the poison." );
					Bottle emptybottle = new Bottle();
					from.AddToBackpack( emptybottle );
					m_Potion.Consume( 1 );
				}
				else
					from.SendMessage( "You cannot poison that." );
			}
		}
	}
}
