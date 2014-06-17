using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Commands;

namespace Server.Mobiles
{
	[CorpseName( "an octopod corpse" )]
	public class Octopod : BaseCreature, ILargePredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Octopod() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an octopod";
			Body = 312;
			BaseSoundID = 0x451;
			
			RangePerception = 8;
			
			IsSneaky = true;

			SetStr( 101, 120 );
			SetDex( 41, 50 );
			SetInt( 35 );

			SetHits( 256, 280 );

			SetDamage( 17, 19 );

			SetDamageType( ResistanceType.Slashing, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Blunt, 30, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 77, 80 );

			SetSkill( SkillName.Invocation, 200.0 );
			SetSkill( SkillName.Magery, 112.6, 117.5 );
			SetSkill( SkillName.Meditation, 200.0 );
			SetSkill( SkillName.MagicResist, 17.6, 20.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 84.1, 88.0 );
			SetSkill( SkillName.Macing, 84.1, 88.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 34;
			
			CanSwim = true;
			CantWalk = true;
			
			this.RangeFight = 4;
		}

		public override int Meat{ get{ return 12; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public Octopod( Serial serial ) : base( serial )
		{
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( this.CanUseSpecial && Utility.RandomMinMax( 1, 100 ) > 60 )
			{
				if( this.Combatant != null )
				{
					if( this.InRange( this.Combatant, 9 ) && this.CanSee( this.Combatant ) )
					{
						this.MovingEffect( this.Combatant, 8139, 12, 1, false, false );
						this.PlaySound( 901 );
						((IKhaerosMobile)this.Combatant).BlindnessTimer = new Misc.EyeRaking.EyeRakingTimer( this.Combatant, 3 );
						((IKhaerosMobile)this.Combatant).BlindnessTimer.Start();
                        this.Combatant.Emote( "*was temporarily blinded by {0}*", this.Name );
                        this.CanUseSpecial = false;
                    }
				}
			}
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new OctopodInkSack());

        }
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average);
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

			if ( BaseSoundID == 357 )
				BaseSoundID = 0x451;
		}
	}
}
