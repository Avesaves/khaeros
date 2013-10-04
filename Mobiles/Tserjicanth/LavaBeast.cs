using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;


namespace Server.Mobiles
{
	[CorpseName( "a lava beast corpse" )]
	public class LavaBeast : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged, IUndead
	{
		[Constructable]
		public LavaBeast() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava beast";
			Body = 318;
			BaseSoundID = 461;
			Hue = 2618;

			SetStr( 366, 405 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 642, 671 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Macing, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 43;

			EquipItem( new LightSource() );
		}
		
		public override void OnAfterMove(Point3D oldLocation)
        {
            Blood gooPuddle = new Blood();
            gooPuddle.ItemID = 0x19AB;
            gooPuddle.Name = "flames";
            gooPuddle.Hue = 0;
            gooPuddle.Light = LightType.Circle150;
            gooPuddle.MoveToWorld(oldLocation, this.Map);
        }

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            if ( Utility.Random(100) > 85 )
			{
                XmlBleedingWound.BeginBleed(defender, this, Utility.RandomMinMax(20, 25));
                this.Emote("*Splashes " + defender.Name + " with lava!*");
                
            } 

        }	
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Rich, 1 );
		}

		public LavaBeast( Serial serial ) : base( serial )
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
