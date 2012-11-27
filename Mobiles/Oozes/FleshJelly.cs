using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Flesh Jelly corpse" )] // TODO: Corpse name?
	public class FleshJelly : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public FleshJelly() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Flesh Jelly";
			Body = 319;
			BaseSoundID = 898;

			SetStr( 61, 70 );
			SetDex( 21, 30 );
			SetInt( 10 );
			
			SetHits( 160, 180 );

			SetMana( 0 );

			SetDamage( 12, 14 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 60.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 44;
			
			Ribs ribs = new Ribs( 3 );
			ribs.Hue = 2935;
			ribs.ItemID = 12681;
			ribs.Name = "lard";
			ribs.RotStage = RotStage.Rotten;
			PackItem( ribs );
		}

		public FleshJelly( Serial serial ) : base( serial )
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
