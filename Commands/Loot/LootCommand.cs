using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Commands
{
    public class LootCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Loot", AccessLevel.Player, new CommandEventHandler(Loot_OnCommand));
        }

        [Usage("Loot")]
        [Description("Retrieves items from the targeted corpse.")]
        private static void Loot_OnCommand(CommandEventArgs args)
        {
            Mobile m = args.Mobile;

            if (m.Mounted)
            {
                m.SendMessage("You cannot use that command while on a mount.");
                return;
            }

            if (UseDelaySystem.CheckContext(m, DelayContextType.LootCommand)) //check for timer
            {
                m.SendMessage("You must wait a few moments before using this command again.");
                return;
            }

            if (args.Length > 0)
            {
                m.SendGump(new LootOptionsGump(m));
            }

            else
            {
                m.SendMessage("Target a corpse to loot.");
                m.BeginTarget(2, false, TargetFlags.None, new TargetCallback(Grab_OnTarget));
            }

        }

        private static void Grab_OnTarget(Mobile from, object target)
        {
            if (!from.Alive || !from.CanSee(target) || !(target is Container))
                return;

            bool canLoot = false;
            Container cont = (Container)target;

            if (target is Corpse)
            {
                Corpse c = (Corpse)target;


                if (c.Owner == null || c.Killer == null) //unable to determine cause of death
                {
                    canLoot = true;
                }

                if (c.Owner is PlayerMobile)
                {
                    canLoot = false;
                    from.SendMessage("You cannot loot a player corpse.");
                }

                else if (c.Owner is BaseCreature && !(c.Owner is PlayerMobile)) //it's a monster corpse: do you have looting rights?
                {
                    BaseCreature bc = (BaseCreature)c.Owner;
                    List<DamageStore> lootingRights = BaseCreature.GetLootingRights(bc.DamageEntries, bc.HitsMax);
                    Mobile master = bc.GetMaster();

                    if (master != null && master == from) //if it's your pet, you always have the right
                        canLoot = true;

                    for (int i = 0; !canLoot && i < lootingRights.Count; i++)
                    {
                        if (lootingRights[i].m_Mobile != from)
                            continue;

                        canLoot = lootingRights[i].m_HasRight;
                    }

                    if (!canLoot)
                        from.SendMessage("You do not have the right to loot from that corpse.");
                }

                else
                {
                    canLoot = false;
                    from.SendMessage("You cannot loot that corpse.");
                }
            }
            else
            {
                canLoot = false;
                from.SendMessage("You cannot loot that!");
            }

            if (canLoot)
                GrabLoot(from, cont);
        }

        private static void GrabLoot(Mobile from, Container cont)
        {
            if (!from.Alive || cont == null)
                return;

            if (cont is Corpse && from == ((Corpse)cont).Owner)
            {
                Corpse corpse = (Corpse)cont;

                if (corpse.Killer == null || corpse.Killer is BaseCreature)
                    corpse.Open(from, true);
                else
                    corpse.Open(from, false);
            }
            else
            {
                bool fullPack = false;
                List<Item> items = new List<Item>(cont.Items);
                LootOptions options = LootGrab.GetOptions(from);

                for (int i = 0; !fullPack && i < items.Count; i++)
                {
                    Item item = items[i];

                    if (options.IsLootable(item))
                    {
                        Container dropCont = options.GetPlacementContainer(LootGrab.ParseType(item));

                        if (dropCont == null || dropCont.Deleted || !dropCont.IsChildOf(from.Backpack))
                            dropCont = from.Backpack;

                        if (!item.DropToItem(from, dropCont, new Point3D(-1, -1, 0)))
                            fullPack = true;

                        if (options.IsToken(item))
                            from.Emote("*loots a few shiny tokens*");
                    }
                }

                if (fullPack)
                    from.SendMessage("You looted as much as you could. The rest remain {0}.", (cont is Corpse ? "on the corpse" : "in the container"));
                else
                    from.SendMessage("You loot all you can from the {0}.", (cont is Corpse ? "corpse" : "container"));

                from.Animate(32, 5, 1, true, false, 0);
                from.PlaySound(79);
                from.RevealingAction();
            }
            UseDelaySystem.AddContext(from, DelayContextType.LootCommand, TimeSpan.FromSeconds(3.0)); //edit timer here, dont forget reference in usedelaysystem.cs!
        }
    }
}