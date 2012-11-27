using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a satyr's corpse" )]
	public class Satyr : BaseCreature, IMagicalForestCreature, IEnraged
	{
		[Constructable]
		public Satyr() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a Satyr";
			Body = 271;
			BaseSoundID = 451;

			SetStr( 116, 125 );
			SetDex( 46, 65 );
			SetInt( 35 );

			SetHits( 70, 83 );

			SetDamage( 8, 10 );
			
			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Slashing, 30, 35 );
			SetResistance( ResistanceType.Piercing, 30, 35 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Musicianship, 120.1, 130.0 );
			SetSkill( SkillName.Peacemaking, 120.1, 130.0 );
			SetSkill( SkillName.Axemanship, 150.5, 200.0 );
			SetSkill( SkillName.Invocation, 120.1, 130.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.MagicResist, 70.5, 80.0 );
			SetSkill( SkillName.Tactics, 70.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 160.1, 180.0 );

			Fame = 4000;
			Karma = 4000;

			VirtualArmor = 30;
			
			PackItem( new BambooFlute() );
		}
		
		public void SpawnPixies( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newPixies = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newPixies; ++i )
			{
				Pixie pixie = new Pixie();

				pixie.Team = this.Team;
				pixie.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				pixie.MoveToWorld( loc, map );
				pixie.Combatant = target;
				pixie.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				this.PlaySound( 0x504 );
			}
			
			this.Emote( "*nearby pixies come to his aid, summoned by the sound of his flute*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.3 >= Utility.RandomDouble() )
				SpawnPixies( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.3 >= Utility.RandomDouble() )
				SpawnPixies( attacker );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public Satyr( Serial serial ) : base( serial )
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
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
