using System;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Items
{
	public class SwampweedPlant : BasePlant
	{
		public override Type Ingredient { get { return typeof( Swampweed ); } }

		[Constructable]
		public SwampweedPlant() : base( 3157 )
		{
			Hue = 256;
			Name = "Swampweed Plant";
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

                            /* Seed.PickPlantSeed(from, "SwampweedPlant"); */

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
		public SwampweedPlant( Serial serial ) : base( serial )
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
}
