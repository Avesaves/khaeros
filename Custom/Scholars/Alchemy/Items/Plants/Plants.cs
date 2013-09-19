using System;
using Server;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Items
{
	public class Pusantia : BasePlant
	{
		public override Type Ingredient { get { return typeof( PusantiaRoot ); } }

		[Constructable]
		public Pusantia() : base( 3182 )
		{
			Hue = 1401;
			Name = "Pusantia";
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Pusantia"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }

		public Pusantia( Serial serial ) : base( serial )
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

	public class WillowBark : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedWillowBark ); } }

		[Constructable]
		public WillowBark() : base( 3671 )
		{
			Hue = 2410;
			Name = "Willow Bark";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "WillowBark"); */
                            

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public WillowBark( Serial serial ) : base( serial )
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

	public class WolfLichen : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedWolfLichen ); } }

		[Constructable]
		public WolfLichen() : base( 3392 )
		{
			Hue = 1196;
			Name = "Wolf Lichen";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "WolfLichen"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public WolfLichen( Serial serial ) : base( serial )
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

	public class Yarrow : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedYarrowFlowers ); } }

		[Constructable]
		public Yarrow() : base( 6810 )
		{
			Hue = 2985;
			Name = "Yarrow";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Yarrow"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Yarrow( Serial serial ) : base( serial )
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

	public class AlpineSorrel : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedAlpineSorrelPetals ); } }

		[Constructable]
		public AlpineSorrel() : base( 3205 )
		{
			Hue = 2841;
			Name = "Alpine Sorrel";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "AlpineSorrel"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public AlpineSorrel( Serial serial ) : base( serial )
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

	public class MyrrhaTree : BasePlant
	{
		public override Type Ingredient { get { return typeof( MyrrhResin ); } }

		[Constructable]
		public MyrrhaTree() : base( 3273 )
		{
			Hue = 1429;
			Name = "Myrrha Tree";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "MyrrhaTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public MyrrhaTree( Serial serial ) : base( serial )
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

	public class DesertSage : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedDesertSageLeaves ); } }

		[Constructable]
		public DesertSage() : base( 3332 )
		{
			Hue = 1072;
			Name = "Desert Sage";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "DesertSage"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public DesertSage( Serial serial ) : base( serial )
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

	public class Chia : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedChiaBuds ); } }

		[Constructable]
		public Chia() : base( 3156 )
		{
			Hue = 2754;
			Name = "Chia";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Chia"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Chia( Serial serial ) : base( serial )
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

	public class Cliffrose : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedCliffrosePlants ); } }

		[Constructable]
		public Cliffrose() : base( 3267 )
		{
			Hue = 2213;
			Name = "Cliffrose";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Cliffrose"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Cliffrose( Serial serial ) : base( serial )
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

	public class Datura : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedDaturaPetals ); } }

		[Constructable]
		public Datura() : base( 9037 )
		{
			Hue = 2985;
			Name = "Datura";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Datura"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Datura( Serial serial ) : base( serial )
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

	public class Dogbane : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedDogbanePlants ); } }

		[Constructable]
		public Dogbane() : base( 3208 )
		{
			Hue = 2698;
			Name = "Dogbane";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Dogbane"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Dogbane( Serial serial ) : base( serial )
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

	public class Agrimony : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedAgrimonyPetals ); } }

		[Constructable]
		public Agrimony() : base( 3204 )
		{
			Hue = 2698;
			Name = "Agrimony";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Agrimony"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Agrimony( Serial serial ) : base( serial )
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

	public class Echinacea : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedEchinaceaBuds ); } }

		[Constructable]
		public Echinacea() : base( 9035 )
		{
			Hue = 1378;
			Name = "Echinacea";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Echinacea"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Echinacea( Serial serial ) : base( serial )
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

	public class Goldenseal : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedGoldensealRoot ); } }

		[Constructable]
		public Goldenseal() : base( 6377 )
		{
			Hue = 0;
			Name = "Goldenseal";
		}

		public Goldenseal( Serial serial ) : base( serial )
		{
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Goldenseal"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
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

	public class Mullein : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedMulleinSeeds ); } }

		[Constructable]
		public Mullein() : base( 3210 )
		{
			Hue = 1410;
			Name = "Mullein";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Mullein"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Mullein( Serial serial ) : base( serial )
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

	public class SkullcapMushroom : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedSkullcapButtons ); } }

		[Constructable]
		public SkullcapMushroom() : base( 3350 )
		{
			Hue = 2591;
			Name = "Skullcap";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "SkullcapMushroom"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public SkullcapMushroom( Serial serial ) : base( serial )
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

	public class Hyssop : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedHyssopPetals ); } }

		[Constructable]
		public Hyssop() : base( 3256 )
		{
			Hue = 2735;
			Name = "Hyssop";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Hyssop"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Hyssop( Serial serial ) : base( serial )
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

	public class SphagnumMoss : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedSphagnumMoss ); } }

		[Constructable]
		public SphagnumMoss() : base( 6948 )
		{
			Hue = 2589;
			Name = "Sphagnum Moss";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "SphagnumMoss"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public SphagnumMoss( Serial serial ) : base( serial )
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

	public class MarshMallow : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedMarshMallowFlowers ); } }

		[Constructable]
		public MarshMallow() : base( 3336 )
		{
			Hue = 2856;
			Name = "Marsh Mallow";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "MarshMallow"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public MarshMallow( Serial serial ) : base( serial )
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

	public class GingkoTree : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedGingkoBerries ); } }

		[Constructable]
		public GingkoTree() : base( 3271 )
		{
			Hue = 0;
			Name = "Gingko Tree";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "GingkoTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public GingkoTree( Serial serial ) : base( serial )
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

	public class Ginger : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedGingerRoot ); } }

		[Constructable]
		public Ginger() : base( 9035 )
		{
			Hue = 1062;
			Name = "Ginger";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Ginger"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Ginger( Serial serial ) : base( serial )
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

	public class PinonTree : BasePlant
	{
		public override Type Ingredient { get { return typeof( PinonResin ); } }

		[Constructable]
		public PinonTree() : base( 3228 )
		{
			Hue = 2595;
			Name = "Pinon Tree";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "PinonTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public PinonTree( Serial serial ) : base( serial )
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

	public class CopalTree : BasePlant
	{
		public override Type Ingredient { get { return typeof( CopalGoldResin ); } }

		[Constructable]
		public CopalTree() : base( 3306 )
		{
			Hue = 2126;
			Name = "Copal Tree";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "CopalTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public CopalTree( Serial serial ) : base( serial )
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

	public class SacraTree : BasePlant
	{
		public override Type Ingredient { get { return typeof( FrankincenseResin ); } }

		[Constructable]
		public SacraTree() : base( 3273 )
		{
			Hue = 2210;
			Name = "Sacra Tree";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "SacraTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public SacraTree( Serial serial ) : base( serial )
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

	public class JuniperBush : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedJuniperBerries ); } }

		[Constructable]
		public JuniperBush() : base( 3271 )
		{
			Hue = 2212;
			Name = "Juniper Bush";
		}

		public JuniperBush( Serial serial ) : base( serial )
		{
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "JuniperBush"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
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

	public class RedValerian : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedRedValerianFlowers ); } }

		[Constructable]
		public RedValerian() : base( 3210 )
		{
			Hue = 2844;
			Name = "Red Valerian";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "RedValerian"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public RedValerian( Serial serial ) : base( serial )
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

	public class Belladonna : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedBelladonnaSeeds ); } }

		[Constructable]
		public Belladonna() : base( 7950 )
		{
			Hue = 0;
			Name = "Belladonna";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Belladonna"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Belladonna( Serial serial ) : base( serial )
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

	public class Comfrey : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedComfreyFlowers ); } }

		[Constructable]
		public Comfrey() : base( 6809 )
		{
			Hue = 218;
			Name = "Comfrey";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Comfrey"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Comfrey( Serial serial ) : base( serial )
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

	public class Purslane : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedPurslaneStems ); } }

		[Constructable]
		public Purslane() : base( 3267 )
		{
			Hue = 2959;
			Name = "Purslane";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Purslane"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Purslane( Serial serial ) : base( serial )
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

	public class Aloe : BasePlant
	{
		public override Type Ingredient { get { return typeof( FreshAloeStems ); } }

		[Constructable]
		public Aloe() : base( 3241 )
		{
			Hue = 2595;
			Name = "Aloe";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            int seedChance = ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            seedChance += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Farming);
                            seedChance += from.RawInt / 10;

                            /* Seed.PickPlantSeed(from, "Aloe"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Aloe( Serial serial ) : base( serial )
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

	public class Cinchona : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedCinchonaBuds ); } }

		[Constructable]
		public Cinchona() : base( 3255 )
		{
			Hue = 2602;
			Name = "Cinchona";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            int seedChance = ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            seedChance += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Farming);
                            seedChance += from.RawInt / 10;

                            /* Seed.PickPlantSeed(from, "Cinchona"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Cinchona( Serial serial ) : base( serial )
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

	public class Lousewort : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedLousewortPetals ); } }

		[Constructable]
		public Lousewort() : base( 3220 )
		{
			Hue = 2729;
			Name = "Lousewort";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Lousewort"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Lousewort( Serial serial ) : base( serial )
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

	public class BlueLily : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedBlueLilyFlowers ); } }

		[Constructable]
		public BlueLily() : base( 9037 )
		{
			Hue = 2961;
			Name = "Blue Lily";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "BlueLily"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public BlueLily( Serial serial ) : base( serial )
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

	public class CatsClaw : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedCatsClawSeeds ); } }

		[Constructable]
		public CatsClaw() : base( 3205 )
		{
			Hue = 2851;
			Name = "Cat's Claw";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "CatsClaw"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public CatsClaw( Serial serial ) : base( serial )
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

	public class Damiana : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedDamianaPetals ); } }

		[Constructable]
		public Damiana() : base( 7950 )
		{
			Hue = 773;
			Name = "Damiana";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Damiana"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Damiana( Serial serial ) : base( serial )
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

	public class Foxglove : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedFoxgloveFlowers ); } }

		[Constructable]
		public Foxglove() : base( 3204 )
		{
			Hue = 0;
			Name = "Foxglove";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Foxglove"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Foxglove( Serial serial ) : base( serial )
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

	public class Wormwood : BasePlant
	{
		public override Type Ingredient { get { return typeof( DriedWormwoodPlants ); } }

		[Constructable]
		public Wormwood() : base( 6811 )
		{
			Hue = 2960;
			Name = "Wormwood";
		}
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Wormwood"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
		public Wormwood( Serial serial ) : base( serial )
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

    public class Passionflower : BasePlant
    {
        public override Type Ingredient { get { return typeof(DriedPassionflowerLeaves); } }

        [Constructable]
        public Passionflower()
            : base(1001)
        {
            Name = "Passionflower";
            Hue = 1163;
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "Passionflower"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
        public Passionflower(Serial serial)
            : base(serial)
        {
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

    public class ChaulmoograTree : BasePlant
    {
        public override Type Ingredient { get { return typeof(DriedChaulmoograLeaves); } }

        [Constructable]
        public ChaulmoograTree()
            : base(Utility.RandomMinMax(9325, 9328))
        {
            Name = "Chaulmoogra Tree";
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "ChaulmoograTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
        public ChaulmoograTree(Serial serial)
            : base(serial)
        {
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

    public class CamphorTree : BasePlant
    {
        public override Type Ingredient { get { return typeof(CamphorWax); } }

        [Constructable]
        public CamphorTree()
            : base(12323)
        {
            Name = "CamphorTree";
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 1))
            {
                if (!from.Mounted)
                {
                    BaseIngredient ingredient = Activator.CreateInstance(Ingredient) as BaseIngredient;
                    if (ingredient != null)
                    {
                        int skillReq = ingredient.SkillRequired;
                        int herbalLore = from.Skills[(SkillName)36].Fixed;
                        if (herbalLore < skillReq || (ingredient is PusantiaRoot && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) < 1))
                        {
                            from.SendMessage("You have no chance of harvesting this crop.");
                            return;
                        }

                        Server.Spells.SpellHelper.Turn(from, this);
                        from.Animate(32, 5, 1, true, false, 0);
                        from.PlaySound(79);

                        double successChance = ((double)(herbalLore - skillReq + 500)) / 1000;
                        if (successChance > Utility.RandomDouble())
                        {
                            from.SendMessage("You skillfully remove the relevant ingredient from the plant.");
                            ingredient.Amount += ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.HerbalGathering);
                            from.AddToBackpack(ingredient);
                            ((PlayerMobile)from).Crafting = true;
                            Misc.LevelSystem.AwardMinimumXP((PlayerMobile)from, 1);
                            ((PlayerMobile)from).Crafting = false;
                            IsPlanted(from);

                            /* Seed.PickPlantSeed(from, "CamphorTree"); */

                            Delete();
                        }

                        else
                        {
                            from.SendMessage("You try to remove the ingredient from the plant, but end up only ruining it.");
                            IsPlanted(from);
                            Delete();
                        }
                    }
                }
                else
                    from.SendMessage("You can't do that while mounted.");
            }
            else
                from.SendMessage("You are too far away.");
        }
        public CamphorTree(Serial serial)
            : base(serial)
        {
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
}
