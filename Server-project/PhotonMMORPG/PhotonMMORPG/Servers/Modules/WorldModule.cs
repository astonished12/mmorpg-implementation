using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.Handlers.World;
using Servers.PubSubModels;
using Servers.Services.WorldServices;
using ServiceStack.Redis;

namespace Servers.Modules
{
    class WorldModule : Module
    {
        protected override void Load(ContainerBuilder builder)  
        {
            base.Load(builder);


            builder.RegisterType<WorldService>().AsImplementedInterfaces();
            builder.RegisterType<ClientEnterWorld>().AsImplementedInterfaces();


            builder.Register<IRedisClientsManager>(c =>
                new BasicRedisClientManager("localhost:6379"));

            builder.RegisterType<WorldRedisPubSub>().As<IRedisPubSubServer>();

        }
    }
}
