using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName("a copperhead corpse")]
	public class Copperhead : Serpent, ICaveCreature
	{
		[Constructable]
		public Copperhead() : base()
		{
            NewBreed = "Copperhead";
            Hue = 2827;
		}

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new Copperhead());
        }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new CopperheadVenom() );
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
                        this.poisonManaDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more disheartening!");
                        break;
                    }
                case 2:
                    {
                        this.poisonIntDamage += this.XPScale;
                        if (this.ControlMaster != null)
                            this.ControlMaster.SendMessage(this.Name + "'s venom is more stupefying!");
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
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, this.poisonManaDamage),
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, this.poisonIntDamage)
                    };
            }
        }

        public override int PoisonDuration { get { return this.poisonDuration; } }
        public override int PoisonActingSpeed { get { return this.poisonSpeed; } }

		public Copperhead(Serial serial) : base(serial)
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
