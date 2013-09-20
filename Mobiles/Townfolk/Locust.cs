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
    public class Locust : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged
	{
		[Constructable]
		public Locust() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
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
            SetStr( 180, 200 );
			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 176, 193 );

			SetDamage( 14, 16 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 50.1, 95.0 );
			SetSkill( SkillName.Tactics, 60.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 100.0 );
            SetSkill(SkillName.Macing, 60.1, 100.0);
            SetSkill(SkillName.ExoticWeaponry, 60.1, 100.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 34;
            this.Hue = 0; 
			int hue = Utility.RandomNeutralHue();
            BoneStaff staff = new BoneStaff();
            staff.ItemID = 0x26BC;
            Robe robe = new Robe();
            Kilt kilt = new Kilt();
            kilt.ItemID = 0x3CA9;
            Claws claws = new Claws();
 
            claws.Name = "Hideous appendages"; 
            Mask mask = new Mask(); 
            mask.ItemID = 0x2682;


    
			if( !this.Female )
			{
             this.FacialHairItemID = 0x31B4; 
			 this.FacialHairHue = hue; 
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
			}
                            else
				EquipItem( new WaistCloth() );
			}





        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if ( Utility.Random(100) > 90 )
            {

                switch (Utility.RandomMinMax(1, 3))
                {
                    case 1:
                        {
                            XmlAttach.AttachTo(defender, new XmlStamDrain(3, 10, 5));
                            this.Emote("*Barrels into " + defender.Name + ", knocking out " + (defender.Female == true ? "her" : "his") + " wind!*");
                            break;
                        }
                    case 2:
                        {
                            XmlAttach.AttachTo(defender, new XmlFreeze(Utility.RandomMinMax(1, 3)));
                            defender.PlaySound(516);
                            this.Emote("*Smashes " + defender.Name + ", with a bloody fist, stunning " + (defender.Female == true ? "her" : "him") + "!*");
                            break;
                        }
                    case 3:
                        {
                            XmlBleedingWound.BeginBleed(defender, this, Utility.RandomMinMax(5, 20));
                            this.Emote("*Tears at " + defender.Name + "'s flesh with its fingers!*");
                            break;
                        }
                }

            }

        }

		public Locust(Serial serial) : base(serial)
		{
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
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
