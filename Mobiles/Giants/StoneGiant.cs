using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Stone Giants corpse" )]
	public class StoneGiant : BaseCreature, ILargePredator, IGiant, IEnraged
	{
		public override int Height{ get{ return 20; } }
		[Constructable]
		public StoneGiant() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Stone Giant";
			BodyValue = 311;
			BaseSoundID = 609;
			Hue = 1801;

			SetStr( 366, 405 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 642, 671 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Slashing, 100 );

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
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 17000;
			Karma = -17000;

			VirtualArmor = 43;
			
			PackItem( new Granite( 3 ) );

            if( Utility.Random( 100 ) > 74 )
                PackItem( new RewardToken( 1 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GiantsSkull() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 3 );
		}

		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 20; } }
		public override int Hides{ get{ return 11; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 10 ) && this.CanSee( m ) )
					{
						GiantRock rock = new GiantRock();
						rock.ItemID += Utility.RandomMinMax( 0, 3 );
						rock.MoveToWorld( m.Location );
		                rock.Map = m.Map;
						this.PlaySound( 66 );
						this.Emote( "*flings a large rock at " + m.Name + "*" );
						this.CanUseSpecial = false;
						
						if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 25, 30 ) );
					}
				}
			}
		}
		
		public StoneGiant( Serial serial ) : base( serial )
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
