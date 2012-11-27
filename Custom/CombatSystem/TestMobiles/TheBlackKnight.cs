using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class TheBlackKnight : BaseKhaerosMobile, IBrigand
	{
		[Constructable]
		public TheBlackKnight() : base( Nation.Vhalurian ) 
		{
			Name = "The Black Knight";
			SetStr( 150 );
			SetDex( 500 );
			SetInt( 9000 );

			SetDamage( 10, 15 );
			
			SetHits( 100 );
			
			SetDamageType( ResistanceType.Blunt, 100 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Parry, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			
			this.Fame = 1500;
			
			this.VirtualArmor = 10;
			this.FightMode = FightMode.Closest;
			int hue = Utility.RandomNeutralHue();
			
			VhalurianOrnatePlateArms arms = new VhalurianOrnatePlateArms();
			arms.Hue = 1;
			EquipItem( arms );
			
			VhalurianOrnatePlateChest chest = new VhalurianOrnatePlateChest();
			chest.Hue = 1;
			EquipItem( chest );
			
			VhalurianOrnatePlateLegs legs = new VhalurianOrnatePlateLegs();
			legs.Hue = 1;
			EquipItem( legs );
			
			VhalurianOrnatePlateGloves gloves = new VhalurianOrnatePlateGloves();
			gloves.Hue = 1;
			EquipItem( gloves );
			
			VhalurianOrnatePlateGorget gorget = new VhalurianOrnatePlateGorget();
			gorget.Hue = 1;
			EquipItem( gorget );
			
			TyreanWingedHelm helm = new TyreanWingedHelm();
			helm.Hue = 1;
			EquipItem( helm );
			
			Longsword swrd = new Longsword();
			swrd.Hue = 0;
			EquipItem( swrd );
		}

		public TheBlackKnight(Serial serial) : base(serial)
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
