using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
	public class MedicineMan : BaseKhaerosMobile, IMhordul
	{
		[Constructable]
		public MedicineMan() : base( Nation.Mhordul ) 
		{
			SetStr( 100 );
			SetDex( 50 );
			SetInt( 20 );

			SetDamage( 3, 6 );
			
			SetHits( 50 );
			SetMana( 30 );
			
			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.Anatomy, 60.0 );
			SetSkill( SkillName.Archery, 60.0 );
			SetSkill( SkillName.Fencing, 60.0 );
			SetSkill( SkillName.Macing, 60.0 );
			SetSkill( SkillName.Swords, 60.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.Polearms, 60.0 );
			SetSkill( SkillName.ExoticWeaponry, 60.0 );
			SetSkill( SkillName.Axemanship, 60.0 );
			
			this.Fame = 1500;
			
			this.VirtualArmor = 10;
			int hue = Utility.RandomNeutralHue();
			
			if( !this.Female )
				EquipItem( new LoinCloth( 1871 ) );
			
			else
				EquipItem( new RaggedSkirt() );
			
			EquipItem( new MedicineManFetish() );
			EquipItem( new MedicineManBoneChest() );
			EquipItem( new MonsterMask() );
			EquipItem( new Sandals() );
		}

		public MedicineMan(Serial serial) : base(serial)
		{
		}
		
		public virtual void HealCountryman( Mobile m )
		{
			Direction = GetDirectionTo( m );

				m.PlaySound( 0x1F2 );
				m.FixedEffect( 0x376A, 9, 32 );
				this.Animate( 17, 5, 1, true, false, 0 );
				this.Mana -= 10;

				m.Hits += 10;
		}

		public override void OnThink()
		{
			if( this.Mana >= 10 && Utility.RandomMinMax( 1, 10 ) > 8 )
			{
		        ArrayList list = new ArrayList();
		
		        foreach( Mobile m in this.GetMobilesInRange( 6 ) )
		            list.Add( m );
		
		        ArrayList targets = new ArrayList();
		
		        for( int i = 0; i < list.Count; ++i )
		        {
		            Mobile m = (Mobile)list[i];
		
		            if( SpellHelper.ValidIndirectTarget( this, m ) )
		            {
		            	if( m == null || m.Deleted || m.Map != this.Map || !m.Alive || !this.CanSee( m ) || !this.CanBeHarmful( m ) || m.Combatant == this )
		                    continue;
		            	
		            	if( m is BaseCreature && ((BaseCreature)m).ControlMaster != null )
	            			continue;
		                
		            	if( ( m is IMhordul || ( m is PlayerMobile && ( (PlayerMobile)m ).Nation == Nation.Mhordul ) ) && this.InLOS( m ) )
		                    targets.Add( m );
		            }
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
