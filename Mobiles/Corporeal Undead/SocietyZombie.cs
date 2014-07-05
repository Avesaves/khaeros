using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;

namespace Server.Mobiles
{
	public class SocietyZombie : BaseKhaerosMobile, IUndead
	{
		[Constructable]
		public SocietyZombie() : base( Nation.Southern ) 
		{
			SetStr( 150 );
			SetDex( 75 );
			SetInt( 75 );

			SetDamage( 10, 15 );
			
			SetHits( 150, 200 );
			
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
			SetResistance( ResistanceType.Fire, 20, 25 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40 );
			
			this.Fame = 12000;
			
			this.VirtualArmor = 0;
			this.FightMode = FightMode.Closest;
			this.Hue = 2931;
		
            Surcoat coat = new Surcoat();
			coat.ItemID = 15483;
			coat.Name = "A Tattered Surcoat";
			coat.Hue = 2939;
			this.EquipItem( coat );
			this.EquipItem( new ElegantCloak(2939) );
			
					PlateChest chest = new PlateChest();
					chest.Resource = CraftResource.Bronze;
					chest.Hue = 2964;
					
					PlateArms arms = new PlateArms();
					arms.Resource = CraftResource.Bronze;
					arms.Hue = 2964;
					
					PlateLegs legs = new PlateLegs();
					legs.Resource = CraftResource.Bronze;
					legs.Hue = 2964;
					
					PlateGorget gorget = new PlateGorget();
					gorget.Resource = CraftResource.Bronze;
					gorget.Hue = 2964;
					
					BoneGloves thpg = new BoneGloves();
					thpg.Hue = 2931;
					this.EquipItem( thpg );
					
					KiteShield shield = new KiteShield();
					shield.Resource = CraftResource.Bronze;
					shield.Name = "Battered Kite Shield";
					shield.Hue = 2964;
					shield.ItemID = 15726;
										
					EquipItem( chest ); 
					EquipItem( arms ); 
					EquipItem( legs );
					EquipItem( gorget ); 
					EquipItem( shield );
					EquipItem( new Longsword() ); 
		}
		
		public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new BlackenedBone());
        }
		public override bool BleedImmune{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
		}
		public SocietyZombie(Serial serial) : base(serial)
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
