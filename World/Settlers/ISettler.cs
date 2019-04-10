using Akka.Actor;
using World.Maps;

namespace World.Settlers
{
    public interface ISettler
    {
        Cell Cell { get; set; }
        IMap Map { get; set; }
        IActorRef Soul{get;}

        int LifeCyclesCount { get; }

        float Energy { get;  }
        
        float Organics { get; }
        float Minerals { get; }

        IGenome Genome { get; set; }

        int Age { get; }

        ISettler SparkSoul(IUntypedActorContext spark);
        ISettler SparkSoul(ActorSystem spark);

    }
}