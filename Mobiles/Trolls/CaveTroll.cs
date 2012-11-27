using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Cave Troll corpse" )]
	public class CaveTroll : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged, ITroll
	{
		[Constructable]
		public CaveTroll() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Cave Troll";
			Body = 130;
			BaseSoundID = 461;

			SetStr( 167, 195 );
			SetDex( 26, 35 );
			SetInt( 26, 30 );

			SetHits( 200, 226 );

			SetDamage( 12, 14 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 30;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new TrollBlood() );
		}
		
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public CaveTroll( Serial serial ) : base( serial )
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
