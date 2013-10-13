using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
	public class DivineConsecration : BaseCustomSpell
	{
		public override bool AffectsItems{ get{ return true; } }
		public override bool UsesTarget{ get{ return true; } }
		public override bool BackpackItemsOnly{ get{ return true; } }
		public override FeatList Feat{ get{ return FeatList.DivineConsecration; } }
		public override int BaseCost{ get{ return 10; } }
		public override string Name
		{
			get
			{
				if( !BadCasting && Caster is PlayerMobile )
				{
					/*switch( ((PlayerMobile)Caster).ChosenDeity )
					{
						case ChosenDeity.Mahtet: return "Stick to Snake";
						case ChosenDeity.Xorgoth: return "Blood of Xorgoth";
						case ChosenDeity.Ohlm: return "Consecrate Weapon";
						case ChosenDeity.Elysia: return "Holy Water";
						case ChosenDeity.Arianthynt: return "Living Tree";
					}*/
                    return "Consecrate Weapon";
				}
				
				return "a spell";
			}
		}
		
		public DivineConsecration( Mobile caster, int featLevel ) : base( caster, featLevel )
		{
		}
		
		public override void Effect()
		{
			PlayerMobile caster = Caster as PlayerMobile;
			
			if( Caster is PlayerMobile )
			{
				if( TargetCanBeAffected && TargetItem is BaseWeapon && CasterHasEnoughMana )
				{
					XmlConsecrateWeapon consecrate = XmlAttach.FindAttachment( TargetItem, typeof( XmlConsecrateWeapon ) ) as XmlConsecrateWeapon;
            
		            if( consecrate == null )
		            {
		            	int duration = 5 * FeatLevel;
		            	XmlConsecrateWeapon cons = new XmlConsecrateWeapon( FeatLevel, duration );
		            	XmlAttach.AttachTo( TargetItem, cons );
		            	caster.PlaySound( 503 );
		            	caster.Mana -= TotalCost;
		            	TargetItem.InvalidateProperties();
		            	Success = true;
		            }
				}
			}
			
			/*else if( Caster is PlayerMobile && caster.ChosenDeity == ChosenDeity.Elysia )
			{
				if( TargetCanBeAffected && TargetItem is Pitcher && ( (Pitcher)TargetItem ).Content == BeverageType.Water && ( (Pitcher)TargetItem ).IsFull && CasterHasEnoughMana )
				{
	            	int power = Math.Max( Convert.ToInt32( ( caster.Skills[SkillName.Faith].Base * 0.5 ) * FeatLevel ), 1 );
	            	TargetItem.Delete();
	            	HolyWater water = new HolyWater();
	            	water.Power = power;
	            	Container pack = caster.Backpack;
	            	
	            	if( pack != null )
	            		pack.DropItem( water );
	            	
	            	caster.PlaySound( 503 );
	            	caster.Mana -= TotalCost;
	            	Success = true;
				}
			}
			
			else if( Caster is PlayerMobile && caster.ChosenDeity == ChosenDeity.Mahtet )
			{
				if( TargetCanBeAffected && TargetItem is Kindling && Caster.Followers < Caster.FollowersMax && !BadCasting && HasEnoughMana( Caster, (FeatLevel * 25) ) )
				{
					if( Caster.Followers < Caster.FollowersMax )
					{
						KingCobra summoned = new KingCobra();
						summoned.RawHits = 70 + (FeatLevel * 10);
						summoned.Hits = summoned.RawHits;
						summoned.DamageMin = 5 + FeatLevel;
						summoned.DamageMax = 7 + FeatLevel;
						summoned.RawInt = 120;
						Caster.Mana -= FeatLevel * 25;
						Summon( Caster, summoned, (int)(Caster.Skills[SkillName.Faith].Base * 0.05), 533, false );
					}
						
					TargetItem.Consume( 1 );
	            	Success = true;
				}
			}
			
			else if( Caster is PlayerMobile && caster.ChosenDeity == ChosenDeity.Arianthynt )
			{
				if( TargetCanBeAffected && TargetItem is Kindling && Caster.Followers < Caster.FollowersMax && !BadCasting && HasEnoughMana( Caster, (FeatLevel * 25) ) )
				{
					if( Caster.Followers < Caster.FollowersMax )
					{
						LivingTree summoned = new LivingTree();
						summoned.RawHits = 10 + (FeatLevel * 20);
						summoned.Hits = summoned.RawHits;
						summoned.DamageMin = 7 + FeatLevel;
						summoned.DamageMax = 9 + FeatLevel;
						Caster.Mana -= FeatLevel * 15;
						Summon( Caster, summoned, (int)(Caster.Skills[SkillName.Faith].Base * 0.05), 533, false );
					}
						
					TargetItem.Consume( 1 );
	            	Success = true;
				}
			}
			
			else if( Caster is PlayerMobile && caster.ChosenDeity == ChosenDeity.Xorgoth )
			{
				if( TargetCanBeAffected && TargetItem is Pitcher && ( (Pitcher)TargetItem ).Content == BeverageType.Water && ( (Pitcher)TargetItem ).IsFull && CasterHasEnoughMana )
				{
	            	TargetItem.Delete();
	            	BloodOfXorgoth blood = new BloodOfXorgoth();
	            	blood.Power = FeatLevel;
	            	Container pack = caster.Backpack;
	            	
	            	if( pack != null )
	            		pack.DropItem( blood );
	            	
	            	caster.PlaySound( 503 );
	            	caster.Mana -= TotalCost;
	            	Success = true;
				}
			} */
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "DivineConsecration", AccessLevel.Player, new CommandEventHandler( DivineConsecration_OnCommand ) );
		}
		
		[Usage( "DivineConsecration" )]
        [Description( "Casts Second Racial Power." )]
        private static void DivineConsecration_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile != null )
        	{
        		if( e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Nation != Nation.None )
        		{
        			
        				SpellInitiator( new DivineConsecration( e.Mobile, GetSpellPower( e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.DivineConsecration) ) ) );
        		}
        		
        		else
        			e.Mobile.SendMessage( "You still need to choose your culture." );
        	}
        }
	}
}
