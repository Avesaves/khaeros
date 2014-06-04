using Server.Mobiles;
using Khaeros.Scripts.Commands.SecondWind;
using System;

namespace Server.Commands
{
    public class SecondWind
    {
        private PlayerMobile target;
        private SecondWindTimer timer;
        private int amountToHeal, amountToDamage;
        private int duration;
        private int cooldown;

        public void ExecuteOn(PlayerMobile target)
        {
            if (target != null)
            {
                this.cooldown = 15; // minutes
                this.duration = 15; // seconds
                this.target = target;
                this.CalculateAmountToUse();

                if (CooldownIsOver())
                {
                    this.target.LastSecondWind = DateTime.Now;
                    this.HealTarget();
                    this.StartDegeneration();
                }
                else
                    this.target.SendMessage(60, "You cannot use Second Wind yet.");
            }
        }

        private bool CooldownIsOver()
        {
            return (this.target.LastSecondWind.AddMinutes(this.cooldown) < DateTime.Now);
        }

        private void StartDegeneration()
        {
           this.timer = new SecondWindTimer(this.target, this.amountToDamage, this.duration);
           this.timer.Start();
        }

        private void CalculateAmountToUse()
        {
            hts = this.target.RawHits;
            stm = this.target.RawStam;

            this.amountToHeal = hts/4;
            this.amountToDamage = 40;
        }

        private void HealTarget()
        {
            this.target.Emote("*Gets "+this.target.GetPossessivePronoun() +" second wind!*");
            this.target.Heal(this.amountToHeal);
            this.target.Stam += stm;
        }
    }
}
