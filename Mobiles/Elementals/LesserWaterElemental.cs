using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lesser water elemental corpse" )]
	public class LesserWaterElemental : BaseCreature, IElemental, IEnraged, IIncorporeal
	{
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }

		[Constructable]
		public LesserWaterElemental () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lesser water elemental";
			Body = 16;
			BaseSoundID = 278;

			SetStr( 126, 155 );
			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 176, 193 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Cold, 20 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 25 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Invocation, 60.1, 75.0 );
			SetSkill( SkillName.Magery, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 100.1, 115.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 70.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
			ControlSlots = 3;
			CanSwim = true;
		}

		public override bool BleedImmune{ get{ return true; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 1 );
		}
		
		        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new PureWater());
            bpc.DropItem(new ElementalGoop());
        }

		public LesserWaterElemental( Serial serial ) : base( serial )
		{
		}
		
		public override void OnThink()
		{
			if( this.Combatant != null && this.Combatant.Alive && this.Combatant.Map == this.Map && 
			   this.InRange( this.Combatant, 10 ) && Utility.RandomMinMax( 1, 100 ) > 95 && 
			  this.GetDistanceToSqrt( this.Combatant ) > 1 )
			{
				Map map = this.Map;

				if ( map == null )
					return;
			
				bool validLocation = false;
				Point3D loc = this.Combatant.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, Combatant.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}
				
				this.Emote( "*shifts itself*" );
				this.Location = loc;
				this.Direction = this.GetDirectionTo( this.Combatant );
			}
			
			base.OnThink();
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
