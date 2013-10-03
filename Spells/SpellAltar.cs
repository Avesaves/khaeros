﻿using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Khaeros.Scripts.Khaeros.Spells
{
    public class SpellAltar : Item
    {


	public override bool HandlesOnSpeech{ get{ return true; } }


        Dictionary<Type, List<SpellScrollCost>> spellCosts;

        void AddSpellCosts()
        {
            List<SpellScrollCost> shapeShiftCosts = new List<SpellScrollCost>();
            shapeShiftCosts.Add(CreateSpellCost(typeof(GlowingEmerald), 1));
            shapeShiftCosts.Add(CreateSpellCost(typeof(GlowingRuby), 1));
            shapeShiftCosts.Add(CreateSpellCost(typeof(GlowingSapphire), 1));

            AddToSpellCosts(typeof (ShapeshiftScroll), shapeShiftCosts);
        }

        void BuySpells(string speech, Container backpack, Mobile buyer)
        {
            if (speech.Contains("xitiiva"))
            {
                BuySpell<ShapeshiftScroll>(backpack, buyer);
                buyer.Emote("*glows*");
                this.PublicOverheadMessage( Network.MessageType.Regular, 0, false, "*glows softly...*" );
            }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
			if (PlayerIsInRange(e))
			{
			Mobile buyer = e.Mobile;	
                e.Mobile.SendMessage("Your words seem to echo back at you....");
			    string speech = e.Speech;
			    e.Mobile.SendMessage(e.Speech);
			    
			    Container backpack = buyer.Backpack;

                Console.WriteLine(e.Speech);

                BuySpells(speech, backpack, buyer);
			}
        }

        void BuySpell<T>(Container backpack, Mobile buyer) where T : Item
        {
            if (PlayerHasEnoughGemsToPurchase(typeof(T), backpack))
            {
                ConsumeGemsByPurchasing(typeof(T), backpack);
                T scroll = Activator.CreateInstance<T>();
                buyer.AddToBackpack(scroll);
            }
        }

        void ConsumeGemsByPurchasing(Type spell, Container backpack)
        {
            List<SpellScrollCost> costs = spellCosts[spell];

            foreach (SpellScrollCost cost in costs)
            {
                backpack.ConsumeTotal(cost.Gem, cost.Amount);
            }
        }

        bool PlayerHasEnoughGemsToPurchase(Type spell, Container backpack)
        {
        if (spell == null || backpack == null) 
        	return false;
        
            if (!spellCosts.ContainsKey(spell))
                return false;

            List<SpellScrollCost> costs = spellCosts[spell];

            foreach (SpellScrollCost cost in costs)
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
            return !e.Handled && e.Mobile.InRange(this.Location, 5);
        }

	void InitializeCosts()
	{
	 	spellCosts = new Dictionary<Type, List<SpellScrollCost>>();
            	AddSpellCosts();
	}

        [Constructable]
		public SpellAltar() : this( 1 )
		{
		}

		[Constructable]
		public SpellAltar( int amount ) : base( 0x3650 )
		{
			Amount = amount;
			InitializeCosts();
		}

        public SpellAltar(Serial serial)
            : base(serial)
		{
			InitializeCosts();
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

