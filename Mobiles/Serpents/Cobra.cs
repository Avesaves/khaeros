using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a cobra corpse" )]
	public class Cobra : Serpent, IDesertCreature
	{
		[Constructable]
		public Cobra() : base()
		{
            NewBreed = "Cobra";
            Hue = 1806;
		}

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Cobra());
        }

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new CobraVenom() );
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
                        this.poisonStamDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more tiring!");
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
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, this.poisonStamDamage),
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, this.poisonDexDamage),
                    };
            }
        }

        public override int PoisonDuration { get { return this.poisonDuration; } }
        public override int PoisonActingSpeed { get { return this.poisonSpeed; } }

		public Cobra(Serial serial) : base(serial)
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
