using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Commands;
using Server.Multis; 

namespace Server.Items
{
    public class QuestDeed : Item
    {
        private int m_Power;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

        [Constructable]
        public QuestDeed()
            : base(0x14EB)
        {
            Stackable = false;
            Weight = 1.0;
            Hue = 2832;
            Name = "A Bounty Deed";

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !(from is PlayerMobile) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed)
                return;

            PlayerMobile pm = from as PlayerMobile;

            /*
            if (pm.DayOfDeath +5 >= 50)
            {
                pm.SendMessage("This just looks too unappetizing to eat right now.");
                return;
            }
            if (pm.Hunger > 19)
            {
                pm.SendMessage("You are too full to eat this ... thing!");
                return;
            } */ 
            if (from.Backpack != null && this.ParentEntity == from.Backpack)
            {
                QuestCamp questc = new QuestCamp(); 
                Point3D l1 = new Point3D(2600, 3286, 12 ); // meteor fields
                Point3D l2 = new Point3D( 2376, 2515, 0 ); // steainnbaer mountains
                Point3D l3 = new Point3D( 2127, 2967, -44 ); // Arianwen 
                Point3D l4 = new Point3D( 4097, 1154, 0 ); // the wastes
                Point3D l5 = new Point3D( 838, 1624, 2 ); // the desert
                Point3D l6 = new Point3D( 1074, 1972, 2 ); //Meteor
                Point3D l7 = new Point3D( 3974, 2120, 60 ); // dragonspine
                Point3D l8 = new Point3D( 3314, 1234, 0 ); // near tyrheim
                Point3D l9 = new Point3D( 3304, 2039, 10 );//veiled sun
                Point3D l10 = new Point3D( 3510, 2975, 25 ); // south of the creepy castle
                Point3D l11 = new Point3D( 856, 2880, 5 ); // the jungles of azhur
                Point3D l12 = new Point3D( 1626, 1058, 2 ); // the heath
                Point3D l13 = new Point3D( 1373, 3147, 0 ); // caerdwyr
                Point3D l14 = new Point3D( 2281, 3392, 2 ); // droeddmor
                Point3D l15 = new Point3D( 1898, 2439, 0 ); // spirit mountain
                Point3D l16 = new Point3D( 4375, 2430, 2 ); // eastern jungles

               				switch ( Utility.Random( 15 ) )
			{
                case 0:
                    {
                        questc.MoveToWorld( l1, pm.Map );
                        from.SendMessage("This deed tells you of a fallen star sighted headed towards the meteor fields, south across the river from Tserjicanth....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 1:
                    {
                        questc.MoveToWorld(l2, pm.Map);
                        from.SendMessage("This deed tells you of a falling star that landed somewhere in the labyrinthine steinnbaer mountains....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 2:
                    {
                        questc.MoveToWorld(l3, pm.Map);
                        from.SendMessage("This deed tells you of a strange occurance near the lost city of arianwen, in the winding degraded paths....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 3:
                    {
                        questc.MoveToWorld(l4, pm.Map);
                        from.SendMessage("This deed tells you of a fallen star landing in the distant frozen wastes of Eastern Khaeros....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 4:
                    {
                        questc.MoveToWorld(l5, pm.Map);
                        from.SendMessage("This deed tells of a keeper rumour that there may have been a celestial event deep in their desert, seen by the denizens of the oases....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }

                case 5:
                    {
                        questc.MoveToWorld(l6, pm.Map);
                        from.SendMessage("Something, the deed writes, has landed in the giant crater to the west....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 6:
                    {
                        questc.MoveToWorld(l7, pm.Map);
                        from.SendMessage("Last night, the deed tells you, a strange celestial event terminated somewhere in the dragonspine pass....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 7:
                    {
                        questc.MoveToWorld(l8, pm.Map);
                        from.SendMessage("The deed tells you of a fallen star, which apparently fell somewhere far to the north, in the area of the world called the 'Frost Veldt', where Tyreans once called home... ");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 8:
                    {
                        questc.MoveToWorld(l9, pm.Map);
                        from.SendMessage("This deed tells you of strange creatures that have appeared near Veiled Sun, the undying, to the east against the great mountains...");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 9:
                    {
                        questc.MoveToWorld(l10, pm.Map);
                        from.SendMessage("This appears to be the story of monstrous unknown creatures falling from the sky, south a creepy abandoned castle in the east...");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 10:
                    {
                        questc.MoveToWorld(l11, pm.Map);
                        from.SendMessage("A wandering savage told, and had his story recorded, about a sky god sending creatures into the jungles of Azhur.");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 11:
                    {
                        questc.MoveToWorld(l12, pm.Map);
                        from.SendMessage("This deed is a formally written report by a Keeper on a strange sighting of a fallen star related event very near the Dawn Temple....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 12:
                    {
                        questc.MoveToWorld(l13, pm.Map);
                        from.SendMessage("This is a report by a drunken guard who was nearly killed by some bizarre creature near the entrance to the Caerdwyr Forest!");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 13:
                    {
                        questc.MoveToWorld(l14, pm.Map);
                        from.SendMessage("This deed tells you of a questionable story about monsters from the celestial spheres living near the ruins of Droeddmor, in the southern wilds...");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 14:
                    {
                        questc.MoveToWorld(l15, pm.Map);
                        from.SendMessage("Something fell recently very nearby - just on the other side of the steinnbaer mountains, near the path connecting north and south.");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    }
                case 15:
                    {
                        questc.MoveToWorld(l16, pm.Map);
                        from.SendMessage("This is a report of a strange tale of a shooting star breaking apart over the exotic Eastern Jungles, just south of the snowfields....");
                        from.SendMessage("You will have a half hour to find and dispatch the monsters at this site, and bring back the unique item.");
                        break; 
                    } 
                    this.Delete();

               // pm.WikiConfig = "fey";

            }
            }

            else
                from.SendMessage("That needs to be in your backpack for you to use it.");
        }

        public QuestDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)m_Power);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Power = reader.ReadInt();
        }

      
    }
}