using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Mobiles
{
	public class InsulariiHealer : BaseKhaerosMobile, IFaction, IInsularii
	{
		[Constructable]
		public InsulariiHealer() : base( Nation.Southern ) 
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
			SetMana( 300 );
			
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
			
			this.Fame = 14000;
			
			this.VirtualArmor = 0;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Healer";
			Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );

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
			boots.Hue = 2591;
			
			GnarledStaff staff = new GnarledStaff();
			staff.Resource = CraftResource.Redwood;
			
			EquipItem( chest ); 
			EquipItem( arms ); 
			EquipItem( legs );
			EquipItem( gorget ); 
			EquipItem( gloves ); 
			EquipItem( boots ); 
			EquipItem( staff ); 
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Insularii Surcoat";
			surcoat.ItemID = 15502;
			surcoat.Hue = 2799;
			
			EquipItem( surcoat ); 
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Poor, 2 );
		}

		public InsulariiHealer(Serial serial) : base(serial)
		{
		}
		
		public virtual void HealCountryman( Mobile m )
		{
			Direction = GetDirectionTo( m );

			m.PlaySound( 0x1F2 );
			m.FixedEffect( 0x376A, 9, 32 );
			this.Animate( 17, 5, 1, true, false, 0 );
			this.Mana -= 30;

			m.Hits += 30;
		}

		public override void OnThink()
		{
			if( this.Mana >= 30 && Utility.RandomMinMax( 1, 10 ) > 8 )
			{
		        ArrayList list = new ArrayList();
		
		        foreach( Mobile m in this.GetMobilesInRange( 6 ) )
		            list.Add( m );
		
		        ArrayList targets = new ArrayList();
		
		        for( int i = 0; i < list.Count; ++i )
		        {
		            Mobile m = (Mobile)list[i];
		            
	            	if( m == null || m.Deleted || m.Map != this.Map || !m.Alive || !this.CanSee( m ) || !this.CanBeHarmful( m ) || m.Combatant == this )
	                    continue;
	            	
	            	if( m is BaseCreature && ((BaseCreature)m).ControlMaster != null )
	            		continue;
	                
	            	if( ( m is IInsularii || ( m is PlayerMobile && ( (PlayerMobile)m ).Friendship.Insularii > 0 ) ) && this.InLOS( m ) )
	                    targets.Add( m );
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealCountryman( m );
					}
		        }
		    }
			
			base.OnThink();
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
