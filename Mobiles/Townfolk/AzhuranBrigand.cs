using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class WesternBrigand : BaseKhaerosMobile, IBrigand
	{
		[Constructable]
		public WesternBrigand() : base( Nation.Western ) 
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
				EquipItem( new WaistCloth( hue ) );
				EquipItem( new MetallicBra() );
				EquipItem( new ShortBow() );
				PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
				AI = AIType.AI_Archer;
			}
			
			else
			{
				EquipItem( new ElegantWaistCloth( hue ) );
				int chance = Utility.RandomMinMax( 0, 2 );
				
				switch( chance )
				{
					case 0: 
					{
						EquipItem( new SpikedMace() );
						EquipItem( new Buckler() );
						break;
					}
						
					case 1: 
					{
						EquipItem( new PrimitiveSpear() );
						break;
					}
						
					case 2: 
					{
						EquipItem( new SpikedClub() );
						EquipItem( new WoodenShield() );
						break;
					}
				}
			}
			
			RogueMask mask = new RogueMask();
			//mask.Hue = hue;
			EquipItem( mask );
			EquipItem( new Sandals() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public WesternBrigand(Serial serial) : base(serial)
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
