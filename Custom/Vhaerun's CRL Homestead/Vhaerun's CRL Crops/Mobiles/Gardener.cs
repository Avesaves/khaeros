using System; 
using System.Collections; 
using Server; 

namespace Server.Mobiles 
{ 
	public class Gardener : BaseVendor 
	{ 
		private ArrayList m_SBInfos = new ArrayList(); 
		protected override ArrayList SBInfos{ get { return m_SBInfos; } } 

		[Constructable]
		public Gardener() : base( "the gardener" ) 
		{ 
			SetSkill( SkillName.Cooking, 80.0, 100.0 );
			SetSkill( SkillName.Alchemy, 90.0, 110.0 );
			SetSkill( SkillName.HerbalLore, 60.0, 110.0 );
		} 

		public override void InitSBInfo() 
		{ 
			m_SBInfos.Add( new SBGardener() ); 
		} 

		public Gardener( Serial serial ) : base( serial ) 
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
