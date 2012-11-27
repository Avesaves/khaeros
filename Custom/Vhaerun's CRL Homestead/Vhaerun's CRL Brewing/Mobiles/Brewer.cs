using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class Brewer : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public Brewer() : base( "the Brewer" )
		{
			SetSkill( SkillName.HerbalLore, 36.0, 68.0 );
			SetSkill( SkillName.Cooking, 36.0, 68.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBBrewer() );
		}

		public override VendorShoeType ShoeType
		{
			get{ return VendorShoeType.Boots; }
		}

		public override int GetShoeHue()
		{
			return 0;
		}

		public Brewer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
