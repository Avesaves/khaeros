using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{

		[CorpseName( "a locust corpse" )]
    public class StrangeLocust : BaseCreature, IRegenerativeCreature, ILargePredator, IHasReach, IEnraged
	{
		[Constructable]
		public StrangeLocust() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            int phance = Utility.RandomMinMax(0, 1);
            switch (phance)
            {
                case 0:
                    {
                        this.Female = true;
                        this.BodyValue = 401;
                        break;
                    }
                case 1:
                    {
                        this.BodyValue = 400;
                        break;
                    }
            }
                    Name = "A Strange Locust";
                    this.Criminal = true;
                    SetStr(300, 400);
                    SetDex(100, 150);
                    SetInt(11, 25);

			SetHits( 700, 900 );

			SetDamage( 40, 50 );


			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 50 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 70 );
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			SetSkill( SkillName.Tactics, 110.0, 120.0 );
			SetSkill( SkillName.UnarmedFighting, 110.0, 120.0 );
            SetSkill(SkillName.Macing, 110.0, 120.0);
            SetSkill(SkillName.ExoticWeaponry, 110.0, 120.0);

			Fame = 45000;
			Karma = -45000;
            this.RangeFight = 2;
			VirtualArmor = 65;
            this.Hue = 2886; 
			int hue = Utility.RandomNeutralHue();
            BoneStaff staff = new BoneStaff();
            staff.ItemID = 0x3B30;
            staff.Name = "Something strange";
            staff.LootType = LootType.Blessed;
            Robe robe = new Robe();
            Kilt kilt = new Kilt();
            kilt.ItemID = 0x3CA9;
            Claws claws = new Claws();
            claws.Resource = CraftResource.Satin;
            claws.LootType = LootType.Blessed;
            WaistCloth waist = new WaistCloth();
            waist.Layer = Layer.Pants;
            claws.Name = "Hideous appendages"; 
            Mask mask = new Mask(); 
            mask.ItemID = 0x2682;


    
			if( !this.Female )
			{
             this.FacialHairItemID = 12726; 
			 this.FacialHairHue = 2989;
             this.HairItemID = 8465;
             this.HairHue = 2989;
			}
			
			
				int chance = Utility.RandomMinMax( 0, 2 );
				
				switch( chance )
				{
					case 0: 
					{
						EquipItem( staff );
                        EquipItem( kilt );
						break;
					}
						
					case 1: 
					{
						EquipItem( claws );
						break;
					}
						
					case 2: 
					{
						EquipItem( mask );
                        EquipItem( staff );
						break;
					}
				}
                			if( this.Female )
			{
				EquipItem( new RaggedBra( hue ) );
				EquipItem( new SmallRaggedSkirt( hue ) );
                this.HairItemID = 12742;
                this.HairHue = 2989;
			}
                            else
				EquipItem( waist );
			}





        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if ( Utility.Random(100) > 70 )
            {

                switch (Utility.RandomMinMax(1, 4))
                {
                    case 1:
                        {
                            XmlAttach.AttachTo(defender, new XmlFreeze(Utility.RandomMinMax(1, 3)));
                            defender.PlaySound(516);
                            if ( Utility.Random(6) < 2)
                            	this.Emote("*Smashes " + defender.Name + ", with a bloody fist, stunning " + (defender.Female == true ? "her" : "him") + "!*");
                            break;
                        }
                    case 2:
                        {
                            XmlAttach.AttachTo(defender, new XmlFreeze(Utility.RandomMinMax(2, 5)));
                            defender.PlaySound(516);
                            if ( Utility.Random(6) < 2)
                            	this.Emote("*Smashes " + defender.Name + ", across the face, stunning " + (defender.Female == true ? "her" : "him") + " with its stone-like flesh!*");
                            break;
                        }
                    case 3:
                        {
                            XmlBleedingWound.BeginBleed(defender, this, Utility.RandomMinMax(30, 40));
                            if ( Utility.Random(6) < 2)
                        	  this.Emote("*Tears out a chunk of " + defender.Name + "'s flesh!*");
                            break;
                        }
                    case 4:
                        {
                            defender.Poison = Poison.Deadly;
                            if ( Utility.Random(6) < 2)
                            	this.Emote("*Tears out a chunk of " + defender.Name + "'s flesh with it's venomous teeth!*");
                            break;
                        }
                }

            }

        }

		public StrangeLocust(Serial serial) : base(serial)
		{
		}


		public override void GenerateLoot()
		{
            AddLoot(LootPack.Rich, 3);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
