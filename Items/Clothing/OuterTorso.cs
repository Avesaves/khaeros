using System;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseOuterTorso : BaseClothing
	{
		private Mobile m_Owner;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}
		
		public BaseOuterTorso( int itemID ) : this( itemID, 0 )
		{
		}

		public BaseOuterTorso( int itemID, int hue ) : base( itemID, Layer.OuterTorso, hue )
		{
		}

		public BaseOuterTorso( Serial serial ) : base( serial )
		{
		}
		
		public override bool CanEquip( Mobile from )
		{
			if( Owner != null )
			{
				if( !(from is PlayerMobile) || ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.ConsecrateItem) < 3 )
					return false;
			}
			
			return base.CanEquip( from );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			writer.Write ( (Mobile) m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version > 0 )
				m_Owner = reader.ReadMobile();
		}
	}

	[Flipable( 0x230E, 0x230D )]
	public class GildedDress : BaseOuterTorso
	{
		[Constructable]
		public GildedDress() : this( 0 )
		{
		}

		[Constructable]
		public GildedDress( int hue ) : base( 0x230E, hue )
		{
			Weight = 3.0;
		}

		public GildedDress( Serial serial ) : base( serial )
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

	[Flipable( 0x1F00, 0x1EFF )]
	public class FancyDress : BaseOuterTorso
	{
		[Constructable]
		public FancyDress() : this( 0 )
		{
		}

		[Constructable]
		public FancyDress( int hue ) : base( 0x1F00, hue )
		{
			Weight = 3.0;
		}

		public FancyDress( Serial serial ) : base( serial )
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

	public class DeathRobe : Robe
	{
		public override bool DisplayLootType
		{
			get{ return false; }
		}

		[Constructable]
		public DeathRobe()
		{
			LootType = LootType.Newbied;
			Hue = 2301;
		}

		public new bool Scissor( Mobile from, Scissors scissors )
		{
			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}

		public DeathRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 && Hue == 0 )
				Hue = 2301;
		}
	}

	[Flipable]
	public class RewardRobe : BaseOuterTorso, Engines.VeteranRewards.IRewardItem
	{
		private int m_LabelNumber;
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Number
		{
			get{ return m_LabelNumber; }
			set{ m_LabelNumber = value; InvalidateProperties(); }
		}

		public override int LabelNumber
		{
			get
			{
				if ( m_LabelNumber > 0 )
					return m_LabelNumber;

				return base.LabelNumber;
			}
		}

		public override int BasePhysicalResistance{ get{ return 3; } }

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
				((Mobile)parent).VirtualArmorMod += 2;
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				((Mobile)parent).VirtualArmorMod -= 2;
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public override bool CanEquip( Mobile m )
		{
			if ( !base.CanEquip( m ) )
				return false;

			return !m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( m, this, new object[]{ Hue, m_LabelNumber } );
		}

		[Constructable]
		public RewardRobe() : this( 0 )
		{
		}

		[Constructable]
		public RewardRobe( int hue ) : this( hue, 0 )
		{
		}

		[Constructable]
		public RewardRobe( int hue, int labelNumber ) : base( 0x1F03, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;

			m_LabelNumber = labelNumber;
		}

		public RewardRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_LabelNumber );
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_LabelNumber = reader.ReadInt();
					m_IsRewardItem = reader.ReadBool();
					break;
				}
			}

			if ( Parent is Mobile )
				((Mobile)Parent).VirtualArmorMod += 2;
		}
	}

	[Flipable]
	public class Robe : BaseOuterTorso, IArcaneEquip
	{
		#region Arcane Impl
		private int m_MaxArcaneCharges, m_CurArcaneCharges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxArcaneCharges
		{
			get{ return m_MaxArcaneCharges; }
			set{ m_MaxArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurArcaneCharges
		{
			get{ return m_CurArcaneCharges; }
			set{ m_CurArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsArcane
		{
			get{ return ( m_MaxArcaneCharges > 0 && m_CurArcaneCharges >= 0 ); }
		}

		public void Update()
		{
			if ( IsArcane )
				ItemID = 0x26AE;
			else if ( ItemID == 0x26AE )
				ItemID = 0x1F04;

			if ( IsArcane && CurArcaneCharges == 0 )
				Hue = 0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( IsArcane )
				list.Add( 1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ); // arcane charges: ~1_val~ / ~2_val~
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( IsArcane )
				LabelTo( from, 1061837, String.Format( "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ) );
		}

		public void Flip()
		{
			if ( ItemID == 0x1F03 )
				ItemID = 0x1F04;
			else if ( ItemID == 0x1F04 )
				ItemID = 0x1F03;
		}
		#endregion

		[Constructable]
		public Robe() : this( 0 )
		{
		}

		[Constructable]
		public Robe( int hue ) : base( 0x1F03, hue )
		{
			Weight = 3.0;
		}

		public Robe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			if ( IsArcane )
			{
				writer.Write( true );
				writer.Write( (int) m_CurArcaneCharges );
				writer.Write( (int) m_MaxArcaneCharges );
			}
			else
			{
				writer.Write( false );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					if ( reader.ReadBool() )
					{
						m_CurArcaneCharges = reader.ReadInt();
						m_MaxArcaneCharges = reader.ReadInt();

						if ( Hue == 2118 )
							Hue = ArcaneGem.DefaultArcaneHue;
					}

					break;
				}
			}
		}
	}

	[Flipable( 0x2684, 0x2683 )]
	public class HoodedShroudOfShadows : BaseOuterTorso
	{
		[Constructable]
		public HoodedShroudOfShadows() : this( 0x455 )
		{
		}

		[Constructable]
		public HoodedShroudOfShadows( int hue ) : base( 0x2684, hue )
		{
			LootType = LootType.Blessed;
			Weight = 3.0;
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public HoodedShroudOfShadows( Serial serial ) : base( serial )
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

	[Flipable( 0x1f01, 0x1f02 )]
	public class PlainDress : BaseOuterTorso
	{
		[Constructable]
		public PlainDress() : this( 0 )
		{
		}

		[Constructable]
		public PlainDress( int hue ) : base( 0x1F01, hue )
		{
			Weight = 2.0;
		}

		public PlainDress( Serial serial ) : base( serial )
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

			if ( Weight == 3.0 )
				Weight = 2.0;
		}
	}

	[Flipable( 0x2799, 0x27E4 )]
	public class Kamishimo : BaseOuterTorso
	{
		[Constructable]
		public Kamishimo() : this( 0 )
		{
		}

		[Constructable]
		public Kamishimo( int hue ) : base( 0x2799, hue )
		{
			Weight = 3.0;
		}

		public Kamishimo( Serial serial ) : base( serial )
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

	[Flipable( 0x279C, 0x27E7 )]
	public class HakamaShita : BaseOuterTorso
	{
		[Constructable]
		public HakamaShita() : this( 0 )
		{
		}

		[Constructable]
		public HakamaShita( int hue ) : base( 0x279C, hue )
		{
			Weight = 3.0;
		}

		public HakamaShita( Serial serial ) : base( serial )
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

	[Flipable( 0x2782, 0x27CD )]
	public class ElegantRobe : BaseOuterTorso
	{
		[Constructable]
		public ElegantRobe() : this( 0 )
		{
		}

		[Constructable]
		public ElegantRobe( int hue ) : base( 0x2782, hue )
		{
			Weight = 3.0;
			Name = "Elegant Robe";
		}

		public ElegantRobe( Serial serial ) : base( serial )
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

	[Flipable( 0x2783, 0x27CE )]
	public class FeElegantRobe : BaseOuterTorso
	{
		[Constructable]
		public FeElegantRobe() : this( 0 )
		{
		}

		[Constructable]
		public FeElegantRobe( int hue ) : base( 0x2783, hue )
		{
			Weight = 3.0;
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public FeElegantRobe( Serial serial ) : base( serial )
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

	[Flipable( 0x2FB9, 0x3173 )]
	public class MaleElvenRobe : BaseOuterTorso
	{
		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public MaleElvenRobe() : this( 0 )
		{
		}

		[Constructable]
		public MaleElvenRobe( int hue ) : base( 0x2FB9, hue )
		{
			Weight = 2.0;
		}

		public MaleElvenRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class OrnateRobe : BaseOuterTorso
	{
		[Constructable]
		public OrnateRobe() : this( 0 )
		{
		}

		[Constructable]
		public OrnateRobe( int hue ) : base( 0x2FBA, hue )
		{
			Weight = 2.0;
			Name = "Ornate Robe";
		}

		public OrnateRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ShortPlainDress : BaseOuterTorso
	{
		[Constructable]
		public ShortPlainDress() : this( 0 )
		{
		}

		[Constructable]
		public ShortPlainDress( int hue ) : base( 0x3C3B, hue )
		{
			Weight = 2.0;
			Name = "Short Plain Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ShortPlainDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LongPlainDress : BaseOuterTorso
	{
		[Constructable]
		public LongPlainDress() : this( 0 )
		{
		}

		[Constructable]
		public LongPlainDress( int hue ) : base( 0x3C3A, hue )
		{
			Weight = 2.0;
			Name = "Long Plain Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public LongPlainDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class Nightgown : BaseOuterTorso
	{
		[Constructable]
		public Nightgown() : this( 0 )
		{
		}

		[Constructable]
		public Nightgown( int hue ) : base( 0x3C38, hue )
		{
			Weight = 1.0;
			Name = "Nightgown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public Nightgown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LongDress : BaseOuterTorso
	{
		[Constructable]
		public LongDress() : this( 0 )
		{
		}

		[Constructable]
		public LongDress( int hue ) : base( 0x3C34, hue )
		{
			Weight = 1.0;
			Name = "Long Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public LongDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class DruidRobe : BaseOuterTorso
	{
		[Constructable]
		public DruidRobe() : this( 0 )
		{
		}

		[Constructable]
		public DruidRobe( int hue ) : base( 0x3C9E, hue )
		{
			Weight = 3.0;
			Name = "Druid Robe";
		}

		public DruidRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LongVest : BaseOuterTorso
	{
		[Constructable]
		public LongVest() : this( 0 )
		{
		}

		[Constructable]
		public LongVest( int hue ) : base( 0x3C7E, hue )
		{
			Weight = 3.0;
			Name = "Long Vest";
		}

		public LongVest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class RunedDress : BaseOuterTorso
	{
		[Constructable]
		public RunedDress() : this( 0 )
		{
		}

		[Constructable]
		public RunedDress( int hue ) : base( 0x3C43, hue )
		{
			Weight = 2.0;
			Name = "Runed Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public RunedDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ElegantShortDress : BaseOuterTorso
	{
		[Constructable]
		public ElegantShortDress() : this( 0 )
		{
		}

		[Constructable]
		public ElegantShortDress( int hue ) : base( 0x3C41, hue )
		{
			Weight = 2.0;
			Name = "Elegant Short Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ElegantShortDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class FancyShortDress : BaseOuterTorso
	{
		[Constructable]
		public FancyShortDress() : this( 0 )
		{
		}

		[Constructable]
		public FancyShortDress( int hue ) : base( 0x3C3C, hue )
		{
			Weight = 2.0;
			Name = "Fancy Short Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public FancyShortDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class BeltedDress : BaseOuterTorso
	{
		[Constructable]
		public BeltedDress() : this( 0 )
		{
		}

		[Constructable]
		public BeltedDress( int hue ) : base( 0x3CAC, hue )
		{
			Weight = 2.0;
			Name = "Belted Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public BeltedDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ExpensiveDress : BaseOuterTorso
	{
		[Constructable]
		public ExpensiveDress() : this( 0 )
		{
		}

		[Constructable]
		public ExpensiveDress( int hue ) : base( 0x3C37, hue )
		{
			Weight = 3.0;
			Name = "Expensive Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ExpensiveDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class GildedFancyDress : BaseOuterTorso
	{
		[Constructable]
		public GildedFancyDress() : this( 0 )
		{
		}

		[Constructable]
		public GildedFancyDress( int hue ) : base( 0x3C30, hue )
		{
			Weight = 3.0;
			Name = "Gilded Fancy Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public GildedFancyDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class PuffyDress : BaseOuterTorso
	{
		[Constructable]
		public PuffyDress() : this( 0 )
		{
		}

		[Constructable]
		public PuffyDress( int hue ) : base( 0x3C65, hue )
		{
			Weight = 3.0;
			Name = "Puffy Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public PuffyDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class PriestessGown : BaseOuterTorso
	{
		[Constructable]
		public PriestessGown() : this( 0 )
		{
		}

		[Constructable]
		public PriestessGown( int hue ) : base( 0x3C3D, hue )
		{
			Weight = 3.0;
			Name = "Priestess Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public PriestessGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class PriestRobe : BaseOuterTorso
	{
		[Constructable]
		public PriestRobe() : this( 0 )
		{
		}

		[Constructable]
		public PriestRobe( int hue ) : base( 0x3C9A, hue )
		{
			Weight = 3.0;
			Name = "Priest Robe";
			Hue = 2985;
		}

		public PriestRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class OrnateDress : BaseOuterTorso
	{
		[Constructable]
		public OrnateDress() : this( 0 )
		{
		}

		[Constructable]
		public OrnateDress( int hue ) : base( 0x3C3E, hue )
		{
			Weight = 2.0;
			Name = "Ornate Dress";
			Hue = 2796;
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public OrnateDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class MaleDress : BaseOuterTorso
	{
		[Constructable]
		public MaleDress() : this( 0 )
		{
		}

		[Constructable]
		public MaleDress( int hue ) : base( 0x3C45, hue )
		{
			Weight = 3.0;
			Name = "Male Dress";
		}

		public MaleDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LongOrnateDress : BaseOuterTorso
	{
		[Constructable]
		public LongOrnateDress() : this( 0 )
		{
		}

		[Constructable]
		public LongOrnateDress( int hue ) : base( 0x3C40, hue )
		{
			Weight = 2.0;
			Name = "Long Ornate Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public LongOrnateDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ExpensiveShortDress : BaseOuterTorso
	{
		[Constructable]
		public ExpensiveShortDress() : this( 0 )
		{
		}

		[Constructable]
		public ExpensiveShortDress( int hue ) : base( 0x3C2F, hue )
		{
			Weight = 2.0;
			Name = "Expensive Short Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ExpensiveShortDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class SmallDress : BaseOuterTorso
	{
		[Constructable]
		public SmallDress() : this( 0 )
		{
		}

		[Constructable]
		public SmallDress( int hue ) : base( 0x3C2E, hue )
		{
			Weight = 2.0;
			Name = "Small Dress";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public SmallDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class PlainLongVest : BaseOuterTorso
	{
		[Constructable]
		public PlainLongVest() : this( 0 )
		{
		}

		[Constructable]
		public PlainLongVest( int hue ) : base( 0x3C9C, hue )
		{
			Weight = 3.0;
			Name = "Plain Long Vest";
		}

		public PlainLongVest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ProphetRobe : BaseOuterTorso
	{
		[Constructable]
		public ProphetRobe() : this( 0 )
		{
		}

		[Constructable]
		public ProphetRobe( int hue ) : base( 0x3C6A, hue )
		{
			Weight = 3.0;
			Name = "Prophet Robe";
		}

		public ProphetRobe( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( this.ParentEntity == from )
			{
				Item helm = from.FindItemOnLayer( Layer.Helm );
				Item robe = from.FindItemOnLayer( Layer.OuterTorso );
				
				if( this.ItemID == 15467 )
				{
					if( robe == null )
					{
						this.Layer = Layer.OuterTorso;
						this.ItemID = 15466;
					}
					
					else
						from.SendMessage( "Remove what you are wearing on your outer torso first." );
				}
				
				else
				{
					if( helm == null )
					{
						this.Layer = Layer.Helm;
						this.ItemID = 15467;
					}
					
					else
						from.SendMessage( "Remove what you are wearing on your head first." );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ShamanRobe : BaseOuterTorso
	{
		[Constructable]
		public ShamanRobe() : this( 0 )
		{
		}

		[Constructable]
		public ShamanRobe( int hue ) : base( 0x3C96, hue )
		{
			Weight = 3.0;
			Name = "Shaman Robe";
		}

		public ShamanRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LacedGown : BaseOuterTorso
	{
		[Constructable]
		public LacedGown() : this( 0 )
		{
		}

		[Constructable]
		public LacedGown( int hue ) : base( 0x3C31, hue )
		{
			Weight = 2.0;
			Name = "Laced Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public LacedGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class MedicineManBoneChest : BaseOuterTorso
	{
		[Constructable]
		public MedicineManBoneChest() : this( 0 )
		{
		}

		[Constructable]
		public MedicineManBoneChest( int hue ) : base( 0x3CA9, hue )
		{
			Weight = 3.0;
			Name = "Medicine Man Bone Chest";
			Layer = Layer.MiddleTorso;
		}

		public MedicineManBoneChest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 3 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			if( version < 1 )
			{
				Name = "Medicine Man Bone Chest";
				Layer = Layer.MiddleTorso;
			}
			
			if( version < 3 )
				Resource = CraftResource.Cotton;
		}
	}
	
	public class ClericRobe : BaseOuterTorso
	{
		[Constructable]
		public ClericRobe() : this( 0 )
		{
		}

		[Constructable]
		public ClericRobe( int hue ) : base( 0x3C6C, hue )
		{
			Weight = 3.0;
			Name = "Cleric Robe";
		}

		public ClericRobe( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( this.ParentEntity == from )
			{
				Item helm = from.FindItemOnLayer( Layer.Helm );
				Item robe = from.FindItemOnLayer( Layer.OuterTorso );
				
				if( this.ItemID == 15469 )
				{
					if( robe == null )
					{
						this.Layer = Layer.OuterTorso;
						this.ItemID = 15468;
					}
					
					else
						from.SendMessage( "Remove what you are wearing on your outer torso first." );
				}
				
				else
				{
					if( helm == null )
					{
						this.Layer = Layer.Helm;
						this.ItemID = 15469;
					}
					
					else
						from.SendMessage( "Remove what you are wearing on your head first." );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ExpensiveGown : BaseOuterTorso
	{
		[Constructable]
		public ExpensiveGown() : this( 0 )
		{
		}

		[Constructable]
		public ExpensiveGown( int hue ) : base( 0x3C33, hue )
		{
			Weight = 4.0;
			Name = "Expensive Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ExpensiveGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ExpensiveLongGown : BaseOuterTorso
	{
		[Constructable]
		public ExpensiveLongGown() : this( 0 )
		{
		}

		[Constructable]
		public ExpensiveLongGown( int hue ) : base( 0x3C47, hue )
		{
			Weight = 4.0;
			Name = "Expensive Long Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ExpensiveLongGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ElegantGown : BaseOuterTorso
	{
		[Constructable]
		public ElegantGown() : this( 0 )
		{
		}

		[Constructable]
		public ElegantGown( int hue ) : base( 0x3C2D, hue )
		{
			Weight = 4.0;
			Name = "Elegant Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ElegantGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class GildedGown : BaseOuterTorso
	{
		[Constructable]
		public GildedGown() : this( 0 )
		{
		}

		[Constructable]
		public GildedGown( int hue ) : base( 0x3C35, hue )
		{
			Weight = 2.0;
			Name = "Gilded Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public GildedGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class LongGown : BaseOuterTorso
	{
		[Constructable]
		public LongGown() : this( 0 )
		{
		}

		[Constructable]
		public LongGown( int hue ) : base( 0x3C3F, hue )
		{
			Weight = 2.0;
			Name = "Long Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public LongGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
	
	public class ElegantLongGown : BaseOuterTorso
	{
		[Constructable]
		public ElegantLongGown() : this( 0 )
		{
		}

		[Constructable]
		public ElegantLongGown( int hue ) : base( 0x3C32, hue )
		{
			Weight = 3.0;
			Name = "Elegant Long Gown";
		}

		public override bool AllowMaleWearer{ get{ return false; } }

		public ElegantLongGown( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
