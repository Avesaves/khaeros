using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Petal corpse" )]
	public class Petal : BaseCreature, IMagicalForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Petal() : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a Petal";
			Body = 264;
			BaseSoundID = 0x467;
			IsSneaky = true;
			
			RangePerception = 6;

			SetStr( 81, 90 );
			SetDex( 56, 75 );
			SetInt( 81, 105 );

			SetHits( 101, 120 );

			SetDamage( 5, 8 );
			
			SetDamageType( ResistanceType.Slashing, 50 );

			SetResistance( ResistanceType.Blunt, 20, 35 );
			SetResistance( ResistanceType.Piercing, 20, 35 );
			SetResistance( ResistanceType.Slashing, 20, 35 );
			SetResistance( ResistanceType.Poison, 20, 35 );
			SetResistance( ResistanceType.Energy, 60, 75 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 40.1, 60.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 60.5, 70.0 );
			SetSkill( SkillName.Tactics, 20.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 20.1, 50.0 );

			Fame = 3000;
			Karma = 3000;

			VirtualArmor = 25;
			
			PackItem( new FlowerGarland() );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new FairyWing( 2 ) );
            bpc.DropItem(new FairyShroom(3));
             bpc.DropItem(new MutilatedFairy());
		}
		
		public override void OnKilledBy( Mobile mob )
		{
			if( mob is PlayerMobile )
				( (PlayerMobile)mob ).LastOffenseToNature = DateTime.Now;
			
			base.OnKilledBy( mob );
		}

		public Petal( Serial serial ) : base( serial )
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
