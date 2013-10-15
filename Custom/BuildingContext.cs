using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Engines.XmlSpawner2;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public enum BuildingQuality
        {
            Awful = -1,
            Poor = 0,
            Average = 1,
            Exceptional = 2,
            Excellent = 3
        }

    [PropertyObject]
    public class BuildingContext
    {
        #region Variable Declaration & Get/Set
        private PlayerMobile m_Build;
        private string m_Name;
        private int m_ID;
        private int m_Hue;
        private List<Item> m_Ingredients = new List<Item>();
        private BuildingQuality m_Quality;
        private BaseTool m_Tool;

        public PlayerMobile Build { get { return m_Build; } set { m_Build = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public int ID { get { return m_ID; } set { m_ID = value; if (m_ID < 2) m_ID = 2; if (m_ID > 16382) m_ID = 16382; } }
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public List<Item> Ingredients { get { return m_Ingredients; } set { m_Ingredients = value; } }
        public BuildingQuality Quality { get { return m_Quality; } set { m_Quality = value; } }
        public BaseTool Tool { get { return m_Tool; } set { m_Tool = value; } }
        #endregion

        public BuildingContext(PlayerMobile Build, BaseTool tool)
        {
            m_Build = Build;

            m_Name = "An Unnamed Object";
            m_ID = 3896;
            m_Hue = 0;
            m_Tool = tool;
        }

        public void AddIngredient(Item ing)
        {
            if (m_Build == null || m_Build.Deleted || !m_Build.Alive)            
                return;
            
            if (ing == null || ing.Deleted)
            {
                Console.WriteLine("Attempt to add material failed; object no longer exists.");
                m_Build.SendGump(new BuildingGump(m_Build, this));
                return;
            }

            //Adding to the list of ingredients.
            if ( IsValidIngredient(ing) && (m_Build.Backpack.Items.Contains(ing) || m_Build.InRange(ing.Location, 1)))
            {
                if (m_Ingredients.Contains(ing))
                    m_Build.SendMessage("You have already added this.");
                else
                    m_Ingredients.Add(ing);

                m_Build.SendGump(new BuildingGump(m_Build, this));
            }
            else
            {
                m_Build.SendMessage("You can only add rare materials!");
                m_Build.SendGump(new BuildingGump(m_Build, this));
                return;
            }
        }

        public static bool IsValidIngredient(Item ing)
        {
            if (ing == null)
                return false;
            if (ing.Deleted)
                return false;

            if(ing is RewardToken)
                return true;
            
            return false;
        }
        
        public static void BuildMeal(BuildingContext context)
        {
            if (context.Build == null || context.Build.Deleted || !context.Build.Alive)
                return;

            if (context.Ingredients.Count < 1)
            {
                context.Build.SendMessage("There are no materials in this object.");
                return;
            }

            int quality = Utility.RandomMinMax(-4, 4);
            quality += context.Build.Feats.GetFeatLevel(FeatList.Craftsmanship);
            if (quality > 3)
                quality = 3;
            context.Quality = (BuildingQuality)quality;

            if (Utility.RandomMinMax(1, 100) < (context.Build.Feats.GetFeatLevel(FeatList.Craftsmanship) * 33))
            {
                List<string> ingredientNames = new List<string>();
                XmlAttachment newPoison = null;
                foreach (Item i in context.Ingredients)
                {
                    if (i != null && !i.Deleted)
                    {
                        if (newPoison == null || newPoison.Deleted)
                        {
                            XmlAttachment thisPoison = XmlAttach.FindAttachment(i, typeof(PoisonAttachment));
                            if(thisPoison != null && !thisPoison.Deleted)
                                newPoison = new PoisonAttachment((thisPoison as PoisonAttachment).Effects, (thisPoison as PoisonAttachment).Duration, (thisPoison as PoisonAttachment).ActingSpeed, (thisPoison as PoisonAttachment).Poisoner);

                            if (thisPoison == null)
                            {
                                thisPoison = XmlAttach.FindAttachment(i, typeof(PoisonedFoodAttachment));
                                if (thisPoison != null && !thisPoison.Deleted)
                                    newPoison = new PoisonedFoodAttachment((thisPoison as PoisonedFoodAttachment).Effects, (thisPoison as PoisonedFoodAttachment).PoisonDuration, (thisPoison as PoisonedFoodAttachment).PoisonActingSpeed, (thisPoison as PoisonedFoodAttachment).Poisoner);
                            }
                        }

                        if (context.Build.InRange(i.Location, 1) || context.Build.Backpack.Items.Contains(i))
                        {
                            if (i is Pitcher)
                            {
                                if ((i as Pitcher).Quantity > 0 && !(i as Pitcher).IsEmpty)
                                {
                                    ingredientNames.Add((i as Pitcher).Content.ToString());
                                    (i as Pitcher).Quantity--;
                                }
                            }
                            else if (i is SackFlour)
                            {
                                if ((i as SackFlour).Quantity > 0)
                                {
                                    ingredientNames.Add("Flour");
                                    (i as SackFlour).Quantity--;
                                }
                            }
                            else if (i is IHasQuantity)
                            {
                                IHasQuantity itemWithQuantity = i as IHasQuantity;
                                if (itemWithQuantity.Quantity > 0)
                                {
                                    RemoveQuantity(itemWithQuantity);
                                    ingredientNames.Add(RemoveBagOfPrefix(i.Name));
                                }
                            }
                            else
                            {
                                if (i.Name != null)
                                    ingredientNames.Add(i.Name);
                                else
                                {
                                    string iName = i.GetType().Name;
                                    ingredientNames.Add(iName);
                                }
                                i.Consume();
                            }
                        }                        
                    }
                    else
                    {
                        context.Build.SendMessage("You do not have all the materials for this.");
                        return;
                    }
                }

                context.Build.PlaySound(0x4b);
                context.Build.PlaySound(0x42);
                CustomBuild newBuild = new CustomBuild(context.Build, context.Name, context.ID, context.Hue, ingredientNames, context.Quality);

                if (newPoison != null && !newPoison.Deleted)
                {
                    XmlAttach.AttachTo(newBuild, newPoison);
                }

                context.Build.AddToBackpack(newBuild);
                context.Build.SendMessage("You have successfully crafted " + newBuild.Name.ToString() + "!");
                context.Build.Crafting = true;
/*                 Misc.LevelSystem.AwardBuildingXP(context.Build); */
                context.Build.Crafting = false;
                context.Tool.UsesRemaining--;
                if (context.Tool.UsesRemaining < 1)
                {
                    context.Build.SendMessage("Your " + context.Tool.Name + " have been used up.");
                    context.Tool.Delete();
                }
                else
                {
                    PlayerMobile pm = context.Build as PlayerMobile;
                    pm.SendGump(new BuildingGump(pm, context));
                }
            }
            else
            {
                context.Build.SendMessage("You have failed and destroyed your materials!");
                List<Item> removeList = new List<Item>();
                foreach (Item i in context.Ingredients)
                {
                    if (i != null && !i.Deleted)
                    {
                        if (context.Build.InRange(i.Location, 1) || context.Build.Backpack.Items.Contains(i))
                        {
                            i.Consume();
                        }
                    }
                }
                return;
            }
        }

        static void RemoveQuantity(IHasQuantity item)
        {
            item.Quantity--;
        }

        static string RemoveBagOfPrefix(string item)
        {
            return item.Replace("Bag of ", String.Empty);
        }
    }

    public class CustomBuild : Item
    {
        private List<string> m_Ingredients = new List<string>();
        private BuildingQuality m_Quality;
        private Mobile m_Crafter;
        private bool m_IsDrink = false;

        [CommandProperty (AccessLevel.GameMaster)]
        public Mobile Crafter { get { return m_Crafter; } set { m_Crafter = value; } }
        public BuildingQuality Quality { get { return m_Quality; } set { m_Quality = value; } }


        public CustomBuild(Mobile crafter, string name, int itemid, int hue, List<string> ingredients, BuildingQuality quality)
            : base(itemid)
        {
            m_Crafter = crafter;
            Name = name;
            Hue = hue;
            m_Ingredients = ingredients;
            m_Quality = quality;
            Weight = m_Ingredients.Count;
            Stackable = false;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            Dictionary<string, int> AddToPropsList = new Dictionary<string,int>();

            foreach (string ingredient in m_Ingredients)
            {
                if (AddToPropsList.ContainsKey(ingredient))
                    AddToPropsList[ingredient]++;
                else
                    AddToPropsList.Add(ingredient, 1);
            }

            string propString = ("Quality:" + " " + m_Quality.ToString());

            foreach (KeyValuePair<string, int> kvp in AddToPropsList)
            {
                propString += "\n ";
                if (kvp.Value > 1)
                    propString += ((kvp.Value.ToString()) + " " + kvp.Key.ToString());
                else
                    propString += (kvp.Key.ToString());
            }
            list.Add(propString);
        }

        public override bool StackWith(Mobile from, Item dropped)
        {
            return false;
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            return false;
        }

        public CustomBuild(Serial serial)
            : base(serial)
        {

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_IsDrink = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Ingredients = new List<string>();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            string iName = reader.ReadString();
                            m_Ingredients.Add(iName);
                        }
                        m_Quality = (BuildingQuality)reader.ReadInt();
                        m_Crafter = reader.ReadMobile();
                        break;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            #region Version 1
            writer.Write((bool)m_IsDrink);
            #endregion

            #region Version 0
            writer.Write((int)m_Ingredients.Count);
            foreach (string name in m_Ingredients)
            {
                writer.Write((string)name);
            }

            writer.Write((int)m_Quality);
            writer.Write((Mobile)m_Crafter);
            #endregion
        }
    }

    public class BuildingGump : Gump
    {
        private enum BuildButton
        {
            Cancel = 0,
            Okay = 1,
            Preview = 2,
            Drink = 3,
            Ingredient = 4
        }

        private enum BuildText
        {
            Name = 1,
            Hue = 2,
            ItemID = 3
        }

        private PlayerMobile m_Viewer;
        private BuildingContext m_Context;

        public BuildingGump(PlayerMobile viewer, BaseTool tool)
            : this(viewer, new BuildingContext(viewer, tool))
        {

        }

        public BuildingGump(PlayerMobile viewer, BuildingContext context) : base(0,0)
        {
            m_Viewer = viewer;
            m_Context = context;

            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump( typeof( BuildingGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
            AddPage(0);

            AddBackground(175, 143, 446, 265, 5120);
            AddBackground(186, 153, 212, 30, 9350);
            AddLabel(220, 158, 247, "Rare Materials");

            #region Ingredient Basket

            AddBackground(187, 187, 211, 182, 9350);

            int m_X = 196;
            int m_Y = 192;
            int i = 0;
            
            // First row
            AddButton(m_X, m_Y, 9803, 9803, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
           /*  AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            // end first row

            // Second row
            m_X = 196;
            m_Y = 252;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            //end second row

            // Third row
            m_X = 196;
            m_Y = 312;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)BuildButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            // end third row */
            #endregion

            #region Custom Specifications


            AddBackground(411, 187, 201, 30, 9350);
            AddLabel(416, 192, 247, "Name:");
            AddTextEntry(460, 192, 144, 20, 0, (int)BuildText.Name, m_Context.Name);

            AddBackground(411, 222, 201, 30, 9350);
            AddLabel(416, 227, 247, "Hue:");
            AddTextEntry(452, 227, 151, 20, 0, (int)BuildText.Hue, m_Context.Hue.ToString());

            AddBackground(411, 257, 201, 30, 9350);
            AddLabel(416, 262, 247, "Item ID:");
            AddTextEntry(470, 262, 133, 20, 0, (int)BuildText.ItemID, m_Context.ID.ToString());

            #endregion

            #region Preview

            AddBackground(411, 292, 201, 107, 2620);
            AddItem(486, 322, m_Context.ID, m_Context.Hue);

            #endregion

            #region Buttons

            AddButton(186, 377, 249, 248, (int)BuildButton.Okay, GumpButtonType.Reply, 0);
            AddButton(256, 377, 243, 241, (int)BuildButton.Cancel, GumpButtonType.Reply, 0);
            AddButton(368, 377, 4005, 4007, (int)BuildButton.Preview, GumpButtonType.Reply, 0);

            #endregion
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID < (int)BuildButton.Ingredient)
            {
                switch (info.ButtonID)
                {
                    case (int)BuildButton.Cancel:
                        {
                            return;
                        }
                    case (int)BuildButton.Okay:
                        {
                            int val = 0;

                            m_Context.Name = info.GetTextEntry((int)BuildText.Name).Text;

                            if (ValidateInt(info.GetTextEntry((int)BuildText.Hue).Text, ref val))
                                m_Context.Hue = val;

                            if (ValidateInt(info.GetTextEntry((int)BuildText.ItemID).Text, ref val))
                                m_Context.ID = val;

                            BuildingContext.BuildMeal(m_Context);
                            return;
                        }
                    case (int)BuildButton.Preview:
                        {
                            int val = 0;

                            m_Context.Name = info.GetTextEntry((int)BuildText.Name).Text;

                            if (ValidateInt(info.GetTextEntry((int)BuildText.Hue).Text, ref val))
                                m_Context.Hue = val;

                            if (ValidateInt(info.GetTextEntry((int)BuildText.ItemID).Text, ref val))
                                m_Context.ID = val;

                            m_Viewer.SendGump(new BuildingGump(m_Viewer, m_Context));
                            return;
                        }
                }
            }
            else
            {
                int val = 0;

                m_Context.Name = info.GetTextEntry((int)BuildText.Name).Text;

                if (ValidateInt(info.GetTextEntry((int)BuildText.Hue).Text, ref val))
                    m_Context.Hue = val;

                if (ValidateInt(info.GetTextEntry((int)BuildText.ItemID).Text, ref val))
                    m_Context.ID = val;

                if (m_Context.Ingredients.Count >= (info.ButtonID - (int)BuildButton.Ingredient) + 1 )
                {
                    m_Context.Ingredients.RemoveAt(info.ButtonID - (int)BuildButton.Ingredient);
                    m_Context.Build.SendMessage("That material has been removed.");
                    m_Viewer.SendGump(new BuildingGump(m_Viewer, m_Context));
                }
                else
                {
                    m_Viewer.Target = new BuildingIngredientTarget(m_Context);
                    m_Viewer.SendMessage("Target a piece of Rare Material.");
                }
            }

            base.OnResponse(sender, info);
        }

        private bool ValidateInt(string st, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
                return false;

            return true;
        }

        private class BuildingIngredientTarget : Target
        {
            private BuildingContext m_Context;

            public BuildingIngredientTarget(BuildingContext context) : base(1, true, TargetFlags.None)
            {
                m_Context = context;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Context == null)
                {
                    return;
                }

                if (targeted is Item)
                {
                    m_Context.AddIngredient(targeted as Item);
                }

                base.OnTarget(from, targeted);
            }
        }
    }
}