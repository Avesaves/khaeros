using System; 
using Server; 
using Server.Items;

namespace Server.Items
{ 
	public class CommonWeaponBag : Bag 
	{ 
		[Constructable] 
		public CommonWeaponBag() : this( 1 ) 
		{ 
		} 

		[Constructable] 
		public CommonWeaponBag( int amount ) 
		{ 
			DropItem( new Greatsword() );
			DropItem( new Claws() );
			DropItem( new DualDaggers() );
			DropItem( new DualPicks() );
			DropItem( new HandScythe() );
			DropItem( new LightHammer() );
			DropItem( new Rapier() );
			DropItem( new RoyalScepter() );
			DropItem( new SerratedSword() );
			DropItem( new Axe() );
			DropItem( new BattleAxe() );
			DropItem( new BeardedDoubleAxe() );
			DropItem( new HaftedAxe() );
			DropItem( new Hatchet() );
			DropItem( new LargeBattleAxe() );
			DropItem( new Pickaxe() );
			DropItem( new TwoHandedAxe() );
			DropItem( new WarAxe() );
			DropItem( new ButcherKnife() );
			DropItem( new Cleaver() );
			DropItem( new Dagger() );
			DropItem( new SkinningKnife() );
			DropItem( new Club() );
			DropItem( new HammerPick() );
			DropItem( new Mace() );
			DropItem( new Maul() );
			DropItem( new BarbarianScepter() );
			DropItem( new WarHammer() );
			DropItem( new WarMace() );
			DropItem( new CompositeLongbow() );
			DropItem( new FlangedMace() );
			DropItem( new Machete() );
			DropItem( new CompositeShortbow() );
			DropItem( new Shortsword() );
			DropItem( new Bardiche() );
			DropItem( new Halberd() );
			DropItem( new Scythe() );
			DropItem( new Bow() );
			DropItem( new CompositeBow() );
			DropItem( new Crossbow() );
			DropItem( new HeavyCrossbow() );
			DropItem( new SpikedClub() );
			DropItem( new Glaive() );
			DropItem( new DoubleBladedStaff() );
			DropItem( new Pike() );
			DropItem( new Pitchfork() );
			DropItem( new ShortSpear() );
			DropItem( new Spear() );
			DropItem( new WarFork() );
			DropItem( new BlackStaff() );
			DropItem( new GnarledStaff() );
			DropItem( new QuarterStaff() );
			DropItem( new ClericCrook() );
			DropItem( new HandScythe() );
			DropItem( new Broadsword() );
			DropItem( new Cutlass() );
			DropItem( new Kryss() );
			DropItem( new Longsword() );
			DropItem( new Scimitar() );
					
			Name = "Common Weapon Bag";
		} 

		public CommonWeaponBag( Serial serial ) : base( serial ) 
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
