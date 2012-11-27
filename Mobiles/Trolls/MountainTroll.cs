using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Mountain Troll corpse" )]
	public class MountainTroll : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged, ITroll
	{
		[Constructable]
		public MountainTroll() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Mountain Troll";
			Body = 267;
			BaseSoundID = 461;

			SetStr( 127, 165 );
			SetDex( 106, 115 );
			SetInt( 26, 30 );

			SetHits( 180, 196 );

			SetDamage( 10, 12 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 30;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new TrollBlood() );
		}
		
		public override int Meat{ get{ return 14; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 14; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public MountainTroll( Serial serial ) : base( serial )
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
