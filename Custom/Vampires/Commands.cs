using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.Targeting;
using Server.Prompts;
using Server.Engines.XmlSpawner2;

namespace Server.Misc.Vampires
{
	public class Commands
	{
		public static void Initialize()
		{
			CommandSystem.Register( "VP", AccessLevel.Player, new CommandEventHandler( VP_OnCommand ) );
		}
		
		[Usage( "VP" )]
        [Description( "View paperdoll." )]
        private static void VP_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( !m.IsVampire )
        	{
        		m.DisplayPaperdollTo( m );
        		return;
        	}
        	
        	if( e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        	{
        		m.SendMessage( "Please add an argument to the command." );
        		m.SendMessage( "Current default options for VP include: \".vp feed\", \".vp bp\", \".vp giveblood\" and \".vp vampsafety\"." );
        		return;
        	}

            if( e.Arguments[0].Trim() == "vampsafety" )
            {
                if( m.VampSafety )
                {
                    m.SendMessage( "Vampiric Powers Safety Lock Off." );
                    m.VampSafety = false;
                }

                else
                {
                    m.SendMessage( "Vampiric Powers Safety Lock On." );
                    m.VampSafety = true;
                }
            }

            else if( m.VampSafety )
                m.SendMessage( "Ability locked. Use \".vp vampsafety\" to disable your Vampiric Powers Safety Lock." );

            else if( e.Arguments[0].Trim() == "feed" && m.CanFeed )
            {
                if( m.Feats.GetFeatLevel( FeatList.Feeding ) > 0 && m.Warmode && m.Combatant != null && !m.Combatant.Deleted && 
                    m.InRange( m.Combatant, 1 ) && m.Combatant.Paralyzed )
                    FeedTarget.TryFeed( m, m.Combatant, true );

                else
                    m.Target = new FeedTarget();
            }

            else if( e.Arguments[0].Trim() == "bp" )
                m.SendMessage( "Bloodpool: " + m.BPs.ToString() + "/" + m.MaxBPs.ToString() + "." );

            else if( e.Arguments[0].Trim() == "heal" && m.CanVampHeal && m.Alive && !m.Paralyzed )
            {
                if( m.Feats.GetFeatLevel( FeatList.Protean ) < 2 )
                    m.SendMessage( "You need the second level of Protean to use this ability." );

                else
                {
                    if( m.Hits >= m.HitsMax && m.Stam >= m.StamMax )
                    {
                        m.SendMessage( "Using your regenerative powers would not result in anything at the moment." );
                        return;
                    }

                    m.BPs--;
                    m.Hits += 20;
                    m.Stam += 20;
                    m.Emote( "*{0} wounds start mending on their own*", m.GetPossessivePronoun() );
                    m.NextVampHealAllowed = DateTime.Now + TimeSpan.FromSeconds( 2 );
                }
            }

            else if( e.Arguments[0].Trim() == "claws" )
                FeatInfo.Protean.HandleClaws( m );

            else if( e.Arguments[0].Trim() == "autoheal" )
            {
                if( m.Feats.GetFeatLevel( FeatList.Protean ) < 2 )
                    m.SendMessage( "You need the second level of Protean to use this ability." );

                else if( m.AutoVampHeal )
                {
                    m.SendMessage( "Auto-Healing Off." );
                    m.AutoVampHeal = false;
                }

                else
                {
                    m.SendMessage( "Auto-Healing On." );
                    m.AutoVampHeal = true;
                }
            }

            else if( e.Arguments[0].Trim() == "celerity" )
                FeatInfo.Celerity.HandleCelerity( m );

            else if( e.Arguments[0].Trim() == "terror" )
                FeatInfo.Terror.HandleTerror( m );

            else if( e.Arguments[0].Trim() == "awe" )
                FeatInfo.Awe.HandleAwe( m );

            else if( e.Arguments[0].Trim() == "shapeshift" )
            {
                if( e.Length < 2 || e.Arguments[1].Trim().Length < 1 )
                    m.SendMessage( "Shapeshifting requires an additional argument for the desired type of creature (human, wolf, cat, rat, bat)." );

                else if( m.Feats.GetFeatLevel( FeatList.Shapeshift ) < 1 )
                    m.SendMessage( "You need the first level of Shapeshift to use this ability." );

                else
                    FeatInfo.Shapeshift.HandleShapeshift( m, e.Arguments[1].Trim() );
            }

            else if( e.Arguments[0].Trim() == "vampsight" )
            {
                if( m.Feats.GetFeatLevel( FeatList.Protean ) < 1 )
                    m.SendMessage( "You need the first level of Protean to use this ability." );

                else if( m.VampSight )
                {
                    m.SendMessage( "Vampire Sight Off." );
                    m.VampSight = false;
                }

                else
                {
                    m.SendMessage( "Vampire Sight On." );
                    m.VampSight = true;
                }
            }

            else if( e.Arguments[0].Trim() == "giveblood" && m.BPs > 0 )
                m.Target = new GhoulTarget();
        }
        
