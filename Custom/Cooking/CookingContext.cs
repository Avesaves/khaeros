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
    public enum FoodQuality
        {
            Disgusting = -1,
            Poor = 0,
            Average = 1,
            Delicious = 2,
            Masterpiece = 3
        }

    [PropertyObject]
    public class CookingContext
    {
        #region Variable Declaration & Get/Set
        private PlayerMobile m_Cook;
        private string m_Name;
        private int m_ID;
        private int m_Hue;
        private List<Item> m_Ingredients = new List<Item>();
        private FoodQuality m_Quality;
        private BaseTool m_Tool;
        private bool m_IsDrink;

        public PlayerMobile Cook { get { return m_Cook; } set { m_Cook = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public int ID { get { return m_ID; } set { m_ID = value; if (m_ID < 2) m_ID = 2; if (m_ID > 16382) m_ID = 16382; } }
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public List<Item> Ingredients { get { return m_Ingredients; } set { m_Ingredients = value; } }
        public FoodQuality Quality { get { return m_Quality; } set { m_Quality = value; } }
        public BaseTool Tool { get { return m_Tool; } set { m_Tool = value; } }
        public bool IsDrink { get { return m_IsDrink; } set { m_IsDrink = value; } }
        #endregion

        public CookingContext(PlayerMobile cook, BaseTool tool)
        {
            m_Cook = cook;

            m_Name = "An Unnamed Dish";
            m_ID = 7817;
            m_Hue = 0;
            m_Tool = tool;
            m_IsDrink = false;
        }

        public void AddIngredient(Item ing)
        {
            if (m_Cook == null || m_Cook.Deleted || !m_Cook.Alive)            
                return;
            
            if (ing == null || ing.Deleted)
            {
                Console.WriteLine("Attempt to add ingredient failed; object no longer exists.");
                m_Cook.SendGump(new CookingGump(m_Cook, this));
                return;
            }

            //Adding to the list of ingredients.
            if ( IsValidIngredient(ing) && (m_Cook.Backpack.Items.Contains(ing) || m_Cook.InRange(ing.Location, 1)))
            {
                if (m_Ingredients.Contains(ing))
                    m_Cook.SendMessage("You have already added this to your meal.");
                else
                    m_Ingredients.Add(ing);

                m_Cook.SendGump(new CookingGump(m_Cook, this));
            }
            else
            {
                m_Cook.SendMessage("You probably shouldn't put that into your meal.");
                m_Cook.SendGump(new CookingGump(m_Cook, this));
                return;
            }
        }

        public static bool IsValidIngredient(Item ing)
        {
            if (ing == null)
                return false;
            if (ing.Deleted)
                return false;

            if(ing is Food)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(Food)))
                return true;
            if(ing is CookableFood)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(CookableFood)))
                return true;
            if (ing.Name == "Silphium")
                return true;				
            if(ing is BaseHerb)
                return true;
            if(ing.GetType().IsSubclassOf(typeof(BaseHerb)))
                return true;
            if(ing is Pitcher)
                return true;
            if(ing is BaseIngredient)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(BaseIngredient)))
                return true;
            if (ing is BaseChewable)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(BaseChewable)))
                return true;
            if (ing is BaseSmokable)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(BaseSmokable)))
                return true;
            if (ing is BaseSnortable)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(BaseSnortable)))
                return true;
            if (ing is SackFlour)
                return true;
            if (ing is TeaLeaves)
                return true;
            if (ing is BagOfSugar)
                return true;
            if (ing is RiceSheath)
                return true;
            if (ing is Sugarcane)
                return true;
            if (ing is JarHoney)
                return true;
            if (ing is BaseReagent)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(BaseReagent)))
                return true;
            if (ing is Eggs)
                return true;
            if (ing is RawBacon)
                return true;
            if (ing is RawChickenLeg)
                return true;
            if (ing is BeefHock)
                return true;
            if (ing is PorkHock)
                return true;
            if (ing is TurkeyHock)
                return true;
            if (ing is FieldCorn)
                return true;
            if (ing is Yeast)
                return true;
            if (ing is WinecrafterYeast)
                return true;
            if (ing is WinecrafterSugar)
                return true;
            if (ing is TeaLeaves)
                return true;
            if (ing is BagOfOats)
                return true;
            if (ing is BagOfBarley)
                return true;
            if (ing is BagOfCornmeal)
                return true;
            if (ing is BagOfCocoa)
                return true;
            if (ing is BagOfCoffee)
                return true;
            if (ing is BagOfMalt)
                return true;
            if (ing is BagOfRicemeal)
                return true;
            if (ing is BagOfSoy)
                return true;
            if (ing is CocoaBean)
                return true;
            if (ing is CocoaNut)
                return true;
            if (ing is DriedHerbs)
                return true;
            if (ing is DriedOnions)
                return true;
            if (ing is Dragonsblood)
                return true;
            if (ing is MilkBottle)
                return true;
            if (ing.GetType().IsSubclassOf(typeof(MilkBottle)))
                return true;
            if (ing is MilkBucket)
                return true;
            if (ing.Name == "Scumtongue")
                return true;
            if (ing is CoffeeBean)
                return true;
            
            return false;
        }
        
        public static void CookMeal(CookingContext context)
        {
            if (context.Cook == null || context.Cook.Deleted || !context.Cook.Alive)
                return;

            if (context.Ingredients.Count < 1)
            {
                context.Cook.SendMessage("There are no ingredients in this meal.");
                return;
            }

            int quality = Utility.RandomMinMax(-1, 3);
            quality += context.Cook.Feats.GetFeatLevel(FeatList.Cooking);
            if (quality > 3)
                quality = 3;
            context.Quality = (FoodQuality)quality;

            if (Utility.RandomMinMax(1, 100) < (context.Cook.Feats.GetFeatLevel(FeatList.Cooking) * 33))
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

                        if (context.Cook.InRange(i.Location, 1) || context.Cook.Backpack.Items.Contains(i))
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
                        context.Cook.SendMessage("You do not have all the ingredients for this.");
                        return;
                    }
                }

                context.Cook.PlaySound(0x04F);
                context.Cook.PlaySound(0x247);
                CustomFood newFood = new CustomFood(context.Cook, context.Name, context.ID, context.Hue, ingredientNames, context.Quality, context.IsDrink);

                if (newPoison != null && !newPoison.Deleted)
                {
                    XmlAttach.AttachTo(newFood, newPoison);
                }

                context.Cook.AddToBackpack(newFood);
                context.Cook.SendMessage("You have successfully cooked " + newFood.Name.ToString() + "!");
                context.Cook.Crafting = true;
                Misc.LevelSystem.AwardCookingXP(context.Cook);
                context.Cook.Crafting = false;
                context.Tool.UsesRemaining--;
                if (context.Tool.UsesRemaining < 1)
                {
                    context.Cook.SendMessage("Your " + context.Tool.Name + " has been worn out.");
                    context.Tool.Delete();
                }
                else
                {
                    PlayerMobile pm = context.Cook as PlayerMobile;
                    pm.SendGump(new CookingGump(pm, context));
                }
            }
            else
            {
                context.Cook.SendMessage("You have failed to cook the meal; the food is ruined!");
                List<Item> removeList = new List<Item>();
                foreach (Item i in context.Ingredients)
                {
                    if (i != null && !i.Deleted)
                    {
                        if (context.Cook.InRange(i.Location, 1) || context.Cook.Backpack.Items.Contains(i))
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

    public class CustomFood : Food
    {
        private List<string> m_Ingredients = new List<string>();
        private FoodQuality m_Quality;
        private Mobile m_Crafter;
        private bool m_IsDrink = false;

        [CommandProperty (AccessLevel.GameMaster)]
        public Mobile Crafter { get { return m_Crafter; } set { m_Crafter = value; } }
        public FoodQuality Quality { get { return m_Quality; } set { m_Quality = value; } }

        public override int HitsBonus
        {
            get
            {
                return (int)m_Quality + base.HitsBonus;
            }
            set
            {
                base.HitsBonus = value;
            }
        }

        public override int ManaBonus
        {
            get
            {
                return (int)m_Quality + base.HitsBonus;
            }
            set
            {
                base.ManaBonus = value;
            }
        }

        public CustomFood(Mobile crafter, string name, int itemid, int hue, List<string> ingredients, FoodQuality quality, bool isdrink)
            : base(itemid)
        {
            m_Crafter = crafter;
            Name = name;
            Hue = hue;
            m_Ingredients = ingredients;
            m_Quality = quality;
            m_IsDrink = isdrink;
            Weight = m_Ingredients.Count;
            Stackable = false;
            FillFactor += (int)m_Quality;
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

            string propString = ("(" + m_Quality.ToString() + ")" + "\n " + "Ingredients:");

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

        public override void CheckRot()
        {
            if (!Movable)
                return;

            base.CheckRot();
        }

        public override bool Eat(Mobile from)
        {
            DamageEntry de = from.FindMostRecentDamageEntry(false);
            if (de != null && DateTime.Compare(DateTime.Now, de.LastDamage + TimeSpan.FromMinutes(5)) < 0)
            {
                from.SendMessage("Your heart is still pounding from the battle. You can't bring yourself to eat right now.");
                return false;
            }

            // Fill the Mobile with FillFactor
            if (m_IsDrink)
            {
                if (BaseCraftWine.Thirsty(from, (int)m_Quality))
                {
                    from.PlaySound(Utility.Random(0x30, 2));

                    #region Injury Healing
                    if (!from.Warmode && (from.Combatant == null || from.Combatant.Deleted || !from.Combatant.Alive))
                    {
                        if (m_Quality == FoodQuality.Masterpiece)
                        {
                            HealthAttachment m_HA = HealthAttachment.GetHA(from);
                            InjuryTimer removeTimer = null;
                            if (m_HA != null)
                            {
                                foreach (InjuryTimer iT in m_HA.CurrentInjuries)
                                {
                                    if ((int)iT.Injury < 4)
                                    {
                                        removeTimer = iT;
                                        continue;
                                    }
                                }
                                if (removeTimer != null)
                                {
                                    removeTimer.Stop();
                                    m_HA.CurrentInjuries.Remove(removeTimer);
                                    from.SendMessage("You feel much better.");
                                }
                            }
                        }
                    }
                    #endregion

                    if (m_Quality == FoodQuality.Masterpiece)
                        HealthAttachment.SpeedDiseaseRecovery(from);

                    if (from.Stam < from.StamMax)
                        from.Stam += Utility.Random(6, 3) + FillFactor / 5;//restore some stamina
                    // added
                    if (from.Hits < from.HitsMax && HitsBonus != 0)
                        from.Hits += Utility.RandomMinMax(HitsBonus, FillFactor + HitsBonus);//restore some health
                    if (from.Mana < from.ManaMax && ManaBonus != 0)
                        from.Mana += Utility.RandomMinMax(ManaBonus, FillFactor + ManaBonus);//restore some mana

                    PoisonedFoodAttachment attachment = XmlAttach.FindAttachment(this, typeof(PoisonedFoodAttachment)) as PoisonedFoodAttachment;

                    if (attachment != null)
                    {
                        attachment.OnConsumed(from);
                        attachment.Delete(); // only one piece of food can be poisoned in the stack
                    }

                    if (RotStage != RotStage.None && from is PlayerMobile && ((PlayerMobile)from).Nation != Nation.Mhordul)
                        ApplyRotPoison(from);

                    Consume();

                    return true;
                }
            }
            else
            {
                if (FillHunger(from, (int)m_Quality, HitsBonus, ManaBonus))  // added HitsBonus, ManaBonus - alari
                {
                    #region Injury Healing
                    if (!from.Warmode && (from.Combatant == null || from.Combatant.Deleted || !from.Combatant.Alive))
                    {
                        if (m_Quality == FoodQuality.Masterpiece)
                        {
                            HealthAttachment m_HA = HealthAttachment.GetHA(from);
                            InjuryTimer removeTimer = null;
                            if (m_HA != null)
                            {
                                foreach (InjuryTimer iT in m_HA.CurrentInjuries)
                                {
                                    if ((int)iT.Injury < 4)
                                    {
                                        removeTimer = iT;
                                        continue;
                                    }
                                }
                                if (removeTimer != null)
                                {
                                    removeTimer.Stop();
                                    m_HA.CurrentInjuries.Remove(removeTimer);
                                    from.SendMessage("You feel much better.");
                                }
                            }
                        }
                    }
                    #endregion

                    if (m_Quality == FoodQuality.Masterpiece)
                        HealthAttachment.SpeedDiseaseRecovery(from);

                    // Play a random "eat" sound
                    from.PlaySound(Utility.Random(0x3A, 3));

                    if (from.Body.IsHuman && !from.Mounted)
                        from.Animate(34, 5, 1, true, false, 0);

                    PoisonedFoodAttachment attachment = XmlAttach.FindAttachment(this, typeof(PoisonedFoodAttachment)) as PoisonedFoodAttachment;

                    if (attachment != null)
                    {
                        attachment.OnConsumed(from);
                        attachment.Delete(); // only one piece of food can be poisoned in the stack
                    }

                    if (RotStage != RotStage.None && from is PlayerMobile && ((PlayerMobile)from).Nation != Nation.Mhordul)
                        ApplyRotPoison(from);

                    Consume();

                    return true;
                }
            }

            return false;
        }

        public override bool StackWith(Mobile from, Item dropped)
        {
            return false;
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            return false;
        }

        public CustomFood(Serial serial)
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
                        m_Quality = (FoodQuality)reader.ReadInt();
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

    public class CookingGump : Gump
    {
        private enum CookButton
        {
            Cancel = 0,
            Okay = 1,
            Preview = 2,
            Drink = 3,
            Ingredient = 4
        }

        private enum CookText
        {
            Name = 1,
            Hue = 2,
            ItemID = 3
        }

        private PlayerMobile m_Viewer;
        private CookingContext m_Context;

        public CookingGump(PlayerMobile viewer, BaseTool tool)
            : this(viewer, new CookingContext(viewer, tool))
        {

        }

        public CookingGump(PlayerMobile viewer, CookingContext context) : base(0,0)
        {
            m_Viewer = viewer;
            m_Context = context;

            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump( typeof( CookingGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
            AddPage(0);

            AddBackground(175, 143, 446, 265, 5120);
            AddBackground(186, 153, 212, 30, 9350);
            AddLabel(220, 158, 247, "Cooking   with   Khaeros");

            #region Ingredient Basket

            AddBackground(187, 187, 211, 182, 9350);

            int m_X = 196;
            int m_Y = 192;
            int i = 0;
            
            // First row
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            // end first row

            // Second row
            m_X = 196;
            m_Y = 252;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            //end second row

            // Third row
            m_X = 196;
            m_Y = 312;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            AddButton(m_X, m_Y, 9802, 9802, ((int)CookButton.Ingredient) + i, GumpButtonType.Reply, 0);
            if (m_Context.Ingredients.Count > i)
                AddItem(m_X + 8, m_Y + 9, m_Context.Ingredients[i].ItemID, m_Context.Ingredients[i].Hue);
            m_X += 70; i++;
            // end third row
            #endregion

            #region Custom Specifications

            AddButton(414, 156, m_Context.IsDrink ? 9027 : 9026, m_Context.IsDrink ? 9026 : 9027, (int)CookButton.Drink, GumpButtonType.Reply, 0);
            AddLabel(440, 156, m_Context.IsDrink ? 37 : 0, "This Is A Drink");

            AddBackground(411, 187, 201, 30, 9350);
            AddLabel(416, 192, 247, "Name:");
            AddTextEntry(460, 192, 144, 20, 0, (int)CookText.Name, m_Context.Name);

            AddBackground(411, 222, 201, 30, 9350);
            AddLabel(416, 227, 247, "Hue:");
            AddTextEntry(452, 227, 151, 20, 0, (int)CookText.Hue, m_Context.Hue.ToString());

            AddBackground(411, 257, 201, 30, 9350);
            AddLabel(416, 262, 247, "Item ID:");
            AddTextEntry(470, 262, 133, 20, 0, (int)CookText.ItemID, m_Context.ID.ToString());

            #endregion

            #region Preview

            AddBackground(411, 292, 201, 107, 2620);
            AddItem(486, 322, m_Context.ID, m_Context.Hue);

            #endregion

            #region Buttons

            AddButton(186, 377, 249, 248, (int)CookButton.Okay, GumpButtonType.Reply, 0);
            AddButton(256, 377, 243, 241, (int)CookButton.Cancel, GumpButtonType.Reply, 0);
            AddButton(368, 377, 4005, 4007, (int)CookButton.Preview, GumpButtonType.Reply, 0);

            #endregion
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID < (int)CookButton.Ingredient)
            {
                switch (info.ButtonID)
                {
                    case (int)CookButton.Cancel:
                        {
                            return;
                        }
                    case (int)CookButton.Okay:
                        {
                            int val = 0;

                            m_Context.Name = info.GetTextEntry((int)CookText.Name).Text;

                            if (ValidateInt(info.GetTextEntry((int)CookText.Hue).Text, ref val))
                                m_Context.Hue = val;

                            if (ValidateInt(info.GetTextEntry((int)CookText.ItemID).Text, ref val))
                                m_Context.ID = val;

                            CookingContext.CookMeal(m_Context);
                            return;
                        }
                    case (int)CookButton.Preview:
                        {
                            int val = 0;

                            m_Context.Name = info.GetTextEntry((int)CookText.Name).Text;

                            if (ValidateInt(info.GetTextEntry((int)CookText.Hue).Text, ref val))
                                m_Context.Hue = val;

                            if (ValidateInt(info.GetTextEntry((int)CookText.ItemID).Text, ref val))
                                m_Context.ID = val;

                            m_Viewer.SendGump(new CookingGump(m_Viewer, m_Context));
                            return;
                        }
                    case (int)CookButton.Drink:
                        {
                            int val = 0;

                            m_Context.Name = info.GetTextEntry((int)CookText.Name).Text;

                            if (ValidateInt(info.GetTextEntry((int)CookText.Hue).Text, ref val))
                                m_Context.Hue = val;

                            if (ValidateInt(info.GetTextEntry((int)CookText.ItemID).Text, ref val))
                                m_Context.ID = val;

                            if (m_Context.IsDrink)
                                m_Context.IsDrink = false;
                            else
                                m_Context.IsDrink = true;

                            m_Viewer.SendGump(new CookingGump(m_Viewer, m_Context));

                            return;
                        }
                }
            }
            else
            {
                int val = 0;

                m_Context.Name = info.GetTextEntry((int)CookText.Name).Text;

                if (ValidateInt(info.GetTextEntry((int)CookText.Hue).Text, ref val))
                    m_Context.Hue = val;

                if (ValidateInt(info.GetTextEntry((int)CookText.ItemID).Text, ref val))
                    m_Context.ID = val;

                if (m_Context.Ingredients.Count >= (info.ButtonID - (int)CookButton.Ingredient) + 1 )
                {
                    m_Context.Ingredients.RemoveAt(info.ButtonID - (int)CookButton.Ingredient);
                    m_Context.Cook.SendMessage("That ingredient has been removed.");
                    m_Viewer.SendGump(new CookingGump(m_Viewer, m_Context));
                }
                else
                {
                    m_Viewer.Target = new CookingIngredientTarget(m_Context);
                    m_Viewer.SendMessage("Target an ingredient to add to your meal.");
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

        private class CookingIngredientTarget : Target
        {
            private CookingContext m_Context;

            public CookingIngredientTarget(CookingContext context) : base(1, true, TargetFlags.None)
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