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
		public TheBlackKnight() : base( Nation.Northern ) 
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
			
			OrnatePlateArms arms = new OrnatePlateArms();
			arms.Hue = 1;
			EquipItem( arms );
			
			OrnatePlateChest chest = new OrnatePlateChest();
			chest.Hue = 1;
			EquipItem( chest );
			
			OrnatePlateLegs legs = new OrnatePlateLegs();
			legs.Hue = 1;
			EquipItem( legs );
			
			OrnatePlateGloves gloves = new OrnatePlateGloves();
			gloves.Hue = 1;
			EquipItem( gloves );
			
			OrnatePlateGorget gorget = new OrnatePlateGorget();
			gorget.Hue = 1;
			EquipItem( gorget );
			
			WingedHelm helm = new WingedHelm();
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
