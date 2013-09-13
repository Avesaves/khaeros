using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class InsulariiMiner : BaseKhaerosMobile, IFaction, IInsularii
	{
		[Constructable]
		public InsulariiMiner() : base( Nation.Southern ) 
		{
			int chance = Utility.RandomMinMax( 1, 6 );
			Nation nation = Nation.Southern;
			
			switch( chance )
			{
				case 1: nation = Nation.Southern; break;
				case 2: nation = Nation.Western; break;
				case 3: nation = Nation.Haluaroc; break;
				case 4: nation = Nation.Mhordul; break;
				case 5: nation = Nation.Tirebladd; break;
				case 6: nation = Nation.Northern; break;
			}
			
			Hue = BaseKhaerosMobile.AssignRacialHue( nation );
			HairItemID = BaseKhaerosMobile.AssignRacialHair( nation, this.Female );
			int hairhue = BaseKhaerosMobile.AssignRacialHairHue( nation );
			HairHue = hairhue;
			
			if( !this.Female )
			{
				FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair( nation );
				FacialHairHue = hairhue;
			}
			
			if( this.Backpack == null )
				AddItem( new Backpack() );
			
			SetStr( 150 );
			SetDex( 75 );
			SetInt( 75 );

			SetDamage( 5, 10 );
			
			SetHits( 100 );
			
			SetDamageType( ResistanceType.Blunt, 100 );
			
			SetResistance( ResistanceType.Blunt, 20 );
			SetResistance( ResistanceType.Piercing, 20 );
			SetResistance( ResistanceType.Slashing, 20 );

			SetSkill( SkillName.Anatomy, 60.0 );
			SetSkill( SkillName.Archery, 60.0 );
			SetSkill( SkillName.Fencing, 60.0 );
			SetSkill( SkillName.Macing, 60.0 );
			SetSkill( SkillName.Swords, 60.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.Polearms, 60.0 );
			SetSkill( SkillName.ExoticWeaponry, 60.0 );
			SetSkill( SkillName.Axemanship, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 60.0 );
			
			this.Fame = 2000;
			
			this.VirtualArmor = 20;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Miner";
			Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );
			
			chance = Utility.RandomMinMax( 1, 3 );
			
			switch( chance )
			{
				case 1:
				{	
					EquipItem( new HalfApron() ); 
					break;
				}
					
				case 2:
				{
					EquipItem( new FullApron() ); 
					break;
				}
					
				case 3:
				{
					EquipItem( new Bandana() ); 
					break;
				}
			}
			
			chance = Utility.RandomMinMax( 1, 3 );
			
			switch( chance )
			{
				case 1:
				{	
					EquipItem( new BeltedPants() ); 
					break;
				}
					
				case 2:
				{
					EquipItem( new LongPants() ); 
					break;
				}
					
				case 3:
				{
					EquipItem( new ShortPants() ); 
					break;
				}
			}
			
			chance = Utility.RandomMinMax( 1, 3 );
			
			switch( chance )
			{
				case 1:
				{	
					EquipItem( new Shirt() ); 
					break;
				}
					
				case 2:
				{
					EquipItem( new FancyShirt() ); 
					break;
				}
					
				case 3:
				{
					EquipItem( new Shirt() ); 
					break;
				}
			}
			
			chance = Utility.RandomMinMax( 1, 3 );
			
			switch( chance )
			{
				case 1:
				{	
					EquipItem( new BlackLeatherBoots() ); 
					break;
				}
					
				case 2:
				{
					EquipItem( new Shoes() ); 
					break;
				}
					
				case 3:
				{
					EquipItem( new Boots() ); 
					break;
				}
			}
			
			EquipItem( new LeatherGloves() );
			
			chance = Utility.RandomMinMax( 1, 3 );
			
			switch( chance )
			{
				case 1:
				{	
					EquipItem( new DualPicks() ); 
					break;
				}
					
				case 2:
				{
					EquipItem( new Pickaxe() ); 
					break;
				}
					
				case 3:
				{
					EquipItem( new HammerPick() ); 
					break;
				}
			}
			
			PackItem( new CopperOre( Utility.RandomMinMax( 2, 6 ) ) );
		}

		public InsulariiMiner(Serial serial) : base(serial)
		{
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
