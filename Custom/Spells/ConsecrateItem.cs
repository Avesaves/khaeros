using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Prompts;

namespace Server.Misc
{
	public class ConsecrateItem : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsItems{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool BackpackItemsOnly{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.ConsecrateItem; } }
		public override string Name{ get{ return "Consecrate Item"; } }
		public override int TotalCost{ get{ return 50; } }
		
		public ConsecrateItem( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && Caster is PlayerMobile && CasterHasEnoughMana )
			{
				PlayerMobile caster = Caster as PlayerMobile;
				
				if( caster.ChosenDeity != Server.Mobiles.ChosenDeity.None && TargetItem is GoldNecklace )
				{
					/* This was from the old religion system, and has been refactored with a new solution below this comment block.
					caster.SendMessage( "What deity do you wish to consecrate this necklace to?" );
					caster.Prompt = new ConsecrateItemPrompt( ( (GoldNecklace)TargetItem ) );
					*/
					GoldNecklace necklace = TargetItem as GoldNecklace;
					if( caster.ChosenDeity == Server.Mobiles.ChosenDeity.Elysia || caster.ChosenDeity == Server.Mobiles.ChosenDeity.Ohlm || caster.ChosenDeity == Server.Mobiles.ChosenDeity.Arianthynt || caster.ChosenDeity == Server.Mobiles.ChosenDeity.Mahtet ) {
						necklace.ItemID = 15234;
					} else if( caster.ChosenDeity == Server.Mobiles.ChosenDeity.Xorgoth ) {
						necklace.ItemID = 15241;
					} else if( caster.ChosenDeity == Server.Mobiles.ChosenDeity.Xipotec ) {
						necklace.ItemID = 15242;
					}
					necklace.Name = "A Holy Symbol of " + caster.ChosenDeity;
					necklace.Resistances.Energy = 5;
					necklace.Hue = 0;
					//necklace.Owner = caster;
					Success = true;
				} else if( caster.ChosenDeity == Server.Mobiles.ChosenDeity.None ) {
					caster.SendMessage( "You must first choose a deity with .ChosenDeity." );
				}
				
				else if( caster.Feats.GetFeatLevel(FeatList.ConsecrateItem) > 1 && (
				         ( caster.Nation == Nation.Northern && TargetItem is ClericCrook ) || 
				         ( caster.Nation == Nation.Western && TargetItem is ShamanStaff ) || 
				         ( caster.Nation == Nation.Tirebladd && TargetItem is ProphetDiviningRod ) ||
				         ( caster.Nation == Nation.Southern && TargetItem is DruidStaff ) || 
				         ( caster.Nation == Nation.Haluaroc && TargetItem is PriestStaff ) ||
				         ( caster.Nation == Nation.Mhordul && TargetItem is MedicineManFetish ) ) )
				{
					BaseStaff staff = TargetItem as BaseStaff;
					if( staff.Name.StartsWith("Consecrated ") ) {
						staff.Name = staff.Name;
					} else {
						staff.Name = "Consecrated " + staff.Name;
					}
					staff.Attributes.DefendChance = 10;
					staff.Attributes.SpellChanneling = 1;
					staff.Owner = caster;
					Success = true;
				}
				
				else if( caster.Feats.GetFeatLevel(FeatList.ConsecrateItem) > 2 && (
				         ( caster.Nation == Nation.Northern && TargetItem is ClericRobe ) || 
				         ( caster.Nation == Nation.Western && TargetItem is ShamanRobe ) || 
				         ( caster.Nation == Nation.Tirebladd && TargetItem is ProphetRobe ) ||
				         ( caster.Nation == Nation.Southern && TargetItem is DruidRobe ) || 
				         ( caster.Nation == Nation.Haluaroc && TargetItem is PriestessGown ) || 
				         ( caster.Nation == Nation.Haluaroc && TargetItem is PriestRobe ) ) )
				{
					BaseOuterTorso robe = TargetItem as BaseOuterTorso;
					robe.Attributes.RegenMana = 2;
					robe.Attributes.RegenHits = 1;
					robe.Owner = caster;
					if( robe.Name.StartsWith("Consecrated") ) {
						robe.Name = robe.Name;
					} else {
						robe.Name = "Consecrated " + robe.Name;
					}
					Success = true;
				}
				
				else if( caster.Nation == Nation.Mhordul && caster.Feats.GetFeatLevel(FeatList.ConsecrateItem) > 2 && TargetItem is MedicineManBoneChest )
				{
					MedicineManBoneChest armor = TargetItem as MedicineManBoneChest;
					armor.Attributes.RegenMana = 2;
					armor.Attributes.RegenHits = 1;
					armor.Owner = caster;
					if( armor.Name.StartsWith("Consecrated") ){
						armor.Name = armor.Name;
					} else {
						armor.Name = "Consecrated " + armor.Name;
					}
					Success = true;
				}
				
				if( Success )
				{
					caster.PlaySound( 508 );
					caster.Mana -= TotalCost;
				}
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "ConsecrateItem", AccessLevel.Player, new CommandEventHandler( ConsecrateItem_OnCommand ) );
		}
		
		[Usage( "ConsecrateItem" )]
        [Description( "Casts Consecrate Item." )]
        private static void ConsecrateItem_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new ConsecrateItem( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.ConsecrateItem) ) ) );
        }
        
	/* This is outdated and no longer necessary. */
