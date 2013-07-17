using System;
using Server;
using Server.Items;

namespace Server.Mobiles 
{ 
	[CorpseName( "a celestial corpse" )] 
	public class Celestial : BaseCreature, ICelestial, IHasReach
	{ 
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}
		
		[Constructable]
		public Celestial() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 0x191; 
			} 
			
			else 
			{ 
				this.Body = 0x190; 
			} 
			
			Name = "A Celestial";
			HairItemID = 12241;
			HairHue = 2415;
			Hue = 1002;

			SetStr( 186, 195 );
			SetDex( 77, 95 );
			SetInt( 51, 70 );

			SetHits( 252, 271 );

			SetDamage( 13, 15 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 50.1, 75.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 6000;
			Karma = 6000;

			VirtualArmor = 50;
			
			HalfPlateChest armour = new HalfPlateChest();
			armour.Name = "Celestial Armour";
			armour.Resource = CraftResource.Starmetal;
			armour.Hue = 2104;
			
			Longsword sword = new Longsword();
			sword.Name = "Celestial Sword";
			sword.Resource = CraftResource.Starmetal;
			sword.Hue = 2832;
			sword.ItemID = 15605;
			
			Cloak wings = new Cloak();
			wings.Hue = 2307;
			wings.ItemID = 15285;
			wings.Name = "Wings";
			
			EquipItem( sword );
			EquipItem( armour );
			EquipItem( wings );
			EquipItem( new OrnateWaistCloth( 2104 ) );
			EquipItem( new Sandals() );
			
			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
		}

		public override int Feathers{ get{ return 100; } }

		public override int GetAngerSound()
		{
			return 0x2F8;
		}

		public override int GetIdleSound()
		{
			return 0x2F8;
		}

		public override int GetAttackSound()
		{
			return Utility.Random( 0x2F5, 2 );
		}

		public override int GetHurtSound()
		{
			return 0x2F9;
		}

		public override int GetDeathSound()
		{
			return 0x2F7;
		}

		public Celestial( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}
}
