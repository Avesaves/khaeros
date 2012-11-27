using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Poisoning;

namespace Server.Items
{
	public abstract class BaseToxinIngredient : Item, IToxinIngredient
	{
		private static int[] ClilocEntries = { 1060658, 1060659, 1060660, 1060661, 1060662, 1060663 };
		public virtual KeyValuePair<PoisonEffectEnum, int>[] Effects { get { return null; } }
		public virtual int Corrosivity { get { return 0; } }
		public virtual int ToxinActingSpeed { get { return 0; } }
		public virtual int ToxinDuration { get { return 0; } }
		public virtual int SkillRequired { get { return 0; } }

		KeyValuePair<PoisonEffectEnum, int>[] IToxinIngredient.Effects { get { return Effects; } }
		int IToxinIngredient.Corrosivity { get { return Corrosivity; } }
		int IToxinIngredient.ToxinActingSpeed { get { return ToxinActingSpeed; } }
		int IToxinIngredient.ToxinDuration { get { return ToxinDuration; } }

		public override double DefaultWeight{ get { return 0.1; } }

		public BaseToxinIngredient( int itemID ) : this( itemID, 1 )
		{
		}

		public BaseToxinIngredient( int itemID, int amount ) : base( itemID )
		{
			Stackable = true;
			Amount = amount;
		}

		public BaseToxinIngredient( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			Dictionary<string, int> effects = new Dictionary<string, int>();

			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in this.Effects )
			{
				PoisonEffect effect = PoisonEffect.GetEffect( kvp.Key );
				if ( effect != null )
					effects[effect.Name] = kvp.Value;
			}

			if ( effects.Count <= ClilocEntries.Length ) // make sure we have enough entries
			{
				int i = 0;
				bool colorStart = false;

				foreach ( KeyValuePair<string, int> kvp in effects )
				{
					if ( !colorStart )
					{
						list.Add( ClilocEntries[i++], "{0}\t{1}",  "<BASEFONT COLOR=#BF2121>" + kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
						colorStart = true;
					}
					else
						list.Add( ClilocEntries[i++], "{0}\t{1}", kvp.Key, kvp.Value ); // ~1_val~: ~2_val~
				}

				list.Add( 1060847, "{0}\t{1}", "<BASEFONT COLOR=#AAAAAA>Used in:", "Toxins" ); // ~1_val~ ~2_val~
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
