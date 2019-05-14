using Autofac;
using Servers.BackgroundThreads.Region;
using Servers.Handlers.Regions;
using Servers.Models;
using Servers.PubSubModels;
using Servers.Services;
using ServiceStack.Redis;

namespace Servers.Modules
{
    public class RegionModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Region>().AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RegionService>().AsImplementedInterfaces().SingleInstance();
            ;
            builder.RegisterType<InterestManagementService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<AssignAreaMapRegionHandler>().AsImplementedInterfaces();
            builder.RegisterType<ClientEnterRegion>().AsImplementedInterfaces();

            builder.RegisterType<AreaOfInterestThread>().AsImplementedInterfaces();
            builder.RegisterType<MobSpawnerThread>().AsImplementedInterfaces();
            builder.RegisterType<PlayerCommunicationThread>().AsImplementedInterfaces();

            builder.Register<IRedisClientsManager>(c =>
                new BasicRedisClientManager("localhost:6379"));

            builder.RegisterType<CacheService>().AsImplementedInterfaces().SingleInstance();

        }
    }
}
