using Akka.Actor;
using System;

namespace World.Settlers
{
    public abstract class SoulActor<TBody> : ReceiveActor
        where TBody:class, ISettler
    {
        protected const string INTERNAL_LIFE_TICK = "INTERNAL_LIFE_TICK";
        private readonly int LifeTickInterval = 100;

        protected readonly TBody Body;
        private ICancelable LifeTimerCancel;

        public SoulActor(TBody settler)
        {
            Body = settler;
            Receive<string>(s => LifeTick(s), s => INTERNAL_LIFE_TICK.Equals(s));
            Receive<Freeze>(m => Freeze(m));

            LifeTimerCancel = StartTheLife();
        }

        private void Freeze(Freeze m)
        {
           // LifeTimerCancel.Cancel();
        }

        private ICancelable StartTheLife()
        {
           return Context.System.Scheduler
                .ScheduleTellOnceCancelable(
                100, Self, INTERNAL_LIFE_TICK, Self);
        }

        protected abstract void LifeTick(object s);
    }
}