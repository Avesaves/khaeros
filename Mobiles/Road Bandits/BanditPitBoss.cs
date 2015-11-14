using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class BanditPitBoss : BaseKhaerosMobile, IBrigand
	{
		[Constructable]
		public BanditPitBoss() : base( Nation.None ) 
		{
			SetStr( 300 );
			SetStr( 300 );
			SetDex( 300 );
			SetInt( 70 );

			SetDamage( 40, 60 );
			
			SetHits( 300, 400 );
			
			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.Anatomy, 160 );
			SetSkill( SkillName.Archery, 160 );
			SetSkill( SkillName.Fencing, 160 );
			SetSkill( SkillName.Macing, 160 );
			SetSkill( SkillName.Swords, 160 );
			SetSkill( SkillName.Tactics, 160 );
			SetSkill( SkillName.Polearms, 160 );
			SetSkill( SkillName.ExoticWeaponry, 160 );
			SetSkill( SkillName.Axemanship, 160 );
			SetResistance( ResistanceType.Blunt, 60, 80 );
			SetResistance( ResistanceType.Piercing, 80, 80 );
			SetResistance( ResistanceType.Slashing, 65, 75 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 75 );
			SetResistance( ResistanceType.Poison, 75 );
			SetResistance( ResistanceType.Energy, 75 );
			
			this.Fame = 11000;
			
			this.VirtualArmor = 80;
			this.FightMode = FightMode.Closest;
		    this.Title = "the Pitboss";
			int hue = Utility.RandomNeutralHue();
			
			if( this.Female )
			{
                EquipItem(new Tunic());
                EquipItem(new RogueMask());
                EquipItem(new HardenedLeatherBoots());
                EquipItem(new LeatherGloves(hue));
                EquipItem(new LeatherArms(hue));
                EquipItem(new LeatherLegs(hue));
                EquipItem(new Broadsword());

			}
			
			else
			{
                EquipItem(new Tunic());
                EquipItem(new RogueMask());
                EquipItem(new HardenedLeatherBoots());
                EquipItem(new LeatherGloves(hue));
                EquipItem(new LeatherArms(hue));
                EquipItem(new LeatherLegs(hue));
                EquipItem(new Broadsword());
			}
		}

        public BanditPitBoss(Serial serial)
            : base(serial)
		{
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
