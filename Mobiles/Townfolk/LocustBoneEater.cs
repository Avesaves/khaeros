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
    public class LocustBoneEater : BaseCreature, IRegenerativeCreature, ILargePredator, IHasReach, IEnraged
	{
		[Constructable]
		public LocustBoneEater() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
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
                    Name = "A Locust Bone-Eater";
                    this.Criminal = true;
                    SetStr(350, 400);
                    SetDex(46, 75);
                    SetInt(11, 25);

			SetHits( 500, 700 );

			SetDamage( 40, 50 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 30, 50 );
			SetResistance( ResistanceType.Piercing, 30, 50 );
			SetResistance( ResistanceType.Slashing, 30, 50 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 15, 25 );


			SetSkill( SkillName.Tactics, 100.0, 120.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0, 120.0 );
            SetSkill(SkillName.Macing, 100.0, 120.0);
            SetSkill(SkillName.ExoticWeaponry, 100.0, 120.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 55;
            this.Hue = 1899; 
			int hue = Utility.RandomNeutralHue();
            BoneStaff staff = new BoneStaff();
            staff.ItemID = 0x26BC;
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

                switch (Utility.RandomMinMax(1, 3))
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
                            XmlBleedingWound.BeginBleed(defender, this, Utility.RandomMinMax(20, 30));
                            if ( Utility.Random(6) < 2)
                        	 this.Emote("*Tears into " + defender.Name + "'s flesh with its icy fingers!*");
                            break;
                        }
                }

            }

        }
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new LocustTeeth());

        }
		public LocustBoneEater(Serial serial) : base(serial)
		{
		}


		public override void GenerateLoot()
		{
            AddLoot(LootPack.FilthyRich, 1);
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
