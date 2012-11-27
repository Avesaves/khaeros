using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a serpent corpse")]
    public class Serpent : BaseBreedableCreature, ISmallPredator, ISerpent, ITinyPet
    {
        public override int MaxCubs { get { return 5; } }

        public override bool ParryDisabled { get { return true; } }
        public override bool DeathAdderCharmable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public int poisonHealthDamage = 0;
        public int poisonStamDamage = 0;
        public int poisonManaDamage = 0;
        public int poisonStrDamage = 0;
        public int poisonDexDamage = 0;
        public int poisonIntDamage = 0;
        public int poisonDuration = 0;
        public int poisonSpeed = 0;

        private DateTime m_LastMilking;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastMilking
        {
            get { return m_LastMilking; }
            set { m_LastMilking = value; }
        }

        public Serpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a serpent";
            Body = 52;
            BaseSoundID = 0xDB;

            SetStr(15,20);
            SetDex(15,20);
            SetInt(5,10);
            SetHits(10,20);
            SetMana(0);

            SetDamage(3);

            SetDamageType(ResistanceType.Piercing, 100);

            SetResistance(ResistanceType.Blunt, 10, 20);
            SetResistance(ResistanceType.Slashing, 10, 15);
            SetResistance(ResistanceType.Piercing, 15, 25);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.Poisoning, 40.0, 60.0);
            SetSkill(SkillName.MagicResist, 0.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.UnarmedFighting, 40.1, 42.0);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 5;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 33.1;
        }

        public override void SetBreedsTraits(string breed, int group)
        {
            switch (breed)
            {
                case "Black Mamba":
                    {
                        this.Name = "a black mamba";
                        this.Hue = 2991;
                        this.ChangeBreed = "Black Mamba";
                        this.setSerpentPoison(50, 0, 0, 0, 0, 0, 120, 2);
                        break;
                    }

                case "Cobra":
                    {
                        this.Name = "a cobra";
                        this.Hue = 1806;
                        this.ChangeBreed = "Cobra";
                        this.setSerpentPoison(15,25,0,0,25,0,120,2);
                        break;
                    }

                case "Copperhead":
                    {
                        this.Name = "a copperhead";
                        this.Hue = 2827;
                        this.ChangeBreed = "Copperhead";
                        this.setSerpentPoison(10, 0, 15, 0, 0, 25, 360, 2);
                        break;
                    }

                case "Grass Snake":
                    {
                        this.Name = "a grass snake";
                        this.Hue = 2003;
                        this.ChangeBreed = "Grass Snake";
                        break;
                    }

                case "Viper":
                    {
                        this.Name = "a viper";
                        this.Hue = 1894;
                        this.ChangeBreed = "Viper";
                        this.setSerpentPoison(10, 0, 0, 20, 20, 0, 360, 2);
                        break;
                    }
            }
        }

        public void setSerpentPoison(int healthDamage, int stamDamage, int manaDamage, int strDamage, int dexDamage, int intDamage, int duration, int speed)
        {
            poisonHealthDamage = healthDamage; //Viper, Copperhead, Cobra, Black Mamba
            poisonStamDamage = stamDamage; //Cobra
            poisonManaDamage = manaDamage; //Copperhead, 
            poisonStrDamage = strDamage; //Viper
            poisonDexDamage = dexDamage; //Viper, Cobra
            poisonIntDamage = intDamage; //Copperhead, 
            poisonDuration = duration;
            poisonSpeed = speed;
        }

        public virtual void incSerpentPoison()
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            int poisonFeatLevel = ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Poisoning);
            int veterinaryFeatLevel = ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Veterinary);
            int farmingFeatLevel = ((IKhaerosMobile)from).Feats.GetFeatLevel(FeatList.Farming);

            int venomCount = (Utility.Random(this.XPScale)+1);;

            DateTime venomWait = this.m_LastMilking.AddDays(5);

            if ( from == this.ControlMaster)
            {
                if (DateTime.Today >= venomWait)
                {
                    if ( poisonFeatLevel > 1 )
                    {
                        if( veterinaryFeatLevel > 0 )
                        {
                            if( farmingFeatLevel > 0 )
                            {
                                venomCount += farmingFeatLevel;

                                if (this is GrassSnake)
                                    from.SendMessage("That has no venom.");
                                else
                                {
                                    if (this is BlackMamba)
                                        from.AddToBackpack(new BlackMambaVenom(venomCount));
                                    else if (this is Cobra)
                                        from.AddToBackpack(new CobraVenom(venomCount));
                                    else if (this is Copperhead)
                                        from.AddToBackpack(new CopperheadVenom(venomCount));
                                    else if (this is Viper)
                                        from.AddToBackpack(new ViperVenom(venomCount));

                                    from.SendMessage("You milk "+ this.Name +" of its venom.");
                                    this.m_LastMilking = DateTime.Today;
                                    this.PlaySound(0x0DC);
                                }
                            }
                            else
                            from.SendMessage("You do not know enough about agriculture.");
                        }
                        else
                            from.SendMessage("You do not know enough about how to treat and care for animals.");
                    }
                    else
                        from.SendMessage("You do now know enough about poison.");
                }
                else
                    from.SendMessage("It is too soon to milk " + this.Name + ".");
            }
            else
                from.SendMessage("You are not " + this.Name + "'s master.");


            base.OnDoubleClick(from);
        }

        public Serpent(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)poisonHealthDamage);
            writer.Write((int)poisonStamDamage);
            writer.Write((int)poisonManaDamage);
            writer.Write((int)poisonStrDamage);
            writer.Write((int)poisonDexDamage);
            writer.Write((int)poisonIntDamage);
            writer.Write((int)poisonDuration);
            writer.Write((int)poisonSpeed);

            writer.Write((DateTime)m_LastMilking);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            poisonHealthDamage = reader.ReadInt();
            poisonStamDamage = reader.ReadInt();
            poisonManaDamage = reader.ReadInt();
            poisonStrDamage = reader.ReadInt();
            poisonDexDamage = reader.ReadInt();
            poisonIntDamage = reader.ReadInt();
            poisonDuration = reader.ReadInt();
            poisonSpeed = reader.ReadInt();

            m_LastMilking = reader.ReadDateTime();

            int version = reader.ReadInt();
        }
    }
}
