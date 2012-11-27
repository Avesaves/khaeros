using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Network;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;



namespace Server.Items
{
    public class BottleOfGoo : Item
    {
        private bool GooInBottle;
        private Mobile gooPet;

        [Constructable]
        public BottleOfGoo()
            : base(0x183C)
        {
            Weight = 1.0;
            Name = "a bottle of goo";
            SetGooInBottle(true);
        }

        public BottleOfGoo(Serial serial) : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write(GooInBottle);
            writer.Write(gooPet);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            GooInBottle = reader.ReadBool();
            gooPet = reader.ReadMobile();
        }

        public bool GetGooInBottle() // Returns whether or not the goo exists, or is "in the bottle". (Get)
        {
            return GooInBottle;
        }

        public void SetGooInBottle(bool x) // Sets whether or not the goo is in the bottle. (Set)
        {
            GooInBottle = x;
            if (x == true)
            {
                this.ItemID = 0x183C;
            }
            else if (x == false)
            {
                this.ItemID = 0x182D;
            }
        }

        public Mobile GetGoo() // Returns the goo mobile. (Get)
        {
            return gooPet;
        }

        public void SetGoo(Goo g) // Sets the goo mobile as g. (Set)
        {
            gooPet = g;
        }

        public override void OnDoubleClick(Mobile m) // When this object -- the bottle -- is double-clicked...
        {

            if (m.Followers >= m.FollowersMax) // If the player's followers is greater than or equal to his max followers...
            {
                m.SendMessage("You need at least one free follower slot to open the bottle.");
            }
            else if (((m.InRange(this, 1) && m.CanSee(this)) || this.IsChildOf(m.Backpack)) && (!GetGooInBottle())) // If the bottle is in-range and the goo is not in its bottle...
            {
                m.SendMessage("The goo is out of its bottle!");
                m.SendLocalizedMessage(1010018); // What do you want to use this item on?

                m.Target = new BottleOfGooTarget(this, gooPet); // Creates a target from this mobile with the goo mobile thrown to the new class as a reference.
            }
            else if (((m.InRange(this, 1) && m.CanSee(this)) || this.IsChildOf(m.Backpack)) && (GetGooInBottle())) // If the bottle is in-range and the goo IS in its bottle, this function creates a new goo mobile.
            {
                gooPet = new Goo(this);

                ((GenericWarrior)gooPet).Controlled = true;
                ((GenericWarrior)gooPet).ControlMaster = m;

                gooPet.MoveToWorld(m.Location, m.Map);
                gooPet.PlaySound(0x1CC);
                gooPet.Emote("*leaps out of " + m.Name + "'s bottle!*");

                SetGooInBottle(false);
            }
            else
            {
                m.SendLocalizedMessage(500446); //That is too far away.
            }
        }
    }

    public class Goo : GenericWarrior // The Mobile goo's class is a GenericWarrior to give it access to the GenericWarrior properties we need for a tamed creature.
    {
        private BottleOfGoo m_OwnerBottle;
        
        public Goo( Serial serial ) : base( serial )
		{
		}
        public Goo(BottleOfGoo owner) : base()
        {
            m_OwnerBottle = owner;

            BodyValue = 51;
            BaseSoundID = 456;
            Hue = 0x14;
            Name = "Goo";
           
            RawStr = 5;
            RawDex = 5;
            RawInt = 5;
            Stam = 5;
            SetHits(5);
            
            Fame = 0;
            Lives = -100;
            Criminal = true;
            RemoveLoot = true;
        }
        public override void OnDeath(Container c) // The primary reason for this class; on the goo's death it sets the parent GooInBottle to true, meaning the goo is now back in the bottle and can be re-summoned.
        {
            base.OnDeath(c);
            if (m_OwnerBottle != null)
            {
                m_OwnerBottle.SetGooInBottle(true);
            }
        }
        public override void OnAfterMove(Point3D oldLocation)
        {
            Blood gooPuddle = new Blood();
            gooPuddle.ItemID = Utility.Random(0x122A, 5);
            gooPuddle.Name = "slime";
            gooPuddle.Hue = 0x14;
            gooPuddle.MoveToWorld(oldLocation, this.Map);
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((Item)m_OwnerBottle);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_OwnerBottle = (BottleOfGoo)reader.ReadItem();
        }
    }

    public class BottleOfGooTarget : Target // Creates the target for the BottleOfGoo.
    {
        private BottleOfGoo m_OwnerBottle;
        private Mobile m_Goo;

        public BottleOfGooTarget(BottleOfGoo owner, Mobile g)
            : base(2,false,TargetFlags.None)
        {
            m_OwnerBottle = owner; // Throws the parent BottleOfGoo to the class, allowing us to use its properties as well as read back to them.
            m_Goo = m_OwnerBottle.GetGoo(); // Sets the m_Goo to the parent BottleOfGoo's goo.
        }
        protected override void OnTarget(Mobile m, object target)
        {
            if ((!m_OwnerBottle.GetGooInBottle()) && (target is Mobile))
            {
                Mobile t = target as Mobile;

                if(t == m_Goo)
                {
                    t.Emote("*slimes its way back into the bottle*");
                    t.PlaySound(0x1CC);
                    t.Delete();
                    m_OwnerBottle.SetGooInBottle(true);
                }
                else
                {
                    m.SendMessage("You don't think that will fit in the bottle...");
                }
            }
            else if (m_OwnerBottle.GetGooInBottle())
            {
                m.SendMessage("The goo is already in its bottle.");
            }
            else
            {
                m.SendMessage("You don't think the goo would like it if you put that in its bottle.");
            }
        }
    }
}
