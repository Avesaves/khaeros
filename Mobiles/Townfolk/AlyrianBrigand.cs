using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class SouthernBrigand : BaseKhaerosMobile, IBrigand
	{
		[Constructable]
		public SouthernBrigand() : base( Nation.Southern ) 
		{
			SetStr( 150 );
			SetStr( 150 );
			SetDex( 150 );
			SetInt( 35 );

			SetDamage( 20, 30 );
			
			SetHits( 150, 200 );
			
			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.Anatomy, 80.0 );
			SetSkill( SkillName.Archery, 80.0 );
			SetSkill( SkillName.Fencing, 80.0 );
			SetSkill( SkillName.Macing, 80.0 );
			SetSkill( SkillName.Swords, 80.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.Polearms, 80.0 );
			SetSkill( SkillName.ExoticWeaponry, 80.0 );
			SetSkill( SkillName.Axemanship, 80.0 );
			SetResistance( ResistanceType.Blunt, 30, 40 );
			SetResistance( ResistanceType.Piercing, 50, 50 );
			SetResistance( ResistanceType.Slashing, 45, 55 );
			SetResistance( ResistanceType.Fire, 20, 25 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 30 );
			
			this.Fame = 8000;
			
			this.VirtualArmor = 45;
			this.FightMode = FightMode.Closest;
			int hue = Utility.RandomNeutralHue();
			
			if( this.Female )
			{
				EquipItem( new Shirt() );
				EquipItem( new LongSkirt( hue ) );
				EquipItem( new CompositeRecurveBow() );
				PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
				AI = AIType.AI_Archer;
			}
			
			else
			{
				EquipItem( new Kilt( hue ) );
				int chance = Utility.RandomMinMax( 0, 2 );
				
				switch( chance )
				{
					case 0: 
					{
						EquipItem( new BattleHammer() );
						break;
					}
						
					case 1: 
					{
						EquipItem( new FlangedMace() );
						EquipItem( new WoodenShield() );
						break;
					}
						
					case 2: 
					{
						EquipItem( new DualPicks() );
						break;
					}
				}
			}
			
			RogueMask mask = new RogueMask();
			mask.Hue = hue;
			EquipItem( mask );
			EquipItem( new Sandals() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public SouthernBrigand(Serial serial) : base(serial)
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
