using System;
using Server;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Engines.XmlSpawner2;
using Server.Prompts;


namespace Server.Misc
{
	public class Root : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
public override FeatList Feat{ get{ return FeatList.EnchantTrinket; } }

        public override int TotalCost { get { return 10; } }


		
		public Root( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                return base.CanBeCast && l.Feats.GetFeatLevel(FeatList.EnchantTrinket) > 1;
            }
        }


		
		public override void Effect( )
		{
           PlayerMobile m = Caster as PlayerMobile;
                
            			if (TargetItem.IsChildOf( Caster.Backpack ))
			 {
				Caster.SendMessage("You cannot use that on an item in your pack.");
				Success = false;
				return;
                        }
                       if (TargetItem.Parent is Mobile)
                        {
                            Caster.SendMessage("You cannot use that on an equipped item.");
                            Success = false;
                            return;
                        }

            if (TargetCanBeAffected && CasterHasEnoughMana && TargetItem.Movable == true)
            {
                

                
                Caster.Mana -= TotalCost;

                Container pack = Caster.Backpack;

                Success = true; 
                TargetItem.Movable = false;
                TargetItem.CanBeGrabbed = true;
                Caster.Hunger -= 1; 
                TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*roots itself to the ground*");
                Caster.SendMessage("This item is now locked down, but it can still be taken using .grab ");
                return; 
                }
                Success = false;
                return; 

        }
			

       
        
   
        private void Flare()
        {
          
            if (TargetItem == null || TargetItem.Deleted)
                return;
            
            BaseWeapon w = TargetItem as BaseWeapon;
            w.Identified = false;
            w.HueMod = -1; 
            TargetItem.PublicOverheadMessage(Network.MessageType.Regular, 0, false, "*Loses it's enchantment.*");


        }
         
		public static void Initialize()
		{
			CommandSystem.Register( "Root", AccessLevel.Player, new CommandEventHandler( Root_OnCommand ) );
		}
		
		[Usage( "Root" )]
        [Description( "Enchants using blood magic." )]
        private static void Root_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new Root( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.EnchantTrinket) ) ) );

        }
	}
}
