using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Khaeros.Scripts.Khaeros.Spells
{
    public class SpellAltar : Item
    {
        readonly Dictionary<Type, List<SpellScrollCost>> spellCosts;

        void AddSpellCosts()
        {
            AddToSpellCosts(typeof(ShapeshiftScroll), new List<SpellScrollCost>
                {
                    CreateSpellCost(typeof(GlowingEmerald), 1),
                    CreateSpellCost(typeof(GlowingRuby), 1),
                    CreateSpellCost(typeof(GlowingSapphire), 1)
                });
        }

        void BuySpells(string speech, Container backpack, Mobile buyer)
        {
            if (speech.Contains("shapeshiftscroll"))
            {
                BuySpell<ShapeshiftScroll>(backpack, buyer);
            }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
			if (PlayerIsInRange(e))
			{
			    var speech = e.Speech;
			    var buyer = e.Mobile;
			    var backpack = buyer.Backpack;

                BuySpells(speech, backpack, buyer);
			}
        }

        void BuySpell<T>(Container backpack, Mobile buyer) where T : Item
        {
            if (PlayerHasEnoughGemsToPurchase(typeof(T), backpack))
            {
                ConsumeGemsByPurchasing(typeof(T), backpack);
                var scroll = Activator.CreateInstance<T>();
                buyer.AddToBackpack(scroll);
            }
        }

        void ConsumeGemsByPurchasing(Type spell, Container backpack)
        {
            var costs = spellCosts[spell];

            foreach (var cost in costs)
            {
                backpack.ConsumeTotal(cost.Gem, cost.Amount);
            }
        }

        bool PlayerHasEnoughGemsToPurchase(Type spell, Container backpack)
        {
            if (!spellCosts.ContainsKey(spell))
                return false;

            var costs = spellCosts[spell];

            foreach (var cost in costs)
            {
                if (backpack.GetAmount(cost.Gem) < cost.Amount)
                    return false;
            }
            return true;
        }

        void AddToSpellCosts(Type spell, List<SpellScrollCost> cost)
        {
            spellCosts.Add(spell, cost);
        }

        SpellScrollCost CreateSpellCost(Type gem, int amount)
        {
            return new SpellScrollCost(gem, amount);
        }

        bool PlayerIsInRange(SpeechEventArgs e)
        {
            return !e.Handled && e.Mobile.InRange( this.Location, 4 );
        }

        [Constructable]
		public SpellAltar() : this( 1 )
		{
		}

		[Constructable]
		public SpellAltar( int amount ) : base( 0x3650 )
		{
			Amount = amount;
            spellCosts = new Dictionary<Type, List<SpellScrollCost>>();
            AddSpellCosts();
		}

        public SpellAltar(Serial serial)
            : base(serial)
		{
		}
    }
}

