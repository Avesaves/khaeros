using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an undead minotaur corpse" )]
	public class UndeadMinotaur : BaseCreature, IUndead, IEnraged, IMinotaur
	{
		[Constructable]
		public UndeadMinotaur() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an undead minotaur";
			Body = 259;			
			BaseSoundID = 589;

			SetStr( 116, 120 );
			SetDex( 56, 75 );
			SetInt( 35 );

			SetHits( 350, 350 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 60.1, 70.0 );
			SetSkill( SkillName.MagicResist, 55.1, 70.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 55.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 15;

			PackItem( new Bone( 12 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new MinotaurHorn( 2 ) );
			
			Ribs ribs = new Ribs( 5 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			
			bpc.DropItem( ribs );
		}

		public UndeadMinotaur( Serial serial ) : base( serial )
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
