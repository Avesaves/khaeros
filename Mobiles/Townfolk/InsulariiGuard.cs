using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class InsulariiGuard : BaseKhaerosMobile, IFaction, IInsularii
	{
		[Constructable]
        public InsulariiGuard() : this( 0 )
        {
        }
        
		[Constructable]
		public InsulariiGuard( int choice ) : base( Nation.Southern ) 
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

			SetDamage( 10, 15 );
			
			SetHits( 400 );
			
			SetDamageType( ResistanceType.Blunt, 100 );
			
			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10 );
			SetResistance( ResistanceType.Slashing, 10 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Archery, 100.0 );
			SetSkill( SkillName.Fencing, 100.0 );
			SetSkill( SkillName.Macing, 100.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Polearms, 100.0 );
			SetSkill( SkillName.ExoticWeaponry, 100.0 );
			SetSkill( SkillName.Axemanship, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0 );
			
			this.Fame = 12000;
			
			this.VirtualArmor = 0;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Guard";
			Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );
			
			if( choice > 3 || choice < 1 )
				choice = Utility.RandomMinMax( 1, 3 );
			
			switch( choice )
			{
				case 1:
				{
					PlateChest chest = new PlateChest();
					chest.Resource = CraftResource.Bronze;
					chest.Hue = 2591;
					
					PlateArms arms = new PlateArms();
					arms.Resource = CraftResource.Bronze;
					arms.Hue = 2591;
					
					PlateLegs legs = new PlateLegs();
					legs.Resource = CraftResource.Bronze;
					legs.Hue = 2591;
					
					PlateGorget gorget = new PlateGorget();
					gorget.Resource = CraftResource.Bronze;
					gorget.Hue = 2591;
					
					PlateGloves gloves = new PlateGloves();
					gloves.Resource = CraftResource.Bronze;
					gloves.Hue = 2591;
					
					PlateHelm helm = new PlateHelm();
					helm.Resource = CraftResource.Bronze;
					helm.Name = "Insularii Horned Helm";
					helm.Hue = 2591;
					helm.ItemID = 15380;
					
					Bardiche bardiche = new Bardiche();
					bardiche.Resource = CraftResource.Bronze;
										
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( helm );
					EquipItem( bardiche ); 
					break;
				}
					
				case 2:
				{
					StuddedChest chest = new StuddedChest();
					chest.Resource = CraftResource.BeastLeather;
					chest.Hue = 2591;
					
					StuddedArms arms = new StuddedArms();
					arms.Resource = CraftResource.BeastLeather;
					arms.Hue = 2591;
					
					StuddedLegs legs = new StuddedLegs();
					legs.Resource = CraftResource.BeastLeather;
					legs.Hue = 2591;
					
					StuddedGorget gorget = new StuddedGorget();
					gorget.Resource = CraftResource.BeastLeather;
					gorget.Hue = 2591;
					
					StuddedGloves gloves = new StuddedGloves();
					gloves.Resource = CraftResource.BeastLeather;
					gloves.Hue = 2591;
					
					BlackLeatherBoots boots = new BlackLeatherBoots();
					boots.Resource = CraftResource.BeastLeather;
					boots.Hue = 2591;
					
					DoubleBladedStaff staff = new DoubleBladedStaff();
					staff.Resource = CraftResource.Bronze;
					
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( boots );
					EquipItem( staff ); 
					break;
				}
					
				case 3:
				{
					LeatherChest chest = new LeatherChest();
					chest.Resource = CraftResource.BeastLeather;
					chest.Hue = 2591;
					
					LeatherArms arms = new LeatherArms();
					arms.Resource = CraftResource.BeastLeather;
					arms.Hue = 2591;
					
					LeatherLegs legs = new LeatherLegs();
					legs.Resource = CraftResource.BeastLeather;
					legs.Hue = 2591;
					
					LeatherGorget gorget = new LeatherGorget();
					gorget.Resource = CraftResource.BeastLeather;
					gorget.Hue = 2591;
					
					LeatherGloves gloves = new LeatherGloves();
					gloves.Resource = CraftResource.BeastLeather;
					gloves.Hue = 2591;
					
					BlackLeatherBoots boots = new BlackLeatherBoots();
					boots.Resource = CraftResource.BeastLeather;
					boots.Hue = 2591;
					
					HeavyCrossbow xbow = new HeavyCrossbow();
					xbow.Resource = CraftResource.Redwood;
					
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( boots ); 
					EquipItem( xbow ); 
					AI = AIType.AI_Archer;
					PackItem( new Bolt( Utility.RandomMinMax( 10, 20 ) ) );
					break;
				}
			}
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Insularii Surcoat";
			surcoat.ItemID = 15502;
			surcoat.Hue = 2799;
			
			EquipItem( surcoat ); 
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public InsulariiGuard(Serial serial) : base(serial)
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
