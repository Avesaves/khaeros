using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;

namespace Server.Mobiles
{
	public class ImperialZombie : BaseKhaerosMobile, IUndead
	{
		[Constructable]
		public ImperialZombie() : base( Nation.Northern ) 
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
			coat.ItemID = 15476;
			coat.Name = "A Tattered Surcoat";
			coat.Hue = 2939;
			this.EquipItem( coat );
			this.EquipItem( new ElegantCloak(2939) );
			
					OrnatePlateLegs vopl = new OrnatePlateLegs();
					vopl.Resource = CraftResource.Bronze;
					vopl.Hue = 2964;
					this.EquipItem( vopl );
					
					OrnatePlateGorget vopo = new OrnatePlateGorget();
					vopo.Resource = CraftResource.Bronze;
					vopo.Hue = 2964;
					this.EquipItem( vopo );
					
					PlateSabatons ps = new PlateSabatons();
					ps.Resource = CraftResource.Bronze;
					ps.Hue = 2964;
					this.EquipItem( ps );
					
					OrnateKiteShield voks = new OrnateKiteShield();
					voks.Resource = CraftResource.Bronze;
					voks.Hue = 2964;
					this.EquipItem( voks );
					
					HorsemanWarhammer hammer = new HorsemanWarhammer();
					hammer.Resource = CraftResource.Iron;
					hammer.Hue = 2964;
				    this.EquipItem( hammer );
				    
				    HalfPlateChest thpc = new HalfPlateChest();
					thpc.Resource = CraftResource.Bronze;
					thpc.Hue = 2964;
					this.EquipItem( thpc );
					
					HalfPlateArms thpa = new HalfPlateArms();
					thpa.Resource = CraftResource.Bronze;
					thpa.Hue = 2964;
					this.EquipItem( thpa );
					
					BoneGloves thpg = new BoneGloves();
					thpg.Hue = 2931;
					this.EquipItem( thpg );
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
		public ImperialZombie(Serial serial) : base(serial)
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
