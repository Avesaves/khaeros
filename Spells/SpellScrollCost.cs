using System;

namespace Khaeros.Scripts.Khaeros.Spells
{
    public class SpellScrollCost
    {
        Type gem;
        int amount;

        public SpellScrollCost(Type gem, int amount)
        {
            this.gem = gem;
            this.amount = amount;
        }

        public Type Gem
        {
            get { return gem; }
        }

        public int Amount
        {
            get { return amount; }
        }
    }
}