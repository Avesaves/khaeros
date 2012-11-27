using System;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a unicorn corpse" )]
	public class Unicorn : BaseMount, IMagicalForestCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		public override TimeSpan MountAbilityDelay { get { return TimeSpan.FromHours( 1.0 ); } }

		public override void OnDisallowedRider( Mobile m )
		{
			m.SendLocalizedMessage( 1042318 ); // The unicorn refuses to allow you to ride it.
		}

		public override bool DoMountAbility( int damage, Mobile attacker ) // Crash my test server again and I'll skin you, equine aberration.
		{
			return false;
		}

		[Constructable]
		public Unicorn() : this( "a unicorn" )
		{
		}

		[Constructable]
		public Unicorn( string name ) : base( name, 0x7A, 0x3EB4, AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x4BC;

			SetStr( 96, 125 );
			SetDex( 96, 115 );
			SetInt( 86, 125 );

			SetHits( 171, 190 );

			SetDamage( 6, 12 );

			SetDamageType( ResistanceType.Blunt, 75 );
			SetDamageType( ResistanceType.Piercing, 25 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Slashing, 35, 40 );
			SetResistance( ResistanceType.Piercing, 35, 40 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25, 40 );

			SetSkill( SkillName.Invocation, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.2, 80.0 );
			SetSkill( SkillName.Meditation, 50.1, 60.0 );
			SetSkill( SkillName.MagicResist, 75.3, 90.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.UnarmedFighting, 80.5, 92.5 );

			Fame = 9000;
			Karma = 9000;
			
			VirtualArmor = 35;
			
			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 100.0;
		}
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new UnicornHorn() );
			bpc.DropItem( new UnicornHead() );
		}
		
		public override int Meat{ get{ return 12; } }
		public override int Bones{ get{ return 12; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Unicorn( Serial serial ) : base( serial )
		{
		}
		
		public override void OnKilledBy( Mobile mob )
		{
			if( mob is PlayerMobile )
				( (PlayerMobile)mob ).LastOffenseToNature = DateTime.Now;
			
			base.OnKilledBy( mob );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
			{
				Tamable = true;
				ControlSlots = 3;
				MinTameSkill = 89.1;
			}
		}
	}
}
