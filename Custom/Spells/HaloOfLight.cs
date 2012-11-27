using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
	public class HaloOfLight : BaseCustomSpell
	{
		public override bool CanTargetSelf{ get{ return true; } }
		public override bool AffectsMobiles{ get{ return true; } }
		public override bool UsesTarget{ get{ return (FeatLevel < 3 ); } }
		public override bool UsesFullEffect{ get{ return true; } }
		public override bool UsesFaith{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.HaloOfLight; } }
		public override string Name{ get{ return "Halo of Light"; } }
		public override int BaseCost{ get{ return 5; } }
		public override int TotalCost{ get{ return (FeatLevel > 2) == true ? 20 : (FeatLevel * BaseCost); } }
		public override double FullEffect{ get{ return (Caster.Skills[SkillName.Faith].Base * 0.02); } }
		
		public HaloOfLight( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{
				TargetMobile.PlaySound( 483 );
				Caster.Mana -= TotalCost;					
				AddHalo( TargetMobile, FeatLevel, TotalEffect );
				Success = true;
			}
			
			else if( FeatLevel > 2 && CasterHasEnoughMana )
			{
				foreach( Mobile m in Caster.GetMobilesInRange( 5 ) )
            	{
                    TargetMobile = m;
                    
                    if( TargetCanBeAffected && ((IKhaerosMobile)Caster).IsAllyOf( m ) )
						AddHalo( TargetMobile, FeatLevel, TotalEffect );
				}
				
				Caster.Mana -= TotalCost;
				Caster.PlaySound( 483 );
				TargetMobile = null;
				Success = true;
			}
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "HaloOfLight", AccessLevel.Player, new CommandEventHandler( HaloOfLight_OnCommand ) );
		}
		
		[Usage( "HaloOfLight" )]
        [Description( "Casts Halo of Light." )]
        private static void HaloOfLight_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        		SpellInitiator( new HaloOfLight( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.HaloOfLight) ) ) );
        }
        
        public class HaloOfLightTimer : Timer
        {
            private Mobile m;

            public HaloOfLightTimer( Mobile from, int delay )
            	: base( TimeSpan.FromSeconds( delay * 60 ) )
            {
                m = from;
            }

            protected override void OnTick()
            {
                Item light = m.FindItemOnLayer( Layer.Talisman );
			
				if( light != null && light is LightSource && light.Name == "Halo of Light" )
					light.Delete();
				
				if( m.Backpack != null && m.Backpack is ArmourBackpack )
				{
					ArmourBackpack pack = m.Backpack as ArmourBackpack;
					pack.Attributes.NightSight = 0;
				}
            }
        }
        
        public static void AddHalo( Mobile m, int featlevel, int delay )
		{
			LightSource newlight = new LightSource();
			newlight.Layer = Layer.Talisman;
			newlight.Light = LightType.Circle300;
			newlight.Name = "Halo of Light";
			
			Item light = m.FindItemOnLayer( Layer.Talisman );
	
			if( light != null && light is LightSource )
				light.Delete();

            light = m.FindItemOnLayer( Layer.ShopBuy );

            if( light != null && light is LightSource )
                light.Delete();

            if( m.FindItemOnLayer( Layer.ShopBuy ) == null )
				m.EquipItem( newlight );
	
			if( m.Backpack is ArmourBackpack && featlevel > 1 )
			{
				ArmourBackpack pack = m.Backpack as ArmourBackpack;
				pack.Attributes.NightSight = 1;
			}
			
			if( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.HaloTimer != null )
					pm.HaloTimer.Stop();
				
				pm.HaloTimer = new HaloOfLightTimer( m, delay );
				pm.HaloTimer.Start();
			}
			
			else
				new HaloOfLightTimer( m, delay ).Start();
		}
	}
}
