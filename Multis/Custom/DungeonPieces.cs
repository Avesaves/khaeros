using System;
using Server;
using Server.Items;

namespace Server.Multis
{
	public class DungeonFloorA1 : BaseMulti
	{
		[Constructable]public DungeonFloorA1() : base( 0x4018 ) {}
		public DungeonFloorA1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorA2 : BaseMulti
	{
		[Constructable]public DungeonFloorA2() : base( 0x4025 ) {}
		public DungeonFloorA2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorA3 : BaseMulti
	{
		[Constructable]public DungeonFloorA3() : base( 0x401F ) {}
		public DungeonFloorA3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorA4 : BaseMulti
	{
		[Constructable]public DungeonFloorA4() : base( 0x4020 ) {}
		public DungeonFloorA4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorB1 : BaseMulti
	{
		[Constructable]public DungeonFloorB1() : base( 0x4019 ) {}
		public DungeonFloorB1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorB2 : BaseMulti
	{
		[Constructable]public DungeonFloorB2() : base( 0x4026 ) {}
		public DungeonFloorB2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorB3 : BaseMulti
	{
		[Constructable]public DungeonFloorB3() : base( 0x4021 ) {}
		public DungeonFloorB3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorB4 : BaseMulti
	{
		[Constructable]public DungeonFloorB4() : base( 0x4022 ) {}
		public DungeonFloorB4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorC1 : BaseMulti
	{
		[Constructable]public DungeonFloorC1() : base( 0x401A ) {}
		public DungeonFloorC1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorC2 : BaseMulti
	{
		[Constructable]public DungeonFloorC2() : base( 0x4027 ) {}
		public DungeonFloorC2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorC3 : BaseMulti
	{
		[Constructable]public DungeonFloorC3() : base( 0x4023 ) {}
		public DungeonFloorC3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorC4 : BaseMulti
	{
		[Constructable]public DungeonFloorC4() : base( 0x4024 ) {}
		public DungeonFloorC4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorD1 : BaseMulti
	{
		[Constructable]public DungeonFloorD1() : base( 0x401B ) {}
		public DungeonFloorD1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorD2 : BaseMulti
	{
		[Constructable]public DungeonFloorD2() : base( 0x4028 ) {}
		public DungeonFloorD2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorE1 : BaseMulti
	{
		[Constructable]public DungeonFloorE1() : base( 0x401C ) {}
		public DungeonFloorE1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorE2 : BaseMulti
	{
		[Constructable]public DungeonFloorE2() : base( 0x4029 ) {}
		public DungeonFloorE2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorE3 : BaseMulti
	{
		[Constructable]public DungeonFloorE3() : base( 0x401D ) {}
		public DungeonFloorE3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonFloorE4 : BaseMulti
	{
		[Constructable]public DungeonFloorE4() : base( 0x401E ) {}
		public DungeonFloorE4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	
	public class DungeonWallA1 : BaseMulti
	{
		[Constructable]public DungeonWallA1() : base( 0x402A ) {}
		public DungeonWallA1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA2 : BaseMulti
	{
		[Constructable]public DungeonWallA2() : base( 0x402B ) {}
		public DungeonWallA2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA3 : BaseMulti
	{
		[Constructable]public DungeonWallA3() : base( 0x402C ) {}
		public DungeonWallA3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA4 : BaseMulti
	{
		[Constructable]public DungeonWallA4() : base( 0x402D ) {}
		public DungeonWallA4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA5 : BaseMulti
	{
		[Constructable]public DungeonWallA5() : base( 0x402E ) {}
		public DungeonWallA5( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA6 : BaseMulti
	{
		[Constructable]public DungeonWallA6() : base( 0x402F ) {}
		public DungeonWallA6( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA7 : BaseMulti
	{
		[Constructable]public DungeonWallA7() : base( 0x4030 ) {}
		public DungeonWallA7( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA8 : BaseMulti
	{
		[Constructable]public DungeonWallA8() : base( 0x4031 ) {}
		public DungeonWallA8( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA9 : BaseMulti
	{
		[Constructable]public DungeonWallA9() : base( 0x4032 ) {}
		public DungeonWallA9( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA10 : BaseMulti
	{
		[Constructable]public DungeonWallA10() : base( 0x4033 ) {}
		public DungeonWallA10( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA11 : BaseMulti
	{
		[Constructable]public DungeonWallA11() : base( 0x4034 ) {}
		public DungeonWallA11( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA12 : BaseMulti
	{
		[Constructable]public DungeonWallA12() : base( 0x4035 ) {}
		public DungeonWallA12( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA13 : BaseMulti
	{
		[Constructable]public DungeonWallA13() : base( 0x4036 ) {}
		public DungeonWallA13( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallA14 : BaseMulti
	{
		[Constructable]public DungeonWallA14() : base( 0x4037 ) {}
		public DungeonWallA14( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB1 : BaseMulti
	{
		[Constructable]public DungeonWallB1() : base( 0x4038 ) {}
		public DungeonWallB1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB2 : BaseMulti
	{
		[Constructable]public DungeonWallB2() : base( 0x4039 ) {}
		public DungeonWallB2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB3 : BaseMulti
	{
		[Constructable]public DungeonWallB3() : base( 0x403A ) {}
		public DungeonWallB3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB4 : BaseMulti
	{
		[Constructable]public DungeonWallB4() : base( 0x403B ) {}
		public DungeonWallB4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB5 : BaseMulti
	{
		[Constructable]public DungeonWallB5() : base( 0x403C ) {}
		public DungeonWallB5( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB6 : BaseMulti
	{
		[Constructable]public DungeonWallB6() : base( 0x403D ) {}
		public DungeonWallB6( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB7 : BaseMulti
	{
		[Constructable]public DungeonWallB7() : base( 0x403E ) {}
		public DungeonWallB7( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB8 : BaseMulti
	{
		[Constructable]public DungeonWallB8() : base( 0x403F ) {}
		public DungeonWallB8( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB9 : BaseMulti
	{
		[Constructable]public DungeonWallB9() : base( 0x4040 ) {}
		public DungeonWallB9( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB10 : BaseMulti
	{
		[Constructable]public DungeonWallB10() : base( 0x4041 ) {}
		public DungeonWallB10( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB11 : BaseMulti
	{
		[Constructable]public DungeonWallB11() : base( 0x4042 ) {}
		public DungeonWallB11( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB12 : BaseMulti
	{
		[Constructable]public DungeonWallB12() : base( 0x4043 ) {}
		public DungeonWallB12( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB13 : BaseMulti
	{
		[Constructable]public DungeonWallB13() : base( 0x4044 ) {}
		public DungeonWallB13( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallB14 : BaseMulti
	{
		[Constructable]public DungeonWallB14() : base( 0x4045 ) {}
		public DungeonWallB14( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC1 : BaseMulti
	{
		[Constructable]public DungeonWallC1() : base( 0x4046 ) {}
		public DungeonWallC1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC2 : BaseMulti
	{
		[Constructable]public DungeonWallC2() : base( 0x4047 ) {}
		public DungeonWallC2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC3 : BaseMulti
	{
		[Constructable]public DungeonWallC3() : base( 0x4048 ) {}
		public DungeonWallC3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC4 : BaseMulti
	{
		[Constructable]public DungeonWallC4() : base( 0x4049 ) {}
		public DungeonWallC4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC5 : BaseMulti
	{
		[Constructable]public DungeonWallC5() : base( 0x404A ) {}
		public DungeonWallC5( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC6 : BaseMulti
	{
		[Constructable]public DungeonWallC6() : base( 0x404B ) {}
		public DungeonWallC6( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC7 : BaseMulti
	{
		[Constructable]public DungeonWallC7() : base( 0x404C ) {}
		public DungeonWallC7( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC8 : BaseMulti
	{
		[Constructable]public DungeonWallC8() : base( 0x404D ) {}
		public DungeonWallC8( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC9 : BaseMulti
	{
		[Constructable]public DungeonWallC9() : base( 0x404E ) {}
		public DungeonWallC9( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC10 : BaseMulti
	{
		[Constructable]public DungeonWallC10() : base( 0x404F ) {}
		public DungeonWallC10( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC11 : BaseMulti
	{
		[Constructable]public DungeonWallC11() : base( 0x4050 ) {}
		public DungeonWallC11( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC12 : BaseMulti
	{
		[Constructable]public DungeonWallC12() : base( 0x4051 ) {}
		public DungeonWallC12( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC13 : BaseMulti
	{
		[Constructable]public DungeonWallC13() : base( 0x4052 ) {}
		public DungeonWallC13( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonWallC14 : BaseMulti
	{
		[Constructable]public DungeonWallC14() : base( 0x4053 ) {}
		public DungeonWallC14( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	
	public class DungeonStep1 : BaseMulti
	{
		[Constructable]public DungeonStep1() : base( 0x4054 ) {}
		public DungeonStep1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStep2 : BaseMulti
	{
		[Constructable]public DungeonStep2() : base( 0x4055 ) {}
		public DungeonStep2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	
	public class DungeonStairsA1 : BaseMulti
	{
		[Constructable]public DungeonStairsA1() : base( 0x4056 ) {}
		public DungeonStairsA1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsA2 : BaseMulti
	{
		[Constructable]public DungeonStairsA2() : base( 0x4057 ) {}
		public DungeonStairsA2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsA3 : BaseMulti
	{
		[Constructable]public DungeonStairsA3() : base( 0x4058 ) {}
		public DungeonStairsA3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsA4 : BaseMulti
	{
		[Constructable]public DungeonStairsA4() : base( 0x4059 ) {}
		public DungeonStairsA4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsB1 : BaseMulti
	{
		[Constructable]public DungeonStairsB1() : base( 0x405A ) {}
		public DungeonStairsB1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsB2 : BaseMulti
	{
		[Constructable]public DungeonStairsB2() : base( 0x405B ) {}
		public DungeonStairsB2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsB3 : BaseMulti
	{
		[Constructable]public DungeonStairsB3() : base( 0x405C ) {}
		public DungeonStairsB3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsB4 : BaseMulti
	{
		[Constructable]public DungeonStairsB4() : base( 0x405D ) {}
		public DungeonStairsB4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsC1 : BaseMulti
	{
		[Constructable]public DungeonStairsC1() : base( 0x405E ) {}
		public DungeonStairsC1( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsC2 : BaseMulti
	{
		[Constructable]public DungeonStairsC2() : base( 0x405F ) {}
		public DungeonStairsC2( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsC3 : BaseMulti
	{
		[Constructable]public DungeonStairsC3() : base( 0x4060 ) {}
		public DungeonStairsC3( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
	
	public class DungeonStairsC4 : BaseMulti
	{
		[Constructable]public DungeonStairsC4() : base( 0x4061 ) {}
		public DungeonStairsC4( Serial serial ) : base( serial ) {}
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}
