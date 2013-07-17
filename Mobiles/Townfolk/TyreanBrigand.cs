using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class TyreanBrigand : BaseKhaerosMobile, IBrigand
	{
		[Constructable]
		public TyreanBrigand() : base( Nation.Tyrean ) 
		{
			SetStr( 100 );
			SetDex( 50 );
			SetInt( 20 );

			SetDamage( 3, 6 );
			
			SetHits( 50 );
			
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
			this.FightMode = FightMode.Closest;
			int hue = Utility.RandomNeutralHue();
			
			if( this.Female )
			{
				EquipItem( new Skirt( hue ) );
				EquipItem( new Boots() );
				EquipItem( new RecurveLongBow() );
				PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
				AI = AIType.AI_Archer;
			}
			
			else
			{
				int chance = Utility.RandomMinMax( 0, 2 );
				
				switch( chance )
				{
					case 0: 
					{
						EquipItem( new MetalShield() );
						EquipItem( new HandAndAHalfSword() );
						break;
					}
						
					case 1: 
					{
						EquipItem( new HeavyWarMace() );
						EquipItem( new MetalShield() );
						break;
					}
						
					case 2: 
					{
						EquipItem( new BeardedDoubleAxe() );
						break;
					}
				}

				EquipItem( new LongVest( hue ) );
				EquipItem( new FurBoots() );
				EquipItem( new LongPants( Utility.RandomNeutralHue() ) );
			}
			
			CeremonialMask mask = new CeremonialMask();
			//mask.Hue = hue;
			EquipItem( mask );
			EquipItem( new FancyShirt() );
		}

		public TyreanBrigand(Serial serial) : base(serial)
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
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
