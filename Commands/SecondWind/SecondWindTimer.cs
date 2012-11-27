using Server;
using System;
using Server.Mobiles;
namespace Khaeros.Scripts.Commands.SecondWind
{
    public class SecondWindTimer : Timer
    {
        private PlayerMobile target;
        private int damagePerTick;

        public SecondWindTimer(PlayerMobile target, int damage, int duration) : base(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2), duration)
        {
            this.damagePerTick = (damage / duration);
            this.target = target;
        }

        protected override void OnTick()
        {
            if (this.target == null)
                return;

            if (this.target.m_InvulTimer != null)
                this.damagePerTick = 0;

            this.target.Damage(this.damagePerTick);
        }
    }
}
