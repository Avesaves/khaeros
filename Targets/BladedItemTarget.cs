using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Targets
{
	public class BladedItemTarget : Target
	{
		private Item m_Item;

		public BladedItemTarget( Item item ) : base( 2, false, TargetFlags.None )
		{
			m_Item = item;
		}

		protected override void OnTargetOutOfRange( Mobile from, object targeted )
		{
			if ( targeted is UnholyBone && from.InRange( ((UnholyBone)targeted), 12 ) )
				((UnholyBone)targeted).Carve( from, m_Item );
			else
				base.OnTargetOutOfRange (from, targeted);
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_Item.Deleted )
				return;

            if (m_Item is Dagger && targeted is Dagger && m_Item != targeted)
            {
                Dagger m_Hilt = m_Item as Dagger;
                Dagger m_Blade = targeted as Dagger;
                DualDaggers weapon = new DualDaggers();
                weapon.NewCrafting = true;
                weapon.QualityDamage = (int)(m_Hilt.QualityDamage * 0.5 + m_Blade.QualityDamage * 0.5);
                weapon.QualitySpeed = (int)(m_Hilt.QualitySpeed * 0.5 + m_Blade.QualitySpeed * 0.5);
                weapon.QualityAccuracy = (int)(m_Hilt.QualityAccuracy * 0.5 + m_Blade.QualityAccuracy * 0.5);
                weapon.QualityDefense = (int)(m_Hilt.QualityDefense * 0.5 + m_Blade.QualityDefense * 0.5);
                weapon.Resource = m_Hilt.Resource;
                int quality = (int)(m_Blade.MaxHitPoints * 0.5 + m_Hilt.MaxHitPoints * 0.5);
                weapon.MaxHitPoints = quality;
                quality = (int)(m_Blade.HitPoints * 0.5 + m_Hilt.HitPoints * 0.5);
                weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(((int)m_Blade.Quality * 50) * 0.5 + ((int)m_Hilt.Quality * 50) * 0.5);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Antique;
                if (quality < 400)
                    weapon.Quality = WeaponQuality.Extraordinary;
                if (quality < 350)
                    weapon.Quality = WeaponQuality.Remarkable;
                if (quality < 300)
                    weapon.Quality = WeaponQuality.Exceptional;
                if (quality < 250)
                    weapon.Quality = WeaponQuality.Superior;
                if (quality < 200)
                    weapon.Quality = WeaponQuality.Regular;
                if (quality < 150)
                    weapon.Quality = WeaponQuality.Inferior;
                if (quality < 100)
                    weapon.Quality = WeaponQuality.Low;
                if (quality < 50)
                    weapon.Quality = WeaponQuality.Poor;
                weapon.BetaNerf = true;
                weapon.InvalidateProperties();
                from.AddToBackpack(weapon);
                weapon.AddItem(m_Blade);
                weapon.AddItem(m_Hilt);
            }

            if (m_Item is HandScythe && targeted is HandScythe && m_Item != targeted)
            {
                HandScythe m_Hilt = m_Item as HandScythe;
                HandScythe m_Blade = targeted as HandScythe;
                DualPicks weapon = new DualPicks();
                weapon.NewCrafting = true;
                weapon.QualityDamage = (int)(m_Hilt.QualityDamage * 0.5 + m_Blade.QualityDamage * 0.5);
                weapon.QualitySpeed = (int)(m_Hilt.QualitySpeed * 0.5 + m_Blade.QualitySpeed * 0.5);
                weapon.QualityAccuracy = (int)(m_Hilt.QualityAccuracy * 0.5 + m_Blade.QualityAccuracy * 0.5);
                weapon.QualityDefense = (int)(m_Hilt.QualityDefense * 0.5 + m_Blade.QualityDefense * 0.5);
                weapon.Resource = m_Hilt.Resource;
                int quality = (int)(m_Blade.MaxHitPoints * 0.5 + m_Hilt.MaxHitPoints * 0.5);
                weapon.MaxHitPoints = quality;
                quality = (int)(m_Blade.HitPoints * 0.5 + m_Hilt.HitPoints * 0.5);
                weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(((int)m_Blade.Quality * 50) * 0.5 + ((int)m_Hilt.Quality * 50) * 0.5);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Antique;
                if (quality < 400)
                    weapon.Quality = WeaponQuality.Extraordinary;
                if (quality < 350)
                    weapon.Quality = WeaponQuality.Remarkable;
                if (quality < 300)
                    weapon.Quality = WeaponQuality.Exceptional;
                if (quality < 250)
                    weapon.Quality = WeaponQuality.Superior;
                if (quality < 200)
                    weapon.Quality = WeaponQuality.Regular;
                if (quality < 150)
                    weapon.Quality = WeaponQuality.Inferior;
                if (quality < 100)
                    weapon.Quality = WeaponQuality.Low;
                if (quality < 50)
                    weapon.Quality = WeaponQuality.Poor;
                weapon.BetaNerf = true;
                weapon.InvalidateProperties();
                from.AddToBackpack(weapon);
                weapon.AddItem(m_Blade);
                weapon.AddItem(m_Hilt);
            }

            if (m_Item is Longsword && targeted is Longsword && m_Item != targeted)
            {
                Longsword m_Hilt = m_Item as Longsword;
                Longsword m_Blade = targeted as Longsword;
                DualSwords weapon = new DualSwords();
                weapon.NewCrafting = true;
                weapon.QualityDamage = (int)(m_Hilt.QualityDamage * 0.5 + m_Blade.QualityDamage * 0.5);
                weapon.QualitySpeed = (int)(m_Hilt.QualitySpeed * 0.5 + m_Blade.QualitySpeed * 0.5);
                weapon.QualityAccuracy = (int)(m_Hilt.QualityAccuracy * 0.5 + m_Blade.QualityAccuracy * 0.5);
                weapon.QualityDefense = (int)(m_Hilt.QualityDefense * 0.5 + m_Blade.QualityDefense * 0.5);
                weapon.Resource = m_Hilt.Resource;
                int quality = (int)(m_Blade.MaxHitPoints * 0.5 + m_Hilt.MaxHitPoints * 0.5);
                weapon.MaxHitPoints = quality;
                quality = (int)(m_Blade.HitPoints * 0.5 + m_Hilt.HitPoints * 0.5);
                weapon.HitPoints = quality;
                weapon.Crafter = from;
                weapon.CraftersOriginalName = from.Name;
                quality = (int)(((int)m_Blade.Quality * 50) * 0.5 + ((int)m_Hilt.Quality * 50) * 0.5);
                if (quality == 500)
                    weapon.Quality = WeaponQuality.Legendary;
                if (quality < 500)
                    weapon.Quality = WeaponQuality.Masterwork;
                if (quality < 450)
                    weapon.Quality = WeaponQuality.Antique;
                if (quality < 400)
                    weapon.Quality = WeaponQuality.Extraordinary;
                if (quality < 350)
                    weapon.Quality = WeaponQuality.Remarkable;
                if (quality < 300)
                    weapon.Quality = WeaponQuality.Exceptional;
                if (quality < 250)
                    weapon.Quality = WeaponQuality.Superior;
                if (quality < 200)
                    weapon.Quality = WeaponQuality.Regular;
                if (quality < 150)
                    weapon.Quality = WeaponQuality.Inferior;
                if (quality < 100)
                    weapon.Quality = WeaponQuality.Low;
                if (quality < 50)
                    weapon.Quality = WeaponQuality.Poor;
                weapon.BetaNerf = true;
                weapon.InvalidateProperties();
                from.AddToBackpack(weapon);
                weapon.AddItem(m_Blade);
                weapon.AddItem(m_Hilt);
            }

			if ( targeted is ICarvable )
			{		
				((ICarvable)targeted).Carve( from, m_Item );
			}
			
			else if ( targeted is BaseClothing && ( (Item)targeted ).IsChildOf( from.Backpack ) )
			{
				BaseClothing clothes = targeted as BaseClothing;
				
				Bandage bandage = new Bandage( 2 );
				bandage.Amount += Math.Max( 0, (int)clothes.Weight );
				bandage.Hue = clothes.Hue;
				
				Container pack = from.Backpack;
				
				pack.DropItem( bandage );
				
				from.SendMessage( "You rip the garment apart and create some bandages out of it" );
				
				clothes.Delete();
			}			
			
			else
			{
				HarvestSystem system = Lumberjacking.System;
				HarvestDefinition def = Lumberjacking.System.Definition;

				int tileID;
				Map map;
				Point3D loc;

				if ( !system.GetHarvestDetails( from, m_Item, targeted, out tileID, out map, out loc ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else if ( !def.Validate( tileID ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else
				{
					HarvestBank bank = def.GetBank( map, loc.X, loc.Y );

					if ( bank == null )
						return;

					if ( bank.Current < 5 )
					{
						from.SendLocalizedMessage( 500493 ); // There's not enough wood here to harvest.
					}
					else
					{
						bank.Consume( def, 5 );

						Item item = new Kindling();

						if ( from.PlaceInBackpack( item ) )
						{
							from.SendLocalizedMessage( 500491 ); // You put some kindling into your backpack.
							from.SendLocalizedMessage( 500492 ); // An axe would probably get you more wood.
						}
						else
						{
							from.SendLocalizedMessage( 500490 ); // You can't place any kindling into your backpack!

							item.Delete();
						}
					}
				}
			}
		}
	}
}
