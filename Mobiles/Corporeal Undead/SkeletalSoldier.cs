using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class SkeletalSoldier : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public SkeletalSoldier() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal soldier";
			Body = 147;
			BaseSoundID = 451;

			SetStr( 96, 120 );
			SetDex( 56, 65 );
			SetInt( 35 );

			SetHits( 288, 295 );

			SetDamage( 14, 16 );

			SetDamageType( ResistanceType.Slashing, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 95.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 60;
			
			PackItem( new Bone( 5 ) );
			
			Shortsword shortsword = new Shortsword();
			
			shortsword.HitPoints = 5;
			shortsword.MaxHitPoints = 5;
			shortsword.Hue = 2964;
			shortsword.Name = "rusty shortsword";
			
			PackItem( shortsword );
			
			Buckler buckler = new Buckler();
			
			buckler.HitPoints = 5;
			buckler.MaxHitPoints = 5;
			buckler.Hue = 2964;
			buckler.Name = "rusty buckler";
			
			PackItem( buckler );
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new BlackenedBone());
        }
		public override bool BleedImmune{ get{ return true; } }

		public SkeletalSoldier( Serial serial ) : base( serial )
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
