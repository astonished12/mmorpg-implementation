using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.Handlers.World;
using Servers.Services.WorldServices;

namespace Servers.Modules
{
    class WorldModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<WorldService>().AsImplementedInterfaces();
            builder.RegisterType<ClientEnterWorld>().AsImplementedInterfaces();
        }
    }
}
