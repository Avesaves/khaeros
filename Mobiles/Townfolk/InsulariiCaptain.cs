using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Mobiles
{
	public class InsulariiCaptain : BaseKhaerosMobile, IFaction, IInsularii
	{
		private DateTime m_NextOrder;
		
		[Constructable]
		public InsulariiCaptain() : base( Nation.Southern ) 
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
			
			SetStr( 250 );
			SetDex( 75 );
			SetInt( 75 );

			SetDamage( 20, 25 );
			
			SetHits( 800 );
			
			SetDamageType( ResistanceType.Blunt, 100 );
			
			SetResistance( ResistanceType.Blunt, 20 );
			SetResistance( ResistanceType.Piercing, 20 );
			SetResistance( ResistanceType.Slashing, 20 );

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
			
			this.Fame = 18000;
			
			this.VirtualArmor = 0;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Captain";
			Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );
			
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
			
			Glaive staff = new Glaive();
			staff.Resource = CraftResource.Bronze;
								
			EquipItem( chest ); 
			EquipItem( arms ); 
			EquipItem( legs );
			EquipItem( gorget ); 
			EquipItem( gloves ); 
			EquipItem( helm );
			EquipItem( staff ); 
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Insularii Surcoat";
			surcoat.ItemID = 15502;
			surcoat.Hue = 2799;
			
			EquipItem( surcoat ); 
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 3 );
			AddLoot( LootPack.Poor, 2 );
		}
		
		public override void OnThink()
		{
			if( DateTime.Compare( DateTime.Now, this.m_NextOrder ) < 0 && this.Alive && !this.Blessed && 
			   this.Combatant != null && this.Combatant.Alive && !this.Combatant.Blessed && 
			   this.CanSee( this.Combatant ) && this.InLOS( this.Combatant ) && this.InRange( this.Combatant, 15 ) )
			{
		        ArrayList list = new ArrayList();
		
		        foreach( Mobile m in this.GetMobilesInRange( 15 ) )
		            list.Add( m );
		
		        for( int i = 0; i < list.Count; ++i )
		        {
		            Mobile m = (Mobile)list[i];
		            
	            	if( m == null || m.Deleted || m.Map != this.Map || !m.Alive || !this.CanSee( m ) || m.Blessed || m.Combatant == this )
	                    continue;
	                
	            	if( m is IInsularii && this.InLOS( m ) )
	                    m.Combatant = this.Combatant;
		        }
		        
		        this.m_NextOrder = DateTime.Now + TimeSpan.FromSeconds( 15 );
		    }

            base.OnThink();
		}

		public InsulariiCaptain(Serial serial) : base(serial)
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
