using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a skullcrusher ogre  corpse" )]
	public class SkullcrusherOgre : BaseCreature, ILargePredator, IMhordulFavoredEnemy, IOgre
	{
		public override int Height{ get{ return 35; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }
		
		[Constructable]
		public SkullcrusherOgre() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skullcrusher ogre";
			Body = 38;
			BaseSoundID = 427;

			SetStr( 126, 255 );
			SetDex( 46, 65 );
			SetInt( 35 );

			SetHits( 250, 277 );
			SetMana( 0 );

			SetDamage( 13, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 25 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 42;
			
			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 80.0;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 14; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 7; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public SkullcrusherOgre( Serial serial ) : base( serial )
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
