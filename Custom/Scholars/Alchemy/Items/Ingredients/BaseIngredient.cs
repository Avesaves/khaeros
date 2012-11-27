using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;

namespace Server.Items
{
	public abstract class BaseIngredient : Item, IAlchemyIngredient
	{
		private static int[] ClilocEntries = { 1060658, 1060659, 1060660, 1060661, 1060662, 1060663 };
		public virtual KeyValuePair<CustomEffect, int>[] Effects { get { return null; } }
		public virtual int SkillRequired { get { return 0; } }

		KeyValuePair<CustomEffect, int>[] IAlchemyIngredient.Effects { get { return Effects; } }

		public override double DefaultWeight{ get { return 0.1; } }

		public BaseIngredient( int itemID ) : this( itemID, 1 )
		{
		}

		public BaseIngredient( int itemID, int amount ) : base( itemID )
		{
			Stackable = true;
			Amount = amount;
		}

		public BaseIngredient( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			// make sure the effects and side effects are separated in the list
			Dictionary<string, int> effects = new Dictionary<string, int>();
			Dictionary<string, int> sideEffects = new Dictionary<string, int>();

			foreach ( KeyValuePair<CustomEffect, int> kvp in this.Effects )
			{
				CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
				if ( effect != null )
				{
					if ( kvp.Value > 0 )
						effects[effect.Name] = kvp.Value;
					else
						sideEffects[effect.Name] = kvp.Value;
				}
			}

			if ( (effects.Count + sideEffects.Count) <= ClilocEntries.Length ) // make sure we have enough entries
			{
				int i = 0;
				bool colorStart = false;

				foreach ( KeyValuePair<string, int> kvp in effects )
				{
					if ( !colorStart ) // saves up some unnecessary bandwidth costs
					{
						list.Add( ClilocEntries[i++], "{0}\t{1}",  "<BASEFONT COLOR=#5BAE0A>" + kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
						colorStart = true;
					}
					else
						list.Add( ClilocEntries[i++], "{0}\t{1}", kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
				}

				colorStart = false;

				foreach ( KeyValuePair<string, int> kvp in sideEffects )
				{
					if ( !colorStart )
					{
						list.Add( ClilocEntries[i++], "{0}\t{1}",  "<BASEFONT COLOR=#BF2121>" + kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
						colorStart = true;
					}
					else
						list.Add( ClilocEntries[i++], "{0}\t{1}", kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
				}

				string usedIn = "";
				if ( this is IDrinkIngredient ) // can be all three
					usedIn += "Drinks";
				if ( this is IBombIngredient )
					usedIn += (usedIn == "" ? "" : ", ") + "Bombs";
				if ( this is IOilIngredient )
					usedIn += (usedIn == "" ? "" : ", ") + "Oils";

				if ( usedIn == "" )
					usedIn = "Nothing";

				list.Add( 1060847, "{0}\t{1}", "<BASEFONT COLOR=#AAAAAA>Used in:", usedIn ); // ~1_val~ ~2_val~
			}
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