        private class GhoulTarget : Target
		{
			public GhoulTarget() : base( 1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is PlayerMobile) )
					return;
				
				PlayerMobile ghoul = targeted as PlayerMobile;
				PlayerMobile vamp = from as PlayerMobile;
				
				if( !ghoul.Alive || ghoul.Paralyzed || ghoul.IsVampire )
					return;
				
				if( !vamp.Alive || vamp.Paralyzed || vamp.BPs < 1 )
					return;
				
				ghoul.CloseGump( typeof(Gumps.AllowGhoulingGump) );
				ghoul.SendGump( new Gumps.AllowGhoulingGump(vamp, ghoul) );
			}
        }
        
        private class FeedTarget : Target
		{
			public FeedTarget() : base( 1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) )
					return;
				
				if( !((PlayerMobile)from).CanFeed )
					return;
				
				if ( targeted is Corpse )
				{
					Corpse corpse = targeted as Corpse;
					
					if( !corpse.Channeled && corpse.Owner != null && (corpse.Owner.BodyValue == 400 || corpse.Owner.BodyValue == 401 || corpse.Owner.BodyValue == 402) )
					{
						from.Emote( "*sinks {0} fangs into the fallen body's neck and drains its blood*", ((PlayerMobile)from).GetPossessivePronoun() );
						from.PlaySound( 49 );
						((PlayerMobile)from).BPs += 2;
						corpse.Hue = 0;
						((PlayerMobile)from).NextFeedingAllowed = DateTime.Now + TimeSpan.FromSeconds( 5 );
						Server.Spells.SpellHelper.Turn( from, corpse );
						from.Animate( 32, 5, 1, true, false, 0 );
						corpse.Channeled = true;
						Effects.SendLocationParticles( corpse, 0x377A, 244, 25, 31, 0, 9550, 0 );

                        foreach(DiseaseTimer timer in HealthAttachment.GetHA(corpse.Owner).CurrentDiseases)
                        {
                            if(!HealthAttachment.GetHA(from).HasDisease(timer.Disease))
                            {
                                DiseaseTimer newDis = new DiseaseTimer(from, timer.Disease);
                                HealthAttachment.GetHA(from).CurrentDiseases.Add(newDis);
                                newDis.Start();
                            }
                        }
					}
				}
				
				else if( targeted is Mobile )
					TryFeed( from, (Mobile)targeted, false );
			}
			
			public static void TryFeed( Mobile from, Mobile targeted, bool forced )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted.BodyValue == 400 || targeted.BodyValue == 401) || !from.InRange(targeted, 1) )
					return;
				
				PlayerMobile m = from as PlayerMobile;
				
				if( !m.CanFeed )
					return;
				
				if( targeted.Paralyzed && !targeted.Blessed )
				{
					from.Emote( "*feeds on {0}*", targeted.Name );
					from.PlaySound( 49 );
					targeted.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Head );
					((PlayerMobile)from).BPs += 2;
					((PlayerMobile)from).NextFeedingAllowed = DateTime.Now + TimeSpan.FromSeconds( 5 );

                    int damage = (1 - m.Feats.GetFeatLevel( FeatList.Feeding )) * 5;

                    if( damage > 0 )
                        targeted.Damage( damage, from );

                    foreach (DiseaseTimer timer in HealthAttachment.GetHA(targeted).CurrentDiseases)
                    {
                        if (!HealthAttachment.GetHA(m).HasDisease(timer.Disease))
                        {
                            DiseaseTimer newDis = new DiseaseTimer(m, timer.Disease);
                            HealthAttachment.GetHA(m).CurrentDiseases.Add(newDis);
                            newDis.Start();
                        }
                    }
				}
				
				else if( targeted is PlayerMobile && !((PlayerMobile)targeted).IsVampire && !forced )
				{
					targeted.CloseGump( typeof(Gumps.AllowFeedingGump) );
					targeted.SendGump( new Gumps.AllowFeedingGump(from, targeted) );
				}
			}
		}
	}
}
