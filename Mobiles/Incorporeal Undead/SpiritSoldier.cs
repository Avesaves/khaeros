using System; 
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
	[CorpseName( "a spirit soldier" )]
	public class SpiritSoldier : BaseCreature, IUndead, IEnraged
	{ 
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}
		
		[Constructable]
		public SpiritSoldier() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Hue = 12345678; 
			Name = "a spirit soldier";

			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 0x191; 
			} 
			else 
			{ 
				this.Body = 0x190; 
			} 

			SetStr( 186, 200 );
			SetDex( 51, 65 );
			SetInt( 35 );
			
			SetHits( 10, 20 );

			SetDamage( 12, 14 );

			SetDamageType( ResistanceType.Energy, 100 );

			//SetResistance( ResistanceType.Blunt, 35, 45 );
		//	SetResistance( ResistanceType.Piercing, 30, 40 );
			//SetResistance( ResistanceType.Slashing, 30, 40 );

			SetResistance( ResistanceType.Poison, 100 );


			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.Fencing, 46.0, 77.5 );
			SetSkill( SkillName.Macing, 35.0, 57.5 );
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 83.5, 92.5 );
			SetSkill( SkillName.Swords, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Lumberjacking, 125.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 0;
	
			Broadsword weapon = new Broadsword();
			weapon.Hue = 12345678;
			AddItem( weapon );
			
			PlateChest armor = new PlateChest();
			armor.Hue = 12345678;
			AddItem( armor );
			
			PlateHelm helm = new PlateHelm();
			helm.Hue = 12345678;
			AddItem( helm );

			PlateGorget gorget = new PlateGorget();
			gorget.Hue = 12345678;
			AddItem( gorget );
			
			PlateArms arms = new PlateArms();
			arms.Hue = 12345678;
			AddItem( arms );
			
			PlateGloves gloves = new PlateGloves();
			gloves.Hue = 12345678;
			AddItem( gloves );
			
			PlateLegs legs = new PlateLegs();
			legs.Hue = 12345678;
			AddItem( legs );
			
			HeaterShield shield = new HeaterShield();
			shield.Hue = 12345678;
			AddItem( shield );
			
			PackItem( new Necroplasm( 5 ) );
		}

		public SpiritSoldier( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	} 
}
