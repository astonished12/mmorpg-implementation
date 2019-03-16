using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.Config;
using Servers.Handlers.World;
using Servers.Models;
using Servers.PubSubModels;
using Servers.Services.WorldServices;
using Servers.Support;
using ServiceStack.Redis;

namespace Servers.Modules
{
    class WorldModule : Module
    {
        protected override void Load(ContainerBuilder builder)  
        {
            base.Load(builder);
            builder.RegisterType<ServerType>().AsImplementedInterfaces();

            builder.RegisterType<ClientCodeRemove>().AsImplementedInterfaces();

            builder.RegisterType<WorldService>().AsImplementedInterfaces();
            builder.RegisterType<ClientEnterWorld>().AsImplementedInterfaces();
            builder.RegisterType<ClientRequestRegion>().AsImplementedInterfaces();
            builder.RegisterType<ClientOperationsRegion>().AsImplementedInterfaces();

            builder.RegisterType<World>().AsSelf()
                                         .AsImplementedInterfaces()
                                         .SingleInstance();


            builder.Register<IRedisClientsManager>(c =>
                new BasicRedisClientManager("localhost:6379"));

            builder.RegisterType<WorldRedisPubSub>().As<IRedisPubSubServer>();

        }
    }
}
