using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a cow corpse" )]
	public class Cow : BaseCreature, ILargePrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		private DateTime m_LastMilking;

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastMilking
        {
            get { return m_LastMilking; }
            set { m_LastMilking = value; }
        }
        
		[Constructable]
		public Cow() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a cow";
			Body = Utility.RandomList( 0xD8, 0xE7 );
			BaseSoundID = 0x78;

			SetStr( 30 );
			SetDex( 15 );
			SetInt( 5 );

			SetHits( 18 );
			SetMana( 0 );

			SetDamage( 1, 4 );

			SetDamage( 1, 4 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 5, 15 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.5 );
			SetSkill( SkillName.UnarmedFighting, 5.5 );

			Fame = 100;
			Karma = 0;

			VirtualArmor = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;
			Female = true;

			if ( Core.AOS && Utility.Random( 1000 ) == 0 ) // 0.1% chance to have mad cows
				FightMode = FightMode.Closest;
		}

		public override int Meat{ get{ return 15; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 4; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BeefHock( 2 ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );

			int random = Utility.Random( 100 );

			if ( random < 25 )
				Tip();
			else if ( random < 20 )
				PlaySound( 120 );
			else if ( random < 40 )
				PlaySound( 121 );
		}

		public void Tip()
		{
			PlaySound( 121 );
			Animate( 8, 0, 3, true, false, 0 );
		}

		public Cow(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
			
			writer.Write( (DateTime) m_LastMilking );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version > 0 )
			{
				m_LastMilking = reader.ReadDateTime();
			}
		}

	}
}
