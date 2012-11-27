using Server;
using System;
using Server.Items;
namespace Khaeros.Scripts.Timers
{
    public class ContainerRelockTimer : Timer
    {
        private LockableContainer target;

        public ContainerRelockTimer(LockableContainer target) : base(TimeSpan.FromSeconds(60))
        {
            this.target = target;
        }

        protected override void OnTick()
        {
            this.target.Locked = true;
        }
    }
}
