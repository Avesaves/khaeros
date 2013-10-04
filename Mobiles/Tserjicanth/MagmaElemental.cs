using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a magma elemental corpse" )]
	public class MagmaElemental : BaseCreature, ILargePredator, IUndead, IEnraged, IHuge
	{
		public override int Height{ get{ return 20; } }
		[Constructable]
		public MagmaElemental() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a magma elemental";
			BodyValue = 194;
			Hue = 2618;

			SetStr( 306, 325 );
			SetDex( 86, 105 );
			SetInt( 36, 75 );

			SetHits( 678, 795 );

			SetDamage( 30, 36 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 30.1, 40.0 );
			SetSkill( SkillName.Magery, 30.1, 40.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 92.5 );
			SetSkill( SkillName.Macing, 90.1, 92.5 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 35000;
			Karma = -35000;

			VirtualArmor = 40;
			
			EquipItem( new LightSource() );

            if( Utility.Random( 100 ) > 74 )
                PackItem( new RewardToken( 3 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.Rich, 1 );
		}

		public override bool HasBreath{ get{ return true; } }
		public override double BreathDamageScalar{ get{ return 0.05; } }
		
		public override void OnAfterMove(Point3D oldLocation)
        {
            Blood gooPuddle = new Blood();
            gooPuddle.ItemID = 0x19AB;
            gooPuddle.Name = "flames";
            gooPuddle.Hue = 0;
            gooPuddle.Light = LightType.Circle150;
            gooPuddle.MoveToWorld(oldLocation, this.Map);
        }
		
		public override int GetAngerSound()
		{
			return 0x10D;
		}

		public override int GetIdleSound()
		{
			return 0x10E;
		}

		public override int GetAttackSound()
		{
			return 0x10F;
		}

		public override int GetHurtSound()
		{
			return 0x110;
		}

		public override int GetDeathSound()
		{
			return 0x111;
		}
		
		public MagmaElemental( Serial serial ) : base( serial )
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
