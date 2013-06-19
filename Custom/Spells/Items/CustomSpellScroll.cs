using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.Items
{
    public class CustomSpellScroll : Item, IEasyCraft
	{
		private CustomMageSpell m_Spell = new CustomMageSpell( null, 1 );

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual CustomMageSpell Spell
		{
			get
			{
				if( m_Spell == null )
					return new CustomMageSpell( null, 1 );
				
				return m_Spell;
			}
			
			set{ m_Spell = value; }
		}
		
		[Constructable]
		public CustomSpellScroll() : base( 8828 ) 
		{
        	Hue = 1899; 
        	Name = "A blank spell scroll";
		}
		
		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if( from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Magery) > 0 )
				list.Add( new MenuEntry( this, (PlayerMobile)from ) );
		}
		
		private class MenuEntry : ContextMenuEntry
		{
			private CustomSpellScroll m_Scroll;
			private PlayerMobile m_From;
			
			public MenuEntry( CustomSpellScroll Scroll, PlayerMobile from ) : base( 5101 ) // 3006132  2132
			{
				m_Scroll = Scroll;
				m_From = from;
			}

			public override void OnClick()
			{
				if( m_From == null || m_Scroll == null || m_From.Deleted || m_Scroll.Deleted || !m_From.Alive || m_From.Paralyzed )
					return;
				
				if( !m_Scroll.IsChildOf( m_From.Backpack ) )
				{
					m_From.SendMessage( "That needs to be in your backpack for you to edit it." );
					return;
				}
				
				if( m_From.IsApprentice )
				{
					m_From.SendMessage( "Despite your magical training, you can't seem to figure out how to use this correctly." );
					return;
				}
				
				if( m_From.Feats.GetFeatLevel(FeatList.Magery) > 0 )
					m_From.SendGump( new CustomSpellScrollGump( m_From, m_Scroll ) );
			}
		}
		
		public static bool TryToEditSpell( PlayerMobile m, CustomMageSpell spell )
		{
			if( m.Feats.GetFeatLevel(FeatList.DamagingEffect) < 3 && spell.Damage > (m.Feats.GetFeatLevel(FeatList.DamagingEffect) * 25) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell that deals that much damage." );
			
			else if( (spell.ExplosionArea > (m.Feats.GetFeatLevel(FeatList.ExplosiveEffect) * 2)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with an explosion area of that magnitude." );
				
			else if( m.Feats.GetFeatLevel(FeatList.ExplosiveEffect) < 3 && spell.ExplosionDamage > (m.Feats.GetFeatLevel(FeatList.ExplosiveEffect) * 25) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell that deals that much damage from explosion." );
			
			else if( (spell.Range > (m.Feats.GetFeatLevel(FeatList.RangedEffect) * 5)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with that much range." );
			
			else if( (spell.ChainedRange > (m.Feats.GetFeatLevel(FeatList.ChainEffect) * 5)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with that much chained range." );
			
			else if( (spell.ChainedTargets > (m.Feats.GetFeatLevel(FeatList.ChainEffect) * 2)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with that many chained targets." );
			
			else if( (m.Feats.GetFeatLevel(FeatList.ChainEffect) < 3 && spell.ChainedDamage > (m.Feats.GetFeatLevel(FeatList.ChainEffect) * 25)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell that deals that much chained damage." );
			
			else if( spell.StatusType > m.Feats.GetFeatLevel(FeatList.StatusEffect) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with that status type." );
			
			else if( spell.StatusDuration > (m.Feats.GetFeatLevel(FeatList.StatusEffect) * 2) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell whose status lasts for that long." );
			
			else if ( (m.Feats.GetFeatLevel(FeatList.RecurrentEffect) < 3 && spell.RepDamage > (m.Feats.GetFeatLevel(FeatList.RecurrentEffect) * 25)) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell that deals that much recurrent damage." );
			
			else if( spell.Reps > (m.Feats.GetFeatLevel(FeatList.RecurrentEffect) * 2) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell with that many repetitions." );
			
			else if( (spell.RepDelay < 11 && m.Feats.GetFeatLevel(FeatList.RecurrentEffect) < 2) || (spell.RepDelay < 6 && m.Feats.GetFeatLevel(FeatList.RecurrentEffect) < 3) )
				m.SendMessage( "You lack the appropriate knowledge to create a spell whose recurrent effect happens so fast." );
            
            else if(!m.Feats.FeatDictionary.ContainsKey(spell.RequiredFeat))
                m.SendMessage(String.Format("You lack the {0} feat to create this spell.", spell.RequiredFeat));
			
			else
				return true;
			
			return false;
		}
		
		public override bool OnDroppedOnto( Mobile from, Item target )
		{
			if( target != null && !target.Deleted && target is CustomSpellBook && target.IsChildOf(from) )
			{
				CustomSpellBook book = target as CustomSpellBook;
				
				if( Spell.CustomName == null || Spell.CustomName.Length < 1 )
					from.SendMessage( "A spell scroll can only be copied onto a spell book after the spell has been properly named." );
				
				else if( book.ContainsSpell(Spell.CustomName) )
					from.SendMessage( "That spell book already contains a spell with the same name as the one on the scroll." );
				
				else
				{
					book.Spells.Add( DupeCustomMageSpell(Spell) );
					from.SendMessage( "You successfully add the spell into the book." );
					CustomSpellScroll.delete;
				}
			}
				
			return base.OnDroppedOnto( from, target );
		}
		
		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Spell.CustomName != null )
				list.Add( Spell.CustomName );

			else
				list.Add( this.Name );
		}
		
		public static void GetCustomMageSpellStats( Mobile m, CustomMageSpell spell )
		{
			m.SendMessage( "Name: " + spell.CustomName );
			m.SendMessage( "Mana Cost: " + spell.ManaCost.ToString() );
			m.SendMessage( "Damage: " + spell.Damage.ToString() );
			m.SendMessage( "Range: " + spell.Range.ToString() );
			m.SendMessage( "Chained Targets: " + spell.ChainedTargets.ToString() );
			m.SendMessage( "Chained Damage: " + spell.ChainedDamage.ToString() );
			m.SendMessage( "Chained Range: " + spell.ChainedRange.ToString() );
			m.SendMessage( "Explosion Damage: " + spell.ExplosionDamage.ToString() );
			m.SendMessage( "Explosion Area: " + spell.ExplosionArea.ToString() );
			m.SendMessage( "Repetitions: " + spell.Reps.ToString() );
			m.SendMessage( "Repetition Delay: " + spell.RepDelay.ToString() );
			m.SendMessage( "Repetition Damage: " + spell.RepDamage.ToString() );
			m.SendMessage( "Status Type: " + spell.StatusType.ToString() );
			m.SendMessage( "Status Duration: " + spell.StatusDuration.ToString() );
			m.SendMessage( "Effect ID: " + spell.EffectID.ToString() );
			m.SendMessage( "Effect Hue: " + spell.EffectHue.ToString() );
			m.SendMessage( "Effect Sound: " + spell.EffectSound.ToString() );
			m.SendMessage( "Explosion ID: " + spell.ExplosionID.ToString() );
			m.SendMessage( "Explosion Hue: " + spell.ExplosionHue.ToString() );
			m.SendMessage( "Explosion Sound: " + spell.ExplosionSound.ToString() );
			m.SendMessage( "Icon ID: " + spell.IconID.ToString() );
            m.SendMessage("Required Feat: "+spell.RequiredFeat.ToString());
		}
		
		public static CustomMageSpell DupeCustomMageSpell( CustomMageSpell Spell )
		{
            CustomMageSpell spell = (CustomMageSpell)Spell.GetNewInstance();

			spell.CustomName = Spell.CustomName;
			spell.Damage = Spell.Damage;
			spell.Range = Spell.Range;
			spell.ChainedTargets = Spell.ChainedTargets;
			spell.ChainedDamage = Spell.ChainedDamage;
			spell.ChainedRange = Spell.ChainedRange;
			spell.ExplosionDamage = Spell.ExplosionDamage;
			spell.ExplosionArea = Spell.ExplosionArea;
			spell.Reps = Spell.Reps;
			spell.RepDelay = Spell.RepDelay;
			spell.RepDamage = Spell.RepDamage;
			spell.StatusType = Spell.StatusType;
			spell.StatusDuration = Spell.StatusDuration;
			spell.EffectID = Spell.EffectID;
			spell.EffectHue = Spell.EffectHue;
			spell.EffectSound = Spell.EffectSound;
			spell.ExplosionID = Spell.ExplosionID;
			spell.ExplosionHue = Spell.ExplosionHue;
			spell.ExplosionSound = Spell.ExplosionSound;
			spell.IconID = Spell.IconID;
		    spell.RequiredFeat = Spell.RequiredFeat;
			
			return spell;
		}

        public static bool IsMageCheck( Mobile m, bool message )
        {
            if( m is PlayerMobile && ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.Magery ) < 1 )
            {
                if( message )
                    m.SendMessage( "You cannot decipher anything from this parchment, which is covered in peculiar runes and odd drawings." );

                return false;
            }

            return true;
        }
		
		public static void CastCustomMageSpell( Mobile m, CustomMageSpell Spell )
		{
			if( !IsMageCheck( m, true ) )
                return;
			
			CustomMageSpell spell = DupeCustomMageSpell( Spell );
			spell.Caster = m;
			BaseCustomSpell.SpellInitiator( spell );
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			if( !this.IsChildOf( m.Backpack ) )
			   return;
			   
			if( Spell.ManaCost > 0 )
				CastCustomMageSpell( m, Spell );
			
			else
				m.SendMessage( "This scroll appears blank." );
        }
		
  		public CustomSpellScroll( Serial serial ) : base( serial ) 
  		{ 
 		}
  		
  		public static void SerializeSpell( GenericWriter writer, CustomMageSpell Spell )
  		{
  			writer.Write( (int) 2 ); // version
            writer.Write( (string) Spell.GetType().Name );
            //writer.Write((string)Spell.RequiredFeat.ToString());
  			writer.Write( (string) Spell.CustomName );
     		writer.Write( (int) Spell.RepDamage );
			writer.Write( (int) Spell.Damage );
			writer.Write( (int) Spell.Range );
			writer.Write( (int) Spell.ChainedTargets );
			writer.Write( (int) Spell.ChainedDamage );
			writer.Write( (int) Spell.ChainedRange );
			writer.Write( (int) Spell.ExplosionDamage );
			writer.Write( (int) Spell.ExplosionArea );
			writer.Write( (int) Spell.Reps );
			writer.Write( (int) Spell.RepDelay );
			writer.Write( (int) Spell.StatusType );
			writer.Write( (int) Spell.StatusDuration );
			writer.Write( (int) Spell.EffectID );
			writer.Write( (int) Spell.EffectHue );
			writer.Write( (int) Spell.EffectSound );
			writer.Write( (int) Spell.ExplosionID );
			writer.Write( (int) Spell.ExplosionHue );
			writer.Write( (int) Spell.ExplosionSound );
			writer.Write( (int) Spell.IconID );
			writer.Write( (string) Spell.RequiredFeat.ToString());
  		}

 		public override void Serialize( GenericWriter writer ) 
  		{
 			base.Serialize( writer );
     		writer.Write( (int) 2 ); // version
     		SerializeSpell( writer, Spell );
  		}
 		
		static void PrintReadValues(GenericReader reader)
		{
			Console.WriteLine("Version: " +reader.ReadInt());
			//Console.WriteLine("Feat: " + reader.ReadString());
			Console.WriteLine("Type: " + reader.ReadString());
			Console.WriteLine("CustomName: " + reader.ReadString());
			Console.WriteLine("Rep Damage: " + reader.ReadInt());
			Console.WriteLine("SpellDamage: "+ reader.ReadInt());
			Console.WriteLine("Range: "+ reader.ReadInt());
			Console.WriteLine("Chained Targets" + reader.ReadInt());
			Console.WriteLine("Chained DMG " + reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine(reader.ReadInt());
			Console.WriteLine("Effect Id" + reader.ReadInt());
			Console.WriteLine("Effect Hue" + reader.ReadInt());
			Console.WriteLine("Effect Sound " + reader.ReadInt());
			Console.WriteLine("Explosion ID " + reader.ReadInt());
			Console.WriteLine("Explosion Hue " +reader.ReadInt());
			Console.WriteLine("Explosion Sound " + reader.ReadInt());
			Console.WriteLine("Icon Id " + reader.ReadInt());
			Console.WriteLine("Extra " + reader.ReadString());
		}
		
 		public static CustomMageSpell DeserializeSpell( GenericReader reader )
 		{			
 			int version = reader.ReadInt();
            CustomMageSpell newSpell = null;		
			
			if (version > 0)
				newSpell = (CustomMageSpell)Activator.CreateInstance(ScriptCompiler.FindTypeByName(reader.ReadString(), true));
			else return new CustomMageSpell(null, 1);
			
			newSpell.CustomName = reader.ReadString();
			newSpell.RepDamage = reader.ReadInt();
			newSpell.Damage = reader.ReadInt();
			newSpell.Range = reader.ReadInt();
			newSpell.ChainedTargets = reader.ReadInt();
			newSpell.ChainedDamage = reader.ReadInt();
			newSpell.ChainedRange = reader.ReadInt();
			newSpell.ExplosionDamage = reader.ReadInt();
			newSpell.ExplosionArea = reader.ReadInt();
			newSpell.Reps = reader.ReadInt();
			newSpell.RepDelay = reader.ReadInt();
			newSpell.StatusType = reader.ReadInt();
			newSpell.StatusDuration = reader.ReadInt();
			newSpell.EffectID = reader.ReadInt();
			newSpell.EffectHue = reader.ReadInt();
			newSpell.EffectSound = reader.ReadInt();
			newSpell.ExplosionID = reader.ReadInt();
			newSpell.ExplosionHue = reader.ReadInt();
			newSpell.ExplosionSound = reader.ReadInt();
			newSpell.IconID = reader.ReadInt();
			newSpell.RequiredFeat = DeserializeRequiredFeat(reader.ReadString());
			
			return newSpell;
			
			/*switch (version) {
				case 2:
				{		
					newSpell.RequiredFeat = DeserializeRequiredFeat(feat);
					goto case 1;
				}
				case 1:
				{
				newSpell.CustomName = reader.ReadString();
				newSpell.RepDamage = reader.ReadInt();
				newSpell.Damage = reader.ReadInt();
				newSpell.Range = reader.ReadInt();
				newSpell.ChainedTargets = reader.ReadInt();
				newSpell.ChainedDamage = reader.ReadInt();
				newSpell.ChainedRange = reader.ReadInt();
				newSpell.ExplosionDamage = reader.ReadInt();
				newSpell.ExplosionArea = reader.ReadInt();
				newSpell.Reps = reader.ReadInt();
				newSpell.RepDelay = reader.ReadInt();
				newSpell.StatusType = reader.ReadInt();
				newSpell.StatusDuration = reader.ReadInt();
				newSpell.EffectID = reader.ReadInt();
				newSpell.EffectHue = reader.ReadInt();
				newSpell.EffectSound = reader.ReadInt();
				newSpell.ExplosionID = reader.ReadInt();
				newSpell.ExplosionHue = reader.ReadInt();
				newSpell.ExplosionSound = reader.ReadInt();
				newSpell.IconID = reader.ReadInt();   
				break;
				}		
				case 0:
				{
					newSpell = new CustomMageSpell(null, 1);
					break;
				}
			}*/
            //return newSpell;	
			//PrintReadValues(reader);
			//return new CustomMageSpell(null, 1);
 		}

        static FeatList DeserializeRequiredFeat(string featAsString)
        {
            try
            {
				if (featAsString == null && featAsString == String.Empty)
					return FeatList.Invocation;
                FeatList feat = (FeatList) (Enum.Parse(typeof (FeatList), featAsString));
                if (feat == null)
                    return FeatList.Invocation;
                return feat;
            }
            catch (Exception)
            {
                return FeatList.Invocation;               
            }
        }

        public override void Deserialize( GenericReader reader ) 
  		{ 
  			base.Deserialize( reader );
     		int version = reader.ReadInt();
     		Spell = DeserializeSpell( reader );
  		}
   	} 
} 
