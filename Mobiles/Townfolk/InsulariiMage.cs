using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class InsulariiMage : BaseKhaerosMobile, IFaction, IInsularii
	{
		[Constructable]
		public InsulariiMage() : base( Nation.Southern ) 
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
			SetMana( 150 );
			
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
			
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Invocation, 100.0 );
			SetSkill( SkillName.Concentration, 100.0 );
			
			this.Fame = 16000;
			
			this.VirtualArmor = 10;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Mage";
			Name = BaseKhaerosMobile.GiveInsulariiName( this.Female );
			
			StuddedChest chest = new StuddedChest();
			chest.Hue = 2591;
			
			StuddedArms arms = new StuddedArms();
			arms.Hue = 2591;
			
			StuddedLegs legs = new StuddedLegs();
			legs.Hue = 2591;
			
			StuddedGorget gorget = new StuddedGorget();
			gorget.Hue = 2591;
			
			StuddedGloves gloves = new StuddedGloves();
			gloves.Hue = 2591;
			
			BlackLeatherBoots boots = new BlackLeatherBoots();
			boots.Hue = 2591;
			
			QuarterStaff staff = new QuarterStaff();
			staff.ItemID = 15813;
			staff.Name = "Insularii Mage Staff";
			
			Cowl cowl = new Cowl();
			cowl.Hue = 2799;
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Insularii Surcoat";
			surcoat.ItemID = 15502;
			surcoat.Hue = 2799;
			
			EquipItem( chest ); 
			EquipItem( arms ); 
			EquipItem( legs );
			EquipItem( gorget ); 
			EquipItem( gloves ); 
			EquipItem( boots );
			EquipItem( staff ); 
			EquipItem( cowl ); 
			EquipItem( surcoat ); 
			
			this.AI = AIType.AI_Mage;
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
			AddLoot( LootPack.Poor, 2 );
		}

		public InsulariiMage(Serial serial) : base(serial)
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
