using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Mobiles
{
	public class GenericLeader : BaseCreature, IEnraged
	{
		private DateTime m_NextOrder;
		
		[Constructable]
		public GenericLeader() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			if( this.Backpack == null )
				AddItem( new Backpack() );
			
			SetStr( 250 );
			SetDex( 75 );
			SetInt( 75 );
			
			BodyValue = 400;

			SetDamage( 20, 25 );
			
			SetHits( 600 );
			
			SetDamageType( ResistanceType.Slashing, 100 );
			
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
			
			this.Fame = 16000;
			this.Karma = -10000;
			
			this.VirtualArmor = 20;
			
			Name = "A Generic Leader";
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
	                
	            	if( m is BaseCreature && ( (BaseCreature)m ).Team == this.Team && this.InLOS( m ) )
	                    m.Combatant = this.Combatant;
		        }
		        
		        this.m_NextOrder = DateTime.Now + TimeSpan.FromSeconds( 15 );
		    }
		}

		public GenericLeader(Serial serial) : base(serial)
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
