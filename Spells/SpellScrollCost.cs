using System;

namespace Khaeros.Scripts.Khaeros.Spells
{
    public class SpellScrollCost
    {
        public SpellScrollCost(Type gem, int amount)
        {
            Gem = gem;
            Amount = amount;
        }

        public Type Gem { get; private set; }
        public int Amount { get; private set; }
    }
}