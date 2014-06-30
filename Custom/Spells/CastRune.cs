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
	public class CastRune : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return false; } }
public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
public override FeatList Feat{ get{ return FeatList.EnchantTrinket; } }

        public override int TotalCost { get { return 50; } }


		
		public CastRune( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                return base.CanBeCast && l.Feats.GetFeatLevel(FeatList.EnchantTrinket) > 2;
            }
        }


		
		public override void Effect( )
		{
            if (CasterHasEnoughMana && Caster.Followers < 4)
            {
                PlayerMobile m = Caster as PlayerMobile;

                Caster.Mana -= TotalCost;
                Success = true;
                int tx = Caster.Location.X;
                int ty = Caster.Location.Y;
                int tz = Caster.Location.Z;
                Caster.PlaySound(492);
                Caster.Hunger -= 1; 
                Caster.Emote("*Scratches a rune into the ground and infuses it with power*"); 
                //Caster.SendMessage("You summon forth an invisible spirit to guard the area for the next few hours...");
                Point3D loc = new Point3D(tx, ty, tz);
                RedMagicTrap trap = new RedMagicTrap(Caster);
                trap.MoveToWorld(loc, Caster.Map);
                Effects.SendLocationParticles(trap, 0x376A, 9, 10, 37, 1, 5025, 1);
                Caster.Followers += 2;
                return; 

            }
            

        }
			

       

         
		public static void Initialize()
		{
			CommandSystem.Register( "CastRune", AccessLevel.Player, new CommandEventHandler( CastRune_OnCommand ) );
		}
		
		[Usage( "CastRune" )]
        [Description( "Creates a red magic trap." )]
        private static void CastRune_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new CastRune( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.EnchantTrinket) ) ) );

        }
	}
}
