using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Fire Giants corpse" )]
	public class FireGiant : BaseCreature, ILargePredator, IGiant, IEnraged
	{
		public override int Height{ get{ return 50; } }
		[Constructable]
		public FireGiant() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Fire Giant";
			BodyValue = 189;
			BaseSoundID = 609;
			Hue = 0;

			SetStr( 886, 825 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 782, 791 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.UnarmedFighting, 110.1, 120.0 );
			SetSkill( SkillName.Macing, 110.1, 120.0 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 46;
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

		public override int Meat{ get{ return 40; } }
		public override int Bones{ get{ return 30; } }
		public override int Hides{ get{ return 15; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 11 ) && this.CanSee( m ) )
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
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 30, 35 ) );
					}
				}
			}
		}
		
		public FireGiant( Serial serial ) : base( serial )
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
