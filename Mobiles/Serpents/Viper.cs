using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a viper corpse" )]
	public class Viper : Serpent
	{
		[Constructable]
		public Viper() : base()
		{
            NewBreed = "Viper";
            Hue = 1894;
		}

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Viper());
        }

        public override void incSerpentPoison()
        {
            int poisonBonus = Utility.Random(3);

            switch (poisonBonus)
            {
                case 0:
                    {
                        this.poisonHealthDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more deadly!");
                        break;
                    }
                case 1:
                    {
                        this.poisonStrDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more enfeebling!");
                        break;
                    }
                case 2:
                    {
                        this.poisonDexDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more paralyzing!");
                        break;
                    }
            }
        }

        public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison
        {
            get
            {
                return new KeyValuePair<PoisonEffectEnum, int>[] 
                    { 
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, this.poisonHealthDamage),
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StrengthDecrease, this.poisonStrDamage),
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, this.poisonDexDamage),
                    };
            }
        }

        public override int PoisonDuration { get { return this.poisonDuration; } }
        public override int PoisonActingSpeed { get { return this.poisonSpeed; } }

		public Viper(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
