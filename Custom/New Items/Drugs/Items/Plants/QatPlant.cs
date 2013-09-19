using System;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Items
{
    public class QatPlant : BasePlant
    {
        public override Type Ingredient { get { return typeof(QatLeaves); } }

        [Constructable]
        public QatPlant()
            : base(0x0897)
        {
            Hue = 1445;
            Name = "qat plant";
            Movable = false;
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

                            /* Seed.PickPlantSeed(from, "QatPlant"); */

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
        public QatPlant(Serial serial)
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
