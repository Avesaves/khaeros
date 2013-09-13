using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class SocietyGuard : BaseKhaerosMobile, IFaction
	{
		[Constructable]
        public SocietyGuard() : this( 0 )
        {
        }
        
		[Constructable]
		public SocietyGuard( int choice ) : base( Nation.Southern ) 
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
			this.Karma = -12000;
			
			this.VirtualArmor = 0;
			
			FightMode = FightMode.Closest;
			
			Name = "Society of Rymaliel Guard";
			
			if( choice > 3 || choice < 1 )
				choice = Utility.RandomMinMax( 1, 3 );
			
			switch( choice )
			{
				case 1:
				{
					PlateChest chest = new PlateChest();
					chest.Resource = CraftResource.Bronze;
					chest.Hue = 2830;
					
					PlateArms arms = new PlateArms();
					arms.Resource = CraftResource.Bronze;
					arms.Hue = 2830;
					
					PlateLegs legs = new PlateLegs();
					legs.Resource = CraftResource.Bronze;
					legs.Hue = 2830;
					
					PlateGorget gorget = new PlateGorget();
					gorget.Resource = CraftResource.Bronze;
					gorget.Hue = 2830;
					
					PlateGloves gloves = new PlateGloves();
					gloves.Resource = CraftResource.Bronze;
					gloves.Hue = 2830;
					
					CloseHelm helm = new CloseHelm();
					helm.Resource = CraftResource.Bronze;
					helm.Hue = 2830;
					
					KiteShield shield = new KiteShield();
					shield.Resource = CraftResource.Bronze;
					shield.Name = "Society of Rymaliel Kite Shield";
					shield.Hue = 2413;
					shield.ItemID = 15726;
										
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( shield );
					EquipItem( helm );
					EquipItem( new Longsword() ); 
					break;
				}
					
				case 2:
				{
					StuddedChest chest = new StuddedChest();
					chest.Resource = CraftResource.BeastLeather;
					chest.Hue = 2830;
					
					StuddedArms arms = new StuddedArms();
					arms.Resource = CraftResource.BeastLeather;
					arms.Hue = 2830;
					
					StuddedLegs legs = new StuddedLegs();
					legs.Resource = CraftResource.BeastLeather;
					legs.Hue = 2830;
					
					StuddedGorget gorget = new StuddedGorget();
					gorget.Resource = CraftResource.BeastLeather;
					gorget.Hue = 2830;
					
					StuddedGloves gloves = new StuddedGloves();
					gloves.Resource = CraftResource.BeastLeather;
					gloves.Hue = 2830;
					
					KiteShield shield = new KiteShield();
					shield.Resource = CraftResource.Bronze;
					shield.Name = "Society of Rymaliel Kite Shield";
					shield.Hue = 2413;
					shield.ItemID = 15726;
					
					ThighBoots boots = new ThighBoots();
					boots.Resource = CraftResource.BeastLeather;
					boots.Hue = 2989;
					
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( shield );
					EquipItem( boots );
					EquipItem( new FlangedMace() ); 
					break;
				}
					
				case 3:
				{
					LeatherChest chest = new LeatherChest();
					chest.Resource = CraftResource.BeastLeather;
					chest.Hue = 2830;
					
					LeatherArms arms = new LeatherArms();
					arms.Resource = CraftResource.BeastLeather;
					arms.Hue = 2830;
					
					LeatherLegs legs = new LeatherLegs();
					legs.Resource = CraftResource.BeastLeather;
					legs.Hue = 2830;
					
					LeatherGorget gorget = new LeatherGorget();
					gorget.Resource = CraftResource.BeastLeather;
					gorget.Hue = 2830;
					
					LeatherGloves gloves = new LeatherGloves();
					gloves.Resource = CraftResource.BeastLeather;
					gloves.Hue = 2830;
					
					ThighBoots boots = new ThighBoots();
					boots.Resource = CraftResource.BeastLeather;
					boots.Hue = 2830;
					
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( gloves ); 
					EquipItem( boots ); 
					EquipItem( new CompositeShortbow() ); 
					AI = AIType.AI_Archer;
					PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
					break;
				}
			}
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Society of Rymaliel Surcoat";
			surcoat.ItemID = 15483;
			
			EquipItem( surcoat ); 
		}

		public SocietyGuard(Serial serial) : base(serial)
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
