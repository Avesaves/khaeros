using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class Etrius : BaseKhaerosMobile, IFaction, IInsularii
	{
		[Constructable]
		public Etrius() : base( Nation.Northern ) 
		{
			Hue = 1038;
			HairItemID = 12751;
			FacialHairItemID = 12722;
			HairHue = 2990;
			FacialHairHue = 2990;
			
			if( this.Backpack == null )
				AddItem( new Backpack() );
			
			SetStr( 150 );
			SetDex( 150 );
			SetInt( 150 );

			SetDamage( 20, 25 );
			
			SetHits( 1000 );
			SetMana( 200 );
			
			SetDamageType( ResistanceType.Blunt, 100 );
			
			SetResistance( ResistanceType.Blunt, 30 );
			SetResistance( ResistanceType.Piercing, 30 );
			SetResistance( ResistanceType.Slashing, 30 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Archery, 100.0 );
			SetSkill( SkillName.Fencing, 100.0 );
			SetSkill( SkillName.Macing, 100.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Polearms, 100.0 );
			SetSkill( SkillName.ExoticWeaponry, 100.0 );
			SetSkill( SkillName.Axemanship, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0 );
			
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Invocation, 100.0 );
			SetSkill( SkillName.Concentration, 100.0 );
			
			this.Fame = 20000;
			
			this.VirtualArmor = 30;
			
			FightMode = FightMode.Closest;
			
			Title = "the Insularii Mage";
			Name = "Etrius";
			
			RunicCloak cloak = new RunicCloak();
			cloak.Hue = 2799;
			
			BeltedPants pants = new BeltedPants();
			pants.Hue = 2591;
			
			ExtravagantShirt shirt = new ExtravagantShirt();
			shirt.Hue = 2591;
			
			FancyGloves gloves = new FancyGloves();
			gloves.Hue = 2591;
			
			ElegantShoes shoes = new ElegantShoes();
			shoes.Hue = 2591;
			
			QuarterStaff staff = new QuarterStaff();
			staff.ItemID = 15813;
			staff.Name = "Insularii Mage Staff";
			
			Cowl cowl = new Cowl();
			cowl.Hue = 2799;
			
			Surcoat surcoat = new Surcoat();
			surcoat.Name = "Insularii Surcoat";
			surcoat.ItemID = 15502;
			surcoat.Hue = 2799;
			
			EquipItem( shirt ); 
			EquipItem( cloak ); 
			EquipItem( pants );
			EquipItem( gloves ); 
			EquipItem( shoes );
			EquipItem( staff ); 
			EquipItem( cowl ); 
			EquipItem( surcoat ); 
			
			this.AI = AIType.AI_Mage;
		}

		public Etrius(Serial serial) : base(serial)
		{
		}
		
		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if( this == null || this.Deleted || !this.Alive )
				return;
			
			if( this.Hits < ( this.HitsMax / 5 ) || willKill )
			{
				this.Hits = this.HitsMax;
				this.Blessed = true;
				this.Warmode = false;
				this.Say( "My, my... You people are stronger than you look like." );
				Timer.DelayCall( TimeSpan.FromSeconds( 3 ), new TimerCallback( this.FleeSequenceOne ) );
			}
		}
		
		private void FleeSequenceOne()
		{
			if( this == null || this.Deleted || !this.Alive )
				return;
			
			this.Say( "No matter... I have something that will take care of you. Ta ta!" );
			Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( this.FleeSequenceTwo ) );
			this.Animate( 17, 5, 1, true, false, 0 );
		}
		
		private void FleeSequenceTwo()
		{
			if( this == null || this.Deleted || !this.Alive )
				return;
			
			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
			this.PlaySound( 0x1FE );
			SpawnAbominations();
			this.Delete();
		}
		
		private void SpawnAbominations()
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSkeletons = 2;

			for ( int i = 0; i < newSkeletons; ++i )
			{
				MinotaurAbomination skeleton = new MinotaurAbomination();

				skeleton.Team = this.Team;
				skeleton.FightMode = FightMode.Closest;

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

				skeleton.MoveToWorld( loc, map );
				skeleton.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				skeleton.VanishEmote = "*crumbles into dust*";
				Effects.SendLocationParticles( EffectItem.Create( skeleton.Location, skeleton.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
