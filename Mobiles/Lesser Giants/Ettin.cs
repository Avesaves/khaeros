using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ettins corpse" )]
	public class Ettin : BaseCreature, ILargePredator, IHasReach, IEnraged, IGiant
	{
		public override int Height{ get{ return 35; } }
		
		[Constructable]
		public Ettin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ettin";
			Body = 18;
			BaseSoundID = 367;

			SetStr( 406, 445 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 822, 831 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 60.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.UnarmedFighting, 110.1, 120.0 );
			SetSkill( SkillName.Macing, 110.1, 120.0 );

			Fame = 22000;
			Karma = -22000;

			VirtualArmor = 49;
            PackItem( new RewardToken( 1 ) );
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 8 ) && this.CanSee( m ) )
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
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 10, 15 ) );
					}
				}
			}
		}

		public override int Meat{ get{ return 16; } }
		public override int Bones{ get{ return 16; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		
		public override bool CanRummageCorpses{ get{ return true; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 4 );
		}

		public Ettin( Serial serial ) : base( serial )
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
