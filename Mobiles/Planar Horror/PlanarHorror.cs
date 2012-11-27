using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Planar Horror corpse" )]
	public class PlanarHorror : BaseCreature
	{
		[Constructable]
		public PlanarHorror() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Planar Horror";
			Body = 316;
			BaseSoundID = 377;

			SetStr( 111, 200 );
			SetDex( 101, 125 );
			SetInt( 35 );

			SetHits( 751, 800 );

			SetDamage( 31, 43 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 15 );
			SetDamageType( ResistanceType.Energy, 85 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 60.1, 70.0 );
			SetSkill( SkillName.Meditation, 60.1, 70.0 );
			SetSkill( SkillName.MagicResist, 50.1, 75.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 90.0 );

			Fame = 22000;
			Karma = -22000;

			VirtualArmor = 54;
			
			PackItem( new PlanarEssence() );
		}

		public override bool BleedImmune{ get{ return true; } }
		
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.5 >= Utility.RandomDouble() )
				ChangeLooks();
		}

		public virtual void ChangeLooks()
		{
			int chance = Utility.RandomMinMax( 1, 4 );
			
			switch( chance )
			{
				case 1: BodyValue = 316; break;
				case 2: BodyValue = 574; break;
				case 3: BodyValue = 318; break;
				case 4: BodyValue = 775; break;
			}
		}

		public PlanarHorror( Serial serial ) : base( serial )
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
