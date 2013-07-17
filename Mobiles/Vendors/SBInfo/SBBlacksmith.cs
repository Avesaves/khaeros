using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBBlacksmith : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBBlacksmith() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 	
				Add( new GenericBuyInfo( typeof( Tongs ), 13, 14, 0xFBB, 0 ) ); 
 
				Add( new GenericBuyInfo( typeof( BronzeShield ), 66, 20, 0x1B72, 0 ) );
				Add( new GenericBuyInfo( typeof( Buckler ), 50, 20, 0x1B73, 0 ) );
				Add( new GenericBuyInfo( typeof( HeaterShield ), 231, 20, 0x1B76, 0 ) );
				Add( new GenericBuyInfo( typeof( MetalShield ), 121, 20, 0x1B7B, 0 ) );


				Add( new GenericBuyInfo( typeof( PlateGorget ), 104, 20, 0x1413, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateChest ), 243, 20, 0x1415, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateLegs ), 218, 20, 0x1411, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateArms ), 188, 20, 0x1410, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateGloves ), 155, 20, 0x1414, 0 ) );

				Add( new GenericBuyInfo( typeof( PlateHelm ), 21, 20, 0x1412, 0 ) );
				Add( new GenericBuyInfo( typeof( CloseHelm ), 18, 20, 0x1408, 0 ) );
				Add( new GenericBuyInfo( typeof( CloseHelm ), 18, 20, 0x1409, 0 ) );
				Add( new GenericBuyInfo( typeof( Helmet ), 31, 20, 0x140A, 0 ) );
				Add( new GenericBuyInfo( typeof( Helmet ), 18, 20, 0x140B, 0 ) );
				Add( new GenericBuyInfo( typeof( NorseHelm ), 18, 20, 0x140E, 0 ) );
				Add( new GenericBuyInfo( typeof( NorseHelm ), 18, 20, 0x140F, 0 ) );
				Add( new GenericBuyInfo( typeof( Bascinet ), 18, 20, 0x140C, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateHelm ), 21, 20, 0x1419, 0 ) );

				Add( new GenericBuyInfo( typeof( ChainCoif ), 17, 20, 0x13BB, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainChest ), 143, 20, 0x13BF, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainLegs ), 149, 20, 0x13BE, 0 ) );

				Add( new GenericBuyInfo( typeof( RingmailChest ), 121, 20, 0x13ec, 0 ) );
				Add( new GenericBuyInfo( typeof( RingmailLegs ), 90, 20, 0x13F0, 0 ) );
				Add( new GenericBuyInfo( typeof( RingmailArms ), 85, 20, 0x13EE, 0 ) );
				Add( new GenericBuyInfo( typeof( RingmailGloves ), 93, 20, 0x13eb, 0 ) );

				Add( new GenericBuyInfo( typeof( Bardiche ), 60, 20, 0xF4D, 0 ) );
				Add( new GenericBuyInfo( typeof( BattleAxe ), 26, 20, 0xF47, 0 ) );
				Add( new GenericBuyInfo( typeof( TwoHandedAxe ), 32, 20, 0x1443, 0 ) );
				Add( new GenericBuyInfo( typeof( Bow ), 35, 20, 0x13B2, 0 ) );
				Add( new GenericBuyInfo( typeof( ButcherKnife ), 14, 20, 0x13F6, 0 ) );
				Add( new GenericBuyInfo( typeof( Crossbow ), 46, 20, 0xF50, 0 ) );
				Add( new GenericBuyInfo( typeof( HeavyCrossbow ), 55, 20, 0x13FD, 0 ) );
				Add( new GenericBuyInfo( typeof( Cutlass ), 24, 20, 0x1441, 0 ) );
				Add( new GenericBuyInfo( typeof( Dagger ), 21, 20, 0xF52, 0 ) );
				Add( new GenericBuyInfo( typeof( Halberd ), 42, 20, 0x143E, 0 ) );
				Add( new GenericBuyInfo( typeof( HammerPick ), 26, 20, 0x143D, 0 ) );
				Add( new GenericBuyInfo( typeof( Katana ), 33, 20, 0x13FF, 0 ) );
				Add( new GenericBuyInfo( typeof( Kryss ), 32, 20, 0x1401, 0 ) );
				Add( new GenericBuyInfo( typeof( Broadsword ), 35, 20, 0xF5E, 0 ) );
				Add( new GenericBuyInfo( typeof( Longsword ), 55, 20, 0xF61, 0 ) );
				Add( new GenericBuyInfo( typeof( ThinLongsword ), 27, 20, 0x13B8, 0 ) );
				Add( new GenericBuyInfo( typeof( Broadsword ), 55, 20, 0x13B9, 0 ) );
				Add( new GenericBuyInfo( typeof( Cleaver ), 15, 20, 0xEC3, 0 ) );
				Add( new GenericBuyInfo( typeof( Axe ), 40, 20, 0xF49, 0 ) );
				Add( new GenericBuyInfo( typeof( BeardedDoubleAxe ), 52, 20, 0xF4B, 0 ) );
				Add( new GenericBuyInfo( typeof( Pickaxe ), 22, 20, 0xE86, 0 ) );
				Add( new GenericBuyInfo( typeof( Pitchfork ), 19, 20, 0xE87, 0 ) );
				Add( new GenericBuyInfo( typeof( Scimitar ), 36, 20, 0x13B6, 0 ) );
				Add( new GenericBuyInfo( typeof( SkinningKnife ), 14, 20, 0xEC4, 0 ) );
				Add( new GenericBuyInfo( typeof( LargeBattleAxe ), 33, 20, 0x13FB, 0 ) );
				Add( new GenericBuyInfo( typeof( WarAxe ), 29, 20, 0x13B0, 0 ) );

				if ( Core.AOS )
				{
					Add( new GenericBuyInfo( typeof( HandScythe ), 35, 20, 0x26BB, 0 ) );
					Add( new GenericBuyInfo( typeof( CrescentBlade ), 37, 20, 0x26C1, 0 ) );
					Add( new GenericBuyInfo( typeof( DoubleBladedStaff ), 35, 20, 0x26BF, 0 ) );
					Add( new GenericBuyInfo( typeof( Lance ), 34, 20, 0x26C0, 0 ) );
					Add( new GenericBuyInfo( typeof( Pike ), 39, 20, 0x26BE, 0 ) );
					Add( new GenericBuyInfo( typeof( Scythe ), 39, 20, 0x26BA, 0 ) );
					Add( new GenericBuyInfo( typeof( CompositeBow ), 50, 20, 0x26C2, 0 ) );
					Add( new GenericBuyInfo( typeof( RepeatingCrossbow ), 57, 20, 0x26C3, 0 ) );
				}

				Add( new GenericBuyInfo( typeof( BlackStaff ), 22, 20, 0xDF1, 0 ) );
				Add( new GenericBuyInfo( typeof( Club ), 16, 20, 0x13B4, 0 ) );
				Add( new GenericBuyInfo( typeof( GnarledStaff ), 16, 20, 0x13F8, 0 ) );
				Add( new GenericBuyInfo( typeof( Mace ), 28, 20, 0xF5C, 0 ) );
				Add( new GenericBuyInfo( typeof( Maul ), 21, 20, 0x143B, 0 ) );
				Add( new GenericBuyInfo( typeof( QuarterStaff ), 19, 20, 0xE89, 0 ) );
				Add( new GenericBuyInfo( typeof( ClericCrook ), 20, 20, 0xE81, 0 ) );
				Add( new GenericBuyInfo( typeof( SmithHammer ), 21, 20, 0x13E3, 0 ) );
				Add( new GenericBuyInfo( typeof( ShortSpear ), 23, 20, 0x1403, 0 ) );
				Add( new GenericBuyInfo( typeof( Spear ), 31, 20, 0xF62, 0 ) );
				Add( new GenericBuyInfo( typeof( WarHammer ), 25, 20, 0x1439, 0 ) );
				Add( new GenericBuyInfo( typeof( WarMace ), 31, 20, 0x1407, 0 ) );

				if( Core.AOS )
				{
					Add( new GenericBuyInfo( typeof( BarbarianScepter ), 39, 20, 0x26BC, 0 ) );
					Add( new GenericBuyInfo( typeof( Glaive ), 40, 20, 0x26BD, 0 ) );
				}
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
			} 
		} 
	} 
}
