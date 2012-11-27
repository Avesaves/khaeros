using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class MhordulSkullcrusherOgreGuard : BaseKhaerosMobile, IRacialGuard, IMhordul
	{
		[Constructable]
		public MhordulSkullcrusherOgreGuard() : base( Nation.Mhordul ) 
		{
			SetStr( 150 );
			SetDex( 150 );
			SetInt( 150 );

			SetDamage( 15, 20 );
			
			SetHits( 600 );
			
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
			
			this.Fame = 6000;
			
			this.VirtualArmor = 30;
			
			this.Name = "a skullcrusher ogre guard";
			this.Hue = 0;
			Body = 38;
			BaseSoundID = 427;
		}

		public MhordulSkullcrusherOgreGuard(Serial serial) : base(serial)
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
