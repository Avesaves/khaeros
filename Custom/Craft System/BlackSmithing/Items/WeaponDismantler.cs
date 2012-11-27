using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.Craft;

namespace Server.Items
{
    public class WeaponDismantler: Item
    {
        

        [Constructable]
        public WeaponDismantler()
            : base(0xFBB)
        {
            Name = "Weapon Repair Tool";
            Weight = 8.0;            
        }

        

        public WeaponDismantler(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new WeaponDismantlerTarget();
        }

        

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WeaponDismantlerTarget : Target
    {
        public WeaponDismantlerTarget(): base(2, false, TargetFlags.None)
        {
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is BaseWeapon))
                return;

            BaseWeapon weapon = targeted as BaseWeapon;

            if(weapon.MaxHitPoints > weapon.HitPoints && !(weapon is BaseRanged))
			{				
                from.SendMessage("You attempt to repair the weapon.");
				AttemptRepair( from, weapon );
			}
        }
		
		public void AttemptRepair(Mobile from, BaseWeapon weapon)
		{
			if (CheckAnvilAndForge( from ))
			{
				double difficulty = 40 + (weapon.MaxHitPoints - weapon.HitPoints) - (int)(from.Skills[SkillName.Blacksmith].Value / 10); 
				if (difficulty * 0.01 > Utility.RandomDouble())
				{
                    weapon.MaxHitPoints--;
					weapon.HitPoints--;

                    if ( weapon.MaxHitPoints < 1 )
                    {
                        weapon.Delete();
                        return;
                    }
				}
				difficulty = ((((weapon.MaxHitPoints - weapon.HitPoints) * 1250) / Math.Max( weapon.MaxHitPoints, 1 )) - 250) * 0.1;
				if (from.CheckSkill( SkillName.Blacksmith, difficulty - 25.0, difficulty + 25.0))
				{
					switch (weapon.Resource)
                            {
                                case CraftResource.Copper:
                                    if (from.Backpack.GetAmount(typeof(CopperIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough copper ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(CopperIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Bronze:
                                    if (from.Backpack.GetAmount(typeof(BronzeIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough bronze ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(BronzeIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Iron:
                                    if (from.Backpack.GetAmount(typeof(IronIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough iron ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(IronIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Steel:
                                    if (from.Backpack.GetAmount(typeof(SteelIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough steel ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(SteelIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Obsidian:
                                    if (from.Backpack.GetAmount(typeof(ObsidianIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough obsidian to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(ObsidianIngot), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Starmetal:
                                    if (from.Backpack.GetAmount(typeof(StarmetalIngot)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough starmetal ingots to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(StarmetalIngot), 10);
                                    break;
                                case CraftResource.Oak:
                                    if (from.Backpack.GetAmount(typeof(Log)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough oak logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(Log), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Redwood:
                                    if (from.Backpack.GetAmount(typeof(RedwoodLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough redwood logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(RedwoodLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Yew:
                                    if (from.Backpack.GetAmount(typeof(YewLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough yew logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(YewLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Ash:
                                    if (from.Backpack.GetAmount(typeof(AshLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough ash logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(AshLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
                                case CraftResource.Greenheart:
                                    if (from.Backpack.GetAmount(typeof(GreenheartLog)) < (int)((weapon.MaxHitPoints - weapon.HitPoints)/10))
                                    {
                                        from.SendMessage("You do not have enough greenheart logs to repair that.");
                                        return;
                                    }
                                    else
                                        from.Backpack.ConsumeUpTo(typeof(GreenheartLog), (int)((weapon.MaxHitPoints - weapon.HitPoints)/10));
                                    break;
							}
						from.SendLocalizedMessage(1044279); // You repair the item.
						weapon.HitPoints = weapon.MaxHitPoints;
				}
				else
					from.SendMessage("You fail to repair the item");
			}
			else
				from.SendMessage("You must be near a forge and anvil to do that.");
			
		}
		
		private static Type typeofAnvil = typeof(AnvilAttribute);
        private static Type typeofForge = typeof(ForgeAttribute);

        public static bool CheckAnvilAndForge(Mobile from)
        {
            bool anvil = false;
            bool forge = false;
            int range = 3;

            Map map = from.Map;

            if (map == null)
                return false;

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, range);

            foreach (Item item in eable)
            {
                Type type = item.GetType();

                bool isAnvil = (type.IsDefined(typeofAnvil, false) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6);
                bool isForge = (type.IsDefined(typeofForge, false) || item.ItemID == 4017 || (item.ItemID >= 6522 && item.ItemID <= 6569) || item.ItemID == 0x2DD8);

                if (isAnvil || isForge)
                {
                    if ((from.Z + 16) < item.Z || (item.Z + 16) < from.Z || !from.InLOS(item))
                        continue;

                    anvil = anvil || isAnvil;
                    forge = forge || isForge;

                    if (anvil && forge)
                        break;
                }
            }

            eable.Free();

            for (int x = -range; (!anvil || !forge) && x <= range; ++x)
            {
                for (int y = -range; (!anvil || !forge) && y <= range; ++y)
                {
                    Tile[] tiles = map.Tiles.GetStaticTiles(from.X + x, from.Y + y, true);

                    for (int i = 0; (!anvil || !forge) && i < tiles.Length; ++i)
                    {
                        int id = tiles[i].ID & 0x3FFF;

                        bool isAnvil = (id == 4015 || id == 4016 || id == 0x2DD5 || id == 0x2DD6);
                        bool isForge = (id == 4017 || (id >= 6522 && id <= 6569) || id == 0x2DD8);

                        if (isAnvil || isForge)
                        {
                            if ((from.Z + 16) < tiles[i].Z || (tiles[i].Z + 16) < from.Z || !from.InLOS(new Point3D(from.X + x, from.Y + y, tiles[i].Z + (tiles[i].Height / 2) + 1)))
                                continue;

                            anvil = anvil || isAnvil;
                            forge = forge || isForge;
                        }
                    }
                }
            }

            if (anvil && forge)
                return true;
            else
                return false;
        }
    }
}
