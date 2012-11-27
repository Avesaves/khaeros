using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Mobiles
{
	public class GenericHealer : BaseCreature
	{
		[Constructable]
		public GenericHealer() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{			
			if( this.Backpack == null )
				AddItem( new Backpack() );
			
			SetStr( 150 );
			SetDex( 75 );
			SetInt( 75 );
			
			BodyValue = 400;

			SetDamage( 10, 15 );
			
			SetHits( 300 );
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
			
			this.Fame = 12000;
			this.Karma = -10000;
			
			this.VirtualArmor = 10;

			Name = "A Generic Healer";
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Poor, 2 );
		}

		public GenericHealer(Serial serial) : base(serial)
		{
		}
		
		public virtual void HealTeamMember( Mobile m )
		{
			Direction = GetDirectionTo( m );

			m.PlaySound( 0x1F2 );
			m.FixedEffect( 0x376A, 9, 32 );
			
			if( this.BodyValue == 400 || this.BodyValue == 401 )
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
	                
	            	if( this.InLOS( m ) && m is BaseCreature && ( (BaseCreature)m ).Team == this.Team )
	                    targets.Add( m );
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealTeamMember( m );
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
