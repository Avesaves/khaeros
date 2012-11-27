using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a rotting corpse" )]
	public class Zombie : BaseCreature, IUndead, IEnraged
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Zombie() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a zombie";
			Body = 3;
			BaseSoundID = 471;
			Hue = 2964;

			SetStr( 46, 70 );
			SetDex( 31, 40 );
			SetInt( 5 );

			SetHits( 228, 232 );

			SetDamage( 9, 11 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 35.1, 50.0 );

			Fame = 4200;
			Karma = -4200;

			VirtualArmor = 48;
			
			switch ( Utility.Random( 10 ))
			{
				case 0: PackItem( new LeftArm() ); break;
				case 1: PackItem( new RightArm() ); break;
				case 2: PackItem( new Torso() ); break;
				case 3: PackItem( new Bone() ); break;
				case 4: PackItem( new RibCage() ); break;
				case 5: PackItem( new RibCage() ); break;
				case 6: PackItem( new BonePile() ); break;
				case 7: PackItem( new BonePile() ); break;
				case 8: PackItem( new BonePile() ); break;
				case 9: PackItem( new BonePile() ); break;
			}
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			
			Ribs ribs = new Ribs( 2 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			bpc.DropItem( ribs );
		}

		public override bool BleedImmune{ get{ return true; } }

		public Zombie( Serial serial ) : base( serial )
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