/*        private class ConsecrateItemPrompt : Prompt
		{
			private GoldNecklace necklace;
	
			public ConsecrateItemPrompt( GoldNecklace lace )
			{
				necklace = lace;
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				if( from != null && from is PlayerMobile && necklace != null && !necklace.Deleted )
				{
					PlayerMobile m = from as PlayerMobile;
					bool worked = false;
					
					if( m.Nation == Nation.Northern && 
					( text == "Elysia" || text == "Xhenos" || text == "Solian"
					|| text == "Khaliana" || text == "Drachus" || text == "Vhariel" ) )
					{
						worked = true;
						necklace.ItemID = 15234;
					}
					
					if( m.Nation == Nation.Tirebladd && 
					( text == "Ohlm" || text == "Maelmia" || text == "Sankath"
					|| text == "Guthraneil" || text == "Athaelbas" || text == "Sildethel" ) )
					{
						worked = true;
						necklace.ItemID = 15234;
					}
					
					if( m.Nation == Nation.Southern && 
					( text == "Arianthynt" || text == "Gwenhun" || text == "Braichmor"
					|| text == "Eurdrych" || text == "Cariadawyr" || text == "Haearnduw" ) )
					{
						worked = true;
						necklace.ItemID = 15234;
					}
					
					if( m.Nation == Nation.Haluaroc && 
					( text == "Mah'tet" || text == "Kha-tekh" || text == "Neph'at"
					|| text == "Pt-chah" || text == "Waret'ta" || text == "Al-falaq" ) )
					{
						worked = true;
						necklace.ItemID = 15234;
					}
					
					if( m.Nation == Nation.Mhordul && 
					( text == "Xorgoth" || text == "Kyral" || text == "Bulgan"
					|| text == "Zyrgha" || text == "Thorgak" || text == "Drauglyr" ) )
					{
						worked = true;
						necklace.ItemID = 15241;
					}
					
					if( m.Nation == Nation.Western && 
					( text == "Xipotec" || text == "Lahti" || text == "Tlaloc"
					|| text == "Kalia" || text == "Camatli" || text == "Omchali" ) )
					{
						worked = true;
						necklace.ItemID = 15242;
					}
					
					if( worked )
					{
						necklace.Name = "A Holy Symbol of " + text;
						necklace.Resistances.Energy = 5;
						necklace.Hue = 0;
					}
					
					else
						from.SendMessage( "Invalid name." );
				}
				
			}
        }*/

	}
}

