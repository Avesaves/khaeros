using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a goblin scavenger corpse" )]
	public class GoblinScavenger : BaseCreature, ISmallPredator, IGoblin
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public GoblinScavenger() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a goblin scavenger";
			BodyValue = 220;
			BaseSoundID = 594;

			SetStr( 26, 35 );
			SetDex( 66, 75 );
			SetInt( 35 );

			SetHits( 12, 18 );
			SetMana( 0 );

			SetDamage( 2, 4 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 10, 15 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.Poisoning, 40.1, 60.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 30.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 50.0 );

			Fame = 900;
			Karma = -900;

			VirtualArmor = 0;
			
		}

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GoblinBrain() );
            bpc.DropItem(new GoblinGonads()); 
		}

		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }

				public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 1 );
		}

		public GoblinScavenger( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 589 )
				BaseSoundID = 594;
		}
	}
}
